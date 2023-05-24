using System;
using UnityEngine;

// Token: 0x02000BAD RID: 2989
[AddComponentMenu("2D Toolkit/Deprecated/Extra/tk2dPixelPerfectHelper")]
public class tk2dPixelPerfectHelper : MonoBehaviour
{
	// Token: 0x1700098E RID: 2446
	// (get) Token: 0x06003F1D RID: 16157 RVA: 0x0013EB28 File Offset: 0x0013CD28
	public static tk2dPixelPerfectHelper inst
	{
		get
		{
			if (tk2dPixelPerfectHelper._inst == null)
			{
				tk2dPixelPerfectHelper._inst = UnityEngine.Object.FindObjectOfType(typeof(tk2dPixelPerfectHelper)) as tk2dPixelPerfectHelper;
				if (tk2dPixelPerfectHelper._inst == null)
				{
					return null;
				}
				tk2dPixelPerfectHelper.inst.Setup();
			}
			return tk2dPixelPerfectHelper._inst;
		}
	}

	// Token: 0x06003F1E RID: 16158 RVA: 0x0013EB80 File Offset: 0x0013CD80
	private void Awake()
	{
		this.Setup();
		tk2dPixelPerfectHelper._inst = this;
	}

	// Token: 0x06003F1F RID: 16159 RVA: 0x0013EB90 File Offset: 0x0013CD90
	public virtual void Setup()
	{
		float num = (float)this.collectionTargetHeight / this.targetResolutionHeight;
		if (base.GetComponent<Camera>() != null)
		{
			this.cam = base.GetComponent<Camera>();
		}
		if (this.cam == null)
		{
			this.cam = Camera.main;
		}
		if (this.cam.orthographic)
		{
			this.scaleK = num * this.cam.orthographicSize / this.collectionOrthoSize;
			this.scaleD = 0f;
		}
		else
		{
			float num2 = num * Mathf.Tan(0.017453292f * this.cam.fieldOfView * 0.5f) / this.collectionOrthoSize;
			this.scaleK = num2 * -this.cam.transform.position.z;
			this.scaleD = num2;
		}
	}

	// Token: 0x06003F20 RID: 16160 RVA: 0x0013EC70 File Offset: 0x0013CE70
	public static float CalculateScaleForPerspectiveCamera(float fov, float zdist)
	{
		return Mathf.Abs(Mathf.Tan(0.017453292f * fov * 0.5f) * zdist);
	}

	// Token: 0x1700098F RID: 2447
	// (get) Token: 0x06003F21 RID: 16161 RVA: 0x0013EC8C File Offset: 0x0013CE8C
	public bool CameraIsOrtho
	{
		get
		{
			return this.cam.orthographic;
		}
	}

	// Token: 0x04003168 RID: 12648
	private static tk2dPixelPerfectHelper _inst;

	// Token: 0x04003169 RID: 12649
	[NonSerialized]
	public Camera cam;

	// Token: 0x0400316A RID: 12650
	public int collectionTargetHeight = 640;

	// Token: 0x0400316B RID: 12651
	public float collectionOrthoSize = 1f;

	// Token: 0x0400316C RID: 12652
	public float targetResolutionHeight = 640f;

	// Token: 0x0400316D RID: 12653
	[NonSerialized]
	public float scaleD;

	// Token: 0x0400316E RID: 12654
	[NonSerialized]
	public float scaleK;
}
