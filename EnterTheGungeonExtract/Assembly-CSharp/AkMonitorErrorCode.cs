using System;

// Token: 0x02001887 RID: 6279
public enum AkMonitorErrorCode
{
	// Token: 0x04009BD7 RID: 39895
	ErrorCode_NoError,
	// Token: 0x04009BD8 RID: 39896
	ErrorCode_FileNotFound,
	// Token: 0x04009BD9 RID: 39897
	ErrorCode_CannotOpenFile,
	// Token: 0x04009BDA RID: 39898
	ErrorCode_CannotStartStreamNoMemory,
	// Token: 0x04009BDB RID: 39899
	ErrorCode_IODevice,
	// Token: 0x04009BDC RID: 39900
	ErrorCode_IncompatibleIOSettings,
	// Token: 0x04009BDD RID: 39901
	ErrorCode_PluginUnsupportedChannelConfiguration,
	// Token: 0x04009BDE RID: 39902
	ErrorCode_PluginMediaUnavailable,
	// Token: 0x04009BDF RID: 39903
	ErrorCode_PluginInitialisationFailed,
	// Token: 0x04009BE0 RID: 39904
	ErrorCode_PluginProcessingFailed,
	// Token: 0x04009BE1 RID: 39905
	ErrorCode_PluginExecutionInvalid,
	// Token: 0x04009BE2 RID: 39906
	ErrorCode_PluginAllocationFailed,
	// Token: 0x04009BE3 RID: 39907
	ErrorCode_VorbisRequireSeekTable,
	// Token: 0x04009BE4 RID: 39908
	ErrorCode_VorbisDecodeError,
	// Token: 0x04009BE5 RID: 39909
	ErrorCode_AACDecodeError,
	// Token: 0x04009BE6 RID: 39910
	ErrorCode_xWMACreateDecoderFailed,
	// Token: 0x04009BE7 RID: 39911
	ErrorCode_ATRAC9CreateDecoderFailed,
	// Token: 0x04009BE8 RID: 39912
	ErrorCode_ATRAC9CreateDecoderFailedChShortage,
	// Token: 0x04009BE9 RID: 39913
	ErrorCode_ATRAC9DecodeFailed,
	// Token: 0x04009BEA RID: 39914
	ErrorCode_ATRAC9ClearContextFailed,
	// Token: 0x04009BEB RID: 39915
	ErrorCode_ATRAC9LoopSectionTooSmall,
	// Token: 0x04009BEC RID: 39916
	ErrorCode_InvalidAudioFileHeader,
	// Token: 0x04009BED RID: 39917
	ErrorCode_AudioFileHeaderTooLarge,
	// Token: 0x04009BEE RID: 39918
	ErrorCode_FileTooSmall,
	// Token: 0x04009BEF RID: 39919
	ErrorCode_TransitionNotAccurateChannel,
	// Token: 0x04009BF0 RID: 39920
	ErrorCode_TransitionNotAccurateStarvation,
	// Token: 0x04009BF1 RID: 39921
	ErrorCode_NothingToPlay,
	// Token: 0x04009BF2 RID: 39922
	ErrorCode_PlayFailed,
	// Token: 0x04009BF3 RID: 39923
	ErrorCode_StingerCouldNotBeScheduled,
	// Token: 0x04009BF4 RID: 39924
	ErrorCode_TooLongSegmentLookAhead,
	// Token: 0x04009BF5 RID: 39925
	ErrorCode_CannotScheduleMusicSwitch,
	// Token: 0x04009BF6 RID: 39926
	ErrorCode_TooManySimultaneousMusicSegments,
	// Token: 0x04009BF7 RID: 39927
	ErrorCode_PlaylistStoppedForEditing,
	// Token: 0x04009BF8 RID: 39928
	ErrorCode_MusicClipsRescheduledAfterTrackEdit,
	// Token: 0x04009BF9 RID: 39929
	ErrorCode_CannotPlaySource_Create,
	// Token: 0x04009BFA RID: 39930
	ErrorCode_CannotPlaySource_VirtualOff,
	// Token: 0x04009BFB RID: 39931
	ErrorCode_CannotPlaySource_TimeSkip,
	// Token: 0x04009BFC RID: 39932
	ErrorCode_CannotPlaySource_InconsistentState,
	// Token: 0x04009BFD RID: 39933
	ErrorCode_MediaNotLoaded,
	// Token: 0x04009BFE RID: 39934
	ErrorCode_VoiceStarving,
	// Token: 0x04009BFF RID: 39935
	ErrorCode_StreamingSourceStarving,
	// Token: 0x04009C00 RID: 39936
	ErrorCode_XMADecoderSourceStarving,
	// Token: 0x04009C01 RID: 39937
	ErrorCode_XMADecodingError,
	// Token: 0x04009C02 RID: 39938
	ErrorCode_InvalidXMAData,
	// Token: 0x04009C03 RID: 39939
	ErrorCode_PluginNotRegistered,
	// Token: 0x04009C04 RID: 39940
	ErrorCode_CodecNotRegistered,
	// Token: 0x04009C05 RID: 39941
	ErrorCode_PluginVersionMismatch,
	// Token: 0x04009C06 RID: 39942
	ErrorCode_EventIDNotFound,
	// Token: 0x04009C07 RID: 39943
	ErrorCode_InvalidGroupID,
	// Token: 0x04009C08 RID: 39944
	ErrorCode_SelectedChildNotAvailable,
	// Token: 0x04009C09 RID: 39945
	ErrorCode_SelectedNodeNotAvailable,
	// Token: 0x04009C0A RID: 39946
	ErrorCode_SelectedMediaNotAvailable,
	// Token: 0x04009C0B RID: 39947
	ErrorCode_NoValidSwitch,
	// Token: 0x04009C0C RID: 39948
	ErrorCode_SelectedNodeNotAvailablePlay,
	// Token: 0x04009C0D RID: 39949
	ErrorCode_FeedbackVoiceStarving,
	// Token: 0x04009C0E RID: 39950
	ErrorCode_BankLoadFailed,
	// Token: 0x04009C0F RID: 39951
	ErrorCode_BankUnloadFailed,
	// Token: 0x04009C10 RID: 39952
	ErrorCode_ErrorWhileLoadingBank,
	// Token: 0x04009C11 RID: 39953
	ErrorCode_InsufficientSpaceToLoadBank,
	// Token: 0x04009C12 RID: 39954
	ErrorCode_LowerEngineCommandListFull,
	// Token: 0x04009C13 RID: 39955
	ErrorCode_SeekNoMarker,
	// Token: 0x04009C14 RID: 39956
	ErrorCode_CannotSeekContinuous,
	// Token: 0x04009C15 RID: 39957
	ErrorCode_SeekAfterEof,
	// Token: 0x04009C16 RID: 39958
	ErrorCode_UnknownGameObject,
	// Token: 0x04009C17 RID: 39959
	ErrorCode_UnknownEmitter,
	// Token: 0x04009C18 RID: 39960
	ErrorCode_UnknownListener,
	// Token: 0x04009C19 RID: 39961
	ErrorCode_GameObjectIsNotListener,
	// Token: 0x04009C1A RID: 39962
	ErrorCode_GameObjectIsNotEmitter,
	// Token: 0x04009C1B RID: 39963
	ErrorCode_UnknownGameObjectEvent,
	// Token: 0x04009C1C RID: 39964
	ErrorCode_GameObjectIsNotEmitterEvent,
	// Token: 0x04009C1D RID: 39965
	ErrorCode_ExternalSourceNotResolved,
	// Token: 0x04009C1E RID: 39966
	ErrorCode_FileFormatMismatch,
	// Token: 0x04009C1F RID: 39967
	ErrorCode_CommandQueueFull,
	// Token: 0x04009C20 RID: 39968
	ErrorCode_CommandTooLarge,
	// Token: 0x04009C21 RID: 39969
	ErrorCode_XMACreateDecoderLimitReached,
	// Token: 0x04009C22 RID: 39970
	ErrorCode_XMAStreamBufferTooSmall,
	// Token: 0x04009C23 RID: 39971
	ErrorCode_ModulatorScopeError_Inst,
	// Token: 0x04009C24 RID: 39972
	ErrorCode_ModulatorScopeError_Obj,
	// Token: 0x04009C25 RID: 39973
	ErrorCode_SeekAfterEndOfPlaylist,
	// Token: 0x04009C26 RID: 39974
	ErrorCode_OpusRequireSeekTable,
	// Token: 0x04009C27 RID: 39975
	ErrorCode_OpusDecodeError,
	// Token: 0x04009C28 RID: 39976
	ErrorCode_OpusCreateDecoderFailed,
	// Token: 0x04009C29 RID: 39977
	ErrorCode_SourcePluginNotFound,
	// Token: 0x04009C2A RID: 39978
	ErrorCode_VirtualVoiceLimit,
	// Token: 0x04009C2B RID: 39979
	ErrorCode_AudioDeviceShareSetNotFound,
	// Token: 0x04009C2C RID: 39980
	ErrorCode_NotEnoughMemoryToStart,
	// Token: 0x04009C2D RID: 39981
	Num_ErrorCodes
}
