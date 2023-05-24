using System;
using UnityEngine;

// Token: 0x02000E21 RID: 3617
[Serializable]
public abstract class GameActorEffect
{
	// Token: 0x17000AC7 RID: 2759
	// (get) Token: 0x06004C92 RID: 19602 RVA: 0x001A2080 File Offset: 0x001A0280
	public virtual bool ResistanceAffectsDuration
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004C93 RID: 19603 RVA: 0x001A2084 File Offset: 0x001A0284
	public virtual void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
	{
	}

	// Token: 0x06004C94 RID: 19604 RVA: 0x001A2088 File Offset: 0x001A0288
	public virtual void OnDarkSoulsAccumulate(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f, Projectile sourceProjectile = null)
	{
	}

	// Token: 0x06004C95 RID: 19605 RVA: 0x001A208C File Offset: 0x001A028C
	public virtual void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
	{
	}

	// Token: 0x06004C96 RID: 19606 RVA: 0x001A2090 File Offset: 0x001A0290
	public virtual void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
	{
	}

	// Token: 0x06004C97 RID: 19607 RVA: 0x001A2094 File Offset: 0x001A0294
	public virtual void ApplyTint(GameActor actor)
	{
		if (this.AppliesTint)
		{
			actor.RegisterOverrideColor(this.TintColor, this.effectIdentifier);
		}
		if (this.AppliesOutlineTint && actor is AIActor)
		{
			AIActor aiactor = actor as AIActor;
			aiactor.SetOverrideOutlineColor(this.OutlineTintColor);
		}
	}

	// Token: 0x06004C98 RID: 19608 RVA: 0x001A20E8 File Offset: 0x001A02E8
	public virtual bool IsFinished(GameActor actor, RuntimeGameActorEffectData effectData, float elapsedTime)
	{
		float num = this.duration;
		if (this is GameActorFireEffect && actor.healthHaver && actor.healthHaver.IsBoss)
		{
			num = Mathf.Min(num, 8f);
		}
		if (this.ResistanceAffectsDuration)
		{
			float resistanceForEffectType = actor.GetResistanceForEffectType(this.resistanceType);
			num *= Mathf.Clamp01(1f - resistanceForEffectType);
		}
		return elapsedTime >= num;
	}

	// Token: 0x04004281 RID: 17025
	public bool AffectsPlayers = true;

	// Token: 0x04004282 RID: 17026
	public bool AffectsEnemies = true;

	// Token: 0x04004283 RID: 17027
	public string effectIdentifier = "effect";

	// Token: 0x04004284 RID: 17028
	public EffectResistanceType resistanceType;

	// Token: 0x04004285 RID: 17029
	public GameActorEffect.EffectStackingMode stackMode;

	// Token: 0x04004286 RID: 17030
	public float duration = 10f;

	// Token: 0x04004287 RID: 17031
	[ShowInInspectorIf("stackMode", 1, false)]
	public float maxStackedDuration = -1f;

	// Token: 0x04004288 RID: 17032
	public bool AppliesTint;

	// Token: 0x04004289 RID: 17033
	[ShowInInspectorIf("AppliesTint", false)]
	public Color TintColor = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x0400428A RID: 17034
	public bool AppliesDeathTint;

	// Token: 0x0400428B RID: 17035
	[ShowInInspectorIf("AppliesDeathTint", false)]
	public Color DeathTintColor = new Color(0.388f, 0.388f, 0.388f, 1f);

	// Token: 0x0400428C RID: 17036
	public bool AppliesOutlineTint;

	// Token: 0x0400428D RID: 17037
	[ColorUsage(true, true, 0f, 1000f, 0.125f, 3f)]
	public Color OutlineTintColor = new Color(0f, 0f, 0f, 1f);

	// Token: 0x0400428E RID: 17038
	public GameObject OverheadVFX;

	// Token: 0x0400428F RID: 17039
	public bool PlaysVFXOnActor;

	// Token: 0x02000E22 RID: 3618
	public enum EffectStackingMode
	{
		// Token: 0x04004291 RID: 17041
		Refresh,
		// Token: 0x04004292 RID: 17042
		Stack,
		// Token: 0x04004293 RID: 17043
		Ignore,
		// Token: 0x04004294 RID: 17044
		DarkSoulsAccumulate
	}
}
