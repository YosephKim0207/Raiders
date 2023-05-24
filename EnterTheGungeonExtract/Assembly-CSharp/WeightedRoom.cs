using System;

// Token: 0x02000F6C RID: 3948
[Serializable]
public class WeightedRoom
{
	// Token: 0x06005520 RID: 21792 RVA: 0x002052D8 File Offset: 0x002034D8
	public bool CheckPrerequisites()
	{
		if (this.additionalPrerequisites == null || this.additionalPrerequisites.Length == 0)
		{
			return true;
		}
		for (int i = 0; i < this.additionalPrerequisites.Length; i++)
		{
			if (!this.additionalPrerequisites[i].CheckConditionsFulfilled())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04004E04 RID: 19972
	public PrototypeDungeonRoom room;

	// Token: 0x04004E05 RID: 19973
	public float weight;

	// Token: 0x04004E06 RID: 19974
	public bool limitedCopies;

	// Token: 0x04004E07 RID: 19975
	public int maxCopies = 1;

	// Token: 0x04004E08 RID: 19976
	public DungeonPrerequisite[] additionalPrerequisites;
}
