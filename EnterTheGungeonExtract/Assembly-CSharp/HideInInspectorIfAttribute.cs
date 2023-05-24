using System;

// Token: 0x02000CFF RID: 3327
public class HideInInspectorIfAttribute : ShowInInspectorIfAttribute
{
	// Token: 0x06004650 RID: 18000 RVA: 0x0016D3FC File Offset: 0x0016B5FC
	public HideInInspectorIfAttribute(string propertyName, bool indent = false)
		: base(propertyName, indent)
	{
	}

	// Token: 0x06004651 RID: 18001 RVA: 0x0016D408 File Offset: 0x0016B608
	public HideInInspectorIfAttribute(string propertyName, int value, bool indent = false)
		: base(propertyName, value, indent)
	{
	}
}
