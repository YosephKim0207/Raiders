using System;

// Token: 0x02000F72 RID: 3954
[Serializable]
public class DungeonFlowLevelEntry
{
	// Token: 0x04004E23 RID: 20003
	public string flowPath;

	// Token: 0x04004E24 RID: 20004
	public float weight = 1f;

	// Token: 0x04004E25 RID: 20005
	public FlowLevelEntryMode levelMode;

	// Token: 0x04004E26 RID: 20006
	public DungeonPrerequisite[] prerequisites;

	// Token: 0x04004E27 RID: 20007
	public bool forceUseIfAvailable;

	// Token: 0x04004E28 RID: 20008
	public bool overridesLevelDetailText;

	// Token: 0x04004E29 RID: 20009
	public string overrideLevelDetailText = string.Empty;
}
