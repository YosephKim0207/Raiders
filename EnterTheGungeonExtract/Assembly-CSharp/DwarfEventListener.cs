using System;
using System.Collections.Generic;

// Token: 0x02000F73 RID: 3955
public class DwarfEventListener : BraveBehaviour, IEventTriggerable
{
	// Token: 0x06005541 RID: 21825 RVA: 0x00206440 File Offset: 0x00204640
	public void Trigger(int index)
	{
		if (this.OnTrigger != null)
		{
			this.OnTrigger(index);
		}
		for (int i = 0; i < this.events.Count; i++)
		{
			if (this.events[i].eventIndex == index)
			{
				base.SendPlaymakerEvent(this.events[i].playmakerEvent);
			}
		}
	}

	// Token: 0x06005542 RID: 21826 RVA: 0x002064B0 File Offset: 0x002046B0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04004E2A RID: 20010
	public List<DwarfEventListener.Pair> events;

	// Token: 0x04004E2B RID: 20011
	public Action<int> OnTrigger;

	// Token: 0x02000F74 RID: 3956
	[Serializable]
	public class Pair
	{
		// Token: 0x04004E2C RID: 20012
		public int eventIndex;

		// Token: 0x04004E2D RID: 20013
		public string playmakerEvent;
	}
}
