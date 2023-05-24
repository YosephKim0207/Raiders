using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001437 RID: 5175
public class MetronomeItem : PassiveItem
{
	// Token: 0x17001191 RID: 4497
	// (get) Token: 0x06007571 RID: 30065 RVA: 0x002EC674 File Offset: 0x002EA874
	private float ModifiedBoost
	{
		get
		{
			if (base.Owner && base.Owner.HasActiveBonusSynergy(CustomSynergyType.KEEPING_THE_BEAT, false))
			{
				return this.damageBoostPerKillSynergy;
			}
			return this.damageBoostPerKill;
		}
	}

	// Token: 0x17001192 RID: 4498
	// (get) Token: 0x06007572 RID: 30066 RVA: 0x002EC6AC File Offset: 0x002EA8AC
	private float ModifiedCap
	{
		get
		{
			if (base.Owner && base.Owner.HasActiveBonusSynergy(CustomSynergyType.KEEPING_THE_BEAT, false))
			{
				return this.synergyMultiplierCap;
			}
			return this.damageMultiplierCap;
		}
	}

	// Token: 0x17001193 RID: 4499
	// (get) Token: 0x06007573 RID: 30067 RVA: 0x002EC6E4 File Offset: 0x002EA8E4
	private Gradient ModifiedGradient
	{
		get
		{
			if (base.Owner && base.Owner.HasActiveBonusSynergy(CustomSynergyType.KEEPING_THE_BEAT, false))
			{
				return this.synergyColorGradient;
			}
			return this.colorGradient;
		}
	}

	// Token: 0x06007574 RID: 30068 RVA: 0x002EC71C File Offset: 0x002EA91C
	public override void MidGameSerialize(List<object> data)
	{
		base.MidGameSerialize(data);
		if (this.m_cachedGunReference != null)
		{
			data.Add(this.m_cachedGunReference.PickupObjectId);
			data.Add(this.m_sequentialKills);
		}
	}

	// Token: 0x06007575 RID: 30069 RVA: 0x002EC768 File Offset: 0x002EA968
	public override void MidGameDeserialize(List<object> data)
	{
		base.MidGameDeserialize(data);
		if (this.m_player && this.m_player.inventory != null && data.Count == 2)
		{
			this.m_sequentialKills = (int)data[1];
			int num = (int)data[0];
			for (int i = 0; i < this.m_player.inventory.AllGuns.Count; i++)
			{
				if (this.m_player.inventory.AllGuns[i] && this.m_player.inventory.AllGuns[i].PickupObjectId == num)
				{
					this.m_cachedGunReference = this.m_player.inventory.AllGuns[i];
				}
			}
		}
	}

	// Token: 0x06007576 RID: 30070 RVA: 0x002EC84C File Offset: 0x002EAA4C
	public float GetCurrentMultiplier()
	{
		return Mathf.Clamp(1f + (float)this.m_sequentialKills * this.ModifiedBoost, 0f, this.ModifiedCap);
	}

