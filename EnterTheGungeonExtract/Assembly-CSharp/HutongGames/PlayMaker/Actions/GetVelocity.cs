using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009BE RID: 2494
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets the Velocity of a Game Object and stores it in a Vector3 Variable or each Axis in a Float Variable. NOTE: The Game Object must have a Rigid Body.")]
	public class GetVelocity : ComponentAction<Rigidbody>
	{
		// Token: 0x060035F4 RID: 13812 RVA: 0x00114914 File Offset: 0x00112B14
		public override void Reset()
		{
			this.gameObject = null;
			this.vector = null;
			this.x = null;
			this.y = null;
			this.z = null;
			this.space = Space.World;
			this.everyFrame = false;
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x00114948 File Offset: 0x00112B48
		public override void OnEnter()
		{
			this.DoGetVelocity();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x00114964 File Offset: 0x00112B64
		public override void OnUpdate()
		{
			this.DoGetVelocity();
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x0011496C File Offset: 0x00112B6C
		private void DoGetVelocity()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			Vector3 vector = base.rigidbody.velocity;
			if (this.space == Space.Self)
			{
				vector = ownerDefaultTarget.transform.InverseTransformDirection(vector);
			}
			this.vector.Value = vector;
			this.x.Value = vector.x;
			this.y.Value = vector.y;
			this.z.Value = vector.z;
		}

		// Token: 0x0400273C RID: 10044
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400273D RID: 10045
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		// Token: 0x0400273E RID: 10046
		[UIHint(UIHint.Variable)]
		public FsmFloat x;

		// Token: 0x0400273F RID: 10047
		[UIHint(UIHint.Variable)]
		public FsmFloat y;

		// Token: 0x04002740 RID: 10048
		[UIHint(UIHint.Variable)]
		public FsmFloat z;

		// Token: 0x04002741 RID: 10049
		public Space space;

		// Token: 0x04002742 RID: 10050
		public bool everyFrame;
	}
}
