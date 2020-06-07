using System;
using System.Threading.Tasks;

namespace Skuld.Core.Models
{
    public class EventResult<T> : IEquatable<EventResult<T>>
    {
        public bool Successful;
        public string Error;
        public T Data;
        public Exception Exception;

        public override bool Equals(object obj)
        {
            return obj is EventResult<T> && this == (EventResult<T>)obj;
        }

        public bool Equals(EventResult<T> obj)
        {
            return this == obj;
        }

        public static bool operator ==(EventResult<T> x, EventResult<T> y)
        {
            if (x.Successful == y.Successful && x.Error == y.Error && x.Data.Equals(y.Data) && x.Exception == y.Exception)
            { 
                return true;
            }

            return false;
        }

        public static bool operator !=(EventResult<T> x, EventResult<T> y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Successful.GetHashCode();
                hash = hash * 23 + Error?.GetHashCode() ?? 0;
                hash = hash * 23 + Data?.GetHashCode() ?? 0;
                hash = hash * 23 + Exception?.GetHashCode() ?? 0;
                return hash;
            }
        }

        public static EventResult<T> FromSuccess<T>(T data)
            => new EventResult<T>
            {
                Successful = true,
                Data = data
            };

        public static EventResult<T> FromFailure(string reason)
            => new EventResult<T>
            {
                Successful = false,
                Error = reason
            };

        public static EventResult<T> FromFailureException(string reason, Exception ex)
            => new EventResult<T>
            {
                Successful = false,
                Error = reason,
                Exception = ex
            };

        public EventResult<T> WithData(T data)
        {
            Data = data;
            return this;
        }

        public EventResult<T> WithException(Exception exception)
        {
            Exception = exception;
            return this;
        }

        public EventResult<T> WithError(string error)
        {
            Error = error;
            return this;
        }

        public EventResult<T> IsError(Action<EventResult<T>> func)
        {
            if(!Successful)
            {
                func.Invoke(this);
            }

            return this;
        }

        public EventResult<T> IsSuccess(Action<EventResult<T>> func)
        {
            if(Successful)
            {
                func.Invoke(this);
            }

            return this;
        }

        public EventResult<T> IsErrorAsync(Func<EventResult<T>, Task> func)
        {
            if(!Successful)
            {
                Task.Run(() => func.Invoke(this));
            }

            return this;
        }

        public EventResult<T> IsSuccessAsync(Func<EventResult<T>, Task> func)
        {
            if (Successful)
            {
                Task.Run(() => func.Invoke(this));
            }

            return this;
        }
    }
}