using System;
using System.Collections;
using UnityEngine;

// Token: 0x020016DE RID: 5854
public class CompanionSynergyProcessor : MonoBehaviour
{
	// Token: 0x17001453 RID: 5203
	// (get) Token: 0x06008828 RID: 34856 RVA: 0x003870CC File Offset: 0x003852CC
	public GameObject ExtantCompanion
	{
		get
		{
			return this.m_extantCompanion;
		}
	}

	// Token: 0x06008829 RID: 34857 RVA: 0x003870D4 File Offset: 0x003852D4
	private void CreateCompanion(PlayerController owner)
	{
		if (this.PreventRespawnOnFloorLoad)
		{
			return;
		}
		AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid);
		Vector3 position = owner.transform.position;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, position, Quaternion.identity);
		this.m_extantCompanion = gameObject;
		CompanionController orAddComponent = this.m_extantCompanion.GetOrAddComponent<CompanionController>();
		orAddComponent.Initialize(owner);
		if (orAddComponent.specRigidbody)
		{
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
		}
	}

	// Token: 0x0600882A RID: 34858 RVA: 0x0038715C File Offset: 0x0038535C
	private void DestroyCompanion()
	{
		if (!this.m_extantCompanion)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.m_extantCompanion);
		this.m_extantCompanion = null;
	}

	// Token: 0x0600882B RID: 34859 RVA: 0x00387184 File Offset: 0x00385384
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		this.m_item = base.GetComponent<PassiveItem>();
		this.m_activeItem = base.GetComponent<PlayerItem>();
	}

	// Token: 0x0600882C RID: 34860 RVA: 0x003871AC File Offset: 0x003853AC
	public void Update()
	{
		PlayerController playerController = this.ManuallyAssignedPlayer;
		if (!playerController && this.m_item)
		{
			playerController = this.m_item.Owner;
		}
		if (!playerController && this.m_activeItem && this.m_activeItem.PickedUp && this.m_activeItem.LastOwner)
		{
			playerController = this.m_activeItem.LastOwner;
		}
		if (!playerController && this.m_gun && this.m_gun.CurrentOwner is PlayerController)
		{
			playerController = this.m_gun.CurrentOwner as PlayerController;
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

	// Token: 0x0600882D RID: 34861 RVA: 0x00387304 File Offset: 0x00385504
	private void OnDisable()
	{
		if (this.m_active && !this.PersistsOnDisable)
		{
			this.DeactivateSynergy();
			this.m_active = false;
		}
		else if (this.m_active && this.m_cachedPlayer)
		{
			this.m_cachedPlayer.StartCoroutine(this.HandleDisabledChecks());
		}
	}

	// Token: 0x0600882E RID: 34862 RVA: 0x00387368 File Offset: 0x00385568
	private IEnumerator HandleDisabledChecks()
	{
		yield return null;
		while (this && this.m_cachedPlayer && !base.isActiveAndEnabled && this.m_active)
		{
			if (!this.m_cachedPlayer.HasActiveBonusSynergy(this.RequiredSynergy, false))
			{
				this.DeactivateSynergy();
				this.m_active = false;
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600882F RID: 34863 RVA: 0x00387384 File Offset: 0x00385584
	private void OnDestroy()
	{
		if (this.m_active)
		{
			this.DeactivateSynergy();
			this.m_active = false;
		}
	}

	// Token: 0x06008830 RID: 34864 RVA: 0x003873A0 File Offset: 0x003855A0
	public void ActivateSynergy(PlayerController player)
	{
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
		this.CreateCompanion(player);
	}

	// Token: 0x06008831 RID: 34865 RVA: 0x003873CC File Offset: 0x003855CC
	private void HandleNewFloor(PlayerController obj)
	{
		this.DestroyCompanion();
		if (!this.PreventRespawnOnFloorLoad)
		{
			this.CreateCompanion(obj);
		}
	}

	// Token: 0x06008832 RID: 34866 RVA: 0x003873E8 File Offset: 0x003855E8
	public void DeactivateSynergy()
	{
		if (this.m_cachedPlayer != null)
		{
			PlayerController cachedPlayer = this.m_cachedPlayer;
			cachedPlayer.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(cachedPlayer.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloor));
			this.m_cachedPlayer = null;
		}
		this.DestroyCompanion();
	}

	// Token: 0x04008D77 RID: 36215
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008D78 RID: 36216
	public bool RequiresNoSynergy;

	// Token: 0x04008D79 RID: 36217
	public bool PersistsOnDisable;

	// Token: 0x04008D7A RID: 36218
	[EnemyIdentifier]
	public string CompanionGuid;

	// Token: 0x04008D7B RID: 36219
	[NonSerialized]
	public bool PreventRespawnOnFloorLoad;

	// Token: 0x04008D7C RID: 36220
	private Gun m_gun;

	// Token: 0x04008D7D RID: 36221
	private PassiveItem m_item;

	// Token: 0x04008D7E RID: 36222
	private PlayerItem m_activeItem;

	// Token: 0x04008D7F RID: 36223
	[NonSerialized]
	public PlayerController ManuallyAssignedPlayer;

	// Token: 0x04008D80 RID: 36224
	private GameObject m_extantCompanion;

	// Token: 0x04008D81 RID: 36225
	private bool m_active;

	// Token: 0x04008D82 RID: 36226
	private PlayerController m_cachedPlayer;
}
