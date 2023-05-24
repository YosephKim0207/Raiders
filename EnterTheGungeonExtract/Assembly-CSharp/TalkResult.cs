using System;
using UnityEngine;

// Token: 0x02001221 RID: 4641
[Serializable]
public class TalkResult
{
	// Token: 0x040063CD RID: 25549
	public TalkResult.TalkResultAction action;

	// Token: 0x040063CE RID: 25550
	public GameObject objectData;

	// Token: 0x040063CF RID: 25551
	public string actionData;

	// Token: 0x040063D0 RID: 25552
	[ShowInInspectorIf("action", 8, false)]
	public GenericLootTable lootTableData;

	// Token: 0x040063D1 RID: 25553
	public string customActionID;

	// Token: 0x02001222 RID: 4642
	public enum TalkResultAction
	{
		// Token: 0x040063D3 RID: 25555
		CHANGE_DEFAULT_MODULE,
		// Token: 0x040063D4 RID: 25556
		OPEN_TRUTH_CHEST,
		// Token: 0x040063D5 RID: 25557
		VANISH,
		// Token: 0x040063D6 RID: 25558
		TOSS_CURRENT_GUN_IN_POT,
		// Token: 0x040063D7 RID: 25559
		RENDER_SILENT,
		// Token: 0x040063D8 RID: 25560
		CHANGE_DEFAULT_MODULE_OF_OTHER_TALKDOER,
		// Token: 0x040063D9 RID: 25561
		SPAWN_ITEM,
		// Token: 0x040063DA RID: 25562
		MAKE_TALKDOER_INTERACTABLE,
		// Token: 0x040063DB RID: 25563
		SPAWN_ITEM_FROM_TABLE,
		// Token: 0x040063DC RID: 25564
		CUSTOM_ACTION = 99
	}
}
