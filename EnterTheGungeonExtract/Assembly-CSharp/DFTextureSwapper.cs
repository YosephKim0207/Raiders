using System;
using UnityEngine;

// Token: 0x02001760 RID: 5984
public class DFTextureSwapper : MonoBehaviour
{
	// Token: 0x06008B43 RID: 35651 RVA: 0x0039FACC File Offset: 0x0039DCCC
	private void Start()
	{
		dfControl component = base.GetComponent<dfControl>();
		if (component)
		{
			component.IsVisibleChanged += this.HandleVisibilityChanged;
			if (component.IsVisible)
			{
				this.HandleVisibilityChanged(component, true);
			}
		}
	}

	// Token: 0x06008B44 RID: 35652 RVA: 0x0039FB10 File Offset: 0x0039DD10
	private void HandleVisibilityChanged(dfControl control, bool value)
	{
		if (control is dfSlicedSprite)
		{
			dfSlicedSprite dfSlicedSprite = control as dfSlicedSprite;
			dfSlicedSprite.SpriteName = ((GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH) ? this.OtherSpriteName : this.EnglishSpriteName);
		}
		else if (control is dfSprite)
		{
			dfSprite dfSprite = control as dfSprite;
			dfSprite.SpriteName = ((GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH) ? this.OtherSpriteName : this.EnglishSpriteName);
		}
	}

	// Token: 0x04009211 RID: 37393
	public string EnglishSpriteName;

	// Token: 0x04009212 RID: 37394
	public string OtherSpriteName;
}
