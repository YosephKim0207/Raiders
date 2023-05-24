using System;

// Token: 0x02001839 RID: 6201
public class WeightedItem<T>
{
	// Token: 0x060092D5 RID: 37589 RVA: 0x003E0304 File Offset: 0x003DE504
	public WeightedItem(T v, float w)
	{
		this.value = v;
		this.weight = w;
	}

	// Token: 0x04009A5E RID: 39518
	public T value;

	// Token: 0x04009A5F RID: 39519
	public float weight;
}
