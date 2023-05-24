using System;

// Token: 0x02001716 RID: 5910
public class TestBulletScript : BraveBehaviour
{
	// Token: 0x06008947 RID: 35143 RVA: 0x0038F888 File Offset: 0x0038DA88
	public void Awake()
	{
		this.m_bulletSource = base.GetComponentInChildren<BulletScriptSource>();
	}

	// Token: 0x06008948 RID: 35144 RVA: 0x0038F898 File Offset: 0x0038DA98
	private void Update()
	{
		if (this.m_bulletSource.IsEnded)
		{
			this.m_counter += BraveTime.DeltaTime;
			if (this.m_counter > this.fireDelay)
			{
				this.m_counter = 0f;
				this.m_bulletSource.Initialize();
			}
		}
	}

	// Token: 0x06008949 RID: 35145 RVA: 0x0038F8F0 File Offset: 0x0038DAF0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008F4D RID: 36685
	public float fireDelay = 1f;

	// Token: 0x04008F4E RID: 36686
	private BulletScriptSource m_bulletSource;

	// Token: 0x04008F4F RID: 36687
	private float m_counter;
}
