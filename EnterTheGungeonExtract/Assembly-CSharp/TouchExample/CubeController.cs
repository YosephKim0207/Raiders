using System;
using InControl;
using UnityEngine;

namespace TouchExample
{
	// Token: 0x02000683 RID: 1667
	public class CubeController : MonoBehaviour
	{
		// Token: 0x060025F1 RID: 9713 RVA: 0x000A28FC File Offset: 0x000A0AFC
		private void Start()
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000A290C File Offset: 0x000A0B0C
		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			if (activeDevice != InputDevice.Null && activeDevice != TouchManager.Device)
			{
				TouchManager.ControlsEnabled = false;
			}
			this.cachedRenderer.material.color = this.GetColorFromActionButtons(activeDevice);
			base.transform.Rotate(Vector3.down, 500f * Time.deltaTime * activeDevice.Direction.X, Space.World);
			base.transform.Rotate(Vector3.right, 500f * Time.deltaTime * activeDevice.Direction.Y, Space.World);
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000A29A4 File Offset: 0x000A0BA4
		private Color GetColorFromActionButtons(InputDevice inputDevice)
		{
			if (inputDevice.Action1)
			{
				return Color.green;
			}
			if (inputDevice.Action2)
			{
				return Color.red;
			}
			if (inputDevice.Action3)
			{
				return Color.blue;
			}
			if (inputDevice.Action4)
			{
				return Color.yellow;
			}
			return Color.white;
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x000A2A10 File Offset: 0x000A0C10
		private void OnGUI()
		{
			float num = 10f;
			int touchCount = TouchManager.TouchCount;
			for (int i = 0; i < touchCount; i++)
			{
				InControl.Touch touch = TouchManager.GetTouch(i);
				GUI.Label(new Rect(10f, num, 500f, num + 15f), string.Concat(new object[]
				{
					string.Empty,
					i,
					": fingerId = ",
					touch.fingerId,
					", phase = ",
					touch.phase.ToString(),
					", position = ",
					touch.position
				}));
				num += 20f;
			}
		}

		// Token: 0x040019D1 RID: 6609
		private Renderer cachedRenderer;
	}
}
