using System;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008E7 RID: 2279
	[Tooltip("Shuffle values in an array. Optionally set a start index and range to shuffle only part of the array.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayShuffle : FsmStateAction
	{
		// Token: 0x0600326A RID: 12906 RVA: 0x00109008 File Offset: 0x00107208
		public override void Reset()
		{
			this.array = null;
			this.startIndex = new FsmInt
			{
				UseVariable = true
			};
			this.shufflingRange = new FsmInt
			{
				UseVariable = true
			};
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x00109044 File Offset: 0x00107244
		public override void OnEnter()
		{
			List<object> list = new List<object>(this.array.Values);
			int num = 0;
			int num2 = list.Count - 1;
			if (this.startIndex.Value > 0)
			{
				num = Mathf.Min(this.startIndex.Value, num2);
			}
			if (this.shufflingRange.Value > 0)
			{
				num2 = Mathf.Min(list.Count - 1, num + this.shufflingRange.Value);
			}
			for (int i = num2; i > num; i--)
			{
				int num3 = UnityEngine.Random.Range(num, i + 1);
				object obj = list[i];
				list[i] = list[num3];
				list[num3] = obj;
			}
			this.array.Values = list.ToArray();
			base.Finish();
		}

		// Token: 0x0400237F RID: 9087
		[Tooltip("The Array to shuffle.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmArray array;

		// Token: 0x04002380 RID: 9088
		[Tooltip("Optional start Index for the shuffling. Leave it to none or 0 for no effect")]
		public FsmInt startIndex;

		// Token: 0x04002381 RID: 9089
		[Tooltip("Optional range for the shuffling, starting at the start index if greater than 0. Leave it to none or 0 for no effect, it will shuffle the whole array")]
		public FsmInt shufflingRange;
	}
}
