using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VPT_Movie_Box.Data; // Thêm namespace này để gọi AdminDbContext
using VPT_Movie_Box.Models;

namespace VPT_Movie_Box.Areas.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AdminDbContext _context; // 1. Khai báo DbContext

        // 2. Tiêm (Inject) AdminDbContext vào Constructor
        public HomeController(ILogger<HomeController> logger, AdminDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // --- XỬ LÝ LOGIC DOANH THU ĐỘNG ---
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            // Tính doanh thu ngày hôm nay (Bỏ qua các vé có trạng thái hủy 'Cancelled' nếu có)
            decimal todayRevenue = _context.Bookings
                .Where(b => b.BookingDate >= today && b.BookingDate < today.AddDays(1) && b.Status != "Cancelled")
                .Sum(b => (decimal?)b.TotalPrice) ?? 0;

            // Tính doanh thu ngày hôm qua để làm mốc so sánh %
            decimal yesterdayRevenue = _context.Bookings
                .Where(b => b.BookingDate >= yesterday && b.BookingDate < today && b.Status != "Cancelled")
                .Sum(b => (decimal?)b.TotalPrice) ?? 0;

            // Tính % tăng trưởng so với hôm qua
            decimal growthPercentage = 0;
            if (yesterdayRevenue > 0)
            {
                growthPercentage = ((todayRevenue - yesterdayRevenue) / yesterdayRevenue) * 100;
            }
            else if (todayRevenue > 0)
            {
                growthPercentage = 100; // Nếu hôm qua bằng 0 và hôm nay có doanh thu -> tăng 100%
            }

            // 3. Đút dữ liệu vào ViewBag để ném sang file hiển thị (.cshtml)
            ViewBag.TodayRevenue = todayRevenue;
            ViewBag.GrowthPercentage = growthPercentage;
            // ----------------------------------

            // ==========================================
            // 2. XỬ LÝ LOGIC VÉ BÁN HÔM NAY (MỚI)
            // ==========================================
            // Đếm tổng số đơn đặt vé thành công trong ngày hôm nay
            int todayTickets = _context.Bookings
                .Where(b => b.BookingDate >= today && b.BookingDate < today.AddDays(1) && b.Status != "Cancelled")
                .Count();

            // Đếm tổng số đơn đặt vé thành công trong ngày hôm qua
            int yesterdayTickets = _context.Bookings
                .Where(b => b.BookingDate >= yesterday && b.BookingDate < today && b.Status != "Cancelled")
                .Count();

            // Tính % tăng trưởng lượng vé so với hôm qua
            decimal ticketGrowthPercentage = 0;
            if (yesterdayTickets > 0)
            {
                // Ép kiểu decimal để phép chia không bị mất phần thập phân
                ticketGrowthPercentage = ((decimal)(todayTickets - yesterdayTickets) / yesterdayTickets) * 100;
            }
            else if (todayTickets > 0)
            {
                ticketGrowthPercentage = 100; // Hôm qua không bán được vé nào, hôm nay bán được => tăng trưởng 100%
            }

            ViewBag.TodayTickets = todayTickets;
            ViewBag.TicketGrowthPercentage = ticketGrowthPercentage;

            // ==========================================
            // 3. LOGIC BIỂU ĐỒ TRÒN & 3 Ô THÔNG SỐ DƯỚI ĐÁY
            // ==========================================
            decimal targetOfMonth = 20000m; // Target tháng (Bạn có thể đổi số này)
            var firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            // A. Tính tổng doanh thu tháng này
            decimal monthRevenue = _context.Bookings
                .Where(b => b.BookingDate >= firstDayOfMonth && b.BookingDate < DateTime.Today.AddDays(1) && b.Status != "Cancelled")
                .Sum(b => (decimal?)b.TotalPrice) ?? 0;

            // B. Tính % đạt được so với Target
            decimal percentTarget = 0;
            if (targetOfMonth > 0)
            {
                percentTarget = (monthRevenue / targetOfMonth) * 100;
                if (percentTarget > 100) percentTarget = 100; // Giới hạn max 100% trên biểu đồ
            }

            // C. So sánh doanh thu với tháng trước (Để hiển thị cái huy hiệu +10% màu xanh)
            var firstDayOfLastMonth = firstDayOfMonth.AddMonths(-1);
            decimal lastMonthRevenue = _context.Bookings
                .Where(b => b.BookingDate >= firstDayOfLastMonth && b.BookingDate < firstDayOfMonth && b.Status != "Cancelled")
                .Sum(b => (decimal?)b.TotalPrice) ?? 0;

            decimal monthGrowth = 0;
            if (lastMonthRevenue > 0)
                monthGrowth = ((monthRevenue - lastMonthRevenue) / lastMonthRevenue) * 100;
            else if (monthRevenue > 0)
                monthGrowth = 100;

            // Gửi toàn bộ ra View
            ViewBag.Target = targetOfMonth;
            ViewBag.MonthRevenue = monthRevenue;
            ViewBag.PercentTarget = percentTarget;
            ViewBag.MonthGrowth = monthGrowth;
            // (ViewBag.TodayRevenue thì bạn đã tính ở đoạn code đầu tiên rồi, ta sẽ dùng luôn)

            return View();
        }

        public IActionResult Privacy() => View();
        public IActionResult FormElement() => View();
        public IActionResult Profile() => RedirectToAction("Index", "Profile");
        public IActionResult Movies() => RedirectToAction("Index", "Movies");
        public IActionResult TransHis() => RedirectToAction("Index", "TransactionHistory");

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}