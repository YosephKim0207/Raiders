using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A99 RID: 2713
	[Tooltip("Interpolates between from and to by t and normalizes the result afterwards.")]
	[ActionCategory(ActionCategory.Quaternion)]
	public class QuaternionLerp : QuaternionBaseAction
	{
		// Token: 0x06003992 RID: 14738 RVA: 0x00126078 File Offset: 0x00124278
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
			this.amount = 0.5f;
			this.storeResult = null;
			this.everyFrame = true;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x001260D4 File Offset: 0x001242D4
		public override void OnEnter()
		{
			this.DoQuatLerp();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003994 RID: 14740 RVA: 0x001260F0 File Offset: 0x001242F0
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.DoQuatLerp();
			}
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x00126104 File Offset: 0x00124304
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.DoQuatLerp();
			}
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x00126118 File Offset: 0x00124318
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.DoQuatLerp();
			}
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x0012612C File Offset: 0x0012432C
		private void DoQuatLerp()
		{
			this.storeResult.Value = Quaternion.Lerp(this.fromQuaternion.Value, this.toQuaternion.Value, this.amount.Value);
		}

		// Token: 0x04002BCB RID: 11211
		[Tooltip("From Quaternion.")]
		[RequiredField]
		public FsmQuaternion fromQuaternion;

		// Token: 0x04002BCC RID: 11212
		[Tooltip("To Quaternion.")]
		[RequiredField]
		public FsmQuaternion toQuaternion;

		// Token: 0x04002BCD RID: 11213
		[HasFloatSlider(0f, 1f)]
		[Tooltip("Interpolate between fromQuaternion and toQuaternion by this amount. Value is clamped to 0-1 range. 0 = fromQuaternion; 1 = toQuaternion; 0.5 = half way between.")]
		[RequiredField]
		public FsmFloat amount;

		// Token: 0x04002BCE RID: 11214
		[Tooltip("Store the result in this quaternion variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmQuaternion storeResult;
	}
}
