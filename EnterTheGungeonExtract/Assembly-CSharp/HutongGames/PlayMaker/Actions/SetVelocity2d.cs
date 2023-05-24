using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A71 RID: 2673
	[Tooltip("Sets the 2d Velocity of a Game Object. To leave any axis unchanged, set variable to 'None'. NOTE: Game object must have a rigidbody 2D.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class SetVelocity2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x060038D5 RID: 14549 RVA: 0x0012393C File Offset: 0x00121B3C
		public override void Reset()
		{
			this.gameObject = null;
			this.vector = null;
			this.x = new FsmFloat
			{
				UseVariable = true
			};
			this.y = new FsmFloat
			{
				UseVariable = true
			};
			this.everyFrame = false;
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x00123988 File Offset: 0x00121B88
		public override void Awake()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x00123998 File Offset: 0x00121B98
		public override void OnEnter()
		{
			this.DoSetVelocity();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x001239B4 File Offset: 0x00121BB4
		public override void OnFixedUpdate()
		{
			this.DoSetVelocity();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x001239D0 File Offset: 0x00121BD0
		private void DoSetVelocity()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			Vector2 vector;
			if (this.vector.IsNone)
			{
				vector = base.rigidbody2d.velocity;
			}
			else
			{
				vector = this.vector.Value;
			}
			if (!this.x.IsNone)
			{
				vector.x = this.x.Value;
			}
			if (!this.y.IsNone)
			{
				vector.y = this.y.Value;
			}
			base.rigidbody2d.velocity = vector;
		}

		// Token: 0x04002B29 RID: 11049
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002B2A RID: 11050
		[Tooltip("A Vector2 value for the velocity")]
		public FsmVector2 vector;

		// Token: 0x04002B2B RID: 11051
		[Tooltip("The y value of the velocity. Overrides 'Vector' x value if set")]
		public FsmFloat x;

		// Token: 0x04002B2C RID: 11052
		[Tooltip("The y value of the velocity. Overrides 'Vector' y value if set")]
		public FsmFloat y;

		// Token: 0x04002B2D RID: 11053
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
