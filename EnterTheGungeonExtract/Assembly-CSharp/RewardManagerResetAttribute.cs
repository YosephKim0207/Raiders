using System;
using UnityEngine;

// Token: 0x02000CEC RID: 3308
public class RewardManagerResetAttribute : PropertyAttribute
{
	// Token: 0x06004639 RID: 17977 RVA: 0x0016D2A0 File Offset: 0x0016B4A0
	public RewardManagerResetAttribute(string headerMessage, string contentMessage, string callbackFunc, int targetType)
	{
		this.header = headerMessage;
		this.content = contentMessage;
		this.callback = callbackFunc;
		this.targetElement = targetType;
	}

	// Token: 0x040038BA RID: 14522
	public string header;

	// Token: 0x040038BB RID: 14523
	public string content;

	// Token: 0x040038BC RID: 14524
	public string callback;

	// Token: 0x040038BD RID: 14525
	public int targetElement;
}
