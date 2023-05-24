using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02001329 RID: 4905
public class GunInventory
{
	// Token: 0x06006F0D RID: 28429 RVA: 0x002C090C File Offset: 0x002BEB0C
	public GunInventory(GameActor owner)
	{
		this.m_owner = owner;
		this.m_guns = new List<Gun>();
		this.m_perGunDrainData = new List<float>();
		this.m_gunStowedTime = new Dictionary<Gun, float>();
	}

	// Token: 0x170010D6 RID: 4310
	// (get) Token: 0x06006F0E RID: 28430 RVA: 0x002C0974 File Offset: 0x002BEB74
	public Gun CurrentGun
	{
		get
		{
			if (this.ForceNoGun)
			{
				return null;
			}
			return this.m_currentGun;
		}
	}

	// Token: 0x170010D7 RID: 4311
	// (get) Token: 0x06006F0F RID: 28431 RVA: 0x002C098C File Offset: 0x002BEB8C
	public Gun CurrentSecondaryGun
	{
		get
		{
			if (!this.DualWielding)
			{
				return null;
			}
			if (this.ForceNoGun)
			{
				return null;
			}
			return this.m_currentSecondaryGun;
		}
	}

	// Token: 0x140000B1 RID: 177
	// (add) Token: 0x06006F10 RID: 28432 RVA: 0x002C09B0 File Offset: 0x002BEBB0
	// (remove) Token: 0x06006F11 RID: 28433 RVA: 0x002C09E8 File Offset: 0x002BEBE8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event GunInventory.OnGunChangedEvent OnGunChanged;

	// Token: 0x170010D8 RID: 4312
	// (get) Token: 0x06006F12 RID: 28434 RVA: 0x002C0A20 File Offset: 0x002BEC20
	public GameActor Owner
	{
		get
		{
			return this.m_owner;
		}
	}

	// Token: 0x06006F13 RID: 28435 RVA: 0x002C0A28 File Offset: 0x002BEC28
	public void SetDualWielding(bool value, string reason)
	{
		bool value2 = this.m_dualWielding.Value;
		Gun gun = ((!value2) ? null : this.CurrentSecondaryGun);
		this.m_dualWielding.SetOverride(reason, value, null);
		if (value2 && !this.m_dualWielding.Value && gun)
		{
			if (!gun.IsPreppedForThrow)
			{
				gun.CeaseAttack(false, null);
			}
			gun.OnPrePlayerChange();
			gun.gameObject.SetActive(false);
		}
	}

	// Token: 0x170010D9 RID: 4313
	// (get) Token: 0x06006F14 RID: 28436 RVA: 0x002C0AB4 File Offset: 0x002BECB4
	public bool DualWielding
	{
		get
		{
			return this.m_dualWielding.Value;
		}
	}

	// Token: 0x170010DA RID: 4314
	// (get) Token: 0x06006F15 RID: 28437 RVA: 0x002C0AC4 File Offset: 0x002BECC4
	public List<Gun> AllGuns
	{
		get
		{
			return this.m_guns;
		}
	}

