using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001717 RID: 5911
public class TileSpriteClipper : BraveBehaviour
{
	// Token: 0x0600894B RID: 35147 RVA: 0x0038F908 File Offset: 0x0038DB08
	private void Start()
	{
		this.DoClip();
	}

	// Token: 0x0600894C RID: 35148 RVA: 0x0038F910 File Offset: 0x0038DB10
	private void OnEnable()
	{
		this.Start();
	}

	// Token: 0x0600894D RID: 35149 RVA: 0x0038F918 File Offset: 0x0038DB18
	private void OnDisable()
	{
		if (base.sprite)
		{
			base.sprite.ForceBuild();
		}
	}

	// Token: 0x0600894E RID: 35150 RVA: 0x0038F938 File Offset: 0x0038DB38
	private void LateUpdate()
	{
		if (this.updateEveryFrame)
		{
			this.DoClip();
		}
		if (base.sprite && !base.sprite.attachParent)
		{
			base.sprite.UpdateZDepth();
		}
	}

	// Token: 0x0600894F RID: 35151 RVA: 0x0038F988 File Offset: 0x0038DB88
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06008950 RID: 35152 RVA: 0x0038F990 File Offset: 0x0038DB90
	public void DoClip()
	{
		if (this.clipMode == TileSpriteClipper.ClipMode.ClipBelowY)
		{
			this.ClipToY();
		}
		else
		{
			this.ClipToTileBounds();
		}
	}

