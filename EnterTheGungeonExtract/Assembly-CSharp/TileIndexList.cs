using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000F40 RID: 3904
[Serializable]
public class TileIndexList
{
	// Token: 0x060053D8 RID: 21464 RVA: 0x001EBEF0 File Offset: 0x001EA0F0
	public TileIndexList()
	{
		this.indices = new List<int>();
		this.indexWeights = new List<float>();
	}

	// Token: 0x060053D9 RID: 21465 RVA: 0x001EBF10 File Offset: 0x001EA110
	public int GetIndexOfIndex(int index)
	{
		for (int i = 0; i < this.indices.Count; i++)
		{
			if (this.indices[i] == index)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060053DA RID: 21466 RVA: 0x001EBF50 File Offset: 0x001EA150
	public int GetIndexByWeight()
	{
		float num = this.indexWeights.Sum();
		float num2 = num * UnityEngine.Random.value;
		float num3 = 0f;
		for (int i = 0; i < this.indices.Count; i++)
		{
			num3 += this.indexWeights[i];
			if (num3 >= num2)
			{
				return this.indices[i];
			}
		}
		if (this.indices.Count == 0)
		{
			return -1;
		}
		return this.indices[this.indices.Count - 1];
	}

	// Token: 0x060053DB RID: 21467 RVA: 0x001EBFE0 File Offset: 0x001EA1E0
	public bool ContainsValid()
	{
		for (int i = 0; i < this.indices.Count; i++)
		{
			if (this.indices[i] != -1)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04004CC9 RID: 19657
	[SerializeField]
	public List<int> indices;

	// Token: 0x04004CCA RID: 19658
	[SerializeField]
	public List<float> indexWeights;
}
