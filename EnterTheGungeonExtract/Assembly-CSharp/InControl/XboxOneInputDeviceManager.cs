using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000814 RID: 2068
	public class XboxOneInputDeviceManager : InputDeviceManager
	{
		// Token: 0x06002C0C RID: 11276 RVA: 0x000DF0D4 File Offset: 0x000DD2D4
		public XboxOneInputDeviceManager()
		{
			for (uint num = 1U; num <= 8U; num += 1U)
			{
				this.devices.Add(new XboxOneInputDevice(num));
			}
			this.UpdateInternal(0UL, 0f);
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x000DF124 File Offset: 0x000DD324
		private void UpdateInternal(ulong updateTick, float deltaTime)
		{
			for (int i = 0; i < 8; i++)
			{
				XboxOneInputDevice xboxOneInputDevice = this.devices[i] as XboxOneInputDevice;
				if (xboxOneInputDevice.IsConnected != this.deviceConnected[i])
				{
					if (xboxOneInputDevice.IsConnected)
					{
						InputManager.AttachDevice(xboxOneInputDevice);
					}
					else
					{
						InputManager.DetachDevice(xboxOneInputDevice);
					}
					this.deviceConnected[i] = xboxOneInputDevice.IsConnected;
				}
			}
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000DF194 File Offset: 0x000DD394
		public override void Update(ulong updateTick, float deltaTime)
		{
			this.UpdateInternal(updateTick, deltaTime);
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000DF1A0 File Offset: 0x000DD3A0
		public override void Destroy()
		{
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x000DF1A4 File Offset: 0x000DD3A4
		public static bool CheckPlatformSupport(ICollection<string> errors)
		{
			return Application.platform == RuntimePlatform.XboxOne;
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x000DF1B8 File Offset: 0x000DD3B8
		internal static bool Enable()
		{
			List<string> list = new List<string>();
			if (XboxOneInputDeviceManager.CheckPlatformSupport(list))
			{
				InputManager.AddDeviceManager<XboxOneInputDeviceManager>();
				return true;
			}
			foreach (string text in list)
			{
				Logger.LogError(text);
			}
			return false;
		}

		// Token: 0x04001DFD RID: 7677
		private const int maxDevices = 8;

		// Token: 0x04001DFE RID: 7678
		private bool[] deviceConnected = new bool[8];
	}
}
