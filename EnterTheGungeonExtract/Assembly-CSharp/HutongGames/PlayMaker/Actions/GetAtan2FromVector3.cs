using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B41 RID: 2881
	[Tooltip("Get the Arc Tangent 2 as in atan2(y,x) from a vector 3, where you pick which is x and y from the vector 3. You can get the result in degrees, simply check on the RadToDeg conversion")]
	[ActionCategory(ActionCategory.Trigonometry)]
	public class GetAtan2FromVector3 : FsmStateAction
	{
		// Token: 0x06003C89 RID: 15497 RVA: 0x001305C0 File Offset: 0x0012E7C0
		public override void Reset()
		{
			this.vector3 = null;
			this.xAxis = GetAtan2FromVector3.aTan2EnumAxis.x;
			this.yAxis = GetAtan2FromVector3.aTan2EnumAxis.y;
			this.RadToDeg = true;
			this.everyFrame = false;
			this.angle = null;
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x001305F4 File Offset: 0x0012E7F4
		public override void OnEnter()
		{
			this.DoATan();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x00130610 File Offset: 0x0012E810
		public override void OnUpdate()
		{
			this.DoATan();
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x00130618 File Offset: 0x0012E818
		private void DoATan()
		{
			float num = this.vector3.Value.x;
			if (this.xAxis == GetAtan2FromVector3.aTan2EnumAxis.y)
			{
				num = this.vector3.Value.y;
			}
			else if (this.xAxis == GetAtan2FromVector3.aTan2EnumAxis.z)
			{
				num = this.vector3.Value.z;
			}
			float num2 = this.vector3.Value.y;
			if (this.yAxis == GetAtan2FromVector3.aTan2EnumAxis.x)
			{
				num2 = this.vector3.Value.x;
			}
			else if (this.yAxis == GetAtan2FromVector3.aTan2EnumAxis.z)
			{
				num2 = this.vector3.Value.z;
			}
			float num3 = Mathf.Atan2(num2, num);
			if (this.RadToDeg.Value)
			{
				num3 *= 57.29578f;
			}
			this.angle.Value = num3;
		}

		// Token: 0x04002ED7 RID: 11991
		[RequiredField]
		[Tooltip("The vector3 definition of the tan")]
		public FsmVector3 vector3;

		// Token: 0x04002ED8 RID: 11992
		[Tooltip("which axis in the vector3 to use as the x value of the tan")]
		[RequiredField]
		public GetAtan2FromVector3.aTan2EnumAxis xAxis;

		// Token: 0x04002ED9 RID: 11993
		[Tooltip("which axis in the vector3 to use as the y value of the tan")]
		[RequiredField]
		public GetAtan2FromVector3.aTan2EnumAxis yAxis;

		// Token: 0x04002EDA RID: 11994
		[Tooltip("The resulting angle. Note:If you want degrees, simply check RadToDeg")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat angle;

		// Token: 0x04002EDB RID: 11995
		[Tooltip("Check on if you want the angle expressed in degrees.")]
		public FsmBool RadToDeg;

		// Token: 0x04002EDC RID: 11996
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x02000B42 RID: 2882
		public enum aTan2EnumAxis
		{
			// Token: 0x04002EDE RID: 11998
			x,
			// Token: 0x04002EDF RID: 11999
			y,
			// Token: 0x04002EE0 RID: 12000
			z
		}
	}
}
