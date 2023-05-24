using System;
using UnityEngine;

// Token: 0x02001707 RID: 5895
public class OrbitalSynergyProcessor : MonoBehaviour
{
	// Token: 0x0600890C RID: 35084 RVA: 0x0038D9E8 File Offset: 0x0038BBE8
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		this.m_item = base.GetComponent<PassiveItem>();
	}

	// Token: 0x0600890D RID: 35085 RVA: 0x0038DA04 File Offset: 0x0038BC04
	private void OnDisable()
	{
		this.DeactivateSynergy();
		this.m_active = false;
	}

	// Token: 0x0600890E RID: 35086 RVA: 0x0038DA14 File Offset: 0x0038BC14
	private void CreateOrbital(PlayerController owner)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((!(this.OrbitalPrefab != null)) ? this.OrbitalFollowerPrefab.gameObject : this.OrbitalPrefab.gameObject, owner.transform.position, Quaternion.identity);
		this.m_extantOrbital = gameObject;
		if (this.OrbitalPrefab != null)
		{
			this.m_extantOrbital.GetComponent<PlayerOrbital>().Initialize(owner);
		}
		else if (this.OrbitalFollowerPrefab != null)
		{
			this.m_extantOrbital.GetComponent<PlayerOrbitalFollower>().Initialize(owner);
		}
	}

	// Token: 0x0600890F RID: 35087 RVA: 0x0038DAB4 File Offset: 0x0038BCB4
	private void DestroyOrbital()
	{
		if (!this.m_extantOrbital)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.m_extantOrbital.gameObject);
		this.m_extantOrbital = null;
	}

	// Token: 0x06008910 RID: 35088 RVA: 0x0038DAE0 File Offset: 0x0038BCE0
	private void HandleNewFloor(PlayerController obj)
	{
		this.DestroyOrbital();
		this.CreateOrbital(obj);
	}

	// Token: 0x06008911 RID: 35089 RVA: 0x0038DAF0 File Offset: 0x0038BCF0
	public void ActivateSynergy(PlayerController player)
	{
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		this.CreateOrbital(player);
	}

	// Token: 0x06008912 RID: 35090 RVA: 0x0038DB1C File Offset: 0x0038BD1C
	public void DeactivateSynergy()
	{
		if (this.m_cachedPlayer != null)
		{
			PlayerController cachedPlayer = this.m_cachedPlayer;
			cachedPlayer.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(cachedPlayer.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
			this.m_cachedPlayer = null;
		}
		this.DestroyOrbital();
	}

	// Token: 0x06008913 RID: 35091 RVA: 0x0038DB70 File Offset: 0x0038BD70
	public void Update()
	{
		PlayerController playerController = null;
		if (this.m_gun)
		{
			playerController = this.m_gun.CurrentOwner as PlayerController;
		}
		else if (this.m_item)
		{
			playerController = this.m_item.Owner;
		}
		if (playerController && (this.RequiresNoSynergy || playerController.HasActiveBonusSynergy(this.RequiredSynergy, false)) && !this.m_active)
		{
			this.m_active = true;
			this.m_cachedPlayer = playerController;
			this.ActivateSynergy(playerController);
		}
		else if (!playerController || (!this.RequiresNoSynergy && !playerController.HasActiveBonusSynergy(this.RequiredSynergy, false) && this.m_active))
		{
			this.DeactivateSynergy();
			this.m_active = false;
		}
	}

	// Token: 0x04008EE4 RID: 36580
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008EE5 RID: 36581
	public bool RequiresNoSynergy;

	// Token: 0x04008EE6 RID: 36582
	public PlayerOrbital OrbitalPrefab;

	// Token: 0x04008EE7 RID: 36583
	public PlayerOrbitalFollower OrbitalFollowerPrefab;

	// Token: 0x04008EE8 RID: 36584
	private Gun m_gun;

	// Token: 0x04008EE9 RID: 36585
	private PassiveItem m_item;

	// Token: 0x04008EEA RID: 36586
	protected GameObject m_extantOrbital;

	// Token: 0x04008EEB RID: 36587
	private bool m_active;

	// Token: 0x04008EEC RID: 36588
	private PlayerController m_cachedPlayer;
}
