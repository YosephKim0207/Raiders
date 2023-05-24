using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200154D RID: 5453
public class BasicInteractItemGiver : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x06007CD8 RID: 31960 RVA: 0x003259FC File Offset: 0x00323BFC
	private void Start()
	{
		this.m_room = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
		this.m_room.RegisterInteractable(this);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
	}

	// Token: 0x06007CD9 RID: 31961 RVA: 0x00325A50 File Offset: 0x00323C50
	public float GetDistanceToPoint(Vector2 point)
	{
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x06007CDA RID: 31962 RVA: 0x00325B30 File Offset: 0x00323D30
	public void OnEnteredRange(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
	}

	// Token: 0x06007CDB RID: 31963 RVA: 0x00325B50 File Offset: 0x00323D50
	public void OnExitRange(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
	}

	// Token: 0x06007CDC RID: 31964 RVA: 0x00325B70 File Offset: 0x00323D70
	public void Interact(PlayerController interactor)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (this.flagToSetOnAcquisition != GungeonFlags.NONE)
		{
			GameStatsManager.Instance.SetFlag(this.flagToSetOnAcquisition, true);
		}
		this.m_pickedUp = true;
		this.m_room.DeregisterInteractable(this);
		PickupObject byId = PickupObjectDatabase.GetById(this.pickupIdToGive);
		LootEngine.TryGivePrefabToPlayer(byId.gameObject, interactor, true);
		if (this.destroyThisOnAcquisition)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06007CDD RID: 31965 RVA: 0x00325BF4 File Offset: 0x00323DF4
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06007CDE RID: 31966 RVA: 0x00325C00 File Offset: 0x00323E00
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06007CDF RID: 31967 RVA: 0x00325C08 File Offset: 0x00323E08
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007FE9 RID: 32745
	[PickupIdentifier]
	public int pickupIdToGive = -1;

	// Token: 0x04007FEA RID: 32746
	public GungeonFlags flagToSetOnAcquisition;

	// Token: 0x04007FEB RID: 32747
	public bool destroyThisOnAcquisition;

	// Token: 0x04007FEC RID: 32748
	private bool m_pickedUp;

	// Token: 0x04007FED RID: 32749
	private RoomHandler m_room;
}
