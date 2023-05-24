using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001378 RID: 4984
[Serializable]
public class HeatRingModule
{
	// Token: 0x17001110 RID: 4368
	// (get) Token: 0x060070EF RID: 28911 RVA: 0x002CD144 File Offset: 0x002CB344
	public bool IsActive
	{
		get
		{
			return this.m_indicator;
		}
	}

	// Token: 0x060070F0 RID: 28912 RVA: 0x002CD154 File Offset: 0x002CB354
	public void Trigger(float Radius, float Duration, GameActorFireEffect HeatEffect, tk2dBaseSprite sprite)
	{
		if (this.m_indicator)
		{
			return;
		}
		sprite.StartCoroutine(this.HandleHeatEffectsCR(Radius, Duration, HeatEffect, sprite));
	}

	// Token: 0x060070F1 RID: 28913 RVA: 0x002CD17C File Offset: 0x002CB37C
	private IEnumerator HandleHeatEffectsCR(float Radius, float Duration, GameActorFireEffect HeatEffect, tk2dBaseSprite sprite)
	{
		this.HandleRadialIndicator(Radius, sprite);
		float elapsed = 0f;
		RoomHandler r = sprite.transform.position.GetAbsoluteRoom();
		Vector3 tableCenter = sprite.WorldCenter.ToVector3ZisY(0f);
		Action<AIActor, float> AuraAction = delegate(AIActor actor, float dist)
		{
			actor.ApplyEffect(HeatEffect, 1f, null);
		};
		while (elapsed < Duration)
		{
			elapsed += BraveTime.DeltaTime;
			r.ApplyActionToNearbyEnemies(tableCenter.XY(), Radius, AuraAction);
			yield return null;
		}
		this.UnhandleRadialIndicator();
		yield break;
	}

	// Token: 0x060070F2 RID: 28914 RVA: 0x002CD1B4 File Offset: 0x002CB3B4
	private void HandleRadialIndicator(float Radius, tk2dBaseSprite sprite)
	{
		if (!this.m_indicator)
		{
			Vector3 vector = sprite.WorldCenter.ToVector3ZisY(0f);
			this.m_indicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), vector, Quaternion.identity, sprite.transform)).GetComponent<HeatIndicatorController>();
			this.m_indicator.CurrentRadius = Radius;
		}
	}

	// Token: 0x060070F3 RID: 28915 RVA: 0x002CD21C File Offset: 0x002CB41C
	private void UnhandleRadialIndicator()
	{
		if (this.m_indicator)
		{
			this.m_indicator.EndEffect();
			this.m_indicator = null;
		}
	}

	// Token: 0x0400706E RID: 28782
	private HeatIndicatorController m_indicator;
}
