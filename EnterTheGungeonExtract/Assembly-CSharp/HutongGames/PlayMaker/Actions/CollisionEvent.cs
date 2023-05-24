using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200090B RID: 2315
	[Tooltip("Detect collisions between the Owner of this FSM and other Game Objects that have RigidBody components.\nNOTE: The system events, COLLISION ENTER, COLLISION STAY, and COLLISION EXIT are sent automatically on collisions with any object. Use this action to filter collisions by Tag.")]
	[ActionCategory(ActionCategory.Physics)]
	public class CollisionEvent : FsmStateAction
	{
		// Token: 0x060032FA RID: 13050 RVA: 0x0010BA74 File Offset: 0x00109C74
		public override void Reset()
		{
			this.collision = CollisionType.OnCollisionEnter;
			this.collideTag = "Untagged";
			this.sendEvent = null;
			this.storeCollider = null;
			this.storeForce = null;
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x0010BAA4 File Offset: 0x00109CA4
		public override void OnPreprocess()
		{
			switch (this.collision)
			{
			case CollisionType.OnCollisionEnter:
				base.Fsm.HandleCollisionEnter = true;
				break;
			case CollisionType.OnCollisionStay:
				base.Fsm.HandleCollisionStay = true;
				break;
			case CollisionType.OnCollisionExit:
				base.Fsm.HandleCollisionExit = true;
				break;
			case CollisionType.OnControllerColliderHit:
				base.Fsm.HandleControllerColliderHit = true;
				break;
			case CollisionType.OnParticleCollision:
				base.Fsm.HandleParticleCollision = true;
				break;
			}
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x0010BB2C File Offset: 0x00109D2C
		private void StoreCollisionInfo(Collision collisionInfo)
		{
			this.storeCollider.Value = collisionInfo.gameObject;
			this.storeForce.Value = collisionInfo.relativeVelocity.magnitude;
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x0010BB64 File Offset: 0x00109D64
		public override void DoCollisionEnter(Collision collisionInfo)
		{
			if (this.collision == CollisionType.OnCollisionEnter && collisionInfo.collider.gameObject.tag == this.collideTag.Value)
			{
				this.StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x0010BBBC File Offset: 0x00109DBC
		public override void DoCollisionStay(Collision collisionInfo)
		{
			if (this.collision == CollisionType.OnCollisionStay && collisionInfo.collider.gameObject.tag == this.collideTag.Value)
			{
				this.StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x0010BC14 File Offset: 0x00109E14
		public override void DoCollisionExit(Collision collisionInfo)
		{
			if (this.collision == CollisionType.OnCollisionExit && collisionInfo.collider.gameObject.tag == this.collideTag.Value)
			{
				this.StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x0010BC6C File Offset: 0x00109E6C
		public override void DoControllerColliderHit(ControllerColliderHit collisionInfo)
		{
			if (this.collision == CollisionType.OnControllerColliderHit && collisionInfo.collider.gameObject.tag == this.collideTag.Value)
			{
				if (this.storeCollider != null)
				{
					this.storeCollider.Value = collisionInfo.gameObject;
				}
				this.storeForce.Value = 0f;
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x0010BCE8 File Offset: 0x00109EE8
		public override void DoParticleCollision(GameObject other)
		{
			if (this.collision == CollisionType.OnParticleCollision && other.tag == this.collideTag.Value)
			{
				if (this.storeCollider != null)
				{
					this.storeCollider.Value = other;
				}
				this.storeForce.Value = 0f;
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x0010BD54 File Offset: 0x00109F54
		public override string ErrorCheck()
		{
			return ActionHelpers.CheckOwnerPhysicsSetup(base.Owner);
		}

		// Token: 0x0400242F RID: 9263
		[Tooltip("The type of collision to detect.")]
		public CollisionType collision;

		// Token: 0x04002430 RID: 9264
		[Tooltip("Filter by Tag.")]
		[UIHint(UIHint.Tag)]
		public FsmString collideTag;

		// Token: 0x04002431 RID: 9265
		[Tooltip("Event to send if a collision is detected.")]
		public FsmEvent sendEvent;

		// Token: 0x04002432 RID: 9266
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the GameObject that collided with the Owner of this FSM.")]
		public FsmGameObject storeCollider;

		// Token: 0x04002433 RID: 9267
		[Tooltip("Store the force of the collision. NOTE: Use Get Collision Info to get more info about the collision.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeForce;
	}
}
