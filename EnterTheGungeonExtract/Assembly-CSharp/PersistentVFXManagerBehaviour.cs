using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015BC RID: 5564
public class PersistentVFXManagerBehaviour : BraveBehaviour
{
	// Token: 0x06007FCD RID: 32717 RVA: 0x00339FE4 File Offset: 0x003381E4
	public void AttachPersistentVFX(GameObject vfx)
	{
		if (this.attachedPersistentVFX == null)
		{
			this.attachedPersistentVFX = new List<GameObject>();
		}
		this.attachedPersistentVFX.Add(vfx);
		vfx.transform.parent = base.transform;
	}

	// Token: 0x06007FCE RID: 32718 RVA: 0x0033A01C File Offset: 0x0033821C
	public void AttachDestructibleVFX(GameObject vfx)
	{
		if (this.m_pvmbDestroyed)
		{
			UnityEngine.Object.Destroy(vfx);
		}
		else
		{
			if (this.attachedDestructibleVFX == null)
			{
				this.attachedDestructibleVFX = new List<GameObject>();
			}
			this.attachedDestructibleVFX.Add(vfx);
			vfx.transform.parent = base.transform;
		}
	}

	// Token: 0x06007FCF RID: 32719 RVA: 0x0033A074 File Offset: 0x00338274
	public void TriggerPersistentVFXClear()
	{
		this.TriggerPersistentVFXClear(Vector3.right, 180f, 0.5f, 0.3f, 0.7f);
	}

	// Token: 0x06007FD0 RID: 32720 RVA: 0x0033A098 File Offset: 0x00338298
	public void TriggerPersistentVFXClear(Vector3 startingForce, float forceVarianceAngle, float forceVarianceMagnitude, float startingHeightMin, float startingHeightMax)
	{
		if (this.attachedPersistentVFX != null)
		{
			for (int i = 0; i < this.attachedPersistentVFX.Count; i++)
			{
				Vector3 vector = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-forceVarianceAngle, forceVarianceAngle)) * startingForce * (1f + UnityEngine.Random.Range(-forceVarianceMagnitude, forceVarianceMagnitude));
				float num = UnityEngine.Random.Range(startingHeightMin, startingHeightMax);
				if (this.attachedPersistentVFX[i])
				{
					this.attachedPersistentVFX[i].transform.parent = null;
					this.attachedPersistentVFX[i].GetComponent<PersistentVFXBehaviour>().BecomeDebris(vector, num, new Type[0]);
				}
			}
			this.attachedPersistentVFX.Clear();
		}
		if (this.attachedDestructibleVFX != null)
		{
			this.TriggerDestructibleVFXClear();
		}
	}

	// Token: 0x06007FD1 RID: 32721 RVA: 0x0033A170 File Offset: 0x00338370
	public void TriggerTemporaryDestructibleVFXClear()
	{
		if (this.attachedDestructibleVFX == null)
		{
			return;
		}
		for (int i = 0; i < this.attachedDestructibleVFX.Count; i++)
		{
			UnityEngine.Object.Destroy(this.attachedDestructibleVFX[i]);
		}
		this.attachedDestructibleVFX.Clear();
	}

	// Token: 0x06007FD2 RID: 32722 RVA: 0x0033A1C4 File Offset: 0x003383C4
	public void TriggerDestructibleVFXClear()
	{
		this.m_pvmbDestroyed = true;
		if (this.attachedDestructibleVFX == null)
		{
			return;
		}
		for (int i = 0; i < this.attachedDestructibleVFX.Count; i++)
		{
			UnityEngine.Object.Destroy(this.attachedDestructibleVFX[i]);
		}
		this.attachedDestructibleVFX.Clear();
	}

	// Token: 0x06007FD3 RID: 32723 RVA: 0x0033A21C File Offset: 0x0033841C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008252 RID: 33362
	protected List<GameObject> attachedPersistentVFX;

	// Token: 0x04008253 RID: 33363
	protected List<GameObject> attachedDestructibleVFX;

	// Token: 0x04008254 RID: 33364
	private bool m_pvmbDestroyed;
}
