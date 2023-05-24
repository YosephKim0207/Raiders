using System;
using UnityEngine;

// Token: 0x02000CED RID: 3309
public class CheckDirectionalAnimationAttribute : PropertyAttribute
{
	// Token: 0x0600463A RID: 17978 RVA: 0x0016D2C8 File Offset: 0x0016B4C8
	public CheckDirectionalAnimationAttribute(string aiAnimator = null)
	{
		this.aiAnimator = aiAnimator;
	}

	// Token: 0x040038BE RID: 14526
	public string aiAnimator;
}
