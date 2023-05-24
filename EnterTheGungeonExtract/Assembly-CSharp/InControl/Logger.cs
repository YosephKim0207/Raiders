using System;
using System.Diagnostics;

namespace InControl
{
	// Token: 0x020006AE RID: 1710
	public class Logger
	{
		// Token: 0x14000076 RID: 118
		// (add) Token: 0x060027B7 RID: 10167 RVA: 0x000A94A4 File Offset: 0x000A76A4
		// (remove) Token: 0x060027B8 RID: 10168 RVA: 0x000A94D8 File Offset: 0x000A76D8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<LogMessage> OnLogMessage;

		// Token: 0x060027B9 RID: 10169 RVA: 0x000A950C File Offset: 0x000A770C
		public static void LogInfo(string text)
		{
			if (Logger.OnLogMessage != null)
			{
				LogMessage logMessage = new LogMessage
				{
					text = text,
					type = LogMessageType.Info
				};
				Logger.OnLogMessage(logMessage);
			}
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x000A9548 File Offset: 0x000A7748
		public static void LogWarning(string text)
		{
			if (Logger.OnLogMessage != null)
			{
				LogMessage logMessage = new LogMessage
				{
					text = text,
					type = LogMessageType.Warning
				};
				Logger.OnLogMessage(logMessage);
			}
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x000A9584 File Offset: 0x000A7784
		public static void LogError(string text)
		{
			if (Logger.OnLogMessage != null)
			{
				LogMessage logMessage = new LogMessage
				{
					text = text,
					type = LogMessageType.Error
				};
				Logger.OnLogMessage(logMessage);
			}
		}
	}
}
