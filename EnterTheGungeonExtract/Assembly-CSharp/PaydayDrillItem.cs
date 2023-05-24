using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x020013CF RID: 5071
public class PaydayDrillItem : PlayerItem, IPaydayItem
{
	// Token: 0x06007303 RID: 29443 RVA: 0x002DB6C8 File Offset: 0x002D98C8
	public void StoreData(string id1, string id2, string id3)
	{
		this.ID01 = id1;
		this.ID02 = id2;
		this.ID03 = id3;
		this.HasSetOrder = true;
	}

	// Token: 0x06007304 RID: 29444 RVA: 0x002DB6E8 File Offset: 0x002D98E8
	public bool HasCachedData()
	{
		return this.HasSetOrder;
	}

	// Token: 0x06007305 RID: 29445 RVA: 0x002DB6F0 File Offset: 0x002D98F0
	public string GetID(int placement)
	{
		if (placement == 0)
		{
			return this.ID01;
		}
		if (placement == 1)
		{
			return this.ID02;
		}
		return this.ID03;
	}

	// Token: 0x06007306 RID: 29446 RVA: 0x002DB714 File Offset: 0x002D9914
	public override void MidGameSerialize(List<object> data)
	{
		base.MidGameSerialize(data);
		data.Add(this.HasSetOrder);
		data.Add(this.ID01);
		data.Add(this.ID02);
		data.Add(this.ID03);
	}

	// Token: 0x06007307 RID: 29447 RVA: 0x002DB754 File Offset: 0x002D9954
	public override void MidGameDeserialize(List<object> data)
	{
		base.MidGameDeserialize(data);
		if (data.Count == 4)
		{
			this.HasSetOrder = (bool)data[0];
			this.ID01 = (string)data[1];
			this.ID02 = (string)data[2];
			this.ID03 = (string)data[3];
		}
	}

	// Token: 0x06007308 RID: 29448 RVA: 0x002DB7BC File Offset: 0x002D99BC
	public override bool CanBeUsed(PlayerController user)
	{
		if (!user || user.CurrentRoom == null)
		{
			return false;
		}
		if (user.CurrentRoom.CompletelyPreventLeaving)
		{
			return false;
		}
		if (user.CurrentRoom.area.PrototypeLostWoodsRoom)
		{
			return false;
		}
		IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
		if (nearestInteractable is InteractableLock || nearestInteractable is Chest || nearestInteractable is DungeonDoorController)
		{
			if (nearestInteractable is InteractableLock)
			{
				InteractableLock interactableLock = nearestInteractable as InteractableLock;
				if (interactableLock && !interactableLock.IsBusted && interactableLock.transform.position.GetAbsoluteRoom() == user.CurrentRoom && interactableLock.IsLocked && !interactableLock.HasBeenPicked && interactableLock.lockMode == InteractableLock.InteractableLockMode.NORMAL)
				{
					return base.CanBeUsed(user);
				}
			}
			else if (nearestInteractable is DungeonDoorController)
			{
				DungeonDoorController dungeonDoorController = nearestInteractable as DungeonDoorController;
				if (dungeonDoorController != null && dungeonDoorController.Mode == DungeonDoorController.DungeonDoorMode.COMPLEX && dungeonDoorController.isLocked && !dungeonDoorController.lockIsBusted)
				{
					return base.CanBeUsed(user);
				}
			}
			else if (nearestInteractable is Chest)
			{
				Chest chest = nearestInteractable as Chest;
				return chest && chest.GetAbsoluteParentRoom() == user.CurrentRoom && chest.IsLocked && !chest.IsLockBroken && base.CanBeUsed(user);
			}
		}
		return false;
	}

