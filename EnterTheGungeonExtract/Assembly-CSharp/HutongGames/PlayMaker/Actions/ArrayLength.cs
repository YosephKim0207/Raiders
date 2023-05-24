using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008E3 RID: 2275
	[Tooltip("Gets the number of items in an Array.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayLength : FsmStateAction
	{
		// Token: 0x0600325C RID: 12892 RVA: 0x00108DFC File Offset: 0x00106FFC
		public override void Reset()
		{
			this.array = null;
			this.length = null;
			this.everyFrame = false;
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x00108E14 File Offset: 0x00107014
		public override void OnEnter()
		{
			this.length.Value = this.array.Length;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x00108E40 File Offset: 0x00107040
		public override void OnUpdate()
		{
			this.length.Value = this.array.Length;
		}

		// Token: 0x04002373 RID: 9075
		[Tooltip("The Array Variable.")]
		[UIHint(UIHint.Variable)]
		public FsmArray array;

		// Token: 0x04002374 RID: 9076
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the length in an Int Variable.")]
		public FsmInt length;

		// Token: 0x04002375 RID: 9077
		[Tooltip("Repeat every frame. Useful if the array is changing and you're waiting for a particular length.")]
		public bool everyFrame;
	}
}
