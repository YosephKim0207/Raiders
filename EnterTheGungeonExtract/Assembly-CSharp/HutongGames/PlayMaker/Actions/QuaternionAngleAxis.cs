using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A93 RID: 2707
	[Tooltip("Creates a rotation which rotates angle degrees around axis.")]
	[ActionCategory(ActionCategory.Quaternion)]
	public class QuaternionAngleAxis : QuaternionBaseAction
	{
		// Token: 0x06003974 RID: 14708 RVA: 0x00125D34 File Offset: 0x00123F34
		public override void Reset()
		{
			this.angle = null;
			this.axis = null;
			this.result = null;
			this.everyFrame = true;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x00125D5C File Offset: 0x00123F5C
		public override void OnEnter()
		{
			this.DoQuatAngleAxis();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x00125D78 File Offset: 0x00123F78
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.DoQuatAngleAxis();
			}
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x00125D8C File Offset: 0x00123F8C
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.DoQuatAngleAxis();
			}
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x00125DA0 File Offset: 0x00123FA0
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.DoQuatAngleAxis();
			}
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x00125DB4 File Offset: 0x00123FB4
		private void DoQuatAngleAxis()
		{
			this.result.Value = Quaternion.AngleAxis(this.angle.Value, this.axis.Value);
		}

		// Token: 0x04002BB9 RID: 11193
		[RequiredField]
		[Tooltip("The angle.")]
		public FsmFloat angle;

		// Token: 0x04002BBA RID: 11194
		[Tooltip("The rotation axis.")]
		[RequiredField]
		public FsmVector3 axis;

		// Token: 0x04002BBB RID: 11195
		[Tooltip("Store the rotation of this quaternion variable.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion result;
	}
}
