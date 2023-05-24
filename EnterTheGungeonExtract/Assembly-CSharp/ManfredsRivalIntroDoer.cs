using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200105C RID: 4188
[RequireComponent(typeof(GenericIntroDoer))]
public class ManfredsRivalIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005C15 RID: 23573 RVA: 0x00234C7C File Offset: 0x00232E7C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005C16 RID: 23574 RVA: 0x00234C84 File Offset: 0x00232E84
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		List<AIActor> activeEnemies = base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			if (!(activeEnemies[i] == base.aiActor))
			{
				animators.Add(activeEnemies[i].spriteAnimator);
				activeEnemies[i].aiAnimator.LockFacingDirection = true;
				activeEnemies[i].aiAnimator.FacingDirection = -90f;
				this.m_knights.Add(activeEnemies[i]);
			}
		}
	}

	// Token: 0x06005C17 RID: 23575 RVA: 0x00234D24 File Offset: 0x00232F24
	public override void OnCleanup()
	{
		for (int i = 0; i < this.m_knights.Count; i++)
		{
			this.m_knights[i].aiAnimator.LockFacingDirection = true;
		}
	}

	// Token: 0x040055C3 RID: 21955
	private List<AIActor> m_knights = new List<AIActor>();
}
