using System;
using UnityEngine;

// Token: 0x0200046D RID: 1133
[AddComponentMenu("Daikon Forge/Examples/3D/Replace GUI Camera")]
public class dfReplaceGUICamera : MonoBehaviour
{
	// Token: 0x06001A27 RID: 6695 RVA: 0x0007A184 File Offset: 0x00078384
	public void OnEnable()
	{
		if (this.mainCamera == null)
		{
			this.mainCamera = Camera.main;
		}
		dfGUIManager component = base.GetComponent<dfGUIManager>();
		if (component == null)
		{
			Debug.LogError("This script should be attached to a dfGUIManager instance", this);
			base.enabled = false;
			return;
		}
		this.mainCamera.cullingMask |= 1 << base.gameObject.layer;
		component.OverrideCamera = true;
		component.RenderCamera = this.mainCamera;
	}

	// Token: 0x04001483 RID: 5251
	public Camera mainCamera;
}
