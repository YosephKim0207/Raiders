using System;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008E8 RID: 2280
	[Tooltip("Sort items in an Array.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArraySort : FsmStateAction
	{
		// Token: 0x0600326D RID: 12909 RVA: 0x0010911C File Offset: 0x0010731C
		public override void Reset()
		{
			this.array = null;
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x00109128 File Offset: 0x00107328
		public override void OnEnter()
		{
			List<object> list = new List<object>(this.array.Values);
			list.Sort();
			this.array.Values = list.ToArray();
			base.Finish();
		}

		// Token: 0x04002382 RID: 9090
		[Tooltip("The Array to sort.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmArray array;
	}
}
