using System;
using System.Text;

// Token: 0x020018D0 RID: 6352
public class AkUtilities
{
	// Token: 0x020018D1 RID: 6353
	public class ShortIDGenerator
	{
		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x06009CD0 RID: 40144 RVA: 0x003ED844 File Offset: 0x003EBA44
		// (set) Token: 0x06009CD1 RID: 40145 RVA: 0x003ED84C File Offset: 0x003EBA4C
		public static byte HashSize
		{
			get
			{
				return AkUtilities.ShortIDGenerator.s_hashSize;
			}
			set
			{
				AkUtilities.ShortIDGenerator.s_hashSize = value;
				AkUtilities.ShortIDGenerator.s_mask = (1U << (int)AkUtilities.ShortIDGenerator.s_hashSize) - 1U;
			}
		} = 32;

		// Token: 0x06009CD2 RID: 40146 RVA: 0x003ED868 File Offset: 0x003EBA68
		public static uint Compute(string in_name)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(in_name.ToLower());
			uint num = 2166136261U;
			for (int i = 0; i < bytes.Length; i++)
			{
				num *= 16777619U;
				num ^= (uint)bytes[i];
			}
			if (AkUtilities.ShortIDGenerator.s_hashSize == 32)
			{
				return num;
			}
			return (num >> (int)AkUtilities.ShortIDGenerator.s_hashSize) ^ (num & AkUtilities.ShortIDGenerator.s_mask);
		}

		// Token: 0x04009E7B RID: 40571
		private const uint s_prime32 = 16777619U;

		// Token: 0x04009E7C RID: 40572
		private const uint s_offsetBasis32 = 2166136261U;

		// Token: 0x04009E7D RID: 40573
		private static byte s_hashSize;

		// Token: 0x04009E7E RID: 40574
		private static uint s_mask;
	}
}
