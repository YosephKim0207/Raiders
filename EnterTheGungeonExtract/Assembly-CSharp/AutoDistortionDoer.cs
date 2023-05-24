using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001515 RID: 5397
public class AutoDistortionDoer : BraveBehaviour
{
	// Token: 0x06007B36 RID: 31542 RVA: 0x00315DDC File Offset: 0x00313FDC
	private void Start()
	{
		this.OnSpawned();
	}

	// Token: 0x06007B37 RID: 31543 RVA: 0x00315DE4 File Offset: 0x00313FE4
	private void OnSpawned()
	{
		if (!this.m_triggered)
		{
			Vector2 vector = ((!base.sprite) ? base.transform.position.XY() : base.sprite.WorldCenter);
			base.StartCoroutine(this.Distort(vector));
			this.m_triggered = true;
		}
	}

	// Token: 0x06007B38 RID: 31544 RVA: 0x00315E44 File Offset: 0x00314044
	private IEnumerator Distort(Vector2 centerPoint)
	{
		yield return new WaitForSeconds(this.DelayTime);
		Exploder.DoDistortionWave(centerPoint, this.Intensity, this.Width, this.Radius, this.Duration);
		yield break;
	}

	// Token: 0x06007B39 RID: 31545 RVA: 0x00315E68 File Offset: 0x00314068
	private void OnDespawned()
	{
		this.m_triggered = false;
	}

	// Token: 0x04007DB5 RID: 32181
	public float Intensity = 0.25f;

	// Token: 0x04007DB6 RID: 32182
	public float Width = 0.125f;

	// Token: 0x04007DB7 RID: 32183
	public float Radius = 5f;

	// Token: 0x04007DB8 RID: 32184
	public float Duration = 1f;

	// Token: 0x04007DB9 RID: 32185
	public float DelayTime = 0.25f;

	// Token: 0x04007DBA RID: 32186
	private bool m_triggered;
}
