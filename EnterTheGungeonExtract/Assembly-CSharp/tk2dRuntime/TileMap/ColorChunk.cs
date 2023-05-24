using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000BF3 RID: 3059
	[Serializable]
	public class ColorChunk
	{
		// Token: 0x06004108 RID: 16648 RVA: 0x0014F018 File Offset: 0x0014D218
		public ColorChunk()
		{
			this.colors = new Color32[0];
			this.colorOverrides = new Color32[0, 0];
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06004109 RID: 16649 RVA: 0x0014F03C File Offset: 0x0014D23C
		// (set) Token: 0x0600410A RID: 16650 RVA: 0x0014F044 File Offset: 0x0014D244
		public bool Dirty { get; set; }

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x0600410B RID: 16651 RVA: 0x0014F050 File Offset: 0x0014D250
		public bool Empty
		{
			get
			{
				return this.colors.Length == 0;
			}
		}

		// Token: 0x040033CD RID: 13261
		public Color32[] colors;

		// Token: 0x040033CE RID: 13262
		public Color32[,] colorOverrides;
	}
}
