using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012C0 RID: 4800
[ExecuteInEditMode]
public class Water : MonoBehaviour
{
	// Token: 0x06006B6F RID: 27503 RVA: 0x002A2F6C File Offset: 0x002A116C
	public void OnWillRenderObject()
	{
		if (!base.enabled || !base.GetComponent<Renderer>() || !base.GetComponent<Renderer>().sharedMaterial || !base.GetComponent<Renderer>().enabled)
		{
			return;
		}
		Camera current = Camera.current;
		if (!current)
		{
			return;
		}
		if (Water.s_InsideWater)
		{
			return;
		}
		Water.s_InsideWater = true;
		this.m_HardwareWaterSupport = this.FindHardwareWaterSupport();
		Water.WaterMode waterMode = this.GetWaterMode();
		Camera camera;
		Camera camera2;
		this.CreateWaterObjects(current, out camera, out camera2);
		Vector3 position = base.transform.position;
		Vector3 up = base.transform.up;
		int pixelLightCount = QualitySettings.pixelLightCount;
		if (this.m_DisablePixelLights)
		{
			QualitySettings.pixelLightCount = 0;
		}
		this.UpdateCameraModes(current, camera);
		this.UpdateCameraModes(current, camera2);
		if (waterMode >= Water.WaterMode.Reflective)
		{
			float num = -Vector3.Dot(up, position) - this.m_ClipPlaneOffset;
			Vector4 vector = new Vector4(up.x, up.y, up.z, num);
			Matrix4x4 zero = Matrix4x4.zero;
			Water.CalculateReflectionMatrix(ref zero, vector);
			Vector3 position2 = current.transform.position;
			Vector3 vector2 = zero.MultiplyPoint(position2);
			camera.worldToCameraMatrix = current.worldToCameraMatrix * zero;
			Vector4 vector3 = this.CameraSpacePlane(camera, position, up, 1f);
			camera.projectionMatrix = current.CalculateObliqueMatrix(vector3);
			camera.cullingMask = -17 & this.m_ReflectLayers.value;
			camera.targetTexture = this.m_ReflectionTexture;
			GL.invertCulling = true;
			camera.transform.position = vector2;
			Vector3 eulerAngles = current.transform.eulerAngles;
			camera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
			camera.Render();
			camera.transform.position = position2;
			GL.invertCulling = false;
			base.GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex", this.m_ReflectionTexture);
		}
		if (waterMode >= Water.WaterMode.Refractive)
		{
			camera2.worldToCameraMatrix = current.worldToCameraMatrix;
			Vector4 vector4 = this.CameraSpacePlane(camera2, position, up, -1f);
			camera2.projectionMatrix = current.CalculateObliqueMatrix(vector4);
			camera2.cullingMask = -17 & this.m_RefractLayers.value;
			camera2.targetTexture = this.m_RefractionTexture;
			camera2.transform.position = current.transform.position;
			camera2.transform.rotation = current.transform.rotation;
			camera2.Render();
			base.GetComponent<Renderer>().sharedMaterial.SetTexture("_RefractionTex", this.m_RefractionTexture);
		}
		if (this.m_DisablePixelLights)
		{
			QualitySettings.pixelLightCount = pixelLightCount;
		}
		if (waterMode != Water.WaterMode.Simple)
		{
			if (waterMode != Water.WaterMode.Reflective)
			{
				if (waterMode == Water.WaterMode.Refractive)
				{
					Shader.DisableKeyword("WATER_SIMPLE");
					Shader.DisableKeyword("WATER_REFLECTIVE");
					Shader.EnableKeyword("WATER_REFRACTIVE");
				}
			}
			else
			{
				Shader.DisableKeyword("WATER_SIMPLE");
				Shader.EnableKeyword("WATER_REFLECTIVE");
				Shader.DisableKeyword("WATER_REFRACTIVE");
			}
		}
		else
		{
			Shader.EnableKeyword("WATER_SIMPLE");
			Shader.DisableKeyword("WATER_REFLECTIVE");
			Shader.DisableKeyword("WATER_REFRACTIVE");
		}
		Water.s_InsideWater = false;
	}

	// Token: 0x06006B70 RID: 27504 RVA: 0x002A32A4 File Offset: 0x002A14A4
	private void OnDisable()
	{
		if (this.m_ReflectionTexture)
		{
			UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
			this.m_ReflectionTexture = null;
		}
		if (this.m_RefractionTexture)
		{
			UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
			this.m_RefractionTexture = null;
		}
		foreach (KeyValuePair<Camera, Camera> keyValuePair in this.m_ReflectionCameras)
		{
			UnityEngine.Object.DestroyImmediate(keyValuePair.Value.gameObject);
		}
		this.m_ReflectionCameras.Clear();
		foreach (KeyValuePair<Camera, Camera> keyValuePair2 in this.m_RefractionCameras)
		{
			UnityEngine.Object.DestroyImmediate(keyValuePair2.Value.gameObject);
		}
		this.m_RefractionCameras.Clear();
	}

