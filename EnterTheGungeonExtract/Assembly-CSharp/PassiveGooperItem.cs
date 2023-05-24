using System;
using UnityEngine;

// Token: 0x02001459 RID: 5209
public class PassiveGooperItem : PassiveItem
{
	// Token: 0x0600764A RID: 30282 RVA: 0x002F118C File Offset: 0x002EF38C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_cachedGoopType = this.goopType;
		base.Pickup(player);
		if (this.TranslatesGleepGlorp)
		{
			player.UnderstandsGleepGlorp = true;
		}
		if (this.condition == PassiveGooperItem.Condition.WhileDodgeRolling)
		{
			player.OnIsRolling += this.OnRollFrame;
		}
		else if (this.condition == PassiveGooperItem.Condition.OnDamaged)
		{
			player.OnReceivedDamage += this.HandleReceivedDamage;
		}
		this.m_player = player;
		for (int i = 0; i < this.modifiers.Length; i++)
		{
			player.healthHaver.damageTypeModifiers.Add(this.modifiers[i]);
		}
	}

	// Token: 0x0600764B RID: 30283 RVA: 0x002F1244 File Offset: 0x002EF444
	protected override void Update()
	{
		base.Update();
		if (this.m_pickedUp && this.m_player != null && !GameManager.Instance.IsLoadingLevel)
		{
			if (this.condition == PassiveGooperItem.Condition.Always)
			{
				this.DoGoop();
			}
			for (int i = 0; i < this.Synergies.Length; i++)
			{
				if (!this.Synergies[i].m_processed && this.m_player.HasActiveBonusSynergy(this.Synergies[i].RequiredSynergy, false))
				{
					this.Synergies[i].m_processed = true;
					this.goopType = this.Synergies[i].overrideGoopType;
					this.m_player.healthHaver.damageTypeModifiers.Add(this.Synergies[i].AdditionalDamageModifier);
				}
				else if (this.Synergies[i].m_processed && !this.m_player.HasActiveBonusSynergy(this.Synergies[i].RequiredSynergy, false))
				{
					this.Synergies[i].m_processed = false;
					this.goopType = this.m_cachedGoopType;
					this.m_player.healthHaver.damageTypeModifiers.Remove(this.Synergies[i].AdditionalDamageModifier);
				}
			}
		}
	}

	// Token: 0x0600764C RID: 30284 RVA: 0x002F1394 File Offset: 0x002EF594
	private void DoGoop()
	{
		if (this.IsDegooperator)
		{
			if (base.Owner && base.Owner.HasActiveBonusSynergy(CustomSynergyType.AIR_SOFT, false) && base.Owner.CurrentGun)
			{
				int num = DeadlyDeadlyGoopManager.CountGoopsInRadius(this.m_player.specRigidbody.UnitCenter, this.goopRadius);
				if (num > 0)
				{
					this.m_synergyAccumAmmo += (float)num * this.AirSoftSynergyAmmoGainRate;
					if (this.m_synergyAccumAmmo > 1f)
					{
						base.Owner.CurrentGun.GainAmmo(Mathf.FloorToInt(this.m_synergyAccumAmmo));
						this.m_synergyAccumAmmo -= (float)Mathf.FloorToInt(this.m_synergyAccumAmmo);
					}
				}
			}
			DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(this.m_player.specRigidbody.UnitCenter, this.goopRadius);
		}
		else if (this.condition == PassiveGooperItem.Condition.OnDamaged)
		{
			DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopType).TimedAddGoopCircle(this.m_player.specRigidbody.UnitCenter, this.goopRadius, 0.5f, false);
		}
		else
		{
			DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopType).AddGoopCircle(this.m_player.specRigidbody.UnitCenter, this.goopRadius, -1, false, -1);
		}
	}

	// Token: 0x0600764D RID: 30285 RVA: 0x002F14EC File Offset: 0x002EF6EC
	private void HandleReceivedDamage(PlayerController obj)
	{
		this.DoGoop();
	}

	// Token: 0x0600764E RID: 30286 RVA: 0x002F14F4 File Offset: 0x002EF6F4
	private void OnRollFrame(PlayerController obj)
	{
		this.DoGoop();
	}

	// Token: 0x0600764F RID: 30287 RVA: 0x002F14FC File Offset: 0x002EF6FC
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<PassiveGooperItem>().m_pickedUpThisRun = true;
		for (int i = 0; i < this.modifiers.Length; i++)
		{
			player.healthHaver.damageTypeModifiers.Remove(this.modifiers[i]);
		}
		if (this.condition == PassiveGooperItem.Condition.WhileDodgeRolling)
		{
			player.OnIsRolling -= this.OnRollFrame;
		}
		else if (this.condition == PassiveGooperItem.Condition.OnDamaged)
		{
			player.OnReceivedDamage -= this.HandleReceivedDamage;
		}
		return debrisObject;
	}

	// Token: 0x06007650 RID: 30288 RVA: 0x002F1590 File Offset: 0x002EF790
	protected override void OnDestroy()
	{
		if (this.m_player != null)
		{
			if (this.condition == PassiveGooperItem.Condition.WhileDodgeRolling)
			{
				this.m_player.OnIsRolling -= this.OnRollFrame;
			}
			else if (this.condition == PassiveGooperItem.Condition.OnDamaged)
			{
				this.m_player.OnReceivedDamage -= this.HandleReceivedDamage;
			}
			for (int i = 0; i < this.modifiers.Length; i++)
			{
				this.m_player.healthHaver.damageTypeModifiers.Remove(this.modifiers[i]);
			}
		}
		base.OnDestroy();
	}

	// Token: 0x04007828 RID: 30760
	public PassiveGooperItem.Condition condition;

	// Token: 0x04007829 RID: 30761
	public bool IsDegooperator;

	// Token: 0x0400782A RID: 30762
	public bool TranslatesGleepGlorp;

	// Token: 0x0400782B RID: 30763
	public GoopDefinition goopType;

	// Token: 0x0400782C RID: 30764
	public float goopRadius;

	// Token: 0x0400782D RID: 30765
	public DamageTypeModifier[] modifiers;

	// Token: 0x0400782E RID: 30766
	public PassiveGooperSynergy[] Synergies;

	// Token: 0x0400782F RID: 30767
	public float AirSoftSynergyAmmoGainRate = 0.05f;

	// Token: 0x04007830 RID: 30768
	private GoopDefinition m_cachedGoopType;

	// Token: 0x04007831 RID: 30769
	private float m_synergyAccumAmmo;

	// Token: 0x04007832 RID: 30770
	private PlayerController m_player;

	// Token: 0x0200145A RID: 5210
	public enum Condition
	{
		// Token: 0x04007834 RID: 30772
		WhileDodgeRolling,
		// Token: 0x04007835 RID: 30773
		Always,
		// Token: 0x04007836 RID: 30774
		OnDamaged
	}
}
