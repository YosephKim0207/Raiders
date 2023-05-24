using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020010D1 RID: 4305
public class VfxController : BraveBehaviour
{
	// Token: 0x06005ED0 RID: 24272 RVA: 0x002464F0 File Offset: 0x002446F0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		for (int i = 0; i < this.OtherVfx.Count; i++)
		{
			AIAnimator.NamedVFXPool namedVFXPool = this.OtherVfx[i];
			namedVFXPool.vfxPool.DestroyAll();
		}
	}

	// Token: 0x06005ED1 RID: 24273 RVA: 0x00246538 File Offset: 0x00244738
	public VFXPool GetVfx(string name)
	{
		AIAnimator.NamedVFXPool namedVFXPool = this.OtherVfx.Find((AIAnimator.NamedVFXPool n) => n.name == name);
		if (namedVFXPool != null)
		{
			return namedVFXPool.vfxPool;
		}
		return null;
	}

	// Token: 0x06005ED2 RID: 24274 RVA: 0x00246578 File Offset: 0x00244778
	public void PlayVfx(string name, Vector2? sourceNormal = null, Vector2? sourceVelocity = null)
	{
		for (int i = 0; i < this.OtherVfx.Count; i++)
		{
			AIAnimator.NamedVFXPool namedVFXPool = this.OtherVfx[i];
			if (namedVFXPool.name == name)
			{
				if (namedVFXPool.anchorTransform)
				{
					namedVFXPool.vfxPool.SpawnAtLocalPosition(Vector3.zero, 0f, namedVFXPool.anchorTransform, sourceNormal, sourceVelocity, true, null, false);
				}
				else
				{
					namedVFXPool.vfxPool.SpawnAtPosition(base.specRigidbody.UnitCenter, 0f, base.transform, sourceNormal, sourceVelocity, null, true, null, null, false);
				}
			}
		}
	}

	// Token: 0x06005ED3 RID: 24275 RVA: 0x0024662C File Offset: 0x0024482C
	public void StopVfx(string name)
	{
		for (int i = 0; i < this.OtherVfx.Count; i++)
		{
			AIAnimator.NamedVFXPool namedVFXPool = this.OtherVfx[i];
			if (namedVFXPool.name == name)
			{
				namedVFXPool.vfxPool.DestroyAll();
			}
		}
	}

	// Token: 0x04005905 RID: 22789
	public List<AIAnimator.NamedVFXPool> OtherVfx;
}
