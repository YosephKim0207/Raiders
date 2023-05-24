using System;
using UnityEngine;

// Token: 0x02000E5B RID: 3675
public class CustomTrailRenderer : BraveBehaviour
{
	// Token: 0x06004E44 RID: 20036 RVA: 0x001B08F4 File Offset: 0x001AEAF4
	public void Awake()
	{
		this.m_cachedMaxAngle = this.maxAngle;
		this.m_cachedMaxVertexDistance = this.maxVertexDistance;
		this.m_cachedOptimizeCount = this.optimizeCount;
	}

	// Token: 0x06004E45 RID: 20037 RVA: 0x001B091C File Offset: 0x001AEB1C
	public void Start()
	{
		MeshFilter meshFilter = base.gameObject.AddComponent<MeshFilter>();
		if (!meshFilter)
		{
			return;
		}
		this.mesh = meshFilter.mesh;
		base.renderer = base.gameObject.AddComponent<MeshRenderer>();
		if (!base.renderer)
		{
			return;
		}
		this.instanceMaterial = new Material(this.material);
		base.renderer.material = this.instanceMaterial;
		if (this.specRigidbody && PhysicsEngine.HasInstance)
		{
			PhysicsEngine.Instance.OnPostRigidbodyMovement += this.OnPostRigidbodyMovement;
		}
	}

	// Token: 0x06004E46 RID: 20038 RVA: 0x001B09C4 File Offset: 0x001AEBC4
	public void Update()
	{
		if (!this.specRigidbody)
		{
			this.UpdateMesh();
		}
	}

	// Token: 0x06004E47 RID: 20039 RVA: 0x001B09DC File Offset: 0x001AEBDC
	private void OnPostRigidbodyMovement()
	{
		if (this.specRigidbody)
		{
			this.UpdateMesh();
		}
	}

	// Token: 0x06004E48 RID: 20040 RVA: 0x001B09F4 File Offset: 0x001AEBF4
	public void Reenable()
	{
		this.emit = true;
		this.emittingDone = false;
		if (base.renderer)
		{
			base.renderer.enabled = true;
		}
		this.maxAngle = this.m_cachedMaxAngle;
		this.maxVertexDistance = this.m_cachedMaxVertexDistance;
		this.optimizeCount = this.m_cachedOptimizeCount;
	}

	// Token: 0x06004E49 RID: 20041 RVA: 0x001B0A50 File Offset: 0x001AEC50
	public void Clear()
	{
		for (int i = this.numPoints - 1; i >= 0; i--)
		{
			this.points[i] = null;
		}
		this.numPoints = 0;
		if (this.mesh)
		{
			this.mesh.Clear();
		}
	}

