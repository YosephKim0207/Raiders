using System;
using UnityEngine;

// Token: 0x0200134B RID: 4939
public class AuraItem : PassiveItem
{
	// Token: 0x170010FF RID: 4351
	// (get) Token: 0x06006FF9 RID: 28665 RVA: 0x002C6000 File Offset: 0x002C4200
	private float ModifiedAuraRadius
	{
		get
		{
			return this.AuraRadius * this.GetRangeMultiplier();
		}
	}

	// Token: 0x17001100 RID: 4352
	// (get) Token: 0x06006FFA RID: 28666 RVA: 0x002C6010 File Offset: 0x002C4210
	private float ModifiedDamagePerSecond
	{
		get
		{
			return this.DamagePerSecond * this.GetDamageMultiplier();
		}
	}

	// Token: 0x06006FFB RID: 28667 RVA: 0x002C6020 File Offset: 0x002C4220
	protected override void Update()
	{
		base.Update();
		if (this.m_pickedUp && this.m_owner && !this.m_owner.IsStealthed && !GameManager.Instance.IsLoadingLevel)
		{
			this.DoAura();
		}
	}

	// Token: 0x06006FFC RID: 28668 RVA: 0x002C6074 File Offset: 0x002C4274
	public override DebrisObject Drop(PlayerController player)
	{
		if (this.m_extantAuraVFX != null)
		{
			UnityEngine.Object.Destroy(this.m_extantAuraVFX);
			this.m_extantAuraVFX = null;
		}
		return base.Drop(player);
	}

	// Token: 0x06006FFD RID: 28669 RVA: 0x002C60A0 File Offset: 0x002C42A0
	protected float GetDamageMultiplier()
	{
		float num = 1f;
		if (this.m_owner != null)
		{
			for (int i = 0; i < this.damageMultiplierSynergies.Length; i++)
			{
				if (this.m_owner.HasActiveBonusSynergy(this.damageMultiplierSynergies[i].RequiredSynergy, false))
				{
					num *= this.damageMultiplierSynergies[i].SynergyMultiplier;
				}
			}
		}
		return num;
	}

	// Token: 0x06006FFE RID: 28670 RVA: 0x002C6114 File Offset: 0x002C4314
	protected float GetRangeMultiplier()
	{
		float num = 1f;
		if (this.m_owner != null)
		{
			for (int i = 0; i < this.rangeMultiplierSynergies.Length; i++)
			{
				if (this.m_owner.HasActiveBonusSynergy(this.rangeMultiplierSynergies[i].RequiredSynergy, false))
				{
					num *= this.rangeMultiplierSynergies[i].SynergyMultiplier;
				}
			}
		}
		return num;
	}

	// Token: 0x06006FFF RID: 28671 RVA: 0x002C6188 File Offset: 0x002C4388
	protected virtual void DoAura()
	{
		if (this.m_extantAuraVFX == null)
		{
		}
		this.didDamageEnemies = false;
		if (this.AuraAction == null)
		{
			this.AuraAction = delegate(AIActor actor, float dist)
			{
				float num = this.ModifiedDamagePerSecond * BraveTime.DeltaTime;
				if (this.DamageFallsOffInRadius)
				{
					float num2 = dist / this.ModifiedAuraRadius;
					num = Mathf.Lerp(num, 0f, num2);
				}
				if (num > 0f)
				{
					this.didDamageEnemies = true;
				}
				actor.healthHaver.ApplyDamage(num, Vector2.zero, "Aura", this.damageTypes, DamageCategory.Normal, false, null, false);
			};
		}
		if (this.m_owner != null && this.m_owner.CurrentRoom != null)
		{
			this.m_owner.CurrentRoom.ApplyActionToNearbyEnemies(this.m_owner.CenterPosition, this.ModifiedAuraRadius, this.AuraAction);
		}
		if (this.didDamageEnemies)
		{
			this.m_owner.DidUnstealthyAction();
		}
	}

	// Token: 0x06007000 RID: 28672 RVA: 0x002C6228 File Offset: 0x002C4428
	protected override void OnDestroy()
	{
		if (this.m_extantAuraVFX != null)
		{
			UnityEngine.Object.Destroy(this.m_extantAuraVFX);
			this.m_extantAuraVFX = null;
		}
		base.OnDestroy();
	}

	// Token: 0x04006F48 RID: 28488
	public float AuraRadius = 5f;

	// Token: 0x04006F49 RID: 28489
	public CoreDamageTypes damageTypes;

	// Token: 0x04006F4A RID: 28490
	public float DamagePerSecond = 5f;

	// Token: 0x04006F4B RID: 28491
	public bool DamageFallsOffInRadius;

	// Token: 0x04006F4C RID: 28492
	public GameObject AuraVFX;

	// Token: 0x04006F4D RID: 28493
	public NumericSynergyMultiplier[] damageMultiplierSynergies;

	// Token: 0x04006F4E RID: 28494
	public NumericSynergyMultiplier[] rangeMultiplierSynergies;

	// Token: 0x04006F4F RID: 28495
	private GameObject m_extantAuraVFX;

	// Token: 0x04006F50 RID: 28496
	private Action<AIActor, float> AuraAction;

	// Token: 0x04006F51 RID: 28497
	private bool didDamageEnemies;
}
