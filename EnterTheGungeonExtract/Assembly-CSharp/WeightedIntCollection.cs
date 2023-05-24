using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001852 RID: 6226
[Serializable]
public class WeightedIntCollection
{
	// Token: 0x0600933C RID: 37692 RVA: 0x003E29A0 File Offset: 0x003E0BA0
	public int SelectByWeight(System.Random generatorRandom)
	{
		List<WeightedInt> list = new List<WeightedInt>();
		float num = 0f;
		for (int i = 0; i < this.elements.Length; i++)
		{
			WeightedInt weightedInt = this.elements[i];
			if (weightedInt.weight > 0f)
			{
				bool flag = true;
				for (int j = 0; j < weightedInt.additionalPrerequisites.Length; j++)
				{
					if (!weightedInt.additionalPrerequisites[j].CheckConditionsFulfilled())
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(weightedInt);
					num += weightedInt.weight;
				}
			}
		}
		float num2 = ((generatorRandom == null) ? UnityEngine.Random.value : ((float)generatorRandom.NextDouble())) * num;
		float num3 = 0f;
		for (int k = 0; k < list.Count; k++)
		{
			num3 += list[k].weight;
			if (num3 > num2)
			{
				return list[k].value;
			}
		}
		return list[0].value;
	}

	// Token: 0x0600933D RID: 37693 RVA: 0x003E2AB4 File Offset: 0x003E0CB4
	public int SelectByWeight()
	{
		return this.SelectByWeight(null);
	}

	// Token: 0x04009AC4 RID: 39620
	public WeightedInt[] elements;
}
