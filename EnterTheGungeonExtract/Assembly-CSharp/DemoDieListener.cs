using System;
using UnityEngine;

// Token: 0x0200175B RID: 5979
public class DemoDieListener : MonoBehaviour
{
	// Token: 0x06008B32 RID: 35634 RVA: 0x0039F55C File Offset: 0x0039D75C
	private void OnClick(dfControl control, dfMouseEventArgs mouseEvent)
	{
		dfGUIManager.PopModal();
		GameManager.Instance.PrimaryPlayer.healthHaver.Die(Vector2.zero);
		base.transform.parent.gameObject.SetActive(false);
	}
}
