using Microsoft.AspNetCore.Mvc;
using SmartEXE.Services;

namespace SmartEXE.Controllers   // ✅ Đặt trong namespace Controllers, KHÔNG phải Pages.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScanController : ControllerBase   // ✅ Dùng ControllerBase, không phải Controller (vì đây là API)
    {
        private readonly VisionService _visionService;

        public ScanController()
        {
            _visionService = new VisionService();  // ✅ Gọi service xử lý ảnh
        }

        [HttpPost("upload")]
        public IActionResult Upload([FromBody] ImageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Base64Image))
                return BadRequest(new { message = "Không có dữ liệu ảnh" });

            var result = _visionService.AnalyzeBase64Image(request.Base64Image);
            return Ok(new { message = result });
        }

        // ✅ Class nhỏ để nhận dữ liệu JSON từ frontend
        public class ImageRequest
        {
            public string? Base64Image { get; set; }
        }
    }
}
