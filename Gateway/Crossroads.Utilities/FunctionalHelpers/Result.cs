using System;

namespace Crossroads.Utilities.FunctionalHelpers
{
    public class Result<T>
    {
        public Result (bool status, T val)
        {
            this.Status = status;
            Value = val;
            ErrorMessage = string.Empty;
        }

        public Result(bool status, string errorMsg)
        {
            this.Status = status;
            Value = default(T);
            ErrorMessage = errorMsg;
        }

        public bool Status { get; }

        public T Value { get; }

        public string ErrorMessage { get; }
    }
    
}