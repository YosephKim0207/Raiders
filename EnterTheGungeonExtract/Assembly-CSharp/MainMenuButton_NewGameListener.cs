using System;
using UnityEngine;

// Token: 0x020017B8 RID: 6072
public class MainMenuButton_NewGameListener : MonoBehaviour
{
	// Token: 0x06008E1B RID: 36379 RVA: 0x003BC294 File Offset: 0x003BA494
	private void OnClick(dfControl control, dfMouseEventArgs mouseEvent)
	{
		GameManager.Instance.LoadCharacterSelect(false, false);
	}
}
