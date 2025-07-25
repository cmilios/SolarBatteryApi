using MediatR;
using Microsoft.AspNetCore.Mvc;
using SPCS.API.Requests;
using SPCS.Application.Files.Commands;
using SPCS.Files.Enums;

namespace SPCS.API.Controllers
{
    public class FileController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [HttpPut("/upload")]
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
            };

            var result = await _mediator.Send(command, cancellationToken);


            return Ok(result);
        }
    }
}
