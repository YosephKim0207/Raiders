using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B1C RID: 2844
	[Tooltip("Sets the Velocity of a Game Object. To leave any axis unchanged, set variable to 'None'. NOTE: Game object must have a rigidbody.")]
	[ActionCategory(ActionCategory.Physics)]
	public class SetVelocity : ComponentAction<Rigidbody>
	{
		// Token: 0x06003BED RID: 15341 RVA: 0x0012DBA0 File Offset: 0x0012BDA0
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
			this.z = new FsmFloat
			{
				UseVariable = true
			};
			this.space = Space.Self;
			this.everyFrame = false;
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x0012DC08 File Offset: 0x0012BE08
		public override void OnPreprocess()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x0012DC18 File Offset: 0x0012BE18
		public override void OnEnter()
		{
			this.DoSetVelocity();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x0012DC34 File Offset: 0x0012BE34
		public override void OnFixedUpdate()
		{
			this.DoSetVelocity();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x0012DC50 File Offset: 0x0012BE50
		private void DoSetVelocity()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			Vector3 vector;
			if (this.vector.IsNone)
			{
				vector = ((this.space != Space.World) ? ownerDefaultTarget.transform.InverseTransformDirection(base.rigidbody.velocity) : base.rigidbody.velocity);
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
			if (!this.z.IsNone)
			{
				vector.z = this.z.Value;
			}
			base.rigidbody.velocity = ((this.space != Space.World) ? ownerDefaultTarget.transform.TransformDirection(vector) : vector);
		}

		// Token: 0x04002E0B RID: 11787
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002E0C RID: 11788
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		// Token: 0x04002E0D RID: 11789
		public FsmFloat x;

		// Token: 0x04002E0E RID: 11790
		public FsmFloat y;

		// Token: 0x04002E0F RID: 11791
		public FsmFloat z;

		// Token: 0x04002E10 RID: 11792
		public Space space;

		// Token: 0x04002E11 RID: 11793
		public bool everyFrame;
	}
}
