using System;
using UnityEngine;

// Token: 0x020015CE RID: 5582
public class PlatformInterfaceGenericPC : PlatformInterface
{
	// Token: 0x06008033 RID: 32819 RVA: 0x0033CBC8 File Offset: 0x0033ADC8
	protected override void OnStart()
	{
		Debug.Log("Starting Generic PC platform interface.");
	}

	// Token: 0x06008034 RID: 32820 RVA: 0x0033CBD4 File Offset: 0x0033ADD4
	protected override void OnAchievementUnlock(Achievement achievement, int playerIndex)
	{
	}

	// Token: 0x06008035 RID: 32821 RVA: 0x0033CBD8 File Offset: 0x0033ADD8
	protected override void OnLateUpdate()
	{
	}

	// Token: 0x06008036 RID: 32822 RVA: 0x0033CBDC File Offset: 0x0033ADDC
	protected override StringTableManager.GungeonSupportedLanguages OnGetPreferredLanguage()
	{
		return StringTableManager.GungeonSupportedLanguages.ENGLISH;
	}
}
