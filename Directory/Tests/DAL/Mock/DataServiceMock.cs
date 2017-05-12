using DAL;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Tests.DAL
{
    public class DataServiceMock : IDataService
    {
        private List<User> _users;
        private List<Record> _records;
        private List<Image> _images;

        public DataServiceMock()
        {
            _users = new List<User>()
            {
                new User() { Id = 1, Email = "test@test.com", Password = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(" 1")) },
                new User() {Id = 2, Email = "test1@test.com", Password = new byte[] { } }
            };
            _records = new List<Record>()
            {
                new Record() { Id = 1, UserId = 1, FirstName = "Andrew", LastName = "Smith" },
                new Record() { Id = 2, UserId = 1, FirstName = "Adrian", LastName = "Smith" },
                new Record() { Id = 3, UserId = 1, FirstName = "Jonathan", LastName = "Davis" },
                new Record() { Id = 4, UserId = 1, FirstName = "Fred", LastName = "Durst" },
                new Record() { Id = 5, UserId = 1, FirstName = "Lars", LastName = "Ulrich" }
            };
            _images = new List<Image>()
            {
                new Image() { Id = 1, RecordId = 1, File = new byte[] { 1, 2 } }
            };
        }

        public int SaveChanges()
        {
            return 0;
        }

        public IRepository<T> GetRepository<T>() where T : class, IEntity, new()
        {
            Type type = typeof(T);
            if (type == typeof(User))
            {
                return new Repository<User>(_users) as Repository<T>;
            }
            else if (type == typeof(Record))
            {
                return new Repository<Record>(_records) as Repository<T>;
            }
            else if (type == typeof(Image))
            {
                return new Repository<Image>(_images) as Repository<T>;
            }
            return null;
        }
    }
}
