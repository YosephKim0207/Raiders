using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000929 RID: 2345
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Print the value of any FSM Variable in the PlayMaker Log Window.")]
	public class DebugFsmVariable : BaseLogAction
	{
		// Token: 0x06003381 RID: 13185 RVA: 0x0010D664 File Offset: 0x0010B864
		public override void Reset()
		{
			this.logLevel = LogLevel.Info;
			this.variable = null;
			base.Reset();
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x0010D67C File Offset: 0x0010B87C
		public override void OnEnter()
		{
			ActionHelpers.DebugLog(base.Fsm, this.logLevel, this.variable.DebugString(), this.sendToUnityLog);
			base.Finish();
		}

		// Token: 0x040024B3 RID: 9395
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		// Token: 0x040024B4 RID: 9396
		[UIHint(UIHint.Variable)]
		[HideTypeFilter]
		[Tooltip("The variable to debug.")]
		public FsmVar variable;
	}
}
