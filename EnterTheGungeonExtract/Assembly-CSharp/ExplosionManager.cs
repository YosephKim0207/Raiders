using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012D0 RID: 4816
public class ExplosionManager : BraveBehaviour
{
	// Token: 0x17000FFC RID: 4092
	// (get) Token: 0x06006BCF RID: 27599 RVA: 0x002A6A74 File Offset: 0x002A4C74
	public static ExplosionManager Instance
	{
		get
		{
			if (!ExplosionManager.m_instance)
			{
				GameObject gameObject = new GameObject("_ExplosionManager", new Type[] { typeof(ExplosionManager) });
				ExplosionManager.m_instance = gameObject.GetComponent<ExplosionManager>();
			}
			return ExplosionManager.m_instance;
		}
	}

	// Token: 0x06006BD0 RID: 27600 RVA: 0x002A6AC0 File Offset: 0x002A4CC0
	public static void ClearPerLevelData()
	{
		ExplosionManager.m_instance = null;
	}

	// Token: 0x06006BD1 RID: 27601 RVA: 0x002A6AC8 File Offset: 0x002A4CC8
	public void Update()
	{
		if (this.m_timer > 0f)
		{
			this.m_timer -= BraveTime.DeltaTime;
		}
	}

	// Token: 0x06006BD2 RID: 27602 RVA: 0x002A6AEC File Offset: 0x002A4CEC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06006BD3 RID: 27603 RVA: 0x002A6AF4 File Offset: 0x002A4CF4
	public void Queue(Exploder exploder)
	{
		this.m_queue.Enqueue(exploder);
	}

	// Token: 0x06006BD4 RID: 27604 RVA: 0x002A6B04 File Offset: 0x002A4D04
	public bool IsExploderReady(Exploder exploder)
	{
		return this.m_queue.Count == 0 || (this.m_queue.Peek() == exploder && this.m_timer <= 0f);
	}

	// Token: 0x06006BD5 RID: 27605 RVA: 0x002A6B44 File Offset: 0x002A4D44
	public void Dequeue()
	{
		if (this.m_queue.Count > 0)
		{
			this.m_queue.Dequeue();
		}
		this.m_timer = 0.125f;
	}

	// Token: 0x17000FFD RID: 4093
	// (get) Token: 0x06006BD6 RID: 27606 RVA: 0x002A6B70 File Offset: 0x002A4D70
	public int QueueCount
	{
		get
		{
			return this.m_queue.Count;
		}
	}

	// Token: 0x040068C7 RID: 26823
	private const float c_explosionStaggerDelay = 0.125f;

	// Token: 0x040068C8 RID: 26824
	private Queue<Exploder> m_queue = new Queue<Exploder>();

	// Token: 0x040068C9 RID: 26825
	private float m_timer;

	// Token: 0x040068CA RID: 26826
	private static ExplosionManager m_instance;
}
