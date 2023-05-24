using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000ED4 RID: 3796
public class DungeonDoorController : PersistentVFXManagerBehaviour, IPlayerInteractable
{
	// Token: 0x17000B67 RID: 2919
	// (get) Token: 0x060050B2 RID: 20658 RVA: 0x001C78D4 File Offset: 0x001C5AD4
	public bool IsUniqueVisiblityDoor
	{
		get
		{
			return this.hasEverBeenOpen && this.doorClosesAfterEveryOpen;
		}
	}

	// Token: 0x17000B68 RID: 2920
	// (get) Token: 0x060050B3 RID: 20659 RVA: 0x001C78EC File Offset: 0x001C5AEC
	public DungeonDoorController.DungeonDoorMode Mode
	{
		get
		{
			return this.doorMode;
		}
	}

	// Token: 0x17000B69 RID: 2921
	// (get) Token: 0x060050B4 RID: 20660 RVA: 0x001C78F4 File Offset: 0x001C5AF4
	public bool IsOpen
	{
		get
		{
			return this.doorMode == DungeonDoorController.DungeonDoorMode.BOSS_DOOR_ONLY_UNSEALS || this.doorMode == DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR || this.doorMode == DungeonDoorController.DungeonDoorMode.ONE_WAY_DOOR_ONLY_UNSEALS || this.m_open;
		}
	}

	// Token: 0x17000B6A RID: 2922
	// (get) Token: 0x060050B5 RID: 20661 RVA: 0x001C7924 File Offset: 0x001C5B24
	public bool IsOpenForVisibilityTest
	{
		get
		{
			return this.doorMode == DungeonDoorController.DungeonDoorMode.BOSS_DOOR_ONLY_UNSEALS || this.doorMode == DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR || this.doorMode == DungeonDoorController.DungeonDoorMode.ONE_WAY_DOOR_ONLY_UNSEALS || (!this.IsSealed && (this.IsUniqueVisiblityDoor || this.m_open));
		}
	}

	// Token: 0x17000B6B RID: 2923
	// (get) Token: 0x060050B6 RID: 20662 RVA: 0x001C797C File Offset: 0x001C5B7C
	// (set) Token: 0x060050B7 RID: 20663 RVA: 0x001C7984 File Offset: 0x001C5B84
	public bool IsSealed
	{
		get
		{
			return this.isSealed;
		}
		set
		{
			if (value && this.m_open)
			{
				this.Close();
			}
			if (this.isSealed != value)
			{
				if (value)
				{
					this.SealInternal();
				}
				else
				{
					this.UnsealInternal();
				}
			}
		}
	}

	// Token: 0x17000B6C RID: 2924
	// (get) Token: 0x060050B8 RID: 20664 RVA: 0x001C79C0 File Offset: 0x001C5BC0
	// (set) Token: 0x060050B9 RID: 20665 RVA: 0x001C79C8 File Offset: 0x001C5BC8
	public bool KeepBossDoorSealed { get; set; }

	// Token: 0x060050BA RID: 20666 RVA: 0x001C79D4 File Offset: 0x001C5BD4
	public void SetSealedSilently(bool v)
	{
		this.isSealed = v;
	}

	// Token: 0x060050BB RID: 20667 RVA: 0x001C79E0 File Offset: 0x001C5BE0
	public void DoSeal(RoomHandler sourceRoom)
	{
		if (this.doorMode == DungeonDoorController.DungeonDoorMode.ONE_WAY_DOOR_ONLY_UNSEALS)
		{
			if (this.subsidiaryDoor != null)
			{
				this.subsidiaryDoor.SealInternal();
			}
			if (this.subsidiaryBlocker != null)
			{
				this.subsidiaryBlocker.Seal();
			}
			return;
		}
		if (this.subsidiaryBlocker != null || this.subsidiaryDoor != null)
		{
			if (this.exitDefinition.upstreamExit.jointedExit)
			{
				if (((this.exitDefinition.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.NORTH || this.exitDefinition.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.SOUTH) && this.exitDefinition.upstreamRoom == sourceRoom) || ((this.exitDefinition.downstreamExit.referencedExit.exitDirection == DungeonData.Direction.NORTH || this.exitDefinition.downstreamExit.referencedExit.exitDirection == DungeonData.Direction.SOUTH) && this.exitDefinition.downstreamRoom == sourceRoom))
				{
					this.SealInternal();
				}
				else
				{
					if (this.subsidiaryDoor != null)
					{
						this.subsidiaryDoor.SealInternal();
					}
					if (this.subsidiaryBlocker != null)
					{
						this.subsidiaryBlocker.Seal();
					}
				}
			}
			else if (sourceRoom == this.exitDefinition.upstreamRoom)
			{
				this.SealInternal();
			}
			else
			{
				if (this.subsidiaryDoor != null)
				{
					this.subsidiaryDoor.SealInternal();
				}
				if (this.subsidiaryBlocker != null)
				{
					this.subsidiaryBlocker.Seal();
				}
			}
		}
		else
		{
			this.SealInternal();
		}
	}

	// Token: 0x060050BC RID: 20668 RVA: 0x001C7B9C File Offset: 0x001C5D9C
	public void AssignPressurePlate(PressurePlate source)
	{
		source.OnPressurePlateDepressed = (Action<PressurePlate>)Delegate.Combine(source.OnPressurePlateDepressed, new Action<PressurePlate>(this.OnPressurePlateTriggered));
	}

	// Token: 0x060050BD RID: 20669 RVA: 0x001C7BC0 File Offset: 0x001C5DC0
	private void OnPressurePlateTriggered(PressurePlate source)
	{
		source.OnPressurePlateDepressed = (Action<PressurePlate>)Delegate.Remove(source.OnPressurePlateDepressed, new Action<PressurePlate>(this.OnPressurePlateTriggered));
		this.DoUnseal(this.downstreamRoom);
	}

	// Token: 0x060050BE RID: 20670 RVA: 0x001C7BF0 File Offset: 0x001C5DF0
	public void DoUnseal(RoomHandler sourceRoom)
	{
		if (this.subsidiaryDoor != null && this.subsidiaryDoor.isSealed)
		{
			this.subsidiaryDoor.UnsealInternal();
		}
		if (this.subsidiaryBlocker != null && this.subsidiaryBlocker.isSealed)
		{
			this.subsidiaryBlocker.Unseal();
		}
		if (this.isSealed)
		{
			this.UnsealInternal();
		}
	}

