using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001244 RID: 4676
public class UsableBasicWarp : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x060068C8 RID: 26824 RVA: 0x00290A88 File Offset: 0x0028EC88
	private void Start()
	{
		GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor)).RegisterInteractable(this);
	}

	// Token: 0x060068C9 RID: 26825 RVA: 0x00290AB8 File Offset: 0x0028ECB8
	public float GetDistanceToPoint(Vector2 point)
	{
		return Vector2.Distance(point, base.sprite.WorldBottomCenter);
	}

	// Token: 0x060068CA RID: 26826 RVA: 0x00290ACC File Offset: 0x0028ECCC
	public void OnEnteredRange(PlayerController interactor)
	{
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
		if (this.IsHelicopterLadder)
		{
			this.m_justWarped = false;
		}
	}

	// Token: 0x060068CB RID: 26827 RVA: 0x00290AF0 File Offset: 0x0028ECF0
	public void OnExitRange(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
	}

	// Token: 0x060068CC RID: 26828 RVA: 0x00290B00 File Offset: 0x0028ED00
	public void Interact(PlayerController interactor)
	{
		if (this.m_justWarped)
		{
			return;
		}
		if (!this.IsRatTrapdoorLadder)
		{
			base.StartCoroutine(this.HandleWarpCooldown(interactor));
		}
		else
		{
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleWarpCooldown(interactor));
		}
	}

	// Token: 0x060068CD RID: 26829 RVA: 0x00290B50 File Offset: 0x0028ED50
	private IEnumerator HandleWarpCooldown(PlayerController player)
	{
		this.m_justWarped = true;
		Pixelator.Instance.FadeToBlack(0.1f, false, 0f);
		yield return new WaitForSeconds(0.1f);
		player.SetInputOverride("arbitrary warp");
		if (this.IsRatTrapdoorLadder)
		{
			ResourcefulRatMinesHiddenTrapdoor resourcefulRatMinesHiddenTrapdoor = StaticReferenceManager.AllRatTrapdoors[0];
			RoomHandler absoluteRoom = resourcefulRatMinesHiddenTrapdoor.transform.position.GetAbsoluteRoom();
			Vector2 vector = resourcefulRatMinesHiddenTrapdoor.transform.position.XY() + new Vector2(2f, 3.25f);
			player.WarpToPoint(vector, false, false);
		}
		else if (this.IsHelicopterLadder)
		{
			RoomHandler roomHandler = null;
			foreach (RoomHandler roomHandler2 in GameManager.Instance.Dungeon.data.rooms)
			{
				if (roomHandler2.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS && roomHandler2.area.PrototypeRoomBossSubcategory != PrototypeDungeonRoom.RoomBossSubCategory.MINI_BOSS)
				{
					roomHandler = roomHandler2;
					break;
				}
			}
			Vector2 vector2 = roomHandler.area.basePosition.ToVector2() + new Vector2((float)roomHandler.area.dimensions.x / 2f, 8f);
			player.WarpToPoint(vector2, false, false);
		}
		else
		{
			RoomHandler entrance = GameManager.Instance.Dungeon.data.Entrance;
			Vector2 vector3 = entrance.GetCenterCell().ToVector2() + new Vector2(0f, -5f);
			player.WarpToPoint(vector3, false, false);
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(player);
			if (otherPlayer && otherPlayer.healthHaver.IsAlive)
			{
				otherPlayer.ReuniteWithOtherPlayer(player, false);
			}
		}
		GameManager.Instance.MainCameraController.ForceToPlayerPosition(player);
		Pixelator.Instance.FadeToBlack(0.1f, true, 0f);
		player.ClearInputOverride("arbitrary warp");
		yield return new WaitForSeconds(1f);
		this.m_justWarped = false;
		yield break;
	}

	// Token: 0x060068CE RID: 26830 RVA: 0x00290B74 File Offset: 0x0028ED74
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060068CF RID: 26831 RVA: 0x00290B80 File Offset: 0x0028ED80
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x04006521 RID: 25889
	public bool IsRatTrapdoorLadder;

	// Token: 0x04006522 RID: 25890
	public bool IsHelicopterLadder;

	// Token: 0x04006523 RID: 25891
	private bool m_justWarped;
}
