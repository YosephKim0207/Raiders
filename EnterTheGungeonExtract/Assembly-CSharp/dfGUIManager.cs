using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020003CC RID: 972
[RequireComponent(typeof(BoxCollider))]
[ExecuteInEditMode]
[AddComponentMenu("Daikon Forge/User Interface/GUI Manager")]
[RequireComponent(typeof(dfInputManager))]
[Serializable]
public class dfGUIManager : MonoBehaviour, IDFControlHost, IComparable<dfGUIManager>
{
	// Token: 0x14000034 RID: 52
	// (add) Token: 0x060012B8 RID: 4792 RVA: 0x0005577C File Offset: 0x0005397C
	// (remove) Token: 0x060012B9 RID: 4793 RVA: 0x000557B0 File Offset: 0x000539B0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event dfGUIManager.RenderCallback BeforeRender;

	// Token: 0x14000035 RID: 53
	// (add) Token: 0x060012BA RID: 4794 RVA: 0x000557E4 File Offset: 0x000539E4
	// (remove) Token: 0x060012BB RID: 4795 RVA: 0x00055818 File Offset: 0x00053A18
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event dfGUIManager.RenderCallback AfterRender;

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x060012BC RID: 4796 RVA: 0x0005584C File Offset: 0x00053A4C
	public MeshRenderer MeshRenderer
	{
		get
		{
			return this.meshRenderer;
		}
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x060012BD RID: 4797 RVA: 0x00055854 File Offset: 0x00053A54
	public static IEnumerable<dfGUIManager> ActiveManagers
	{
		get
		{
			return dfGUIManager.activeInstances;
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x060012BE RID: 4798 RVA: 0x0005585C File Offset: 0x00053A5C
	// (set) Token: 0x060012BF RID: 4799 RVA: 0x00055864 File Offset: 0x00053A64
	public int TotalDrawCalls { get; private set; }

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x060012C0 RID: 4800 RVA: 0x00055870 File Offset: 0x00053A70
	// (set) Token: 0x060012C1 RID: 4801 RVA: 0x00055878 File Offset: 0x00053A78
	public int TotalTriangles { get; private set; }

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x060012C2 RID: 4802 RVA: 0x00055884 File Offset: 0x00053A84
	// (set) Token: 0x060012C3 RID: 4803 RVA: 0x0005588C File Offset: 0x00053A8C
	public int NumControlsRendered { get; private set; }

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x060012C4 RID: 4804 RVA: 0x00055898 File Offset: 0x00053A98
	// (set) Token: 0x060012C5 RID: 4805 RVA: 0x000558A0 File Offset: 0x00053AA0
	public int FramesRendered { get; private set; }

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x060012C6 RID: 4806 RVA: 0x000558AC File Offset: 0x00053AAC
	public IList<dfControl> ControlsRendered
	{
		get
		{
			return this.controlsRendered;
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x060012C7 RID: 4807 RVA: 0x000558B4 File Offset: 0x00053AB4
	public IList<int> DrawCallStartIndices
	{
		get
		{
			return this.drawCallIndices;
		}
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x060012C8 RID: 4808 RVA: 0x000558BC File Offset: 0x00053ABC
	// (set) Token: 0x060012C9 RID: 4809 RVA: 0x000558C4 File Offset: 0x00053AC4
	public int RenderQueueBase
	{
		get
		{
			return this.renderQueueBase;
		}
		set
		{
			if (value != this.renderQueueBase)
			{
				this.renderQueueBase = value;
				dfGUIManager.RefreshAll();
			}
		}
	}

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x060012CA RID: 4810 RVA: 0x000558E0 File Offset: 0x00053AE0
	public static dfControl ActiveControl
	{
		get
		{
			return dfGUIManager.activeControl;
		}
	}

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x060012CB RID: 4811 RVA: 0x000558E8 File Offset: 0x00053AE8
	// (set) Token: 0x060012CC RID: 4812 RVA: 0x000558F0 File Offset: 0x00053AF0
	public float UIScale
	{
		get
		{
			return this.uiScale;
		}
		set
		{
			if (!Mathf.Approximately(value, this.uiScale))
			{
				this.uiScale = value;
				this.onResolutionChanged();
			}
		}
	}

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x060012CD RID: 4813 RVA: 0x00055910 File Offset: 0x00053B10
	// (set) Token: 0x060012CE RID: 4814 RVA: 0x00055918 File Offset: 0x00053B18
	public bool UIScaleLegacyMode
	{
		get
		{
			return this.uiScaleLegacy;
		}
		set
		{
			if (value != this.uiScaleLegacy)
			{
				this.uiScaleLegacy = value;
				this.onResolutionChanged();
			}
		}
	}

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x060012CF RID: 4815 RVA: 0x00055934 File Offset: 0x00053B34
	// (set) Token: 0x060012D0 RID: 4816 RVA: 0x0005593C File Offset: 0x00053B3C
	public Vector2 UIOffset
	{
		get
		{
			return this.uiOffset;
		}
		set
		{
			if (!object.Equals(this.uiOffset, value))
			{
				this.uiOffset = value;
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x060012D1 RID: 4817 RVA: 0x00055968 File Offset: 0x00053B68
	// (set) Token: 0x060012D2 RID: 4818 RVA: 0x00055970 File Offset: 0x00053B70
	public Camera RenderCamera
	{
		get
		{
			return this.renderCamera;
		}
		set
		{
			if (!object.ReferenceEquals(this.renderCamera, value))
			{
				this.renderCamera = value;
				this.Invalidate();
				if (value != null && value.gameObject.GetComponent<dfGUICamera>() == null)
				{
					value.gameObject.AddComponent<dfGUICamera>();
				}
				if (this.inputManager != null)
				{
					this.inputManager.RenderCamera = value;
				}
			}
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x060012D3 RID: 4819 RVA: 0x000559E8 File Offset: 0x00053BE8
	// (set) Token: 0x060012D4 RID: 4820 RVA: 0x000559F0 File Offset: 0x00053BF0
	public bool MergeMaterials
	{
		get
		{
			return this.mergeMaterials;
		}
		set
		{
			if (value != this.mergeMaterials)
			{
				this.mergeMaterials = value;
				this.invalidateAllControls();
			}
		}
	}

	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x060012D5 RID: 4821 RVA: 0x00055A0C File Offset: 0x00053C0C
	// (set) Token: 0x060012D6 RID: 4822 RVA: 0x00055A14 File Offset: 0x00053C14
	public bool GenerateNormals
	{
		get
		{
			return this.generateNormals;
		}
		set
		{
			if (value != this.generateNormals)
			{
				this.generateNormals = value;
				if (this.renderMesh != null)
				{
					this.renderMesh[0].Clear();
					this.renderMesh[1].Clear();
				}
				dfRenderData.FlushObjectPool();
				this.invalidateAllControls();
			}
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x060012D7 RID: 4823 RVA: 0x00055A64 File Offset: 0x00053C64
	// (set) Token: 0x060012D8 RID: 4824 RVA: 0x00055A6C File Offset: 0x00053C6C
	public bool PixelPerfectMode
	{
		get
		{
			return this.pixelPerfectMode;
		}
		set
		{
			if (value != this.pixelPerfectMode)
			{
				this.pixelPerfectMode = value;
				this.onResolutionChanged();
				this.Invalidate();
			}
		}
	}

	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x060012D9 RID: 4825 RVA: 0x00055A90 File Offset: 0x00053C90
	// (set) Token: 0x060012DA RID: 4826 RVA: 0x00055A98 File Offset: 0x00053C98
	public dfAtlas DefaultAtlas
	{
		get
		{
			return this.atlas;
		}
		set
		{
			if (!dfAtlas.Equals(value, this.atlas))
			{
				this.atlas = value;
				this.invalidateAllControls();
			}
		}
	}

	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x060012DB RID: 4827 RVA: 0x00055AB8 File Offset: 0x00053CB8
	// (set) Token: 0x060012DC RID: 4828 RVA: 0x00055AC0 File Offset: 0x00053CC0
	public dfFontBase DefaultFont
	{
		get
		{
			return this.defaultFont;
		}
		set
		{
			if (value != this.defaultFont)
			{
				this.defaultFont = value;
				this.invalidateAllControls();
			}
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x060012DD RID: 4829 RVA: 0x00055AE0 File Offset: 0x00053CE0
	// (set) Token: 0x060012DE RID: 4830 RVA: 0x00055AE8 File Offset: 0x00053CE8
	public int FixedWidth
	{
		get
		{
			return this.fixedWidth;
		}
		set
		{
			if (value != this.fixedWidth)
			{
				this.fixedWidth = value;
				this.onResolutionChanged();
			}
		}
	}

	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x060012DF RID: 4831 RVA: 0x00055B04 File Offset: 0x00053D04
	// (set) Token: 0x060012E0 RID: 4832 RVA: 0x00055B0C File Offset: 0x00053D0C
	public int FixedHeight
	{
		get
		{
			return this.fixedHeight;
		}
		set
		{
			if (value != this.fixedHeight)
			{
				int num = this.fixedHeight;
				this.fixedHeight = value;
				this.onResolutionChanged(num, value);
			}
		}
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x060012E1 RID: 4833 RVA: 0x00055B3C File Offset: 0x00053D3C
	// (set) Token: 0x060012E2 RID: 4834 RVA: 0x00055B44 File Offset: 0x00053D44
	public bool ConsumeMouseEvents
	{
		get
		{
			return this.consumeMouseEvents;
		}
		set
		{
			this.consumeMouseEvents = value;
		}
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x060012E3 RID: 4835 RVA: 0x00055B50 File Offset: 0x00053D50
	// (set) Token: 0x060012E4 RID: 4836 RVA: 0x00055B58 File Offset: 0x00053D58
	public bool OverrideCamera
	{
		get
		{
			return this.overrideCamera;
		}
		set
		{
			this.overrideCamera = value;
		}
	}

	// Token: 0x060012E5 RID: 4837 RVA: 0x00055B64 File Offset: 0x00053D64
	public void OnApplicationQuit()
	{
		this.shutdownInProcess = true;
	}

	// Token: 0x060012E6 RID: 4838 RVA: 0x00055B70 File Offset: 0x00053D70
	public void Awake()
	{
		dfRenderData.FlushObjectPool();
	}

	// Token: 0x060012E7 RID: 4839 RVA: 0x00055B78 File Offset: 0x00053D78
	public void OnEnable()
	{
		Camera[] allCameras = Camera.allCameras;
		for (int i = 0; i < allCameras.Length; i++)
		{
			allCameras[i].eventMask &= ~(1 << base.gameObject.layer);
		}
		if (this.meshRenderer == null)
		{
			this.initialize();
		}
		base.useGUILayout = !this.ConsumeMouseEvents;
		dfGUIManager.activeInstances.Add(this);
		this.FramesRendered = 0;
		this.TotalDrawCalls = 0;
		this.TotalTriangles = 0;
		if (this.meshRenderer != null)
		{
			this.meshRenderer.enabled = true;
		}
		this.inputManager = base.GetComponent<dfInputManager>() ?? base.gameObject.AddComponent<dfInputManager>();
		this.inputManager.RenderCamera = this.RenderCamera;
		this.FramesRendered = 0;
		if (this.meshRenderer != null)
		{
			this.meshRenderer.enabled = true;
		}
		if (Application.isPlaying)
		{
			this.onResolutionChanged();
		}
		this.Invalidate();
	}

	// Token: 0x060012E8 RID: 4840 RVA: 0x00055C8C File Offset: 0x00053E8C
	public void OnDisable()
	{
		dfGUIManager.activeInstances.Remove(this);
		if (this.meshRenderer != null)
		{
			this.meshRenderer.enabled = false;
		}
		this.resetDrawCalls();
	}

	// Token: 0x060012E9 RID: 4841 RVA: 0x00055CC0 File Offset: 0x00053EC0
	public void OnDestroy()
	{
		if (dfGUIManager.activeInstances.Count == 0)
		{
			dfMaterialCache.Clear();
		}
		if (this.renderMesh == null || this.renderFilter == null)
		{
			return;
		}
		this.renderFilter.sharedMesh = null;
		UnityEngine.Object.DestroyImmediate(this.renderMesh[0]);
		UnityEngine.Object.DestroyImmediate(this.renderMesh[1]);
		this.renderMesh = null;
	}

	// Token: 0x060012EA RID: 4842 RVA: 0x00055D2C File Offset: 0x00053F2C
	public void Start()
	{
		Camera[] allCameras = Camera.allCameras;
		for (int i = 0; i < allCameras.Length; i++)
		{
			allCameras[i].eventMask &= ~(1 << base.gameObject.layer);
		}
	}

	// Token: 0x060012EB RID: 4843 RVA: 0x00055D74 File Offset: 0x00053F74
	public void Update()
	{
		dfGUIManager.activeInstances.Sort();
		if (this.renderCamera == null || !base.enabled)
		{
			if (this.meshRenderer != null)
			{
				this.meshRenderer.enabled = false;
			}
			return;
		}
		if (this.renderMesh == null || this.renderMesh.Length == 0)
		{
			this.initialize();
			if (Application.isEditor && !Application.isPlaying)
			{
				this.Render();
			}
		}
		if (this.cachedChildCount != base.transform.childCount)
		{
			this.cachedChildCount = base.transform.childCount;
			this.Invalidate();
		}
		Vector2 screenSize = this.GetScreenSize();
		if ((screenSize - this.cachedScreenSize).sqrMagnitude > 1E-45f)
		{
			this.onResolutionChanged(this.cachedScreenSize, screenSize);
			this.cachedScreenSize = screenSize;
		}
	}

	// Token: 0x060012EC RID: 4844 RVA: 0x00055E64 File Offset: 0x00054064
	public void LateUpdate()
	{
		if (this.renderMesh == null || this.renderMesh.Length == 0)
		{
			this.initialize();
		}
		if (!Application.isPlaying)
		{
			BoxCollider boxCollider = base.GetComponent<Collider>() as BoxCollider;
			if (boxCollider != null)
			{
				Vector2 vector = this.GetScreenSize() * this.PixelsToUnits();
				boxCollider.center = Vector3.zero;
				boxCollider.size = vector;
			}
		}
		if (dfGUIManager.activeInstances[0] == this)
		{
			dfFontManager.RebuildDynamicFonts();
			bool flag = false;
			for (int i = 0; i < dfGUIManager.activeInstances.Count; i++)
			{
				dfGUIManager dfGUIManager = dfGUIManager.activeInstances[i];
				if (dfGUIManager.isDirty && dfGUIManager.suspendCount <= 0)
				{
					flag = true;
					dfGUIManager.abortRendering = false;
					dfGUIManager.isDirty = false;
					dfGUIManager.Render();
				}
			}
			if (flag)
			{
				dfMaterialCache.Reset();
				this.updateDrawCalls();
				for (int j = 0; j < dfGUIManager.activeInstances.Count; j++)
				{
					dfGUIManager.activeInstances[j].updateDrawCalls();
				}
			}
		}
	}

	// Token: 0x060012ED RID: 4845 RVA: 0x00055F94 File Offset: 0x00054194
	public void SuspendRendering()
	{
		this.suspendCount++;
	}

	// Token: 0x060012EE RID: 4846 RVA: 0x00055FA4 File Offset: 0x000541A4
	public void ResumeRendering()
	{
		if (this.suspendCount == 0)
		{
			return;
		}
		if (--this.suspendCount == 0)
		{
			this.Invalidate();
		}
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x00055FDC File Offset: 0x000541DC
	public static dfControl HitTestAll(Vector2 screenPosition)
	{
		dfControl dfControl = null;
		float num = float.MinValue;
		for (int i = 0; i < dfGUIManager.activeInstances.Count; i++)
		{
			if (dfGUIManager.activeInstances[i].GetComponent<dfInputManager>().enabled)
			{
				dfGUIManager dfGUIManager = dfGUIManager.activeInstances[i];
				Camera camera = dfGUIManager.RenderCamera;
				if (camera.depth >= num)
				{
					dfControl dfControl2 = dfGUIManager.HitTest(screenPosition);
					if (dfControl2 != null)
					{
						dfControl = dfControl2;
						num = camera.depth;
					}
				}
			}
		}
		return dfControl;
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x00056074 File Offset: 0x00054274
	public dfControl HitTest(Vector2 screenPosition)
	{
		Vector2 vector = screenPosition;
		Ray ray = this.renderCamera.ScreenPointToRay(vector);
		float num = this.renderCamera.farClipPlane - this.renderCamera.nearClipPlane;
		dfControl modalControl = dfGUIManager.GetModalControl();
		dfList<dfControl> dfList = this.controlsRendered;
		int count = dfList.Count;
		dfControl[] items = dfList.Items;
		if (this.occluders.Count != count)
		{
			UnityEngine.Debug.LogWarning("Occluder count does not match control count");
			return null;
		}
		Vector2 vector2 = vector;
		vector2.y = (float)this.RenderCamera.pixelHeight / this.RenderCamera.rect.height - vector.y;
		for (int i = count - 1; i >= 0; i--)
		{
			dfControl dfControl = items[i];
			if (!(dfControl == null))
			{
				RaycastHit raycastHit;
				if (!(dfControl.GetComponent<Collider>() == null) && dfControl.GetComponent<Collider>().Raycast(ray, out raycastHit, num))
				{
					bool flag = (modalControl != null && !dfControl.transform.IsChildOf(modalControl.transform)) || !dfControl.IsInteractive || !dfControl.IsEnabled;
					if (!flag)
					{
						if (dfGUIManager.isInsideClippingRegion(raycastHit.point, dfControl))
						{
							return dfControl;
						}
					}
				}
			}
		}
		return null;
	}

	// Token: 0x060012F1 RID: 4849 RVA: 0x000561E0 File Offset: 0x000543E0
	public Vector2 WorldPointToGUI(Vector3 worldPoint)
	{
		Camera camera = Camera.main ?? this.renderCamera;
		return this.ScreenToGui(camera.WorldToScreenPoint(worldPoint));
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x00056214 File Offset: 0x00054414
	public float PixelsToUnits()
	{
		float num = 2f / (float)this.FixedHeight;
		return num * this.UIScale;
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x00056238 File Offset: 0x00054438
	public Plane[] GetClippingPlanes()
	{
		Vector3[] array = this.GetCorners();
		Vector3 vector = base.transform.TransformDirection(Vector3.right);
		Vector3 vector2 = base.transform.TransformDirection(Vector3.left);
		Vector3 vector3 = base.transform.TransformDirection(Vector3.up);
		Vector3 vector4 = base.transform.TransformDirection(Vector3.down);
		if (dfGUIManager.clippingPlanes == null)
		{
			dfGUIManager.clippingPlanes = new Plane[4];
		}
		dfGUIManager.clippingPlanes[0] = new Plane(vector, array[0]);
		dfGUIManager.clippingPlanes[1] = new Plane(vector2, array[1]);
		dfGUIManager.clippingPlanes[2] = new Plane(vector3, array[2]);
		dfGUIManager.clippingPlanes[3] = new Plane(vector4, array[0]);
		return dfGUIManager.clippingPlanes;
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x00056334 File Offset: 0x00054534
	public Vector3[] GetCorners()
	{
		float num = this.PixelsToUnits();
		Vector2 vector = this.GetScreenSize() * num;
		float x = vector.x;
		float y = vector.y;
		Vector3 vector2 = new Vector3(-x * 0.5f, y * 0.5f);
		Vector3 vector3 = vector2 + new Vector3(x, 0f);
		Vector3 vector4 = vector2 + new Vector3(0f, -y);
		Vector3 vector5 = vector3 + new Vector3(0f, -y);
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		this.corners[0] = localToWorldMatrix.MultiplyPoint(vector2);
		this.corners[1] = localToWorldMatrix.MultiplyPoint(vector3);
		this.corners[2] = localToWorldMatrix.MultiplyPoint(vector5);
		this.corners[3] = localToWorldMatrix.MultiplyPoint(vector4);
		return this.corners;
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x00056434 File Offset: 0x00054634
	public Vector2 GetScreenSize()
	{
		Camera camera = this.RenderCamera;
		bool flag = Application.isPlaying && camera != null;
		Vector2 vector = Vector2.zero;
		if (flag)
		{
			float num = ((!this.PixelPerfectMode) ? ((float)camera.pixelHeight / (float)this.fixedHeight) : 1f);
			if (this.guiCamera == null)
			{
				this.guiCamera = camera.GetComponent<dfGUICamera>();
			}
			vector = (new Vector2((float)camera.pixelWidth, (float)camera.pixelHeight) / num).CeilToInt();
			if (!this.guiCamera.MaintainCameraAspect)
			{
			}
			if (this.uiScaleLegacy)
			{
				vector *= this.uiScale;
			}
			else
			{
				vector /= this.uiScale;
			}
		}
		else
		{
			vector = new Vector2((float)this.FixedWidth, (float)this.FixedHeight);
			if (!this.uiScaleLegacy)
			{
				vector /= this.uiScale;
			}
		}
		return vector;
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x00056538 File Offset: 0x00054738
	public T AddControl<T>() where T : dfControl
	{
		return (T)((object)this.AddControl(typeof(T)));
	}

	// Token: 0x060012F7 RID: 4855 RVA: 0x00056550 File Offset: 0x00054750
	public dfControl AddControl(Type controlType)
	{
		if (!typeof(dfControl).IsAssignableFrom(controlType))
		{
			throw new InvalidCastException();
		}
		dfControl dfControl = new GameObject(controlType.Name)
		{
			transform = 
			{
				parent = base.transform
			},
			layer = base.gameObject.layer
		}.AddComponent(controlType) as dfControl;
		dfControl.ZOrder = this.getMaxZOrder() + 1;
		return dfControl;
	}

	// Token: 0x060012F8 RID: 4856 RVA: 0x000565C4 File Offset: 0x000547C4
	public void AddControl(dfControl child)
	{
		child.transform.parent = base.transform;
	}

	// Token: 0x060012F9 RID: 4857 RVA: 0x000565D8 File Offset: 0x000547D8
	public dfControl AddPrefab(GameObject prefab)
	{
		if (prefab.GetComponent<dfControl>() == null)
		{
			throw new InvalidCastException();
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		gameObject.transform.parent = base.transform;
		gameObject.layer = base.gameObject.layer;
		dfControl component = gameObject.GetComponent<dfControl>();
		component.transform.parent = base.transform;
		component.PerformLayout();
		this.BringToFront(component);
		return component;
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x0005664C File Offset: 0x0005484C
	public dfRenderData GetDrawCallBuffer(int drawCallNumber)
	{
		return this.drawCallBuffers[drawCallNumber];
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x0005665C File Offset: 0x0005485C
	public static dfControl GetModalControl()
	{
		return (dfGUIManager.modalControlStack.Count <= 0) ? null : dfGUIManager.modalControlStack.Peek().control;
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x00056694 File Offset: 0x00054894
	public Vector2 ScreenToGui(Vector2 position)
	{
		Vector2 screenSize = this.GetScreenSize();
		Camera camera = GameManager.Instance.MainCameraController.Camera ?? this.renderCamera;
		position.x = (float)(camera.pixelWidth / Screen.width) * position.x;
		position.y = (float)(camera.pixelHeight / Screen.height) * position.y;
		position.y = screenSize.y - position.y;
		return position;
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x00056714 File Offset: 0x00054914
	public static void PushModal(dfControl control)
	{
		dfGUIManager.PushModal(control, null);
	}

	// Token: 0x060012FE RID: 4862 RVA: 0x00056720 File Offset: 0x00054920
	public static void PushModal(dfControl control, dfGUIManager.ModalPoppedCallback callback)
	{
		if (control == null)
		{
			throw new NullReferenceException("Cannot call PushModal() with a null reference");
		}
		dfGUIManager.modalControlStack.Push(new dfGUIManager.ModalControlReference
		{
			control = control,
			callback = callback
		});
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x00056768 File Offset: 0x00054968
	public static void PopModal()
	{
		if (dfGUIManager.modalControlStack.Count == 0)
		{
			throw new InvalidOperationException("Modal stack is empty");
		}
		dfGUIManager.ModalControlReference modalControlReference = dfGUIManager.modalControlStack.Pop();
		if (modalControlReference.callback != null)
		{
			modalControlReference.callback(modalControlReference.control);
		}
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x000567BC File Offset: 0x000549BC
	public static bool ModalStackContainsControl(dfControl control)
	{
		dfGUIManager.ModalControlReference[] array = dfGUIManager.modalControlStack.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].control == control)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x00056804 File Offset: 0x00054A04
	public static void PopModalToControl(dfControl control, bool includeControl)
	{
		while (dfGUIManager.modalControlStack.Count > 0)
		{
			if (dfGUIManager.modalControlStack.Peek().control == control)
			{
				if (includeControl)
				{
					dfGUIManager.modalControlStack.Pop();
				}
				break;
			}
			dfGUIManager.modalControlStack.Pop();
		}
	}

	// Token: 0x06001302 RID: 4866 RVA: 0x00056864 File Offset: 0x00054A64
	public static void SetFocus(dfControl control, bool allowScrolling = true)
	{
		if (dfGUIManager.activeControl == control || (control != null && !control.CanFocus))
		{
			return;
		}
		dfControl dfControl = dfGUIManager.activeControl;
		dfGUIManager.activeControl = control;
		dfFocusEventArgs args = new dfFocusEventArgs(control, dfControl, allowScrolling);
		dfList<dfControl> prevFocusChain = dfList<dfControl>.Obtain();
		if (dfControl != null)
		{
			dfControl dfControl2 = dfControl;
			while (dfControl2 != null)
			{
				prevFocusChain.Add(dfControl2);
				dfControl2 = dfControl2.Parent;
			}
		}
		dfList<dfControl> newFocusChain = dfList<dfControl>.Obtain();
		if (control != null)
		{
			dfControl dfControl3 = control;
			while (dfControl3 != null)
			{
				newFocusChain.Add(dfControl3);
				dfControl3 = dfControl3.Parent;
			}
		}
		if (dfControl != null)
		{
			prevFocusChain.ForEach(delegate(dfControl c)
			{
				if (!newFocusChain.Contains(c))
				{
					c.OnLeaveFocus(args);
				}
			});
			dfControl.OnLostFocus(args);
		}
		if (control != null)
		{
			newFocusChain.ForEach(delegate(dfControl c)
			{
				if (!prevFocusChain.Contains(c))
				{
					c.OnEnterFocus(args);
				}
			});
			control.OnGotFocus(args);
		}
		newFocusChain.Release();
		prevFocusChain.Release();
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x000569A8 File Offset: 0x00054BA8
	public static bool HasFocus(dfControl control)
	{
		return !(control == null) && dfGUIManager.activeControl == control;
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x000569C4 File Offset: 0x00054BC4
	public static bool ContainsFocus(dfControl control)
	{
		if (dfGUIManager.activeControl == control)
		{
			return true;
		}
		if (dfGUIManager.activeControl == null || control == null)
		{
			return object.ReferenceEquals(dfGUIManager.activeControl, control);
		}
		return dfGUIManager.activeControl.transform.IsChildOf(control.transform);
	}

	// Token: 0x06001305 RID: 4869 RVA: 0x00056A20 File Offset: 0x00054C20
	public void BringToFront(dfControl control)
	{
		if (control.Parent != null)
		{
			control = control.GetRootContainer();
		}
		using (dfList<dfControl> topLevelControls = this.getTopLevelControls())
		{
			int num = 0;
			for (int i = 0; i < topLevelControls.Count; i++)
			{
				dfControl dfControl = topLevelControls[i];
				if (dfControl != control)
				{
					dfControl.ZOrder = num++;
				}
			}
			control.ZOrder = num;
			this.Invalidate();
		}
	}

	// Token: 0x06001306 RID: 4870 RVA: 0x00056AB4 File Offset: 0x00054CB4
	public void SendToBack(dfControl control)
	{
		if (control.Parent != null)
		{
			control = control.GetRootContainer();
		}
		using (dfList<dfControl> topLevelControls = this.getTopLevelControls())
		{
			int num = 1;
			for (int i = 0; i < topLevelControls.Count; i++)
			{
				dfControl dfControl = topLevelControls[i];
				if (dfControl != control)
				{
					dfControl.ZOrder = num++;
				}
			}
			control.ZOrder = 0;
			this.Invalidate();
		}
	}

	// Token: 0x06001307 RID: 4871 RVA: 0x00056B48 File Offset: 0x00054D48
	public void Invalidate()
	{
		if (this.isDirty)
		{
			return;
		}
		this.isDirty = true;
		this.updateRenderSettings();
	}

	// Token: 0x06001308 RID: 4872 RVA: 0x00056B64 File Offset: 0x00054D64
	public static void InvalidateAll()
	{
		for (int i = 0; i < dfGUIManager.activeInstances.Count; i++)
		{
			dfGUIManager.activeInstances[i].Invalidate();
		}
	}

	// Token: 0x06001309 RID: 4873 RVA: 0x00056B9C File Offset: 0x00054D9C
	public static void RefreshAll()
	{
		dfGUIManager.RefreshAll(false);
	}

	// Token: 0x0600130A RID: 4874 RVA: 0x00056BA4 File Offset: 0x00054DA4
	public static void RefreshAll(bool force)
	{
		List<dfGUIManager> list = dfGUIManager.activeInstances;
		for (int i = 0; i < list.Count; i++)
		{
			dfGUIManager dfGUIManager = list[i];
			if (dfGUIManager.renderMesh != null && dfGUIManager.renderMesh.Length != 0)
			{
				dfGUIManager.invalidateAllControls();
				if (force || !Application.isPlaying)
				{
					dfGUIManager.Render();
				}
			}
		}
	}

	// Token: 0x0600130B RID: 4875 RVA: 0x00056C10 File Offset: 0x00054E10
	internal void AbortRender()
	{
		this.abortRendering = true;
	}

	// Token: 0x0600130C RID: 4876 RVA: 0x00056C1C File Offset: 0x00054E1C
	public void Render()
	{
		if (this.meshRenderer == null)
		{
			return;
		}
		this.meshRenderer.enabled = false;
		this.FramesRendered++;
		if (dfGUIManager.BeforeRender != null)
		{
			dfGUIManager.BeforeRender(this);
		}
		try
		{
			this.occluders.Clear();
			this.occluders.EnsureCapacity(this.NumControlsRendered);
			this.NumControlsRendered = 0;
			this.controlsRendered.Clear();
			this.drawCallIndices.Clear();
			this.renderGroups.Clear();
			this.TotalDrawCalls = 0;
			this.TotalTriangles = 0;
			if (this.RenderCamera == null || !base.enabled)
			{
				if (this.meshRenderer != null)
				{
					this.meshRenderer.enabled = false;
				}
			}
			else
			{
				if (this.meshRenderer != null && !this.meshRenderer.enabled)
				{
					this.meshRenderer.enabled = true;
				}
				if (this.renderMesh == null || this.renderMesh.Length == 0)
				{
					UnityEngine.Debug.LogError("GUI Manager not initialized before Render() called");
				}
				else
				{
					this.resetDrawCalls();
					dfRenderData dfRenderData = null;
					this.clipStack.Clear();
					this.clipStack.Push(dfTriangleClippingRegion.Obtain());
					uint start_VALUE = dfChecksumUtil.START_VALUE;
					using (dfList<dfControl> topLevelControls = this.getTopLevelControls())
					{
						this.updateRenderOrder(topLevelControls);
						int num = 0;
						while (num < topLevelControls.Count && !this.abortRendering)
						{
							dfControl dfControl = topLevelControls[num];
							this.renderControl(ref dfRenderData, dfControl, start_VALUE, 1f);
							num++;
						}
					}
					if (this.abortRendering)
					{
						this.clipStack.Clear();
						throw new dfAbortRenderingException();
					}
					this.drawCallBuffers.RemoveAll(new Predicate<dfRenderData>(this.isEmptyBuffer));
					this.drawCallCount = this.drawCallBuffers.Count;
					this.TotalDrawCalls = this.drawCallCount;
					if (this.drawCallBuffers.Count == 0)
					{
						if (this.renderFilter.sharedMesh != null)
						{
							this.renderFilter.sharedMesh.Clear();
						}
						if (this.clipStack.Count > 0)
						{
							this.clipStack.Pop().Release();
							this.clipStack.Clear();
						}
					}
					else
					{
						dfRenderData dfRenderData2 = this.compileMasterBuffer();
						this.TotalTriangles = dfRenderData2.Triangles.Count / 3;
						Mesh mesh = this.getRenderMesh();
						this.renderFilter.sharedMesh = mesh;
						Mesh mesh2 = mesh;
						mesh2.Clear(true);
						mesh2.vertices = dfRenderData2.Vertices.Items;
						mesh2.uv = dfRenderData2.UV.Items;
						mesh2.colors32 = dfRenderData2.Colors.Items;
						if (this.generateNormals && dfRenderData2.Normals.Items.Length == dfRenderData2.Vertices.Items.Length)
						{
							mesh2.normals = dfRenderData2.Normals.Items;
							mesh2.tangents = dfRenderData2.Tangents.Items;
						}
						mesh2.subMeshCount = this.submeshes.Count;
						for (int i = 0; i < this.submeshes.Count; i++)
						{
							int num2 = this.submeshes[i];
							int num3 = dfRenderData2.Triangles.Count - num2;
							if (i < this.submeshes.Count - 1)
							{
								num3 = this.submeshes[i + 1] - num2;
							}
							int[] array = dfTempArray<int>.Obtain(num3);
							dfRenderData2.Triangles.CopyTo(num2, array, 0, num3);
							mesh2.SetTriangles(array, i);
						}
						if (this.clipStack.Count != 1)
						{
							UnityEngine.Debug.LogError("Clip stack not properly maintained");
						}
						this.clipStack.Pop().Release();
						this.clipStack.Clear();
						this.updateRenderSettings();
					}
				}
			}
		}
		catch (dfAbortRenderingException)
		{
			this.isDirty = true;
			this.abortRendering = false;
		}
		finally
		{
			this.meshRenderer.enabled = true;
			if (dfGUIManager.AfterRender != null)
			{
				dfGUIManager.AfterRender(this);
			}
		}
	}

	// Token: 0x0600130D RID: 4877 RVA: 0x000570B8 File Offset: 0x000552B8
	private void updateDrawCalls()
	{
		if (this.meshRenderer == null)
		{
			this.initialize();
		}
		Material[] array = this.gatherMaterials();
		this.meshRenderer.sharedMaterials = array;
		int num = this.renderQueueBase + array.Length;
		dfRenderGroup[] items = this.renderGroups.Items;
		int count = this.renderGroups.Count;
		for (int i = 0; i < count; i++)
		{
			items[i].UpdateRenderQueue(ref num);
		}
	}

	// Token: 0x0600130E RID: 4878 RVA: 0x00057134 File Offset: 0x00055334
	private static bool isInsideClippingRegion(Vector3 point, dfControl control)
	{
		while (control != null)
		{
			Plane[] array = ((!control.ClipChildren) ? null : control.GetClippingPlanes());
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (!array[i].GetSide(point))
					{
						return false;
					}
				}
			}
			control = control.Parent;
		}
		return true;
	}

	// Token: 0x0600130F RID: 4879 RVA: 0x000571AC File Offset: 0x000553AC
	private int getMaxZOrder()
	{
		int num = -1;
		using (dfList<dfControl> topLevelControls = this.getTopLevelControls())
		{
			for (int i = 0; i < topLevelControls.Count; i++)
			{
				num = Mathf.Max(num, topLevelControls[i].ZOrder);
			}
		}
		return num;
	}

	// Token: 0x06001310 RID: 4880 RVA: 0x00057210 File Offset: 0x00055410
	private bool isEmptyBuffer(dfRenderData buffer)
	{
		return buffer.Vertices.Count == 0;
	}

	// Token: 0x06001311 RID: 4881 RVA: 0x00057220 File Offset: 0x00055420
	private dfList<dfControl> getTopLevelControls()
	{
		int childCount = base.transform.childCount;
		dfList<dfControl> dfList = dfList<dfControl>.Obtain(childCount);
		dfControl[] items = dfControl.ActiveInstances.Items;
		int count = dfControl.ActiveInstances.Count;
		for (int i = 0; i < count; i++)
		{
			dfControl dfControl = items[i];
			if (dfControl.IsTopLevelControl(this))
			{
				dfList.Add(dfControl);
			}
		}
		dfList.Sort();
		return dfList;
	}

	// Token: 0x06001312 RID: 4882 RVA: 0x00057290 File Offset: 0x00055490
	private void updateRenderSettings()
	{
		Camera camera = this.RenderCamera;
		if (camera == null)
		{
			return;
		}
		if (!this.overrideCamera)
		{
			this.updateRenderCamera(camera);
		}
		if (base.transform.hasChanged)
		{
			Vector3 localScale = base.transform.localScale;
			bool flag = localScale.x < float.Epsilon || !Mathf.Approximately(localScale.x, localScale.y) || !Mathf.Approximately(localScale.x, localScale.z);
			if (flag)
			{
				localScale.y = (localScale.z = (localScale.x = Mathf.Max(localScale.x, 0.001f)));
				base.transform.localScale = localScale;
			}
		}
		if (!this.overrideCamera)
		{
			float num = 1f;
			if (Application.isPlaying && this.PixelPerfectMode)
			{
				float num2 = (float)camera.pixelHeight / (float)this.fixedHeight;
				camera.orthographicSize = num2 / num;
				camera.fieldOfView = 60f * num2;
			}
			else
			{
				camera.orthographicSize = 1f / num;
				camera.fieldOfView = 60f;
			}
		}
		camera.transparencySortMode = TransparencySortMode.Orthographic;
		if (this.cachedScreenSize.sqrMagnitude <= 1E-45f)
		{
			this.cachedScreenSize = new Vector2((float)this.FixedWidth, (float)this.FixedHeight);
		}
		base.transform.hasChanged = false;
	}

	// Token: 0x06001313 RID: 4883 RVA: 0x00057410 File Offset: 0x00055610
	private void updateRenderCamera(Camera camera)
	{
		if (Application.isPlaying && camera.targetTexture != null)
		{
			camera.clearFlags = CameraClearFlags.Color;
			camera.backgroundColor = Color.clear;
		}
		else
		{
			camera.clearFlags = this.overrideClearFlags;
		}
		dfGUICamera component = camera.GetComponent<dfGUICamera>();
		Vector3 vector = Vector3.zero;
		if (component != null)
		{
			vector = component.cameraPositionOffset;
		}
		Vector3 vector2 = ((!Application.isPlaying) ? vector : (-this.uiOffset * this.PixelsToUnits() + vector));
		if (camera.orthographic)
		{
			camera.nearClipPlane = Mathf.Min(camera.nearClipPlane, -1f);
			camera.farClipPlane = Mathf.Max(camera.farClipPlane, 1f);
		}
		else
		{
			float num = camera.fieldOfView * 0.017453292f;
			Vector3[] array = this.GetCorners();
			float num2 = ((!this.uiScaleLegacy) ? this.uiScale : 1f);
			float num3 = Vector3.Distance(array[3], array[0]) * num2;
			float num4 = num3 / (2f * Mathf.Tan(num / 2f));
			Vector3 vector3 = base.transform.TransformDirection(Vector3.back) * num4;
			camera.farClipPlane = Mathf.Max(num4 * 2f, camera.farClipPlane);
			vector2 += vector3 / this.uiScale;
		}
		int height = Screen.height;
		float num5 = 2f / (float)height * ((float)height / (float)this.FixedHeight);
		if (Application.isPlaying && !component.ForceNoHalfPixelOffset && this.needHalfPixelOffset())
		{
			Vector3 vector4 = new Vector3(num5 * 0.5f, num5 * -0.5f, 0f);
			if (AmmonomiconController.GuiManagerIsPageRenderer(this))
			{
				vector4.x /= 2f;
			}
			vector2 += vector4;
		}
		if (!this.overrideCamera)
		{
			camera.renderingPath = RenderingPath.Forward;
			if (camera.pixelWidth % 2 != 0)
			{
				vector2.x += num5 * 0.5f;
			}
			if (camera.pixelHeight % 2 != 0)
			{
				vector2.y += num5 * 0.5f;
			}
			if (Vector3.SqrMagnitude(camera.transform.localPosition - vector2) > 1E-45f)
			{
				camera.transform.localPosition = vector2;
			}
			camera.transform.hasChanged = false;
		}
	}

	// Token: 0x06001314 RID: 4884 RVA: 0x000576B0 File Offset: 0x000558B0
	private dfRenderData compileMasterBuffer()
	{
		dfRenderData dfRenderData2;
		try
		{
			this.submeshes.Clear();
			dfGUIManager.masterBuffer.Clear();
			dfRenderData[] items = this.drawCallBuffers.Items;
			int num = 0;
			for (int i = 0; i < this.drawCallCount; i++)
			{
				num += items[i].Vertices.Count;
			}
			dfGUIManager.masterBuffer.EnsureCapacity(num);
			for (int j = 0; j < this.drawCallCount; j++)
			{
				this.submeshes.Add(dfGUIManager.masterBuffer.Triangles.Count);
				dfRenderData dfRenderData = items[j];
				if (this.generateNormals && dfRenderData.Normals.Count == 0)
				{
					this.generateNormalsAndTangents(dfRenderData);
				}
				dfGUIManager.masterBuffer.Merge(dfRenderData, false);
			}
			dfGUIManager.masterBuffer.ApplyTransform(base.transform.worldToLocalMatrix);
			dfRenderData2 = dfGUIManager.masterBuffer;
		}
		finally
		{
		}
		return dfRenderData2;
	}

	// Token: 0x06001315 RID: 4885 RVA: 0x000577B0 File Offset: 0x000559B0
	private void generateNormalsAndTangents(dfRenderData buffer)
	{
		Vector3 normalized = buffer.Transform.MultiplyVector(Vector3.back).normalized;
		Vector4 vector = buffer.Transform.MultiplyVector(Vector3.right).normalized;
		vector.w = -1f;
		for (int i = 0; i < buffer.Vertices.Count; i++)
		{
			buffer.Normals.Add(normalized);
			buffer.Tangents.Add(vector);
		}
	}

	// Token: 0x06001316 RID: 4886 RVA: 0x00057838 File Offset: 0x00055A38
	private bool needHalfPixelOffset()
	{
		if (this.applyHalfPixelOffset != null)
		{
			return this.applyHalfPixelOffset.Value;
		}
		RuntimePlatform platform = Application.platform;
		bool flag = this.pixelPerfectMode && (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor) && SystemInfo.graphicsDeviceVersion.ToLower().StartsWith("direct");
		bool flag2 = SystemInfo.graphicsShaderLevel >= 40;
		this.applyHalfPixelOffset = new bool?((Application.isEditor || flag) && !flag2);
		return flag;
	}

	// Token: 0x06001317 RID: 4887 RVA: 0x000578C8 File Offset: 0x00055AC8
	private Material[] gatherMaterials()
	{
		Material[] array2;
		try
		{
			int materialCount = this.getMaterialCount();
			int num = 0;
			int num2 = this.renderQueueBase;
			Material[] array = dfTempArray<Material>.Obtain(materialCount);
			for (int i = 0; i < this.drawCallBuffers.Count; i++)
			{
				dfRenderData dfRenderData = this.drawCallBuffers[i];
				if (!(dfRenderData.Material == null))
				{
					Material material = dfMaterialCache.Lookup(dfRenderData.Material);
					material.mainTexture = dfRenderData.Material.mainTexture;
					material.shader = dfRenderData.Shader ?? material.shader;
					if (this.renderQueueSecondDraw > -1 && material.shader.renderQueue > 6000)
					{
						material.renderQueue = material.shader.renderQueue;
						num2++;
					}
					else
					{
						material.renderQueue = num2++;
					}
					material.mainTextureOffset = Vector2.zero;
					material.mainTextureScale = Vector2.zero;
					array[num++] = material;
				}
			}
			array2 = array;
		}
		finally
		{
		}
		return array2;
	}

	// Token: 0x06001318 RID: 4888 RVA: 0x00057A04 File Offset: 0x00055C04
	private int getMaterialCount()
	{
		int num = 0;
		for (int i = 0; i < this.drawCallCount; i++)
		{
			if (this.drawCallBuffers[i] != null && this.drawCallBuffers[i].Material != null)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x00057A5C File Offset: 0x00055C5C
	private void resetDrawCalls()
	{
		this.drawCallCount = 0;
		for (int i = 0; i < this.drawCallBuffers.Count; i++)
		{
			this.drawCallBuffers[i].Release();
		}
		this.drawCallBuffers.Clear();
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x00057AA8 File Offset: 0x00055CA8
	private dfRenderData getDrawCallBuffer(Material material)
	{
		dfRenderData dfRenderData;
		if (this.MergeMaterials && material != null)
		{
			dfRenderData = this.findDrawCallBufferByMaterial(material);
			if (dfRenderData != null)
			{
				return dfRenderData;
			}
		}
		dfRenderData = dfRenderData.Obtain();
		dfRenderData.Material = material;
		this.drawCallBuffers.Add(dfRenderData);
		this.drawCallCount++;
		return dfRenderData;
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x00057B08 File Offset: 0x00055D08
	private dfRenderData findDrawCallBufferByMaterial(Material material)
	{
		for (int i = 0; i < this.drawCallCount; i++)
		{
			if (this.drawCallBuffers[i].Material == material)
			{
				return this.drawCallBuffers[i];
			}
		}
		return null;
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x00057B58 File Offset: 0x00055D58
	private Mesh getRenderMesh()
	{
		this.activeRenderMesh = ((this.activeRenderMesh != 1) ? 1 : 0);
		return this.renderMesh[this.activeRenderMesh];
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x00057B80 File Offset: 0x00055D80
	private void renderControl(ref dfRenderData buffer, dfControl control, uint checksum, float opacity)
	{
		if (!control.enabled || !control.gameObject.activeSelf)
		{
			return;
		}
		float num = opacity * control.Opacity;
		dfRenderGroup renderGroupForControl = dfRenderGroup.GetRenderGroupForControl(control, true);
		if (renderGroupForControl != null && renderGroupForControl.enabled)
		{
			this.renderGroups.Add(renderGroupForControl);
			renderGroupForControl.Render(this.renderCamera, control, this.occluders, this.controlsRendered, checksum, num);
			return;
		}
		if (num <= 0.001f || !control.GetIsVisibleRaw())
		{
			return;
		}
		dfTriangleClippingRegion dfTriangleClippingRegion = this.clipStack.Peek();
		checksum = dfChecksumUtil.Calculate(checksum, control.Version);
		Bounds bounds = control.GetBounds();
		bool flag = false;
		if (!(control is IDFMultiRender))
		{
			dfRenderData dfRenderData = control.Render();
			if (dfRenderData != null)
			{
				this.processRenderData(ref buffer, dfRenderData, ref bounds, checksum, dfTriangleClippingRegion, ref flag);
			}
		}
		else
		{
			dfList<dfRenderData> dfList = ((IDFMultiRender)control).RenderMultiple();
			if (dfList != null)
			{
				dfRenderData[] items = dfList.Items;
				int count = dfList.Count;
				for (int i = 0; i < count; i++)
				{
					dfRenderData dfRenderData2 = items[i];
					if (dfRenderData2 != null)
					{
						this.processRenderData(ref buffer, dfRenderData2, ref bounds, checksum, dfTriangleClippingRegion, ref flag);
					}
				}
			}
		}
		control.setClippingState(flag);
		this.NumControlsRendered++;
		this.occluders.Add(this.getControlOccluder(control));
		this.controlsRendered.Add(control);
		this.drawCallIndices.Add(this.drawCallBuffers.Count - 1);
		if (control.ClipChildren)
		{
			dfTriangleClippingRegion = dfTriangleClippingRegion.Obtain(dfTriangleClippingRegion, control);
			this.clipStack.Push(dfTriangleClippingRegion);
		}
		dfControl[] items2 = control.Controls.Items;
		int count2 = control.Controls.Count;
		this.controlsRendered.EnsureCapacity(this.controlsRendered.Count + count2);
		this.occluders.EnsureCapacity(this.occluders.Count + count2);
		for (int j = 0; j < count2; j++)
		{
			this.renderControl(ref buffer, items2[j], checksum, num);
		}
		if (control.ClipChildren)
		{
			this.clipStack.Pop().Release();
		}
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x00057DB8 File Offset: 0x00055FB8
	private Rect getControlOccluder(dfControl control)
	{
		if (!control.IsInteractive)
		{
			return default(Rect);
		}
		Rect screenRect = control.GetScreenRect();
		Vector2 vector = new Vector2(screenRect.width * control.HotZoneScale.x, screenRect.height * control.HotZoneScale.y);
		Vector2 vector2 = new Vector2(vector.x - screenRect.width, vector.y - screenRect.height) * 0.5f;
		return new Rect(screenRect.x - vector2.x, screenRect.y - vector2.y, vector.x, vector.y);
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x00057E78 File Offset: 0x00056078
	private bool processRenderData(ref dfRenderData buffer, dfRenderData controlData, ref Bounds bounds, uint checksum, dfTriangleClippingRegion clipInfo, ref bool wasClipped)
	{
		wasClipped = false;
		if (controlData == null || controlData.Material == null || !controlData.IsValid())
		{
			return false;
		}
		bool flag = false;
		if (buffer == null)
		{
			flag = true;
		}
		else if (!object.Equals(controlData.Material, buffer.Material))
		{
			flag = true;
		}
		else if (!this.textureEqual(controlData.Material.mainTexture, buffer.Material.mainTexture))
		{
			flag = true;
		}
		else if (!this.shaderEqual(buffer.Shader, controlData.Shader))
		{
			flag = true;
		}
		if (flag)
		{
			buffer = this.getDrawCallBuffer(controlData.Material);
			buffer.Material = controlData.Material;
			buffer.Material.mainTexture = controlData.Material.mainTexture;
			buffer.Material.shader = controlData.Shader ?? controlData.Material.shader;
		}
		if (clipInfo.PerformClipping(buffer, ref bounds, checksum, controlData))
		{
			return true;
		}
		wasClipped = true;
		return false;
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x00057F94 File Offset: 0x00056194
	private bool textureEqual(Texture lhs, Texture rhs)
	{
		return object.Equals(lhs, rhs);
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x00057FA0 File Offset: 0x000561A0
	private bool shaderEqual(Shader lhs, Shader rhs)
	{
		if (lhs == null || rhs == null)
		{
			return object.ReferenceEquals(lhs, rhs);
		}
		return lhs.name.Equals(rhs.name);
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x00057FD4 File Offset: 0x000561D4
	private void initialize()
	{
		if (Application.isPlaying && this.renderCamera == null)
		{
			UnityEngine.Debug.LogError("No camera is assigned to the GUIManager");
			return;
		}
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		if (this.meshRenderer == null)
		{
			this.meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
		}
		this.renderFilter = base.GetComponent<MeshFilter>();
		if (this.renderFilter == null)
		{
			this.renderFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		this.renderMesh = new Mesh[]
		{
			new Mesh
			{
				hideFlags = HideFlags.DontSave
			},
			new Mesh
			{
				hideFlags = HideFlags.DontSave
			}
		};
		this.renderMesh[0].MarkDynamic();
		this.renderMesh[1].MarkDynamic();
		if (this.fixedWidth < 0)
		{
			this.fixedWidth = Mathf.RoundToInt((float)this.fixedHeight * 1.33333f);
			dfControl[] componentsInChildren = base.GetComponentsInChildren<dfControl>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ResetLayout();
			}
		}
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x000580F4 File Offset: 0x000562F4
	private void onResolutionChanged()
	{
		int num = ((!Application.isPlaying) ? this.FixedHeight : this.renderCamera.pixelHeight);
		this.onResolutionChanged(this.FixedHeight, num);
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x06001324 RID: 4900 RVA: 0x00058130 File Offset: 0x00056330
	private float RenderAspect
	{
		get
		{
			return (!this.FixedAspect) ? this.RenderCamera.aspect : 1.7777778f;
		}
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x00058154 File Offset: 0x00056354
	private void onResolutionChanged(int oldSize, int currentSize)
	{
		float renderAspect = this.RenderAspect;
		float num = (float)oldSize * renderAspect;
		float num2 = (float)currentSize * renderAspect;
		Vector2 vector = new Vector2(num, (float)oldSize);
		Vector2 vector2 = new Vector2(num2, (float)currentSize);
		this.onResolutionChanged(vector, vector2);
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x00058190 File Offset: 0x00056390
	public static void ForceResolutionUpdates()
	{
		for (int i = 0; i < dfGUIManager.activeInstances.Count; i++)
		{
			dfGUIManager.activeInstances[i].onResolutionChanged();
		}
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x000581C8 File Offset: 0x000563C8
	public void ResolutionChanged()
	{
		this.onResolutionChanged();
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x000581D0 File Offset: 0x000563D0
	private void onResolutionChanged(Vector2 oldSize, Vector2 currentSize)
	{
		if (this.shutdownInProcess)
		{
			return;
		}
		this.cachedScreenSize = currentSize;
		this.applyHalfPixelOffset = null;
		float renderAspect = this.RenderAspect;
		float num = oldSize.y * renderAspect;
		float num2 = currentSize.y * renderAspect;
		Vector2 vector = new Vector2(num, oldSize.y);
		Vector2 vector2 = new Vector2(num2, currentSize.y);
		dfControl[] componentsInChildren = base.GetComponentsInChildren<dfControl>();
		Array.Sort<dfControl>(componentsInChildren, new Comparison<dfControl>(this.renderSortFunc));
		this.ResolutionIsChanging = true;
		for (int i = componentsInChildren.Length - 1; i >= 0; i--)
		{
			if (this.pixelPerfectMode && componentsInChildren[i].Parent == null)
			{
				componentsInChildren[i].MakePixelPerfect();
			}
			componentsInChildren[i].OnResolutionChanged(vector, vector2);
		}
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].PerformLayout();
		}
		int num3 = 0;
		while (num3 < componentsInChildren.Length && this.pixelPerfectMode)
		{
			if (componentsInChildren[num3].Parent == null)
			{
				componentsInChildren[num3].MakePixelPerfect();
			}
			num3++;
		}
		this.ResolutionIsChanging = false;
		this.isDirty = true;
		this.updateRenderSettings();
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x00058328 File Offset: 0x00056528
	private void invalidateAllControls()
	{
		dfControl[] componentsInChildren = base.GetComponentsInChildren<dfControl>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Invalidate();
		}
		this.updateRenderOrder();
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x00058360 File Offset: 0x00056560
	private int renderSortFunc(dfControl lhs, dfControl rhs)
	{
		return lhs.RenderOrder.CompareTo(rhs.RenderOrder);
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x00058384 File Offset: 0x00056584
	private void updateRenderOrder()
	{
		this.updateRenderOrder(null);
	}

	// Token: 0x0600132C RID: 4908 RVA: 0x00058390 File Offset: 0x00056590
	private void updateRenderOrder(dfList<dfControl> list)
	{
		dfList<dfControl> dfList = list;
		bool flag = false;
		if (list == null)
		{
			dfList = this.getTopLevelControls();
			flag = true;
		}
		else
		{
			dfList.Sort();
		}
		int num = 0;
		int count = dfList.Count;
		dfControl[] items = dfList.Items;
		for (int i = 0; i < count; i++)
		{
			dfControl dfControl = items[i];
			if (dfControl.Parent == null)
			{
				dfControl.setRenderOrder(ref num);
			}
		}
		if (flag)
		{
			dfList.Release();
		}
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x00058410 File Offset: 0x00056610
	public int CompareTo(dfGUIManager other)
	{
		int num = this.renderQueueBase.CompareTo(other.renderQueueBase);
		if (num == 0 && this.RenderCamera != null && other.RenderCamera != null)
		{
			return this.RenderCamera.depth.CompareTo(other.RenderCamera.depth);
		}
		return num;
	}

	// Token: 0x04001063 RID: 4195
	[SerializeField]
	public CameraClearFlags overrideClearFlags = CameraClearFlags.Depth;

	// Token: 0x04001064 RID: 4196
	[SerializeField]
	protected float uiScale = 1f;

	// Token: 0x04001065 RID: 4197
	[SerializeField]
	public Vector2 InputOffsetScreenPercent;

	// Token: 0x04001066 RID: 4198
	[SerializeField]
	protected bool uiScaleLegacy = true;

	// Token: 0x04001067 RID: 4199
	[SerializeField]
	protected dfInputManager inputManager;

	// Token: 0x04001068 RID: 4200
	[SerializeField]
	protected int fixedWidth = -1;

	// Token: 0x04001069 RID: 4201
	[SerializeField]
	protected int fixedHeight = 600;

	// Token: 0x0400106A RID: 4202
	[SerializeField]
	public bool FixedAspect;

	// Token: 0x0400106B RID: 4203
	[SerializeField]
	protected dfAtlas atlas;

	// Token: 0x0400106C RID: 4204
	[SerializeField]
	protected dfFontBase defaultFont;

	// Token: 0x0400106D RID: 4205
	[SerializeField]
	protected bool mergeMaterials;

	// Token: 0x0400106E RID: 4206
	[SerializeField]
	protected bool pixelPerfectMode = true;

	// Token: 0x0400106F RID: 4207
	[SerializeField]
	protected Camera renderCamera;

	// Token: 0x04001070 RID: 4208
	[SerializeField]
	protected bool generateNormals;

	// Token: 0x04001071 RID: 4209
	[SerializeField]
	protected bool consumeMouseEvents;

	// Token: 0x04001072 RID: 4210
	[SerializeField]
	protected bool overrideCamera;

	// Token: 0x04001073 RID: 4211
	[SerializeField]
	protected int renderQueueBase = 3000;

	// Token: 0x04001074 RID: 4212
	[SerializeField]
	public int renderQueueSecondDraw = -1;

	// Token: 0x04001075 RID: 4213
	[SerializeField]
	public List<dfDesignGuide> guides = new List<dfDesignGuide>();

	// Token: 0x04001076 RID: 4214
	private static List<dfGUIManager> activeInstances = new List<dfGUIManager>();

	// Token: 0x04001077 RID: 4215
	private static dfControl activeControl = null;

	// Token: 0x04001078 RID: 4216
	private static Stack<dfGUIManager.ModalControlReference> modalControlStack = new Stack<dfGUIManager.ModalControlReference>();

	// Token: 0x04001079 RID: 4217
	private dfGUICamera guiCamera;

	// Token: 0x0400107A RID: 4218
	private Mesh[] renderMesh;

	// Token: 0x0400107B RID: 4219
	private MeshFilter renderFilter;

	// Token: 0x0400107C RID: 4220
	private MeshRenderer meshRenderer;

	// Token: 0x0400107D RID: 4221
	private int activeRenderMesh;

	// Token: 0x0400107E RID: 4222
	private int cachedChildCount;

	// Token: 0x0400107F RID: 4223
	private bool isDirty;

	// Token: 0x04001080 RID: 4224
	private bool abortRendering;

	// Token: 0x04001081 RID: 4225
	private Vector2 cachedScreenSize;

	// Token: 0x04001082 RID: 4226
	private Vector3[] corners = new Vector3[4];

	// Token: 0x04001083 RID: 4227
	private dfList<Rect> occluders = new dfList<Rect>(256);

	// Token: 0x04001084 RID: 4228
	private Stack<dfTriangleClippingRegion> clipStack = new Stack<dfTriangleClippingRegion>();

	// Token: 0x04001085 RID: 4229
	private static dfRenderData masterBuffer = new dfRenderData(4096);

	// Token: 0x04001086 RID: 4230
	private dfList<dfRenderData> drawCallBuffers = new dfList<dfRenderData>();

	// Token: 0x04001087 RID: 4231
	private dfList<dfRenderGroup> renderGroups = new dfList<dfRenderGroup>();

	// Token: 0x04001088 RID: 4232
	private List<int> submeshes = new List<int>();

	// Token: 0x04001089 RID: 4233
	private int drawCallCount;

	// Token: 0x0400108A RID: 4234
	private Vector2 uiOffset = Vector2.zero;

	// Token: 0x0400108B RID: 4235
	private static Plane[] clippingPlanes;

	// Token: 0x0400108C RID: 4236
	private dfList<int> drawCallIndices = new dfList<int>();

	// Token: 0x0400108D RID: 4237
	private dfList<dfControl> controlsRendered = new dfList<dfControl>();

	// Token: 0x0400108E RID: 4238
	private bool shutdownInProcess;

	// Token: 0x0400108F RID: 4239
	private int suspendCount;

	// Token: 0x04001094 RID: 4244
	private bool? applyHalfPixelOffset;

	// Token: 0x04001095 RID: 4245
	[NonSerialized]
	public bool ResolutionIsChanging;

	// Token: 0x020003CD RID: 973
	// (Invoke) Token: 0x06001330 RID: 4912
	[dfEventCategory("Modal Dialog")]
	public delegate void ModalPoppedCallback(dfControl control);

	// Token: 0x020003CE RID: 974
	// (Invoke) Token: 0x06001334 RID: 4916
	[dfEventCategory("Global Callbacks")]
	public delegate void RenderCallback(dfGUIManager manager);

	// Token: 0x020003CF RID: 975
	private struct ModalControlReference
	{
		// Token: 0x04001096 RID: 4246
		public dfControl control;

		// Token: 0x04001097 RID: 4247
		public dfGUIManager.ModalPoppedCallback callback;
	}
}
