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
                Result<Record> resultRecord = Check(await _recordRepo.Get(id), userId, out Record record);
                if (!resultRecord.IsSuccess)
                {
                    return Result.Error(resultRecord.ErrorCodes);
                }
                var result = await _recordRepo.Delete(id);
                _dataService.SaveChanges();
                if (result.IsSuccess)
                {
                    _logger.LogInformation($"Record {record} has been deleted");
                }
                return result;
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
                Result<Record> resultRecord = Check(await _recordRepo.Get(record.Id), userId, out record);
                if (!resultRecord.IsSuccess)
                {
                    return Result.Error(resultRecord.ErrorCodes);
                }
                var result = await _recordRepo.Delete(record);
                _dataService.SaveChanges();
                if (result.IsSuccess)
                {
                    _logger.LogInformation($"Record {record} has been deleted.");
                }
                return result;
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
                Result<Record> resultRecord = Check(await _recordRepo.Get(id), userId, out Record record);
                if (!resultRecord.IsSuccess)
                {
                    return Result<IModel<Record>>.Error(resultRecord.ErrorCodes);
                }
                return Result<IModel<Record>>.Success(new RecordModel(record));
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
                var resultRecords = await _recordRepo.GetMany(expression, fetch);
                if (!resultRecords.IsSuccess)
                {
                    return Result<IFetchResult<IModel<Record>>>.Error(resultRecords.ErrorCodes);
                }
                List<IModel<Record>> records = new List<IModel<Record>>();
                foreach (var record in resultRecords.Data.Items)
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
                Result<Record> resultRecord = Check(await _recordRepo.Get(recordId), userId, out Record record);
                if (!resultRecord.IsSuccess)
                {
                    return Result<Image>.Error(resultRecord.ErrorCodes);
                }
                IRepository<Image> imageRepo = _dataService.GetRepository<Image>();
                Result<Image> resultImage = await imageRepo.Get(i => i.RecordId == record.Id);
                if (resultImage.Data == null)
                {
                    resultImage.AddError(ErrorCodes.NOT_FOUND);
                }
                if (!resultImage.IsSuccess)
                {
                    return Result<Image>.Error(resultImage.ErrorCodes);
                }
                return Result<Image>.Success(resultImage.Data);
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
                var result = await _recordRepo.Add(record);
                _dataService.SaveChanges();
                if (result.IsSuccess)
                {
                    _logger.LogInformation($"Record {record} has been saved.");
                }
                return result;
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
                Result<Record> resultRecord = Check(await _recordRepo.Get(record.Id), userId, out Record recordEntity);
                if (!resultRecord.IsSuccess)
                {
                    return Result.Error(resultRecord.ErrorCodes);
                }

                string logPart = $"Record {recordEntity}";
                recordEntity.Update(record);
                var result = await _recordRepo.Update(recordEntity);
                _dataService.SaveChanges();
                if (result.IsSuccess)
                {
                    _logger.LogInformation(logPart + $" has been updated to {recordEntity}");
                }
                return result;
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
                Result<Record> resultRecord = Check(await _recordRepo.Get(recordId), userId, out Record record);
                if (!resultRecord.IsSuccess)
                {
                    return Result.Error(resultRecord.ErrorCodes);
                }
                IRepository<Image> imageRepo = _dataService.GetRepository<Image>();
                Result<Image> resultImage = await imageRepo.Get(i => i.RecordId == record.Id);
                if (!resultImage.IsSuccess)
                {
                    return Result.Error(resultImage.ErrorCodes);
                }
                Image image = resultImage.Data ?? new Image() { RecordId = record.Id };

                using (var stream = file.OpenReadStream())
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    byte[] bytes = reader.ReadBytes((int)stream.Length);
                    image.File = bytes;
                    image.ContentType = file.ContentType;
                }
                _dataService.SaveChanges();
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Result.Error(ErrorCodes.UNEXPECTED);
        }

        private Result<Record> Check(Result<Record> resultRecord, int userId, out Record recordEntity)
        {
            recordEntity = resultRecord.Data;
            if (recordEntity == null)
            {
                resultRecord.AddError(ErrorCodes.NOT_FOUND);
                return resultRecord;
            }
            if (recordEntity.UserId != userId)
            {
                resultRecord.AddError(ErrorCodes.AUTH_NOT_ASSIGNED_TO_USER);
            }
            return resultRecord;
        }
    }
}
