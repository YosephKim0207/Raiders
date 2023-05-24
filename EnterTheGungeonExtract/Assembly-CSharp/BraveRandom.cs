using System;
using UnityEngine;

// Token: 0x02001820 RID: 6176
public static class BraveRandom
{
	// Token: 0x170015DB RID: 5595
	// (get) Token: 0x060091FE RID: 37374 RVA: 0x003DB9CC File Offset: 0x003D9BCC
	public static System.Random GeneratorRandom
	{
		get
		{
			return BraveRandom.m_generationRandom;
		}
	}

	// Token: 0x060091FF RID: 37375 RVA: 0x003DB9D4 File Offset: 0x003D9BD4
	public static bool IsInitialized()
	{
		return BraveRandom.m_generationRandom != null;
	}

	// Token: 0x06009200 RID: 37376 RVA: 0x003DB9E4 File Offset: 0x003D9BE4
	public static void InitializeRandom()
	{
		BraveRandom.m_generationRandom = new System.Random();
	}

	// Token: 0x06009201 RID: 37377 RVA: 0x003DB9F0 File Offset: 0x003D9BF0
	public static void InitializeWithSeed(int seed)
	{
		BraveRandom.m_generationRandom = new System.Random(seed);
	}

	// Token: 0x06009202 RID: 37378 RVA: 0x003DBA00 File Offset: 0x003D9C00
	public static float GenerationRandomValue()
	{
		return (float)BraveRandom.m_generationRandom.NextDouble();
	}

	// Token: 0x06009203 RID: 37379 RVA: 0x003DBA10 File Offset: 0x003D9C10
	public static float GenerationRandomRange(float min, float max)
	{
		return (max - min) * BraveRandom.GenerationRandomValue() + min;
	}

	// Token: 0x06009204 RID: 37380 RVA: 0x003DBA20 File Offset: 0x003D9C20
	public static int GenerationRandomRange(int min, int max)
	{
		return Mathf.FloorToInt((float)(max - min) * BraveRandom.GenerationRandomValue()) + min;
	}

	// Token: 0x040099E0 RID: 39392
	public static bool IgnoreGenerationDifferentiator;

	// Token: 0x040099E1 RID: 39393
	private static System.Random m_generationRandom;
}
