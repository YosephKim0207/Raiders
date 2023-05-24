using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;

namespace InControl
{
	// Token: 0x020006B7 RID: 1719
	public class InputManager
	{
		// Token: 0x14000077 RID: 119
		// (add) Token: 0x0600288A RID: 10378 RVA: 0x000AC0EC File Offset: 0x000AA2EC
		// (remove) Token: 0x0600288B RID: 10379 RVA: 0x000AC120 File Offset: 0x000AA320
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnSetup;

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x0600288C RID: 10380 RVA: 0x000AC154 File Offset: 0x000AA354
		// (remove) Token: 0x0600288D RID: 10381 RVA: 0x000AC188 File Offset: 0x000AA388
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<ulong, float> OnUpdate;

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x0600288E RID: 10382 RVA: 0x000AC1BC File Offset: 0x000AA3BC
		// (remove) Token: 0x0600288F RID: 10383 RVA: 0x000AC1F0 File Offset: 0x000AA3F0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnReset;

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x06002890 RID: 10384 RVA: 0x000AC224 File Offset: 0x000AA424
		// (remove) Token: 0x06002891 RID: 10385 RVA: 0x000AC258 File Offset: 0x000AA458
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<InputDevice> OnDeviceAttached;

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06002892 RID: 10386 RVA: 0x000AC28C File Offset: 0x000AA48C
		// (remove) Token: 0x06002893 RID: 10387 RVA: 0x000AC2C0 File Offset: 0x000AA4C0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<InputDevice> OnDeviceDetached;

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06002894 RID: 10388 RVA: 0x000AC2F4 File Offset: 0x000AA4F4
		// (remove) Token: 0x06002895 RID: 10389 RVA: 0x000AC328 File Offset: 0x000AA528
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<InputDevice> OnActiveDeviceChanged;

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06002896 RID: 10390 RVA: 0x000AC35C File Offset: 0x000AA55C
		// (remove) Token: 0x06002897 RID: 10391 RVA: 0x000AC390 File Offset: 0x000AA590
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal static event Action<ulong, float> OnUpdateDevices;

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06002898 RID: 10392 RVA: 0x000AC3C4 File Offset: 0x000AA5C4
		// (remove) Token: 0x06002899 RID: 10393 RVA: 0x000AC3F8 File Offset: 0x000AA5F8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal static event Action<ulong, float> OnCommitDevices;

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x0600289A RID: 10394 RVA: 0x000AC42C File Offset: 0x000AA62C
		// (set) Token: 0x0600289B RID: 10395 RVA: 0x000AC434 File Offset: 0x000AA634
		public static bool CommandWasPressed { get; private set; }

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x0600289C RID: 10396 RVA: 0x000AC43C File Offset: 0x000AA63C
		// (set) Token: 0x0600289D RID: 10397 RVA: 0x000AC444 File Offset: 0x000AA644
		public static bool InvertYAxis { get; set; }

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x0600289E RID: 10398 RVA: 0x000AC44C File Offset: 0x000AA64C
		// (set) Token: 0x0600289F RID: 10399 RVA: 0x000AC454 File Offset: 0x000AA654
		public static bool IsSetup { get; private set; }

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x060028A0 RID: 10400 RVA: 0x000AC45C File Offset: 0x000AA65C
		// (set) Token: 0x060028A1 RID: 10401 RVA: 0x000AC464 File Offset: 0x000AA664
		internal static string Platform { get; private set; }

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x060028A2 RID: 10402 RVA: 0x000AC46C File Offset: 0x000AA66C
		[Obsolete("Use InputManager.CommandWasPressed instead.")]
		public static bool MenuWasPressed
		{
			get
			{
				return InputManager.CommandWasPressed;
			}
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000AC474 File Offset: 0x000AA674
		[Obsolete("Calling InputManager.Setup() directly is no longer supported. Use the InControlManager component to manage the lifecycle of the input manager instead.", true)]
		public static void Setup()
		{
			InputManager.SetupInternal();
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000AC47C File Offset: 0x000AA67C
		internal static bool SetupInternal()
		{
			if (InputManager.IsSetup)
			{
				return false;
			}
			InputManager.Platform = Utility.GetWindowsVersion().ToUpper();
			InputManager.initialTime = 0f;
			InputManager.currentTime = 0f;
			InputManager.lastUpdateTime = 0f;
			InputManager.currentTick = 0UL;
			InputManager.applicationIsFocused = true;
			InputManager.deviceManagers.Clear();
			InputManager.deviceManagerTable.Clear();
			InputManager.devices.Clear();
			InputManager.Devices = new ReadOnlyCollection<InputDevice>(InputManager.devices);
			InputManager.activeDevice = InputDevice.Null;
			InputManager.playerActionSets.Clear();
			InputManager.IsSetup = true;
			bool flag = true;
			bool flag2 = InputManager.EnableNativeInput && NativeInputDeviceManager.Enable();
			if (flag2)
			{
				flag = false;
			}
			if (InputManager.EnableXInput && flag)
			{
				XInputDeviceManager.Enable();
			}
			if (InputManager.OnSetup != null)
			{
				InputManager.OnSetup();
				InputManager.OnSetup = null;
			}
			if (flag)
			{
				InputManager.AddDeviceManager<UnityInputDeviceManager>();
			}
			return true;
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000AC570 File Offset: 0x000AA770
		[Obsolete("Calling InputManager.Reset() method directly is no longer supported. Use the InControlManager component to manage the lifecycle of the input manager instead.", true)]
		public static void Reset()
		{
			InputManager.ResetInternal();
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000AC578 File Offset: 0x000AA778
		internal static void ResetInternal()
		{
			if (InputManager.OnReset != null)
			{
				InputManager.OnReset();
			}
			InputManager.OnSetup = null;
			InputManager.OnUpdate = null;
			InputManager.OnReset = null;
			InputManager.OnActiveDeviceChanged = null;
			InputManager.OnDeviceAttached = null;
			InputManager.OnDeviceDetached = null;
			InputManager.OnUpdateDevices = null;
			InputManager.OnCommitDevices = null;
			InputManager.DestroyDeviceManagers();
			InputManager.DestroyDevices();
			InputManager.playerActionSets.Clear();
			InputManager.IsSetup = false;
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000AC5E4 File Offset: 0x000AA7E4
		[Obsolete("Calling InputManager.Update() directly is no longer supported. Use the InControlManager component to manage the lifecycle of the input manager instead.", true)]
		public static void Update()
		{
			InputManager.UpdateInternal();
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000AC5EC File Offset: 0x000AA7EC
		internal static void UpdateInternal()
		{
			InputManager.AssertIsSetup();
			if (InputManager.OnSetup != null)
			{
				InputManager.OnSetup();
				InputManager.OnSetup = null;
			}
			if (!InputManager.enabled)
			{
				return;
			}
			if (InputManager.SuspendInBackground && !InputManager.applicationIsFocused)
			{
				return;
			}
			InputManager.currentTick += 1UL;
			InputManager.UpdateCurrentTime();
			float num = InputManager.currentTime - InputManager.lastUpdateTime;
			InputManager.UpdateDeviceManagers(num);
			InputManager.CommandWasPressed = false;
			InputManager.UpdateDevices(num);
			InputManager.CommitDevices(num);
			InputManager.UpdateActiveDevice();
			InputManager.UpdatePlayerActionSets(num);
			if (InputManager.OnUpdate != null)
			{
				InputManager.OnUpdate(InputManager.currentTick, num);
			}
			InputManager.lastUpdateTime = InputManager.currentTime;
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000AC6A0 File Offset: 0x000AA8A0
		public static void Reload()
		{
			InputManager.ResetInternal();
			InputManager.SetupInternal();
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000AC6B0 File Offset: 0x000AA8B0
		private static void AssertIsSetup()
		{
			if (!InputManager.IsSetup)
			{
				throw new Exception("InputManager is not initialized. Call InputManager.Setup() first.");
			}
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000AC6C8 File Offset: 0x000AA8C8
		private static void SetZeroTickOnAllControls()
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				ReadOnlyCollection<InputControl> controls = InputManager.devices[i].Controls;
				int count2 = controls.Count;
				for (int j = 0; j < count2; j++)
				{
					InputControl inputControl = controls[j];
					if (inputControl != null)
					{
						inputControl.SetZeroTick();
					}
				}
			}
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x000AC738 File Offset: 0x000AA938
		public static void ClearInputState()
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.devices[i].ClearInputState();
			}
			int count2 = InputManager.playerActionSets.Count;
			for (int j = 0; j < count2; j++)
			{
				InputManager.playerActionSets[j].ClearInputState();
			}
			InputManager.activeDevice = InputDevice.Null;
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000AC7AC File Offset: 0x000AA9AC
		internal static void OnApplicationFocus(bool focusState)
		{
			if (!focusState)
			{
				if (InputManager.SuspendInBackground)
				{
					InputManager.ClearInputState();
				}
				InputManager.SetZeroTickOnAllControls();
			}
			InputManager.applicationIsFocused = focusState;
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000AC7D0 File Offset: 0x000AA9D0
		internal static void OnApplicationPause(bool pauseState)
		{
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000AC7D4 File Offset: 0x000AA9D4
		internal static void OnApplicationQuit()
		{
			InputManager.ResetInternal();
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000AC7DC File Offset: 0x000AA9DC
		internal static void OnLevelWasLoaded()
		{
			InputManager.SetZeroTickOnAllControls();
			InputManager.UpdateInternal();
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000AC7E8 File Offset: 0x000AA9E8
		public static void AddDeviceManager(InputDeviceManager deviceManager)
		{
			InputManager.AssertIsSetup();
			Type type = deviceManager.GetType();
			if (InputManager.deviceManagerTable.ContainsKey(type))
			{
				Logger.LogError("A device manager of type '" + type.Name + "' already exists; cannot add another.");
				return;
			}
			InputManager.deviceManagers.Add(deviceManager);
			InputManager.deviceManagerTable.Add(type, deviceManager);
			deviceManager.Update(InputManager.currentTick, InputManager.currentTime - InputManager.lastUpdateTime);
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x000AC85C File Offset: 0x000AAA5C
		public static void AddDeviceManager<T>() where T : InputDeviceManager, new()
		{
			InputManager.AddDeviceManager(new T());
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x000AC870 File Offset: 0x000AAA70
		public static T GetDeviceManager<T>() where T : InputDeviceManager
		{
			InputDeviceManager inputDeviceManager;
			if (InputManager.deviceManagerTable.TryGetValue(typeof(T), out inputDeviceManager))
			{
				return inputDeviceManager as T;
			}
			return (T)((object)null);
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000AC8AC File Offset: 0x000AAAAC
		public static bool HasDeviceManager<T>() where T : InputDeviceManager
		{
			return InputManager.deviceManagerTable.ContainsKey(typeof(T));
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x000AC8C4 File Offset: 0x000AAAC4
		private static void UpdateCurrentTime()
		{
			if (InputManager.initialTime < 1E-45f)
			{
				InputManager.initialTime = Time.realtimeSinceStartup;
			}
			InputManager.currentTime = Mathf.Max(0f, Time.realtimeSinceStartup - InputManager.initialTime);
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x000AC8FC File Offset: 0x000AAAFC
		private static void UpdateDeviceManagers(float deltaTime)
		{
			int count = InputManager.deviceManagers.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.deviceManagers[i].Update(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x000AC93C File Offset: 0x000AAB3C
		private static void DestroyDeviceManagers()
		{
			int count = InputManager.deviceManagers.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.deviceManagers[i].Destroy();
			}
			InputManager.deviceManagers.Clear();
			InputManager.deviceManagerTable.Clear();
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x000AC98C File Offset: 0x000AAB8C
		private static void DestroyDevices()
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.devices[i];
				inputDevice.OnDetached();
			}
			InputManager.devices.Clear();
			InputManager.activeDevice = InputDevice.Null;
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x000AC9DC File Offset: 0x000AABDC
		private static void UpdateDevices(float deltaTime)
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.devices[i];
				inputDevice.Update(InputManager.currentTick, deltaTime);
			}
			if (InputManager.OnUpdateDevices != null)
			{
				InputManager.OnUpdateDevices(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x000ACA38 File Offset: 0x000AAC38
		private static void CommitDevices(float deltaTime)
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.devices[i];
				inputDevice.Commit(InputManager.currentTick, deltaTime);
				if (inputDevice.CommandWasPressed)
				{
					InputManager.CommandWasPressed = true;
				}
			}
			if (InputManager.OnCommitDevices != null)
			{
				InputManager.OnCommitDevices(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x000ACAA8 File Offset: 0x000AACA8
		private static void UpdateActiveDevice()
		{
			InputDevice inputDevice = InputManager.ActiveDevice;
			if (InputManager.ActiveDevice is XInputDevice && !GameManager.Options.allowXinputControllers)
			{
				InputManager.ActiveDevice = null;
			}
			if (!(InputManager.ActiveDevice is XInputDevice) && !GameManager.Options.allowNonXinputControllers)
			{
				InputManager.ActiveDevice = null;
			}
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice2 = InputManager.devices[i];
				if (GameManager.Options.allowXinputControllers || !(inputDevice2 is XInputDevice))
				{
					if (GameManager.Options.allowNonXinputControllers || inputDevice2 is XInputDevice)
					{
						if (inputDevice2.LastChangedAfter(InputManager.ActiveDevice) && !inputDevice2.Passive)
						{
							if ((InputManager.ActiveDevice is XInputDevice && !(inputDevice2 is XInputDevice)) || (inputDevice2 is XInputDevice && !(InputManager.ActiveDevice is XInputDevice)))
							{
								if (inputDevice2.LastChangeAfterTime(InputManager.ActiveDevice))
								{
									InputManager.ActiveDevice = inputDevice2;
								}
								else if (inputDevice2 is XInputDevice)
								{
									InputManager.ActiveDevice = inputDevice2;
								}
							}
							else
							{
								InputManager.ActiveDevice = inputDevice2;
							}
						}
					}
				}
			}
			if (inputDevice != InputManager.ActiveDevice)
			{
				if (GameManager.HasInstance && GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER)
				{
					if (inputDevice != null)
					{
						inputDevice.Vibrate(0f);
					}
					UnityEngine.Debug.LogWarningFormat("swapping active device from {0} ({1}|{2}) to {3} ({4}|{5})", new object[]
					{
						inputDevice.Name,
						inputDevice.LastChangeTick,
						inputDevice.LastChangeTime,
						InputManager.ActiveDevice.Name,
						InputManager.ActiveDevice.LastChangeTick,
						InputManager.ActiveDevice.LastChangeTime
					});
				}
				if (InputManager.OnActiveDeviceChanged != null)
				{
					InputManager.OnActiveDeviceChanged(InputManager.ActiveDevice);
				}
			}
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x000ACCA4 File Offset: 0x000AAEA4
		public static void AttachDevice(InputDevice inputDevice)
		{
			InputManager.AssertIsSetup();
			if (!inputDevice.IsSupportedOnThisPlatform)
			{
				return;
			}
			if (inputDevice.IsAttached)
			{
				return;
			}
			if (!InputManager.devices.Contains(inputDevice))
			{
				InputManager.devices.Add(inputDevice);
				InputManager.devices.Sort((InputDevice d1, InputDevice d2) => d1.SortOrder.CompareTo(d2.SortOrder));
			}
			inputDevice.OnAttached();
			if (InputManager.OnDeviceAttached != null)
			{
				InputManager.OnDeviceAttached(inputDevice);
			}
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x000ACD2C File Offset: 0x000AAF2C
		public static void DetachDevice(InputDevice inputDevice)
		{
			if (!InputManager.IsSetup)
			{
				return;
			}
			if (!inputDevice.IsAttached)
			{
				return;
			}
			InputManager.devices.Remove(inputDevice);
			if (InputManager.ActiveDevice == inputDevice)
			{
				InputManager.ActiveDevice = InputDevice.Null;
			}
			inputDevice.OnDetached();
			if (InputManager.OnDeviceDetached != null)
			{
				InputManager.OnDeviceDetached(inputDevice);
			}
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x000ACD8C File Offset: 0x000AAF8C
		public static void HideDevicesWithProfile(Type type)
		{
			if (type.IsSubclassOf(typeof(UnityInputDeviceProfile)))
			{
				InputDeviceProfile.Hide(type);
			}
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x000ACDAC File Offset: 0x000AAFAC
		internal static void AttachPlayerActionSet(PlayerActionSet playerActionSet)
		{
			if (!InputManager.playerActionSets.Contains(playerActionSet))
			{
				InputManager.playerActionSets.Add(playerActionSet);
			}
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x000ACDCC File Offset: 0x000AAFCC
		internal static void DetachPlayerActionSet(PlayerActionSet playerActionSet)
		{
			InputManager.playerActionSets.Remove(playerActionSet);
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x000ACDDC File Offset: 0x000AAFDC
		internal static void UpdatePlayerActionSets(float deltaTime)
		{
			int count = InputManager.playerActionSets.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.playerActionSets[i].Update(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060028C2 RID: 10434 RVA: 0x000ACE1C File Offset: 0x000AB01C
		public static bool AnyKeyIsPressed
		{
			get
			{
				return KeyCombo.Detect(true).IncludeCount > 0;
			}
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x000ACE3C File Offset: 0x000AB03C
		public static InputDevice GetActiveDeviceForPlayer(int playerID)
		{
			if (GameManager.Instance.AllPlayers.Length < 2)
			{
				return InputManager.ActiveDevice;
			}
			int num = playerID;
			if (GameManager.Options.PlayerIDtoDeviceIndexMap.ContainsKey(playerID))
			{
				num = GameManager.Options.PlayerIDtoDeviceIndexMap[playerID];
			}
			else
			{
				GameManager.Options.PlayerIDtoDeviceIndexMap.Add(playerID, playerID);
			}
			if (num >= InputManager.devices.Count || num < 0)
			{
				return null;
			}
			return InputManager.devices[num];
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060028C4 RID: 10436 RVA: 0x000ACEC4 File Offset: 0x000AB0C4
		// (set) Token: 0x060028C5 RID: 10437 RVA: 0x000ACEE0 File Offset: 0x000AB0E0
		public static InputDevice ActiveDevice
		{
			get
			{
				return (InputManager.activeDevice != null) ? InputManager.activeDevice : InputDevice.Null;
			}
			set
			{
				InputManager.activeDevice = ((value != null) ? value : InputDevice.Null);
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060028C6 RID: 10438 RVA: 0x000ACEF8 File Offset: 0x000AB0F8
		// (set) Token: 0x060028C7 RID: 10439 RVA: 0x000ACF00 File Offset: 0x000AB100
		public static bool Enabled
		{
			get
			{
				return InputManager.enabled;
			}
			set
			{
				if (InputManager.enabled != value)
				{
					if (value)
					{
						InputManager.SetZeroTickOnAllControls();
						InputManager.UpdateInternal();
					}
					else
					{
						InputManager.ClearInputState();
						InputManager.SetZeroTickOnAllControls();
					}
					InputManager.enabled = value;
				}
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060028C8 RID: 10440 RVA: 0x000ACF34 File Offset: 0x000AB134
		// (set) Token: 0x060028C9 RID: 10441 RVA: 0x000ACF3C File Offset: 0x000AB13C
		public static bool SuspendInBackground { get; internal set; }

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x060028CA RID: 10442 RVA: 0x000ACF44 File Offset: 0x000AB144
		// (set) Token: 0x060028CB RID: 10443 RVA: 0x000ACF4C File Offset: 0x000AB14C
		public static bool EnableNativeInput { get; internal set; }

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060028CC RID: 10444 RVA: 0x000ACF54 File Offset: 0x000AB154
		// (set) Token: 0x060028CD RID: 10445 RVA: 0x000ACF74 File Offset: 0x000AB174
		public static bool EnableXInput
		{
			get
			{
				return Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || InputManager.enableXInput;
			}
			set
			{
				InputManager.enableXInput = value;
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060028CE RID: 10446 RVA: 0x000ACF7C File Offset: 0x000AB17C
		// (set) Token: 0x060028CF RID: 10447 RVA: 0x000ACF84 File Offset: 0x000AB184
		public static uint XInputUpdateRate { get; internal set; }

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x060028D0 RID: 10448 RVA: 0x000ACF8C File Offset: 0x000AB18C
		// (set) Token: 0x060028D1 RID: 10449 RVA: 0x000ACF94 File Offset: 0x000AB194
		public static uint XInputBufferSize { get; internal set; }

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x060028D2 RID: 10450 RVA: 0x000ACF9C File Offset: 0x000AB19C
		// (set) Token: 0x060028D3 RID: 10451 RVA: 0x000ACFA4 File Offset: 0x000AB1A4
		public static bool NativeInputEnableXInput { get; internal set; }

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x000ACFAC File Offset: 0x000AB1AC
		// (set) Token: 0x060028D5 RID: 10453 RVA: 0x000ACFB4 File Offset: 0x000AB1B4
		public static bool NativeInputPreventSleep { get; internal set; }

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x060028D6 RID: 10454 RVA: 0x000ACFBC File Offset: 0x000AB1BC
		// (set) Token: 0x060028D7 RID: 10455 RVA: 0x000ACFC4 File Offset: 0x000AB1C4
		public static uint NativeInputUpdateRate { get; internal set; }

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x060028D8 RID: 10456 RVA: 0x000ACFCC File Offset: 0x000AB1CC
		// (set) Token: 0x060028D9 RID: 10457 RVA: 0x000ACFD4 File Offset: 0x000AB1D4
		public static bool EnableICade { get; internal set; }

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x000ACFDC File Offset: 0x000AB1DC
		internal static VersionInfo UnityVersion
		{
			get
			{
				if (InputManager.unityVersion == null)
				{
					InputManager.unityVersion = new VersionInfo?(VersionInfo.UnityVersion());
				}
				return InputManager.unityVersion.Value;
			}
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000AD008 File Offset: 0x000AB208
		public static ulong GetCurrentTick()
		{
			return InputManager.CurrentTick;
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x060028DC RID: 10460 RVA: 0x000AD010 File Offset: 0x000AB210
		internal static ulong CurrentTick
		{
			get
			{
				return InputManager.currentTick;
			}
		}

		// Token: 0x04001C42 RID: 7234
		public static readonly VersionInfo Version = VersionInfo.InControlVersion();

		// Token: 0x04001C4B RID: 7243
		private static List<InputDeviceManager> deviceManagers = new List<InputDeviceManager>();

		// Token: 0x04001C4C RID: 7244
		private static Dictionary<Type, InputDeviceManager> deviceManagerTable = new Dictionary<Type, InputDeviceManager>();

		// Token: 0x04001C4D RID: 7245
		private static InputDevice activeDevice = InputDevice.Null;

		// Token: 0x04001C4E RID: 7246
		private static List<InputDevice> devices = new List<InputDevice>();

		// Token: 0x04001C4F RID: 7247
		private static List<PlayerActionSet> playerActionSets = new List<PlayerActionSet>();

		// Token: 0x04001C50 RID: 7248
		public static ReadOnlyCollection<InputDevice> Devices;

		// Token: 0x04001C55 RID: 7253
		private static bool applicationIsFocused;

		// Token: 0x04001C56 RID: 7254
		private static float initialTime;

		// Token: 0x04001C57 RID: 7255
		private static float currentTime;

		// Token: 0x04001C58 RID: 7256
		private static float lastUpdateTime;

		// Token: 0x04001C59 RID: 7257
		private static ulong currentTick;

		// Token: 0x04001C5A RID: 7258
		private static VersionInfo? unityVersion;

		// Token: 0x04001C5B RID: 7259
		private static bool enabled;

		// Token: 0x04001C5E RID: 7262
		private static bool enableXInput;
	}
}
