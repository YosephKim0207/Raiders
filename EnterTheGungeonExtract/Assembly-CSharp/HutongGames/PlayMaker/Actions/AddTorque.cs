using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000881 RID: 2177
	[Tooltip("Adds torque (rotational force) to a Game Object.")]
	[ActionCategory(ActionCategory.Physics)]
	public class AddTorque : ComponentAction<Rigidbody>
	{
		// Token: 0x06003082 RID: 12418 RVA: 0x000FF2E4 File Offset: 0x000FD4E4
		public override void Reset()
		{
			this.gameObject = null;
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

		// Token: 0x06003083 RID: 12419 RVA: 0x000FF34C File Offset: 0x000FD54C
		public override void OnPreprocess()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		// Token: 0x06003084 RID: 12420 RVA: 0x000FF35C File Offset: 0x000FD55C
		public override void OnEnter()
		{
			this.DoAddTorque();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x000FF378 File Offset: 0x000FD578
		public override void OnFixedUpdate()
		{
			this.DoAddTorque();
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x000FF380 File Offset: 0x000FD580
		private void DoAddTorque()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			Vector3 vector = ((!this.vector.IsNone) ? this.vector.Value : new Vector3(this.x.Value, this.y.Value, this.z.Value));
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
				base.rigidbody.AddTorque(vector, this.forceMode);
			}
			else
			{
				base.rigidbody.AddRelativeTorque(vector, this.forceMode);
			}
		}

		// Token: 0x04002125 RID: 8485
		[CheckForComponent(typeof(Rigidbody))]
		[Tooltip("The GameObject to add torque to.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002126 RID: 8486
		[UIHint(UIHint.Variable)]
		[Tooltip("A Vector3 torque. Optionally override any axis with the X, Y, Z parameters.")]
		public FsmVector3 vector;

		// Token: 0x04002127 RID: 8487
		[Tooltip("Torque around the X axis. To leave unchanged, set to 'None'.")]
		public FsmFloat x;

		// Token: 0x04002128 RID: 8488
		[Tooltip("Torque around the Y axis. To leave unchanged, set to 'None'.")]
		public FsmFloat y;

		// Token: 0x04002129 RID: 8489
		[Tooltip("Torque around the Z axis. To leave unchanged, set to 'None'.")]
		public FsmFloat z;

		// Token: 0x0400212A RID: 8490
		[Tooltip("Apply the force in world or local space.")]
		public Space space;

		// Token: 0x0400212B RID: 8491
		[Tooltip("The type of force to apply. See Unity Physics docs.")]
		public ForceMode forceMode;

		// Token: 0x0400212C RID: 8492
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
