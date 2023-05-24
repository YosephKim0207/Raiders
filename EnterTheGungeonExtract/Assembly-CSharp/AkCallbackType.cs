using System;

// Token: 0x02001863 RID: 6243
public enum AkCallbackType
{
	// Token: 0x04009AF3 RID: 39667
	AK_EndOfEvent = 1,
	// Token: 0x04009AF4 RID: 39668
	AK_EndOfDynamicSequenceItem,
	// Token: 0x04009AF5 RID: 39669
	AK_Marker = 4,
	// Token: 0x04009AF6 RID: 39670
	AK_Duration = 8,
	// Token: 0x04009AF7 RID: 39671
	AK_SpeakerVolumeMatrix = 16,
	// Token: 0x04009AF8 RID: 39672
	AK_Starvation = 32,
	// Token: 0x04009AF9 RID: 39673
	AK_MusicPlaylistSelect = 64,
	// Token: 0x04009AFA RID: 39674
	AK_MusicPlayStarted = 128,
	// Token: 0x04009AFB RID: 39675
	AK_MusicSyncBeat = 256,
	// Token: 0x04009AFC RID: 39676
	AK_MusicSyncBar = 512,
	// Token: 0x04009AFD RID: 39677
	AK_MusicSyncEntry = 1024,
	// Token: 0x04009AFE RID: 39678
	AK_MusicSyncExit = 2048,
	// Token: 0x04009AFF RID: 39679
	AK_MusicSyncGrid = 4096,
	// Token: 0x04009B00 RID: 39680
	AK_MusicSyncUserCue = 8192,
	// Token: 0x04009B01 RID: 39681
	AK_MusicSyncPoint = 16384,
	// Token: 0x04009B02 RID: 39682
	AK_MusicSyncAll = 32512,
	// Token: 0x04009B03 RID: 39683
	AK_MIDIEvent = 65536,
	// Token: 0x04009B04 RID: 39684
	AK_CallbackBits = 1048575,
	// Token: 0x04009B05 RID: 39685
	AK_EnableGetSourcePlayPosition,
	// Token: 0x04009B06 RID: 39686
	AK_EnableGetMusicPlayPosition = 2097152,
	// Token: 0x04009B07 RID: 39687
	AK_EnableGetSourceStreamBuffering = 4194304,
	// Token: 0x04009B08 RID: 39688
	AK_Monitoring = 536870912,
	// Token: 0x04009B09 RID: 39689
	AK_AudioSourceChange = 587202560,
	// Token: 0x04009B0A RID: 39690
	AK_Bank = 1073741824,
	// Token: 0x04009B0B RID: 39691
	AK_AudioInterruption = 570425344
}
