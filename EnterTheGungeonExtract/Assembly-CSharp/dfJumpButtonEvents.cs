using System;
using UnityEngine;

// Token: 0x02000484 RID: 1156
public class dfJumpButtonEvents : MonoBehaviour
{
	// Token: 0x06001A92 RID: 6802 RVA: 0x0007C2BC File Offset: 0x0007A4BC
	public void OnMouseDown(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.isMouseDown = true;
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x0007C2C8 File Offset: 0x0007A4C8
	public void OnMouseUp(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.isMouseDown = false;
	}

	// Token: 0x040014E6 RID: 5350
	public bool isMouseDown;
}
