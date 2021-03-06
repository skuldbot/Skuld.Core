﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Skuld.Core.Converters
{
	public class LongToStringConverter : JsonConverter
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JToken jt = JToken.ReadFrom(reader);

			if (jt.ToObject<object>() is long)
			{
				return jt.Value<long>();
			}

			if (jt.ToObject<object>() is ulong)
			{
				return jt.Value<ulong>();
			}

			throw new InvalidOperationException("Couldn't parse to long");
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(long).Equals(objectType) || typeof(ulong).Equals(objectType);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, value.ToString());
		}
	}
}
