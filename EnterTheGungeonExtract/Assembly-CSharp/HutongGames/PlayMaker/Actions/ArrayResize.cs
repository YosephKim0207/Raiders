using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008E4 RID: 2276
	[ActionCategory(ActionCategory.Array)]
	[Tooltip("Resize an array.")]
	public class ArrayResize : FsmStateAction
	{
		// Token: 0x06003260 RID: 12896 RVA: 0x00108E60 File Offset: 0x00107060
		public override void OnEnter()
		{
			if (this.newSize.Value >= 0)
			{
				this.array.Resize(this.newSize.Value);
			}
			else
			{
				base.LogError("Size out of range: " + this.newSize.Value);
				base.Fsm.Event(this.sizeOutOfRangeEvent);
			}
			base.Finish();
		}

		// Token: 0x04002376 RID: 9078
		[Tooltip("The Array Variable to resize")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmArray array;

		// Token: 0x04002377 RID: 9079
		[Tooltip("The new size of the array.")]
		public FsmInt newSize;

		// Token: 0x04002378 RID: 9080
		[Tooltip("The event to trigger if the new size is out of range")]
		public FsmEvent sizeOutOfRangeEvent;
	}
}