	// Token: 0x060050BF RID: 20671 RVA: 0x001C7C68 File Offset: 0x001C5E68
	private void Start()
	{
		if (this.doorMode != DungeonDoorController.DungeonDoorMode.BOSS_DOOR_ONLY_UNSEALS && this.doorMode != DungeonDoorController.DungeonDoorMode.ONE_WAY_DOOR_ONLY_UNSEALS && this.doorMode != DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR)
		{
			for (int i = 0; i < this.doorModules.Length; i++)
			{
				tk2dSprite component = this.doorModules[i].animator.GetComponent<tk2dSprite>();
				component.depthUsesTrimmedBounds = true;
				SpeculativeRigidbody component2 = this.doorModules[i].animator.GetComponent<SpeculativeRigidbody>();
				SpeculativeRigidbody speculativeRigidbody = component2;
				speculativeRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
				SpeculativeRigidbody speculativeRigidbody2 = component2;
				speculativeRigidbody2.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(speculativeRigidbody2.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.OnEnterTrigger));
				this.doorModules[i].sprite = component;
				this.doorModules[i].rigidbody = component2;
				tk2dSpriteAnimator animator = this.doorModules[i].animator;
				animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnAnimationCompleted));
			}
			this.UpdateDoorDepths();
		}
		if (this.doorMode == DungeonDoorController.DungeonDoorMode.COMPLEX && !this.northSouth)
		{
			for (int j = 0; j < this.sealAnimators.Length; j++)
			{
				if (this.sealAnimators[j].sprite.attachParent != null)
				{
					this.sealAnimators[j].sprite.attachParent.DetachRenderer(this.sealAnimators[j].sprite);
				}
				this.sealAnimators[j].sprite.automaticallyManagesDepth = true;
				this.sealAnimators[j].sprite.attachParent = null;
				this.sealAnimators[j].sprite.depthUsesTrimmedBounds = false;
				this.sealAnimators[j].sprite.HeightOffGround = 0f;
			}
		}
		if (this.doorMode == DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR)
		{
			IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Round) + new IntVector2(-2, -1);
			for (int k = 0; k < 8; k++)
			{
				for (int l = 0; l < 6; l++)
				{
					IntVector2 intVector2 = intVector + new IntVector2(l, k);
					if (this.upstreamRoom != null)
					{
						this.upstreamRoom.FeatureCells.Add(intVector2);
					}
				}
			}
		}
		if (this.sealAnimators != null)
		{
			for (int m = 0; m < this.sealAnimators.Length; m++)
			{
				this.sealAnimators[m].alwaysUpdateOffscreen = true;
			}
		}
		if (this.exitDefinition != null && this.exitDefinition.upstreamExit != null && this.exitDefinition.upstreamExit.isLockedDoor)
		{
			this.isLocked = true;
		}
		if (this.isLocked && this.parentDoor != null && this.parentDoor.subsidiaryDoor == this)
		{
			this.isLocked = false;
		}
		if (this.isLocked)
		{
			if (this.LockAnimator == null)
			{
				this.BecomeLockedDoor();
			}
			RoomHandler.unassignedInteractableObjects.Add(this);
		}
		SpeculativeRigidbody[] componentsInChildren = base.GetComponentsInChildren<SpeculativeRigidbody>();
		for (int n = 0; n < componentsInChildren.Length; n++)
		{
			componentsInChildren[n].PreventPiercing = true;
		}
	}

	// Token: 0x060050C0 RID: 20672 RVA: 0x001C7FBC File Offset: 0x001C61BC
	public void ForceBecomeLockedDoor()
	{
		if (this.isLocked)
		{
			if (this.LockAnimator == null)
			{
				this.BecomeLockedDoor();
			}
			RoomHandler.unassignedInteractableObjects.Add(this);
		}
	}

	// Token: 0x060050C1 RID: 20673 RVA: 0x001C7FEC File Offset: 0x001C61EC
	protected void BecomeLockedDoor()
	{
		if (this.doorMode != DungeonDoorController.DungeonDoorMode.COMPLEX)
		{
			return;
		}
		if (!this.northSouth)
		{
			GameObject gameObject = (GameObject)BraveResources.Load("Global Prefabs/DoorLockPrefab_Horizontal", ".prefab");
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			float num = 0f;
			if (this.doorModules[0].animator && this.doorModules[0].animator.Sprite)
			{
				num = this.doorModules[0].animator.transform.localPosition.x + this.doorModules[0].animator.Sprite.GetBounds().max.x;
			}
			else if (this.doorModules[0].sprite)
			{
				num = this.doorModules[0].sprite.transform.localPosition.x + this.doorModules[0].sprite.GetBounds().max.x;
			}
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.localPosition = new Vector3(num, 0f, 0f);
			this.LockAnimator = gameObject2.GetComponent<tk2dSpriteAnimator>();
			this.ChainsAnimator = gameObject2.transform.GetChild(0).GetComponent<tk2dSpriteAnimator>();
		}
		else
		{
			GameObject gameObject3 = (GameObject)BraveResources.Load("Global Prefabs/DoorLockPrefab_Vertical", ".prefab");
			GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(gameObject3);
			gameObject4.transform.parent = base.transform;
			gameObject4.transform.localPosition = new Vector3(0f, -0.75f, 0f);
			this.LockAnimator = gameObject4.GetComponent<tk2dSpriteAnimator>();
			this.ChainsAnimator = gameObject4.transform.GetChild(0).GetComponent<tk2dSpriteAnimator>();
		}
		this.LockAnimator.sprite.UpdateZDepth();
		this.ChainsAnimator.sprite.UpdateZDepth();
		if (!this.northSouth)
		{
			this.LockAnimator.sprite.IsPerpendicular = true;
			this.ChainsAnimator.sprite.UpdateZDepth();
		}
		if (!this.northSouth && this.exitDefinition.upstreamExit.referencedExit.exitDirection == DungeonData.Direction.EAST)
		{
			this.FlipLockToOtherSide();
		}
	}

	// Token: 0x060050C2 RID: 20674 RVA: 0x001C8254 File Offset: 0x001C6454
	protected void UpdateDoorDepths()
	{
		for (int i = 0; i < this.doorModules.Length; i++)
		{
			if (!this.doorModules[i].isLerping)
			{
				float num = ((!this.m_open) ? this.doorModules[i].closedDepth : this.doorModules[i].openDepth);
				if (!this.northSouth && !this.doorModules[i].sprite.depthUsesTrimmedBounds)
				{
					num = -5.25f;
				}
				if (this.doorModules[i].sprite.HeightOffGround != num)
				{
					this.AnimationDepthLerp(this.doorModules[i].sprite, num, null, this.doorModules[i], !this.northSouth && i == 0);
				}
			}
		}
	}

	// Token: 0x060050C3 RID: 20675 RVA: 0x001C832C File Offset: 0x001C652C
	private void Update()
	{
		if (this.isSealed && (this.doorMode == DungeonDoorController.DungeonDoorMode.BOSS_DOOR_ONLY_UNSEALS || (!this.m_finalBossDoorHasOpened && this.doorMode == DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR)))
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i] && GameManager.Instance.AllPlayers[i].healthHaver)
				{
					if (!GameManager.Instance.AllPlayers[i].healthHaver.IsDead)
					{
						if (!GameManager.Instance.PreventPausing)
						{
							if (GameManager.Instance.AllPlayers[i].CurrentRoom != null)
							{
								if ((GameManager.Instance.AllPlayers[i].CurrentRoom == this.upstreamRoom || GameManager.Instance.AllPlayers[i].CurrentRoom == this.downstreamRoom) && GameManager.Instance.AllPlayers[i].CurrentRoom.UnsealConditionsMet() && (this.unsealDistanceMaximum <= 0f || Vector2.Distance(this.sealAnimators[0].Sprite.WorldCenter, GameManager.Instance.AllPlayers[i].specRigidbody.UnitCenter) < this.unsealDistanceMaximum))
								{
									if (this.doorMode == DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR)
									{
										bool flag = false;
										if (GameManager.Instance.AllPlayers[i].CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
										{
											List<AIActor> activeEnemies = GameManager.Instance.AllPlayers[i].CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
											for (int j = 0; j < activeEnemies.Count; j++)
											{
												if (activeEnemies[j] && !activeEnemies[j].IgnoreForRoomClear && activeEnemies[j].HasBeenEngaged && activeEnemies[j].IsNormalEnemy)
												{
													flag = true;
												}
											}
										}
										if (!flag)
										{
											this.m_finalBossDoorHasOpened = true;
											this.UnsealInternal();
										}
									}
									else if (this.doorMode != DungeonDoorController.DungeonDoorMode.BOSS_DOOR_ONLY_UNSEALS || !this.KeepBossDoorSealed)
									{
										this.UnsealInternal();
									}
								}
							}
						}
					}
				}
			}
		}
		if (this.isLocked && this.lockIsBusted)
		{
			string text = ((!this.northSouth) ? "lock_guy_side_busted" : "lock_guy_busted");
			if (!this.LockAnimator.IsPlaying(text))
			{
				this.LockAnimator.Play(text);
			}
		}
		else if (this.northSouth && this.isLocked)
		{
			Vector2 vector = Vector2.zero;
			for (int k = 0; k < this.doorModules.Length; k++)
			{
				vector += this.doorModules[k].rigidbody.UnitCenter;
			}
			vector /= (float)this.doorModules.Length;
			float num = Vector2.Distance(vector, GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter);
			if (!this.m_lockHasApproached && num < 2.5f)
			{
				this.LockAnimator.Play("lock_guy_approach");
				this.m_lockHasApproached = true;
			}
			else if (num > 2.5f)
			{
				if (this.m_lockHasLaughed)
				{
					this.LockAnimator.Play("lock_guy_spit");
				}
				this.m_lockHasLaughed = false;
				this.m_lockHasApproached = false;
			}
			if (!this.m_lockHasSpit && this.LockAnimator != null && this.LockAnimator.IsPlaying("lock_guy_spit") && this.LockAnimator.CurrentFrame == 3)
			{
				this.m_lockHasSpit = true;
				GameObject gameObject = SpawnManager.SpawnVFX(BraveResources.Load("Global VFX/VFX_Lock_Spit", ".prefab") as GameObject, false);
				tk2dSprite componentInChildren = gameObject.GetComponentInChildren<tk2dSprite>();
				componentInChildren.UpdateZDepth();
				componentInChildren.PlaceAtPositionByAnchor(this.LockAnimator.sprite.WorldCenter, tk2dBaseSprite.Anchor.UpperCenter);
			}
		}
		if (this.northSouth && this.isSealed)
		{
			for (int l = 0; l < this.sealAnimators.Length; l++)
			{
				this.sealAnimators[l].sprite.UpdateZDepth();
			}
			if (!string.IsNullOrEmpty(this.playerNearSealedAnimationName))
			{
				Vector2 vector2 = Vector2.zero;
				for (int m = 0; m < this.doorModules.Length; m++)
				{
					vector2 += this.doorModules[m].rigidbody.UnitCenter;
				}
				vector2 /= (float)this.doorModules.Length;
				if (Vector2.Distance(vector2, GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter) < 4f)
				{
					for (int n = 0; n < this.sealAnimators.Length; n++)
					{
						if (!this.sealAnimators[n].IsPlaying(this.playerNearSealedAnimationName) && !this.sealAnimators[n].IsPlaying(this.unsealAnimationName) && !this.sealAnimators[n].IsPlaying(this.sealAnimationName))
						{
							this.sealAnimators[n].Play(this.playerNearSealedAnimationName);
						}
					}
				}
				else
				{
					for (int num2 = 0; num2 < this.sealAnimators.Length; num2++)
					{
						if (this.sealAnimators[num2].IsPlaying(this.playerNearSealedAnimationName))
						{
							this.sealAnimators[num2].Stop();
							tk2dSpriteAnimationClip clipByName = this.sealAnimators[num2].GetClipByName(this.sealAnimationName);
							this.sealAnimators[num2].Sprite.SetSprite(clipByName.frames[clipByName.frames.Length - 1].spriteId);
						}
					}
				}
			}
		}
	}

	// Token: 0x060050C4 RID: 20676 RVA: 0x001C8920 File Offset: 0x001C6B20
	protected void AnimationDepthLerp(tk2dSprite targetSprite, float targetDepth, tk2dSpriteAnimationClip clip, DungeonDoorController.DoorModule m = null, bool isSpecialHorizontalTopCase = false)
	{
		float num = 1f;
		if (clip != null)
		{
			num = (float)clip.frames.Length / clip.fps;
		}
		base.StartCoroutine(this.DepthLerp(targetSprite, targetDepth, num, m, isSpecialHorizontalTopCase));
	}

	// Token: 0x060050C5 RID: 20677 RVA: 0x001C8960 File Offset: 0x001C6B60
	private IEnumerator DepthLerp(tk2dSprite targetSprite, float targetDepth, float duration, DungeonDoorController.DoorModule m = null, bool isSpecialHorizontalTopCase = false)
	{
		if (m != null)
		{
			if (!this.m_open)
			{
				targetSprite.IsPerpendicular = true;
			}
			m.isLerping = true;
		}
		float elapsed = 0f;
		float startingDepth = targetSprite.HeightOffGround;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			targetSprite.HeightOffGround = Mathf.Lerp(startingDepth, targetDepth, t);
			if (this.m_open && isSpecialHorizontalTopCase)
			{
				targetSprite.depthUsesTrimmedBounds = false;
				targetSprite.HeightOffGround = -5.25f;
			}
			targetSprite.UpdateZDepth();
			yield return null;
		}
		targetSprite.HeightOffGround = (targetSprite.depthUsesTrimmedBounds ? targetDepth : (-5.25f));
		targetSprite.UpdateZDepth();
		if (m != null)
		{
			if (this.m_open)
			{
				targetSprite.IsPerpendicular = m.openPerpendicular;
			}
			m.isLerping = false;
		}
		yield break;
	}

	// Token: 0x060050C6 RID: 20678 RVA: 0x001C89A0 File Offset: 0x001C6BA0
	public void OnAnimationCompleted(tk2dSpriteAnimator a, tk2dSpriteAnimationClip c)
	{
		this.UpdateDoorDepths();
	}

	// Token: 0x060050C7 RID: 20679 RVA: 0x001C89A8 File Offset: 0x001C6BA8
	public void Open(bool flipped = false)
	{
		if (this.doorMode == DungeonDoorController.DungeonDoorMode.BOSS_DOOR_ONLY_UNSEALS)
		{
			return;
		}
		if (this.doorMode == DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR)
		{
			return;
		}
		if (this.doorMode == DungeonDoorController.DungeonDoorMode.ONE_WAY_DOOR_ONLY_UNSEALS)
		{
			return;
		}
		if (this.IsSealed || this.isLocked || this.m_isDestroyed)
		{
			return;
		}
		if (!this.m_open)
		{
			if (!this.hasEverBeenOpen)
			{
				RoomHandler roomHandler = null;
				if (this.exitDefinition != null)
				{
					if (this.exitDefinition.upstreamRoom != null && this.exitDefinition.upstreamRoom.WillSealOnEntry())
					{
						roomHandler = this.exitDefinition.upstreamRoom;
					}
					else if (this.exitDefinition.downstreamRoom != null && this.exitDefinition.downstreamRoom.WillSealOnEntry())
					{
						roomHandler = this.exitDefinition.downstreamRoom;
					}
				}
				if (roomHandler != null && (this.subsidiaryDoor || this.parentDoor))
				{
					DungeonDoorController dungeonDoorController = ((!this.subsidiaryDoor) ? this.parentDoor : this.subsidiaryDoor);
					Vector2 center = roomHandler.area.Center;
					float num = Vector2.Distance(center, base.transform.position);
					float num2 = Vector2.Distance(center, dungeonDoorController.transform.position);
					if (num2 < num)
					{
						roomHandler = null;
					}
				}
				if (roomHandler != null)
				{
					BraveMemory.HandleRoomEntered(roomHandler.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All));
				}
			}
			AkSoundEngine.PostEvent("play_OBJ_door_open_01", base.gameObject);
			this.SetState(true, flipped);
			if (this.doorClosesAfterEveryOpen)
			{
				base.StartCoroutine(this.DelayedReclose());
			}
		}
	}

	// Token: 0x060050C8 RID: 20680 RVA: 0x001C8B58 File Offset: 0x001C6D58
	private IEnumerator DelayedReclose()
	{
		yield return new WaitForSeconds(0.1f);
		for (;;)
		{
			bool containsPlayer = false;
			for (int i = 0; i < this.doorModules.Length; i++)
			{
				for (int j = 0; j < this.doorModules[i].rigidbody.PixelColliders.Count; j++)
				{
					List<SpeculativeRigidbody> overlappingRigidbodies = PhysicsEngine.Instance.GetOverlappingRigidbodies(this.doorModules[i].rigidbody.PixelColliders[j], null, false);
					for (int k = 0; k < overlappingRigidbodies.Count; k++)
					{
						if (overlappingRigidbodies[k].GetComponent<PlayerController>() != null)
						{
							containsPlayer = true;
							break;
						}
					}
					if (containsPlayer)
					{
						break;
					}
				}
				if (containsPlayer)
				{
					break;
				}
			}
			if (!containsPlayer)
			{
				break;
			}
			yield return null;
		}
		this.Close();
		yield break;
	}

	// Token: 0x060050C9 RID: 20681 RVA: 0x001C8B74 File Offset: 0x001C6D74
	protected virtual void OnRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		this.CheckForPlayerCollision(rigidbodyCollision.OtherRigidbody, rigidbodyCollision.Normal);
		if (!this.IsSealed && this.isLocked && rigidbodyCollision.OtherRigidbody)
		{
			if (rigidbodyCollision.OtherRigidbody.GetComponent<KeyProjModifier>())
			{
				this.Unlock();
			}
			if (rigidbodyCollision.OtherRigidbody.GetComponent<KeyBullet>() != null)
			{
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.DOORS_UNLOCKED_WITH_KEY_BULLETS, 1f);
				this.isLocked = false;
				bool flag = false;
				if (rigidbodyCollision.Normal.y < 0f && this.northSouth)
				{
					flag = true;
				}
				if (rigidbodyCollision.Normal.x < 0f && !this.northSouth)
				{
					flag = true;
				}
				this.Open(flag);
				this.m_isDestroyed = true;
			}
		}
	}

	// Token: 0x060050CA RID: 20682 RVA: 0x001C8C58 File Offset: 0x001C6E58
	private void OnEnterTrigger(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		this.CheckForPlayerCollision(specRigidbody, sourceSpecRigidbody.UnitCenter - specRigidbody.UnitCenter);
	}

	// Token: 0x060050CB RID: 20683 RVA: 0x001C8C74 File Offset: 0x001C6E74
	private void CheckForPlayerCollision(SpeculativeRigidbody otherRigidbody, Vector2 normal)
	{
		if (this.isSealed || this.isLocked)
		{
			return;
		}
		PlayerController component = otherRigidbody.GetComponent<PlayerController>();
		if (component != null && !this.m_open)
		{
			bool flag = false;
			if (normal.y < 0f && this.northSouth)
			{
				flag = true;
			}
			if (normal.x < 0f && !this.northSouth)
			{
				flag = true;
			}
			bool flag2 = GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER;
			if (flag2)
			{
				this.Open(flag);
				BraveInput.DoVibrationForAllPlayers(Vibration.Time.Quick, Vibration.Strength.Light);
			}
			else
			{
				bool flag3 = true;
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					if (!GameManager.Instance.AllPlayers[i].IsGhost)
					{
						if (!GameManager.Instance.AllPlayers[i].healthHaver.IsDead || GameManager.Instance.AllPlayers[i].IsGhost)
						{
							float distanceToPlayer = this.GetDistanceToPlayer(GameManager.Instance.AllPlayers[i].specRigidbody);
							if (distanceToPlayer > 0.3f)
							{
								flag3 = false;
								break;
							}
						}
					}
				}
				if (flag3)
				{
					this.Open(flag);
					BraveInput.DoVibrationForAllPlayers(Vibration.Time.Quick, Vibration.Strength.Light);
					if (this.exitDefinition != null && this.exitDefinition.downstreamRoom != null && ((this.exitDefinition.upstreamRoom != null && this.exitDefinition.upstreamRoom.WillSealOnEntry()) || (this.exitDefinition.downstreamRoom != null && this.exitDefinition.downstreamRoom.WillSealOnEntry())))
					{
						this.HandleCoopPlayers(flag);
					}
				}
				else if (!this.m_isCoopArrowing)
				{
					base.StartCoroutine(this.DoCoopArrowWhilePlayerIsNear(component));
				}
			}
		}
	}

	// Token: 0x060050CC RID: 20684 RVA: 0x001C8E5C File Offset: 0x001C705C
	private IEnumerator DoCoopArrowWhilePlayerIsNear(PlayerController nearPlayer)
	{
		if (this.m_isCoopArrowing)
		{
			yield break;
		}
		this.m_isCoopArrowing = true;
		while (!this.IsSealed)
		{
			if (!this.IsOpen)
			{
				if (!this.isLocked)
				{
					float playerDist = this.GetDistanceToPlayer(nearPlayer.specRigidbody);
					if (playerDist <= 1f)
					{
						if (nearPlayer.IsPrimaryPlayer)
						{
							GameManager.Instance.SecondaryPlayer.DoCoopArrow();
						}
						else
						{
							GameManager.Instance.PrimaryPlayer.DoCoopArrow();
						}
						yield return null;
						continue;
					}
				}
			}
			IL_105:
			this.m_isCoopArrowing = false;
			yield break;
		}
		goto IL_105;
	}

	// Token: 0x060050CD RID: 20685 RVA: 0x001C8E80 File Offset: 0x001C7080
	private void HandleCoopPlayers(bool flipped)
	{
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			Vector2 vector = Vector2.zero;
			if (this.northSouth)
			{
				if (flipped)
				{
					vector = -Vector2.up * 1.25f;
				}
				else
				{
					vector = Vector2.up * 2.75f;
				}
			}
			else if (flipped)
			{
				vector = -Vector2.right * 1.25f;
			}
			else
			{
				vector = Vector2.right * 1.25f;
			}
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i])
				{
					if (!GameManager.Instance.AllPlayers[i].IsGhost)
					{
						Vector2 vector2 = this.GetSRBAveragePosition() + vector;
						float num = ((!this.northSouth) ? 0.1f : 0.2f);
						List<SpeculativeRigidbody> list = new List<SpeculativeRigidbody>();
						for (int j = 0; j < this.doorModules.Length; j++)
						{
							list.Add(this.doorModules[j].sprite.specRigidbody);
						}
						for (int k = 0; k < this.sealAnimators.Length; k++)
						{
							list.Add(this.sealAnimators[k].sprite.specRigidbody);
						}
						GameManager.Instance.AllPlayers[i].ForceMoveInDirectionUntilThreshold(vector.normalized, (!this.northSouth) ? vector2.x : vector2.y, num, 1f, list);
					}
				}
			}
		}
	}

	// Token: 0x060050CE RID: 20686 RVA: 0x001C9044 File Offset: 0x001C7244
	public void Close()
	{
		if (this.doorMode == DungeonDoorController.DungeonDoorMode.BOSS_DOOR_ONLY_UNSEALS)
		{
			return;
		}
		if (this.doorMode == DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR)
		{
			return;
		}
		if (this.doorMode == DungeonDoorController.DungeonDoorMode.ONE_WAY_DOOR_ONLY_UNSEALS)
		{
			return;
		}
		if (this.m_isDestroyed)
		{
			return;
		}
		if (this.m_open)
		{
			this.SetState(false, false);
		}
	}

	// Token: 0x060050CF RID: 20687 RVA: 0x001C9098 File Offset: 0x001C7298
	private IEnumerator MoveTransformSmoothly(Transform target, Vector3 delta, float animationTime, Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> action)
	{
		float elapsed = 0f;
		Vector3 startPosition = target.position;
		Vector3 endPosition = target.position + delta;
		tk2dSprite targetSprite = target.GetComponent<tk2dSprite>();
		float startHeightOffGround = targetSprite.HeightOffGround;
		float endHeightOffGround = targetSprite.HeightOffGround + delta.y + delta.z;
		while (elapsed < animationTime)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / animationTime));
			Vector3 currentPosition = BraveUtility.QuantizeVector(Vector3.Lerp(startPosition, endPosition, t));
			targetSprite.HeightOffGround = Mathf.Lerp(startHeightOffGround, endHeightOffGround, t);
			target.position = currentPosition;
			targetSprite.UpdateZDepth();
			yield return null;
		}
		targetSprite.HeightOffGround = endHeightOffGround;
		target.position = endPosition;
		targetSprite.UpdateZDepth();
		if (action != null)
		{
			action(target.GetComponent<tk2dSpriteAnimator>(), null);
		}
		yield break;
	}

	// Token: 0x060050D0 RID: 20688 RVA: 0x001C90CC File Offset: 0x001C72CC
	private void SealInternal()
	{
		this.m_wasOpenWhenSealed = this.m_open;
		if (this.m_open)
		{
			this.Close();
		}
		if (this.Mode == DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR)
		{
			this.sealAnimators[0].Play(this.sealAnimationName);
			this.sealAnimators[0].specRigidbody.PrimaryPixelCollider.Enabled = true;
		}
		else if (!string.IsNullOrEmpty(this.sealAnimationName))
		{
			for (int i = 0; i < this.sealAnimators.Length; i++)
			{
				this.sealAnimators[i].Sprite.UpdateZDepth();
				this.sealAnimators[i].AnimationCompleted = null;
				tk2dSpriteAnimator tk2dSpriteAnimator = this.sealAnimators[i];
				tk2dSpriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(tk2dSpriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.OnSealAnimationEvent));
				this.sealAnimators[i].gameObject.SetActive(true);
				this.sealAnimators[i].Play(this.sealAnimationName);
				if (this.sealAnimators[i].Sprite.specRigidbody != null)
				{
					this.sealAnimators[i].Sprite.specRigidbody.enabled = true;
					this.sealAnimators[i].Sprite.specRigidbody.Initialize();
					for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
					{
						this.sealAnimators[i].Sprite.specRigidbody.RegisterGhostCollisionException(GameManager.Instance.AllPlayers[j].specRigidbody);
					}
				}
			}
			AkSoundEngine.PostEvent("Play_OBJ_gate_slam_01", base.gameObject);
			for (int k = 0; k < this.sealChainAnimators.Length; k++)
			{
				if (this.sealChainAnimators[k].GetClipByName(this.sealAnimationName + "_chain") != null)
				{
					this.sealChainAnimators[k].AnimationCompleted = null;
					this.sealChainAnimators[k].gameObject.SetActive(true);
					this.sealChainAnimators[k].Play(this.sealAnimationName + "_chain");
				}
			}
		}
		else if (this.Mode != DungeonDoorController.DungeonDoorMode.ONE_WAY_DOOR_ONLY_UNSEALS)
		{
			if (this.Mode == DungeonDoorController.DungeonDoorMode.BOSS_DOOR_ONLY_UNSEALS)
			{
				for (int l = 0; l < this.sealAnimators.Length; l++)
				{
					base.StartCoroutine(this.MoveTransformSmoothly(this.sealAnimators[l].transform, new Vector3(0f, -1.5625f, 0f), 1.5f, null));
				}
			}
		}
		if (this.doorMode == DungeonDoorController.DungeonDoorMode.SINGLE_DOOR)
		{
			for (int m = 0; m < this.doorModules.Length; m++)
			{
				this.doorModules[m].rigidbody.enabled = true;
			}
		}
		for (int n = 0; n < this.sealAnimators.Length; n++)
		{
			tk2dSpriteAnimator tk2dSpriteAnimator2 = this.sealAnimators[n];
			if (tk2dSpriteAnimator2.GetComponent<SpeculativeRigidbody>() != null)
			{
				tk2dSpriteAnimator2.GetComponent<SpeculativeRigidbody>().enabled = true;
			}
		}
		this.isSealed = true;
	}

	// Token: 0x060050D1 RID: 20689 RVA: 0x001C93E0 File Offset: 0x001C75E0
	private GameObject SpawnVFXAtPoint(GameObject vfx, Vector3 position)
	{
		GameObject gameObject = SpawnManager.SpawnVFX(vfx, position, Quaternion.identity, true);
		tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
		component.HeightOffGround = 0.25f;
		component.PlaceAtPositionByAnchor(position, tk2dBaseSprite.Anchor.MiddleCenter);
		component.IsPerpendicular = false;
		component.UpdateZDepth();
		return gameObject;
	}

	// Token: 0x060050D2 RID: 20690 RVA: 0x001C9424 File Offset: 0x001C7624
	private IEnumerator DelayedLayerChange(GameObject targetObject, string layer, float delay)
	{
		yield return new WaitForSeconds(delay);
		targetObject.layer = LayerMask.NameToLayer(layer);
		yield break;
	}

	// Token: 0x060050D3 RID: 20691 RVA: 0x001C9450 File Offset: 0x001C7650
	private void UnsealInternal()
	{
		if (this.Mode == DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR)
		{
			if (!this.IsSealed)
			{
				return;
			}
			this.sealAnimators[0].Play(this.unsealAnimationName);
			this.sealVFX[0].gameObject.SetActive(true);
			this.sealVFX[0].PlayAndDisableObject(string.Empty, null);
		}
		else if (!string.IsNullOrEmpty(this.unsealAnimationName))
		{
			for (int i = 0; i < this.sealAnimators.Length; i++)
			{
				this.sealAnimators[i].Sprite.UpdateZDepth();
				this.sealAnimators[i].Play(this.unsealAnimationName);
				tk2dSpriteAnimator tk2dSpriteAnimator = this.sealAnimators[i];
				tk2dSpriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(tk2dSpriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnUnsealAnimationCompleted));
				this.sealAnimators[i].AnimationEventTriggered = null;
				if (this.sealAnimators[i].Sprite.specRigidbody != null)
				{
					this.sealAnimators[i].Sprite.specRigidbody.enabled = false;
				}
			}
			AkSoundEngine.PostEvent("Play_OBJ_gate_open_01", base.gameObject);
			for (int j = 0; j < this.sealChainAnimators.Length; j++)
			{
				if (this.sealChainAnimators[j].GetClipByName(this.unsealAnimationName + "_chain") != null)
				{
					this.sealChainAnimators[j].Play(this.unsealAnimationName + "_chain");
					tk2dSpriteAnimator tk2dSpriteAnimator2 = this.sealChainAnimators[j];
					tk2dSpriteAnimator2.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(tk2dSpriteAnimator2.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnUnsealAnimationCompleted));
					this.sealChainAnimators[j].AnimationEventTriggered = null;
				}
			}
		}
		else if (this.Mode == DungeonDoorController.DungeonDoorMode.ONE_WAY_DOOR_ONLY_UNSEALS)
		{
			if (this.northSouth)
			{
				for (int k = 0; k < this.sealAnimators.Length; k++)
				{
					this.sealAnimators[k].gameObject.layer = LayerMask.NameToLayer("BG_Critical");
					base.StartCoroutine(this.MoveTransformSmoothly(this.sealAnimators[k].transform, new Vector3(0f, -2f, -2.25f), 1.5f, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnUnsealAnimationCompleted)));
				}
				base.StartCoroutine(this.DelayedLayerChange(this.sealAnimators[0].transform.GetChild(0).gameObject, "BG_Critical", 1.5f));
				if (GameManager.Instance.Dungeon.SecretRoomHorizontalPoofVFX != null)
				{
					GameObject gameObject = this.SpawnVFXAtPoint(GameManager.Instance.Dungeon.SecretRoomHorizontalPoofVFX, base.transform.position + new Vector3(0f, -0.25f, 0f));
					tk2dSpriteAnimator component = gameObject.GetComponent<tk2dSpriteAnimator>();
					component.PlayAndDestroyObject(string.Empty, null);
				}
			}
			else
			{
				for (int l = 0; l < this.sealAnimators.Length; l++)
				{
					this.sealAnimators[l].gameObject.layer = LayerMask.NameToLayer("BG_Critical");
					base.StartCoroutine(this.MoveTransformSmoothly(this.sealAnimators[l].transform, new Vector3(0f, -2.5f, -3.25f), 1.5f, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnUnsealAnimationCompleted)));
				}
				base.StartCoroutine(this.DelayedLayerChange(this.sealAnimators[0].transform.GetChild(0).gameObject, "BG_Critical", 1.35f));
				if (GameManager.Instance.Dungeon.SecretRoomVerticalPoofVFX != null)
				{
					GameObject gameObject2 = this.SpawnVFXAtPoint(GameManager.Instance.Dungeon.SecretRoomVerticalPoofVFX, base.transform.position + Vector3.up);
					gameObject2.transform.position = base.transform.position + new Vector3(-1.25f, 0.75f, 0f);
					tk2dSpriteAnimator component2 = gameObject2.GetComponent<tk2dSpriteAnimator>();
					component2.PlayAndDestroyObject(string.Empty, null);
				}
			}
			AkSoundEngine.PostEvent("Play_OBJ_secret_door_01", base.gameObject);
		}
		else if (this.Mode == DungeonDoorController.DungeonDoorMode.BOSS_DOOR_ONLY_UNSEALS)
		{
			for (int m = 0; m < this.sealAnimators.Length; m++)
			{
				base.StartCoroutine(this.MoveTransformSmoothly(this.sealAnimators[m].transform, new Vector3(0f, 1.5625f, 0f), 1.5f, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnUnsealAnimationCompleted)));
			}
			AkSoundEngine.PostEvent("Play_OBJ_bossdoor_open_01", base.gameObject);
		}
		if (this.doorMode == DungeonDoorController.DungeonDoorMode.SINGLE_DOOR && this.m_open && !this.isLocked)
		{
			for (int n = 0; n < this.doorModules.Length; n++)
			{
				this.doorModules[n].rigidbody.enabled = false;
			}
		}
		if (this.usesUnsealScreenShake)
		{
			GameManager.Instance.MainCameraController.DoScreenShake(this.unsealScreenShake, new Vector2?(base.transform.position), false);
		}
		this.isSealed = false;
		if (this.m_wasOpenWhenSealed)
		{
			this.Open(false);
		}
	}

	// Token: 0x060050D4 RID: 20692 RVA: 0x001C99A4 File Offset: 0x001C7BA4
	public void OnSealAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		if (clip.GetFrame(frameNo).eventInfo == "SealVFX" && this.sealVFX.Length > 0)
		{
			for (int i = 0; i < this.sealVFX.Length; i++)
			{
				this.sealVFX[i].gameObject.SetActive(true);
				this.sealVFX[i].Play();
			}
			animator.Sprite.UpdateZDepth();
		}
	}

	// Token: 0x060050D5 RID: 20693 RVA: 0x001C9A20 File Offset: 0x001C7C20
	private IEnumerator HandleFrameDelayedUnsealedVFXOverride()
	{
		if (!this.m_hasGC)
		{
			this.m_hasGC = true;
			yield return null;
		}
		this.unsealedVFXOverride.SetActive(true);
		BraveInput.DoVibrationForAllPlayers(Vibration.Time.Normal, Vibration.Strength.Medium);
		yield break;
	}

	// Token: 0x060050D6 RID: 20694 RVA: 0x001C9A3C File Offset: 0x001C7C3C
	public void OnUnsealAnimationCompleted(tk2dSpriteAnimator a, tk2dSpriteAnimationClip c)
	{
		if (this.hideSealAnimators)
		{
			a.gameObject.SetActive(false);
		}
		if (a.GetComponent<SpeculativeRigidbody>() != null)
		{
			a.GetComponent<SpeculativeRigidbody>().enabled = false;
		}
		if (this.unsealedVFXOverride != null)
		{
			base.StartCoroutine(this.HandleFrameDelayedUnsealedVFXOverride());
		}
	}

	// Token: 0x060050D7 RID: 20695 RVA: 0x001C9A9C File Offset: 0x001C7C9C
	public void OnCloseAnimationCompleted(tk2dSpriteAnimator a, tk2dSpriteAnimationClip c)
	{
		a.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(a.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnCloseAnimationCompleted));
		if (a.Sprite.FlipX)
		{
			a.Sprite.FlipX = false;
		}
	}

	// Token: 0x060050D8 RID: 20696 RVA: 0x001C9ADC File Offset: 0x001C7CDC
	private void SetState(bool openState, bool flipped = false)
	{
		if (openState)
		{
			this.hasEverBeenOpen = true;
		}
		base.TriggerPersistentVFXClear();
		this.m_open = openState;
		if (!this.northSouth)
		{
			for (int i = 0; i < this.doorModules.Length; i++)
			{
				if (this.doorModules[i].horizontalFlips)
				{
					this.doorModules[i].sprite.FlipX = ((!openState) ? this.m_openIsFlipped : flipped);
				}
			}
		}
		if (openState)
		{
			for (int j = 0; j < this.doorModules.Length; j++)
			{
				this.m_openIsFlipped = flipped;
				DungeonDoorController.DoorModule doorModule = this.doorModules[j];
				string text = doorModule.openAnimationName;
				tk2dSpriteAnimationClip tk2dSpriteAnimationClip = null;
				if (!string.IsNullOrEmpty(text))
				{
					if (flipped && this.northSouth)
					{
						text = text.Replace("_north", "_south");
					}
					tk2dSpriteAnimationClip = doorModule.animator.GetClipByName(text);
				}
				if (tk2dSpriteAnimationClip != null)
				{
					doorModule.animator.Play(tk2dSpriteAnimationClip);
				}
				for (int k = 0; k < doorModule.AOAnimatorsToDisable.Count; k++)
				{
					doorModule.AOAnimatorsToDisable[k].PlayAndDisableObject(string.Empty, null);
				}
				doorModule.rigidbody.enabled = false;
				this.AnimationDepthLerp(doorModule.sprite, doorModule.openDepth, tk2dSpriteAnimationClip, doorModule, !this.northSouth && j == 0);
			}
		}
		else
		{
			for (int l = 0; l < this.doorModules.Length; l++)
			{
				DungeonDoorController.DoorModule doorModule2 = this.doorModules[l];
				string text2 = doorModule2.closeAnimationName;
				tk2dSpriteAnimationClip tk2dSpriteAnimationClip2 = null;
				if (!string.IsNullOrEmpty(text2))
				{
					if (this.m_openIsFlipped && this.northSouth)
					{
						text2 = text2.Replace("_north", "_south");
					}
					tk2dSpriteAnimationClip2 = doorModule2.animator.GetClipByName(text2);
				}
				if (tk2dSpriteAnimationClip2 != null)
				{
					doorModule2.animator.Play(tk2dSpriteAnimationClip2);
					tk2dSpriteAnimator animator = doorModule2.animator;
					animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnCloseAnimationCompleted));
				}
				else
				{
					doorModule2.animator.StopAndResetFrame();
				}
				for (int m = 0; m < doorModule2.AOAnimatorsToDisable.Count; m++)
				{
					doorModule2.AOAnimatorsToDisable[m].gameObject.SetActive(true);
					doorModule2.AOAnimatorsToDisable[m].StopAndResetFrame();
				}
				doorModule2.rigidbody.enabled = true;
				this.AnimationDepthLerp(doorModule2.sprite, doorModule2.closedDepth, tk2dSpriteAnimationClip2, doorModule2, false);
			}
		}
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		if (this.upstreamRoom != null && this.upstreamRoom.visibility != RoomHandler.VisibilityStatus.OBSCURED)
		{
			Pixelator.Instance.ProcessRoomAdditionalExits(intVector, this.upstreamRoom, false);
		}
		if (this.downstreamRoom != null && this.downstreamRoom.visibility != RoomHandler.VisibilityStatus.OBSCURED)
		{
			Pixelator.Instance.ProcessRoomAdditionalExits(intVector, this.downstreamRoom, false);
		}
	}

	// Token: 0x060050D9 RID: 20697 RVA: 0x001C9E00 File Offset: 0x001C8000
	public float GetDistanceToPlayer(SpeculativeRigidbody playerRigidbody)
	{
		Vector4 srbboundingBox = this.GetSRBBoundingBox();
		return BraveMathCollege.DistBetweenRectangles(new Vector2(srbboundingBox.x, srbboundingBox.y), new Vector2(srbboundingBox.z - srbboundingBox.x, srbboundingBox.w - srbboundingBox.y), playerRigidbody.UnitBottomLeft, playerRigidbody.UnitDimensions);
	}

	// Token: 0x060050DA RID: 20698 RVA: 0x001C9E5C File Offset: 0x001C805C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060050DB RID: 20699 RVA: 0x001C9E64 File Offset: 0x001C8064
	private Vector2 GetModuleAveragePosition()
	{
		Vector2 vector = Vector2.zero;
		for (int i = 0; i < this.doorModules.Length; i++)
		{
			vector += this.doorModules[i].animator.Sprite.WorldCenter;
		}
		return vector / (float)this.doorModules.Length;
	}

	// Token: 0x060050DC RID: 20700 RVA: 0x001C9EC0 File Offset: 0x001C80C0
	private Vector4 GetSRBBoundingBox()
	{
		Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
		Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
		for (int i = 0; i < this.doorModules.Length; i++)
		{
			if (this.doorModules[i].rigidbody != null)
			{
				vector = Vector2.Min(vector, this.doorModules[i].rigidbody.UnitBottomLeft);
				vector2 = Vector2.Max(vector2, this.doorModules[i].rigidbody.UnitTopRight);
			}
		}
		return new Vector4(vector.x, vector.y, vector2.x, vector2.y);
	}

	// Token: 0x060050DD RID: 20701 RVA: 0x001C9F74 File Offset: 0x001C8174
	private Vector2 GetSRBAveragePosition()
	{
		Vector2 vector = Vector2.zero;
		for (int i = 0; i < this.doorModules.Length; i++)
		{
			vector += this.doorModules[i].animator.GetComponent<SpeculativeRigidbody>().UnitCenter;
			if (!this.northSouth)
			{
				vector += new Vector2(0f, -0.375f);
			}
		}
		return vector / (float)this.doorModules.Length;
	}

	// Token: 0x060050DE RID: 20702 RVA: 0x001C9FF0 File Offset: 0x001C81F0
	public float GetDistanceToPoint(Vector2 point)
	{
		if (!this.isLocked || this.lockIsBusted)
		{
			return 1000f;
		}
		return Vector2.Distance(point, this.GetModuleAveragePosition()) / 3f;
	}

	// Token: 0x060050DF RID: 20703 RVA: 0x001CA020 File Offset: 0x001C8220
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (this.isLocked && interactor.carriedConsumables.KeyBullets <= 0 && !interactor.carriedConsumables.InfiniteKeys)
		{
			return;
		}
		if (this.IsSealed)
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(this.LockAnimator.Sprite, Color.white, 0.05f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		this.LockAnimator.sprite.UpdateZDepth();
	}

	// Token: 0x060050E0 RID: 20704 RVA: 0x001CA0A4 File Offset: 0x001C82A4
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (this.IsSealed)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(this.LockAnimator.Sprite, false);
	}

	// Token: 0x060050E1 RID: 20705 RVA: 0x001CA0D0 File Offset: 0x001C82D0
	public void FlipLockToOtherSide()
	{
		this.LockAnimator.Sprite.FlipX = true;
		this.LockAnimator.Sprite.transform.position += new Vector3(-0.375f, 0f, 0f);
		this.ChainsAnimator.Sprite.FlipX = true;
		if (this.ChainsAnimator.transform.parent != this.LockAnimator.transform)
		{
			this.ChainsAnimator.Sprite.transform.position += new Vector3(-0.375f, 0f, 0f);
		}
	}

	// Token: 0x060050E2 RID: 20706 RVA: 0x001CA18C File Offset: 0x001C838C
	public void Unlock()
	{
		if (this.isLocked)
		{
			this.isLocked = false;
			if (this.LockAnimator != null)
			{
				if (this.northSouth)
				{
					this.LockAnimator.PlayAndDestroyObject("look_guy_unlock", null);
					this.ChainsAnimator.PlayAndDestroyObject("lock_guy_chain_north_unlock", null);
				}
				else
				{
					this.LockAnimator.PlayAndDestroyObject("lock_guy_side_unlock", null);
					this.ChainsAnimator.PlayAndDestroyObject("lock_guy_chain_side_unlock", null);
				}
			}
			RoomHandler.unassignedInteractableObjects.Remove(this);
		}
	}

	// Token: 0x060050E3 RID: 20707 RVA: 0x001CA21C File Offset: 0x001C841C
	public void BreakLock()
	{
		if (this.isLocked && !this.lockIsBusted)
		{
			this.lockIsBusted = true;
		}
	}

	// Token: 0x060050E4 RID: 20708 RVA: 0x001CA23C File Offset: 0x001C843C
	public void Interact(PlayerController interactor)
	{
		if (this.IsSealed || this.lockIsBusted)
		{
			return;
		}
		if (this.isLocked)
		{
			if (interactor.carriedConsumables.KeyBullets <= 0 && !interactor.carriedConsumables.InfiniteKeys)
			{
				if (this.northSouth && this.LockAnimator != null)
				{
					this.LockAnimator.Play("lock_guy_laugh");
					this.m_lockHasLaughed = true;
					this.m_lockHasSpit = false;
				}
				return;
			}
			if (!interactor.carriedConsumables.InfiniteKeys)
			{
				interactor.carriedConsumables.KeyBullets--;
			}
			this.Unlock();
		}
	}

	// Token: 0x060050E5 RID: 20709 RVA: 0x001CA2F0 File Offset: 0x001C84F0
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060050E6 RID: 20710 RVA: 0x001CA2FC File Offset: 0x001C84FC
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x040048A2 RID: 18594
	[SerializeField]
	protected DungeonDoorController.DungeonDoorMode doorMode;

	// Token: 0x040048A3 RID: 18595
	[SerializeField]
	protected bool doorClosesAfterEveryOpen;

	// Token: 0x040048A4 RID: 18596
	[NonSerialized]
	private bool hasEverBeenOpen;

	// Token: 0x040048A5 RID: 18597
	public DungeonDoorController.DoorModule[] doorModules;

	// Token: 0x040048A6 RID: 18598
	public bool hideSealAnimators = true;

	// Token: 0x040048A7 RID: 18599
	public tk2dSpriteAnimator[] sealAnimators;

	// Token: 0x040048A8 RID: 18600
	public tk2dSpriteAnimator[] sealChainAnimators;

	// Token: 0x040048A9 RID: 18601
	public tk2dSpriteAnimator[] sealVFX;

	// Token: 0x040048AA RID: 18602
	public float unsealDistanceMaximum = -1f;

	// Token: 0x040048AB RID: 18603
	public GameObject unsealedVFXOverride;

	// Token: 0x040048AC RID: 18604
	public string sealAnimationName;

	// Token: 0x040048AD RID: 18605
	public string unsealAnimationName;

	// Token: 0x040048AE RID: 18606
	public string playerNearSealedAnimationName;

	// Token: 0x040048AF RID: 18607
	public bool SupportsSubsidiaryDoors = true;

	// Token: 0x040048B0 RID: 18608
	public bool northSouth = true;

	// Token: 0x040048B1 RID: 18609
	[NonSerialized]
	public RuntimeExitDefinition exitDefinition;

	// Token: 0x040048B2 RID: 18610
	[NonSerialized]
	public RoomHandler upstreamRoom;

	// Token: 0x040048B3 RID: 18611
	[NonSerialized]
	public RoomHandler downstreamRoom;

	// Token: 0x040048B4 RID: 18612
	[NonSerialized]
	public bool OneWayDoor;

	// Token: 0x040048B5 RID: 18613
	[HideInInspector]
	public DungeonDoorSubsidiaryBlocker subsidiaryBlocker;

	// Token: 0x040048B6 RID: 18614
	[HideInInspector]
	public DungeonDoorController subsidiaryDoor;

	// Token: 0x040048B7 RID: 18615
	[HideInInspector]
	public DungeonDoorController parentDoor;

	// Token: 0x040048B8 RID: 18616
	[NonSerialized]
	public Transform messageTransformPoint;

	// Token: 0x040048B9 RID: 18617
	[NonSerialized]
	public string messageToDisplay;

	// Token: 0x040048BA RID: 18618
	public tk2dSpriteAnimator LockAnimator;

	// Token: 0x040048BB RID: 18619
	public tk2dSpriteAnimator ChainsAnimator;

	// Token: 0x040048BC RID: 18620
	private bool m_open;

	// Token: 0x040048BD RID: 18621
	public bool isLocked;

	// Token: 0x040048BE RID: 18622
	public bool lockIsBusted;

	// Token: 0x040048BF RID: 18623
	[SerializeField]
	private bool isSealed;

	// Token: 0x040048C0 RID: 18624
	private bool m_openIsFlipped;

	// Token: 0x040048C1 RID: 18625
	public bool usesUnsealScreenShake;

	// Token: 0x040048C2 RID: 18626
	public ScreenShakeSettings unsealScreenShake;

	// Token: 0x040048C3 RID: 18627
	private bool m_isDestroyed;

	// Token: 0x040048C4 RID: 18628
	private bool m_wasOpenWhenSealed;

	// Token: 0x040048C6 RID: 18630
	private bool m_lockHasApproached;

	// Token: 0x040048C7 RID: 18631
	private bool m_lockHasLaughed;

	// Token: 0x040048C8 RID: 18632
	private bool m_lockHasSpit;

	// Token: 0x040048C9 RID: 18633
	private bool m_finalBossDoorHasOpened;

	// Token: 0x040048CA RID: 18634
	private bool m_isCoopArrowing;

	// Token: 0x040048CB RID: 18635
	private bool m_hasGC;

	// Token: 0x02000ED5 RID: 3797
	public enum DungeonDoorMode
	{
		// Token: 0x040048CD RID: 18637
		COMPLEX,
		// Token: 0x040048CE RID: 18638
		BOSS_DOOR_ONLY_UNSEALS,
		// Token: 0x040048CF RID: 18639
		SINGLE_DOOR,
		// Token: 0x040048D0 RID: 18640
		ONE_WAY_DOOR_ONLY_UNSEALS,
		// Token: 0x040048D1 RID: 18641
		FINAL_BOSS_DOOR
	}

	// Token: 0x02000ED6 RID: 3798
	[Serializable]
	public class DoorModule
	{
		// Token: 0x040048D2 RID: 18642
		public tk2dSpriteAnimator animator;

		// Token: 0x040048D3 RID: 18643
		public float openDepth;

		// Token: 0x040048D4 RID: 18644
		public float closedDepth;

		// Token: 0x040048D5 RID: 18645
		public bool openPerpendicular = true;

		// Token: 0x040048D6 RID: 18646
		public bool horizontalFlips = true;

		// Token: 0x040048D7 RID: 18647
		public string openAnimationName;

		// Token: 0x040048D8 RID: 18648
		public string closeAnimationName;

		// Token: 0x040048D9 RID: 18649
		public List<tk2dSpriteAnimator> AOAnimatorsToDisable;

		// Token: 0x040048DA RID: 18650
		[HideInInspector]
		public tk2dSprite sprite;

		// Token: 0x040048DB RID: 18651
		[HideInInspector]
		public SpeculativeRigidbody rigidbody;

		// Token: 0x040048DC RID: 18652
		[HideInInspector]
		[NonSerialized]
		public bool isLerping;
	}
}
