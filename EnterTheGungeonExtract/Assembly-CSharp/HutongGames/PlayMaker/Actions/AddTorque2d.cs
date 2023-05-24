using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A52 RID: 2642
	[Tooltip("Adds a 2d torque (rotational force) to a Game Object.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class AddTorque2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x0600383C RID: 14396 RVA: 0x00120628 File Offset: 0x0011E828
		public override void OnPreprocess()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x00120638 File Offset: 0x0011E838
		public override void Reset()
		{
			this.gameObject = null;
			this.torque = null;
			this.everyFrame = false;
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x00120650 File Offset: 0x0011E850
		public override void OnEnter()
		{
			this.DoAddTorque();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x0012066C File Offset: 0x0011E86C
		public override void OnFixedUpdate()
		{
			this.DoAddTorque();
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x00120674 File Offset: 0x0011E874
		private void DoAddTorque()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			base.rigidbody2d.AddTorque(this.torque.Value, this.forceMode);
		}

		// Token: 0x04002A38 RID: 10808
		[Tooltip("The GameObject to add torque to.")]
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody2D))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002A39 RID: 10809
		[Tooltip("Option for applying the force")]
		public ForceMode2D forceMode;

		// Token: 0x04002A3A RID: 10810
		[Tooltip("Torque")]
		public FsmFloat torque;

		// Token: 0x04002A3B RID: 10811
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
