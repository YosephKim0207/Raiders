using System;

// Token: 0x0200139D RID: 5021
public interface IPaydayItem
{
	// Token: 0x060071C3 RID: 29123
	void StoreData(string id1, string id2, string id3);

	// Token: 0x060071C4 RID: 29124
	string GetID(int placement);

	// Token: 0x060071C5 RID: 29125
	bool HasCachedData();
}
