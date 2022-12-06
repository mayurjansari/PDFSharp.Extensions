using System.Collections.Generic;
using SixLabors.ImageSharp;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;
using SixLabors.ImageSharp.Formats.Png;
using static MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource;
using SixLabors.ImageSharp.PixelFormats;

// ReSharper disable CheckNamespace
namespace PdfSharpCore.Images;

/// <summary>
/// Extension methods for the <see cref="Image"/> class.
/// </summary>
public static class ImageExtensions
{

/// <summary>
/// Generates a PDF page from the image.
/// </summary>
/// <param name="image">The image to convert to a <see cref="PdfPage"/>.</param>
/// <returns>The <see cref="PdfPage"/> containing the image.</returns>
public static PdfDocument ToPdf(this Image image)
{
  if (image == null) return (new PdfDocument());
  return (ToPdf(new[] { image }));
}

/// <summary>
/// Generates a PDF document from the collection of enumerations with each image representing
/// a new page.
/// </summary>
/// <param name="images">The collection of images to convert to pages.</param>
/// <returns>The <see cref="PdfDocument"/> containing the images.</returns>
public static PdfDocument ToPdf(this IEnumerable<Image> images)
{
  PdfDocument document = new PdfDocument();
  PngFormat fmt = PngFormat.Instance;
  foreach (var image in images) {
    PdfPage page = new PdfPage() {
      Width = image.Width,
      Height = image.Height
    };
    document.AddPage(page);

    XGraphics xGraphics = XGraphics.FromPdfPage(page);
    using Image<Rgba32> tmp = image.CloneAs<Rgba32>();
    IImageSource source = ImageSharpImageSource<Rgba32>.FromImageSharpImage(tmp, fmt);
    XImage xImage = XImage.FromImageSource(source);
    xGraphics.DrawImage(xImage, 0, 0, image.Width, image.Height);
  }
  return (document);
}

}