	// Token: 0x06004E4A RID: 20042 RVA: 0x001B0AA4 File Offset: 0x001AECA4
	private void UpdateMesh()
	{
		if (this.specRigidbody && this.specRigidbody.transform.rotation.eulerAngles.z != 0f)
		{
			base.transform.localRotation = Quaternion.Euler(0f, 0f, -this.specRigidbody.transform.rotation.eulerAngles.z);
		}
		if (!this.emit)
		{
			this.emittingDone = true;
		}
		if (this.emittingDone)
		{
			this.emit = false;
		}
		int num = 0;
		for (int i = this.numPoints - 1; i >= 0; i--)
		{
			CustomTrailRenderer.Point point = this.points[i];
			if (point != null && point.timeAlive < this.lifeTime)
			{
				break;
			}
			num++;
		}
		if (num > 1)
		{
			int num2 = this.numPoints - num + 1;
			while (this.numPoints > num2)
			{
				this.points[this.numPoints - 1] = null;
				this.numPoints--;
			}
		}
		if (this.numPoints > this.optimizeCount)
		{
			this.maxAngle += this.optimizeAngleInterval;
			this.maxVertexDistance += this.optimizeDistanceInterval;
			this.optimizeCount++;
		}
		if (this.emit)
		{
			if (this.numPoints == 0)
			{
				this.points[this.numPoints++] = new CustomTrailRenderer.Point(base.transform);
				this.points[this.numPoints++] = new CustomTrailRenderer.Point(base.transform);
			}
			if (this.numPoints == 1)
			{
				this.InsertPoint();
			}
			bool flag = false;
			float sqrMagnitude = (this.points[1].position - base.transform.position).sqrMagnitude;
			if (sqrMagnitude > this.minVertexDistance * this.minVertexDistance)
			{
				if (sqrMagnitude > this.maxVertexDistance * this.maxVertexDistance)
				{
					flag = true;
				}
				else if (Quaternion.Angle(base.transform.rotation, this.points[1].rotation) > this.maxAngle)
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (this.numPoints == this.points.Length)
				{
					Array.Resize<CustomTrailRenderer.Point>(ref this.points, this.points.Length + 50);
				}
				this.InsertPoint();
			}
			else
			{
				this.points[0].Update(base.transform);
			}
		}
		if (this.numPoints < 2)
		{
			base.renderer.enabled = false;
			return;
		}
		base.renderer.enabled = true;
		this.lifeTimeRatio = ((this.lifeTime != 0f) ? (1f / this.lifeTime) : 0f);
		if (this.emit)
		{
			Vector3[] array = new Vector3[this.numPoints * 2];
			Vector2[] array2 = new Vector2[this.numPoints * 2];
			int[] array3 = new int[(this.numPoints - 1) * 6];
			Color[] array4 = new Color[this.numPoints * 2];
			float num3 = 1f / (this.points[this.numPoints - 1].timeAlive - this.points[0].timeAlive);
			for (int j = 0; j < this.numPoints; j++)
			{
				CustomTrailRenderer.Point point2 = this.points[j];
				float num4 = point2.timeAlive * this.lifeTimeRatio;
				Vector3 vector;
				if (j == 0 && this.numPoints > 1)
				{
					vector = this.points[j + 1].position - this.points[j].position;
				}
				else if (j == this.numPoints - 1 && this.numPoints > 1)
				{
					vector = this.points[j].position - this.points[j - 1].position;
				}
				else if (this.numPoints > 2)
				{
					vector = (this.points[j + 1].position - this.points[j].position + (this.points[j].position - this.points[j - 1].position)) * 0.5f;
				}
				else
				{
					vector = Vector3.right;
				}
				Color color;
				if (this.colors.Length == 0)
				{
					color = Color.Lerp(Color.white, Color.clear, num4);
				}
				else if (this.colors.Length == 1)
				{
					color = Color.Lerp(this.colors[0], Color.clear, num4);
				}
				else if (this.colors.Length == 2)
				{
					color = Color.Lerp(this.colors[0], this.colors[1], num4);
				}
				else if (num4 <= 0f)
				{
					color = this.colors[0];
				}
				else if (num4 >= 1f)
				{
					color = this.colors[this.colors.Length - 1];
				}
				else
				{
					float num5 = num4 * (float)(this.colors.Length - 1);
					int num6 = Mathf.Min(this.colors.Length - 2, (int)Mathf.Floor(num5));
					float num7 = Mathf.InverseLerp((float)num6, (float)(num6 + 1), num5);
					if (num6 < 0 || num6 + 1 >= this.colors.Length)
					{
						Debug.LogFormat("{0}, {1}, {2}, {3}", new object[]
						{
							num5,
							num6,
							num7,
							num6 + 1
						});
					}
					color = Color.Lerp(this.colors[num6], this.colors[num6 + 1], num7);
				}
				array4[j * 2] = color;
				array4[j * 2 + 1] = color;
				Vector3 vector2 = point2.position;
				if (j > 0 && j == this.numPoints - 1)
				{
					float num8 = Mathf.InverseLerp(this.points[j - 1].timeAlive, point2.timeAlive, this.lifeTime);
					vector2 = Vector3.Lerp(this.points[j - 1].position, point2.position, num8);
				}
				float num9;
				if (this.widths.Length == 0)
				{
					num9 = 1f;
				}
				else if (this.widths.Length == 1)
				{
					num9 = this.widths[0];
				}
				else if (this.widths.Length == 2)
				{
					num9 = Mathf.Lerp(this.widths[0], this.widths[1], num4);
				}
				else if (num4 <= 0f)
				{
					num9 = this.widths[0];
				}
				else if (num4 >= 1f)
				{
					num9 = this.widths[this.widths.Length - 1];
				}
				else
				{
					float num10 = num4 * (float)(this.widths.Length - 1);
					int num11 = (int)Mathf.Floor(num10);
					float num12 = Mathf.InverseLerp((float)num11, (float)(num11 + 1), num10);
					num9 = Mathf.Lerp(this.widths[num11], this.widths[num11 + 1], num12);
				}
				vector = vector.normalized.RotateBy(Quaternion.Euler(0f, 0f, 90f)) * 0.5f * num9;
				array[j * 2] = vector2 - base.transform.position + vector;
				array[j * 2 + 1] = vector2 - base.transform.position - vector;
				float num13 = (point2.timeAlive - this.points[0].timeAlive) * num3;
				array2[j * 2] = new Vector2(num13, 0f);
				array2[j * 2 + 1] = new Vector2(num13, 1f);
				if (j > 0)
				{
					int num14 = (j - 1) * 6;
					int num15 = j * 2;
					array3[num14] = num15 - 2;
					array3[num14 + 1] = num15 - 1;
					array3[num14 + 2] = num15;
					array3[num14 + 3] = num15 + 1;
					array3[num14 + 4] = num15;
					array3[num14 + 5] = num15 - 1;
				}
			}
			this.mesh.Clear();
			this.mesh.vertices = array;
			this.mesh.colors = array4;
			this.mesh.uv = array2;
			this.mesh.triangles = array3;
			return;
		}
		if (this.numPoints == 0)
		{
			return;
		}
	}

