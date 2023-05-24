using System;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000BFC RID: 3068
	[Serializable]
	public class LayerInfo
	{
		// Token: 0x0600414F RID: 16719 RVA: 0x001515B4 File Offset: 0x0014F7B4
		public LayerInfo()
		{
			this.unityLayer = 0;
			this.useColor = true;
			this.generateCollider = true;
			this.skipMeshGeneration = false;
		}

		// Token: 0x040033E4 RID: 13284
		public string name;

		// Token: 0x040033E5 RID: 13285
		public int hash;

		// Token: 0x040033E6 RID: 13286
		public bool useColor;

		// Token: 0x040033E7 RID: 13287
		public bool generateCollider;

		// Token: 0x040033E8 RID: 13288
		public float z = 0.1f;

		// Token: 0x040033E9 RID: 13289
		public int unityLayer;

		// Token: 0x040033EA RID: 13290
		public int renderQueueOffset;

		// Token: 0x040033EB RID: 13291
		public string sortingLayerName = string.Empty;

		// Token: 0x040033EC RID: 13292
		public int sortingOrder;

		// Token: 0x040033ED RID: 13293
		[NonSerialized]
		public bool ForceNonAnimating;

		// Token: 0x040033EE RID: 13294
		public bool overrideChunkable;

		// Token: 0x040033EF RID: 13295
		public int overrideChunkXOffset;

		// Token: 0x040033F0 RID: 13296
		public int overrideChunkYOffset;

		// Token: 0x040033F1 RID: 13297
		[NonSerialized]
		public bool[] preprocessedFlags;

		// Token: 0x040033F2 RID: 13298
		public bool skipMeshGeneration;

		// Token: 0x040033F3 RID: 13299
		public PhysicMaterial physicMaterial;

		// Token: 0x040033F4 RID: 13300
		public PhysicsMaterial2D physicsMaterial2D;
	}
}
