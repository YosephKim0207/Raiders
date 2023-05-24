using System;
using InControl;
using UnityEngine;

namespace VirtualDeviceExample
{
	// Token: 0x02000685 RID: 1669
	public class VirtualDeviceExample : MonoBehaviour
	{
		// Token: 0x060025FD RID: 9725 RVA: 0x000A2E14 File Offset: 0x000A1014
		private void OnEnable()
		{
			this.virtualDevice = new VirtualDevice();
			InputManager.OnSetup += delegate
			{
				InputManager.AttachDevice(this.virtualDevice);
			};
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000A2E34 File Offset: 0x000A1034
		private void OnDisable()
		{
			InputManager.DetachDevice(this.virtualDevice);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000A2E44 File Offset: 0x000A1044
		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			this.leftObject.transform.Rotate(Vector3.down, 500f * Time.deltaTime * activeDevice.LeftStickX, Space.World);
			this.leftObject.transform.Rotate(Vector3.right, 500f * Time.deltaTime * activeDevice.LeftStickY, Space.World);
			this.rightObject.transform.Rotate(Vector3.down, 500f * Time.deltaTime * activeDevice.RightStickX, Space.World);
			this.rightObject.transform.Rotate(Vector3.right, 500f * Time.deltaTime * activeDevice.RightStickY, Space.World);
			Color color = Color.white;
			if (activeDevice.Action1.IsPressed)
			{
				color = Color.green;
			}
			if (activeDevice.Action2.IsPressed)
			{
				color = Color.red;
			}
			if (activeDevice.Action3.IsPressed)
			{
				color = Color.blue;
			}
			if (activeDevice.Action4.IsPressed)
			{
				color = Color.yellow;
			}
			this.leftObject.GetComponent<Renderer>().material.color = color;
		}

		// Token: 0x040019D8 RID: 6616
		public GameObject leftObject;

		// Token: 0x040019D9 RID: 6617
		public GameObject rightObject;

		// Token: 0x040019DA RID: 6618
		private VirtualDevice virtualDevice;
	}
}