	// Token: 0x06006B71 RID: 27505 RVA: 0x002A33BC File Offset: 0x002A15BC
	private void Update()
	{
		if (!base.GetComponent<Renderer>())
		{
			return;
		}
		Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
		if (!sharedMaterial)
		{
			return;
		}
		Vector4 vector = sharedMaterial.GetVector("WaveSpeed");
		float @float = sharedMaterial.GetFloat("_WaveScale");
		Vector4 vector2 = new Vector4(@float, @float, @float * 0.4f, @float * 0.45f);
		double num = (double)Time.timeSinceLevelLoad / 20.0;
		Vector4 vector3 = new Vector4((float)Math.IEEERemainder((double)(vector.x * vector2.x) * num, 1.0), (float)Math.IEEERemainder((double)(vector.y * vector2.y) * num, 1.0), (float)Math.IEEERemainder((double)(vector.z * vector2.z) * num, 1.0), (float)Math.IEEERemainder((double)(vector.w * vector2.w) * num, 1.0));
		sharedMaterial.SetVector("_WaveOffset", vector3);
		sharedMaterial.SetVector("_WaveScale4", vector2);
		Vector3 size = base.GetComponent<Renderer>().bounds.size;
		Vector3 vector4 = new Vector3(size.x * vector2.x, size.z * vector2.y, 1f);
		Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(vector3.x, vector3.y, 0f), Quaternion.identity, vector4);
		sharedMaterial.SetMatrix("_WaveMatrix", matrix4x);
		vector4 = new Vector3(size.x * vector2.z, size.z * vector2.w, 1f);
		matrix4x = Matrix4x4.TRS(new Vector3(vector3.z, vector3.w, 0f), Quaternion.identity, vector4);
		sharedMaterial.SetMatrix("_WaveMatrix2", matrix4x);
	}

	// Token: 0x06006B72 RID: 27506 RVA: 0x002A35AC File Offset: 0x002A17AC
	private void UpdateCameraModes(Camera src, Camera dest)
	{
		if (dest == null)
		{
			return;
		}
		dest.clearFlags = src.clearFlags;
		dest.backgroundColor = src.backgroundColor;
		if (src.clearFlags == CameraClearFlags.Skybox)
		{
			Skybox skybox = src.GetComponent(typeof(Skybox)) as Skybox;
			Skybox skybox2 = dest.GetComponent(typeof(Skybox)) as Skybox;
			if (!skybox || !skybox.material)
			{
				skybox2.enabled = false;
			}
			else
			{
				skybox2.enabled = true;
				skybox2.material = skybox.material;
			}
		}
		dest.farClipPlane = src.farClipPlane;
		dest.nearClipPlane = src.nearClipPlane;
		dest.orthographic = src.orthographic;
		dest.fieldOfView = src.fieldOfView;
		dest.aspect = src.aspect;
		dest.orthographicSize = src.orthographicSize;
	}

	// Token: 0x06006B73 RID: 27507 RVA: 0x002A3698 File Offset: 0x002A1898
	private void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
	{
		Water.WaterMode waterMode = this.GetWaterMode();
		reflectionCamera = null;
		refractionCamera = null;
		if (waterMode >= Water.WaterMode.Reflective)
		{
			if (!this.m_ReflectionTexture || this.m_OldReflectionTextureSize != this.m_TextureSize)
			{
				if (this.m_ReflectionTexture)
				{
					UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
				}
				this.m_ReflectionTexture = new RenderTexture(this.m_TextureSize, this.m_TextureSize, 16);
				this.m_ReflectionTexture.name = "__WaterReflection" + base.GetInstanceID();
				this.m_ReflectionTexture.isPowerOfTwo = true;
				this.m_ReflectionTexture.hideFlags = HideFlags.DontSave;
				this.m_OldReflectionTextureSize = this.m_TextureSize;
			}
			this.m_ReflectionCameras.TryGetValue(currentCamera, out reflectionCamera);
			if (!reflectionCamera)
			{
				GameObject gameObject = new GameObject(string.Concat(new object[]
				{
					"Water Refl Camera id",
					base.GetInstanceID(),
					" for ",
					currentCamera.GetInstanceID()
				}), new Type[]
				{
					typeof(Camera),
					typeof(Skybox)
				});
				reflectionCamera = gameObject.GetComponent<Camera>();
				reflectionCamera.enabled = false;
				reflectionCamera.transform.position = base.transform.position;
				reflectionCamera.transform.rotation = base.transform.rotation;
				reflectionCamera.gameObject.AddComponent<FlareLayer>();
				gameObject.hideFlags = HideFlags.HideAndDontSave;
				this.m_ReflectionCameras[currentCamera] = reflectionCamera;
			}
		}
		if (waterMode >= Water.WaterMode.Refractive)
		{
			if (!this.m_RefractionTexture || this.m_OldRefractionTextureSize != this.m_TextureSize)
			{
				if (this.m_RefractionTexture)
				{
					UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
				}
				this.m_RefractionTexture = new RenderTexture(this.m_TextureSize, this.m_TextureSize, 16);
				this.m_RefractionTexture.name = "__WaterRefraction" + base.GetInstanceID();
				this.m_RefractionTexture.isPowerOfTwo = true;
				this.m_RefractionTexture.hideFlags = HideFlags.DontSave;
				this.m_OldRefractionTextureSize = this.m_TextureSize;
			}
			this.m_RefractionCameras.TryGetValue(currentCamera, out refractionCamera);
			if (!refractionCamera)
			{
				GameObject gameObject2 = new GameObject(string.Concat(new object[]
				{
					"Water Refr Camera id",
					base.GetInstanceID(),
					" for ",
					currentCamera.GetInstanceID()
				}), new Type[]
				{
					typeof(Camera),
					typeof(Skybox)
				});
				refractionCamera = gameObject2.GetComponent<Camera>();
				refractionCamera.enabled = false;
				refractionCamera.transform.position = base.transform.position;
				refractionCamera.transform.rotation = base.transform.rotation;
				refractionCamera.gameObject.AddComponent<FlareLayer>();
				gameObject2.hideFlags = HideFlags.HideAndDontSave;
				this.m_RefractionCameras[currentCamera] = refractionCamera;
			}
		}
	}

	// Token: 0x06006B74 RID: 27508 RVA: 0x002A39A4 File Offset: 0x002A1BA4
	private Water.WaterMode GetWaterMode()
	{
		if (this.m_HardwareWaterSupport < this.m_WaterMode)
		{
			return this.m_HardwareWaterSupport;
		}
		return this.m_WaterMode;
	}

	// Token: 0x06006B75 RID: 27509 RVA: 0x002A39C4 File Offset: 0x002A1BC4
	private Water.WaterMode FindHardwareWaterSupport()
	{
		if (!SystemInfo.supportsRenderTextures || !base.GetComponent<Renderer>())
		{
			return Water.WaterMode.Simple;
		}
		Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
		if (!sharedMaterial)
		{
			return Water.WaterMode.Simple;
		}
		string tag = sharedMaterial.GetTag("WATERMODE", false);
		if (tag == "Refractive")
		{
			return Water.WaterMode.Refractive;
		}
		if (tag == "Reflective")
		{
			return Water.WaterMode.Reflective;
		}
		return Water.WaterMode.Simple;
	}

	// Token: 0x06006B76 RID: 27510 RVA: 0x002A3A38 File Offset: 0x002A1C38
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

	// Token: 0x06006B77 RID: 27511 RVA: 0x002A3A64 File Offset: 0x002A1C64
	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 vector = pos + normal * this.m_ClipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 vector2 = worldToCameraMatrix.MultiplyPoint(vector);
		Vector3 vector3 = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(vector3.x, vector3.y, vector3.z, -Vector3.Dot(vector2, vector3));
	}

	// Token: 0x06006B78 RID: 27512 RVA: 0x002A3AD0 File Offset: 0x002A1CD0
	private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
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
	}

	// Token: 0x04006864 RID: 26724
	public Water.WaterMode m_WaterMode = Water.WaterMode.Refractive;

	// Token: 0x04006865 RID: 26725
	public bool m_DisablePixelLights = true;

	// Token: 0x04006866 RID: 26726
	public int m_TextureSize = 256;

	// Token: 0x04006867 RID: 26727
	public float m_ClipPlaneOffset = 0.07f;

	// Token: 0x04006868 RID: 26728
	public LayerMask m_ReflectLayers = -1;

	// Token: 0x04006869 RID: 26729
	public LayerMask m_RefractLayers = -1;

	// Token: 0x0400686A RID: 26730
	private Dictionary<Camera, Camera> m_ReflectionCameras = new Dictionary<Camera, Camera>();

	// Token: 0x0400686B RID: 26731
	private Dictionary<Camera, Camera> m_RefractionCameras = new Dictionary<Camera, Camera>();

	// Token: 0x0400686C RID: 26732
	private RenderTexture m_ReflectionTexture;

	// Token: 0x0400686D RID: 26733
	private RenderTexture m_RefractionTexture;

	// Token: 0x0400686E RID: 26734
	private Water.WaterMode m_HardwareWaterSupport = Water.WaterMode.Refractive;

	// Token: 0x0400686F RID: 26735
	private int m_OldReflectionTextureSize;

	// Token: 0x04006870 RID: 26736
	private int m_OldRefractionTextureSize;

	// Token: 0x04006871 RID: 26737
	private static bool s_InsideWater;

	// Token: 0x020012C1 RID: 4801
	public enum WaterMode
	{
		// Token: 0x04006873 RID: 26739
		Simple,
		// Token: 0x04006874 RID: 26740
		Reflective,
		// Token: 0x04006875 RID: 26741
		Refractive
	}
}
