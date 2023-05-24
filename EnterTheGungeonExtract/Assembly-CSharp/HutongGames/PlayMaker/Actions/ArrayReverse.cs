using System;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008E5 RID: 2277
	[Tooltip("Reverse the order of items in an Array.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayReverse : FsmStateAction
	{
		// Token: 0x06003262 RID: 12898 RVA: 0x00108ED8 File Offset: 0x001070D8
		public override void Reset()
		{
			this.array = null;
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x00108EE4 File Offset: 0x001070E4
		public override void OnEnter()
		{
			List<object> list = new List<object>(this.array.Values);
			list.Reverse();
			this.array.Values = list.ToArray();
			base.Finish();
		}

		// Token: 0x04002379 RID: 9081
		[Tooltip("The Array to reverse.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmArray array;
	}
}
