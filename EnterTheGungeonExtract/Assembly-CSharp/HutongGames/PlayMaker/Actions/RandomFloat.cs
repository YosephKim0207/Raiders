using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AA0 RID: 2720
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Sets a Float Variable to a random value between Min/Max.")]
	public class RandomFloat : FsmStateAction
	{
		// Token: 0x060039BD RID: 14781 RVA: 0x0012681C File Offset: 0x00124A1C
		public override void Reset()
		{
			this.min = 0f;
			this.max = 1f;
			this.storeResult = null;
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x00126848 File Offset: 0x00124A48
		public override void OnEnter()
		{
			this.storeResult.Value = UnityEngine.Random.Range(this.min.Value, this.max.Value);
			base.Finish();
		}

		// Token: 0x04002BE3 RID: 11235
		[RequiredField]
		public FsmFloat min;

		// Token: 0x04002BE4 RID: 11236
		[RequiredField]
		public FsmFloat max;

		// Token: 0x04002BE5 RID: 11237
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeResult;
	}
}
