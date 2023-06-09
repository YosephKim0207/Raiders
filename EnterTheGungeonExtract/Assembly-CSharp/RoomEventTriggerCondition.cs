﻿using System;

// Token: 0x02000F13 RID: 3859
public enum RoomEventTriggerCondition
{
	// Token: 0x04004AE4 RID: 19172
	ON_ENTER,
	// Token: 0x04004AE5 RID: 19173
	ON_EXIT,
	// Token: 0x04004AE6 RID: 19174
	ON_ENEMIES_CLEARED,
	// Token: 0x04004AE7 RID: 19175
	ON_ENTER_WITH_ENEMIES,
	// Token: 0x04004AE8 RID: 19176
	ON_ONE_QUARTER_ENEMY_HP_DEPLETED = 8,
	// Token: 0x04004AE9 RID: 19177
	ON_HALF_ENEMY_HP_DEPLETED = 12,
	// Token: 0x04004AEA RID: 19178
	ON_THREE_QUARTERS_ENEMY_HP_DEPLETED = 16,
	// Token: 0x04004AEB RID: 19179
	ON_NINETY_PERCENT_ENEMY_HP_DEPLETED = 20,
	// Token: 0x04004AEC RID: 19180
	TIMER = 30,
	// Token: 0x04004AED RID: 19181
	SHRINE_WAVE_A = 40,
	// Token: 0x04004AEE RID: 19182
	SHRINE_WAVE_B,
	// Token: 0x04004AEF RID: 19183
	SHRINE_WAVE_C,
	// Token: 0x04004AF0 RID: 19184
	NPC_TRIGGER_A = 60,
	// Token: 0x04004AF1 RID: 19185
	NPC_TRIGGER_B,
	// Token: 0x04004AF2 RID: 19186
	NPC_TRIGGER_C,
	// Token: 0x04004AF3 RID: 19187
	ENEMY_BEHAVIOR = 80,
	// Token: 0x04004AF4 RID: 19188
	SEQUENTIAL_WAVE_TRIGGER = 100
}
