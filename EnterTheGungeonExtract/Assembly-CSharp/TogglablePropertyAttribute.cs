using System;
using UnityEngine;

// Token: 0x02000D04 RID: 3332
public class TogglablePropertyAttribute : PropertyAttribute
{
	// Token: 0x06004655 RID: 18005 RVA: 0x0016D440 File Offset: 0x0016B640
	public TogglablePropertyAttribute(string togglePropertyName, string label = null)
	{
		this.TogglePropertyName = togglePropertyName;
		this.Label = label;
	}

	// Token: 0x040038D1 RID: 14545
	public string TogglePropertyName;

	// Token: 0x040038D2 RID: 14546
	public string Label;
}
