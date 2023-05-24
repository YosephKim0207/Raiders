using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A6C RID: 2668
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Sets The degree to which this object is affected by gravity.  NOTE: Game object must have a rigidbody 2D.")]
	public class SetGravity2dScale : ComponentAction<Rigidbody2D>
	{
		// Token: 0x060038BF RID: 14527 RVA: 0x001234B4 File Offset: 0x001216B4
		public override void Reset()
		{
			this.gameObject = null;
			this.gravityScale = 1f;
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x001234D0 File Offset: 0x001216D0
		public override void OnEnter()
		{
			this.DoSetGravityScale();
			base.Finish();
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x001234E0 File Offset: 0x001216E0
		private void DoSetGravityScale()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			base.rigidbody2d.gravityScale = this.gravityScale.Value;
		}

		// Token: 0x04002B15 RID: 11029
		[Tooltip("The GameObject with a Rigidbody 2d attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002B16 RID: 11030
		[Tooltip("The gravity scale effect")]
		[RequiredField]
		public FsmFloat gravityScale;
	}
}
