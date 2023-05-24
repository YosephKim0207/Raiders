using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C87 RID: 3207
	[Tooltip("Makes this NPC become an enemy.")]
	[ActionCategory(".NPCs")]
	public class BecomeHostile : FsmStateAction
	{
		// Token: 0x060044C0 RID: 17600 RVA: 0x0016349C File Offset: 0x0016169C
		public override void Reset()
		{
			this.enemyGuid = null;
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x001634A8 File Offset: 0x001616A8
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			if (this.alternativeTarget != null)
			{
				component = this.alternativeTarget;
			}
			SetNpcVisibility.SetVisible(component, false);
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.enemyGuid.Value);
			AIActor aiactor = AIActor.Spawn(orLoadByGuid, component.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor), component.ParentRoom, false, AIActor.AwakenAnimationType.Default, true);
			aiactor.specRigidbody.Initialize();
			aiactor.transform.position += component.specRigidbody.UnitBottomLeft - aiactor.specRigidbody.UnitBottomLeft;
			aiactor.specRigidbody.Reinitialize();
			aiactor.HasBeenEngaged = true;
			if (this.alternativeTarget == null)
			{
				GenericIntroDoer component2 = aiactor.GetComponent<GenericIntroDoer>();
				if (component2)
				{
					component2.TriggerSequence(component.TalkingPlayer);
				}
			}
			component.HostileObject = aiactor;
			base.Finish();
		}

		// Token: 0x040036C9 RID: 14025
		[Tooltip("The enemy prefab to spawn.")]
		public FsmString enemyGuid;

		// Token: 0x040036CA RID: 14026
		[Tooltip("Optionally, a different TalkDoerLite to become hostile. Used for controlling groups.")]
		public TalkDoerLite alternativeTarget;
	}
}
