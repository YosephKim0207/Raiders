using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200092A RID: 2346
	[Tooltip("Logs the value of a Game Object Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugGameObject : BaseLogAction
	{
		// Token: 0x06003384 RID: 13188 RVA: 0x0010D6B0 File Offset: 0x0010B8B0
		public override void Reset()
		{
			this.logLevel = LogLevel.Info;
			this.gameObject = null;
			base.Reset();
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x0010D6C8 File Offset: 0x0010B8C8
		public override void OnEnter()
		{
			string text = "None";
			if (!this.gameObject.IsNone)
			{
				text = this.gameObject.Name + ": " + this.gameObject;
			}
			ActionHelpers.DebugLog(base.Fsm, this.logLevel, text, this.sendToUnityLog);
			base.Finish();
		}

		// Token: 0x040024B5 RID: 9397
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		// Token: 0x040024B6 RID: 9398
		[Tooltip("The GameObject variable to debug.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObject;
	}
}
