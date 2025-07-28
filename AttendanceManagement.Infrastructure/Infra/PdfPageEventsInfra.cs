using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class PdfPageEvents : PdfPageEventHelper
    {
        private readonly Image _footerImage;
        private readonly Image _watermarkImage;

        public PdfPageEvents(string footerPath, string watermarkPath)
        {
            if (File.Exists(footerPath))
            {
                _footerImage = Image.GetInstance(footerPath);
                _footerImage.ScaleToFit(500f, 40f);
            }

            if (File.Exists(watermarkPath))
            {
                _watermarkImage = Image.GetInstance(watermarkPath);
                _watermarkImage.ScaleToFit(300f, 300f); // adjust size as needed
            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            Console.WriteLine($"OnEndPage triggered on Page: {writer.PageNumber}");

            PdfContentByte canvas = writer.DirectContentUnder;

            if (_watermarkImage != null)
            {
                float width = 300f;  // Force good visibility size
                float height = 300f;

                _watermarkImage.ScaleToFit(width, height);

                float x = (document.PageSize.Width - width) / 2;
                float y = (document.PageSize.Height - height) / 2;

                _watermarkImage.SetAbsolutePosition(x, y);

                PdfGState gState = new PdfGState
                {
                    FillOpacity = 0.5f // Start with 40% for visibility testing
                };

                canvas.SaveState();
                canvas.SetGState(gState);
                canvas.AddImage(_watermarkImage);
                canvas.RestoreState();
            }

            if (_footerImage != null)
            {
                float x = (document.PageSize.Width - _footerImage.ScaledWidth) / 2;
                float y = document.Bottom - 25f;

                _footerImage.SetAbsolutePosition(x, y);
                writer.DirectContent.AddImage(_footerImage);
            }
        }
    }
}