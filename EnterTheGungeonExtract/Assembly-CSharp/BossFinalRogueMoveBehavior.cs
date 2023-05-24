using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DDA RID: 3546
[InspectorDropdownName("Bosses/BossFinalRogue/MoveBehavior")]
public class BossFinalRogueMoveBehavior : MovementBehaviorBase
{
	// Token: 0x06004B1E RID: 19230 RVA: 0x00196378 File Offset: 0x00194578
	public override void Start()
	{
		base.Start();
		this.m_updateEveryFrame = true;
	}

	// Token: 0x06004B1F RID: 19231 RVA: 0x00196388 File Offset: 0x00194588
	public override void Upkeep()
	{
		base.Upkeep();
		this.m_aiActor.BehaviorOverridesVelocity = true;
	}

	// Token: 0x06004B20 RID: 19232 RVA: 0x0019639C File Offset: 0x0019459C
	public override BehaviorResult Update()
	{
		if (!this.m_aiActor.TargetRigidbody)
		{
			return BehaviorResult.Continue;
		}
		if (this.m_centerX == null)
		{
			this.m_centerX = new float?(this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitCenter.x);
		}
		Vector2 unitCenter = this.m_aiActor.TargetRigidbody.UnitCenter;
		Vector2 zero = Vector2.zero;
		if (this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitCenter.x < this.m_centerX.Value - 2f)
		{
			zero.x = 1f;
		}
		else if (this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitCenter.x > this.m_centerX.Value + 2f)
		{
			zero.x = -1f;
		}
		float num = this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitBottom - unitCenter.y;
		bool flag = false;
		if (num < -1.5f)
		{
			if (unitCenter.x < this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitLeft)
			{
				flag = true;
				zero.x = -1f;
			}
			else if (unitCenter.x > this.m_aiActor.specRigidbody.HitboxPixelCollider.UnitRight)
			{
				flag = true;
				zero.x = 1f;
			}
		}
		this.m_aiActor.BehaviorVelocity.x = this.RamMoveTowards(this.m_aiActor.BehaviorVelocity.x, zero.x * this.maxMoveSpeed.x, this.moveAcceleration.x * this.m_deltaTime, flag);
		this.m_aiActor.BehaviorVelocity.y = 0f;
		return BehaviorResult.Continue;
	}

	// Token: 0x06004B21 RID: 19233 RVA: 0x0019658C File Offset: 0x0019478C
	private float RamMoveTowards(float current, float target, float maxDelta, bool useRamingSpeed)
	{
		float num = target;
		float num2 = maxDelta;
		if (useRamingSpeed)
		{
			num = target * this.ramMultiplier;
			num2 = maxDelta * this.ramMultiplier;
		}
		if ((num < 0f && (current < num || current >= 0f)) || (num > 0f && (current > num || current <= 0f)))
		{
			num2 = maxDelta * this.ramMultiplier;
		}
		if (Mathf.Abs(num - current) <= num2)
		{
			return num;
		}
		return current + Mathf.Sign(num - current) * num2;
	}

	// Token: 0x04004079 RID: 16505
	public Vector2 maxMoveSpeed = new Vector2(3f, 0f);

	// Token: 0x0400407A RID: 16506
	public Vector2 moveAcceleration = new Vector2(2f, 0f);

	// Token: 0x0400407B RID: 16507
	public float ramMultiplier = 3f;

	// Token: 0x0400407C RID: 16508
	public float minPlayerDist = 5f;

	// Token: 0x0400407D RID: 16509
	public float maxPlayerDist = 12f;

	// Token: 0x0400407E RID: 16510
	public float minYHeight;

	// Token: 0x0400407F RID: 16511
	public float maxYHeight;

	// Token: 0x04004080 RID: 16512
	private Vector2 m_targetCenter;

	// Token: 0x04004081 RID: 16513
	private float? m_centerX;
}
