using Google.Cloud.Vision.V1;
using Grpc.Core; // để bắt RpcException
using System;
using System.Linq;

namespace SmartEXE.Services
{
    public class VisionService
    {
        private readonly ImageAnnotatorClient _client;

        // Có thể dùng DI: đăng ký ImageAnnotatorClient trong Program.cs rồi inject vào đây
        public VisionService(ImageAnnotatorClient? client = null)
        {
            // Nếu chưa dùng DI thì tạo từ ADC (đọc GOOGLE_APPLICATION_CREDENTIALS)
            _client = client ?? ImageAnnotatorClient.Create();
        }

        /// <summary>
        /// Phân tích ảnh base64 và trả về mô tả vật thể/cảnh.
        /// </summary>
        public string AnalyzeBase64Image(string base64Image)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
                return "Không có ảnh để phân tích.";

            try
            {
                var clean = ExtractBase64(base64Image);
                var bytes = Convert.FromBase64String(clean);
                var image = Image.FromBytes(bytes);

                // Lấy tối đa 10 nhãn, hiển thị 5 nhãn đầu
                var labels = _client.DetectLabels(image, maxResults: 10);
                if (labels == null || labels.Count == 0)
                    return "Không nhận diện được vật thể nào.";

                var result = string.Join(", ",
                    labels.Take(5).Select(l => $"{l.Description} ({l.Score:P0})"));

                return result;
            }
            catch (FormatException)
            {
                return "Ảnh base64 không hợp lệ.";
            }
            catch (RpcException ex) when (
                ex.StatusCode == StatusCode.PermissionDenied &&
                (ex.Status.Detail?.IndexOf("billing", StringComparison.OrdinalIgnoreCase) ?? -1) >= 0)
            {
                // Trường hợp phổ biến nhất bạn vừa gặp
                return "Google Vision: Project GCP chưa gắn Billing hoặc key thuộc project chưa có Billing.";
            }
            catch (RpcException ex) when (
                ex.StatusCode == StatusCode.Unauthenticated ||
                ex.StatusCode == StatusCode.PermissionDenied)
            {
                var keyPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
                var hint = string.IsNullOrEmpty(keyPath)
                    ? "Biến GOOGLE_APPLICATION_CREDENTIALS chưa được thiết lập."
                    : $"Đang dùng key: {keyPath}. Hãy kiểm tra quyền của service account.";
                return $"Google Vision không thể xác thực/quyền không đủ. {hint}";
            }
            catch (RpcException ex)
            {
                return $"Google Vision lỗi ({ex.StatusCode}): {ex.Status.Detail}";
            }
            catch (Exception ex)
            {
                return $"Lỗi xử lý ảnh: {ex.Message}";
            }
        }

        private static string ExtractBase64(string s)
        {
            var i = s.IndexOf(',');
            return i >= 0 ? s[(i + 1)..] : s;
        }
    }
}
