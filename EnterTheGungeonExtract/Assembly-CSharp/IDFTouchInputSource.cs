using System;
using System.Collections.Generic;

// Token: 0x02000428 RID: 1064
public interface IDFTouchInputSource
{
	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x06001868 RID: 6248
	int TouchCount { get; }

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x06001869 RID: 6249
	IList<dfTouchInfo> Touches { get; }

	// Token: 0x0600186A RID: 6250
	void Update();

	// Token: 0x0600186B RID: 6251
	dfTouchInfo GetTouch(int index);
}
