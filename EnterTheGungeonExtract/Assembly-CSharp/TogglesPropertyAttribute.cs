using System;
using UnityEngine;

// Token: 0x02000D05 RID: 3333
public class TogglesPropertyAttribute : PropertyAttribute
{
	// Token: 0x06004656 RID: 18006 RVA: 0x0016D458 File Offset: 0x0016B658
	public TogglesPropertyAttribute(string propertyName, string label = null)
	{
		this.PropertyName = propertyName;
		this.Label = label;
	}

	// Token: 0x040038D3 RID: 14547
	public string PropertyName;

	// Token: 0x040038D4 RID: 14548
	public string Label;
}
