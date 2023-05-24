using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A76 RID: 2678
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Detect 2D trigger collisions between the Owner of this FSM and other Game Objects that have RigidBody2D components.\nNOTE: The system events, TRIGGER ENTER 2D, TRIGGER STAY 2D, and TRIGGER EXIT 2D are sent automatically on collisions triggers with any object. Use this action to filter collision triggers by Tag.")]
	public class Trigger2dEvent : FsmStateAction
	{
		// Token: 0x060038EC RID: 14572 RVA: 0x001242B0 File Offset: 0x001224B0
		public override void Reset()
		{
			this.trigger = Trigger2DType.OnTriggerEnter2D;
			this.collideTag = "Untagged";
			this.sendEvent = null;
			this.storeCollider = null;
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x001242D8 File Offset: 0x001224D8
		public override void OnPreprocess()
		{
			Trigger2DType trigger2DType = this.trigger;
			if (trigger2DType != Trigger2DType.OnTriggerEnter2D)
			{
				if (trigger2DType != Trigger2DType.OnTriggerStay2D)
				{
					if (trigger2DType == Trigger2DType.OnTriggerExit2D)
					{
						base.Fsm.HandleTriggerExit2D = true;
					}
				}
				else
				{
					base.Fsm.HandleTriggerStay2D = true;
				}
			}
			else
			{
				base.Fsm.HandleTriggerEnter2D = true;
			}
		}

		// Token: 0x060038EE RID: 14574 RVA: 0x00124338 File Offset: 0x00122538
		private void StoreCollisionInfo(Collider2D collisionInfo)
		{
			this.storeCollider.Value = collisionInfo.gameObject;
		}

		// Token: 0x060038EF RID: 14575 RVA: 0x0012434C File Offset: 0x0012254C
		public override void DoTriggerEnter2D(Collider2D other)
		{
			if (this.trigger == Trigger2DType.OnTriggerEnter2D && other.gameObject.tag == this.collideTag.Value)
			{
				this.StoreCollisionInfo(other);
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x060038F0 RID: 14576 RVA: 0x0012439C File Offset: 0x0012259C
		public override void DoTriggerStay2D(Collider2D other)
		{
			if (this.trigger == Trigger2DType.OnTriggerStay2D && other.gameObject.tag == this.collideTag.Value)
			{
				this.StoreCollisionInfo(other);
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x060038F1 RID: 14577 RVA: 0x001243F0 File Offset: 0x001225F0
		public override void DoTriggerExit2D(Collider2D other)
		{
			if (this.trigger == Trigger2DType.OnTriggerExit2D && other.gameObject.tag == this.collideTag.Value)
			{
				this.StoreCollisionInfo(other);
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x00124444 File Offset: 0x00122644
		public override string ErrorCheck()
		{
			return ActionHelpers.CheckOwnerPhysics2dSetup(base.Owner);
		}

		// Token: 0x04002B4F RID: 11087
		[Tooltip("The type of trigger event to detect.")]
		public Trigger2DType trigger;

		// Token: 0x04002B50 RID: 11088
		[Tooltip("Filter by Tag.")]
		[UIHint(UIHint.Tag)]
		public FsmString collideTag;

		// Token: 0x04002B51 RID: 11089
		[Tooltip("Event to send if the trigger event is detected.")]
		public FsmEvent sendEvent;

		// Token: 0x04002B52 RID: 11090
		[Tooltip("Store the GameObject that collided with the Owner of this FSM.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeCollider;
	}
}
