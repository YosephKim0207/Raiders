using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A51 RID: 2641
	[Tooltip("Adds a 2d force to a Game Object. Use Vector2 variable and/or Float variables for each axis.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class AddForce2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x06003836 RID: 14390 RVA: 0x0012043C File Offset: 0x0011E63C
		public override void Reset()
		{
			this.gameObject = null;
			this.atPosition = new FsmVector2
			{
				UseVariable = true
			};
			this.forceMode = ForceMode2D.Force;
			this.vector = null;
			this.vector3 = new FsmVector3
			{
				UseVariable = true
			};
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

		// Token: 0x06003837 RID: 14391 RVA: 0x001204B8 File Offset: 0x0011E6B8
		public override void OnPreprocess()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x001204C8 File Offset: 0x0011E6C8
		public override void OnEnter()
		{
			this.DoAddForce();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x001204E4 File Offset: 0x0011E6E4
		public override void OnFixedUpdate()
		{
			this.DoAddForce();
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x001204EC File Offset: 0x0011E6EC
		private void DoAddForce()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			Vector2 vector = ((!this.vector.IsNone) ? this.vector.Value : new Vector2(this.x.Value, this.y.Value));
			if (!this.vector3.IsNone)
			{
				vector.x = this.vector3.Value.x;
				vector.y = this.vector3.Value.y;
			}
			if (!this.x.IsNone)
			{
				vector.x = this.x.Value;
			}
			if (!this.y.IsNone)
			{
				vector.y = this.y.Value;
			}
			if (!this.atPosition.IsNone)
			{
				base.rigidbody2d.AddForceAtPosition(vector, this.atPosition.Value, this.forceMode);
			}
			else
			{
				base.rigidbody2d.AddForce(vector, this.forceMode);
			}
		}

		// Token: 0x04002A30 RID: 10800
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody2D))]
		[Tooltip("The GameObject to apply the force to.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002A31 RID: 10801
		[Tooltip("Option for applying the force")]
		public ForceMode2D forceMode;

		// Token: 0x04002A32 RID: 10802
		[Tooltip("Optionally apply the force at a position on the object. This will also add some torque. The position is often returned from MousePick or GetCollision2dInfo actions.")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 atPosition;

		// Token: 0x04002A33 RID: 10803
		[Tooltip("A Vector2 force to add. Optionally override any axis with the X, Y parameters.")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 vector;

		// Token: 0x04002A34 RID: 10804
		[Tooltip("Force along the X axis. To leave unchanged, set to 'None'.")]
		public FsmFloat x;

		// Token: 0x04002A35 RID: 10805
		[Tooltip("Force along the Y axis. To leave unchanged, set to 'None'.")]
		public FsmFloat y;

		// Token: 0x04002A36 RID: 10806
		[Tooltip("A Vector3 force to add. z is ignored")]
		public FsmVector3 vector3;

		// Token: 0x04002A37 RID: 10807
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
