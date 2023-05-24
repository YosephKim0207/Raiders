using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B44 RID: 2884
	[Tooltip("Get the sine. You can use degrees, simply check on the DegToRad conversion")]
	[ActionCategory(ActionCategory.Trigonometry)]
	public class GetSine : FsmStateAction
	{
		// Token: 0x06003C93 RID: 15507 RVA: 0x001307AC File Offset: 0x0012E9AC
		public override void Reset()
		{
			this.angle = null;
			this.DegToRad = true;
			this.everyFrame = false;
			this.result = null;
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x001307D0 File Offset: 0x0012E9D0
		public override void OnEnter()
		{
			this.DoSine();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x001307EC File Offset: 0x0012E9EC
		public override void OnUpdate()
		{
			this.DoSine();
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x001307F4 File Offset: 0x0012E9F4
		private void DoSine()
		{
			float num = this.angle.Value;
			if (this.DegToRad.Value)
			{
				num *= 0.017453292f;
			}
			this.result.Value = Mathf.Sin(num);
		}

		// Token: 0x04002EE5 RID: 12005
		[Tooltip("The angle. Note: You can use degrees, simply check DegtoRad if the angle is expressed in degrees.")]
		[RequiredField]
		public FsmFloat angle;

		// Token: 0x04002EE6 RID: 12006
		[Tooltip("Check on if the angle is expressed in degrees.")]
		public FsmBool DegToRad;

		// Token: 0x04002EE7 RID: 12007
		[Tooltip("The angle tan")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat result;

		// Token: 0x04002EE8 RID: 12008
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
