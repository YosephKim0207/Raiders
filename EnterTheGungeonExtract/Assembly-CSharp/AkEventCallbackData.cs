using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018EB RID: 6379
public class AkEventCallbackData : ScriptableObject
{
	// Token: 0x04009EB2 RID: 40626
	public List<int> callbackFlags = new List<int>();

	// Token: 0x04009EB3 RID: 40627
	public List<string> callbackFunc = new List<string>();

	// Token: 0x04009EB4 RID: 40628
	public List<GameObject> callbackGameObj = new List<GameObject>();

	// Token: 0x04009EB5 RID: 40629
	public int uFlags;
}
