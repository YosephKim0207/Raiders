using System;
using UnityEngine;

// Token: 0x02001539 RID: 5433
public class SimpleLightIntensityCurve : MonoBehaviour
{
	// Token: 0x06007C54 RID: 31828 RVA: 0x003208D4 File Offset: 0x0031EAD4
	private void Start()
	{
		this.m_light = base.GetComponent<Light>();
		this.m_light.intensity = this.Curve.Evaluate(0f) * (this.MaxIntensity - this.MinIntensity) + this.MinIntensity;
	}

	// Token: 0x06007C55 RID: 31829 RVA: 0x00320914 File Offset: 0x0031EB14
	private void Update()
	{
		this.m_elapsed += BraveTime.DeltaTime;
		if (this.m_elapsed < this.Duration)
		{
			this.m_light.intensity = this.Curve.Evaluate(this.m_elapsed / this.Duration) * (this.MaxIntensity - this.MinIntensity) + this.MinIntensity;
		}
		else
		{
			this.m_light.intensity = this.Curve.Evaluate(1f) * (this.MaxIntensity - this.MinIntensity) + this.MinIntensity;
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x04007F47 RID: 32583
	public float Duration = 1f;

	// Token: 0x04007F48 RID: 32584
	public float MinIntensity;

	// Token: 0x04007F49 RID: 32585
	public float MaxIntensity = 1f;

	// Token: 0x04007F4A RID: 32586
	[CurveRange(0f, 0f, 1f, 1f)]
	public AnimationCurve Curve;

	// Token: 0x04007F4B RID: 32587
	protected Light m_light;

	// Token: 0x04007F4C RID: 32588
	protected float m_elapsed;
}
