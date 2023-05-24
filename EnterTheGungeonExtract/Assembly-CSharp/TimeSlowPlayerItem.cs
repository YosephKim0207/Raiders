using System;
using System.Collections;

// Token: 0x020014DD RID: 5341
public class TimeSlowPlayerItem : PlayerItem
{
	// Token: 0x06007977 RID: 31095 RVA: 0x00309A5C File Offset: 0x00307C5C
	protected override void DoEffect(PlayerController user)
	{
		user.StartCoroutine(this.HandleDuration(user));
	}

	// Token: 0x06007978 RID: 31096 RVA: 0x00309A6C File Offset: 0x00307C6C
	private IEnumerator HandleDuration(PlayerController user)
	{
		AkSoundEngine.PostEvent("State_Bullet_Time_on", base.gameObject);
		base.IsCurrentlyActive = true;
		this.m_activeElapsed = 0f;
		this.m_activeDuration = this.duration;
		float num = ((!this.HasSynergy || !user || !user.HasActiveBonusSynergy(this.RequiredSynergy, false)) ? this.timeScale : this.overrideTimeScale);
		this.test.DoRadialSlow(user.CenterPosition, user.CurrentRoom);
		float ela = 0f;
		while (ela < this.m_activeDuration)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			this.m_activeElapsed = ela;
			yield return null;
		}
		if (this)
		{
			AkSoundEngine.PostEvent("State_Bullet_Time_off", base.gameObject);
		}
		base.IsCurrentlyActive = false;
		yield break;
	}

	// Token: 0x06007979 RID: 31097 RVA: 0x00309A90 File Offset: 0x00307C90
	protected override void OnPreDrop(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			base.StopAllCoroutines();
			AkSoundEngine.PostEvent("State_Bullet_Time_off", base.gameObject);
			BraveTime.ClearMultiplier(base.gameObject);
			base.IsCurrentlyActive = false;
		}
	}

	// Token: 0x0600797A RID: 31098 RVA: 0x00309AC8 File Offset: 0x00307CC8
	protected override void OnDestroy()
	{
		if (base.IsCurrentlyActive)
		{
			base.StopAllCoroutines();
			AkSoundEngine.PostEvent("State_Bullet_Time_off", base.gameObject);
			BraveTime.ClearMultiplier(base.gameObject);
			base.IsCurrentlyActive = false;
		}
		base.OnDestroy();
	}

	// Token: 0x04007BEF RID: 31727
	public float timeScale = 0.5f;

	// Token: 0x04007BF0 RID: 31728
	public float duration = 5f;

	// Token: 0x04007BF1 RID: 31729
	public bool HasSynergy;

	// Token: 0x04007BF2 RID: 31730
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04007BF3 RID: 31731
	public float overrideTimeScale;

	// Token: 0x04007BF4 RID: 31732
	public RadialSlowInterface test;
}
