using System;
using UnityEngine;

// Token: 0x02001523 RID: 5411
public class MagicCircleHelper : MonoBehaviour
{
	// Token: 0x06007B7C RID: 31612 RVA: 0x003172FC File Offset: 0x003154FC
	private void Start()
	{
		if (!MagicCircleHelper.indicesInitialized)
		{
			MagicCircleHelper.indicesInitialized = true;
			MagicCircleHelper.powerIndex = Shader.PropertyToID("_EmissivePower");
			MagicCircleHelper.colorPowerIndex = Shader.PropertyToID("_EmissiveColorPower");
			MagicCircleHelper.circlefadeIndex = Shader.PropertyToID("_RadialFade");
			MagicCircleHelper.uvRangeIndex = Shader.PropertyToID("_UVMinMax");
			MagicCircleHelper.brightnessIndex = Shader.PropertyToID("_Brightness");
		}
		tk2dBaseSprite component = base.GetComponent<tk2dBaseSprite>();
		if (component != null)
		{
			component.usesOverrideMaterial = true;
		}
		this.m_mf = base.GetComponent<MeshFilter>();
		this.m_materialInst = base.GetComponent<Renderer>().material;
		this.m_materialInst.SetFloat(MagicCircleHelper.powerIndex, this.minEmissivePower);
		this.m_materialInst.SetFloat(MagicCircleHelper.colorPowerIndex, this.EmissiveColorPower);
	}

	// Token: 0x06007B7D RID: 31613 RVA: 0x003173C8 File Offset: 0x003155C8
	public void OnSpawned()
	{
		this.elapsed = 0f;
	}

	// Token: 0x06007B7E RID: 31614 RVA: 0x003173D8 File Offset: 0x003155D8
	private Vector4 GetMinMaxUVs()
	{
		Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
		Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
		for (int i = 0; i < this.m_mf.sharedMesh.uv.Length; i++)
		{
			vector = Vector2.Min(this.m_mf.sharedMesh.uv[i], vector);
			vector2 = Vector2.Max(this.m_mf.sharedMesh.uv[i], vector2);
		}
		return new Vector4(vector.x, vector.y, vector2.x, vector2.y);
	}

	// Token: 0x06007B7F RID: 31615 RVA: 0x00317490 File Offset: 0x00315690
	private void LateUpdate()
	{
		this.m_materialInst.SetVector(MagicCircleHelper.uvRangeIndex, this.GetMinMaxUVs());
		this.elapsed += BraveTime.DeltaTime;
		this.m_materialInst.SetFloat(MagicCircleHelper.circlefadeIndex, Mathf.Lerp(1f, 0f, this.elapsed / this.fadeInTime));
		float num = Mathf.PingPong(this.elapsed, this.pulsePeriod) / this.pulsePeriod;
		this.m_materialInst.SetFloat(MagicCircleHelper.brightnessIndex, Mathf.Lerp(this.minBrightness, this.maxBrightness, num) * Mathf.Clamp01(this.elapsed / this.fadeInTime));
		this.m_materialInst.SetFloat(MagicCircleHelper.powerIndex, Mathf.Lerp(this.minEmissivePower, this.maxEmissivePower, num) * Mathf.Clamp01(this.elapsed / this.fadeInTime));
		if (this.CircleParticles != null)
		{
			BraveUtility.EnableEmission(this.CircleParticles, this.elapsed / this.fadeInTime >= this.CircleStartVal);
		}
	}

	// Token: 0x04007E09 RID: 32265
	public ParticleSystem CircleParticles;

	// Token: 0x04007E0A RID: 32266
	public float CircleStartVal = 0.75f;

	// Token: 0x04007E0B RID: 32267
	public float EmissiveColorPower = 7f;

	// Token: 0x04007E0C RID: 32268
	public float minBrightness = 0.5f;

	// Token: 0x04007E0D RID: 32269
	public float maxBrightness = 1f;

	// Token: 0x04007E0E RID: 32270
	public float minEmissivePower = 50f;

	// Token: 0x04007E0F RID: 32271
	public float maxEmissivePower = 100f;

	// Token: 0x04007E10 RID: 32272
	public float pulsePeriod = 1f;

	// Token: 0x04007E11 RID: 32273
	public float fadeInTime = 1f;

	// Token: 0x04007E12 RID: 32274
	private float elapsed;

	// Token: 0x04007E13 RID: 32275
	private Material m_materialInst;

	// Token: 0x04007E14 RID: 32276
	private MeshFilter m_mf;

	// Token: 0x04007E15 RID: 32277
	private static bool indicesInitialized;

	// Token: 0x04007E16 RID: 32278
	private static int powerIndex;

	// Token: 0x04007E17 RID: 32279
	private static int colorPowerIndex;

	// Token: 0x04007E18 RID: 32280
	private static int circlefadeIndex;

	// Token: 0x04007E19 RID: 32281
	private static int uvRangeIndex;

	// Token: 0x04007E1A RID: 32282
	private static int brightnessIndex;
}
