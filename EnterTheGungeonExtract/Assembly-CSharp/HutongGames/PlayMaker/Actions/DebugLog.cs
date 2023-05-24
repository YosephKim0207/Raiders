using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200092C RID: 2348
	[Tooltip("Sends a log message to the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugLog : BaseLogAction
	{
		// Token: 0x0600338A RID: 13194 RVA: 0x0010D7B0 File Offset: 0x0010B9B0
		public override void Reset()
		{
			this.logLevel = LogLevel.Info;
			this.text = string.Empty;
			base.Reset();
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x0010D7D0 File Offset: 0x0010B9D0
		public override void OnEnter()
		{
			if (!string.IsNullOrEmpty(this.text.Value))
			{
				ActionHelpers.DebugLog(base.Fsm, this.logLevel, this.text.Value, this.sendToUnityLog);
			}
			base.Finish();
		}

		// Token: 0x040024B9 RID: 9401
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		// Token: 0x040024BA RID: 9402
		[Tooltip("Text to send to the log.")]
		public FsmString text;
	}
}
