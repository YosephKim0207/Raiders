using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A9C RID: 2716
	[ActionCategory(ActionCategory.Quaternion)]
	[Tooltip("Rotates a rotation from towards to. This is essentially the same as Quaternion.Slerp but instead the function will ensure that the angular speed never exceeds maxDegreesDelta. Negative values of maxDegreesDelta pushes the rotation away from to.")]
	public class QuaternionRotateTowards : QuaternionBaseAction
	{
		// Token: 0x060039A7 RID: 14759 RVA: 0x001264BC File Offset: 0x001246BC
		public override void Reset()
		{
			this.fromQuaternion = new FsmQuaternion
			{
				UseVariable = true
			};
			this.toQuaternion = new FsmQuaternion
			{
				UseVariable = true
			};
			this.maxDegreesDelta = 10f;
			this.storeResult = null;
			this.everyFrame = true;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x00126518 File Offset: 0x00124718
		public override void OnEnter()
		{
			this.DoQuatRotateTowards();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x00126534 File Offset: 0x00124734
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.DoQuatRotateTowards();
			}
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x00126548 File Offset: 0x00124748
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.DoQuatRotateTowards();
			}
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x0012655C File Offset: 0x0012475C
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.DoQuatRotateTowards();
			}
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x00126570 File Offset: 0x00124770
		private void DoQuatRotateTowards()
		{
			this.storeResult.Value = Quaternion.RotateTowards(this.fromQuaternion.Value, this.toQuaternion.Value, this.maxDegreesDelta.Value);
		}

		// Token: 0x04002BD5 RID: 11221
		[Tooltip("From Quaternion.")]
		[RequiredField]
		public FsmQuaternion fromQuaternion;

		// Token: 0x04002BD6 RID: 11222
		[Tooltip("To Quaternion.")]
		[RequiredField]
		public FsmQuaternion toQuaternion;

		// Token: 0x04002BD7 RID: 11223
		[Tooltip("The angular speed never exceeds maxDegreesDelta. Negative values of maxDegreesDelta pushes the rotation away from to.")]
		[RequiredField]
		public FsmFloat maxDegreesDelta;

		// Token: 0x04002BD8 RID: 11224
		[Tooltip("Store the result in this quaternion variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmQuaternion storeResult;
	}
}
