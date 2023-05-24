using System;
using System.Collections;

// Token: 0x02000F8C RID: 3980
public abstract class CustomReinforceDoer : BraveBehaviour
{
	// Token: 0x06005637 RID: 22071 RVA: 0x0020E2EC File Offset: 0x0020C4EC
	public virtual void StartIntro()
	{
	}

	// Token: 0x17000C34 RID: 3124
	// (get) Token: 0x06005638 RID: 22072 RVA: 0x0020E2F0 File Offset: 0x0020C4F0
	public virtual bool IsFinished
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06005639 RID: 22073 RVA: 0x0020E2F4 File Offset: 0x0020C4F4
	public virtual void OnCleanup()
	{
	}

	// Token: 0x0600563A RID: 22074 RVA: 0x0020E2F8 File Offset: 0x0020C4F8
	public IEnumerator TimeInvariantWait(float duration)
	{
		for (float elapsed = 0f; elapsed < duration; elapsed += GameManager.INVARIANT_DELTA_TIME)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600563B RID: 22075 RVA: 0x0020E314 File Offset: 0x0020C514
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
