using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B3C RID: 2876
	[Tooltip("Get the Arc Cosine. You can get the result in degrees, simply check on the RadToDeg conversion")]
	[ActionCategory(ActionCategory.Trigonometry)]
	public class GetACosine : FsmStateAction
	{
		// Token: 0x06003C70 RID: 15472 RVA: 0x001302AC File Offset: 0x0012E4AC
		public override void Reset()
		{
			this.angle = null;
			this.RadToDeg = true;
			this.everyFrame = false;
			this.Value = null;
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x001302D0 File Offset: 0x0012E4D0
		public override void OnEnter()
		{
			this.DoACosine();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x001302EC File Offset: 0x0012E4EC
		public override void OnUpdate()
		{
			this.DoACosine();
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x001302F4 File Offset: 0x0012E4F4
		private void DoACosine()
		{
			float num = Mathf.Acos(this.Value.Value);
			if (this.RadToDeg.Value)
			{
				num *= 57.29578f;
			}
			this.angle.Value = num;
		}

		// Token: 0x04002EC2 RID: 11970
		[Tooltip("The value of the cosine")]
		[RequiredField]
		public FsmFloat Value;

		// Token: 0x04002EC3 RID: 11971
		[Tooltip("The resulting angle. Note:If you want degrees, simply check RadToDeg")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat angle;

		// Token: 0x04002EC4 RID: 11972
		[Tooltip("Check on if you want the angle expressed in degrees.")]
		public FsmBool RadToDeg;

		// Token: 0x04002EC5 RID: 11973
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
