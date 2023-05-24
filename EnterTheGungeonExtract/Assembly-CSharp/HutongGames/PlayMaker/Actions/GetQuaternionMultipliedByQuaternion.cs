using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A91 RID: 2705
	[ActionCategory(ActionCategory.Quaternion)]
	[Tooltip("Get the quaternion from a quaternion multiplied by a quaternion.")]
	public class GetQuaternionMultipliedByQuaternion : QuaternionBaseAction
	{
		// Token: 0x06003966 RID: 14694 RVA: 0x00125BD4 File Offset: 0x00123DD4
		public override void Reset()
		{
			this.quaternionA = null;
			this.quaternionB = null;
			this.result = null;
			this.everyFrame = false;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x00125BFC File Offset: 0x00123DFC
		public override void OnEnter()
		{
			this.DoQuatMult();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x00125C18 File Offset: 0x00123E18
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.DoQuatMult();
			}
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x00125C2C File Offset: 0x00123E2C
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.DoQuatMult();
			}
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x00125C40 File Offset: 0x00123E40
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.DoQuatMult();
			}
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x00125C54 File Offset: 0x00123E54
		private void DoQuatMult()
		{
			this.result.Value = this.quaternionA.Value * this.quaternionB.Value;
		}

		// Token: 0x04002BB3 RID: 11187
		[Tooltip("The first quaternion to multiply")]
		[RequiredField]
		public FsmQuaternion quaternionA;

		// Token: 0x04002BB4 RID: 11188
		[Tooltip("The second quaternion to multiply")]
		[RequiredField]
		public FsmQuaternion quaternionB;

		// Token: 0x04002BB5 RID: 11189
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The resulting quaternion")]
		public FsmQuaternion result;
	}
}
