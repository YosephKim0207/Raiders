using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B45 RID: 2885
	[Tooltip("Get the Tangent. You can use degrees, simply check on the DegToRad conversion")]
	[ActionCategory(ActionCategory.Trigonometry)]
	public class GetTan : FsmStateAction
	{
		// Token: 0x06003C98 RID: 15512 RVA: 0x00130840 File Offset: 0x0012EA40
		public override void Reset()
		{
			this.angle = null;
			this.DegToRad = true;
			this.everyFrame = false;
			this.result = null;
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x00130864 File Offset: 0x0012EA64
		public override void OnEnter()
		{
			this.DoTan();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x00130880 File Offset: 0x0012EA80
		public override void OnUpdate()
		{
			this.DoTan();
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x00130888 File Offset: 0x0012EA88
		private void DoTan()
		{
			float num = this.angle.Value;
			if (this.DegToRad.Value)
			{
				num *= 0.017453292f;
			}
			this.result.Value = Mathf.Tan(num);
		}

		// Token: 0x04002EE9 RID: 12009
		[Tooltip("The angle. Note: You can use degrees, simply check DegtoRad if the angle is expressed in degrees.")]
		[RequiredField]
		public FsmFloat angle;

		// Token: 0x04002EEA RID: 12010
		[Tooltip("Check on if the angle is expressed in degrees.")]
		public FsmBool DegToRad;

		// Token: 0x04002EEB RID: 12011
		[Tooltip("The angle tan")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat result;

		// Token: 0x04002EEC RID: 12012
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
