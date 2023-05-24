using System;
using UnityEngine;

// Token: 0x0200182B RID: 6187
public class EarlyUpdater : MonoBehaviour
{
	// Token: 0x06009278 RID: 37496 RVA: 0x003DDC6C File Offset: 0x003DBE6C
	private void Awake()
	{
		BraveTime.CacheDeltaTimeForFrame();
	}

	// Token: 0x06009279 RID: 37497 RVA: 0x003DDC74 File Offset: 0x003DBE74
	private void Update()
	{
		BraveTime.CacheDeltaTimeForFrame();
	}
}
