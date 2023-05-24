using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020014C4 RID: 5316
public class SunglassesItem : PassiveItem
{
	// Token: 0x060078D4 RID: 30932 RVA: 0x00305064 File Offset: 0x00303264
	protected override void Update()
	{
		base.Update();
		if (this.m_owner != null && this.m_pickedUp)
		{
			if (this.m_remainingSlowTime <= 0f)
			{
				this.m_internalCooldown -= BraveTime.DeltaTime;
				BraveTime.ClearMultiplier(base.gameObject);
				SunglassesItem.SunglassesActive = false;
			}
			else
			{
				SunglassesItem.SunglassesActive = true;
				this.m_remainingSlowTime -= GameManager.INVARIANT_DELTA_TIME;
				BraveTime.SetTimeScaleMultiplier(this.timeScaleMultiplier, base.gameObject);
			}
		}
		else
		{
			BraveTime.ClearMultiplier(base.gameObject);
		}
	}

	// Token: 0x060078D5 RID: 30933 RVA: 0x00305104 File Offset: 0x00303304
	private void OnExplosion()
	{
		if (this.m_internalCooldown > 0f)
		{
			return;
		}
		this.m_internalCooldown = this.InternalCooldown;
		this.m_remainingSlowTime = this.Duration;
		if (base.Owner && base.Owner.HasActiveBonusSynergy(CustomSynergyType.MEN_IN_BLACK, false) && base.Owner.CurrentGun)
		{
			AkSoundEngine.PostEvent("Play_WPN_active_reload_01", base.gameObject);
			base.Owner.CurrentGun.ForceImmediateReload(false);
			base.Owner.CurrentGun.TriggerTemporaryBoost(this.MIBSynergyDamage, this.MIBSynergyScale, this.Duration, true);
		}
	}

	// Token: 0x060078D6 RID: 30934 RVA: 0x003051BC File Offset: 0x003033BC
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		Exploder.OnExplosionTriggered = (Action)Delegate.Combine(Exploder.OnExplosionTriggered, new Action(this.OnExplosion));
		if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
		{
			PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
		}
		if (!PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player].Add(base.GetType(), 1);
		}
		else
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = PassiveItem.ActiveFlagItems[player][base.GetType()] + 1;
		}
		base.Pickup(player);
	}

	// Token: 0x060078D7 RID: 30935 RVA: 0x00305284 File Offset: 0x00303484
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		Exploder.OnExplosionTriggered = (Action)Delegate.Remove(Exploder.OnExplosionTriggered, new Action(this.OnExplosion));
		BraveTime.ClearMultiplier(base.gameObject);
		if (PassiveItem.ActiveFlagItems.ContainsKey(player) && PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
			}
		}
		debrisObject.GetComponent<SunglassesItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x060078D8 RID: 30936 RVA: 0x00305364 File Offset: 0x00303564
	protected override void OnDestroy()
	{
		SunglassesItem.SunglassesActive = false;
		BraveTime.ClearMultiplier(base.gameObject);
		Exploder.OnExplosionTriggered = (Action)Delegate.Remove(Exploder.OnExplosionTriggered, new Action(this.OnExplosion));
		BraveTime.ClearMultiplier(base.gameObject);
		if (this.m_pickedUp && PassiveItem.ActiveFlagItems.ContainsKey(this.m_owner) && PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[this.m_owner].Remove(base.GetType());
			}
		}
		base.OnDestroy();
	}

	// Token: 0x04007B19 RID: 31513
	public static bool SunglassesActive;

	// Token: 0x04007B1A RID: 31514
	public float timeScaleMultiplier = 0.25f;

	// Token: 0x04007B1B RID: 31515
	public float Duration = 3f;

	// Token: 0x04007B1C RID: 31516
	public float InternalCooldown = 5f;

	// Token: 0x04007B1D RID: 31517
	private float m_remainingSlowTime;

	// Token: 0x04007B1E RID: 31518
	private float m_internalCooldown;

	// Token: 0x04007B1F RID: 31519
	public float MIBSynergyScale = 1.33f;

	// Token: 0x04007B20 RID: 31520
	public float MIBSynergyDamage = 1.8f;
}
