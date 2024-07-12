namespace UserContextDb.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? HashPassword { get; set; }
        public int Country { get; set; }
        public int Provinces { get; set; }
    }
}
