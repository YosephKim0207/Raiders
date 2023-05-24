using System;

namespace InControl
{
	// Token: 0x0200069D RID: 1693
	public class UnknownDeviceBindingSourceListener : BindingSourceListener
	{
		// Token: 0x0600271B RID: 10011 RVA: 0x000A7A10 File Offset: 0x000A5C10
		public void Reset()
		{
			this.detectFound = UnknownDeviceControl.None;
			this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForInitialRelease;
			this.TakeSnapshotOnUnknownDevices();
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x000A7A2C File Offset: 0x000A5C2C
		private void TakeSnapshotOnUnknownDevices()
		{
			int count = InputManager.Devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.Devices[i];
				if (inputDevice.IsUnknown)
				{
					inputDevice.TakeSnapshot();
				}
			}
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000A7A74 File Offset: 0x000A5C74
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeUnknownControllers || device.IsKnown)
			{
				return null;
			}
			if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlRelease && this.detectFound && !this.IsPressed(this.detectFound, device))
			{
				UnknownDeviceBindingSource unknownDeviceBindingSource = new UnknownDeviceBindingSource(this.detectFound);
				this.Reset();
				return unknownDeviceBindingSource;
			}
			UnknownDeviceControl unknownDeviceControl = this.ListenForControl(listenOptions, device);
			if (unknownDeviceControl)
			{
				if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlPress)
				{
					this.detectFound = unknownDeviceControl;
					this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlRelease;
				}
			}
			else if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForInitialRelease)
			{
				this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlPress;
			}
			return null;
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000A7B24 File Offset: 0x000A5D24
		private bool IsPressed(UnknownDeviceControl control, InputDevice device)
		{
			float value = control.GetValue(device);
			return Utility.AbsoluteIsOverThreshold(value, 0.5f);
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x000A7B48 File Offset: 0x000A5D48
		private UnknownDeviceControl ListenForControl(BindingListenOptions listenOptions, InputDevice device)
		{
			if (device.IsUnknown)
			{
				UnknownDeviceControl firstPressedButton = device.GetFirstPressedButton();
				if (firstPressedButton)
				{
					return firstPressedButton;
				}
				UnknownDeviceControl firstPressedAnalog = device.GetFirstPressedAnalog();
				if (firstPressedAnalog)
				{
					return firstPressedAnalog;
				}
			}
			return UnknownDeviceControl.None;
		}

		// Token: 0x04001ACC RID: 6860
		private UnknownDeviceControl detectFound;

		// Token: 0x04001ACD RID: 6861
		private UnknownDeviceBindingSourceListener.DetectPhase detectPhase;

		// Token: 0x0200069E RID: 1694
		private enum DetectPhase
		{
			// Token: 0x04001ACF RID: 6863
			WaitForInitialRelease,
			// Token: 0x04001AD0 RID: 6864
			WaitForControlPress,
			// Token: 0x04001AD1 RID: 6865
			WaitForControlRelease
		}
	}
}
