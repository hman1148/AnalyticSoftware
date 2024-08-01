namespace AnalyticSoftware.Models
{
    public class RegisterRequest
    {
        public string SuperUserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string S3Bucket { get; set; }

    }
}
