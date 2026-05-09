namespace VideoArchiveAPI.Models
{
    public class UploadModel
    {
        public IFormFile? Video { get; set; }

        public IFormFile? Thumbnail { get; set; }
    }
}