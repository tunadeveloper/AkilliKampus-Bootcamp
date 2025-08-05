using Microsoft.AspNetCore.Mvc;
using Bootcamp.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace Bootcamp.PresentationLayer.Controllers
{
    [Authorize]
    public class PdfSummaryController : Controller
    {
        private readonly IGeminiService _geminiService;

        public PdfSummaryController(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SummarizePdf(IFormFile pdfFile)
        {
            try
            {
                if (pdfFile == null || pdfFile.Length == 0)
                {
                    return Json(new { success = false, message = "Lütfen bir PDF dosyası seçin." });
                }

                if (!pdfFile.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { success = false, message = "Sadece PDF dosyaları kabul edilir." });
                }

                if (pdfFile.Length > 10 * 1024 * 1024) // 10MB limit
                {
                    return Json(new { success = false, message = "PDF dosyası 10MB'dan büyük olamaz." });
                }

                // PDF içeriğini oku
                using var stream = pdfFile.OpenReadStream();
                var pdfContent = await ExtractTextFromPdf(stream);

                if (string.IsNullOrWhiteSpace(pdfContent))
                {
                    return Json(new { success = false, message = "PDF'den metin çıkarılamadı. Dosyanın metin içerdiğinden emin olun." });
                }

                // Gemini ile özetle
                var summary = await _geminiService.SummarizePdfContentAsync(pdfContent, pdfFile.FileName);

                return Json(new { success = true, summary = summary });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata oluştu: {ex.Message}" });
            }
        }

        private async Task<string> ExtractTextFromPdf(Stream pdfStream)
        {
            try
            {
                using var pdfReader = new iTextSharp.text.pdf.PdfReader(pdfStream);
                var text = new StringBuilder();

                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    var pageText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, i);
                    text.AppendLine(pageText);
                }

                return text.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"PDF okuma hatası: {ex.Message}");
            }
        }
    }
} 