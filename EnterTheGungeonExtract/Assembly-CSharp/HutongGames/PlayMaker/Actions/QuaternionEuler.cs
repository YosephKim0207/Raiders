using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A97 RID: 2711
	[ActionCategory(ActionCategory.Quaternion)]
	[Tooltip("Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis (in that order).")]
	public class QuaternionEuler : QuaternionBaseAction
	{
		// Token: 0x06003984 RID: 14724 RVA: 0x00125F38 File Offset: 0x00124138
		public override void Reset()
		{
			this.eulerAngles = null;
			this.result = null;
			this.everyFrame = true;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x00125F58 File Offset: 0x00124158
		public override void OnEnter()
		{
			this.DoQuatEuler();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x00125F74 File Offset: 0x00124174
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.DoQuatEuler();
			}
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x00125F88 File Offset: 0x00124188
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.DoQuatEuler();
			}
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x00125F9C File Offset: 0x0012419C
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.DoQuatEuler();
			}
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x00125FB0 File Offset: 0x001241B0
		private void DoQuatEuler()
		{
			this.result.Value = Quaternion.Euler(this.eulerAngles.Value);
		}

		// Token: 0x04002BC7 RID: 11207
		[Tooltip("The Euler angles.")]
		[RequiredField]
		public FsmVector3 eulerAngles;

		// Token: 0x04002BC8 RID: 11208
		[Tooltip("Store the euler angles of this quaternion variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmQuaternion result;
	}
}
