using System;

// Token: 0x0200180E RID: 6158
public class NotificationParams
{
	// Token: 0x04009949 RID: 39241
	public bool isSingleLine;

	// Token: 0x0400994A RID: 39242
	public string EncounterGuid;

	// Token: 0x0400994B RID: 39243
	public int pickupId = -1;

	// Token: 0x0400994C RID: 39244
	public string PrimaryTitleString;

	// Token: 0x0400994D RID: 39245
	public string SecondaryDescriptionString;

	// Token: 0x0400994E RID: 39246
	public tk2dSpriteCollectionData SpriteCollection;

	// Token: 0x0400994F RID: 39247
	public int SpriteID;

	// Token: 0x04009950 RID: 39248
	public UINotificationController.NotificationColor forcedColor;

	// Token: 0x04009951 RID: 39249
	public bool OnlyIfSynergy;

	// Token: 0x04009952 RID: 39250
	public bool HasAttachedSynergy;

	// Token: 0x04009953 RID: 39251
	public AdvancedSynergyEntry AttachedSynergy;
}
