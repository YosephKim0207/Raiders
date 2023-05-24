using System;
using InControl;
using UnityEngine;

namespace InterfaceMovement
{
	// Token: 0x0200067C RID: 1660
	public class ButtonManager : MonoBehaviour
	{
		// Token: 0x060025C4 RID: 9668 RVA: 0x000A1C00 File Offset: 0x0009FE00
		private void Awake()
		{
			this.filteredDirection = new TwoAxisInputControl();
			this.filteredDirection.StateThreshold = 0.5f;
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000A1C20 File Offset: 0x0009FE20
		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			this.filteredDirection.Filter(activeDevice.Direction, Time.deltaTime);
			if (this.filteredDirection.Left.WasRepeated)
			{
				Debug.Log("!!!");
			}
			if (this.filteredDirection.Up.WasPressed)
			{
				this.MoveFocusTo(this.focusedButton.up);
			}
			if (this.filteredDirection.Down.WasPressed)
			{
				this.MoveFocusTo(this.focusedButton.down);
			}
			if (this.filteredDirection.Left.WasPressed)
			{
				this.MoveFocusTo(this.focusedButton.left);
			}
			if (this.filteredDirection.Right.WasPressed)
			{
				this.MoveFocusTo(this.focusedButton.right);
			}
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000A1D00 File Offset: 0x0009FF00
		private void MoveFocusTo(Button newFocusedButton)
		{
			if (newFocusedButton != null)
			{
				this.focusedButton = newFocusedButton;
			}
		}

		// Token: 0x040019B8 RID: 6584
		public Button focusedButton;

		// Token: 0x040019B9 RID: 6585
		private TwoAxisInputControl filteredDirection;
	}
}
