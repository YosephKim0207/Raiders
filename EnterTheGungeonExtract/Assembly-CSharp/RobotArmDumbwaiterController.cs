using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011F2 RID: 4594
public class RobotArmDumbwaiterController : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x06006698 RID: 26264 RVA: 0x0027EAD0 File Offset: 0x0027CCD0
	public static void HandlePuzzleSetup(GameObject RobotArmPrefab)
	{
		if (!GameStatsManager.Instance.GetFlag(GungeonFlags.META_SHOP_DELIVERED_ROBOT_ARM))
		{
			if (GameStatsManager.Instance.CurrentRobotArmFloor <= 0 || GameStatsManager.Instance.CurrentRobotArmFloor > 5)
			{
				GameStatsManager.Instance.CurrentRobotArmFloor = 5;
			}
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GameStatsManager.Instance.GetCurrentRobotArmTileset())
			{
				if (GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
				{
					List<PickupObject> list = new List<PickupObject>();
					list.Add(RobotArmPrefab.GetComponent<PickupObject>());
					if (list.Count > 0)
					{
						GameManager.Instance.Dungeon.data.DistributeComplexSecretPuzzleItems(list, null, true, 0f);
					}
				}
			}
		}
	}

	// Token: 0x06006699 RID: 26265 RVA: 0x0027EB9C File Offset: 0x0027CD9C
	private void Start()
	{
		RobotArmDumbwaiterController.HandlePuzzleSetup(this.RobotArmObject);
	}

	// Token: 0x0600669A RID: 26266 RVA: 0x0027EBAC File Offset: 0x0027CDAC
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x0600669B RID: 26267 RVA: 0x0027EBE0 File Offset: 0x0027CDE0
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x0600669C RID: 26268 RVA: 0x0027EC08 File Offset: 0x0027CE08
	public float GetDistanceToPoint(Vector2 point)
	{
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x0600669D RID: 26269 RVA: 0x0027ECE8 File Offset: 0x0027CEE8
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x0600669E RID: 26270 RVA: 0x0027ECF0 File Offset: 0x0027CEF0
	public void Interact(PlayerController player)
	{
		bool flag = false;
		for (int i = 0; i < player.additionalItems.Count; i++)
		{
			if (player.additionalItems[i] is RobotArmItem)
			{
				flag = true;
			}
		}
		if (flag)
		{
			this.OnExitRange(player);
			for (int j = 0; j < player.additionalItems.Count; j++)
			{
				if (player.additionalItems[j] is RobotArmItem)
				{
					player.UsePuzzleItem(player.additionalItems[j]);
					break;
				}
			}
			GameStatsManager.Instance.CurrentRobotArmFloor = GameStatsManager.Instance.CurrentRobotArmFloor - 1;
			if (GameStatsManager.Instance.CurrentRobotArmFloor == 0)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.META_SHOP_DELIVERED_ROBOT_ARM, true);
			}
		}
	}

	// Token: 0x0600669F RID: 26271 RVA: 0x0027EDC4 File Offset: 0x0027CFC4
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060066A0 RID: 26272 RVA: 0x0027EDD0 File Offset: 0x0027CFD0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400626C RID: 25196
	public GameObject RobotArmObject;
}
