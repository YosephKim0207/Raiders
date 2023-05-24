using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FullSerializer
{
	// Token: 0x020005A2 RID: 1442
	public sealed class fsData
	{
		// Token: 0x06002216 RID: 8726 RVA: 0x000962E8 File Offset: 0x000944E8
		public fsData()
		{
			this._value = null;
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x000962F8 File Offset: 0x000944F8
		public fsData(bool boolean)
		{
			this._value = boolean;
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x0009630C File Offset: 0x0009450C
		public fsData(double f)
		{
			this._value = f;
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x00096320 File Offset: 0x00094520
		public fsData(long i)
		{
			this._value = i;
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x00096334 File Offset: 0x00094534
		public fsData(string str)
		{
			this._value = str;
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x00096344 File Offset: 0x00094544
		public fsData(Dictionary<string, fsData> dict)
		{
			this._value = dict;
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x00096354 File Offset: 0x00094554
		public fsData(List<fsData> list)
		{
			this._value = list;
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x00096364 File Offset: 0x00094564
		public static fsData CreateDictionary()
		{
			return new fsData(new Dictionary<string, fsData>((!fsConfig.IsCaseSensitive) ? StringComparer.CurrentCultureIgnoreCase : StringComparer.CurrentCulture));
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x0009638C File Offset: 0x0009458C
		public static fsData CreateList()
		{
			return new fsData(new List<fsData>());
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x00096398 File Offset: 0x00094598
		public static fsData CreateList(int capacity)
		{
			return new fsData(new List<fsData>(capacity));
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000963A8 File Offset: 0x000945A8
		internal void BecomeDictionary()
		{
			this._value = new Dictionary<string, fsData>();
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x000963B8 File Offset: 0x000945B8
		internal fsData Clone()
		{
			return new fsData
			{
				_value = this._value
			};
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06002222 RID: 8738 RVA: 0x000963D8 File Offset: 0x000945D8
		public fsDataType Type
		{
			get
			{
				if (this._value == null)
				{
					return fsDataType.Null;
				}
				if (this._value is double)
				{
					return fsDataType.Double;
				}
				if (this._value is long)
				{
					return fsDataType.Int64;
				}
				if (this._value is bool)
				{
					return fsDataType.Boolean;
				}
				if (this._value is string)
				{
					return fsDataType.String;
				}
				if (this._value is Dictionary<string, fsData>)
				{
					return fsDataType.Object;
				}
				if (this._value is List<fsData>)
				{
					return fsDataType.Array;
				}
				throw new InvalidOperationException("unknown JSON data type");
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06002223 RID: 8739 RVA: 0x00096468 File Offset: 0x00094668
		public bool IsNull
		{
			get
			{
				return this._value == null;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06002224 RID: 8740 RVA: 0x00096474 File Offset: 0x00094674
		public bool IsDouble
		{
			get
			{
				return this._value is double;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06002225 RID: 8741 RVA: 0x00096484 File Offset: 0x00094684
		public bool IsInt64
		{
			get
			{
				return this._value is long;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06002226 RID: 8742 RVA: 0x00096494 File Offset: 0x00094694
		public bool IsBool
		{
			get
			{
				return this._value is bool;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06002227 RID: 8743 RVA: 0x000964A4 File Offset: 0x000946A4
		public bool IsString
		{
			get
			{
				return this._value is string;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06002228 RID: 8744 RVA: 0x000964B4 File Offset: 0x000946B4
		public bool IsDictionary
		{
			get
			{
				return this._value is Dictionary<string, fsData>;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06002229 RID: 8745 RVA: 0x000964C4 File Offset: 0x000946C4
		public bool IsList
		{
			get
			{
				return this._value is List<fsData>;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x0600222A RID: 8746 RVA: 0x000964D4 File Offset: 0x000946D4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public double AsDouble
		{
			get
			{
				return this.Cast<double>();
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x0600222B RID: 8747 RVA: 0x000964DC File Offset: 0x000946DC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public long AsInt64
		{
			get
			{
				return this.Cast<long>();
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x0600222C RID: 8748 RVA: 0x000964E4 File Offset: 0x000946E4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool AsBool
		{
			get
			{
				return this.Cast<bool>();
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x0600222D RID: 8749 RVA: 0x000964EC File Offset: 0x000946EC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public string AsString
		{
			get
			{
				return this.Cast<string>();
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x000964F4 File Offset: 0x000946F4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public Dictionary<string, fsData> AsDictionary
		{
			get
			{
				return this.Cast<Dictionary<string, fsData>>();
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x0600222F RID: 8751 RVA: 0x000964FC File Offset: 0x000946FC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public List<fsData> AsList
		{
			get
			{
				return this.Cast<List<fsData>>();
			}
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x00096504 File Offset: 0x00094704
		private T Cast<T>()
		{
			if (this._value is T)
			{
				return (T)((object)this._value);
			}
			throw new InvalidCastException(string.Concat(new object[]
			{
				"Unable to cast <",
				this,
				"> (with type = ",
				this._value.GetType(),
				") to type ",
				typeof(T)
			}));
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x00096574 File Offset: 0x00094774
		public override string ToString()
		{
			return fsJsonPrinter.CompressedJson(this);
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x0009657C File Offset: 0x0009477C
		public override bool Equals(object obj)
		{
			return this.Equals(obj as fsData);
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x0009658C File Offset: 0x0009478C
		public bool Equals(fsData other)
		{
			if (other == null || this.Type != other.Type)
			{
				return false;
			}
			switch (this.Type)
			{
			case fsDataType.Array:
			{
				List<fsData> asList = this.AsList;
				List<fsData> asList2 = other.AsList;
				if (asList.Count != asList2.Count)
				{
					return false;
				}
				for (int i = 0; i < asList.Count; i++)
				{
					if (!asList[i].Equals(asList2[i]))
					{
						return false;
					}
				}
				return true;
			}
			case fsDataType.Object:
			{
				Dictionary<string, fsData> asDictionary = this.AsDictionary;
				Dictionary<string, fsData> asDictionary2 = other.AsDictionary;
				if (asDictionary.Count != asDictionary2.Count)
				{
					return false;
				}
				foreach (string text in asDictionary.Keys)
				{
					if (!asDictionary2.ContainsKey(text))
					{
						return false;
					}
					if (!asDictionary[text].Equals(asDictionary2[text]))
					{
						return false;
					}
				}
				return true;
			}
			case fsDataType.Double:
				return this.AsDouble == other.AsDouble || Math.Abs(this.AsDouble - other.AsDouble) < double.Epsilon;
			case fsDataType.Int64:
				return this.AsInt64 == other.AsInt64;
			case fsDataType.Boolean:
				return this.AsBool == other.AsBool;
			case fsDataType.String:
				return this.AsString == other.AsString;
			case fsDataType.Null:
				return true;
			default:
				throw new Exception("Unknown data type");
			}
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x00096758 File Offset: 0x00094958
		public static bool operator ==(fsData a, fsData b)
		{
			if (object.ReferenceEquals(a, b))
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.IsDouble && b.IsDouble)
			{
				return Math.Abs(a.AsDouble - b.AsDouble) < double.Epsilon;
			}
			return a.Equals(b);
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x000967BC File Offset: 0x000949BC
		public static bool operator !=(fsData a, fsData b)
		{
			return !(a == b);
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000967C8 File Offset: 0x000949C8
		public override int GetHashCode()
		{
			return this._value.GetHashCode();
		}

		// Token: 0x04001843 RID: 6211
		private object _value;

		// Token: 0x04001844 RID: 6212
		public static readonly fsData True = new fsData(true);

		// Token: 0x04001845 RID: 6213
		public static readonly fsData False = new fsData(false);

		// Token: 0x04001846 RID: 6214
		public static readonly fsData Null = new fsData();
	}
}
