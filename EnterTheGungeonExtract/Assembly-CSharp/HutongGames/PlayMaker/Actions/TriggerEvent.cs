using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B3B RID: 2875
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Detect collisions with objects that have RigidBody components. \nNOTE: The system events, TRIGGER ENTER, TRIGGER STAY, and TRIGGER EXIT are sent when any object collides with the trigger. Use this action to filter collisions by Tag.")]
	public class TriggerEvent : FsmStateAction
	{
		// Token: 0x06003C68 RID: 15464 RVA: 0x00130100 File Offset: 0x0012E300
		public override void Reset()
		{
			this.trigger = TriggerType.OnTriggerEnter;
			this.collideTag = "Untagged";
			this.sendEvent = null;
			this.storeCollider = null;
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x00130128 File Offset: 0x0012E328
		public override void OnPreprocess()
		{
			TriggerType triggerType = this.trigger;
			if (triggerType != TriggerType.OnTriggerEnter)
			{
				if (triggerType != TriggerType.OnTriggerStay)
				{
					if (triggerType == TriggerType.OnTriggerExit)
					{
						base.Fsm.HandleTriggerExit = true;
					}
				}
				else
				{
					base.Fsm.HandleTriggerStay = true;
				}
			}
			else
			{
				base.Fsm.HandleTriggerEnter = true;
			}
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x00130188 File Offset: 0x0012E388
		private void StoreCollisionInfo(Collider collisionInfo)
		{
			this.storeCollider.Value = collisionInfo.gameObject;
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x0013019C File Offset: 0x0012E39C
		public override void DoTriggerEnter(Collider other)
		{
			if (this.trigger == TriggerType.OnTriggerEnter && other.gameObject.tag == this.collideTag.Value)
			{
				this.StoreCollisionInfo(other);
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x001301EC File Offset: 0x0012E3EC
		public override void DoTriggerStay(Collider other)
		{
			if (this.trigger == TriggerType.OnTriggerStay && other.gameObject.tag == this.collideTag.Value)
			{
				this.StoreCollisionInfo(other);
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x00130240 File Offset: 0x0012E440
		public override void DoTriggerExit(Collider other)
		{
			if (this.trigger == TriggerType.OnTriggerExit && other.gameObject.tag == this.collideTag.Value)
			{
				this.StoreCollisionInfo(other);
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x00130294 File Offset: 0x0012E494
		public override string ErrorCheck()
		{
			return ActionHelpers.CheckOwnerPhysicsSetup(base.Owner);
		}

		// Token: 0x04002EBE RID: 11966
		[Tooltip("The type of trigger event to detect.")]
		public TriggerType trigger;

		// Token: 0x04002EBF RID: 11967
		[Tooltip("Filter by Tag.")]
		[UIHint(UIHint.Tag)]
		public FsmString collideTag;

		// Token: 0x04002EC0 RID: 11968
		[Tooltip("Event to send if the trigger event is detected.")]
		public FsmEvent sendEvent;

		// Token: 0x04002EC1 RID: 11969
		[Tooltip("Store the GameObject that collided with the Owner of this FSM.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeCollider;
	}
}
