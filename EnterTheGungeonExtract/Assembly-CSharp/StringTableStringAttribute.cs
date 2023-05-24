using System;
using UnityEngine;

// Token: 0x02000D00 RID: 3328
public class StringTableStringAttribute : PropertyAttribute
{
	// Token: 0x06004652 RID: 18002 RVA: 0x0016D414 File Offset: 0x0016B614
	public StringTableStringAttribute(string tableTarget = null)
	{
		this.stringTableTarget = tableTarget;
	}

	// Token: 0x040038C8 RID: 14536
	public string stringTableTarget;

	// Token: 0x040038C9 RID: 14537
	public bool isInToggledState;

	// Token: 0x040038CA RID: 14538
	public string keyToWrite = string.Empty;

	// Token: 0x040038CB RID: 14539
	public StringTableStringAttribute.TargetStringTableType targetStringTable;

	// Token: 0x02000D01 RID: 3329
	public enum TargetStringTableType
	{
		// Token: 0x040038CD RID: 14541
		DEFAULT,
		// Token: 0x040038CE RID: 14542
		ENEMIES,
		// Token: 0x040038CF RID: 14543
		ITEMS,
		// Token: 0x040038D0 RID: 14544
		UI
	}
}
