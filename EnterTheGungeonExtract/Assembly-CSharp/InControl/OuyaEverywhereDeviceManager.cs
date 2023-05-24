using System;

namespace InControl
{
	// Token: 0x02000687 RID: 1671
	public class OuyaEverywhereDeviceManager : InputDeviceManager
	{
		// Token: 0x06002607 RID: 9735 RVA: 0x000A3128 File Offset: 0x000A1328
		public OuyaEverywhereDeviceManager()
		{
			for (int i = 0; i < 4; i++)
			{
				this.devices.Add(new OuyaEverywhereDevice(i));
			}
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x000A316C File Offset: 0x000A136C
		public override void Update(ulong updateTick, float deltaTime)
		{
			for (int i = 0; i < 4; i++)
			{
				OuyaEverywhereDevice ouyaEverywhereDevice = this.devices[i] as OuyaEverywhereDevice;
				if (ouyaEverywhereDevice.IsConnected != this.deviceConnected[i])
				{
					if (ouyaEverywhereDevice.IsConnected)
					{
						ouyaEverywhereDevice.BeforeAttach();
						InputManager.AttachDevice(ouyaEverywhereDevice);
					}
					else
					{
						InputManager.DetachDevice(ouyaEverywhereDevice);
					}
					this.deviceConnected[i] = ouyaEverywhereDevice.IsConnected;
				}
			}
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x000A31E0 File Offset: 0x000A13E0
		public static void Enable()
		{
		}

		// Token: 0x040019DE RID: 6622
		private bool[] deviceConnected = new bool[4];
	}
}
