using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020016F8 RID: 5880
public class HoveringGunSynergyProcessor : MonoBehaviour
{
	// Token: 0x060088AC RID: 34988 RVA: 0x0038A7F8 File Offset: 0x003889F8
	public void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		this.m_item = base.GetComponent<PassiveItem>();
	}

	// Token: 0x060088AD RID: 34989 RVA: 0x0038A814 File Offset: 0x00388A14
	private bool IsInitialized(int index)
	{
		return this.m_initialized.Count > index && this.m_initialized[index];
	}

	// Token: 0x060088AE RID: 34990 RVA: 0x0038A838 File Offset: 0x00388A38
	public void Update()
	{
		if (this.Trigger == HoveringGunSynergyProcessor.TriggerStyle.CONSTANT)
		{
			if (this.m_gun)
			{
				if (this.m_gun && this.m_gun.isActiveAndEnabled && this.m_gun.CurrentOwner && this.m_gun.OwnerHasSynergy(this.RequiredSynergy))
				{
					for (int i = 0; i < this.NumToTrigger; i++)
					{
						if (!this.IsInitialized(i))
						{
							this.Enable(i);
						}
					}
				}
				else
				{
					this.DisableAll();
				}
			}
			else if (this.m_item)
			{
				if (this.m_item && this.m_item.Owner && this.m_item.Owner.HasActiveBonusSynergy(this.RequiredSynergy, false))
				{
					for (int j = 0; j < this.NumToTrigger; j++)
					{
						if (!this.IsInitialized(j))
						{
							this.Enable(j);
						}
					}
				}
				else
				{
					this.DisableAll();
				}
			}
		}
		else if (this.Trigger == HoveringGunSynergyProcessor.TriggerStyle.ON_DAMAGE)
		{
			if (!this.m_actionsLinked && this.m_gun && this.m_gun.CurrentOwner)
			{
				PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
				this.m_cachedLinkedPlayer = playerController;
				playerController.OnReceivedDamage += this.HandleOwnerDamaged;
				this.m_actionsLinked = true;
			}
			else if (this.m_actionsLinked && this.m_gun && !this.m_gun.CurrentOwner && this.m_cachedLinkedPlayer)
			{
				this.m_cachedLinkedPlayer.OnReceivedDamage -= this.HandleOwnerDamaged;
				this.m_cachedLinkedPlayer = null;
				this.m_actionsLinked = false;
			}
		}
		else if (this.Trigger == HoveringGunSynergyProcessor.TriggerStyle.ON_ACTIVE_ITEM)
		{
			if (!this.m_actionsLinked && this.m_gun && this.m_gun.CurrentOwner)
			{
				PlayerController playerController2 = this.m_gun.CurrentOwner as PlayerController;
				this.m_cachedLinkedPlayer = playerController2;
				playerController2.OnUsedPlayerItem += this.HandleOwnerItemUsed;
				this.m_actionsLinked = true;
			}
			else if (this.m_actionsLinked && this.m_gun && !this.m_gun.CurrentOwner && this.m_cachedLinkedPlayer)
			{
				this.m_cachedLinkedPlayer.OnUsedPlayerItem -= this.HandleOwnerItemUsed;
				this.m_cachedLinkedPlayer = null;
				this.m_actionsLinked = false;
			}
		}
	}

	// Token: 0x060088AF RID: 34991 RVA: 0x0038AB24 File Offset: 0x00388D24
	private void HandleOwnerItemUsed(PlayerController sourcePlayer, PlayerItem sourceItem)
	{
		if (sourcePlayer.HasActiveBonusSynergy(this.RequiredSynergy, false) && this.GetOwner())
		{
			for (int i = 0; i < this.NumToTrigger; i++)
			{
				int num = 0;
				while (this.IsInitialized(num))
				{
					num++;
				}
				this.Enable(num);
				base.StartCoroutine(this.ActiveItemDisable(num, sourcePlayer));
			}
		}
	}

	// Token: 0x060088B0 RID: 34992 RVA: 0x0038AB98 File Offset: 0x00388D98
	private void HandleOwnerDamaged(PlayerController sourcePlayer)
	{
		if (sourcePlayer.HasActiveBonusSynergy(this.RequiredSynergy, false))
		{
			for (int i = 0; i < this.NumToTrigger; i++)
			{
				int num = 0;
				while (this.IsInitialized(num))
				{
					num++;
				}
				this.Enable(num);
				base.StartCoroutine(this.TimedDisable(num, this.TriggerDuration));
			}
		}
	}

	// Token: 0x060088B1 RID: 34993 RVA: 0x0038AC00 File Offset: 0x00388E00
	private IEnumerator ActiveItemDisable(int index, PlayerController player)
	{
		yield return null;
		while (player && player.CurrentItem && player.CurrentItem.IsActive)
		{
			yield return null;
		}
		this.Disable(index);
		yield break;
	}

	// Token: 0x060088B2 RID: 34994 RVA: 0x0038AC2C File Offset: 0x00388E2C
	private IEnumerator TimedDisable(int index, float duration)
	{
		yield return new WaitForSeconds(duration);
		this.Disable(index);
		yield break;
	}

	// Token: 0x060088B3 RID: 34995 RVA: 0x0038AC58 File Offset: 0x00388E58
	private void OnDisable()
	{
		this.DisableAll();
	}

	// Token: 0x060088B4 RID: 34996 RVA: 0x0038AC60 File Offset: 0x00388E60
	private PlayerController GetOwner()
	{
		if (this.m_gun)
		{
			return this.m_gun.CurrentOwner as PlayerController;
		}
		if (this.m_item)
		{
			return this.m_item.Owner;
		}
		return null;
	}

	// Token: 0x060088B5 RID: 34997 RVA: 0x0038ACA0 File Offset: 0x00388EA0
	private void Enable(int index)
	{
		if (this.m_initialized.Count > index && this.m_initialized[index])
		{
			return;
		}
		PlayerController owner = this.GetOwner();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(ResourceCache.Acquire("Global Prefabs/HoveringGun") as GameObject, owner.CenterPosition.ToVector3ZisY(0f), Quaternion.identity);
		gameObject.transform.parent = owner.transform;
		while (this.m_hovers.Count < index + 1)
		{
			this.m_hovers.Add(null);
			this.m_initialized.Add(false);
		}
		this.m_hovers[index] = gameObject.GetComponent<HoveringGunController>();
		this.m_hovers[index].ShootAudioEvent = this.ShootAudioEvent;
		this.m_hovers[index].OnEveryShotAudioEvent = this.OnEveryShotAudioEvent;
		this.m_hovers[index].FinishedShootingAudioEvent = this.FinishedShootingAudioEvent;
		this.m_hovers[index].ConsumesTargetGunAmmo = this.ConsumesTargetGunAmmo;
		this.m_hovers[index].ChanceToConsumeTargetGunAmmo = this.ChanceToConsumeTargetGunAmmo;
		this.m_hovers[index].Position = this.PositionType;
		this.m_hovers[index].Aim = this.AimType;
		this.m_hovers[index].Trigger = this.FireType;
		this.m_hovers[index].CooldownTime = this.FireCooldown;
		this.m_hovers[index].ShootDuration = this.FireDuration;
		this.m_hovers[index].OnlyOnEmptyReload = this.OnlyOnEmptyReload;
		Gun gun = null;
		int num = this.TargetGunID;
		if (this.UsesMultipleGuns)
		{
			num = this.TargetGunIDs[index];
		}
		for (int i = 0; i < owner.inventory.AllGuns.Count; i++)
		{
			if (owner.inventory.AllGuns[i].PickupObjectId == num)
			{
				gun = owner.inventory.AllGuns[i];
			}
		}
		if (!gun)
		{
			gun = PickupObjectDatabase.Instance.InternalGetById(num) as Gun;
		}
		this.m_hovers[index].Initialize(gun, owner);
		this.m_initialized[index] = true;
	}

	// Token: 0x060088B6 RID: 34998 RVA: 0x0038AF00 File Offset: 0x00389100
	private void Disable(int index)
	{
		if (this.m_hovers[index])
		{
			UnityEngine.Object.Destroy(this.m_hovers[index].gameObject);
		}
	}

	// Token: 0x060088B7 RID: 34999 RVA: 0x0038AF30 File Offset: 0x00389130
	private void DisableAll()
	{
		for (int i = 0; i < this.m_hovers.Count; i++)
		{
			if (this.m_hovers[i])
			{
				UnityEngine.Object.Destroy(this.m_hovers[i].gameObject);
			}
		}
		this.m_hovers.Clear();
		this.m_initialized.Clear();
	}

	// Token: 0x060088B8 RID: 35000 RVA: 0x0038AF9C File Offset: 0x0038919C
	public void OnDestroy()
	{
		if (this.m_actionsLinked && this.m_cachedLinkedPlayer)
		{
			this.m_cachedLinkedPlayer.OnReceivedDamage -= this.HandleOwnerDamaged;
			this.m_cachedLinkedPlayer = null;
			this.m_actionsLinked = false;
		}
	}

	// Token: 0x04008E1D RID: 36381
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008E1E RID: 36382
	[PickupIdentifier]
	public int TargetGunID;

	// Token: 0x04008E1F RID: 36383
	public bool UsesMultipleGuns;

	// Token: 0x04008E20 RID: 36384
	[PickupIdentifier]
	public int[] TargetGunIDs;

	// Token: 0x04008E21 RID: 36385
	public HoveringGunController.HoverPosition PositionType;

	// Token: 0x04008E22 RID: 36386
	public HoveringGunController.AimType AimType;

	// Token: 0x04008E23 RID: 36387
	public HoveringGunController.FireType FireType;

	// Token: 0x04008E24 RID: 36388
	public float FireCooldown = 1f;

	// Token: 0x04008E25 RID: 36389
	public float FireDuration = 2f;

	// Token: 0x04008E26 RID: 36390
	public bool OnlyOnEmptyReload;

	// Token: 0x04008E27 RID: 36391
	public string ShootAudioEvent;

	// Token: 0x04008E28 RID: 36392
	public string OnEveryShotAudioEvent;

	// Token: 0x04008E29 RID: 36393
	public string FinishedShootingAudioEvent;

	// Token: 0x04008E2A RID: 36394
	public HoveringGunSynergyProcessor.TriggerStyle Trigger;

	// Token: 0x04008E2B RID: 36395
	public int NumToTrigger = 1;

	// Token: 0x04008E2C RID: 36396
	public float TriggerDuration = -1f;

	// Token: 0x04008E2D RID: 36397
	public bool ConsumesTargetGunAmmo;

	// Token: 0x04008E2E RID: 36398
	public float ChanceToConsumeTargetGunAmmo = 0.5f;

	// Token: 0x04008E2F RID: 36399
	private bool m_actionsLinked;

	// Token: 0x04008E30 RID: 36400
	private PlayerController m_cachedLinkedPlayer;

	// Token: 0x04008E31 RID: 36401
	private Gun m_gun;

	// Token: 0x04008E32 RID: 36402
	private PassiveItem m_item;

	// Token: 0x04008E33 RID: 36403
	private List<HoveringGunController> m_hovers = new List<HoveringGunController>();

	// Token: 0x04008E34 RID: 36404
	private List<bool> m_initialized = new List<bool>();

	// Token: 0x020016F9 RID: 5881
	public enum TriggerStyle
	{
		// Token: 0x04008E36 RID: 36406
		CONSTANT,
		// Token: 0x04008E37 RID: 36407
		ON_DAMAGE,
		// Token: 0x04008E38 RID: 36408
		ON_ACTIVE_ITEM
	}
}
