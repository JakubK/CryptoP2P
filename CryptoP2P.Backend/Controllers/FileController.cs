using CryptoP2P.Backend.Messages;
using Microsoft.AspNetCore.Mvc;

namespace CryptoP2P.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
  private readonly IWebHostEnvironment _webHostEnvironment;
  public FileController(IWebHostEnvironment webHostEnvironment)
  {
    _webHostEnvironment = webHostEnvironment;
  }

  [HttpPost]
  public async Task<ActionResult<ChatMessage>> PostFile(IFormFile formFile)
  {
    var path = Path.Combine(_webHostEnvironment.WebRootPath, "static");
    var filePath = Path.Combine(path, formFile.FileName);
    using var fileStream = new FileStream(filePath, FileMode.Create);
    await formFile.CopyToAsync(fileStream);

    var outputPath = Path.Combine("static", formFile.FileName);
    
    return Ok(new ChatMessage
    {
      Message = outputPath
    });
  }
}