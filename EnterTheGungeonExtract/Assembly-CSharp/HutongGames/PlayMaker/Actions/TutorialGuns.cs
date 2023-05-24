using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dungeonator;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CDB RID: 3291
	[Tooltip("Controls the wall guns in the tutorial.")]
	[ActionCategory(".NPCs")]
	public class TutorialGuns : FsmStateAction
	{
		// Token: 0x060045E0 RID: 17888 RVA: 0x0016AEB0 File Offset: 0x001690B0
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			List<AIActor> activeEnemies = component.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				AIActor aiactor = activeEnemies[i];
				if (this.enable.Value)
				{
					aiactor.enabled = true;
					aiactor.specRigidbody.enabled = true;
					aiactor.State = AIActor.ActorState.Normal;
				}
				if (this.disable.Value)
				{
					aiactor.enabled = false;
					aiactor.aiAnimator.PlayUntilCancelled("deactivate", false, null, -1f, false);
					aiactor.specRigidbody.enabled = false;
				}
			}
			if (this.disable.Value)
			{
				ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
				for (int j = allProjectiles.Count - 1; j >= 0; j--)
				{
					if (allProjectiles[j])
					{
						allProjectiles[j].DieInAir(false, true, true, false);
					}
				}
			}
			base.Finish();
		}

		// Token: 0x04003822 RID: 14370
		public FsmBool enable;

		// Token: 0x04003823 RID: 14371
		public FsmBool disable;
	}
}
