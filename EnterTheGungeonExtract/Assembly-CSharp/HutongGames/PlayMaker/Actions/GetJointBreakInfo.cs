using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200098C RID: 2444
	[Tooltip("Gets info on the last joint break event.")]
	[ActionCategory(ActionCategory.Physics)]
	public class GetJointBreakInfo : FsmStateAction
	{
		// Token: 0x06003522 RID: 13602 RVA: 0x00112888 File Offset: 0x00110A88
		public override void Reset()
		{
			this.breakForce = null;
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x00112894 File Offset: 0x00110A94
		public override void OnEnter()
		{
			this.breakForce.Value = base.Fsm.JointBreakForce;
			base.Finish();
		}

		// Token: 0x0400268C RID: 9868
		[Tooltip("Get the force that broke the joint.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat breakForce;
	}
}
