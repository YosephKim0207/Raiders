using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B84 RID: 2948
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Camera/tk2dCamera")]
public class tk2dCamera : MonoBehaviour
{
	// Token: 0x17000933 RID: 2355
	// (get) Token: 0x06003D99 RID: 15769 RVA: 0x00134718 File Offset: 0x00132918
	public tk2dCameraSettings CameraSettings
	{
		get
		{
			return this.cameraSettings;
		}
	}

	// Token: 0x17000934 RID: 2356
	// (get) Token: 0x06003D9A RID: 15770 RVA: 0x00134720 File Offset: 0x00132920
	public tk2dCameraResolutionOverride CurrentResolutionOverride
	{
		get
		{
			tk2dCamera settingsRoot = this.SettingsRoot;
			Camera screenCamera = this.ScreenCamera;
			float num = (float)screenCamera.pixelWidth;
			float num2 = (float)screenCamera.pixelHeight;
			tk2dCameraResolutionOverride tk2dCameraResolutionOverride = null;
			if (tk2dCameraResolutionOverride == null || (tk2dCameraResolutionOverride != null && ((float)tk2dCameraResolutionOverride.width != num || (float)tk2dCameraResolutionOverride.height != num2)))
			{
				tk2dCameraResolutionOverride = null;
				if (settingsRoot.resolutionOverride != null)
				{
					foreach (tk2dCameraResolutionOverride tk2dCameraResolutionOverride2 in settingsRoot.resolutionOverride)
					{
						if (tk2dCameraResolutionOverride2.Match((int)num, (int)num2))
						{
							tk2dCameraResolutionOverride = tk2dCameraResolutionOverride2;
							break;
						}
					}
				}
			}
			return tk2dCameraResolutionOverride;
		}
	}

	// Token: 0x17000935 RID: 2357
	// (get) Token: 0x06003D9B RID: 15771 RVA: 0x001347CC File Offset: 0x001329CC
	// (set) Token: 0x06003D9C RID: 15772 RVA: 0x001347D4 File Offset: 0x001329D4
	public tk2dCamera InheritConfig
	{
		get
		{
			return this.inheritSettings;
		}
		set
		{
			if (this.inheritSettings != value)
			{
				this.inheritSettings = value;
				this._settingsRoot = null;
			}
		}
	}

	// Token: 0x17000936 RID: 2358
	// (get) Token: 0x06003D9D RID: 15773 RVA: 0x001347F8 File Offset: 0x001329F8
	private Camera UnityCamera
	{
		get
		{
			if (this._unityCamera == null)
			{
				this._unityCamera = base.GetComponent<Camera>();
				if (this._unityCamera == null)
				{
					Debug.LogError("A unity camera must be attached to the tk2dCamera script");
				}
			}
			return this._unityCamera;
		}
	}

	// Token: 0x17000937 RID: 2359
	// (get) Token: 0x06003D9E RID: 15774 RVA: 0x00134838 File Offset: 0x00132A38
	public static tk2dCamera Instance
	{
		get
		{
			return tk2dCamera.inst;
		}
	}