	// Token: 0x170010DB RID: 4315
	// (get) Token: 0x06006F16 RID: 28438 RVA: 0x002C0ACC File Offset: 0x002BECCC
	public int GunCountModified
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.m_guns.Count; i++)
			{
				if (!this.m_guns[i].name.StartsWith("ArtfulDodger"))
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x170010DC RID: 4316
	// (get) Token: 0x06006F17 RID: 28439 RVA: 0x002C0B24 File Offset: 0x002BED24
	// (set) Token: 0x06006F18 RID: 28440 RVA: 0x002C0B2C File Offset: 0x002BED2C
	public int maxGuns
	{
		get
		{
			return this.m_maxGuns;
		}
		set
		{
			this.m_maxGuns = value;
		}
	}

	// Token: 0x170010DD RID: 4317
	// (get) Token: 0x06006F19 RID: 28441 RVA: 0x002C0B38 File Offset: 0x002BED38
	// (set) Token: 0x06006F1A RID: 28442 RVA: 0x002C0B40 File Offset: 0x002BED40
	public bool ForceNoGun { get; set; }

	// Token: 0x06006F1B RID: 28443 RVA: 0x002C0B4C File Offset: 0x002BED4C
	public bool ContainsGun(int gunID)
	{
		for (int i = 0; i < this.m_guns.Count; i++)
		{
			if (this.m_guns[i].PickupObjectId == gunID)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006F1C RID: 28444 RVA: 0x002C0B90 File Offset: 0x002BED90
	public int ContainsGunOfClass(GunClass targetClass, bool respectsOverrides)
	{
		int num = 0;
		if (respectsOverrides && this.m_gunClassOverrides.Contains(targetClass))
		{
			return 0;
		}
		for (int i = 0; i < this.m_guns.Count; i++)
		{
			if (this.m_guns[i].gunClass == targetClass)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06006F1D RID: 28445 RVA: 0x002C0BF0 File Offset: 0x002BEDF0
	public void RegisterGunClassOverride(GunClass overridden)
	{
		if (!this.m_gunClassOverrides.Contains(overridden))
		{
			this.m_gunClassOverrides.Add(overridden);
		}
	}

	// Token: 0x06006F1E RID: 28446 RVA: 0x002C0C10 File Offset: 0x002BEE10
	public void DeregisterGunClassOverride(GunClass overridden)
	{
		this.m_gunClassOverrides.Remove(overridden);
	}

	// Token: 0x06006F1F RID: 28447 RVA: 0x002C0C20 File Offset: 0x002BEE20
	public void HandleAmmoDrain(float percentAmmoDrain)
	{
		for (int i = 0; i < this.m_guns.Count; i++)
		{
			if (this.m_guns[i].AdjustedMaxAmmo > 0)
			{
				if (this.m_guns[i].ammo > 0)
				{
					this.m_perGunDrainData[i] = this.m_perGunDrainData[i] + percentAmmoDrain;
					int num = Mathf.FloorToInt((float)this.m_guns[i].AdjustedMaxAmmo * this.m_perGunDrainData[i]);
					if (num >= 1)
					{
						float num2 = (float)num / (float)this.m_guns[i].AdjustedMaxAmmo;
						this.m_perGunDrainData[i] = this.m_perGunDrainData[i] - num2;
						this.m_guns[i].LoseAmmo(num);
					}
				}
			}
		}
	}

	// Token: 0x06006F20 RID: 28448 RVA: 0x002C0D0C File Offset: 0x002BEF0C
	public void ClearAmmoDrain()
	{
		for (int i = 0; i < this.m_guns.Count; i++)
		{
			this.m_perGunDrainData[i] = 0f;
		}
	}

	// Token: 0x06006F21 RID: 28449 RVA: 0x002C0D48 File Offset: 0x002BEF48
	public void FrameUpdate()
	{
		for (int i = 0; i < this.AllGuns.Count; i++)
		{
			if (this.AllGuns[i] == this.CurrentGun)
			{
				this.m_gunStowedTime[this.AllGuns[i]] = 0f;
			}
			else
			{
				Dictionary<Gun, float> gunStowedTime;
				Gun gun;
				(gunStowedTime = this.m_gunStowedTime)[gun = this.AllGuns[i]] = gunStowedTime[gun] + BraveTime.DeltaTime;
				if (this.m_gunStowedTime[this.AllGuns[i]] > 2f * this.AllGuns[i].reloadTime)
				{
					this.AllGuns[i].ForceImmediateReload(false);
					this.m_gunStowedTime[this.AllGuns[i]] = -1000f;
				}
			}
		}
	}

	// Token: 0x06006F22 RID: 28450 RVA: 0x002C0E38 File Offset: 0x002BF038
	public Gun AddGunToInventory(Gun gun, bool makeActive = false)
	{
		if (gun && gun.ShouldBeDestroyedOnExistence(!(this.m_owner is PlayerController)))
		{
			return null;
		}
		Gun ownedCopy = this.GetOwnedCopy(gun);
		if (ownedCopy != null)
		{
			ownedCopy.GainAmmo(gun);
			return ownedCopy;
		}
		if (!gun.name.StartsWith("ArtfulDodger") && this.maxGuns > 0 && this.GunCountModified >= this.maxGuns)
		{
			if (!(this.m_owner is PlayerController))
			{
				return null;
			}
			Gun currentGun = this.m_owner.CurrentGun;
			this.RemoveGunFromInventory(currentGun);
			currentGun.DropGun(0.5f);
		}
		Gun gun2 = this.CreateGunForAdd(gun);
		gun2.HasBeenPickedUp = true;
		gun2.HasProcessedStatMods = gun.HasProcessedStatMods;
		gun2.CopyStateFrom(gun);
		this.m_guns.Add(gun2);
		this.m_perGunDrainData.Add(0f);
		this.m_gunStowedTime.Add(gun2, 0f);
		if (this.m_guns.Count == 1)
		{
			this.m_currentGun = this.m_guns[0];
			this.ChangeGun(0, true, false);
		}
		if (makeActive)
		{
			int num = this.m_guns.Count - 1 - this.m_guns.IndexOf(this.m_currentGun);
			this.ChangeGun(num, true, false);
			gun2.HandleSpriteFlip(this.m_owner.SpriteFlipped);
		}
		if (this.m_owner is PlayerController)
		{
			(this.m_owner as PlayerController).stats.RecalculateStats(this.m_owner as PlayerController, false, false);
		}
		return gun2;
	}

	// Token: 0x06006F23 RID: 28451 RVA: 0x002C0FE4 File Offset: 0x002BF1E4
	public Gun GetTargetGunWithChange(int amt)
	{
		if (this.m_guns.Count == 0)
		{
			return null;
		}
		int i = this.m_guns.IndexOf(this.m_currentGun);
		for (i += amt; i < 0; i += this.m_guns.Count)
		{
		}
		i %= this.m_guns.Count;
		return this.m_guns[i];
	}

	// Token: 0x06006F24 RID: 28452 RVA: 0x002C1050 File Offset: 0x002BF250
	public void SwapDualGuns()
	{
		if (!this.DualWielding)
		{
			return;
		}
		if (this.m_currentSecondaryGun && this.m_currentGun)
		{
			Gun currentGun = this.m_currentGun;
			Gun currentSecondaryGun = this.m_currentSecondaryGun;
			this.m_currentGun = this.m_currentSecondaryGun;
			this.m_currentSecondaryGun = currentGun;
			this.m_currentGun.OnEnable();
			this.m_currentSecondaryGun.OnEnable();
			this.m_currentGun.HandleSpriteFlip(this.m_currentGun.CurrentOwner.SpriteFlipped);
			this.m_currentSecondaryGun.HandleSpriteFlip(this.m_currentSecondaryGun.CurrentOwner.SpriteFlipped);
			if (this.OnGunChanged != null)
			{
				this.OnGunChanged(currentGun, this.m_currentGun, currentSecondaryGun, this.CurrentSecondaryGun, false);
			}
		}
	}

	// Token: 0x06006F25 RID: 28453 RVA: 0x002C111C File Offset: 0x002BF31C
	public void ChangeGun(int amt, bool newGun = false, bool overrideGunLock = false)
	{
		if (this.m_guns.Count == 0)
		{
			return;
		}
		if (this.m_currentGun != null && this.m_currentGun.UnswitchableGun)
		{
			return;
		}
		if (this.GunLocked.Value && !overrideGunLock)
		{
			return;
		}
		Gun currentGun = this.m_currentGun;
		Gun currentSecondaryGun = this.m_currentSecondaryGun;
		if (this.m_currentGun != null && !this.ForceNoGun)
		{
			if (!this.m_currentGun.IsPreppedForThrow)
			{
				this.CurrentGun.CeaseAttack(false, null);
			}
			this.m_currentGun.OnPrePlayerChange();
			this.m_currentGun.gameObject.SetActive(false);
		}
		if (this.DualWielding && this.CurrentSecondaryGun)
		{
			if (!this.CurrentSecondaryGun.IsPreppedForThrow)
			{
				this.CurrentSecondaryGun.CeaseAttack(false, null);
			}
			this.CurrentSecondaryGun.OnPrePlayerChange();
			this.CurrentSecondaryGun.gameObject.SetActive(false);
		}
		int i = this.m_guns.IndexOf(this.m_currentGun);
		for (i += amt; i < 0; i += this.m_guns.Count)
		{
		}
		i %= this.m_guns.Count;
		this.m_currentGun = this.m_guns[i];
		this.m_currentGun.gameObject.SetActive(true);
		if (this.DualWielding)
		{
			if (this.m_guns.Count <= 1)
			{
				this.m_currentSecondaryGun = null;
			}
			if ((this.m_currentSecondaryGun == null || this.m_currentSecondaryGun == this.m_currentGun) && this.m_guns.Count > 1)
			{
				this.m_currentSecondaryGun = this.m_guns[(i + 1) % this.m_guns.Count];
			}
			if (this.CurrentSecondaryGun)
			{
				this.CurrentSecondaryGun.gameObject.SetActive(true);
			}
		}
		if (this.OnGunChanged != null)
		{
			this.OnGunChanged(currentGun, this.m_currentGun, currentSecondaryGun, this.CurrentSecondaryGun, newGun);
		}
	}

	// Token: 0x06006F26 RID: 28454 RVA: 0x002C1350 File Offset: 0x002BF550
	public Gun CreateGunForAdd(Gun gunPrototype)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(gunPrototype.gameObject);
		gameObject.name = gunPrototype.name;
		Gun component = gameObject.GetComponent<Gun>();
		if (!component.enabled)
		{
			component.enabled = true;
		}
		component.prefabName = ((!(gunPrototype.prefabName == string.Empty)) ? gunPrototype.prefabName : gunPrototype.name);
		Transform gunPivot = this.m_owner.GunPivot;
		IGunInheritable[] interfaces = gameObject.GetInterfaces<IGunInheritable>();
		if (interfaces != null)
		{
			for (int i = 0; i < interfaces.Length; i++)
			{
				interfaces[i].InheritData(gunPrototype);
			}
		}
		gameObject.transform.parent = gunPivot;
		if (component.PrimaryHandAttachPoint != null)
		{
			gameObject.transform.localPosition = -component.PrimaryHandAttachPoint.localPosition;
		}
		gameObject.SetActive(false);
		component.Initialize(this.m_owner);
		if (!gunPrototype.HasBeenPickedUp && gunPrototype.ArmorToGainOnPickup > 0)
		{
			this.m_owner.healthHaver.Armor += (float)gunPrototype.ArmorToGainOnPickup;
		}
		if (!gunPrototype.HasBeenPickedUp && !component.InfiniteAmmo)
		{
			float num = (float)component.AdjustedMaxAmmo / (float)component.GetBaseMaxAmmo();
			int num2 = Mathf.CeilToInt(num * (float)component.ammo);
			if (num2 > component.ammo)
			{
				component.GainAmmo(num2 - component.ammo);
			}
			else if (num2 < component.ammo)
			{
				component.LoseAmmo(component.ammo - num2);
			}
		}
		if (component && component.DefaultModule != null && this.m_owner && this.m_owner is AIActor && component.DefaultModule.shootStyle == ProjectileModule.ShootStyle.Charged)
		{
			component.DefaultModule.projectiles = new List<Projectile>();
			component.DefaultModule.projectiles.Add(component.DefaultModule.GetChargeProjectile(1000f).Projectile);
			component.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
		}
		return component;
	}

	// Token: 0x06006F27 RID: 28455 RVA: 0x002C1574 File Offset: 0x002BF774
	public void DestroyGun(Gun g)
	{
		this.RemoveGunFromInventory(g);
		UnityEngine.Object.Destroy(g.gameObject);
	}

	// Token: 0x06006F28 RID: 28456 RVA: 0x002C1588 File Offset: 0x002BF788
	public void DestroyCurrentGun()
	{
		Gun currentGun = this.m_currentGun;
		if (currentGun != null)
		{
			this.RemoveGunFromInventory(currentGun);
			UnityEngine.Object.Destroy(currentGun.gameObject);
		}
	}

	// Token: 0x06006F29 RID: 28457 RVA: 0x002C15BC File Offset: 0x002BF7BC
	public void DestroyAllGuns()
	{
		for (int i = 0; i < this.m_guns.Count; i++)
		{
			Gun gun = this.m_guns[i];
			this.RemoveGunFromInventory(gun);
			UnityEngine.Object.Destroy(gun.gameObject);
			i--;
		}
		this.GunLocked.ClearOverrides();
	}

	// Token: 0x06006F2A RID: 28458 RVA: 0x002C1614 File Offset: 0x002BF814
	public void RemoveGunFromInventory(Gun gun)
	{
		Gun ownedCopy = this.GetOwnedCopy(gun);
		if (ownedCopy == null)
		{
			UnityEngine.Debug.Log("Removing unknown gun " + gun.gunName + " from player inventory!");
			return;
		}
		bool flag = (ownedCopy == this.CurrentGun || ownedCopy == this.CurrentSecondaryGun) && this.DualWielding;
		bool flag2 = flag && ownedCopy == this.CurrentGun;
		int num = this.m_guns.IndexOf(ownedCopy);
		int num2 = this.m_guns.IndexOf(this.m_currentGun);
		if (flag)
		{
			if (flag2)
			{
				this.m_currentGun = this.m_currentSecondaryGun;
				this.m_currentSecondaryGun = null;
				this.m_currentGun.OnEnable();
				this.m_dualWielding.ClearOverrides();
				this.ChangeGun(0, false, false);
			}
		}
		else if (num == num2 && this.m_guns.Count > 1)
		{
			this.ChangeGun(-1, false, true);
		}
		else if (num == num2)
		{
			this.m_currentGun = null;
		}
		this.m_guns.RemoveAt(num);
		this.m_perGunDrainData.RemoveAt(num);
		this.m_gunStowedTime.Remove(ownedCopy);
		if (this.m_owner is PlayerController)
		{
			(this.m_owner as PlayerController).stats.RecalculateStats(this.m_owner as PlayerController, false, false);
		}
	}

	// Token: 0x06006F2B RID: 28459 RVA: 0x002C1788 File Offset: 0x002BF988
	private Gun GetOwnedCopy(Gun w)
	{
		Gun gun = null;
		for (int i = 0; i < this.m_guns.Count; i++)
		{
			if (this.m_guns[i].PickupObjectId == w.PickupObjectId)
			{
				gun = this.m_guns[i];
				break;
			}
		}
		return gun;
	}

	// Token: 0x04006E89 RID: 28297
	public bool GunChangeForgiveness;

	// Token: 0x04006E8A RID: 28298
	public List<GunClass> m_gunClassOverrides = new List<GunClass>();

	// Token: 0x04006E8B RID: 28299
	private GameActor m_owner;

	// Token: 0x04006E8C RID: 28300
	private Gun m_currentGun;

	// Token: 0x04006E8D RID: 28301
	private OverridableBool m_dualWielding = new OverridableBool(false);

	// Token: 0x04006E8E RID: 28302
	private Gun m_currentSecondaryGun;

	// Token: 0x04006E8F RID: 28303
	private int m_maxGuns = -1;

	// Token: 0x04006E91 RID: 28305
	public OverridableBool GunLocked = new OverridableBool(false);

	// Token: 0x04006E92 RID: 28306
	private List<Gun> m_guns;

	// Token: 0x04006E93 RID: 28307
	private Dictionary<Gun, float> m_gunStowedTime;

	// Token: 0x04006E94 RID: 28308
	private List<float> m_perGunDrainData;

	// Token: 0x0200132A RID: 4906
	// (Invoke) Token: 0x06006F2D RID: 28461
	public delegate void OnGunChangedEvent(Gun previous, Gun current, Gun previousSecondary, Gun currentSecondary, bool newGun);
}
