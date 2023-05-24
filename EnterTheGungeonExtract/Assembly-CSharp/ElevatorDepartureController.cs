using System;
using System.Collections;
using Dungeonator;
using HutongGames.PlayMaker;
using UnityEngine;

// Token: 0x02001153 RID: 4435
public class ElevatorDepartureController : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x0600625F RID: 25183 RVA: 0x002617CC File Offset: 0x0025F9CC
	public void ConfigureOnPlacement(RoomHandler room)
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		for (int i = 0; i < 6; i++)
		{
			for (int j = -2; j < 6; j++)
			{
				CellData cellData = GameManager.Instance.Dungeon.data.cellData[intVector.x + i][intVector.y + j];
				cellData.cellVisualData.precludeAllTileDrawing = true;
				if (j < 4)
				{
					cellData.type = CellType.PIT;
					cellData.fallingPrevented = true;
				}
				cellData.isOccupied = true;
			}
		}
		if ((GameManager.Instance.CurrentGameMode == GameManager.GameMode.NORMAL || GameManager.Instance.CurrentGameMode == GameManager.GameMode.SHORTCUT) && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.TUTORIAL)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/CryoElevatorButton", ".prefab"), base.transform.position + new Vector3(-1f, 0f, 0f), Quaternion.identity);
			IntVector2 intVector2 = base.transform.position.IntXY(VectorConversions.Floor) + new IntVector2(-2, 0);
			for (int k = 0; k < 2; k++)
			{
				for (int l = -1; l < 2; l++)
				{
					if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector2 + new IntVector2(k, l)))
					{
						CellData cellData2 = GameManager.Instance.Dungeon.data[intVector2 + new IntVector2(k, l)];
						cellData2.cellVisualData.containsWallSpaceStamp = true;
						cellData2.cellVisualData.containsObjectSpaceStamp = true;
					}
				}
			}
			this.m_cryoButton = gameObject.GetComponentInChildren<TalkDoerLite>();
			room.RegisterInteractable(this.m_cryoButton);
			TalkDoerLite cryoButton = this.m_cryoButton;
			cryoButton.OnGenericFSMActionA = (Action)Delegate.Combine(cryoButton.OnGenericFSMActionA, new Action(this.SwitchToCryoElevator));
			TalkDoerLite cryoButton2 = this.m_cryoButton;
			cryoButton2.OnGenericFSMActionB = (Action)Delegate.Combine(cryoButton2.OnGenericFSMActionB, new Action(this.RescindCryoElevator));
			this.m_cryoBool = this.m_cryoButton.playmakerFsm.FsmVariables.GetFsmBool("IS_CRYO");
			this.m_normalBool = this.m_cryoButton.playmakerFsm.FsmVariables.GetFsmBool("IS_NORMAL");
		}
	}

	// Token: 0x06006260 RID: 25184 RVA: 0x00261A30 File Offset: 0x0025FC30
	private void ToggleSprites(bool prior)
	{
		for (int i = 0; i < this.priorSprites.Length; i++)
		{
			if (this.priorSprites[i] && this.priorSprites[i].renderer)
			{
				this.priorSprites[i].renderer.enabled = prior;
			}
		}
		for (int j = 0; j < this.postSprites.Length; j++)
		{
			if (this.postSprites[j] && this.postSprites[j].renderer)
			{
				this.postSprites[j].renderer.enabled = !prior;
			}
		}
	}

	// Token: 0x06006261 RID: 25185 RVA: 0x00261AEC File Offset: 0x0025FCEC
	public void SwitchToCryoElevator()
	{
		if (this.m_isArrived != Tribool.Ready)
		{
			return;
		}
		this.DoPlayerlessDeparture();
		GameManager.Instance.Dungeon.StartCoroutine(this.CryoWaitForPreviousElevatorDeparture());
	}

	// Token: 0x06006262 RID: 25186 RVA: 0x00261B20 File Offset: 0x0025FD20
	private IEnumerator CryoWaitForPreviousElevatorDeparture()
	{
		this.m_isCryoArrived = Tribool.Unready;
		while (this.elevatorAnimator.enabled && this.elevatorAnimator.gameObject.activeSelf)
		{
			yield return null;
		}
		if (this.m_activeCryoElevatorAnimator == null)
		{
			GameObject gameObject = (GameObject)BraveResources.Load("Global Prefabs/CryoElevator", ".prefab");
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.localPosition = this.elevatorAnimator.transform.localPosition;
			this.m_activeCryoElevatorAnimator = gameObject2.GetComponent<tk2dSpriteAnimator>();
		}
		this.m_activeCryoElevatorAnimator.gameObject.SetActive(true);
		this.m_activeCryoElevatorAnimator.Play("arrive");
		yield return null;
		MeshRenderer floorObject = this.m_activeCryoElevatorAnimator.transform.Find("ElevatorInterior (1)").GetComponent<MeshRenderer>();
		floorObject.enabled = true;
		this.elevatorFloor.SetActive(true);
		this.elevatorFloor.GetComponent<MeshRenderer>().enabled = false;
		while (this.m_activeCryoElevatorAnimator.IsPlaying("arrive") && this.m_activeCryoElevatorAnimator.CurrentFrame < this.m_activeCryoElevatorAnimator.CurrentClip.frames.Length - 2)
		{
			yield return null;
		}
		tk2dSpriteAnimator mistAnimator = this.m_activeCryoElevatorAnimator.transform.Find("Sierra").GetComponent<tk2dSpriteAnimator>();
		tk2dSpriteAnimator doorAnimator = this.m_activeCryoElevatorAnimator.transform.Find("Door").GetComponent<tk2dSpriteAnimator>();
		doorAnimator.GetComponent<MeshRenderer>().enabled = true;
		doorAnimator.Play("door_open");
		mistAnimator.GetComponent<MeshRenderer>().enabled = true;
		mistAnimator.PlayAndDisableRenderer("mist");
		yield return null;
		while (doorAnimator.IsPlaying("door_open"))
		{
			yield return null;
		}
		this.m_isCryoArrived = Tribool.Ready;
		yield break;
	}

	// Token: 0x06006263 RID: 25187 RVA: 0x00261B3C File Offset: 0x0025FD3C
	private void SetFSMStates()
	{
		if (this.m_cryoButton)
		{
			this.m_cryoBool.Value = this.m_isCryoArrived == Tribool.Ready;
			this.m_normalBool.Value = this.m_isArrived == Tribool.Ready;
		}
	}

	// Token: 0x06006264 RID: 25188 RVA: 0x00261B90 File Offset: 0x0025FD90
	public void RescindCryoElevator()
	{
		if (this.m_isCryoArrived != Tribool.Ready)
		{
			return;
		}
		this.DoCryoDeparture(true);
	}

	// Token: 0x06006265 RID: 25189 RVA: 0x00261BB0 File Offset: 0x0025FDB0
	public void DoCryoDeparture(bool playerless = true)
	{
		if (this.m_activeCryoElevatorAnimator == null)
		{
			return;
		}
		if (this.m_isCryoArrived != Tribool.Ready)
		{
			return;
		}
		this.m_isCryoArrived = Tribool.Complete;
		if (!playerless)
		{
			if (Minimap.Instance)
			{
				Minimap.Instance.PreventAllTeleports = true;
			}
			if (GameManager.HasInstance && GameManager.Instance.AllPlayers != null)
			{
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					if (GameManager.Instance.AllPlayers[i])
					{
						GameManager.Instance.AllPlayers[i].CurrentInputState = PlayerInputState.NoInput;
					}
				}
			}
		}
		GameManager.Instance.Dungeon.StartCoroutine(this.HandleCryoDeparture(playerless));
	}

	// Token: 0x06006266 RID: 25190 RVA: 0x00261C88 File Offset: 0x0025FE88
	public IEnumerator HandleCryoDeparture(bool playerless)
	{
		MeshRenderer floorObject = this.m_activeCryoElevatorAnimator.transform.Find("ElevatorInterior (1)").GetComponent<MeshRenderer>();
		tk2dSpriteAnimator doorAnimator = this.m_activeCryoElevatorAnimator.transform.Find("Door").GetComponent<tk2dSpriteAnimator>();
		doorAnimator.Play("door_close");
		yield return null;
		while (doorAnimator.IsPlaying("door_close"))
		{
			yield return null;
		}
		this.elevatorFloor.SetActive(false);
		GameManager.Instance.MainCameraController.DoDelayedScreenShake(this.departureShake, 0.25f, null);
		if (!playerless)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				GameManager.Instance.AllPlayers[i].PrepareForSceneTransition();
			}
			Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
			GameUIRoot.Instance.HideCoreUI(string.Empty);
			GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
			AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
			GlobalDungeonData.ValidTilesets nextTileset = GameManager.Instance.GetNextTileset(GameManager.Instance.Dungeon.tileIndices.tilesetId);
			GameManager.DoMidgameSave(nextTileset);
			float num = 0.5f;
			GameManager.Instance.DelayedLoadCharacterSelect(num, true, true);
		}
		doorAnimator.GetComponent<MeshRenderer>().enabled = false;
		floorObject.enabled = false;
		this.m_activeCryoElevatorAnimator.Play("depart");
		yield return null;
		while (this.m_activeCryoElevatorAnimator.IsPlaying("depart"))
		{
			yield return null;
		}
		this.m_activeCryoElevatorAnimator.gameObject.SetActive(false);
		if (playerless)
		{
			this.m_isArrived = Tribool.Unready;
		}
		yield break;
	}

	// Token: 0x06006267 RID: 25191 RVA: 0x00261CAC File Offset: 0x0025FEAC
	private void TransitionToDoorOpen(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.TransitionToDoorOpen));
		this.elevatorFloor.SetActive(true);
		this.elevatorFloor.GetComponent<MeshRenderer>().enabled = true;
		this.smokeAnimator.gameObject.SetActive(true);
		this.smokeAnimator.PlayAndDisableObject(string.Empty, null);
		GameManager.Instance.MainCameraController.DoScreenShake(this.doorOpenShake, null, false);
		animator.Play(this.elevatorOpenAnimName);
	}

	// Token: 0x06006268 RID: 25192 RVA: 0x00261D48 File Offset: 0x0025FF48
	private void TransitionToDoorClose(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		GameManager.Instance.MainCameraController.DoScreenShake(this.doorCloseShake, null, false);
		animator.Play(this.elevatorCloseAnimName);
		animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.TransitionToDepart));
	}

	// Token: 0x06006269 RID: 25193 RVA: 0x00261DA4 File Offset: 0x0025FFA4
	private void TransitionToDepart(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		GameManager.Instance.MainCameraController.DoDelayedScreenShake(this.departureShake, 0.25f, null);
		if (!this.m_depatureIsPlayerless)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				GameManager.Instance.AllPlayers[i].PrepareForSceneTransition();
			}
			Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
			GameUIRoot.Instance.HideCoreUI(string.Empty);
			GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
			float num = 0.5f;
			if (this.ReturnToFoyerWithNewInstance)
			{
				GameManager.Instance.DelayedReturnToFoyer(num);
			}
			else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH)
			{
				GameManager.Instance.DelayedLoadBossrushFloor(num);
			}
			else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
			{
				GameManager.Instance.DelayedLoadBossrushFloor(num);
			}
			else
			{
				if (!GameManager.Instance.IsFoyer && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE)
				{
					GlobalDungeonData.ValidTilesets nextTileset = GameManager.Instance.GetNextTileset(GameManager.Instance.Dungeon.tileIndices.tilesetId);
					GameManager.DoMidgameSave(nextTileset);
				}
				if (this.UsesOverrideTargetFloor)
				{
					GlobalDungeonData.ValidTilesets overrideTargetFloor = this.OverrideTargetFloor;
					if (overrideTargetFloor != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
					{
						if (overrideTargetFloor == GlobalDungeonData.ValidTilesets.FORGEGEON)
						{
							GameManager.Instance.DelayedLoadCustomLevel(num, "tt_forge");
						}
					}
					else
					{
						GameManager.Instance.DelayedLoadCustomLevel(num, "tt_catacombs");
					}
				}
				else
				{
					GameManager.Instance.DelayedLoadNextLevel(num);
				}
				AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
			}
		}
		this.elevatorFloor.SetActive(false);
		animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.TransitionToDepart));
		animator.PlayAndDisableObject(this.elevatorDepartAnimName, null);
	}

	// Token: 0x0600626A RID: 25194 RVA: 0x00261F98 File Offset: 0x00260198
	private void DeflagCells()
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		for (int i = 0; i < 6; i++)
		{
			for (int j = -2; j < 6; j++)
			{
				if (j != -2 || (i >= 2 && i <= 3))
				{
					if (j != -1 || (i >= 1 && i <= 4))
					{
						CellData cellData = GameManager.Instance.Dungeon.data.cellData[intVector.x + i][intVector.y + j];
						if (j < 4)
						{
							cellData.fallingPrevented = false;
						}
					}
				}
			}
		}
	}

	// Token: 0x0600626B RID: 25195 RVA: 0x00262048 File Offset: 0x00260248
	private IEnumerator HandleDepartMotion()
	{
		Transform elevatorTransform = this.elevatorAnimator.transform;
		Vector3 elevatorStartDepartPosition = elevatorTransform.position;
		float elapsed = 0f;
		float duration = 0.55f;
		float yDistance = 20f;
		bool hasLayerSwapped = false;
		while (elapsed < duration)
		{
			if (elapsed > 0.15f && !this.crumblyBumblyAnimator.gameObject.activeSelf)
			{
				this.crumblyBumblyAnimator.gameObject.SetActive(true);
				this.crumblyBumblyAnimator.PlayAndDisableObject(string.Empty, null);
			}
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			float yOffset = BraveMathCollege.SmoothLerp(0f, -yDistance, t);
			if (yOffset < -2f && !hasLayerSwapped)
			{
				hasLayerSwapped = true;
				this.elevatorAnimator.gameObject.SetLayerRecursively(LayerMask.NameToLayer("BG_Critical"));
			}
			elevatorTransform.position = elevatorStartDepartPosition + new Vector3(0f, yOffset, 0f);
			if (this.facewallAnimator != null)
			{
				this.facewallAnimator.Sprite.UpdateZDepth();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600626C RID: 25196 RVA: 0x00262064 File Offset: 0x00260264
	private void Start()
	{
		Material material = UnityEngine.Object.Instantiate<Material>(this.priorSprites[1].renderer.material);
		material.shader = ShaderCache.Acquire("Brave/Unity Transparent Cutout");
		this.priorSprites[1].renderer.material = material;
		Material material2 = UnityEngine.Object.Instantiate<Material>(this.postSprites[2].renderer.material);
		material2.shader = ShaderCache.Acquire("Brave/Unity Transparent Cutout");
		this.postSprites[2].renderer.material = material2;
		this.postSprites[1].HeightOffGround = this.postSprites[1].HeightOffGround - 0.0625f;
		this.postSprites[3].HeightOffGround = this.postSprites[3].HeightOffGround - 0.0625f;
		this.postSprites[1].UpdateZDepth();
		SpeculativeRigidbody component = this.elevatorFloor.GetComponent<SpeculativeRigidbody>();
		if (component)
		{
			component.PrimaryPixelCollider.ManualOffsetY -= 8;
			component.PrimaryPixelCollider.ManualHeight += 8;
			component.Reinitialize();
			SpeculativeRigidbody speculativeRigidbody = component;
			speculativeRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(speculativeRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnElevatorTriggerEnter));
		}
		this.ToggleSprites(true);
	}

	// Token: 0x0600626D RID: 25197 RVA: 0x002621A0 File Offset: 0x002603A0
	private void OnElevatorTriggerEnter(SpeculativeRigidbody otherSpecRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (this.m_isArrived == Tribool.Ready)
		{
			if (otherSpecRigidbody.GetComponent<PlayerController>() != null)
			{
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					bool flag = true;
					for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
					{
						if (!GameManager.Instance.AllPlayers[i].healthHaver.IsDead)
						{
							if (!sourceSpecRigidbody.ContainsPoint(GameManager.Instance.AllPlayers[i].SpriteBottomCenter.XY(), 2147483647, true))
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						this.DoDeparture();
					}
				}
				else
				{
					this.DoDeparture();
				}
			}
		}
		else if (this.m_isCryoArrived == Tribool.Ready && this.m_activeCryoElevatorAnimator != null && otherSpecRigidbody.GetComponent<PlayerController>() != null)
		{
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				bool flag2 = true;
				for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
				{
					if (!GameManager.Instance.AllPlayers[j].healthHaver.IsDead)
					{
						if (!sourceSpecRigidbody.ContainsPoint(GameManager.Instance.AllPlayers[j].SpriteBottomCenter.XY(), 2147483647, true))
						{
							flag2 = false;
							break;
						}
					}
				}
				if (flag2)
				{
					this.DoCryoDeparture(false);
				}
			}
			else
			{
				this.DoCryoDeparture(false);
			}
		}
	}

	// Token: 0x0600626E RID: 25198 RVA: 0x0026233C File Offset: 0x0026053C
	private void Update()
	{
		PlayerController activePlayerClosestToPoint = GameManager.Instance.GetActivePlayerClosestToPoint(this.spawnTransform.position.XY(), true);
		if (activePlayerClosestToPoint != null && this.m_isArrived == Tribool.Unready && Vector2.Distance(this.spawnTransform.position.XY(), activePlayerClosestToPoint.CenterPosition) < 8f)
		{
			this.DoArrival();
		}
		if (this.m_cryoBool != null && this.m_normalBool != null)
		{
			this.SetFSMStates();
		}
	}

	// Token: 0x0600626F RID: 25199 RVA: 0x002623D0 File Offset: 0x002605D0
	public void DoPlayerlessDeparture()
	{
		this.m_depatureIsPlayerless = true;
		this.m_isArrived = Tribool.Complete;
		this.TransitionToDoorClose(this.elevatorAnimator, this.elevatorAnimator.CurrentClip);
	}

	// Token: 0x06006270 RID: 25200 RVA: 0x002623FC File Offset: 0x002605FC
	public void DoDeparture()
	{
		this.m_depatureIsPlayerless = false;
		this.m_isArrived = Tribool.Complete;
		if (Minimap.Instance)
		{
			Minimap.Instance.PreventAllTeleports = true;
		}
		if (GameManager.HasInstance && GameManager.Instance.AllPlayers != null)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i])
				{
					GameManager.Instance.AllPlayers[i].CurrentInputState = PlayerInputState.NoInput;
				}
			}
		}
		this.TransitionToDoorClose(this.elevatorAnimator, this.elevatorAnimator.CurrentClip);
	}

	// Token: 0x06006271 RID: 25201 RVA: 0x002624AC File Offset: 0x002606AC
	public void DoArrival()
	{
		this.m_isArrived = Tribool.Ready;
		this.m_hasEverArrived = true;
		base.StartCoroutine(this.HandleArrival(0f));
	}

	// Token: 0x06006272 RID: 25202 RVA: 0x002624D4 File Offset: 0x002606D4
	private IEnumerator HandleArrival(float initialDelay)
	{
		yield return new WaitForSeconds(initialDelay);
		this.elevatorAnimator.gameObject.SetActive(true);
		Transform elevatorTransform = this.elevatorAnimator.transform;
		Vector3 elevatorStartPosition = elevatorTransform.position;
		int cachedCeilingFrame = ((!(this.ceilingAnimator != null)) ? (-1) : this.ceilingAnimator.Sprite.spriteId);
		int cachedFacewallFrame = ((!(this.facewallAnimator != null)) ? (-1) : this.facewallAnimator.Sprite.spriteId);
		int cachedFloorframe = this.floorAnimator.Sprite.spriteId;
		this.elevatorFloor.SetActive(false);
		this.elevatorAnimator.Play(this.elevatorDescendAnimName);
		this.elevatorAnimator.StopAndResetFrame();
		if (this.ceilingAnimator != null)
		{
			this.ceilingAnimator.Sprite.SetSprite(cachedCeilingFrame);
		}
		if (this.facewallAnimator != null)
		{
			this.facewallAnimator.Sprite.SetSprite(cachedFacewallFrame);
		}
		this.floorAnimator.Sprite.SetSprite(cachedFloorframe);
		if (!this.m_hasEverArrived)
		{
			this.ToggleSprites(true);
		}
		float elapsed = 0f;
		float duration = 0.1f;
		float yDistance = 20f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			float yOffset = Mathf.Lerp(yDistance, 0f, t);
			elevatorTransform.position = elevatorStartPosition + new Vector3(0f, yOffset, 0f);
			if (this.facewallAnimator != null)
			{
				this.facewallAnimator.Sprite.UpdateZDepth();
			}
			yield return null;
		}
		GameManager.Instance.MainCameraController.DoScreenShake(this.arrivalShake, null, false);
		this.elevatorAnimator.Play();
		tk2dSpriteAnimator tk2dSpriteAnimator = this.elevatorAnimator;
		tk2dSpriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(tk2dSpriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.TransitionToDoorOpen));
		this.ToggleSprites(false);
		if (this.chunker != null)
		{
			this.chunker.Trigger(true, new Vector3?(base.transform.position + new Vector3(3f, 3f, 3f)));
		}
		if (this.ceilingAnimator != null)
		{
			this.ceilingAnimator.Play();
		}
		if (this.facewallAnimator != null)
		{
			this.facewallAnimator.Play();
		}
		this.floorAnimator.Play();
		yield break;
	}

	// Token: 0x06006273 RID: 25203 RVA: 0x002624F8 File Offset: 0x002606F8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005D4B RID: 23883
	public tk2dSpriteAnimator elevatorAnimator;

	// Token: 0x04005D4C RID: 23884
	public tk2dSpriteAnimator ceilingAnimator;

	// Token: 0x04005D4D RID: 23885
	public tk2dSpriteAnimator facewallAnimator;

	// Token: 0x04005D4E RID: 23886
	public tk2dSpriteAnimator floorAnimator;

	// Token: 0x04005D4F RID: 23887
	public tk2dSprite[] priorSprites;

	// Token: 0x04005D50 RID: 23888
	public tk2dSprite[] postSprites;

	// Token: 0x04005D51 RID: 23889
	public BreakableChunk chunker;

	// Token: 0x04005D52 RID: 23890
	public Transform spawnTransform;

	// Token: 0x04005D53 RID: 23891
	public GameObject elevatorFloor;

	// Token: 0x04005D54 RID: 23892
	public tk2dSpriteAnimator crumblyBumblyAnimator;

	// Token: 0x04005D55 RID: 23893
	public tk2dSpriteAnimator smokeAnimator;

	// Token: 0x04005D56 RID: 23894
	public string elevatorDescendAnimName;

	// Token: 0x04005D57 RID: 23895
	public string elevatorOpenAnimName;

	// Token: 0x04005D58 RID: 23896
	public string elevatorCloseAnimName;

	// Token: 0x04005D59 RID: 23897
	public string elevatorDepartAnimName;

	// Token: 0x04005D5A RID: 23898
	public ScreenShakeSettings arrivalShake;

	// Token: 0x04005D5B RID: 23899
	public ScreenShakeSettings doorOpenShake;

	// Token: 0x04005D5C RID: 23900
	public ScreenShakeSettings doorCloseShake;

	// Token: 0x04005D5D RID: 23901
	public ScreenShakeSettings departureShake;

	// Token: 0x04005D5E RID: 23902
	public bool ReturnToFoyerWithNewInstance;

	// Token: 0x04005D5F RID: 23903
	public bool UsesOverrideTargetFloor;

	// Token: 0x04005D60 RID: 23904
	public GlobalDungeonData.ValidTilesets OverrideTargetFloor;

	// Token: 0x04005D61 RID: 23905
	private Tribool m_isArrived = Tribool.Unready;

	// Token: 0x04005D62 RID: 23906
	private Tribool m_isCryoArrived = Tribool.Unready;

	// Token: 0x04005D63 RID: 23907
	private TalkDoerLite m_cryoButton;

	// Token: 0x04005D64 RID: 23908
	private FsmBool m_cryoBool;

	// Token: 0x04005D65 RID: 23909
	private FsmBool m_normalBool;

	// Token: 0x04005D66 RID: 23910
	public const bool c_savingEnabled = true;

	// Token: 0x04005D67 RID: 23911
	private tk2dSpriteAnimator m_activeCryoElevatorAnimator;

	// Token: 0x04005D68 RID: 23912
	private bool m_depatureIsPlayerless;

	// Token: 0x04005D69 RID: 23913
	private bool m_hasEverArrived;
}
