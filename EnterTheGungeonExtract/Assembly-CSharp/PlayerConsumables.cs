using System;
using UnityEngine;

// Token: 0x020015F6 RID: 5622
[Serializable]
public class PlayerConsumables
{
	// Token: 0x17001373 RID: 4979
	// (get) Token: 0x0600826E RID: 33390 RVA: 0x00355100 File Offset: 0x00353300
	// (set) Token: 0x0600826F RID: 33391 RVA: 0x00355108 File Offset: 0x00353308
	public int Currency
	{
		get
		{
			return this.m_currency;
		}
		set
		{
			int num = Mathf.Max(0, value);
			if (num > this.m_currency && GameStatsManager.HasInstance)
			{
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.TOTAL_MONEY_COLLECTED, (float)(num - this.m_currency));
			}
			if (num >= 300 && GameManager.HasInstance && GameManager.Instance.platformInterface != null)
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				if (realtimeSinceStartup > PlatformInterface.LastManyCoinsUnlockTime + 5f || realtimeSinceStartup < PlatformInterface.LastManyCoinsUnlockTime)
				{
					GameManager.Instance.platformInterface.AchievementUnlock(Achievement.HAVE_MANY_COINS, 0);
					PlatformInterface.LastManyCoinsUnlockTime = realtimeSinceStartup;
				}
			}
			this.m_currency = num;
			if (GameUIRoot.HasInstance)
			{
				GameUIRoot.Instance.UpdatePlayerConsumables(this);
			}
		}
	}

	// Token: 0x17001374 RID: 4980
	// (get) Token: 0x06008270 RID: 33392 RVA: 0x003551C4 File Offset: 0x003533C4
	// (set) Token: 0x06008271 RID: 33393 RVA: 0x003551CC File Offset: 0x003533CC
	public int KeyBullets
	{
		get
		{
			return this.m_keyBullets;
		}
		set
		{
			this.m_keyBullets = value;
			GameStatsManager.Instance.UpdateMaximum(TrackedMaximums.MOST_KEYS_HELD, (float)this.m_keyBullets);
			GameUIRoot.Instance.UpdatePlayerConsumables(this);
		}
	}

	// Token: 0x17001375 RID: 4981
	// (get) Token: 0x06008272 RID: 33394 RVA: 0x003551F4 File Offset: 0x003533F4
	// (set) Token: 0x06008273 RID: 33395 RVA: 0x003551FC File Offset: 0x003533FC
	public int ResourcefulRatKeys
	{
		get
		{
			return this.m_ratKeys;
		}
		set
		{
			this.m_ratKeys = value;
			GameUIRoot.Instance.UpdatePlayerConsumables(this);
		}
	}

	// Token: 0x06008274 RID: 33396 RVA: 0x00355210 File Offset: 0x00353410
	public void Initialize()
	{
		this.Currency = this.StartCurrency;
		this.KeyBullets = this.StartKeyBullets;
	}

	// Token: 0x06008275 RID: 33397 RVA: 0x0035522C File Offset: 0x0035342C
	public void ForceUpdateUI()
	{
		if (GameUIRoot.Instance != null)
		{
			GameUIRoot.Instance.UpdatePlayerConsumables(this);
		}
	}

	// Token: 0x17001376 RID: 4982
	// (get) Token: 0x06008276 RID: 33398 RVA: 0x0035524C File Offset: 0x0035344C
	// (set) Token: 0x06008277 RID: 33399 RVA: 0x003552D0 File Offset: 0x003534D0
	public bool InfiniteKeys
	{
		get
		{
			if (GameManager.Instance.AllPlayers != null)
			{
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					PlayerController playerController = GameManager.Instance.AllPlayers[i];
					if (playerController && playerController.CurrentGun && playerController.CurrentGun.gunName == "AKey-47")
					{
						return true;
					}
				}
			}
			return this.m_infiniteKeys;
		}
		set
		{
			this.m_infiniteKeys = value;
		}
	}

	// Token: 0x0400857A RID: 34170
	[NonSerialized]
	private bool m_infiniteKeys;

	// Token: 0x0400857B RID: 34171
	[SerializeField]
	private int StartCurrency;

	// Token: 0x0400857C RID: 34172
	[SerializeField]
	private int StartKeyBullets = 1;

	// Token: 0x0400857D RID: 34173
	private int m_currency;

	// Token: 0x0400857E RID: 34174
	private int m_keyBullets;

	// Token: 0x0400857F RID: 34175
	private int m_ratKeys;
}
