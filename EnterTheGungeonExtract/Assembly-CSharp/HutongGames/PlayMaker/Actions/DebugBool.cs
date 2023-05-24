using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000924 RID: 2340
	[Tooltip("Logs the value of a Bool Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugBool : BaseLogAction
	{
		// Token: 0x06003375 RID: 13173 RVA: 0x0010D398 File Offset: 0x0010B598
		public override void Reset()
		{
			this.logLevel = LogLevel.Info;
			this.boolVariable = null;
			base.Reset();
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x0010D3B0 File Offset: 0x0010B5B0
		public override void OnEnter()
		{
			string text = "None";
			if (!this.boolVariable.IsNone)
			{
				text = this.boolVariable.Name + ": " + this.boolVariable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, this.logLevel, text, this.sendToUnityLog);
			base.Finish();
		}

		// Token: 0x040024A3 RID: 9379
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		// Token: 0x040024A4 RID: 9380
		[Tooltip("The Bool variable to debug.")]
		[UIHint(UIHint.Variable)]
		public FsmBool boolVariable;
	}
}
