using System;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000696 RID: 1686
	public class MouseBindingSource : BindingSource
	{
		// Token: 0x06002673 RID: 9843 RVA: 0x000A537C File Offset: 0x000A357C
		internal MouseBindingSource()
		{
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x000A5384 File Offset: 0x000A3584
		public MouseBindingSource(Mouse mouseControl)
		{
			this.Control = mouseControl;
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002675 RID: 9845 RVA: 0x000A5394 File Offset: 0x000A3594
		// (set) Token: 0x06002676 RID: 9846 RVA: 0x000A539C File Offset: 0x000A359C
		public Mouse Control { get; protected set; }

		// Token: 0x06002677 RID: 9847 RVA: 0x000A53A8 File Offset: 0x000A35A8
		internal static bool SafeGetMouseButton(int button)
		{
			try
			{
				return Input.GetMouseButton(button);
			}
			catch (ArgumentException)
			{
			}
			return false;
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x000A53DC File Offset: 0x000A35DC
		internal static bool ButtonIsPressed(Mouse control)
		{
			int num = MouseBindingSource.buttonTable[(int)control];
			return num >= 0 && MouseBindingSource.SafeGetMouseButton(num);
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x000A5400 File Offset: 0x000A3600
		internal static bool NegativeScrollWheelIsActive(float threshold)
		{
			float num = Mathf.Min(Input.GetAxisRaw("mouse z") * MouseBindingSource.ScaleZ, 0f);
			return num < -threshold;
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x000A5430 File Offset: 0x000A3630
		internal static bool PositiveScrollWheelIsActive(float threshold)
		{
			float num = Mathf.Max(0f, Input.GetAxisRaw("mouse z") * MouseBindingSource.ScaleZ);
			return num > threshold;
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x000A545C File Offset: 0x000A365C
		internal static float GetValue(Mouse mouseControl)
		{
			int num = MouseBindingSource.buttonTable[(int)mouseControl];
			if (num >= 0)
			{
				return (!MouseBindingSource.SafeGetMouseButton(num)) ? 0f : 1f;
			}
			switch (mouseControl)
			{
			case Mouse.NegativeX:
				return -Mathf.Min(Input.GetAxisRaw("mouse x") * MouseBindingSource.ScaleX, 0f);
			case Mouse.PositiveX:
				return Mathf.Max(0f, Input.GetAxisRaw("mouse x") * MouseBindingSource.ScaleX);
			case Mouse.NegativeY:
				return -Mathf.Min(Input.GetAxisRaw("mouse y") * MouseBindingSource.ScaleY, 0f);
			case Mouse.PositiveY:
				return Mathf.Max(0f, Input.GetAxisRaw("mouse y") * MouseBindingSource.ScaleY);
			case Mouse.PositiveScrollWheel:
				return Mathf.Max(0f, Input.GetAxisRaw("mouse z") * MouseBindingSource.ScaleZ);
			case Mouse.NegativeScrollWheel:
				return -Mathf.Min(Input.GetAxisRaw("mouse z") * MouseBindingSource.ScaleZ, 0f);
			default:
				return 0f;
			}
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x000A5564 File Offset: 0x000A3764
		public override float GetValue(InputDevice inputDevice)
		{
			return MouseBindingSource.GetValue(this.Control);
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x000A5574 File Offset: 0x000A3774
		public override bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x000A5584 File Offset: 0x000A3784
		public static string GetLocalizedMouseButtonName(int buttonIndex)
		{
			if (GameUIRoot.Instance)
			{
				dfControl p_playerCoinLabel = GameUIRoot.Instance.p_playerCoinLabel;
				if (p_playerCoinLabel)
				{
					if (buttonIndex == 0)
					{
						return p_playerCoinLabel.ForceGetLocalizedValue("#CONTROL_LMB");
					}
					if (buttonIndex == 1)
					{
						return p_playerCoinLabel.ForceGetLocalizedValue("#CONTROL_MMB");
					}
					if (buttonIndex == 2)
					{
						return p_playerCoinLabel.ForceGetLocalizedValue("#CONTROL_RMB");
					}
				}
			}
			return string.Empty;
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x0600267F RID: 9855 RVA: 0x000A55F8 File Offset: 0x000A37F8
		public override string Name
		{
			get
			{
				if (GameUIRoot.Instance)
				{
					dfControl p_playerCoinLabel = GameUIRoot.Instance.p_playerCoinLabel;
					if (p_playerCoinLabel)
					{
						if (this.Control == Mouse.LeftButton)
						{
							return p_playerCoinLabel.ForceGetLocalizedValue("#CONTROL_LMB");
						}
						if (this.Control == Mouse.MiddleButton)
						{
							return p_playerCoinLabel.ForceGetLocalizedValue("#CONTROL_MMB");
						}
						if (this.Control == Mouse.RightButton)
						{
							return p_playerCoinLabel.ForceGetLocalizedValue("#CONTROL_RMB");
						}
					}
				}
				return this.Control.ToString();
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002680 RID: 9856 RVA: 0x000A5688 File Offset: 0x000A3888
		public override string DeviceName
		{
			get
			{
				return "Mouse";
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06002681 RID: 9857 RVA: 0x000A5690 File Offset: 0x000A3890
		public override InputDeviceClass DeviceClass
		{
			get
			{
				return InputDeviceClass.Mouse;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002682 RID: 9858 RVA: 0x000A5694 File Offset: 0x000A3894
		public override InputDeviceStyle DeviceStyle
		{
			get
			{
				return InputDeviceStyle.Unknown;
			}
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x000A5698 File Offset: 0x000A3898
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			MouseBindingSource mouseBindingSource = other as MouseBindingSource;
			return mouseBindingSource != null && this.Control == mouseBindingSource.Control;
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x000A56D8 File Offset: 0x000A38D8
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			MouseBindingSource mouseBindingSource = other as MouseBindingSource;
			return mouseBindingSource != null && this.Control == mouseBindingSource.Control;
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x000A5710 File Offset: 0x000A3910
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002686 RID: 9862 RVA: 0x000A5734 File Offset: 0x000A3934
		public override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.MouseBindingSource;
			}
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x000A5738 File Offset: 0x000A3938
		internal override void Save(BinaryWriter writer)
		{
			writer.Write((int)this.Control);
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x000A5748 File Offset: 0x000A3948
		internal override void Load(BinaryReader reader, ushort dataFormatVersion, bool upgrade)
		{
			this.Control = (Mouse)reader.ReadInt32();
		}

		// Token: 0x04001A8D RID: 6797
		public static float ScaleX = 0.05f;

		// Token: 0x04001A8E RID: 6798
		public static float ScaleY = 0.05f;

		// Token: 0x04001A8F RID: 6799
		public static float ScaleZ = 0.05f;

		// Token: 0x04001A90 RID: 6800
		public static float JitterThreshold = 0.05f;

		// Token: 0x04001A91 RID: 6801
		private static readonly int[] buttonTable = new int[]
		{
			-1, 0, 1, 2, -1, -1, -1, -1, -1, -1,
			3, 4, 5, 6, 7, 8
		};
	}
}
