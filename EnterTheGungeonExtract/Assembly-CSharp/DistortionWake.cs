using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001291 RID: 4753
public class DistortionWake : BraveBehaviour
{
	// Token: 0x06006A5D RID: 27229 RVA: 0x0029B58C File Offset: 0x0029978C
	private void Start()
	{
		this.m_material = new Material(ShaderCache.Acquire("Brave/Internal/DistortionLine"));
		this.m_material.SetVector("_WavePoint1", this.CalculateSettings(base.specRigidbody.UnitCenter, 0f));
		this.m_material.SetVector("_WavePoint2", this.CalculateSettings(base.specRigidbody.UnitCenter, 0f));
		this.m_material.SetFloat("_DistortProgress", this.initialOffset);
		Pixelator.Instance.RegisterAdditionalRenderPass(this.m_material);
	}

	// Token: 0x06006A5E RID: 27230 RVA: 0x0029B620 File Offset: 0x00299820
	private Vector4 CalculateSettings(Vector2 worldPoint, float t)
	{
		Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(worldPoint.ToVector3ZUp(0f));
		return new Vector4(vector.x, vector.y, Mathf.Lerp(this.initialRadius, this.maxRadius, t), Mathf.Lerp(this.initialIntensity, this.maxIntensity, t));
	}

	// Token: 0x06006A5F RID: 27231 RVA: 0x0029B684 File Offset: 0x00299884
	private void LateUpdate()
	{
		this.m_positions.Add(base.specRigidbody.UnitCenter);
		if (this.m_positions.Count == 1)
		{
			return;
		}
		if (this.m_positions.Count != 2)
		{
			Vector2 vector = this.m_positions[this.m_positions.Count - 1];
			while (Vector2.Distance(vector, this.m_positions[1]) > this.maxLength)
			{
				this.m_positions.RemoveAt(0);
			}
		}
		this.m_material.SetVector("_WavePoint1", this.CalculateSettings(this.m_positions[this.m_positions.Count - 1], 0f));
		float num = Vector2.Distance(this.m_positions[this.m_positions.Count - 1], this.m_positions[0]);
		this.m_material.SetVector("_WavePoint2", this.CalculateSettings(this.m_positions[0], Mathf.Clamp01(num / this.maxLength)));
		float num2 = this.initialOffset;
		if (this.offsetVariance > 0f)
		{
			num2 += Mathf.Sin(Time.realtimeSinceStartup * this.offsetVarianceSpeed) * this.offsetVariance;
		}
		this.m_material.SetFloat("_DistortProgress", num2);
	}

	// Token: 0x06006A60 RID: 27232 RVA: 0x0029B7E8 File Offset: 0x002999E8
	protected override void OnDestroy()
	{
		if (Pixelator.HasInstance && this.m_material)
		{
			Pixelator.Instance.DeregisterAdditionalRenderPass(this.m_material);
		}
		if (this.m_material)
		{
			UnityEngine.Object.Destroy(this.m_material);
		}
	}

	// Token: 0x040066DD RID: 26333
	public float maxLength = 3f;

	// Token: 0x040066DE RID: 26334
	public float initialIntensity;

	// Token: 0x040066DF RID: 26335
	public float maxIntensity = 1f;

	// Token: 0x040066E0 RID: 26336
	public float initialRadius;

	// Token: 0x040066E1 RID: 26337
	public float maxRadius = 0.5f;

	// Token: 0x040066E2 RID: 26338
	public float initialOffset;

	// Token: 0x040066E3 RID: 26339
	public float offsetVariance;

	// Token: 0x040066E4 RID: 26340
	public float offsetVarianceSpeed = 1f;

	// Token: 0x040066E5 RID: 26341
	[NonSerialized]
	private Material m_material;

	// Token: 0x040066E6 RID: 26342
	[NonSerialized]
	private List<Vector2> m_positions = new List<Vector2>();
}
