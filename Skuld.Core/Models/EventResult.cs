using System;
using System.Threading.Tasks;

namespace Skuld.Core.Models
{
	public class EventResult : IEquatable<EventResult>
	{
		public bool Successful;
		public string Error;
		public Exception Exception;

		public override bool Equals(object obj)
		{
			return obj is EventResult result && this == result;
		}

		public bool Equals(EventResult obj)
		{
			return this == obj;
		}

		public static bool operator ==(EventResult x, EventResult y)
		{
			if (x.Successful == y.Successful && x.Error == y.Error && x.Exception == y.Exception)
			{
				return true;
			}

			return false;
		}

		public static bool operator !=(EventResult x, EventResult y)
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
				hash = hash * 23 + Exception?.GetHashCode() ?? 0;
				return hash;
			}
		}

		public static EventResult FromFailure(string reason)
			=> new()
			{
				Successful = false,
				Error = reason
			};

		public static EventResult FromFailureException(string reason, Exception ex)
			=> new()
			{
				Successful = false,
				Error = reason,
				Exception = ex
			};

		public static EventResult FromSuccess()
			=> new()
			{
				Successful = true
			};

		public EventResult IsError(Action<EventResult> func)
		{
			if (!Successful)
			{
				func.Invoke(this);
			}

			return this;
		}

		public EventResult IsSuccess(Action<EventResult> func)
		{
			if (Successful)
			{
				func.Invoke(this);
			}

			return this;
		}

		public EventResult IsErrorAsync(Func<EventResult, Task> func)
		{
			if (!Successful)
			{
				Task.Run(() => func.Invoke(this));
			}

			return this;
		}

		public EventResult IsSuccessAsync(Func<EventResult, Task> func)
		{
			if (Successful)
			{
				Task.Run(() => func.Invoke(this));
			}

			return this;
		}

		public EventResult WithException(Exception exception)
		{
			Exception = exception;
			return this;
		}

		public EventResult WithError(string error)
		{
			Error = error;
			return this;
		}
	}

	public class EventResult<T> : EventResult
	{
		public T Data;

		public override bool Equals(object obj)
		{
			return obj is EventResult<T> result && this == result;
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
			=> new()
			{
				Successful = true,
				Data = data
			};

		public static EventResult<T> FromFailure<T>(T data, string reason)
			=> new()
			{
				Data = data,
				Successful = false,
				Error = reason
			};

		public static EventResult<T> FromFailureException<T>(T data, string reason, Exception ex)
			=> new()
			{
				Data = data,
				Successful = false,
				Error = reason,
				Exception = ex
			};

		public EventResult<T> WithData(T data)
		{
			Data = data;
			return this;
		}

		public EventResult<T> IsError(Action<EventResult<T>> func)
		{
			if (!Successful)
			{
				func.Invoke(this);
			}

			return this;
		}

		public EventResult<T> IsSuccess(Action<EventResult<T>> func)
		{
			if (Successful)
			{
				func.Invoke(this);
			}

			return this;
		}

		public EventResult<T> IsErrorAsync(Func<EventResult<T>, Task> func)
		{
			if (!Successful)
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