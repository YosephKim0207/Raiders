using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200087D RID: 2173
	[Tooltip("Applies a force to a Game Object that simulates explosion effects. The explosion force will fall off linearly with distance. Hint: Use the Explosion Action instead to apply an explosion force to all objects in a blast radius.")]
	[ActionCategory(ActionCategory.Physics)]
	public class AddExplosionForce : ComponentAction<Rigidbody>
	{
		// Token: 0x0600306D RID: 12397 RVA: 0x000FEE48 File Offset: 0x000FD048
		public override void Reset()
		{
			this.gameObject = null;
			this.center = new FsmVector3
			{
				UseVariable = true
			};
			this.upwardsModifier = 0f;
			this.forceMode = ForceMode.Force;
			this.everyFrame = false;
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x000FEE90 File Offset: 0x000FD090
		public override void OnPreprocess()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x000FEEA0 File Offset: 0x000FD0A0
		public override void OnEnter()
		{
			this.DoAddExplosionForce();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x000FEEBC File Offset: 0x000FD0BC
		public override void OnFixedUpdate()
		{
			this.DoAddExplosionForce();
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x000FEEC4 File Offset: 0x000FD0C4
		private void DoAddExplosionForce()
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			if (this.center == null || !base.UpdateCache(gameObject))
			{
				return;
			}
			base.rigidbody.AddExplosionForce(this.force.Value, this.center.Value, this.radius.Value, this.upwardsModifier.Value, this.forceMode);
		}

		// Token: 0x0400210D RID: 8461
		[Tooltip("The GameObject to add the explosion force to.")]
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400210E RID: 8462
		[RequiredField]
		[Tooltip("The center of the explosion. Hint: this is often the position returned from a GetCollisionInfo action.")]
		public FsmVector3 center;

		// Token: 0x0400210F RID: 8463
		[RequiredField]
		[Tooltip("The strength of the explosion.")]
		public FsmFloat force;

		// Token: 0x04002110 RID: 8464
		[RequiredField]
		[Tooltip("The radius of the explosion. Force falls off linearly with distance.")]
		public FsmFloat radius;

		// Token: 0x04002111 RID: 8465
		[Tooltip("Applies the force as if it was applied from beneath the object. This is useful since explosions that throw things up instead of pushing things to the side look cooler. A value of 2 will apply a force as if it is applied from 2 meters below while not changing the actual explosion position.")]
		public FsmFloat upwardsModifier;

		// Token: 0x04002112 RID: 8466
		[Tooltip("The type of force to apply. See Unity Physics docs.")]
		public ForceMode forceMode;

		// Token: 0x04002113 RID: 8467
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
