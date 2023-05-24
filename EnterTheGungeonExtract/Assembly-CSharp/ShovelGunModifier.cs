using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200167C RID: 5756
public class ShovelGunModifier : MonoBehaviour, IGunInheritable
{
	// Token: 0x06008644 RID: 34372 RVA: 0x00378AC8 File Offset: 0x00376CC8
	public void Start()
	{
		this.m_gun = base.GetComponent<Gun>();
		this.m_gun.LockedHorizontalOnReload = true;
		if (this.OnlyOnEmptyReload)
		{
			Gun gun = this.m_gun;
			gun.OnAutoReload = (Action<PlayerController, Gun>)Delegate.Combine(gun.OnAutoReload, new Action<PlayerController, Gun>(this.HandleReloadPressedSimple));
		}
		else
		{
			Gun gun2 = this.m_gun;
			gun2.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun2.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReloadPressed));
		}
	}

	// Token: 0x06008645 RID: 34373 RVA: 0x00378B4C File Offset: 0x00376D4C
	private void Update()
	{
		if (this.m_wasReloading && this.m_gun && !this.m_gun.IsReloading)
		{
			this.m_wasReloading = false;
		}
		if (this.m_gun && this.m_gun.CurrentOwner != null && this.m_gun.ClipShotsRemaining > 0)
		{
			this.m_alreadyRolledReward = false;
		}
	}

	// Token: 0x06008646 RID: 34374 RVA: 0x00378BCC File Offset: 0x00376DCC
	private void HandleReloadPressedSimple(PlayerController ownerPlayer, Gun sourceGun)
	{
		this.HandleReloadPressed(ownerPlayer, sourceGun, false);
	}

	// Token: 0x06008647 RID: 34375 RVA: 0x00378BD8 File Offset: 0x00376DD8
	private void HandleReloadPressed(PlayerController ownerPlayer, Gun sourceGun, bool something)
	{
		if (sourceGun.IsReloading)
		{
			if (!this.m_wasReloading)
			{
				this.m_wasReloading = true;
				ownerPlayer.StartCoroutine(this.HandleDig(sourceGun));
			}
		}
		else
		{
			this.m_wasReloading = false;
		}
	}

	// Token: 0x06008648 RID: 34376 RVA: 0x00378C14 File Offset: 0x00376E14
	private IEnumerator HandleDig(Gun sourceGun)
	{
		float lootChanceMultiplier = 1f;
		if (this.WeightedByShotsRemaining)
		{
			float num = (float)sourceGun.ClipShotsRemaining * 1f / (float)sourceGun.ClipCapacity;
			if (UnityEngine.Random.value < num)
			{
				lootChanceMultiplier = 0f;
			}
		}
		float elapsed = 0f;
		while (elapsed < 0.75f)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		Vector2 offset = new Vector3(0f, -1.125f);
		SpawnManager.SpawnVFX(this.HoleVFX, this.m_gun.barrelOffset.position.XY() + offset, Quaternion.identity);
		if (lootChanceMultiplier > 0f && !this.m_alreadyRolledReward)
		{
			GenericLootTable genericLootTable = this.BadDigLootTable;
			bool flag = sourceGun && sourceGun.OwnerHasSynergy(CustomSynergyType.TWO_KINDS_OF_PEOPLE);
			bool flag2 = this.m_goodDigsUsed < ((!flag) ? this.NumberOfGoodDigs : (this.NumberOfGoodDigs + this.NumberOfGoodDigsAddedBySynergy)) && UnityEngine.Random.value > 0.5f;
			if (flag2 && sourceGun && sourceGun.CurrentOwner)
			{
				RoomHandler currentRoom = (sourceGun.CurrentOwner as PlayerController).CurrentRoom;
				if (currentRoom == this.m_lastRoomDugGood)
				{
					flag2 = false;
				}
				else
				{
					this.m_lastRoomDugGood = currentRoom;
				}
			}
			if (flag)
			{
				if (flag2)
				{
					this.m_goodDigsUsed++;
					genericLootTable = this.SynergyGoodDigLootTable;
				}
				else
				{
					genericLootTable = this.SynergyBadDigLootTable;
				}
			}
			else if (flag2)
			{
				this.m_goodDigsUsed++;
				genericLootTable = this.GoodDigLootTable;
			}
			GameObject gameObject = genericLootTable.SelectByWeight(false);
			if (gameObject)
			{
				LootEngine.SpawnItem(gameObject, this.m_gun.barrelOffset.position.XY() + offset, Vector2.zero, 0f, true, false, false);
			}
		}
		this.m_alreadyRolledReward = true;
		yield break;
	}

	// Token: 0x06008649 RID: 34377 RVA: 0x00378C38 File Offset: 0x00376E38
	public void InheritData(Gun sourceGun)
	{
		ShovelGunModifier component = sourceGun.GetComponent<ShovelGunModifier>();
		if (component)
		{
			this.m_goodDigsUsed = component.m_goodDigsUsed;
		}
	}

	// Token: 0x0600864A RID: 34378 RVA: 0x00378C64 File Offset: 0x00376E64
	public void MidGameSerialize(List<object> data, int dataIndex)
	{
		data.Add(this.m_goodDigsUsed);
	}

	// Token: 0x0600864B RID: 34379 RVA: 0x00378C78 File Offset: 0x00376E78
	public void MidGameDeserialize(List<object> data, ref int dataIndex)
	{
		this.m_goodDigsUsed = (int)data[dataIndex];
		dataIndex++;
	}

	// Token: 0x04008B15 RID: 35605
	public GameObject HoleVFX;

	// Token: 0x04008B16 RID: 35606
	public GenericLootTable GoodDigLootTable;

	// Token: 0x04008B17 RID: 35607
	public GenericLootTable SynergyGoodDigLootTable;

	// Token: 0x04008B18 RID: 35608
	public GenericLootTable BadDigLootTable;

	// Token: 0x04008B19 RID: 35609
	public GenericLootTable SynergyBadDigLootTable;

	// Token: 0x04008B1A RID: 35610
	public int NumberOfGoodDigs = 5;

	// Token: 0x04008B1B RID: 35611
	public int NumberOfGoodDigsAddedBySynergy = 5;

	// Token: 0x04008B1C RID: 35612
	public bool WeightedByShotsRemaining = true;

	// Token: 0x04008B1D RID: 35613
	public bool OnlyOnEmptyReload;

	// Token: 0x04008B1E RID: 35614
	private Gun m_gun;

	// Token: 0x04008B1F RID: 35615
	private bool m_alreadyRolledReward;

	// Token: 0x04008B20 RID: 35616
	private int m_goodDigsUsed;

	// Token: 0x04008B21 RID: 35617
	private bool m_wasReloading;

	// Token: 0x04008B22 RID: 35618
	private RoomHandler m_lastRoomDugGood;
}
