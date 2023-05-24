using System;
using System.IO;

namespace InControl
{
	// Token: 0x0200068A RID: 1674
	public abstract class BindingSource : InputControlSource, IEquatable<BindingSource>
	{
		// Token: 0x06002610 RID: 9744
		public abstract float GetValue(InputDevice inputDevice);

		// Token: 0x06002611 RID: 9745
		public abstract bool GetState(InputDevice inputDevice);

		// Token: 0x06002612 RID: 9746
		public abstract bool Equals(BindingSource other);

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002613 RID: 9747
		public abstract string Name { get; }

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06002614 RID: 9748
		public abstract string DeviceName { get; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06002615 RID: 9749
		public abstract InputDeviceClass DeviceClass { get; }

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06002616 RID: 9750
		public abstract InputDeviceStyle DeviceStyle { get; }

		// Token: 0x06002617 RID: 9751 RVA: 0x000A3268 File Offset: 0x000A1468
		public static bool operator ==(BindingSource a, BindingSource b)
		{
			return object.ReferenceEquals(a, b) || (a != null && b != null && a.BindingSourceType == b.BindingSourceType && a.Equals(b));
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x000A32A0 File Offset: 0x000A14A0
		public static bool operator !=(BindingSource a, BindingSource b)
		{
			return !(a == b);
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000A32AC File Offset: 0x000A14AC
		public override bool Equals(object obj)
		{
			return this.Equals((BindingSource)obj);
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x000A32BC File Offset: 0x000A14BC
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x0600261B RID: 9755
		public abstract BindingSourceType BindingSourceType { get; }

		// Token: 0x0600261C RID: 9756
		internal abstract void Save(BinaryWriter writer);

		// Token: 0x0600261D RID: 9757
		internal abstract void Load(BinaryReader reader, ushort dataFormatVersion, bool upgrade);

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x0600261E RID: 9758 RVA: 0x000A32C4 File Offset: 0x000A14C4
		// (set) Token: 0x0600261F RID: 9759 RVA: 0x000A32CC File Offset: 0x000A14CC
		internal PlayerAction BoundTo { get; set; }

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002620 RID: 9760 RVA: 0x000A32D8 File Offset: 0x000A14D8
		internal virtual bool IsValid
		{
			get
			{
				return this.BoundTo != null;
			}
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x000A32E8 File Offset: 0x000A14E8
		public static int UpgradeInputRangeType(int oldInt)
		{
			switch (oldInt)
			{
			case 0:
				return 0;
			case 1:
				return 1;
			case 2:
				return 3;
			case 3:
				return 4;
			case 4:
				return 7;
			case 5:
				return 8;
			case 6:
				return 9;
			default:
				return oldInt;
			}
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x000A3324 File Offset: 0x000A1524
		public static int UpgradeInputControlType(int oldInt)
		{
			switch (oldInt)
			{
			case 0:
				return 0;
			case 1:
				return 1;
			case 2:
				return 2;
			case 3:
				return 3;
			case 4:
				return 4;
			case 5:
				return 5;
			case 6:
				return 6;
			case 7:
				return 7;
			case 8:
				return 8;
			case 9:
				return 9;
			case 10:
				return 10;
			case 11:
				return 11;
			case 12:
				return 12;
			case 13:
				return 13;
			case 14:
				return 14;
			case 15:
				return 19;
			case 16:
				return 20;
			case 17:
				return 21;
			case 18:
				return 22;
			case 19:
				return 15;
			case 20:
				return 16;
			case 21:
				return 17;
			case 22:
				return 18;
			case 23:
				return 300;
			case 24:
				return 100;
			case 25:
				return 101;
			case 26:
				return 102;
			case 27:
				return 103;
			case 28:
				return 104;
			case 29:
				return 105;
			case 30:
				return 106;
			case 31:
				return 107;
			case 32:
				return 108;
			case 33:
				return 109;
			case 34:
				return 110;
			case 35:
				return 250;
			case 36:
				return 251;
			case 37:
				return 252;
			case 38:
				return 253;
			case 39:
				return 255;
			case 40:
				return 256;
			case 41:
				return 257;
			case 42:
				return 400;
			case 43:
				return 401;
			case 44:
				return 402;
			case 45:
				return 403;
			case 46:
				return 404;
			case 47:
				return 405;
			case 48:
				return 406;
			case 49:
				return 407;
			case 50:
				return 408;
			case 51:
				return 409;
			case 52:
				return 410;
			case 53:
				return 411;
			case 54:
				return 412;
			case 55:
				return 413;
			case 56:
				return 414;
			case 57:
				return 415;
			case 58:
				return 416;
			case 59:
				return 417;
			case 60:
				return 418;
			case 61:
				return 419;
			case 62:
				return 500;
			case 63:
				return 501;
			case 64:
				return 502;
			case 65:
				return 503;
			case 66:
				return 504;
			case 67:
				return 505;
			case 68:
				return 506;
			case 69:
				return 507;
			case 70:
				return 508;
			case 71:
				return 509;
			case 72:
				return 510;
			case 73:
				return 511;
			case 74:
				return 512;
			case 75:
				return 513;
			case 76:
				return 514;
			case 77:
				return 515;
			case 78:
				return 516;
			case 79:
				return 517;
			case 80:
				return 518;
			case 81:
				return 519;
			default:
				return oldInt;
			}
		}
	}
}