	// Token: 0x06007309 RID: 29449 RVA: 0x002DB958 File Offset: 0x002D9B58
	protected override void DoEffect(PlayerController user)
	{
		base.DoEffect(user);
		AkSoundEngine.PostEvent("Play_OBJ_paydaydrill_start_01", GameManager.Instance.gameObject);
		AkSoundEngine.PostEvent("Play_OBJ_paydaydrill_loop_01", GameManager.Instance.gameObject);
		IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
		if (nearestInteractable is InteractableLock || nearestInteractable is Chest || nearestInteractable is DungeonDoorController)
		{
			if (nearestInteractable is InteractableLock)
			{
				InteractableLock interactableLock = nearestInteractable as InteractableLock;
				if (interactableLock.lockMode == InteractableLock.InteractableLockMode.NORMAL)
				{
					interactableLock.ForceUnlock();
					AkSoundEngine.PostEvent("m_OBJ_lock_pick_01", GameManager.Instance.gameObject);
				}
				AkSoundEngine.PostEvent("Stop_OBJ_paydaydrill_loop_01", GameManager.Instance.gameObject);
				return;
			}
			if (nearestInteractable is DungeonDoorController)
			{
				DungeonDoorController dungeonDoorController = nearestInteractable as DungeonDoorController;
				if (dungeonDoorController != null && dungeonDoorController.Mode == DungeonDoorController.DungeonDoorMode.COMPLEX && dungeonDoorController.isLocked)
				{
					dungeonDoorController.Unlock();
					AkSoundEngine.PostEvent("m_OBJ_lock_pick_01", GameManager.Instance.gameObject);
				}
				AkSoundEngine.PostEvent("Stop_OBJ_paydaydrill_loop_01", GameManager.Instance.gameObject);
			}
			else if (nearestInteractable is Chest)
			{
				Chest chest = nearestInteractable as Chest;
				if (chest.IsLocked)
				{
					if (chest.IsLockBroken)
					{
						AkSoundEngine.PostEvent("Stop_OBJ_paydaydrill_loop_01", GameManager.Instance.gameObject);
					}
					else if (chest.IsMimic && chest.majorBreakable)
					{
						chest.majorBreakable.ApplyDamage(1000f, Vector2.zero, false, false, true);
						AkSoundEngine.PostEvent("Stop_OBJ_paydaydrill_loop_01", GameManager.Instance.gameObject);
					}
					else
					{
						chest.ForceKillFuse();
						chest.PreventFuse = true;
						RoomHandler absoluteRoom = chest.transform.position.GetAbsoluteRoom();
						this.m_inEffect = true;
						if (absoluteRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.REWARD)
						{
							GameManager.Instance.Dungeon.StartCoroutine(this.HandleSeamlessTransitionToCombatRoom(absoluteRoom, chest));
						}
						else
						{
							GameManager.Instance.Dungeon.StartCoroutine(this.HandleTransitionToFallbackCombatRoom(absoluteRoom, chest));
						}
					}
				}
			}
		}
		else
		{
			AkSoundEngine.PostEvent("Stop_OBJ_paydaydrill_loop_01", GameManager.Instance.gameObject);
		}
	}

