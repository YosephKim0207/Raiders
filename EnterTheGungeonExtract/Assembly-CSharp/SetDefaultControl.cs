using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000474 RID: 1140
[AddComponentMenu("Daikon Forge/Examples/General/Set Default Control")]
public class SetDefaultControl : MonoBehaviour
{
	// Token: 0x06001A37 RID: 6711 RVA: 0x0007A510 File Offset: 0x00078710
	public void Awake()
	{
		this.thisControl = base.GetComponent<dfControl>();
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x0007A520 File Offset: 0x00078720
	public void OnEnable()
	{
		if (this.defaultControl != null && this.defaultControl.IsVisible)
		{
			this.defaultControl.Focus(true);
		}
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x0007A550 File Offset: 0x00078750
	public IEnumerator OnIsVisibleChanged(dfControl control, bool value)
	{
		if (control == this.thisControl && value && this.defaultControl != null)
		{
			yield return new WaitForEndOfFrame();
			this.defaultControl.Focus(true);
		}
		yield break;
	}

	// Token: 0x04001490 RID: 5264
	public dfControl defaultControl;

	// Token: 0x04001491 RID: 5265
	private dfControl thisControl;
}
