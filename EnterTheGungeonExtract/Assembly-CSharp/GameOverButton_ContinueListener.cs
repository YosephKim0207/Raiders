using System;
using UnityEngine;

// Token: 0x020017B7 RID: 6071
public class GameOverButton_ContinueListener : MonoBehaviour
{
	// Token: 0x06008E19 RID: 36377 RVA: 0x003BC274 File Offset: 0x003BA474
	private void OnClick(dfControl control, dfMouseEventArgs mouseEvent)
	{
		UnityEngine.Object.Destroy(GameManager.Instance);
		GameManager.Instance.LoadMainMenu();
	}
}
