using System;
using UnityEngine;

// Token: 0x020012C9 RID: 4809
[ExecuteInEditMode]
public class WaterTile : MonoBehaviour
{
	// Token: 0x06006B98 RID: 27544 RVA: 0x002A46DC File Offset: 0x002A28DC
	public void Start()
	{
		this.AcquireComponents();
	}

	// Token: 0x06006B99 RID: 27545 RVA: 0x002A46E4 File Offset: 0x002A28E4
	private void AcquireComponents()
	{
		if (!this.reflection)
		{
			if (base.transform.parent)
			{
				this.reflection = base.transform.parent.GetComponent<PlanarReflection>();
			}
			else
			{
				this.reflection = base.transform.GetComponent<PlanarReflection>();
			}
		}
		if (!this.waterBase)
		{
			if (base.transform.parent)
			{
				this.waterBase = base.transform.parent.GetComponent<WaterBase>();
			}
			else
			{
				this.waterBase = base.transform.GetComponent<WaterBase>();
			}
		}
	}

	// Token: 0x06006B9A RID: 27546 RVA: 0x002A4794 File Offset: 0x002A2994
	public void OnWillRenderObject()
	{
		if (this.reflection)
		{
			this.reflection.WaterTileBeingRendered(base.transform, Camera.current);
		}
		if (this.waterBase)
		{
			this.waterBase.WaterTileBeingRendered(base.transform, Camera.current);
		}
	}

	// Token: 0x0400688B RID: 26763
	public PlanarReflection reflection;

	// Token: 0x0400688C RID: 26764
	public WaterBase waterBase;
}
