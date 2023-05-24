using System;
using UnityEngine;

// Token: 0x020012C2 RID: 4802
[ExecuteInEditMode]
[RequireComponent(typeof(WaterBase))]
public class Displace : MonoBehaviour
{
	// Token: 0x06006B7B RID: 27515 RVA: 0x002A3C84 File Offset: 0x002A1E84
	public void Awake()
	{
		if (base.enabled)
		{
			this.OnEnable();
		}
		else
		{
			this.OnDisable();
		}
	}

	// Token: 0x06006B7C RID: 27516 RVA: 0x002A3CA4 File Offset: 0x002A1EA4
	public void OnEnable()
	{
		Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
		Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
	}

	// Token: 0x06006B7D RID: 27517 RVA: 0x002A3CBC File Offset: 0x002A1EBC
	public void OnDisable()
	{
		Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
		Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
	}
}
