using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B64 RID: 2916
	[Tooltip("Linearly interpolates between 2 vectors.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3Lerp : FsmStateAction
	{
		// Token: 0x06003D1A RID: 15642 RVA: 0x00132114 File Offset: 0x00130314
		public override void Reset()
		{
			this.fromVector = new FsmVector3
			{
				UseVariable = true
			};
			this.toVector = new FsmVector3
			{
				UseVariable = true
			};
			this.storeResult = null;
			this.everyFrame = true;
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x00132158 File Offset: 0x00130358
		public override void OnEnter()
		{
			this.DoVector3Lerp();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x00132174 File Offset: 0x00130374
		public override void OnUpdate()
		{
			this.DoVector3Lerp();
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x0013217C File Offset: 0x0013037C
		private void DoVector3Lerp()
		{
			this.storeResult.Value = Vector3.Lerp(this.fromVector.Value, this.toVector.Value, this.amount.Value);
		}

		// Token: 0x04002F65 RID: 12133
		[Tooltip("First Vector.")]
		[RequiredField]
		public FsmVector3 fromVector;

		// Token: 0x04002F66 RID: 12134
		[Tooltip("Second Vector.")]
		[RequiredField]
		public FsmVector3 toVector;

		// Token: 0x04002F67 RID: 12135
		[Tooltip("Interpolate between From Vector and ToVector by this amount. Value is clamped to 0-1 range. 0 = From Vector; 1 = To Vector; 0.5 = half way between.")]
		[RequiredField]
		public FsmFloat amount;

		// Token: 0x04002F68 RID: 12136
		[Tooltip("Store the result in this vector variable.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeResult;

		// Token: 0x04002F69 RID: 12137
		[Tooltip("Repeat every frame. Useful if any of the values are changing.")]
		public bool everyFrame;
	}
}
