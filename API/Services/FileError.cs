namespace API.Services
{
    public class FileError : Exception
    {
        public FileError (string message) : base (message) { }
    }
}
