using System;
using UnityEngine;

// Token: 0x020017F0 RID: 6128
public class SpeedrunTimer : MonoBehaviour
{
	// Token: 0x06009040 RID: 36928 RVA: 0x003CF544 File Offset: 0x003CD744
	private void Start()
	{
		this.m_label = base.GetComponent<dfLabel>();
		this.m_lastPlayedSeconds = 0;
		if (this.tk2dTarget)
		{
			this.tk2dRenderer = this.tk2dTarget.GetComponent<Renderer>();
		}
	}

	// Token: 0x06009041 RID: 36929 RVA: 0x003CF57C File Offset: 0x003CD77C
	private void Update()
	{
		if (this.tk2dTarget)
		{
			if (!this.tk2dRenderer.enabled && GameManager.Options.SpeedrunMode)
			{
				this.m_label.Parent.IsVisible = true;
				this.m_label.IsVisible = false;
				this.tk2dRenderer.enabled = true;
			}
			if (this.tk2dRenderer.enabled && !GameManager.Options.SpeedrunMode)
			{
				this.m_label.Parent.IsVisible = false;
				this.m_label.IsVisible = false;
				this.tk2dRenderer.enabled = false;
			}
			if (!GameManager.HasInstance || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
			{
				this.m_label.Parent.IsVisible = false;
				this.m_label.IsVisible = false;
				this.tk2dRenderer.enabled = false;
			}
			if (!this.tk2dRenderer.enabled)
			{
				return;
			}
		}
		else
		{
			if (!this.m_label.Parent.IsVisible && GameManager.Options.SpeedrunMode)
			{
				this.m_label.Parent.IsVisible = true;
			}
			if (this.m_label.Parent.IsVisible && !GameManager.Options.SpeedrunMode)
			{
				this.m_label.Parent.IsVisible = false;
			}
			if (!this.m_label.IsVisible)
			{
				return;
			}
		}
		this.m_label.Parent.Parent.RelativePosition = this.m_label.Parent.Parent.RelativePosition.WithY(GameUIRoot.Instance.p_playerCoinLabel.Parent.Parent.RelativePosition.y + GameUIRoot.Instance.p_playerCoinLabel.Parent.Parent.Height + 3f);
		float sessionStatValue = GameStatsManager.Instance.GetSessionStatValue(TrackedStats.TIME_PLAYED);
		int num = Mathf.FloorToInt(sessionStatValue);
		if (this.tk2dTarget)
		{
			int num2 = num / 3600;
			int num3 = num / 60 % 60;
			int num4 = num % 60;
			int num5 = Mathf.FloorToInt(1000f * (sessionStatValue % 1f));
			int num6 = 48;
			this.m_formattedTimeSpan[0] = (char)(num6 + num2 % 10);
			this.m_formattedTimeSpan[1] = ':';
			this.m_formattedTimeSpan[2] = (char)(num6 + num3 / 10 % 10);
			this.m_formattedTimeSpan[3] = (char)(num6 + num3 % 10);
			this.m_formattedTimeSpan[4] = ':';
			this.m_formattedTimeSpan[5] = (char)(num6 + num4 / 10 % 10);
			this.m_formattedTimeSpan[6] = (char)(num6 + num4 % 10);
			this.m_formattedTimeSpan[7] = '.';
			this.m_formattedTimeSpan[8] = (char)(num6 + num5 / 100 % 10);
			this.m_formattedTimeSpan[9] = (char)(num6 + num5 / 10 % 10);
			this.m_formattedTimeSpan[10] = (char)(num6 + num5 % 10);
			this.tk2dTarget.text = new string(this.m_formattedTimeSpan);
			float num7 = this.m_label.PixelsToUnits();
			this.tk2dTarget.scale = new Vector3(num7, num7, num7) * 16f * 3f;
		}
		else if (!GameManager.Options.DisplaySpeedrunCentiseconds)
		{
			if (num != this.m_lastPlayedSeconds || num <= 0)
			{
				this.m_lastPlayedSeconds = num;
				TimeSpan timeSpan = new TimeSpan(0, 0, 0, num);
				string text = string.Format("{0:0}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
				this.m_label.Text = text;
			}
		}
		else
		{
			int num8 = Mathf.FloorToInt(1000f * (GameStatsManager.Instance.GetSessionStatValue(TrackedStats.TIME_PLAYED) % 1f));
			TimeSpan timeSpan2 = new TimeSpan(0, 0, 0, num, num8);
			string text2 = string.Format("{0:0}:{1:00}:{2:00}.{3:00}", new object[]
			{
				timeSpan2.Hours,
				timeSpan2.Minutes,
				timeSpan2.Seconds,
				timeSpan2.Milliseconds / 10
			});
			this.m_label.Text = text2;
		}
	}

	// Token: 0x0400985C RID: 39004
	public tk2dTextMesh tk2dTarget;

	// Token: 0x0400985D RID: 39005
	private Renderer tk2dRenderer;

	// Token: 0x0400985E RID: 39006
	private dfLabel m_label;

	// Token: 0x0400985F RID: 39007
	private int m_lastPlayedSeconds;

	// Token: 0x04009860 RID: 39008
	private char[] m_formattedTimeSpan = new char[11];
}
