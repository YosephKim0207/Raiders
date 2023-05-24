using System;
using UnityEngine;

// Token: 0x02000417 RID: 1047
public interface IDFControlHost
{
	// Token: 0x060017C3 RID: 6083
	T AddControl<T>() where T : dfControl;

	// Token: 0x060017C4 RID: 6084
	dfControl AddControl(Type controlType);

	// Token: 0x060017C5 RID: 6085
	void AddControl(dfControl child);

	// Token: 0x060017C6 RID: 6086
	dfControl AddPrefab(GameObject prefab);
}
