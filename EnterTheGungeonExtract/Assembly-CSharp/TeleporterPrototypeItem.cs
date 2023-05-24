using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x020014D4 RID: 5332
public class TeleporterPrototypeItem : PlayerItem
{
	// Token: 0x06007936 RID: 31030 RVA: 0x00307FAC File Offset: 0x003061AC
	public override bool CanBeUsed(PlayerController user)
	{
		if (!user || user.IsInMinecart)
		{
			return false;
		}
		if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON)
		{
			return false;
		}
		if (user.CurrentRoom != null)
		{
			if (user.CurrentRoom.CompletelyPreventLeaving)
			{
				return false;
			}
			if (GameManager.Instance.Dungeon != null && GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON && user.CurrentRoom != null && user.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
			{
				return false;
			}
			if (user.CurrentRoom.IsSealed && Mathf.Abs(user.RealtimeEnteredCurrentRoom - Time.realtimeSinceStartup) < 0.5f)
			{
				return false;
			}
			if (!user.CurrentRoom.CanBeEscaped())
			{
				return false;
			}
		}
		return base.CanBeUsed(user);
	}

	// Token: 0x06007937 RID: 31031 RVA: 0x003080AC File Offset: 0x003062AC
	protected void TelefragRandomEnemy(RoomHandler room)
	{
		AIActor randomActiveEnemy = room.GetRandomActiveEnemy(true);
		if (randomActiveEnemy.IsNormalEnemy && randomActiveEnemy.healthHaver && !randomActiveEnemy.healthHaver.IsBoss)
		{
			Vector2 vector = ((!randomActiveEnemy.specRigidbody) ? randomActiveEnemy.sprite.WorldBottomLeft : randomActiveEnemy.specRigidbody.UnitBottomLeft);
			Vector2 vector2 = ((!randomActiveEnemy.specRigidbody) ? randomActiveEnemy.sprite.WorldTopRight : randomActiveEnemy.specRigidbody.UnitTopRight);
			UnityEngine.Object.Instantiate<GameObject>(this.TelefragVFXPrefab, randomActiveEnemy.CenterPosition.ToVector3ZisY(0f), Quaternion.identity);
			randomActiveEnemy.healthHaver.ApplyDamage(100000f, Vector2.zero, "Telefrag", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
		}
	}

	// Token: 0x06007938 RID: 31032 RVA: 0x00308184 File Offset: 0x00306384
	protected void TelefragRoom(RoomHandler room)
	{
		Pixelator.Instance.FadeToColor(0.25f, Color.white, true, 0f);
		List<AIActor> activeEnemies = room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			if (activeEnemies[i].IsNormalEnemy && activeEnemies[i].healthHaver && !activeEnemies[i].healthHaver.IsBoss)
			{
				Vector2 vector = ((!activeEnemies[i].specRigidbody) ? activeEnemies[i].sprite.WorldBottomLeft : activeEnemies[i].specRigidbody.UnitBottomLeft);
				Vector2 vector2 = ((!activeEnemies[i].specRigidbody) ? activeEnemies[i].sprite.WorldTopRight : activeEnemies[i].specRigidbody.UnitTopRight);
				UnityEngine.Object.Instantiate<GameObject>(this.TelefragVFXPrefab, activeEnemies[i].CenterPosition.ToVector3ZisY(0f), Quaternion.identity);
				activeEnemies[i].healthHaver.ApplyDamage(100000f, Vector2.zero, "Telefrag", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
			}
		}
	}

	// Token: 0x06007939 RID: 31033 RVA: 0x003082D0 File Offset: 0x003064D0
	protected override void DoEffect(PlayerController user)
	{
		if (user.CurrentRoom != null && user.CurrentRoom.CompletelyPreventLeaving)
		{
			return;
		}
		AkSoundEngine.PostEvent("Play_OBJ_teleport_depart_01", base.gameObject);
		RoomHandler roomHandler = null;
		GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
		float value = UnityEngine.Random.value;
		bool flag = user.HasActiveBonusSynergy(CustomSynergyType.DOUBLE_TELEPORTERS, false);
		this.LastCooldownModifier = 1f;
		if (value < this.ChanceToGoToNextFloor)
		{
			if (tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON && tilesetId != GlobalDungeonData.ValidTilesets.HELLGEON)
			{
				this.PlayTeleporterEffect(user);
				Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
				GameManager.Instance.DelayedLoadNextLevel(0.5f);
			}
		}
		else if (value < this.ChanceToGoToNextFloor + this.ChanceToGoToEyeballRoom && !user.IsInCombat)
		{
			this.PlayTeleporterEffect(user);
			base.StartCoroutine(this.HandleCreepyEyeWarp(user));
			AkSoundEngine.PostEvent("Play_OBJ_teleport_depart_01", base.gameObject);
			if (flag)
			{
				this.LastCooldownModifier = 0.5f;
			}
		}
		else if (value < this.ChanceToGoToNextFloor + this.ChanceToGoToEyeballRoom + this.ChanceToGoToSpecialRoom)
		{
			List<int> list = Enumerable.Range(0, GameManager.Instance.Dungeon.data.rooms.Count).ToList<int>().Shuffle<int>();
			for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
			{
				RoomHandler roomHandler2 = GameManager.Instance.Dungeon.data.rooms[list[i]];
				if (roomHandler2.IsSecretRoom)
				{
					roomHandler = roomHandler2;
				}
			}
			if (roomHandler == null)
			{
				for (int j = 0; j < GameManager.Instance.Dungeon.data.rooms.Count; j++)
				{
					RoomHandler roomHandler3 = GameManager.Instance.Dungeon.data.rooms[list[j]];
					if (roomHandler3.IsShop || roomHandler3.IsSecretRoom || roomHandler3.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.REWARD)
					{
						if (roomHandler3.IsSecretRoom)
						{
							roomHandler3.secretRoomManager.HandleDoorBrokenOpen(roomHandler3.secretRoomManager.doorObjects[0]);
						}
						roomHandler = roomHandler3;
						break;
					}
				}
			}
			if (flag)
			{
				this.LastCooldownModifier = 0.5f;
			}
		}
		else if (value < this.ChanceToGoToNextFloor + this.ChanceToGoToEyeballRoom + this.ChanceToGoToSpecialRoom + this.ChanceToGoToBossFoyer)
		{
			RoomHandler roomHandler4 = ChestTeleporterItem.FindBossFoyer();
			if (roomHandler4 != null)
			{
				roomHandler = roomHandler4;
			}
			if (flag)
			{
				this.LastCooldownModifier = 0.5f;
			}
		}
		else if (value < this.ChanceToGoToNextFloor + this.ChanceToGoToEyeballRoom + this.ChanceToGoToSpecialRoom + this.ChanceToGoToSecretFloor && (tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON || tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON))
		{
			if (tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON)
			{
				this.PlayTeleporterEffect(user);
				Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
				GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.CATHEDRALGEON);
				GameManager.Instance.DelayedLoadCustomLevel(0.5f, "tt_sewer");
			}
			else if (tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON)
			{
				this.PlayTeleporterEffect(user);
				Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
				GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.CATHEDRALGEON);
				GameManager.Instance.DelayedLoadCustomLevel(0.5f, "tt_cathedral");
			}
		}
		else
		{
			List<int> list2 = Enumerable.Range(0, GameManager.Instance.Dungeon.data.rooms.Count).ToList<int>().Shuffle<int>();
			for (int k = 0; k < GameManager.Instance.Dungeon.data.rooms.Count; k++)
			{
				RoomHandler roomHandler5 = GameManager.Instance.Dungeon.data.rooms[list2[k]];
				if ((roomHandler5.area.PrototypeRoomNormalSubcategory != PrototypeDungeonRoom.RoomNormalSubCategory.TRAP && roomHandler5.IsStandardRoom && roomHandler5.EverHadEnemies) || roomHandler5.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.REWARD || roomHandler5.IsShop)
				{
					roomHandler = roomHandler5;
					break;
				}
			}
		}
		if (roomHandler != null)
		{
			user.EscapeRoom(PlayerController.EscapeSealedRoomStyle.TELEPORTER, true, roomHandler);
			if (roomHandler.IsSecretRoom && roomHandler.secretRoomManager != null && roomHandler.secretRoomManager.doorObjects.Count > 0)
			{
				roomHandler.secretRoomManager.doorObjects[0].BreakOpen();
			}
			bool flag2 = flag;
			if ((roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.NORMAL && roomHandler.area.PrototypeRoomNormalSubcategory == PrototypeDungeonRoom.RoomNormalSubCategory.COMBAT) || roomHandler.area.IsProceduralRoom)
			{
				user.StartCoroutine(this.HandleTelefragDelay(roomHandler, flag2));
			}
		}
	}

	// Token: 0x0600793A RID: 31034 RVA: 0x003087BC File Offset: 0x003069BC
	protected override void AfterCooldownApplied(PlayerController user)
	{
		if (this.LastCooldownModifier < 1f)
		{
			base.AfterCooldownApplied(user);
			base.DidDamage(user, base.CurrentDamageCooldown * (1f - this.LastCooldownModifier));
		}
	}

	// Token: 0x0600793B RID: 31035 RVA: 0x003087F0 File Offset: 0x003069F0
	private IEnumerator HandleTelefragDelay(RoomHandler targetRoom, bool allEnemies)
	{
		yield return new WaitForSeconds(1.5f);
		if (targetRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
		{
			if (allEnemies)
			{
				this.TelefragRoom(targetRoom);
			}
			else
			{
				this.TelefragRandomEnemy(targetRoom);
			}
		}
		yield break;
	}

	// Token: 0x0600793C RID: 31036 RVA: 0x0030881C File Offset: 0x00306A1C
	private IEnumerator HandleCreepyEyeWarp(PlayerController interactor)
	{
		RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(new IntVector2(24, 24), (GameObject)BraveResources.Load("Global Prefabs/CreepyEye_Room", ".prefab"));
		GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_YELLOW_CHAMBER, true);
		yield return new WaitForSeconds(0.25f);
		Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, creepyRoom.area.basePosition, creepyRoom.area.dimensions);
		interactor.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(12, 4)).ToVector2(), false, false);
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.GetOtherPlayer(interactor).ReuniteWithOtherPlayer(interactor, false);
		}
		yield break;
	}

	// Token: 0x0600793D RID: 31037 RVA: 0x00308838 File Offset: 0x00306A38
	private void PlayTeleporterEffect(PlayerController p)
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (!GameManager.Instance.AllPlayers[i].IsGhost)
			{
				GameManager.Instance.AllPlayers[i].healthHaver.TriggerInvulnerabilityPeriod(1f);
				GameManager.Instance.AllPlayers[i].knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
			}
		}
		GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Teleport_Beam");
		if (gameObject != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(p.specRigidbody.UnitBottomCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
			gameObject2.transform.position = gameObject2.transform.position.Quantize(0.0625f);
			gameObject2.GetComponent<tk2dBaseSprite>().UpdateZDepth();
		}
	}

	// Token: 0x04007BB0 RID: 31664
	public float ChanceToGoToSpecialRoom = 0.1f;

	// Token: 0x04007BB1 RID: 31665
	public float ChanceToGoToEyeballRoom = 0.01f;

	// Token: 0x04007BB2 RID: 31666
	public float ChanceToGoToNextFloor = 0.01f;

	// Token: 0x04007BB3 RID: 31667
	public float ChanceToGoToSecretFloor = 0.01f;

	// Token: 0x04007BB4 RID: 31668
	public float ChanceToGoToBossFoyer = 0.01f;

	// Token: 0x04007BB5 RID: 31669
	[Header("Synergies")]
	public GameObject TelefragVFXPrefab;

	// Token: 0x04007BB6 RID: 31670
	private float LastCooldownModifier = 1f;
}
