using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012C5 RID: 4805
[RequireComponent(typeof(WaterBase))]
[ExecuteInEditMode]
public class PlanarReflection : MonoBehaviour
{
	// Token: 0x06006B82 RID: 27522 RVA: 0x002A3D5C File Offset: 0x002A1F5C
	public void Start()
	{
		this.sharedMaterial = ((WaterBase)base.gameObject.GetComponent(typeof(WaterBase))).sharedMaterial;
	}

	// Token: 0x06006B83 RID: 27523 RVA: 0x002A3D84 File Offset: 0x002A1F84
	private Camera CreateReflectionCameraFor(Camera cam)
	{
		string text = base.gameObject.name + "Reflection" + cam.name;
		GameObject gameObject = GameObject.Find(text);
		if (!gameObject)
		{
			gameObject = new GameObject(text, new Type[] { typeof(Camera) });
		}
		if (!gameObject.GetComponent(typeof(Camera)))
		{
			gameObject.AddComponent(typeof(Camera));
		}
		Camera component = gameObject.GetComponent<Camera>();
		component.backgroundColor = this.clearColor;
		component.clearFlags = ((!this.reflectSkybox) ? CameraClearFlags.Color : CameraClearFlags.Skybox);
		this.SetStandardCameraParameter(component, this.reflectionMask);
		if (!component.targetTexture)
		{
			component.targetTexture = this.CreateTextureFor(cam);
		}
		return component;
	}

	// Token: 0x06006B84 RID: 27524 RVA: 0x002A3E5C File Offset: 0x002A205C
	private void SetStandardCameraParameter(Camera cam, LayerMask mask)
	{
		cam.cullingMask = mask & ~(1 << LayerMask.NameToLayer("Water"));
		cam.backgroundColor = Color.black;
		cam.enabled = false;
	}

	// Token: 0x06006B85 RID: 27525 RVA: 0x002A3E90 File Offset: 0x002A2090
	private RenderTexture CreateTextureFor(Camera cam)
	{
		return new RenderTexture(Mathf.FloorToInt((float)cam.pixelWidth * 0.5f), Mathf.FloorToInt((float)cam.pixelHeight * 0.5f), 24)
		{
			hideFlags = HideFlags.DontSave
		};
	}

	// Token: 0x06006B86 RID: 27526 RVA: 0x002A3ED4 File Offset: 0x002A20D4
	public void RenderHelpCameras(Camera currentCam)
	{
		if (this.helperCameras == null)
		{
			this.helperCameras = new Dictionary<Camera, bool>();
		}
		if (!this.helperCameras.ContainsKey(currentCam))
		{
			this.helperCameras.Add(currentCam, false);
		}
		if (this.helperCameras[currentCam])
		{
			return;
		}
		if (!this.reflectionCamera)
		{
			this.reflectionCamera = this.CreateReflectionCameraFor(currentCam);
		}
		this.RenderReflectionFor(currentCam, this.reflectionCamera);
		this.helperCameras[currentCam] = true;
	}

	// Token: 0x06006B87 RID: 27527 RVA: 0x002A3F60 File Offset: 0x002A2160
	public void LateUpdate()
	{
		if (this.helperCameras != null)
		{
			this.helperCameras.Clear();
		}
	}

	// Token: 0x06006B88 RID: 27528 RVA: 0x002A3F78 File Offset: 0x002A2178
	public void WaterTileBeingRendered(Transform tr, Camera currentCam)
	{
		this.RenderHelpCameras(currentCam);
		if (this.reflectionCamera && this.sharedMaterial)
		{
			this.sharedMaterial.SetTexture(this.reflectionSampler, this.reflectionCamera.targetTexture);
		}
	}

	// Token: 0x06006B89 RID: 27529 RVA: 0x002A3FC8 File Offset: 0x002A21C8
	public void OnEnable()
	{
		Shader.EnableKeyword("WATER_REFLECTIVE");
		Shader.DisableKeyword("WATER_SIMPLE");
	}

	// Token: 0x06006B8A RID: 27530 RVA: 0x002A3FE0 File Offset: 0x002A21E0
	public void OnDisable()
	{
		Shader.EnableKeyword("WATER_SIMPLE");
		Shader.DisableKeyword("WATER_REFLECTIVE");
	}

