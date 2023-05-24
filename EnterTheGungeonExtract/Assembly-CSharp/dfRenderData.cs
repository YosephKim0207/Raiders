using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003ED RID: 1005
public class dfRenderData : IDisposable
{
	// Token: 0x06001544 RID: 5444 RVA: 0x00062E2C File Offset: 0x0006102C
	internal dfRenderData()
		: this(32)
	{
	}

	// Token: 0x06001545 RID: 5445 RVA: 0x00062E38 File Offset: 0x00061038
	internal dfRenderData(int capacity)
	{
		this.Vertices = new dfList<Vector3>(capacity);
		this.Triangles = new dfList<int>(capacity);
		this.Normals = new dfList<Vector3>(capacity);
		this.Tangents = new dfList<Vector4>(capacity);
		this.UV = new dfList<Vector2>(capacity);
		this.Colors = new dfList<Color32>(capacity);
		this.Transform = Matrix4x4.identity;
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x00062EA0 File Offset: 0x000610A0
	public static dfRenderData Obtain()
	{
		object obj = dfRenderData.pool;
		dfRenderData dfRenderData;
		lock (obj)
		{
			dfRenderData = ((dfRenderData.pool.Count <= 0) ? new dfRenderData() : dfRenderData.pool.Dequeue());
		}
		return dfRenderData;
	}

	// Token: 0x06001547 RID: 5447 RVA: 0x00062EFC File Offset: 0x000610FC
	public static void FlushObjectPool()
	{
		object obj = dfRenderData.pool;
		lock (obj)
		{
			while (dfRenderData.pool.Count > 0)
			{
				dfRenderData dfRenderData = dfRenderData.pool.Dequeue();
				dfRenderData.Vertices.TrimExcess();
				dfRenderData.Triangles.TrimExcess();
				dfRenderData.UV.TrimExcess();
				dfRenderData.Colors.TrimExcess();
			}
		}
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x00062F80 File Offset: 0x00061180
	public void Release()
	{
		object obj = dfRenderData.pool;
		lock (obj)
		{
			this.Clear();
			dfRenderData.pool.Enqueue(this);
		}
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x00062FC8 File Offset: 0x000611C8
	public void Clear()
	{
		this.Material = null;
		this.Shader = null;
		this.Transform = Matrix4x4.identity;
		this.Checksum = 0U;
		this.Intersection = dfIntersectionType.None;
		this.Vertices.Clear();
		this.UV.Clear();
		this.Triangles.Clear();
		this.Colors.Clear();
		this.Normals.Clear();
		this.Tangents.Clear();
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x00063040 File Offset: 0x00061240
	public bool IsValid()
	{
		int count = this.Vertices.Count;
		return count > 0 && count <= 65000 && this.UV.Count == count && this.Colors.Count == count;
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x00063090 File Offset: 0x00061290
	public void EnsureCapacity(int capacity)
	{
		this.Vertices.EnsureCapacity(capacity);
		this.Triangles.EnsureCapacity(Mathf.RoundToInt((float)capacity * 1.5f));
		this.UV.EnsureCapacity(capacity);
		this.Colors.EnsureCapacity(capacity);
		if (this.Normals != null)
		{
			this.Normals.EnsureCapacity(capacity);
		}
		if (this.Tangents != null)
		{
			this.Tangents.EnsureCapacity(capacity);
		}
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x00063108 File Offset: 0x00061308
	public void Merge(dfRenderData buffer)
	{
		this.Merge(buffer, true);
	}

	// Token: 0x0600154D RID: 5453 RVA: 0x00063114 File Offset: 0x00061314
	public void Merge(dfRenderData buffer, bool transformVertices)
	{
		int count = this.Vertices.Count;
		this.Vertices.AddRange(buffer.Vertices);
		if (transformVertices)
		{
			Vector3[] items = this.Vertices.Items;
			int count2 = buffer.Vertices.Count;
			Matrix4x4 transform = buffer.Transform;
			for (int i = count; i < count + count2; i++)
			{
				items[i] = transform.MultiplyPoint(items[i]);
			}
		}
		int count3 = this.Triangles.Count;
		this.Triangles.AddRange(buffer.Triangles);
		int count4 = buffer.Triangles.Count;
		int[] items2 = this.Triangles.Items;
		for (int j = count3; j < count3 + count4; j++)
		{
			items2[j] += count;
		}
		this.UV.AddRange(buffer.UV);
		this.Colors.AddRange(buffer.Colors);
		this.Normals.AddRange(buffer.Normals);
		this.Tangents.AddRange(buffer.Tangents);
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x00063244 File Offset: 0x00061444
	internal void ApplyTransform(Matrix4x4 transform)
	{
		int count = this.Vertices.Count;
		Vector3[] items = this.Vertices.Items;
		for (int i = 0; i < count; i++)
		{
			items[i] = transform.MultiplyPoint(items[i]);
		}
		if (this.Normals.Count > 0)
		{
			Vector3[] items2 = this.Normals.Items;
			for (int j = 0; j < count; j++)
			{
				items2[j] = transform.MultiplyVector(items2[j]);
			}
		}
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x000632F0 File Offset: 0x000614F0
	public override string ToString()
	{
		return string.Format("V:{0} T:{1} U:{2} C:{3}", new object[]
		{
			this.Vertices.Count,
			this.Triangles.Count,
			this.UV.Count,
			this.Colors.Count
		});
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x0006335C File Offset: 0x0006155C
	public void Dispose()
	{
		this.Release();
	}

	// Token: 0x04001219 RID: 4633
	private static Queue<dfRenderData> pool = new Queue<dfRenderData>();

	// Token: 0x0400121A RID: 4634
	public Material Material;

	// Token: 0x0400121B RID: 4635
	public Shader Shader;

	// Token: 0x0400121C RID: 4636
	public Matrix4x4 Transform;

	// Token: 0x0400121D RID: 4637
	public dfList<Vector3> Vertices;

	// Token: 0x0400121E RID: 4638
	public dfList<Vector2> UV;

	// Token: 0x0400121F RID: 4639
	public dfList<Vector3> Normals;

	// Token: 0x04001220 RID: 4640
	public dfList<Vector4> Tangents;

	// Token: 0x04001221 RID: 4641
	public dfList<int> Triangles;

	// Token: 0x04001222 RID: 4642
	public dfList<Color32> Colors;

	// Token: 0x04001223 RID: 4643
	public uint Checksum;

	// Token: 0x04001224 RID: 4644
	public dfIntersectionType Intersection;

	// Token: 0x04001225 RID: 4645
	public bool Glitchy;
}
