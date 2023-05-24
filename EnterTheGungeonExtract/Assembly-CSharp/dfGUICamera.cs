using System;
using UnityEngine;

// Token: 0x020003CB RID: 971
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Daikon Forge/User Interface/GUI Camera")]
[Serializable]
public class dfGUICamera : MonoBehaviour
{
	// Token: 0x060012B3 RID: 4787 RVA: 0x000555A8 File Offset: 0x000537A8
	public void Awake()
	{
		if (Application.isPlaying && this.MaintainCameraAspect)
		{
			if (this.ForceToSixteenNine)
			{
				BraveCameraUtility.MaintainCameraAspectForceAspect(base.GetComponent<Camera>(), 1.7777778f);
			}
			else
			{
				BraveCameraUtility.MaintainCameraAspect(base.GetComponent<Camera>());
			}
			base.transform.parent.GetComponent<dfGUIManager>().ResolutionChanged();
		}
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x0005560C File Offset: 0x0005380C
	public void OnEnable()
	{
	}

	// Token: 0x060012B5 RID: 4789 RVA: 0x00055610 File Offset: 0x00053810
	public void Start()
	{
		this.m_camera = base.GetComponent<Camera>();
		this.m_camera.transparencySortMode = TransparencySortMode.Orthographic;
		this.m_camera.useOcclusionCulling = false;
		this.m_camera.eventMask &= ~base.GetComponent<Camera>().cullingMask;
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x00055660 File Offset: 0x00053860
	private void Update()
	{
		if (Application.isPlaying && this.MaintainCameraAspect)
		{
			if (this.ForceToSixteenNine)
			{
				BraveCameraUtility.MaintainCameraAspectForceAspect(this.m_camera, 1.7777778f);
			}
			else
			{
				BraveCameraUtility.MaintainCameraAspect(this.m_camera);
			}
		}
	}

	// Token: 0x0400105C RID: 4188
	public Vector3 cameraPositionOffset;

	// Token: 0x0400105D RID: 4189
	public bool MaintainCameraAspect = true;

	// Token: 0x0400105E RID: 4190
	public bool ForceToSixteenNine;

	// Token: 0x0400105F RID: 4191
	public bool ForceNoHalfPixelOffset;

	// Token: 0x04001060 RID: 4192
	private Camera m_camera;
}
