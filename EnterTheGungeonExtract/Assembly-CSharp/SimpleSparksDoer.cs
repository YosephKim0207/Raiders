using System;
using UnityEngine;

// Token: 0x02001212 RID: 4626
public class SimpleSparksDoer : MonoBehaviour
{
	// Token: 0x0600677E RID: 26494 RVA: 0x00287F64 File Offset: 0x00286164
	private void Start()
	{
		this.m_transform = base.gameObject.transform;
	}

	// Token: 0x0600677F RID: 26495 RVA: 0x00287F78 File Offset: 0x00286178
	private void Update()
	{
		if (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW)
		{
			Color? color = null;
			if (this.DefineColor)
			{
				color = new Color?(new Color(UnityEngine.Random.Range(this.Color1.r, this.Color2.r), UnityEngine.Random.Range(this.Color1.g, this.Color2.g), UnityEngine.Random.Range(this.Color1.b, this.Color2.b), UnityEngine.Random.Range(this.Color1.a, this.Color2.a)));
			}
			this.m_particlesToSpawn += (float)this.SparksPerSecond * BraveTime.DeltaTime;
			GlobalSparksDoer.DoRandomParticleBurst((int)this.m_particlesToSpawn, this.m_transform.position + this.localMin, this.m_transform.position + this.localMax, this.baseDirection, this.angleVariance, this.magnitudeVariance, null, new float?(UnityEngine.Random.Range(this.LifespanMin, this.LifespanMax)), color, this.sparksType);
			this.m_particlesToSpawn %= 1f;
		}
	}

	// Token: 0x04006354 RID: 25428
	public Vector3 localMin;

	// Token: 0x04006355 RID: 25429
	public Vector3 localMax;

	// Token: 0x04006356 RID: 25430
	public GlobalSparksDoer.SparksType sparksType;

	// Token: 0x04006357 RID: 25431
	public Vector3 baseDirection = Vector3.up;

	// Token: 0x04006358 RID: 25432
	public float magnitudeVariance = 0.5f;

	// Token: 0x04006359 RID: 25433
	public float angleVariance = 45f;

	// Token: 0x0400635A RID: 25434
	public float LifespanMin = 0.5f;

	// Token: 0x0400635B RID: 25435
	public float LifespanMax = 1f;

	// Token: 0x0400635C RID: 25436
	public int SparksPerSecond = 60;

	// Token: 0x0400635D RID: 25437
	public bool DefineColor;

	// Token: 0x0400635E RID: 25438
	public Color Color1;

	// Token: 0x0400635F RID: 25439
	public Color Color2;

	// Token: 0x04006360 RID: 25440
	private Transform m_transform;

	// Token: 0x04006361 RID: 25441
	private float m_particlesToSpawn;
}
