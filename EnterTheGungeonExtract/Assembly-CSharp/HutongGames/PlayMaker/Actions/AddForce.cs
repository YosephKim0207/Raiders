using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200087E RID: 2174
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Adds a force to a Game Object. Use Vector3 variable and/or Float variables for each axis.")]
	public class AddForce : ComponentAction<Rigidbody>
	{
		// Token: 0x06003073 RID: 12403 RVA: 0x000FEF5C File Offset: 0x000FD15C
		public override void Reset()
		{
			this.gameObject = null;
			this.atPosition = new FsmVector3
			{
				UseVariable = true
			};
			this.vector = null;
			this.x = new FsmFloat
			{
				UseVariable = true
			};
			this.y = new FsmFloat
			{
				UseVariable = true
			};
			this.z = new FsmFloat
			{
				UseVariable = true
			};
			this.space = Space.World;
			this.forceMode = ForceMode.Force;
			this.everyFrame = false;
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x000FEFDC File Offset: 0x000FD1DC
		public override void OnPreprocess()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x000FEFEC File Offset: 0x000FD1EC
		public override void OnEnter()
		{
			this.DoAddForce();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x000FF008 File Offset: 0x000FD208
		public override void OnFixedUpdate()
		{
			this.DoAddForce();
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x000FF010 File Offset: 0x000FD210
		private void DoAddForce()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			Vector3 vector = ((!this.vector.IsNone) ? this.vector.Value : default(Vector3));
			if (!this.x.IsNone)
			{
				vector.x = this.x.Value;
			}
			if (!this.y.IsNone)
			{
				vector.y = this.y.Value;
			}
			if (!this.z.IsNone)
			{
				vector.z = this.z.Value;
			}
			if (this.space == Space.World)
			{
				if (!this.atPosition.IsNone)
				{
					base.rigidbody.AddForceAtPosition(vector, this.atPosition.Value, this.forceMode);
				}
				else
				{
					base.rigidbody.AddForce(vector, this.forceMode);
				}
			}
			else
			{
				base.rigidbody.AddRelativeForce(vector, this.forceMode);
			}
		}

		// Token: 0x04002114 RID: 8468
		[Tooltip("The GameObject to apply the force to.")]
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002115 RID: 8469
		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally apply the force at a position on the object. This will also add some torque. The position is often returned from MousePick or GetCollisionInfo actions.")]
		public FsmVector3 atPosition;

		// Token: 0x04002116 RID: 8470
		[Tooltip("A Vector3 force to add. Optionally override any axis with the X, Y, Z parameters.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		// Token: 0x04002117 RID: 8471
		[Tooltip("Force along the X axis. To leave unchanged, set to 'None'.")]
		public FsmFloat x;

		// Token: 0x04002118 RID: 8472
		[Tooltip("Force along the Y axis. To leave unchanged, set to 'None'.")]
		public FsmFloat y;

		// Token: 0x04002119 RID: 8473
		[Tooltip("Force along the Z axis. To leave unchanged, set to 'None'.")]
		public FsmFloat z;

		// Token: 0x0400211A RID: 8474
		[Tooltip("Apply the force in world or local space.")]
		public Space space;

		// Token: 0x0400211B RID: 8475
		[Tooltip("The type of force to apply. See Unity Physics docs.")]
		public ForceMode forceMode;

		// Token: 0x0400211C RID: 8476
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
