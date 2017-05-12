using System;

namespace DAL
{
    public class Record : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Info { get; set; }
        public string Phone { get; set; }

        public User User { get; set; }

        public void Update(Record record)
        {
            LastName = record.LastName;
            FirstName = record.FirstName;
            Surname = record.Surname;
            DateOfBirth = record.DateOfBirth;
            Info = record.Info;
            Phone = record.Phone;
        }

        public override string ToString()
        {
            return $"Id: {Id}, UserId: {UserId}, FirstName: {FirstName}, LastName: {LastName}, Surname: {Surname}, DateOfBirth: {DateOfBirth}, Info: {Info}, Phone: {Phone}";
        }
    }
}
