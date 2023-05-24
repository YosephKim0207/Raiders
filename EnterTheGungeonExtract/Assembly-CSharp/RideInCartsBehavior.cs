using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DF0 RID: 3568
public class RideInCartsBehavior : MovementBehaviorBase
{
	// Token: 0x06004B98 RID: 19352 RVA: 0x0019B78C File Offset: 0x0019998C
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_findNewCartTimer, false);
	}

	// Token: 0x06004B99 RID: 19353 RVA: 0x0019B7A4 File Offset: 0x001999A4
	private MineCartController GetAvailableMineCart()
	{
		List<MineCartController> componentsInRoom = this.m_aiActor.ParentRoom.GetComponentsInRoom<MineCartController>();
		for (int i = 0; i < componentsInRoom.Count; i++)
		{
			if (componentsInRoom[i].IsOnlyPlayerMinecart || componentsInRoom[i].occupation != MineCartController.CartOccupationState.EMPTY)
			{
				componentsInRoom.RemoveAt(i);
				i--;
			}
		}
		componentsInRoom.Sort((MineCartController a, MineCartController b) => Vector2.Distance(this.m_aiActor.CenterPosition, a.sprite.WorldCenter).CompareTo(Vector2.Distance(this.m_aiActor.CenterPosition, b.sprite.WorldCenter)));
		if (componentsInRoom.Count == 0)
		{
			return null;
		}
		return componentsInRoom[0];
	}

	// Token: 0x06004B9A RID: 19354 RVA: 0x0019B82C File Offset: 0x00199A2C
	public override BehaviorResult Update()
	{
		if (GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.MINEGEON)
		{
			return BehaviorResult.Continue;
		}
		if (this.m_ridingCart)
		{
			return BehaviorResult.SkipRemainingClassBehaviors;
		}
		if (this.m_findNewCartTimer <= 0f)
		{
			this.m_currentTarget = this.GetAvailableMineCart();
			this.m_findNewCartTimer = 5f;
		}
		if (!(this.m_currentTarget != null))
		{
			return BehaviorResult.Continue;
		}
		if (this.m_currentTarget.occupation != MineCartController.CartOccupationState.EMPTY)
		{
			this.m_findNewCartTimer = 0f;
			return BehaviorResult.Continue;
		}
		this.m_aiActor.PathfindToPosition(this.m_currentTarget.sprite.WorldCenter, null, true, null, null, null, false);
		if (Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, this.m_currentTarget.specRigidbody.UnitCenter) < 5f && BraveMathCollege.DistBetweenRectangles(this.m_aiActor.specRigidbody.UnitBottomLeft, this.m_aiActor.specRigidbody.UnitDimensions, this.m_currentTarget.specRigidbody.UnitBottomLeft, this.m_currentTarget.specRigidbody.UnitDimensions) < 0.5f)
		{
			this.m_aiActor.ClearPath();
			this.m_currentTarget.BecomeOccupied(this.m_aiActor);
			this.m_ridingCart = true;
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x0400414E RID: 16718
	private MineCartController m_currentTarget;

	// Token: 0x0400414F RID: 16719
	private bool m_ridingCart;

	// Token: 0x04004150 RID: 16720
	protected float m_findNewCartTimer;
}
