using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A53 RID: 2643
	[Tooltip("Detect collisions between the Owner of this FSM and other Game Objects that have RigidBody2D components.\nNOTE: The system events, COLLISION ENTER 2D, COLLISION STAY 2D, and COLLISION EXIT 2D are sent automatically on collisions with any object. Use this action to filter collisions by Tag.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class Collision2dEvent : FsmStateAction
	{
		// Token: 0x06003842 RID: 14402 RVA: 0x001206C4 File Offset: 0x0011E8C4
		public override void Reset()
		{
			this.collision = Collision2DType.OnCollisionEnter2D;
			this.collideTag = "Untagged";
			this.sendEvent = null;
			this.storeCollider = null;
			this.storeForce = null;
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x001206F4 File Offset: 0x0011E8F4
		public override void OnPreprocess()
		{
			switch (this.collision)
			{
			case Collision2DType.OnCollisionEnter2D:
				base.Fsm.HandleCollisionEnter2D = true;
				break;
			case Collision2DType.OnCollisionStay2D:
				base.Fsm.HandleCollisionStay2D = true;
				break;
			case Collision2DType.OnCollisionExit2D:
				base.Fsm.HandleCollisionExit2D = true;
				break;
			case Collision2DType.OnParticleCollision:
				base.Fsm.HandleParticleCollision = true;
				break;
			}
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x00120768 File Offset: 0x0011E968
		private void StoreCollisionInfo(Collision2D collisionInfo)
		{
			this.storeCollider.Value = collisionInfo.gameObject;
			this.storeForce.Value = collisionInfo.relativeVelocity.magnitude;
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x001207A0 File Offset: 0x0011E9A0
		public override void DoCollisionEnter2D(Collision2D collisionInfo)
		{
			if (this.collision == Collision2DType.OnCollisionEnter2D && collisionInfo.collider.gameObject.tag == this.collideTag.Value)
			{
				this.StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x001207F8 File Offset: 0x0011E9F8
		public override void DoCollisionStay2D(Collision2D collisionInfo)
		{
			if (this.collision == Collision2DType.OnCollisionStay2D && collisionInfo.collider.gameObject.tag == this.collideTag.Value)
			{
				this.StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x00120850 File Offset: 0x0011EA50
		public override void DoCollisionExit2D(Collision2D collisionInfo)
		{
			if (this.collision == Collision2DType.OnCollisionExit2D && collisionInfo.collider.gameObject.tag == this.collideTag.Value)
			{
				this.StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x001208A8 File Offset: 0x0011EAA8
		public override void DoParticleCollision(GameObject other)
		{
			if (this.collision == Collision2DType.OnParticleCollision && other.tag == this.collideTag.Value)
			{
				if (this.storeCollider != null)
				{
					this.storeCollider.Value = other;
				}
				this.storeForce.Value = 0f;
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x00120914 File Offset: 0x0011EB14
		public override string ErrorCheck()
		{
			return ActionHelpers.CheckOwnerPhysics2dSetup(base.Owner);
		}

		// Token: 0x04002A3C RID: 10812
		[Tooltip("The type of collision to detect.")]
		public Collision2DType collision;

		// Token: 0x04002A3D RID: 10813
		[Tooltip("Filter by Tag.")]
		[UIHint(UIHint.Tag)]
		public FsmString collideTag;

		// Token: 0x04002A3E RID: 10814
		[Tooltip("Event to send if a collision is detected.")]
		public FsmEvent sendEvent;

		// Token: 0x04002A3F RID: 10815
		[Tooltip("Store the GameObject that collided with the Owner of this FSM.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeCollider;

		// Token: 0x04002A40 RID: 10816
		[Tooltip("Store the force of the collision. NOTE: Use Get Collision 2D Info to get more info about the collision.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeForce;
	}
}
