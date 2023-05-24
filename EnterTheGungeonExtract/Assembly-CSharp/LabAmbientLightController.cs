using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200151A RID: 5402
public class LabAmbientLightController : MonoBehaviour
{
	// Token: 0x06007B49 RID: 31561 RVA: 0x003160CC File Offset: 0x003142CC
	private IEnumerator Start()
	{
		yield return null;
		this.m_lightStarts = new Vector3[this.HallwayLights.Length];
		this.HallwayLightManagers = new ShadowSystem[this.HallwayLights.Length];
		this.m_lightIntensities = new float[this.HallwayLights.Length];
		this.m_colorID = Shader.PropertyToID("_TintColor");
		for (int i = 0; i < this.HallwayLights.Length; i++)
		{
			this.HallwayLightManagers[i] = this.HallwayLights[i].GetComponentInChildren<ShadowSystem>();
			this.m_lightStarts[i] = this.HallwayLights[i].position;
			this.m_lightIntensities[i] = this.HallwayLightManagers[i].uLightIntensity;
		}
		yield break;
	}

	// Token: 0x06007B4A RID: 31562 RVA: 0x003160E8 File Offset: 0x003142E8
	private void Update()
	{
		if (this.m_lightStarts == null)
		{
			return;
		}
		GameManager.Instance.Dungeon.OverrideAmbientLight = true;
		GameManager.Instance.Dungeon.OverrideAmbientColor = this.colorGradient.Evaluate(Time.timeSinceLevelLoad % this.period / this.period);
		float num = Time.timeSinceLevelLoad % this.HallwayPeriod / this.HallwayPeriod;
		float num2 = Mathf.PingPong(num, 0.5f) * 2f;
		for (int i = 0; i < this.HallwayLights.Length; i++)
		{
			this.HallwayLightManagers[i].uLightIntensity = this.m_lightIntensities[i] * num2;
			Material sharedMaterial = this.HallwayLightManagers[i].renderer.sharedMaterial;
			sharedMaterial.SetColor(this.m_colorID, sharedMaterial.GetColor(this.m_colorID).WithAlpha(num2));
			this.HallwayLights[i].position = this.m_lightStarts[i] + new Vector3(this.HallwayXTranslation * num, 0f, 0f);
		}
		PlatformInterface.SetAlienFXAmbientColor(new Color(1f, 0f, 0f, num2));
	}

	// Token: 0x04007DCB RID: 32203
	public Gradient colorGradient;

	// Token: 0x04007DCC RID: 32204
	public float period = 5f;

	// Token: 0x04007DCD RID: 32205
	public Transform[] HallwayLights;

	// Token: 0x04007DCE RID: 32206
	public float HallwayXTranslation = 10f;

	// Token: 0x04007DCF RID: 32207
	public float HallwayPeriod = 3f;

	// Token: 0x04007DD0 RID: 32208
	private ShadowSystem[] HallwayLightManagers;

	// Token: 0x04007DD1 RID: 32209
	private float[] m_lightIntensities;

	// Token: 0x04007DD2 RID: 32210
	private Vector3[] m_lightStarts;

	// Token: 0x04007DD3 RID: 32211
	private int m_colorID;
}
