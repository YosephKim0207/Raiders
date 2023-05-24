using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001538 RID: 5432
public class PlayerLightController : MonoBehaviour
{
	// Token: 0x06007C4F RID: 31823 RVA: 0x003204A8 File Offset: 0x0031E6A8
	private void Start()
	{
		this.mf = base.GetComponent<MeshFilter>();
		if (this.mf == null)
		{
			this.mf = base.gameObject.AddComponent<MeshFilter>();
		}
		this.mr = base.GetComponent<MeshRenderer>();
		if (this.mr == null)
		{
			this.mr = base.gameObject.AddComponent<MeshRenderer>();
		}
		this.vertices = new List<Vector3>();
		this.triangles = new List<int>();
		this.uvs = new List<Vector2>();
		this.directionCache = new Vector3[this.resolution];
		this.CacheDirections();
		this.UpdateVertices(true);
		this.m = new Mesh();
		this.m.vertices = this.vertices.ToArray();
		this.m.triangles = this.triangles.ToArray();
		this.m.uv = this.uvs.ToArray();
		this.m.RecalculateBounds();
		this.m.RecalculateNormals();
		this.mf.sharedMesh = this.m;
		this.mr.material = this.shadowMaterial;
	}

	// Token: 0x06007C50 RID: 31824 RVA: 0x003205D8 File Offset: 0x0031E7D8
	private void CacheDirections()
	{
		for (int i = 0; i < this.resolution; i++)
		{
			float num = (float)i * (360f / (float)this.resolution);
			Vector3 vector = Quaternion.Euler(0f, 0f, num) * Vector3.up;
			this.directionCache[i] = vector.normalized;
		}
	}

	// Token: 0x06007C51 RID: 31825 RVA: 0x00320640 File Offset: 0x0031E840
	private void UpdateVertices(bool generateTrisAndUVs)
	{
		this.vertices.Clear();
		if (generateTrisAndUVs)
		{
			this.triangles.Clear();
			this.uvs.Clear();
		}
		for (int i = 0; i < this.resolution; i++)
		{
			Ray ray = new Ray(base.transform.position, this.directionCache[i]);
			RaycastHit raycastHit = default(RaycastHit);
			Vector3 vector;
			Vector3 vector2;
			float num;
			if (Physics.Raycast(ray, out raycastHit, this.maxDistance, this.layerMask))
			{
				vector = raycastHit.point;
				vector2 = ray.GetPoint(this.maxDistance + 1f);
				num = Mathf.Max(raycastHit.distance / this.maxDistance, 0.5f);
				num = Mathf.Clamp01(1f - num);
			}
			else
			{
				vector = ray.GetPoint(this.maxDistance);
				vector2 = ray.GetPoint(this.maxDistance + 1f);
				num = 0f;
			}
			this.vertices.Add(base.transform.InverseTransformPoint(vector) + this.directionCache[i] * (this.distortionMax * num));
			this.vertices.Add(base.transform.InverseTransformPoint(vector2));
			if (generateTrisAndUVs)
			{
				if (i > 1)
				{
					this.triangles.Add(i * 2 - 1);
					this.triangles.Add(i * 2 - 2);
					this.triangles.Add(i * 2);
					this.triangles.Add(i * 2);
					this.triangles.Add(i * 2 + 1);
					this.triangles.Add(i * 2 - 1);
				}
				this.uvs.Add(Vector2.zero);
				this.uvs.Add(Vector2.zero);
			}
		}
		if (generateTrisAndUVs)
		{
			this.triangles.Add(this.vertices.Count - 1);
			this.triangles.Add(this.vertices.Count - 2);
			this.triangles.Add(0);
			this.triangles.Add(0);
			this.triangles.Add(1);
			this.triangles.Add(this.vertices.Count - 1);
		}
	}

	// Token: 0x06007C52 RID: 31826 RVA: 0x00320894 File Offset: 0x0031EA94
	private void LateUpdate()
	{
		this.UpdateVertices(false);
		this.m.vertices = this.vertices.ToArray();
	}

	// Token: 0x04007F3B RID: 32571
	public int resolution = 1000;

	// Token: 0x04007F3C RID: 32572
	public float maxDistance = 10f;

	// Token: 0x04007F3D RID: 32573
	public float distortionMax = 0.5f;

	// Token: 0x04007F3E RID: 32574
	public Material shadowMaterial;

	// Token: 0x04007F3F RID: 32575
	private MeshFilter mf;

	// Token: 0x04007F40 RID: 32576
	private MeshRenderer mr;

	// Token: 0x04007F41 RID: 32577
	private Mesh m;

	// Token: 0x04007F42 RID: 32578
	private List<Vector3> vertices;

	// Token: 0x04007F43 RID: 32579
	private List<int> triangles;

	// Token: 0x04007F44 RID: 32580
	private List<Vector2> uvs;

	// Token: 0x04007F45 RID: 32581
	private Vector3[] directionCache;

	// Token: 0x04007F46 RID: 32582
	private int layerMask = -1025;
}
