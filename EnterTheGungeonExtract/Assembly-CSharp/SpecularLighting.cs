using System;
using UnityEngine;

// Token: 0x020012C6 RID: 4806
[RequireComponent(typeof(WaterBase))]
[ExecuteInEditMode]
public class SpecularLighting : MonoBehaviour
{
	// Token: 0x06006B91 RID: 27537 RVA: 0x002A44F0 File Offset: 0x002A26F0
	public void Start()
	{
		this.waterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
	}

	// Token: 0x06006B92 RID: 27538 RVA: 0x002A4514 File Offset: 0x002A2714
	public void Update()
	{
		if (!this.waterBase)
		{
			this.waterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
		}
		if (this.specularLight && this.waterBase.sharedMaterial)
		{
			this.waterBase.sharedMaterial.SetVector("_WorldLightDir", this.specularLight.transform.forward);
		}
	}

	// Token: 0x04006882 RID: 26754
	public Transform specularLight;

	// Token: 0x04006883 RID: 26755
	private WaterBase waterBase;
}
