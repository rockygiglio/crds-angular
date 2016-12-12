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

        public Result<T> Map (Func<T, T> transform)
        {
            if (this.Status)
            {
                try
                {
                    var res = transform(this.Value);
                    return new Ok<T>(res);
                }
                catch (Exception e)
                {
                    return new Err<T>(e.Message);
                }

            }
            else
            {
                return new Err<T>(this.ErrorMessage);
            }
        }

        public bool Status { get; }

        public T Value { get; }

        public string ErrorMessage { get; }
    }

    public class Ok<T> : Result<T>
    {
        public Ok(T val) : base(true, val)
        {
        }
    }

    public class Err<T> : Result<T>
    {
        public Err(string errorMessage) : base(false, errorMessage)
        {
        }

        public Err(Exception e) : base(false, e.Message)
        {            
        }
    }
    
}