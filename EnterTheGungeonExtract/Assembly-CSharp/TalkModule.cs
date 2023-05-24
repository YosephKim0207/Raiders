using System;
using System.Collections.Generic;

// Token: 0x0200121F RID: 4639
[Serializable]
public class TalkModule
{
	// Token: 0x060067C3 RID: 26563 RVA: 0x0028A2A4 File Offset: 0x002884A4
	public void CopyFrom(TalkModule source)
	{
		this.moduleID = source.moduleID + " copy";
		this.stringKeys = new List<string>(source.stringKeys).ToArray();
		this.sequentialStrings = source.sequentialStrings;
		this.usesAnimation = source.usesAnimation;
		this.animationName = source.animationName;
		this.animationDuration = source.animationDuration;
		this.additionalAnimationName = source.additionalAnimationName;
		this.responses = new List<TalkResponse>(source.responses);
		this.moduleResultActions = new List<TalkResult>(source.moduleResultActions);
	}

	// Token: 0x040063BF RID: 25535
	public string moduleID;

	// Token: 0x040063C0 RID: 25536
	public string[] stringKeys;

	// Token: 0x040063C1 RID: 25537
	public bool sequentialStrings;

	// Token: 0x040063C2 RID: 25538
	[NonSerialized]
	public int sequentialStringLastIndex = -1;

	// Token: 0x040063C3 RID: 25539
	public bool usesAnimation;

	// Token: 0x040063C4 RID: 25540
	[ShowInInspectorIf("usesAnimation", false)]
	public string animationName = string.Empty;

	// Token: 0x040063C5 RID: 25541
	[ShowInInspectorIf("usesAnimation", false)]
	public float animationDuration = -1f;

	// Token: 0x040063C6 RID: 25542
	public string additionalAnimationName = string.Empty;

	// Token: 0x040063C7 RID: 25543
	public List<TalkResponse> responses;

	// Token: 0x040063C8 RID: 25544
	public string noResponseFollowupModule = string.Empty;

	// Token: 0x040063C9 RID: 25545
	public List<TalkResult> moduleResultActions;
}
