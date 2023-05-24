using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000DF6 RID: 3574
public class TakeCoverBehavior : MovementBehaviorBase
{
	// Token: 0x06004BB1 RID: 19377 RVA: 0x0019C874 File Offset: 0x0019AA74
	public static void ClearPerLevelData()
	{
		TakeCoverBehavior.allCover = null;
		TakeCoverBehavior.ClaimedCover.Clear();
	}

	// Token: 0x17000A9F RID: 2719
	// (get) Token: 0x06004BB2 RID: 19378 RVA: 0x0019C888 File Offset: 0x0019AA88
	private bool LastEnemyAndCantSeePlayer
	{
		get
		{
			return this.m_aiActor.ParentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) == 1 && this.m_failedLineOfSightTimer > 1f;
		}
	}

	// Token: 0x06004BB3 RID: 19379 RVA: 0x0019C8B4 File Offset: 0x0019AAB4
	public override void Start()
	{
		if (TakeCoverBehavior.allCover == null || TakeCoverBehavior.allCover.Length == 0)
		{
			TakeCoverBehavior.allCover = UnityEngine.Object.FindObjectsOfType<FlippableCover>();
		}
		this.m_cachedSpeed = this.m_aiActor.MovementSpeed;
		this.m_state = TakeCoverBehavior.CoverState.Disinterested;
		if (UnityEngine.Random.value < this.InitialCoverChance)
		{
			this.SearchForCover();
			this.m_seekTimer = this.RepeatingCoverInterval;
		}
	}

	// Token: 0x06004BB4 RID: 19380 RVA: 0x0019C91C File Offset: 0x0019AB1C
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
		base.DecrementTimer(ref this.m_coverTimer, false);
		base.DecrementTimer(ref this.m_seekTimer, false);
	}

	// Token: 0x06004BB5 RID: 19381 RVA: 0x0019C94C File Offset: 0x0019AB4C
	public override void Destroy()
	{
		base.Destroy();
		if (this.m_claimedCover != null)
		{
			TakeCoverBehavior.ClaimedCover.Remove(this.m_claimedCover);
		}
	}

	// Token: 0x06004BB6 RID: 19382 RVA: 0x0019C978 File Offset: 0x0019AB78
	public override BehaviorResult Update()
	{
		if (this.m_aiActor.TargetRigidbody == null)
		{
			return BehaviorResult.Continue;
		}
		this.m_aiShooter.OverrideAimPoint = null;
		bool flag = this.m_aiActor.CanTargetEnemies && !this.m_aiActor.CanTargetPlayers;
		if (this.m_state == TakeCoverBehavior.CoverState.Disinterested)
		{
			if (flag || this.LastEnemyAndCantSeePlayer)
			{
				return BehaviorResult.Continue;
			}
			if (this.m_seekTimer == 0f)
			{
				this.m_seekTimer = this.RepeatingCoverInterval;
				if (UnityEngine.Random.value < this.RepeatingCoverChance)
				{
					this.SearchForCover();
					if (this.m_claimedCover != null)
					{
						return BehaviorResult.SkipRemainingClassBehaviors;
					}
				}
			}
			return BehaviorResult.Continue;
		}
		else
		{
			bool flag2 = !this.m_claimedCover || this.m_claimedCover.IsBroken;
			int tableQuadrant = this.m_tableQuadrant;
			Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
			Vector2 unitCenter2 = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
			Vector2? vector = ((!flag2) ? this.CalculateCoverPosition(unitCenter2) : null);
			bool flag3 = false;
			if (this.m_claimedCover)
			{
				flag3 = Vector2.Distance(this.m_claimedCover.specRigidbody.UnitCenter, unitCenter2) >= this.MaxCoverDistanceToTarget;
			}
			bool flag4 = this.m_state == TakeCoverBehavior.CoverState.InCover && this.m_coverTimer <= 0f && this.LastEnemyAndCantSeePlayer;
			if (flag2 || vector == null || flag || this.m_aiActor.aiAnimator.FpsScale < 1f || flag3 || flag4)
			{
				this.BecomeDisinterested(tableQuadrant);
				return BehaviorResult.Continue;
			}
			if (this.m_state != TakeCoverBehavior.CoverState.PopOut)
			{
				this.m_coverPosition = vector.Value;
			}
			if (this.m_claimedCover && !this.m_claimedCover.IsFlipped && Vector2.Distance(this.m_coverPosition, unitCenter) < this.FlipCoverDistance && this.DesiredFlipDirection == this.m_claimedCover.GetFlipDirection(this.m_aiActor.specRigidbody))
			{
				this.m_claimedCover.Flip(this.m_aiActor.specRigidbody);
			}
			if (this.m_state == TakeCoverBehavior.CoverState.MovingToCover)
			{
				if (this.m_repathTimer == 0f)
				{
					if (Vector2.Distance(unitCenter, this.m_coverPosition) > PhysicsEngine.PixelToUnit(2) && !this.m_aiActor.PathfindToPosition(this.m_coverPosition, new Vector2?(this.m_coverPosition), true, null, null, null, false))
					{
						this.BecomeDisinterested(tableQuadrant);
						this.m_aiActor.ClearPath();
						return BehaviorResult.Continue;
					}
					this.m_repathTimer = this.PathInterval;
				}
				if (this.m_aiActor.PathComplete)
				{
					this.m_state = TakeCoverBehavior.CoverState.InCover;
					this.m_coverTimer = this.InsideCoverTime;
					this.m_repathTimer = 0f;
					this.m_failedLineOfSightTimer = 0f;
				}
			}
			else if (this.m_state == TakeCoverBehavior.CoverState.InCover)
			{
				if (!this.m_aiActor.HasLineOfSightToTarget)
				{
					this.m_aiShooter.OverrideAimDirection = new Vector2?(IntVector2.Cardinals[this.m_tableQuadrant / 2 * 2 + 1].ToVector2());
				}
				if (this.m_coverTimer == 0f)
				{
					this.m_popOutPosition = this.CalculatePopOutPosition(unitCenter2);
					if (!this.LineOfSightToLeaveCover || this.m_aiActor.HasLineOfSightToTargetFromPosition(this.m_popOutPosition))
					{
						this.m_state = TakeCoverBehavior.CoverState.PopOut;
						this.m_coverTimer = this.OutsideCoverTime;
						this.m_repathTimer = 0f;
						this.m_aiActor.MovementSpeed = this.m_cachedSpeed * this.PopOutSpeedMultiplier;
						this.m_aiAnimator.PlayForDuration(this.emergeAnimations[this.m_tableQuadrant], this.OutsideCoverTime, false, null, -1f, false);
						return BehaviorResult.SkipRemainingClassBehaviors;
					}
					if (this.LineOfSightToLeaveCover)
					{
						this.m_failedLineOfSightTimer += this.m_deltaTime;
					}
				}
				if (this.m_repathTimer == 0f)
				{
					if (Vector2.Distance(unitCenter, this.m_coverPosition) > PhysicsEngine.PixelToUnit(2))
					{
						bool flag5 = this.m_aiActor.PathfindToPosition(this.m_coverPosition, new Vector2?(this.m_coverPosition), true, null, null, null, false);
						this.m_aiAnimator.EndAnimationIf(this.coverAnimations[tableQuadrant]);
						if (!flag5)
						{
							this.BecomeDisinterested(tableQuadrant);
							this.m_aiActor.ClearPath();
							return BehaviorResult.Continue;
						}
					}
					this.m_repathTimer = this.PathInterval;
				}
				if (this.m_aiActor.PathComplete && !this.m_aiActor.spriteAnimator.IsPlaying(this.coverAnimations[this.m_tableQuadrant]))
				{
					this.m_aiAnimator.PlayUntilFinished(this.coverAnimations[this.m_tableQuadrant], false, null, -1f, false);
				}
			}
			else if (this.m_state == TakeCoverBehavior.CoverState.PopOut)
			{
				if (this.m_coverTimer == 0f)
				{
					this.m_state = TakeCoverBehavior.CoverState.InCover;
					this.m_coverTimer = this.InsideCoverTime;
					this.m_repathTimer = 0f;
					this.m_failedLineOfSightTimer = 0f;
					this.m_aiActor.MovementSpeed = this.m_cachedSpeed * this.PopInSpeedMultiplier;
				}
				else if (this.m_repathTimer == 0f)
				{
					Vector2 unitCenter3 = this.m_aiActor.specRigidbody.UnitCenter;
					if (Vector2.Distance(unitCenter3, this.m_popOutPosition) < 2f)
					{
						this.m_aiActor.FakePathToPosition(this.m_popOutPosition);
					}
					else if (!this.m_aiActor.PathfindToPosition(this.m_popOutPosition, new Vector2?(this.m_popOutPosition), true, null, null, null, false))
					{
						this.BecomeDisinterested(tableQuadrant);
						this.m_aiActor.ClearPath();
						return BehaviorResult.Continue;
					}
					this.m_repathTimer = this.PathInterval;
				}
			}
			return BehaviorResult.SkipRemainingClassBehaviors;
		}
	}

	// Token: 0x17000AA0 RID: 2720
	// (get) Token: 0x06004BB7 RID: 19383 RVA: 0x0019CF8C File Offset: 0x0019B18C
	private DungeonData.Direction DesiredFlipDirection
	{
		get
		{
			if (this.m_tableQuadrant == 0)
			{
				return DungeonData.Direction.SOUTH;
			}
			if (this.m_tableQuadrant == 1)
			{
				return DungeonData.Direction.WEST;
			}
			if (this.m_tableQuadrant == 2)
			{
				return DungeonData.Direction.NORTH;
			}
			if (this.m_tableQuadrant == 3)
			{
				return DungeonData.Direction.EAST;
			}
			Debug.LogError("Unknown flip direction!");
			return DungeonData.Direction.NORTH;
		}
	}

	// Token: 0x06004BB8 RID: 19384 RVA: 0x0019CFDC File Offset: 0x0019B1DC
	protected void SearchForCover()
	{
		if (this.m_claimedCover != null)
		{
			TakeCoverBehavior.ClaimedCover.Remove(this.m_claimedCover);
		}
		this.m_claimedCover = null;
		if (!this.m_aiActor.TargetRigidbody)
		{
			return;
		}
		RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(this.m_aiActor.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
		Vector2 unitCenter = this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox);
		float num = float.MaxValue;
		for (int i = 0; i < TakeCoverBehavior.allCover.Length; i++)
		{
			if (TakeCoverBehavior.allCover[i])
			{
				if (!TakeCoverBehavior.allCover[i].IsBroken)
				{
					if (!TakeCoverBehavior.ClaimedCover.Contains(TakeCoverBehavior.allCover[i]))
					{
						RoomHandler roomFromPosition2 = GameManager.Instance.Dungeon.GetRoomFromPosition(TakeCoverBehavior.allCover[i].transform.position.IntXY(VectorConversions.Floor));
						if (roomFromPosition == roomFromPosition2)
						{
							float num2 = Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, TakeCoverBehavior.allCover[i].specRigidbody.UnitCenter);
							float num3 = Vector2.Distance(TakeCoverBehavior.allCover[i].specRigidbody.UnitCenter, unitCenter);
							if (num2 < this.MaxCoverDistance && num2 < num && num3 < this.MaxCoverDistanceToTarget)
							{
								num = num2;
								this.m_claimedCover = TakeCoverBehavior.allCover[i];
							}
						}
					}
				}
			}
		}
		if (this.m_claimedCover != null)
		{
			TakeCoverBehavior.ClaimedCover.Add(this.m_claimedCover);
			this.m_repathTimer = 0f;
			this.m_state = TakeCoverBehavior.CoverState.MovingToCover;
		}
	}

	// Token: 0x06004BB9 RID: 19385 RVA: 0x0019D1A4 File Offset: 0x0019B3A4
	protected Vector2? CalculateCoverPosition(Vector2 targetPosition)
	{
		Vector2? vector = null;
		PixelCollider primaryPixelCollider = this.m_aiActor.specRigidbody.PrimaryPixelCollider;
		PixelCollider pixelCollider = this.m_aiActor.specRigidbody[CollisionLayer.EnemyHitBox];
		PixelCollider pixelCollider2 = ((!this.m_claimedCover.IsFlipped) ? this.m_claimedCover.specRigidbody.PrimaryPixelCollider : this.m_claimedCover.specRigidbody[CollisionLayer.LowObstacle]);
		Vector2 unitCenter = pixelCollider2.UnitCenter;
		Vector2 vector2 = targetPosition - unitCenter;
		Vector2 vector3 = BraveUtility.GetMajorAxis(vector2);
		int i = 0;
		while (i < 2)
		{
			Vector2 vector4;
			if (vector3.x != 0f)
			{
				if (pixelCollider2.Height >= pixelCollider.Height)
				{
					vector4 = new Vector2((vector3.x <= 0f) ? Mathf.Ceil(pixelCollider2.UnitRight) : Mathf.Floor(pixelCollider2.UnitLeft), pixelCollider2.UnitCenter.y);
					this.m_tableQuadrant = ((vector3.x <= 0f) ? 1 : 3);
					goto IL_193;
				}
			}
			else if (pixelCollider2.Width >= pixelCollider.Width)
			{
				vector4 = new Vector2(pixelCollider2.UnitCenter.x, (vector3.y <= 0f) ? Mathf.Ceil(pixelCollider2.UnitTop) : Mathf.Floor(pixelCollider2.UnitBottom));
				this.m_tableQuadrant = ((vector3.y <= 0f) ? 0 : 2);
				goto IL_193;
			}
			vector3 = BraveUtility.GetMinorAxis(vector2);
			i++;
			continue;
			IL_193:
			vector = new Vector2?(vector4 + Vector2.Scale(-vector3, primaryPixelCollider.UnitDimensions / 2f));
			break;
		}
		if (vector == null)
		{
			Debug.LogError("Didn't find a valid cover position!");
			return new Vector2?(this.m_claimedCover.transform.position.XY());
		}
		return vector;
	}

	// Token: 0x06004BBA RID: 19386 RVA: 0x0019D3C0 File Offset: 0x0019B5C0
	protected Vector2 CalculatePopOutPosition(Vector2 targetPosition)
	{
		PixelCollider primaryPixelCollider = this.m_aiActor.specRigidbody.PrimaryPixelCollider;
		PixelCollider hitboxPixelCollider = this.m_aiActor.specRigidbody.HitboxPixelCollider;
		PixelCollider pixelCollider = this.m_claimedCover.specRigidbody[CollisionLayer.BulletBlocker];
		Vector2 coverPosition = this.m_coverPosition;
		Vector2 vector = targetPosition - pixelCollider.UnitCenter;
		Vector2 vector2 = primaryPixelCollider.UnitDimensions / 2f;
		if (this.m_tableQuadrant == 0 || this.m_tableQuadrant == 2)
		{
			coverPosition.x = ((vector.x >= 0f) ? (pixelCollider.UnitRight + vector2.x) : (pixelCollider.UnitLeft - vector2.x));
		}
		else
		{
			coverPosition.y = ((vector.y >= 0f) ? (pixelCollider.UnitTop + vector2.y) : (pixelCollider.UnitBottom - hitboxPixelCollider.UnitDimensions.y + vector2.y));
		}
		return coverPosition;
	}

	// Token: 0x06004BBB RID: 19387 RVA: 0x0019D4C8 File Offset: 0x0019B6C8
	private void BecomeDisinterested(int previousTableQuadrant)
	{
		this.m_state = TakeCoverBehavior.CoverState.Disinterested;
		this.m_seekTimer = this.RepeatingCoverInterval;
		this.m_aiShooter.OverrideAimPoint = null;
		this.m_aiActor.MovementSpeed = this.m_cachedSpeed;
		this.m_aiAnimator.EndAnimationIf(this.coverAnimations[previousTableQuadrant]);
		this.m_aiAnimator.EndAnimationIf(this.emergeAnimations[previousTableQuadrant]);
	}

	// Token: 0x0400417E RID: 16766
	protected static FlippableCover[] allCover;

	// Token: 0x0400417F RID: 16767
	protected static HashSet<FlippableCover> ClaimedCover = new HashSet<FlippableCover>();

	// Token: 0x04004180 RID: 16768
	public float PathInterval = 0.25f;

	// Token: 0x04004181 RID: 16769
	public bool LineOfSightToLeaveCover = true;

	// Token: 0x04004182 RID: 16770
	public float MaxCoverDistance = 10f;

	// Token: 0x04004183 RID: 16771
	public float MaxCoverDistanceToTarget = 10f;

	// Token: 0x04004184 RID: 16772
	public float FlipCoverDistance = 1f;

	// Token: 0x04004185 RID: 16773
	public float InsideCoverTime = 2f;

	// Token: 0x04004186 RID: 16774
	public float OutsideCoverTime = 1f;

	// Token: 0x04004187 RID: 16775
	public float PopOutSpeedMultiplier = 1f;

	// Token: 0x04004188 RID: 16776
	public float PopInSpeedMultiplier = 1f;

	// Token: 0x04004189 RID: 16777
	public float InitialCoverChance = 0.33f;

	// Token: 0x0400418A RID: 16778
	public float RepeatingCoverChance = 0.05f;

	// Token: 0x0400418B RID: 16779
	public float RepeatingCoverInterval = 1f;

	// Token: 0x0400418C RID: 16780
	private TakeCoverBehavior.CoverState m_state;

	// Token: 0x0400418D RID: 16781
	private int m_tableQuadrant;

	// Token: 0x0400418E RID: 16782
	private float m_repathTimer;

	// Token: 0x0400418F RID: 16783
	private float m_coverTimer;

	// Token: 0x04004190 RID: 16784
	private float m_seekTimer;

	// Token: 0x04004191 RID: 16785
	private float m_failedLineOfSightTimer;

	// Token: 0x04004192 RID: 16786
	private float m_cachedSpeed;

	// Token: 0x04004193 RID: 16787
	private FlippableCover m_claimedCover;

	// Token: 0x04004194 RID: 16788
	private Vector2 m_coverPosition;

	// Token: 0x04004195 RID: 16789
	private Vector2 m_popOutPosition;

	// Token: 0x04004196 RID: 16790
	private string[] coverAnimations = new string[] { "cover_idle_right", "cover_idle_right", "cover_idle_left", "cover_idle_left" };

	// Token: 0x04004197 RID: 16791
	private string[] emergeAnimations = new string[] { "cover_leap_right", "cover_leap_right", "cover_leap_left", "cover_leap_left" };

	// Token: 0x02000DF7 RID: 3575
	private enum CoverState
	{
		// Token: 0x04004199 RID: 16793
		Disinterested,
		// Token: 0x0400419A RID: 16794
		MovingToCover,
		// Token: 0x0400419B RID: 16795
		InCover,
		// Token: 0x0400419C RID: 16796
		PopOut
	}
}