	// Token: 0x06004E4B RID: 20043 RVA: 0x001B13E4 File Offset: 0x001AF5E4
	private void InsertPoint()
	{
		for (int i = this.numPoints; i > 0; i--)
		{
			this.points[i] = this.points[i - 1];
		}
		this.points[0] = new CustomTrailRenderer.Point(base.transform);
		this.numPoints++;
	}

	// Token: 0x0400449B RID: 17563
	public new SpeculativeRigidbody specRigidbody;

	// Token: 0x0400449C RID: 17564
	public Material material;

	// Token: 0x0400449D RID: 17565
	private Material instanceMaterial;

	// Token: 0x0400449E RID: 17566
	public bool emit = true;

	// Token: 0x0400449F RID: 17567
	private bool emittingDone;

	// Token: 0x040044A0 RID: 17568
	public float lifeTime = 1f;

	// Token: 0x040044A1 RID: 17569
	private float lifeTimeRatio = 1f;

	// Token: 0x040044A2 RID: 17570
	public Color[] colors;

	// Token: 0x040044A3 RID: 17571
	public float[] widths;

	// Token: 0x040044A4 RID: 17572
	public float maxAngle = 2f;

	// Token: 0x040044A5 RID: 17573
	public float minVertexDistance = 0.1f;

	// Token: 0x040044A6 RID: 17574
	public float maxVertexDistance = 1f;

	// Token: 0x040044A7 RID: 17575
	public float optimizeAngleInterval = 0.1f;

	// Token: 0x040044A8 RID: 17576
	public float optimizeDistanceInterval = 0.05f;

	// Token: 0x040044A9 RID: 17577
	public int optimizeCount = 30;

	// Token: 0x040044AA RID: 17578
	private Mesh mesh;

	// Token: 0x040044AB RID: 17579
	private CustomTrailRenderer.Point[] points = new CustomTrailRenderer.Point[100];

	// Token: 0x040044AC RID: 17580
	private int numPoints;

	// Token: 0x040044AD RID: 17581
	private float m_cachedMaxAngle;

	// Token: 0x040044AE RID: 17582
	private float m_cachedMaxVertexDistance;

	// Token: 0x040044AF RID: 17583
	private int m_cachedOptimizeCount;

	// Token: 0x02000E5C RID: 3676
	private class Point
	{
		// Token: 0x06004E4C RID: 20044 RVA: 0x001B143C File Offset: 0x001AF63C
		public Point(Transform trans)
		{
			this.position = trans.position;
			this.rotation = trans.rotation;
			this.timeCreated = BraveTime.ScaledTimeSinceStartup;
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06004E4D RID: 20045 RVA: 0x001B1488 File Offset: 0x001AF688
		public float timeAlive
		{
			get
			{
				return BraveTime.ScaledTimeSinceStartup - this.timeCreated;
			}
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x001B1498 File Offset: 0x001AF698
		public void Update(Transform trans)
		{
			this.position = trans.position;
			this.rotation = trans.rotation;
			this.timeCreated = BraveTime.ScaledTimeSinceStartup;
		}

		// Token: 0x040044B0 RID: 17584
		public float timeCreated;

		// Token: 0x040044B1 RID: 17585
		public float fadeAlpha;

		// Token: 0x040044B2 RID: 17586
		public Vector3 position = Vector3.zero;

		// Token: 0x040044B3 RID: 17587
		public Quaternion rotation = Quaternion.identity;
	}
}
