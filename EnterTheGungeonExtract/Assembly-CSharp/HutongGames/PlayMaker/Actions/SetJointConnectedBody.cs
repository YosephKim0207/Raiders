using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AF7 RID: 2807
	[Tooltip("Connect a joint to a game object.")]
	[ActionCategory(ActionCategory.Physics)]
	public class SetJointConnectedBody : FsmStateAction
	{
		// Token: 0x06003B4B RID: 15179 RVA: 0x0012BB3C File Offset: 0x00129D3C
		public override void Reset()
		{
			this.joint = null;
			this.rigidBody = null;
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x0012BB4C File Offset: 0x00129D4C
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.joint);
			if (ownerDefaultTarget != null)
			{
				Joint component = ownerDefaultTarget.GetComponent<Joint>();
				if (component != null)
				{
					component.connectedBody = ((!(this.rigidBody.Value == null)) ? this.rigidBody.Value.GetComponent<Rigidbody>() : null);
				}
			}
			base.Finish();
		}

		// Token: 0x04002D80 RID: 11648
		[Tooltip("The joint to connect. Requires a Joint component.")]
		[CheckForComponent(typeof(Joint))]
		[RequiredField]
		public FsmOwnerDefault joint;

		// Token: 0x04002D81 RID: 11649
		[Tooltip("The game object to connect to the Joint. Set to none to connect the Joint to the world.")]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmGameObject rigidBody;
	}
}
