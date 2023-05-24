using System;
using UnityEngine;

// Token: 0x02000CEE RID: 3310
public class CheckAnimationAttribute : PropertyAttribute
{
	// Token: 0x0600463B RID: 17979 RVA: 0x0016D2D8 File Offset: 0x0016B4D8
	public CheckAnimationAttribute(string animator = null)
	{
		this.animator = animator;
	}

	// Token: 0x040038BF RID: 14527
	public string animator;
}
