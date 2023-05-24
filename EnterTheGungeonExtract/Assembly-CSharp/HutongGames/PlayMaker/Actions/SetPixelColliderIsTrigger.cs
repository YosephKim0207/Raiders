using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C6A RID: 3178
	[Tooltip("Sets the IsTriger property of a PixelCollider.")]
	[ActionCategory(".Brave")]
	public class SetPixelColliderIsTrigger : FsmStateAction
	{
		// Token: 0x06004454 RID: 17492 RVA: 0x001612A4 File Offset: 0x0015F4A4
		public override void Reset()
		{
			this.colliderIndex = 0;
			this.isTriggerValue = false;
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x001612C0 File Offset: 0x0015F4C0
		public override void OnEnter()
		{
			if (this.targetObject.Value == null)
			{
				TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
				component.specRigidbody.PixelColliders[this.colliderIndex.Value].IsTrigger = this.isTriggerValue.Value;
			}
			else
			{
				TalkDoerLite component2 = this.targetObject.Value.GetComponent<TalkDoerLite>();
				component2.specRigidbody.PixelColliders[this.colliderIndex.Value].IsTrigger = this.isTriggerValue.Value;
			}
			base.Finish();
		}

		// Token: 0x04003664 RID: 13924
		[Tooltip("If null, use self.")]
		public FsmGameObject targetObject;

		// Token: 0x04003665 RID: 13925
		[Tooltip("PixelCollider index to set (0 indexed).")]
		public FsmInt colliderIndex;

		// Token: 0x04003666 RID: 13926
		[Tooltip("The new value of the IsTrigger flag on the PixelCollider.")]
		public FsmBool isTriggerValue;
	}
}
