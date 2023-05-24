using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000804 RID: 2052
	public class UnityInputDeviceManager : InputDeviceManager
	{
		// Token: 0x06002B69 RID: 11113 RVA: 0x000DC220 File Offset: 0x000DA420
		public UnityInputDeviceManager()
		{
			this.AddSystemDeviceProfiles();
			this.QueryJoystickInfo();
			this.AttachDevices();
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x000DC258 File Offset: 0x000DA458
		public override void Update(ulong updateTick, float deltaTime)
		{
			this.deviceRefreshTimer += deltaTime;
			if (this.deviceRefreshTimer >= 1f)
			{
				this.deviceRefreshTimer = 0f;
				this.QueryJoystickInfo();
				if (this.JoystickInfoHasChanged)
				{
					Logger.LogInfo("Change in attached Unity joysticks detected; refreshing device list.");
					this.DetachDevices();
					this.AttachDevices();
				}
			}
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x000DC2B8 File Offset: 0x000DA4B8
		private void QueryJoystickInfo()
		{
			this.joystickNames = Input.GetJoystickNames();
			this.joystickCount = this.joystickNames.Length;
			this.joystickHash = 527 + this.joystickCount;
			for (int i = 0; i < this.joystickCount; i++)
			{
				this.joystickHash = this.joystickHash * 31 + this.joystickNames[i].GetHashCode();
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06002B6C RID: 11116 RVA: 0x000DC324 File Offset: 0x000DA524
		private bool JoystickInfoHasChanged
		{
			get
			{
				return this.joystickHash != this.lastJoystickHash || this.joystickCount != this.lastJoystickCount;
			}
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x000DC34C File Offset: 0x000DA54C
		private void AttachDevices()
		{
			this.AttachKeyboardDevices();
			this.AttachJoystickDevices();
			this.lastJoystickCount = this.joystickCount;
			this.lastJoystickHash = this.joystickHash;
			if (UnityInputDeviceManager.OnAllDevicesReattached != null)
			{
				UnityInputDeviceManager.OnAllDevicesReattached();
			}
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x000DC388 File Offset: 0x000DA588
		private void DetachDevices()
		{
			int count = this.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.DetachDevice(this.devices[i]);
			}
			this.devices.Clear();
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x000DC3D0 File Offset: 0x000DA5D0
		public void ReloadDevices()
		{
			this.QueryJoystickInfo();
			this.DetachDevices();
			this.AttachDevices();
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x000DC3E4 File Offset: 0x000DA5E4
		private void AttachDevice(UnityInputDevice device)
		{
			this.devices.Add(device);
			InputManager.AttachDevice(device);
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x000DC3F8 File Offset: 0x000DA5F8
		private void AttachKeyboardDevices()
		{
			int count = this.systemDeviceProfiles.Count;
			for (int i = 0; i < count; i++)
			{
				UnityInputDeviceProfileBase unityInputDeviceProfileBase = this.systemDeviceProfiles[i];
				if (unityInputDeviceProfileBase.IsNotJoystick && unityInputDeviceProfileBase.IsSupportedOnThisPlatform)
				{
					this.AttachDevice(new UnityInputDevice(unityInputDeviceProfileBase));
				}
			}
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x000DC454 File Offset: 0x000DA654
		private void AttachJoystickDevices()
		{
			try
			{
				for (int i = 0; i < this.joystickCount; i++)
				{
					this.DetectJoystickDevice(i + 1, this.joystickNames[i]);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.Message);
				Logger.LogError(ex.StackTrace);
			}
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x000DC4BC File Offset: 0x000DA6BC
		private bool HasAttachedDeviceWithJoystickId(int unityJoystickId)
		{
			int count = this.devices.Count;
			for (int i = 0; i < count; i++)
			{
				UnityInputDevice unityInputDevice = this.devices[i] as UnityInputDevice;
				if (unityInputDevice != null && unityInputDevice.JoystickId == unityJoystickId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x000DC510 File Offset: 0x000DA710
		private void DetectJoystickDevice(int unityJoystickId, string unityJoystickName)
		{
			if (this.HasAttachedDeviceWithJoystickId(unityJoystickId))
			{
				return;
			}
			if (unityJoystickName.IndexOf("webcam", StringComparison.OrdinalIgnoreCase) != -1)
			{
				return;
			}
			if (InputManager.UnityVersion < new VersionInfo(4, 5, 0, 0) && (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) && unityJoystickName == "Unknown Wireless Controller")
			{
				return;
			}
			if (InputManager.UnityVersion >= new VersionInfo(4, 6, 3, 0) && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && string.IsNullOrEmpty(unityJoystickName))
			{
				return;
			}
			UnityInputDeviceProfileBase unityInputDeviceProfileBase = null;
			if (unityInputDeviceProfileBase == null)
			{
				unityInputDeviceProfileBase = this.customDeviceProfiles.Find((UnityInputDeviceProfileBase config) => config.HasJoystickName(unityJoystickName));
			}
			if (unityInputDeviceProfileBase == null)
			{
				unityInputDeviceProfileBase = this.systemDeviceProfiles.Find((UnityInputDeviceProfileBase config) => config.HasJoystickName(unityJoystickName));
			}
			if (unityInputDeviceProfileBase == null)
			{
				unityInputDeviceProfileBase = this.customDeviceProfiles.Find((UnityInputDeviceProfileBase config) => config.HasLastResortRegex(unityJoystickName));
			}
			if (unityInputDeviceProfileBase == null)
			{
				unityInputDeviceProfileBase = this.systemDeviceProfiles.Find((UnityInputDeviceProfileBase config) => config.HasLastResortRegex(unityJoystickName));
			}
			if (unityInputDeviceProfileBase == null)
			{
				UnityInputDevice unityInputDevice = new UnityInputDevice(unityJoystickId, unityJoystickName);
				this.AttachDevice(unityInputDevice);
				Debug.Log(string.Concat(new object[] { "[InControl] Joystick ", unityJoystickId, ": \"", unityJoystickName, "\"" }));
				Logger.LogWarning(string.Concat(new object[] { "Device ", unityJoystickId, " with name \"", unityJoystickName, "\" does not match any supported profiles and will be considered an unknown controller." }));
				return;
			}
			if (!unityInputDeviceProfileBase.IsHidden)
			{
				UnityInputDevice unityInputDevice2 = new UnityInputDevice(unityInputDeviceProfileBase, unityJoystickId, unityJoystickName);
				this.AttachDevice(unityInputDevice2);
				Logger.LogInfo(string.Concat(new object[]
				{
					"Device ",
					unityJoystickId,
					" matched profile ",
					unityInputDeviceProfileBase.GetType().Name,
					" (",
					unityInputDeviceProfileBase.Name,
					")"
				}));
			}
			else
			{
				Logger.LogInfo(string.Concat(new object[]
				{
					"Device ",
					unityJoystickId,
					" matching profile ",
					unityInputDeviceProfileBase.GetType().Name,
					" (",
					unityInputDeviceProfileBase.Name,
					") is hidden and will not be attached."
				}));
			}
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x000DC7A0 File Offset: 0x000DA9A0
		private void AddSystemDeviceProfile(UnityInputDeviceProfile deviceProfile)
		{
			if (deviceProfile.IsSupportedOnThisPlatform)
			{
				this.systemDeviceProfiles.Add(deviceProfile);
			}
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x000DC7BC File Offset: 0x000DA9BC
		private void AddSystemDeviceProfiles()
		{
			foreach (string text in UnityInputDeviceProfileList.Profiles)
			{
				UnityInputDeviceProfile unityInputDeviceProfile = (UnityInputDeviceProfile)Activator.CreateInstance(Type.GetType(text));
				this.AddSystemDeviceProfile(unityInputDeviceProfile);
			}
		}

		// Token: 0x04001D9B RID: 7579
		public static Action OnAllDevicesReattached;

		// Token: 0x04001D9C RID: 7580
		private const float deviceRefreshInterval = 1f;

		// Token: 0x04001D9D RID: 7581
		private float deviceRefreshTimer;

		// Token: 0x04001D9E RID: 7582
		private List<UnityInputDeviceProfileBase> systemDeviceProfiles = new List<UnityInputDeviceProfileBase>(UnityInputDeviceProfileList.Profiles.Length);

		// Token: 0x04001D9F RID: 7583
		private List<UnityInputDeviceProfileBase> customDeviceProfiles = new List<UnityInputDeviceProfileBase>();

		// Token: 0x04001DA0 RID: 7584
		private string[] joystickNames;

		// Token: 0x04001DA1 RID: 7585
		private int lastJoystickCount;

		// Token: 0x04001DA2 RID: 7586
		private int lastJoystickHash;

		// Token: 0x04001DA3 RID: 7587
		private int joystickCount;

		// Token: 0x04001DA4 RID: 7588
		private int joystickHash;
	}
}
