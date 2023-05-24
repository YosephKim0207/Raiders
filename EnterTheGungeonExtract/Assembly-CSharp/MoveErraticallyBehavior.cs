using System;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02000DED RID: 3565
public class MoveErraticallyBehavior : MovementBehaviorBase
{
	// Token: 0x06004B88 RID: 19336 RVA: 0x0019AF9C File Offset: 0x0019919C
	public override void Start()
	{
		base.Start();
		this.m_pauseTimer = this.InitialDelay;
	}

	// Token: 0x06004B89 RID: 19337 RVA: 0x0019AFB0 File Offset: 0x001991B0
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
		base.DecrementTimer(ref this.m_pauseTimer, false);
		if (this.StayOnScreen)
		{
			Vector2 vector = BraveUtility.ViewportToWorldpoint(new Vector2(0f, 0f), ViewportType.Gameplay);
			Vector2 vector2 = BraveUtility.ViewportToWorldpoint(new Vector2(1f, 1f), ViewportType.Gameplay);
			this.m_cachedCameraBottomLeft = vector.ToIntVector2(VectorConversions.Ceil);
			this.m_cachedCameraBottomRight = vector2.ToIntVector2(VectorConversions.Floor) - IntVector2.One;
		}
		if (this.AvoidTarget && this.m_aiActor.TargetRigidbody)
		{
			this.m_cachedTargetPos = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.Ground);
			this.m_cachedAngleFromTarget = (this.m_aiActor.specRigidbody.UnitCenter - this.m_cachedTargetPos).ToAngle();
			PlayerController playerController = this.m_aiActor.PlayerTarget as PlayerController;
			if (playerController && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(playerController);
				this.m_cachedOtherTargetPos = new Vector2?(otherPlayer.specRigidbody.GetUnitCenter(ColliderType.Ground));
				this.m_cachedAngleFromOtherTarget = new float?((this.m_aiActor.specRigidbody.UnitCenter - this.m_cachedTargetPos).ToAngle());
			}
		}
	}

	// Token: 0x06004B8A RID: 19338 RVA: 0x0019B11C File Offset: 0x0019931C
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		IntVector2? targetPos = this.m_targetPos;
		if (targetPos == null && this.m_repathTimer > 0f)
		{
			return BehaviorResult.Continue;
		}
		if (this.m_pauseTimer > 0f)
		{
			return BehaviorResult.SkipRemainingClassBehaviors;
		}
		IntVector2? targetPos2 = this.m_targetPos;
		if (targetPos2 != null && this.m_aiActor.PathComplete)
		{
			this.m_targetPos = null;
			if (this.PointReachedPauseTime > 0f)
			{
				this.m_pauseTimer = this.PointReachedPauseTime;
				return BehaviorResult.SkipAllRemainingBehaviors;
			}
		}
		if (this.m_repathTimer <= 0f)
		{
			this.m_repathTimer = this.PathInterval;
			IntVector2? targetPos3 = this.m_targetPos;
			if (targetPos3 != null && !this.SimpleCellValidator(this.m_targetPos.Value))
			{
				this.m_targetPos = null;
			}
			IntVector2? targetPos4 = this.m_targetPos;
			if (targetPos4 == null)
			{
				RoomHandler roomHandler = this.m_aiActor.ParentRoom;
				if (this.UseTargetsRoom && this.m_aiActor.TargetRigidbody)
				{
					PlayerController playerController = ((!this.m_aiActor.TargetRigidbody.gameActor) ? null : (this.m_aiActor.TargetRigidbody.gameActor as PlayerController));
					if (playerController)
					{
						roomHandler = playerController.CurrentRoom;
					}
				}
				this.m_targetPos = roomHandler.GetRandomAvailableCell(new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, new CellValidator(this.FullCellValidator));
			}
			IntVector2? targetPos5 = this.m_targetPos;
			if (targetPos5 == null)
			{
				return BehaviorResult.Continue;
			}
			this.m_aiActor.PathfindToPosition(this.m_targetPos.Value.ToCenterVector2(), null, true, null, null, null, false);
		}
		if (this.PreventFiringWhileMoving)
		{
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x06004B8B RID: 19339 RVA: 0x0019B340 File Offset: 0x00199540
	public void ResetPauseTimer()
	{
		this.m_pauseTimer = 0f;
	}

	// Token: 0x17000A9A RID: 2714
	// (get) Token: 0x06004B8C RID: 19340 RVA: 0x0019B350 File Offset: 0x00199550
	public override bool AllowFearRunState
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004B8D RID: 19341 RVA: 0x0019B354 File Offset: 0x00199554
	private bool SimpleCellValidator(IntVector2 c)
	{
		for (int i = 0; i < this.m_aiActor.Clearance.x; i++)
		{
			for (int j = 0; j < this.m_aiActor.Clearance.y; j++)
			{
				if (GameManager.Instance.Dungeon.data.isTopWall(c.x + i, c.y + j))
				{
					return false;
				}
			}
		}
		return !this.StayOnScreen || (c.x >= this.m_cachedCameraBottomLeft.x && c.y >= this.m_cachedCameraBottomLeft.y && c.x + this.m_aiActor.Clearance.x - 1 <= this.m_cachedCameraBottomRight.x && c.y + this.m_aiActor.Clearance.y - 1 <= this.m_cachedCameraBottomRight.y);
	}

	// Token: 0x06004B8E RID: 19342 RVA: 0x0019B470 File Offset: 0x00199670
	private bool FullCellValidator(IntVector2 c)
	{
		if (!this.SimpleCellValidator(c))
		{
			return false;
		}
		if (this.AvoidTarget && this.m_aiActor.TargetRigidbody)
		{
			float num = (Pathfinder.GetClearanceOffset(c, this.m_aiActor.Clearance) - this.m_cachedTargetPos).ToAngle();
			if (BraveMathCollege.AbsAngleBetween(num, this.m_cachedAngleFromTarget) > 90f)
			{
				return false;
			}
			Vector2? cachedOtherTargetPos = this.m_cachedOtherTargetPos;
			if (cachedOtherTargetPos != null)
			{
				float? cachedAngleFromOtherTarget = this.m_cachedAngleFromOtherTarget;
				if (cachedAngleFromOtherTarget != null)
				{
					num = (Pathfinder.GetClearanceOffset(c, this.m_aiActor.Clearance) - this.m_cachedOtherTargetPos.Value).ToAngle();
					if (BraveMathCollege.AbsAngleBetween(num, this.m_cachedAngleFromOtherTarget.Value) > 90f)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x04004136 RID: 16694
	public float PathInterval = 0.25f;

	// Token: 0x04004137 RID: 16695
	public float PointReachedPauseTime;

	// Token: 0x04004138 RID: 16696
	public bool PreventFiringWhileMoving;

	// Token: 0x04004139 RID: 16697
	public float InitialDelay;

	// Token: 0x0400413A RID: 16698
	public bool StayOnScreen = true;

	// Token: 0x0400413B RID: 16699
	public bool AvoidTarget = true;

	// Token: 0x0400413C RID: 16700
	public bool UseTargetsRoom;

	// Token: 0x0400413D RID: 16701
	private float m_repathTimer;

	// Token: 0x0400413E RID: 16702
	private float m_pauseTimer;

	// Token: 0x0400413F RID: 16703
	private IntVector2? m_targetPos;

	// Token: 0x04004140 RID: 16704
	private IntVector2 m_cachedCameraBottomLeft;

	// Token: 0x04004141 RID: 16705
	private IntVector2 m_cachedCameraBottomRight;

	// Token: 0x04004142 RID: 16706
	private float m_cachedAngleFromTarget;

	// Token: 0x04004143 RID: 16707
	private Vector2 m_cachedTargetPos;

	// Token: 0x04004144 RID: 16708
	private float? m_cachedAngleFromOtherTarget;

	// Token: 0x04004145 RID: 16709
	private Vector2? m_cachedOtherTargetPos;
}
