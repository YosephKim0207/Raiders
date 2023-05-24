using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000DE2 RID: 3554
public class BabyGoodMimicCombatMovementBehavior : MovementBehaviorBase
{
	// Token: 0x06004B45 RID: 19269 RVA: 0x0019764C File Offset: 0x0019584C
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004B46 RID: 19270 RVA: 0x00197654 File Offset: 0x00195854
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
	}

	// Token: 0x06004B47 RID: 19271 RVA: 0x0019766C File Offset: 0x0019586C
	public override BehaviorResult Update()
	{
		if (this.m_repathTimer > 0f)
		{
			Vector2? targetPos = this.m_targetPos;
			return (targetPos == null) ? BehaviorResult.Continue : BehaviorResult.SkipRemainingClassBehaviors;
		}
		PlayerController playerController = GameManager.Instance.BestActivePlayer;
		if (this.m_aiActor && this.m_aiActor.CompanionOwner)
		{
			playerController = this.m_aiActor.CompanionOwner;
		}
		if (!playerController)
		{
			return BehaviorResult.Continue;
		}
		if (this.m_cachedRoom != null && playerController.CurrentRoom != this.m_cachedRoom)
		{
			this.m_cachedRoom = null;
			this.m_targetPos = null;
		}
		if (playerController.IsInCombat && playerController.CurrentRoom.IsSealed)
		{
			if (this.m_targetPos == null)
			{
				IntVector2? intVector = null;
				IntVector2 intVector2 = new IntVector2(3, 3);
				while (intVector == null && intVector2.x > 0)
				{
					intVector = new IntVector2?(playerController.CurrentRoom.GetRandomAvailableCell(new IntVector2?(intVector2), new CellTypes?(CellTypes.FLOOR), false, null).Value);
					intVector2 -= IntVector2.One;
				}
				if (intVector != null)
				{
					this.m_targetPos = new Vector2?(intVector.Value.ToVector2() + intVector2.ToVector2() / 2f);
					this.m_cachedRoom = playerController.CurrentRoom;
				}
			}
		}
		else
		{
			this.m_targetPos = null;
		}
		Vector2? targetPos2 = this.m_targetPos;
		if (targetPos2 != null)
		{
			this.m_aiActor.PathfindToPosition(this.m_targetPos.Value, null, true, null, null, null, false);
			this.m_repathTimer = this.PathInterval;
		}
		Vector2? targetPos3 = this.m_targetPos;
		return (targetPos3 == null) ? BehaviorResult.Continue : BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x040040AC RID: 16556
	public float PathInterval = 0.25f;

	// Token: 0x040040AD RID: 16557
	private float m_repathTimer;

	// Token: 0x040040AE RID: 16558
	private Vector2? m_targetPos;

	// Token: 0x040040AF RID: 16559
	private RoomHandler m_cachedRoom;
}
