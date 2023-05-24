using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A5F RID: 2655
	[Tooltip("Gets the 2d Velocity of a Game Object and stores it in a Vector2 Variable or each Axis in a Float Variable. NOTE: The Game Object must have a Rigid Body 2D.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetVelocity2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x0600387E RID: 14462 RVA: 0x00121F94 File Offset: 0x00120194
		public override void Reset()
		{
			this.gameObject = null;
			this.vector = null;
			this.x = null;
			this.y = null;
			this.space = Space.World;
			this.everyFrame = false;
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x00121FC0 File Offset: 0x001201C0
		public override void OnEnter()
		{
			this.DoGetVelocity();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x00121FDC File Offset: 0x001201DC
		public override void OnUpdate()
		{
			this.DoGetVelocity();
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x00121FE4 File Offset: 0x001201E4
		private void DoGetVelocity()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			Vector2 vector = base.rigidbody2d.velocity;
			if (this.space == Space.Self)
			{
				vector = base.rigidbody2d.transform.InverseTransformDirection(vector);
			}
			this.vector.Value = vector;
			this.x.Value = vector.x;
			this.y.Value = vector.y;
		}

		// Token: 0x04002AA9 RID: 10921
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002AAA RID: 10922
		[Tooltip("The velocity")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 vector;

		// Token: 0x04002AAB RID: 10923
		[Tooltip("The x value of the velocity")]
		[UIHint(UIHint.Variable)]
		public FsmFloat x;

		// Token: 0x04002AAC RID: 10924
		[Tooltip("The y value of the velocity")]
		[UIHint(UIHint.Variable)]
		public FsmFloat y;

		// Token: 0x04002AAD RID: 10925
		[Tooltip("The space reference to express the velocity")]
		public Space space;

		// Token: 0x04002AAE RID: 10926
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
