using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AA1 RID: 2721
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Sets an Integer Variable to a random value between Min/Max.")]
	public class RandomInt : FsmStateAction
	{
		// Token: 0x060039C0 RID: 14784 RVA: 0x00126880 File Offset: 0x00124A80
		public override void Reset()
		{
			this.min = 0;
			this.max = 100;
			this.storeResult = null;
			this.inclusiveMax = false;
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x001268AC File Offset: 0x00124AAC
		public override void OnEnter()
		{
			this.storeResult.Value = ((!this.inclusiveMax) ? UnityEngine.Random.Range(this.min.Value, this.max.Value) : UnityEngine.Random.Range(this.min.Value, this.max.Value + 1));
			base.Finish();
		}

		// Token: 0x04002BE6 RID: 11238
		[RequiredField]
		public FsmInt min;

		// Token: 0x04002BE7 RID: 11239
		[RequiredField]
		public FsmInt max;

		// Token: 0x04002BE8 RID: 11240
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt storeResult;

		// Token: 0x04002BE9 RID: 11241
		[Tooltip("Should the Max value be included in the possible results?")]
		public bool inclusiveMax;
	}
}