	// Token: 0x06007577 RID: 30071 RVA: 0x002EC874 File Offset: 0x002EAA74
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		this.m_player = player;
		player.OnKilledEnemy += this.OnKilledEnemy;
		player.GunChanged += this.OnGunChanged;
		player.healthHaver.OnDamaged += this.OnReceivedDamage;
		this.m_cachedGunReference = player.CurrentGun;
	}

	// Token: 0x06007578 RID: 30072 RVA: 0x002EC8E4 File Offset: 0x002EAAE4
	private void OnGunChanged(Gun old, Gun current, bool newGun)
	{
		bool flag = false;
		if (this.m_player && this.m_player.CharacterUsesRandomGuns)
		{
			flag = true;
		}
		bool flag2 = false;
		if (this.m_player && this.m_player.inventory != null && this.m_player.inventory.GunChangeForgiveness)
		{
			flag2 = true;
		}
		if (old != current && !newGun && !flag && !flag2)
		{
			this.DoMetronomeBroken(current);
		}
		this.m_cachedGunReference = current;
	}

	// Token: 0x06007579 RID: 30073 RVA: 0x002EC97C File Offset: 0x002EAB7C
	private void DoMetronomeUp()
	{
		this.m_sequentialKills++;
		this.m_player.stats.RecalculateStats(this.m_player, false, false);
		AkSoundEngine.SetRTPCValue("Pitch_Metronome", (float)this.m_sequentialKills);
		AkSoundEngine.PostEvent("Play_OBJ_metronome_jingle_01", this.m_player.gameObject);
		float currentMultiplier = this.GetCurrentMultiplier();
		float num = Mathf.InverseLerp(1f, this.ModifiedCap, currentMultiplier);
		Color color = this.ModifiedGradient.Evaluate(num);
		if (currentMultiplier >= 2f)
		{
			this.m_player.BloopItemAboveHead(this.doubleEighthNoteSprite, string.Empty, color, false);
		}
		else
		{
			this.m_player.BloopItemAboveHead(this.eighthNoteSprite, string.Empty, color, false);
		}
	}

	// Token: 0x0600757A RID: 30074 RVA: 0x002ECA40 File Offset: 0x002EAC40
	private void DoMetronomeBroken(Gun current)
	{
		float currentMultiplier = this.GetCurrentMultiplier();
		if (currentMultiplier > 1f)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metronome_fail_01", this.m_player.gameObject);
			float num = Mathf.InverseLerp(1f, this.ModifiedCap, currentMultiplier);
			Color color = this.ModifiedGradient.Evaluate(num);
			GameObject gameObject = this.m_player.PlayEffectOnActor((currentMultiplier < 2f) ? this.eighthNoteSprite.gameObject : this.doubleEighthNoteSprite.gameObject, Vector3.up * 1.5f, true, false, false);
			gameObject.GetComponent<tk2dBaseSprite>().color = color;
		}
		AkSoundEngine.SetRTPCValue("Pitch_Metronome", 0f);
		this.m_sequentialKills = 0;
		this.m_cachedGunReference = current;
		this.m_player.stats.RecalculateStats(this.m_player, false, false);
	}

	// Token: 0x0600757B RID: 30075 RVA: 0x002ECB1C File Offset: 0x002EAD1C
	private void OnReceivedDamage(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		this.DoMetronomeBroken(this.m_cachedGunReference);
	}

	// Token: 0x0600757C RID: 30076 RVA: 0x002ECB2C File Offset: 0x002EAD2C
	private void OnKilledEnemy(PlayerController source)
	{
		if (source.CurrentGun != this.m_cachedGunReference)
		{
			this.DoMetronomeBroken(source.CurrentGun);
		}
		this.DoMetronomeUp();
	}

	// Token: 0x0600757D RID: 30077 RVA: 0x002ECB58 File Offset: 0x002EAD58
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		player.OnKilledEnemy -= this.OnKilledEnemy;
		player.GunChanged -= this.OnGunChanged;
		player.healthHaver.OnDamaged -= this.OnReceivedDamage;
		debrisObject.GetComponent<MetronomeItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x0600757E RID: 30078 RVA: 0x002ECBB8 File Offset: 0x002EADB8
	protected override void OnDestroy()
	{
		if (this.m_owner)
		{
			this.m_owner.OnKilledEnemy -= this.OnKilledEnemy;
			this.m_owner.GunChanged -= this.OnGunChanged;
			this.m_owner.healthHaver.OnDamaged -= this.OnReceivedDamage;
		}
		base.OnDestroy();
	}

	// Token: 0x04007753 RID: 30547
	public float damageBoostPerKill = 0.05f;

	// Token: 0x04007754 RID: 30548
	public float damageBoostPerKillSynergy = 0.04f;

	// Token: 0x04007755 RID: 30549
	public float damageMultiplierCap = 3f;

	// Token: 0x04007756 RID: 30550
	public float synergyMultiplierCap = 5f;

	// Token: 0x04007757 RID: 30551
	public tk2dSprite eighthNoteSprite;

	// Token: 0x04007758 RID: 30552
	public tk2dSprite doubleEighthNoteSprite;

	// Token: 0x04007759 RID: 30553
	public Gradient colorGradient;

	// Token: 0x0400775A RID: 30554
	public Gradient synergyColorGradient;

	// Token: 0x0400775B RID: 30555
	[NonSerialized]
	private Gun m_cachedGunReference;

	// Token: 0x0400775C RID: 30556
	[NonSerialized]
	private int m_sequentialKills;

	// Token: 0x0400775D RID: 30557
	[NonSerialized]
	private PlayerController m_player;
}
