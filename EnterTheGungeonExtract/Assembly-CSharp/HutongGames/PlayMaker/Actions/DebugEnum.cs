using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000927 RID: 2343
	[Tooltip("Logs the value of an Enum Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugEnum : BaseLogAction
	{
		// Token: 0x0600337B RID: 13179 RVA: 0x0010D558 File Offset: 0x0010B758
		public override void Reset()
		{
			this.logLevel = LogLevel.Info;
			this.enumVariable = null;
			base.Reset();
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x0010D570 File Offset: 0x0010B770
		public override void OnEnter()
		{
			string text = "None";
			if (!this.enumVariable.IsNone)
			{
				text = this.enumVariable.Name + ": " + this.enumVariable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, this.logLevel, text, this.sendToUnityLog);
			base.Finish();
		}

		// Token: 0x040024AF RID: 9391
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		// Token: 0x040024B0 RID: 9392
		[Tooltip("The Enum Variable to debug.")]
		[UIHint(UIHint.Variable)]
		public FsmEnum enumVariable;
	}
}
