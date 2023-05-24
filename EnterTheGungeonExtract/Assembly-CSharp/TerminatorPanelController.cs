using System;
using System.Collections;
using UnityEngine;

// Token: 0x020017F7 RID: 6135
public class TerminatorPanelController : MonoBehaviour
{
	// Token: 0x06009084 RID: 36996 RVA: 0x003D1E34 File Offset: 0x003D0034
	private void Awake()
	{
		this.m_panel = base.GetComponent<dfPanel>();
		this.m_panel.Opacity = 0f;
	}

	// Token: 0x06009085 RID: 36997 RVA: 0x003D1E54 File Offset: 0x003D0054
	public void Trigger()
	{
		this.IsActive = true;
		base.StartCoroutine(this.HandleTrigger());
	}

	// Token: 0x06009086 RID: 36998 RVA: 0x003D1E6C File Offset: 0x003D006C
	private int GetNumberOfDigitsNotContainingOne(int digits)
	{
		int num = 0;
		for (int i = 0; i < digits; i++)
		{
			int num2 = UnityEngine.Random.Range(2, 10);
			num += num2 * (int)Mathf.Pow(10f, (float)i);
		}
		return num;
	}

	// Token: 0x06009087 RID: 36999 RVA: 0x003D1EAC File Offset: 0x003D00AC
	private string GenerateLeftString(string lsb, string lsfb)
	{
		string text = string.Format(lsfb, this.GetNumberOfDigitsNotContainingOne(6), this.GetNumberOfDigitsNotContainingOne(3), this.GetNumberOfDigitsNotContainingOne(2));
		string text2 = string.Format(lsfb, this.GetNumberOfDigitsNotContainingOne(6), this.GetNumberOfDigitsNotContainingOne(3), this.GetNumberOfDigitsNotContainingOne(2));
		string text3 = string.Format(lsfb, this.GetNumberOfDigitsNotContainingOne(6), this.GetNumberOfDigitsNotContainingOne(3), this.GetNumberOfDigitsNotContainingOne(2));
		string text4 = string.Format(lsfb, this.GetNumberOfDigitsNotContainingOne(6), this.GetNumberOfDigitsNotContainingOne(3), this.GetNumberOfDigitsNotContainingOne(2));
		string text5 = string.Format(lsfb, this.GetNumberOfDigitsNotContainingOne(6), this.GetNumberOfDigitsNotContainingOne(3), this.GetNumberOfDigitsNotContainingOne(2));
		string text6 = string.Format(lsfb, this.GetNumberOfDigitsNotContainingOne(6), this.GetNumberOfDigitsNotContainingOne(3), this.GetNumberOfDigitsNotContainingOne(2));
		string text7 = string.Format(lsfb, this.GetNumberOfDigitsNotContainingOne(6), this.GetNumberOfDigitsNotContainingOne(3), this.GetNumberOfDigitsNotContainingOne(2));
		return string.Format(lsb, new object[] { text, text2, text3, text4, text5, text6, text7 });
	}

	// Token: 0x06009088 RID: 37000 RVA: 0x003D2018 File Offset: 0x003D0218
	private IEnumerator HandleTrigger()
	{
		this.m_panel.Opacity = 1f;
		float elapsed = 0f;
		float duration = 5f;
		string rightStringBase = "SCAN MODE {0}\nSIZE ASSESSMENT\n\nASSESSMENT COMPLETE\nFIT PROBABILITY 0.99\n\nRESET TO ACQUISITION\nMODE SPEECH LEVEL 76\n\nPRIORITY OVERRIDE\nDEFENSE SYSTEMS SET\nACTIVE STATUS\nLEVEL {1} MAX";
		string leftStringBase = "ANALYSIS:\n******************\n{0}\n{1}\n{2}\n{3}\n\n{4}\n{5}\n{6}";
		string leftStringFragBase = "{0} {1} {2}";
		string rightString = string.Format(rightStringBase, 43894, UnityEngine.Random.Range(1000000, 9999999));
		string leftString = this.GenerateLeftString(leftStringBase, leftStringFragBase);
		int frameCount = 0;
		while (elapsed < duration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			frameCount++;
			this.leftLabel.Height = (float)(84 + 42 * Mathf.Min(8, Mathf.FloorToInt(elapsed / duration * 10f)));
			if (frameCount % 5 == 0)
			{
				rightString = string.Format(rightStringBase, 43894, UnityEngine.Random.Range(1000000, 9999999));
				leftString = this.GenerateLeftString(leftStringBase, leftStringFragBase);
				this.rightLabel.Text = rightString;
				this.leftLabel.Text = leftString;
			}
			yield return null;
		}
		yield return new WaitForSeconds(this.HangTime);
		base.gameObject.SetActive(false);
		this.IsActive = false;
		yield break;
	}

	// Token: 0x04009885 RID: 39045
	public dfLabel leftLabel;

	// Token: 0x04009886 RID: 39046
	public dfLabel rightLabel;

	// Token: 0x04009887 RID: 39047
	public dfLabel bottomLabel;

	// Token: 0x04009888 RID: 39048
	[NonSerialized]
	public bool IsActive;

	// Token: 0x04009889 RID: 39049
	public float HangTime = 3f;

	// Token: 0x0400988A RID: 39050
	private dfPanel m_panel;
}
