using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x0200111F RID: 4383
public class ChallengeShrineController : DungeonPlaceableBehaviour, IPlayerInteractable, IPlaceConfigurable
{
	// Token: 0x060060B5 RID: 24757 RVA: 0x00253780 File Offset: 0x00251980
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_parentRoom = room;
		this.m_parentRoom.PreventStandardRoomReward = true;
		this.RegisterMinimapIcon();
	}

	// Token: 0x060060B6 RID: 24758 RVA: 0x0025379C File Offset: 0x0025199C
	private void Update()
	{
		if (this.m_parentRoom.IsSealed && GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.CurrentRoom != null)
		{
			if (GameManager.Instance.PrimaryPlayer.CurrentRoom != this.m_parentRoom)
			{
				this.m_parentRoom.npcSealState = RoomHandler.NPCSealState.SealNone;
				this.m_parentRoom.UnsealRoom();
			}
			else if (!this.m_parentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
			{
				this.m_noEnemySealTime += BraveTime.DeltaTime;
				if (this.m_noEnemySealTime > 3f)
				{
					this.m_parentRoom.TriggerNextReinforcementLayer();
				}
				if (this.m_noEnemySealTime > 5f)
				{
					this.m_parentRoom.npcSealState = RoomHandler.NPCSealState.SealNone;
					this.m_parentRoom.UnsealRoom();
				}
			}
			else
			{
				this.m_noEnemySealTime = 0f;
			}
		}
	}

	// Token: 0x060060B7 RID: 24759 RVA: 0x0025388C File Offset: 0x00251A8C
	public void RegisterMinimapIcon()
	{
		this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_parentRoom, (GameObject)BraveResources.Load("Global Prefabs/Minimap_Shrine_Icon", ".prefab"), false);
	}

	// Token: 0x060060B8 RID: 24760 RVA: 0x002538BC File Offset: 0x00251ABC
	public void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_parentRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x060060B9 RID: 24761 RVA: 0x002538EC File Offset: 0x00251AEC
	private void DoShrineEffect(PlayerController player)
	{
		this.m_parentRoom.TriggerReinforcementLayersOnEvent(RoomEventTriggerCondition.SHRINE_WAVE_A, false);
		this.m_parentRoom.npcSealState = RoomHandler.NPCSealState.SealAll;
		this.m_parentRoom.SealRoom();
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.GetOtherPlayer(player).ReuniteWithOtherPlayer(player, false);
		}
		RoomHandler parentRoom = this.m_parentRoom;
		parentRoom.OnEnemiesCleared = (Action)Delegate.Combine(parentRoom.OnEnemiesCleared, new Action(this.HandleEnemiesClearedA));
		if (this.onPlayerVFX != null)
		{
			player.PlayEffectOnActor(this.onPlayerVFX, this.playerVFXOffset, true, false, false);
		}
		this.GetRidOfMinimapIcon();
	}

	// Token: 0x060060BA RID: 24762 RVA: 0x00253998 File Offset: 0x00251B98
	private void HandleEnemiesClearedA()
	{
		RoomHandler parentRoom = this.m_parentRoom;
		parentRoom.OnEnemiesCleared = (Action)Delegate.Remove(parentRoom.OnEnemiesCleared, new Action(this.HandleEnemiesClearedA));
		RoomHandler parentRoom2 = this.m_parentRoom;
		parentRoom2.OnEnemiesCleared = (Action)Delegate.Combine(parentRoom2.OnEnemiesCleared, new Action(this.HandleEnemiesClearedB));
		if (!this.m_parentRoom.TriggerReinforcementLayersOnEvent(RoomEventTriggerCondition.SHRINE_WAVE_B, false))
		{
			this.HandleFinalEnemiesCleared();
		}
	}

	// Token: 0x060060BB RID: 24763 RVA: 0x00253A0C File Offset: 0x00251C0C
	private void HandleEnemiesClearedB()
	{
		RoomHandler parentRoom = this.m_parentRoom;
		parentRoom.OnEnemiesCleared = (Action)Delegate.Remove(parentRoom.OnEnemiesCleared, new Action(this.HandleEnemiesClearedB));
		if (!this.m_parentRoom.TriggerReinforcementLayersOnEvent(RoomEventTriggerCondition.SHRINE_WAVE_C, false))
		{
			this.HandleFinalEnemiesCleared();
		}
		else
		{
			RoomHandler parentRoom2 = this.m_parentRoom;
			parentRoom2.OnEnemiesCleared = (Action)Delegate.Combine(parentRoom2.OnEnemiesCleared, new Action(this.HandleFinalEnemiesCleared));
		}
	}

	// Token: 0x060060BC RID: 24764 RVA: 0x00253A88 File Offset: 0x00251C88
	private void HandleFinalEnemiesCleared()
	{
		this.m_parentRoom.npcSealState = RoomHandler.NPCSealState.SealNone;
		RoomHandler parentRoom = this.m_parentRoom;
		parentRoom.OnEnemiesCleared = (Action)Delegate.Remove(parentRoom.OnEnemiesCleared, new Action(this.HandleFinalEnemiesCleared));
		Chest chest = GameManager.Instance.RewardManager.SpawnRewardChestAt(this.m_parentRoom.GetBestRewardLocation(new IntVector2(3, 2), RoomHandler.RewardLocationStyle.CameraCenter, true), -1f, PickupObject.ItemQuality.EXCLUDED);
		if (chest)
		{
			chest.ForceUnlock();
			chest.RegisterChestOnMinimap(this.m_parentRoom);
		}
	}

	// Token: 0x060060BD RID: 24765 RVA: 0x00253B10 File Offset: 0x00251D10
	public float GetDistanceToPoint(Vector2 point)
	{
		if (base.sprite == null)
		{
			return 100f;
		}
		Vector3 vector = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
		return Vector2.Distance(point, vector) / 1.5f;
	}

	// Token: 0x060060BE RID: 24766 RVA: 0x00253B68 File Offset: 0x00251D68
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x060060BF RID: 24767 RVA: 0x00253B70 File Offset: 0x00251D70
	public void OnEnteredRange(PlayerController interactor)
	{
		SpriteOutlineManager.AddOutlineToSprite(this.AlternativeOutlineTarget ?? base.sprite, Color.white);
	}

	// Token: 0x060060C0 RID: 24768 RVA: 0x00253B90 File Offset: 0x00251D90
	public void OnExitRange(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(this.AlternativeOutlineTarget ?? base.sprite, false);
	}

	// Token: 0x060060C1 RID: 24769 RVA: 0x00253BAC File Offset: 0x00251DAC
	private IEnumerator HandleShrineConversation(PlayerController interactor)
	{
		TextBoxManager.ShowStoneTablet(this.talkPoint.position, this.talkPoint, -1f, StringTableManager.GetString(this.displayTextKey), true, false);
		int selectedResponse = -1;
		interactor.SetInputOverride("shrineConversation");
		yield return null;
		GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, StringTableManager.GetString(this.acceptOptionKey), StringTableManager.GetString(this.declineOptionKey));
		while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
		{
			yield return null;
		}
		interactor.ClearInputOverride("shrineConversation");
		TextBoxManager.ClearTextBox(this.talkPoint);
		if (selectedResponse == 0)
		{
			this.DoShrineEffect(interactor);
		}
		else
		{
			this.m_useCount--;
			this.m_parentRoom.RegisterInteractable(this);
		}
		yield break;
	}

	// Token: 0x060060C2 RID: 24770 RVA: 0x00253BD0 File Offset: 0x00251DD0
	public void Interact(PlayerController interactor)
	{
		if (this.m_useCount > 0)
		{
			return;
		}
		this.m_useCount++;
		this.m_parentRoom.DeregisterInteractable(this);
		base.StartCoroutine(this.HandleShrineConversation(interactor));
	}

	// Token: 0x060060C3 RID: 24771 RVA: 0x00253C08 File Offset: 0x00251E08
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060060C4 RID: 24772 RVA: 0x00253C14 File Offset: 0x00251E14
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005B5B RID: 23387
	public string displayTextKey;

	// Token: 0x04005B5C RID: 23388
	public string acceptOptionKey;

	// Token: 0x04005B5D RID: 23389
	public string declineOptionKey;

	// Token: 0x04005B5E RID: 23390
	public Transform talkPoint;

	// Token: 0x04005B5F RID: 23391
	public GameObject onPlayerVFX;

	// Token: 0x04005B60 RID: 23392
	public Vector3 playerVFXOffset = Vector3.zero;

	// Token: 0x04005B61 RID: 23393
	public bool usesCustomChestTable;

	// Token: 0x04005B62 RID: 23394
	public WeightedGameObjectCollection CustomChestTable;

	// Token: 0x04005B63 RID: 23395
	public tk2dBaseSprite AlternativeOutlineTarget;

	// Token: 0x04005B64 RID: 23396
	private int m_useCount;

	// Token: 0x04005B65 RID: 23397
	private RoomHandler m_parentRoom;

	// Token: 0x04005B66 RID: 23398
	private GameObject m_instanceMinimapIcon;

	// Token: 0x04005B67 RID: 23399
	private float m_noEnemySealTime;
}
