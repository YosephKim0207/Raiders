using System;

// Token: 0x0200189E RID: 6302
public enum AKRESULT
{
	// Token: 0x04009C6B RID: 40043
	AK_NotImplemented,
	// Token: 0x04009C6C RID: 40044
	AK_Success,
	// Token: 0x04009C6D RID: 40045
	AK_Fail,
	// Token: 0x04009C6E RID: 40046
	AK_PartialSuccess,
	// Token: 0x04009C6F RID: 40047
	AK_NotCompatible,
	// Token: 0x04009C70 RID: 40048
	AK_AlreadyConnected,
	// Token: 0x04009C71 RID: 40049
	AK_NameNotSet,
	// Token: 0x04009C72 RID: 40050
	AK_InvalidFile,
	// Token: 0x04009C73 RID: 40051
	AK_AudioFileHeaderTooLarge,
	// Token: 0x04009C74 RID: 40052
	AK_MaxReached,
	// Token: 0x04009C75 RID: 40053
	AK_InputsInUsed,
	// Token: 0x04009C76 RID: 40054
	AK_OutputsInUsed,
	// Token: 0x04009C77 RID: 40055
	AK_InvalidName,
	// Token: 0x04009C78 RID: 40056
	AK_NameAlreadyInUse,
	// Token: 0x04009C79 RID: 40057
	AK_InvalidID,
	// Token: 0x04009C7A RID: 40058
	AK_IDNotFound,
	// Token: 0x04009C7B RID: 40059
	AK_InvalidInstanceID,
	// Token: 0x04009C7C RID: 40060
	AK_NoMoreData,
	// Token: 0x04009C7D RID: 40061
	AK_NoSourceAvailable,
	// Token: 0x04009C7E RID: 40062
	AK_StateGroupAlreadyExists,
	// Token: 0x04009C7F RID: 40063
	AK_InvalidStateGroup,
	// Token: 0x04009C80 RID: 40064
	AK_ChildAlreadyHasAParent,
	// Token: 0x04009C81 RID: 40065
	AK_InvalidLanguage,
	// Token: 0x04009C82 RID: 40066
	AK_CannotAddItseflAsAChild,
	// Token: 0x04009C83 RID: 40067
	AK_UserNotInList = 29,
	// Token: 0x04009C84 RID: 40068
	AK_NoTransitionPoint,
	// Token: 0x04009C85 RID: 40069
	AK_InvalidParameter,
	// Token: 0x04009C86 RID: 40070
	AK_ParameterAdjusted,
	// Token: 0x04009C87 RID: 40071
	AK_IsA3DSound,
	// Token: 0x04009C88 RID: 40072
	AK_NotA3DSound,
	// Token: 0x04009C89 RID: 40073
	AK_ElementAlreadyInList,
	// Token: 0x04009C8A RID: 40074
	AK_PathNotFound,
	// Token: 0x04009C8B RID: 40075
	AK_PathNoVertices,
	// Token: 0x04009C8C RID: 40076
	AK_PathNotRunning,
	// Token: 0x04009C8D RID: 40077
	AK_PathNotPaused,
	// Token: 0x04009C8E RID: 40078
	AK_PathNodeAlreadyInList,
	// Token: 0x04009C8F RID: 40079
	AK_PathNodeNotInList,
	// Token: 0x04009C90 RID: 40080
	AK_VoiceNotFound,
	// Token: 0x04009C91 RID: 40081
	AK_DataNeeded,
	// Token: 0x04009C92 RID: 40082
	AK_NoDataNeeded,
	// Token: 0x04009C93 RID: 40083
	AK_DataReady,
	// Token: 0x04009C94 RID: 40084
	AK_NoDataReady,
	// Token: 0x04009C95 RID: 40085
	AK_NoMoreSlotAvailable,
	// Token: 0x04009C96 RID: 40086
	AK_SlotNotFound,
	// Token: 0x04009C97 RID: 40087
	AK_ProcessingOnly,
	// Token: 0x04009C98 RID: 40088
	AK_MemoryLeak,
	// Token: 0x04009C99 RID: 40089
	AK_CorruptedBlockList,
	// Token: 0x04009C9A RID: 40090
	AK_InsufficientMemory,
	// Token: 0x04009C9B RID: 40091
	AK_Cancelled,
	// Token: 0x04009C9C RID: 40092
	AK_UnknownBankID,
	// Token: 0x04009C9D RID: 40093
	AK_IsProcessing,
	// Token: 0x04009C9E RID: 40094
	AK_BankReadError,
	// Token: 0x04009C9F RID: 40095
	AK_InvalidSwitchType,
	// Token: 0x04009CA0 RID: 40096
	AK_VoiceDone,
	// Token: 0x04009CA1 RID: 40097
	AK_UnknownEnvironment,
	// Token: 0x04009CA2 RID: 40098
	AK_EnvironmentInUse,
	// Token: 0x04009CA3 RID: 40099
	AK_UnknownObject,
	// Token: 0x04009CA4 RID: 40100
	AK_NoConversionNeeded,
	// Token: 0x04009CA5 RID: 40101
	AK_FormatNotReady,
	// Token: 0x04009CA6 RID: 40102
	AK_WrongBankVersion,
	// Token: 0x04009CA7 RID: 40103
	AK_DataReadyNoProcess,
	// Token: 0x04009CA8 RID: 40104
	AK_FileNotFound,
	// Token: 0x04009CA9 RID: 40105
	AK_DeviceNotReady,
	// Token: 0x04009CAA RID: 40106
	AK_CouldNotCreateSecBuffer,
	// Token: 0x04009CAB RID: 40107
	AK_BankAlreadyLoaded,
	// Token: 0x04009CAC RID: 40108
	AK_RenderedFX = 71,
	// Token: 0x04009CAD RID: 40109
	AK_ProcessNeeded,
	// Token: 0x04009CAE RID: 40110
	AK_ProcessDone,
	// Token: 0x04009CAF RID: 40111
	AK_MemManagerNotInitialized,
	// Token: 0x04009CB0 RID: 40112
	AK_StreamMgrNotInitialized,
	// Token: 0x04009CB1 RID: 40113
	AK_SSEInstructionsNotSupported,
	// Token: 0x04009CB2 RID: 40114
	AK_Busy,
	// Token: 0x04009CB3 RID: 40115
	AK_UnsupportedChannelConfig,
	// Token: 0x04009CB4 RID: 40116
	AK_PluginMediaNotAvailable,
	// Token: 0x04009CB5 RID: 40117
	AK_MustBeVirtualized,
	// Token: 0x04009CB6 RID: 40118
	AK_CommandTooLarge,
	// Token: 0x04009CB7 RID: 40119
	AK_RejectedByFilter,
	// Token: 0x04009CB8 RID: 40120
	AK_InvalidCustomPlatformName,
	// Token: 0x04009CB9 RID: 40121
	AK_DLLCannotLoad,
	// Token: 0x04009CBA RID: 40122
	AK_DLLPathNotFound,
	// Token: 0x04009CBB RID: 40123
	AK_NoJavaVM,
	// Token: 0x04009CBC RID: 40124
	AK_OpenSLError,
	// Token: 0x04009CBD RID: 40125
	AK_PluginNotRegistered,
	// Token: 0x04009CBE RID: 40126
	AK_DataAlignmentError
}
