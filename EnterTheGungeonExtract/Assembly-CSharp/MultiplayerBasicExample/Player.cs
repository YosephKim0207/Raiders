using System;
using InControl;
using UnityEngine;

namespace MultiplayerBasicExample
{
	// Token: 0x0200067E RID: 1662
	public class Player : MonoBehaviour
	{
		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060025CA RID: 9674 RVA: 0x000A1D34 File Offset: 0x0009FF34
		// (set) Token: 0x060025CB RID: 9675 RVA: 0x000A1D3C File Offset: 0x0009FF3C
		public InputDevice Device { get; set; }

		// Token: 0x060025CC RID: 9676 RVA: 0x000A1D48 File Offset: 0x0009FF48
		private void Start()
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000A1D58 File Offset: 0x0009FF58
		private void Update()
		{
			if (this.Device == null)
			{
				this.cachedRenderer.material.color = new Color(1f, 1f, 1f, 0.2f);
			}
			else
			{
				this.cachedRenderer.material.color = this.GetColorFromInput();
				base.transform.Rotate(Vector3.down, 500f * Time.deltaTime * this.Device.Direction.X, Space.World);
				base.transform.Rotate(Vector3.right, 500f * Time.deltaTime * this.Device.Direction.Y, Space.World);
			}
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x000A1E10 File Offset: 0x000A0010
		private Color GetColorFromInput()
		{
			if (this.Device.Action1)
			{
				return Color.green;
			}
			if (this.Device.Action2)
			{
				return Color.red;
			}
			if (this.Device.Action3)
			{
				return Color.blue;
			}
			if (this.Device.Action4)
			{
				return Color.yellow;
			}
			return Color.white;
		}

		// Token: 0x040019BB RID: 6587
		private Renderer cachedRenderer;
	}
}
