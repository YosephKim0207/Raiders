using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020011A1 RID: 4513
public class LedgeGoblinHelmetController : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x0600646D RID: 25709 RVA: 0x0026ED04 File Offset: 0x0026CF04
	private void Start()
	{
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
	}

	// Token: 0x0600646E RID: 25710 RVA: 0x0026ED18 File Offset: 0x0026CF18
	public float GetDistanceToPoint(Vector2 point)
	{
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x0600646F RID: 25711 RVA: 0x0026EDF8 File Offset: 0x0026CFF8
	public void OnEnteredRange(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
		GameStatsManager.Instance.SetFlag(GungeonFlags.LEDGEGOBLIN_ACTIVE_IN_FOYER, true);
	}

	// Token: 0x06006470 RID: 25712 RVA: 0x0026EE28 File Offset: 0x0026D028
	public void OnExitRange(PlayerController interactor)
	{
		if (base.sprite)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
		}
	}

	// Token: 0x06006471 RID: 25713 RVA: 0x0026EE58 File Offset: 0x0026D058
	public void Interact(PlayerController interactor)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_pickedUp = true;
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		GameManager.BroadcastRoomTalkDoerFsmEvent("modeAnnoyed");
		bool flag = GameStatsManager.Instance.GetFlag(GungeonFlags.LEDGEGOBLIN_COMPLETED_FIRST_DUNGEON);
		bool flag2 = GameStatsManager.Instance.GetFlag(GungeonFlags.LEDGEGOBLIN_COMPLETED_SECOND_DUNGEON);
		if (!GameStatsManager.Instance.GetFlag(GungeonFlags.LEDGEGOBLIN_COMPLETED_THIRD_DUNGEON) && flag2 && flag)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.LEDGEGOBLIN_TRIGGERED_THIRD_DUNGEON, true);
		}
		else if (!flag2 && flag)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.LEDGEGOBLIN_TRIGGERED_SECOND_DUNGEON, true);
		}
		else if (!flag)
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.LEDGEGOBLIN_TRIGGERED_FIRST_DUNGEON, true);
		}
		RoomHandler.unassignedInteractableObjects.Remove(this);
		interactor.RemoveBrokenInteractable(this);
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.LEDGEGOBLIN_COMPLETED_THIRD_DUNGEON))
		{
			base.spriteAnimator.Play("helmte_kick_chain");
			base.transform.position.GetAbsoluteRoom().DeregisterInteractable(this);
			RoomHandler.unassignedInteractableObjects.Remove(this);
		}
		else
		{
			base.spriteAnimator.PlayAndDestroyObject(string.Empty, null);
		}
	}

	// Token: 0x06006472 RID: 25714 RVA: 0x0026EF8C File Offset: 0x0026D18C
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06006473 RID: 25715 RVA: 0x0026EF98 File Offset: 0x0026D198
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006474 RID: 25716 RVA: 0x0026EFA0 File Offset: 0x0026D1A0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005FF5 RID: 24565
	private bool m_pickedUp;
}
