using System;
using UnityEngine;

// Token: 0x02000CFB RID: 3323
public class PrettyDungeonMaterialAttribute : PropertyAttribute
{
	// Token: 0x06004649 RID: 17993 RVA: 0x0016D3AC File Offset: 0x0016B5AC
	public PrettyDungeonMaterialAttribute(string tilesetPropertyName)
	{
		this.tilesetProperty = tilesetPropertyName;
	}

	// Token: 0x040038C5 RID: 14533
	public string tilesetProperty;
}
