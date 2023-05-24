using System;
using UnityEngine;

// Token: 0x02000CEF RID: 3311
public class CheckSpriteAttribute : PropertyAttribute
{
	// Token: 0x0600463C RID: 17980 RVA: 0x0016D2E8 File Offset: 0x0016B4E8
	public CheckSpriteAttribute(string sprite = null)
	{
		this.sprite = sprite;
	}

	// Token: 0x040038C0 RID: 14528
	public string sprite;
}
