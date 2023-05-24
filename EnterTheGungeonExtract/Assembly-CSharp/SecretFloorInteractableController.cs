using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using HutongGames.PlayMaker;
using UnityEngine;

// Token: 0x020011FE RID: 4606
public class SecretFloorInteractableController : DungeonPlaceableBehaviour, IPlayerInteractable
{
	// Token: 0x060066DD RID: 26333 RVA: 0x00281210 File Offset: 0x0027F410
	public override GameObject InstantiateObject(RoomHandler targetRoom, IntVector2 loc, bool deferConfiguration = false)
	{
		if (this.IsResourcefulRatPit)
		{
			IntVector2 intVector = loc + targetRoom.area.basePosition;
			int num = intVector.x;
			int num2 = intVector.x + this.placeableWidth;
			int num3 = intVector.y;
			int num4 = intVector.y + this.placeableHeight;
			if (this.GoesToRatFloor)
			{
				num++;
				num3++;
				num2--;
				num4--;
			}
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					CellData cellData = GameManager.Instance.Dungeon.data.cellData[i][j];
					cellData.type = CellType.PIT;
					cellData.fallingPrevented = true;
					if (this.OverridePitIndex != null)
					{
						cellData.cellVisualData.HasTriggeredPitVFX = true;
						cellData.cellVisualData.PitVFXCooldown = float.MaxValue;
						bool flag = j == intVector.y + this.placeableHeight - 1;
						if (flag)
						{
							cellData.cellVisualData.pitOverrideIndex = this.OverridePitIndex.topIndices.GetIndexByWeight();
						}
						else
						{
							cellData.cellVisualData.pitOverrideIndex = this.OverridePitIndex.centerIndices.GetIndexByWeight();
						}
					}
				}
			}
		}
		return base.InstantiateObject(targetRoom, loc, deferConfiguration);
	}

	// Token: 0x060066DE RID: 26334 RVA: 0x00281374 File Offset: 0x0027F574
	private void Start()
	{
		RoomHandler absoluteParentRoom = base.GetAbsoluteParentRoom();
		for (int i = 0; i < this.WorldLocks.Count; i++)
		{
			absoluteParentRoom.RegisterInteractable(this.WorldLocks[i]);
		}
		if (this.IsResourcefulRatPit)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerEntered));
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.OnExitTrigger = (SpeculativeRigidbody.OnTriggerExitDelegate)Delegate.Combine(specRigidbody2.OnExitTrigger, new SpeculativeRigidbody.OnTriggerExitDelegate(this.HandleTriggerExited));
			SpeculativeRigidbody specRigidbody3 = base.specRigidbody;
			specRigidbody3.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody3.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerRemain));
		}
		if (this.FlightCollider)
		{
			SpeculativeRigidbody flightCollider = this.FlightCollider;
			flightCollider.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(flightCollider.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleFlightCollider));
		}
		TalkDoerLite componentInChildren = absoluteParentRoom.hierarchyParent.GetComponentInChildren<TalkDoerLite>();
		if (componentInChildren && componentInChildren.name.Contains("CryoButton"))
		{
			TalkDoerLite talkDoerLite = componentInChildren;
			talkDoerLite.OnGenericFSMActionA = (Action)Delegate.Combine(talkDoerLite.OnGenericFSMActionA, new Action(this.SwitchToCryoElevator));
			TalkDoerLite talkDoerLite2 = componentInChildren;
			talkDoerLite2.OnGenericFSMActionB = (Action)Delegate.Combine(talkDoerLite2.OnGenericFSMActionB, new Action(this.RescindCryoElevator));
			this.m_cryoBool = componentInChildren.playmakerFsm.FsmVariables.GetFsmBool("IS_CRYO");
			this.m_normalBool = componentInChildren.playmakerFsm.FsmVariables.GetFsmBool("IS_NORMAL");
			this.m_cryoBool.Value = false;
			this.m_normalBool.Value = true;
		}
	}

	// Token: 0x060066DF RID: 26335 RVA: 0x0028152C File Offset: 0x0027F72C
	private void RescindCryoElevator()
	{
		this.m_cryoBool.Value = false;
		this.m_normalBool.Value = true;
		if (this.cryoAnimator && !string.IsNullOrEmpty(this.cyroDepartAnimation))
		{
			this.cryoAnimator.Play(this.cyroDepartAnimation);
		}
	}

	// Token: 0x060066E0 RID: 26336 RVA: 0x00281584 File Offset: 0x0027F784
	private void SwitchToCryoElevator()
	{
		this.m_cryoBool.Value = true;
		this.m_normalBool.Value = false;
		if (this.cryoAnimator && !string.IsNullOrEmpty(this.cryoArriveAnimation))
		{
			this.cryoAnimator.Play(this.cryoArriveAnimation);
		}
	}

	// Token: 0x060066E1 RID: 26337 RVA: 0x002815DC File Offset: 0x0027F7DC
	private void HandleFlightCollider(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (!GameManager.Instance.IsLoadingLevel && this.IsValidForUse())
		{
			PlayerController component = specRigidbody.GetComponent<PlayerController>();
			if (component && component.IsFlying && !this.m_isLoading && !GameManager.Instance.IsLoadingLevel && !string.IsNullOrEmpty(this.targetLevelName))
			{
				this.m_timeHovering += BraveTime.DeltaTime;
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(component);
					if (component.IsFlying && !otherPlayer.IsFlying && !otherPlayer.IsGhost)
					{
						this.m_timeHovering = 0f;
					}
				}
				if (this.m_timeHovering > 0.5f)
				{
					this.m_isLoading = true;
					GameManager.Instance.LoadCustomLevel(this.targetLevelName);
				}
			}
		}
	}

	// Token: 0x060066E2 RID: 26338 RVA: 0x002816CC File Offset: 0x0027F8CC
	private void HandleTriggerRemain(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (this.IsValidForUse() && !this.m_isLoading)
		{
			PlayerController component = specRigidbody.GetComponent<PlayerController>();
			base.StartCoroutine(this.FrameDelayedTriggerRemainCheck(component));
		}
	}

	// Token: 0x060066E3 RID: 26339 RVA: 0x00281704 File Offset: 0x0027F904
	private IEnumerator FrameDelayedTriggerRemainCheck(PlayerController targetPlayer)
	{
		yield return null;
		if (targetPlayer && (targetPlayer.IsFalling || targetPlayer.IsFlying) && this.m_cryoBool != null && this.m_cryoBool.Value)
		{
			this.m_isLoading = true;
			Pixelator.Instance.FadeToBlack(1f, false, 0f);
			GameUIRoot.Instance.HideCoreUI(string.Empty);
			GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
			AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
			GameManager.DoMidgameSave(this.TargetTileset);
			float num = 1.5f;
			targetPlayer.specRigidbody.Velocity = Vector2.zero;
			targetPlayer.LevelToLoadOnPitfall = "midgamesave";
			GameManager.Instance.DelayedLoadCharacterSelect(num, true, true);
		}
		yield break;
	}

	// Token: 0x060066E4 RID: 26340 RVA: 0x00281728 File Offset: 0x0027F928
	private void HandleTriggerEntered(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		PlayerController component = specRigidbody.GetComponent<PlayerController>();
		if (component)
		{
			if (this.m_cryoBool != null && this.m_cryoBool.Value)
			{
				component.LevelToLoadOnPitfall = "midgamesave";
			}
			else
			{
				component.LevelToLoadOnPitfall = this.targetLevelName;
			}
		}
	}

	// Token: 0x060066E5 RID: 26341 RVA: 0x00281780 File Offset: 0x0027F980
	private void HandleTriggerExited(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody)
	{
		PlayerController component = specRigidbody.GetComponent<PlayerController>();
		if (component)
		{
			component.LevelToLoadOnPitfall = string.Empty;
		}
	}

	// Token: 0x060066E6 RID: 26342 RVA: 0x002817AC File Offset: 0x0027F9AC
	private void Update()
	{
		if (!this.m_hasOpened && this.IsResourcefulRatPit && this.IsValidForUse())
		{
			this.m_hasOpened = true;
			base.spriteAnimator.Play();
			IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
			int num = intVector.x;
			int num2 = intVector.x + this.placeableWidth;
			int num3 = intVector.y;
			int num4 = intVector.y + this.placeableHeight;
			if (this.GoesToRatFloor)
			{
				num++;
				num3++;
				num2--;
				num4--;
			}
			for (int i = num; i < num2; i++)
			{
				int j = num3;
				while (j < num4)
				{
					if (!this.GoesToRatFloor)
					{
						goto IL_15C;
					}
					if (i != intVector.x + 1 || j != intVector.y + 1)
					{
						if (i != intVector.x + 1 || j != intVector.y + this.placeableHeight - 2)
						{
							if (i != intVector.x + this.placeableWidth - 2 || j != intVector.y + 1)
							{
								if (i != intVector.x + this.placeableWidth - 2 || j != intVector.y + this.placeableHeight - 2)
								{
									goto IL_15C;
								}
							}
						}
					}
					IL_180:
					j++;
					continue;
					IL_15C:
					CellData cellData = GameManager.Instance.Dungeon.data.cellData[i][j];
					cellData.fallingPrevented = false;
					goto IL_180;
				}
			}
		}
	}

	// Token: 0x060066E7 RID: 26343 RVA: 0x00281958 File Offset: 0x0027FB58
	private bool IsValidForUse()
	{
		bool flag = true;
		for (int i = 0; i < this.WorldLocks.Count; i++)
		{
			if (this.WorldLocks[i].IsLocked || this.WorldLocks[i].spriteAnimator.IsPlaying(this.WorldLocks[i].spriteAnimator.CurrentClip))
			{
				flag = false;
			}
		}
		return flag;
	}

	// Token: 0x060066E8 RID: 26344 RVA: 0x002819D0 File Offset: 0x0027FBD0
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x060066E9 RID: 26345 RVA: 0x00281A04 File Offset: 0x0027FC04
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x060066EA RID: 26346 RVA: 0x00281A2C File Offset: 0x0027FC2C
	public float GetDistanceToPoint(Vector2 point)
	{
		if (this.IsResourcefulRatPit)
		{
			return 1000f;
		}
		if (!this.IsValidForUse())
		{
			return 1000f;
		}
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x060066EB RID: 26347 RVA: 0x00281B30 File Offset: 0x0027FD30
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x060066EC RID: 26348 RVA: 0x00281B38 File Offset: 0x0027FD38
	public void Interact(PlayerController player)
	{
		if (this.IsValidForUse())
		{
			GameManager.Instance.LoadCustomLevel(this.targetLevelName);
		}
	}

	// Token: 0x060066ED RID: 26349 RVA: 0x00281B58 File Offset: 0x0027FD58
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060066EE RID: 26350 RVA: 0x00281B64 File Offset: 0x0027FD64
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040062B8 RID: 25272
	public bool IsResourcefulRatPit;

	// Token: 0x040062B9 RID: 25273
	public bool GoesToRatFloor;

	// Token: 0x040062BA RID: 25274
	public string targetLevelName;

	// Token: 0x040062BB RID: 25275
	public List<InteractableLock> WorldLocks;

	// Token: 0x040062BC RID: 25276
	public SpeculativeRigidbody FlightCollider;

	// Token: 0x040062BD RID: 25277
	public GlobalDungeonData.ValidTilesets TargetTileset = GlobalDungeonData.ValidTilesets.SEWERGEON;

	// Token: 0x040062BE RID: 25278
	public TileIndexGrid OverridePitIndex;

	// Token: 0x040062BF RID: 25279
	private bool m_hasOpened;

	// Token: 0x040062C0 RID: 25280
	public tk2dSpriteAnimator cryoAnimator;

	// Token: 0x040062C1 RID: 25281
	public string cryoArriveAnimation;

	// Token: 0x040062C2 RID: 25282
	public string cyroDepartAnimation;

	// Token: 0x040062C3 RID: 25283
	private FsmBool m_cryoBool;

	// Token: 0x040062C4 RID: 25284
	private FsmBool m_normalBool;

	// Token: 0x040062C5 RID: 25285
	private float m_timeHovering;

	// Token: 0x040062C6 RID: 25286
	private bool m_isLoading;
}
