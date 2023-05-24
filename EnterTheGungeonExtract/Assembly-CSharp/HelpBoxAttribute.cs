using System;
using UnityEngine;

// Token: 0x02000CF8 RID: 3320
public class HelpBoxAttribute : PropertyAttribute
{
	// Token: 0x06004646 RID: 17990 RVA: 0x0016D38C File Offset: 0x0016B58C
	public HelpBoxAttribute(string message)
	{
		this.Message = message;
	}

	// Token: 0x040038C4 RID: 14532
	public string Message;
}
