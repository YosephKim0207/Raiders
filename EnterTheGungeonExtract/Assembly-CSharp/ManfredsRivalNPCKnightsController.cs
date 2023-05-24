using System;
using System.Collections.Generic;
using Dungeonator;

// Token: 0x0200105E RID: 4190
public class ManfredsRivalNPCKnightsController : BraveBehaviour
{
	// Token: 0x06005C1E RID: 23582 RVA: 0x00235040 File Offset: 0x00233240
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005C1F RID: 23583 RVA: 0x00235048 File Offset: 0x00233248
	public void ManfredKnightsSpawned()
	{
		List<AIActor> activeEnemies = base.talkDoer.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			if (!(activeEnemies[i] == base.aiActor))
			{
				activeEnemies[i].behaviorSpeculator.enabled = false;
				this.m_knights.Add(activeEnemies[i]);
			}
		}
	}

	// Token: 0x040055C7 RID: 21959
	private List<AIActor> m_knights = new List<AIActor>();
}
