using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B3E RID: 2878
	[Tooltip("Get the Arc Tangent. You can get the result in degrees, simply check on the RadToDeg conversion")]
	[ActionCategory(ActionCategory.Trigonometry)]
	public class GetAtan : FsmStateAction
	{
		// Token: 0x06003C7A RID: 15482 RVA: 0x001303D4 File Offset: 0x0012E5D4
		public override void Reset()
		{
			this.Value = null;
			this.RadToDeg = true;
			this.everyFrame = false;
			this.angle = null;
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x001303F8 File Offset: 0x0012E5F8
		public override void OnEnter()
		{
			this.DoATan();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x00130414 File Offset: 0x0012E614
		public override void OnUpdate()
		{
			this.DoATan();
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x0013041C File Offset: 0x0012E61C
		private void DoATan()
		{
			float num = Mathf.Atan(this.Value.Value);
			if (this.RadToDeg.Value)
			{
				num *= 57.29578f;
			}
			this.angle.Value = num;
		}

		// Token: 0x04002ECA RID: 11978
		[RequiredField]
		[Tooltip("The value of the tan")]
		public FsmFloat Value;

		// Token: 0x04002ECB RID: 11979
		[UIHint(UIHint.Variable)]
		[Tooltip("The resulting angle. Note:If you want degrees, simply check RadToDeg")]
		[RequiredField]
		public FsmFloat angle;

		// Token: 0x04002ECC RID: 11980
		[Tooltip("Check on if you want the angle expressed in degrees.")]
		public FsmBool RadToDeg;

		// Token: 0x04002ECD RID: 11981
		public bool everyFrame;
	}
}
