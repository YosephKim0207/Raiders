using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009AA RID: 2474
	[Tooltip("Gets the Rotation of a Game Object and stores it in a Vector3 Variable or each Axis in a Float Variable")]
	[ActionCategory(ActionCategory.Transform)]
	public class GetRotation : FsmStateAction
	{
		// Token: 0x0600359E RID: 13726 RVA: 0x00113AB8 File Offset: 0x00111CB8
		public override void Reset()
		{
			this.gameObject = null;
			this.quaternion = null;
			this.vector = null;
			this.xAngle = null;
			this.yAngle = null;
			this.zAngle = null;
			this.space = Space.World;
			this.everyFrame = false;
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x00113AF4 File Offset: 0x00111CF4
		public override void OnEnter()
		{
			this.DoGetRotation();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x00113B10 File Offset: 0x00111D10
		public override void OnUpdate()
		{
			this.DoGetRotation();
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x00113B18 File Offset: 0x00111D18
		private void DoGetRotation()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (this.space == Space.World)
			{
				this.quaternion.Value = ownerDefaultTarget.transform.rotation;
				Vector3 eulerAngles = ownerDefaultTarget.transform.eulerAngles;
				this.vector.Value = eulerAngles;
				this.xAngle.Value = eulerAngles.x;
				this.yAngle.Value = eulerAngles.y;
				this.zAngle.Value = eulerAngles.z;
			}
			else
			{
				Vector3 localEulerAngles = ownerDefaultTarget.transform.localEulerAngles;
				this.quaternion.Value = Quaternion.Euler(localEulerAngles);
				this.vector.Value = localEulerAngles;
				this.xAngle.Value = localEulerAngles.x;
				this.yAngle.Value = localEulerAngles.y;
				this.zAngle.Value = localEulerAngles.z;
			}
		}

		// Token: 0x040026E9 RID: 9961
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040026EA RID: 9962
		[UIHint(UIHint.Variable)]
		public FsmQuaternion quaternion;

		// Token: 0x040026EB RID: 9963
		[Title("Euler Angles")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		// Token: 0x040026EC RID: 9964
		[UIHint(UIHint.Variable)]
		public FsmFloat xAngle;

		// Token: 0x040026ED RID: 9965
		[UIHint(UIHint.Variable)]
		public FsmFloat yAngle;

		// Token: 0x040026EE RID: 9966
		[UIHint(UIHint.Variable)]
		public FsmFloat zAngle;

		// Token: 0x040026EF RID: 9967
		public Space space;

		// Token: 0x040026F0 RID: 9968
		public bool everyFrame;
	}
}
