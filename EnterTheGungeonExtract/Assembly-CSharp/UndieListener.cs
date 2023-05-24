using System;
using UnityEngine;

// Token: 0x02001814 RID: 6164
public class UndieListener : MonoBehaviour
{
	// Token: 0x0600915F RID: 37215 RVA: 0x003D8084 File Offset: 0x003D6284
	private void OnClick(dfControl control, dfMouseEventArgs mouseEvent)
	{
		dfGUIManager.PopModal();
		Pixelator.Instance.LerpToLetterbox(0.5f, 0f);
		GameManager.Instance.PrimaryPlayer.healthHaver.FullHeal();
		base.transform.parent.gameObject.SetActive(false);
		GameManager.Instance.Unpause();
		GameManager.Instance.PrimaryPlayer.ClearDeadFlags();
	}
}
