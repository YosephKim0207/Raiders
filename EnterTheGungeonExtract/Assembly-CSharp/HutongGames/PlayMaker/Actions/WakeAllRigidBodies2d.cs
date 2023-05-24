using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A77 RID: 2679
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Rigid bodies 2D start sleeping when they come to rest. This action wakes up all rigid bodies 2D in the scene. E.g., if you Set Gravity 2D and want objects at rest to respond.")]
	public class WakeAllRigidBodies2d : FsmStateAction
	{
		// Token: 0x060038F4 RID: 14580 RVA: 0x0012445C File Offset: 0x0012265C
		public override void Reset()
		{
			this.everyFrame = false;
		}

		// Token: 0x060038F5 RID: 14581 RVA: 0x00124468 File Offset: 0x00122668
		public override void OnEnter()
		{
			this.DoWakeAll();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060038F6 RID: 14582 RVA: 0x00124484 File Offset: 0x00122684
		public override void OnUpdate()
		{
			this.DoWakeAll();
		}

		// Token: 0x060038F7 RID: 14583 RVA: 0x0012448C File Offset: 0x0012268C
		private void DoWakeAll()
		{
			Rigidbody2D[] array = UnityEngine.Object.FindObjectsOfType(typeof(Rigidbody2D)) as Rigidbody2D[];
			if (array != null)
			{
				foreach (Rigidbody2D rigidbody2D in array)
				{
					rigidbody2D.WakeUp();
				}
			}
		}

		// Token: 0x04002B53 RID: 11091
		[Tooltip("Repeat every frame. Note: This would be very expensive!")]
		public bool everyFrame;
	}
}
