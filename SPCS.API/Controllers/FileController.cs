using MediatR;
using Microsoft.AspNetCore.Mvc;
using SPCS.Application.Files.Commands;
using SPCS.Files.Enums;

namespace SPCS.API.Controllers
{
    public class FileController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [HttpPut("/upload")]
        public async Task<ActionResult> Upload([FromForm] IFormFile file, [FromForm] int type, CancellationToken cancellationToken)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, cancellationToken);
            var command = new FileUploadCommand
            {
                FileName = file.FileName,
                Content = ms.ToArray(),
                ContentType = file.ContentType,
                FileType = Enum.IsDefined(typeof(FileType), type) ? (FileType)type : FileType.Unknown,
            };

            var result = await _mediator.Send(command, cancellationToken);


            return Ok(result);
        }
    }
}
