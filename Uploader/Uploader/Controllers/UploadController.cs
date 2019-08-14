using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using BusinessLayer;
using Microsoft.Extensions.Configuration;

namespace Uploader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IStreamManager _streamManager;
        private readonly int _fileSizeLimit;

        public UploadController(IStreamManager streamManager, IConfiguration configuration)
        {
            _streamManager = streamManager;
            _fileSizeLimit = int.Parse(configuration["FileSizeLimit"]);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            if (!IsMultipartContentType(Request.ContentType))
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");

            var boundary = GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType));
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            if (section == null) return BadRequest($"Expecting 1 multipart section.");

            if (!ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
                return BadRequest($"ContentDisposition has a wrong value '{section.ContentDisposition}'");

            if (StringSegment.IsNullOrEmpty(contentDisposition.FileName))
                return BadRequest($"Filename is missing");

            await _streamManager.ProcessStream(section.Body);
            return Ok("File saved");
        }

        private static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string GetBoundary(MediaTypeHeaderValue contentType)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
            if (StringSegment.IsNullOrEmpty(boundary))
                throw new InvalidDataException("Missing content-type boundary.");

            return boundary.ToString();
        }
    }
}