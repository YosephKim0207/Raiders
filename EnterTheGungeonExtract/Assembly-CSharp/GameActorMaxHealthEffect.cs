using System;

// Token: 0x02000E29 RID: 3625
[Serializable]
public class GameActorMaxHealthEffect : GameActorEffect
{
	// Token: 0x06004CB3 RID: 19635 RVA: 0x001A3754 File Offset: 0x001A1954
	public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
	{
		float currentHealthPercentage = actor.healthHaver.GetCurrentHealthPercentage();
		float num = actor.healthHaver.GetMaxHealth() * this.HealthMultiplier;
		actor.healthHaver.SetHealthMaximum(num, null, false);
		if (this.KeepHealthPercentage)
		{
			actor.healthHaver.ForceSetCurrentHealth(currentHealthPercentage * num);
		}
	}

	// Token: 0x06004CB4 RID: 19636 RVA: 0x001A37B0 File Offset: 0x001A19B0
	public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
	{
		float currentHealthPercentage = actor.healthHaver.GetCurrentHealthPercentage();
		float num = actor.healthHaver.GetMaxHealth() / this.HealthMultiplier;
		actor.healthHaver.SetHealthMaximum(num, null, false);
		if (this.KeepHealthPercentage)
		{
			actor.healthHaver.ForceSetCurrentHealth(currentHealthPercentage * num);
		}
	}

	// Token: 0x040042C1 RID: 17089
	public float HealthMultiplier = 1f;

	// Token: 0x040042C2 RID: 17090
	public bool KeepHealthPercentage = true;
}
