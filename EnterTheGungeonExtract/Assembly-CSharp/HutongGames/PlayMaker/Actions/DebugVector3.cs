using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200092E RID: 2350
	[Tooltip("Logs the value of a Vector3 Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugVector3 : BaseLogAction
	{
		// Token: 0x06003390 RID: 13200 RVA: 0x0010D898 File Offset: 0x0010BA98
		public override void Reset()
		{
			this.logLevel = LogLevel.Info;
			this.vector3Variable = null;
			base.Reset();
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x0010D8B0 File Offset: 0x0010BAB0
		public override void OnEnter()
		{
			string text = "None";
			if (!this.vector3Variable.IsNone)
			{
				text = this.vector3Variable.Name + ": " + this.vector3Variable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, this.logLevel, text, this.sendToUnityLog);
			base.Finish();
		}

		// Token: 0x040024BD RID: 9405
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		// Token: 0x040024BE RID: 9406
		[Tooltip("The Vector3 variable to debug.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;
	}
}
