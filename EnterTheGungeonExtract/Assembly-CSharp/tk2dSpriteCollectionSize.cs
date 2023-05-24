using System;

// Token: 0x02000BDC RID: 3036
[Serializable]
public class tk2dSpriteCollectionSize
{
	// Token: 0x0600403A RID: 16442 RVA: 0x001463A4 File Offset: 0x001445A4
	public static tk2dSpriteCollectionSize Explicit(float orthoSize, float targetHeight)
	{
		return tk2dSpriteCollectionSize.ForResolution(orthoSize, targetHeight, targetHeight);
	}

	// Token: 0x0600403B RID: 16443 RVA: 0x001463B0 File Offset: 0x001445B0
	public static tk2dSpriteCollectionSize PixelsPerMeter(float pixelsPerMeter)
	{
		return new tk2dSpriteCollectionSize
		{
			type = tk2dSpriteCollectionSize.Type.PixelsPerMeter,
			pixelsPerMeter = pixelsPerMeter
		};
	}

	// Token: 0x0600403C RID: 16444 RVA: 0x001463D4 File Offset: 0x001445D4
	public static tk2dSpriteCollectionSize ForResolution(float orthoSize, float width, float height)
	{
		return new tk2dSpriteCollectionSize
		{
			type = tk2dSpriteCollectionSize.Type.Explicit,
			orthoSize = orthoSize,
			width = width,
			height = height
		};
	}

	// Token: 0x0600403D RID: 16445 RVA: 0x00146404 File Offset: 0x00144604
	public static tk2dSpriteCollectionSize ForTk2dCamera()
	{
		return new tk2dSpriteCollectionSize
		{
			type = tk2dSpriteCollectionSize.Type.PixelsPerMeter,
			pixelsPerMeter = 1f
		};
	}

	// Token: 0x0600403E RID: 16446 RVA: 0x0014642C File Offset: 0x0014462C
	public static tk2dSpriteCollectionSize ForTk2dCamera(tk2dCamera camera)
	{
		tk2dSpriteCollectionSize tk2dSpriteCollectionSize = new tk2dSpriteCollectionSize();
		tk2dCameraSettings cameraSettings = camera.SettingsRoot.CameraSettings;
		if (cameraSettings.projection == tk2dCameraSettings.ProjectionType.Orthographic)
		{
			tk2dCameraSettings.OrthographicType orthographicType = cameraSettings.orthographicType;
			if (orthographicType != tk2dCameraSettings.OrthographicType.PixelsPerMeter)
			{
				if (orthographicType == tk2dCameraSettings.OrthographicType.OrthographicSize)
				{
					tk2dSpriteCollectionSize.type = tk2dSpriteCollectionSize.Type.Explicit;
					tk2dSpriteCollectionSize.height = (float)camera.nativeResolutionHeight;
					tk2dSpriteCollectionSize.orthoSize = cameraSettings.orthographicSize;
				}
			}
			else
			{
				tk2dSpriteCollectionSize.type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;
				tk2dSpriteCollectionSize.pixelsPerMeter = cameraSettings.orthographicPixelsPerMeter;
			}
		}
		else if (cameraSettings.projection == tk2dCameraSettings.ProjectionType.Perspective)
		{
			tk2dSpriteCollectionSize.type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;
			tk2dSpriteCollectionSize.pixelsPerMeter = 100f;
		}
		return tk2dSpriteCollectionSize;
	}

	// Token: 0x0600403F RID: 16447 RVA: 0x001464D0 File Offset: 0x001446D0
	public static tk2dSpriteCollectionSize Default()
	{
		return tk2dSpriteCollectionSize.PixelsPerMeter(100f);
	}

	// Token: 0x06004040 RID: 16448 RVA: 0x001464DC File Offset: 0x001446DC
	public void CopyFromLegacy(bool useTk2dCamera, float orthoSize, float targetHeight)
	{
		if (useTk2dCamera)
		{
			this.type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;
			this.pixelsPerMeter = 1f;
		}
		else
		{
			this.type = tk2dSpriteCollectionSize.Type.Explicit;
			this.height = targetHeight;
			this.orthoSize = orthoSize;
		}
	}

	// Token: 0x06004041 RID: 16449 RVA: 0x00146510 File Offset: 0x00144710
	public void CopyFrom(tk2dSpriteCollectionSize source)
	{
		this.type = source.type;
		this.width = source.width;
		this.height = source.height;
		this.orthoSize = source.orthoSize;
		this.pixelsPerMeter = source.pixelsPerMeter;
	}

	// Token: 0x170009BE RID: 2494
	// (get) Token: 0x06004042 RID: 16450 RVA: 0x00146550 File Offset: 0x00144750
	public float OrthoSize
	{
		get
		{
			tk2dSpriteCollectionSize.Type type = this.type;
			if (type == tk2dSpriteCollectionSize.Type.Explicit)
			{
				return this.orthoSize;
			}
			if (type != tk2dSpriteCollectionSize.Type.PixelsPerMeter)
			{
				return this.orthoSize;
			}
			return 0.5f;
		}
	}

	// Token: 0x170009BF RID: 2495
	// (get) Token: 0x06004043 RID: 16451 RVA: 0x0014658C File Offset: 0x0014478C
	public float TargetHeight
	{
		get
		{
			tk2dSpriteCollectionSize.Type type = this.type;
			if (type == tk2dSpriteCollectionSize.Type.Explicit)
			{
				return this.height;
			}
			if (type != tk2dSpriteCollectionSize.Type.PixelsPerMeter)
			{
				return this.height;
			}
			return this.pixelsPerMeter;
		}
	}

	// Token: 0x0400333A RID: 13114
	public tk2dSpriteCollectionSize.Type type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;

	// Token: 0x0400333B RID: 13115
	public float orthoSize = 10f;

	// Token: 0x0400333C RID: 13116
	public float pixelsPerMeter = 100f;

	// Token: 0x0400333D RID: 13117
	public float width = 960f;

	// Token: 0x0400333E RID: 13118
	public float height = 640f;

	// Token: 0x02000BDD RID: 3037
	public enum Type
	{
		// Token: 0x04003340 RID: 13120
		Explicit,
		// Token: 0x04003341 RID: 13121
		PixelsPerMeter
	}
}
