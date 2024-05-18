namespace HMS.Models
{
    public class UserLoginModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Password { get; set; }
        public string HasencryptKey { get; set; }
    }
}
