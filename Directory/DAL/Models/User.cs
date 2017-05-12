using System.Collections.Generic;

namespace DAL
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }

        public ICollection<Record> Records { get; set; }

        public void Update(User user)
        {
            Email = user.Email;
            Password = user.Password;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Email: {Email}";
        }
    }
}
