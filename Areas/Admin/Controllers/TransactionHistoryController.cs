using Microsoft.AspNetCore.Mvc;
using VPT_Movie_Box.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VPT_Movie_Box.Data; // Thay bằng namespace chứa ApplicationDbContext của bạn


namespace VPT_Movie_Box.Controllers
{
    [Area("Admin")]
    public class TransactionHistoryController : Controller
    {
        private readonly AdminDbContext _context;

        public TransactionHistoryController(AdminDbContext context)
        {
            _context = context;
        }

        // Thêm tham số page (mặc định là 1 nếu không truyền)
        public IActionResult Index(int page = 1)
        {
            int pageSize = 20; // Giới hạn 20 dòng trên 1 trang
            //var fiveDaysAgo = DateTime.Now.AddDays(-5);

            try
            {
                // 1. Tạo query cơ bản (chưa lấy dữ liệu ngay - chưa gọi ToList)
                var query = _context.Bookings
                    //.Where(b => b.BookingDate >= fiveDaysAgo)
                    .OrderByDescending(b => b.BookingDate);

                // 2. Đếm tổng số bản ghi thỏa mãn để tính số trang
                int totalRecords = query.Count();
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Đảm bảo số trang luôn hợp lệ
                if (page < 1) page = 1;
                if (page > totalPages && totalPages > 0) page = totalPages;

                // 3. Phân trang bằng Skip() và Take()
                var transactions = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(b => new TransactionHistoryViewModel
                    {
                        TransactionId = $"#MTB-{b.BookingId:D5}",
                        Date = b.BookingDate ?? DateTime.Now,
                        Amount = b.TotalPrice ?? 0,
                        Status = b.Status
                    })
                    .ToList();

                // 4. Ném dữ liệu phân trang sang View qua ViewBag
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;

                return View(transactions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LỖI: {ex.Message}");
                ViewBag.CurrentPage = 1;
                ViewBag.TotalPages = 1;
                return View(new List<TransactionHistoryViewModel>());
            }
        }

        public IActionResult Details(string id)
        {
            // Lưu tạm id vào ViewBag để hiển thị lên View
            ViewBag.TransactionId = id;
            return View();
        }
    }
}