	// Token: 0x0600730A RID: 29450 RVA: 0x002DBBA0 File Offset: 0x002D9DA0
	protected IEnumerator HandleCombatWaves(Dungeon d, RoomHandler newRoom, Chest sourceChest)
	{
		DrillWaveDefinition[] wavesToUse = this.D_Quality_Waves;
		switch (GameManager.Instance.RewardManager.GetQualityFromChest(sourceChest))
		{
		case PickupObject.ItemQuality.C:
			wavesToUse = this.C_Quality_Waves;
			break;
		case PickupObject.ItemQuality.B:
			wavesToUse = this.B_Quality_Waves;
			break;
		case PickupObject.ItemQuality.A:
			wavesToUse = this.A_Quality_Waves;
			break;
		case PickupObject.ItemQuality.S:
			wavesToUse = this.S_Quality_Waves;
			break;
		}
		foreach (DrillWaveDefinition currentWave in wavesToUse)
		{
			int numEnemiesToSpawn = UnityEngine.Random.Range(currentWave.MinEnemies, currentWave.MaxEnemies + 1);
			for (int i = 0; i < numEnemiesToSpawn; i++)
			{
				newRoom.AddSpecificEnemyToRoomProcedurally(d.GetWeightedProceduralEnemy().enemyGuid, true, null);
			}
			yield return new WaitForSeconds(3f);
			while (newRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.RoomClear) > 0)
			{
				yield return new WaitForSeconds(1f);
			}
			if (newRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) > 0)
			{
				List<AIActor> activeEnemies = newRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				for (int j = 0; j < activeEnemies.Count; j++)
				{
					if (activeEnemies[j].IsNormalEnemy)
					{
						activeEnemies[j].EraseFromExistence(false);
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x0600730B RID: 29451 RVA: 0x002DBBD0 File Offset: 0x002D9DD0
	protected IEnumerator HandleTransitionToFallbackCombatRoom(RoomHandler sourceRoom, Chest sourceChest)
	{
		Dungeon d = GameManager.Instance.Dungeon;
		sourceChest.majorBreakable.TemporarilyInvulnerable = true;
		sourceRoom.DeregisterInteractable(sourceChest);
		RoomHandler newRoom = d.AddRuntimeRoom(this.GenericFallbackCombatRoom, null, DungeonData.LightGenerationStyle.FORCE_COLOR);
		newRoom.CompletelyPreventLeaving = true;
		Vector3 oldChestPosition = sourceChest.transform.position;
		sourceChest.transform.position = newRoom.Epicenter.ToVector3();
		if (sourceChest.transform.parent == sourceRoom.hierarchyParent)
		{
			sourceChest.transform.parent = newRoom.hierarchyParent;
		}
		SpeculativeRigidbody component = sourceChest.GetComponent<SpeculativeRigidbody>();
		if (component)
		{
			component.Reinitialize();
			PathBlocker.BlockRigidbody(component, false);
		}
		tk2dBaseSprite component2 = sourceChest.GetComponent<tk2dBaseSprite>();
		if (component2)
		{
			component2.UpdateZDepth();
		}
		Vector3 chestOffset = this.m_baseChestOffset;
		if (sourceChest.name.Contains("_Red") || sourceChest.name.Contains("_Black"))
		{
			chestOffset += this.m_largeChestOffset;
		}
		GameObject spawnedVFX = SpawnManager.SpawnVFX(this.DrillVFXPrefab, sourceChest.transform.position + chestOffset, Quaternion.identity);
		tk2dBaseSprite spawnedSprite = spawnedVFX.GetComponent<tk2dBaseSprite>();
		spawnedSprite.HeightOffGround = 1f;
		spawnedSprite.UpdateZDepth();
		Vector2 oldPlayerPosition = GameManager.Instance.BestActivePlayer.transform.position.XY();
		Vector2 newPlayerPosition = newRoom.Epicenter.ToVector2() + new Vector2(0f, -3f);
		Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0.125f);
		Pathfinder.Instance.InitializeRegion(d.data, newRoom.area.basePosition, newRoom.area.dimensions);
		GameManager.Instance.BestActivePlayer.WarpToPoint(newPlayerPosition, false, false);
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.GetOtherPlayer(GameManager.Instance.BestActivePlayer).ReuniteWithOtherPlayer(GameManager.Instance.BestActivePlayer, false);
		}
		yield return null;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].WarpFollowersToPlayer(false);
			GameManager.Instance.AllPlayers[i].WarpCompanionsToPlayer(false);
		}
		yield return new WaitForSeconds(this.DelayPostExpansionPreEnemies);
		yield return base.StartCoroutine(this.HandleCombatWaves(d, newRoom, sourceChest));
		this.DisappearDrillPoof.SpawnAtPosition(spawnedSprite.WorldBottomLeft + new Vector2(-0.0625f, 0.25f), 0f, null, null, null, new float?(3f), false, null, null, false);
		UnityEngine.Object.Destroy(spawnedVFX.gameObject);
		AkSoundEngine.PostEvent("Stop_OBJ_paydaydrill_loop_01", GameManager.Instance.gameObject);
		AkSoundEngine.PostEvent("Play_OBJ_item_spawn_01", GameManager.Instance.gameObject);
		sourceChest.ForceUnlock();
		bool goodToGo = false;
		while (!goodToGo)
		{
			goodToGo = true;
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				float num = Vector2.Distance(sourceChest.specRigidbody.UnitCenter, GameManager.Instance.AllPlayers[j].CenterPosition);
				if (num > 3f)
				{
					goodToGo = false;
				}
			}
			yield return null;
		}
		Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0.125f);
		GameManager.Instance.BestActivePlayer.WarpToPoint(oldPlayerPosition, false, false);
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.GetOtherPlayer(GameManager.Instance.BestActivePlayer).ReuniteWithOtherPlayer(GameManager.Instance.BestActivePlayer, false);
		}
		sourceChest.transform.position = oldChestPosition;
		if (sourceChest.transform.parent == newRoom.hierarchyParent)
		{
			sourceChest.transform.parent = sourceRoom.hierarchyParent;
		}
		SpeculativeRigidbody component3 = sourceChest.GetComponent<SpeculativeRigidbody>();
		if (component3)
		{
			component3.Reinitialize();
		}
		tk2dBaseSprite component4 = sourceChest.GetComponent<tk2dBaseSprite>();
		if (component4)
		{
			component4.UpdateZDepth();
		}
		sourceRoom.RegisterInteractable(sourceChest);
		this.m_inEffect = false;
		yield break;
	}

	// Token: 0x0600730C RID: 29452 RVA: 0x002DBBFC File Offset: 0x002D9DFC
	protected IEnumerator HandleSeamlessTransitionToCombatRoom(RoomHandler sourceRoom, Chest sourceChest)
	{
		Dungeon d = GameManager.Instance.Dungeon;
		sourceChest.majorBreakable.TemporarilyInvulnerable = true;
		sourceRoom.DeregisterInteractable(sourceChest);
		int tmapExpansion = 13;
		RoomHandler newRoom = d.RuntimeDuplicateChunk(sourceRoom.area.basePosition, sourceRoom.area.dimensions, tmapExpansion, sourceRoom, true);
		newRoom.CompletelyPreventLeaving = true;
		List<Transform> movedObjects = new List<Transform>();
		for (int i = 0; i < this.c_rewardRoomObjects.Length; i++)
		{
			Transform transform = sourceRoom.hierarchyParent.Find(this.c_rewardRoomObjects[i]);
			if (transform)
			{
				movedObjects.Add(transform);
				this.MoveObjectBetweenRooms(transform, sourceRoom, newRoom);
			}
		}
		this.MoveObjectBetweenRooms(sourceChest.transform, sourceRoom, newRoom);
		if (sourceChest.specRigidbody)
		{
			PathBlocker.BlockRigidbody(sourceChest.specRigidbody, false);
		}
		Vector3 chestOffset = this.m_baseChestOffset;
		if (sourceChest.name.Contains("_Red") || sourceChest.name.Contains("_Black"))
		{
			chestOffset += this.m_largeChestOffset;
		}
		GameObject spawnedVFX = SpawnManager.SpawnVFX(this.DrillVFXPrefab, sourceChest.transform.position + chestOffset, Quaternion.identity);
		tk2dBaseSprite spawnedSprite = spawnedVFX.GetComponent<tk2dBaseSprite>();
		spawnedSprite.HeightOffGround = 1f;
		spawnedSprite.UpdateZDepth();
		Vector2 oldPlayerPosition = GameManager.Instance.BestActivePlayer.transform.position.XY();
		Vector2 playerOffset = oldPlayerPosition - sourceRoom.area.basePosition.ToVector2();
		Vector2 newPlayerPosition = newRoom.area.basePosition.ToVector2() + playerOffset;
		Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0.125f);
		Pathfinder.Instance.InitializeRegion(d.data, newRoom.area.basePosition, newRoom.area.dimensions);
		GameManager.Instance.BestActivePlayer.WarpToPoint(newPlayerPosition, false, false);
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.GetOtherPlayer(GameManager.Instance.BestActivePlayer).ReuniteWithOtherPlayer(GameManager.Instance.BestActivePlayer, false);
		}
		yield return null;
		for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
		{
			GameManager.Instance.AllPlayers[j].WarpFollowersToPlayer(false);
			GameManager.Instance.AllPlayers[j].WarpCompanionsToPlayer(false);
		}
		yield return d.StartCoroutine(this.HandleCombatRoomExpansion(sourceRoom, newRoom, sourceChest));
		this.DisappearDrillPoof.SpawnAtPosition(spawnedSprite.WorldBottomLeft + new Vector2(-0.0625f, 0.25f), 0f, null, null, null, new float?(3f), false, null, null, false);
		UnityEngine.Object.Destroy(spawnedVFX.gameObject);
		sourceChest.ForceUnlock();
		AkSoundEngine.PostEvent("Stop_OBJ_paydaydrill_loop_01", GameManager.Instance.gameObject);
		AkSoundEngine.PostEvent("Play_OBJ_item_spawn_01", GameManager.Instance.gameObject);
		bool goodToGo = false;
		while (!goodToGo)
		{
			goodToGo = true;
			for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
			{
				float num = Vector2.Distance(sourceChest.specRigidbody.UnitCenter, GameManager.Instance.AllPlayers[k].CenterPosition);
				if (num > 3f)
				{
					goodToGo = false;
				}
			}
			yield return null;
		}
		GameManager.Instance.MainCameraController.SetManualControl(true, true);
		GameManager.Instance.MainCameraController.OverridePosition = GameManager.Instance.BestActivePlayer.CenterPosition;
		for (int l = 0; l < GameManager.Instance.AllPlayers.Length; l++)
		{
			GameManager.Instance.AllPlayers[l].SetInputOverride("shrinkage");
		}
		yield return d.StartCoroutine(this.HandleCombatRoomShrinking(newRoom));
		for (int m = 0; m < GameManager.Instance.AllPlayers.Length; m++)
		{
			GameManager.Instance.AllPlayers[m].ClearInputOverride("shrinkage");
		}
		Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0.125f);
		AkSoundEngine.PostEvent("Play_OBJ_paydaydrill_end_01", GameManager.Instance.gameObject);
		GameManager.Instance.MainCameraController.SetManualControl(false, false);
		GameManager.Instance.BestActivePlayer.WarpToPoint(oldPlayerPosition, false, false);
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.GetOtherPlayer(GameManager.Instance.BestActivePlayer).ReuniteWithOtherPlayer(GameManager.Instance.BestActivePlayer, false);
		}
		this.MoveObjectBetweenRooms(sourceChest.transform, newRoom, sourceRoom);
		for (int n = 0; n < movedObjects.Count; n++)
		{
			this.MoveObjectBetweenRooms(movedObjects[n], newRoom, sourceRoom);
		}
		sourceRoom.RegisterInteractable(sourceChest);
		this.m_inEffect = false;
		yield break;
	}

	// Token: 0x0600730D RID: 29453 RVA: 0x002DBC28 File Offset: 0x002D9E28
	private void MoveObjectBetweenRooms(Transform foundObject, RoomHandler fromRoom, RoomHandler toRoom)
	{
		Vector2 vector = foundObject.position.XY() - fromRoom.area.basePosition.ToVector2();
		Vector2 vector2 = toRoom.area.basePosition.ToVector2() + vector;
		foundObject.transform.position = vector2;
		if (foundObject.parent == fromRoom.hierarchyParent)
		{
			foundObject.parent = toRoom.hierarchyParent;
		}
		SpeculativeRigidbody component = foundObject.GetComponent<SpeculativeRigidbody>();
		if (component)
		{
			component.Reinitialize();
		}
		tk2dBaseSprite component2 = foundObject.GetComponent<tk2dBaseSprite>();
		if (component2)
		{
			component2.UpdateZDepth();
		}
	}

	// Token: 0x0600730E RID: 29454 RVA: 0x002DBCD0 File Offset: 0x002D9ED0
	private IEnumerator HandleCombatRoomShrinking(RoomHandler targetRoom)
	{
		float elapsed = 5.5f;
		int numExpansionsDone = 6;
		while (elapsed > 0f)
		{
			elapsed -= BraveTime.DeltaTime * 9f;
			while (elapsed < (float)numExpansionsDone && numExpansionsDone > 0)
			{
				numExpansionsDone--;
				this.ShrinkRoom(targetRoom);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600730F RID: 29455 RVA: 0x002DBCF4 File Offset: 0x002D9EF4
	private IEnumerator HandleCombatRoomExpansion(RoomHandler sourceRoom, RoomHandler targetRoom, Chest sourceChest)
	{
		yield return new WaitForSeconds(this.DelayPreExpansion);
		float duration = 5.5f;
		float elapsed = 0f;
		int numExpansionsDone = 0;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime * 9f;
			while (elapsed > (float)numExpansionsDone)
			{
				numExpansionsDone++;
				this.ExpandRoom(targetRoom);
				AkSoundEngine.PostEvent("Play_OBJ_rock_break_01", GameManager.Instance.gameObject);
			}
			yield return null;
		}
		Dungeon d = GameManager.Instance.Dungeon;
		Pathfinder.Instance.InitializeRegion(d.data, targetRoom.area.basePosition + new IntVector2(-5, -5), targetRoom.area.dimensions + new IntVector2(10, 10));
		yield return new WaitForSeconds(this.DelayPostExpansionPreEnemies);
		yield return this.HandleCombatWaves(d, targetRoom, sourceChest);
		yield break;
	}

	// Token: 0x06007310 RID: 29456 RVA: 0x002DBD20 File Offset: 0x002D9F20
	private void ShrinkRoom(RoomHandler r)
	{
		Dungeon dungeon = GameManager.Instance.Dungeon;
		AkSoundEngine.PostEvent("Play_OBJ_stone_crumble_01", GameManager.Instance.gameObject);
		tk2dTileMap tk2dTileMap = null;
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		for (int i = -5; i < r.area.dimensions.x + 5; i++)
		{
			for (int j = -5; j < r.area.dimensions.y + 5; j++)
			{
				IntVector2 intVector = r.area.basePosition + new IntVector2(i, j);
				CellData cellData = ((!dungeon.data.CheckInBoundsAndValid(intVector)) ? null : dungeon.data[intVector]);
				if (cellData != null && cellData.type != CellType.WALL && cellData.HasTypeNeighbor(dungeon.data, CellType.WALL))
				{
					hashSet.Add(cellData.position);
				}
			}
		}
		foreach (IntVector2 intVector2 in hashSet)
		{
			CellData cellData2 = dungeon.data[intVector2];
			cellData2.breakable = true;
			cellData2.occlusionData.overrideOcclusion = true;
			cellData2.occlusionData.cellOcclusionDirty = true;
			tk2dTileMap = dungeon.ConstructWallAtPosition(intVector2.x, intVector2.y, true);
			r.Cells.Remove(cellData2.position);
			r.CellsWithoutExits.Remove(cellData2.position);
			r.RawCells.Remove(cellData2.position);
		}
		Pixelator.Instance.MarkOcclusionDirty();
		Pixelator.Instance.ProcessOcclusionChange(r.Epicenter, 1f, r, false);
		if (tk2dTileMap)
		{
			dungeon.RebuildTilemap(tk2dTileMap);
		}
	}

	// Token: 0x06007311 RID: 29457 RVA: 0x002DBF10 File Offset: 0x002DA110
	private void ExpandRoom(RoomHandler r)
	{
		Dungeon dungeon = GameManager.Instance.Dungeon;
		AkSoundEngine.PostEvent("Play_OBJ_stone_crumble_01", GameManager.Instance.gameObject);
		tk2dTileMap tk2dTileMap = null;
		HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
		for (int i = -5; i < r.area.dimensions.x + 5; i++)
		{
			for (int j = -5; j < r.area.dimensions.y + 5; j++)
			{
				IntVector2 intVector = r.area.basePosition + new IntVector2(i, j);
				CellData cellData = ((!dungeon.data.CheckInBoundsAndValid(intVector)) ? null : dungeon.data[intVector]);
				if (cellData != null && cellData.type == CellType.WALL && cellData.HasTypeNeighbor(dungeon.data, CellType.FLOOR))
				{
					hashSet.Add(cellData.position);
				}
			}
		}
		foreach (IntVector2 intVector2 in hashSet)
		{
			CellData cellData2 = dungeon.data[intVector2];
			cellData2.breakable = true;
			cellData2.occlusionData.overrideOcclusion = true;
			cellData2.occlusionData.cellOcclusionDirty = true;
			tk2dTileMap = dungeon.DestroyWallAtPosition(intVector2.x, intVector2.y, true);
			if (UnityEngine.Random.value < 0.25f)
			{
				this.VFXDustPoof.SpawnAtPosition(intVector2.ToCenterVector3((float)intVector2.y), 0f, null, null, null, null, false, null, null, false);
			}
			r.Cells.Add(cellData2.position);
			r.CellsWithoutExits.Add(cellData2.position);
			r.RawCells.Add(cellData2.position);
		}
		Pixelator.Instance.MarkOcclusionDirty();
		Pixelator.Instance.ProcessOcclusionChange(r.Epicenter, 1f, r, false);
		if (tk2dTileMap)
		{
			dungeon.RebuildTilemap(tk2dTileMap);
		}
	}

	// Token: 0x04007460 RID: 29792
	public GameObject DrillVFXPrefab;

	// Token: 0x04007461 RID: 29793
	public VFXPool VFXDustPoof;

	// Token: 0x04007462 RID: 29794
	public VFXPool DisappearDrillPoof;

	// Token: 0x04007463 RID: 29795
	public PrototypeDungeonRoom GenericFallbackCombatRoom;

	// Token: 0x04007464 RID: 29796
	[Header("Timing")]
	public float DelayPreExpansion = 2.5f;

	// Token: 0x04007465 RID: 29797
	public float DelayPostExpansionPreEnemies = 2f;

	// Token: 0x04007466 RID: 29798
	[Header("Waves")]
	public DrillWaveDefinition[] D_Quality_Waves;

	// Token: 0x04007467 RID: 29799
	public DrillWaveDefinition[] C_Quality_Waves;

	// Token: 0x04007468 RID: 29800
	public DrillWaveDefinition[] B_Quality_Waves;

	// Token: 0x04007469 RID: 29801
	public DrillWaveDefinition[] A_Quality_Waves;

	// Token: 0x0400746A RID: 29802
	public DrillWaveDefinition[] S_Quality_Waves;

	// Token: 0x0400746B RID: 29803
	private bool m_inEffect;

	// Token: 0x0400746C RID: 29804
	[NonSerialized]
	public bool HasSetOrder;

	// Token: 0x0400746D RID: 29805
	[NonSerialized]
	public string ID01;

	// Token: 0x0400746E RID: 29806
	[NonSerialized]
	public string ID02;

	// Token: 0x0400746F RID: 29807
	[NonSerialized]
	public string ID03;

	// Token: 0x04007470 RID: 29808
	private Vector3 m_baseChestOffset = new Vector3(0.5f, 0.25f, 0f);

	// Token: 0x04007471 RID: 29809
	private Vector3 m_largeChestOffset = new Vector3(0.4375f, 0.0625f, 0f);

	// Token: 0x04007472 RID: 29810
	private string[] c_rewardRoomObjects = new string[] { "Gungeon_Treasure_Dais(Clone)", "GodRay_Placeable(Clone)" };
}