	// Token: 0x06003D9F RID: 15775 RVA: 0x00134840 File Offset: 0x00132A40
	public static tk2dCamera CameraForLayer(int layer)
	{
		int num = 1 << layer;
		int count = tk2dCamera.allCameras.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dCamera tk2dCamera = tk2dCamera.allCameras[i];
			if ((tk2dCamera.UnityCamera.cullingMask & num) == num)
			{
				return tk2dCamera;
			}
		}
		return null;
	}

	// Token: 0x17000938 RID: 2360
	// (get) Token: 0x06003DA0 RID: 15776 RVA: 0x00134894 File Offset: 0x00132A94
	public Rect ScreenExtents
	{
		get
		{
			return this._screenExtents;
		}
	}

	// Token: 0x17000939 RID: 2361
	// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x0013489C File Offset: 0x00132A9C
	public Rect NativeScreenExtents
	{
		get
		{
			return this._nativeScreenExtents;
		}
	}

	// Token: 0x1700093A RID: 2362
	// (get) Token: 0x06003DA2 RID: 15778 RVA: 0x001348A4 File Offset: 0x00132AA4
	public Vector2 TargetResolution
	{
		get
		{
			return this._targetResolution;
		}
	}

	// Token: 0x1700093B RID: 2363
	// (get) Token: 0x06003DA3 RID: 15779 RVA: 0x001348AC File Offset: 0x00132AAC
	public Vector2 NativeResolution
	{
		get
		{
			return new Vector2((float)this.nativeResolutionWidth, (float)this.nativeResolutionHeight);
		}
	}

	// Token: 0x1700093C RID: 2364
	// (get) Token: 0x06003DA4 RID: 15780 RVA: 0x001348C4 File Offset: 0x00132AC4
	[Obsolete]
	public Vector2 ScreenOffset
	{
		get
		{
			return new Vector2(this.ScreenExtents.xMin - this.NativeScreenExtents.xMin, this.ScreenExtents.yMin - this.NativeScreenExtents.yMin);
		}
	}

	// Token: 0x1700093D RID: 2365
	// (get) Token: 0x06003DA5 RID: 15781 RVA: 0x00134910 File Offset: 0x00132B10
	[Obsolete]
	public Vector2 resolution
	{
		get
		{
			return new Vector2(this.ScreenExtents.xMax, this.ScreenExtents.yMax);
		}
	}

	// Token: 0x1700093E RID: 2366
	// (get) Token: 0x06003DA6 RID: 15782 RVA: 0x00134940 File Offset: 0x00132B40
	[Obsolete]
	public Vector2 ScreenResolution
	{
		get
		{
			return new Vector2(this.ScreenExtents.xMax, this.ScreenExtents.yMax);
		}
	}

	// Token: 0x1700093F RID: 2367
	// (get) Token: 0x06003DA7 RID: 15783 RVA: 0x00134970 File Offset: 0x00132B70
	[Obsolete]
	public Vector2 ScaledResolution
	{
		get
		{
			return new Vector2(this.ScreenExtents.width, this.ScreenExtents.height);
		}
	}

	// Token: 0x17000940 RID: 2368
	// (get) Token: 0x06003DA8 RID: 15784 RVA: 0x001349A0 File Offset: 0x00132BA0
	// (set) Token: 0x06003DA9 RID: 15785 RVA: 0x001349A8 File Offset: 0x00132BA8
	public float ZoomFactor
	{
		get
		{
			return this.zoomFactor;
		}
		set
		{
			this.zoomFactor = Mathf.Max(0.01f, value);
		}
	}

	// Token: 0x17000941 RID: 2369
	// (get) Token: 0x06003DAA RID: 15786 RVA: 0x001349BC File Offset: 0x00132BBC
	// (set) Token: 0x06003DAB RID: 15787 RVA: 0x001349D4 File Offset: 0x00132BD4
	[Obsolete]
	public float zoomScale
	{
		get
		{
			return 1f / Mathf.Max(0.001f, this.zoomFactor);
		}
		set
		{
			this.ZoomFactor = 1f / Mathf.Max(0.001f, value);
		}
	}

	// Token: 0x17000942 RID: 2370
	// (get) Token: 0x06003DAC RID: 15788 RVA: 0x001349F0 File Offset: 0x00132BF0
	public Camera ScreenCamera
	{
		get
		{
			bool flag = this.viewportClippingEnabled && this.inheritSettings != null && this.inheritSettings.UnityCamera.rect == this.unitRect;
			return (!flag) ? this.UnityCamera : this.inheritSettings.UnityCamera;
		}
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x00134A54 File Offset: 0x00132C54
	private void Awake()
	{
		this.Upgrade();
		if (tk2dCamera.allCameras.IndexOf(this) == -1)
		{
			tk2dCamera.allCameras.Add(this);
		}
		tk2dCamera settingsRoot = this.SettingsRoot;
		tk2dCameraSettings tk2dCameraSettings = settingsRoot.CameraSettings;
		if (tk2dCameraSettings.projection == tk2dCameraSettings.ProjectionType.Perspective)
		{
			this.UnityCamera.transparencySortMode = tk2dCameraSettings.transparencySortMode;
		}
	}

	// Token: 0x06003DAE RID: 15790 RVA: 0x00134AB0 File Offset: 0x00132CB0
	private void OnEnable()
	{
		if (this.UnityCamera != null)
		{
			this.UpdateCameraMatrix();
		}
		else
		{
			base.GetComponent<Camera>().enabled = false;
		}
		if (!this.viewportClippingEnabled)
		{
			tk2dCamera.inst = this;
		}
		if (tk2dCamera.allCameras.IndexOf(this) == -1)
		{
			tk2dCamera.allCameras.Add(this);
		}
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x00134B14 File Offset: 0x00132D14
	private void OnDestroy()
	{
		int num = tk2dCamera.allCameras.IndexOf(this);
		if (num != -1)
		{
			tk2dCamera.allCameras.RemoveAt(num);
		}
	}

	// Token: 0x06003DB0 RID: 15792 RVA: 0x00134B40 File Offset: 0x00132D40
	private void OnPreCull()
	{
		tk2dUpdateManager.FlushQueues();
		this.UpdateCameraMatrix();
	}

	// Token: 0x06003DB1 RID: 15793 RVA: 0x00134B50 File Offset: 0x00132D50
	public float GetSizeAtDistance(float distance)
	{
		tk2dCameraSettings tk2dCameraSettings = this.SettingsRoot.CameraSettings;
		tk2dCameraSettings.ProjectionType projection = tk2dCameraSettings.projection;
		if (projection != tk2dCameraSettings.ProjectionType.Orthographic)
		{
			if (projection != tk2dCameraSettings.ProjectionType.Perspective)
			{
				return 1f;
			}
			return Mathf.Tan(this.CameraSettings.fieldOfView * 0.017453292f * 0.5f) * distance * 2f / (float)this.SettingsRoot.nativeResolutionHeight;
		}
		else
		{
			if (tk2dCameraSettings.orthographicType == tk2dCameraSettings.OrthographicType.PixelsPerMeter)
			{
				return 1f / tk2dCameraSettings.orthographicPixelsPerMeter;
			}
			return 2f * tk2dCameraSettings.orthographicSize / (float)this.SettingsRoot.nativeResolutionHeight;
		}
	}

	// Token: 0x17000943 RID: 2371
	// (get) Token: 0x06003DB2 RID: 15794 RVA: 0x00134BEC File Offset: 0x00132DEC
	public tk2dCamera SettingsRoot
	{
		get
		{
			if (this._settingsRoot == null)
			{
				this._settingsRoot = ((!(this.inheritSettings == null) && !(this.inheritSettings == this)) ? this.inheritSettings.SettingsRoot : this);
			}
			return this._settingsRoot;
		}
	}

	// Token: 0x06003DB3 RID: 15795 RVA: 0x00134C4C File Offset: 0x00132E4C
	public Matrix4x4 OrthoOffCenter(Vector2 scale, float left, float right, float bottom, float top, float near, float far)
	{
		float num = 2f / (right - left) * scale.x;
		float num2 = 2f / (top - bottom) * scale.y;
		float num3 = -2f / (far - near);
		float num4 = -(right + left) / (right - left);
		float num5 = -(bottom + top) / (top - bottom);
		float num6 = -(far + near) / (far - near);
		Matrix4x4 matrix4x = default(Matrix4x4);
		matrix4x[0, 0] = num;
		matrix4x[0, 1] = 0f;
		matrix4x[0, 2] = 0f;
		matrix4x[0, 3] = num4;
		matrix4x[1, 0] = 0f;
		matrix4x[1, 1] = num2;
		matrix4x[1, 2] = 0f;
		matrix4x[1, 3] = num5;
		matrix4x[2, 0] = 0f;
		matrix4x[2, 1] = 0f;
		matrix4x[2, 2] = num3;
		matrix4x[2, 3] = num6;
		matrix4x[3, 0] = 0f;
		matrix4x[3, 1] = 0f;
		matrix4x[3, 2] = 0f;
		matrix4x[3, 3] = 1f;
		return matrix4x;
	}

	// Token: 0x06003DB4 RID: 15796 RVA: 0x00134D84 File Offset: 0x00132F84
	private Vector2 GetScaleForOverride(tk2dCamera settings, tk2dCameraResolutionOverride currentOverride, float width, float height)
	{
		Vector2 one = Vector2.one;
		if (currentOverride == null)
		{
			return one;
		}
		float num;
		switch (currentOverride.autoScaleMode)
		{
		case tk2dCameraResolutionOverride.AutoScaleMode.FitWidth:
			num = width / (float)settings.nativeResolutionWidth;
			one.Set(num, num);
			return one;
		case tk2dCameraResolutionOverride.AutoScaleMode.FitHeight:
			num = height / (float)settings.nativeResolutionHeight;
			one.Set(num, num);
			return one;
		case tk2dCameraResolutionOverride.AutoScaleMode.FitVisible:
		case tk2dCameraResolutionOverride.AutoScaleMode.ClosestMultipleOfTwo:
		{
			float num2 = (float)settings.nativeResolutionWidth / (float)settings.nativeResolutionHeight;
			float num3 = width / height;
			if (num3 < num2)
			{
				num = width / (float)settings.nativeResolutionWidth;
			}
			else
			{
				num = height / (float)settings.nativeResolutionHeight;
			}
			if (currentOverride.autoScaleMode == tk2dCameraResolutionOverride.AutoScaleMode.ClosestMultipleOfTwo)
			{
				if (num > 1f)
				{
					num = Mathf.Floor(num);
				}
				else
				{
					num = Mathf.Pow(2f, Mathf.Floor(Mathf.Log(num, 2f)));
				}
			}
			one.Set(num, num);
			return one;
		}
		case tk2dCameraResolutionOverride.AutoScaleMode.StretchToFit:
			one.Set(width / (float)settings.nativeResolutionWidth, height / (float)settings.nativeResolutionHeight);
			return one;
		case tk2dCameraResolutionOverride.AutoScaleMode.PixelPerfect:
			num = 1f;
			one.Set(num, num);
			return one;
		case tk2dCameraResolutionOverride.AutoScaleMode.Fill:
			num = Mathf.Max(width / (float)settings.nativeResolutionWidth, height / (float)settings.nativeResolutionHeight);
			one.Set(num, num);
			return one;
		}
		num = currentOverride.scale;
		one.Set(num, num);
		return one;
	}

	// Token: 0x06003DB5 RID: 15797 RVA: 0x00134EFC File Offset: 0x001330FC
	private Vector2 GetOffsetForOverride(tk2dCamera settings, tk2dCameraResolutionOverride currentOverride, Vector2 scale, float width, float height)
	{
		Vector2 vector = Vector2.zero;
		if (currentOverride == null)
		{
			return vector;
		}
		tk2dCameraResolutionOverride.FitMode fitMode = currentOverride.fitMode;
		if (fitMode != tk2dCameraResolutionOverride.FitMode.Center)
		{
			if (fitMode != tk2dCameraResolutionOverride.FitMode.Constant)
			{
			}
			vector = -currentOverride.offsetPixels;
		}
		else if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.BottomLeft)
		{
			vector = new Vector2(Mathf.Round(((float)settings.nativeResolutionWidth * scale.x - width) / 2f), Mathf.Round(((float)settings.nativeResolutionHeight * scale.y - height) / 2f));
		}
		return vector;
	}

	// Token: 0x06003DB6 RID: 15798 RVA: 0x00134F98 File Offset: 0x00133198
	private Matrix4x4 GetProjectionMatrixForOverride(tk2dCamera settings, tk2dCameraResolutionOverride currentOverride, float pixelWidth, float pixelHeight, bool halfTexelOffset, out Rect screenExtents, out Rect unscaledScreenExtents)
	{
		Vector2 scaleForOverride = this.GetScaleForOverride(settings, currentOverride, pixelWidth, pixelHeight);
		Vector2 offsetForOverride = this.GetOffsetForOverride(settings, currentOverride, scaleForOverride, pixelWidth, pixelHeight);
		float num = offsetForOverride.x;
		float num2 = offsetForOverride.y;
		float num3 = pixelWidth + offsetForOverride.x;
		float num4 = pixelHeight + offsetForOverride.y;
		Vector2 zero = Vector2.zero;
		if (this.viewportClippingEnabled && this.InheritConfig != null)
		{
			float num5 = (num3 - num) / scaleForOverride.x;
			float num6 = (num4 - num2) / scaleForOverride.y;
			Vector4 vector = new Vector4((float)((int)this.viewportRegion.x), (float)((int)this.viewportRegion.y), (float)((int)this.viewportRegion.z), (float)((int)this.viewportRegion.w));
			float num7 = -offsetForOverride.x / pixelWidth + vector.x / num5;
			float num8 = -offsetForOverride.y / pixelHeight + vector.y / num6;
			float num9 = vector.z / num5;
			float num10 = vector.w / num6;
			if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.Center)
			{
				num7 += (pixelWidth - (float)settings.nativeResolutionWidth * scaleForOverride.x) / pixelWidth / 2f;
				num8 += (pixelHeight - (float)settings.nativeResolutionHeight * scaleForOverride.y) / pixelHeight / 2f;
			}
			Rect rect = new Rect(num7, num8, num9, num10);
			if (this.UnityCamera.rect.x != num7 || this.UnityCamera.rect.y != num8 || this.UnityCamera.rect.width != num9 || this.UnityCamera.rect.height != num10)
			{
				this.UnityCamera.rect = rect;
			}
			float num11 = Mathf.Min(1f - rect.x, rect.width);
			float num12 = Mathf.Min(1f - rect.y, rect.height);
			float num13 = vector.x * scaleForOverride.x - offsetForOverride.x;
			float num14 = vector.y * scaleForOverride.y - offsetForOverride.y;
			if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.Center)
			{
				num13 -= (float)settings.nativeResolutionWidth * 0.5f * scaleForOverride.x;
				num14 -= (float)settings.nativeResolutionHeight * 0.5f * scaleForOverride.y;
			}
			if (rect.x < 0f)
			{
				num13 += -rect.x * pixelWidth;
				num11 = rect.x + rect.width;
			}
			if (rect.y < 0f)
			{
				num14 += -rect.y * pixelHeight;
				num12 = rect.y + rect.height;
			}
			num += num13;
			num2 += num14;
			num3 = pixelWidth * num11 + offsetForOverride.x + num13;
			num4 = pixelHeight * num12 + offsetForOverride.y + num14;
		}
		else
		{
			if (this.UnityCamera.rect != this.CameraSettings.rect)
			{
				this.UnityCamera.rect = this.CameraSettings.rect;
			}
			if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.Center)
			{
				float num15 = (num3 - num) * 0.5f;
				num -= num15;
				num3 -= num15;
				float num16 = (num4 - num2) * 0.5f;
				num4 -= num16;
				num2 -= num16;
				zero.Set((float)(-(float)this.nativeResolutionWidth) / 2f, (float)(-(float)this.nativeResolutionHeight) / 2f);
			}
		}
		float num17 = 1f / this.ZoomFactor;
		bool flag = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
		float num18 = ((!halfTexelOffset || !flag) ? 0f : 0.5f);
		float num19 = settings.cameraSettings.orthographicSize;
		tk2dCameraSettings.OrthographicType orthographicType = settings.cameraSettings.orthographicType;
		if (orthographicType != tk2dCameraSettings.OrthographicType.OrthographicSize)
		{
			if (orthographicType == tk2dCameraSettings.OrthographicType.PixelsPerMeter)
			{
				num19 = 1f / settings.cameraSettings.orthographicPixelsPerMeter;
			}
		}
		else
		{
			num19 = 2f * settings.cameraSettings.orthographicSize / (float)settings.nativeResolutionHeight;
		}
		float num20 = num19 * num17;
		screenExtents = new Rect(num * num20 / scaleForOverride.x, num2 * num20 / scaleForOverride.y, (num3 - num) * num20 / scaleForOverride.x, (num4 - num2) * num20 / scaleForOverride.y);
		unscaledScreenExtents = new Rect(zero.x * num20, zero.y * num20, (float)this.nativeResolutionWidth * num20, (float)this.nativeResolutionHeight * num20);
		return this.OrthoOffCenter(scaleForOverride, num19 * (num + num18) * num17, num19 * (num3 + num18) * num17, num19 * (num2 - num18) * num17, num19 * (num4 - num18) * num17, this.UnityCamera.nearClipPlane, this.UnityCamera.farClipPlane);
	}

	// Token: 0x06003DB7 RID: 15799 RVA: 0x001354C0 File Offset: 0x001336C0
	private Vector2 GetScreenPixelDimensions(tk2dCamera settings)
	{
		Vector2 vector = new Vector2((float)this.ScreenCamera.pixelWidth, (float)this.ScreenCamera.pixelHeight);
		return vector;
	}

	// Token: 0x06003DB8 RID: 15800 RVA: 0x001354F0 File Offset: 0x001336F0
	private void Upgrade()
	{
		if (this.version != tk2dCamera.CURRENT_VERSION)
		{
			if (this.version == 0)
			{
				this.cameraSettings.orthographicPixelsPerMeter = 1f;
				this.cameraSettings.orthographicType = tk2dCameraSettings.OrthographicType.PixelsPerMeter;
				this.cameraSettings.orthographicOrigin = tk2dCameraSettings.OrthographicOrigin.BottomLeft;
				this.cameraSettings.projection = tk2dCameraSettings.ProjectionType.Orthographic;
				foreach (tk2dCameraResolutionOverride tk2dCameraResolutionOverride in this.resolutionOverride)
				{
					tk2dCameraResolutionOverride.Upgrade(this.version);
				}
				Camera component = base.GetComponent<Camera>();
				if (component != null)
				{
					this.cameraSettings.rect = component.rect;
					if (!component.orthographic)
					{
						this.cameraSettings.projection = tk2dCameraSettings.ProjectionType.Perspective;
						this.cameraSettings.fieldOfView = component.fieldOfView * this.ZoomFactor;
					}
					component.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
				}
			}
			Debug.Log("tk2dCamera '" + base.name + "' - Upgraded from version " + this.version.ToString());
			this.version = tk2dCamera.CURRENT_VERSION;
		}
	}

	// Token: 0x06003DB9 RID: 15801 RVA: 0x00135608 File Offset: 0x00133808
	public void UpdateCameraMatrix()
	{
		this.Upgrade();
		if (!this.viewportClippingEnabled)
		{
			tk2dCamera.inst = this;
		}
		Camera unityCamera = this.UnityCamera;
		tk2dCamera settingsRoot = this.SettingsRoot;
		tk2dCameraSettings tk2dCameraSettings = settingsRoot.CameraSettings;
		if (unityCamera.rect != this.cameraSettings.rect)
		{
			unityCamera.rect = this.cameraSettings.rect;
		}
		this._targetResolution = this.GetScreenPixelDimensions(settingsRoot);
		if (tk2dCameraSettings.projection == tk2dCameraSettings.ProjectionType.Perspective)
		{
			if (unityCamera.orthographic)
			{
				unityCamera.orthographic = false;
			}
			float num = Mathf.Min(179.9f, tk2dCameraSettings.fieldOfView / Mathf.Max(0.001f, this.ZoomFactor));
			if (unityCamera.fieldOfView != num)
			{
				unityCamera.fieldOfView = num;
			}
			this._screenExtents.Set(-unityCamera.aspect, -1f, unityCamera.aspect * 2f, 2f);
			this._nativeScreenExtents = this._screenExtents;
			unityCamera.ResetProjectionMatrix();
		}
		else
		{
			if (!unityCamera.orthographic)
			{
				unityCamera.orthographic = true;
			}
			Matrix4x4 matrix4x = this.GetProjectionMatrixForOverride(settingsRoot, settingsRoot.CurrentResolutionOverride, this._targetResolution.x, this._targetResolution.y, true, out this._screenExtents, out this._nativeScreenExtents);
			if (Application.platform == RuntimePlatform.WP8Player && (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight))
			{
				float num2 = ((Screen.orientation != ScreenOrientation.LandscapeRight) ? (-90f) : 90f);
				Matrix4x4 matrix4x2 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, num2), Vector3.one);
				matrix4x = matrix4x2 * matrix4x;
			}
			if (unityCamera.projectionMatrix != matrix4x)
			{
				unityCamera.projectionMatrix = matrix4x;
			}
		}
	}

	// Token: 0x04003001 RID: 12289
	private static int CURRENT_VERSION = 1;

	// Token: 0x04003002 RID: 12290
	public int version;

	// Token: 0x04003003 RID: 12291
	[SerializeField]
	private tk2dCameraSettings cameraSettings = new tk2dCameraSettings();

	// Token: 0x04003004 RID: 12292
	public tk2dCameraResolutionOverride[] resolutionOverride = new tk2dCameraResolutionOverride[] { tk2dCameraResolutionOverride.DefaultOverride };

	// Token: 0x04003005 RID: 12293
	[SerializeField]
	private tk2dCamera inheritSettings;

	// Token: 0x04003006 RID: 12294
	public int nativeResolutionWidth = 960;

	// Token: 0x04003007 RID: 12295
	public int nativeResolutionHeight = 640;

	// Token: 0x04003008 RID: 12296
	[SerializeField]
	private Camera _unityCamera;

	// Token: 0x04003009 RID: 12297
	private static tk2dCamera inst;

	// Token: 0x0400300A RID: 12298
	private static List<tk2dCamera> allCameras = new List<tk2dCamera>();

	// Token: 0x0400300B RID: 12299
	public bool viewportClippingEnabled;

	// Token: 0x0400300C RID: 12300
	public Vector4 viewportRegion = new Vector4(0f, 0f, 100f, 100f);

	// Token: 0x0400300D RID: 12301
	private Vector2 _targetResolution = Vector2.zero;

	// Token: 0x0400300E RID: 12302
	[SerializeField]
	private float zoomFactor = 1f;

	// Token: 0x0400300F RID: 12303
	[HideInInspector]
	public bool forceResolutionInEditor;

	// Token: 0x04003010 RID: 12304
	[HideInInspector]
	public Vector2 forceResolution = new Vector2(960f, 640f);

	// Token: 0x04003011 RID: 12305
	private Rect _screenExtents;

	// Token: 0x04003012 RID: 12306
	private Rect _nativeScreenExtents;

	// Token: 0x04003013 RID: 12307
	private Rect unitRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x04003014 RID: 12308
	private tk2dCamera _settingsRoot;
}
