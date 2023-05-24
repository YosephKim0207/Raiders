using System;
using System.Linq;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008DC RID: 2268
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if 2 Array Variables have the same values.")]
	public class ArrayCompare : FsmStateAction
	{
		// Token: 0x06003237 RID: 12855 RVA: 0x0010868C File Offset: 0x0010688C
		public override void Reset()
		{
			this.array1 = null;
			this.array2 = null;
			this.SequenceEqual = null;
			this.SequenceNotEqual = null;
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x001086AC File Offset: 0x001068AC
		public override void OnEnter()
		{
			this.DoSequenceEqual();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x001086C8 File Offset: 0x001068C8
		private void DoSequenceEqual()
		{
			if (this.array1.Values == null || this.array2.Values == null)
			{
				return;
			}
			this.storeResult.Value = this.array1.Values.SequenceEqual(this.array2.Values);
			base.Fsm.Event((!this.storeResult.Value) ? this.SequenceNotEqual : this.SequenceEqual);
		}

		// Token: 0x0400234F RID: 9039
		[Tooltip("The first Array Variable to test.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmArray array1;

		// Token: 0x04002350 RID: 9040
		[Tooltip("The second Array Variable to test.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmArray array2;

		// Token: 0x04002351 RID: 9041
		[Tooltip("Event to send if the 2 arrays have the same values.")]
		public FsmEvent SequenceEqual;

		// Token: 0x04002352 RID: 9042
		[Tooltip("Event to send if the 2 arrays have different values.")]
		public FsmEvent SequenceNotEqual;

		// Token: 0x04002353 RID: 9043
		[Tooltip("Store the result in a Bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x04002354 RID: 9044
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
