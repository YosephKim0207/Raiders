using System;
using Dungeonator;
using FullInspector;
using Pathfinding;
using UnityEngine;

// Token: 0x02000DEA RID: 3562
public class FleeTargetBehavior : MovementBehaviorBase
{
	// Token: 0x06004B71 RID: 19313 RVA: 0x001998A4 File Offset: 0x00197AA4
	public override void Start()
	{
		if (this.m_aiActor && this.m_aiActor.healthHaver)
		{
			this.m_aiActor.healthHaver.OnDamaged += this.OnDamaged;
		}
	}

	// Token: 0x06004B72 RID: 19314 RVA: 0x001998F4 File Offset: 0x00197AF4
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
		base.DecrementTimer(ref this.m_closeTimer, false);
		if (this.m_aiActor.DistanceToTarget > this.CloseDistance)
		{
			this.m_closeTimer = this.CloseTime;
		}
		this.m_shouldRun = false;
		if (this.m_wasDamaged)
		{
			this.m_shouldRun = true;
			this.m_wasDamaged = false;
		}
		this.m_otherTargetRigidbody = null;
		if (this.m_aiActor.PlayerTarget is PlayerController && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this.m_aiActor.PlayerTarget as PlayerController);
			if (otherPlayer && otherPlayer.healthHaver && otherPlayer.healthHaver.IsAlive)
			{
				this.m_otherTargetRigidbody = otherPlayer.specRigidbody;
			}
		}
	}

	// Token: 0x06004B73 RID: 19315 RVA: 0x001999E4 File Offset: 0x00197BE4
	public override bool OverrideOtherBehaviors()
	{
		return this.ShouldRun();
	}

	// Token: 0x06004B74 RID: 19316 RVA: 0x001999EC File Offset: 0x00197BEC
	public override BehaviorResult Update()
	{
		IntVector2? targetPos = this.m_targetPos;
		if (targetPos == null && this.m_repathTimer > 0f)
		{
			return BehaviorResult.Continue;
		}
		IntVector2? targetPos2 = this.m_targetPos;
		if (targetPos2 != null && this.m_aiActor.PathComplete)
		{
			this.m_targetPos = null;
		}
		IntVector2? targetPos3 = this.m_targetPos;
		if (targetPos3 == null && this.m_aiActor.TargetRigidbody && this.ShouldRun() && this.m_aiActor.ParentRoom != null)
		{
			RoomHandler parentRoom = this.m_aiActor.ParentRoom;
			this.m_targetPos = parentRoom.GetRandomAvailableCell(new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, new CellValidator(this.CellValidator));
			IntVector2? targetPos4 = this.m_targetPos;
			if (targetPos4 == null)
			{
				this.m_targetPos = parentRoom.GetRandomWeightedAvailableCell(new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(this.m_aiActor.PathableTiles), false, new CellValidator(this.CellValidator), new Func<IntVector2, float>(this.CellWeighter), 1f);
			}
			this.m_repathTimer = 0f;
			this.m_closeTimer = 0f;
			this.ForceRun = false;
		}
		if (this.m_repathTimer <= 0f)
		{
			IntVector2? targetPos5 = this.m_targetPos;
			if (targetPos5 != null && this.m_aiActor.TargetRigidbody)
			{
				this.m_repathTimer = this.PathInterval;
				this.m_cachedPlayerCell = this.m_aiActor.TargetRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
				this.m_cachedOtherPlayerCell = ((!this.m_otherTargetRigidbody) ? null : new IntVector2?(this.m_otherTargetRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor)));
				this.m_aiActor.PathfindToPosition(this.m_targetPos.Value.ToCenterVector2(), null, true, null, new ExtraWeightingFunction(this.CellPathingWeighter), null, false);
			}
		}
		IntVector2? targetPos6 = this.m_targetPos;
		if (targetPos6 == null)
		{
			return BehaviorResult.Continue;
		}
		return (!this.CanAttackWhileMoving) ? BehaviorResult.SkipAllRemainingBehaviors : BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x06004B75 RID: 19317 RVA: 0x00199C68 File Offset: 0x00197E68
	private bool CellValidator(IntVector2 c)
	{
		if (this.ManuallyDefineRoom && ((float)c.x < this.roomMin.x || (float)c.x > this.roomMax.x || (float)c.y < this.roomMin.y || (float)c.y > this.roomMax.y))
		{
			return false;
		}
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
		return Vector2.Distance(Pathfinder.GetClearanceOffset(c, this.m_aiActor.Clearance), this.m_aiActor.TargetRigidbody.UnitCenter) >= this.DesiredDistance && (!this.m_otherTargetRigidbody || Vector2.Distance(Pathfinder.GetClearanceOffset(c, this.m_aiActor.Clearance), this.m_otherTargetRigidbody.UnitCenter) >= this.DesiredDistance);
	}

	// Token: 0x06004B76 RID: 19318 RVA: 0x00199DCC File Offset: 0x00197FCC
	private float CellWeighter(IntVector2 c)
	{
		for (int i = 0; i < this.m_aiActor.Clearance.x; i++)
		{
			for (int j = 0; j < this.m_aiActor.Clearance.y; j++)
			{
				if (GameManager.Instance.Dungeon.data.isTopWall(c.x + i, c.y + j))
				{
					return 1000000f;
				}
			}
		}
		float num = Vector2.Distance(Pathfinder.GetClearanceOffset(c, this.m_aiActor.Clearance), this.m_aiActor.TargetRigidbody.UnitCenter);
		if (this.m_otherTargetRigidbody)
		{
			num = Mathf.Min(num, Vector2.Distance(Pathfinder.GetClearanceOffset(c, this.m_aiActor.Clearance), this.m_otherTargetRigidbody.UnitCenter));
		}
		return num;
	}

	// Token: 0x06004B77 RID: 19319 RVA: 0x00199EB8 File Offset: 0x001980B8
	private int CellPathingWeighter(IntVector2 prevStep, IntVector2 thisStep)
	{
		if (IntVector2.Distance(thisStep, this.m_cachedPlayerCell) < (float)this.PlayerPersonalSpace)
		{
			return 100;
		}
		IntVector2? cachedOtherPlayerCell = this.m_cachedOtherPlayerCell;
		if (cachedOtherPlayerCell != null && IntVector2.Distance(thisStep, this.m_cachedOtherPlayerCell.Value) < (float)this.PlayerPersonalSpace)
		{
			return 100;
		}
		return 0;
	}

	// Token: 0x06004B78 RID: 19320 RVA: 0x00199F14 File Offset: 0x00198114
	private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		this.m_wasDamaged = true;
	}

	// Token: 0x06004B79 RID: 19321 RVA: 0x00199F20 File Offset: 0x00198120
	private bool ShouldRun()
	{
		if (this.m_shouldRun || this.ForceRun)
		{
			return true;
		}
		float num = this.m_aiActor.DistanceToTarget;
		if (this.m_aiActor.PlayerTarget is PlayerController && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(this.m_aiActor.PlayerTarget as PlayerController);
			if (otherPlayer && otherPlayer.healthHaver && otherPlayer.healthHaver.IsAlive)
			{
				float num2 = Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, otherPlayer.specRigidbody.GetUnitCenter(ColliderType.HitBox));
				num = Mathf.Min(num, num2);
			}
		}
		return (num < this.TooCloseDistance && (!this.TooCloseLOS || this.m_aiActor.HasLineOfSightToTarget)) || (num < this.CloseDistance && this.m_closeTimer <= 0f);
	}

	// Token: 0x04004109 RID: 16649
	public float PathInterval = 0.25f;

	// Token: 0x0400410A RID: 16650
	public float CloseDistance = 9f;

	// Token: 0x0400410B RID: 16651
	public float CloseTime = 3f;

	// Token: 0x0400410C RID: 16652
	public float TooCloseDistance = 6f;

	// Token: 0x0400410D RID: 16653
	public bool TooCloseLOS = true;

	// Token: 0x0400410E RID: 16654
	public float DesiredDistance = 20f;

	// Token: 0x0400410F RID: 16655
	public int PlayerPersonalSpace;

	// Token: 0x04004110 RID: 16656
	public bool CanAttackWhileMoving;

	// Token: 0x04004111 RID: 16657
	public bool ManuallyDefineRoom;

	// Token: 0x04004112 RID: 16658
	[InspectorShowIf("ManuallyDefineRoom")]
	public Vector2 roomMin;

	// Token: 0x04004113 RID: 16659
	[InspectorShowIf("ManuallyDefineRoom")]
	public Vector2 roomMax;

	// Token: 0x04004114 RID: 16660
	[NonSerialized]
	public bool ForceRun;

	// Token: 0x04004115 RID: 16661
	private float m_repathTimer;

	// Token: 0x04004116 RID: 16662
	private float m_closeTimer;

	// Token: 0x04004117 RID: 16663
	private bool m_wasDamaged;

	// Token: 0x04004118 RID: 16664
	private bool m_shouldRun;

	// Token: 0x04004119 RID: 16665
	private SpeculativeRigidbody m_otherTargetRigidbody;

	// Token: 0x0400411A RID: 16666
	private IntVector2? m_targetPos;

	// Token: 0x0400411B RID: 16667
	private IntVector2 m_cachedPlayerCell;

	// Token: 0x0400411C RID: 16668
	private IntVector2? m_cachedOtherPlayerCell;
}
