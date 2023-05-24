using System;
using UnityEngine;

// Token: 0x020017A8 RID: 6056
public class CharacterSelectButton_Listener : MonoBehaviour
{
	// Token: 0x06008DBD RID: 36285 RVA: 0x003BA1B4 File Offset: 0x003B83B4
	private void OnClick(dfControl control, dfMouseEventArgs mouseEvent)
	{
		GameManager.PlayerPrefabForNewGame = this.playerToSelect;
		GameManager.Instance.LoadNextLevel();
	}

	// Token: 0x0400957C RID: 38268
	public GameObject playerToSelect;
}
