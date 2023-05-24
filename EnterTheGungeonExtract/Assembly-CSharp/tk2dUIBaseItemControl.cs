using System;
using UnityEngine;

// Token: 0x02000C03 RID: 3075
[AddComponentMenu("2D Toolkit/UI/tk2dUIBaseItemControl")]
public abstract class tk2dUIBaseItemControl : MonoBehaviour
{
	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x06004162 RID: 16738 RVA: 0x00152A5C File Offset: 0x00150C5C
	// (set) Token: 0x06004163 RID: 16739 RVA: 0x00152A7C File Offset: 0x00150C7C
	public GameObject SendMessageTarget
	{
		get
		{
			if (this.uiItem != null)
			{
				return this.uiItem.sendMessageTarget;
			}
			return null;
		}
		set
		{
			if (this.uiItem != null)
			{
				this.uiItem.sendMessageTarget = value;
			}
		}
	}

	// Token: 0x06004164 RID: 16740 RVA: 0x00152A9C File Offset: 0x00150C9C
	public static void ChangeGameObjectActiveState(GameObject go, bool isActive)
	{
		go.SetActive(isActive);
	}

	// Token: 0x06004165 RID: 16741 RVA: 0x00152AA8 File Offset: 0x00150CA8
	public static void ChangeGameObjectActiveStateWithNullCheck(GameObject go, bool isActive)
	{
		if (go != null)
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveState(go, isActive);
		}
	}

	// Token: 0x06004166 RID: 16742 RVA: 0x00152AC0 File Offset: 0x00150CC0
	protected void DoSendMessage(string methodName, object parameter)
	{
		if (this.SendMessageTarget != null && methodName.Length > 0)
		{
			this.SendMessageTarget.SendMessage(methodName, parameter, SendMessageOptions.RequireReceiver);
		}
	}

	// Token: 0x0400341A RID: 13338
	public tk2dUIItem uiItem;
}
