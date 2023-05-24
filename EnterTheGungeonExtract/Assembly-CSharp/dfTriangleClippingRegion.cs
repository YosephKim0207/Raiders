using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000394 RID: 916
internal class dfTriangleClippingRegion : IDisposable
{
	// Token: 0x06001014 RID: 4116 RVA: 0x0004AFA4 File Offset: 0x000491A4
	private dfTriangleClippingRegion()
	{
		this.planes = new dfList<Plane>();
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x0004AFB8 File Offset: 0x000491B8
	public static dfTriangleClippingRegion Obtain()
	{
		return (dfTriangleClippingRegion.pool.Count <= 0) ? new dfTriangleClippingRegion() : dfTriangleClippingRegion.pool.Dequeue();
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x0004AFE0 File Offset: 0x000491E0
	public static dfTriangleClippingRegion Obtain(dfTriangleClippingRegion parent, dfControl control)
	{
		dfTriangleClippingRegion dfTriangleClippingRegion = ((dfTriangleClippingRegion.pool.Count <= 0) ? new dfTriangleClippingRegion() : dfTriangleClippingRegion.pool.Dequeue());
		dfTriangleClippingRegion.planes.AddRange(control.GetClippingPlanes());
		if (parent != null)
		{
			dfTriangleClippingRegion.planes.AddRange(parent.planes);
		}
		return dfTriangleClippingRegion;
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x0004B03C File Offset: 0x0004923C
	public void Release()
	{
		this.planes.Clear();
		if (!dfTriangleClippingRegion.pool.Contains(this))
		{
			dfTriangleClippingRegion.pool.Enqueue(this);
		}
	}

	// Token: 0x06001018 RID: 4120 RVA: 0x0004B064 File Offset: 0x00049264
	public bool PerformClipping(dfRenderData dest, ref Bounds bounds, uint checksum, dfRenderData controlData)
	{
		if (this.planes == null || this.planes.Count == 0)
		{
			dest.Merge(controlData);
			return true;
		}
		if (controlData.Checksum == checksum)
		{
			if (controlData.Intersection == dfIntersectionType.Inside)
			{
				dest.Merge(controlData);
				return true;
			}
			if (controlData.Intersection == dfIntersectionType.None)
			{
				return false;
			}
		}
		bool flag = false;
		dfIntersectionType dfIntersectionType;
		dfList<Plane> dfList = this.TestIntersection(bounds, out dfIntersectionType);
		if (dfIntersectionType == dfIntersectionType.Inside)
		{
			dest.Merge(controlData);
			flag = true;
		}
		else if (dfIntersectionType == dfIntersectionType.Intersecting)
		{
			this.clipToPlanes(dfList, controlData, dest, checksum);
			flag = true;
		}
		controlData.Checksum = checksum;
		controlData.Intersection = dfIntersectionType;
		return flag;
	}

	// Token: 0x06001019 RID: 4121 RVA: 0x0004B114 File Offset: 0x00049314
	public dfList<Plane> TestIntersection(Bounds bounds, out dfIntersectionType type)
	{
		if (this.planes == null || this.planes.Count == 0)
		{
			type = dfIntersectionType.Inside;
			return null;
		}
		dfTriangleClippingRegion.intersectedPlanes.Clear();
		Vector3 center = bounds.center;
		Vector3 extents = bounds.extents;
		bool flag = false;
		int count = this.planes.Count;
		Plane[] items = this.planes.Items;
		for (int i = 0; i < count; i++)
		{
			Plane plane = items[i];
			Vector3 normal = plane.normal;
			float distance = plane.distance;
			float num = extents.x * Mathf.Abs(normal.x) + extents.y * Mathf.Abs(normal.y) + extents.z * Mathf.Abs(normal.z);
			float num2 = Vector3.Dot(normal, center) + distance;
			if (Mathf.Abs(num2) <= num)
			{
				flag = true;
				dfTriangleClippingRegion.intersectedPlanes.Add(plane);
			}
			else if (num2 < -num)
			{
				type = dfIntersectionType.None;
				return null;
			}
		}
		if (flag)
		{
			type = dfIntersectionType.Intersecting;
			return dfTriangleClippingRegion.intersectedPlanes;
		}
		type = dfIntersectionType.Inside;
		return null;
	}

	// Token: 0x0600101A RID: 4122 RVA: 0x0004B240 File Offset: 0x00049440
	public void clipToPlanes(dfList<Plane> planes, dfRenderData data, dfRenderData dest, uint controlChecksum)
	{
		if (data == null || data.Vertices.Count == 0)
		{
			return;
		}
		if (planes == null || planes.Count == 0)
		{
			dest.Merge(data);
			return;
		}
		dfClippingUtil.Clip(planes, data, dest);
	}

	// Token: 0x0600101B RID: 4123 RVA: 0x0004B27C File Offset: 0x0004947C
	public void Dispose()
	{
		this.Release();
	}

	// Token: 0x04000F18 RID: 3864
	private static Queue<dfTriangleClippingRegion> pool = new Queue<dfTriangleClippingRegion>();

	// Token: 0x04000F19 RID: 3865
	private static dfList<Plane> intersectedPlanes = new dfList<Plane>(32);

	// Token: 0x04000F1A RID: 3866
	private dfList<Plane> planes;
}
