using System;
using System.IO;

namespace InControl
{
	// Token: 0x02000691 RID: 1681
	public class KeyBindingSource : BindingSource
	{
		// Token: 0x0600263E RID: 9790 RVA: 0x000A3A38 File Offset: 0x000A1C38
		internal KeyBindingSource()
		{
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000A3A40 File Offset: 0x000A1C40
		public KeyBindingSource(KeyCombo keyCombo)
		{
			this.Control = keyCombo;
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000A3A50 File Offset: 0x000A1C50
		public KeyBindingSource(params Key[] keys)
		{
			this.Control = new KeyCombo(keys);
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002641 RID: 9793 RVA: 0x000A3A64 File Offset: 0x000A1C64
		// (set) Token: 0x06002642 RID: 9794 RVA: 0x000A3A6C File Offset: 0x000A1C6C
		public KeyCombo Control { get; protected set; }

		// Token: 0x06002643 RID: 9795 RVA: 0x000A3A78 File Offset: 0x000A1C78
		public override float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000A3A98 File Offset: 0x000A1C98
		public override bool GetState(InputDevice inputDevice)
		{
			return this.Control.IsPressed;
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002645 RID: 9797 RVA: 0x000A3AB4 File Offset: 0x000A1CB4
		public override string Name
		{
			get
			{
				return this.Control.ToString();
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002646 RID: 9798 RVA: 0x000A3AD8 File Offset: 0x000A1CD8
		public override string DeviceName
		{
			get
			{
				return "Keyboard";
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002647 RID: 9799 RVA: 0x000A3AE0 File Offset: 0x000A1CE0
		public override InputDeviceClass DeviceClass
		{
			get
			{
				return InputDeviceClass.Keyboard;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06002648 RID: 9800 RVA: 0x000A3AE4 File Offset: 0x000A1CE4
		public override InputDeviceStyle DeviceStyle
		{
			get
			{
				return InputDeviceStyle.Unknown;
			}
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000A3AE8 File Offset: 0x000A1CE8
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			KeyBindingSource keyBindingSource = other as KeyBindingSource;
			return keyBindingSource != null && this.Control == keyBindingSource.Control;
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x000A3B2C File Offset: 0x000A1D2C
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			KeyBindingSource keyBindingSource = other as KeyBindingSource;
			return keyBindingSource != null && this.Control == keyBindingSource.Control;
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000A3B68 File Offset: 0x000A1D68
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x0600264C RID: 9804 RVA: 0x000A3B8C File Offset: 0x000A1D8C
		public override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.KeyBindingSource;
			}
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x000A3B90 File Offset: 0x000A1D90
		internal override void Load(BinaryReader reader, ushort dataFormatVersion, bool upgrade)
		{
			KeyCombo keyCombo = default(KeyCombo);
			keyCombo.Load(reader, dataFormatVersion, upgrade);
			this.Control = keyCombo;
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x000A3BB8 File Offset: 0x000A1DB8
		internal override void Save(BinaryWriter writer)
		{
			this.Control.Save(writer);
		}
	}
}
