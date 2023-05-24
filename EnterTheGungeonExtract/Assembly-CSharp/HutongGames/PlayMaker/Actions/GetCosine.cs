using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B43 RID: 2883
	[ActionCategory(ActionCategory.Trigonometry)]
	[Tooltip("Get the cosine. You can use degrees, simply check on the DegToRad conversion")]
	public class GetCosine : FsmStateAction
	{
		// Token: 0x06003C8E RID: 15502 RVA: 0x00130718 File Offset: 0x0012E918
		public override void Reset()
		{
			this.angle = null;
			this.DegToRad = true;
			this.everyFrame = false;
			this.result = null;
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x0013073C File Offset: 0x0012E93C
		public override void OnEnter()
		{
			this.DoCosine();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x00130758 File Offset: 0x0012E958
		public override void OnUpdate()
		{
			this.DoCosine();
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x00130760 File Offset: 0x0012E960
		private void DoCosine()
		{
			float num = this.angle.Value;
			if (this.DegToRad.Value)
			{
				num *= 0.017453292f;
			}
			this.result.Value = Mathf.Cos(num);
		}

		// Token: 0x04002EE1 RID: 12001
		[Tooltip("The angle. Note: You can use degrees, simply check DegtoRad if the angle is expressed in degrees.")]
		[RequiredField]
		public FsmFloat angle;

		// Token: 0x04002EE2 RID: 12002
		[Tooltip("Check on if the angle is expressed in degrees.")]
		public FsmBool DegToRad;

		// Token: 0x04002EE3 RID: 12003
		[Tooltip("The angle cosinus")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat result;

		// Token: 0x04002EE4 RID: 12004
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
