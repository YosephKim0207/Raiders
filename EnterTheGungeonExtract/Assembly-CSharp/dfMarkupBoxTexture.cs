using System;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class dfMarkupBoxTexture : dfMarkupBox
{
	// Token: 0x06001B57 RID: 6999 RVA: 0x00081188 File Offset: 0x0007F388
	public dfMarkupBoxTexture(dfMarkupElement element, dfMarkupDisplayType display, dfMarkupStyle style)
		: base(element, display, style)
	{
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x06001B58 RID: 7000 RVA: 0x000811A0 File Offset: 0x0007F3A0
	// (set) Token: 0x06001B59 RID: 7001 RVA: 0x000811A8 File Offset: 0x0007F3A8
	public Texture Texture { get; set; }

	// Token: 0x06001B5A RID: 7002 RVA: 0x000811B4 File Offset: 0x0007F3B4
	internal void LoadTexture(Texture texture)
	{
		if (texture == null)
		{
			throw new InvalidOperationException();
		}
		this.Texture = texture;
		this.Size = new Vector2((float)texture.width, (float)texture.height);
		this.Baseline = (int)this.Size.y;
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x00081208 File Offset: 0x0007F408
	protected override dfRenderData OnRebuildRenderData()
	{
		this.renderData.Clear();
		this.ensureMaterial();
		this.renderData.Material = this.material;
		this.renderData.Material.mainTexture = this.Texture;
		Vector3 zero = Vector3.zero;
		Vector3 vector = zero + Vector3.right * this.Size.x;
		Vector3 vector2 = vector + Vector3.down * this.Size.y;
		Vector3 vector3 = zero + Vector3.down * this.Size.y;
		this.renderData.Vertices.Add(zero);
		this.renderData.Vertices.Add(vector);
		this.renderData.Vertices.Add(vector2);
		this.renderData.Vertices.Add(vector3);
		this.renderData.Triangles.AddRange(dfMarkupBoxTexture.TRIANGLE_INDICES);
		this.renderData.UV.Add(new Vector2(0f, 1f));
		this.renderData.UV.Add(new Vector2(1f, 1f));
		this.renderData.UV.Add(new Vector2(1f, 0f));
		this.renderData.UV.Add(new Vector2(0f, 0f));
		Color color = this.Style.Color;
		this.renderData.Colors.Add(color);
		this.renderData.Colors.Add(color);
		this.renderData.Colors.Add(color);
		this.renderData.Colors.Add(color);
		return this.renderData;
	}

	// Token: 0x06001B5C RID: 7004 RVA: 0x000813EC File Offset: 0x0007F5EC
	private void ensureMaterial()
	{
		if (this.material != null || this.Texture == null)
		{
			return;
		}
		Shader shader = Shader.Find("Daikon Forge/Default UI Shader");
		if (shader == null)
		{
			Debug.LogError("Failed to find default shader");
			return;
		}
		this.material = new Material(shader)
		{
			name = "Default Texture Shader",
			hideFlags = HideFlags.DontSave,
			mainTexture = this.Texture
		};
	}

	// Token: 0x06001B5D RID: 7005 RVA: 0x0008146C File Offset: 0x0007F66C
	private static void addTriangleIndices(dfList<Vector3> verts, dfList<int> triangles)
	{
		int count = verts.Count;
		int[] triangle_INDICES = dfMarkupBoxTexture.TRIANGLE_INDICES;
		for (int i = 0; i < triangle_INDICES.Length; i++)
		{
			triangles.Add(count + triangle_INDICES[i]);
		}
	}

	// Token: 0x04001573 RID: 5491
	private static int[] TRIANGLE_INDICES = new int[] { 0, 1, 2, 0, 2, 3 };

	// Token: 0x04001575 RID: 5493
	private dfRenderData renderData = new dfRenderData();

	// Token: 0x04001576 RID: 5494
	private Material material;
}
