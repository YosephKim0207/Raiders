using System;
using UnityEngine;

// Token: 0x02001845 RID: 6213
public class UVScrollTriggerableInitializer : MonoBehaviour
{
	// Token: 0x06009306 RID: 37638 RVA: 0x003E0F50 File Offset: 0x003DF150
	public void OnSpawned()
	{
		this.ResetAnimation();
	}

	// Token: 0x06009307 RID: 37639 RVA: 0x003E0F58 File Offset: 0x003DF158
	public void TriggerAnimation()
	{
		float num = Time.realtimeSinceStartup;
		num %= (float)this.NumberFrames * this.TimePerFrame;
		Material material = base.GetComponent<MeshRenderer>().material;
		material.SetFloat("_TimeOffset", num);
		material.SetFloat("_ForcedFrame", -1f);
	}

	// Token: 0x06009308 RID: 37640 RVA: 0x003E0FA4 File Offset: 0x003DF1A4
	public void ResetAnimation()
	{
		Material material = base.GetComponent<MeshRenderer>().material;
		material.SetFloat("_ForcedFrame", 0f);
	}

	// Token: 0x04009A98 RID: 39576
	public int NumberFrames;

	// Token: 0x04009A99 RID: 39577
	public float TimePerFrame;
}
