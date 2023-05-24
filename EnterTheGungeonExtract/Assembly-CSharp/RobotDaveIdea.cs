using System;
using Dungeonator;

// Token: 0x020016AF RID: 5807
[Serializable]
public class RobotDaveIdea
{
	// Token: 0x06008758 RID: 34648 RVA: 0x00382564 File Offset: 0x00380764
	public RobotDaveIdea()
	{
	}

	// Token: 0x06008759 RID: 34649 RVA: 0x00382574 File Offset: 0x00380774
	public RobotDaveIdea(RobotDaveIdea source)
	{
		if (source == null)
		{
			return;
		}
		this.ValidEasyEnemyPlaceables = new DungeonPlaceable[(source.ValidEasyEnemyPlaceables == null) ? 0 : source.ValidEasyEnemyPlaceables.Length];
		for (int i = 0; i < this.ValidEasyEnemyPlaceables.Length; i++)
		{
			this.ValidEasyEnemyPlaceables[i] = source.ValidEasyEnemyPlaceables[i];
		}
		this.ValidHardEnemyPlaceables = new DungeonPlaceable[(source.ValidHardEnemyPlaceables == null) ? 0 : source.ValidHardEnemyPlaceables.Length];
		for (int j = 0; j < this.ValidHardEnemyPlaceables.Length; j++)
		{
			this.ValidHardEnemyPlaceables[j] = source.ValidHardEnemyPlaceables[j];
		}
		this.UseWallSawblades = source.UseWallSawblades;
		this.UseRollingLogsHorizontal = source.UseRollingLogsHorizontal;
		this.UseRollingLogsVertical = source.UseRollingLogsVertical;
		this.UseFloorPitTraps = source.UseFloorPitTraps;
		this.UseFloorFlameTraps = source.UseFloorFlameTraps;
		this.UseFloorSpikeTraps = source.UseFloorSpikeTraps;
		this.UseFloorConveyorBelts = source.UseFloorConveyorBelts;
		this.UseCaveIns = source.UseCaveIns;
		this.UseAlarmMushrooms = source.UseAlarmMushrooms;
		this.UseMineCarts = source.UseMineCarts;
		this.UseChandeliers = source.UseChandeliers;
		this.CanIncludePits = source.CanIncludePits;
	}

	// Token: 0x04008C42 RID: 35906
	public DungeonPlaceable[] ValidEasyEnemyPlaceables;

	// Token: 0x04008C43 RID: 35907
	public DungeonPlaceable[] ValidHardEnemyPlaceables;

	// Token: 0x04008C44 RID: 35908
	public bool UseWallSawblades;

	// Token: 0x04008C45 RID: 35909
	public bool UseRollingLogsVertical;

	// Token: 0x04008C46 RID: 35910
	public bool UseRollingLogsHorizontal;

	// Token: 0x04008C47 RID: 35911
	public bool UseFloorPitTraps;

	// Token: 0x04008C48 RID: 35912
	public bool UseFloorFlameTraps;

	// Token: 0x04008C49 RID: 35913
	public bool UseFloorSpikeTraps;

	// Token: 0x04008C4A RID: 35914
	public bool UseFloorConveyorBelts;

	// Token: 0x04008C4B RID: 35915
	public bool UseCaveIns;

	// Token: 0x04008C4C RID: 35916
	public bool UseAlarmMushrooms;

	// Token: 0x04008C4D RID: 35917
	public bool UseMineCarts;

	// Token: 0x04008C4E RID: 35918
	public bool UseChandeliers;

	// Token: 0x04008C4F RID: 35919
	public bool CanIncludePits = true;
}
