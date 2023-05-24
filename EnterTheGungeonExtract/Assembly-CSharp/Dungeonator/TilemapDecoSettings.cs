using System;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000EB9 RID: 3769
	[Serializable]
	public class TilemapDecoSettings
	{
		// Token: 0x06004FAE RID: 20398 RVA: 0x001BAF2C File Offset: 0x001B912C
		public Texture2D GetRandomLightCookie()
		{
			return this.lightCookies[UnityEngine.Random.Range(0, this.lightCookies.Length)];
		}

		// Token: 0x040047A6 RID: 18342
		public WeightedIntCollection standardRoomVisualSubtypes;

		// Token: 0x040047A7 RID: 18343
		public TilemapDecoSettings.DecoStyle decalLayerStyle;

		// Token: 0x040047A8 RID: 18344
		public int decalSize = 1;

		// Token: 0x040047A9 RID: 18345
		public int decalSpacing = 1;

		// Token: 0x040047AA RID: 18346
		public int decalExpansion;

		// Token: 0x040047AB RID: 18347
		public TilemapDecoSettings.DecoStyle patternLayerStyle;

		// Token: 0x040047AC RID: 18348
		public int patternSize = 1;

		// Token: 0x040047AD RID: 18349
		public int patternSpacing = 1;

		// Token: 0x040047AE RID: 18350
		public int patternExpansion;

		// Token: 0x040047AF RID: 18351
		public float decoPatchFrequency = 0.01f;

		// Token: 0x040047B0 RID: 18352
		[ColorUsage(true, true, 0f, 8f, 0.125f, 3f)]
		[Header("Lights")]
		public Color ambientLightColor = Color.black;

		// Token: 0x040047B1 RID: 18353
		[ColorUsage(true, true, 0f, 8f, 0.125f, 3f)]
		public Color ambientLightColorTwo = Color.clear;

		// Token: 0x040047B2 RID: 18354
		[ColorUsage(true, true, 0f, 8f, 0.125f, 3f)]
		public Color lowQualityAmbientLightColor = Color.white;

		// Token: 0x040047B3 RID: 18355
		[ColorUsage(true, true, 0f, 8f, 0.125f, 3f)]
		public Color lowQualityAmbientLightColorTwo = Color.white;

		// Token: 0x040047B4 RID: 18356
		public Vector4 lowQualityCheapLightVector = new Vector4(1f, 0f, -1f, 0f);

		// Token: 0x040047B5 RID: 18357
		public bool UsesAlienFXFloorColor;

		// Token: 0x040047B6 RID: 18358
		public Color AlienFXFloorColor = Color.black;

		// Token: 0x040047B7 RID: 18359
		public bool generateLights = true;

		// Token: 0x040047B8 RID: 18360
		public float lightCullingPercentage = 0.2f;

		// Token: 0x040047B9 RID: 18361
		public int lightOverlapRadius = 5;

		// Token: 0x040047BA RID: 18362
		public float nearestAllowedLight = 5f;

		// Token: 0x040047BB RID: 18363
		public int minLightExpanseWidth = 4;

		// Token: 0x040047BC RID: 18364
		public float lightHeight = 1.5f;

		// Token: 0x040047BD RID: 18365
		public Texture2D[] lightCookies;

		// Token: 0x040047BE RID: 18366
		public bool debug_view;

		// Token: 0x02000EBA RID: 3770
		public enum DecoStyle
		{
			// Token: 0x040047C0 RID: 18368
			GROW_FROM_WALLS,
			// Token: 0x040047C1 RID: 18369
			PERLIN_NOISE,
			// Token: 0x040047C2 RID: 18370
			HORIZONTAL_STRIPES,
			// Token: 0x040047C3 RID: 18371
			VERTICAL_STRIPES,
			// Token: 0x040047C4 RID: 18372
			AROUND_LIGHTS,
			// Token: 0x040047C5 RID: 18373
			WATER_CHANNELS,
			// Token: 0x040047C6 RID: 18374
			PATCHES,
			// Token: 0x040047C7 RID: 18375
			NONE = 99
		}
	}
}
