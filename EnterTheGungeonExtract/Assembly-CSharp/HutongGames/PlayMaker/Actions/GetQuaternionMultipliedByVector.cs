using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A92 RID: 2706
	[Tooltip("Get the vector3 from a quaternion multiplied by a vector.")]
	[ActionCategory(ActionCategory.Quaternion)]
	public class GetQuaternionMultipliedByVector : QuaternionBaseAction
	{
		// Token: 0x0600396D RID: 14701 RVA: 0x00125C84 File Offset: 0x00123E84
		public override void Reset()
		{
			this.quaternion = null;
			this.vector3 = null;
			this.result = null;
			this.everyFrame = false;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x00125CAC File Offset: 0x00123EAC
		public override void OnEnter()
		{
			this.DoQuatMult();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x00125CC8 File Offset: 0x00123EC8
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.DoQuatMult();
			}
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x00125CDC File Offset: 0x00123EDC
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.DoQuatMult();
			}
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x00125CF0 File Offset: 0x00123EF0
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.DoQuatMult();
			}
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x00125D04 File Offset: 0x00123F04
		private void DoQuatMult()
		{
			this.result.Value = this.quaternion.Value * this.vector3.Value;
		}

		// Token: 0x04002BB6 RID: 11190
		[Tooltip("The quaternion to multiply")]
		[RequiredField]
		public FsmQuaternion quaternion;

		// Token: 0x04002BB7 RID: 11191
		[RequiredField]
		[Tooltip("The vector3 to multiply")]
		public FsmVector3 vector3;

		// Token: 0x04002BB8 RID: 11192
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The resulting vector3")]
		public FsmVector3 result;
	}
}
