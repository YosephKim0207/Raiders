using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001442 RID: 5186
public class NanomachinesItem : PassiveItem
{
	// Token: 0x060075BB RID: 30139 RVA: 0x002EE208 File Offset: 0x002EC408
	public override void MidGameSerialize(List<object> data)
	{
		base.MidGameSerialize(data);
		data.Add(this.m_receivedDamageCounter);
	}

	// Token: 0x060075BC RID: 30140 RVA: 0x002EE224 File Offset: 0x002EC424
	public override void MidGameDeserialize(List<object> data)
	{
		base.MidGameDeserialize(data);
		if (data.Count == 1)
		{
			this.m_receivedDamageCounter = (float)data[0];
		}
	}

	// Token: 0x060075BD RID: 30141 RVA: 0x002EE24C File Offset: 0x002EC44C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (!this.m_pickedUpThisRun)
		{
			player.healthHaver.Armor += (float)this.initialArmorBoost;
		}
		player.OnReceivedDamage += this.PlayerReceivedDamage;
		base.Pickup(player);
	}

	// Token: 0x060075BE RID: 30142 RVA: 0x002EE2A4 File Offset: 0x002EC4A4
	private void PlayerReceivedDamage(PlayerController obj)
	{
		this.m_receivedDamageCounter += 0.5f;
		float num = 0f;
		if (base.Owner.HasActiveBonusSynergy(CustomSynergyType.NANOMACHINES_SON, false))
		{
			num = 0.5f;
		}
		if (this.m_receivedDamageCounter >= this.DamagePerArmor - num)
		{
			this.m_receivedDamageCounter = 0f;
			this.m_owner.healthHaver.Armor += 1f;
			this.HandleRageEffect();
		}
	}

	// Token: 0x060075BF RID: 30143 RVA: 0x002EE324 File Offset: 0x002EC524
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<NanomachinesItem>().m_pickedUpThisRun = true;
		player.OnReceivedDamage -= this.PlayerReceivedDamage;
		return debrisObject;
	}

	// Token: 0x060075C0 RID: 30144 RVA: 0x002EE358 File Offset: 0x002EC558
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_owner)
		{
			this.m_owner.OnReceivedDamage -= this.PlayerReceivedDamage;
		}
	}

	// Token: 0x060075C1 RID: 30145 RVA: 0x002EE388 File Offset: 0x002EC588
	private void HandleRageEffect()
	{
		if (base.Owner.HasActiveBonusSynergy(CustomSynergyType.NANOMACHINES_SON, false))
		{
			if (this.m_rageElapsed > 0f)
			{
				this.m_rageElapsed = this.RageSynergyDuration;
				if (this.RageOverheadVFX && this.rageInstanceVFX == null)
				{
					this.rageInstanceVFX = base.Owner.PlayEffectOnActor(this.RageOverheadVFX, new Vector3(0f, 1.375f, 0f), true, true, false);
				}
			}
			else
			{
				base.Owner.StartCoroutine(this.HandleRageCooldown());
			}
		}
	}

	// Token: 0x060075C2 RID: 30146 RVA: 0x002EE42C File Offset: 0x002EC62C
	private IEnumerator HandleRageCooldown()
	{
		this.rageInstanceVFX = null;
		if (this.RageOverheadVFX)
		{
			this.rageInstanceVFX = base.Owner.PlayEffectOnActor(this.RageOverheadVFX, new Vector3(0f, 1.375f, 0f), true, true, false);
		}
		this.m_rageElapsed = this.RageSynergyDuration;
		StatModifier damageStat = new StatModifier();
		damageStat.amount = this.RageDamageMultiplier;
		damageStat.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
		damageStat.statToBoost = PlayerStats.StatType.Damage;
		PlayerController cachedOwner = base.Owner;
		cachedOwner.ownerlessStatModifiers.Add(damageStat);
		cachedOwner.stats.RecalculateStats(cachedOwner, false, false);
		Color rageColor = this.RageFlatColor;
		while (this.m_rageElapsed > 0f)
		{
			cachedOwner.baseFlatColorOverride = rageColor.WithAlpha(Mathf.Lerp(rageColor.a, 0f, 1f - Mathf.Clamp01(this.m_rageElapsed)));
			if (this.rageInstanceVFX && this.m_rageElapsed < this.RageSynergyDuration - 1f)
			{
				this.rageInstanceVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
				this.rageInstanceVFX = null;
			}
			yield return null;
			this.m_rageElapsed -= BraveTime.DeltaTime;
		}
		if (this.rageInstanceVFX)
		{
			this.rageInstanceVFX.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject("rage_face_vfx_out", null);
		}
		cachedOwner.ownerlessStatModifiers.Remove(damageStat);
		cachedOwner.stats.RecalculateStats(cachedOwner, false, false);
		yield break;
	}

	// Token: 0x0400777F RID: 30591
	public int initialArmorBoost = 2;

	// Token: 0x04007780 RID: 30592
	public float DamagePerArmor = 2f;

	// Token: 0x04007781 RID: 30593
	protected float m_receivedDamageCounter;

	// Token: 0x04007782 RID: 30594
	[Header("Nanomachines, Son")]
	public float RageSynergyDuration = 10f;

	// Token: 0x04007783 RID: 30595
	public Color RageFlatColor = Color.red;

	// Token: 0x04007784 RID: 30596
	public float RageDamageMultiplier = 2f;

	// Token: 0x04007785 RID: 30597
	public GameObject RageOverheadVFX;

	// Token: 0x04007786 RID: 30598
	private float m_rageElapsed;

	// Token: 0x04007787 RID: 30599
	private GameObject rageInstanceVFX;
}
