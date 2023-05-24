using System;

// Token: 0x02001872 RID: 6258
public enum AkGlobalCallbackLocation
{
	// Token: 0x04009B39 RID: 39737
	AkGlobalCallbackLocation_Register = 1,
	// Token: 0x04009B3A RID: 39738
	AkGlobalCallbackLocation_Begin,
	// Token: 0x04009B3B RID: 39739
	AkGlobalCallbackLocation_PreProcessMessageQueueForRender = 4,
	// Token: 0x04009B3C RID: 39740
	AkGlobalCallbackLocation_PostMessagesProcessed = 8,
	// Token: 0x04009B3D RID: 39741
	AkGlobalCallbackLocation_BeginRender = 16,
	// Token: 0x04009B3E RID: 39742
	AkGlobalCallbackLocation_EndRender = 32,
	// Token: 0x04009B3F RID: 39743
	AkGlobalCallbackLocation_End = 64,
	// Token: 0x04009B40 RID: 39744
	AkGlobalCallbackLocation_Term = 128,
	// Token: 0x04009B41 RID: 39745
	AkGlobalCallbackLocation_Monitor = 256,
	// Token: 0x04009B42 RID: 39746
	AkGlobalCallbackLocation_Num = 9
}
