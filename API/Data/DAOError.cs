namespace API.Data
{
    public class DAOError : Exception
    {
        public DAOError(string msg) : base(msg) { }
    }
}
