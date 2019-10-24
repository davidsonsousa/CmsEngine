namespace CmsEngine.Core
{
    public struct UploadFilesResult
    {
        public string FileName { get; set; }
        public string ThumbnailName { get; set; }
        public string Path { get; set; }
        public long Length { get; set; }
        public string Size { get; set; }
        public string ContentType { get; set; }
        public bool IsImage { get; set; }
    }
}
