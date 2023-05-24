using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
	// Token: 0x020006B3 RID: 1715
	public abstract class InputDeviceProfile
	{
		// Token: 0x0600283F RID: 10303 RVA: 0x000AB0E8 File Offset: 0x000A92E8
		public InputDeviceProfile()
		{
			this.Name = string.Empty;
			this.Meta = string.Empty;
			this.ControllerSymbology = GameOptions.ControllerSymbology.Xbox;
			this.AnalogMappings = new InputControlMapping[0];
			this.ButtonMappings = new InputControlMapping[0];
			this.IncludePlatforms = new string[0];
			this.ExcludePlatforms = new string[0];
			this.MinSystemBuildNumber = 0;
			this.MaxSystemBuildNumber = 0;
			this.DeviceClass = InputDeviceClass.Unknown;
			this.DeviceStyle = InputDeviceStyle.Unknown;
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06002840 RID: 10304 RVA: 0x000AB184 File Offset: 0x000A9384
		// (set) Token: 0x06002841 RID: 10305 RVA: 0x000AB18C File Offset: 0x000A938C
		[SerializeField]
		public string Name { get; protected set; }

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002842 RID: 10306 RVA: 0x000AB198 File Offset: 0x000A9398
		// (set) Token: 0x06002843 RID: 10307 RVA: 0x000AB1A0 File Offset: 0x000A93A0
		[SerializeField]
		public string Meta { get; protected set; }

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06002844 RID: 10308 RVA: 0x000AB1AC File Offset: 0x000A93AC
		// (set) Token: 0x06002845 RID: 10309 RVA: 0x000AB1B4 File Offset: 0x000A93B4
		[SerializeField]
		public InputControlMapping[] AnalogMappings { get; protected set; }

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06002846 RID: 10310 RVA: 0x000AB1C0 File Offset: 0x000A93C0
		// (set) Token: 0x06002847 RID: 10311 RVA: 0x000AB1C8 File Offset: 0x000A93C8
		[SerializeField]
		public InputControlMapping[] ButtonMappings { get; protected set; }

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06002848 RID: 10312 RVA: 0x000AB1D4 File Offset: 0x000A93D4
		// (set) Token: 0x06002849 RID: 10313 RVA: 0x000AB1DC File Offset: 0x000A93DC
		[SerializeField]
		public string[] IncludePlatforms { get; protected set; }

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x0600284A RID: 10314 RVA: 0x000AB1E8 File Offset: 0x000A93E8
		// (set) Token: 0x0600284B RID: 10315 RVA: 0x000AB1F0 File Offset: 0x000A93F0
		[SerializeField]
		public string[] ExcludePlatforms { get; protected set; }

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x0600284C RID: 10316 RVA: 0x000AB1FC File Offset: 0x000A93FC
		// (set) Token: 0x0600284D RID: 10317 RVA: 0x000AB204 File Offset: 0x000A9404
		[SerializeField]
		public int MaxSystemBuildNumber { get; protected set; }

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x0600284E RID: 10318 RVA: 0x000AB210 File Offset: 0x000A9410
		// (set) Token: 0x0600284F RID: 10319 RVA: 0x000AB218 File Offset: 0x000A9418
		[SerializeField]
		public int MinSystemBuildNumber { get; protected set; }

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06002850 RID: 10320 RVA: 0x000AB224 File Offset: 0x000A9424
		// (set) Token: 0x06002851 RID: 10321 RVA: 0x000AB22C File Offset: 0x000A942C
		[SerializeField]
		public InputDeviceClass DeviceClass { get; protected set; }

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06002852 RID: 10322 RVA: 0x000AB238 File Offset: 0x000A9438
		// (set) Token: 0x06002853 RID: 10323 RVA: 0x000AB240 File Offset: 0x000A9440
		[SerializeField]
		public InputDeviceStyle DeviceStyle { get; protected set; }

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06002854 RID: 10324 RVA: 0x000AB24C File Offset: 0x000A944C
		// (set) Token: 0x06002855 RID: 10325 RVA: 0x000AB254 File Offset: 0x000A9454
		[SerializeField]
		public GameOptions.ControllerSymbology ControllerSymbology
		{
			get
			{
				return this.m_controllerSymbology;
			}
			protected set
			{
				this.m_controllerSymbology = value;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06002856 RID: 10326 RVA: 0x000AB260 File Offset: 0x000A9460
		// (set) Token: 0x06002857 RID: 10327 RVA: 0x000AB268 File Offset: 0x000A9468
		[SerializeField]
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			protected set
			{
				this.sensitivity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06002858 RID: 10328 RVA: 0x000AB278 File Offset: 0x000A9478
		// (set) Token: 0x06002859 RID: 10329 RVA: 0x000AB280 File Offset: 0x000A9480
		[SerializeField]
		public float LowerDeadZone
		{
			get
			{
				return this.lowerDeadZone;
			}
			protected set
			{
				this.lowerDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x0600285A RID: 10330 RVA: 0x000AB290 File Offset: 0x000A9490
		// (set) Token: 0x0600285B RID: 10331 RVA: 0x000AB298 File Offset: 0x000A9498
		[SerializeField]
		public float UpperDeadZone
		{
			get
			{
				return this.upperDeadZone;
			}
			protected set
			{
				this.upperDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x0600285C RID: 10332 RVA: 0x000AB2A8 File Offset: 0x000A94A8
		// (set) Token: 0x0600285D RID: 10333 RVA: 0x000AB2B0 File Offset: 0x000A94B0
		[Obsolete("This property has been renamed to IncludePlatforms.", false)]
		public string[] SupportedPlatforms
		{
			get
			{
				return this.IncludePlatforms;
			}
			protected set
			{
				this.IncludePlatforms = value;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x0600285E RID: 10334 RVA: 0x000AB2BC File Offset: 0x000A94BC
		public virtual bool IsSupportedOnThisPlatform
		{
			get
			{
				int systemBuildNumber = Utility.GetSystemBuildNumber();
				if (this.MaxSystemBuildNumber > 0 && systemBuildNumber > this.MaxSystemBuildNumber)
				{
					return false;
				}
				if (this.MinSystemBuildNumber > 0 && systemBuildNumber < this.MinSystemBuildNumber)
				{
					return false;
				}
				if (this.ExcludePlatforms != null)
				{
					int num = this.ExcludePlatforms.Length;
					for (int i = 0; i < num; i++)
					{
						if (InputManager.Platform.Contains(this.ExcludePlatforms[i].ToUpper()))
						{
							return false;
						}
					}
				}
				if (this.IncludePlatforms == null || this.IncludePlatforms.Length == 0)
				{
					return true;
				}
				if (this.IncludePlatforms != null)
				{
					int num2 = this.IncludePlatforms.Length;
					for (int j = 0; j < num2; j++)
					{
						if (InputManager.Platform.Contains(this.IncludePlatforms[j].ToUpper()))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000AB3AC File Offset: 0x000A95AC
		internal static void Hide(Type type)
		{
			InputDeviceProfile.hideList.Add(type);
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06002860 RID: 10336 RVA: 0x000AB3BC File Offset: 0x000A95BC
		internal bool IsHidden
		{
			get
			{
				return InputDeviceProfile.hideList.Contains(base.GetType());
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06002861 RID: 10337 RVA: 0x000AB3D0 File Offset: 0x000A95D0
		public int AnalogCount
		{
			get
			{
				return this.AnalogMappings.Length;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06002862 RID: 10338 RVA: 0x000AB3DC File Offset: 0x000A95DC
		public int ButtonCount
		{
			get
			{
				return this.ButtonMappings.Length;
			}
		}

		// Token: 0x04001C0A RID: 7178
		private GameOptions.ControllerSymbology m_controllerSymbology = GameOptions.ControllerSymbology.Xbox;

		// Token: 0x04001C0B RID: 7179
		private static HashSet<Type> hideList = new HashSet<Type>();

		// Token: 0x04001C0C RID: 7180
		private float sensitivity = 1f;

		// Token: 0x04001C0D RID: 7181
		private float lowerDeadZone;

		// Token: 0x04001C0E RID: 7182
		private float upperDeadZone = 1f;
	}
}
