using System;
using UnityEngine;

// Token: 0x02000478 RID: 1144
[AddComponentMenu("Daikon Forge/Examples/General/Window Tilt")]
public class WindowTilt : MonoBehaviour
{
	// Token: 0x06001A49 RID: 6729 RVA: 0x0007A7E4 File Offset: 0x000789E4
	private void Start()
	{
		this.control = base.GetComponent<dfControl>();
		if (this.control == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x0007A80C File Offset: 0x00078A0C
	private void Update()
	{
		Camera camera = this.control.GetCamera();
		Vector3 center = this.control.GetCenter();
		Vector3 vector = camera.WorldToViewportPoint(center);
		this.control.transform.localRotation = Quaternion.Euler(0f, (vector.x * 2f - 1f) * 20f, 0f);
	}

	// Token: 0x0400149C RID: 5276
	private dfControl control;
}
