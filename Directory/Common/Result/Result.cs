using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class Result
    {
        private List<int> _errorCodes;

        public bool IsSuccess { get; private set; }
        public IEnumerable<int> ErrorCodes => _errorCodes;

        public Result(bool isSuccess, IEnumerable<int> errorCodes)
        {
            IsSuccess = isSuccess;
            _errorCodes = errorCodes == null ? new List<int>() : new List<int>(errorCodes);
        }

        public Result(bool isSuccess, params int[] errorCodes)
        {
            IsSuccess = isSuccess;
            _errorCodes = errorCodes == null ? new List<int>() : new List<int>(errorCodes);
        }

        public void AddError(int errorCode)
        {
            IsSuccess = false;
            _errorCodes.Add(errorCode);
        }

        public void AddErrors(IEnumerable<int> errorCodes)
        {
            bool tempRes = errorCodes == null || !errorCodes.Any();
            IsSuccess &= tempRes;
            if (!tempRes)
            {
                _errorCodes.AddRange(errorCodes);
            }
        }

        public static Result Success() => new Result(true);
        public static Result Error(params int[] errorCodes) => new Result(false, errorCodes);
        public static Result Error(IEnumerable<int> errorCodes) => new Result(false, errorCodes);
    }

    public class Result<T>
    {
        private Result _result;

        public T Data { get; private set; }
        public bool IsSuccess => _result.IsSuccess;
        public IEnumerable<int> ErrorCodes => _result.ErrorCodes;

        public Result(bool isSuccess, T data, params int[] errorCodes)
        {
            _result = new Result(isSuccess, errorCodes);
            Data = data;
        }

        public Result(bool isSuccess, T data, IEnumerable<int> errorCodes)
        {
            _result = new Result(isSuccess, errorCodes);
            Data = data;
        }

        public void AddError(int errorCode)
        {
            _result.AddError(errorCode);
            Data = default(T);
        }
        public void AddErrors(IEnumerable<int> errorCodes)
        {
            _result.AddErrors(errorCodes);
            if (errorCodes != null && errorCodes.Any())
            {
                Data = default(T);
            }
        }
        public static Result<T> Success(T data) => new Result<T>(true, data);
        public static Result<T> Error(params int[] errorCodes) => new Result<T>(false, default(T), errorCodes);
        public static Result<T> Error(IEnumerable<int> errorCodes) => new Result<T>(false, default(T), errorCodes);
    }
}
