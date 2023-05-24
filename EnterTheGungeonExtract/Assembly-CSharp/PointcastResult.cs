using System;

// Token: 0x02000860 RID: 2144
public class PointcastResult : IComparable<PointcastResult>
{
	// Token: 0x06002F49 RID: 12105 RVA: 0x000F96EC File Offset: 0x000F78EC
	private PointcastResult()
	{
	}

	// Token: 0x06002F4A RID: 12106 RVA: 0x000F96F4 File Offset: 0x000F78F4
	public void SetAll(HitDirection hitDirection, int pointIndex, int boneIndex, RaycastResult hitResult)
	{
		this.hitDirection = hitDirection;
		this.pointIndex = pointIndex;
		this.boneIndex = boneIndex;
		this.hitResult = hitResult;
	}

	// Token: 0x06002F4B RID: 12107 RVA: 0x000F9714 File Offset: 0x000F7914
	public static void Cleanup(PointcastResult pointcastResult)
	{
		pointcastResult.hitDirection = HitDirection.Unknown;
		pointcastResult.pointIndex = 0;
		pointcastResult.boneIndex = 0;
		RaycastResult.Pool.Free(ref pointcastResult.hitResult);
	}

	// Token: 0x06002F4C RID: 12108 RVA: 0x000F973C File Offset: 0x000F793C
	public int CompareTo(PointcastResult other)
	{
		int num = this.boneIndex - other.boneIndex;
		if (num != 0)
		{
			return num;
		}
		return this.pointIndex - other.pointIndex;
	}

	// Token: 0x04002051 RID: 8273
	public RaycastResult hitResult;

	// Token: 0x04002052 RID: 8274
	public int pointIndex;

	// Token: 0x04002053 RID: 8275
	public int boneIndex;

	// Token: 0x04002054 RID: 8276
	public HitDirection hitDirection;

	// Token: 0x04002055 RID: 8277
	public static ObjectPool<PointcastResult> Pool = new ObjectPool<PointcastResult>(() => new PointcastResult(), 10, new ObjectPool<PointcastResult>.Cleanup(PointcastResult.Cleanup));
}
