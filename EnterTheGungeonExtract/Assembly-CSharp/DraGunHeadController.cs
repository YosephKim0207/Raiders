using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200101C RID: 4124
public class DraGunHeadController : BraveBehaviour
{
	// Token: 0x17000D1E RID: 3358
	// (get) Token: 0x06005A69 RID: 23145 RVA: 0x00229010 File Offset: 0x00227210
	// (set) Token: 0x06005A6A RID: 23146 RVA: 0x00229018 File Offset: 0x00227218
	public float? TargetX { get; set; }

	// Token: 0x17000D1F RID: 3359
	// (get) Token: 0x06005A6B RID: 23147 RVA: 0x00229024 File Offset: 0x00227224
	// (set) Token: 0x06005A6C RID: 23148 RVA: 0x0022902C File Offset: 0x0022722C
	public Vector2? OverrideDesiredPosition { get; set; }

	// Token: 0x17000D20 RID: 3360
	// (get) Token: 0x06005A6D RID: 23149 RVA: 0x00229038 File Offset: 0x00227238
	public bool ReachedOverridePosition
	{
		get
		{
			return this.OverrideDesiredPosition != null && Vector2.Distance(base.transform.position.XY(), this.OverrideDesiredPosition.Value) < 0.5f;
		}
	}

	// Token: 0x06005A6E RID: 23150 RVA: 0x00229088 File Offset: 0x00227288
	public IEnumerator Start()
	{
		yield return null;
		this.m_initialized = true;
		this.m_startingHeadPosition = base.transform.position;
		yield break;
	}

	// Token: 0x06005A6F RID: 23151 RVA: 0x002290A4 File Offset: 0x002272A4
	public void UpdateHead()
	{
		if (!this.m_initialized)
		{
			return;
		}
		Vector2 vector = base.transform.position.XY();
		Vector2 value = new Vector2(vector.x, this.m_startingHeadPosition.y);
		if (this.OverrideDesiredPosition != null)
		{
			value = this.OverrideDesiredPosition.Value;
		}
		else
		{
			if (this.TargetX != null)
			{
				value.x = this.TargetX.Value;
			}
			value.y = this.m_startingHeadPosition.y + Mathf.Sin(Time.timeSinceLevelLoad * 6.2831855f / 1.5f) * 1.5f;
		}
		Vector2 vector2;
		if (this.OverrideDesiredPosition != null)
		{
			vector2 = Vector2.SmoothDamp(vector, value, ref this.m_currentVelocity, this.overrideMoveTime, this.overrideMaxSpeed, BraveTime.DeltaTime);
		}
		else
		{
			vector2 = Vector2.SmoothDamp(vector, value, ref this.m_currentVelocity, this.moveTime, this.maxSpeed, BraveTime.DeltaTime);
		}
		base.transform.position = vector2;
		Vector2 vector3 = vector2 - this.m_startingHeadPosition;
		Vector2 vector4 = vector3;
		if (this.OverrideDesiredPosition == null)
		{
			if (Mathf.Abs(vector4.x) > 6f)
			{
				vector4.x = (float)(Math.Sign(vector4.x) * 6);
			}
			if (vector4.y < -5f)
			{
				vector4.y = -5f;
			}
			if (vector4.y > 4f)
			{
				vector4.y = 4f;
			}
		}
		if (vector4 != vector3)
		{
			base.transform.position = this.m_startingHeadPosition + vector4;
			vector3 = vector4;
		}
		for (int i = 0; i < this.neckPieces.Count; i++)
		{
			this.neckPieces[i].UpdateHeadDelta(vector3);
		}
	}

	// Token: 0x06005A70 RID: 23152 RVA: 0x002292C8 File Offset: 0x002274C8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005A71 RID: 23153 RVA: 0x002292D0 File Offset: 0x002274D0
	public void TriggerAnimationEvent(string eventInfo)
	{
		base.aiActor.behaviorSpeculator.TriggerAnimationEvent(eventInfo);
	}

	// Token: 0x040053DB RID: 21467
	public List<DraGunNeckPieceController> neckPieces;

	// Token: 0x040053DC RID: 21468
	public float moveTime = 1f;

	// Token: 0x040053DD RID: 21469
	public float maxSpeed = 3f;

	// Token: 0x040053DE RID: 21470
	public float overrideMoveTime = 0.5f;

	// Token: 0x040053DF RID: 21471
	public float overrideMaxSpeed = 9f;

	// Token: 0x040053E2 RID: 21474
	private bool m_initialized;

	// Token: 0x040053E3 RID: 21475
	private Vector2 m_startingHeadPosition;

	// Token: 0x040053E4 RID: 21476
	private Vector2 m_currentVelocity;
}
