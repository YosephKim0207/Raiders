using System;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C6E RID: 3182
	[ActionCategory(".Brave")]
	[Tooltip("Responds to trigger events with Speculative Rigidbodies.")]
	public class SpecTriggerEvent : FsmStateAction
	{
		// Token: 0x06004464 RID: 17508 RVA: 0x00161AB8 File Offset: 0x0015FCB8
		public override void Reset()
		{
			this.triggerIndices = new FsmInt[0];
			this.events = new FsmEvent[0];
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x00161AD4 File Offset: 0x0015FCD4
		public override string ErrorCheck()
		{
			string text = string.Empty;
			SpeculativeRigidbody component = base.Owner.GetComponent<SpeculativeRigidbody>();
			if (!component)
			{
				text += "Owner does not have a Speculative Rigidbody.\n";
			}
			else
			{
				int num = 0;
				for (int i = 0; i < component.PixelColliders.Count; i++)
				{
					if (component.PixelColliders[i].IsTrigger)
					{
						num++;
					}
				}
				for (int j = 0; j < this.triggerIndices.Length; j++)
				{
					if (this.triggerIndices[j].Value >= num)
					{
						text += string.Format("Trigger index {0} is too high for a Speculative Rigidbody with {1} triggers.\n", this.triggerIndices[j].Value, num);
					}
				}
			}
			return text;
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x00161BA4 File Offset: 0x0015FDA4
		public override void OnEnter()
		{
			this.m_specRigidbody = base.Owner.GetComponent<SpeculativeRigidbody>();
			if (this.m_specRigidbody)
			{
				for (int i = 0; i < this.m_specRigidbody.PixelColliders.Count; i++)
				{
					PixelCollider pixelCollider = this.m_specRigidbody.PixelColliders[i];
					if (pixelCollider.IsTrigger)
					{
						this.m_triggerColliders.Add(pixelCollider);
					}
				}
				SpeculativeRigidbody specRigidbody = this.m_specRigidbody;
				specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.OnEnterTrigger));
			}
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x00161C44 File Offset: 0x0015FE44
		public override void OnExit()
		{
			if (this.m_specRigidbody)
			{
				SpeculativeRigidbody specRigidbody = this.m_specRigidbody;
				specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Remove(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.OnEnterTrigger));
			}
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x00161C80 File Offset: 0x0015FE80
		private void OnEnterTrigger(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
		{
			for (int i = 0; i < this.triggerIndices.Length; i++)
			{
				if (collisionData.MyPixelCollider == this.m_triggerColliders[this.triggerIndices[i].Value])
				{
					base.Fsm.Event(this.events[i]);
				}
			}
		}

		// Token: 0x04003675 RID: 13941
		[CompoundArray("Events", "Trigger Index", "Send Event")]
		[Tooltip("Event to play when the corresponding trigger detects a collision.")]
		public FsmInt[] triggerIndices;

		// Token: 0x04003676 RID: 13942
		public FsmEvent[] events;

		// Token: 0x04003677 RID: 13943
		private SpeculativeRigidbody m_specRigidbody;

		// Token: 0x04003678 RID: 13944
		private List<PixelCollider> m_triggerColliders = new List<PixelCollider>();
	}
}
