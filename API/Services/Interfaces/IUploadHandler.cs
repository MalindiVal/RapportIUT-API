namespace API.Services.Interfaces
{
    public interface IUploadHandler
    {
        public string Upload(IFormFile file);

        public void DeleteFile(string nomFichier);
    }
}
