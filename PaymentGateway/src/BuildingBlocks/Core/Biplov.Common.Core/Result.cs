using System;

namespace Biplov.Common.Core
{
    /// <summary>
    /// Lightweight Result class that represent whether an operation was successful or not.
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; }

        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException("Success result cannot contain errors.");

            if (!isSuccess && string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException("Fail result must have error specified.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Fail(string message) => 
            new Result(false, message);

        public static Result<T> Fail<T>(string message) => 
            new Result<T>(default, false, message);
        

        public static Result Ok() =>
            new Result(true, string.Empty);

        public static Result<T> Ok<T>(T value) => 
            new Result<T>(value, true, string.Empty);
        
    }

    public class Result<T>: Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("Fail results cannot contain a value.");

                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            _value = value;
        }
    }
}
