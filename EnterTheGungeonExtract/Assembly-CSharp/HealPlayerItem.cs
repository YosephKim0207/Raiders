using System;
using UnityEngine;

// Token: 0x02001419 RID: 5145
public class HealPlayerItem : PlayerItem
{
	// Token: 0x060074C5 RID: 29893 RVA: 0x002E7F78 File Offset: 0x002E6178
	public override bool CanBeUsed(PlayerController user)
	{
		return (!this.DoesRevive || GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER || !GameManager.Instance.PrimaryPlayer.healthHaver.IsAlive || !GameManager.Instance.SecondaryPlayer.healthHaver.IsAlive) && base.CanBeUsed(user);
	}

	// Token: 0x060074C6 RID: 29894 RVA: 0x002E7FDC File Offset: 0x002E61DC
	protected override void OnPreDrop(PlayerController user)
	{
		base.OnPreDrop(user);
		if (base.transform.childCount > 0)
		{
			SimpleSpriteRotator[] componentsInChildren = base.GetComponentsInChildren<SimpleSpriteRotator>(true);
			if (componentsInChildren.Length > 0)
			{
				componentsInChildren[0].gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x060074C7 RID: 29895 RVA: 0x002E8020 File Offset: 0x002E6220
	public override void Pickup(PlayerController player)
	{
		if (base.transform.childCount > 0)
		{
			SimpleSpriteRotator componentInChildren = base.GetComponentInChildren<SimpleSpriteRotator>();
			if (componentInChildren)
			{
				componentInChildren.gameObject.SetActive(false);
			}
		}
		base.Pickup(player);
	}

	// Token: 0x060074C8 RID: 29896 RVA: 0x002E8064 File Offset: 0x002E6264
	private void RemoveTemporaryBuff(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		this.m_buffedTarget.healthHaver.OnDamaged -= this.RemoveTemporaryBuff;
		this.m_buffedTarget.ownerlessStatModifiers.Remove(this.m_temporaryModifier);
		this.m_buffedTarget.stats.RecalculateStats(this.m_buffedTarget, false, false);
		this.m_temporaryModifier = null;
		this.m_buffedTarget = null;
	}

	// Token: 0x060074C9 RID: 29897 RVA: 0x002E80CC File Offset: 0x002E62CC
	private float GetHealingAmount(PlayerController user)
	{
		if (this.HasHealingSynergy && user.HasActiveBonusSynergy(this.HealingSynergyRequired, false))
		{
			return this.synergyHealingAmount;
		}
		return this.healingAmount;
	}

	// Token: 0x060074CA RID: 29898 RVA: 0x002E80F8 File Offset: 0x002E62F8
	protected override void DoEffect(PlayerController user)
	{
		if (this.DoesRevive && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(user);
			if (otherPlayer.healthHaver.IsDead)
			{
				otherPlayer.ResurrectFromBossKill();
			}
		}
		if (this.IsOrange)
		{
			StatModifier statModifier = new StatModifier();
			statModifier.amount = 1f;
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			statModifier.statToBoost = PlayerStats.StatType.Health;
			user.ownerlessStatModifiers.Add(statModifier);
			user.stats.RecalculateStats(user, false, false);
			AkSoundEngine.PostEvent("Play_OBJ_orange_love_01", base.gameObject);
		}
		if (this.ProvidesTemporaryDamageBuff && this.m_temporaryModifier == null)
		{
			this.m_buffedTarget = user;
			this.m_temporaryModifier = new StatModifier();
			this.m_temporaryModifier.statToBoost = PlayerStats.StatType.Damage;
			this.m_temporaryModifier.amount = this.TemporaryDamageMultiplier;
			this.m_temporaryModifier.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
			this.m_temporaryModifier.isMeatBunBuff = true;
			user.ownerlessStatModifiers.Add(this.m_temporaryModifier);
			user.stats.RecalculateStats(user, false, false);
			user.healthHaver.OnDamaged += this.RemoveTemporaryBuff;
		}
		float num = this.GetHealingAmount(user);
		if (num > 0f)
		{
			if (this.HealsBothPlayers)
			{
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					if (GameManager.Instance.AllPlayers[i].healthHaver.IsAlive)
					{
						GameManager.Instance.AllPlayers[i].healthHaver.ApplyHealing(num);
						GameManager.Instance.AllPlayers[i].PlayEffectOnActor(this.healVFX, Vector3.zero, true, false, false);
					}
				}
			}
			else
			{
				user.healthHaver.ApplyHealing(num);
				if (this.healVFX != null)
				{
					user.PlayEffectOnActor(this.healVFX, Vector3.zero, true, false, false);
				}
			}
			AkSoundEngine.PostEvent("Play_OBJ_med_kit_01", base.gameObject);
		}
	}

	// Token: 0x060074CB RID: 29899 RVA: 0x002E8300 File Offset: 0x002E6500
	private void LateUpdate()
	{
		if (this.IsOrange)
		{
			base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
			base.sprite.renderer.enabled = false;
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		}
	}

	// Token: 0x060074CC RID: 29900 RVA: 0x002E8340 File Offset: 0x002E6540
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007699 RID: 30361
	public float healingAmount = 1f;

	// Token: 0x0400769A RID: 30362
	public GameObject healVFX;

	// Token: 0x0400769B RID: 30363
	public bool HealsBothPlayers;

	// Token: 0x0400769C RID: 30364
	public bool DoesRevive;

	// Token: 0x0400769D RID: 30365
	public bool ProvidesTemporaryDamageBuff;

	// Token: 0x0400769E RID: 30366
	public float TemporaryDamageMultiplier = 2f;

	// Token: 0x0400769F RID: 30367
	public bool IsOrange;

	// Token: 0x040076A0 RID: 30368
	public bool HasHealingSynergy;

	// Token: 0x040076A1 RID: 30369
	[LongNumericEnum]
	public CustomSynergyType HealingSynergyRequired;

	// Token: 0x040076A2 RID: 30370
	[ShowInInspectorIf("HasHealingSynergy", false)]
	public float synergyHealingAmount = 5f;

	// Token: 0x040076A3 RID: 30371
	protected PlayerController m_buffedTarget;

	// Token: 0x040076A4 RID: 30372
	protected StatModifier m_temporaryModifier;
}
