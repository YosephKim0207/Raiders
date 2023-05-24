using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B3F RID: 2879
	[Tooltip("Get the Arc Tangent 2 as in atan2(y,x). You can get the result in degrees, simply check on the RadToDeg conversion")]
	[ActionCategory(ActionCategory.Trigonometry)]
	public class GetAtan2 : FsmStateAction
	{
		// Token: 0x06003C7F RID: 15487 RVA: 0x00130468 File Offset: 0x0012E668
		public override void Reset()
		{
			this.xValue = null;
			this.yValue = null;
			this.RadToDeg = true;
			this.everyFrame = false;
			this.angle = null;
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x00130494 File Offset: 0x0012E694
		public override void OnEnter()
		{
			this.DoATan();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x001304B0 File Offset: 0x0012E6B0
		public override void OnUpdate()
		{
			this.DoATan();
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x001304B8 File Offset: 0x0012E6B8
		private void DoATan()
		{
			float num = Mathf.Atan2(this.yValue.Value, this.xValue.Value);
			if (this.RadToDeg.Value)
			{
				num *= 57.29578f;
			}
			this.angle.Value = num;
		}

		// Token: 0x04002ECE RID: 11982
		[RequiredField]
		[Tooltip("The x value of the tan")]
		public FsmFloat xValue;

		// Token: 0x04002ECF RID: 11983
		[RequiredField]
		[Tooltip("The y value of the tan")]
		public FsmFloat yValue;

		// Token: 0x04002ED0 RID: 11984
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The resulting angle. Note:If you want degrees, simply check RadToDeg")]
		public FsmFloat angle;

		// Token: 0x04002ED1 RID: 11985
		[Tooltip("Check on if you want the angle expressed in degrees.")]
		public FsmBool RadToDeg;

		// Token: 0x04002ED2 RID: 11986
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
