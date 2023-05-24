using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A8F RID: 2703
	[ActionCategory(ActionCategory.Quaternion)]
	[Tooltip("Gets a quaternion as euler angles.")]
	public class GetQuaternionEulerAngles : QuaternionBaseAction
	{
		// Token: 0x06003958 RID: 14680 RVA: 0x00125A78 File Offset: 0x00123C78
		public override void Reset()
		{
			this.quaternion = null;
			this.eulerAngles = null;
			this.everyFrame = true;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x00125A98 File Offset: 0x00123C98
		public override void OnEnter()
		{
			this.GetQuatEuler();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x00125AB4 File Offset: 0x00123CB4
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.GetQuatEuler();
			}
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x00125AC8 File Offset: 0x00123CC8
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.GetQuatEuler();
			}
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x00125ADC File Offset: 0x00123CDC
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.GetQuatEuler();
			}
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x00125AF0 File Offset: 0x00123CF0
		private void GetQuatEuler()
		{
			this.eulerAngles.Value = this.quaternion.Value.eulerAngles;
		}

		// Token: 0x04002BAE RID: 11182
		[Tooltip("The rotation")]
		[RequiredField]
		public FsmQuaternion quaternion;

		// Token: 0x04002BAF RID: 11183
		[Tooltip("The euler angles of the quaternion.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 eulerAngles;
	}
}
