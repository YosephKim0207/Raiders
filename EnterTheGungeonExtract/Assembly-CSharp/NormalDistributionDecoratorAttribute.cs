using System;
using UnityEngine;

// Token: 0x02000CFD RID: 3325
public class NormalDistributionDecoratorAttribute : PropertyAttribute
{
	// Token: 0x0600464D RID: 17997 RVA: 0x0016D3D4 File Offset: 0x0016B5D4
	public NormalDistributionDecoratorAttribute(string meanPropertyName, string devPropertyName)
	{
		this.MeanProperty = meanPropertyName;
		this.StdDevProperty = devPropertyName;
	}

	// Token: 0x040038C6 RID: 14534
	public string MeanProperty;

	// Token: 0x040038C7 RID: 14535
	public string StdDevProperty;
}
