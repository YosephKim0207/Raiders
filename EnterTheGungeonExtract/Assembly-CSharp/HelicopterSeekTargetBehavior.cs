using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000DDE RID: 3550
[InspectorDropdownName("Bosses/Helicopter/SeekTargetBehavior")]
public class HelicopterSeekTargetBehavior : MovementBehaviorBase
{
	// Token: 0x06004B2F RID: 19247 RVA: 0x00196BE0 File Offset: 0x00194DE0
	public override void Start()
	{
		base.Start();
		this.m_updateEveryFrame = true;
	}

	// Token: 0x06004B30 RID: 19248 RVA: 0x00196BF0 File Offset: 0x00194DF0
	public override void Upkeep()
	{
		base.Upkeep();
		this.m_aiActor.OverridePathVelocity = null;
	}

	// Token: 0x06004B31 RID: 19249 RVA: 0x00196C18 File Offset: 0x00194E18
	public override BehaviorResult Update()
	{
		this.m_timer += this.m_deltaTime;
		Vector2 unitBottomLeft = this.m_aiActor.ParentRoom.area.UnitBottomLeft;
		float num = 0f;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (playerController.healthHaver.IsAlive)
			{
				num = Mathf.Max(num, playerController.specRigidbody.UnitCenter.y);
			}
		}
		num = Mathf.Max(0f, num - unitBottomLeft.y);
		Vector2 vector = unitBottomLeft + new Vector2(Mathf.SmoothStep(this.minPoint.x, this.maxPoint.x, Mathf.PingPong(this.m_timer, this.period.x) / this.period.x), Mathf.SmoothStep(this.minPoint.y, this.maxPoint.y, Mathf.PingPong(this.m_timer, this.period.y) / this.period.y) + num);
		Vector2 unitCenter = this.m_aiActor.specRigidbody.UnitCenter;
		Vector2 vector2 = vector - unitCenter;
		Vector2 vector3;
		if (this.m_deltaTime > 0f && vector2.magnitude > 0f)
		{
			vector3 = vector2 / BraveTime.DeltaTime;
			if (this.MaxSpeed >= 0f && vector3.magnitude > this.MaxSpeed)
			{
				vector3 = this.MaxSpeed * vector3.normalized;
			}
		}
		else
		{
			vector3 = Vector2.zero;
		}
		this.m_isMoving = true;
		this.m_aiActor.OverridePathVelocity = new Vector2?(vector3);
		return BehaviorResult.Continue;
	}

	// Token: 0x04004096 RID: 16534
	public Vector2 minPoint;

	// Token: 0x04004097 RID: 16535
	public Vector2 maxPoint;

	// Token: 0x04004098 RID: 16536
	public Vector2 period;

	// Token: 0x04004099 RID: 16537
	public float MaxSpeed = 6f;

	// Token: 0x0400409A RID: 16538
	private float m_timer;

	// Token: 0x0400409B RID: 16539
	private bool m_isMoving;
}
