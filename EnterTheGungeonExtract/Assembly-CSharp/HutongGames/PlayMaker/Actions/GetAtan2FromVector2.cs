using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B40 RID: 2880
	[ActionCategory(ActionCategory.Trigonometry)]
	[Tooltip("Get the Arc Tangent 2 as in atan2(y,x) from a vector 2. You can get the result in degrees, simply check on the RadToDeg conversion")]
	public class GetAtan2FromVector2 : FsmStateAction
	{
		// Token: 0x06003C84 RID: 15492 RVA: 0x00130510 File Offset: 0x0012E710
		public override void Reset()
		{
			this.vector2 = null;
			this.RadToDeg = true;
			this.everyFrame = false;
			this.angle = null;
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x00130534 File Offset: 0x0012E734
		public override void OnEnter()
		{
			this.DoATan();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x00130550 File Offset: 0x0012E750
		public override void OnUpdate()
		{
			this.DoATan();
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x00130558 File Offset: 0x0012E758
		private void DoATan()
		{
			float num = Mathf.Atan2(this.vector2.Value.y, this.vector2.Value.x);
			if (this.RadToDeg.Value)
			{
				num *= 57.29578f;
			}
			this.angle.Value = num;
		}

		// Token: 0x04002ED3 RID: 11987
		[Tooltip("The vector2 of the tan")]
		[RequiredField]
		public FsmVector2 vector2;

		// Token: 0x04002ED4 RID: 11988
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The resulting angle. Note:If you want degrees, simply check RadToDeg")]
		public FsmFloat angle;

		// Token: 0x04002ED5 RID: 11989
		[Tooltip("Check on if you want the angle expressed in degrees.")]
		public FsmBool RadToDeg;

		// Token: 0x04002ED6 RID: 11990
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
