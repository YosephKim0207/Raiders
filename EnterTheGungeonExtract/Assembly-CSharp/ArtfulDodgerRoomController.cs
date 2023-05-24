using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using HutongGames.PlayMaker;
using UnityEngine;

// Token: 0x020010F6 RID: 4342
public class ArtfulDodgerRoomController : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x17000E11 RID: 3601
	// (get) Token: 0x06005FAE RID: 24494 RVA: 0x0024D460 File Offset: 0x0024B660
	public bool Completed
	{
		get
		{
			return this.m_rewardHandled;
		}
	}

	// Token: 0x06005FAF RID: 24495 RVA: 0x0024D468 File Offset: 0x0024B668
	public void RegisterTarget(ArtfulDodgerTargetController target)
	{
		this.m_targets.Add(target);
	}

	// Token: 0x06005FB0 RID: 24496 RVA: 0x0024D478 File Offset: 0x0024B678
	public void RegisterCameraZone(ArtfulDodgerCameraManipulator zone)
	{
		this.m_cameraZones.Add(zone);
	}

	// Token: 0x06005FB1 RID: 24497 RVA: 0x0024D488 File Offset: 0x0024B688
	public void Activate(Fsm sourceFsm)
	{
		this.m_hasActivated = true;
		this.m_fsm = sourceFsm;
		for (int i = 0; i < this.m_cameraZones.Count; i++)
		{
			this.m_cameraZones[i].Active = true;
		}
		for (int j = 0; j < this.m_targets.Count; j++)
		{
			this.m_targets[j].Activate();
		}
		GameManager.Instance.DungeonMusicController.StartArcadeGame();
	}

	// Token: 0x06005FB2 RID: 24498 RVA: 0x0024D510 File Offset: 0x0024B710
	public void DoHandleReward()
	{
		GameManager.Instance.DungeonMusicController.SwitchToArcadeMusic();
		base.StartCoroutine(this.HandleReward());
	}

	// Token: 0x06005FB3 RID: 24499 RVA: 0x0024D530 File Offset: 0x0024B730
	private void DoConfetti(Vector2 targetCenter)
	{
		string[] array = new string[] { "Global VFX/Confetti_Blue_001", "Global VFX/Confetti_Yellow_001", "Global VFX/Confetti_Green_001" };
		for (int i = 0; i < 8; i++)
		{
			GameObject gameObject = (GameObject)BraveResources.Load(array[UnityEngine.Random.Range(0, 3)], ".prefab");
			WaftingDebrisObject component = UnityEngine.Object.Instantiate<GameObject>(gameObject).GetComponent<WaftingDebrisObject>();
			component.sprite.PlaceAtPositionByAnchor(targetCenter.ToVector3ZUp(0f) + new Vector3(0.5f, 0.5f, 0f), tk2dBaseSprite.Anchor.MiddleCenter);
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			insideUnitCircle.y = -Mathf.Abs(insideUnitCircle.y);
			component.Trigger(insideUnitCircle.ToVector3ZUp(1.5f) * UnityEngine.Random.Range(0.5f, 2f), 0.5f, 0f);
		}
	}

	// Token: 0x06005FB4 RID: 24500 RVA: 0x0024D614 File Offset: 0x0024B814
	public IEnumerator HandleReward()
	{
		if (this.m_rewardHandled)
		{
			yield break;
		}
		if (GameManager.Instance.BestActivePlayer.CurrentRoom != base.GetAbsoluteParentRoom())
		{
			this.m_fsm.Variables.GetFsmBool("SilentEnd").Value = true;
		}
		this.m_rewardHandled = true;
		int numBroken = 0;
		for (int i = 0; i < this.m_targets.Count; i++)
		{
			if (this.m_targets[i].IsBroken)
			{
				numBroken++;
			}
		}
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.WINCHESTER_GAMES_PLAYED, 1f);
		if (numBroken == this.m_targets.Count)
		{
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.WINCHESTER_GAMES_ACED, 1f);
			GameStatsManager.Instance.SetFlag(GungeonFlags.WINCHESTER_ACED_ONCE, true);
		}
		if (numBroken > 0)
		{
			yield return new WaitForSeconds(0.25f);
			this.m_fsm.Variables.FindFsmString("VictoryTextKey").Value = "#DODGER_VICTORY_01";
			this.m_fsm.Variables.FindFsmString("VictoryAnim").Value = "clap";
			if (numBroken == this.m_targets.Count)
			{
				this.m_fsm.Variables.FindFsmString("VictoryTextKey").Value = "#DODGER_GREAT_VICTORY_01";
				this.m_fsm.Variables.FindFsmString("VictoryAnim").Value = "bow";
			}
			else if (numBroken == this.m_targets.Count - 1)
			{
				this.m_fsm.Variables.FindFsmString("VictoryTextKey").Value = "#DODGER_GOOD_VICTORY_01";
				this.m_fsm.Variables.FindFsmString("VictoryAnim").Value = "clap";
			}
			if (this.gamePlayingPlayer && this.m_fsm != null && this.m_fsm.Owner)
			{
				TalkDoerLite component = this.m_fsm.Owner.GetComponent<TalkDoerLite>();
				component.TalkingPlayer = this.gamePlayingPlayer;
			}
			GameManager.BroadcastRoomFsmEvent("ArtfulDodgerSuccess", base.GetAbsoluteParentRoom());
			while (!this.m_fsm.Variables.GetFsmBool("ShouldSpawnChest").Value)
			{
				yield return null;
			}
			IntVector2 pos = base.GetAbsoluteParentRoom().GetBestRewardLocation(new IntVector2(2, 1), RoomHandler.RewardLocationStyle.Original, true);
			GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
			Chest chestPrefab = null;
			if (tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON || tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON)
			{
				if (numBroken == this.m_targets.Count)
				{
					chestPrefab = GameManager.Instance.RewardManager.A_Chest;
				}
				else if (numBroken == this.m_targets.Count - 1)
				{
					chestPrefab = GameManager.Instance.RewardManager.B_Chest;
				}
				else if (numBroken == this.m_targets.Count - 2)
				{
					chestPrefab = GameManager.Instance.RewardManager.C_Chest;
				}
				else
				{
					chestPrefab = GameManager.Instance.RewardManager.D_Chest;
				}
			}
			else if (tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON || tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON || tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
			{
				if (numBroken == this.m_targets.Count)
				{
					chestPrefab = GameManager.Instance.RewardManager.S_Chest;
				}
				else if (numBroken == this.m_targets.Count - 1)
				{
					chestPrefab = GameManager.Instance.RewardManager.A_Chest;
				}
				else if (numBroken == this.m_targets.Count - 2)
				{
					chestPrefab = GameManager.Instance.RewardManager.B_Chest;
				}
				else
				{
					chestPrefab = GameManager.Instance.RewardManager.C_Chest;
				}
			}
			if (chestPrefab == null)
			{
				if (numBroken == this.m_targets.Count)
				{
					chestPrefab = GameManager.Instance.RewardManager.A_Chest;
				}
				else if (numBroken == this.m_targets.Count - 1)
				{
					chestPrefab = GameManager.Instance.RewardManager.B_Chest;
				}
				else if (numBroken == this.m_targets.Count - 2)
				{
					chestPrefab = GameManager.Instance.RewardManager.C_Chest;
				}
				else
				{
					chestPrefab = GameManager.Instance.RewardManager.D_Chest;
				}
			}
			Chest c = Chest.Spawn(chestPrefab, pos, base.GetAbsoluteParentRoom(), true);
			AkSoundEngine.PostEvent("Play_OBJ_prize_won_01", c.gameObject);
			this.DoConfetti(c.sprite.WorldCenter);
			c.ForceUnlock();
			if (c != null)
			{
				c.RegisterChestOnMinimap(base.GetAbsoluteParentRoom());
			}
		}
		else
		{
			this.m_fsm.Variables.FindFsmString("VictoryTextKey").Value = "#DODGER_FAILURE_01";
			this.m_fsm.Variables.FindFsmString("VictoryTextKey2").Value = "#DODGER_FAILURE_02";
			this.m_fsm.Variables.FindFsmString("VictoryAnim").Value = "laugh";
			yield return new WaitForSeconds(0.25f);
			if (this.gamePlayingPlayer && this.m_fsm != null && this.m_fsm.Owner)
			{
				TalkDoerLite component2 = this.m_fsm.Owner.GetComponent<TalkDoerLite>();
				component2.TalkingPlayer = this.gamePlayingPlayer;
			}
			GameManager.BroadcastRoomFsmEvent("ArtfulDodgerFailure", base.GetAbsoluteParentRoom());
		}
		for (int j = 0; j < this.m_targets.Count; j++)
		{
			this.m_targets[j].DisappearSadly();
		}
		for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
		{
			if (GameManager.Instance.AllPlayers[k] && GameManager.Instance.AllPlayers[k].healthHaver.IsAlive)
			{
				for (int l = 0; l < GameManager.Instance.AllPlayers[k].inventory.AllGuns.Count; l++)
				{
					if (GameManager.Instance.AllPlayers[k].inventory.AllGuns[l].name.StartsWith("ArtfulDodger"))
					{
						GameManager.Instance.AllPlayers[k].inventory.DestroyGun(GameManager.Instance.AllPlayers[k].inventory.AllGuns[l]);
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x06005FB5 RID: 24501 RVA: 0x0024D630 File Offset: 0x0024B830
	public void LateUpdate()
	{
		if (this.m_hasActivated && !this.m_rewardHandled)
		{
			bool flag = true;
			for (int i = 0; i < this.m_targets.Count; i++)
			{
				if (!this.m_targets[i].IsBroken)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				base.StartCoroutine(this.HandleReward());
			}
		}
	}

	// Token: 0x06005FB6 RID: 24502 RVA: 0x0024D6A4 File Offset: 0x0024B8A4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005FB7 RID: 24503 RVA: 0x0024D6AC File Offset: 0x0024B8AC
	public void ConfigureOnPlacement(RoomHandler room)
	{
		if (room.RoomVisualSubtype >= 0 && room.RoomVisualSubtype < GameManager.Instance.BestGenerationDungeonPrefab.roomMaterialDefinitions.Length)
		{
			DungeonMaterial dungeonMaterial = GameManager.Instance.BestGenerationDungeonPrefab.roomMaterialDefinitions[room.RoomVisualSubtype];
			if (!dungeonMaterial.supportsPits)
			{
				room.RoomVisualSubtype = 0;
			}
		}
		room.IsWinchesterArcadeRoom = true;
		room.Entered += this.HandleArcadeMusicEvents;
		if (room.connectedRooms.Count == 1)
		{
			room.ShouldAttemptProceduralLock = true;
			room.AttemptProceduralLockChance = Mathf.Max(room.AttemptProceduralLockChance, UnityEngine.Random.Range(0.3f, 0.5f));
		}
	}

	// Token: 0x06005FB8 RID: 24504 RVA: 0x0024D75C File Offset: 0x0024B95C
	private void HandleArcadeMusicEvents(PlayerController p)
	{
		GameManager.Instance.DungeonMusicController.SwitchToArcadeMusic();
	}

	// Token: 0x04005A32 RID: 23090
	[DwarfConfigurable]
	public float NumberShots = 3f;

	// Token: 0x04005A33 RID: 23091
	[DwarfConfigurable]
	public float NumberBounces = 1f;

	// Token: 0x04005A34 RID: 23092
	private Fsm m_fsm;

	// Token: 0x04005A35 RID: 23093
	private bool m_hasActivated;

	// Token: 0x04005A36 RID: 23094
	private bool m_rewardHandled;

	// Token: 0x04005A37 RID: 23095
	private List<ArtfulDodgerTargetController> m_targets = new List<ArtfulDodgerTargetController>();

	// Token: 0x04005A38 RID: 23096
	private List<ArtfulDodgerCameraManipulator> m_cameraZones = new List<ArtfulDodgerCameraManipulator>();

	// Token: 0x04005A39 RID: 23097
	[NonSerialized]
	public PlayerController gamePlayingPlayer;
}
