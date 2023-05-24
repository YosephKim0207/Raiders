using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

// Token: 0x02001207 RID: 4615
public class ShortcutElevatorController : BraveBehaviour
{
	// Token: 0x06006733 RID: 26419 RVA: 0x00286484 File Offset: 0x00284684
	private void Start()
	{
		this.DetermineAvailableShortcuts();
		if (this.availableShortcuts.Count > 0)
		{
			PressurePlate rotateLeftPlate = this.RotateLeftPlate;
			rotateLeftPlate.OnPressurePlateDepressed = (Action<PressurePlate>)Delegate.Combine(rotateLeftPlate.OnPressurePlateDepressed, new Action<PressurePlate>(this.RotateLeft));
			PressurePlate rotateRightPlate = this.RotateRightPlate;
			rotateRightPlate.OnPressurePlateDepressed = (Action<PressurePlate>)Delegate.Combine(rotateRightPlate.OnPressurePlateDepressed, new Action<PressurePlate>(this.RotateRight));
			base.StartCoroutine(this.RotateRight("bullet_elevator_turn", "bullet_elevator_bullet_turn", 1, this.GetCachedAvailableShortcut()));
			SpeculativeRigidbody component = this.ElevatorFloor.GetComponent<SpeculativeRigidbody>();
			component.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(component.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnElevatorTriggerEnter));
			if (this.BossRushTriggerZone != null)
			{
				SpeculativeRigidbody bossRushTriggerZone = this.BossRushTriggerZone;
				bossRushTriggerZone.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(bossRushTriggerZone.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleRushTriggerEntered));
			}
		}
	}

	// Token: 0x06006734 RID: 26420 RVA: 0x00286578 File Offset: 0x00284778
	private void HandleRushTriggerEntered(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (this.availableShortcuts[this.m_selectedShortcutIndex].IsBossRush && !this.m_bossRushValid)
		{
			for (int i = 0; i < StaticReferenceManager.AllNpcs.Count; i++)
			{
				TalkDoerLite talkDoerLite = StaticReferenceManager.AllNpcs[i];
				if (talkDoerLite)
				{
					if (!Foyer.DoMainMenu && !GameManager.Instance.IsSelectingCharacter && GameManager.Instance.PrimaryPlayer.CurrentRoom == talkDoerLite.ParentRoom)
					{
						talkDoerLite.SendPlaymakerEvent("announceBossRush");
					}
				}
			}
		}
	}

	// Token: 0x06006735 RID: 26421 RVA: 0x00286624 File Offset: 0x00284824
	public void SetBossRushPaymentValid()
	{
		this.m_bossRushValid = true;
	}

	// Token: 0x06006736 RID: 26422 RVA: 0x00286630 File Offset: 0x00284830
	private int GetCachedAvailableShortcut()
	{
		string lastUsedShortcutTarget = GameManager.Options.lastUsedShortcutTarget;
		int num = -1;
		for (int i = 0; i < this.availableShortcuts.Count; i++)
		{
			if (this.availableShortcuts[i].targetLevelName == lastUsedShortcutTarget)
			{
				num = i;
				break;
			}
		}
		return num;
	}

	// Token: 0x06006737 RID: 26423 RVA: 0x00286690 File Offset: 0x00284890
	private bool CheckPlayerPositions()
	{
		if (!GameManager.Instance.IsSelectingCharacter)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i].CurrentRoom == base.transform.position.GetAbsoluteRoom() && GameManager.Instance.AllPlayers[i].transform.position.y > this.RotatorBase.Sprite.WorldBottomCenter.y)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06006738 RID: 26424 RVA: 0x00286730 File Offset: 0x00284930
	private void RotateRight(PressurePlate obj)
	{
		bool flag = this.CheckPlayerPositions();
		if (!this.m_isRotating && flag)
		{
			base.StartCoroutine(this.RotateRight("bullet_elevator_turn", "bullet_elevator_bullet_turn", 1, -1));
		}
		else if (flag)
		{
			this.m_queuedRotationLeft = false;
			this.m_queuedRotationRight = true;
		}
	}

	// Token: 0x06006739 RID: 26425 RVA: 0x00286788 File Offset: 0x00284988
	private void RotateLeft(PressurePlate obj)
	{
		bool flag = this.CheckPlayerPositions();
		if (!this.m_isRotating && flag)
		{
			base.StartCoroutine(this.RotateRight("bullet_elevator_turn_reverse", "bullet_elevator_bullet_turn_reverse", -1, -1));
		}
		else if (flag)
		{
			this.m_queuedRotationLeft = true;
			this.m_queuedRotationRight = false;
		}
	}

	// Token: 0x0600673A RID: 26426 RVA: 0x002867E0 File Offset: 0x002849E0
	private void OnElevatorTriggerEnter(SpeculativeRigidbody otherSpecRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (this.availableShortcuts[this.m_selectedShortcutIndex].IsBossRush && !this.m_bossRushValid)
		{
			return;
		}
		if (this.availableShortcuts[this.m_selectedShortcutIndex].IsSuperBossRush && !this.m_bossRushValid)
		{
			return;
		}
		if (sourceSpecRigidbody.renderer.enabled && !this.m_isDeparting && otherSpecRigidbody.GetComponent<PlayerController>() != null)
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
					base.StartCoroutine(this.TriggerShortcut());
				}
			}
			else
			{
				base.StartCoroutine(this.TriggerShortcut());
			}
		}
	}

	// Token: 0x0600673B RID: 26427 RVA: 0x00286910 File Offset: 0x00284B10
	private void DetermineAvailableShortcuts()
	{
		this.availableShortcuts.Clear();
		for (int i = 0; i < this.definedShortcuts.Length; i++)
		{
			if (this.definedShortcuts[i].requiredFlag == GungeonFlags.NONE || GameStatsManager.Instance.GetFlag(this.definedShortcuts[i].requiredFlag))
			{
				this.availableShortcuts.Add(this.definedShortcuts[i]);
			}
		}
		if (this.availableShortcuts.Count == 0)
		{
			this.m_selectedShortcutIndex = -1;
		}
		else
		{
			this.m_selectedShortcutIndex = 0;
		}
	}

	// Token: 0x0600673C RID: 26428 RVA: 0x002869B8 File Offset: 0x00284BB8
	protected void SetSherpaText(string key)
	{
		for (int i = 0; i < StaticReferenceManager.AllNpcs.Count; i++)
		{
			TalkDoerLite talkDoerLite = StaticReferenceManager.AllNpcs[i];
			if (talkDoerLite)
			{
				if (!Foyer.DoMainMenu && !GameManager.Instance.IsSelectingCharacter && GameManager.Instance.PrimaryPlayer.CurrentRoom == talkDoerLite.ParentRoom)
				{
					for (int j = 0; j < talkDoerLite.playmakerFsms.Length; j++)
					{
						FsmString fsmString = talkDoerLite.playmakerFsms[j].FsmVariables.FindFsmString("CurrentShortcutText");
						if (fsmString != null)
						{
							fsmString.Value = key;
						}
					}
					talkDoerLite.SendPlaymakerEvent("rotatoPotato");
				}
			}
		}
	}

	// Token: 0x0600673D RID: 26429 RVA: 0x00286A78 File Offset: 0x00284C78
	public void UpdateFloorSprite(ShortcutDefinition currentDef)
	{
		if (this.elevatorFloorSprite && !string.IsNullOrEmpty(currentDef.elevatorFloorSpriteName))
		{
			this.elevatorFloorSprite.SetSprite(currentDef.elevatorFloorSpriteName);
		}
		else
		{
			this.elevatorFloorSprite.SetSprite("elevator_bottom_001");
		}
	}

	// Token: 0x0600673E RID: 26430 RVA: 0x00286AD0 File Offset: 0x00284CD0
	public IEnumerator RotateRight(string rotateAnim, string rotateAnimShells, int change, int specificAvailableToUse = -1)
	{
		if (this.availableShortcuts.Count == 0)
		{
			yield break;
		}
		this.m_isRotating = true;
		this.PlayerBlocker.enabled = true;
		this.Elevator.Play("bullet_elevator_no_thanks");
		float elapsed = 0f;
		while (this.Elevator.IsPlaying(this.Elevator.CurrentClip))
		{
			elapsed += BraveTime.DeltaTime;
			if (elapsed > 0.5f)
			{
				this.ElevatorFloor.enabled = false;
			}
			yield return null;
		}
		this.Elevator.sprite.renderer.enabled = false;
		this.m_selectedShortcutIndex = (this.m_selectedShortcutIndex + this.availableShortcuts.Count + change) % this.availableShortcuts.Count;
		if (specificAvailableToUse != -1 && specificAvailableToUse >= 0 && specificAvailableToUse < this.availableShortcuts.Count)
		{
			this.m_selectedShortcutIndex = specificAvailableToUse;
		}
		ShortcutDefinition currentDef = this.availableShortcuts[this.m_selectedShortcutIndex];
		GameManager.Options.lastUsedShortcutTarget = currentDef.targetLevelName;
		this.RotatorBase.Play(rotateAnim);
		this.RotatorShells.Play(rotateAnimShells);
		while (this.RotatorBase.IsPlaying(this.RotatorBase.CurrentClip))
		{
			yield return null;
		}
		this.UpdateFloorSprite(currentDef);
		this.SetSherpaText(currentDef.sherpaTextKey);
		this.Elevator.sprite.renderer.enabled = true;
		this.Elevator.Play("bullet_elevator_arrive");
		elapsed = 0f;
		while (this.Elevator.IsPlaying(this.Elevator.CurrentClip))
		{
			elapsed += BraveTime.DeltaTime;
			if (elapsed > 0.5f)
			{
				this.ElevatorFloor.enabled = true;
			}
			yield return null;
		}
		this.PlayerBlocker.enabled = false;
		this.m_isRotating = false;
		if (this.m_queuedRotationLeft)
		{
			base.StartCoroutine(this.RotateRight("bullet_elevator_turn_reverse", "bullet_elevator_bullet_turn_reverse", -1, -1));
			this.m_queuedRotationLeft = false;
		}
		else if (this.m_queuedRotationRight)
		{
			base.StartCoroutine(this.RotateRight("bullet_elevator_turn", "bullet_elevator_bullet_turn", 1, -1));
			this.m_queuedRotationRight = false;
		}
		yield break;
	}

	// Token: 0x0600673F RID: 26431 RVA: 0x00286B08 File Offset: 0x00284D08
	public IEnumerator TriggerShortcut()
	{
		this.m_isDeparting = true;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (!GameManager.Instance.AllPlayers[i].healthHaver.IsDead)
			{
				GameManager.Instance.AllPlayers[i].CurrentInputState = PlayerInputState.NoInput;
			}
		}
		for (int j = 0; j < this.availableShortcuts.Count; j++)
		{
			ShortcutDefinition shortcutDefinition = this.availableShortcuts[j];
			GameStatsManager.Instance.SetFlag(shortcutDefinition.requiredFlag, true);
		}
		this.RotatorShells.sprite.SetSprite("elevator_chamber_bullet_empty_idle_001");
		this.Elevator.Play("bullet_elevator_depart");
		float elapsed = 0f;
		bool postprocessed = false;
		while (this.Elevator.IsPlaying(this.Elevator.CurrentClip))
		{
			elapsed += BraveTime.DeltaTime;
			if (elapsed > 0.5f && !postprocessed)
			{
				postprocessed = true;
				this.ElevatorFloor.enabled = false;
				Foyer.Instance.OnDepartedFoyer();
				for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
				{
					GameManager.Instance.AllPlayers[k].PrepareForSceneTransition();
				}
			}
			yield return null;
		}
		if (this.availableShortcuts[this.m_selectedShortcutIndex].IsBossRush)
		{
			GameManager.Instance.CurrentGameMode = GameManager.GameMode.BOSSRUSH;
			Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
			GameManager.Instance.SetNextLevelIndex(1);
			GameManager.Instance.DelayedLoadBossrushFloor(0.5f);
		}
		else if (this.availableShortcuts[this.m_selectedShortcutIndex].IsSuperBossRush)
		{
			GameManager.Instance.CurrentGameMode = GameManager.GameMode.SUPERBOSSRUSH;
			Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
			GameManager.Instance.SetNextLevelIndex(1);
			GameManager.Instance.DelayedLoadBossrushFloor(0.5f);
		}
		else
		{
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.NUMBER_ATTEMPTS, 1f);
			GameManager.Instance.CurrentGameMode = GameManager.GameMode.SHORTCUT;
			Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
			GungeonFlags requiredFlag = this.availableShortcuts[this.m_selectedShortcutIndex].requiredFlag;
			if (requiredFlag != GungeonFlags.SHERPA_UNLOCK1_COMPLETE)
			{
				if (requiredFlag != GungeonFlags.SHERPA_UNLOCK2_COMPLETE)
				{
					if (requiredFlag != GungeonFlags.SHERPA_UNLOCK3_COMPLETE)
					{
						if (requiredFlag != GungeonFlags.SHERPA_UNLOCK4_COMPLETE)
						{
							GameManager.Instance.LastShortcutFloorLoaded = 1;
						}
						else
						{
							GameManager.Instance.LastShortcutFloorLoaded = 4;
						}
					}
					else
					{
						GameManager.Instance.LastShortcutFloorLoaded = 3;
					}
				}
				else
				{
					GameManager.Instance.LastShortcutFloorLoaded = 2;
				}
			}
			else
			{
				GameManager.Instance.LastShortcutFloorLoaded = 1;
			}
			GameManager.Instance.IsLoadingFirstShortcutFloor = true;
			GameManager.Instance.DelayedLoadCustomLevel(0.5f, this.availableShortcuts[this.m_selectedShortcutIndex].targetLevelName);
		}
		yield break;
	}

	// Token: 0x06006740 RID: 26432 RVA: 0x00286B24 File Offset: 0x00284D24
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400630A RID: 25354
	public ShortcutDefinition[] definedShortcuts;

	// Token: 0x0400630B RID: 25355
	public tk2dSprite elevatorFloorSprite;

	// Token: 0x0400630C RID: 25356
	public PressurePlate RotateLeftPlate;

	// Token: 0x0400630D RID: 25357
	public PressurePlate RotateRightPlate;

	// Token: 0x0400630E RID: 25358
	public SpeculativeRigidbody PlayerBlocker;

	// Token: 0x0400630F RID: 25359
	public SpeculativeRigidbody BossRushTriggerZone;

	// Token: 0x04006310 RID: 25360
	public tk2dSpriteAnimator RotatorBase;

	// Token: 0x04006311 RID: 25361
	public tk2dSpriteAnimator RotatorShells;

	// Token: 0x04006312 RID: 25362
	public tk2dSpriteAnimator Elevator;

	// Token: 0x04006313 RID: 25363
	public MeshRenderer ElevatorFloor;

	// Token: 0x04006314 RID: 25364
	[NonSerialized]
	private List<ShortcutDefinition> availableShortcuts = new List<ShortcutDefinition>();

	// Token: 0x04006315 RID: 25365
	[NonSerialized]
	private int m_selectedShortcutIndex;

	// Token: 0x04006316 RID: 25366
	private bool m_isDeparting;

	// Token: 0x04006317 RID: 25367
	private bool m_isRotating;

	// Token: 0x04006318 RID: 25368
	private bool m_bossRushValid;

	// Token: 0x04006319 RID: 25369
	private bool m_queuedRotationRight;

	// Token: 0x0400631A RID: 25370
	private bool m_queuedRotationLeft;
}
