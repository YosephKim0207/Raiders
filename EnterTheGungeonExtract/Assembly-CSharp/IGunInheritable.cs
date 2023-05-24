using System;
using System.Collections.Generic;

// Token: 0x02001449 RID: 5193
public interface IGunInheritable
{
	// Token: 0x060075E6 RID: 30182
	void InheritData(Gun sourceGun);

	// Token: 0x060075E7 RID: 30183
	void MidGameSerialize(List<object> data, int dataIndex);

	// Token: 0x060075E8 RID: 30184
	void MidGameDeserialize(List<object> data, ref int dataIndex);
}
