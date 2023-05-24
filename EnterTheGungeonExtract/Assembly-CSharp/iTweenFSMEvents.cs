using System;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

// Token: 0x02000B72 RID: 2930
public class iTweenFSMEvents : MonoBehaviour
{
	// Token: 0x06003D51 RID: 15697 RVA: 0x00132DEC File Offset: 0x00130FEC
	private void iTweenOnStart(int aniTweenID)
	{
		if (this.itweenID == aniTweenID)
		{
			this.itweenFSMAction.Fsm.Event(this.itweenFSMAction.startEvent);
		}
	}

	// Token: 0x06003D52 RID: 15698 RVA: 0x00132E18 File Offset: 0x00131018
	private void iTweenOnComplete(int aniTweenID)
	{
		if (this.itweenID == aniTweenID)
		{
			if (this.islooping)
			{
				if (!this.donotfinish)
				{
					this.itweenFSMAction.Fsm.Event(this.itweenFSMAction.finishEvent);
					this.itweenFSMAction.Finish();
				}
			}
			else
			{
				this.itweenFSMAction.Fsm.Event(this.itweenFSMAction.finishEvent);
				this.itweenFSMAction.Finish();
			}
		}
	}

	// Token: 0x04002FA8 RID: 12200
	public static int itweenIDCount;

	// Token: 0x04002FA9 RID: 12201
	public int itweenID;

	// Token: 0x04002FAA RID: 12202
	public iTweenFsmAction itweenFSMAction;

	// Token: 0x04002FAB RID: 12203
	public bool donotfinish;

	// Token: 0x04002FAC RID: 12204
	public bool islooping;
}
