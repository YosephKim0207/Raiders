using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Dungeonator;
using UnityEngine;

// Token: 0x020011F0 RID: 4592
public class ResourcefulRatMazeSystemController : DungeonPlaceableBehaviour
{
	// Token: 0x06006687 RID: 26247 RVA: 0x0027DC94 File Offset: 0x0027BE94
	public IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		base.transform.parent = null;
		this.Initialize();
		yield break;
	}

	// Token: 0x06006688 RID: 26248 RVA: 0x0027DCB0 File Offset: 0x0027BEB0
	private void ResetMaze()
	{
		Debug.LogError("resetting the maze!!!!");
		this.m_currentTargetDirectionIndex = 0;
		this.m_currentLivingRoomIndex = 0;
		this.m_sequentialErrors = 0;
		this.m_errors = 0;
		this.m_playerHasLeftEntrance = false;
		this.m_mazeIsActive = true;
	}

	// Token: 0x06006689 RID: 26249 RVA: 0x0027DCE8 File Offset: 0x0027BEE8
	public void Initialize()
	{
		if (this.m_isInitialized)
		{
			return;
		}
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		this.m_centralRoomSeries = new List<RoomHandler>();
		this.m_centralRoomSeries.Add(absoluteRoom);
		absoluteRoom.OverrideVisibility = RoomHandler.VisibilityStatus.CURRENT;
		Pixelator.Instance.ProcessOcclusionChange(IntVector2.Zero, 1f, absoluteRoom, false);
		this.m_currentSolution = GameManager.GetResourcefulRatSolution();
		bool flag = GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_06);
		if (flag)
		{
			StringBuilder stringBuilder = new StringBuilder("Rat Solution: ");
			foreach (DungeonData.Direction direction in this.m_currentSolution)
			{
				stringBuilder.Append(direction).Append(" ");
			}
			Debug.LogError(stringBuilder);
		}
		PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
		for (int j = 0; j < absoluteRoom.connectedRooms.Count; j++)
		{
			absoluteRoom.connectedRooms[j].PreventRevealEver = true;
		}
		for (int k = 0; k < GameManager.Instance.Dungeon.data.rooms.Count; k++)
		{
			RoomHandler roomHandler = GameManager.Instance.Dungeon.data.rooms[k];
			if (roomHandler.connectedRooms.Count == 1 && roomHandler.connectedRooms[0] == GameManager.Instance.Dungeon.data.Entrance)
			{
				roomHandler.TargetPitfallRoom = absoluteRoom;
				roomHandler.ForcePitfallForFliers = true;
			}
			if (roomHandler.area.PrototypeRoomSpecialSubcategory == PrototypeDungeonRoom.RoomSpecialSubCategory.NPC_STORY && roomHandler != absoluteRoom && !this.m_centralRoomSeries.Contains(roomHandler))
			{
				this.m_centralRoomSeries.Add(roomHandler);
				for (int l = 0; l < roomHandler.connectedRooms.Count; l++)
				{
					roomHandler.connectedRooms[l].PreventRevealEver = true;
				}
			}
		}
		this.m_mazeIsActive = true;
		this.m_isInitialized = true;
	}

	// Token: 0x0600668A RID: 26250 RVA: 0x0027DF08 File Offset: 0x0027C108
	private void Update()
	{
		if (Dungeon.IsGenerating || !GameManager.HasInstance || GameManager.Instance.Dungeon == null)
		{
			return;
		}
		if (!this.m_isInitialized)
		{
			this.Initialize();
		}
		PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
		if (this.m_playerHasLeftEntrance && bestActivePlayer != null && bestActivePlayer.CurrentRoom == GameManager.Instance.Dungeon.data.Entrance)
		{
			this.ResetMaze();
			return;
		}
		if (!bestActivePlayer || bestActivePlayer.IsGhost)
		{
			return;
		}
		if (bestActivePlayer && bestActivePlayer.healthHaver && bestActivePlayer.healthHaver.IsDead)
		{
			return;
		}
		if (!this.m_mazeIsActive)
		{
			return;
		}
		if (!this.m_playerHasLeftEntrance)
		{
			if (bestActivePlayer && bestActivePlayer.CurrentRoom != null && bestActivePlayer.CurrentRoom == this.m_centralRoomSeries[0])
			{
				this.m_playerHasLeftEntrance = true;
			}
		}
		else
		{
			PlayerController playerController = bestActivePlayer;
			RoomHandler roomHandler = this.m_centralRoomSeries[this.m_currentLivingRoomIndex];
			DungeonData.Direction directionFromVector = DungeonData.GetDirectionFromVector2(BraveUtility.GetMajorAxis(playerController.CenterPosition - roomHandler.Epicenter.ToVector2()));
			if (playerController.CurrentRoom != roomHandler && !playerController.InExitCell)
			{
				if (this.m_errors < 2 && directionFromVector == this.m_currentSolution[this.m_currentTargetDirectionIndex])
				{
					if (this.m_currentTargetDirectionIndex == 5)
					{
						this.HandleVictory(playerController);
						this.m_mazeIsActive = false;
					}
					else
					{
						int num = 0;
						if (this.m_currentLivingRoomIndex < 5)
						{
							num = this.m_currentLivingRoomIndex + 1;
						}
						else if (this.m_currentLivingRoomIndex == 6)
						{
							num = 1;
						}
						this.HandleContinuance(playerController, num);
						this.m_currentTargetDirectionIndex++;
					}
				}
				else if (this.m_errors >= 2)
				{
					this.HandleFailure(playerController);
					this.m_mazeIsActive = false;
				}
				else
				{
					this.HandleTemporaryFailure(playerController);
					this.m_errors++;
				}
			}
		}
	}

	// Token: 0x0600668B RID: 26251 RVA: 0x0027E134 File Offset: 0x0027C334
	private void HandleVictory(PlayerController cp)
	{
		RoomHandler roomHandler = null;
		foreach (RoomHandler roomHandler2 in GameManager.Instance.Dungeon.data.rooms)
		{
			if (roomHandler2.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
			{
				roomHandler = roomHandler2;
				break;
			}
		}
		for (int i = 0; i < roomHandler.connectedRooms.Count; i++)
		{
			if (roomHandler.connectedRooms[i].distanceFromEntrance <= roomHandler.distanceFromEntrance)
			{
				roomHandler = roomHandler.connectedRooms[i];
				break;
			}
		}
		for (int j = 0; j < roomHandler.connectedRooms.Count; j++)
		{
			if (roomHandler.connectedRooms[j].distanceFromEntrance <= roomHandler.distanceFromEntrance)
			{
				roomHandler = roomHandler.connectedRooms[j];
				break;
			}
		}
		Vector2 vector = GameManager.Instance.MainCameraController.transform.position.XY() - cp.transform.position.XY();
		Vector2 vector2 = cp.transform.position.XY() - cp.CurrentRoom.area.basePosition.ToVector2();
		Vector2 vector3 = roomHandler.area.basePosition.ToVector2() + vector2 + new Vector2(3f, 3f);
		cp.WarpToPointAndBringCoopPartner(vector3, false, true);
		cp.ForceChangeRoom(roomHandler);
		GameManager.Instance.MainCameraController.transform.position = (cp.transform.position.XY() + vector).ToVector3ZUp(GameManager.Instance.MainCameraController.transform.position.z);
	}

	// Token: 0x0600668C RID: 26252 RVA: 0x0027E33C File Offset: 0x0027C53C
	private void HandleContinuance(PlayerController cp, int newLivingRoomIndex)
	{
		int currentLivingRoomIndex = this.m_currentLivingRoomIndex;
		this.m_currentLivingRoomIndex = newLivingRoomIndex;
		Vector2 vector = GameManager.Instance.MainCameraController.transform.position.XY() - cp.transform.position.XY();
		Vector2 vector2 = cp.transform.position.XY() - cp.CurrentRoom.area.basePosition.ToVector2();
		Vector2 vector3 = this.m_centralRoomSeries[this.m_currentLivingRoomIndex].area.basePosition.ToVector2() + vector2;
		cp.WarpToPointAndBringCoopPartner(vector3, false, true);
		cp.ForceChangeRoom(this.m_centralRoomSeries[this.m_currentLivingRoomIndex]);
		this.HandleNearestExitOcclusion(cp);
		GameManager.Instance.MainCameraController.transform.position = (cp.transform.position.XY() + vector).ToVector3ZUp(GameManager.Instance.MainCameraController.transform.position.z);
	}

	// Token: 0x0600668D RID: 26253 RVA: 0x0027E44C File Offset: 0x0027C64C
	private void HandleNearestExitOcclusion(PlayerController cp)
	{
		RuntimeExitDefinition runtimeExitDefinition = null;
		float num = float.MaxValue;
		for (int i = 0; i < cp.CurrentRoom.area.instanceUsedExits.Count; i++)
		{
			RuntimeRoomExitData runtimeRoomExitData = cp.CurrentRoom.area.exitToLocalDataMap[cp.CurrentRoom.area.instanceUsedExits[i]];
			float magnitude = ((cp.CurrentRoom.area.basePosition + runtimeRoomExitData.ExitOrigin - IntVector2.One).ToCenterVector2() - cp.CenterPosition).magnitude;
			if (magnitude < num && cp.CurrentRoom.exitDefinitionsByExit.ContainsKey(runtimeRoomExitData))
			{
				num = magnitude;
				runtimeExitDefinition = cp.CurrentRoom.exitDefinitionsByExit[runtimeRoomExitData];
			}
		}
		if (runtimeExitDefinition != null)
		{
			IntVector2 intVector = IntVector2.MaxValue;
			IntVector2 intVector2 = IntVector2.MinValue;
			DungeonData data = GameManager.Instance.Dungeon.data;
			foreach (IntVector2 intVector3 in runtimeExitDefinition.GetCellsForRoom(runtimeExitDefinition.downstreamRoom))
			{
				this.ProcessCell(data, intVector3);
				intVector = IntVector2.Min(intVector, intVector3);
				intVector2 = IntVector2.Max(intVector2, intVector3 + new IntVector2(0, 2));
			}
			foreach (IntVector2 intVector4 in runtimeExitDefinition.GetCellsForRoom(runtimeExitDefinition.upstreamRoom))
			{
				this.ProcessCell(data, intVector4);
				intVector = IntVector2.Min(intVector, intVector4);
				intVector2 = IntVector2.Max(intVector2, intVector4 + new IntVector2(0, 2));
			}
			foreach (IntVector2 intVector5 in runtimeExitDefinition.IntermediaryCells)
			{
				this.ProcessCell(data, intVector5);
				intVector = IntVector2.Min(intVector, intVector5);
				intVector2 = IntVector2.Max(intVector2, intVector5 + new IntVector2(0, 2));
			}
			Pixelator.Instance.ProcessModifiedRanges(intVector, intVector2);
			Pixelator.Instance.MarkOcclusionDirty();
		}
	}

	// Token: 0x0600668E RID: 26254 RVA: 0x0027E6D0 File Offset: 0x0027C8D0
	private void ProcessCell(DungeonData data, IntVector2 pos)
	{
		CellData cellData = data[pos];
		if (cellData != null)
		{
			cellData.occlusionData.cellRoomVisiblityCount = Mathf.RoundToInt(Mathf.Clamp01(1f));
			cellData.occlusionData.cellRoomVisitedCount = Mathf.RoundToInt(Mathf.Clamp01(1f));
			cellData.occlusionData.cellVisibleTargetOcclusion = 0f;
			cellData.occlusionData.cellVisitedTargetOcclusion = 0.7f;
			cellData.occlusionData.overrideOcclusion = true;
			cellData.occlusionData.cellOcclusion = 0f;
			BraveUtility.DrawDebugSquare(pos.ToVector2(), Color.green, 1000f);
		}
		CellData cellData2 = data[cellData.position + IntVector2.Up];
		if (cellData2 != null)
		{
			cellData2.occlusionData.cellRoomVisiblityCount = Mathf.RoundToInt(Mathf.Clamp01(1f));
			cellData2.occlusionData.cellRoomVisitedCount = Mathf.RoundToInt(Mathf.Clamp01(1f));
			cellData2.occlusionData.cellVisibleTargetOcclusion = 0f;
			cellData2.occlusionData.cellVisitedTargetOcclusion = 0.7f;
			cellData2.occlusionData.overrideOcclusion = true;
			cellData2.occlusionData.cellOcclusion = 0f;
		}
		CellData cellData3 = data[cellData.position + IntVector2.Up * 2];
		if (cellData3 != null)
		{
			cellData3.occlusionData.cellRoomVisiblityCount = Mathf.RoundToInt(Mathf.Clamp01(1f));
			cellData3.occlusionData.cellRoomVisitedCount = Mathf.RoundToInt(Mathf.Clamp01(1f));
			cellData3.occlusionData.cellVisibleTargetOcclusion = 0f;
			cellData3.occlusionData.cellVisitedTargetOcclusion = 0.7f;
			cellData3.occlusionData.overrideOcclusion = true;
			cellData3.occlusionData.cellOcclusion = 0f;
		}
	}

	// Token: 0x0600668F RID: 26255 RVA: 0x0027E894 File Offset: 0x0027CA94
	private void HandleTemporaryFailure(PlayerController cp)
	{
		this.m_currentTargetDirectionIndex = 0;
		this.HandleContinuance(cp, (this.m_errors != 0) ? 7 : 6);
	}

	// Token: 0x06006690 RID: 26256 RVA: 0x0027E8B8 File Offset: 0x0027CAB8
	private void HandleFailure(PlayerController cp)
	{
		RoomHandler roomHandler = null;
		foreach (RoomHandler roomHandler2 in GameManager.Instance.Dungeon.data.rooms)
		{
			if (roomHandler2.area.PrototypeRoomName.Contains("FailRoom"))
			{
				roomHandler = roomHandler2;
				break;
			}
		}
		Vector2 vector = GameManager.Instance.MainCameraController.transform.position.XY() - cp.transform.position.XY();
		Vector2 vector2 = cp.transform.position.XY() - cp.CurrentRoom.area.basePosition.ToVector2();
		Vector2 vector3 = roomHandler.area.basePosition.ToVector2() + vector2;
		cp.WarpToPointAndBringCoopPartner(vector3, false, true);
		cp.ForceChangeRoom(roomHandler);
		this.HandleNearestExitOcclusion(cp);
		GameManager.Instance.MainCameraController.transform.position = (cp.transform.position.XY() + vector).ToVector3ZUp(GameManager.Instance.MainCameraController.transform.position.z);
	}

	// Token: 0x0400625F RID: 25183
	private List<RoomHandler> m_centralRoomSeries;

	// Token: 0x04006260 RID: 25184
	private bool m_playerHasLeftEntrance;

	// Token: 0x04006261 RID: 25185
	private DungeonData.Direction[] m_currentSolution;

	// Token: 0x04006262 RID: 25186
	private int m_currentTargetDirectionIndex;

	// Token: 0x04006263 RID: 25187
	private int m_currentLivingRoomIndex;

	// Token: 0x04006264 RID: 25188
	private int m_sequentialErrors;

	// Token: 0x04006265 RID: 25189
	private int m_errors;

	// Token: 0x04006266 RID: 25190
	private bool m_mazeIsActive;

	// Token: 0x04006267 RID: 25191
	private bool m_isInitialized;
}
