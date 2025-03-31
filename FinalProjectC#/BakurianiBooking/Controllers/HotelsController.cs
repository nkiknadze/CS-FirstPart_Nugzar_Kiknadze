    using BakurianiBooking.Data;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;
using System.Linq;
using BakurianiBooking.Models.DTOs;
    using BakurianiBooking.Models.Entities;
using Microsoft.AspNetCore.Authorization;

    namespace BakurianiBooking.Controllers
    {
        public class HotelsController : Controller
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public HotelsController(ApplicationDbContext context , IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            [HttpGet]
            public async Task<IActionResult> Index()
            {
                var hotel = await _context.Hotels.Include(x => x.HotelImages.Take(1)).ToListAsync();
                if (hotel == null)
                {
                    return NotFound();
                }
                return View(hotel);
            }

            [HttpGet]
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }
                var hotel = await _context.Hotels.FirstOrDefaultAsync(x => x.Id == id);
                if (hotel == null)
                {
                    return NotFound();
                }
                var hotelDto = _mapper.Map<HotelDetailsDto>(hotel);
                return View(hotelDto);
            }

            [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
            {
                return View();
            }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(HotelCreateDto hotelCreateDto, List<IFormFile> ImageFiles)
        {
            if (ModelState.IsValid)
            {
                var hotel = _mapper.Map<Hotel>(hotelCreateDto);
                hotel.HotelImages = new List<HotelImage>();

                if (ImageFiles != null && ImageFiles.Any())
                {
                    foreach (var ImageFile in ImageFiles)
                    {
                        if (ImageFile != null && ImageFile.Length > 0)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/hotels", fileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await ImageFile.CopyToAsync(fileStream);
                            }

                            hotel.HotelImages.Add(new HotelImage { ImageUrl = fileName });
                        }
                    }
        
                    hotel.ImageUrl = hotel.HotelImages.FirstOrDefault()?.ImageUrl;
                }

                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotelCreateDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }
                var hotel = await _context.Hotels.FirstOrDefaultAsync(x => x.Id == id);
                if (hotel == null)
                {
                    return NotFound();
                }
                var hotelUpdateDto = _mapper.Map<HotelUpdateDto>(hotel);
                return View(hotelUpdateDto);
            }
            private bool HotelExists(int id)
            {
                return _context.Hotels.Any(x => x.Id == id);
            }

        [Authorize(Roles = "Admin")]
            [HttpPost]
            [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int? id , HotelUpdateDto hotelUpdateDto)
            {
                if (id != hotelUpdateDto.Id)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        var hotel = _mapper.Map<Hotel>(hotelUpdateDto);
                        _context.Update(hotel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!HotelExists(hotelUpdateDto.Id))
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
                return View(hotelUpdateDto);
            }

        [Authorize(Roles = "Admin")]
            [HttpGet]
        public async Task<IActionResult> Delete(int id)
            {
                var hotel = await _context.Hotels.FirstOrDefaultAsync(x => x.Id == id);
                if (hotel == null) { return NotFound(); }
                return View(hotel);
            }
            [HttpPost, ActionName("Delete")]
            [AutoValidateAntiforgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var hotel = await _context.Hotels.FirstOrDefaultAsync(x => x.Id == id);
                        var activeBookings = await _context.Bookings
            .Include(x => x.Room)
            .Where(x => x.Room.HotelId == id)
            .ToListAsync();
            if (activeBookings.Any())
            {
                ViewBag.ErrorMessage = "სასტუმროზე არის აქტიური ჯავშნები და მისი წაშლა შეუძლებელია.";
                return View("Delete", hotel);
            }
                _context.Hotels.Remove(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> HotelRooms(int id)
        {
            var hotel = await _context.Hotels.FirstOrDefaultAsync(x => x.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            var rooms = await _context.Rooms.Where(r => r.HotelId == id).ToListAsync();
            ViewBag.HotelId = id;
            return View(rooms);
        }
    }
    }
