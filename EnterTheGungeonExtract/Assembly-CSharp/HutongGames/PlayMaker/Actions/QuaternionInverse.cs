using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A98 RID: 2712
	[Tooltip("Inverse a quaternion")]
	[ActionCategory(ActionCategory.Quaternion)]
	public class QuaternionInverse : QuaternionBaseAction
	{
		// Token: 0x0600398B RID: 14731 RVA: 0x00125FD8 File Offset: 0x001241D8
		public override void Reset()
		{
			this.rotation = null;
			this.result = null;
			this.everyFrame = true;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x00125FF8 File Offset: 0x001241F8
		public override void OnEnter()
		{
			this.DoQuatInverse();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x00126014 File Offset: 0x00124214
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.DoQuatInverse();
			}
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x00126028 File Offset: 0x00124228
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.DoQuatInverse();
			}
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x0012603C File Offset: 0x0012423C
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.DoQuatInverse();
			}
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x00126050 File Offset: 0x00124250
		private void DoQuatInverse()
		{
			this.result.Value = Quaternion.Inverse(this.rotation.Value);
		}

		// Token: 0x04002BC9 RID: 11209
		[Tooltip("the rotation")]
		[RequiredField]
		public FsmQuaternion rotation;

		// Token: 0x04002BCA RID: 11210
		[Tooltip("Store the inverse of the rotation variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmQuaternion result;
	}
}
