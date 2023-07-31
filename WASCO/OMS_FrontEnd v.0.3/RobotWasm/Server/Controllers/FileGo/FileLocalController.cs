using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotWasm.Shared.Data.ML.Shared;

namespace RobotWasm.Server.Controllers.FileGo {
    [Route("api/[controller]")]
    [ApiController]
    public class FileLocalController : ControllerBase {
		[HttpPost("UpFile")]
		public async Task UpFile([FromBody] ImageFile[] files) { 
			foreach (var file in files) {
				var buf = Convert.FromBase64String(file.base64data); 
				string upload_path = @"D:\ImageStorage\Asset\" + file.fileName;
				await System.IO.File.WriteAllBytesAsync(upload_path, buf);
			}
			//api/FileLocal/UpFile
		}
		[HttpGet("DownFile")]
		public IActionResult DownFile() {
			string filePath = @"D:\ImageStorage\Asset\2022-11-25_20-20-27.png"; 
			if (!System.IO.File.Exists(filePath)) {
				//	contentType = "image/jpeg";
				filePath = "default_notfound_image_path_here";
			}
			var data = PhysicalFile(filePath, "image/png");
			// src="https://xxxxx/FetchImageAndVideo/2/mytestvideo.mp4"
			return data;
		}
	}
}
