using AutoMapper;
using BakurianiBooking.Data;
using BakurianiBooking.Models.Entities;
using BakurianiBooking.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace BakurianiBooking.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly EmailService _emailService;

        public BookingsController(ApplicationDbContext context, IMapper mapper, EmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            var booking = await _context.Bookings
                .Include(x => x.Hotel)
                .Include(x => x.Room)    
                .OrderByDescending(x => x.StartDate)
                .Skip((page - 1) * pageSize )
                .Take(pageSize)
                .ToListAsync();


            var bookingTotals = booking.Select(x => new
            {
                Booking = x,
                Days = (x.EndDate - x.StartDate).Days,
                TotalAmount = (x.EndDate - x.StartDate).Days * x.Price
            }).ToList();
            var dynamicbookingTotals = bookingTotals.Cast<dynamic>().ToList();

            var userEmails = new Dictionary<string, string>();
            foreach (var book in booking)
            {
                if (!string.IsNullOrEmpty(book.IdentityUserId) && !userEmails.ContainsKey(book.IdentityUserId))
                {
                    var user = await _context.Users.FindAsync(book.IdentityUserId);
                    if (user != null)
                    {
                        userEmails[book.IdentityUserId] = user.Email;
                    }
                }
            }
            ViewBag.UserEmails = userEmails;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = await _context.Bookings.CountAsync();
            return View(dynamicbookingTotals);

        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            var bookingDto = _mapper.Map<BookingDetailsDto>(booking);
            return View(bookingDto);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int roomId, int hotelId, DateTime? startDate)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            var hotel = await _context.Hotels.FindAsync(hotelId);

            if (room == null || hotel == null)
            {
                return NotFound();
            }

            ViewBag.RoomId = roomId;
            ViewBag.HotelId = hotelId;
            ViewBag.RoomName = room.Name;
            ViewBag.HotelName = hotel.Name;
            ViewBag.RoomPrice = room.Price;
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");
            if (startDate.HasValue && startDate.Value > DateTime.Now)
            {
                ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            }
            else { 
                ViewBag.Startdate = ViewBag.Today;
            }
            var userEmail = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("emailaddress");
            ViewBag.UserEmail = userEmail;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingCreateDto bookingCreateDto, int roomid, int hotelId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError(string.Empty, "მომხმარებელი არ არის ავტორიზებული.");
                return await HandleCreateError(bookingCreateDto, roomid, hotelId);
            }

            if (ModelState.IsValid)
            {
                var isRoomBooked = await _context.Bookings
                    .AnyAsync(x => x.RoomId == roomid &&
                    x.StartDate < bookingCreateDto.EndDate &&
                    x.EndDate > bookingCreateDto.StartDate
                    );
                if (isRoomBooked)
                {
                    ModelState.AddModelError(string.Empty, "ოთახი აღნიშნულ პერიოდში დაჯავშნიალია");
                    return View(bookingCreateDto);
                }
            }

                bookingCreateDto.IdentityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!ModelState.IsValid || !await ValidateBookingDates(bookingCreateDto))
                {
                    return await HandleCreateError(bookingCreateDto, roomid, hotelId);
                }

                try
                {
                    bookingCreateDto.Status = "მომზადებული";
                    bookingCreateDto.PaymentStatus = "გადასახდელი";

                    var booking = _mapper.Map<Booking>(bookingCreateDto);
                    _context.Bookings.Add(booking);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating booking: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error...");
                    return await HandleCreateError(bookingCreateDto, roomid, hotelId);
                }

            }
        
        private async Task<bool> ValidateBookingDates(BookingCreateDto bookingCreateDto)
        {
            if (bookingCreateDto.EndDate <= bookingCreateDto.StartDate)
            {
                ModelState.AddModelError("EndDate", "დასრულების თარიღი არ უნდა იყოს საწყის თარიღზე ნაკლები ან ტოლი ");
                return false;
            }
            return true;
        }

        private async Task<IActionResult> HandleCreateError(BookingCreateDto bookingCreateDto, int roomid, int hotelId)
        {
            var room = await _context.Rooms.FindAsync(roomid);
            var hotel = await _context.Hotels.FindAsync(hotelId);
            var userEm = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("emailaddress");

            ViewBag.HotelId = hotelId;
            ViewBag.RoomId = roomid;
            ViewBag.HotelName = hotel.Name;
            ViewBag.RoomName = room.Name;
            ViewBag.RoomPrice = room.Price;
            ViewBag.UserEmail = userEm;
            ViewBag.HasError = true;

            return View(bookingCreateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            var bookingUpdateDto = _mapper.Map<BookingUpdateDto>(booking);
            return View(bookingUpdateDto);
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(x => x.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookingUpdateDto bookingUpdateDto)
        {
            if (id != bookingUpdateDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var booking = _mapper.Map<Booking>(bookingUpdateDto);
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(bookingUpdateDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bookingUpdateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet,ActionName("Invoice")]
        public async Task<IActionResult> GenerateInvoice(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings
                .Include(x => x.Hotel)
                .Include(x => x.Room)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View("Invoice", booking );
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmPayment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings
                .Include(b => b.Hotel)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            if (booking.Status == "მომზადებული")
            {
                booking.Status = "გადასახდელი";
                _context.Update(booking);
                await _context.SaveChangesAsync();
            }

            //if (booking.Hotel != null && !string.IsNullOrEmpty(booking.Hotel.OwnerEmail))
            //{
            //    await _emailService.SendEmailAsync(booking.Hotel.OwnerEmail, "ახალი ჯავშანი", "განხორციელდა ახალი ჯავშანი!");
            //}
            //else
            //{
            //    Console.WriteLine($"სასტუმროს მეილის მისამართი არ არის ხელმისაწვდომი ჯავშნისთვის ID: {booking.Id}");
            //}

            return RedirectToAction("Index", new { id = booking.Id });
        }
    }
}