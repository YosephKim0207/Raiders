using System;
using System.Collections.Generic;
using System.IO;

namespace InControl
{
	// Token: 0x02000693 RID: 1683
	public struct KeyCombo
	{
		// Token: 0x06002652 RID: 9810 RVA: 0x000A3C98 File Offset: 0x000A1E98
		public KeyCombo(params Key[] keys)
		{
			this.includeData = 0UL;
			this.includeSize = 0;
			this.excludeData = 0UL;
			this.excludeSize = 0;
			for (int i = 0; i < keys.Length; i++)
			{
				this.AddInclude(keys[i]);
			}
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x000A3CE0 File Offset: 0x000A1EE0
		private void AddIncludeInt(int key)
		{
			if (this.includeSize == 8)
			{
				return;
			}
			this.includeData |= (ulong)((ulong)((long)key & 255L) << this.includeSize * 8);
			this.includeSize++;
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x000A3D20 File Offset: 0x000A1F20
		private int GetIncludeInt(int index)
		{
			return (int)((this.includeData >> index * 8) & 255UL);
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x000A3D38 File Offset: 0x000A1F38
		[Obsolete("Use KeyCombo.AddInclude instead.")]
		public void Add(Key key)
		{
			this.AddInclude(key);
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x000A3D44 File Offset: 0x000A1F44
		[Obsolete("Use KeyCombo.GetInclude instead.")]
		public Key Get(int index)
		{
			return this.GetInclude(index);
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x000A3D50 File Offset: 0x000A1F50
		public void AddInclude(Key key)
		{
			this.AddIncludeInt((int)key);
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000A3D5C File Offset: 0x000A1F5C
		public Key GetInclude(int index)
		{
			if (index < 0 || index >= this.includeSize)
			{
				throw new IndexOutOfRangeException(string.Concat(new object[] { "Index ", index, " is out of the range 0..", this.includeSize }));
			}
			return (Key)this.GetIncludeInt(index);
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x000A3DBC File Offset: 0x000A1FBC
		private void AddExcludeInt(int key)
		{
			if (this.excludeSize == 8)
			{
				return;
			}
			this.excludeData |= (ulong)((ulong)((long)key & 255L) << this.excludeSize * 8);
			this.excludeSize++;
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x000A3DFC File Offset: 0x000A1FFC
		private int GetExcludeInt(int index)
		{
			return (int)((this.excludeData >> index * 8) & 255UL);
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x000A3E14 File Offset: 0x000A2014
		public void AddExclude(Key key)
		{
			this.AddExcludeInt((int)key);
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x000A3E20 File Offset: 0x000A2020
		public Key GetExclude(int index)
		{
			if (index < 0 || index >= this.excludeSize)
			{
				throw new IndexOutOfRangeException(string.Concat(new object[] { "Index ", index, " is out of the range 0..", this.excludeSize }));
			}
			return (Key)this.GetExcludeInt(index);
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x000A3E80 File Offset: 0x000A2080
		public static KeyCombo With(params Key[] keys)
		{
			return new KeyCombo(keys);
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x000A3E88 File Offset: 0x000A2088
		public KeyCombo AndNot(params Key[] keys)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				this.AddExclude(keys[i]);
			}
			return this;
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x000A3EB8 File Offset: 0x000A20B8
		public void Clear()
		{
			this.includeData = 0UL;
			this.includeSize = 0;
			this.excludeData = 0UL;
			this.excludeSize = 0;
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06002660 RID: 9824 RVA: 0x000A3ED8 File Offset: 0x000A20D8
		[Obsolete("Use KeyCombo.IncludeCount instead.")]
		public int Count
		{
			get
			{
				return this.includeSize;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06002661 RID: 9825 RVA: 0x000A3EE0 File Offset: 0x000A20E0
		public int IncludeCount
		{
			get
			{
				return this.includeSize;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06002662 RID: 9826 RVA: 0x000A3EE8 File Offset: 0x000A20E8
		public int ExcludeCount
		{
			get
			{
				return this.excludeSize;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06002663 RID: 9827 RVA: 0x000A3EF0 File Offset: 0x000A20F0
		public bool IsPressed
		{
			get
			{
				if (this.includeSize == 0)
				{
					return false;
				}
				bool flag = true;
				for (int i = 0; i < this.includeSize; i++)
				{
					int includeInt = this.GetIncludeInt(i);
					flag = flag && KeyInfo.KeyList[includeInt].IsPressed;
				}
				for (int j = 0; j < this.excludeSize; j++)
				{
					int excludeInt = this.GetExcludeInt(j);
					if (KeyInfo.KeyList[excludeInt].IsPressed)
					{
						return false;
					}
				}
				return flag;
			}
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x000A3F80 File Offset: 0x000A2180
		public static KeyCombo Detect(bool modifiersAsKeys)
		{
			KeyCombo keyCombo = default(KeyCombo);
			if (modifiersAsKeys)
			{
				for (int i = 5; i < 13; i++)
				{
					if (KeyInfo.KeyList[i].IsPressed)
					{
						keyCombo.AddIncludeInt(i);
						return keyCombo;
					}
				}
			}
			else
			{
				for (int j = 1; j < 5; j++)
				{
					if (KeyInfo.KeyList[j].IsPressed)
					{
						keyCombo.AddIncludeInt(j);
					}
				}
			}
			for (int k = 13; k < KeyInfo.KeyList.Length; k++)
			{
				if (KeyInfo.KeyList[k].IsPressed)
				{
					keyCombo.AddIncludeInt(k);
					return keyCombo;
				}
			}
			keyCombo.Clear();
			return keyCombo;
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x000A4044 File Offset: 0x000A2244
		public override string ToString()
		{
			string text;
			if (!KeyCombo.cachedStrings.TryGetValue(this.includeData, out text))
			{
				text = string.Empty;
				for (int i = 0; i < this.includeSize; i++)
				{
					if (i != 0)
					{
						text += " ";
					}
					int includeInt = this.GetIncludeInt(i);
					text += KeyInfo.KeyList[includeInt].Name;
				}
			}
			return text;
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x000A40B8 File Offset: 0x000A22B8
		public static bool operator ==(KeyCombo a, KeyCombo b)
		{
			return a.includeData == b.includeData && a.excludeData == b.excludeData;
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x000A40E0 File Offset: 0x000A22E0
		public static bool operator !=(KeyCombo a, KeyCombo b)
		{
			return a.includeData != b.includeData || a.excludeData != b.excludeData;
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x000A410C File Offset: 0x000A230C
		public override bool Equals(object other)
		{
			if (other is KeyCombo)
			{
				KeyCombo keyCombo = (KeyCombo)other;
				return this.includeData == keyCombo.includeData && this.excludeData == keyCombo.excludeData;
			}
			return false;
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x000A4154 File Offset: 0x000A2354
		public override int GetHashCode()
		{
			int num = 17;
			num = num * 31 + this.includeData.GetHashCode();
			return num * 31 + this.excludeData.GetHashCode();
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x000A4194 File Offset: 0x000A2394
		internal void Load(BinaryReader reader, ushort dataFormatVersion, bool upgrade)
		{
			if (dataFormatVersion == 1)
			{
				this.includeSize = reader.ReadInt32();
				this.includeData = reader.ReadUInt64();
				return;
			}
			if (dataFormatVersion == 2)
			{
				this.includeSize = reader.ReadInt32();
				this.includeData = reader.ReadUInt64();
				this.excludeSize = reader.ReadInt32();
				this.excludeData = reader.ReadUInt64();
				return;
			}
			throw new InControlException("Unknown data format version: " + dataFormatVersion);
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x000A4210 File Offset: 0x000A2410
		internal void Save(BinaryWriter writer)
		{
			writer.Write(this.includeSize);
			writer.Write(this.includeData);
			writer.Write(this.excludeSize);
			writer.Write(this.excludeData);
		}

		// Token: 0x04001A71 RID: 6769
		private int includeSize;

		// Token: 0x04001A72 RID: 6770
		private ulong includeData;

		// Token: 0x04001A73 RID: 6771
		private int excludeSize;

		// Token: 0x04001A74 RID: 6772
		private ulong excludeData;

		// Token: 0x04001A75 RID: 6773
		private static Dictionary<ulong, string> cachedStrings = new Dictionary<ulong, string>();
	}
}
