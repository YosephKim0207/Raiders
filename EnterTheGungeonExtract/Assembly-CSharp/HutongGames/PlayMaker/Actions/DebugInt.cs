using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200092B RID: 2347
	[Tooltip("Logs the value of an Integer Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugInt : BaseLogAction
	{
		// Token: 0x06003387 RID: 13191 RVA: 0x0010D730 File Offset: 0x0010B930
		public override void Reset()
		{
			this.logLevel = LogLevel.Info;
			this.intVariable = null;
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x0010D740 File Offset: 0x0010B940
		public override void OnEnter()
		{
			string text = "None";
			if (!this.intVariable.IsNone)
			{
				text = this.intVariable.Name + ": " + this.intVariable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, this.logLevel, text, this.sendToUnityLog);
			base.Finish();
		}

		// Token: 0x040024B7 RID: 9399
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		// Token: 0x040024B8 RID: 9400
		[Tooltip("The Int variable to debug.")]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;
	}
}
