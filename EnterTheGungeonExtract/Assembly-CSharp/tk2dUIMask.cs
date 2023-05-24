using System;
using UnityEngine;

// Token: 0x02000C1F RID: 3103
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIMask")]
[RequireComponent(typeof(MeshFilter))]
public class tk2dUIMask : MonoBehaviour
{
	// Token: 0x17000A1D RID: 2589
	// (get) Token: 0x060042D1 RID: 17105 RVA: 0x0015A2E0 File Offset: 0x001584E0
	private MeshFilter ThisMeshFilter
	{
		get
		{
			if (this._thisMeshFilter == null)
			{
				this._thisMeshFilter = base.GetComponent<MeshFilter>();
			}
			return this._thisMeshFilter;
		}
	}

	// Token: 0x17000A1E RID: 2590
	// (get) Token: 0x060042D2 RID: 17106 RVA: 0x0015A308 File Offset: 0x00158508
	private BoxCollider ThisBoxCollider
	{
		get
		{
			if (this._thisBoxCollider == null)
			{
				this._thisBoxCollider = base.GetComponent<BoxCollider>();
			}
			return this._thisBoxCollider;
		}
	}

	// Token: 0x060042D3 RID: 17107 RVA: 0x0015A330 File Offset: 0x00158530
	private void Awake()
	{
		this.Build();
	}

	// Token: 0x060042D4 RID: 17108 RVA: 0x0015A338 File Offset: 0x00158538
	private void OnDestroy()
	{
		if (this.ThisMeshFilter.sharedMesh != null)
		{
			UnityEngine.Object.Destroy(this.ThisMeshFilter.sharedMesh);
		}
	}

