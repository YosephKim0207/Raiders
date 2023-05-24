using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000928 RID: 2344
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of a Float Variable in the PlayMaker Log Window.")]
	public class DebugFloat : BaseLogAction
	{
		// Token: 0x0600337E RID: 13182 RVA: 0x0010D5DC File Offset: 0x0010B7DC
		public override void Reset()
		{
			this.logLevel = LogLevel.Info;
			this.floatVariable = null;
			base.Reset();
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x0010D5F4 File Offset: 0x0010B7F4
		public override void OnEnter()
		{
			string text = "None";
			if (!this.floatVariable.IsNone)
			{
				text = this.floatVariable.Name + ": " + this.floatVariable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, this.logLevel, text, this.sendToUnityLog);
			base.Finish();
		}

		// Token: 0x040024B1 RID: 9393
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		// Token: 0x040024B2 RID: 9394
		[Tooltip("The Float variable to debug.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;
	}
}
