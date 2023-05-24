using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000755 RID: 1877
	public class NativeInputDeviceManager : InputDeviceManager
	{
		// Token: 0x060029A0 RID: 10656 RVA: 0x000BD150 File Offset: 0x000BB350
		public NativeInputDeviceManager()
		{
			this.attachedDevices = new List<NativeInputDevice>();
			this.detachedDevices = new List<NativeInputDevice>();
			this.systemDeviceProfiles = new List<NativeInputDeviceProfile>(NativeInputDeviceProfileList.Profiles.Length);
			this.customDeviceProfiles = new List<NativeInputDeviceProfile>();
			this.deviceEvents = new uint[32];
			this.AddSystemDeviceProfiles();
			NativeInputOptions nativeInputOptions = default(NativeInputOptions);
			nativeInputOptions.enableXInput = InputManager.NativeInputEnableXInput;
			nativeInputOptions.preventSleep = InputManager.NativeInputPreventSleep;
			if (InputManager.NativeInputUpdateRate > 0U)
			{
				nativeInputOptions.updateRate = (ushort)InputManager.NativeInputUpdateRate;
			}
			else
			{
				nativeInputOptions.updateRate = (ushort)Mathf.FloorToInt(1f / Time.fixedDeltaTime);
			}
			Native.Init(nativeInputOptions);
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000BD204 File Offset: 0x000BB404
		public override void Destroy()
		{
			Native.Stop();
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x000BD20C File Offset: 0x000BB40C
		private uint NextPowerOfTwo(uint x)
		{
			if (x < 0U)
			{
				return 0U;
			}
			x -= 1U;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1U;
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x000BD244 File Offset: 0x000BB444
		public override void Update(ulong updateTick, float deltaTime)
		{
			IntPtr intPtr;
			int num = Native.GetDeviceEvents(out intPtr);
			if (num > 0)
			{
				Utility.ArrayExpand<uint>(ref this.deviceEvents, num);
				MarshalUtility.Copy(intPtr, this.deviceEvents, num);
				int num2 = 0;
				uint num3 = this.deviceEvents[num2++];
				int num4 = 0;
				while ((long)num4 < (long)((ulong)num3))
				{
					uint num5 = this.deviceEvents[num2++];
					StringBuilder stringBuilder = new StringBuilder(256);
					stringBuilder.Append("Attached native device with handle " + num5 + ":\n");
					NativeDeviceInfo nativeDeviceInfo;
					if (Native.GetDeviceInfo(num5, out nativeDeviceInfo))
					{
						stringBuilder.AppendFormat("Name: {0}\n", nativeDeviceInfo.name);
						stringBuilder.AppendFormat("Driver Type: {0}\n", nativeDeviceInfo.driverType);
						stringBuilder.AppendFormat("Location ID: {0}\n", nativeDeviceInfo.location);
						stringBuilder.AppendFormat("Serial Number: {0}\n", nativeDeviceInfo.serialNumber);
						stringBuilder.AppendFormat("Vendor ID: 0x{0:x}\n", nativeDeviceInfo.vendorID);
						stringBuilder.AppendFormat("Product ID: 0x{0:x}\n", nativeDeviceInfo.productID);
						stringBuilder.AppendFormat("Version Number: 0x{0:x}\n", nativeDeviceInfo.versionNumber);
						stringBuilder.AppendFormat("Buttons: {0}\n", nativeDeviceInfo.numButtons);
						stringBuilder.AppendFormat("Analogs: {0}\n", nativeDeviceInfo.numAnalogs);
						this.DetectDevice(num5, nativeDeviceInfo);
					}
					Logger.LogInfo(stringBuilder.ToString());
					num4++;
				}
				uint num6 = this.deviceEvents[num2++];
				int num7 = 0;
				while ((long)num7 < (long)((ulong)num6))
				{
					uint num8 = this.deviceEvents[num2++];
					Logger.LogInfo("Detached native device with handle " + num8 + ":");
					NativeInputDevice nativeInputDevice = this.FindAttachedDevice(num8);
					if (nativeInputDevice != null)
					{
						this.DetachDevice(nativeInputDevice);
					}
					else
					{
						Logger.LogWarning("Couldn't find device to detach with handle: " + num8);
					}
					num7++;
				}
			}
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x000BD454 File Offset: 0x000BB654
		private void DetectDevice(uint deviceHandle, NativeDeviceInfo deviceInfo)
		{
			NativeInputDeviceProfile nativeInputDeviceProfile = null;
			nativeInputDeviceProfile = nativeInputDeviceProfile ?? this.customDeviceProfiles.Find((NativeInputDeviceProfile profile) => profile.Matches(deviceInfo));
			nativeInputDeviceProfile = nativeInputDeviceProfile ?? this.systemDeviceProfiles.Find((NativeInputDeviceProfile profile) => profile.Matches(deviceInfo));
			nativeInputDeviceProfile = nativeInputDeviceProfile ?? this.customDeviceProfiles.Find((NativeInputDeviceProfile profile) => profile.LastResortMatches(deviceInfo));
			nativeInputDeviceProfile = nativeInputDeviceProfile ?? this.systemDeviceProfiles.Find((NativeInputDeviceProfile profile) => profile.LastResortMatches(deviceInfo));
			NativeInputDevice nativeInputDevice = this.FindDetachedDevice(deviceInfo) ?? new NativeInputDevice();
			nativeInputDevice.Initialize(deviceHandle, deviceInfo, nativeInputDeviceProfile);
			this.AttachDevice(nativeInputDevice);
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x000BD520 File Offset: 0x000BB720
		private void AttachDevice(NativeInputDevice device)
		{
			this.detachedDevices.Remove(device);
			this.attachedDevices.Add(device);
			InputManager.AttachDevice(device);
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x000BD544 File Offset: 0x000BB744
		private void DetachDevice(NativeInputDevice device)
		{
			this.attachedDevices.Remove(device);
			this.detachedDevices.Add(device);
			InputManager.DetachDevice(device);
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x000BD568 File Offset: 0x000BB768
		private NativeInputDevice FindAttachedDevice(uint deviceHandle)
		{
			int count = this.attachedDevices.Count;
			for (int i = 0; i < count; i++)
			{
				NativeInputDevice nativeInputDevice = this.attachedDevices[i];
				if (nativeInputDevice.Handle == deviceHandle)
				{
					return nativeInputDevice;
				}
			}
			return null;
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x000BD5B0 File Offset: 0x000BB7B0
		private NativeInputDevice FindDetachedDevice(NativeDeviceInfo deviceInfo)
		{
			ReadOnlyCollection<NativeInputDevice> readOnlyCollection = new ReadOnlyCollection<NativeInputDevice>(this.detachedDevices);
			if (NativeInputDeviceManager.CustomFindDetachedDevice != null)
			{
				return NativeInputDeviceManager.CustomFindDetachedDevice(deviceInfo, readOnlyCollection);
			}
			return NativeInputDeviceManager.SystemFindDetachedDevice(deviceInfo, readOnlyCollection);
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x000BD5E8 File Offset: 0x000BB7E8
		private static NativeInputDevice SystemFindDetachedDevice(NativeDeviceInfo deviceInfo, ReadOnlyCollection<NativeInputDevice> detachedDevices)
		{
			int count = detachedDevices.Count;
			for (int i = 0; i < count; i++)
			{
				NativeInputDevice nativeInputDevice = detachedDevices[i];
				if (nativeInputDevice.Info.HasSameVendorID(deviceInfo) && nativeInputDevice.Info.HasSameProductID(deviceInfo) && nativeInputDevice.Info.HasSameSerialNumber(deviceInfo))
				{
					return nativeInputDevice;
				}
			}
			for (int j = 0; j < count; j++)
			{
				NativeInputDevice nativeInputDevice2 = detachedDevices[j];
				if (nativeInputDevice2.Info.HasSameVendorID(deviceInfo) && nativeInputDevice2.Info.HasSameProductID(deviceInfo) && nativeInputDevice2.Info.HasSameLocation(deviceInfo))
				{
					return nativeInputDevice2;
				}
			}
			for (int k = 0; k < count; k++)
			{
				NativeInputDevice nativeInputDevice3 = detachedDevices[k];
				if (nativeInputDevice3.Info.HasSameVendorID(deviceInfo) && nativeInputDevice3.Info.HasSameProductID(deviceInfo) && nativeInputDevice3.Info.HasSameVersionNumber(deviceInfo))
				{
					return nativeInputDevice3;
				}
			}
			for (int l = 0; l < count; l++)
			{
				NativeInputDevice nativeInputDevice4 = detachedDevices[l];
				if (nativeInputDevice4.Info.HasSameLocation(deviceInfo))
				{
					return nativeInputDevice4;
				}
			}
			return null;
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x000BD75C File Offset: 0x000BB95C
		private void AddSystemDeviceProfile(NativeInputDeviceProfile deviceProfile)
		{
			if (deviceProfile.IsSupportedOnThisPlatform)
			{
				this.systemDeviceProfiles.Add(deviceProfile);
			}
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x000BD778 File Offset: 0x000BB978
		private void AddSystemDeviceProfiles()
		{
			foreach (string text in NativeInputDeviceProfileList.Profiles)
			{
				NativeInputDeviceProfile nativeInputDeviceProfile = (NativeInputDeviceProfile)Activator.CreateInstance(Type.GetType(text));
				this.AddSystemDeviceProfile(nativeInputDeviceProfile);
			}
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x000BD7BC File Offset: 0x000BB9BC
		public static bool CheckPlatformSupport(ICollection<string> errors)
		{
			if (Application.platform != RuntimePlatform.OSXPlayer && Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
			{
				return false;
			}
			try
			{
				NativeVersionInfo nativeVersionInfo;
				Native.GetVersionInfo(out nativeVersionInfo);
				Logger.LogInfo(string.Concat(new object[] { "InControl Native (version ", nativeVersionInfo.major, ".", nativeVersionInfo.minor, ".", nativeVersionInfo.patch, ")" }));
			}
			catch (DllNotFoundException ex)
			{
				if (errors != null)
				{
					errors.Add(ex.Message + Utility.PluginFileExtension() + " could not be found or is missing a dependency.");
				}
				return false;
			}
			return true;
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x000BD89C File Offset: 0x000BBA9C
		internal static bool Enable()
		{
			List<string> list = new List<string>();
			if (NativeInputDeviceManager.CheckPlatformSupport(list))
			{
				InputManager.AddDeviceManager<NativeInputDeviceManager>();
				return true;
			}
			foreach (string text in list)
			{
				Debug.LogError("Error enabling NativeInputDeviceManager: " + text);
			}
			return false;
		}

		// Token: 0x04001C89 RID: 7305
		public static Func<NativeDeviceInfo, ReadOnlyCollection<NativeInputDevice>, NativeInputDevice> CustomFindDetachedDevice;

		// Token: 0x04001C8A RID: 7306
		private List<NativeInputDevice> attachedDevices;

		// Token: 0x04001C8B RID: 7307
		private List<NativeInputDevice> detachedDevices;

		// Token: 0x04001C8C RID: 7308
		private List<NativeInputDeviceProfile> systemDeviceProfiles;

		// Token: 0x04001C8D RID: 7309
		private List<NativeInputDeviceProfile> customDeviceProfiles;

		// Token: 0x04001C8E RID: 7310
		private uint[] deviceEvents;
	}
}
