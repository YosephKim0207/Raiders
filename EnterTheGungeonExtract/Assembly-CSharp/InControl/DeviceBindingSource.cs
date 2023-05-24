using System;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200068E RID: 1678
	public class DeviceBindingSource : BindingSource
	{
		// Token: 0x06002625 RID: 9765 RVA: 0x000A3604 File Offset: 0x000A1804
		internal DeviceBindingSource()
		{
			this.Control = InputControlType.None;
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x000A3614 File Offset: 0x000A1814
		public DeviceBindingSource(InputControlType control)
		{
			this.Control = control;
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002627 RID: 9767 RVA: 0x000A3624 File Offset: 0x000A1824
		// (set) Token: 0x06002628 RID: 9768 RVA: 0x000A362C File Offset: 0x000A182C
		public InputControlType Control { get; protected set; }

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06002629 RID: 9769 RVA: 0x000A3638 File Offset: 0x000A1838
		// (set) Token: 0x0600262A RID: 9770 RVA: 0x000A3640 File Offset: 0x000A1840
		public bool ForceRawInput { get; set; }

		// Token: 0x0600262B RID: 9771 RVA: 0x000A364C File Offset: 0x000A184C
		public override float GetValue(InputDevice inputDevice)
		{
			if (inputDevice == null)
			{
				return 0f;
			}
			return (!this.ForceRawInput) ? inputDevice.GetControl(this.Control).Value : inputDevice.GetControl(this.Control).RawValue;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000A368C File Offset: 0x000A188C
		public override bool GetState(InputDevice inputDevice)
		{
			return inputDevice != null && inputDevice.GetControl(this.Control).State;
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x0600262D RID: 9773 RVA: 0x000A36A8 File Offset: 0x000A18A8
		public override string Name
		{
			get
			{
				if (base.BoundTo == null)
				{
					return string.Empty;
				}
				InputDevice device = base.BoundTo.Device;
				InputControl control = device.GetControl(this.Control);
				if (control == InputControl.Null)
				{
					return this.Control.ToString();
				}
				return device.GetControl(this.Control).Handle;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x0600262E RID: 9774 RVA: 0x000A3710 File Offset: 0x000A1910
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
					return "Controller";
				}
				return device.Name;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x0600262F RID: 9775 RVA: 0x000A3754 File Offset: 0x000A1954
		public override InputDeviceClass DeviceClass
		{
			get
			{
				return (base.BoundTo != null) ? base.BoundTo.Device.DeviceClass : InputDeviceClass.Unknown;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002630 RID: 9776 RVA: 0x000A3778 File Offset: 0x000A1978
		public override InputDeviceStyle DeviceStyle
		{
			get
			{
				return (base.BoundTo != null) ? base.BoundTo.Device.DeviceStyle : InputDeviceStyle.Unknown;
			}
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x000A379C File Offset: 0x000A199C
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			DeviceBindingSource deviceBindingSource = other as DeviceBindingSource;
			return deviceBindingSource != null && this.Control == deviceBindingSource.Control;
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x000A37DC File Offset: 0x000A19DC
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			DeviceBindingSource deviceBindingSource = other as DeviceBindingSource;
			return deviceBindingSource != null && this.Control == deviceBindingSource.Control;
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x000A3814 File Offset: 0x000A1A14
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06002634 RID: 9780 RVA: 0x000A3838 File Offset: 0x000A1A38
		public override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.DeviceBindingSource;
			}
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000A383C File Offset: 0x000A1A3C
		internal override void Save(BinaryWriter writer)
		{
			writer.Write((int)this.Control);
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x000A384C File Offset: 0x000A1A4C
		internal override void Load(BinaryReader reader, ushort dataFormatVersion, bool upgrade)
		{
			if (upgrade)
			{
				this.Control = (InputControlType)BindingSource.UpgradeInputControlType(reader.ReadInt32());
			}
			else
			{
				this.Control = (InputControlType)reader.ReadInt32();
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002637 RID: 9783 RVA: 0x000A3878 File Offset: 0x000A1A78
		internal override bool IsValid
		{
			get
			{
				if (base.BoundTo == null)
				{
					Debug.LogError("Cannot query property 'IsValid' for unbound BindingSource.");
					return false;
				}
				return base.BoundTo.Device.HasControl(this.Control) || Utility.TargetIsStandard(this.Control);
			}
		}
	}
}
