using System;

// Token: 0x02001835 RID: 6197
public class TimeInvariantMonoBehaviour : BraveBehaviour
{
	// Token: 0x060092BB RID: 37563 RVA: 0x003DFF24 File Offset: 0x003DE124
	protected virtual void Update()
	{
		this.m_deltaTime = GameManager.INVARIANT_DELTA_TIME;
		this.InvariantUpdate(GameManager.INVARIANT_DELTA_TIME);
	}

	// Token: 0x060092BC RID: 37564 RVA: 0x003DFF3C File Offset: 0x003DE13C
	protected virtual void InvariantUpdate(float realDeltaTime)
	{
	}

	// Token: 0x060092BD RID: 37565 RVA: 0x003DFF40 File Offset: 0x003DE140
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04009A55 RID: 39509
	protected float m_deltaTime;
}
