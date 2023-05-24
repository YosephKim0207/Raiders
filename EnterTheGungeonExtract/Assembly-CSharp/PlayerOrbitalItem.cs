using System;
using UnityEngine;

// Token: 0x02001466 RID: 5222
public class PlayerOrbitalItem : PassiveItem
{
	// Token: 0x060076B7 RID: 30391 RVA: 0x002F5300 File Offset: 0x002F3500
	public static GameObject CreateOrbital(PlayerController owner, GameObject targetOrbitalPrefab, bool isFollower, PlayerOrbitalItem sourceItem = null)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(targetOrbitalPrefab, owner.transform.position, Quaternion.identity);
		if (!isFollower)
		{
			PlayerOrbital component = gameObject.GetComponent<PlayerOrbital>();
			component.Initialize(owner);
			component.SourceItem = sourceItem;
		}
		else
		{
			PlayerOrbitalFollower component2 = gameObject.GetComponent<PlayerOrbitalFollower>();
			if (component2)
			{
				component2.Initialize(owner);
			}
		}
		return gameObject;
	}

	// Token: 0x060076B8 RID: 30392 RVA: 0x002F5360 File Offset: 0x002F3560
	private void CreateOrbital(PlayerController owner)
	{
		GameObject gameObject = ((!(this.OrbitalPrefab != null)) ? this.OrbitalFollowerPrefab.gameObject : this.OrbitalPrefab.gameObject);
		if (this.HasUpgradeSynergy && this.m_synergyUpgradeActive)
		{
			gameObject = ((!(this.UpgradeOrbitalPrefab != null)) ? this.UpgradeOrbitalFollowerPrefab.gameObject : this.UpgradeOrbitalPrefab.gameObject);
		}
		this.m_extantOrbital = PlayerOrbitalItem.CreateOrbital(owner, gameObject, this.OrbitalFollowerPrefab != null, this);
		if (this.BreaksUponContact && this.m_extantOrbital)
		{
			SpeculativeRigidbody component = this.m_extantOrbital.GetComponent<SpeculativeRigidbody>();
			if (component)
			{
				SpeculativeRigidbody speculativeRigidbody = component;
				speculativeRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleBreakOnCollision));
			}
		}
		if (this.BreaksUponOwnerDamage && owner)
		{
			owner.OnReceivedDamage += this.HandleBreakOnOwnerDamage;
		}
	}

	// Token: 0x060076B9 RID: 30393 RVA: 0x002F5474 File Offset: 0x002F3674
	private void HandleBreakOnOwnerDamage(PlayerController arg1)
	{
		if (!this)
		{
			return;
		}
		if (this.BreakVFX && this.m_extantOrbital && this.m_extantOrbital.GetComponentInChildren<tk2dSprite>())
		{
			SpawnManager.SpawnVFX(this.BreakVFX, this.m_extantOrbital.GetComponentInChildren<tk2dSprite>().WorldCenter.ToVector3ZisY(0f), Quaternion.identity);
		}
		if (this.m_owner)
		{
			this.m_owner.RemovePassiveItem(this.PickupObjectId);
			this.m_owner.OnReceivedDamage -= this.HandleBreakOnOwnerDamage;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060076BA RID: 30394 RVA: 0x002F5530 File Offset: 0x002F3730
	private void HandleBreakOnCollision(CollisionData rigidbodyCollision)
	{
		if (this.m_owner)
		{
			this.m_owner.RemovePassiveItem(this.PickupObjectId);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060076BB RID: 30395 RVA: 0x002F5560 File Offset: 0x002F3760
	public void DecoupleOrbital()
	{
		this.m_extantOrbital = null;
		if (this.BreaksUponOwnerDamage && this.m_owner)
		{
			this.m_owner.OnReceivedDamage -= this.HandleBreakOnOwnerDamage;
		}
	}

	// Token: 0x060076BC RID: 30396 RVA: 0x002F559C File Offset: 0x002F379C
	private void DestroyOrbital()
	{
		if (!this.m_extantOrbital)
		{
			return;
		}
		if (this.BreaksUponOwnerDamage && this.m_owner)
		{
			this.m_owner.OnReceivedDamage -= this.HandleBreakOnOwnerDamage;
		}
		UnityEngine.Object.Destroy(this.m_extantOrbital.gameObject);
		this.m_extantOrbital = null;
	}

	// Token: 0x060076BD RID: 30397 RVA: 0x002F5604 File Offset: 0x002F3804
	protected override void Update()
	{
		base.Update();
		if (this.HasUpgradeSynergy)
		{
			if (this.m_synergyUpgradeActive && (!this.m_owner || !this.m_owner.HasActiveBonusSynergy(this.UpgradeSynergy, false)))
			{
				if (this.m_owner)
				{
					for (int i = 0; i < this.synergyModifiers.Length; i++)
					{
						this.m_owner.healthHaver.damageTypeModifiers.Remove(this.synergyModifiers[i]);
					}
				}
				this.m_synergyUpgradeActive = false;
				this.DestroyOrbital();
				if (this.m_owner)
				{
					this.CreateOrbital(this.m_owner);
				}
			}
			else if (!this.m_synergyUpgradeActive && this.m_owner && this.m_owner.HasActiveBonusSynergy(this.UpgradeSynergy, false))
			{
				this.m_synergyUpgradeActive = true;
				this.DestroyOrbital();
				if (this.m_owner)
				{
					this.CreateOrbital(this.m_owner);
				}
				for (int j = 0; j < this.synergyModifiers.Length; j++)
				{
					this.m_owner.healthHaver.damageTypeModifiers.Add(this.synergyModifiers[j]);
				}
			}
		}
	}

	// Token: 0x060076BE RID: 30398 RVA: 0x002F575C File Offset: 0x002F395C
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		for (int i = 0; i < this.modifiers.Length; i++)
		{
			player.healthHaver.damageTypeModifiers.Add(this.modifiers[i]);
		}
		this.CreateOrbital(player);
	}

	// Token: 0x060076BF RID: 30399 RVA: 0x002F57CC File Offset: 0x002F39CC
	private void HandleNewFloor(PlayerController obj)
	{
		this.DestroyOrbital();
		this.CreateOrbital(obj);
	}

	// Token: 0x060076C0 RID: 30400 RVA: 0x002F57DC File Offset: 0x002F39DC
	public override DebrisObject Drop(PlayerController player)
	{
		this.DestroyOrbital();
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		for (int i = 0; i < this.modifiers.Length; i++)
		{
			player.healthHaver.damageTypeModifiers.Remove(this.modifiers[i]);
		}
		for (int j = 0; j < this.synergyModifiers.Length; j++)
		{
			player.healthHaver.damageTypeModifiers.Remove(this.synergyModifiers[j]);
		}
		return base.Drop(player);
	}

	// Token: 0x060076C1 RID: 30401 RVA: 0x002F587C File Offset: 0x002F3A7C
	protected override void OnDestroy()
	{
		if (this.m_owner != null)
		{
			PlayerController owner = this.m_owner;
			owner.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(owner.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
			for (int i = 0; i < this.modifiers.Length; i++)
			{
				this.m_owner.healthHaver.damageTypeModifiers.Remove(this.modifiers[i]);
			}
			for (int j = 0; j < this.synergyModifiers.Length; j++)
			{
				this.m_owner.healthHaver.damageTypeModifiers.Remove(this.synergyModifiers[j]);
			}
			this.m_owner.OnReceivedDamage -= this.HandleBreakOnOwnerDamage;
		}
		this.DestroyOrbital();
		base.OnDestroy();
	}

	// Token: 0x040078B2 RID: 30898
	public PlayerOrbital OrbitalPrefab;

	// Token: 0x040078B3 RID: 30899
	public PlayerOrbitalFollower OrbitalFollowerPrefab;

	// Token: 0x040078B4 RID: 30900
	public bool HasUpgradeSynergy;

	// Token: 0x040078B5 RID: 30901
	[LongNumericEnum]
	public CustomSynergyType UpgradeSynergy;

	// Token: 0x040078B6 RID: 30902
	public GameObject UpgradeOrbitalPrefab;

	// Token: 0x040078B7 RID: 30903
	public GameObject UpgradeOrbitalFollowerPrefab;

	// Token: 0x040078B8 RID: 30904
	public bool CanBeMimicked;

	// Token: 0x040078B9 RID: 30905
	[Header("Random Stuff, probably for Ioun Stones")]
	public DamageTypeModifier[] modifiers;

	// Token: 0x040078BA RID: 30906
	public DamageTypeModifier[] synergyModifiers;

	// Token: 0x040078BB RID: 30907
	public bool BreaksUponContact;

	// Token: 0x040078BC RID: 30908
	public bool BreaksUponOwnerDamage;

	// Token: 0x040078BD RID: 30909
	public GameObject BreakVFX;

	// Token: 0x040078BE RID: 30910
	protected GameObject m_extantOrbital;

	// Token: 0x040078BF RID: 30911
	protected bool m_synergyUpgradeActive;
}
