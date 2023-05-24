using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A5D RID: 2653
	[Tooltip("Gets the 2d Speed of a Game Object and stores it in a Float Variable. NOTE: The Game Object must have a rigid body 2D.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetSpeed2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x06003875 RID: 14453 RVA: 0x00121E24 File Offset: 0x00120024
		public override void Reset()
		{
			this.gameObject = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x00121E3C File Offset: 0x0012003C
		public override void OnEnter()
		{
			this.DoGetSpeed();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x00121E58 File Offset: 0x00120058
		public override void OnUpdate()
		{
			this.DoGetSpeed();
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x00121E60 File Offset: 0x00120060
		private void DoGetSpeed()
		{
			if (this.storeResult.IsNone)
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			this.storeResult.Value = base.rigidbody2d.velocity.magnitude;
		}

		// Token: 0x04002AA3 RID: 10915
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002AA4 RID: 10916
		[Tooltip("The speed, or in technical terms: velocity magnitude")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeResult;

		// Token: 0x04002AA5 RID: 10917
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
