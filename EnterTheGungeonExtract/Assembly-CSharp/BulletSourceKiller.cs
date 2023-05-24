using System;
using UnityEngine;

// Token: 0x02000E3B RID: 3643
public class BulletSourceKiller : BraveBehaviour
{
	// Token: 0x06004D17 RID: 19735 RVA: 0x001A6334 File Offset: 0x001A4534
	public void Start()
	{
		if (!this.BraveSource)
		{
			this.BraveSource = base.GetComponent<BulletScriptSource>();
		}
	}

	// Token: 0x06004D18 RID: 19736 RVA: 0x001A6354 File Offset: 0x001A4554
	public void Update()
	{
		if (this.TrackRigidbody)
		{
			base.transform.position = this.TrackRigidbody.GetUnitCenter(ColliderType.HitBox);
		}
		if (this.BraveSource && this.BraveSource.IsEnded)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (!this.BraveSource)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06004D19 RID: 19737 RVA: 0x001A63D4 File Offset: 0x001A45D4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04004343 RID: 17219
	public BulletScriptSource BraveSource;

	// Token: 0x04004344 RID: 17220
	public SpeculativeRigidbody TrackRigidbody;
}
