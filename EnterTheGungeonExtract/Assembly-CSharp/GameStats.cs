using System;
using System.Collections.Generic;
using FullSerializer;
using UnityEngine;

// Token: 0x020014F9 RID: 5369
[fsObject]
public class GameStats
{
	// Token: 0x06007A43 RID: 31299 RVA: 0x00310030 File Offset: 0x0030E230
	public GameStats()
	{
		this.stats = new Dictionary<TrackedStats, float>(new TrackedStatsComparer());
		this.maxima = new Dictionary<TrackedMaximums, float>(new TrackedMaximumsComparer());
	}

	// Token: 0x06007A44 RID: 31300 RVA: 0x00310064 File Offset: 0x0030E264
	public float GetStatValue(TrackedStats statToCheck)
	{
		if (!this.stats.ContainsKey(statToCheck))
		{
			return 0f;
		}
		return this.stats[statToCheck];
	}

	// Token: 0x06007A45 RID: 31301 RVA: 0x0031008C File Offset: 0x0030E28C
	public float GetMaximumValue(TrackedMaximums maxToCheck)
	{
		if (!this.maxima.ContainsKey(maxToCheck))
		{
			return 0f;
		}
		return this.maxima[maxToCheck];
	}

	// Token: 0x06007A46 RID: 31302 RVA: 0x003100B4 File Offset: 0x0030E2B4
	public bool GetFlag(CharacterSpecificGungeonFlags flag)
	{
		if (flag == CharacterSpecificGungeonFlags.NONE)
		{
			Debug.LogError("Something is attempting to get a NONE character-specific save flag!");
			return false;
		}
		return this.m_flags.Contains(flag);
	}

	// Token: 0x06007A47 RID: 31303 RVA: 0x003100D4 File Offset: 0x0030E2D4
	public void SetStat(TrackedStats stat, float val)
	{
		if (this.stats.ContainsKey(stat))
		{
			this.stats[stat] = val;
		}
		else
		{
			this.stats.Add(stat, val);
		}
	}

	// Token: 0x06007A48 RID: 31304 RVA: 0x00310108 File Offset: 0x0030E308
	public void SetMax(TrackedMaximums max, float val)
	{
		if (this.maxima.ContainsKey(max))
		{
			this.maxima[max] = Mathf.Max(this.maxima[max], val);
		}
		else
		{
			this.maxima.Add(max, val);
		}
	}

	// Token: 0x06007A49 RID: 31305 RVA: 0x00310158 File Offset: 0x0030E358
	public void SetFlag(CharacterSpecificGungeonFlags flag, bool value)
	{
		if (flag == CharacterSpecificGungeonFlags.NONE)
		{
			Debug.LogError("Something is attempting to set a NONE character-specific save flag!");
			return;
		}
		if (value)
		{
			this.m_flags.Add(flag);
		}
		else
		{
			this.m_flags.Remove(flag);
		}
	}

	// Token: 0x06007A4A RID: 31306 RVA: 0x00310190 File Offset: 0x0030E390
	public void IncrementStat(TrackedStats stat, float val)
	{
		if (this.stats.ContainsKey(stat))
		{
			this.stats[stat] = this.stats[stat] + val;
		}
		else
		{
			this.stats.Add(stat, val);
		}
	}

	// Token: 0x06007A4B RID: 31307 RVA: 0x003101D0 File Offset: 0x0030E3D0
	public void AddStats(GameStats otherStats)
	{
		foreach (KeyValuePair<TrackedStats, float> keyValuePair in otherStats.stats)
		{
			this.IncrementStat(keyValuePair.Key, keyValuePair.Value);
		}
		foreach (KeyValuePair<TrackedMaximums, float> keyValuePair2 in otherStats.maxima)
		{
			this.SetMax(keyValuePair2.Key, keyValuePair2.Value);
		}
		foreach (CharacterSpecificGungeonFlags characterSpecificGungeonFlags in otherStats.m_flags)
		{
			this.m_flags.Add(characterSpecificGungeonFlags);
		}
	}

	// Token: 0x06007A4C RID: 31308 RVA: 0x003102E8 File Offset: 0x0030E4E8
	public void ClearAllState()
	{
		List<TrackedStats> list = new List<TrackedStats>();
		foreach (KeyValuePair<TrackedStats, float> keyValuePair in this.stats)
		{
			list.Add(keyValuePair.Key);
		}
		foreach (TrackedStats trackedStats in list)
		{
			this.stats[trackedStats] = 0f;
		}
		List<TrackedMaximums> list2 = new List<TrackedMaximums>();
		foreach (KeyValuePair<TrackedMaximums, float> keyValuePair2 in this.maxima)
		{
			list2.Add(keyValuePair2.Key);
		}
		foreach (TrackedMaximums trackedMaximums in list2)
		{
			this.maxima[trackedMaximums] = 0f;
		}
	}

	// Token: 0x04007D0F RID: 32015
	[fsProperty]
	private Dictionary<TrackedStats, float> stats;

	// Token: 0x04007D10 RID: 32016
	[fsProperty]
	private Dictionary<TrackedMaximums, float> maxima;

	// Token: 0x04007D11 RID: 32017
	[fsProperty]
	public HashSet<CharacterSpecificGungeonFlags> m_flags = new HashSet<CharacterSpecificGungeonFlags>();
}
