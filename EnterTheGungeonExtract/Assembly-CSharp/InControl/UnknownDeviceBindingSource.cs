using System;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200069C RID: 1692
	public class UnknownDeviceBindingSource : BindingSource
	{
		// Token: 0x06002709 RID: 9993 RVA: 0x000A7728 File Offset: 0x000A5928
		internal UnknownDeviceBindingSource()
		{
			this.Control = UnknownDeviceControl.None;
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000A773C File Offset: 0x000A593C
		public UnknownDeviceBindingSource(UnknownDeviceControl control)
		{
			this.Control = control;
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x0600270B RID: 9995 RVA: 0x000A774C File Offset: 0x000A594C
		// (set) Token: 0x0600270C RID: 9996 RVA: 0x000A7754 File Offset: 0x000A5954
		public UnknownDeviceControl Control { get; protected set; }

		// Token: 0x0600270D RID: 9997 RVA: 0x000A7760 File Offset: 0x000A5960
		public override float GetValue(InputDevice device)
		{
			return this.Control.GetValue(device);
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x000A777C File Offset: 0x000A597C
		public override bool GetState(InputDevice device)
		{
			return device != null && Utility.IsNotZero(this.GetValue(device));
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x0600270F RID: 9999 RVA: 0x000A7794 File Offset: 0x000A5994
		public override string Name
		{
			get
			{
				if (base.BoundTo == null)
				{
					return string.Empty;
				}
				string text = string.Empty;
				if (this.Control.SourceRange == InputRangeType.ZeroToMinusOne)
				{
					text = "Negative ";
				}
				else if (this.Control.SourceRange == InputRangeType.ZeroToOne)
				{
					text = "Positive ";
				}
				InputDevice device = base.BoundTo.Device;
				if (device == InputDevice.Null)
				{
					return text + this.Control.Control.ToString();
				}
				InputControl control = device.GetControl(this.Control.Control);
				if (control == InputControl.Null)
				{
					return text + this.Control.Control.ToString();
				}
				return text + control.Handle;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002710 RID: 10000 RVA: 0x000A787C File Offset: 0x000A5A7C
		public override string DeviceName
		{
			get
			{
				if (base.BoundTo == null)
				{
					return string.Empty;
				}
				InputDevice device = base.BoundTo.Device;
				if (device == InputDevice.Null)
				{
					return "Unknown Controller";
				}
				return device.Name;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002711 RID: 10001 RVA: 0x000A78C0 File Offset: 0x000A5AC0
		public override InputDeviceClass DeviceClass
		{
			get
			{
				return InputDeviceClass.Controller;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002712 RID: 10002 RVA: 0x000A78C4 File Offset: 0x000A5AC4
		public override InputDeviceStyle DeviceStyle
		{
			get
			{
				return InputDeviceStyle.Unknown;
			}
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000A78C8 File Offset: 0x000A5AC8
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			UnknownDeviceBindingSource unknownDeviceBindingSource = other as UnknownDeviceBindingSource;
			return unknownDeviceBindingSource != null && this.Control == unknownDeviceBindingSource.Control;
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x000A790C File Offset: 0x000A5B0C
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			UnknownDeviceBindingSource unknownDeviceBindingSource = other as UnknownDeviceBindingSource;
			return unknownDeviceBindingSource != null && this.Control == unknownDeviceBindingSource.Control;
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x000A7948 File Offset: 0x000A5B48
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002716 RID: 10006 RVA: 0x000A796C File Offset: 0x000A5B6C
		public override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.UnknownDeviceBindingSource;
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002717 RID: 10007 RVA: 0x000A7970 File Offset: 0x000A5B70
		internal override bool IsValid
		{
			get
			{
				if (base.BoundTo == null)
				{
					Debug.LogError("Cannot query property 'IsValid' for unbound BindingSource.");
					return false;
				}
				InputDevice device = base.BoundTo.Device;
				return device == InputDevice.Null || device.HasControl(this.Control.Control);
			}
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000A79C4 File Offset: 0x000A5BC4
		internal override void Load(BinaryReader reader, ushort dataFormatVersion, bool upgrade)
		{
			UnknownDeviceControl unknownDeviceControl = default(UnknownDeviceControl);
			unknownDeviceControl.Load(reader, upgrade);
			this.Control = unknownDeviceControl;
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000A79EC File Offset: 0x000A5BEC
		internal override void Save(BinaryWriter writer)
		{
			this.Control.Save(writer);
		}
	}
}
