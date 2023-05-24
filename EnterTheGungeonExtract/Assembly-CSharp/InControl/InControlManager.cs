using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InControl
{
	// Token: 0x020006A0 RID: 1696
	public class InControlManager : SingletonMonoBehavior<InControlManager, MonoBehaviour>
	{
		// Token: 0x0600272E RID: 10030 RVA: 0x000A7DA8 File Offset: 0x000A5FA8
		private void OnEnable()
		{
			if (!base.EnforceSingleton())
			{
				return;
			}
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
			{
				this.enableXInput = true;
			}
			InputManager.InvertYAxis = this.invertYAxis;
			InputManager.SuspendInBackground = this.suspendInBackground;
			InputManager.EnableICade = this.enableICade;
			InputManager.EnableXInput = this.enableXInput;
			InputManager.XInputUpdateRate = (uint)Mathf.Max(this.xInputUpdateRate, 0);
			InputManager.XInputBufferSize = (uint)Mathf.Max(this.xInputBufferSize, 0);
			InputManager.EnableNativeInput = this.enableNativeInput;
			InputManager.NativeInputEnableXInput = this.nativeInputEnableXInput;
			InputManager.NativeInputUpdateRate = (uint)Mathf.Max(this.nativeInputUpdateRate, 0);
			InputManager.NativeInputPreventSleep = this.nativeInputPreventSleep;
			if (InputManager.SetupInternal())
			{
				Debug.Log("InControl (version " + InputManager.Version + ")");
				Logger.OnLogMessage -= InControlManager.LogMessage;
				Logger.OnLogMessage += InControlManager.LogMessage;
				foreach (string text in this.customProfiles)
				{
					Type type = Type.GetType(text);
					if (type == null)
					{
						Debug.LogError("Cannot find class for custom profile: " + text);
					}
					else
					{
						UnityInputDeviceProfileBase unityInputDeviceProfileBase = Activator.CreateInstance(type) as UnityInputDeviceProfileBase;
						if (unityInputDeviceProfileBase != null)
						{
							InputManager.AttachDevice(new UnityInputDevice(unityInputDeviceProfileBase));
						}
					}
				}
			}
			SceneManager.sceneLoaded -= this.OnSceneWasLoaded;
			SceneManager.sceneLoaded += this.OnSceneWasLoaded;
			if (this.dontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x000A7F84 File Offset: 0x000A6184
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.OnSceneWasLoaded;
			if (SingletonMonoBehavior<InControlManager, MonoBehaviour>.Instance == this)
			{
				InputManager.ResetInternal();
			}
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x000A7FAC File Offset: 0x000A61AC
		private void Update()
		{
			if (!this.useFixedUpdate || Utility.IsZero(Time.timeScale))
			{
				InputManager.UpdateInternal();
			}
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x000A7FD0 File Offset: 0x000A61D0
		private void FixedUpdate()
		{
			if (this.useFixedUpdate)
			{
				InputManager.UpdateInternal();
			}
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x000A7FE4 File Offset: 0x000A61E4
		private void OnApplicationFocus(bool focusState)
		{
			InputManager.OnApplicationFocus(focusState);
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x000A7FEC File Offset: 0x000A61EC
		private void OnApplicationPause(bool pauseState)
		{
			InputManager.OnApplicationPause(pauseState);
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x000A7FF4 File Offset: 0x000A61F4
		private void OnApplicationQuit()
		{
			InputManager.OnApplicationQuit();
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x000A7FFC File Offset: 0x000A61FC
		private void OnSceneWasLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			InputManager.OnLevelWasLoaded();
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x000A8004 File Offset: 0x000A6204
		private static void LogMessage(LogMessage logMessage)
		{
			LogMessageType type = logMessage.type;
			if (type != LogMessageType.Info)
			{
				if (type != LogMessageType.Warning)
				{
					if (type == LogMessageType.Error)
					{
						Debug.LogError(logMessage.text);
					}
				}
				else
				{
					Debug.LogWarning(logMessage.text);
				}
			}
			else
			{
				Debug.Log(logMessage.text);
			}
		}

		// Token: 0x04001AD7 RID: 6871
		public bool logDebugInfo;

		// Token: 0x04001AD8 RID: 6872
		public bool invertYAxis;

		// Token: 0x04001AD9 RID: 6873
		public bool useFixedUpdate;

		// Token: 0x04001ADA RID: 6874
		public bool dontDestroyOnLoad;

		// Token: 0x04001ADB RID: 6875
		public bool suspendInBackground;

		// Token: 0x04001ADC RID: 6876
		public bool enableICade;

		// Token: 0x04001ADD RID: 6877
		public bool enableXInput;

		// Token: 0x04001ADE RID: 6878
		public bool xInputOverrideUpdateRate;

		// Token: 0x04001ADF RID: 6879
		public int xInputUpdateRate;

		// Token: 0x04001AE0 RID: 6880
		public bool xInputOverrideBufferSize;

		// Token: 0x04001AE1 RID: 6881
		public int xInputBufferSize;

		// Token: 0x04001AE2 RID: 6882
		public bool enableNativeInput;

		// Token: 0x04001AE3 RID: 6883
		public bool nativeInputEnableXInput = true;

		// Token: 0x04001AE4 RID: 6884
		public bool nativeInputPreventSleep;

		// Token: 0x04001AE5 RID: 6885
		public bool nativeInputOverrideUpdateRate;

		// Token: 0x04001AE6 RID: 6886
		public int nativeInputUpdateRate;

		// Token: 0x04001AE7 RID: 6887
		public List<string> customProfiles = new List<string>();
	}
}
