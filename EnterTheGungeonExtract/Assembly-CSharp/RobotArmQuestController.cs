using System;
using System.Collections.Generic;
using DaikonForge.Tween;
using Dungeonator;
using UnityEngine;

// Token: 0x020011F4 RID: 4596
public static class RobotArmQuestController
{
	// Token: 0x060066A3 RID: 26275 RVA: 0x0027F03C File Offset: 0x0027D23C
	public static void HandlePuzzleSetup()
	{
		if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_CLEARED_FORGE) == 0f)
		{
			return;
		}
		PickupObject byId = PickupObjectDatabase.GetById(GlobalItemIds.RobotArm);
		PickupObject byId2 = PickupObjectDatabase.GetById(GlobalItemIds.RobotBalloons);
		List<PickupObject> list = new List<PickupObject>();
		if (!GameStatsManager.Instance.GetFlag(GungeonFlags.META_SHOP_DELIVERED_ROBOT_ARM))
		{
			if (GameStatsManager.Instance.CurrentRobotArmFloor < 0 || GameStatsManager.Instance.CurrentRobotArmFloor > 5)
			{
				GameStatsManager.Instance.CurrentRobotArmFloor = 5;
			}
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER && GameStatsManager.Instance.CurrentRobotArmFloor == 0)
			{
				RoomHandler entrance = GameManager.Instance.Dungeon.data.Entrance;
				if (entrance != null)
				{
					IntVector2 intVector = new IntVector2(29, 62);
					DungeonPlaceableUtility.InstantiateDungeonPlaceable(BraveResources.Load("Global Prefabs/Global Items/RobotArmPlaceable", ".prefab") as GameObject, entrance, intVector - entrance.area.basePosition, false, AIActor.AwakenAnimationType.Default, false);
				}
			}
			else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GameStatsManager.Instance.GetCurrentRobotArmTileset())
			{
				if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
				{
					BaseShopController[] array = UnityEngine.Object.FindObjectsOfType<BaseShopController>();
					RoomHandler roomHandler = null;
					Transform transform = null;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].name.Contains("Blacksmith"))
						{
							roomHandler = array[i].GetAbsoluteParentRoom();
							transform = array[i].transform.Find("ArmPoint");
							break;
						}
					}
					if (roomHandler != null)
					{
						bool flag = false;
						IntVector2 intVector2 = IntVector2.Zero;
						if (transform != null)
						{
							flag = true;
							intVector2 = transform.position.IntXY(VectorConversions.Round);
						}
						else
						{
							intVector2 = roomHandler.GetCenteredVisibleClearSpot(2, 2, out flag, true);
						}
						if (flag)
						{
							DungeonPlaceableUtility.InstantiateDungeonPlaceable(BraveResources.Load("Global Prefabs/Global Items/RobotArmPlaceable", ".prefab") as GameObject, roomHandler, intVector2 - roomHandler.area.basePosition, false, AIActor.AwakenAnimationType.Default, false);
							if (GameStatsManager.Instance.GetFlag(GungeonFlags.META_SHOP_EVER_SEEN_ROBOT_ARM))
							{
								list.Add(byId2.GetComponent<PickupObject>());
							}
						}
					}
				}
				else
				{
					list.Add(byId);
					list.Add(byId2);
				}
			}
		}
		if (list.Count > 0)
		{
			GameManager.Instance.Dungeon.data.DistributeComplexSecretPuzzleItems(list, null, true, 0f);
		}
	}

	// Token: 0x060066A4 RID: 26276 RVA: 0x0027F2B0 File Offset: 0x0027D4B0
	public static void CombineBalloonsWithArm(PickupObject balloonsObject, PickupObject armObject, PlayerController relevantPlayer)
	{
		relevantPlayer.UsePuzzleItem(balloonsObject);
		relevantPlayer.UsePuzzleItem(armObject);
		if (balloonsObject)
		{
			UnityEngine.Object.Destroy(balloonsObject.gameObject);
		}
		if (armObject)
		{
			UnityEngine.Object.Destroy(armObject.gameObject);
		}
		BalloonAttachmentDoer balloonAttachmentDoer = UnityEngine.Object.FindObjectOfType<BalloonAttachmentDoer>();
		if (balloonAttachmentDoer)
		{
			UnityEngine.Object.Destroy(balloonAttachmentDoer.gameObject);
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global VFX/VFX_BalloonArmLift", ".prefab"));
		gameObject.transform.position = relevantPlayer.SpriteBottomCenter;
		Tween<Vector3> tween = gameObject.transform.TweenMoveTo(gameObject.transform.position + new Vector3(0f, 20f, 0f));
		AnimationCurve sourceCurve = gameObject.GetComponent<SimpleAnimationCurveHolder>().curve;
		tween.Easing = (float a) => sourceCurve.Evaluate(a);
		tween.Duration = 4.5f;
		tween.Play();
		GameStatsManager.Instance.CurrentRobotArmFloor = GameStatsManager.Instance.CurrentRobotArmFloor - 1;
		GameUIRoot.Instance.notificationController.DoCustomNotification(StringTableManager.GetString("#METASHOP_ARM_UP_ONE_LEVEL_HEADER"), StringTableManager.GetString("#METASHOP_ARM_UP_ONE_LEVEL_BODY"), gameObject.GetComponent<tk2dBaseSprite>().Collection, gameObject.GetComponent<tk2dBaseSprite>().spriteId, UINotificationController.NotificationColor.GOLD, false, false);
	}
}
