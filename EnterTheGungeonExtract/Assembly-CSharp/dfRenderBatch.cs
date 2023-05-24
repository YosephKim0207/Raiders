using System;
using UnityEngine;

// Token: 0x02000410 RID: 1040
internal class dfRenderBatch
{
	// Token: 0x06001793 RID: 6035 RVA: 0x00070380 File Offset: 0x0006E580
	public void Add(dfRenderData buffer)
	{
		if (this.Material == null && buffer.Material != null)
		{
			this.Material = buffer.Material;
		}
		this.buffers.Add(buffer);
	}

	// Token: 0x06001794 RID: 6036 RVA: 0x000703BC File Offset: 0x0006E5BC
	public dfRenderData Combine()
	{
		dfRenderData dfRenderData = dfRenderData.Obtain();
		int count = this.buffers.Count;
		dfRenderData[] items = this.buffers.Items;
		if (count == 0)
		{
			return dfRenderData;
		}
		dfRenderData.Material = this.buffers[0].Material;
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			num = items[i].Vertices.Count;
		}
		dfRenderData.EnsureCapacity(num);
		int[] items2 = dfRenderData.Triangles.Items;
		for (int j = 0; j < count; j++)
		{
			dfRenderData dfRenderData2 = items[j];
			int count2 = dfRenderData.Vertices.Count;
			int count3 = dfRenderData.Triangles.Count;
			int count4 = dfRenderData2.Triangles.Count;
			dfRenderData.Vertices.AddRange(dfRenderData2.Vertices);
			dfRenderData.Triangles.AddRange(dfRenderData2.Triangles);
			dfRenderData.Colors.AddRange(dfRenderData2.Colors);
			dfRenderData.UV.AddRange(dfRenderData2.UV);
			for (int k = count3; k < count3 + count4; k++)
			{
				items2[k] += count2;
			}
		}
		return dfRenderData;
	}

	// Token: 0x040012F9 RID: 4857
	public Material Material;

	// Token: 0x040012FA RID: 4858
	private dfList<dfRenderData> buffers = new dfList<dfRenderData>();
}
