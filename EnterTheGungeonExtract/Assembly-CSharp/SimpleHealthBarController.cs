using System;
using UnityEngine;

// Token: 0x020014AB RID: 5291
public class SimpleHealthBarController : MonoBehaviour
{
	// Token: 0x06007856 RID: 30806 RVA: 0x00301DC8 File Offset: 0x002FFFC8
	public void Initialize(SpeculativeRigidbody srb, HealthHaver h)
	{
		this.m_healthHaver = h;
		this.m_baseScale = 1f;
		if (srb.UnitWidth > 1f)
		{
			this.m_baseScale = srb.UnitWidth;
		}
		base.transform.parent = this.m_healthHaver.transform;
		base.transform.position = srb.UnitBottomCenter.Quantize(0.0625f).ToVector3ZisY(0f) + new Vector3(0f, -0.25f, 0f);
		this.fg.localScale = this.fg.localScale.WithX(this.m_baseScale);
		this.bg.localScale = this.bg.localScale.WithX(this.m_baseScale);
		this.fg.localPosition = new Vector3(-1f * (this.m_baseScale * 0.5f), 0f, 0f);
	}

	// Token: 0x06007857 RID: 30807 RVA: 0x00301EC8 File Offset: 0x003000C8
	private void Update()
	{
		if (this.m_healthHaver)
		{
			this.fg.localScale = this.fg.localScale.WithX(this.m_healthHaver.GetCurrentHealthPercentage() * this.m_baseScale);
		}
	}

	// Token: 0x04007A8B RID: 31371
	public Transform fg;

	// Token: 0x04007A8C RID: 31372
	public Transform bg;

	// Token: 0x04007A8D RID: 31373
	private HealthHaver m_healthHaver;

	// Token: 0x04007A8E RID: 31374
	private float m_baseScale = 1f;
}
