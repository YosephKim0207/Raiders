using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A9A RID: 2714
	[ActionCategory(ActionCategory.Quaternion)]
	[Tooltip("Creates a rotation that looks along forward with the the head upwards along upwards.")]
	public class QuaternionLookRotation : QuaternionBaseAction
	{
		// Token: 0x06003999 RID: 14745 RVA: 0x00126168 File Offset: 0x00124368
		public override void Reset()
		{
			this.direction = null;
			this.upVector = new FsmVector3
			{
				UseVariable = true
			};
			this.result = null;
			this.everyFrame = true;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x001261A8 File Offset: 0x001243A8
		public override void OnEnter()
		{
			this.DoQuatLookRotation();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x001261C4 File Offset: 0x001243C4
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.DoQuatLookRotation();
			}
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x001261D8 File Offset: 0x001243D8
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.DoQuatLookRotation();
			}
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x001261EC File Offset: 0x001243EC
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.DoQuatLookRotation();
			}
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x00126200 File Offset: 0x00124400
		private void DoQuatLookRotation()
		{
			if (this.upVector.IsNone)
			{
				this.result.Value = Quaternion.LookRotation(this.direction.Value, this.upVector.Value);
			}
			else
			{
				this.result.Value = Quaternion.LookRotation(this.direction.Value);
			}
		}

		// Token: 0x04002BCF RID: 11215
		[Tooltip("the rotation direction")]
		[RequiredField]
		public FsmVector3 direction;

		// Token: 0x04002BD0 RID: 11216
		[Tooltip("The up direction")]
		public FsmVector3 upVector;

		// Token: 0x04002BD1 RID: 11217
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the inverse of the rotation variable.")]
		public FsmQuaternion result;
	}
}
