using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A54 RID: 2644
	[Tooltip("Gets info on the last collision 2D event and store in variables. See Unity and PlayMaker docs on Unity 2D physics.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetCollision2dInfo : FsmStateAction
	{
		// Token: 0x0600384B RID: 14411 RVA: 0x0012092C File Offset: 0x0011EB2C
		public override void Reset()
		{
			this.gameObjectHit = null;
			this.relativeVelocity = null;
			this.relativeSpeed = null;
			this.contactPoint = null;
			this.contactNormal = null;
			this.shapeCount = null;
			this.physics2dMaterialName = null;
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x00120960 File Offset: 0x0011EB60
		private void StoreCollisionInfo()
		{
			if (base.Fsm.Collision2DInfo == null)
			{
				return;
			}
			this.gameObjectHit.Value = base.Fsm.Collision2DInfo.gameObject;
			this.relativeSpeed.Value = base.Fsm.Collision2DInfo.relativeVelocity.magnitude;
			this.relativeVelocity.Value = base.Fsm.Collision2DInfo.relativeVelocity;
			this.physics2dMaterialName.Value = ((!(base.Fsm.Collision2DInfo.collider.sharedMaterial != null)) ? string.Empty : base.Fsm.Collision2DInfo.collider.sharedMaterial.name);
			this.shapeCount.Value = base.Fsm.Collision2DInfo.collider.shapeCount;
			if (base.Fsm.Collision2DInfo.contacts != null && base.Fsm.Collision2DInfo.contacts.Length > 0)
			{
				this.contactPoint.Value = base.Fsm.Collision2DInfo.contacts[0].point;
				this.contactNormal.Value = base.Fsm.Collision2DInfo.contacts[0].normal;
			}
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x00120AD0 File Offset: 0x0011ECD0
		public override void OnEnter()
		{
			this.StoreCollisionInfo();
			base.Finish();
		}

		// Token: 0x04002A41 RID: 10817
		[Tooltip("Get the GameObject hit.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;

		// Token: 0x04002A42 RID: 10818
		[Tooltip("Get the relative velocity of the collision.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 relativeVelocity;

		// Token: 0x04002A43 RID: 10819
		[Tooltip("Get the relative speed of the collision. Useful for controlling reactions. E.g., selecting an appropriate sound fx.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat relativeSpeed;

		// Token: 0x04002A44 RID: 10820
		[Tooltip("Get the world position of the collision contact. Useful for spawning effects etc.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 contactPoint;

		// Token: 0x04002A45 RID: 10821
		[Tooltip("Get the collision normal vector. Useful for aligning spawned effects etc.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 contactNormal;

		// Token: 0x04002A46 RID: 10822
		[Tooltip("The number of separate shaped regions in the collider.")]
		[UIHint(UIHint.Variable)]
		public FsmInt shapeCount;

		// Token: 0x04002A47 RID: 10823
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the name of the physics 2D material of the colliding GameObject. Useful for triggering different effects. Audio, particles...")]
		public FsmString physics2dMaterialName;
	}
}
