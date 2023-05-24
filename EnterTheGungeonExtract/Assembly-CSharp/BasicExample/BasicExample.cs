using System;
using InControl;
using UnityEngine;

namespace BasicExample
{
	// Token: 0x02000677 RID: 1655
	public class BasicExample : MonoBehaviour
	{
		// Token: 0x060025AF RID: 9647 RVA: 0x000A1284 File Offset: 0x0009F484
		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			base.transform.Rotate(Vector3.down, 500f * Time.deltaTime * activeDevice.LeftStickX, Space.World);
			base.transform.Rotate(Vector3.right, 500f * Time.deltaTime * activeDevice.LeftStickY, Space.World);
			Color color = ((!activeDevice.Action1.IsPressed) ? Color.white : Color.red);
			Color color2 = ((!activeDevice.Action2.IsPressed) ? Color.white : Color.green);
			base.GetComponent<Renderer>().material.color = Color.Lerp(color, color2, 0.5f);
		}
	}
}
