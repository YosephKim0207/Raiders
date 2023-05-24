using System;
using Dungeonator;
using FullInspector;
using Pathfinding;
using UnityEngine;

// Token: 0x02000D86 RID: 3462
[InspectorDropdownName("Bosses/BossFinalBullet/AgunimMoveBehavior")]
public class BossFinalBulletAgunimMoveBehavior : BasicAttackBehavior
{
	// Token: 0x0600494B RID: 18763 RVA: 0x00187230 File Offset: 0x00185430
	public override void Start()
	{
		base.Start();
		this.m_shadowTrail = this.m_aiActor.GetComponent<AfterImageTrailController>();
	}

	// Token: 0x0600494C RID: 18764 RVA: 0x0018724C File Offset: 0x0018544C
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		this.m_aiActor.ClearPath();
		this.m_aiActor.BehaviorOverridesVelocity = true;
		this.m_aiActor.BehaviorVelocity = Vector2.zero;
		this.m_aiAnimator.LockFacingDirection = true;
		this.m_aiAnimator.FacingDirection = -90f;
		if (!string.IsNullOrEmpty(this.preMoveAnimation))
		{
			this.State = BossFinalBulletAgunimMoveBehavior.MoveState.PreMove;
		}
		else
		{
			this.State = BossFinalBulletAgunimMoveBehavior.MoveState.Move;
		}
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuous;
	}

	// Token: 0x0600494D RID: 18765 RVA: 0x001872FC File Offset: 0x001854FC
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		if (this.State == BossFinalBulletAgunimMoveBehavior.MoveState.PreMove)
		{
			if (!this.m_aiAnimator.IsPlaying(this.preMoveAnimation))
			{
				this.State = BossFinalBulletAgunimMoveBehavior.MoveState.Move;
				return ContinuousBehaviorResult.Continue;
			}
		}
		else if (this.State == BossFinalBulletAgunimMoveBehavior.MoveState.Move)
		{
			if (this.m_setupTimer > this.m_moveTime)
			{
				this.m_aiActor.BehaviorVelocity = Vector2.zero;
				if (!string.IsNullOrEmpty(this.postMoveAnimation))
				{
					this.State = BossFinalBulletAgunimMoveBehavior.MoveState.PostMove;
					return ContinuousBehaviorResult.Continue;
				}
				return ContinuousBehaviorResult.Finished;
			}
			else if (this.m_deltaTime > 0f)
			{
				Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
				Vector2 vector = Vector2Extensions.SmoothStep(this.m_startPoint, this.m_targetPoint, this.m_setupTimer / this.m_moveTime);
				this.m_aiActor.BehaviorVelocity = (vector - unitCenter) / this.m_deltaTime;
				this.m_setupTimer += this.m_deltaTime;
			}
		}
		else if (this.State == BossFinalBulletAgunimMoveBehavior.MoveState.PostMove && !this.m_aiAnimator.IsPlaying(this.postMoveAnimation))
		{
			return ContinuousBehaviorResult.Finished;
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x0600494E RID: 18766 RVA: 0x00187424 File Offset: 0x00185624
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.DisableCollisionDuringMove)
		{
			this.m_aiActor.specRigidbody.CollideWithOthers = true;
			this.m_aiActor.IsGone = false;
		}
		this.State = BossFinalBulletAgunimMoveBehavior.MoveState.None;
		if (!string.IsNullOrEmpty(this.preMoveAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.preMoveAnimation);
		}
		if (!string.IsNullOrEmpty(this.moveAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.moveAnimation);
		}
		if (!string.IsNullOrEmpty(this.postMoveAnimation))
		{
			this.m_aiAnimator.EndAnimationIf(this.postMoveAnimation);
		}
		this.m_aiAnimator.LockFacingDirection = false;
		this.m_aiActor.BehaviorOverridesVelocity = false;
		this.m_updateEveryFrame = false;
		this.UpdateCooldowns();
	}

	// Token: 0x0600494F RID: 18767 RVA: 0x001874F4 File Offset: 0x001856F4
	private void UpdateTargetPoint()
	{
		float minDistanceFromPlayerSquared = (float)(this.MinDistanceFromPlayer * this.MinDistanceFromPlayer);
		bool hasOtherPlayer = false;
		Vector2 playerLowerLeft = this.m_aiActor.TargetRigidbody.HitboxPixelCollider.UnitBottomLeft;
		Vector2 playerUpperRight = this.m_aiActor.TargetRigidbody.HitboxPixelCollider.UnitTopRight;
		Vector2 otherPlayerLowerLeft = Vector2.zero;
		Vector2 otherPlayerUpperRight = Vector2.zero;
		float maxPlayerY = playerLowerLeft.y;
		float minPlayerY = playerLowerLeft.y;
		PlayerController playerController = this.m_behaviorSpeculator.PlayerTarget as PlayerController;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && playerController)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(playerController);
			if (otherPlayer && otherPlayer.healthHaver.IsAlive)
			{
				hasOtherPlayer = true;
				otherPlayerLowerLeft = otherPlayer.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
				otherPlayerUpperRight = otherPlayer.specRigidbody.HitboxPixelCollider.UnitTopRight;
				maxPlayerY = Mathf.Max(maxPlayerY, otherPlayerLowerLeft.y);
				minPlayerY = Mathf.Min(minPlayerY, otherPlayerLowerLeft.y);
			}
		}
		int minDx = -this.MinDistFromHorizontalWall;
		int maxDx = this.MinDistFromHorizontalWall + this.m_aiActor.Clearance.x - 2;
		float roomMinY = this.m_aiActor.ParentRoom.area.UnitBottomLeft.y;
		float roomMaxY = this.m_aiActor.ParentRoom.area.UnitTopRight.y;
		int minTilesAbovePlayer = this.MinTilesAbovePlayer;
		int minTilesBelowPlayer = this.MinTilesBelowPlayer;
		CellValidator cellValidator = delegate(IntVector2 c)
		{
			for (int i = 0; i < this.m_aiActor.Clearance.x; i++)
			{
				int num = c.x + i;
				for (int j = 0; j < this.m_aiActor.Clearance.y; j++)
				{
					int num2 = c.y + j;
					if (GameManager.Instance.Dungeon.data.isTopWall(num, num2))
					{
						return false;
					}
				}
			}
			float num3 = (float)c.y - maxPlayerY;
			float num4 = minPlayerY - (float)c.y;
			bool flag = num3 >= (float)minTilesAbovePlayer && num3 <= (float)this.MaxTilesAbovePlayer;
			bool flag2 = this.UseSouthWall && num4 >= (float)minTilesBelowPlayer && num3 <= (float)this.MaxTilesBelowPlayer;
			if (!flag && !flag2)
			{
				return false;
			}
			if (this.MinDistanceFromPlayer > 0)
			{
				PixelCollider hitboxPixelCollider = this.m_aiActor.specRigidbody.HitboxPixelCollider;
				Vector2 vector = new Vector2((float)c.x + 0.5f * ((float)this.m_aiActor.Clearance.x - hitboxPixelCollider.UnitWidth), (float)c.y);
				Vector2 vector2 = vector + hitboxPixelCollider.UnitDimensions;
				if (this.MinDistanceFromPlayer > 0)
				{
					if (BraveMathCollege.AABBDistanceSquared(vector, vector2, playerLowerLeft, playerUpperRight) < minDistanceFromPlayerSquared)
					{
						return false;
					}
					if (hasOtherPlayer && BraveMathCollege.AABBDistanceSquared(vector, vector2, otherPlayerLowerLeft, otherPlayerUpperRight) < minDistanceFromPlayerSquared)
					{
						return false;
					}
				}
			}
			for (int k = minDx; k <= maxDx; k++)
			{
				if (GameManager.Instance.Dungeon.data.isWall(c.x + k, c.y))
				{
					return false;
				}
			}
			return roomMaxY - (float)c.y >= (float)(this.MinDistFromNorthWall + 1) && (!this.UseSouthWall || (float)c.y - roomMinY >= (float)this.MinDistFromSouthWall);
		};
		IntVector2? intVector = this.m_aiActor.ParentRoom.GetRandomAvailableCell(new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, cellValidator);
		if (intVector == null)
		{
			minTilesAbovePlayer = 0;
			minTilesBelowPlayer = 0;
			intVector = this.m_aiActor.ParentRoom.GetRandomAvailableCell(new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, cellValidator);
		}
		if (intVector != null)
		{
			this.m_targetPoint = intVector.Value.ToCenterVector2();
		}
		else
		{
			Debug.LogWarning("AGUNIM MOVE FAILED!", this.m_aiActor);
			this.m_targetPoint = this.m_aiActor.specRigidbody.UnitCenter;
		}
	}

	// Token: 0x17000A8B RID: 2699
	// (get) Token: 0x06004950 RID: 18768 RVA: 0x001877C4 File Offset: 0x001859C4
	// (set) Token: 0x06004951 RID: 18769 RVA: 0x001877CC File Offset: 0x001859CC
	private BossFinalBulletAgunimMoveBehavior.MoveState State
	{
		get
		{
			return this.m_state;
		}
		set
		{
			if (this.m_state != value)
			{
				this.EndState(this.m_state);
				this.m_state = value;
				this.BeginState(this.m_state);
			}
		}
	}

	// Token: 0x06004952 RID: 18770 RVA: 0x001877FC File Offset: 0x001859FC
	private void BeginState(BossFinalBulletAgunimMoveBehavior.MoveState state)
	{
		if (state == BossFinalBulletAgunimMoveBehavior.MoveState.PreMove)
		{
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_aiAnimator.PlayUntilCancelled(this.preMoveAnimation, false, null, -1f, false);
			if (this.DisableCollisionDuringMove)
			{
				this.m_aiActor.specRigidbody.CollideWithOthers = false;
				this.m_aiActor.IsGone = true;
			}
		}
		else if (state == BossFinalBulletAgunimMoveBehavior.MoveState.Move)
		{
			this.m_startPoint = this.m_aiActor.specRigidbody.UnitCenter;
			this.UpdateTargetPoint();
			Vector2 vector = this.m_targetPoint - this.m_startPoint;
			float magnitude = vector.magnitude;
			this.m_moveTime = this.MoveTime;
			if (this.MinSpeed > 0f)
			{
				this.m_moveTime = Mathf.Min(this.m_moveTime, magnitude / this.MinSpeed);
			}
			if (this.MaxSpeed > 0f)
			{
				this.m_moveTime = Mathf.Max(this.m_moveTime, magnitude / this.MaxSpeed);
			}
			this.m_aiAnimator.FacingDirection = vector.ToAngle();
			this.m_aiAnimator.LockFacingDirection = true;
			this.m_aiAnimator.PlayUntilCancelled(this.moveAnimation, false, null, -1f, false);
			this.m_setupTimer = 0f;
			if (this.DisableCollisionDuringMove)
			{
				this.m_aiActor.specRigidbody.CollideWithOthers = false;
				this.m_aiActor.IsGone = true;
			}
			if (this.enableShadowTrail)
			{
				this.m_shadowTrail.spawnShadows = true;
			}
		}
		else if (state == BossFinalBulletAgunimMoveBehavior.MoveState.PostMove)
		{
			this.m_aiActor.ClearPath();
			this.m_aiActor.BehaviorOverridesVelocity = true;
			this.m_aiActor.BehaviorVelocity = Vector2.zero;
			this.m_aiAnimator.PlayUntilCancelled(this.postMoveAnimation, false, null, -1f, false);
			if (this.DisableCollisionDuringMove)
			{
				this.m_aiActor.specRigidbody.CollideWithOthers = true;
				this.m_aiActor.IsGone = false;
			}
		}
	}

	// Token: 0x06004953 RID: 18771 RVA: 0x00187A10 File Offset: 0x00185C10
	private void EndState(BossFinalBulletAgunimMoveBehavior.MoveState state)
	{
		if (state == BossFinalBulletAgunimMoveBehavior.MoveState.Move && this.enableShadowTrail)
		{
			this.m_shadowTrail.spawnShadows = false;
		}
	}

	// Token: 0x04003D86 RID: 15750
	public float MoveTime = 1f;

	// Token: 0x04003D87 RID: 15751
	public float MinSpeed;

	// Token: 0x04003D88 RID: 15752
	public float MaxSpeed;

	// Token: 0x04003D89 RID: 15753
	public bool DisableCollisionDuringMove;

	// Token: 0x04003D8A RID: 15754
	public int MinDistFromHorizontalWall = 4;

	// Token: 0x04003D8B RID: 15755
	public int MinDistFromNorthWall = 2;

	// Token: 0x04003D8C RID: 15756
	public int MinTilesAbovePlayer = 4;

	// Token: 0x04003D8D RID: 15757
	public int MaxTilesAbovePlayer = 8;

	// Token: 0x04003D8E RID: 15758
	public int MinDistanceFromPlayer = 4;

	// Token: 0x04003D8F RID: 15759
	public bool UseSouthWall;

	// Token: 0x04003D90 RID: 15760
	[InspectorIndent]
	[InspectorShowIf("UseSouthWall")]
	public int MinTilesBelowPlayer = 4;

	// Token: 0x04003D91 RID: 15761
	[InspectorShowIf("UseSouthWall")]
	[InspectorIndent]
	public int MaxTilesBelowPlayer = 4;

	// Token: 0x04003D92 RID: 15762
	[InspectorShowIf("UseSouthWall")]
	[InspectorIndent]
	public int MinDistFromSouthWall = 2;

	// Token: 0x04003D93 RID: 15763
	[InspectorCategory("Visuals")]
	public string preMoveAnimation;

	// Token: 0x04003D94 RID: 15764
	[InspectorCategory("Visuals")]
	public string moveAnimation;

	// Token: 0x04003D95 RID: 15765
	[InspectorCategory("Visuals")]
	public string postMoveAnimation;

	// Token: 0x04003D96 RID: 15766
	[InspectorCategory("Visuals")]
	public bool enableShadowTrail;

	// Token: 0x04003D97 RID: 15767
	private BossFinalBulletAgunimMoveBehavior.MoveState m_state;

	// Token: 0x04003D98 RID: 15768
	private Vector2 m_startPoint;

	// Token: 0x04003D99 RID: 15769
	private Vector2 m_targetPoint;

	// Token: 0x04003D9A RID: 15770
	private float m_moveTime;

	// Token: 0x04003D9B RID: 15771
	private float m_setupTimer;

	// Token: 0x04003D9C RID: 15772
	private AfterImageTrailController m_shadowTrail;

	// Token: 0x02000D87 RID: 3463
	private enum MoveState
	{
		// Token: 0x04003D9E RID: 15774
		None,
		// Token: 0x04003D9F RID: 15775
		PreMove,
		// Token: 0x04003DA0 RID: 15776
		Move,
		// Token: 0x04003DA1 RID: 15777
		PostMove
	}
}
