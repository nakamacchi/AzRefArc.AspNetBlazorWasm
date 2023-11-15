using Microsoft.AspNetCore.Mvc;

namespace AzRefArc.AspNetWebApi.Controllers.Utilities
{
    [ApiController]
    [Route("/api/Utilities/ReportClientError")]
    public class ReportClientErrorController : ControllerBase
    {
        private ILogger Logger { get; set; }

        public ReportClientErrorController(ILogger<ReportClientErrorController> logger)
        {
            Logger = logger;
        }


        [HttpPost("UploadErrorInformation")]
        public void UploadErrorInformation([FromBody] string errorMessage)
        {
            Logger.LogError("クライアントブラウザからエラーが報告されました。\r\n" + errorMessage);
        }
    }
}
