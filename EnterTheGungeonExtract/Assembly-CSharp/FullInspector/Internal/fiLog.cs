using System;
using System.Collections.Generic;
using FullSerializer;

namespace FullInspector.Internal
{
	// Token: 0x02000602 RID: 1538
	public static class fiLog
	{
		// Token: 0x060023F6 RID: 9206 RVA: 0x0009D288 File Offset: 0x0009B488
		public static void InsertAndClearMessagesTo(List<string> buffer)
		{
			object typeFromHandle = typeof(fiLog);
			lock (typeFromHandle)
			{
				buffer.AddRange(fiLog._messages);
				fiLog._messages.Clear();
			}
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x0009D2D8 File Offset: 0x0009B4D8
		public static void Blank()
		{
			object typeFromHandle = typeof(fiLog);
			lock (typeFromHandle)
			{
				fiLog._messages.Add(string.Empty);
			}
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x0009D324 File Offset: 0x0009B524
		private static string GetTag(object tag)
		{
			if (tag == null)
			{
				return string.Empty;
			}
			if (tag is string)
			{
				return (string)tag;
			}
			if (tag is Type)
			{
				return "[" + ((Type)tag).CSharpName() + "]: ";
			}
			return "[" + tag.GetType().CSharpName() + "]: ";
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x0009D390 File Offset: 0x0009B590
		public static void Log(object tag, string message)
		{
			object typeFromHandle = typeof(fiLog);
			lock (typeFromHandle)
			{
				fiLog._messages.Add(fiLog.GetTag(tag) + message);
			}
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x0009D3E0 File Offset: 0x0009B5E0
		public static void Log(object tag, string format, params object[] args)
		{
			object typeFromHandle = typeof(fiLog);
			lock (typeFromHandle)
			{
				fiLog._messages.Add(fiLog.GetTag(tag) + string.Format(format, args));
			}
		}

		// Token: 0x04001902 RID: 6402
		private static readonly List<string> _messages = new List<string>();
	}
}
