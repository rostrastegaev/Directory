namespace DAL
{
    public class Image : IEntity
    {
        public int Id { get; set; }
        public int RecordId { get; set; }
        public byte[] File { get; set; }
        public string ContentType { get; set; }

        public void Update(Image image)
        {
            File = image.File;
            ContentType = image.ContentType;
        }
    }
}
