using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using tk2dRuntime.TileMap;
using UnityEngine;

// Token: 0x020017C0 RID: 6080
public class Minimap : MonoBehaviour
{
	// Token: 0x17001550 RID: 5456
	// (get) Token: 0x06008E61 RID: 36449 RVA: 0x003BE410 File Offset: 0x003BC610
	public Minimap.MinimapDisplayMode MinimapMode
	{
		get
		{
			if (this.PreventMinimap)
			{
				return Minimap.MinimapDisplayMode.NEVER;
			}
			if (GameManager.Instance.IsFoyer)
			{
				return Minimap.MinimapDisplayMode.NEVER;
			}
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.RESOURCEFUL_RAT)
			{
				return Minimap.MinimapDisplayMode.NEVER;
			}
			if (GameManager.Instance.BestActivePlayer && GameManager.Instance.BestActivePlayer.CurrentRoom != null && GameManager.Instance.BestActivePlayer.CurrentRoom.PreventMinimapUpdates)
			{
				return Minimap.MinimapDisplayMode.NEVER;
			}
			return GameManager.Options.MinimapDisplayMode;
		}
	}

	// Token: 0x17001551 RID: 5457
	// (get) Token: 0x06008E62 RID: 36450 RVA: 0x003BE49C File Offset: 0x003BC69C
	public static bool DoMinimap
	{
		get
		{
			return !GameManager.Instance.IsLoadingLevel && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.END_TIMES && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.CHARACTER_PAST && !TextBoxManager.ExtantTextBoxVisible && !TimeTubeCreditsController.IsTimeTubing;
		}
	}

	// Token: 0x17001552 RID: 5458
	// (get) Token: 0x06008E63 RID: 36451 RVA: 0x003BE4F8 File Offset: 0x003BC6F8
	public Dictionary<RoomHandler, GameObject> RoomToTeleportMap
	{
		get
		{
			return this.roomToTeleportIconMap;
		}
	}

	// Token: 0x17001553 RID: 5459
	// (get) Token: 0x06008E64 RID: 36452 RVA: 0x003BE500 File Offset: 0x003BC700
	public bool IsFullscreen
	{
		get
		{
			return this.m_isFullscreen;
		}
	}

	// Token: 0x17001554 RID: 5460
	// (get) Token: 0x06008E65 RID: 36453 RVA: 0x003BE508 File Offset: 0x003BC708
	// (set) Token: 0x06008E66 RID: 36454 RVA: 0x003BE52C File Offset: 0x003BC72C
	public static Minimap Instance
	{
		get
		{
			if (Minimap.m_instance == null)
			{
				Minimap.m_instance = UnityEngine.Object.FindObjectOfType<Minimap>();
			}
			return Minimap.m_instance;
		}
		set
		{
			Minimap.m_instance = value;
		}
	}

	// Token: 0x17001555 RID: 5461
	// (get) Token: 0x06008E67 RID: 36455 RVA: 0x003BE534 File Offset: 0x003BC734
	public static bool HasInstance
	{
		get
		{
			return Minimap.m_instance != null;
		}
	}

	// Token: 0x17001556 RID: 5462
	// (get) Token: 0x06008E68 RID: 36456 RVA: 0x003BE544 File Offset: 0x003BC744
	// (set) Token: 0x06008E69 RID: 36457 RVA: 0x003BE54C File Offset: 0x003BC74C
	public bool HeldOpen { get; set; }

	// Token: 0x17001557 RID: 5463
	public bool this[int x, int y]
	{
		get
		{
			return this.m_simplifiedData != null && x >= 0 && y >= 0 && x < this.m_simplifiedData.GetLength(0) && y < this.m_simplifiedData.GetLength(1) && this.m_simplifiedData[x, y];
		}
	}

	// Token: 0x06008E6B RID: 36459 RVA: 0x003BE5B4 File Offset: 0x003BC7B4
	private void AssignColorToTile(int ix, int iy, int layer, Color32 color, tk2dTileMap map)
	{
		if (!map.HasColorChannel())
		{
			map.CreateColorChannel();
		}
		ColorChannel colorChannel = map.ColorChannel;
		map.data.Layers[layer].useColor = true;
		colorChannel.SetColor(ix, iy, color);
	}

	// Token: 0x06008E6C RID: 36460 RVA: 0x003BE600 File Offset: 0x003BC800
	private void ToggleMinimapRat(bool fullscreen, bool holdOpen = false)
	{
		this.cameraRef.cullingMask = 0;
		GameUIRoot.Instance.notificationController.ForceHide();
		GameUIRoot.Instance.ToggleAllDefaultLabels(!fullscreen, "minimap");
		this.m_isFullscreen = fullscreen;
		this.HeldOpen = holdOpen;
		if (fullscreen)
		{
			this.m_cachedFadeValue = (float)((!this.m_isFaded) ? 1 : 0);
			this.m_mapMaskMaterial.SetFloat("_Fade", 1f);
			this.currentXRectFactor = 1f;
			this.currentYRectFactor = 1f;
		}
		else
		{
			this.m_mapMaskMaterial.SetFloat("_Fade", this.m_cachedFadeValue);
			this.currentXRectFactor = 0.25f;
			this.currentYRectFactor = 0.25f;
			BraveInput.ConsumeAllAcrossInstances(GungeonActions.GungeonActionType.Shoot);
		}
		Shader.SetGlobalFloat("_FullMapActive", (float)((!fullscreen) ? 0 : 1));
		this.UpdateScale();
		if (fullscreen)
		{
			AkSoundEngine.PostEvent("Play_UI_map_open_01", base.gameObject);
		}
		this.m_cameraPanOffset = Vector3.zero;
		if (fullscreen)
		{
			Pixelator.Instance.FadeColor = Color.black;
			Pixelator.Instance.fade = 0.3f;
			GameUIRoot.Instance.HideCoreUI(string.Empty);
			GameUIRoot.Instance.UnfoldGunventory(GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameUIRoot.Instance.ToggleItemPanels(false);
			}
			this.UIMinimap.ToggleState(true);
		}
		else
		{
			Pixelator.Instance.FadeColor = Color.black;
			Pixelator.Instance.fade = 1f;
			GameUIRoot.Instance.ShowCoreUI(string.Empty);
			GameUIRoot.Instance.RefoldGunventory();
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameUIRoot.Instance.ToggleItemPanels(true);
			}
			this.UIMinimap.ToggleState(false);
		}
		if (this.m_isFullscreen)
		{
			this.m_cameraBasePosition = this.GetCameraBasePosition();
			this.cameraRef.transform.position = this.m_cameraBasePosition + this.m_cameraPanOffset;
		}
	}

	// Token: 0x06008E6D RID: 36461 RVA: 0x003BE81C File Offset: 0x003BCA1C
	public void ToggleMinimap(bool fullscreen, bool holdOpen = false)
	{
		if (!fullscreen)
		{
			this.HeldOpen = false;
		}
		bool flag = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.RESOURCEFUL_RAT;
		if (this.PreventMinimap && !flag)
		{
			return;
		}
		if (flag)
		{
			this.cameraRef.cullingMask = 0;
		}
		GameUIRoot.Instance.notificationController.ForceHide();
		GameUIRoot.Instance.ToggleAllDefaultLabels(!fullscreen, "minimap");
		this.m_isFullscreen = fullscreen;
		this.HeldOpen = holdOpen;
		if (fullscreen)
		{
			this.m_cachedFadeValue = (float)((!this.m_isFaded) ? 1 : 0);
			this.m_mapMaskMaterial.SetFloat("_Fade", 1f);
			this.currentXRectFactor = 1f;
			this.currentYRectFactor = 1f;
		}
		else
		{
			this.m_mapMaskMaterial.SetFloat("_Fade", this.m_cachedFadeValue);
			this.currentXRectFactor = 0.25f;
			this.currentYRectFactor = 0.25f;
			BraveInput.ConsumeAllAcrossInstances(GungeonActions.GungeonActionType.Shoot);
		}
		Shader.SetGlobalFloat("_FullMapActive", (float)((!fullscreen) ? 0 : 1));
		this.UpdateScale();
		if (fullscreen)
		{
			AkSoundEngine.PostEvent("Play_UI_map_open_01", base.gameObject);
		}
		this.m_cameraPanOffset = Vector3.zero;
		if (fullscreen)
		{
			Pixelator.Instance.FadeColor = Color.black;
			Pixelator.Instance.fade = 0.3f;
			GameUIRoot.Instance.HideCoreUI(string.Empty);
			GameUIRoot.Instance.UnfoldGunventory(GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameUIRoot.Instance.ToggleItemPanels(false);
			}
			this.UIMinimap.ToggleState(true);
		}
		else
		{
			Pixelator.Instance.FadeColor = Color.black;
			Pixelator.Instance.fade = 1f;
			GameUIRoot.Instance.ShowCoreUI(string.Empty);
			GameUIRoot.Instance.RefoldGunventory();
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameUIRoot.Instance.ToggleItemPanels(true);
			}
			this.UIMinimap.ToggleState(false);
		}
		if (this.m_isFullscreen)
		{
			this.m_cameraBasePosition = this.GetCameraBasePosition();
			this.cameraRef.transform.position = this.m_cameraBasePosition + this.m_cameraPanOffset;
		}
	}

	// Token: 0x06008E6E RID: 36462 RVA: 0x003BEA68 File Offset: 0x003BCC68
	private Vector3 GetCameraBasePosition()
	{
		if (this.m_playerMarkers == null || this.m_playerMarkers.Count == 0)
		{
			return Vector3.zero;
		}
		Vector3 vector = Vector3.zero;
		int num = 0;
		for (int i = 0; i < this.m_playerMarkers.Count; i++)
		{
			if (i != 0 || !(GameManager.Instance.PrimaryPlayer != null) || !GameManager.Instance.PrimaryPlayer.healthHaver.IsDead)
			{
				if (i != 1 || !(GameManager.Instance.SecondaryPlayer != null) || !GameManager.Instance.SecondaryPlayer.healthHaver.IsDead)
				{
					num++;
					vector += this.m_playerMarkers[i].First.position;
				}
			}
		}
		vector /= (float)num;
		return vector.WithZ(-5f);
	}

	// Token: 0x06008E6F RID: 36463 RVA: 0x003BEB64 File Offset: 0x003BCD64
	public void AttemptPanCamera(Vector3 delta)
	{
		this.m_cameraPanOffset += delta * this.cameraRef.orthographicSize;
	}

	// Token: 0x17001558 RID: 5464
	// (get) Token: 0x06008E70 RID: 36464 RVA: 0x003BEB88 File Offset: 0x003BCD88
	public bool IsPanning
	{
		get
		{
			return this.m_isAutoPanning;
		}
	}

	// Token: 0x06008E71 RID: 36465 RVA: 0x003BEB90 File Offset: 0x003BCD90
	public void PanToPosition(Vector3 position)
	{
		base.StartCoroutine(this.HandleAutoPan((position - this.m_cameraBasePosition).WithZ(0f)));
	}

	// Token: 0x06008E72 RID: 36466 RVA: 0x003BEBB8 File Offset: 0x003BCDB8
	private IEnumerator HandleAutoPan(Vector3 targetPan)
	{
		while (this.m_isAutoPanning)
		{
			yield return null;
		}
		this.m_isAutoPanning = true;
		float elapsed = 0f;
		float duration = 0.2f;
		Vector3 startPan = this.m_cameraPanOffset;
		while (elapsed < duration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			this.m_cameraPanOffset = Vector3.Lerp(startPan, targetPan, t);
			yield return null;
		}
		this.m_isAutoPanning = false;
		yield break;
	}

	// Token: 0x06008E73 RID: 36467 RVA: 0x003BEBDC File Offset: 0x003BCDDC
	public void TogglePresetZoomValue()
	{
		if (Minimap.m_cameraOrthoOffset == 0f)
		{
			Minimap.m_cameraOrthoOffset = 4.25f;
		}
		else if (Minimap.m_cameraOrthoOffset == 4.25f)
		{
			Minimap.m_cameraOrthoOffset = 8.5f;
		}
		else if (Minimap.m_cameraOrthoOffset == 8.5f)
		{
			Minimap.m_cameraOrthoOffset = 0f;
		}
		else
		{
			Minimap.m_cameraOrthoOffset = 0f;
		}
		GameManager.Options.PreferredMapZoom = Minimap.m_cameraOrthoOffset;
	}

	// Token: 0x06008E74 RID: 36468 RVA: 0x003BEC5C File Offset: 0x003BCE5C
	public void AttemptZoomCamera(float zoom)
	{
		Minimap.m_cameraOrthoOffset = Mathf.Clamp(Minimap.m_cameraOrthoOffset + zoom * 2f, -2f, 9f);
		GameManager.Options.PreferredMapZoom = Minimap.m_cameraOrthoOffset;
	}

	// Token: 0x06008E75 RID: 36469 RVA: 0x003BEC90 File Offset: 0x003BCE90
	public void AttemptZoomMinimap(float zoom)
	{
		this.m_currentMinimapZoom = Mathf.Clamp(this.m_currentMinimapZoom + zoom * 2f, -1f, 4f);
		GameManager.Options.PreferredMinimapZoom = this.m_currentMinimapZoom;
	}

	// Token: 0x17001559 RID: 5465
	// (get) Token: 0x06008E76 RID: 36470 RVA: 0x003BECC8 File Offset: 0x003BCEC8
	private bool PreventMinimap
	{
		get
		{
			return GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.RESOURCEFUL_RAT || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES || (GameUIRoot.Instance && GameUIRoot.Instance.DisplayingConversationBar) || this.TemporarilyPreventMinimap;
		}
	}

	// Token: 0x06008E77 RID: 36471 RVA: 0x003BED34 File Offset: 0x003BCF34
	public void InitializeMinimap(DungeonData data)
	{
		if (this.PreventMinimap)
		{
			return;
		}
		TK2DDungeonAssembler.RuntimeResizeTileMap(this.tilemap, data.Width, data.Height, this.tilemap.partitionSizeX, this.tilemap.partitionSizeY);
		for (int i = 0; i < data.Width; i++)
		{
			for (int j = 0; j < data.Height; j++)
			{
				Color color = new Color(1f, 1f, 1f, 0.75f);
				this.AssignColorToTile(i, j, 0, color, this.tilemap);
			}
		}
		this.tilemap.ForceBuild();
		float num = (float)data.Width / 2f * 0.125f;
		float num2 = (float)data.Height / 2f * 0.125f;
		this.m_cameraBasePosition = this.tilemap.transform.position + new Vector3(num, num2, -5f);
		this.cameraRef.transform.position = this.m_cameraBasePosition;
	}

	// Token: 0x06008E78 RID: 36472 RVA: 0x003BEE4C File Offset: 0x003BD04C
	public void UpdatePlayerPositionExact(Vector3 worldPosition, PlayerController player, bool isDying = false)
	{
		if (this.PreventMinimap)
		{
			return;
		}
		if (this.m_playerMarkers == null)
		{
			this.m_playerMarkers = new List<Tuple<Transform, Renderer>>();
		}
		if (this.m_playerMarkers.Count == 0)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.AllPlayers[i].minimapIconPrefab, base.transform.position, Quaternion.identity);
				gameObject.transform.parent = base.transform;
				tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
				Tuple<Transform, Renderer> tuple = new Tuple<Transform, Renderer>(gameObject.transform, component.renderer);
				this.m_playerMarkers.Add(tuple);
				if (component != null)
				{
					component.renderer.sortingLayerName = "Foreground";
				}
			}
		}
		Vector3 vector = base.transform.position + new Vector3(worldPosition.x * 0.125f, worldPosition.y * 0.125f, -1f);
		int num = ((!player.IsPrimaryPlayer) ? 1 : 0);
		if (player && player.CurrentRoom != null && player.CurrentRoom.PreventMinimapUpdates)
		{
			if (num < this.m_playerMarkers.Count)
			{
				this.m_playerMarkers[num].Second.enabled = false;
			}
			if (!this.m_isFullscreen)
			{
				this.m_cameraBasePosition = this.GetCameraBasePosition().Quantize(0.0625f) + CameraController.PLATFORM_CAMERA_OFFSET;
			}
			this.cameraRef.transform.position = this.m_cameraBasePosition + this.m_cameraPanOffset;
			return;
		}
		if (num < this.m_playerMarkers.Count)
		{
			this.m_playerMarkers[num].First.position = vector.Quantize(0.0625f);
			if (isDying || player.healthHaver.IsDead)
			{
				this.m_playerMarkers[num].Second.enabled = false;
			}
			else
			{
				this.m_playerMarkers[num].Second.enabled = true;
			}
		}
		if (!this.m_isFullscreen)
		{
			this.m_cameraBasePosition = this.GetCameraBasePosition().Quantize(0.0625f) + CameraController.PLATFORM_CAMERA_OFFSET;
		}
		this.cameraRef.transform.position = this.m_cameraBasePosition + this.m_cameraPanOffset;
	}

	// Token: 0x06008E79 RID: 36473 RVA: 0x003BF0D4 File Offset: 0x003BD2D4
	private void PixelQuantizeCameraPosition()
	{
		Vector3 position = this.cameraRef.transform.position;
		float num = 1f / (this.cameraRef.orthographicSize * 2f * 16f);
		float num2 = 16f / (this.cameraRef.orthographicSize * 2f * 16f * 9f);
		this.cameraRef.transform.position = position.WithX(BraveMathCollege.QuantizeFloat(position.x, num2)).WithY(BraveMathCollege.QuantizeFloat(position.y, num));
	}

	// Token: 0x06008E7A RID: 36474 RVA: 0x003BF16C File Offset: 0x003BD36C
	public void RevealAllRooms(bool revealSecretRooms)
	{
		if (this.PreventMinimap)
		{
			return;
		}
		for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
		{
			RoomHandler roomHandler = GameManager.Instance.Dungeon.data.rooms[i];
			if (roomHandler.connectedRooms.Count != 0)
			{
				if (revealSecretRooms || roomHandler.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.SECRET)
				{
					roomHandler.RevealedOnMap = true;
					this.RevealMinimapRoom(roomHandler, true, false, roomHandler == GameManager.Instance.PrimaryPlayer.CurrentRoom);
				}
			}
		}
		for (int j = 0; j < GameManager.Instance.Dungeon.data.rooms.Count; j++)
		{
			RoomHandler roomHandler2 = GameManager.Instance.Dungeon.data.rooms[j];
			if (roomHandler2.connectedRooms.Count != 0)
			{
				if (this.roomToUnknownIconMap.ContainsKey(roomHandler2))
				{
					UnityEngine.Object.Destroy(this.roomToUnknownIconMap[roomHandler2]);
				}
			}
		}
		this.roomToUnknownIconMap.Clear();
		base.StartCoroutine(this.DelayedMarkDirty());
	}

	// Token: 0x06008E7B RID: 36475 RVA: 0x003BF2AC File Offset: 0x003BD4AC
	private IEnumerator DelayedMarkDirty()
	{
		yield return null;
		this.m_shouldBuildTilemap = true;
		yield break;
	}

	// Token: 0x06008E7C RID: 36476 RVA: 0x003BF2C8 File Offset: 0x003BD4C8
	public void DeflagPreviousRoom(RoomHandler previousRoom)
	{
		if (this.PreventMinimap)
		{
			return;
		}
		this.RevealMinimapRoom(previousRoom, true, true, false);
	}

	// Token: 0x06008E7D RID: 36477 RVA: 0x003BF2E0 File Offset: 0x003BD4E0
	private void DrawUnknownRoomSquare(RoomHandler current, RoomHandler adjacent, bool doBuild = true, int overrideCellIndex = -1, bool isLockedDoor = false)
	{
		if (this.PreventMinimap)
		{
			return;
		}
		if (adjacent.IsSecretRoom)
		{
			return;
		}
		if (adjacent.RevealedOnMap)
		{
			return;
		}
		int num = ((overrideCellIndex == -1) ? 49 : overrideCellIndex);
		RuntimeExitDefinition exitDefinitionForConnectedRoom = adjacent.GetExitDefinitionForConnectedRoom(current);
		IntVector2 cellAdjacentToExit = adjacent.GetCellAdjacentToExit(exitDefinitionForConnectedRoom);
		IntVector2 intVector = IntVector2.Zero;
		RuntimeRoomExitData runtimeRoomExitData = ((exitDefinitionForConnectedRoom.upstreamRoom != adjacent) ? exitDefinitionForConnectedRoom.downstreamExit : exitDefinitionForConnectedRoom.upstreamExit);
		if (runtimeRoomExitData != null && runtimeRoomExitData.referencedExit != null)
		{
			intVector = DungeonData.GetIntVector2FromDirection(runtimeRoomExitData.referencedExit.exitDirection);
		}
		if (cellAdjacentToExit == IntVector2.Zero)
		{
			return;
		}
		for (int i = -1; i < 3; i++)
		{
			for (int j = -1; j < 3; j++)
			{
				this.tilemap.SetTile(cellAdjacentToExit.x + i, cellAdjacentToExit.y + j, 0, num);
			}
		}
		IntVector2 intVector2 = cellAdjacentToExit + IntVector2.Left;
		IntVector2 intVector3 = cellAdjacentToExit + IntVector2.Left;
		GameObject gameObject = null;
		GameObject gameObject2;
		if (!adjacent.area.IsProceduralRoom && adjacent.area.runtimePrototypeData.associatedMinimapIcon != null)
		{
			gameObject2 = UnityEngine.Object.Instantiate<GameObject>(adjacent.area.runtimePrototypeData.associatedMinimapIcon);
			if (isLockedDoor)
			{
				gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Minimap_Locked_Icon", ".prefab"));
				intVector3 = intVector3 + IntVector2.Right + IntVector2.Down + intVector * 6;
			}
		}
		else if (isLockedDoor)
		{
			gameObject2 = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Minimap_Locked_Icon", ".prefab"));
			intVector2 = intVector2 + IntVector2.Right + IntVector2.Down;
		}
		else if (overrideCellIndex != -1)
		{
			gameObject2 = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Minimap_Blocked_Icon", ".prefab"));
		}
		else
		{
			gameObject2 = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Minimap_Unknown_Icon", ".prefab"));
		}
		gameObject2.transform.parent = base.transform;
		gameObject2.transform.position = base.transform.position + intVector2.ToVector3() * 0.125f;
		if (this.roomToUnknownIconMap.ContainsKey(adjacent))
		{
			gameObject2.transform.parent = this.roomToUnknownIconMap[adjacent].transform;
		}
		else
		{
			this.roomToUnknownIconMap.Add(adjacent, gameObject2);
		}
		if (gameObject != null)
		{
			gameObject.transform.parent = this.roomToUnknownIconMap[adjacent].transform;
			gameObject.transform.position = base.transform.position + intVector3.ToVector3() * 0.125f;
		}
	}

	// Token: 0x06008E7E RID: 36478 RVA: 0x003BF5E0 File Offset: 0x003BD7E0
	private void UpdateTeleporterIconForRoom(RoomHandler targetRoom)
	{
		if (this.PreventMinimap)
		{
			return;
		}
		if (this.roomToTeleportIconMap.ContainsKey(targetRoom) && targetRoom.TeleportersActive)
		{
			GameObject gameObject = this.roomToTeleportIconMap[targetRoom];
			tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
			if (component.GetCurrentSpriteDef().name == "teleport_001")
			{
				component.SetSprite("teleport_active_001");
			}
		}
	}

	// Token: 0x06008E7F RID: 36479 RVA: 0x003BF650 File Offset: 0x003BD850
	public void RevealMinimapRoom(RoomHandler revealedRoom, bool force = false, bool doBuild = true, bool isCurrentRoom = true)
	{
		if (revealedRoom.OverrideTilemap)
		{
			return;
		}
		base.StartCoroutine(this.RevealMinimapRoomInternal(revealedRoom, force, doBuild, isCurrentRoom));
	}

	// Token: 0x06008E80 RID: 36480 RVA: 0x003BF678 File Offset: 0x003BD878
	public IEnumerator RevealMinimapRoomInternal(RoomHandler revealedRoom, bool force = false, bool doBuild = true, bool isCurrentRoom = true)
	{
		revealedRoom.RevealedOnMap = true;
		yield return null;
		if (!this.m_isInitialized)
		{
			this.HandleInitialization();
		}
		if (this.PreventMinimap)
		{
			yield break;
		}
		if (this.m_revealProcessedMap == null)
		{
			this.m_revealProcessedMap = new bool[GameManager.Instance.Dungeon.data.Width, GameManager.Instance.Dungeon.data.Height];
		}
		else
		{
			Array.Clear(this.m_revealProcessedMap, 0, this.m_revealProcessedMap.GetLength(0) * this.m_revealProcessedMap.GetLength(1));
		}
		int assignedDefaultIndex = ((!isCurrentRoom) ? 49 : 50);
		if (revealedRoom.visibility != RoomHandler.VisibilityStatus.CURRENT && !force)
		{
			yield break;
		}
		if (revealedRoom.visibility == RoomHandler.VisibilityStatus.OBSCURED && revealedRoom.RevealedOnMap)
		{
			assignedDefaultIndex = -1;
		}
		IntVector2[] cardinals = IntVector2.CardinalsAndOrdinals;
		DungeonData data = GameManager.Instance.Dungeon.data;
		HashSet<IntVector2> AllCells = revealedRoom.GetCellsAndAllConnectedExitsSlow(false);
		DungeonData dungeonData = GameManager.Instance.Dungeon.data;
		foreach (IntVector2 intVector in AllCells)
		{
			int num = assignedDefaultIndex;
			int num2 = 0;
			if (data[intVector] != null)
			{
				if (!data[intVector].isSecretRoomCell && !data[intVector].isWallMimicHideout)
				{
					if (data[intVector].isExitCell)
					{
						TileIndexGrid tileIndexGrid = this.darkIndexGrid;
						if (data[intVector].exitDoor != null && data[intVector].exitDoor.isLocked)
						{
							tileIndexGrid = this.redIndexGrid;
						}
						if (data[intVector].exitDoor != null && data[intVector].exitDoor.OneWayDoor && data[intVector].exitDoor.IsSealed)
						{
							tileIndexGrid = this.redIndexGrid;
						}
						IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
						num = tileIndexGrid.GetIndexGivenSides(dungeonData[intVector + cardinalsAndOrdinals[0]].type == CellType.WALL, dungeonData[intVector + cardinalsAndOrdinals[1]].type == CellType.WALL, dungeonData[intVector + cardinalsAndOrdinals[2]].type == CellType.WALL, dungeonData[intVector + cardinalsAndOrdinals[3]].type == CellType.WALL, dungeonData[intVector + cardinalsAndOrdinals[4]].type == CellType.WALL, dungeonData[intVector + cardinalsAndOrdinals[5]].type == CellType.WALL, dungeonData[intVector + cardinalsAndOrdinals[6]].type == CellType.WALL, dungeonData[intVector + cardinalsAndOrdinals[7]].type == CellType.WALL);
						num2 = 1;
					}
					else if (intVector.x >= 0 && intVector.y >= 0 && intVector.x < this.m_revealProcessedMap.GetLength(0) && intVector.y < this.m_revealProcessedMap.GetLength(1))
					{
						this.m_revealProcessedMap[intVector.x, intVector.y] = true;
					}
					this.tilemap.SetTile(intVector.x, intVector.y, num2, num);
				}
			}
		}
		foreach (IntVector2 intVector2 in AllCells)
		{
			for (int i = 0; i < cardinals.Length; i++)
			{
				IntVector2 intVector3 = intVector2 + cardinals[i];
				if (intVector3.x >= 0 && intVector3.x < this.m_revealProcessedMap.GetLength(0) && intVector3.y >= 0 && intVector3.y < this.m_revealProcessedMap.GetLength(1))
				{
					if (!this.m_revealProcessedMap[intVector3.x, intVector3.y])
					{
						this.m_revealProcessedMap[intVector3.x, intVector3.y] = true;
						CellData cellData = data[intVector3];
						if (cellData.isWallMimicHideout || cellData.type == CellType.WALL || cellData.isExitCell || cellData.isSecretRoomCell)
						{
							List<CellData> cellNeighbors = data.GetCellNeighbors(cellData, true);
							TileIndexGrid tileIndexGrid2 = ((revealedRoom.visibility != RoomHandler.VisibilityStatus.OBSCURED) ? this.indexGrid : this.CurrentRoomBorderIndexGrid);
							int indexGivenSides = tileIndexGrid2.GetIndexGivenSides(cellNeighbors[0] != null && cellNeighbors[0].type != CellType.WALL && !cellNeighbors[0].isExitCell && !cellNeighbors[0].isWallMimicHideout, cellNeighbors[1] != null && cellNeighbors[1].type != CellType.WALL && !cellNeighbors[1].isExitCell && !cellNeighbors[1].isWallMimicHideout, cellNeighbors[2] != null && cellNeighbors[2].type != CellType.WALL && !cellNeighbors[2].isExitCell && !cellNeighbors[2].isWallMimicHideout, cellNeighbors[3] != null && cellNeighbors[3].type != CellType.WALL && !cellNeighbors[3].isExitCell && !cellNeighbors[3].isWallMimicHideout, cellNeighbors[4] != null && cellNeighbors[4].type != CellType.WALL && !cellNeighbors[4].isExitCell && !cellNeighbors[4].isWallMimicHideout, cellNeighbors[5] != null && cellNeighbors[5].type != CellType.WALL && !cellNeighbors[5].isExitCell && !cellNeighbors[5].isWallMimicHideout, cellNeighbors[6] != null && cellNeighbors[6].type != CellType.WALL && !cellNeighbors[6].isExitCell && !cellNeighbors[6].isWallMimicHideout, cellNeighbors[7] != null && cellNeighbors[7].type != CellType.WALL && !cellNeighbors[7].isExitCell && !cellNeighbors[7].isWallMimicHideout);
							if ((cellNeighbors[0] == null || cellNeighbors[0].type != CellType.FLOOR || cellNeighbors[0].parentRoom == revealedRoom || cellNeighbors[0].isExitCell) && (cellNeighbors[1] == null || cellNeighbors[1].type != CellType.FLOOR || cellNeighbors[1].parentRoom == revealedRoom || cellNeighbors[1].isExitCell) && (cellNeighbors[2] == null || cellNeighbors[2].type != CellType.FLOOR || cellNeighbors[2].parentRoom == revealedRoom || cellNeighbors[2].isExitCell) && (cellNeighbors[3] == null || cellNeighbors[3].type != CellType.FLOOR || cellNeighbors[3].parentRoom == revealedRoom || cellNeighbors[3].isExitCell) && (cellNeighbors[4] == null || cellNeighbors[4].type != CellType.FLOOR || cellNeighbors[4].parentRoom == revealedRoom || cellNeighbors[4].isExitCell) && (cellNeighbors[5] == null || cellNeighbors[5].type != CellType.FLOOR || cellNeighbors[5].parentRoom == revealedRoom || cellNeighbors[5].isExitCell) && (cellNeighbors[6] == null || cellNeighbors[6].type != CellType.FLOOR || cellNeighbors[6].parentRoom == revealedRoom || cellNeighbors[6].isExitCell) && (cellNeighbors[7] == null || cellNeighbors[7].type != CellType.FLOOR || cellNeighbors[7].parentRoom == revealedRoom || cellNeighbors[7].isExitCell))
							{
								this.tilemap.SetTile(intVector3.x, intVector3.y, 0, indexGivenSides);
							}
						}
					}
				}
			}
		}
		for (int j = 0; j < revealedRoom.connectedRooms.Count; j++)
		{
			if (revealedRoom.connectedRooms[j].visibility == RoomHandler.VisibilityStatus.OBSCURED && !force)
			{
				int num3 = -1;
				RuntimeExitDefinition exitDefinitionForConnectedRoom = revealedRoom.GetExitDefinitionForConnectedRoom(revealedRoom.connectedRooms[j]);
				if (exitDefinitionForConnectedRoom.linkedDoor != null && exitDefinitionForConnectedRoom.linkedDoor.OneWayDoor && exitDefinitionForConnectedRoom.linkedDoor.IsSealed)
				{
					num3 = this.redIndexGrid.centerIndices.GetIndexByWeight();
				}
				if (exitDefinitionForConnectedRoom.linkedDoor != null && exitDefinitionForConnectedRoom.linkedDoor.isLocked)
				{
					num3 = this.redIndexGrid.centerIndices.GetIndexByWeight();
				}
				this.DrawUnknownRoomSquare(revealedRoom, revealedRoom.connectedRooms[j], true, num3, exitDefinitionForConnectedRoom.linkedDoor != null && exitDefinitionForConnectedRoom.linkedDoor.isLocked);
			}
		}
		if (this.roomToUnknownIconMap.ContainsKey(revealedRoom))
		{
			UnityEngine.Object.Destroy(this.roomToUnknownIconMap[revealedRoom]);
			this.roomToUnknownIconMap.Remove(revealedRoom);
		}
		if (this.roomToIconsMap.ContainsKey(revealedRoom))
		{
			for (int k = 0; k < this.roomToIconsMap[revealedRoom].Count; k++)
			{
				this.roomToIconsMap[revealedRoom][k].SetActive(true);
			}
		}
		if (this.roomToTeleportIconMap.ContainsKey(revealedRoom))
		{
			this.roomToTeleportIconMap[revealedRoom].SetActive(true);
		}
		this.UpdateTeleporterIconForRoom(revealedRoom);
		yield return null;
		if (doBuild)
		{
			this.m_shouldBuildTilemap = true;
		}
		yield break;
	}

	// Token: 0x06008E81 RID: 36481 RVA: 0x003BF6B0 File Offset: 0x003BD8B0
	private void Start()
	{
		Minimap.m_cameraOrthoOffset = GameManager.Options.PreferredMapZoom;
		this.m_currentMinimapZoom = GameManager.Options.PreferredMinimapZoom;
		this.HandleInitialization();
	}

	// Token: 0x06008E82 RID: 36482 RVA: 0x003BF6D8 File Offset: 0x003BD8D8
	private void HandleInitialization()
	{
		if (this.m_isInitialized)
		{
			return;
		}
		this.m_isInitialized = true;
		this.cameraRef.enabled = true;
		this.m_mapMaskMaterial = this.cameraRef.GetComponent<MinimapRenderer>().QuadTransform.GetComponent<MeshRenderer>().sharedMaterial;
		this.m_whiteTex = new Texture2D(1, 1);
		this.m_whiteTex.SetPixel(0, 0, Color.white);
		this.m_whiteTex.Apply();
		if (GameManager.Instance.IsFoyer || this.MinimapMode == Minimap.MinimapDisplayMode.NEVER)
		{
			this.m_isFaded = true;
			this.m_cachedFadeValue = 0f;
			this.m_mapMaskMaterial.SetFloat("_Fade", 0f);
		}
		this.ToggleMinimap(false, false);
	}

	// Token: 0x06008E83 RID: 36483 RVA: 0x003BF798 File Offset: 0x003BD998
	private void UpdateScale()
	{
		if (this.m_isFullscreen)
		{
			if (this.m_cameraOrthoBase != GameManager.Instance.MainCameraController.GetComponent<Camera>().orthographicSize)
			{
				this.m_cameraOrthoBase = GameManager.Instance.MainCameraController.GetComponent<Camera>().orthographicSize;
			}
			if (this.cameraRef.orthographicSize != this.m_cameraOrthoBase + Minimap.m_cameraOrthoOffset)
			{
				this.cameraRef.orthographicSize = this.m_cameraOrthoBase + Minimap.m_cameraOrthoOffset;
			}
			this.cameraRef.orthographicSize = BraveMathCollege.QuantizeFloat(this.cameraRef.orthographicSize, 0.5f);
		}
		else
		{
			this.cameraRef.orthographicSize = 2.109375f + this.m_currentMinimapZoom;
		}
	}

	// Token: 0x06008E84 RID: 36484 RVA: 0x003BF858 File Offset: 0x003BDA58
	private void LateUpdate()
	{
		this.UpdateScale();
		if (this.MinimapMode == Minimap.MinimapDisplayMode.NEVER || !Minimap.DoMinimap || this.TemporarilyPreventMinimap || GameManager.Instance.IsPaused)
		{
			if (!this.m_isFaded)
			{
				base.StartCoroutine(this.TransitionToNewFadeState(true));
			}
		}
		else if (this.MinimapMode == Minimap.MinimapDisplayMode.FADE_ON_ROOM_SEAL)
		{
			this.CheckRoomSealState();
		}
		else if (this.MinimapMode == Minimap.MinimapDisplayMode.ALWAYS && this.m_isFaded)
		{
			base.StartCoroutine(this.TransitionToNewFadeState(false));
		}
		bool flag = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.RESOURCEFUL_RAT;
		if (this.m_shouldBuildTilemap && !flag)
		{
			this.m_shouldBuildTilemap = false;
			this.tilemap.Build(tk2dTileMap.BuildFlags.Default);
		}
	}

	// Token: 0x06008E85 RID: 36485 RVA: 0x003BF928 File Offset: 0x003BDB28
	private void CheckRoomSealState()
	{
		if (GameManager.Instance.PrimaryPlayer == null)
		{
			return;
		}
		if (GameManager.Instance.IsFoyer)
		{
			return;
		}
		RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		if (currentRoom == null)
		{
			return;
		}
		if (currentRoom.IsSealed && !this.m_isFaded)
		{
			base.StartCoroutine(this.TransitionToNewFadeState(true));
		}
		else if (!currentRoom.IsSealed && this.m_isFaded)
		{
			base.StartCoroutine(this.TransitionToNewFadeState(false));
		}
		else if (currentRoom.IsSealed && this.m_isFaded && !this.m_isTransitioning)
		{
			this.m_mapMaskMaterial.SetFloat("_Fade", (float)((!this.m_isFullscreen) ? 0 : 1));
		}
		else if (!currentRoom.IsSealed && !this.m_isFaded && !this.m_isTransitioning)
		{
			this.m_mapMaskMaterial.SetFloat("_Fade", 1f);
		}
	}

	// Token: 0x06008E86 RID: 36486 RVA: 0x003BFA44 File Offset: 0x003BDC44
	private IEnumerator TransitionToNewFadeState(bool newFadeState)
	{
		this.m_isTransitioning = true;
		this.m_isFaded = newFadeState;
		float elapsed = 0f;
		float duration = 0.5f;
		while (elapsed < duration && this.m_isFaded == newFadeState && !this.m_isFullscreen)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / duration;
			if (this.m_isFaded)
			{
				t = 1f - t;
			}
			if (this.m_mapMaskMaterial)
			{
				this.m_mapMaskMaterial.SetFloat("_Fade", Mathf.Clamp01(t));
			}
			yield return null;
		}
		this.m_cachedFadeValue = (float)((!newFadeState) ? 1 : 0);
		this.m_isTransitioning = false;
		yield break;
	}

	// Token: 0x06008E87 RID: 36487 RVA: 0x003BFA68 File Offset: 0x003BDC68
	public RoomHandler CheckIconsNearCursor(Vector3 screenPosition, out GameObject icon)
	{
		Vector2 vector = BraveUtility.GetMinimapViewportPosition(screenPosition);
		vector.x = (vector.x - 0.5f) * (BraveCameraUtility.ASPECT / 1.7777778f) + 0.5f;
		Vector2 vector2 = (this.cameraRef.ViewportPointToRay(vector).origin.XY() - base.transform.position.XY()) * 8f;
		IntVector2 intVector = vector2.ToIntVector2(VectorConversions.Floor);
		if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector))
		{
			CellData cellData = GameManager.Instance.Dungeon.data[intVector];
			RoomHandler parentRoom = cellData.parentRoom;
			if (parentRoom != null && this.roomToTeleportIconMap.ContainsKey(parentRoom))
			{
				icon = this.roomToTeleportIconMap[parentRoom];
				return parentRoom;
			}
		}
		icon = null;
		return null;
	}

	// Token: 0x06008E88 RID: 36488 RVA: 0x003BFB5C File Offset: 0x003BDD5C
	public RoomHandler GetNearestVisibleRoom(Vector2 screenPosition, out float dist)
	{
		float num = screenPosition.x / (float)Screen.width;
		float num2 = screenPosition.y / (float)Screen.height;
		num = (num - 0.5f) / BraveCameraUtility.GetRect().width + 0.5f;
		num2 = (num2 - 0.5f) / BraveCameraUtility.GetRect().height + 0.5f;
		Vector2 vector = this.cameraRef.ViewportPointToRay(new Vector3(num, num2, 0f)).origin.XY();
		dist = float.MaxValue;
		RoomHandler roomHandler = null;
		foreach (RoomHandler roomHandler2 in this.roomToTeleportIconMap.Keys)
		{
			if (roomHandler2.TeleportersActive)
			{
				GameObject gameObject = this.roomToTeleportIconMap[roomHandler2];
				Debug.DrawLine(vector, gameObject.GetComponent<tk2dBaseSprite>().WorldCenter, Color.red, 5f);
				float num3 = Vector2.Distance(vector, gameObject.GetComponent<tk2dBaseSprite>().WorldCenter);
				if (num3 < dist)
				{
					dist = num3;
					roomHandler = roomHandler2;
				}
			}
		}
		return roomHandler;
	}

	// Token: 0x06008E89 RID: 36489 RVA: 0x003BFCB0 File Offset: 0x003BDEB0
	private void OrganizeExtantIcons(RoomHandler targetRoom, bool includeTeleIcon = false)
	{
		if (!this.roomToIconsMap.ContainsKey(targetRoom) && !this.roomToTeleportIconMap.ContainsKey(targetRoom))
		{
			Debug.LogError("ORGANIZING ROOM: " + targetRoom.GetRoomName() + " IN MINIMAP WITH NO ICONS, TELL BR.NET");
			return;
		}
		List<GameObject> list = ((!this.roomToIconsMap.ContainsKey(targetRoom)) ? null : this.roomToIconsMap[targetRoom]);
		if (this.roomHasMovedTeleportIcon.ContainsKey(targetRoom))
		{
			includeTeleIcon = true;
		}
		bool flag = this.roomToTeleportIconMap.ContainsKey(targetRoom);
		flag = flag && includeTeleIcon;
		int num = ((list != null) ? list.Count : 0) + ((!flag) ? 0 : 1);
		float num2 = 6f;
		float num3 = (float)(num - 1) * num2;
		float num4 = num3 / 2f;
		IntVector2 centerCell = targetRoom.GetCenterCell();
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = ((!flag || i != num - 1) ? list[i] : this.roomToTeleportIconMap[targetRoom]);
			if (gameObject)
			{
				tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
				if (component)
				{
					Vector3 vector = new Vector3(num2 * (float)i - num4, 0f, 0f);
					Vector3 vector2 = base.transform.position + (centerCell.ToVector3() + vector) * 0.125f + new Vector3(0f, 0f, 1f);
					component.PlaceAtPositionByAnchor(vector2, tk2dBaseSprite.Anchor.MiddleCenter);
				}
			}
		}
		for (int j = 0; j < num; j++)
		{
			GameObject gameObject2 = ((!flag || j != num - 1) ? list[j] : this.roomToTeleportIconMap[targetRoom]);
			if (gameObject2)
			{
				gameObject2.transform.position = gameObject2.transform.position.Quantize(0.015625f);
			}
		}
		if (!includeTeleIcon && this.roomToTeleportIconMap.ContainsKey(targetRoom) && num > 0)
		{
			tk2dBaseSprite component2 = this.roomToTeleportIconMap[targetRoom].GetComponent<tk2dBaseSprite>();
			float num5 = float.MaxValue;
			for (int k = 0; k < num; k++)
			{
				tk2dBaseSprite component3 = list[k].GetComponent<tk2dBaseSprite>();
				num5 = Mathf.Min(num5, Vector2.Distance(component3.WorldCenter, component2.WorldCenter));
			}
			if (num5 <= 0.375f)
			{
				this.roomHasMovedTeleportIcon.Add(targetRoom, true);
				this.OrganizeExtantIcons(targetRoom, true);
			}
		}
		else if (this.roomToTeleportIconMap.ContainsKey(targetRoom) && num == 0)
		{
			this.roomToTeleportIconMap[targetRoom].transform.position = this.roomToInitialTeleportIconPositionMap[targetRoom];
		}
	}

	// Token: 0x06008E8A RID: 36490 RVA: 0x003BFFAC File Offset: 0x003BE1AC
	private void AddIconToRoomList(RoomHandler room, GameObject instanceIcon)
	{
		if (this.roomToIconsMap.ContainsKey(room))
		{
			this.roomToIconsMap[room].Add(instanceIcon);
		}
		else
		{
			List<GameObject> list = new List<GameObject>();
			list.Add(instanceIcon);
			this.roomToIconsMap.Add(room, list);
		}
		this.OrganizeExtantIcons(room, false);
	}

	// Token: 0x06008E8B RID: 36491 RVA: 0x003C0004 File Offset: 0x003BE204
	private void RemoveIconFromRoomList(RoomHandler room, GameObject instanceIcon)
	{
		if (this.roomToIconsMap.ContainsKey(room) && this.roomToIconsMap[room].Remove(instanceIcon))
		{
			UnityEngine.Object.Destroy(instanceIcon);
			this.OrganizeExtantIcons(room, false);
		}
	}

	// Token: 0x06008E8C RID: 36492 RVA: 0x003C003C File Offset: 0x003BE23C
	public bool HasTeleporterIcon(RoomHandler room)
	{
		return this.roomToTeleportIconMap.ContainsKey(room);
	}

	// Token: 0x06008E8D RID: 36493 RVA: 0x003C004C File Offset: 0x003BE24C
	private void ClampIconToRoomBounds(RoomHandler room, GameObject instanceIcon, Vector2 placedPosition)
	{
		Vector2 vector = base.transform.position.XY() + room.area.basePosition.ToVector2() * 0.125f;
		Vector2 vector2 = base.transform.position.XY() + (room.area.basePosition.ToVector2() + room.area.dimensions.ToVector2()) * 0.125f;
		vector += Vector2.one * 0.5f;
		vector2 -= Vector2.one * 0.5f;
		placedPosition = BraveMathCollege.ClampToBounds(placedPosition, vector, vector2);
		instanceIcon.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(placedPosition, tk2dBaseSprite.Anchor.MiddleCenter);
	}

	// Token: 0x06008E8E RID: 36494 RVA: 0x003C0118 File Offset: 0x003BE318
	public void RegisterTeleportIcon(RoomHandler room, GameObject iconPrefab, Vector2 position)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(iconPrefab);
		Vector2 vector = base.transform.position + position.ToVector3ZUp(0f) * 0.125f + new Vector3(0f, 0f, 1f);
		gameObject.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(vector, tk2dBaseSprite.Anchor.MiddleCenter);
		this.ClampIconToRoomBounds(room, gameObject, vector);
		gameObject.transform.position = gameObject.transform.position.WithZ(-1f);
		gameObject.transform.parent = base.transform;
		gameObject.SetActive(room.visibility != RoomHandler.VisibilityStatus.OBSCURED);
		this.roomsContainingTeleporters.Add(room);
		this.roomToTeleportIconMap.Add(room, gameObject);
		this.roomToInitialTeleportIconPositionMap.Add(room, gameObject.transform.position);
		this.roomsContainingTeleporters.Sort(delegate(RoomHandler a, RoomHandler b)
		{
			Vector2 vector2 = this.roomToInitialTeleportIconPositionMap[a];
			Vector2 vector3 = this.roomToInitialTeleportIconPositionMap[b];
			if (vector2.y == vector3.y)
			{
				return vector2.x.CompareTo(vector3.x);
			}
			return vector2.y.CompareTo(vector3.y);
		});
		this.OrganizeExtantIcons(room, false);
	}

	// Token: 0x06008E8F RID: 36495 RVA: 0x003C0224 File Offset: 0x003BE424
	public GameObject RegisterRoomIcon(RoomHandler room, GameObject iconPrefab, bool forceActive = false)
	{
		if (iconPrefab == null)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(iconPrefab);
		gameObject.transform.position = gameObject.transform.position.WithZ(-1f);
		gameObject.transform.parent = base.transform;
		if (forceActive)
		{
			gameObject.SetActive(true);
		}
		else
		{
			gameObject.SetActive(room.visibility != RoomHandler.VisibilityStatus.OBSCURED);
		}
		this.AddIconToRoomList(room, gameObject);
		return gameObject;
	}

	// Token: 0x06008E90 RID: 36496 RVA: 0x003C02AC File Offset: 0x003BE4AC
	public void DeregisterRoomIcon(RoomHandler room, GameObject instanceIcon)
	{
		this.RemoveIconFromRoomList(room, instanceIcon);
	}

	// Token: 0x06008E91 RID: 36497 RVA: 0x003C02B8 File Offset: 0x003BE4B8
	public void OnDestroy()
	{
		Minimap.m_instance = null;
	}

	// Token: 0x06008E92 RID: 36498 RVA: 0x003C02C0 File Offset: 0x003BE4C0
	public RoomHandler NextSelectedTeleporter(ref int selectedIndex, int dir)
	{
		selectedIndex = Mathf.Clamp(selectedIndex, 0, this.RoomToTeleportMap.Count - 1);
		int num = selectedIndex;
		RoomHandler roomHandler;
		for (;;)
		{
			num = (num + dir + this.RoomToTeleportMap.Count) % this.RoomToTeleportMap.Count;
			roomHandler = this.roomsContainingTeleporters[num];
			if (roomHandler.TeleportersActive)
			{
				break;
			}
			if (num == selectedIndex)
			{
				goto Block_2;
			}
		}
		selectedIndex = num;
		return roomHandler;
		Block_2:
		return null;
	}

	// Token: 0x04009659 RID: 38489
	[NonSerialized]
	public bool PreventAllTeleports;

	// Token: 0x0400965A RID: 38490
	public tk2dTileMap tilemap;

	// Token: 0x0400965B RID: 38491
	public TileIndexGrid indexGrid;

	// Token: 0x0400965C RID: 38492
	public TileIndexGrid darkIndexGrid;

	// Token: 0x0400965D RID: 38493
	public TileIndexGrid redIndexGrid;

	// Token: 0x0400965E RID: 38494
	public TileIndexGrid CurrentRoomBorderIndexGrid;

	// Token: 0x0400965F RID: 38495
	public Camera cameraRef;

	// Token: 0x04009660 RID: 38496
	public MinimapUIController UIMinimap;

	// Token: 0x04009661 RID: 38497
	public float targetSaturation = 0.3f;

	// Token: 0x04009662 RID: 38498
	[NonSerialized]
	public float currentXRectFactor = 1f;

	// Token: 0x04009663 RID: 38499
	[NonSerialized]
	public float currentYRectFactor = 1f;

	// Token: 0x04009664 RID: 38500
	private bool[,] m_simplifiedData;

	// Token: 0x04009665 RID: 38501
	private List<Tuple<Transform, Renderer>> m_playerMarkers = new List<Tuple<Transform, Renderer>>();

	// Token: 0x04009666 RID: 38502
	private static float SCALE_FACTOR = 15f;

	// Token: 0x04009667 RID: 38503
	[SerializeField]
	private Material m_mapMaskMaterial;

	// Token: 0x04009668 RID: 38504
	private Texture m_itemsMaskTex;

	// Token: 0x04009669 RID: 38505
	private Texture2D m_whiteTex;

	// Token: 0x0400966A RID: 38506
	private Dictionary<RoomHandler, List<GameObject>> roomToIconsMap = new Dictionary<RoomHandler, List<GameObject>>();

	// Token: 0x0400966B RID: 38507
	private Dictionary<RoomHandler, bool> roomHasMovedTeleportIcon = new Dictionary<RoomHandler, bool>();

	// Token: 0x0400966C RID: 38508
	private Dictionary<RoomHandler, GameObject> roomToTeleportIconMap = new Dictionary<RoomHandler, GameObject>();

	// Token: 0x0400966D RID: 38509
	private Dictionary<RoomHandler, Vector3> roomToInitialTeleportIconPositionMap = new Dictionary<RoomHandler, Vector3>();

	// Token: 0x0400966E RID: 38510
	public List<RoomHandler> roomsContainingTeleporters = new List<RoomHandler>();

	// Token: 0x0400966F RID: 38511
	private Dictionary<RoomHandler, GameObject> roomToUnknownIconMap = new Dictionary<RoomHandler, GameObject>();

	// Token: 0x04009670 RID: 38512
	private Vector3 m_cameraBasePosition;

	// Token: 0x04009671 RID: 38513
	private Vector3 m_cameraPanOffset;

	// Token: 0x04009672 RID: 38514
	private float m_cameraOrthoBase;

	// Token: 0x04009673 RID: 38515
	private static float m_cameraOrthoOffset;

	// Token: 0x04009674 RID: 38516
	private float m_currentMinimapZoom;

	// Token: 0x04009675 RID: 38517
	private bool m_isFullscreen;

	// Token: 0x04009676 RID: 38518
	private static Minimap m_instance;

	// Token: 0x04009678 RID: 38520
	private bool m_isAutoPanning;

	// Token: 0x04009679 RID: 38521
	public bool TemporarilyPreventMinimap;

	// Token: 0x0400967A RID: 38522
	private bool[,] m_revealProcessedMap;

	// Token: 0x0400967B RID: 38523
	protected bool m_shouldBuildTilemap;

	// Token: 0x0400967C RID: 38524
	protected bool m_isInitialized;

	// Token: 0x0400967D RID: 38525
	private float m_cachedFadeValue = 1f;

	// Token: 0x0400967E RID: 38526
	private bool m_isFaded;

	// Token: 0x0400967F RID: 38527
	private bool m_isTransitioning;

	// Token: 0x020017C1 RID: 6081
	public enum MinimapDisplayMode
	{
		// Token: 0x04009681 RID: 38529
		NEVER,
		// Token: 0x04009682 RID: 38530
		ALWAYS,
		// Token: 0x04009683 RID: 38531
		FADE_ON_ROOM_SEAL
	}
}
