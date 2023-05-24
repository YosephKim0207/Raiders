using System;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EBD RID: 3773
	[Serializable]
	public class GenericCurrencyDropSettings
	{
		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06004FB2 RID: 20402 RVA: 0x001BAF8C File Offset: 0x001B918C
		public GameObject bronzeCoinPrefab
		{
			get
			{
				return PickupObjectDatabase.GetById(this.bronzeCoinId).gameObject;
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06004FB3 RID: 20403 RVA: 0x001BAFA0 File Offset: 0x001B91A0
		public GameObject silverCoinPrefab
		{
			get
			{
				return PickupObjectDatabase.GetById(this.silverCoinId).gameObject;
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06004FB4 RID: 20404 RVA: 0x001BAFB4 File Offset: 0x001B91B4
		public GameObject goldCoinPrefab
		{
			get
			{
				return PickupObjectDatabase.GetById(this.goldCoinId).gameObject;
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06004FB5 RID: 20405 RVA: 0x001BAFC8 File Offset: 0x001B91C8
		public GameObject metaCoinPrefab
		{
			get
			{
				return PickupObjectDatabase.GetById(this.metaCoinId).gameObject;
			}
		}

		// Token: 0x040047D7 RID: 18391
		[PickupIdentifier]
		public int bronzeCoinId = -1;

		// Token: 0x040047D8 RID: 18392
		[PickupIdentifier]
		public int silverCoinId = -1;

		// Token: 0x040047D9 RID: 18393
		[PickupIdentifier]
		public int goldCoinId = -1;

		// Token: 0x040047DA RID: 18394
		[PickupIdentifier]
		public int metaCoinId = -1;

		// Token: 0x040047DB RID: 18395
		public WeightedIntCollection blackPhantomCoinDropChances;
	}
}
