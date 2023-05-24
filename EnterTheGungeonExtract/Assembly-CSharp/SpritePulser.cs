using System;
using System.Collections;
using UnityEngine;

// Token: 0x020016CE RID: 5838
public class SpritePulser : BraveBehaviour
{
	// Token: 0x060087CC RID: 34764 RVA: 0x00384C74 File Offset: 0x00382E74
	private void Start()
	{
		if (base.sprite == null)
		{
			Debug.LogError("No sprite on SpritePulser!", this);
		}
	}

	// Token: 0x060087CD RID: 34765 RVA: 0x00384C98 File Offset: 0x00382E98
	private void Update()
	{
		if (this.m_active)
		{
			float num = Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.realtimeSinceStartup, this.duration) / this.duration);
			Color color = base.sprite.color;
			color.a = Mathf.Lerp(this.minAlpha, 1f, num);
			base.sprite.color = color;
		}
	}

	// Token: 0x060087CE RID: 34766 RVA: 0x00384D08 File Offset: 0x00382F08
	private void OnBecameVisible()
	{
		this.m_active = true;
	}

	// Token: 0x060087CF RID: 34767 RVA: 0x00384D14 File Offset: 0x00382F14
	private void OnBecameInvisible()
	{
		this.m_active = false;
	}

	// Token: 0x060087D0 RID: 34768 RVA: 0x00384D20 File Offset: 0x00382F20
	private IEnumerator Pulse()
	{
		for (;;)
		{
			if (this.m_active)
			{
				float num = Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.realtimeSinceStartup, this.duration) / this.duration);
				Color color = base.sprite.color;
				color.a = Mathf.Lerp(this.minAlpha, 1f, num);
				base.sprite.color = color;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060087D1 RID: 34769 RVA: 0x00384D3C File Offset: 0x00382F3C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008CF6 RID: 36086
	public float duration = 1f;

	// Token: 0x04008CF7 RID: 36087
	public float minDuration = 0.3f;

	// Token: 0x04008CF8 RID: 36088
	public float maxDuration = 2.9f;

	// Token: 0x04008CF9 RID: 36089
	public float metaDuration = 6f;

	// Token: 0x04008CFA RID: 36090
	public float minAlpha = 0.3f;

	// Token: 0x04008CFB RID: 36091
	public float minScale = 0.9f;

	// Token: 0x04008CFC RID: 36092
	public float maxScale = 1.1f;

	// Token: 0x04008CFD RID: 36093
	private bool m_active;
}
