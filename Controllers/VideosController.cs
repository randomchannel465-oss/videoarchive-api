using Microsoft.AspNetCore.Mvc;
using VideoArchiveAPI.Models;

namespace VideoArchiveAPI.Controllers
{
    [ApiController]
    [Route("api/videos")]
    public class VideosController : ControllerBase
    {
        private readonly string videosFolder =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Videos");

        private readonly string thumbnailsFolder =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Thumbnails");

        [HttpGet]
        public IActionResult GetVideos()
        {
            Directory.CreateDirectory(videosFolder);

            Directory.CreateDirectory(thumbnailsFolder);

            List<object> videos =
                new List<object>();

            string[] files =
                Directory.GetFiles(videosFolder);

            foreach (string file in files)
            {
                string ext =
                    Path.GetExtension(file)
                    .ToLower();

                if (ext != ".mp4")
                    continue;

                string name =
                    Path.GetFileNameWithoutExtension(file);

                string thumbnail =
                    $"{Request.Scheme}://{Request.Host}/Thumbnails/{name}.jpg";

                string pngThumbnail =
                    Path.Combine(
                        thumbnailsFolder,
                        name + ".png");

                if (System.IO.File.Exists(pngThumbnail))
                {
                    thumbnail =
                        $"{Request.Scheme}://{Request.Host}/Thumbnails/{name}.png";
                }

                videos.Add(new
                {
                    title = name,

                    url =
                        $"{Request.Scheme}://{Request.Host}/Videos/{Path.GetFileName(file)}",

                    thumbnail = thumbnail
                });
            }

            return Ok(videos);
        }

        [HttpPost("upload")]
        [RequestSizeLimit(500_000_000)]
        public async Task<IActionResult> Upload(
            [FromForm] UploadModel model)
        {
            if (model.Video == null)
            {
                return BadRequest(
                    "Kein Video ausgewählt");
            }

            Directory.CreateDirectory(videosFolder);

            Directory.CreateDirectory(thumbnailsFolder);

            string videoFileName =
                Path.GetFileName(
                    model.Video.FileName);

            string videoPath =
                Path.Combine(
                    videosFolder,
                    videoFileName);

            using (FileStream stream =
                   new FileStream(
                       videoPath,
                       FileMode.Create))
            {
                await model.Video.CopyToAsync(stream);
            }

            if (model.Thumbnail != null)
            {
                string thumbnailFileName =
                    Path.GetFileName(
                        model.Thumbnail.FileName);

                string thumbnailPath =
                    Path.Combine(
                        thumbnailsFolder,
                        thumbnailFileName);

                using (FileStream stream =
                       new FileStream(
                           thumbnailPath,
                           FileMode.Create))
                {
                    await model.Thumbnail.CopyToAsync(stream);
                }
            }

            return Ok(new
            {
                success = true
            });
        }
    }
}