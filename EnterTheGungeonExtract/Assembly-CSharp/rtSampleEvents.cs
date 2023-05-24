using System;
using UnityEngine;

// Token: 0x0200045D RID: 1117
[AddComponentMenu("Daikon Forge/Examples/Rich Text/Rich Text Events")]
public class rtSampleEvents : MonoBehaviour
{
	// Token: 0x060019DC RID: 6620 RVA: 0x00078AD4 File Offset: 0x00076CD4
	public void OnLinkClicked(dfRichTextLabel sender, dfMarkupTagAnchor tag)
	{
		string href = tag.HRef;
		if (href.ToLowerInvariant().StartsWith("http:"))
		{
			Application.OpenURL(href);
		}
	}
}
