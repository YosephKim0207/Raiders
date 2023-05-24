using System;
using UnityEngine;

// Token: 0x02001604 RID: 5636
[Serializable]
public class ProceduralDataPoint
{
	// Token: 0x060082EB RID: 33515 RVA: 0x003587A0 File Offset: 0x003569A0
	public ProceduralDataPoint(float min, float max)
	{
		this.minValue = min;
		this.maxValue = max;
		this.distribution = new AnimationCurve();
	}

	// Token: 0x060082EC RID: 33516 RVA: 0x003587C4 File Offset: 0x003569C4
	private float FullIntegortion()
	{
		float num = 0f;
		for (float num2 = 0.01f; num2 <= 1f; num2 += 0.01f)
		{
			num += 0.01f * this.distribution.Evaluate(num2);
		}
		return num;
	}

	// Token: 0x060082ED RID: 33517 RVA: 0x0035880C File Offset: 0x00356A0C
	private float PartialIntegortion(float target)
	{
		float num = this.FullIntegortion();
		float num2 = 0f;
		for (float num3 = 0.01f; num3 <= 1f; num3 += 0.01f)
		{
			num2 += 0.01f * this.distribution.Evaluate(num3);
			if (num2 / num > target)
			{
				return num3;
			}
		}
		return 1f;
	}

	// Token: 0x060082EE RID: 33518 RVA: 0x0035886C File Offset: 0x00356A6C
	public float GetSpecificValue(float p)
	{
		return this.minValue + this.PartialIntegortion(p) * (this.maxValue - this.minValue);
	}

	// Token: 0x060082EF RID: 33519 RVA: 0x0035888C File Offset: 0x00356A8C
	public int GetSpecificIntValue(float p)
	{
		return Mathf.RoundToInt(this.GetSpecificValue(p));
	}

	// Token: 0x060082F0 RID: 33520 RVA: 0x0035889C File Offset: 0x00356A9C
	public float GetRandomValue()
	{
		return this.GetSpecificValue(UnityEngine.Random.value);
	}

	// Token: 0x060082F1 RID: 33521 RVA: 0x003588AC File Offset: 0x00356AAC
	public int GetRandomIntValue()
	{
		return Mathf.RoundToInt(this.GetRandomValue());
	}

	// Token: 0x04008606 RID: 34310
	public float minValue;

	// Token: 0x04008607 RID: 34311
	public float maxValue;

	// Token: 0x04008608 RID: 34312
	public AnimationCurve distribution;

	// Token: 0x04008609 RID: 34313
	private const float INTEGRATION_STEP = 0.01f;
}
