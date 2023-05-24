using System;

// Token: 0x02000EC7 RID: 3783
public struct GeneratedEnemyData
{
	// Token: 0x0600502D RID: 20525 RVA: 0x001C1F38 File Offset: 0x001C0138
	public GeneratedEnemyData(string id, float percent, bool isSig)
	{
		this.enemyGuid = id;
		this.percentOfEnemies = percent;
		this.isSignatureEnemy = isSig;
	}

	// Token: 0x0400485E RID: 18526
	public string enemyGuid;

	// Token: 0x0400485F RID: 18527
	public float percentOfEnemies;

	// Token: 0x04004860 RID: 18528
	public bool isSignatureEnemy;
}
