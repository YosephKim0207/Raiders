using System;
using UnityEngine;

// Token: 0x020004D0 RID: 1232
public abstract class dfTweenPlayableBase : MonoBehaviour
{
	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x06001D15 RID: 7445
	// (set) Token: 0x06001D16 RID: 7446
	public abstract string TweenName { get; set; }

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x06001D17 RID: 7447
	public abstract bool IsPlaying { get; }

	// Token: 0x06001D18 RID: 7448
	public abstract void Play();

	// Token: 0x06001D19 RID: 7449
	public abstract void Stop();

	// Token: 0x06001D1A RID: 7450
	public abstract void Reset();

	// Token: 0x06001D1B RID: 7451 RVA: 0x00087F14 File Offset: 0x00086114
	public void Enable()
	{
		base.enabled = true;
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x00087F20 File Offset: 0x00086120
	public void Disable()
	{
		base.enabled = false;
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x00087F2C File Offset: 0x0008612C
	public override string ToString()
	{
		return this.TweenName + " - " + base.ToString();
	}
}
