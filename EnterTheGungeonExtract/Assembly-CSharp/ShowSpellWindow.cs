using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000432 RID: 1074
[AddComponentMenu("Daikon Forge/Examples/Actionbar/Show Spell Window")]
public class ShowSpellWindow : MonoBehaviour
{
	// Token: 0x0600189C RID: 6300 RVA: 0x000743C0 File Offset: 0x000725C0
	private void OnEnable()
	{
		dfControl component = GameObject.Find("Spell Window").GetComponent<dfControl>();
		component.IsVisible = false;
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x000743E4 File Offset: 0x000725E4
	private void OnClick()
	{
		if (this.busy)
		{
			return;
		}
		base.StopAllCoroutines();
		dfControl component = GameObject.Find("Spell Window").GetComponent<dfControl>();
		if (!this.isVisible)
		{
			base.StartCoroutine(this.showWindow(component));
		}
		else
		{
			base.StartCoroutine(this.hideWindow(component));
		}
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x00074440 File Offset: 0x00072640
	private IEnumerator hideWindow(dfControl window)
	{
		this.busy = true;
		this.isVisible = false;
		window.IsVisible = true;
		window.GetManager().BringToFront(window);
		dfAnimatedFloat opacity = new dfAnimatedFloat(1f, 0f, 0.33f);
		while (opacity > 0.05f)
		{
			window.Opacity = opacity;
			yield return null;
		}
		window.Opacity = 0f;
		this.busy = false;
		yield break;
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x00074464 File Offset: 0x00072664
	private IEnumerator showWindow(dfControl window)
	{
		this.isVisible = true;
		this.busy = true;
		window.IsVisible = true;
		window.GetManager().BringToFront(window);
		dfAnimatedFloat opacity = new dfAnimatedFloat(0f, 1f, 0.33f);
		while (opacity < 0.95f)
		{
			window.Opacity = opacity;
			yield return null;
		}
		window.Opacity = 1f;
		this.busy = false;
		this.isVisible = true;
		yield break;
	}

	// Token: 0x04001381 RID: 4993
	private bool busy;

	// Token: 0x04001382 RID: 4994
	private bool isVisible;
}
