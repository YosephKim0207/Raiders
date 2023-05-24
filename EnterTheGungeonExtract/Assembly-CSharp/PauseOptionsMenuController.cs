using System;
using UnityEngine;

// Token: 0x020017E8 RID: 6120
public class PauseOptionsMenuController : MonoBehaviour
{
	// Token: 0x17001590 RID: 5520
	// (get) Token: 0x0600900E RID: 36878 RVA: 0x003CE9B4 File Offset: 0x003CCBB4
	// (set) Token: 0x0600900F RID: 36879 RVA: 0x003CE9C4 File Offset: 0x003CCBC4
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
				this.InitializeFromOptions();
				this.m_panel.IsVisible = value;
				if (value)
				{
					dfGUIManager.PushModal(this.m_panel);
				}
				else if (dfGUIManager.GetModalControl() == this.m_panel)
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

	// Token: 0x06009010 RID: 36880 RVA: 0x003CEA34 File Offset: 0x003CCC34
	public void InitializeFromOptions()
	{
		Debug.Log("initializing...");
		this.MusicVolumeSlider.Value = GameManager.Options.MusicVolume;
		this.SoundVolumeSlider.Value = GameManager.Options.SoundVolume;
		if (this.UIVolumeSlider != null)
		{
			this.UIVolumeSlider.Value = GameManager.Options.UIVolume;
		}
		GameOptions.AudioHardwareMode audioHardware = GameManager.Options.AudioHardware;
		if (audioHardware != GameOptions.AudioHardwareMode.HEADPHONES)
		{
			if (audioHardware == GameOptions.AudioHardwareMode.SPEAKERS)
			{
				this.SpeakersButton.ForceState(dfButton.ButtonState.Pressed);
			}
		}
		else
		{
			this.HeadphonesButton.ForceState(dfButton.ButtonState.Pressed);
		}
	}

	// Token: 0x06009011 RID: 36881 RVA: 0x003CEADC File Offset: 0x003CCCDC
	private void Start()
	{
		this.m_panel = base.GetComponent<dfPanel>();
		this.InitializeFromOptions();
		this.MusicVolumeSlider.ValueChanged += delegate(dfControl control, float value)
		{
			GameManager.Options.MusicVolume = value;
		};
		this.SoundVolumeSlider.ValueChanged += delegate(dfControl control, float value)
		{
			GameManager.Options.SoundVolume = value;
		};
		if (this.UIVolumeSlider != null)
		{
			this.UIVolumeSlider.ValueChanged += delegate(dfControl control, float value)
			{
				GameManager.Options.UIVolume = value;
			};
		}
		this.HeadphonesButton.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			AkSoundEngine.PostEvent("Play_UI_menu_confirm_01", base.gameObject);
			this.HeadphonesButton.ForceState(dfButton.ButtonState.Pressed);
			this.SpeakersButton.ForceState(dfButton.ButtonState.Default);
			GameManager.Options.AudioHardware = GameOptions.AudioHardwareMode.HEADPHONES;
		};
		this.SpeakersButton.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			AkSoundEngine.PostEvent("Play_UI_menu_confirm_01", base.gameObject);
			this.HeadphonesButton.ForceState(dfButton.ButtonState.Default);
			this.SpeakersButton.ForceState(dfButton.ButtonState.Pressed);
			GameManager.Options.AudioHardware = GameOptions.AudioHardwareMode.SPEAKERS;
		};
		this.AcceptButton.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			this.IsVisible = false;
			GameUIRoot.Instance.ShowPauseMenu();
		};
	}

	// Token: 0x04009834 RID: 38964
	public dfProgressBar MusicVolumeSlider;

	// Token: 0x04009835 RID: 38965
	public dfProgressBar SoundVolumeSlider;

	// Token: 0x04009836 RID: 38966
	public dfProgressBar UIVolumeSlider;

	// Token: 0x04009837 RID: 38967
	public dfButton HeadphonesButton;

	// Token: 0x04009838 RID: 38968
	public dfButton SpeakersButton;

	// Token: 0x04009839 RID: 38969
	public dfButton AcceptButton;

	// Token: 0x0400983A RID: 38970
	protected dfPanel m_panel;
}
