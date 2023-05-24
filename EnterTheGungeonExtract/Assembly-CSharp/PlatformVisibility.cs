using System;
using UnityEngine;

// Token: 0x02000471 RID: 1137
[AddComponentMenu("Daikon Forge/Examples/General/Platform-based Visibility")]
public class PlatformVisibility : MonoBehaviour
{
	// Token: 0x06001A31 RID: 6705 RVA: 0x0007A44C File Offset: 0x0007864C
	private void Start()
	{
		dfControl component = base.GetComponent<dfControl>();
		if (component == null)
		{
			return;
		}
		bool flag = this.HideInEditor && Application.isEditor;
		if (flag)
		{
			component.Hide();
		}
	}

	// Token: 0x0400148B RID: 5259
	public bool HideOnWeb;

	// Token: 0x0400148C RID: 5260
	public bool HideOnMobile;

	// Token: 0x0400148D RID: 5261
	public bool HideInEditor;
}