	// Token: 0x06008951 RID: 35153 RVA: 0x0038F9B0 File Offset: 0x0038DBB0
	private void ClipToY()
	{
		if (BraveUtility.isLoadingLevel)
		{
			return;
		}
		if (GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		if (!base.sprite)
		{
			return;
		}
		Transform transform = base.transform;
		Bounds bounds = base.sprite.GetBounds();
		Vector2 vector = bounds.min.XY();
		Vector2 vector2 = bounds.max.XY();
		Vector3 vector3 = new Vector3(Mathf.Sign(transform.lossyScale.x), Mathf.Sign(transform.lossyScale.y), Mathf.Sign(transform.lossyScale.z));
		Vector2 vector4 = Vector2.Scale(vector3.XY(), vector);
		Vector2 vector5 = Vector2.Scale(vector3.XY(), vector2);
		vector = transform.position.XY() + Vector2.Min(vector4, vector5);
		vector2 = transform.position.XY() + Vector2.Max(vector4, vector5);
		tk2dSpriteDefinition tk2dSpriteDefinition = base.sprite.Collection.spriteDefinitions[base.sprite.spriteId];
		Vector2 vector6 = new Vector2(float.MaxValue, float.MaxValue);
		Vector2 vector7 = new Vector2(float.MinValue, float.MinValue);
		for (int i = 0; i < tk2dSpriteDefinition.uvs.Length; i++)
		{
			vector6 = Vector2.Min(vector6, tk2dSpriteDefinition.uvs[i]);
			vector7 = Vector2.Max(vector7, tk2dSpriteDefinition.uvs[i]);
		}
		if (this.m_vertices == null || this.m_vertices.Length != 4)
		{
			this.m_vertices = new Vector3[4];
		}
		if (this.m_triangles == null || this.m_triangles.Length != 6)
		{
			this.m_triangles = new int[6];
		}
		if (this.m_uvs == null || this.m_uvs.Length != 4)
		{
			this.m_uvs = new Vector2[4];
		}
		float num = vector.x - transform.position.x;
		float num2 = vector2.x - transform.position.x;
		float num3 = Mathf.Max(vector.y, Mathf.Min(this.clipY, vector2.y)) - transform.position.y;
		float num4 = vector2.y - transform.position.y;
		Vector3 vector8 = new Vector3(num, num3, 0f);
		Vector3 vector9 = new Vector3(num2, num3, 0f);
		Vector3 vector10 = new Vector3(num, num4, 0f);
		Vector3 vector11 = new Vector3(num2, num4, 0f);
		vector8 = Vector3.Scale(vector3, vector8);
		vector9 = Vector3.Scale(vector3, vector9);
		vector10 = Vector3.Scale(vector3, vector10);
		vector11 = Vector3.Scale(vector3, vector11);
		this.m_vertices[0] = vector8;
		this.m_vertices[1] = vector9;
		this.m_vertices[2] = vector10;
		this.m_vertices[3] = vector11;
		if (base.sprite.ShouldDoTilt)
		{
			for (int j = 0; j < 4; j++)
			{
				if (base.sprite.IsPerpendicular)
				{
					this.m_vertices[j] = this.m_vertices[j].WithZ(this.m_vertices[j].z - this.m_vertices[j].y);
				}
				else
				{
					this.m_vertices[j] = this.m_vertices[j].WithZ(this.m_vertices[j].z + this.m_vertices[j].y);
				}
			}
		}
		this.m_triangles[0] = 0;
		this.m_triangles[1] = 2;
		this.m_triangles[2] = 1;
		this.m_triangles[3] = 2;
		this.m_triangles[4] = 3;
		this.m_triangles[5] = 1;
		float num5 = (num + transform.position.x - vector.x) / (vector2.x - vector.x);
		float num6 = (num2 + transform.position.x - vector.x) / (vector2.x - vector.x);
		float num7 = (num3 + transform.position.y - vector.y) / (vector2.y - vector.y);
		float num8 = (num4 + transform.position.y - vector.y) / (vector2.y - vector.y);
		if (tk2dSpriteDefinition.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
		{
			Vector2 vector12 = new Vector2(Mathf.Lerp(vector6.x, vector7.x, num7), Mathf.Lerp(vector6.y, vector7.y, num5));
			Vector2 vector13 = new Vector2(Mathf.Lerp(vector6.x, vector7.x, num8), Mathf.Lerp(vector6.y, vector7.y, num6));
			this.m_uvs[0] = new Vector2(vector12.x, vector12.y);
			this.m_uvs[1] = new Vector2(vector12.x, vector13.y);
			this.m_uvs[2] = new Vector2(vector13.x, vector12.y);
			this.m_uvs[3] = new Vector2(vector13.x, vector13.y);
		}
		else
		{
			float num9 = Mathf.Lerp(vector6.x, vector7.x, num5);
			float num10 = Mathf.Lerp(vector6.x, vector7.x, num6);
			float num11 = Mathf.Lerp(vector6.y, vector7.y, num7);
			float num12 = Mathf.Lerp(vector6.y, vector7.y, num8);
			this.m_uvs[0] = new Vector2(num9, num11);
			this.m_uvs[1] = new Vector2(num10, num11);
			this.m_uvs[2] = new Vector2(num9, num12);
			this.m_uvs[3] = new Vector2(num10, num12);
		}
		MeshFilter component = base.GetComponent<MeshFilter>();
		Mesh mesh = component.mesh;
		if (mesh == null)
		{
			mesh = new Mesh();
		}
		else if (mesh.vertexCount != this.m_vertices.Length)
		{
			mesh.Clear(false);
		}
		mesh.vertices = this.m_vertices;
		mesh.triangles = this.m_triangles;
		mesh.uv = this.m_uvs;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		component.mesh = mesh;
	}

	// Token: 0x06008952 RID: 35154 RVA: 0x003900EC File Offset: 0x0038E2EC
	private void ClipToTileBounds()
	{
		if (BraveUtility.isLoadingLevel)
		{
			return;
		}
		if (GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		if (!base.sprite)
		{
			return;
		}
		Transform transform = base.transform;
		Bounds bounds = base.sprite.GetBounds();
		Vector2 vector = bounds.min.XY();
		Vector2 vector2 = bounds.max.XY();
		Vector3 vector3 = new Vector3(Mathf.Sign(transform.lossyScale.x), Mathf.Sign(transform.lossyScale.y), Mathf.Sign(transform.lossyScale.z));
		Vector2 vector4 = Vector2.Scale(vector3.XY(), vector);
		Vector2 vector5 = Vector2.Scale(vector3.XY(), vector2);
		vector = transform.position.XY() + Vector2.Min(vector4, vector5);
		vector2 = transform.position.XY() + Vector2.Max(vector4, vector5);
		IntVector2 intVector = vector.ToIntVector2(VectorConversions.Floor);
		IntVector2 intVector2 = vector2.ToIntVector2(VectorConversions.Floor);
		tk2dSpriteDefinition tk2dSpriteDefinition = base.sprite.Collection.spriteDefinitions[base.sprite.spriteId];
		Vector2 vector6 = new Vector2(float.MaxValue, float.MaxValue);
		Vector2 vector7 = new Vector2(float.MinValue, float.MinValue);
		for (int i = 0; i < tk2dSpriteDefinition.uvs.Length; i++)
		{
			vector6 = Vector2.Min(vector6, tk2dSpriteDefinition.uvs[i]);
			vector7 = Vector2.Max(vector7, tk2dSpriteDefinition.uvs[i]);
		}
		int num = (intVector2.x - intVector.x + 2) * (intVector2.y - intVector.y + 2) * 4 * 2;
		if (this.m_vertices != null && num / 2 > this.m_vertices.Length)
		{
			this.m_vertices = null;
			this.m_triangles = null;
			this.m_uvs = null;
		}
		if (this.m_vertices == null)
		{
			this.m_vertices = new Vector3[num];
		}
		if (this.m_triangles == null)
		{
			this.m_triangles = new int[this.m_vertices.Length / 4 * 6];
		}
		if (this.m_uvs == null)
		{
			this.m_uvs = new Vector2[num];
		}
		int num2 = 0;
		int num3 = 0;
		for (int j = intVector.x; j <= intVector2.x; j++)
		{
			for (int k = intVector.y; k <= intVector2.y; k++)
			{
				if (j >= 0 && j < GameManager.Instance.Dungeon.data.Width && k >= 0 && k < GameManager.Instance.Dungeon.data.Height)
				{
					CellData cellData = GameManager.Instance.Dungeon.data.cellData[j][k];
					if (cellData != null)
					{
						if (this.clipMode == TileSpriteClipper.ClipMode.GroundDecal)
						{
							if (cellData.type != CellType.FLOOR && !cellData.fallingPrevented)
							{
								goto IL_89D;
							}
							if (cellData.cellVisualData.floorType == CellVisualData.CellFloorType.Water)
							{
								goto IL_89D;
							}
						}
						else if (this.clipMode == TileSpriteClipper.ClipMode.WallEnterer)
						{
							if (cellData.type == CellType.WALL && !GameManager.Instance.Dungeon.data.isFaceWallLower(j, k))
							{
								goto IL_89D;
							}
						}
						else if (this.clipMode == TileSpriteClipper.ClipMode.PitBounds && (cellData.type != CellType.PIT || cellData.fallingPrevented))
						{
							goto IL_89D;
						}
						int num4 = num2;
						float num5 = Mathf.Max((float)j, vector.x) - transform.position.x;
						float num6 = Mathf.Min((float)(j + 1), vector2.x) - transform.position.x;
						float num7 = Mathf.Max((float)k, vector.y) - transform.position.y;
						float num8 = Mathf.Min((float)(k + 1), vector2.y) - transform.position.y;
						Vector3 vector8 = new Vector3(num5, num7, 0f);
						Vector3 vector9 = new Vector3(num6, num7, 0f);
						Vector3 vector10 = new Vector3(num5, num8, 0f);
						Vector3 vector11 = new Vector3(num6, num8, 0f);
						vector8 = Vector3.Scale(vector3, vector8);
						vector9 = Vector3.Scale(vector3, vector9);
						vector10 = Vector3.Scale(vector3, vector10);
						vector11 = Vector3.Scale(vector3, vector11);
						this.m_vertices[num2] = vector8;
						this.m_vertices[num2 + 1] = vector9;
						this.m_vertices[num2 + 2] = vector10;
						this.m_vertices[num2 + 3] = vector11;
						if (base.sprite.ShouldDoTilt)
						{
							for (int l = num2; l < num2 + 4; l++)
							{
								if (base.sprite.IsPerpendicular)
								{
									this.m_vertices[l] = this.m_vertices[l].WithZ(this.m_vertices[l].z - this.m_vertices[l].y);
								}
								else
								{
									this.m_vertices[l] = this.m_vertices[l].WithZ(this.m_vertices[l].z + this.m_vertices[l].y);
								}
							}
						}
						this.m_triangles[num3] = num4;
						this.m_triangles[num3 + 1] = num4 + 2;
						this.m_triangles[num3 + 2] = num4 + 1;
						this.m_triangles[num3 + 3] = num4 + 2;
						this.m_triangles[num3 + 4] = num4 + 3;
						this.m_triangles[num3 + 5] = num4 + 1;
						float num9 = (num5 + transform.position.x - vector.x) / (vector2.x - vector.x);
						float num10 = (num6 + transform.position.x - vector.x) / (vector2.x - vector.x);
						float num11 = (num7 + transform.position.y - vector.y) / (vector2.y - vector.y);
						float num12 = (num8 + transform.position.y - vector.y) / (vector2.y - vector.y);
						if (tk2dSpriteDefinition.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
						{
							Vector2 vector12 = new Vector2(Mathf.Lerp(vector6.x, vector7.x, num11), Mathf.Lerp(vector6.y, vector7.y, num9));
							Vector2 vector13 = new Vector2(Mathf.Lerp(vector6.x, vector7.x, num12), Mathf.Lerp(vector6.y, vector7.y, num10));
							this.m_uvs[num2] = new Vector2(vector12.x, vector12.y);
							this.m_uvs[num2 + 1] = new Vector2(vector12.x, vector13.y);
							this.m_uvs[num2 + 2] = new Vector2(vector13.x, vector12.y);
							this.m_uvs[num2 + 3] = new Vector2(vector13.x, vector13.y);
						}
						else
						{
							float num13 = Mathf.Lerp(vector6.x, vector7.x, num9);
							float num14 = Mathf.Lerp(vector6.x, vector7.x, num10);
							float num15 = Mathf.Lerp(vector6.y, vector7.y, num11);
							float num16 = Mathf.Lerp(vector6.y, vector7.y, num12);
							this.m_uvs[num2] = new Vector2(num13, num15);
							this.m_uvs[num2 + 1] = new Vector2(num14, num15);
							this.m_uvs[num2 + 2] = new Vector2(num13, num16);
							this.m_uvs[num2 + 3] = new Vector2(num14, num16);
						}
						num2 += 4;
						num3 += 6;
					}
				}
				IL_89D:;
			}
		}
		for (int m = num2; m < this.m_vertices.Length; m++)
		{
			this.m_vertices[m] = Vector3.zero;
			this.m_uvs[m] = Vector2.zero;
		}
		for (int n = num3; n < this.m_triangles.Length; n++)
		{
			this.m_triangles[n] = 0;
		}
		MeshFilter component = base.GetComponent<MeshFilter>();
		Mesh mesh = component.mesh;
		if (mesh == null)
		{
			mesh = new Mesh();
		}
		mesh.vertices = this.m_vertices;
		mesh.triangles = this.m_triangles;
		mesh.uv = this.m_uvs;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		component.mesh = mesh;
	}

	// Token: 0x04008F50 RID: 36688
	public bool doOptimize = true;

	// Token: 0x04008F51 RID: 36689
	public bool updateEveryFrame;

	// Token: 0x04008F52 RID: 36690
	public TileSpriteClipper.ClipMode clipMode;

	// Token: 0x04008F53 RID: 36691
	[ShowInInspectorIf("clipMode", 3, false)]
	public float clipY;

	// Token: 0x04008F54 RID: 36692
	private Vector3[] m_vertices;

	// Token: 0x04008F55 RID: 36693
	private int[] m_triangles;

	// Token: 0x04008F56 RID: 36694
	private Vector2[] m_uvs;

	// Token: 0x02001718 RID: 5912
	public enum ClipMode
	{
		// Token: 0x04008F58 RID: 36696
		GroundDecal,
		// Token: 0x04008F59 RID: 36697
		WallEnterer,
		// Token: 0x04008F5A RID: 36698
		PitBounds,
		// Token: 0x04008F5B RID: 36699
		ClipBelowY
	}
}
