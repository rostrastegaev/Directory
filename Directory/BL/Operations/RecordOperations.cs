using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using DAL;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BL
{
    public class RecordOperations : IRecordOperations
    {
        private IDataService _dataService;
        private IRepository<Record> _recordRepo;
        private IFetchProvider<Record> _fetchProvider;
        private ILogger<RecordOperations> _logger;

        public RecordOperations(IDataService dataService, IFetchProvider<Record> fetchProvider,
            ILogger<RecordOperations> logger)
        {
            _logger = logger;
            _dataService = dataService;
            _recordRepo = _dataService.GetRepository<Record>();
            _fetchProvider = fetchProvider;
        }

        public async Task<Result> Delete(int id, int userId)
        {
            try
            {
                Result<Record> resultRecord = Check(await _recordRepo.Get(id), userId);
                var record = resultRecord.Data;
                if (!resultRecord.IsSuccess)
                {
                    return Result.Error(resultRecord.ErrorCodes);
                }

                await _recordRepo.Delete(id);
                if (_dataService.SaveChanges() == 0)
                {
                    return Result.Error(ErrorCodes.OPERATION_ERROR);
                }
                _logger.LogInformation($"Record {record} has been deleted");
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result.Error(ErrorCodes.UNEXPECTED);
        }

        public async Task<Result> Delete(IModel<Record> recordModel, int userId)
        {
            try
            {
                Record record = recordModel.ToEntity();
                Result<Record> resultRecord = Check(await _recordRepo.Get(record.Id), userId);
                record = resultRecord.Data;
                if (!resultRecord.IsSuccess)
                {
                    return Result.Error(resultRecord.ErrorCodes);
                }

                await _recordRepo.Delete(record);
                if (_dataService.SaveChanges() == 0)
                {
                    return Result.Error(ErrorCodes.OPERATION_ERROR);
                }
                _logger.LogInformation($"Record {record} has been deleted.");
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result.Error(ErrorCodes.UNEXPECTED);
        }

        public async Task<Result<IModel<Record>>> Get(int id, int userId)
        {
            try
            {
                Result<Record> resultRecord = Check(await _recordRepo.Get(id), userId);
                if (!resultRecord.IsSuccess)
                {
                    return Result<IModel<Record>>.Error(resultRecord.ErrorCodes);
                }
                return Result<IModel<Record>>.Success(new RecordModel(resultRecord.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result<IModel<Record>>.Error(ErrorCodes.UNEXPECTED);
        }

        public async Task<Result<IFetchResult<IModel<Record>>>> Get(IFetch fetch, int userId)
        {
            try
            {
                var expression = _fetchProvider.Compile(fetch, userId);
                var findedRecords = await _recordRepo.GetMany(expression, fetch);
                List<IModel<Record>> records = new List<IModel<Record>>();
                foreach (var record in findedRecords)
                {
                    records.Add(new RecordModel(record));
                }
                IFetchResult<IModel<Record>> fetchResult = new FetchResult<IModel<Record>>(
                    records, fetch.PageNumber, fetch.PageSize);
                return Result<IFetchResult<IModel<Record>>>.Success(fetchResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result<IFetchResult<IModel<Record>>>.Error(ErrorCodes.UNEXPECTED);
        }

        public async Task<Result<Image>> GetImage(int recordId, int userId)
        {
            try
            {
                Result<Record> resultRecord = Check(await _recordRepo.Get(recordId), userId);
                var record = resultRecord.Data;
                if (!resultRecord.IsSuccess)
                {
                    return Result<Image>.Error(resultRecord.ErrorCodes);
                }
                IRepository<Image> imageRepo = _dataService.GetRepository<Image>();
                Image image = await imageRepo.Get(i => i.RecordId == record.Id);
                return Result<Image>.Success(image);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            return Result<Image>.Error(ErrorCodes.UNEXPECTED);
        }

        public async Task<Result> Save(IModel<Record> recordModel, int userId)
        {
            try
            {
                var record = recordModel.ToEntity();
                if (record.Id != 0)
                {
                    return Result.Error(ErrorCodes.ALREADY_EXISTS);
                }

                record.UserId = userId;
                await _recordRepo.Add(record);
                if (_dataService.SaveChanges() == 0)
                {
                    return Result.Error(ErrorCodes.OPERATION_ERROR);
                }
                _logger.LogInformation($"Record {record} has been saved.");
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result.Error(ErrorCodes.UNEXPECTED);
        }

        public async Task<Result> Update(IModel<Record> recordModel, int userId)
        {
            try
            {
                var record = recordModel.ToEntity();
                if (record.Id == 0)
                {
                    return Result.Error(ErrorCodes.NOT_FOUND);
                }

                Result<Record> resultRecord = Check(await _recordRepo.Get(record.Id), userId);
                var recordEntity = resultRecord.Data;
                if (!resultRecord.IsSuccess)
                {
                    return Result.Error(resultRecord.ErrorCodes);
                }

                string logPart = $"Record {recordEntity}";
                recordEntity.Update(record);
                await _recordRepo.Update(recordEntity);
                if (_dataService.SaveChanges() == 0)
                {
                    return Result.Error(ErrorCodes.OPERATION_ERROR);
                }
                _logger.LogInformation(logPart + $" has been updated to {recordEntity}");
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result.Error();
        }

        public async Task<Result> UploadImage(int recordId, IFormFile file, int userId)
        {
            try
            {
                Result<Record> resultRecord = Check(await _recordRepo.Get(recordId), userId);
                var record = resultRecord.Data;
                if (!resultRecord.IsSuccess)
                {
                    return Result.Error(resultRecord.ErrorCodes);
                }
                IRepository<Image> imageRepo = _dataService.GetRepository<Image>();
                Image image = await imageRepo.Get(i => i.RecordId == record.Id) ??
                    new Image() { RecordId = record.Id };

                using (var stream = file.OpenReadStream())
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    byte[] bytes = reader.ReadBytes((int)stream.Length);
                    image.File = bytes;
                    image.ContentType = file.ContentType;
                }
                await imageRepo.Add(image);
                if (_dataService.SaveChanges() == 0)
                {
                    return Result.Error(ErrorCodes.OPERATION_ERROR);
                }
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result.Error(ErrorCodes.UNEXPECTED);
        }

        private Result<Record> Check(Record record, int userId)
        {
            Result<Record> resultRecord = Result<Record>.Success(record);
            if (record == null)
            {
                resultRecord.AddError(ErrorCodes.NOT_FOUND);
                return resultRecord;
            }
            if (record.UserId != userId)
            {
                resultRecord.AddError(ErrorCodes.AUTH_NOT_ASSIGNED_TO_USER);
            }
            return resultRecord;
        }
    }
}
