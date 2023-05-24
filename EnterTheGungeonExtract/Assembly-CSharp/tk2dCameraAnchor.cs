using System;
using UnityEngine;

// Token: 0x02000B85 RID: 2949
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Camera/tk2dCameraAnchor")]
public class tk2dCameraAnchor : MonoBehaviour
{
	// Token: 0x17000944 RID: 2372
	// (get) Token: 0x06003DBC RID: 15804 RVA: 0x00135810 File Offset: 0x00133A10
	// (set) Token: 0x06003DBD RID: 15805 RVA: 0x00135898 File Offset: 0x00133A98
	public tk2dBaseSprite.Anchor AnchorPoint
	{
		get
		{
			if (this.anchor != -1)
			{
				if (this.anchor >= 0 && this.anchor <= 2)
				{
					this._anchorPoint = this.anchor + tk2dBaseSprite.Anchor.UpperLeft;
				}
				else if (this.anchor >= 6 && this.anchor <= 8)
				{
					this._anchorPoint = (tk2dBaseSprite.Anchor)(this.anchor - 6);
				}
				else
				{
					this._anchorPoint = (tk2dBaseSprite.Anchor)this.anchor;
				}
				this.anchor = -1;
			}
			return this._anchorPoint;
		}
		set
		{
			this._anchorPoint = value;
		}
	}

	// Token: 0x17000945 RID: 2373
	// (get) Token: 0x06003DBE RID: 15806 RVA: 0x001358A4 File Offset: 0x00133AA4
	// (set) Token: 0x06003DBF RID: 15807 RVA: 0x001358AC File Offset: 0x00133AAC
	public Vector2 AnchorOffsetPixels
	{
		get
		{
			return this.offset;
		}
		set
		{
			this.offset = value;
		}
	}

	// Token: 0x17000946 RID: 2374
	// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x001358B8 File Offset: 0x00133AB8
	// (set) Token: 0x06003DC1 RID: 15809 RVA: 0x001358C0 File Offset: 0x00133AC0
	public bool AnchorToNativeBounds
	{
		get
		{
			return this.anchorToNativeBounds;
		}
		set
		{
			this.anchorToNativeBounds = value;
		}
	}

	// Token: 0x17000947 RID: 2375
	// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x001358CC File Offset: 0x00133ACC
	// (set) Token: 0x06003DC3 RID: 15811 RVA: 0x00135900 File Offset: 0x00133B00
	public Camera AnchorCamera
	{
		get
		{
			if (this.tk2dCamera != null)
			{
				this._anchorCamera = this.tk2dCamera.GetComponent<Camera>();
				this.tk2dCamera = null;
			}
			return this._anchorCamera;
		}
		set
		{
			this._anchorCamera = value;
			this._anchorCameraCached = null;
		}
	}

	// Token: 0x17000948 RID: 2376
	// (get) Token: 0x06003DC4 RID: 15812 RVA: 0x00135910 File Offset: 0x00133B10
	private tk2dCamera AnchorTk2dCamera
	{
		get
		{
			if (this._anchorCameraCached != this._anchorCamera)
			{
				this._anchorTk2dCamera = this._anchorCamera.GetComponent<tk2dCamera>();
				this._anchorCameraCached = this._anchorCamera;
			}
			return this._anchorTk2dCamera;
		}
	}

	// Token: 0x17000949 RID: 2377
	// (get) Token: 0x06003DC5 RID: 15813 RVA: 0x0013594C File Offset: 0x00133B4C
	private Transform myTransform
	{
		get
		{
			if (this._myTransform == null)
			{
				this._myTransform = base.transform;
			}
			return this._myTransform;
		}
	}

	// Token: 0x06003DC6 RID: 15814 RVA: 0x00135974 File Offset: 0x00133B74
	private void Start()
	{
		this.UpdateTransform();
	}

	// Token: 0x06003DC7 RID: 15815 RVA: 0x0013597C File Offset: 0x00133B7C
	private void UpdateTransform()
	{
		if (this.AnchorCamera == null)
		{
			return;
		}
		float num = 1f;
		Vector3 localPosition = this.myTransform.localPosition;
		tk2dCamera tk2dCamera = ((!(this.AnchorTk2dCamera != null) || this.AnchorTk2dCamera.CameraSettings.projection == tk2dCameraSettings.ProjectionType.Perspective) ? null : this.AnchorTk2dCamera);
		Rect rect = default(Rect);
		if (tk2dCamera != null)
		{
			rect = ((!this.anchorToNativeBounds) ? tk2dCamera.ScreenExtents : tk2dCamera.NativeScreenExtents);
			num = tk2dCamera.GetSizeAtDistance(1f);
		}
		else
		{
			rect.Set(0f, 0f, (float)this.AnchorCamera.pixelWidth, (float)this.AnchorCamera.pixelHeight);
		}
		float yMin = rect.yMin;
		float yMax = rect.yMax;
		float num2 = (yMin + yMax) * 0.5f;
		float xMin = rect.xMin;
		float xMax = rect.xMax;
		float num3 = (xMin + xMax) * 0.5f;
		Vector3 zero = Vector3.zero;
		switch (this.AnchorPoint)
		{
		case tk2dBaseSprite.Anchor.LowerLeft:
			zero = new Vector3(xMin, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.LowerCenter:
			zero = new Vector3(num3, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
			zero = new Vector3(xMax, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
			zero = new Vector3(xMin, num2, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleCenter:
			zero = new Vector3(num3, num2, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleRight:
			zero = new Vector3(xMax, num2, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
			zero = new Vector3(xMin, yMax, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperCenter:
			zero = new Vector3(num3, yMax, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperRight:
			zero = new Vector3(xMax, yMax, localPosition.z);
			break;
		}
		Vector3 vector = zero + new Vector3(num * this.offset.x, num * this.offset.y, 0f);
		if (tk2dCamera == null)
		{
			Vector3 vector2 = this.AnchorCamera.ScreenToWorldPoint(vector);
			if (this.myTransform.position != vector2)
			{
				this.myTransform.position = vector2;
			}
		}
		else
		{
			Vector3 localPosition2 = this.myTransform.localPosition;
			if (localPosition2 != vector)
			{
				this.myTransform.localPosition = vector;
			}
		}
	}

	// Token: 0x06003DC8 RID: 15816 RVA: 0x00135C34 File Offset: 0x00133E34
	public void ForceUpdateTransform()
	{
		this.UpdateTransform();
	}

	// Token: 0x06003DC9 RID: 15817 RVA: 0x00135C3C File Offset: 0x00133E3C
	private void LateUpdate()
	{
		this.UpdateTransform();
	}

	// Token: 0x04003015 RID: 12309
	[SerializeField]
	private int anchor = -1;

	// Token: 0x04003016 RID: 12310
	[SerializeField]
	private tk2dBaseSprite.Anchor _anchorPoint = tk2dBaseSprite.Anchor.UpperLeft;

	// Token: 0x04003017 RID: 12311
	[SerializeField]
	private bool anchorToNativeBounds;

	// Token: 0x04003018 RID: 12312
	[SerializeField]
	private Vector2 offset = Vector2.zero;

	// Token: 0x04003019 RID: 12313
	[SerializeField]
	private tk2dCamera tk2dCamera;

	// Token: 0x0400301A RID: 12314
	[SerializeField]
	private Camera _anchorCamera;

	// Token: 0x0400301B RID: 12315
	private Camera _anchorCameraCached;

	// Token: 0x0400301C RID: 12316
	private tk2dCamera _anchorTk2dCamera;

	// Token: 0x0400301D RID: 12317
	private Transform _myTransform;
}
