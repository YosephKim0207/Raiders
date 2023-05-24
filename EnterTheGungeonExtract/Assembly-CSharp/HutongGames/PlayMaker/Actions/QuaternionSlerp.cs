using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A9D RID: 2717
	[Tooltip("Spherically interpolates between from and to by t.")]
	[ActionCategory(ActionCategory.Quaternion)]
	public class QuaternionSlerp : QuaternionBaseAction
	{
		// Token: 0x060039AE RID: 14766 RVA: 0x001265AC File Offset: 0x001247AC
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
			this.amount = 0.1f;
			this.storeResult = null;
			this.everyFrame = true;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x00126608 File Offset: 0x00124808
		public override void OnEnter()
		{
			this.DoQuatSlerp();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x00126624 File Offset: 0x00124824
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.DoQuatSlerp();
			}
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x00126638 File Offset: 0x00124838
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.DoQuatSlerp();
			}
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x0012664C File Offset: 0x0012484C
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.DoQuatSlerp();
			}
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x00126660 File Offset: 0x00124860
		private void DoQuatSlerp()
		{
			this.storeResult.Value = Quaternion.Slerp(this.fromQuaternion.Value, this.toQuaternion.Value, this.amount.Value);
		}

		// Token: 0x04002BD9 RID: 11225
		[Tooltip("From Quaternion.")]
		[RequiredField]
		public FsmQuaternion fromQuaternion;

		// Token: 0x04002BDA RID: 11226
		[Tooltip("To Quaternion.")]
		[RequiredField]
		public FsmQuaternion toQuaternion;

		// Token: 0x04002BDB RID: 11227
		[HasFloatSlider(0f, 1f)]
		[Tooltip("Interpolate between fromQuaternion and toQuaternion by this amount. Value is clamped to 0-1 range. 0 = fromQuaternion; 1 = toQuaternion; 0.5 = half way between.")]
		[RequiredField]
		public FsmFloat amount;

		// Token: 0x04002BDC RID: 11228
		[Tooltip("Store the result in this quaternion variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmQuaternion storeResult;
	}
}
