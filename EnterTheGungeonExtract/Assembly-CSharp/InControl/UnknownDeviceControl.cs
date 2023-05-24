using System;
using System.IO;

namespace InControl
{
	// Token: 0x0200069F RID: 1695
	public struct UnknownDeviceControl : IEquatable<UnknownDeviceControl>
	{
		// Token: 0x06002720 RID: 10016 RVA: 0x000A7B90 File Offset: 0x000A5D90
		public UnknownDeviceControl(InputControlType control, InputRangeType sourceRange)
		{
			this.Control = control;
			this.SourceRange = sourceRange;
			this.IsButton = Utility.TargetIsButton(control);
			this.IsAnalog = !this.IsButton;
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x000A7BBC File Offset: 0x000A5DBC
		internal float GetValue(InputDevice device)
		{
			if (device == null)
			{
				return 0f;
			}
			float value = device.GetControl(this.Control).Value;
			return InputRange.Remap(value, this.SourceRange, InputRangeType.ZeroToOne);
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002722 RID: 10018 RVA: 0x000A7BF4 File Offset: 0x000A5DF4
		public int Index
		{
			get
			{
				return this.Control - ((!this.IsButton) ? InputControlType.Analog0 : InputControlType.Button0);
			}
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x000A7C18 File Offset: 0x000A5E18
		public static bool operator ==(UnknownDeviceControl a, UnknownDeviceControl b)
		{
			if (object.ReferenceEquals(null, a))
			{
				return object.ReferenceEquals(null, b);
			}
			return a.Equals(b);
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000A7C40 File Offset: 0x000A5E40
		public static bool operator !=(UnknownDeviceControl a, UnknownDeviceControl b)
		{
			return !(a == b);
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x000A7C4C File Offset: 0x000A5E4C
		public bool Equals(UnknownDeviceControl other)
		{
			return this.Control == other.Control && this.SourceRange == other.SourceRange;
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x000A7C74 File Offset: 0x000A5E74
		public override bool Equals(object other)
		{
			return this.Equals((UnknownDeviceControl)other);
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000A7C84 File Offset: 0x000A5E84
		public override int GetHashCode()
		{
			return this.Control.GetHashCode() ^ this.SourceRange.GetHashCode();
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000A7CAC File Offset: 0x000A5EAC
		public static implicit operator bool(UnknownDeviceControl control)
		{
			return control.Control != InputControlType.None;
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000A7CBC File Offset: 0x000A5EBC
		public override string ToString()
		{
			return string.Format("UnknownDeviceControl( {0}, {1} )", this.Control.ToString(), this.SourceRange.ToString());
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x000A7CEC File Offset: 0x000A5EEC
		internal void Save(BinaryWriter writer)
		{
			writer.Write((int)this.Control);
			writer.Write((int)this.SourceRange);
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x000A7D08 File Offset: 0x000A5F08
		internal void Load(BinaryReader reader, bool upgrade)
		{
			if (upgrade)
			{
				this.Control = (InputControlType)BindingSource.UpgradeInputControlType(reader.ReadInt32());
				this.SourceRange = (InputRangeType)BindingSource.UpgradeInputRangeType(reader.ReadInt32());
			}
			else
			{
				this.Control = (InputControlType)reader.ReadInt32();
				this.SourceRange = (InputRangeType)reader.ReadInt32();
			}
			this.IsButton = Utility.TargetIsButton(this.Control);
			this.IsAnalog = !this.IsButton;
		}

		// Token: 0x04001AD2 RID: 6866
		public static readonly UnknownDeviceControl None = new UnknownDeviceControl(InputControlType.None, InputRangeType.None);

		// Token: 0x04001AD3 RID: 6867
		public InputControlType Control;

		// Token: 0x04001AD4 RID: 6868
		public InputRangeType SourceRange;

		// Token: 0x04001AD5 RID: 6869
		public bool IsButton;

		// Token: 0x04001AD6 RID: 6870
		public bool IsAnalog;
	}
}
