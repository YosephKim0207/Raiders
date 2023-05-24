using System;
using UnityEngine;

// Token: 0x02000B8A RID: 2954
[Serializable]
public class tk2dCameraResolutionOverride
{
	// Token: 0x06003DCC RID: 15820 RVA: 0x00135CE0 File Offset: 0x00133EE0
	public bool Match(int pixelWidth, int pixelHeight)
	{
		tk2dCameraResolutionOverride.MatchByType matchByType = this.matchBy;
		if (matchByType == tk2dCameraResolutionOverride.MatchByType.Wildcard)
		{
			return true;
		}
		if (matchByType == tk2dCameraResolutionOverride.MatchByType.Resolution)
		{
			return pixelWidth == this.width && pixelHeight == this.height;
		}
		if (matchByType != tk2dCameraResolutionOverride.MatchByType.AspectRatio)
		{
			return false;
		}
		float num = (float)pixelWidth * this.aspectRatioDenominator / this.aspectRatioNumerator;
		return Mathf.Approximately(num, (float)pixelHeight);
	}

	// Token: 0x06003DCD RID: 15821 RVA: 0x00135D48 File Offset: 0x00133F48
	public void Upgrade(int version)
	{
		if (version == 0)
		{
			this.matchBy = (((this.width != -1 || this.height != -1) && (this.width != 0 || this.height != 0)) ? tk2dCameraResolutionOverride.MatchByType.Resolution : tk2dCameraResolutionOverride.MatchByType.Wildcard);
		}
	}

	// Token: 0x1700094A RID: 2378
	// (get) Token: 0x06003DCE RID: 15822 RVA: 0x00135D98 File Offset: 0x00133F98
	public static tk2dCameraResolutionOverride DefaultOverride
	{
		get
		{
			return new tk2dCameraResolutionOverride
			{
				name = "Override",
				matchBy = tk2dCameraResolutionOverride.MatchByType.Wildcard,
				autoScaleMode = tk2dCameraResolutionOverride.AutoScaleMode.FitVisible,
				fitMode = tk2dCameraResolutionOverride.FitMode.Center
			};
		}
	}

	// Token: 0x0400302F RID: 12335
	public string name;

	// Token: 0x04003030 RID: 12336
	public tk2dCameraResolutionOverride.MatchByType matchBy;

	// Token: 0x04003031 RID: 12337
	public int width;

	// Token: 0x04003032 RID: 12338
	public int height;

	// Token: 0x04003033 RID: 12339
	public float aspectRatioNumerator = 4f;

	// Token: 0x04003034 RID: 12340
	public float aspectRatioDenominator = 3f;

	// Token: 0x04003035 RID: 12341
	public float scale = 1f;

	// Token: 0x04003036 RID: 12342
	public Vector2 offsetPixels = new Vector2(0f, 0f);

	// Token: 0x04003037 RID: 12343
	public tk2dCameraResolutionOverride.AutoScaleMode autoScaleMode;

	// Token: 0x04003038 RID: 12344
	public tk2dCameraResolutionOverride.FitMode fitMode;

	// Token: 0x02000B8B RID: 2955
	public enum MatchByType
	{
		// Token: 0x0400303A RID: 12346
		Resolution,
		// Token: 0x0400303B RID: 12347
		AspectRatio,
		// Token: 0x0400303C RID: 12348
		Wildcard
	}

	// Token: 0x02000B8C RID: 2956
	public enum AutoScaleMode
	{
		// Token: 0x0400303E RID: 12350
		None,
		// Token: 0x0400303F RID: 12351
		FitWidth,
		// Token: 0x04003040 RID: 12352
		FitHeight,
		// Token: 0x04003041 RID: 12353
		FitVisible,
		// Token: 0x04003042 RID: 12354
		StretchToFit,
		// Token: 0x04003043 RID: 12355
		ClosestMultipleOfTwo,
		// Token: 0x04003044 RID: 12356
		PixelPerfect,
		// Token: 0x04003045 RID: 12357
		Fill
	}

	// Token: 0x02000B8D RID: 2957
	public enum FitMode
	{
		// Token: 0x04003047 RID: 12359
		Constant,
		// Token: 0x04003048 RID: 12360
		Center
	}
}
