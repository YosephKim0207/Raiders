using System;
using UnityEngine;

// Token: 0x02001844 RID: 6212
public class UVScrollTimeOffsetInitializer : MonoBehaviour
{
	// Token: 0x06009304 RID: 37636 RVA: 0x003E0F0C File Offset: 0x003DF10C
	public void OnSpawned()
	{
		float num = Time.realtimeSinceStartup;
		num %= (float)this.NumberFrames * this.TimePerFrame;
		Material material = base.GetComponent<MeshRenderer>().material;
		material.SetFloat("_TimeOffset", num);
	}

	// Token: 0x04009A96 RID: 39574
	public int NumberFrames;

	// Token: 0x04009A97 RID: 39575
	public float TimePerFrame;
}
