using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using VPT_Movie_Box.Data;
using VPT_Movie_Box.Data;
using VPT_Movie_Box.Models;
using VPT_Movie_Box.Models;

namespace VPT_Movie_Box.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var movies = _context.Movies.ToList();

            return View(movies);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Movies()
        {
            var movies = _context.Movies.ToList();
            return View(movies);
        }

        // GET: show contact form
        [HttpGet]
        public IActionResult Contact_Us()
        {
            return View(new Contact());
        }

        // POST: save contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact_Us(Contact contact)
        {
            if (contact == null)
            {
                ModelState.AddModelError("", "Dữ liệu gửi lên không hợp lệ.");
                return View(new Contact());
            }

            if (!ModelState.IsValid)
            {
                return View(contact);
            }

            contact.CreatedAt = DateTime.Now;
            _context.Contacts.Add(contact);
            _context.SaveChanges();

            TempData["ContactSuccess"] = "Cảm ơn bạn! Thông điệp đã được gửi.";
            return RedirectToAction(nameof(Contact_Us));
        }

        public IActionResult Sign_In()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            // 1. Kiểm tra dữ liệu đầu vào từ Form
            if (!ModelState.IsValid)
            {
                return View("Sign_In");
            }

            // 2. Kiểm tra Email đã tồn tại chưa
            var emailExists = _context.Users.Any(u => u.Email == model.Email);
            if (emailExists)
            {
                ViewBag.Error = "Email này đã được đăng ký. Vui lòng thử email khác.";
                return View("Sign_In");
            }

            // 3. Khởi tạo đối tượng User mới
            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                // THÊM DÒNG NÀY: Gán mặc định là User khi đăng ký từ trang công cộng
                Role = "User",
                CreatedAt = DateTime.Now
            };

            // 4. HASH MẬT KHẨU
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

            try
            {
                // 5. Lưu vào Database
                _context.Users.Add(user);
                _context.SaveChanges();

                ViewBag.Success = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";
            }
            catch (Exception) // Bỏ 'ex' nếu không dùng để tránh cảnh báo CS0168
            {
                ViewBag.Error = "Có lỗi xảy ra trong quá trình lưu dữ liệu.";
            }

            return View("Sign_In");
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View("Sign_In");

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user != null)
            {
                var passwordHasher = new PasswordHasher<User>();
                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName ?? user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                // ĐỔI DÒNG NÀY: Dùng ClaimTypes.NameIdentifier để khớp với BookingHistory
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    // Lưu Session (nếu vẫn muốn dùng song song)
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserId", user.UserId.ToString());

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Email hoặc mật khẩu không đúng!";
            return View("Sign_In");
        }
        public IActionResult ETicket(int bookingId)
        {
            var booking = _context.Bookings
                .Include(b => b.Showtime)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.Showtime)
                    .ThenInclude(s => s.Screen)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return Content($"LỖI: Không tìm thấy BookingId {bookingId}");
            }

            if (booking.Showtime == null)
            {
                return Content($"LỖI: Booking không liên kết được với Showtime.");
            }

            var viewModel = new ETicketViewModel // Đảm bảo tên Class này khớp với @model ở View
            {
                BookingId = booking.BookingId,
                MovieTitle = booking.Showtime?.Movie?.Title ?? "N/A",
                PosterUrl = booking.Showtime?.Movie?.PosterUrl,
                ScreenName = booking.Showtime?.Screen?.ScreenName ?? "N/A",

                // Gán giá tiền (Lấy 110.00 từ DB)
                Price = booking.TotalPrice > 0 ? booking.TotalPrice : (booking.Showtime?.Price ?? 0),

                // Gán thời gian (Sẽ sửa được lỗi 01/01/0001)
                StartTime = booking.Showtime?.StartTime ?? DateTime.Now,
                EndTime = booking.Showtime?.EndTime ?? DateTime.Now,

                // Xử lý hiển thị ghế (Nếu không có dòng này, ghế sẽ bị trống)
                SeatRow = string.Join(", ", booking.BookingSeats.Select(bs => bs.Seat.SeatRow).Distinct()),
                SeatNumber = string.Join(", ", booking.BookingSeats.Select(bs => bs.Seat.SeatNumber))
            };

            return View("E_ticket", viewModel);
        }
        public IActionResult seat_sel(int showtimeId)
        {
            // 1. Lấy thông tin suất chiếu cùng với Phim và Phòng chiếu
            var showtime = _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .FirstOrDefault(s => s.ShowtimeId == showtimeId);

            if (showtime == null) return NotFound();

            // 2. Lấy tất cả ghế thuộc Phòng chiếu này
            // Sắp xếp theo Hàng và Số ghế để vẽ bản đồ chính xác
            var allSeats = _context.Seats
                .Where(s => s.ScreenId == showtime.ScreenId)
                .OrderBy(s => s.SeatRow)
                .ThenBy(s => s.SeatNumber)
                .ToList();

            // 3. Tạo danh sách các hàng (A, B, C...) để tính chỉ số index (1, 2, 3...)
            var rowList = allSeats.Select(s => s.SeatRow).Distinct().ToList();

            // 4. Tạo mảng "map" cho thư viện seat-charts (Ví dụ: ["aaaaa", "aaaaa"])
            var seatMap = allSeats
                .GroupBy(s => s.SeatRow)
                .Select(group => new string('a', group.Count()))
                .ToList();

            // 5. LẤY DANH SÁCH GHẾ ĐÃ ĐẶT (QUAN TRỌNG)
            // Truy vấn các ghế đã có trong bảng BookingSeats ứng với suất chiếu này
            var bookedSeats = _context.BookingSeats
                .Where(bs => bs.Booking.ShowtimeId == showtimeId)
                .Select(bs => new { bs.Seat.SeatRow, bs.Seat.SeatNumber })
                .AsEnumerable() // Chuyển về xử lý C# để dùng IndexOf
                .Select(x => $"{rowList.IndexOf(x.SeatRow) + 1}_{x.SeatNumber}")
                .ToList();

            // 6. Tạo Map ID để gửi về View (Ánh xạ ID của thư viện sang ID thật trong DB)
            // Key: "1_5" (Hàng_Số), Value: SeatId (int)
            var seatIdMap = allSeats.ToDictionary(
                s => $"{rowList.IndexOf(s.SeatRow) + 1}_{s.SeatNumber}",
                s => s.SeatId
            );

            // 7. Gửi dữ liệu sang ViewBag
            ViewBag.SeatMap = seatMap;
            ViewBag.BookedSeats = bookedSeats; // Danh sách này sẽ làm ghế có màu đỏ
            ViewBag.SeatIdMap = seatIdMap;
            ViewBag.ShowtimeId = showtimeId;
            ViewBag.MovieTitle = showtime.Movie?.Title;
            ViewBag.Price = showtime.Price;
            ViewBag.StartTime = showtime.StartTime.ToString("dd/MM/yyyy HH:mm");

            return View();
        }

        [HttpPost]
        public IActionResult BookSeats([FromBody] BookingRequest request)
        {
            // 1. Kiểm tra đăng nhập qua Session
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized(new { message = "Vui lòng đăng nhập để thực hiện đặt vé!" });
            }

            // 2. Kiểm tra dữ liệu đầu vào
            if (request?.SeatIds == null || !request.SeatIds.Any())
            {
                return BadRequest(new { message = "Bạn chưa chọn ghế. Vui lòng chọn ít nhất một ghế." });
            }

            // 3. Kiểm tra suất chiếu và lấy thông tin giá/thời gian
            var showtime = _context.Showtimes
                .Include(s => s.Movie)
                .FirstOrDefault(s => s.ShowtimeId == request.ShowtimeId);

            if (showtime == null)
            {
                return NotFound(new { message = "Suất chiếu không tồn tại hoặc đã bị hủy." });
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 4. Kiểm tra xem ghế đã bị ai khác đặt trong lúc mình đang chọn không (Race Condition)
                    var alreadyBooked = _context.BookingSeats
                        .Where(bs => bs.Booking.ShowtimeId == request.ShowtimeId)
                        .Select(bs => bs.SeatId)
                        .ToList();

                    if (request.SeatIds.Intersect(alreadyBooked).Any())
                    {
                        return BadRequest(new { message = "Một trong số các ghế bạn chọn vừa có người đặt. Vui lòng chọn lại ghế khác." });
                    }

                    // 5. Tạo đối tượng Booking mới
                    var booking = new Bookings()
                    {
                        UserId = userId,
                        ShowtimeId = request.ShowtimeId,
                        BookingDate = DateTime.Now,
                        // Ưu tiên giá từ Showtime, nếu không có lấy mặc định 110k
                        TotalPrice = request.SeatIds.Count * (showtime.Price > 0 ? showtime.Price : 110000),
                        Status = "Success"
                    };

                    _context.Bookings.Add(booking);

                    // 6. Lưu danh sách ghế đã chọn và lấy tên ghế để trả về Client
                    var seatDetails = _context.Seats
                        .Where(s => request.SeatIds.Contains(s.SeatId))
                        .ToList();

                    foreach (var seat in seatDetails)
                    {
                        _context.BookingSeats.Add(new BookingSeats
                        {
                            Booking = booking, // EF tự động lấy BookingId sau khi SaveChanges
                            SeatId = seat.SeatId
                        });
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    // 7. TRẢ VỀ DỮ LIỆU ĐỂ HIỂN THỊ TẠI BƯỚC 4
                    return Ok(new
                    {
                        success = true,
                        bookingId = booking.BookingId,
                        totalPrice = booking.TotalPrice,
                        // Trả về chuỗi ghế ví dụ: "A1, A2"
                        seats = string.Join(", ", seatDetails.Select(s => s.SeatRow + s.SeatNumber)),
                        // Trả về thời gian để cập nhật giao diện
                        startTime = showtime.StartTime.ToString("HH:mm"),
                        endTime = showtime.EndTime.ToString("HH:mm"),
                        date = showtime.StartTime.ToString("dd/MM/yyyy")
                    });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, new { message = "Lỗi hệ thống khi lưu vé: " + ex.Message });
                }
            }
        }
        public IActionResult Ticket_Booking(int id)
        {
            var showtimes = _context.Showtimes
                .Include(s => s.Screen)
                .Where(s => s.MovieId == id)
                .ToList();

            var movie = _context.Movies.FirstOrDefault(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound(); // hoặc redirect
            }

            var model = new BookingViewModel
            {
                Showtimes = showtimes,
                MovieTitle = movie.Title,
                PosterUrl = movie.PosterUrl
            };

            return View(model);
        }

        public IActionResult BookingHistory()
        {
            // 1. Kiểm tra đăng nhập qua Claims (Đồng bộ với User.Identity.IsAuthenticated ở Header)
            // Lấy UserId từ Claim NameIdentifier đã lưu khi đăng nhập
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Sign_In");
            }

            int userId = int.Parse(userIdStr);

            // 2. Bước 1: Query lấy dữ liệu thô từ Database (Flat data) - GIỮ NGUYÊN LOGIC
            var rawData = (from b in _context.Bookings
                           join st in _context.Showtimes on b.ShowtimeId equals st.ShowtimeId
                           join m in _context.Movies on st.MovieId equals m.MovieId
                           join s in _context.Screens on st.ScreenId equals s.ScreenId
                           join bs in _context.BookingSeats on b.BookingId equals bs.BookingId
                           join se in _context.Seats on bs.SeatId equals se.SeatId
                           where b.UserId == userId
                           select new
                           {
                               b.BookingId,
                               b.TotalPrice,
                               b.Status,
                               MovieTitle = m.Title,
                               ScreenName = s.ScreenName,
                               BookingDate = b.BookingDate,
                               StartTime = st.StartTime,
                               SRow = se.SeatRow,    // Kiểu CHAR(1)
                               SNum = se.SeatNumber  // Kiểu INT
                           }).ToList();

            // 3. Bước 2: Grouping dữ liệu trên RAM để tách Row và Seat - GIỮ NGUYÊN LOGIC
            var data = rawData
                .GroupBy(x => new
                {
                    x.BookingId,
                    x.BookingDate,
                    x.TotalPrice,
                    x.Status,
                    x.MovieTitle,
                    x.ScreenName,
                    x.StartTime
                })
                .Select(g => new BookingHistoryViewModel
                {
                    BookingId = g.Key.BookingId,
                    BookingDate = g.Key.BookingDate,
                    TotalPrice = g.Key.TotalPrice,
                    Status = g.Key.Status,
                    MovieTitle = g.Key.MovieTitle,
                    ScreenName = g.Key.ScreenName,
                    StartTime = g.Key.StartTime,

                    // Tách Row: Lấy các chữ cái hàng duy nhất, ví dụ: "A, B"
                    SRow = string.Join(", ", g.Select(x => x.SRow).Distinct().OrderBy(r => r)),

                    // Tách Seat: Lấy các số ghế, ví dụ: "1, 2, 5"
                    Seats = string.Join(", ", g.Select(x => x.SNum).OrderBy(n => n))
                })
                .OrderByDescending(x => x.BookingDate)
                .ToList();

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Xóa Cookie định danh
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Xóa Session nếu có
            HttpContext.Session.Clear();

            return RedirectToAction("Sign_In", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Rất quan trọng để chống tấn công CSRF
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            // 1. Lấy ID người dùng từ Claims (đã lưu khi Login)
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập lại." });
            }

            int userId = int.Parse(userIdStr);

            // 2. Tìm User trong Database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return Json(new { success = false, message = "Người dùng không tồn tại." });
            }

            // 3. Kiểm tra mật khẩu cũ (Dùng PasswordHasher vì bạn lưu mật khẩu dạng Hash)
            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, oldPassword);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Json(new { success = false, message = "Mật khẩu cũ không chính xác." });
            }

            // 4. Kiểm tra mật khẩu mới không trùng mật khẩu cũ
            if (oldPassword == newPassword)
            {
                return Json(new { success = false, message = "Mật khẩu mới không được giống mật khẩu cũ." });
            }

            // 5. Cập nhật mật khẩu mới (Hash trước khi lưu)
            user.PasswordHash = passwordHasher.HashPassword(user, newPassword);

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
