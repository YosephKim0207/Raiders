using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A90 RID: 2704
	[Tooltip("Creates a rotation which rotates from fromDirection to toDirection. Usually you use this to rotate a transform so that one of its axes eg. the y-axis - follows a target direction toDirection in world space.")]
	[ActionCategory(ActionCategory.Quaternion)]
	public class GetQuaternionFromRotation : QuaternionBaseAction
	{
		// Token: 0x0600395F RID: 14687 RVA: 0x00125B24 File Offset: 0x00123D24
		public override void Reset()
		{
			this.fromDirection = null;
			this.toDirection = null;
			this.result = null;
			this.everyFrame = false;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x00125B4C File Offset: 0x00123D4C
		public override void OnEnter()
		{
			this.DoQuatFromRotation();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x00125B68 File Offset: 0x00123D68
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.DoQuatFromRotation();
			}
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x00125B7C File Offset: 0x00123D7C
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.DoQuatFromRotation();
			}
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x00125B90 File Offset: 0x00123D90
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.DoQuatFromRotation();
			}
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x00125BA4 File Offset: 0x00123DA4
		private void DoQuatFromRotation()
		{
			this.result.Value = Quaternion.FromToRotation(this.fromDirection.Value, this.toDirection.Value);
		}

		// Token: 0x04002BB0 RID: 11184
		[Tooltip("the 'from' direction")]
		[RequiredField]
		public FsmVector3 fromDirection;

		// Token: 0x04002BB1 RID: 11185
		[Tooltip("the 'to' direction")]
		[RequiredField]
		public FsmVector3 toDirection;

		// Token: 0x04002BB2 RID: 11186
		[RequiredField]
		[Tooltip("the resulting quaternion")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion result;
	}
}
