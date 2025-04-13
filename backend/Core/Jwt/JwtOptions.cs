namespace backend.Core.JwtOp
{
    public class JwtOptions
    {
        public string SekretKey { get; set; }
        public int ExpiresHours { get; set; }
    }
}
