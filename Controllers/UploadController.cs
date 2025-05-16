using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ConversationBackend.Services;
using ConversationBackend.Models;

namespace ConversationBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public UploadController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file
            , string conversationId
            )
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            Console.WriteLine(conversationId);
            var allowedTypes = new[] { "image/png", "image/jpeg", "application/pdf" };
            if (!Array.Exists(allowedTypes, type => type == file.ContentType))
            {
                return BadRequest("Invalid file type");
            }

            if (file.Length > 15 * 1024 * 1024)
                return BadRequest("Oversize");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var fileDocument = new FileDocument
            {
                ConversationId = conversationId,
                Filename = file.FileName,
                ContentType = file.ContentType,
                Data = fileBytes,
                UploadDate = DateTime.UtcNow


            };

            //Generate file name
            //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            ////Check the upload directory Exists
            //Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            ////save
            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await file.CopyToAsync(stream);
            //}

            //var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            //var fileMetadata = new
            //{
            //    FileId = fileName,
            //    FileUrl = fileUrl,
            //    FileType = file.ContentType,
            //    UploadDate = DateTime.UtcNow,
            //    OriginalFileName = file.FileName
            //};

            //await _mongoDbService.GetCollection<object>("fileMetdata").InsertOneAsync(fileMetadata);

            var filesCollection = _mongoDbService.GetCollection<FileDocument>("files");
            await filesCollection.InsertOneAsync(fileDocument);

            return Ok(new { fileId = fileDocument.Id.ToString() });
        }


    
    }
}
