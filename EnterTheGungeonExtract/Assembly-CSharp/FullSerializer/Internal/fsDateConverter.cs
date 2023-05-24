using System;
using System.Globalization;

namespace FullSerializer.Internal
{
	// Token: 0x02000589 RID: 1417
	public class fsDateConverter : fsConverter
	{
		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06002193 RID: 8595 RVA: 0x00093C68 File Offset: 0x00091E68
		private string DateTimeFormatString
		{
			get
			{
				return fsConfig.CustomDateTimeFormatString ?? "o";
			}
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x00093C7C File Offset: 0x00091E7C
		public override bool CanProcess(Type type)
		{
			return type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(TimeSpan);
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x00093CB0 File Offset: 0x00091EB0
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			if (instance is DateTime)
			{
				serialized = new fsData(((DateTime)instance).ToString(this.DateTimeFormatString));
				return fsResult.Success;
			}
			if (instance is DateTimeOffset)
			{
				serialized = new fsData(((DateTimeOffset)instance).ToString("o"));
				return fsResult.Success;
			}
			if (instance is TimeSpan)
			{
				serialized = new fsData(((TimeSpan)instance).ToString());
				return fsResult.Success;
			}
			throw new InvalidOperationException("FullSerializer Internal Error -- Unexpected serialization type");
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x00093D4C File Offset: 0x00091F4C
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			if (!data.IsString)
			{
				return fsResult.Fail("Date deserialization requires a string, not " + data.Type);
			}
			if (storageType == typeof(DateTime))
			{
				DateTime dateTime;
				if (DateTime.TryParse(data.AsString, null, DateTimeStyles.RoundtripKind, out dateTime))
				{
					instance = dateTime;
					return fsResult.Success;
				}
				return fsResult.Fail("Unable to parse " + data.AsString + " into a DateTime");
			}
			else if (storageType == typeof(DateTimeOffset))
			{
				DateTimeOffset dateTimeOffset;
				if (DateTimeOffset.TryParse(data.AsString, null, DateTimeStyles.RoundtripKind, out dateTimeOffset))
				{
					instance = dateTimeOffset;
					return fsResult.Success;
				}
				return fsResult.Fail("Unable to parse " + data.AsString + " into a DateTimeOffset");
			}
			else
			{
				if (storageType != typeof(TimeSpan))
				{
					throw new InvalidOperationException("FullSerializer Internal Error -- Unexpected deserialization type");
				}
				TimeSpan timeSpan;
				if (TimeSpan.TryParse(data.AsString, out timeSpan))
				{
					instance = timeSpan;
					return fsResult.Success;
				}
				return fsResult.Fail("Unable to parse " + data.AsString + " into a TimeSpan");
			}
		}

		// Token: 0x04001821 RID: 6177
		private const string DefaultDateTimeFormatString = "o";

		// Token: 0x04001822 RID: 6178
		private const string DateTimeOffsetFormatString = "o";
	}
}
