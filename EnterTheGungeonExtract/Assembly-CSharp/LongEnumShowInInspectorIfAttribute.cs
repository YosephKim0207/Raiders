using System;
using UnityEngine;

// Token: 0x02000CFC RID: 3324
public class LongEnumShowInInspectorIfAttribute : PropertyAttribute
{
	// Token: 0x0600464A RID: 17994 RVA: 0x0016D3BC File Offset: 0x0016B5BC
	public LongEnumShowInInspectorIfAttribute(string propertyName, bool value = true, bool indent = false)
	{
	}

	// Token: 0x0600464B RID: 17995 RVA: 0x0016D3C4 File Offset: 0x0016B5C4
	public LongEnumShowInInspectorIfAttribute(string propertyName, int value, bool indent = false)
	{
	}

	// Token: 0x0600464C RID: 17996 RVA: 0x0016D3CC File Offset: 0x0016B5CC
	public LongEnumShowInInspectorIfAttribute(string propertyName, UnityEngine.Object value, bool indent = false)
	{
	}
}
