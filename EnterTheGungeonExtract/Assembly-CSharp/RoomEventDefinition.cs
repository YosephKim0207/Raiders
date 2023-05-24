using System;

// Token: 0x02000F15 RID: 3861
[Serializable]
public class RoomEventDefinition
{
	// Token: 0x0600525C RID: 21084 RVA: 0x001D944C File Offset: 0x001D764C
	public RoomEventDefinition(RoomEventTriggerCondition c, RoomEventTriggerAction a)
	{
		this.condition = c;
		this.action = a;
	}

	// Token: 0x04004AFA RID: 19194
	public RoomEventTriggerCondition condition;

	// Token: 0x04004AFB RID: 19195
	public RoomEventTriggerAction action;
}
