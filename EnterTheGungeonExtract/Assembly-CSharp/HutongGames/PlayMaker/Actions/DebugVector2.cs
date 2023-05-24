using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B48 RID: 2888
	[Tooltip("Logs the value of a Vector2 Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugVector2 : FsmStateAction
	{
		// Token: 0x06003CA4 RID: 15524 RVA: 0x00130980 File Offset: 0x0012EB80
		public override void Reset()
		{
			this.logLevel = LogLevel.Info;
			this.vector2Variable = null;
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x00130990 File Offset: 0x0012EB90
		public override void OnEnter()
		{
			string text = "None";
			if (!this.vector2Variable.IsNone)
			{
				text = this.vector2Variable.Name + ": " + this.vector2Variable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, this.logLevel, text, false);
			base.Finish();
		}

		// Token: 0x04002EF0 RID: 12016
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		// Token: 0x04002EF1 RID: 12017
		[Tooltip("Prints the value of a Vector2 variable in the PlayMaker log window.")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 vector2Variable;
	}
}
