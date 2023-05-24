using System;
using System.Collections.Generic;

// Token: 0x020010D7 RID: 4311
[Serializable]
public class EnemyFactoryWaveDefinition
{
	// Token: 0x0400592B RID: 22827
	public bool exactDefinition;

	// Token: 0x0400592C RID: 22828
	public List<AIActor> enemyList;

	// Token: 0x0400592D RID: 22829
	public int inexactMinCount = 2;

	// Token: 0x0400592E RID: 22830
	public int inexactMaxCount = 4;
}
