using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000812 RID: 2066
	public struct VersionInfo : IComparable<VersionInfo>
	{
		// Token: 0x06002BF0 RID: 11248 RVA: 0x000DEA38 File Offset: 0x000DCC38
		public VersionInfo(int major, int minor, int patch, int build)
		{
			this.Major = major;
			this.Minor = minor;
			this.Patch = patch;
			this.Build = build;
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x000DEA58 File Offset: 0x000DCC58
		public static VersionInfo InControlVersion()
		{
			return new VersionInfo
			{
				Major = 1,
				Minor = 6,
				Patch = 17,
				Build = 9143
			};
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x000DEA94 File Offset: 0x000DCC94
		public static VersionInfo UnityVersion()
		{
			Match match = Regex.Match(Application.unityVersion, "^(\\d+)\\.(\\d+)\\.(\\d+)");
			int num = 0;
			return new VersionInfo
			{
				Major = Convert.ToInt32(match.Groups[1].Value),
				Minor = Convert.ToInt32(match.Groups[2].Value),
				Patch = Convert.ToInt32(match.Groups[3].Value),
				Build = num
			};
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002BF3 RID: 11251 RVA: 0x000DEB1C File Offset: 0x000DCD1C
		public static VersionInfo Min
		{
			get
			{
				return new VersionInfo(int.MinValue, int.MinValue, int.MinValue, int.MinValue);
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06002BF4 RID: 11252 RVA: 0x000DEB38 File Offset: 0x000DCD38
		public static VersionInfo Max
		{
			get
			{
				return new VersionInfo(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
			}
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x000DEB54 File Offset: 0x000DCD54
		public int CompareTo(VersionInfo other)
		{
			if (this.Major < other.Major)
			{
				return -1;
			}
			if (this.Major > other.Major)
			{
				return 1;
			}
			if (this.Minor < other.Minor)
			{
				return -1;
			}
			if (this.Minor > other.Minor)
			{
				return 1;
			}
			if (this.Patch < other.Patch)
			{
				return -1;
			}
			if (this.Patch > other.Patch)
			{
				return 1;
			}
			if (this.Build < other.Build)
			{
				return -1;
			}
			if (this.Build > other.Build)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x000DEC04 File Offset: 0x000DCE04
		public static bool operator ==(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) == 0;
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x000DEC14 File Offset: 0x000DCE14
		public static bool operator !=(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) != 0;
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x000DEC24 File Offset: 0x000DCE24
		public static bool operator <=(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) <= 0;
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x000DEC34 File Offset: 0x000DCE34
		public static bool operator >=(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) >= 0;
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000DEC44 File Offset: 0x000DCE44
		public static bool operator <(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) < 0;
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x000DEC54 File Offset: 0x000DCE54
		public static bool operator >(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) > 0;
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x000DEC64 File Offset: 0x000DCE64
		public override bool Equals(object other)
		{
			return other is VersionInfo && this == (VersionInfo)other;
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000DEC84 File Offset: 0x000DCE84
		public override int GetHashCode()
		{
			return this.Major.GetHashCode() ^ this.Minor.GetHashCode() ^ this.Patch.GetHashCode() ^ this.Build.GetHashCode();
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000DECD8 File Offset: 0x000DCED8
		public override string ToString()
		{
			if (this.Build == 0)
			{
				return string.Format("{0}.{1}.{2}", this.Major, this.Minor, this.Patch);
			}
			return string.Format("{0}.{1}.{2} build {3}", new object[] { this.Major, this.Minor, this.Patch, this.Build });
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x000DED64 File Offset: 0x000DCF64
		public string ToShortString()
		{
			if (this.Build == 0)
			{
				return string.Format("{0}.{1}.{2}", this.Major, this.Minor, this.Patch);
			}
			return string.Format("{0}.{1}.{2}b{3}", new object[] { this.Major, this.Minor, this.Patch, this.Build });
		}

		// Token: 0x04001DEE RID: 7662
		public int Major;

		// Token: 0x04001DEF RID: 7663
		public int Minor;

		// Token: 0x04001DF0 RID: 7664
		public int Patch;

		// Token: 0x04001DF1 RID: 7665
		public int Build;
	}
}
