using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A55 RID: 2645
	[Tooltip("Gets info on the last joint break 2D event.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetJointBreak2dInfo : FsmStateAction
	{
		// Token: 0x0600384F RID: 14415 RVA: 0x00120AE8 File Offset: 0x0011ECE8
		public override void Reset()
		{
			this.brokenJoint = null;
			this.reactionForce = null;
			this.reactionTorque = null;
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x00120B00 File Offset: 0x0011ED00
		private void StoreInfo()
		{
			if (base.Fsm.BrokenJoint2D == null)
			{
				return;
			}
			this.brokenJoint.Value = base.Fsm.BrokenJoint2D;
			this.reactionForce.Value = base.Fsm.BrokenJoint2D.reactionForce;
			this.reactionForceMagnitude.Value = base.Fsm.BrokenJoint2D.reactionForce.magnitude;
			this.reactionTorque.Value = base.Fsm.BrokenJoint2D.reactionTorque;
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x00120B94 File Offset: 0x0011ED94
		public override void OnEnter()
		{
			this.StoreInfo();
			base.Finish();
		}

		// Token: 0x04002A48 RID: 10824
		[Tooltip("Get the broken joint.")]
		[UIHint(UIHint.Variable)]
		[ObjectType(typeof(Joint2D))]
		public FsmObject brokenJoint;

		// Token: 0x04002A49 RID: 10825
		[Tooltip("Get the reaction force exerted by the broken joint. Unity 5.3+")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 reactionForce;

		// Token: 0x04002A4A RID: 10826
		[Tooltip("Get the magnitude of the reaction force exerted by the broken joint. Unity 5.3+")]
		[UIHint(UIHint.Variable)]
		public FsmFloat reactionForceMagnitude;

		// Token: 0x04002A4B RID: 10827
		[Tooltip("Get the reaction torque exerted by the broken joint. Unity 5.3+")]
		[UIHint(UIHint.Variable)]
		public FsmFloat reactionTorque;
	}
}
