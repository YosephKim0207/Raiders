using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A73 RID: 2675
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Forces a Game Object's Rigid Body 2D to Sleep at least one frame.")]
	public class Sleep2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x060038E0 RID: 14560 RVA: 0x00123CFC File Offset: 0x00121EFC
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x00123D08 File Offset: 0x00121F08
		public override void OnEnter()
		{
			this.DoSleep();
			base.Finish();
		}

		// Token: 0x060038E2 RID: 14562 RVA: 0x00123D18 File Offset: 0x00121F18
		private void DoSleep()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			base.rigidbody2d.Sleep();
		}

		// Token: 0x04002B39 RID: 11065
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody2D))]
		[Tooltip("The GameObject with a Rigidbody2d attached")]
		public FsmOwnerDefault gameObject;
	}
}
