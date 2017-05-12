using System;
using DAL;

namespace BL
{
    public class RecordModel : IModel<Record>
    {
        private Record _record;

        public int Id { get => _record.Id; set { _record.Id = value; } }
        public string FirstName { get => _record.FirstName; set { _record.FirstName = value; } }
        public string LastName { get => _record.LastName; set { _record.LastName = value; } }
        public string Surname { get => _record.Surname; set { _record.Surname = value; } }
        public DateTime? DateOfBirth { get => _record.DateOfBirth; set { _record.DateOfBirth = value; } }
        public string Info { get => _record.Info; set { _record.Info = value; } }
        public string Phone { get => _record.Phone; set { _record.Phone = value; } }

        public RecordModel()
        {
            _record = new Record();
        }

        public RecordModel(Record record)
        {
            _record = record;
        }

        public Record ToEntity() => _record;
    }
}
