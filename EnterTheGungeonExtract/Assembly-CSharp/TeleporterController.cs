using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001235 RID: 4661
public class TeleporterController : DungeonPlaceableBehaviour, IPlaceConfigurable, IPlayerInteractable
{
	// Token: 0x06006873 RID: 26739 RVA: 0x0028EA08 File Offset: 0x0028CC08
	public void Start()
	{
		IntVector2 intVector = (base.transform.position.XY() + new Vector2(0.5f, 0.5f)).ToIntVector2(VectorConversions.Floor);
		for (int i = intVector.x; i < intVector.x + this.placeableWidth; i++)
		{
			for (int j = intVector.y; j < intVector.y + this.placeableHeight; j++)
			{
				GameManager.Instance.Dungeon.data[i, j].PreventRewardSpawn = true;
			}
		}
	}

	// Token: 0x06006874 RID: 26740 RVA: 0x0028EAA8 File Offset: 0x0028CCA8
	public void Update()
	{
		if (!this.m_activated && GameManager.Instance.PrimaryPlayer != null && GameManager.Instance.PrimaryPlayer.CurrentRoom != null)
		{
			bool flag = GameManager.Instance.PrimaryPlayer.CurrentRoom == this.m_room || (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer != null && GameManager.Instance.SecondaryPlayer.CurrentRoom != null && GameManager.Instance.SecondaryPlayer.CurrentRoom == this.m_room);
			if (flag && !this.m_room.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
			{
				this.Activate();
			}
		}
	}

	// Token: 0x06006875 RID: 26741 RVA: 0x0028EB78 File Offset: 0x0028CD78
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06006876 RID: 26742 RVA: 0x0028EB80 File Offset: 0x0028CD80
	public void SetReturnActive()
	{
		this.m_wasJustWarpedTo = true;
		this.portalVFX.gameObject.SetActive(true);
	}

	// Token: 0x06006877 RID: 26743 RVA: 0x0028EB9C File Offset: 0x0028CD9C
	public void ClearReturnActive()
	{
		this.portalVFX.gameObject.SetActive(false);
	}

	// Token: 0x06006878 RID: 26744 RVA: 0x0028EBB0 File Offset: 0x0028CDB0
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
		Minimap.Instance.RegisterTeleportIcon(this.m_room, this.teleporterIcon, base.sprite.WorldCenter);
	}

	// Token: 0x06006879 RID: 26745 RVA: 0x0028EBDC File Offset: 0x0028CDDC
	public float GetDistanceToPoint(Vector2 point)
	{
		if (!this.portalVFX.gameObject.activeSelf)
		{
			return 10000f;
		}
		return Vector2.Distance(point, base.sprite.WorldCenter) / 3f;
	}

	// Token: 0x0600687A RID: 26746 RVA: 0x0028EC10 File Offset: 0x0028CE10
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x0600687B RID: 26747 RVA: 0x0028EC18 File Offset: 0x0028CE18
	public void OnEnteredRange(PlayerController interactor)
	{
		if (this.m_wasJustWarpedTo)
		{
			return;
		}
		if (this.m_activated && interactor.CanReturnTeleport)
		{
			this.portalVFX.sprite.HeightOffGround = -1f;
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		}
	}

	// Token: 0x0600687C RID: 26748 RVA: 0x0028EC78 File Offset: 0x0028CE78
	public void OnExitRange(PlayerController interactor)
	{
		this.m_wasJustWarpedTo = false;
		if (this.m_activated)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		}
	}

	// Token: 0x0600687D RID: 26749 RVA: 0x0028EC98 File Offset: 0x0028CE98
	public void Interact(PlayerController interactor)
	{
		if (GameManager.Instance.AllPlayers != null)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				if (playerController && playerController.IsTalking)
				{
					return;
				}
			}
		}
		if (this.m_activated)
		{
			if (interactor.CanReturnTeleport)
			{
				for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
				{
					GameManager.Instance.AllPlayers[j].AttemptReturnTeleport(this);
				}
			}
		}
	}

	// Token: 0x0600687E RID: 26750 RVA: 0x0028ED40 File Offset: 0x0028CF40
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x0600687F RID: 26751 RVA: 0x0028ED4C File Offset: 0x0028CF4C
	private void Activate()
	{
		this.m_activated = true;
		base.spriteAnimator.Play("teleport_pad_activate");
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.TriggerActiveVFX));
		if (this.onetimeActivateVFX != null)
		{
			this.onetimeActivateVFX.SetActive(true);
			this.onetimeActivateVFX.GetComponent<tk2dSprite>().IsPerpendicular = false;
		}
	}

	// Token: 0x06006880 RID: 26752 RVA: 0x0028EDC8 File Offset: 0x0028CFC8
	private void TriggerActiveVFX(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
	{
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.TriggerActiveVFX));
		this.extantActiveVFX.SetActive(true);
	}

	// Token: 0x040064B3 RID: 25779
	public GameObject teleporterIcon;

	// Token: 0x040064B4 RID: 25780
	public GameObject teleportDepartureVFX;

	// Token: 0x040064B5 RID: 25781
	public GameObject teleportArrivalVFX;

	// Token: 0x040064B6 RID: 25782
	public GameObject onetimeActivateVFX;

	// Token: 0x040064B7 RID: 25783
	public GameObject extantActiveVFX;

	// Token: 0x040064B8 RID: 25784
	public tk2dSpriteAnimator portalVFX;

	// Token: 0x040064B9 RID: 25785
	private bool m_wasJustWarpedTo;

	// Token: 0x040064BA RID: 25786
	private RoomHandler m_room;

	// Token: 0x040064BB RID: 25787
	private bool m_activated;
}
