using Common;
using DAL;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace BL
{
    public interface IRecordOperations
    {
        Task<Result<IModel<Record>>> Get(int id, int userId);
        Task<Result<IFetchResult<IModel<Record>>>> Get(IFetch fetch, int userId);
        Task<Result> Save(IModel<Record> recordModel, int userId);
        Task<Result> Update(IModel<Record> recordModel, int userId);
        Task<Result> Delete(int id, int userId);
        Task<Result> Delete(IModel<Record> recordModel, int userId);
        Task<Result> UploadImage(int recordId, IFormFile file, int userId);
        Task<Result<Image>> GetImage(int recordId, int userId);
    }
}
