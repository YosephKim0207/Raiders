using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001023 RID: 4131
public class DraGunNeckPieceController : BraveBehaviour
{
	// Token: 0x06005A97 RID: 23191 RVA: 0x00229AD8 File Offset: 0x00227CD8
	public IEnumerator Start()
	{
		yield return null;
		this.m_initialized = true;
		this.m_startingPos = base.transform.position;
		this.m_idleTimer = this.idleOffset;
		yield break;
	}

	// Token: 0x06005A98 RID: 23192 RVA: 0x00229AF4 File Offset: 0x00227CF4
	public void Update()
	{
		if (!this.m_initialized)
		{
			return;
		}
		this.m_idleTimer -= BraveTime.DeltaTime;
		if (this.m_idleTimer < 0f)
		{
			this.m_idleTimer += this.idleTime;
			this.m_isIdleUp = !this.m_isIdleUp;
		}
	}

	// Token: 0x06005A99 RID: 23193 RVA: 0x00229B54 File Offset: 0x00227D54
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005A9A RID: 23194 RVA: 0x00229B5C File Offset: 0x00227D5C
	public void UpdateHeadDelta(Vector2 headDelta)
	{
		if (!this.m_initialized)
		{
			return;
		}
		Vector2 vector = this.m_startingPos;
		vector += new Vector2(Mathf.Sign(headDelta.x) * this.xCurve.Evaluate(Mathf.Abs(headDelta.x)), 0f);
		vector += new Vector2(0f, this.yCurve.Evaluate(headDelta.y));
		if (this.m_isIdleUp)
		{
			vector += PhysicsEngine.PixelToUnit(new IntVector2(0, 1));
		}
		base.transform.position = new Vector3(BraveMathCollege.QuantizeFloat(vector.x, 0.0625f), BraveMathCollege.QuantizeFloat(vector.y, 0.0625f));
		if (Mathf.Abs(headDelta.x) > this.flipThreshold)
		{
			base.sprite.SetSprite((Mathf.Sign(headDelta.x) >= 0f) ? this.rightSprite : this.leftSprite);
		}
		else
		{
			base.sprite.SetSprite(this.forwardSprite);
		}
	}

	// Token: 0x040053FB RID: 21499
	public string leftSprite;

	// Token: 0x040053FC RID: 21500
	public string forwardSprite;

	// Token: 0x040053FD RID: 21501
	public string rightSprite;

	// Token: 0x040053FE RID: 21502
	public float flipThreshold;

	// Token: 0x040053FF RID: 21503
	[CurveRange(0f, -6f, 6f, 12f)]
	public AnimationCurve xCurve;

	// Token: 0x04005400 RID: 21504
	[CurveRange(-5f, -5f, 9f, 10f)]
	public AnimationCurve yCurve;

	// Token: 0x04005401 RID: 21505
	public float idleTime;

	// Token: 0x04005402 RID: 21506
	public float idleOffset;

	// Token: 0x04005403 RID: 21507
	private bool m_initialized;

	// Token: 0x04005404 RID: 21508
	private Vector2 m_startingPos;

	// Token: 0x04005405 RID: 21509
	private bool m_isIdleUp;

	// Token: 0x04005406 RID: 21510
	private float m_idleTimer;
}
