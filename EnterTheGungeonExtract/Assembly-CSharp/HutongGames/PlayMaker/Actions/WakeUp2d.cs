using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A78 RID: 2680
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Forces a Game Object's Rigid Body 2D to wake up.")]
	public class WakeUp2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x060038F9 RID: 14585 RVA: 0x001244DC File Offset: 0x001226DC
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x001244E8 File Offset: 0x001226E8
		public override void OnEnter()
		{
			this.DoWakeUp();
			base.Finish();
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x001244F8 File Offset: 0x001226F8
		private void DoWakeUp()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			base.rigidbody2d.WakeUp();
		}

		// Token: 0x04002B54 RID: 11092
		[Tooltip("The GameObject with a Rigidbody2d attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;
	}
}
