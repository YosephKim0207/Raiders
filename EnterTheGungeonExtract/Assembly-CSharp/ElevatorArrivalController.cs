using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001150 RID: 4432
public class ElevatorArrivalController : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x06006244 RID: 25156 RVA: 0x00260A0C File Offset: 0x0025EC0C
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
			}
		}
		for (int k = 0; k < 6; k++)
		{
			for (int l = -2; l < 8; l++)
			{
				CellData cellData2 = GameManager.Instance.Dungeon.data.cellData[intVector.x + k][intVector.y + l];
				cellData2.cellVisualData.containsObjectSpaceStamp = true;
				cellData2.cellVisualData.containsWallSpaceStamp = true;
			}
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_READY_FOR_UNLOCKS))
		{
			bool flag = false;
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
			if (flag)
			{
				GameObject gameObject = ResourceCache.Acquire("Global Prefabs/ElevatorMaintenanceSign") as GameObject;
				UnityEngine.Object.Instantiate<GameObject>(gameObject, base.transform.position + gameObject.transform.position, Quaternion.identity);
			}
		}
	}

	// Token: 0x06006245 RID: 25157 RVA: 0x00260C54 File Offset: 0x0025EE54
	private void TransitionToDoorOpen(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.TransitionToDoorOpen));
		this.elevatorFloor.SetActive(true);
		this.smokeAnimator.gameObject.SetActive(true);
		this.smokeAnimator.PlayAndDisableObject(string.Empty, null);
		GameManager.Instance.MainCameraController.DoScreenShake(this.doorOpenShake, null, false);
		animator.Play(this.elevatorOpenAnimName);
		animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnDoorOpened));
	}

	// Token: 0x06006246 RID: 25158 RVA: 0x00260D00 File Offset: 0x0025EF00
	private void OnDoorOpened(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnDoorOpened));
		if (animator.specRigidbody)
		{
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(animator.specRigidbody, null, false);
		}
		if (this.elevatorCollider)
		{
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(this.elevatorCollider, null, false);
		}
	}

	// Token: 0x06006247 RID: 25159 RVA: 0x00260D84 File Offset: 0x0025EF84
	private void TransitionToDoorClose(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		GameManager.Instance.MainCameraController.DoScreenShake(this.doorCloseShake, null, false);
		animator.Play(this.elevatorCloseAnimName);
		animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.TransitionToDepart));
	}

	// Token: 0x06006248 RID: 25160 RVA: 0x00260DE0 File Offset: 0x0025EFE0
	private void TransitionToDepart(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		this.elevatorFloor.SetActive(false);
		GameManager.Instance.MainCameraController.DoDelayedScreenShake(this.departureShake, 0.25f, new Vector2?(animator.sprite.WorldCenter));
		animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.TransitionToDepart));
		animator.PlayAndDisableObject(this.elevatorDepartAnimName, null);
		base.StartCoroutine(this.HandleDepartBumbly());
		if (this.elevatorCollider)
		{
			this.elevatorCollider.enabled = false;
		}
	}

	// Token: 0x06006249 RID: 25161 RVA: 0x00260E7C File Offset: 0x0025F07C
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

	// Token: 0x0600624A RID: 25162 RVA: 0x00260F2C File Offset: 0x0025F12C
	private IEnumerator HandleDepartBumbly()
	{
		float elapsed = 0f;
		float duration = 0.55f;
		while (elapsed < duration)
		{
			if (elapsed > 0.3f && !this.crumblyBumblyAnimator.gameObject.activeSelf)
			{
				this.crumblyBumblyAnimator.gameObject.SetActive(true);
				this.crumblyBumblyAnimator.PlayAndDisableObject(string.Empty, null);
			}
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600624B RID: 25163 RVA: 0x00260F48 File Offset: 0x0025F148
	private void Start()
	{
		Material material = UnityEngine.Object.Instantiate<Material>(this.priorSprites[1].renderer.material);
		material.shader = ShaderCache.Acquire("Brave/Unity Transparent Cutout");
		this.priorSprites[1].renderer.material = material;
		Material material2 = UnityEngine.Object.Instantiate<Material>(this.postSprites[2].renderer.material);
		material2.shader = ShaderCache.Acquire("Brave/Unity Transparent Cutout");
		this.postSprites[3].renderer.material = material2;
		this.postSprites[1].HeightOffGround = this.postSprites[1].HeightOffGround - 0.0625f;
		this.postSprites[3].HeightOffGround = this.postSprites[3].HeightOffGround - 0.0625f;
		this.postSprites[1].UpdateZDepth();
		this.ToggleSprites(true);
	}

	// Token: 0x0600624C RID: 25164 RVA: 0x00261020 File Offset: 0x0025F220
	private void Update()
	{
		if (this.m_isArrived)
		{
			bool flag = true;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (Vector2.Distance(this.spawnTransform.position.XY(), GameManager.Instance.AllPlayers[i].CenterPosition) < 6f)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.DoDeparture();
			}
		}
	}

	// Token: 0x0600624D RID: 25165 RVA: 0x0026109C File Offset: 0x0025F29C
	public void DoDeparture()
	{
		this.m_isArrived = false;
		this.TransitionToDoorClose(this.elevatorAnimator, this.elevatorAnimator.CurrentClip);
		this.DeflagCells();
	}

	// Token: 0x0600624E RID: 25166 RVA: 0x002610C4 File Offset: 0x0025F2C4
	public void DoArrival(PlayerController player, float initialDelay)
	{
		if (this.m_isArrived)
		{
			return;
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.ToggleGunRenderers(false, string.Empty);
			playerController.ToggleShadowVisiblity(false);
			playerController.ToggleHandRenderers(false, string.Empty);
			playerController.ToggleFollowerRenderers(false);
			playerController.SetInputOverride("elevator arrival");
		}
		this.m_isArrived = true;
		base.StartCoroutine(this.HandleArrival(initialDelay));
	}

	// Token: 0x0600624F RID: 25167 RVA: 0x00261150 File Offset: 0x0025F350
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

	// Token: 0x06006250 RID: 25168 RVA: 0x0026120C File Offset: 0x0025F40C
	private IEnumerator HandleArrival(float initialDelay)
	{
		float ela = 0f;
		while (ela < initialDelay)
		{
			ela += BraveTime.DeltaTime;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				GameManager.Instance.AllPlayers[i].ToggleFollowerRenderers(false);
			}
			yield return null;
		}
		this.elevatorAnimator.gameObject.SetActive(true);
		Transform elevatorTransform = this.elevatorAnimator.transform;
		Vector3 elevatorStartPosition = elevatorTransform.position;
		int cachedFloorframe = this.floorAnimator.Sprite.spriteId;
		this.elevatorFloor.SetActive(false);
		this.elevatorAnimator.Play(this.elevatorDescendAnimName);
		this.elevatorAnimator.StopAndResetFrame();
		this.ToggleSprites(true);
		this.floorAnimator.Sprite.SetSprite(cachedFloorframe);
		float elapsed = 0f;
		float duration = 0.2f;
		float yDistance = 20f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			float yOffset = Mathf.Lerp(yDistance, 0f, t);
			elevatorTransform.position = elevatorStartPosition + new Vector3(0f, yOffset, 0f);
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
		this.floorAnimator.Play();
		yield return new WaitForSeconds(0.1f);
		for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[j];
			playerController.ClearInputOverride("elevator arrival");
		}
		for (int k = 0; k < this.poofObjects.Count; k++)
		{
			this.poofObjects[k].SetActive(true);
			this.poofObjects[k].GetComponent<tk2dBaseSprite>().IsPerpendicular = true;
			this.poofObjects[k].GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject(string.Empty, null);
			this.poofObjects[k].GetComponent<tk2dSpriteAnimator>().ClipFps = this.poofObjects[k].GetComponent<tk2dSpriteAnimator>().ClipFps * UnityEngine.Random.Range(0.8f, 1.1f);
		}
		yield break;
	}

	// Token: 0x06006251 RID: 25169 RVA: 0x00261230 File Offset: 0x0025F430
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005D23 RID: 23843
	public tk2dSpriteAnimator elevatorAnimator;

	// Token: 0x04005D24 RID: 23844
	public tk2dSpriteAnimator floorAnimator;

	// Token: 0x04005D25 RID: 23845
	public SpeculativeRigidbody elevatorCollider;

	// Token: 0x04005D26 RID: 23846
	public tk2dSprite[] priorSprites;

	// Token: 0x04005D27 RID: 23847
	public tk2dSprite[] postSprites;

	// Token: 0x04005D28 RID: 23848
	public BreakableChunk chunker;

	// Token: 0x04005D29 RID: 23849
	public List<GameObject> poofObjects;

	// Token: 0x04005D2A RID: 23850
	public Transform spawnTransform;

	// Token: 0x04005D2B RID: 23851
	public GameObject elevatorFloor;

	// Token: 0x04005D2C RID: 23852
	public tk2dSpriteAnimator crumblyBumblyAnimator;

	// Token: 0x04005D2D RID: 23853
	public tk2dSpriteAnimator smokeAnimator;

	// Token: 0x04005D2E RID: 23854
	[CheckAnimation("elevatorAnimator")]
	public string elevatorDescendAnimName;

	// Token: 0x04005D2F RID: 23855
	[CheckAnimation("elevatorAnimator")]
	public string elevatorOpenAnimName;

	// Token: 0x04005D30 RID: 23856
	[CheckAnimation("elevatorAnimator")]
	public string elevatorCloseAnimName;

	// Token: 0x04005D31 RID: 23857
	[CheckAnimation("elevatorAnimator")]
	public string elevatorDepartAnimName;

	// Token: 0x04005D32 RID: 23858
	public ScreenShakeSettings arrivalShake;

	// Token: 0x04005D33 RID: 23859
	public ScreenShakeSettings doorOpenShake;

	// Token: 0x04005D34 RID: 23860
	public ScreenShakeSettings doorCloseShake;

	// Token: 0x04005D35 RID: 23861
	public ScreenShakeSettings departureShake;

	// Token: 0x04005D36 RID: 23862
	private bool m_isArrived;
}
