using AutoMapper;
using BakurianiBooking.Data;
using BakurianiBooking.Models.DTOs;
using BakurianiBooking.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace BakurianiBooking.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoomsController(ApplicationDbContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var room = await _context.Rooms
            .Include(x => x.RoomImages)
            .Include(x => x.Hotel)
            .ToListAsync();
            return View(room);
        }
        
        [HttpGet]
        public async Task<IActionResult> HotelRooms(int id)
        {
            var room = await _context.Rooms
            .Where(a => a.HotelId == id)
            .Include(x => x.RoomImages)
            .Include(x=>x.Hotel)
            .ToListAsync();
            if (room == null)
            {
                return NotFound();
            }
            ViewBag.HotelId = id; 
            return View("Index" ,room);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        { if (id == null) 
            { 
            return NotFound();
            }
            var room = await _context.Rooms
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            var roomDto = _mapper.Map<RoomDetailsDto>(room);
            return View(roomDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            ViewBag.HotelId = id;
            ViewBag.HotelName = hotel.Name;
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(RoomCreateDto roomCreateDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var room = _mapper.Map<Room>(roomCreateDto);
                    room.RoomImages = new List<RoomImage>();

                    if (roomCreateDto.ImageFiles != null && roomCreateDto.ImageFiles.Any())
                    {
                        foreach (var imageFile in roomCreateDto.ImageFiles)
                        {
                            if (imageFile != null && imageFile.Length > 0)
                            {
                                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/rooms", fileName);

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await imageFile.CopyToAsync(fileStream);
                                }

                                room.RoomImages.Add(new RoomImage { ImageUrl = fileName });
                            }
                        }
                    }

                    _context.Add(room);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("HotelRooms", "Rooms", new { id = roomCreateDto.HotelId });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("", "An error occurred while saving the room.");
                    return View(roomCreateDto);
                }
            }

            return View(roomCreateDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) 
            { 
            return NotFound();
            }
            var room = await _context.Rooms.FirstOrDefaultAsync(_x => _x.Id == id);
            if (room == null) 
            { 
            return NotFound();
            }

            var roomUpdateDto = _mapper.Map<RoomUpdateDto>(room);
            ViewBag.HotelId = room.HotelId;
            return View(roomUpdateDto);
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(x => x.Id == id);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, RoomUpdateDto roomUpdateDto)
        {
            if (id != roomUpdateDto.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var room = _mapper.Map<Room>(roomUpdateDto);
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(roomUpdateDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("HotelRooms", new { id = roomUpdateDto.HotelId });
            }
            return View(roomUpdateDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var room = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            ViewBag.HotelId = room.HotelId;
            return View(room);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            var activeBookings = await _context.Bookings
                .Where(x => x.RoomId == id)
                .ToListAsync();

            if (activeBookings.Any())
            {
                ViewBag.ErrorMessage = "ოთახზე არის აქტიური ჯავშნები და მისი წაშლა შეუძლებელია.";
                return View("Delete" , room);
            }
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction("HotelRooms", new { id = room.HotelId });
        }

    }
}
