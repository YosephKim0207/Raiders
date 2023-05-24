using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001202 RID: 4610
public class SecretRoomManager : MonoBehaviour
{
	// Token: 0x17000F43 RID: 3907
	// (get) Token: 0x06006718 RID: 26392 RVA: 0x00284E20 File Offset: 0x00283020
	// (set) Token: 0x06006719 RID: 26393 RVA: 0x00284E28 File Offset: 0x00283028
	public bool IsOpen
	{
		get
		{
			return this.m_isOpen;
		}
		set
		{
			if (!this.m_isOpen && value)
			{
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.SECRET_ROOMS_FOUND, 1f);
			}
			this.m_isOpen = value;
		}
	}

	// Token: 0x17000F44 RID: 3908
	// (get) Token: 0x0600671A RID: 26394 RVA: 0x00284E54 File Offset: 0x00283054
	public bool OpenedByExplosion
	{
		get
		{
			return this.revealStyle != SecretRoomManager.SecretRoomRevealStyle.ComplexPuzzle;
		}
	}

	// Token: 0x0600671B RID: 26395 RVA: 0x00284E64 File Offset: 0x00283064
	public void InitializeCells(List<IntVector2> ceilingCellList)
	{
		this.ceilingCells = ceilingCellList;
	}

	// Token: 0x0600671C RID: 26396 RVA: 0x00284E70 File Offset: 0x00283070
	public void InitializeForStyle()
	{
		for (int i = 0; i < this.doorObjects.Count; i++)
		{
			this.doorObjects[i].manager = this;
		}
		switch (this.revealStyle)
		{
		case SecretRoomManager.SecretRoomRevealStyle.Simple:
			this.InitializeSimple();
			break;
		case SecretRoomManager.SecretRoomRevealStyle.ComplexPuzzle:
			this.InitializeSecretRoomPuzzle();
			break;
		case SecretRoomManager.SecretRoomRevealStyle.ShootToBreak:
			this.InitializeShootToBreak();
			break;
		}
		for (int j = 0; j < this.doorObjects.Count; j++)
		{
			this.doorObjects[j].GeneratePotentiallyNecessaryShards();
		}
		for (int k = 0; k < this.doorObjects.Count; k++)
		{
			this.doorObjects[k].exitDef.GenerateSecretRoomBlocker(GameManager.Instance.Dungeon.data, this, this.doorObjects[k], null);
		}
	}

	// Token: 0x0600671D RID: 26397 RVA: 0x00284F74 File Offset: 0x00283174
	public void HandleDoorBrokenOpen(SecretRoomDoorBeer doorBroken)
	{
		this.ceilingRenderer.enabled = false;
		if (this.borderRenderer != null)
		{
			this.borderRenderer.enabled = false;
		}
		for (int i = 0; i < this.ceilingCells.Count; i++)
		{
			if (GameManager.Instance.Dungeon.data[this.ceilingCells[i]] != null)
			{
				GameManager.Instance.Dungeon.data[this.ceilingCells[i]].isSecretRoomCell = false;
			}
		}
		for (int j = 0; j < this.doorObjects.Count; j++)
		{
			foreach (IntVector2 intVector in this.doorObjects[j].exitDef.GetCellsForRoom(this.room))
			{
				GameManager.Instance.Dungeon.data[intVector].isSecretRoomCell = false;
			}
			foreach (IntVector2 intVector2 in this.doorObjects[j].exitDef.GetCellsForOtherRoom(this.room))
			{
				GameManager.Instance.Dungeon.data[intVector2].isSecretRoomCell = false;
			}
		}
		for (int k = 0; k < this.doorObjects.Count; k++)
		{
			if (this.doorObjects[k].subsidiaryBlocker != null)
			{
				this.doorObjects[k].subsidiaryBlocker.ToggleRenderers(true);
			}
		}
		this.room.visibility = RoomHandler.VisibilityStatus.VISITED;
		Minimap.Instance.RevealMinimapRoom(this.room, true, true, false);
		Pixelator.Instance.ProcessOcclusionChange(doorBroken.transform.position.IntXY(VectorConversions.Round), 0.3f, this.room, true);
		for (int l = 0; l < this.doorObjects.Count; l++)
		{
			Pixelator.Instance.ProcessRoomAdditionalExits(this.doorObjects[l].exitDef.GetUpstreamBasePosition(), this.doorObjects[l].linkedRoom, false);
		}
		doorBroken.gameObject.SetActive(false);
		this.OnFinishedOpeningDoors();
		if (this.m_simpleTrigger != null)
		{
			this.m_simpleTrigger.Disable();
		}
	}

	// Token: 0x0600671E RID: 26398 RVA: 0x0028523C File Offset: 0x0028343C
	protected void InitializeSimple()
	{
		for (int i = 0; i < this.doorObjects.Count; i++)
		{
			GameObject colliderObject = this.doorObjects[i].collider.colliderObject;
			IntVector2 intVector = colliderObject.transform.position.IntXY(VectorConversions.Floor);
			IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(this.doorObjects[i].collider.exitDirection);
			CellData cellData = GameManager.Instance.Dungeon.data[intVector];
			RoomHandler roomHandler = ((cellData.parentRoom != null) ? cellData.parentRoom : cellData.nearestRoom);
			CellData nearestFaceOrSidewall = roomHandler.GetNearestFaceOrSidewall(intVector + intVector2FromDirection);
			IntVector2 intVector2 = IntVector2.Zero;
			bool flag = false;
			GameObject gameObject;
			if (!nearestFaceOrSidewall.IsSideWallAdjacent())
			{
				gameObject = GameManager.Instance.Dungeon.SecretRoomSimpleTriggersFacewall[UnityEngine.Random.Range(0, GameManager.Instance.Dungeon.SecretRoomSimpleTriggersFacewall.Count)];
				intVector2 = IntVector2.Up;
			}
			else
			{
				gameObject = GameManager.Instance.Dungeon.SecretRoomSimpleTriggersSidewall[UnityEngine.Random.Range(0, GameManager.Instance.Dungeon.SecretRoomSimpleTriggersSidewall.Count)];
				intVector2 = IntVector2.Right + IntVector2.Up;
				if (GameManager.Instance.Dungeon.data[nearestFaceOrSidewall.position + IntVector2.Right].type == CellType.WALL)
				{
					flag = true;
				}
				else
				{
					intVector2 += IntVector2.Left;
				}
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.transform.parent = roomHandler.hierarchyParent;
			if (flag)
			{
				gameObject2.GetComponent<tk2dSprite>().FlipX = true;
			}
			nearestFaceOrSidewall.cellVisualData.containsObjectSpaceStamp = true;
			nearestFaceOrSidewall.cellVisualData.containsWallSpaceStamp = true;
			GameManager.Instance.Dungeon.data[nearestFaceOrSidewall.position + IntVector2.Up].cellVisualData.containsObjectSpaceStamp = true;
			GameManager.Instance.Dungeon.data[nearestFaceOrSidewall.position + IntVector2.Up].cellVisualData.containsWallSpaceStamp = true;
			gameObject2.transform.position = (nearestFaceOrSidewall.position + intVector2).ToVector3();
			SimpleSecretRoomTrigger simpleSecretRoomTrigger = gameObject2.AddComponent<SimpleSecretRoomTrigger>();
			simpleSecretRoomTrigger.referencedSecretRoom = this;
			simpleSecretRoomTrigger.parentRoom = roomHandler;
			gameObject2.GetComponent<Renderer>().sortingLayerName = "Background";
			gameObject2.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
			gameObject2.GetComponent<tk2dSprite>().UpdateZDepth();
			simpleSecretRoomTrigger.Initialize();
			this.m_simpleTrigger = simpleSecretRoomTrigger;
		}
	}

	// Token: 0x0600671F RID: 26399 RVA: 0x002854F4 File Offset: 0x002836F4
	protected void InitializeFireplacePuzzle()
	{
		for (int i = 0; i < this.doorObjects.Count; i++)
		{
			this.doorObjects[i].InitializeFireplace();
		}
	}

	// Token: 0x06006720 RID: 26400 RVA: 0x00285530 File Offset: 0x00283730
	protected void InitializeSecretRoomPuzzle()
	{
		if (this.doorObjects.Count != 0)
		{
			if (this.doorObjects.Count > 1)
			{
				Debug.LogError("Attempting to render a complex secret puzzle onto a multi-exit secret room. This is unsupported...");
			}
			else
			{
				GameObject colliderObject = this.doorObjects[0].collider.colliderObject;
				IntVector2 intVector = colliderObject.transform.position.IntXY(VectorConversions.Floor);
				IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(this.doorObjects[0].collider.exitDirection);
				CellData cellData = GameManager.Instance.Dungeon.data[intVector];
				RoomHandler roomHandler = ((cellData.parentRoom != null) ? cellData.parentRoom : cellData.nearestRoom);
				CellData nearestFloorFacewall = roomHandler.GetNearestFloorFacewall(intVector + intVector2FromDirection);
				if (nearestFloorFacewall == null)
				{
					Debug.LogError("failed complex puzzle generation due to lack of floor facewall.");
					return;
				}
				GameObject gameObject = GameManager.Instance.Dungeon.SecretRoomComplexTriggers[UnityEngine.Random.Range(0, GameManager.Instance.Dungeon.SecretRoomComplexTriggers.Count)].gameObject;
				IntVector2 up = IntVector2.Up;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				gameObject2.transform.parent = roomHandler.hierarchyParent;
				nearestFloorFacewall.cellVisualData.containsObjectSpaceStamp = true;
				nearestFloorFacewall.cellVisualData.containsWallSpaceStamp = true;
				GameManager.Instance.Dungeon.data[nearestFloorFacewall.position + IntVector2.Up].cellVisualData.containsObjectSpaceStamp = true;
				GameManager.Instance.Dungeon.data[nearestFloorFacewall.position + IntVector2.Up].cellVisualData.containsWallSpaceStamp = true;
				gameObject2.transform.position = (nearestFloorFacewall.position + up).ToVector3();
				ComplexSecretRoomTrigger component = gameObject2.GetComponent<ComplexSecretRoomTrigger>();
				component.referencedSecretRoom = this;
				component.Initialize(roomHandler);
			}
		}
	}

	// Token: 0x06006721 RID: 26401 RVA: 0x0028571C File Offset: 0x0028391C
	protected void InitializeShootToBreak()
	{
		for (int i = 0; i < this.doorObjects.Count; i++)
		{
			this.doorObjects[i].InitializeShootToBreak();
		}
	}

	// Token: 0x06006722 RID: 26402 RVA: 0x00285758 File Offset: 0x00283958
	public void DoSeal()
	{
		for (int i = 0; i < this.doorObjects.Count; i++)
		{
			if (this.doorObjects[i].subsidiaryBlocker != null)
			{
				this.doorObjects[i].subsidiaryBlocker.Seal();
			}
		}
	}

	// Token: 0x06006723 RID: 26403 RVA: 0x002857B4 File Offset: 0x002839B4
	public void DoUnseal()
	{
		for (int i = 0; i < this.doorObjects.Count; i++)
		{
			if (this.doorObjects[i].subsidiaryBlocker != null)
			{
				this.doorObjects[i].subsidiaryBlocker.Unseal();
			}
		}
	}

	// Token: 0x06006724 RID: 26404 RVA: 0x00285810 File Offset: 0x00283A10
	public void OpenDoor()
	{
		AkSoundEngine.PostEvent("Play_UI_secret_reveal_01", base.gameObject);
		AkSoundEngine.PostEvent("Play_OBJ_secret_door_01", base.gameObject);
		this.IsOpen = true;
		this.ceilingRenderer.enabled = false;
		if (this.borderRenderer != null)
		{
			this.borderRenderer.enabled = false;
		}
		for (int i = 0; i < this.ceilingCells.Count; i++)
		{
			if (GameManager.Instance.Dungeon.data[this.ceilingCells[i]] != null)
			{
				GameManager.Instance.Dungeon.data[this.ceilingCells[i]].isSecretRoomCell = false;
			}
		}
		for (int j = 0; j < this.doorObjects.Count; j++)
		{
			if (this.doorObjects[j].subsidiaryBlocker != null)
			{
				this.doorObjects[j].subsidiaryBlocker.ToggleRenderers(true);
			}
		}
		Minimap.Instance.RevealMinimapRoom(this.room, true, true, false);
		if (this.doorObjects.Count > 0)
		{
			for (int k = 0; k < this.doorObjects.Count; k++)
			{
				base.StartCoroutine(this.HandleDoorOpening(this.IsOpen, this.doorObjects[k]));
			}
		}
	}

	// Token: 0x06006725 RID: 26405 RVA: 0x0028598C File Offset: 0x00283B8C
	private string GetFrameName(string name, DungeonData.Direction dir)
	{
		if (name.Contains("{0}"))
		{
			string text;
			switch (dir)
			{
			case DungeonData.Direction.NORTH:
				text = "_top_top";
				break;
			default:
				if (dir != DungeonData.Direction.WEST)
				{
					text = string.Empty;
				}
				else
				{
					text = "_left_top";
				}
				break;
			case DungeonData.Direction.EAST:
				text = "_right_top";
				break;
			}
			return string.Format(name, text);
		}
		return name;
	}

	// Token: 0x06006726 RID: 26406 RVA: 0x002859FC File Offset: 0x00283BFC
	private GameObject SpawnVFXAtPoint(GameObject vfx, Vector3 position)
	{
		GameObject gameObject = SpawnManager.SpawnVFX(vfx, position, Quaternion.identity);
		tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
		component.HeightOffGround = 0.25f;
		component.PlaceAtPositionByAnchor(position, tk2dBaseSprite.Anchor.MiddleCenter);
		component.IsPerpendicular = false;
		component.UpdateZDepth();
		return gameObject;
	}

	// Token: 0x06006727 RID: 26407 RVA: 0x00285A40 File Offset: 0x00283C40
	private void DoSparkAtPoint(Vector3 position, List<Transform> refTransformList)
	{
		GameObject gameObject = SpawnManager.SpawnVFX(GameManager.Instance.Dungeon.SecretRoomDoorSparkVFX, false);
		tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
		component.HeightOffGround = 3.5f;
		component.PlaceAtPositionByAnchor(position, tk2dBaseSprite.Anchor.MiddleCenter);
		component.UpdateZDepth();
		refTransformList.Add(component.transform);
	}

	// Token: 0x06006728 RID: 26408 RVA: 0x00285A90 File Offset: 0x00283C90
	private void OnFinishedOpeningDoors()
	{
		for (int i = 0; i < this.doorObjects.Count; i++)
		{
			this.doorObjects[i].SetBreakable();
		}
		ShadowSystem.ForceRoomLightsUpdate(this.room, 0.1f);
		for (int j = 0; j < this.doorObjects.Count; j++)
		{
			ShadowSystem.ForceRoomLightsUpdate(this.doorObjects[j].linkedRoom, 0.1f);
		}
	}

	// Token: 0x06006729 RID: 26409 RVA: 0x00285B14 File Offset: 0x00283D14
	private IEnumerator HandleDoorOpening(bool openState, SecretRoomDoorBeer doorObject)
	{
		if (!openState)
		{
			doorObject.gameObject.SetActive(true);
		}
		if (doorObject.exitDef.GetDirectionFromRoom(this.room) == DungeonData.Direction.NORTH || doorObject.exitDef.GetDirectionFromRoom(this.room) == DungeonData.Direction.SOUTH)
		{
			doorObject.gameObject.layer = LayerMask.NameToLayer("BG_Nonsense");
		}
		float elapsed = 0f;
		float visibilityTrigger = 0f;
		float aoDisable = 2f;
		float duration = 3f;
		Transform target = doorObject.transform;
		Vector3 startPosition = target.position + ((!openState) ? Vector3.zero : new Vector3(0f, 0f, 1f));
		Vector3 endPosition = startPosition + ((!openState) ? new Vector3(0f, 2f, -2f) : new Vector3(0f, -2f, 2f));
		if (this.doorObjects[0].collider.exitDirection != DungeonData.Direction.NORTH && this.doorObjects[0].collider.exitDirection != DungeonData.Direction.SOUTH)
		{
			startPosition += new Vector3(0f, -0.5f, -0.5f);
			endPosition += new Vector3(0f, -1f, -1f);
		}
		ScreenShakeSettings continuousShake = new ScreenShakeSettings(0.1f, 7f, 0.1f, 0f);
		GameManager.Instance.MainCameraController.DoContinuousScreenShake(continuousShake, this, false);
		List<Transform> additionalTransformsToMove = new List<Transform>();
		List<GameObject> vfxToDestroy = new List<GameObject>();
		for (int i = 0; i < this.doorObjects.Count; i++)
		{
			SecretRoomExitData collider = this.doorObjects[i].collider;
			Vector3 vector = collider.colliderObject.GetComponent<SpeculativeRigidbody>().UnitBottomLeft.ToVector3ZUp(0f);
			if (collider.exitDirection == DungeonData.Direction.NORTH || collider.exitDirection == DungeonData.Direction.SOUTH)
			{
				if (GameManager.Instance.Dungeon.SecretRoomDoorSparkVFX != null)
				{
					this.DoSparkAtPoint(vector + new Vector3(0f, 1.9f, -0.25f), additionalTransformsToMove);
					this.DoSparkAtPoint(vector + new Vector3(2f, 1.9f, -0.25f), additionalTransformsToMove);
				}
				if (GameManager.Instance.Dungeon.SecretRoomHorizontalPoofVFX != null)
				{
					float num = (float)((collider.exitDirection != DungeonData.Direction.NORTH) ? 0 : (-1));
					GameObject gameObject = this.SpawnVFXAtPoint(GameManager.Instance.Dungeon.SecretRoomHorizontalPoofVFX, vector + new Vector3(0f, num, 0f));
					tk2dSpriteAnimator component = gameObject.GetComponent<tk2dSpriteAnimator>();
					component.Play();
					vfxToDestroy.Add(gameObject);
				}
			}
			else if (GameManager.Instance.Dungeon.SecretRoomVerticalPoofVFX != null)
			{
				float num2 = (float)((collider.exitDirection != DungeonData.Direction.EAST) ? (-1) : (-1));
				GameObject gameObject2 = this.SpawnVFXAtPoint(GameManager.Instance.Dungeon.SecretRoomVerticalPoofVFX, vector + new Vector3(num2, 0f, 0f));
				tk2dSpriteAnimator component2 = gameObject2.GetComponent<tk2dSpriteAnimator>();
				component2.Play();
				vfxToDestroy.Add(gameObject2);
			}
		}
		if (this.aoRenderer != null && this.aoRenderer.transform)
		{
			additionalTransformsToMove.Add(this.aoRenderer.transform);
		}
		bool hasTriggeredVisibility = false;
		if (visibilityTrigger == 0f)
		{
			hasTriggeredVisibility = true;
			Pixelator.Instance.ProcessRoomAdditionalExits(doorObject.exitDef.GetUpstreamBasePosition(), doorObject.linkedRoom, false);
		}
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			if (elapsed >= visibilityTrigger && !hasTriggeredVisibility)
			{
				hasTriggeredVisibility = true;
				Pixelator.Instance.ProcessRoomAdditionalExits(doorObject.exitDef.GetUpstreamBasePosition(), doorObject.linkedRoom, false);
			}
			if (elapsed >= aoDisable)
			{
				if (this.aoRenderer != null)
				{
					this.aoRenderer.enabled = false;
				}
				GameManager.Instance.MainCameraController.StopContinuousScreenShake(this);
				aoDisable = duration * 2f;
				for (int j = 0; j < additionalTransformsToMove.Count; j++)
				{
					additionalTransformsToMove[j].GetComponent<Renderer>().enabled = false;
				}
				for (int k = 0; k < this.doorObjects.Count; k++)
				{
					this.doorObjects[k].collider.colliderObject.GetComponent<SpeculativeRigidbody>().enabled = !openState;
				}
			}
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			Vector3 frameDisplacement = target.position;
			target.position = Vector3.Lerp(startPosition, endPosition, t);
			frameDisplacement = target.position - frameDisplacement;
			for (int l = 0; l < additionalTransformsToMove.Count; l++)
			{
				additionalTransformsToMove[l].position += frameDisplacement;
			}
			yield return null;
		}
		for (int m = 0; m < additionalTransformsToMove.Count; m++)
		{
			UnityEngine.Object.Destroy(additionalTransformsToMove[m].gameObject);
		}
		for (int n = 0; n < vfxToDestroy.Count; n++)
		{
			UnityEngine.Object.Destroy(vfxToDestroy[n]);
		}
		if (openState)
		{
			doorObject.gameObject.SetActive(false);
		}
		this.OnFinishedOpeningDoors();
		yield break;
	}

	// Token: 0x040062DD RID: 25309
	public SecretRoomManager.SecretRoomRevealStyle revealStyle = SecretRoomManager.SecretRoomRevealStyle.ShootToBreak;

	// Token: 0x040062DE RID: 25310
	public Renderer ceilingRenderer;

	// Token: 0x040062DF RID: 25311
	public Renderer borderRenderer;

	// Token: 0x040062E0 RID: 25312
	public Renderer aoRenderer;

	// Token: 0x040062E1 RID: 25313
	public RoomHandler room;

	// Token: 0x040062E2 RID: 25314
	public List<SecretRoomDoorBeer> doorObjects = new List<SecretRoomDoorBeer>();

	// Token: 0x040062E3 RID: 25315
	private List<IntVector2> ceilingCells;

	// Token: 0x040062E4 RID: 25316
	private SimpleSecretRoomTrigger m_simpleTrigger;

	// Token: 0x040062E5 RID: 25317
	private bool m_isOpen;

	// Token: 0x02001203 RID: 4611
	public enum SecretRoomRevealStyle
	{
		// Token: 0x040062E7 RID: 25319
		Simple,
		// Token: 0x040062E8 RID: 25320
		ComplexPuzzle,
		// Token: 0x040062E9 RID: 25321
		ShootToBreak,
		// Token: 0x040062EA RID: 25322
		FireplacePuzzle
	}
}
