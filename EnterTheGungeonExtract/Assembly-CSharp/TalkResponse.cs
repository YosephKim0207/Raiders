using System;
using System.Collections.Generic;

// Token: 0x02001220 RID: 4640
[Serializable]
public class TalkResponse
{
	// Token: 0x040063CA RID: 25546
	public string response;

	// Token: 0x040063CB RID: 25547
	public string followupModuleID;

	// Token: 0x040063CC RID: 25548
	public List<TalkResult> resultActions;
}
