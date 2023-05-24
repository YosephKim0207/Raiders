using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200090E RID: 2318
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Adds a text area to the action list. NOTE: Doesn't do anything, just for notes...")]
	public class Comment : FsmStateAction
	{
		// Token: 0x0600330F RID: 13071 RVA: 0x0010C0F8 File Offset: 0x0010A2F8
		public override void Reset()
		{
			this.comment = string.Empty;
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x0010C108 File Offset: 0x0010A308
		public override void OnEnter()
		{
			base.Finish();
		}

		// Token: 0x0400243F RID: 9279
		[UIHint(UIHint.Comment)]
		public string comment;
	}
}
