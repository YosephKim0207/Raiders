using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B54 RID: 2900
	[Tooltip("Linearly interpolates between 2 vectors.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2Lerp : FsmStateAction
	{
		// Token: 0x06003CD9 RID: 15577 RVA: 0x001312EC File Offset: 0x0012F4EC
		public override void Reset()
		{
			this.fromVector = new FsmVector2
			{
				UseVariable = true
			};
			this.toVector = new FsmVector2
			{
				UseVariable = true
			};
			this.storeResult = null;
			this.everyFrame = true;
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x00131330 File Offset: 0x0012F530
		public override void OnEnter()
		{
			this.DoVector2Lerp();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x0013134C File Offset: 0x0012F54C
		public override void OnUpdate()
		{
			this.DoVector2Lerp();
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x00131354 File Offset: 0x0012F554
		private void DoVector2Lerp()
		{
			this.storeResult.Value = Vector2.Lerp(this.fromVector.Value, this.toVector.Value, this.amount.Value);
		}

		// Token: 0x04002F1E RID: 12062
		[RequiredField]
		[Tooltip("First Vector.")]
		public FsmVector2 fromVector;

		// Token: 0x04002F1F RID: 12063
		[RequiredField]
		[Tooltip("Second Vector.")]
		public FsmVector2 toVector;

		// Token: 0x04002F20 RID: 12064
		[Tooltip("Interpolate between From Vector and ToVector by this amount. Value is clamped to 0-1 range. 0 = From Vector; 1 = To Vector; 0.5 = half way between.")]
		[RequiredField]
		public FsmFloat amount;

		// Token: 0x04002F21 RID: 12065
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Store the result in this vector variable.")]
		public FsmVector2 storeResult;

		// Token: 0x04002F22 RID: 12066
		[Tooltip("Repeat every frame. Useful if any of the values are changing.")]
		public bool everyFrame;
	}
}
