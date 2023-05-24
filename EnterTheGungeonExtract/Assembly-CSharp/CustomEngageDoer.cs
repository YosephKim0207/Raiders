using System;
using System.Collections;

// Token: 0x02000F8E RID: 3982
public abstract class CustomEngageDoer : BraveBehaviour
{
	// Token: 0x06005643 RID: 22083 RVA: 0x0020E3DC File Offset: 0x0020C5DC
	public virtual void StartIntro()
	{
	}

	// Token: 0x17000C37 RID: 3127
	// (get) Token: 0x06005644 RID: 22084 RVA: 0x0020E3E0 File Offset: 0x0020C5E0
	public virtual bool IsFinished
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06005645 RID: 22085 RVA: 0x0020E3E4 File Offset: 0x0020C5E4
	public virtual void OnCleanup()
	{
	}

	// Token: 0x06005646 RID: 22086 RVA: 0x0020E3E8 File Offset: 0x0020C5E8
	public IEnumerator TimeInvariantWait(float duration)
	{
		for (float elapsed = 0f; elapsed < duration; elapsed += GameManager.INVARIANT_DELTA_TIME)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06005647 RID: 22087 RVA: 0x0020E404 File Offset: 0x0020C604
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
