using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VPT_Movie_Box.Data;
using VPT_Movie_Box.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace VPT_Movie_Box.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        private readonly AdminDbContext _context;

        // Tiêm DbContext vào Controller qua Constructor
        public MoviesController(AdminDbContext context)
        {
            _context = context;
        }

        // Action Index để lấy danh sách phim
        public async Task<IActionResult> Index()
        {
            // Lấy toàn bộ phim từ DB, có thể thêm .OrderByDescending(m => m.MovieId) nếu muốn
            var movies = await _context.Movies.ToListAsync();

            // Trả data về cho View
            return View(movies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoviesViewModel movie, IFormFile PosterFile)
        {
            // Loại bỏ MovieId khỏi kiểm tra vì nó tự tăng trong DB
            ModelState.Remove("MovieId");
            ModelState.Remove("PosterUrl");

            if (ModelState.IsValid)
            {
                // 1. Xử lý file ảnh
                if (PosterFile != null && PosterFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(PosterFile.FileName);
                    // Lưu ý: Đảm bảo thư mục "wwwroot/uploads" đã được tạo
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PosterFile.CopyToAsync(stream);
                    }
                    movie.PosterUrl = "/uploads/" + fileName;
                }

                // 2. Lưu vào Database
                _context.Movies.Add(movie); // Đảm bảo tên DbSet trong DbContext là Movies
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi, trả về View cùng dữ liệu đã nhập để hiện thông báo đỏ
            return View(movie);
        }
    }
}
