using System;

// Token: 0x02000E1F RID: 3615
[Serializable]
public class GameActorCharmEffect : GameActorEffect
{
	// Token: 0x06004C8F RID: 19599 RVA: 0x001A1F6C File Offset: 0x001A016C
	public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
	{
		if (actor is AIActor)
		{
			AkSoundEngine.PostEvent("Play_OBJ_enemy_charmed_01", GameManager.Instance.gameObject);
			AIActor aiactor = actor as AIActor;
			aiactor.CanTargetEnemies = true;
			aiactor.CanTargetPlayers = false;
		}
	}

	// Token: 0x06004C90 RID: 19600 RVA: 0x001A1FB0 File Offset: 0x001A01B0
	public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
	{
		if (actor is AIActor)
		{
			AIActor aiactor = actor as AIActor;
			aiactor.CanTargetEnemies = false;
			aiactor.CanTargetPlayers = true;
		}
	}
}
