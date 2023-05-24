using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200183A RID: 6202
public class WeightedList<T>
{
	// Token: 0x060092D7 RID: 37591 RVA: 0x003E0324 File Offset: 0x003DE524
	public void Add(T item, float weight)
	{
		if (this.elements == null)
		{
			this.elements = new List<WeightedItem<T>>();
		}
		this.elements.Add(new WeightedItem<T>(item, weight));
	}

	// Token: 0x060092D8 RID: 37592 RVA: 0x003E0350 File Offset: 0x003DE550
	public T SelectByWeight()
	{
		if (this.elements == null || this.elements.Count == 0)
		{
			return default(T);
		}
		float num = 0f;
		for (int i = 0; i < this.elements.Count; i++)
		{
			num += this.elements[i].weight;
		}
		float num2 = UnityEngine.Random.value * num;
		float num3 = 0f;
		for (int j = 0; j < this.elements.Count; j++)
		{
			num3 += this.elements[j].weight;
			if (num3 > num2)
			{
				return this.elements[j].value;
			}
		}
		return this.elements[this.elements.Count - 1].value;
	}

	// Token: 0x04009A60 RID: 39520
	public List<WeightedItem<T>> elements;
}
