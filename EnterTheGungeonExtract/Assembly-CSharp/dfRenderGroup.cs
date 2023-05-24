using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000411 RID: 1041
[ExecuteInEditMode]
[AddComponentMenu("Daikon Forge/User Interface/Render Group")]
[Serializable]
internal class dfRenderGroup : MonoBehaviour
{
	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x06001796 RID: 6038 RVA: 0x0007057C File Offset: 0x0006E77C
	// (set) Token: 0x06001797 RID: 6039 RVA: 0x00070584 File Offset: 0x0006E784
	public dfClippingMethod ClipType
	{
		get
		{
			return this.clipType;
		}
		set
		{
			if (value != this.clipType)
			{
				this.clipType = value;
				if (this.attachedControl != null)
				{
					this.attachedControl.Invalidate();
				}
			}
		}
	}

	// Token: 0x06001798 RID: 6040 RVA: 0x000705B8 File Offset: 0x0006E7B8
	public void OnEnable()
	{
		dfRenderGroup.activeInstances.Add(this);
		this.isDirty = true;
		if (this.meshRenderer == null)
		{
			this.initialize();
		}
		this.meshRenderer.enabled = true;
		if (this.attachedControl != null)
		{
			this.attachedControl.Invalidate();
		}
		else
		{
			dfGUIManager.InvalidateAll();
		}
		this.attachedControl = base.GetComponent<dfControl>();
	}

	// Token: 0x06001799 RID: 6041 RVA: 0x0007062C File Offset: 0x0006E82C
	public void OnDisable()
	{
		dfRenderGroup.activeInstances.Remove(this);
		if (this.meshRenderer != null)
		{
			this.meshRenderer.enabled = false;
		}
		if (this.attachedControl != null)
		{
			this.attachedControl.Invalidate();
		}
	}

	// Token: 0x0600179A RID: 6042 RVA: 0x00070680 File Offset: 0x0006E880
	public void OnDestroy()
	{
		if (this.renderFilter != null)
		{
			this.renderFilter.sharedMesh = null;
		}
		this.renderFilter = null;
		this.meshRenderer = null;
		if (this.renderMesh != null)
		{
			this.renderMesh.Clear();
			UnityEngine.Object.DestroyImmediate(this.renderMesh);
			this.renderMesh = null;
		}
		dfGUIManager.InvalidateAll();
	}

	// Token: 0x0600179B RID: 6043 RVA: 0x000706EC File Offset: 0x0006E8EC
	internal static dfRenderGroup GetRenderGroupForControl(dfControl control)
	{
		return dfRenderGroup.GetRenderGroupForControl(control, false);
	}

