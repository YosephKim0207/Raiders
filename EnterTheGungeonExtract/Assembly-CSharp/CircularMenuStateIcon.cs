using System;
using UnityEngine;

// Token: 0x02000457 RID: 1111
public class CircularMenuStateIcon : MonoBehaviour
{
	// Token: 0x060019AC RID: 6572 RVA: 0x00077E28 File Offset: 0x00076028
	public void OnEnable()
	{
		if (this.sprite == null)
		{
			this.sprite = base.GetComponent<dfSprite>();
		}
		if (this.menu == null)
		{
			this.menu = base.GetComponent<dfRadialMenu>();
		}
		this.sprite.SpriteName = ((!this.menu.IsOpen) ? this.OffSprite : this.OnSprite);
		this.menu.MenuOpened += this.OnMenuOpened;
		this.menu.MenuClosed += this.OnMenuClosed;
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x00077ECC File Offset: 0x000760CC
	public void OnMenuOpened(dfRadialMenu menu)
	{
		this.sprite.SpriteName = this.OnSprite;
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x00077EE0 File Offset: 0x000760E0
	public void OnMenuClosed(dfRadialMenu menu)
	{
		this.sprite.SpriteName = this.OffSprite;
	}

	// Token: 0x0400140A RID: 5130
	public string OffSprite;

	// Token: 0x0400140B RID: 5131
	public string OnSprite;

	// Token: 0x0400140C RID: 5132
	public dfSprite sprite;

	// Token: 0x0400140D RID: 5133
	public dfRadialMenu menu;
}
