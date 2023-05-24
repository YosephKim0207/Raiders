using System;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000BF2 RID: 3058
	[Serializable]
	public class SpriteChannel
	{
		// Token: 0x06004107 RID: 16647 RVA: 0x0014F004 File Offset: 0x0014D204
		public SpriteChannel()
		{
			this.chunks = new SpriteChunk[0];
		}

		// Token: 0x040033CB RID: 13259
		public SpriteChunk[] chunks;
	}
}
