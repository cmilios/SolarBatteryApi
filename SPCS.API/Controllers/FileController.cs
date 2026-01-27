using MediatR;
using Microsoft.AspNetCore.Mvc;
using SPCS.API.Requests;
using SPCS.Application.Files.Commands;
using SPCS.Files.Enums;

namespace SPCS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [HttpPut("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Upload(
            [FromForm] FileUploadRequest request,
            CancellationToken cancellationToken)
        {
            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms, cancellationToken);
            var command = new FileUploadCommand
            {
                FileName = request.File.FileName,
                Content = ms.ToArray(),
                ContentType = request.File.ContentType,
                FileType = Enum.IsDefined(typeof(FileType), request.Type) ? (FileType)request.Type : FileType.Unknown,
                ApplicationId = request.ApplicationId
            };

            var result = await _mediator.Send(command, cancellationToken);


            return Ok(result);
        }

        [HttpGet]
        public async Task<IEnumerable<SPCS.Files.Dtos.FileDto>> GetAll(CancellationToken cancellationToken)
        {
            var query = new SPCS.Application.Files.Queries.GetFilesQuery();
            return await _mediator.Send(query, cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SPCS.Files.Dtos.FileDto?>> GetById(int id, CancellationToken cancellationToken)
        {
            var query = new SPCS.Application.Files.Queries.GetFileByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(int id, CancellationToken cancellationToken)
        {
            var query = new SPCS.Application.Files.Queries.DownloadFileQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            if (result == null) return NotFound();
            return File(result.Content, result.ContentType, result.Name);
        }

        [HttpPost("{id}/parse")]
        public async Task<IActionResult> Parse(int id, CancellationToken cancellationToken)
        {
            var command = new SPCS.Application.Files.Commands.ParseFileCommand { FileId = id };
            var result = await _mediator.Send(command, cancellationToken);
            if (!result) return BadRequest();
            return Ok();
        }
    }
}
