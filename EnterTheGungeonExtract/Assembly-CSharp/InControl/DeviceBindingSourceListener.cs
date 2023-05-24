using System;

namespace InControl
{
	// Token: 0x0200068F RID: 1679
	public class DeviceBindingSourceListener : BindingSourceListener
	{
		// Token: 0x06002639 RID: 9785 RVA: 0x000A38D0 File Offset: 0x000A1AD0
		public void Reset()
		{
			this.detectFound = InputControlType.None;
			this.detectPhase = 0;
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x000A38E0 File Offset: 0x000A1AE0
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeControllers || device.IsUnknown)
			{
				return null;
			}
			if (this.detectFound != InputControlType.None && !this.IsPressed(this.detectFound, device) && this.detectPhase == 2)
			{
				DeviceBindingSource deviceBindingSource = new DeviceBindingSource(this.detectFound);
				this.Reset();
				return deviceBindingSource;
			}
			InputControlType inputControlType = this.ListenForControl(listenOptions, device);
			if (inputControlType != InputControlType.None)
			{
				if (this.detectPhase == 1)
				{
					this.detectFound = inputControlType;
					this.detectPhase = 2;
				}
			}
			else if (this.detectPhase == 0)
			{
				this.detectPhase = 1;
			}
			return null;
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x000A3984 File Offset: 0x000A1B84
		private bool IsPressed(InputControl control)
		{
			return Utility.AbsoluteIsOverThreshold(control.Value, 0.5f);
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x000A3998 File Offset: 0x000A1B98
		private bool IsPressed(InputControlType control, InputDevice device)
		{
			return this.IsPressed(device.GetControl(control));
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000A39A8 File Offset: 0x000A1BA8
		private InputControlType ListenForControl(BindingListenOptions listenOptions, InputDevice device)
		{
			if (device.IsKnown)
			{
				int count = device.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = device.Controls[i];
					if (inputControl != null && this.IsPressed(inputControl) && (listenOptions.IncludeNonStandardControls || inputControl.IsStandard))
					{
						InputControlType target = inputControl.Target;
						if (target != InputControlType.Command || !listenOptions.IncludeNonStandardControls)
						{
							return target;
						}
					}
				}
			}
			return InputControlType.None;
		}

		// Token: 0x040019FC RID: 6652
		private InputControlType detectFound;

		// Token: 0x040019FD RID: 6653
		private int detectPhase;
	}
}