	// Token: 0x060042D5 RID: 17109 RVA: 0x0015A360 File Offset: 0x00158560
	private Mesh FillMesh(Mesh mesh)
	{
		Vector3 zero = Vector3.zero;
		switch (this.anchor)
		{
		case tk2dBaseSprite.Anchor.LowerLeft:
			zero = new Vector3(0f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.LowerCenter:
			zero = new Vector3(-this.size.x / 2f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
			zero = new Vector3(-this.size.x, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
			zero = new Vector3(0f, -this.size.y / 2f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleCenter:
			zero = new Vector3(-this.size.x / 2f, -this.size.y / 2f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleRight:
			zero = new Vector3(-this.size.x, -this.size.y / 2f, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
			zero = new Vector3(0f, -this.size.y, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperCenter:
			zero = new Vector3(-this.size.x / 2f, -this.size.y, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperRight:
			zero = new Vector3(-this.size.x, -this.size.y, 0f);
			break;
		}
		Vector3[] array = new Vector3[]
		{
			zero + new Vector3(0f, 0f, -this.depth),
			zero + new Vector3(this.size.x, 0f, -this.depth),
			zero + new Vector3(0f, this.size.y, -this.depth),
			zero + new Vector3(this.size.x, this.size.y, -this.depth)
		};
		mesh.vertices = array;
		mesh.uv = tk2dUIMask.uv;
		mesh.triangles = tk2dUIMask.indices;
		Bounds bounds = default(Bounds);
		bounds.SetMinMax(zero, zero + new Vector3(this.size.x, this.size.y, 0f));
		mesh.bounds = bounds;
		return mesh;
	}

	// Token: 0x060042D6 RID: 17110 RVA: 0x0015A630 File Offset: 0x00158830
	private void OnDrawGizmosSelected()
	{
		Mesh sharedMesh = this.ThisMeshFilter.sharedMesh;
		if (sharedMesh != null)
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Bounds bounds = sharedMesh.bounds;
			Gizmos.color = new Color32(56, 146, 227, 96);
			float num = -this.depth * 1.001f;
			Vector3 vector = new Vector3(bounds.center.x, bounds.center.y, num * 0.5f);
			Vector3 vector2 = new Vector3(bounds.extents.x * 2f, bounds.extents.y * 2f, Mathf.Abs(num));
			Gizmos.DrawCube(vector, vector2);
			Gizmos.color = new Color32(22, 145, byte.MaxValue, byte.MaxValue);
			Gizmos.DrawWireCube(vector, vector2);
		}
	}

	// Token: 0x060042D7 RID: 17111 RVA: 0x0015A730 File Offset: 0x00158930
	public void Build()
	{
		if (this.ThisMeshFilter.sharedMesh == null)
		{
			Mesh mesh = new Mesh();
			mesh.hideFlags = HideFlags.DontSave;
			this.ThisMeshFilter.mesh = this.FillMesh(mesh);
		}
		else
		{
			this.FillMesh(this.ThisMeshFilter.sharedMesh);
		}
		if (this.createBoxCollider)
		{
			if (this.ThisBoxCollider == null)
			{
				this._thisBoxCollider = base.gameObject.AddComponent<BoxCollider>();
			}
			Bounds bounds = this.ThisMeshFilter.sharedMesh.bounds;
			this.ThisBoxCollider.center = new Vector3(bounds.center.x, bounds.center.y, -this.depth);
			this.ThisBoxCollider.size = new Vector3(bounds.size.x, bounds.size.y, 0.0002f);
		}
		else if (this.ThisBoxCollider != null)
		{
			UnityEngine.Object.Destroy(this.ThisBoxCollider);
		}
	}

	// Token: 0x060042D8 RID: 17112 RVA: 0x0015A854 File Offset: 0x00158A54
	public void ReshapeBounds(Vector3 dMin, Vector3 dMax)
	{
		Vector3 vector = new Vector3(this.size.x, this.size.y);
		Vector3 vector2 = Vector3.zero;
		switch (this.anchor)
		{
		case tk2dBaseSprite.Anchor.LowerLeft:
			vector2.Set(0f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.LowerCenter:
			vector2.Set(0.5f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
			vector2.Set(1f, 0f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
			vector2.Set(0f, 0.5f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleCenter:
			vector2.Set(0.5f, 0.5f, 0f);
			break;
		case tk2dBaseSprite.Anchor.MiddleRight:
			vector2.Set(1f, 0.5f, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
			vector2.Set(0f, 1f, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperCenter:
			vector2.Set(0.5f, 1f, 0f);
			break;
		case tk2dBaseSprite.Anchor.UpperRight:
			vector2.Set(1f, 1f, 0f);
			break;
		}
		vector2 = Vector3.Scale(vector2, vector) * -1f;
		Vector3 vector3 = vector + dMax - dMin;
		Vector3 vector4 = new Vector3((!Mathf.Approximately(vector.x, 0f)) ? (vector2.x * vector3.x / vector.x) : 0f, (!Mathf.Approximately(vector.y, 0f)) ? (vector2.y * vector3.y / vector.y) : 0f);
		Vector3 vector5 = vector2 + dMin - vector4;
		vector5.z = 0f;
		base.transform.position = base.transform.TransformPoint(vector5);
		this.size = new Vector2(vector3.x, vector3.y);
		this.Build();
	}

	// Token: 0x04003517 RID: 13591
	public tk2dBaseSprite.Anchor anchor = tk2dBaseSprite.Anchor.MiddleCenter;

	// Token: 0x04003518 RID: 13592
	public Vector2 size = new Vector2(1f, 1f);

	// Token: 0x04003519 RID: 13593
	public float depth = 1f;

	// Token: 0x0400351A RID: 13594
	public bool createBoxCollider = true;

	// Token: 0x0400351B RID: 13595
	private MeshFilter _thisMeshFilter;

	// Token: 0x0400351C RID: 13596
	private BoxCollider _thisBoxCollider;

	// Token: 0x0400351D RID: 13597
	private static readonly Vector2[] uv = new Vector2[]
	{
		new Vector2(0f, 0f),
		new Vector2(1f, 0f),
		new Vector2(0f, 1f),
		new Vector2(1f, 1f)
	};

	// Token: 0x0400351E RID: 13598
	private static readonly int[] indices = new int[] { 0, 3, 1, 2, 3, 0 };
}
