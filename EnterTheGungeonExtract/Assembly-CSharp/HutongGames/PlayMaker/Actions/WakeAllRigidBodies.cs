using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B6E RID: 2926
	[Tooltip("Rigid bodies start sleeping when they come to rest. This action wakes up all rigid bodies in the scene. E.g., if you Set Gravity and want objects at rest to respond.")]
	[ActionCategory(ActionCategory.Physics)]
	public class WakeAllRigidBodies : FsmStateAction
	{
		// Token: 0x06003D3F RID: 15679 RVA: 0x00132970 File Offset: 0x00130B70
		public override void Reset()
		{
			this.everyFrame = false;
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x0013297C File Offset: 0x00130B7C
		public override void OnEnter()
		{
			this.bodies = UnityEngine.Object.FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
			this.DoWakeAll();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x001329B0 File Offset: 0x00130BB0
		public override void OnUpdate()
		{
			this.DoWakeAll();
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x001329B8 File Offset: 0x00130BB8
		private void DoWakeAll()
		{
			this.bodies = UnityEngine.Object.FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
			if (this.bodies != null)
			{
				foreach (Rigidbody rigidbody in this.bodies)
				{
					rigidbody.WakeUp();
				}
			}
		}

		// Token: 0x04002F93 RID: 12179
		public bool everyFrame;

		// Token: 0x04002F94 RID: 12180
		private Rigidbody[] bodies;
	}
}
