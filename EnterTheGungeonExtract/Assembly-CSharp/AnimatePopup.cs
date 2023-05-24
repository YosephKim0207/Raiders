using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200045E RID: 1118
[AddComponentMenu("Daikon Forge/Examples/General/Animate Popup")]
public class AnimatePopup : MonoBehaviour
{
	// Token: 0x060019DE RID: 6622 RVA: 0x00078B0C File Offset: 0x00076D0C
	private void OnDropdownOpen(dfDropdown dropdown, dfListbox popup)
	{
		if (this.target != null)
		{
			base.StopCoroutine("animateOpen");
			base.StopCoroutine("animateClose");
			UnityEngine.Object.Destroy(this.target.gameObject);
		}
		this.target = popup;
		base.StartCoroutine(this.animateOpen(popup));
	}

	// Token: 0x060019DF RID: 6623 RVA: 0x00078B68 File Offset: 0x00076D68
	private void OnDropdownClose(dfDropdown dropdown, dfListbox popup)
	{
		base.StartCoroutine(this.animateClose(popup));
	}

	// Token: 0x060019E0 RID: 6624 RVA: 0x00078B78 File Offset: 0x00076D78
	private IEnumerator animateOpen(dfListbox popup)
	{
		float runningTime = 0f;
		float startAlpha = 0f;
		float endAlpha = 1f;
		float startHeight = 20f;
		float endHeight = popup.Height;
		while (this.target == popup && runningTime < 0.15f)
		{
			runningTime = Mathf.Min(runningTime + BraveTime.DeltaTime, 0.15f);
			popup.Opacity = Mathf.Lerp(startAlpha, endAlpha, runningTime / 0.15f);
			float height = Mathf.Lerp(startHeight, endHeight, runningTime / 0.15f);
			popup.Height = height;
			yield return null;
		}
		popup.Opacity = 1f;
		popup.Height = endHeight;
		yield return null;
		popup.Invalidate();
		yield break;
	}

	// Token: 0x060019E1 RID: 6625 RVA: 0x00078B9C File Offset: 0x00076D9C
	private IEnumerator animateClose(dfListbox popup)
	{
		float runningTime = 0f;
		float startAlpha = 1f;
		float endAlpha = 0f;
		float startHeight = popup.Height;
		float endHeight = 20f;
		while (this.target == popup && runningTime < 0.15f)
		{
			runningTime = Mathf.Min(runningTime + BraveTime.DeltaTime, 0.15f);
			popup.Opacity = Mathf.Lerp(startAlpha, endAlpha, runningTime / 0.15f);
			float height = Mathf.Lerp(startHeight, endHeight, runningTime / 0.15f);
			popup.Height = height;
			yield return null;
		}
		this.target = null;
		UnityEngine.Object.Destroy(popup.gameObject);
		yield break;
	}

	// Token: 0x04001435 RID: 5173
	private const float ANIMATION_LENGTH = 0.15f;

	// Token: 0x04001436 RID: 5174
	private dfListbox target;
}
