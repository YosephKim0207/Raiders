using System;

namespace Dungeonator
{
	// Token: 0x02000F64 RID: 3940
	[Serializable]
	public class RuntimeInjectionFlags
	{
		// Token: 0x060054E6 RID: 21734 RVA: 0x00202D28 File Offset: 0x00200F28
		public void Clear()
		{
			this.ShopAnnexed = false;
			this.CastleFireplace = false;
		}

		// Token: 0x060054E7 RID: 21735 RVA: 0x00202D38 File Offset: 0x00200F38
		public bool Merge(RuntimeInjectionFlags flags)
		{
			bool flag = false;
			if (!this.CastleFireplace && flags.CastleFireplace)
			{
				flag = true;
			}
			this.ShopAnnexed |= flags.ShopAnnexed;
			this.CastleFireplace |= flags.CastleFireplace;
			return flag;
		}

		// Token: 0x060054E8 RID: 21736 RVA: 0x00202D88 File Offset: 0x00200F88
		public bool IsValid(RuntimeInjectionFlags other)
		{
			bool flag = true;
			if (this.ShopAnnexed && other.ShopAnnexed)
			{
				flag = false;
			}
			if (this.CastleFireplace && other.CastleFireplace)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x04004DED RID: 19949
		public bool ShopAnnexed;

		// Token: 0x04004DEE RID: 19950
		public bool CastleFireplace;
	}
}
