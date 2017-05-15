namespace DAL
{
    public class RefreshToken : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }

        public User User { get; set; }
    }
}
