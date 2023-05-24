using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A96 RID: 2710
	[Tooltip("Check if two quaternions are equals or not. Takes in account inversed representations of quaternions")]
	[ActionCategory(ActionCategory.Quaternion)]
	public class QuaternionCompare : QuaternionBaseAction
	{
		// Token: 0x0600397D RID: 14717 RVA: 0x00125E14 File Offset: 0x00124014
		public override void Reset()
		{
			this.Quaternion1 = new FsmQuaternion
			{
				UseVariable = true
			};
			this.Quaternion2 = new FsmQuaternion
			{
				UseVariable = true
			};
			this.equal = null;
			this.equalEvent = null;
			this.notEqualEvent = null;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x00125E68 File Offset: 0x00124068
		public override void OnEnter()
		{
			this.DoQuatCompare();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x00125E84 File Offset: 0x00124084
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.DoQuatCompare();
			}
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x00125E98 File Offset: 0x00124098
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.DoQuatCompare();
			}
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x00125EAC File Offset: 0x001240AC
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.DoQuatCompare();
			}
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x00125EC0 File Offset: 0x001240C0
		private void DoQuatCompare()
		{
			bool flag = Mathf.Abs(Quaternion.Dot(this.Quaternion1.Value, this.Quaternion2.Value)) > 0.999999f;
			this.equal.Value = flag;
			if (flag)
			{
				base.Fsm.Event(this.equalEvent);
			}
			else
			{
				base.Fsm.Event(this.notEqualEvent);
			}
		}

		// Token: 0x04002BC2 RID: 11202
		[Tooltip("First Quaternion")]
		[RequiredField]
		public FsmQuaternion Quaternion1;

		// Token: 0x04002BC3 RID: 11203
		[Tooltip("Second Quaternion")]
		[RequiredField]
		public FsmQuaternion Quaternion2;

		// Token: 0x04002BC4 RID: 11204
		[Tooltip("true if Quaternions are equal")]
		public FsmBool equal;

		// Token: 0x04002BC5 RID: 11205
		[Tooltip("Event sent if Quaternions are equal")]
		public FsmEvent equalEvent;

		// Token: 0x04002BC6 RID: 11206
		[Tooltip("Event sent if Quaternions are not equal")]
		public FsmEvent notEqualEvent;
	}
}
