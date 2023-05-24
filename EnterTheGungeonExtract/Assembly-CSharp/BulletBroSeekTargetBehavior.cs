using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DDB RID: 3547
[InspectorDropdownName("Bosses/BulletBro/SeekTargetBehavior")]
public class BulletBroSeekTargetBehavior : MovementBehaviorBase
{
	// Token: 0x06004B23 RID: 19235 RVA: 0x00196640 File Offset: 0x00194840
	public override void Start()
	{
		base.Start();
		BroController otherBro = BroController.GetOtherBro(this.m_aiActor.gameObject);
		if (otherBro)
		{
			this.m_otherBro = otherBro.aiActor;
		}
	}

	// Token: 0x06004B24 RID: 19236 RVA: 0x0019667C File Offset: 0x0019487C
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
	}

	// Token: 0x06004B25 RID: 19237 RVA: 0x00196694 File Offset: 0x00194894
	public override BehaviorResult Update()
	{
		SpeculativeRigidbody targetRigidbody = this.m_aiActor.TargetRigidbody;
		if (!(targetRigidbody != null))
		{
			return BehaviorResult.Continue;
		}
		float desiredCombatDistance = this.m_aiActor.DesiredCombatDistance;
		if (this.StopWhenInRange && this.m_aiActor.DistanceToTarget <= desiredCombatDistance)
		{
			this.m_aiActor.ClearPath();
			return BehaviorResult.Continue;
		}
		if (this.m_repathTimer <= 0f)
		{
			Vector2 vector;
			if (!this.m_otherBro)
			{
				vector = targetRigidbody.UnitCenter;
			}
			else
			{
				Vector2 unitCenter = this.m_aiActor.TargetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox);
				Vector2 unitCenter2 = this.m_aiActor.specRigidbody.UnitCenter;
				Vector2 unitCenter3 = this.m_otherBro.specRigidbody.UnitCenter;
				float num = (unitCenter2 - unitCenter).ToAngle();
				float num2 = (unitCenter3 - unitCenter).ToAngle();
				float num3 = (num + num2) / 2f;
				float num4;
				if (BraveMathCollege.ClampAngle180(num - num3) > 0f)
				{
					num4 = num3 + 90f;
				}
				else
				{
					num4 = num3 - 90f;
				}
				vector = unitCenter + BraveMathCollege.DegreesToVector(num4, 1f) * this.DesiredCombatDistance;
			}
			this.m_aiActor.PathfindToPosition(vector, null, true, null, null, null, false);
			this.m_repathTimer = this.PathInterval;
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x17000A98 RID: 2712
	// (get) Token: 0x06004B26 RID: 19238 RVA: 0x00196800 File Offset: 0x00194A00
	public override float DesiredCombatDistance
	{
		get
		{
			return this.CustomRange;
		}
	}

	// Token: 0x04004082 RID: 16514
	public bool StopWhenInRange = true;

	// Token: 0x04004083 RID: 16515
	public float CustomRange = -1f;

	// Token: 0x04004084 RID: 16516
	public float PathInterval = 0.25f;

	// Token: 0x04004085 RID: 16517
	private float m_repathTimer;

	// Token: 0x04004086 RID: 16518
	private AIActor m_otherBro;
}
