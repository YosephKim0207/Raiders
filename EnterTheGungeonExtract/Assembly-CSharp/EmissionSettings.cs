using System;
using UnityEngine;

// Token: 0x02001518 RID: 5400
public class EmissionSettings : MonoBehaviour
{
	// Token: 0x06007B44 RID: 31556 RVA: 0x00315FB8 File Offset: 0x003141B8
	private void Start()
	{
		if (!EmissionSettings.indicesInitialized)
		{
			EmissionSettings.indicesInitialized = true;
			EmissionSettings.powerIndex = Shader.PropertyToID("_EmissivePower");
			EmissionSettings.colorPowerIndex = Shader.PropertyToID("_EmissiveColorPower");
		}
		tk2dBaseSprite component = base.GetComponent<tk2dBaseSprite>();
		if (component != null)
		{
			component.usesOverrideMaterial = true;
		}
		base.GetComponent<Renderer>().material.SetFloat(EmissionSettings.powerIndex, this.EmissivePower);
		base.GetComponent<Renderer>().material.SetFloat(EmissionSettings.colorPowerIndex, this.EmissiveColorPower);
	}

	// Token: 0x04007DC4 RID: 32196
	public float EmissivePower;

	// Token: 0x04007DC5 RID: 32197
	public float EmissiveColorPower = 7f;

	// Token: 0x04007DC6 RID: 32198
	private static bool indicesInitialized;

	// Token: 0x04007DC7 RID: 32199
	private static int powerIndex;

	// Token: 0x04007DC8 RID: 32200
	private static int colorPowerIndex;
}
