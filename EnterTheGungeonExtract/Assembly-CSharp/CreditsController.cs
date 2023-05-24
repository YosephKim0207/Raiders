using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

// Token: 0x02001756 RID: 5974
public class CreditsController : MonoBehaviour
{
	// Token: 0x06008B13 RID: 35603 RVA: 0x0039EEC8 File Offset: 0x0039D0C8
	private void Start()
	{
		GameManager.Instance.ClearActiveGameData(false, false);
		UnityEngine.Object.Destroy(GameManager.Instance.DungeonMusicController);
		base.StartCoroutine(this.ScrollToNextThreshold());
	}

	// Token: 0x06008B14 RID: 35604 RVA: 0x0039EEF4 File Offset: 0x0039D0F4
	private IEnumerator ScrollToNextThreshold()
	{
		float elapsed = 0f;
		float startYScroll = this.creditsPanel.ScrollPosition.y;
		float duration = ((float)this.scrollThresholds[this.m_currentThreshold] - this.creditsPanel.ScrollPosition.y) / this.maxScrollSpeed;
		while (elapsed < duration)
		{
			InputDevice input = InputManager.ActiveDevice;
			if (input.AnyButton.WasPressed || Input.anyKeyDown)
			{
				this.GoToMainMenu();
				yield break;
			}
			this.creditsPanel.ScrollPosition = new Vector2(this.creditsPanel.ScrollPosition.x, BraveMathCollege.SmoothLerp(startYScroll, (float)this.scrollThresholds[this.m_currentThreshold], Mathf.Clamp01(elapsed / duration)));
			this.creditsPanel.ScrollPosition = this.creditsPanel.ScrollPosition.Quantize(3f);
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		base.StartCoroutine(this.WaitForNextThreshold());
		yield break;
	}

	// Token: 0x06008B15 RID: 35605 RVA: 0x0039EF10 File Offset: 0x0039D110
	private IEnumerator WaitForNextThreshold()
	{
		float elapsed = 0f;
		while (elapsed < this.scrollDelays[this.m_currentThreshold])
		{
			InputDevice input = InputManager.ActiveDevice;
			if (input.AnyButton.WasPressed || Input.anyKeyDown)
			{
				this.GoToMainMenu();
				yield break;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		this.m_currentThreshold++;
		if (this.m_currentThreshold < this.scrollThresholds.Count)
		{
			base.StartCoroutine(this.ScrollToNextThreshold());
		}
		else
		{
			this.GoToMainMenu();
		}
		yield break;
	}

	// Token: 0x06008B16 RID: 35606 RVA: 0x0039EF2C File Offset: 0x0039D12C
	private void GoToMainMenu()
	{
		Cursor.visible = true;
		GameManager.Instance.LoadCharacterSelect(false, false);
	}

	// Token: 0x040091E4 RID: 37348
	public dfScrollPanel creditsPanel;

	// Token: 0x040091E5 RID: 37349
	public List<int> scrollThresholds;

	// Token: 0x040091E6 RID: 37350
	public List<float> scrollDelays;

	// Token: 0x040091E7 RID: 37351
	public float maxScrollSpeed = 20f;

	// Token: 0x040091E8 RID: 37352
	private int m_currentThreshold;
}
