using System;
using UnityEngine;

// Token: 0x0200175F RID: 5983
public class DFScaleFixer : MonoBehaviour
{
	// Token: 0x06008B40 RID: 35648 RVA: 0x0039FA7C File Offset: 0x0039DC7C
	private void Start()
	{
		this.m_manager = base.GetComponent<dfGUIManager>();
	}

	// Token: 0x06008B41 RID: 35649 RVA: 0x0039FA8C File Offset: 0x0039DC8C
	private void Update()
	{
		this.m_manager.UIScaleLegacyMode = false;
		this.m_manager.UIScale = (float)this.m_manager.RenderCamera.pixelHeight / (float)this.m_manager.FixedHeight;
	}

	// Token: 0x04009210 RID: 37392
	private dfGUIManager m_manager;
}