	// Token: 0x0600179C RID: 6044 RVA: 0x000706F8 File Offset: 0x0006E8F8
	internal static dfRenderGroup GetRenderGroupForControl(dfControl control, bool directlyAttachedOnly)
	{
		Transform transform = control.transform;
		for (int i = 0; i < dfRenderGroup.activeInstances.Count; i++)
		{
			dfRenderGroup dfRenderGroup = dfRenderGroup.activeInstances[i];
			if (dfRenderGroup.attachedControl == control)
			{
				return dfRenderGroup;
			}
			if (!directlyAttachedOnly && transform.IsChildOf(dfRenderGroup.transform))
			{
				return dfRenderGroup;
			}
		}
		return null;
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x00070760 File Offset: 0x0006E960
	internal static void InvalidateGroupForControl(dfControl control)
	{
		Transform transform = control.transform;
		for (int i = 0; i < dfRenderGroup.activeInstances.Count; i++)
		{
			dfRenderGroup dfRenderGroup = dfRenderGroup.activeInstances[i];
			if (transform.IsChildOf(dfRenderGroup.transform))
			{
				dfRenderGroup.isDirty = true;
			}
		}
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x000707B4 File Offset: 0x0006E9B4
	internal void Render(Camera renderCamera, dfControl control, dfList<Rect> occluders, dfList<dfControl> controlsRendered, uint checksum, float opacity)
	{
		if (this.meshRenderer == null)
		{
			this.initialize();
		}
		this.renderCamera = renderCamera;
		this.attachedControl = control;
		if (!this.isDirty)
		{
			occluders.AddRange(this.groupOccluders);
			controlsRendered.AddRange(this.groupControls);
			return;
		}
		this.groupOccluders.Clear();
		this.groupControls.Clear();
		this.renderGroups.Clear();
		this.resetDrawCalls();
		this.clipInfo = default(dfRenderGroup.ClipRegionInfo);
		this.clipRect = default(Rect);
		dfRenderData dfRenderData = null;
		using (dfTriangleClippingRegion dfTriangleClippingRegion = dfTriangleClippingRegion.Obtain())
		{
			this.clipStack.Clear();
			this.clipStack.Push(dfTriangleClippingRegion);
			this.renderControl(ref dfRenderData, control, checksum, opacity);
			this.clipStack.Pop();
		}
		this.drawCallBuffers.RemoveAll(new Predicate<dfRenderData>(this.isEmptyBuffer));
		this.drawCallCount = this.drawCallBuffers.Count;
		if (this.drawCallBuffers.Count == 0)
		{
			this.meshRenderer.enabled = false;
			return;
		}
		this.meshRenderer.enabled = true;
		dfRenderData dfRenderData2 = this.compileMasterBuffer();
		Mesh mesh = this.renderMesh;
		mesh.Clear(true);
		mesh.vertices = dfRenderData2.Vertices.Items;
		mesh.uv = dfRenderData2.UV.Items;
		mesh.colors32 = dfRenderData2.Colors.Items;
		mesh.subMeshCount = this.submeshes.Count;
		for (int i = 0; i < this.submeshes.Count; i++)
		{
			int num = this.submeshes[i];
			int num2 = dfRenderData2.Triangles.Count - num;
			if (i < this.submeshes.Count - 1)
			{
				num2 = this.submeshes[i + 1] - num;
			}
			int[] array = dfTempArray<int>.Obtain(num2);
			dfRenderData2.Triangles.CopyTo(num, array, 0, num2);
			mesh.SetTriangles(array, i);
		}
		this.isDirty = false;
		occluders.AddRange(this.groupOccluders);
		controlsRendered.AddRange(this.groupControls);
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x00070A10 File Offset: 0x0006EC10
	internal void UpdateRenderQueue(ref int renderQueueBase)
	{
		int materialCount = this.getMaterialCount();
		int num = 0;
		Material[] array = dfTempArray<Material>.Obtain(materialCount);
		for (int i = 0; i < this.drawCallBuffers.Count; i++)
		{
			if (!(this.drawCallBuffers[i].Material == null))
			{
				Material material = dfMaterialCache.Lookup(this.drawCallBuffers[i].Material);
				material.mainTexture = this.drawCallBuffers[i].Material.mainTexture;
				material.shader = this.drawCallBuffers[i].Shader ?? material.shader;
				material.mainTextureScale = Vector2.zero;
				material.mainTextureOffset = Vector2.zero;
				material.renderQueue = ++renderQueueBase;
				bool flag = Application.isPlaying && this.clipType == dfClippingMethod.Shader && !this.clipInfo.IsEmpty && i > 0;
				if (flag)
				{
					Vector3 vector = this.attachedControl.Pivot.TransformToCenter(this.attachedControl.Size);
					float num2 = vector.x + this.clipInfo.Offset.x;
					float num3 = vector.y + this.clipInfo.Offset.y;
					float num4 = this.attachedControl.PixelsToUnits();
					material.mainTextureScale = new Vector2(1f / (-this.clipInfo.Size.x * 0.5f * num4), 1f / (-this.clipInfo.Size.y * 0.5f * num4));
					material.mainTextureOffset = new Vector2(num2 / this.clipInfo.Size.x * 2f, num3 / this.clipInfo.Size.y * 2f);
				}
				array[num++] = material;
			}
		}
		this.meshRenderer.sharedMaterials = array;
		dfRenderGroup[] items = this.renderGroups.Items;
		int count = this.renderGroups.Count;
		for (int j = 0; j < count; j++)
		{
			items[j].UpdateRenderQueue(ref renderQueueBase);
		}
	}

	// Token: 0x060017A0 RID: 6048 RVA: 0x00070C60 File Offset: 0x0006EE60
	private void renderControl(ref dfRenderData buffer, dfControl control, uint checksum, float opacity)
	{
		if (!control.enabled || !control.gameObject.activeSelf)
		{
			return;
		}
		float num = opacity * control.Opacity;
		if (num <= 0.001f)
		{
			return;
		}
		dfRenderGroup renderGroupForControl = dfRenderGroup.GetRenderGroupForControl(control, true);
		if (renderGroupForControl != null && renderGroupForControl != this && renderGroupForControl.enabled)
		{
			this.renderGroups.Add(renderGroupForControl);
			renderGroupForControl.Render(this.renderCamera, control, this.groupOccluders, this.groupControls, checksum, num);
			return;
		}
		if (!control.GetIsVisibleRaw())
		{
			return;
		}
		dfTriangleClippingRegion dfTriangleClippingRegion = this.clipStack.Peek();
		checksum = dfChecksumUtil.Calculate(checksum, control.Version);
		Bounds bounds = control.GetBounds();
		Rect screenRect = control.GetScreenRect();
		Rect controlOccluder = this.getControlOccluder(ref screenRect, control);
		bool flag = false;
		if (!(control is IDFMultiRender))
		{
			dfRenderData dfRenderData = control.Render();
			if (dfRenderData != null)
			{
				this.processRenderData(ref buffer, dfRenderData, ref bounds, ref screenRect, checksum, dfTriangleClippingRegion, ref flag);
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
						this.processRenderData(ref buffer, dfRenderData2, ref bounds, ref screenRect, checksum, dfTriangleClippingRegion, ref flag);
					}
				}
			}
		}
		control.setClippingState(flag);
		this.groupOccluders.Add(controlOccluder);
		this.groupControls.Add(control);
		if (control.ClipChildren)
		{
			if (!Application.isPlaying || this.clipType == dfClippingMethod.Software)
			{
				dfTriangleClippingRegion = dfTriangleClippingRegion.Obtain(dfTriangleClippingRegion, control);
				this.clipStack.Push(dfTriangleClippingRegion);
			}
			else if (this.clipInfo.IsEmpty)
			{
				this.setClipRegion(control, ref screenRect);
			}
		}
		dfControl[] items2 = control.Controls.Items;
		int count2 = control.Controls.Count;
		this.groupControls.EnsureCapacity(this.groupControls.Count + count2);
		this.groupOccluders.EnsureCapacity(this.groupOccluders.Count + count2);
		for (int j = 0; j < count2; j++)
		{
			this.renderControl(ref buffer, items2[j], checksum, num);
		}
		if (control.ClipChildren && (!Application.isPlaying || this.clipType == dfClippingMethod.Software))
		{
			this.clipStack.Pop().Release();
		}
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x00070ED8 File Offset: 0x0006F0D8
	private void setClipRegion(dfControl control, ref Rect screenRect)
	{
		Vector2 size = control.Size;
		RectOffset clipPadding = control.GetClipPadding();
		float num = Mathf.Min(Mathf.Max(0f, Mathf.Min(size.x, (float)clipPadding.horizontal)), size.x);
		float num2 = Mathf.Min(Mathf.Max(0f, Mathf.Min(size.y, (float)clipPadding.vertical)), size.y);
		this.clipInfo = default(dfRenderGroup.ClipRegionInfo);
		this.clipInfo.Size = Vector2.Max(new Vector2(size.x - num, size.y - num2), Vector3.zero);
		this.clipInfo.Offset = new Vector3((float)(clipPadding.left - clipPadding.right), (float)(-(float)(clipPadding.top - clipPadding.bottom))) * 0.5f;
		this.clipRect = ((!this.containerRect.IsEmpty()) ? this.containerRect.Intersection(screenRect) : screenRect);
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x00070FF8 File Offset: 0x0006F1F8
	private bool processRenderData(ref dfRenderData buffer, dfRenderData controlData, ref Bounds bounds, ref Rect screenRect, uint checksum, dfTriangleClippingRegion clipInfo, ref bool wasClipped)
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
		else if (!this.clipInfo.IsEmpty && this.drawCallBuffers.Count == 1)
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
		if (!Application.isPlaying || this.clipType == dfClippingMethod.Software)
		{
			if (clipInfo.PerformClipping(buffer, ref bounds, checksum, controlData))
			{
				return true;
			}
			wasClipped = true;
		}
		else if (this.clipRect.IsEmpty() || screenRect.Intersects(this.clipRect))
		{
			buffer.Merge(controlData);
		}
		else
		{
			wasClipped = true;
		}
		return false;
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x00071190 File Offset: 0x0006F390
	private dfRenderData compileMasterBuffer()
	{
		this.submeshes.Clear();
		dfRenderGroup.masterBuffer.Clear();
		dfRenderData[] items = this.drawCallBuffers.Items;
		int num = 0;
		for (int i = 0; i < this.drawCallCount; i++)
		{
			num += items[i].Vertices.Count;
		}
		dfRenderGroup.masterBuffer.EnsureCapacity(num);
		for (int j = 0; j < this.drawCallCount; j++)
		{
			this.submeshes.Add(dfRenderGroup.masterBuffer.Triangles.Count);
			dfRenderData dfRenderData = items[j];
			dfRenderGroup.masterBuffer.Merge(dfRenderData, false);
		}
		dfRenderGroup.masterBuffer.ApplyTransform(base.transform.worldToLocalMatrix);
		return dfRenderGroup.masterBuffer;
	}

	// Token: 0x060017A4 RID: 6052 RVA: 0x00071250 File Offset: 0x0006F450
	private bool isEmptyBuffer(dfRenderData buffer)
	{
		return buffer.Vertices.Count == 0;
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x00071260 File Offset: 0x0006F460
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

	// Token: 0x060017A6 RID: 6054 RVA: 0x000712B8 File Offset: 0x0006F4B8
	private void resetDrawCalls()
	{
		this.drawCallCount = 0;
		for (int i = 0; i < this.drawCallBuffers.Count; i++)
		{
			this.drawCallBuffers[i].Release();
		}
		this.drawCallBuffers.Clear();
	}

	// Token: 0x060017A7 RID: 6055 RVA: 0x00071304 File Offset: 0x0006F504
	private dfRenderData getDrawCallBuffer(Material material)
	{
		dfRenderData dfRenderData = dfRenderData.Obtain();
		dfRenderData.Material = material;
		this.drawCallBuffers.Add(dfRenderData);
		this.drawCallCount++;
		return dfRenderData;
	}

	// Token: 0x060017A8 RID: 6056 RVA: 0x0007133C File Offset: 0x0006F53C
	private Rect getControlOccluder(ref Rect screenRect, dfControl control)
	{
		if (!control.IsInteractive)
		{
			return default(Rect);
		}
		Vector2 vector = new Vector2(screenRect.width * control.HotZoneScale.x, screenRect.height * control.HotZoneScale.y);
		Vector2 vector2 = new Vector2(vector.x - screenRect.width, vector.y - screenRect.height) * 0.5f;
		return new Rect(screenRect.x - vector2.x, screenRect.y - vector2.y, vector.x, vector.y);
	}

	// Token: 0x060017A9 RID: 6057 RVA: 0x000713EC File Offset: 0x0006F5EC
	private bool textureEqual(Texture lhs, Texture rhs)
	{
		return object.Equals(lhs, rhs);
	}

	// Token: 0x060017AA RID: 6058 RVA: 0x000713F8 File Offset: 0x0006F5F8
	private bool shaderEqual(Shader lhs, Shader rhs)
	{
		if (lhs == null || rhs == null)
		{
			return object.ReferenceEquals(lhs, rhs);
		}
		return lhs.name.Equals(rhs.name);
	}

	// Token: 0x060017AB RID: 6059 RVA: 0x0007142C File Offset: 0x0006F62C
	private void initialize()
	{
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		if (this.meshRenderer == null)
		{
			this.meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
		}
		this.meshRenderer.hideFlags = HideFlags.HideInInspector;
		this.renderFilter = base.GetComponent<MeshFilter>();
		if (this.renderFilter == null)
		{
			this.renderFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		this.renderFilter.hideFlags = HideFlags.HideInInspector;
		this.renderMesh = new Mesh
		{
			hideFlags = HideFlags.DontSave
		};
		this.renderMesh.MarkDynamic();
		this.renderFilter.sharedMesh = this.renderMesh;
	}

	// Token: 0x040012FB RID: 4859
	private static List<dfRenderGroup> activeInstances = new List<dfRenderGroup>();

	// Token: 0x040012FC RID: 4860
	[SerializeField]
	protected dfClippingMethod clipType;

	// Token: 0x040012FD RID: 4861
	private Mesh renderMesh;

	// Token: 0x040012FE RID: 4862
	private MeshFilter renderFilter;

	// Token: 0x040012FF RID: 4863
	private MeshRenderer meshRenderer;

	// Token: 0x04001300 RID: 4864
	private Camera renderCamera;

	// Token: 0x04001301 RID: 4865
	private dfControl attachedControl;

	// Token: 0x04001302 RID: 4866
	private static dfRenderData masterBuffer = new dfRenderData(4096);

	// Token: 0x04001303 RID: 4867
	private dfList<dfRenderData> drawCallBuffers = new dfList<dfRenderData>();

	// Token: 0x04001304 RID: 4868
	private List<int> submeshes = new List<int>();

	// Token: 0x04001305 RID: 4869
	private Stack<dfTriangleClippingRegion> clipStack = new Stack<dfTriangleClippingRegion>();

	// Token: 0x04001306 RID: 4870
	private dfList<Rect> groupOccluders = new dfList<Rect>();

	// Token: 0x04001307 RID: 4871
	private dfList<dfControl> groupControls = new dfList<dfControl>();

	// Token: 0x04001308 RID: 4872
	private dfList<dfRenderGroup> renderGroups = new dfList<dfRenderGroup>();

	// Token: 0x04001309 RID: 4873
	private dfRenderGroup.ClipRegionInfo clipInfo = default(dfRenderGroup.ClipRegionInfo);

	// Token: 0x0400130A RID: 4874
	private Rect clipRect = default(Rect);

	// Token: 0x0400130B RID: 4875
	private Rect containerRect = default(Rect);

	// Token: 0x0400130C RID: 4876
	private int drawCallCount;

	// Token: 0x0400130D RID: 4877
	private bool isDirty;

	// Token: 0x02000412 RID: 1042
	private struct ClipRegionInfo
	{
		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060017AD RID: 6061 RVA: 0x000714FC File Offset: 0x0006F6FC
		public bool IsEmpty
		{
			get
			{
				return this.Offset == Vector2.zero && this.Size == Vector2.zero;
			}
		}

		// Token: 0x0400130E RID: 4878
		public Vector2 Offset;

		// Token: 0x0400130F RID: 4879
		public Vector2 Size;
	}
}
