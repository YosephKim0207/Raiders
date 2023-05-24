using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C9D RID: 3229
	[ActionCategory(".NPCs")]
	[Tooltip("Makes this NPC become an enemy.")]
	public class EndHositlity : FsmStateAction
	{
		// Token: 0x06004515 RID: 17685 RVA: 0x001662E0 File Offset: 0x001644E0
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			if (!this.DontMoveNPC.Value)
			{
				component.transform.position += component.HostileObject.specRigidbody.UnitBottomLeft - component.specRigidbody.UnitBottomLeft;
				component.specRigidbody.Reinitialize();
			}
			SetNpcVisibility.SetVisible(component, true);
			component.aiAnimator.FacingDirection = component.HostileObject.aiAnimator.FacingDirection;
			component.aiAnimator.LockFacingDirection = true;
			base.Finish();
		}

		// Token: 0x04003742 RID: 14146
		public FsmBool DontMoveNPC = false;
	}
}
