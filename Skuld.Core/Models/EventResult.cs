using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Skuld.Core.Models
{
	public class EventResult
	{
		public bool Successful { get; set; }
		public string Error { get; set; }
		[JsonIgnore] public Exception Exception { get; set; }
		public string Warning { get; set; }
		public object Data { get; set; }

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

		public static EventResult FromSuccess(object data = null)
			=> new EventResult()
			{
				Successful = true,
				Data = data
			};

		public static EventResult FromFailure(string reason)
			=> new EventResult()
			{
				Successful = false,
				Error = reason
			};

		public static EventResult FromFailure(object data, string reason)
			=> new EventResult()
			{
				Successful = false,
				Error = reason,
				Data = data
			};

		public static EventResult FromFailureException(string reason, Exception ex)
			=> new EventResult()
			{
				Successful = false,
				Error = reason,
				Exception = ex
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

		public EventResult WithWarning(string warning)
		{
			Warning = warning;
			return this;
		}

		public EventResult WithData(object data)
		{
			Data = data;
			return this;
		}

		public string ToJson()
			=> JsonConvert.SerializeObject(this, new JsonSerializerSettings()
			{
				NullValueHandling = NullValueHandling.Include,
				Formatting = Formatting.Indented
			});
	}
}