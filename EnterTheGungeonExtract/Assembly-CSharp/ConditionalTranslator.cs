using System;
using UnityEngine;

// Token: 0x02001753 RID: 5971
public class ConditionalTranslator : MonoBehaviour
{
	// Token: 0x06008AFF RID: 35583 RVA: 0x0039E408 File Offset: 0x0039C608
	private void Start()
	{
		this.m_control = base.GetComponent<dfControl>();
		if (this.m_control)
		{
			this.m_control.IsEnabledChanged += this.HandleTranslation;
			this.m_control.IsVisibleChanged += this.HandleTranslation;
		}
	}

	// Token: 0x06008B00 RID: 35584 RVA: 0x0039E460 File Offset: 0x0039C660
	private void SetText(string targetText)
	{
		if (this.m_control is dfLabel)
		{
			(this.m_control as dfLabel).Text = targetText;
		}
	}

	// Token: 0x06008B01 RID: 35585 RVA: 0x0039E484 File Offset: 0x0039C684
	private void HandleTranslation(dfControl control, bool value)
	{
		if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			this.m_control.IsLocalized = false;
			this.SetText(this.EnglishText);
		}
		else if (this.useUiTable)
		{
			this.m_control.IsLocalized = true;
			this.SetText(this.NonEnglishItemsKey);
		}
		else
		{
			this.m_control.IsLocalized = false;
			this.SetText(StringTableManager.GetItemsString(this.NonEnglishItemsKey, -1));
		}
	}

	// Token: 0x040091CC RID: 37324
	public string EnglishText;

	// Token: 0x040091CD RID: 37325
	public string NonEnglishItemsKey;

	// Token: 0x040091CE RID: 37326
	public bool useUiTable;

	// Token: 0x040091CF RID: 37327
	private dfControl m_control;
}
