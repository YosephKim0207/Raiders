using System;
using System.Collections;
using UnityEngine;

// Token: 0x020017E9 RID: 6121
public class PreOptionsMenuController : MonoBehaviour
{
	// Token: 0x17001591 RID: 5521
	// (get) Token: 0x06009019 RID: 36889 RVA: 0x003CEC88 File Offset: 0x003CCE88
	// (set) Token: 0x0600901A RID: 36890 RVA: 0x003CEC98 File Offset: 0x003CCE98
	public bool IsVisible
	{
		get
		{
			return this.m_panel.IsVisible;
		}
		set
		{
			if (this.m_panel.IsVisible != value)
			{
				if (value)
				{
					this.m_panel.IsVisible = value;
					this.ShwoopOpen();
					this.ShowPreOptionsMenu();
				}
				else
				{
					this.m_timeOpen = 0f;
					this.ShwoopClosed();
					if (dfGUIManager.GetModalControl() == this.m_panel)
					{
						dfGUIManager.PopModal();
					}
					else
					{
						Debug.LogError("failure.");
					}
				}
			}
		}
	}

	// Token: 0x0600901B RID: 36891 RVA: 0x003CED14 File Offset: 0x003CCF14
	public void MakeVisibleWithoutAnim()
	{
		if (!this.m_panel.IsVisible)
		{
			this.m_panel.IsVisible = true;
			if (!this.HeaderLabel.Text.StartsWith("#"))
			{
				this.HeaderLabel.ModifyLocalizedText(this.HeaderLabel.Text.ToUpperInvariant());
			}
			this.m_panel.Opacity = 1f;
			this.m_panel.transform.localScale = Vector3.one;
			this.m_panel.MakePixelPerfect();
			this.ShowPreOptionsMenu();
		}
	}

	// Token: 0x0600901C RID: 36892 RVA: 0x003CEDA8 File Offset: 0x003CCFA8
	private void ShowPreOptionsMenu()
	{
		dfGUIManager.PushModal(this.m_panel);
		this.TabGameplaySelector.Focus(true);
	}

	// Token: 0x0600901D RID: 36893 RVA: 0x003CEDC4 File Offset: 0x003CCFC4
	public void ReturnToPreOptionsMenu()
	{
		this.FullOptionsMenu.IsVisible = false;
		this.IsVisible = true;
		this.TabGameplaySelector.Focus(true);
		AkSoundEngine.PostEvent("Play_UI_menu_back_01", base.gameObject);
		dfGUIManager.PopModalToControl(this.m_panel, false);
	}

	// Token: 0x0600901E RID: 36894 RVA: 0x003CEE04 File Offset: 0x003CD004
	public void ToggleToPanel(dfScrollPanel targetPanel, bool val, bool force = false)
	{
		if (!force && this.m_timeOpen < 0.2f)
		{
			return;
		}
		this.FullOptionsMenu.ToggleToPanel(targetPanel, val);
		this.m_panel.IsVisible = false;
	}

	// Token: 0x0600901F RID: 36895 RVA: 0x003CEE38 File Offset: 0x003CD038
	private void Awake()
	{
		this.m_panel = base.GetComponent<dfPanel>();
		this.TabAudioSelector.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			this.ToggleToPanel(this.FullOptionsMenu.TabAudio, false, false);
		};
		this.TabVideoSelector.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			this.ToggleToPanel(this.FullOptionsMenu.TabVideo, false, false);
		};
		this.TabGameplaySelector.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			this.ToggleToPanel(this.FullOptionsMenu.TabGameplay, false, false);
		};
		this.TabControlsSelector.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			this.ToggleToPanel(this.FullOptionsMenu.TabControls, false, false);
		};
	}

	// Token: 0x06009020 RID: 36896 RVA: 0x003CEEB0 File Offset: 0x003CD0B0
	private void Update()
	{
		if (this.IsVisible)
		{
			this.m_timeOpen += GameManager.INVARIANT_DELTA_TIME;
		}
		else
		{
			this.m_timeOpen = 0f;
		}
	}

	// Token: 0x06009021 RID: 36897 RVA: 0x003CEEE0 File Offset: 0x003CD0E0
	public void ShwoopOpen()
	{
		if (!this.HeaderLabel.Text.StartsWith("#"))
		{
			this.HeaderLabel.ModifyLocalizedText(this.HeaderLabel.Text.ToUpperInvariant());
		}
		base.StartCoroutine(this.HandleShwoop(false));
	}

	// Token: 0x06009022 RID: 36898 RVA: 0x003CEF30 File Offset: 0x003CD130
	private IEnumerator HandleShwoop(bool reverse)
	{
		float timer = 0.1f;
		float elapsed = 0f;
		Vector3 smallScale = new Vector3(0.01f, 0.01f, 1f);
		Vector3 bigScale = Vector3.one;
		PauseMenuController pmc = GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>();
		while (elapsed < timer)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / timer;
			AnimationCurve targetCurve = ((!reverse) ? pmc.ShwoopInCurve : pmc.ShwoopOutCurve);
			this.m_panel.Opacity = Mathf.Lerp(0f, 1f, (!reverse) ? (t * 2f) : (1f - t * 2f));
			this.m_panel.transform.localScale = smallScale + bigScale * Mathf.Clamp01(targetCurve.Evaluate(t));
			yield return null;
		}
		if (!reverse)
		{
			this.m_panel.transform.localScale = Vector3.one;
			this.m_panel.MakePixelPerfect();
		}
		if (reverse)
		{
			this.m_panel.IsVisible = false;
		}
		yield break;
	}

	// Token: 0x06009023 RID: 36899 RVA: 0x003CEF54 File Offset: 0x003CD154
	public void ShwoopClosed()
	{
		base.StartCoroutine(this.HandleShwoop(true));
	}

	// Token: 0x0400983E RID: 38974
	public dfButton TabAudioSelector;

	// Token: 0x0400983F RID: 38975
	public dfButton TabVideoSelector;

	// Token: 0x04009840 RID: 38976
	public dfButton TabGameplaySelector;

	// Token: 0x04009841 RID: 38977
	public dfButton TabControlsSelector;

	// Token: 0x04009842 RID: 38978
	public dfLabel HeaderLabel;

	// Token: 0x04009843 RID: 38979
	public FullOptionsMenuController FullOptionsMenu;

	// Token: 0x04009844 RID: 38980
	private dfPanel m_panel;

	// Token: 0x04009845 RID: 38981
	private float m_timeOpen;
}
