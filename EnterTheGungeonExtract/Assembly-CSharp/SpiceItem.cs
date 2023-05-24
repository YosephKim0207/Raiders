using System;
using System.Collections.Generic;

// Token: 0x020014BA RID: 5306
public class SpiceItem : PlayerItem
{
	// Token: 0x0600789D RID: 30877 RVA: 0x003037DC File Offset: 0x003019DC
	public static float GetSpiceWeight(int spiceCount)
	{
		if (spiceCount <= 0)
		{
			return 0f;
		}
		if (spiceCount == 1)
		{
			return SpiceItem.ONE_SPICE_WEIGHT;
		}
		if (spiceCount == 2)
		{
			return SpiceItem.TWO_SPICE_WEIGHT;
		}
		if (spiceCount == 3)
		{
			return SpiceItem.THREE_SPICE_WEIGHT;
		}
		return SpiceItem.FOUR_PLUS_SPICE_WEIGHT;
	}

	// Token: 0x0600789E RID: 30878 RVA: 0x00303818 File Offset: 0x00301A18
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
		if (user.spiceCount == 0)
		{
			for (int i = 0; i < this.FirstTimeStatModifiers.Count; i++)
			{
				user.ownerlessStatModifiers.Add(this.FirstTimeStatModifiers[i]);
			}
		}
		else if (user.spiceCount == 1)
		{
			for (int j = 0; j < this.SecondTimeStatModifiers.Count; j++)
			{
				user.ownerlessStatModifiers.Add(this.SecondTimeStatModifiers[j]);
			}
		}
		else if (user.spiceCount == 2)
		{
			for (int k = 0; k < this.ThirdTimeStatModifiers.Count; k++)
			{
				user.ownerlessStatModifiers.Add(this.ThirdTimeStatModifiers[k]);
			}
		}
		else if (user.spiceCount > 2)
		{
			for (int l = 0; l < this.FourthAndBeyondStatModifiers.Count; l++)
			{
				user.ownerlessStatModifiers.Add(this.FourthAndBeyondStatModifiers[l]);
			}
		}
		user.stats.RecalculateStats(user, false, false);
		user.spiceCount++;
	}

	// Token: 0x0600789F RID: 30879 RVA: 0x0030395C File Offset: 0x00301B5C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007AD7 RID: 31447
	public static float ONE_SPICE_WEIGHT = 0.1f;

	// Token: 0x04007AD8 RID: 31448
	public static float TWO_SPICE_WEIGHT = 0.3f;

	// Token: 0x04007AD9 RID: 31449
	public static float THREE_SPICE_WEIGHT = 0.5f;

	// Token: 0x04007ADA RID: 31450
	public static float FOUR_PLUS_SPICE_WEIGHT = 0.8f;

	// Token: 0x04007ADB RID: 31451
	public List<StatModifier> FirstTimeStatModifiers;

	// Token: 0x04007ADC RID: 31452
	public List<StatModifier> SecondTimeStatModifiers;

	// Token: 0x04007ADD RID: 31453
	public List<StatModifier> ThirdTimeStatModifiers;

	// Token: 0x04007ADE RID: 31454
	public List<StatModifier> FourthAndBeyondStatModifiers;
}
