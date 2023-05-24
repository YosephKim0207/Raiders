using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020011F3 RID: 4595
public static class TelevisionQuestController
{
	// Token: 0x060066A1 RID: 26273 RVA: 0x0027EDD8 File Offset: 0x0027CFD8
	public static void RemoveMaintenanceRoomBackpack()
	{
		bool flag = false;
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_READY_FOR_UNLOCKS))
		{
			GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
			if (tilesetId != GlobalDungeonData.ValidTilesets.GUNGEON)
			{
				if (tilesetId != GlobalDungeonData.ValidTilesets.MINEGEON)
				{
					if (tilesetId != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
					{
						if (tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
						{
							if (!GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK4_COMPLETE) && GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK3_COMPLETE))
							{
								flag = true;
							}
						}
					}
					else if (!GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK3_COMPLETE) && GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK2_COMPLETE))
					{
						flag = true;
					}
				}
				else if (!GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK2_COMPLETE) && GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK1_COMPLETE))
				{
					flag = true;
				}
			}
			else if (!GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK1_COMPLETE))
			{
				flag = true;
			}
		}
		if (!flag)
		{
			GameObject gameObject = GameObject.Find("MaintenanceRoom(Clone)");
			if (gameObject != null)
			{
				Transform transform = gameObject.transform.Find("Pack");
				if (transform != null)
				{
					transform.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x060066A2 RID: 26274 RVA: 0x0027EF28 File Offset: 0x0027D128
	public static void HandlePuzzleSetup()
	{
		if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE && GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK1_COMPLETE))
		{
			RoomHandler roomHandler = null;
			for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
			{
				string roomName = GameManager.Instance.Dungeon.data.rooms[i].GetRoomName();
				if (roomName != null && roomName.Contains("Maintenance"))
				{
					roomHandler = GameManager.Instance.Dungeon.data.rooms[i];
				}
			}
			if (roomHandler != null)
			{
				bool flag = false;
				IntVector2 centeredVisibleClearSpot = roomHandler.GetCenteredVisibleClearSpot(2, 2, out flag, true);
				if (flag)
				{
					DungeonPlaceableUtility.InstantiateDungeonPlaceable(BraveResources.Load("Global Prefabs/Global Items/BustedTelevisionPlaceable", ".prefab") as GameObject, roomHandler, centeredVisibleClearSpot - roomHandler.area.basePosition, false, AIActor.AwakenAnimationType.Default, false);
				}
			}
		}
	}
}
