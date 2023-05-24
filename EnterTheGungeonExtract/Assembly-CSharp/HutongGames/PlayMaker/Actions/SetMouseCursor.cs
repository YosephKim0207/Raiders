using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B07 RID: 2823
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("Controls the appearance of Mouse Cursor.")]
	public class SetMouseCursor : FsmStateAction
	{
		// Token: 0x06003B8F RID: 15247 RVA: 0x0012C7B0 File Offset: 0x0012A9B0
		public override void Reset()
		{
			this.cursorTexture = null;
			this.hideCursor = false;
			this.lockCursor = false;
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x0012C7D4 File Offset: 0x0012A9D4
		public override void OnEnter()
		{
			PlayMakerGUI.LockCursor = this.lockCursor.Value;
			PlayMakerGUI.HideCursor = this.hideCursor.Value;
			PlayMakerGUI.MouseCursor = this.cursorTexture.Value;
			base.Finish();
		}

		// Token: 0x04002DB2 RID: 11698
		public FsmTexture cursorTexture;

		// Token: 0x04002DB3 RID: 11699
		public FsmBool hideCursor;

		// Token: 0x04002DB4 RID: 11700
		public FsmBool lockCursor;
	}
}
