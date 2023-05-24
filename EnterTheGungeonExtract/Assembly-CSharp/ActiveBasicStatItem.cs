using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CE9 RID: 3305
public class ActiveBasicStatItem : PlayerItem
{
	// Token: 0x0600462D RID: 17965 RVA: 0x0016D044 File Offset: 0x0016B244
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
		this.m_cachedUser = user;
		base.StartCoroutine(this.HandleDuration(user));
	}

	// Token: 0x0600462E RID: 17966 RVA: 0x0016D06C File Offset: 0x0016B26C
	private IEnumerator HandleDuration(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			Debug.LogError("Using a ActiveBasicStatItem while it is already active!");
			yield break;
		}
		base.IsCurrentlyActive = true;
		user.stats.RecalculateStats(user, false, false);
		this.m_activeElapsed = 0f;
		this.m_activeDuration = this.duration;
		while (this.m_activeElapsed < this.m_activeDuration && base.IsCurrentlyActive)
		{
			yield return null;
		}
		base.IsCurrentlyActive = false;
		user.stats.RecalculateStats(user, false, false);
		yield break;
	}

	// Token: 0x0600462F RID: 17967 RVA: 0x0016D090 File Offset: 0x0016B290
	protected override void OnPreDrop(PlayerController user)
	{
		if (base.IsCurrentlyActive && this.m_cachedUser)
		{
			this.m_cachedUser.stats.RecalculateStats(this.m_cachedUser, false, false);
		}
		this.m_cachedUser = null;
		base.OnPreDrop(user);
	}

	// Token: 0x06004630 RID: 17968 RVA: 0x0016D0E0 File Offset: 0x0016B2E0
	public override void OnItemSwitched(PlayerController user)
	{
		base.OnItemSwitched(user);
		base.IsCurrentlyActive = false;
		if (this.m_cachedUser)
		{
			this.m_cachedUser.stats.RecalculateStats(this.m_cachedUser, false, false);
		}
	}

	// Token: 0x06004631 RID: 17969 RVA: 0x0016D118 File Offset: 0x0016B318
	protected override void OnDestroy()
	{
		base.IsCurrentlyActive = false;
		if (this.m_cachedUser)
		{
			this.m_cachedUser.stats.RecalculateStats(this.m_cachedUser, false, false);
		}
		this.m_cachedUser = null;
		base.OnDestroy();
	}

	// Token: 0x040038B2 RID: 14514
	public float duration = 5f;

	// Token: 0x040038B3 RID: 14515
	[BetterList]
	public List<StatModifier> modifiers;

	// Token: 0x040038B4 RID: 14516
	private PlayerController m_cachedUser;
}
