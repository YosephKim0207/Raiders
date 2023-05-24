using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200182D RID: 6189
public class OverridableBool
{
	// Token: 0x06009289 RID: 37513 RVA: 0x003DE474 File Offset: 0x003DC674
	public OverridableBool(bool defaultValue)
	{
		this.BaseValue = defaultValue;
	}

	// Token: 0x0600928A RID: 37514 RVA: 0x003DE490 File Offset: 0x003DC690
	public void Debug()
	{
		for (int i = 0; i < this.m_overrides.Count; i++)
		{
			float? duration = this.m_overrides[i].duration;
			string text = ((duration != null) ? this.m_overrides[i].duration.Value.ToString() : "null");
			UnityEngine.Debug.LogWarningFormat("override set: {0} (duration: {1})", new object[]
			{
				this.m_overrides[i],
				text
			});
		}
	}

	// Token: 0x170015E1 RID: 5601
	// (get) Token: 0x0600928B RID: 37515 RVA: 0x003DE52C File Offset: 0x003DC72C
	public bool Value
	{
		get
		{
			return (this.m_overrides.Count <= 0) ? this.BaseValue : (!this.BaseValue);
		}
	}

	// Token: 0x0600928C RID: 37516 RVA: 0x003DE554 File Offset: 0x003DC754
	public bool HasOverride(string key)
	{
		for (int i = 0; i < this.m_overrides.Count; i++)
		{
			if (this.m_overrides[i].key == key)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600928D RID: 37517 RVA: 0x003DE59C File Offset: 0x003DC79C
	public void AddOverride(string key, float? duration = null)
	{
		for (int i = 0; i < this.m_overrides.Count; i++)
		{
			if (this.m_overrides[i].key == key)
			{
				if (duration != null)
				{
					float? duration2 = this.m_overrides[i].duration;
					if (duration2 != null)
					{
						this.m_overrides[i].duration = new float?(Mathf.Max(this.m_overrides[i].duration.Value, duration.Value));
						return;
					}
				}
				this.m_overrides[i].duration = null;
				return;
			}
		}
		this.m_overrides.Add(new OverridableBool.OverrideData
		{
			key = key,
			duration = duration
		});
	}

	// Token: 0x0600928E RID: 37518 RVA: 0x003DE68C File Offset: 0x003DC88C
	public void RemoveOverride(string key)
	{
		for (int i = 0; i < this.m_overrides.Count; i++)
		{
			if (this.m_overrides[i].key == key)
			{
				this.m_overrides.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x0600928F RID: 37519 RVA: 0x003DE6E0 File Offset: 0x003DC8E0
	public void SetOverride(string key, bool value, float? duration = null)
	{
		if (value != this.BaseValue)
		{
			this.AddOverride(key, duration);
		}
		else
		{
			if (duration != null)
			{
				UnityEngine.Debug.LogWarningFormat("Trying to disable an override with a duration! {0} {1} {2}", new object[] { key, value, duration.Value });
			}
			this.RemoveOverride(key);
		}
	}

	// Token: 0x06009290 RID: 37520 RVA: 0x003DE748 File Offset: 0x003DC948
	public void ClearOverrides()
	{
		this.m_overrides.Clear();
	}

	// Token: 0x06009291 RID: 37521 RVA: 0x003DE758 File Offset: 0x003DC958
	public bool UpdateTimers(float deltaTime)
	{
		bool flag = false;
		for (int i = this.m_overrides.Count - 1; i >= 0; i--)
		{
			float? duration = this.m_overrides[i].duration;
			if (duration != null)
			{
				OverridableBool.OverrideData overrideData = this.m_overrides[i];
				float? duration2 = overrideData.duration;
				overrideData.duration = ((duration2 == null) ? null : new float?(duration2.GetValueOrDefault() - deltaTime));
				if (this.m_overrides[i].duration <= 0f)
				{
					this.m_overrides.RemoveAt(i);
					flag = true;
				}
			}
		}
		return flag;
	}

	// Token: 0x04009A17 RID: 39447
	public bool BaseValue;

	// Token: 0x04009A18 RID: 39448
	private List<OverridableBool.OverrideData> m_overrides = new List<OverridableBool.OverrideData>();

	// Token: 0x0200182E RID: 6190
	private class OverrideData
	{
		// Token: 0x04009A19 RID: 39449
		public string key;

		// Token: 0x04009A1A RID: 39450
		public float? duration;
	}
}
