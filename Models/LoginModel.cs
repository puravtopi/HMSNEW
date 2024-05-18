namespace HMS.Models
{
    public class LoginModel : BaseResponseModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
