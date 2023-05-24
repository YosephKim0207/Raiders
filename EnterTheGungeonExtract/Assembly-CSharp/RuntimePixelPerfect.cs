using System;
using UnityEngine;

// Token: 0x02000473 RID: 1139
[AddComponentMenu("Daikon Forge/Examples/General/Pixel-Perfect Platform Settings")]
public class RuntimePixelPerfect : MonoBehaviour
{
	// Token: 0x06001A35 RID: 6709 RVA: 0x0007A4B4 File Offset: 0x000786B4
	private void Awake()
	{
		dfGUIManager component = base.GetComponent<dfGUIManager>();
		if (component == null)
		{
			throw new MissingComponentException("dfGUIManager instance not found");
		}
		if (Application.isEditor)
		{
			component.PixelPerfectMode = this.PixelPerfectInEditor;
		}
		else
		{
			component.PixelPerfectMode = this.PixelPerfectAtRuntime;
		}
	}

	// Token: 0x0400148E RID: 5262
	public bool PixelPerfectInEditor;

	// Token: 0x0400148F RID: 5263
	public bool PixelPerfectAtRuntime = true;
}
