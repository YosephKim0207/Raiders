using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000944 RID: 2372
	[Tooltip("Applies an explosion Force to all Game Objects with a Rigid Body inside a Radius.")]
	[ActionCategory(ActionCategory.Physics)]
	public class Explosion : FsmStateAction
	{
		// Token: 0x060033E3 RID: 13283 RVA: 0x0010E7D0 File Offset: 0x0010C9D0
		public override void Reset()
		{
			this.center = null;
			this.upwardsModifier = 0f;
			this.forceMode = ForceMode.Force;
			this.everyFrame = false;
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x0010E7F8 File Offset: 0x0010C9F8
		public override void OnPreprocess()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x0010E808 File Offset: 0x0010CA08
		public override void OnEnter()
		{
			this.DoExplosion();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x0010E824 File Offset: 0x0010CA24
		public override void OnFixedUpdate()
		{
			this.DoExplosion();
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x0010E82C File Offset: 0x0010CA2C
		private void DoExplosion()
		{
			Collider[] array = Physics.OverlapSphere(this.center.Value, this.radius.Value);
			foreach (Collider collider in array)
			{
				Rigidbody component = collider.gameObject.GetComponent<Rigidbody>();
				if (component != null && this.ShouldApplyForce(collider.gameObject))
				{
					component.AddExplosionForce(this.force.Value, this.center.Value, this.radius.Value, this.upwardsModifier.Value, this.forceMode);
				}
			}
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x0010E8D4 File Offset: 0x0010CAD4
		private bool ShouldApplyForce(GameObject go)
		{
			int num = ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value);
			return ((1 << go.layer) & num) > 0;
		}

		// Token: 0x04002503 RID: 9475
		[RequiredField]
		[Tooltip("The world position of the center of the explosion.")]
		public FsmVector3 center;

		// Token: 0x04002504 RID: 9476
		[Tooltip("The strength of the explosion.")]
		[RequiredField]
		public FsmFloat force;

		// Token: 0x04002505 RID: 9477
		[Tooltip("The radius of the explosion. Force falls of linearly with distance.")]
		[RequiredField]
		public FsmFloat radius;

		// Token: 0x04002506 RID: 9478
		[Tooltip("Applies the force as if it was applied from beneath the object. This is useful since explosions that throw things up instead of pushing things to the side look cooler. A value of 2 will apply a force as if it is applied from 2 meters below while not changing the actual explosion position.")]
		public FsmFloat upwardsModifier;

		// Token: 0x04002507 RID: 9479
		[Tooltip("The type of force to apply.")]
		public ForceMode forceMode;

		// Token: 0x04002508 RID: 9480
		[UIHint(UIHint.Layer)]
		public FsmInt layer;

		// Token: 0x04002509 RID: 9481
		[Tooltip("Layers to effect.")]
		[UIHint(UIHint.Layer)]
		public FsmInt[] layerMask;

		// Token: 0x0400250A RID: 9482
		[Tooltip("Invert the mask, so you effect all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x0400250B RID: 9483
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
