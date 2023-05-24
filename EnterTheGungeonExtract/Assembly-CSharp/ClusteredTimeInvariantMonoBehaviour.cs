using System;

// Token: 0x02001827 RID: 6183
public class ClusteredTimeInvariantMonoBehaviour : BraveBehaviour
{
	// Token: 0x06009267 RID: 37479 RVA: 0x003DD98C File Offset: 0x003DBB8C
	protected virtual void Awake()
	{
		StaticReferenceManager.AllClusteredTimeInvariantBehaviours.Add(this);
	}

	// Token: 0x06009268 RID: 37480 RVA: 0x003DD99C File Offset: 0x003DBB9C
	public void DoUpdate(float realDeltaTime)
	{
		this.m_deltaTime = realDeltaTime;
		this.InvariantUpdate(realDeltaTime);
	}

	// Token: 0x06009269 RID: 37481 RVA: 0x003DD9AC File Offset: 0x003DBBAC
	protected virtual void InvariantUpdate(float realDeltaTime)
	{
	}

	// Token: 0x0600926A RID: 37482 RVA: 0x003DD9B0 File Offset: 0x003DBBB0
	protected override void OnDestroy()
	{
		StaticReferenceManager.AllClusteredTimeInvariantBehaviours.Remove(this);
		base.OnDestroy();
	}

	// Token: 0x040099FA RID: 39418
	protected float m_deltaTime;
}
