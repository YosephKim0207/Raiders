using System;
using UnityEngine;

// Token: 0x0200046E RID: 1134
[AddComponentMenu("Daikon Forge/Examples/General/Window Tilt")]
public class dfWindowTilt : MonoBehaviour
{
	// Token: 0x06001A29 RID: 6697 RVA: 0x0007A210 File Offset: 0x00078410
	private void Start()
	{
		this.control = base.GetComponent<dfControl>();
		if (this.control == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x0007A238 File Offset: 0x00078438
	private void Update()
	{
		Camera camera = this.control.GetCamera();
		Vector3 center = this.control.GetCenter();
		Vector3 vector = camera.WorldToViewportPoint(center);
		this.control.transform.localRotation = Quaternion.Euler(0f, (vector.x * 2f - 1f) * 20f, 0f);
	}

	// Token: 0x04001484 RID: 5252
	private dfControl control;
}
