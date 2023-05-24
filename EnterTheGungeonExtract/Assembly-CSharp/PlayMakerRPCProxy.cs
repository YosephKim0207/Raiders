using System;
using UnityEngine;

// Token: 0x02000A4B RID: 2635
public class PlayMakerRPCProxy : MonoBehaviour
{
	// Token: 0x0600381F RID: 14367 RVA: 0x00120124 File Offset: 0x0011E324
	public void Reset()
	{
		this.fsms = base.GetComponents<PlayMakerFSM>();
	}

	// Token: 0x06003820 RID: 14368 RVA: 0x00120134 File Offset: 0x0011E334
	public void ForwardEvent(string eventName)
	{
		foreach (PlayMakerFSM playMakerFSM in this.fsms)
		{
			playMakerFSM.SendEvent(eventName);
		}
	}

	// Token: 0x04002A1C RID: 10780
	public PlayMakerFSM[] fsms;
}
