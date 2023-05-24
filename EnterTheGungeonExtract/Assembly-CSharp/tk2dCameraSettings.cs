using System;
using UnityEngine;

// Token: 0x02000B86 RID: 2950
[Serializable]
public class tk2dCameraSettings
{
	// Token: 0x0400301E RID: 12318
	public tk2dCameraSettings.ProjectionType projection;

	// Token: 0x0400301F RID: 12319
	public float orthographicSize = 10f;

	// Token: 0x04003020 RID: 12320
	public float orthographicPixelsPerMeter = 100f;

	// Token: 0x04003021 RID: 12321
	public tk2dCameraSettings.OrthographicOrigin orthographicOrigin = tk2dCameraSettings.OrthographicOrigin.Center;

	// Token: 0x04003022 RID: 12322
	public tk2dCameraSettings.OrthographicType orthographicType;

	// Token: 0x04003023 RID: 12323
	public TransparencySortMode transparencySortMode;

	// Token: 0x04003024 RID: 12324
	public float fieldOfView = 60f;

	// Token: 0x04003025 RID: 12325
	public Rect rect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x02000B87 RID: 2951
	public enum ProjectionType
	{
		// Token: 0x04003027 RID: 12327
		Orthographic,
		// Token: 0x04003028 RID: 12328
		Perspective
	}

	// Token: 0x02000B88 RID: 2952
	public enum OrthographicType
	{
		// Token: 0x0400302A RID: 12330
		PixelsPerMeter,
		// Token: 0x0400302B RID: 12331
		OrthographicSize
	}

	// Token: 0x02000B89 RID: 2953
	public enum OrthographicOrigin
	{
		// Token: 0x0400302D RID: 12333
		BottomLeft,
		// Token: 0x0400302E RID: 12334
		Center
	}
}
