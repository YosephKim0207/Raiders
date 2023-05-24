using System;

namespace Dungeonator
{
	// Token: 0x02000F11 RID: 3857
	[Serializable]
	public class Subrule
	{
		// Token: 0x04004ADA RID: 19162
		public string ruleName = "Generic";

		// Token: 0x04004ADB RID: 19163
		public RoomCreationRule.PlacementStrategy placementRule;

		// Token: 0x04004ADC RID: 19164
		public int minToSpawn = 1;

		// Token: 0x04004ADD RID: 19165
		public int maxToSpawn = 1;

		// Token: 0x04004ADE RID: 19166
		public DungeonPlaceable placeableObject;
	}
}
