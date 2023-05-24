using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200092D RID: 2349
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Logs the value of an Object Variable in the PlayMaker Log Window.")]
	public class DebugObject : BaseLogAction
	{
		// Token: 0x0600338D RID: 13197 RVA: 0x0010D818 File Offset: 0x0010BA18
		public override void Reset()
		{
			this.logLevel = LogLevel.Info;
			this.fsmObject = null;
			base.Reset();
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x0010D830 File Offset: 0x0010BA30
		public override void OnEnter()
		{
			string text = "None";
			if (!this.fsmObject.IsNone)
			{
				text = this.fsmObject.Name + ": " + this.fsmObject;
			}
			ActionHelpers.DebugLog(base.Fsm, this.logLevel, text, this.sendToUnityLog);
			base.Finish();
		}

		// Token: 0x040024BB RID: 9403
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		// Token: 0x040024BC RID: 9404
		[Tooltip("The Object variable to debug.")]
		[UIHint(UIHint.Variable)]
		public FsmObject fsmObject;
	}
}
