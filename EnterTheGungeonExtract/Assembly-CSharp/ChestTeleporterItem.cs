using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02001371 RID: 4977
public class ChestTeleporterItem : PlayerItem
{
	// Token: 0x060070C1 RID: 28865 RVA: 0x002CC070 File Offset: 0x002CA270
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloorLoaded));
	}

	// Token: 0x060070C2 RID: 28866 RVA: 0x002CC09C File Offset: 0x002CA29C
	protected override void OnPreDrop(PlayerController user)
	{
		user.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(user.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloorLoaded));
		base.OnPreDrop(user);
	}

	// Token: 0x060070C3 RID: 28867 RVA: 0x002CC0C8 File Offset: 0x002CA2C8
	protected override void OnDestroy()
	{
		if (this.LastOwner)
		{
			PlayerController lastOwner = this.LastOwner;
			lastOwner.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(lastOwner.OnNewFloorLoaded, new Action<PlayerController>(this.HandleNewFloorLoaded));
		}
		base.OnDestroy();
	}

	// Token: 0x060070C4 RID: 28868 RVA: 0x002CC108 File Offset: 0x002CA308
	private void HandleNewFloorLoaded(PlayerController obj)
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST)
		{
			return;
		}
		base.StartCoroutine(this.LaunchChestSpawns());
	}

	// Token: 0x060070C5 RID: 28869 RVA: 0x002CC128 File Offset: 0x002CA328
	public static RoomHandler FindBossFoyer()
	{
		RoomHandler roomHandler = null;
		foreach (RoomHandler roomHandler2 in GameManager.Instance.Dungeon.data.rooms)
		{
			if (roomHandler2.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS && roomHandler2.area.PrototypeRoomBossSubcategory == PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS)
			{
				roomHandler = roomHandler2;
				break;
			}
		}
		for (int i = 0; i < roomHandler.connectedRooms.Count; i++)
		{
			if (roomHandler.connectedRooms[i].distanceFromEntrance <= roomHandler.distanceFromEntrance)
			{
				return roomHandler.connectedRooms[i];
			}
		}
		return null;
	}

	// Token: 0x060070C6 RID: 28870 RVA: 0x002CC200 File Offset: 0x002CA400
	private IEnumerator LaunchChestSpawns()
	{
		if (this.m_isSpawning)
		{
			yield break;
		}
		this.m_isSpawning = true;
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		yield return null;
		List<CachedChestData> failedList = new List<CachedChestData>();
		for (int i = 0; i < this.m_chestos.Count; i++)
		{
			CachedChestData cachedChestData = this.m_chestos[i];
			RoomHandler entrance = GameManager.Instance.Dungeon.data.Entrance;
			RoomHandler roomHandler = entrance;
			float num = this.ChanceToBossFoyerAndUpgrade;
			if (this.LastOwner && this.LastOwner.HasActiveBonusSynergy(CustomSynergyType.DOUBLE_TELEPORTERS, false))
			{
				num = 1f;
			}
			if (UnityEngine.Random.value <= num)
			{
				roomHandler = ChestTeleporterItem.FindBossFoyer() ?? roomHandler;
				cachedChestData.Upgrade();
			}
			CellValidator cellValidator = delegate(IntVector2 c)
			{
				for (int n = 0; n < 5; n++)
				{
					for (int num2 = 0; num2 < 5; num2++)
					{
						if (!GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(c.x + n, c.y + num2) || GameManager.Instance.Dungeon.data[c.x + n, c.y + num2].type == CellType.PIT || GameManager.Instance.Dungeon.data[c.x + n, c.y + num2].isOccupied)
						{
							return false;
						}
					}
				}
				return true;
			};
			IntVector2? randomAvailableCell = roomHandler.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 5), new CellTypes?(CellTypes.FLOOR), false, cellValidator);
			IntVector2? intVector = ((randomAvailableCell == null) ? null : new IntVector2?(randomAvailableCell.GetValueOrDefault() + IntVector2.One));
			if (intVector != null)
			{
				cachedChestData.SpawnChest(intVector.Value);
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < 3; k++)
					{
						IntVector2 intVector2 = intVector.Value + IntVector2.One + new IntVector2(j, k);
						GameManager.Instance.Dungeon.data[intVector2].isOccupied = true;
					}
				}
			}
			else
			{
				roomHandler = ((roomHandler != entrance) ? entrance : ChestTeleporterItem.FindBossFoyer());
				if (roomHandler == null)
				{
					roomHandler = entrance;
				}
				IntVector2? randomAvailableCell2 = roomHandler.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 5), new CellTypes?(CellTypes.FLOOR), false, cellValidator);
				intVector = ((randomAvailableCell2 == null) ? null : new IntVector2?(randomAvailableCell2.GetValueOrDefault() + IntVector2.One));
				if (intVector != null)
				{
					cachedChestData.SpawnChest(intVector.Value);
					for (int l = 0; l < 3; l++)
					{
						for (int m = 0; m < 3; m++)
						{
							IntVector2 intVector3 = intVector.Value + IntVector2.One + new IntVector2(l, m);
							GameManager.Instance.Dungeon.data[intVector3].isOccupied = true;
						}
					}
				}
				else
				{
					failedList.Add(cachedChestData);
				}
			}
		}
		this.m_chestos.Clear();
		this.m_chestos.AddRange(failedList);
		this.m_isSpawning = false;
		yield break;
	}

	// Token: 0x060070C7 RID: 28871 RVA: 0x002CC21C File Offset: 0x002CA41C
	public override bool CanBeUsed(PlayerController user)
	{
		if (!user || user.CurrentRoom == null)
		{
			return false;
		}
		IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
		if (nearestInteractable is Chest)
		{
			Chest chest = nearestInteractable as Chest;
			return chest && !chest.IsOpen && chest.GetAbsoluteParentRoom() == user.CurrentRoom && chest.ChestIdentifier != Chest.SpecialChestIdentifier.RAT && base.CanBeUsed(user);
		}
		return false;
	}

	// Token: 0x060070C8 RID: 28872 RVA: 0x002CC2B0 File Offset: 0x002CA4B0
	protected override void DoEffect(PlayerController user)
	{
		if (!user || user.CurrentRoom == null)
		{
			return;
		}
		IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
		AkSoundEngine.PostEvent("Play_OBJ_chestwarp_use_01", base.gameObject);
		if (nearestInteractable is Chest)
		{
			Chest chest = nearestInteractable as Chest;
			if (!chest)
			{
				return;
			}
			if (chest.IsOpen)
			{
				return;
			}
			if (chest.GetAbsoluteParentRoom() != user.CurrentRoom)
			{
				return;
			}
			CachedChestData cachedChestData = new CachedChestData(chest);
			SpawnManager.SpawnVFX(this.TeleportVFX, chest.sprite.WorldCenter, Quaternion.identity, true);
			user.CurrentRoom.DeregisterInteractable(chest);
			chest.DeregisterChestOnMinimap();
			if (chest.majorBreakable)
			{
				chest.majorBreakable.TemporarilyInvulnerable = true;
			}
			UnityEngine.Object.Destroy(chest.gameObject, 0.8f);
			this.m_chestos.Add(cachedChestData);
		}
	}

	// Token: 0x060070C9 RID: 28873 RVA: 0x002CC3AC File Offset: 0x002CA5AC
	public override void MidGameSerialize(List<object> data)
	{
		base.MidGameSerialize(data);
		data.Add(this.m_chestos.Count);
		for (int i = 0; i < this.m_chestos.Count; i++)
		{
			data.Add(this.m_chestos[i].Serialize());
		}
	}

	// Token: 0x060070CA RID: 28874 RVA: 0x002CC40C File Offset: 0x002CA60C
	public override void MidGameDeserialize(List<object> data)
	{
		base.MidGameDeserialize(data);
		int num = (int)data[0];
		this.m_chestos.Clear();
		for (int i = 1; i < num + 1; i++)
		{
			string text = (string)data[i];
			CachedChestData cachedChestData = new CachedChestData(text);
			this.m_chestos.Add(cachedChestData);
		}
		if (this.m_chestos.Count > 0)
		{
			base.StartCoroutine(this.LaunchChestSpawns());
		}
	}

	// Token: 0x0400704D RID: 28749
	public GameObject TeleportVFX;

	// Token: 0x0400704E RID: 28750
	public float ChanceToBossFoyerAndUpgrade = 0.5f;

	// Token: 0x0400704F RID: 28751
	private List<CachedChestData> m_chestos = new List<CachedChestData>();

	// Token: 0x04007050 RID: 28752
	private bool m_isSpawning;
}
