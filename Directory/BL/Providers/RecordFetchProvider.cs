using System;
using System.Linq.Expressions;
using Common;
using DAL;

namespace BL
{
    public class RecordFetchProvider : IFetchProvider<Record>
    {
        public Expression<Func<Record, bool>> Compile(IFetch fetch, int userId)
        {
            DateTime.TryParse(fetch.Value, out DateTime dateValue);

            return (record) => (record.FirstName.Contains(fetch.Value) ||
                record.LastName.Contains(fetch.Value) ||
                record.Surname.Contains(fetch.Value) ||
                record.DateOfBirth.Value.Equals(dateValue) ||
                record.Info.Contains(fetch.Value) ||
                record.Phone.Contains(fetch.Value)) &&
                record.UserId == userId;
        }
    }
}
