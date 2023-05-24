using System;
using UnityEngine;

// Token: 0x020011DF RID: 4575
public class PlaymakerEventInteractable : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x06006617 RID: 26135 RVA: 0x0027ABDC File Offset: 0x00278DDC
	public float GetDistanceToPoint(Vector2 point)
	{
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x06006618 RID: 26136 RVA: 0x0027ACBC File Offset: 0x00278EBC
	public void OnEnteredRange(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
	}

	// Token: 0x06006619 RID: 26137 RVA: 0x0027ACDC File Offset: 0x00278EDC
	public void OnExitRange(PlayerController interactor)
	{
		if (base.sprite)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
		}
	}

	// Token: 0x0600661A RID: 26138 RVA: 0x0027AD0C File Offset: 0x00278F0C
	public void Interact(PlayerController interactor)
	{
		GameManager.BroadcastRoomFsmEvent(this.EventToTrigger, interactor.CurrentRoom);
	}

	// Token: 0x0600661B RID: 26139 RVA: 0x0027AD20 File Offset: 0x00278F20
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x0600661C RID: 26140 RVA: 0x0027AD2C File Offset: 0x00278F2C
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x040061ED RID: 25069
	public string EventToTrigger;
}
