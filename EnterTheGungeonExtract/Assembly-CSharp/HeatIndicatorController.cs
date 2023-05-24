using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200183F RID: 6207
public class HeatIndicatorController : MonoBehaviour
{
	// Token: 0x060092E9 RID: 37609 RVA: 0x003E0824 File Offset: 0x003DEA24
	public void Awake()
	{
		this.m_radiusID = Shader.PropertyToID("_Radius");
		this.m_centerID = Shader.PropertyToID("_WorldCenter");
		this.m_colorID = Shader.PropertyToID("_RingColor");
		this.m_materialInst = base.GetComponent<MeshRenderer>().material;
	}

	// Token: 0x060092EA RID: 37610 RVA: 0x003E0874 File Offset: 0x003DEA74
	public void Start()
	{
		base.StartCoroutine(this.LerpColor(this, new Color(0f, 0f, 0f, 0f), this.CurrentColor, 0.5f));
		if (!this.IsFire)
		{
			ParticleSystem componentInChildren = base.GetComponentInChildren<ParticleSystem>();
			if (componentInChildren)
			{
				componentInChildren.emission.enabled = false;
				componentInChildren.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060092EB RID: 37611 RVA: 0x003E08EC File Offset: 0x003DEAEC
	public void LateUpdate()
	{
		base.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
		base.transform.localScale = new Vector3(this.CurrentRadius * 2f, this.CurrentRadius * 2f * Mathf.Sqrt(2f), 1f);
		Vector3 position = base.transform.position;
		this.m_materialInst.SetVector(this.m_centerID, new Vector4(position.x, position.y, position.z, 0f));
		this.m_materialInst.SetFloat(this.m_radiusID, this.CurrentRadius);
		this.m_materialInst.SetColor(this.m_colorID, this.CurrentColor);
	}

	// Token: 0x060092EC RID: 37612 RVA: 0x003E09BC File Offset: 0x003DEBBC
	public void EndEffect()
	{
		base.StartCoroutine(this.LerpColor(this, this.CurrentColor, new Color(0f, 0f, 0f, 0f), 0.5f));
		base.StartCoroutine(this.HandleDeathDelay());
	}

	// Token: 0x060092ED RID: 37613 RVA: 0x003E0A08 File Offset: 0x003DEC08
	private IEnumerator HandleDeathDelay()
	{
		ParticleSystem m_particleSystem = base.GetComponentInChildren<ParticleSystem>();
		if (m_particleSystem)
		{
			m_particleSystem.emission.enabled = false;
		}
		float elapsed = 0f;
		while (m_particleSystem && m_particleSystem.particleCount > 0)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		while ((double)elapsed < 0.5)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060092EE RID: 37614 RVA: 0x003E0A24 File Offset: 0x003DEC24
	private IEnumerator LerpColor(HeatIndicatorController indicator, Color startColor, Color targetColor, float lerpTime)
	{
		float elapsed = 0f;
		while (elapsed < lerpTime)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.Clamp01(elapsed / lerpTime);
			Color c = Color.Lerp(startColor, targetColor, t);
			indicator.CurrentColor = c;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04009A73 RID: 39539
	public float CurrentRadius = 5f;

	// Token: 0x04009A74 RID: 39540
	public Color CurrentColor = new Color(1f, 0f, 0f, 1f);

	// Token: 0x04009A75 RID: 39541
	public bool IsFire = true;

	// Token: 0x04009A76 RID: 39542
	private Material m_materialInst;

	// Token: 0x04009A77 RID: 39543
	private int m_radiusID;

	// Token: 0x04009A78 RID: 39544
	private int m_centerID;

	// Token: 0x04009A79 RID: 39545
	private int m_colorID;
}