	// Token: 0x06006B8B RID: 27531 RVA: 0x002A3FF8 File Offset: 0x002A21F8
	private void RenderReflectionFor(Camera cam, Camera reflectCamera)
	{
		if (!reflectCamera)
		{
			return;
		}
		if (this.sharedMaterial && !this.sharedMaterial.HasProperty(this.reflectionSampler))
		{
			return;
		}
		reflectCamera.cullingMask = this.reflectionMask & ~(1 << LayerMask.NameToLayer("Water"));
		this.SaneCameraSettings(reflectCamera);
		reflectCamera.backgroundColor = this.clearColor;
		reflectCamera.clearFlags = ((!this.reflectSkybox) ? CameraClearFlags.Color : CameraClearFlags.Skybox);
		if (this.reflectSkybox && cam.gameObject.GetComponent(typeof(Skybox)))
		{
			Skybox skybox = (Skybox)reflectCamera.gameObject.GetComponent(typeof(Skybox));
			if (!skybox)
			{
				skybox = (Skybox)reflectCamera.gameObject.AddComponent(typeof(Skybox));
			}
			skybox.material = ((Skybox)cam.GetComponent(typeof(Skybox))).material;
		}
		GL.invertCulling = true;
		Transform transform = base.transform;
		Vector3 eulerAngles = cam.transform.eulerAngles;
		reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
		reflectCamera.transform.position = cam.transform.position;
		Vector3 position = transform.transform.position;
		position.y = transform.position.y;
		Vector3 up = transform.transform.up;
		float num = -Vector3.Dot(up, position) - this.clipPlaneOffset;
		Vector4 vector = new Vector4(up.x, up.y, up.z, num);
		Matrix4x4 matrix4x = Matrix4x4.zero;
		matrix4x = PlanarReflection.CalculateReflectionMatrix(matrix4x, vector);
		this.oldpos = cam.transform.position;
		Vector3 vector2 = matrix4x.MultiplyPoint(this.oldpos);
		reflectCamera.worldToCameraMatrix = cam.worldToCameraMatrix * matrix4x;
		Vector4 vector3 = this.CameraSpacePlane(reflectCamera, position, up, 1f);
		reflectCamera.projectionMatrix = cam.CalculateObliqueMatrix(vector3);
		reflectCamera.transform.position = vector2;
		Vector3 eulerAngles2 = cam.transform.eulerAngles;
		reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles2.x, eulerAngles2.y, eulerAngles2.z);
		reflectCamera.Render();
		GL.invertCulling = false;
	}

	// Token: 0x06006B8C RID: 27532 RVA: 0x002A4274 File Offset: 0x002A2474
	private void SaneCameraSettings(Camera helperCam)
	{
		helperCam.depthTextureMode = DepthTextureMode.None;
		helperCam.backgroundColor = Color.black;
		helperCam.clearFlags = CameraClearFlags.Color;
		helperCam.renderingPath = RenderingPath.Forward;
	}

	// Token: 0x06006B8D RID: 27533 RVA: 0x002A4298 File Offset: 0x002A2498
	private static Matrix4x4 CalculateReflectionMatrix(Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
		reflectionMat.m01 = -2f * plane[0] * plane[1];
		reflectionMat.m02 = -2f * plane[0] * plane[2];
		reflectionMat.m03 = -2f * plane[3] * plane[0];
		reflectionMat.m10 = -2f * plane[1] * plane[0];
		reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
		reflectionMat.m12 = -2f * plane[1] * plane[2];
		reflectionMat.m13 = -2f * plane[3] * plane[1];
		reflectionMat.m20 = -2f * plane[2] * plane[0];
		reflectionMat.m21 = -2f * plane[2] * plane[1];
		reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
		reflectionMat.m23 = -2f * plane[3] * plane[2];
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;
		return reflectionMat;
	}

	// Token: 0x06006B8E RID: 27534 RVA: 0x002A4450 File Offset: 0x002A2650
	private static float sgn(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	// Token: 0x06006B8F RID: 27535 RVA: 0x002A447C File Offset: 0x002A267C
	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 vector = pos + normal * this.clipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 vector2 = worldToCameraMatrix.MultiplyPoint(vector);
		Vector3 vector3 = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(vector3.x, vector3.y, vector3.z, -Vector3.Dot(vector2, vector3));
	}

	// Token: 0x04006879 RID: 26745
	public LayerMask reflectionMask;

	// Token: 0x0400687A RID: 26746
	public bool reflectSkybox;

	// Token: 0x0400687B RID: 26747
	public Color clearColor = Color.grey;

	// Token: 0x0400687C RID: 26748
	public string reflectionSampler = "_ReflectionTex";

	// Token: 0x0400687D RID: 26749
	public float clipPlaneOffset = 0.07f;

	// Token: 0x0400687E RID: 26750
	private Vector3 oldpos = Vector3.zero;

	// Token: 0x0400687F RID: 26751
	private Camera reflectionCamera;

	// Token: 0x04006880 RID: 26752
	private Material sharedMaterial;

	// Token: 0x04006881 RID: 26753
	private Dictionary<Camera, bool> helperCameras;
}
