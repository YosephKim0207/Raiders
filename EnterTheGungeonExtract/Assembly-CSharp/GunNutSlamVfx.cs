using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200183D RID: 6205
public class GunNutSlamVfx : MonoBehaviour
{
	// Token: 0x060092E0 RID: 37600 RVA: 0x003E0538 File Offset: 0x003DE738
	public void OnSpawned()
	{
		base.StartCoroutine(this.DoVfx());
	}

	// Token: 0x060092E1 RID: 37601 RVA: 0x003E0548 File Offset: 0x003DE748
	private IEnumerator DoVfx()
	{
		yield return null;
		Vector2 dir = base.transform.right;
		Vector2 sideDir = Quaternion.Euler(0f, 0f, 90f) * dir;
		Vector2 pos = base.transform.position;
		int i = 0;
		while ((float)i < this.SlamCount)
		{
			this.SlamVfx.SpawnAtPosition(pos, 0f, null, null, null, null, false, null, null, false);
			this.DustVfx.SpawnAtPosition(pos + sideDir * this.DustOffset, 0f, null, null, null, null, false, null, null, false);
			this.DustVfx.SpawnAtPosition(pos - sideDir * this.DustOffset, 0f, null, null, null, null, false, null, null, false);
			if ((float)i < this.SlamCount - 1f)
			{
				yield return new WaitForSeconds(this.SlamDelay);
				pos += dir * this.SlamDistance;
			}
			i++;
		}
		yield break;
	}

	// Token: 0x04009A65 RID: 39525
	public VFXPool SlamVfx;

	// Token: 0x04009A66 RID: 39526
	public float SlamCount;

	// Token: 0x04009A67 RID: 39527
	public float SlamDistance;

	// Token: 0x04009A68 RID: 39528
	public float SlamDelay;

	// Token: 0x04009A69 RID: 39529
	public VFXPool DustVfx;

	// Token: 0x04009A6A RID: 39530
	public float DustOffset;
}
