using System;
using System.Runtime.InteropServices;

// Token: 0x020018B3 RID: 6323
internal class AkSoundEnginePINVOKE
{
	// Token: 0x06009832 RID: 38962
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AK_SOUNDBANK_VERSION_get();

	// Token: 0x06009833 RID: 38963
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkExternalSourceInfo_iExternalSrcCookie_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009834 RID: 38964
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkExternalSourceInfo_iExternalSrcCookie_get(IntPtr jarg1);

	// Token: 0x06009835 RID: 38965
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkExternalSourceInfo_idCodec_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009836 RID: 38966
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkExternalSourceInfo_idCodec_get(IntPtr jarg1);

	// Token: 0x06009837 RID: 38967
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkExternalSourceInfo_szFile_set(IntPtr jarg1, [MarshalAs(UnmanagedType.LPWStr)] string jarg2);

	// Token: 0x06009838 RID: 38968
	[DllImport("AkSoundEngine")]
	public static extern string CSharp_AkExternalSourceInfo_szFile_get(IntPtr jarg1);

	// Token: 0x06009839 RID: 38969
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkExternalSourceInfo_pInMemory_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x0600983A RID: 38970
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkExternalSourceInfo_pInMemory_get(IntPtr jarg1);

	// Token: 0x0600983B RID: 38971
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkExternalSourceInfo_uiMemorySize_set(IntPtr jarg1, uint jarg2);

	// Token: 0x0600983C RID: 38972
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkExternalSourceInfo_uiMemorySize_get(IntPtr jarg1);

	// Token: 0x0600983D RID: 38973
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkExternalSourceInfo_idFile_set(IntPtr jarg1, uint jarg2);

	// Token: 0x0600983E RID: 38974
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkExternalSourceInfo_idFile_get(IntPtr jarg1);

	// Token: 0x0600983F RID: 38975
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkExternalSourceInfo__SWIG_0();

	// Token: 0x06009840 RID: 38976
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkExternalSourceInfo__SWIG_1(IntPtr jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06009841 RID: 38977
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkExternalSourceInfo__SWIG_2([MarshalAs(UnmanagedType.LPWStr)] string jarg1, uint jarg2, uint jarg3);

	// Token: 0x06009842 RID: 38978
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkExternalSourceInfo__SWIG_3(uint jarg1, uint jarg2, uint jarg3);

	// Token: 0x06009843 RID: 38979
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkExternalSourceInfo(IntPtr jarg1);

	// Token: 0x06009844 RID: 38980
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkVector_Zero(IntPtr jarg1);

	// Token: 0x06009845 RID: 38981
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkVector_X_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009846 RID: 38982
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkVector_X_get(IntPtr jarg1);

	// Token: 0x06009847 RID: 38983
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkVector_Y_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009848 RID: 38984
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkVector_Y_get(IntPtr jarg1);

	// Token: 0x06009849 RID: 38985
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkVector_Z_set(IntPtr jarg1, float jarg2);

	// Token: 0x0600984A RID: 38986
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkVector_Z_get(IntPtr jarg1);

	// Token: 0x0600984B RID: 38987
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkVector();

	// Token: 0x0600984C RID: 38988
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkVector(IntPtr jarg1);

	// Token: 0x0600984D RID: 38989
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkTransform_Position(IntPtr jarg1);

	// Token: 0x0600984E RID: 38990
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkTransform_OrientationFront(IntPtr jarg1);

	// Token: 0x0600984F RID: 38991
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkTransform_OrientationTop(IntPtr jarg1);

	// Token: 0x06009850 RID: 38992
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTransform_Set__SWIG_0(IntPtr jarg1, IntPtr jarg2, IntPtr jarg3, IntPtr jarg4);

	// Token: 0x06009851 RID: 38993
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTransform_Set__SWIG_1(IntPtr jarg1, float jarg2, float jarg3, float jarg4, float jarg5, float jarg6, float jarg7, float jarg8, float jarg9, float jarg10);

	// Token: 0x06009852 RID: 38994
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTransform_SetPosition__SWIG_0(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009853 RID: 38995
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTransform_SetPosition__SWIG_1(IntPtr jarg1, float jarg2, float jarg3, float jarg4);

	// Token: 0x06009854 RID: 38996
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTransform_SetOrientation__SWIG_0(IntPtr jarg1, IntPtr jarg2, IntPtr jarg3);

	// Token: 0x06009855 RID: 38997
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTransform_SetOrientation__SWIG_1(IntPtr jarg1, float jarg2, float jarg3, float jarg4, float jarg5, float jarg6, float jarg7);

	// Token: 0x06009856 RID: 38998
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkTransform();

	// Token: 0x06009857 RID: 38999
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkTransform(IntPtr jarg1);

	// Token: 0x06009858 RID: 39000
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkObstructionOcclusionValues_occlusion_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009859 RID: 39001
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkObstructionOcclusionValues_occlusion_get(IntPtr jarg1);

	// Token: 0x0600985A RID: 39002
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkObstructionOcclusionValues_obstruction_set(IntPtr jarg1, float jarg2);

	// Token: 0x0600985B RID: 39003
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkObstructionOcclusionValues_obstruction_get(IntPtr jarg1);

	// Token: 0x0600985C RID: 39004
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkObstructionOcclusionValues();

	// Token: 0x0600985D RID: 39005
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkObstructionOcclusionValues(IntPtr jarg1);

	// Token: 0x0600985E RID: 39006
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkChannelEmitter_position_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x0600985F RID: 39007
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkChannelEmitter_position_get(IntPtr jarg1);

	// Token: 0x06009860 RID: 39008
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkChannelEmitter_uInputChannels_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009861 RID: 39009
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkChannelEmitter_uInputChannels_get(IntPtr jarg1);

	// Token: 0x06009862 RID: 39010
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkChannelEmitter();

	// Token: 0x06009863 RID: 39011
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkChannelEmitter(IntPtr jarg1);

	// Token: 0x06009864 RID: 39012
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAuxSendValue_listenerID_set(IntPtr jarg1, ulong jarg2);

	// Token: 0x06009865 RID: 39013
	[DllImport("AkSoundEngine")]
	public static extern ulong CSharp_AkAuxSendValue_listenerID_get(IntPtr jarg1);

	// Token: 0x06009866 RID: 39014
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAuxSendValue_auxBusID_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009867 RID: 39015
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAuxSendValue_auxBusID_get(IntPtr jarg1);

	// Token: 0x06009868 RID: 39016
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAuxSendValue_fControlValue_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009869 RID: 39017
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkAuxSendValue_fControlValue_get(IntPtr jarg1);

	// Token: 0x0600986A RID: 39018
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAuxSendValue_Set(IntPtr jarg1, ulong jarg2, uint jarg3, float jarg4);

	// Token: 0x0600986B RID: 39019
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkAuxSendValue_IsSame(IntPtr jarg1, ulong jarg2, uint jarg3);

	// Token: 0x0600986C RID: 39020
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkAuxSendValue_GetSizeOf();

	// Token: 0x0600986D RID: 39021
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkAuxSendValue_SetGameObjectAuxSendValues(IntPtr jarg1, ulong jarg2, uint jarg3);

	// Token: 0x0600986E RID: 39022
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkAuxSendValue_GetGameObjectAuxSendValues(IntPtr jarg1, ulong jarg2, ref uint jarg3);

	// Token: 0x0600986F RID: 39023
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkAuxSendValue(IntPtr jarg1);

	// Token: 0x06009870 RID: 39024
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkRamp__SWIG_0();

	// Token: 0x06009871 RID: 39025
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkRamp__SWIG_1(float jarg1, float jarg2);

	// Token: 0x06009872 RID: 39026
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkRamp_fPrev_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009873 RID: 39027
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkRamp_fPrev_get(IntPtr jarg1);

	// Token: 0x06009874 RID: 39028
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkRamp_fNext_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009875 RID: 39029
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkRamp_fNext_get(IntPtr jarg1);

	// Token: 0x06009876 RID: 39030
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkRamp(IntPtr jarg1);

	// Token: 0x06009877 RID: 39031
	[DllImport("AkSoundEngine")]
	public static extern ushort CSharp_AK_INT_get();

	// Token: 0x06009878 RID: 39032
	[DllImport("AkSoundEngine")]
	public static extern ushort CSharp_AK_FLOAT_get();

	// Token: 0x06009879 RID: 39033
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AK_INTERLEAVED_get();

	// Token: 0x0600987A RID: 39034
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AK_NONINTERLEAVED_get();

	// Token: 0x0600987B RID: 39035
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AK_LE_NATIVE_BITSPERSAMPLE_get();

	// Token: 0x0600987C RID: 39036
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AK_LE_NATIVE_SAMPLETYPE_get();

	// Token: 0x0600987D RID: 39037
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AK_LE_NATIVE_INTERLEAVE_get();

	// Token: 0x0600987E RID: 39038
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAudioFormat_uSampleRate_set(IntPtr jarg1, uint jarg2);

	// Token: 0x0600987F RID: 39039
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAudioFormat_uSampleRate_get(IntPtr jarg1);

	// Token: 0x06009880 RID: 39040
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAudioFormat_channelConfig_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009881 RID: 39041
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkAudioFormat_channelConfig_get(IntPtr jarg1);

	// Token: 0x06009882 RID: 39042
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAudioFormat_uBitsPerSample_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009883 RID: 39043
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAudioFormat_uBitsPerSample_get(IntPtr jarg1);

	// Token: 0x06009884 RID: 39044
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAudioFormat_uBlockAlign_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009885 RID: 39045
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAudioFormat_uBlockAlign_get(IntPtr jarg1);

	// Token: 0x06009886 RID: 39046
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAudioFormat_uTypeID_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009887 RID: 39047
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAudioFormat_uTypeID_get(IntPtr jarg1);

	// Token: 0x06009888 RID: 39048
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAudioFormat_uInterleaveID_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009889 RID: 39049
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAudioFormat_uInterleaveID_get(IntPtr jarg1);

	// Token: 0x0600988A RID: 39050
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAudioFormat_GetNumChannels(IntPtr jarg1);

	// Token: 0x0600988B RID: 39051
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAudioFormat_GetBitsPerSample(IntPtr jarg1);

	// Token: 0x0600988C RID: 39052
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAudioFormat_GetBlockAlign(IntPtr jarg1);

	// Token: 0x0600988D RID: 39053
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAudioFormat_GetTypeID(IntPtr jarg1);

	// Token: 0x0600988E RID: 39054
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAudioFormat_GetInterleaveID(IntPtr jarg1);

	// Token: 0x0600988F RID: 39055
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAudioFormat_SetAll(IntPtr jarg1, uint jarg2, IntPtr jarg3, uint jarg4, uint jarg5, uint jarg6, uint jarg7);

	// Token: 0x06009890 RID: 39056
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkAudioFormat_IsChannelConfigSupported(IntPtr jarg1);

	// Token: 0x06009891 RID: 39057
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkAudioFormat();

	// Token: 0x06009892 RID: 39058
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkAudioFormat(IntPtr jarg1);

	// Token: 0x06009893 RID: 39059
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkIterator_pItem_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009894 RID: 39060
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkIterator_pItem_get(IntPtr jarg1);

	// Token: 0x06009895 RID: 39061
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkIterator_NextIter(IntPtr jarg1);

	// Token: 0x06009896 RID: 39062
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkIterator_PrevIter(IntPtr jarg1);

	// Token: 0x06009897 RID: 39063
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkIterator_GetItem(IntPtr jarg1);

	// Token: 0x06009898 RID: 39064
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkIterator_IsEqualTo(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009899 RID: 39065
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkIterator_IsDifferentFrom(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x0600989A RID: 39066
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkIterator();

	// Token: 0x0600989B RID: 39067
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkIterator(IntPtr jarg1);

	// Token: 0x0600989C RID: 39068
	[DllImport("AkSoundEngine")]
	public static extern int CSharp__ArrayPoolDefault_Get();

	// Token: 0x0600989D RID: 39069
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new__ArrayPoolDefault();

	// Token: 0x0600989E RID: 39070
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete__ArrayPoolDefault(IntPtr jarg1);

	// Token: 0x0600989F RID: 39071
	[DllImport("AkSoundEngine")]
	public static extern int CSharp__ArrayPoolLEngineDefault_Get();

	// Token: 0x060098A0 RID: 39072
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new__ArrayPoolLEngineDefault();

	// Token: 0x060098A1 RID: 39073
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete__ArrayPoolLEngineDefault(IntPtr jarg1);

	// Token: 0x060098A2 RID: 39074
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkPlaylistItem__SWIG_0();

	// Token: 0x060098A3 RID: 39075
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkPlaylistItem__SWIG_1(IntPtr jarg1);

	// Token: 0x060098A4 RID: 39076
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkPlaylistItem(IntPtr jarg1);

	// Token: 0x060098A5 RID: 39077
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistItem_Assign(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098A6 RID: 39078
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkPlaylistItem_IsEqualTo(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098A7 RID: 39079
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPlaylistItem_SetExternalSources(IntPtr jarg1, uint jarg2, IntPtr jarg3);

	// Token: 0x060098A8 RID: 39080
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlaylistItem_audioNodeID_set(IntPtr jarg1, uint jarg2);

	// Token: 0x060098A9 RID: 39081
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkPlaylistItem_audioNodeID_get(IntPtr jarg1);

	// Token: 0x060098AA RID: 39082
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlaylistItem_msDelay_set(IntPtr jarg1, int jarg2);

	// Token: 0x060098AB RID: 39083
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPlaylistItem_msDelay_get(IntPtr jarg1);

	// Token: 0x060098AC RID: 39084
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlaylistItem_pCustomInfo_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098AD RID: 39085
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistItem_pCustomInfo_get(IntPtr jarg1);

	// Token: 0x060098AE RID: 39086
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkPlaylistArray();

	// Token: 0x060098AF RID: 39087
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkPlaylistArray(IntPtr jarg1);

	// Token: 0x060098B0 RID: 39088
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistArray_Begin(IntPtr jarg1);

	// Token: 0x060098B1 RID: 39089
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistArray_End(IntPtr jarg1);

	// Token: 0x060098B2 RID: 39090
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistArray_FindEx(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098B3 RID: 39091
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistArray_Erase__SWIG_0(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098B4 RID: 39092
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlaylistArray_Erase__SWIG_1(IntPtr jarg1, uint jarg2);

	// Token: 0x060098B5 RID: 39093
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistArray_EraseSwap(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098B6 RID: 39094
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPlaylistArray_Reserve(IntPtr jarg1, uint jarg2);

	// Token: 0x060098B7 RID: 39095
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkPlaylistArray_Reserved(IntPtr jarg1);

	// Token: 0x060098B8 RID: 39096
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlaylistArray_Term(IntPtr jarg1);

	// Token: 0x060098B9 RID: 39097
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkPlaylistArray_Length(IntPtr jarg1);

	// Token: 0x060098BA RID: 39098
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkPlaylistArray_IsEmpty(IntPtr jarg1);

	// Token: 0x060098BB RID: 39099
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistArray_Exists(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098BC RID: 39100
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistArray_AddLast__SWIG_0(IntPtr jarg1);

	// Token: 0x060098BD RID: 39101
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistArray_AddLast__SWIG_1(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098BE RID: 39102
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistArray_Last(IntPtr jarg1);

	// Token: 0x060098BF RID: 39103
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlaylistArray_RemoveLast(IntPtr jarg1);

	// Token: 0x060098C0 RID: 39104
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPlaylistArray_Remove(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098C1 RID: 39105
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPlaylistArray_RemoveSwap(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098C2 RID: 39106
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlaylistArray_RemoveAll(IntPtr jarg1);

	// Token: 0x060098C3 RID: 39107
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistArray_ItemAtIndex(IntPtr jarg1, uint jarg2);

	// Token: 0x060098C4 RID: 39108
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylistArray_Insert(IntPtr jarg1, uint jarg2);

	// Token: 0x060098C5 RID: 39109
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkPlaylistArray_GrowArray__SWIG_0(IntPtr jarg1, uint jarg2);

	// Token: 0x060098C6 RID: 39110
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkPlaylistArray_GrowArray__SWIG_1(IntPtr jarg1);

	// Token: 0x060098C7 RID: 39111
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkPlaylistArray_Resize(IntPtr jarg1, uint jarg2);

	// Token: 0x060098C8 RID: 39112
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlaylistArray_Transfer(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098C9 RID: 39113
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPlaylistArray_Copy(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098CA RID: 39114
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPlaylist_Enqueue__SWIG_0(IntPtr jarg1, uint jarg2, int jarg3, IntPtr jarg4, uint jarg5, IntPtr jarg6);

	// Token: 0x060098CB RID: 39115
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPlaylist_Enqueue__SWIG_1(IntPtr jarg1, uint jarg2, int jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x060098CC RID: 39116
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPlaylist_Enqueue__SWIG_2(IntPtr jarg1, uint jarg2, int jarg3, IntPtr jarg4);

	// Token: 0x060098CD RID: 39117
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPlaylist_Enqueue__SWIG_3(IntPtr jarg1, uint jarg2, int jarg3);

	// Token: 0x060098CE RID: 39118
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPlaylist_Enqueue__SWIG_4(IntPtr jarg1, uint jarg2);

	// Token: 0x060098CF RID: 39119
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkPlaylist();

	// Token: 0x060098D0 RID: 39120
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkPlaylist(IntPtr jarg1);

	// Token: 0x060098D1 RID: 39121
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_DynamicSequenceOpen__SWIG_0(ulong jarg1, uint jarg2, IntPtr jarg3, IntPtr jarg4, int jarg5);

	// Token: 0x060098D2 RID: 39122
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_DynamicSequenceOpen__SWIG_1(ulong jarg1, uint jarg2, IntPtr jarg3, IntPtr jarg4);

	// Token: 0x060098D3 RID: 39123
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_DynamicSequenceOpen__SWIG_2(ulong jarg1, uint jarg2);

	// Token: 0x060098D4 RID: 39124
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_DynamicSequenceOpen__SWIG_3(ulong jarg1);

	// Token: 0x060098D5 RID: 39125
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequenceClose(uint jarg1);

	// Token: 0x060098D6 RID: 39126
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequencePlay__SWIG_0(uint jarg1, int jarg2, int jarg3);

	// Token: 0x060098D7 RID: 39127
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequencePlay__SWIG_1(uint jarg1, int jarg2);

	// Token: 0x060098D8 RID: 39128
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequencePlay__SWIG_2(uint jarg1);

	// Token: 0x060098D9 RID: 39129
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequencePause__SWIG_0(uint jarg1, int jarg2, int jarg3);

	// Token: 0x060098DA RID: 39130
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequencePause__SWIG_1(uint jarg1, int jarg2);

	// Token: 0x060098DB RID: 39131
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequencePause__SWIG_2(uint jarg1);

	// Token: 0x060098DC RID: 39132
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequenceResume__SWIG_0(uint jarg1, int jarg2, int jarg3);

	// Token: 0x060098DD RID: 39133
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequenceResume__SWIG_1(uint jarg1, int jarg2);

	// Token: 0x060098DE RID: 39134
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequenceResume__SWIG_2(uint jarg1);

	// Token: 0x060098DF RID: 39135
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequenceStop__SWIG_0(uint jarg1, int jarg2, int jarg3);

	// Token: 0x060098E0 RID: 39136
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequenceStop__SWIG_1(uint jarg1, int jarg2);

	// Token: 0x060098E1 RID: 39137
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequenceStop__SWIG_2(uint jarg1);

	// Token: 0x060098E2 RID: 39138
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequenceBreak(uint jarg1);

	// Token: 0x060098E3 RID: 39139
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequenceGetPauseTimes(uint jarg1, out uint jarg2, out uint jarg3);

	// Token: 0x060098E4 RID: 39140
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_DynamicSequenceLockPlaylist(uint jarg1);

	// Token: 0x060098E5 RID: 39141
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_DynamicSequenceUnlockPlaylist(uint jarg1);

	// Token: 0x060098E6 RID: 39142
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkOutputSettings__SWIG_0();

	// Token: 0x060098E7 RID: 39143
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkOutputSettings__SWIG_1([MarshalAs(UnmanagedType.LPStr)] string jarg1, uint jarg2, IntPtr jarg3, int jarg4);

	// Token: 0x060098E8 RID: 39144
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkOutputSettings__SWIG_2([MarshalAs(UnmanagedType.LPStr)] string jarg1, uint jarg2, IntPtr jarg3);

	// Token: 0x060098E9 RID: 39145
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkOutputSettings__SWIG_3([MarshalAs(UnmanagedType.LPStr)] string jarg1, uint jarg2);

	// Token: 0x060098EA RID: 39146
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkOutputSettings__SWIG_4([MarshalAs(UnmanagedType.LPStr)] string jarg1);

	// Token: 0x060098EB RID: 39147
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkOutputSettings_audioDeviceShareset_set(IntPtr jarg1, uint jarg2);

	// Token: 0x060098EC RID: 39148
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkOutputSettings_audioDeviceShareset_get(IntPtr jarg1);

	// Token: 0x060098ED RID: 39149
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkOutputSettings_idDevice_set(IntPtr jarg1, uint jarg2);

	// Token: 0x060098EE RID: 39150
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkOutputSettings_idDevice_get(IntPtr jarg1);

	// Token: 0x060098EF RID: 39151
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkOutputSettings_ePanningRule_set(IntPtr jarg1, int jarg2);

	// Token: 0x060098F0 RID: 39152
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkOutputSettings_ePanningRule_get(IntPtr jarg1);

	// Token: 0x060098F1 RID: 39153
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkOutputSettings_channelConfig_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098F2 RID: 39154
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkOutputSettings_channelConfig_get(IntPtr jarg1);

	// Token: 0x060098F3 RID: 39155
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkOutputSettings(IntPtr jarg1);

	// Token: 0x060098F4 RID: 39156
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_pfnAssertHook_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x060098F5 RID: 39157
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkInitSettings_pfnAssertHook_get(IntPtr jarg1);

	// Token: 0x060098F6 RID: 39158
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_uMaxNumPaths_set(IntPtr jarg1, uint jarg2);

	// Token: 0x060098F7 RID: 39159
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkInitSettings_uMaxNumPaths_get(IntPtr jarg1);

	// Token: 0x060098F8 RID: 39160
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_uDefaultPoolSize_set(IntPtr jarg1, uint jarg2);

	// Token: 0x060098F9 RID: 39161
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkInitSettings_uDefaultPoolSize_get(IntPtr jarg1);

	// Token: 0x060098FA RID: 39162
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_fDefaultPoolRatioThreshold_set(IntPtr jarg1, float jarg2);

	// Token: 0x060098FB RID: 39163
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkInitSettings_fDefaultPoolRatioThreshold_get(IntPtr jarg1);

	// Token: 0x060098FC RID: 39164
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_uCommandQueueSize_set(IntPtr jarg1, uint jarg2);

	// Token: 0x060098FD RID: 39165
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkInitSettings_uCommandQueueSize_get(IntPtr jarg1);

	// Token: 0x060098FE RID: 39166
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_uPrepareEventMemoryPoolID_set(IntPtr jarg1, int jarg2);

	// Token: 0x060098FF RID: 39167
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkInitSettings_uPrepareEventMemoryPoolID_get(IntPtr jarg1);

	// Token: 0x06009900 RID: 39168
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_bEnableGameSyncPreparation_set(IntPtr jarg1, bool jarg2);

	// Token: 0x06009901 RID: 39169
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkInitSettings_bEnableGameSyncPreparation_get(IntPtr jarg1);

	// Token: 0x06009902 RID: 39170
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_uContinuousPlaybackLookAhead_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009903 RID: 39171
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkInitSettings_uContinuousPlaybackLookAhead_get(IntPtr jarg1);

	// Token: 0x06009904 RID: 39172
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_uNumSamplesPerFrame_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009905 RID: 39173
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkInitSettings_uNumSamplesPerFrame_get(IntPtr jarg1);

	// Token: 0x06009906 RID: 39174
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_uMonitorPoolSize_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009907 RID: 39175
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkInitSettings_uMonitorPoolSize_get(IntPtr jarg1);

	// Token: 0x06009908 RID: 39176
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_uMonitorQueuePoolSize_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009909 RID: 39177
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkInitSettings_uMonitorQueuePoolSize_get(IntPtr jarg1);

	// Token: 0x0600990A RID: 39178
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_settingsMainOutput_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x0600990B RID: 39179
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkInitSettings_settingsMainOutput_get(IntPtr jarg1);

	// Token: 0x0600990C RID: 39180
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_uMaxHardwareTimeoutMs_set(IntPtr jarg1, uint jarg2);

	// Token: 0x0600990D RID: 39181
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkInitSettings_uMaxHardwareTimeoutMs_get(IntPtr jarg1);

	// Token: 0x0600990E RID: 39182
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_bUseSoundBankMgrThread_set(IntPtr jarg1, bool jarg2);

	// Token: 0x0600990F RID: 39183
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkInitSettings_bUseSoundBankMgrThread_get(IntPtr jarg1);

	// Token: 0x06009910 RID: 39184
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_bUseLEngineThread_set(IntPtr jarg1, bool jarg2);

	// Token: 0x06009911 RID: 39185
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkInitSettings_bUseLEngineThread_get(IntPtr jarg1);

	// Token: 0x06009912 RID: 39186
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkInitSettings_szPluginDLLPath_set(IntPtr jarg1, [MarshalAs(UnmanagedType.LPWStr)] string jarg2);

	// Token: 0x06009913 RID: 39187
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkInitSettings_szPluginDLLPath_get(IntPtr jarg1);

	// Token: 0x06009914 RID: 39188
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkInitSettings();

	// Token: 0x06009915 RID: 39189
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkInitSettings(IntPtr jarg1);

	// Token: 0x06009916 RID: 39190
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSourceSettings_sourceID_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009917 RID: 39191
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkSourceSettings_sourceID_get(IntPtr jarg1);

	// Token: 0x06009918 RID: 39192
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSourceSettings_pMediaMemory_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009919 RID: 39193
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkSourceSettings_pMediaMemory_get(IntPtr jarg1);

	// Token: 0x0600991A RID: 39194
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSourceSettings_uMediaSize_set(IntPtr jarg1, uint jarg2);

	// Token: 0x0600991B RID: 39195
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkSourceSettings_uMediaSize_get(IntPtr jarg1);

	// Token: 0x0600991C RID: 39196
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkSourceSettings();

	// Token: 0x0600991D RID: 39197
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkSourceSettings(IntPtr jarg1);

	// Token: 0x0600991E RID: 39198
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAudioSettings_uNumSamplesPerFrame_set(IntPtr jarg1, uint jarg2);

	// Token: 0x0600991F RID: 39199
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAudioSettings_uNumSamplesPerFrame_get(IntPtr jarg1);

	// Token: 0x06009920 RID: 39200
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkAudioSettings_uNumSamplesPerSecond_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009921 RID: 39201
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkAudioSettings_uNumSamplesPerSecond_get(IntPtr jarg1);

	// Token: 0x06009922 RID: 39202
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkAudioSettings();

	// Token: 0x06009923 RID: 39203
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkAudioSettings(IntPtr jarg1);

	// Token: 0x06009924 RID: 39204
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_IsInitialized();

	// Token: 0x06009925 RID: 39205
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetAudioSettings(IntPtr jarg1);

	// Token: 0x06009926 RID: 39206
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_GetSpeakerConfiguration__SWIG_0(ulong jarg1);

	// Token: 0x06009927 RID: 39207
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_GetSpeakerConfiguration__SWIG_1();

	// Token: 0x06009928 RID: 39208
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetPanningRule__SWIG_0(out int jarg1, ulong jarg2);

	// Token: 0x06009929 RID: 39209
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetPanningRule__SWIG_1(out int jarg1);

	// Token: 0x0600992A RID: 39210
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetPanningRule__SWIG_0(int jarg1, ulong jarg2);

	// Token: 0x0600992B RID: 39211
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetPanningRule__SWIG_1(int jarg1);

	// Token: 0x0600992C RID: 39212
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetSpeakerAngles__SWIG_0([MarshalAs(UnmanagedType.LPArray)] [In] [Out] float[] jarg1, ref uint jarg2, out float jarg3, ulong jarg4);

	// Token: 0x0600992D RID: 39213
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetSpeakerAngles__SWIG_1([MarshalAs(UnmanagedType.LPArray)] [In] [Out] float[] jarg1, ref uint jarg2, out float jarg3);

	// Token: 0x0600992E RID: 39214
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetSpeakerAngles__SWIG_0([MarshalAs(UnmanagedType.LPArray)] [In] float[] jarg1, uint jarg2, float jarg3, ulong jarg4);

	// Token: 0x0600992F RID: 39215
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetSpeakerAngles__SWIG_1([MarshalAs(UnmanagedType.LPArray)] [In] float[] jarg1, uint jarg2, float jarg3);

	// Token: 0x06009930 RID: 39216
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetVolumeThreshold(float jarg1);

	// Token: 0x06009931 RID: 39217
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetMaxNumVoicesLimit(ushort jarg1);

	// Token: 0x06009932 RID: 39218
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RenderAudio__SWIG_0(bool jarg1);

	// Token: 0x06009933 RID: 39219
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RenderAudio__SWIG_1();

	// Token: 0x06009934 RID: 39220
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RegisterPluginDLL([MarshalAs(UnmanagedType.LPWStr)] string jarg1);

	// Token: 0x06009935 RID: 39221
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_GetIDFromString__SWIG_0([MarshalAs(UnmanagedType.LPWStr)] string jarg1);

	// Token: 0x06009936 RID: 39222
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_PostEvent__SWIG_0(uint jarg1, ulong jarg2, uint jarg3, IntPtr jarg4, IntPtr jarg5, uint jarg6, IntPtr jarg7, uint jarg8);

	// Token: 0x06009937 RID: 39223
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_PostEvent__SWIG_1(uint jarg1, ulong jarg2, uint jarg3, IntPtr jarg4, IntPtr jarg5, uint jarg6, IntPtr jarg7);

	// Token: 0x06009938 RID: 39224
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_PostEvent__SWIG_2(uint jarg1, ulong jarg2, uint jarg3, IntPtr jarg4, IntPtr jarg5, uint jarg6);

	// Token: 0x06009939 RID: 39225
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_PostEvent__SWIG_3(uint jarg1, ulong jarg2, uint jarg3, IntPtr jarg4, IntPtr jarg5);

	// Token: 0x0600993A RID: 39226
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_PostEvent__SWIG_4(uint jarg1, ulong jarg2, uint jarg3);

	// Token: 0x0600993B RID: 39227
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_PostEvent__SWIG_5(uint jarg1, ulong jarg2);

	// Token: 0x0600993C RID: 39228
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_PostEvent__SWIG_6([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, uint jarg3, IntPtr jarg4, IntPtr jarg5, uint jarg6, IntPtr jarg7, uint jarg8);

	// Token: 0x0600993D RID: 39229
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_PostEvent__SWIG_7([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, uint jarg3, IntPtr jarg4, IntPtr jarg5, uint jarg6, IntPtr jarg7);

	// Token: 0x0600993E RID: 39230
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_PostEvent__SWIG_8([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, uint jarg3, IntPtr jarg4, IntPtr jarg5, uint jarg6);

	// Token: 0x0600993F RID: 39231
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_PostEvent__SWIG_9([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, uint jarg3, IntPtr jarg4, IntPtr jarg5);

	// Token: 0x06009940 RID: 39232
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_PostEvent__SWIG_10([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, uint jarg3);

	// Token: 0x06009941 RID: 39233
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_PostEvent__SWIG_11([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2);

	// Token: 0x06009942 RID: 39234
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ExecuteActionOnEvent__SWIG_0(uint jarg1, int jarg2, ulong jarg3, int jarg4, int jarg5, uint jarg6);

	// Token: 0x06009943 RID: 39235
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ExecuteActionOnEvent__SWIG_1(uint jarg1, int jarg2, ulong jarg3, int jarg4, int jarg5);

	// Token: 0x06009944 RID: 39236
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ExecuteActionOnEvent__SWIG_2(uint jarg1, int jarg2, ulong jarg3, int jarg4);

	// Token: 0x06009945 RID: 39237
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ExecuteActionOnEvent__SWIG_3(uint jarg1, int jarg2, ulong jarg3);

	// Token: 0x06009946 RID: 39238
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ExecuteActionOnEvent__SWIG_4(uint jarg1, int jarg2);

	// Token: 0x06009947 RID: 39239
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ExecuteActionOnEvent__SWIG_5([MarshalAs(UnmanagedType.LPWStr)] string jarg1, int jarg2, ulong jarg3, int jarg4, int jarg5, uint jarg6);

	// Token: 0x06009948 RID: 39240
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ExecuteActionOnEvent__SWIG_6([MarshalAs(UnmanagedType.LPWStr)] string jarg1, int jarg2, ulong jarg3, int jarg4, int jarg5);

	// Token: 0x06009949 RID: 39241
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ExecuteActionOnEvent__SWIG_7([MarshalAs(UnmanagedType.LPWStr)] string jarg1, int jarg2, ulong jarg3, int jarg4);

	// Token: 0x0600994A RID: 39242
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ExecuteActionOnEvent__SWIG_8([MarshalAs(UnmanagedType.LPWStr)] string jarg1, int jarg2, ulong jarg3);

	// Token: 0x0600994B RID: 39243
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ExecuteActionOnEvent__SWIG_9([MarshalAs(UnmanagedType.LPWStr)] string jarg1, int jarg2);

	// Token: 0x0600994C RID: 39244
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostMIDIOnEvent(uint jarg1, ulong jarg2, IntPtr jarg3, ushort jarg4);

	// Token: 0x0600994D RID: 39245
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_StopMIDIOnEvent__SWIG_0(uint jarg1, ulong jarg2);

	// Token: 0x0600994E RID: 39246
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_StopMIDIOnEvent__SWIG_1(uint jarg1);

	// Token: 0x0600994F RID: 39247
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_StopMIDIOnEvent__SWIG_2();

	// Token: 0x06009950 RID: 39248
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PinEventInStreamCache__SWIG_0(uint jarg1, char jarg2, char jarg3);

	// Token: 0x06009951 RID: 39249
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PinEventInStreamCache__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, char jarg2, char jarg3);

	// Token: 0x06009952 RID: 39250
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnpinEventInStreamCache__SWIG_0(uint jarg1);

	// Token: 0x06009953 RID: 39251
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnpinEventInStreamCache__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1);

	// Token: 0x06009954 RID: 39252
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetBufferStatusForPinnedEvent__SWIG_0(uint jarg1, out float jarg2, out int jarg3);

	// Token: 0x06009955 RID: 39253
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetBufferStatusForPinnedEvent__SWIG_1([MarshalAs(UnmanagedType.LPStr)] string jarg1, out float jarg2, out int jarg3);

	// Token: 0x06009956 RID: 39254
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SeekOnEvent__SWIG_0(uint jarg1, ulong jarg2, int jarg3, bool jarg4, uint jarg5);

	// Token: 0x06009957 RID: 39255
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SeekOnEvent__SWIG_1(uint jarg1, ulong jarg2, int jarg3, bool jarg4);

	// Token: 0x06009958 RID: 39256
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SeekOnEvent__SWIG_2(uint jarg1, ulong jarg2, int jarg3);

	// Token: 0x06009959 RID: 39257
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SeekOnEvent__SWIG_3([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, int jarg3, bool jarg4, uint jarg5);

	// Token: 0x0600995A RID: 39258
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SeekOnEvent__SWIG_4([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, int jarg3, bool jarg4);

	// Token: 0x0600995B RID: 39259
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SeekOnEvent__SWIG_5([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, int jarg3);

	// Token: 0x0600995C RID: 39260
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SeekOnEvent__SWIG_9(uint jarg1, ulong jarg2, float jarg3, bool jarg4, uint jarg5);

	// Token: 0x0600995D RID: 39261
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SeekOnEvent__SWIG_10(uint jarg1, ulong jarg2, float jarg3, bool jarg4);

	// Token: 0x0600995E RID: 39262
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SeekOnEvent__SWIG_11(uint jarg1, ulong jarg2, float jarg3);

	// Token: 0x0600995F RID: 39263
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SeekOnEvent__SWIG_12([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, float jarg3, bool jarg4, uint jarg5);

	// Token: 0x06009960 RID: 39264
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SeekOnEvent__SWIG_13([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, float jarg3, bool jarg4);

	// Token: 0x06009961 RID: 39265
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SeekOnEvent__SWIG_14([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, float jarg3);

	// Token: 0x06009962 RID: 39266
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_CancelEventCallbackCookie(IntPtr jarg1);

	// Token: 0x06009963 RID: 39267
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_CancelEventCallbackGameObject(ulong jarg1);

	// Token: 0x06009964 RID: 39268
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_CancelEventCallback(uint jarg1);

	// Token: 0x06009965 RID: 39269
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetSourcePlayPosition__SWIG_0(uint jarg1, out int jarg2, bool jarg3);

	// Token: 0x06009966 RID: 39270
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetSourcePlayPosition__SWIG_1(uint jarg1, out int jarg2);

	// Token: 0x06009967 RID: 39271
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetSourceStreamBuffering(uint jarg1, out int jarg2, out int jarg3);

	// Token: 0x06009968 RID: 39272
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_StopAll__SWIG_0(ulong jarg1);

	// Token: 0x06009969 RID: 39273
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_StopAll__SWIG_1();

	// Token: 0x0600996A RID: 39274
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_StopPlayingID__SWIG_0(uint jarg1, int jarg2, int jarg3);

	// Token: 0x0600996B RID: 39275
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_StopPlayingID__SWIG_1(uint jarg1, int jarg2);

	// Token: 0x0600996C RID: 39276
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_StopPlayingID__SWIG_2(uint jarg1);

	// Token: 0x0600996D RID: 39277
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_SetRandomSeed(uint jarg1);

	// Token: 0x0600996E RID: 39278
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_MuteBackgroundMusic(bool jarg1);

	// Token: 0x0600996F RID: 39279
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_GetBackgroundMusicMute();

	// Token: 0x06009970 RID: 39280
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SendPluginCustomGameData(uint jarg1, ulong jarg2, int jarg3, uint jarg4, uint jarg5, IntPtr jarg6, uint jarg7);

	// Token: 0x06009971 RID: 39281
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnregisterAllGameObj();

	// Token: 0x06009972 RID: 39282
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetMultiplePositions__SWIG_0(ulong jarg1, IntPtr jarg2, ushort jarg3, int jarg4);

	// Token: 0x06009973 RID: 39283
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetMultiplePositions__SWIG_1(ulong jarg1, IntPtr jarg2, ushort jarg3);

	// Token: 0x06009974 RID: 39284
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetMultiplePositions__SWIG_2(ulong jarg1, IntPtr jarg2, ushort jarg3, int jarg4);

	// Token: 0x06009975 RID: 39285
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetMultiplePositions__SWIG_3(ulong jarg1, IntPtr jarg2, ushort jarg3);

	// Token: 0x06009976 RID: 39286
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetScalingFactor(ulong jarg1, float jarg2);

	// Token: 0x06009977 RID: 39287
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ClearBanks();

	// Token: 0x06009978 RID: 39288
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetBankLoadIOSettings(float jarg1, char jarg2);

	// Token: 0x06009979 RID: 39289
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_LoadBank__SWIG_0([MarshalAs(UnmanagedType.LPWStr)] string jarg1, int jarg2, out uint jarg3);

	// Token: 0x0600997A RID: 39290
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_LoadBank__SWIG_1(uint jarg1, int jarg2);

	// Token: 0x0600997B RID: 39291
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_LoadBank__SWIG_2(IntPtr jarg1, uint jarg2, out uint jarg3);

	// Token: 0x0600997C RID: 39292
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_LoadBank__SWIG_3(IntPtr jarg1, uint jarg2, int jarg3, out uint jarg4);

	// Token: 0x0600997D RID: 39293
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_LoadBank__SWIG_4([MarshalAs(UnmanagedType.LPWStr)] string jarg1, IntPtr jarg2, IntPtr jarg3, int jarg4, out uint jarg5);

	// Token: 0x0600997E RID: 39294
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_LoadBank__SWIG_5(uint jarg1, IntPtr jarg2, IntPtr jarg3, int jarg4);

	// Token: 0x0600997F RID: 39295
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_LoadBank__SWIG_6(IntPtr jarg1, uint jarg2, IntPtr jarg3, IntPtr jarg4, out uint jarg5);

	// Token: 0x06009980 RID: 39296
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_LoadBank__SWIG_7(IntPtr jarg1, uint jarg2, IntPtr jarg3, IntPtr jarg4, int jarg5, out uint jarg6);

	// Token: 0x06009981 RID: 39297
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnloadBank__SWIG_0([MarshalAs(UnmanagedType.LPWStr)] string jarg1, IntPtr jarg2, out int jarg3);

	// Token: 0x06009982 RID: 39298
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnloadBank__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, IntPtr jarg2);

	// Token: 0x06009983 RID: 39299
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnloadBank__SWIG_4(uint jarg1, IntPtr jarg2, out int jarg3);

	// Token: 0x06009984 RID: 39300
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnloadBank__SWIG_5(uint jarg1, IntPtr jarg2);

	// Token: 0x06009985 RID: 39301
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnloadBank__SWIG_6([MarshalAs(UnmanagedType.LPWStr)] string jarg1, IntPtr jarg2, IntPtr jarg3, IntPtr jarg4);

	// Token: 0x06009986 RID: 39302
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnloadBank__SWIG_8(uint jarg1, IntPtr jarg2, IntPtr jarg3, IntPtr jarg4);

	// Token: 0x06009987 RID: 39303
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_CancelBankCallbackCookie(IntPtr jarg1);

	// Token: 0x06009988 RID: 39304
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareBank__SWIG_0(int jarg1, [MarshalAs(UnmanagedType.LPWStr)] string jarg2, int jarg3);

	// Token: 0x06009989 RID: 39305
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareBank__SWIG_1(int jarg1, [MarshalAs(UnmanagedType.LPWStr)] string jarg2);

	// Token: 0x0600998A RID: 39306
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareBank__SWIG_4(int jarg1, uint jarg2, int jarg3);

	// Token: 0x0600998B RID: 39307
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareBank__SWIG_5(int jarg1, uint jarg2);

	// Token: 0x0600998C RID: 39308
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareBank__SWIG_6(int jarg1, [MarshalAs(UnmanagedType.LPWStr)] string jarg2, IntPtr jarg3, IntPtr jarg4, int jarg5);

	// Token: 0x0600998D RID: 39309
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareBank__SWIG_7(int jarg1, [MarshalAs(UnmanagedType.LPWStr)] string jarg2, IntPtr jarg3, IntPtr jarg4);

	// Token: 0x0600998E RID: 39310
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareBank__SWIG_10(int jarg1, uint jarg2, IntPtr jarg3, IntPtr jarg4, int jarg5);

	// Token: 0x0600998F RID: 39311
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareBank__SWIG_11(int jarg1, uint jarg2, IntPtr jarg3, IntPtr jarg4);

	// Token: 0x06009990 RID: 39312
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ClearPreparedEvents();

	// Token: 0x06009991 RID: 39313
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareEvent__SWIG_0(int jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x06009992 RID: 39314
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareEvent__SWIG_1(int jarg1, [MarshalAs(UnmanagedType.LPArray)] [In] uint[] jarg2, uint jarg3);

	// Token: 0x06009993 RID: 39315
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareEvent__SWIG_2(int jarg1, IntPtr jarg2, uint jarg3, IntPtr jarg4, IntPtr jarg5);

	// Token: 0x06009994 RID: 39316
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareEvent__SWIG_3(int jarg1, [MarshalAs(UnmanagedType.LPArray)] [In] uint[] jarg2, uint jarg3, IntPtr jarg4, IntPtr jarg5);

	// Token: 0x06009995 RID: 39317
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetMedia(IntPtr jarg1, uint jarg2);

	// Token: 0x06009996 RID: 39318
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnsetMedia(IntPtr jarg1, uint jarg2);

	// Token: 0x06009997 RID: 39319
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareGameSyncs__SWIG_0(int jarg1, int jarg2, [MarshalAs(UnmanagedType.LPWStr)] string jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x06009998 RID: 39320
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareGameSyncs__SWIG_1(int jarg1, int jarg2, uint jarg3, [MarshalAs(UnmanagedType.LPArray)] [In] uint[] jarg4, uint jarg5);

	// Token: 0x06009999 RID: 39321
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareGameSyncs__SWIG_2(int jarg1, int jarg2, [MarshalAs(UnmanagedType.LPWStr)] string jarg3, IntPtr jarg4, uint jarg5, IntPtr jarg6, IntPtr jarg7);

	// Token: 0x0600999A RID: 39322
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PrepareGameSyncs__SWIG_3(int jarg1, int jarg2, uint jarg3, [MarshalAs(UnmanagedType.LPArray)] [In] uint[] jarg4, uint jarg5, IntPtr jarg6, IntPtr jarg7);

	// Token: 0x0600999B RID: 39323
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AddListener(ulong jarg1, ulong jarg2);

	// Token: 0x0600999C RID: 39324
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RemoveListener(ulong jarg1, ulong jarg2);

	// Token: 0x0600999D RID: 39325
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AddDefaultListener(ulong jarg1);

	// Token: 0x0600999E RID: 39326
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RemoveDefaultListener(ulong jarg1);

	// Token: 0x0600999F RID: 39327
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ResetListenersToDefault(ulong jarg1);

	// Token: 0x060099A0 RID: 39328
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetListenerSpatialization__SWIG_0(ulong jarg1, bool jarg2, IntPtr jarg3, [MarshalAs(UnmanagedType.LPArray)] [In] float[] jarg4);

	// Token: 0x060099A1 RID: 39329
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetListenerSpatialization__SWIG_1(ulong jarg1, bool jarg2, IntPtr jarg3);

	// Token: 0x060099A2 RID: 39330
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValue__SWIG_0(uint jarg1, float jarg2, ulong jarg3, int jarg4, int jarg5, bool jarg6);

	// Token: 0x060099A3 RID: 39331
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValue__SWIG_1(uint jarg1, float jarg2, ulong jarg3, int jarg4, int jarg5);

	// Token: 0x060099A4 RID: 39332
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValue__SWIG_2(uint jarg1, float jarg2, ulong jarg3, int jarg4);

	// Token: 0x060099A5 RID: 39333
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValue__SWIG_3(uint jarg1, float jarg2, ulong jarg3);

	// Token: 0x060099A6 RID: 39334
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValue__SWIG_4(uint jarg1, float jarg2);

	// Token: 0x060099A7 RID: 39335
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValue__SWIG_5([MarshalAs(UnmanagedType.LPWStr)] string jarg1, float jarg2, ulong jarg3, int jarg4, int jarg5, bool jarg6);

	// Token: 0x060099A8 RID: 39336
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValue__SWIG_6([MarshalAs(UnmanagedType.LPWStr)] string jarg1, float jarg2, ulong jarg3, int jarg4, int jarg5);

	// Token: 0x060099A9 RID: 39337
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValue__SWIG_7([MarshalAs(UnmanagedType.LPWStr)] string jarg1, float jarg2, ulong jarg3, int jarg4);

	// Token: 0x060099AA RID: 39338
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValue__SWIG_8([MarshalAs(UnmanagedType.LPWStr)] string jarg1, float jarg2, ulong jarg3);

	// Token: 0x060099AB RID: 39339
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValue__SWIG_9([MarshalAs(UnmanagedType.LPWStr)] string jarg1, float jarg2);

	// Token: 0x060099AC RID: 39340
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValueByPlayingID__SWIG_0(uint jarg1, float jarg2, uint jarg3, int jarg4, int jarg5, bool jarg6);

	// Token: 0x060099AD RID: 39341
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValueByPlayingID__SWIG_1(uint jarg1, float jarg2, uint jarg3, int jarg4, int jarg5);

	// Token: 0x060099AE RID: 39342
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValueByPlayingID__SWIG_2(uint jarg1, float jarg2, uint jarg3, int jarg4);

	// Token: 0x060099AF RID: 39343
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValueByPlayingID__SWIG_3(uint jarg1, float jarg2, uint jarg3);

	// Token: 0x060099B0 RID: 39344
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValueByPlayingID__SWIG_4([MarshalAs(UnmanagedType.LPWStr)] string jarg1, float jarg2, uint jarg3, int jarg4, int jarg5, bool jarg6);

	// Token: 0x060099B1 RID: 39345
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValueByPlayingID__SWIG_5([MarshalAs(UnmanagedType.LPWStr)] string jarg1, float jarg2, uint jarg3, int jarg4, int jarg5);

	// Token: 0x060099B2 RID: 39346
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValueByPlayingID__SWIG_6([MarshalAs(UnmanagedType.LPWStr)] string jarg1, float jarg2, uint jarg3, int jarg4);

	// Token: 0x060099B3 RID: 39347
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRTPCValueByPlayingID__SWIG_7([MarshalAs(UnmanagedType.LPWStr)] string jarg1, float jarg2, uint jarg3);

	// Token: 0x060099B4 RID: 39348
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ResetRTPCValue__SWIG_0(uint jarg1, ulong jarg2, int jarg3, int jarg4, bool jarg5);

	// Token: 0x060099B5 RID: 39349
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ResetRTPCValue__SWIG_1(uint jarg1, ulong jarg2, int jarg3, int jarg4);

	// Token: 0x060099B6 RID: 39350
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ResetRTPCValue__SWIG_2(uint jarg1, ulong jarg2, int jarg3);

	// Token: 0x060099B7 RID: 39351
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ResetRTPCValue__SWIG_3(uint jarg1, ulong jarg2);

	// Token: 0x060099B8 RID: 39352
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ResetRTPCValue__SWIG_4(uint jarg1);

	// Token: 0x060099B9 RID: 39353
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ResetRTPCValue__SWIG_5([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, int jarg3, int jarg4, bool jarg5);

	// Token: 0x060099BA RID: 39354
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ResetRTPCValue__SWIG_6([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, int jarg3, int jarg4);

	// Token: 0x060099BB RID: 39355
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ResetRTPCValue__SWIG_7([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, int jarg3);

	// Token: 0x060099BC RID: 39356
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ResetRTPCValue__SWIG_8([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2);

	// Token: 0x060099BD RID: 39357
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_ResetRTPCValue__SWIG_9([MarshalAs(UnmanagedType.LPWStr)] string jarg1);

	// Token: 0x060099BE RID: 39358
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetSwitch__SWIG_0(uint jarg1, uint jarg2, ulong jarg3);

	// Token: 0x060099BF RID: 39359
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetSwitch__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, [MarshalAs(UnmanagedType.LPWStr)] string jarg2, ulong jarg3);

	// Token: 0x060099C0 RID: 39360
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostTrigger__SWIG_0(uint jarg1, ulong jarg2);

	// Token: 0x060099C1 RID: 39361
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostTrigger__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2);

	// Token: 0x060099C2 RID: 39362
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetState__SWIG_0(uint jarg1, uint jarg2);

	// Token: 0x060099C3 RID: 39363
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetState__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, [MarshalAs(UnmanagedType.LPWStr)] string jarg2);

	// Token: 0x060099C4 RID: 39364
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetGameObjectAuxSendValues(ulong jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x060099C5 RID: 39365
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetGameObjectOutputBusVolume(ulong jarg1, ulong jarg2, float jarg3);

	// Token: 0x060099C6 RID: 39366
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetActorMixerEffect(uint jarg1, uint jarg2, uint jarg3);

	// Token: 0x060099C7 RID: 39367
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetBusEffect__SWIG_0(uint jarg1, uint jarg2, uint jarg3);

	// Token: 0x060099C8 RID: 39368
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetBusEffect__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, uint jarg2, uint jarg3);

	// Token: 0x060099C9 RID: 39369
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetMixer__SWIG_0(uint jarg1, uint jarg2);

	// Token: 0x060099CA RID: 39370
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetMixer__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, uint jarg2);

	// Token: 0x060099CB RID: 39371
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetBusConfig__SWIG_0(uint jarg1, IntPtr jarg2);

	// Token: 0x060099CC RID: 39372
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetBusConfig__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, IntPtr jarg2);

	// Token: 0x060099CD RID: 39373
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetObjectObstructionAndOcclusion(ulong jarg1, ulong jarg2, float jarg3, float jarg4);

	// Token: 0x060099CE RID: 39374
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetMultipleObstructionAndOcclusion(ulong jarg1, ulong jarg2, IntPtr jarg3, uint jarg4);

	// Token: 0x060099CF RID: 39375
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_StartOutputCapture([MarshalAs(UnmanagedType.LPWStr)] string jarg1);

	// Token: 0x060099D0 RID: 39376
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_StopOutputCapture();

	// Token: 0x060099D1 RID: 39377
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AddOutputCaptureMarker([MarshalAs(UnmanagedType.LPStr)] string jarg1);

	// Token: 0x060099D2 RID: 39378
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_StartProfilerCapture([MarshalAs(UnmanagedType.LPWStr)] string jarg1);

	// Token: 0x060099D3 RID: 39379
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_StopProfilerCapture();

	// Token: 0x060099D4 RID: 39380
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RemoveOutput(ulong jarg1);

	// Token: 0x060099D5 RID: 39381
	[DllImport("AkSoundEngine")]
	public static extern ulong CSharp_GetOutputID__SWIG_0(uint jarg1, uint jarg2);

	// Token: 0x060099D6 RID: 39382
	[DllImport("AkSoundEngine")]
	public static extern ulong CSharp_GetOutputID__SWIG_1([MarshalAs(UnmanagedType.LPStr)] string jarg1, uint jarg2);

	// Token: 0x060099D7 RID: 39383
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetBusDevice__SWIG_0(uint jarg1, uint jarg2);

	// Token: 0x060099D8 RID: 39384
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetBusDevice__SWIG_1([MarshalAs(UnmanagedType.LPStr)] string jarg1, [MarshalAs(UnmanagedType.LPStr)] string jarg2);

	// Token: 0x060099D9 RID: 39385
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetOutputVolume(ulong jarg1, float jarg2);

	// Token: 0x060099DA RID: 39386
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_Suspend__SWIG_0(bool jarg1);

	// Token: 0x060099DB RID: 39387
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_Suspend__SWIG_1();

	// Token: 0x060099DC RID: 39388
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_WakeupFromSuspend();

	// Token: 0x060099DD RID: 39389
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_GetBufferTick();

	// Token: 0x060099DE RID: 39390
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSegmentInfo_iCurrentPosition_set(IntPtr jarg1, int jarg2);

	// Token: 0x060099DF RID: 39391
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkSegmentInfo_iCurrentPosition_get(IntPtr jarg1);

	// Token: 0x060099E0 RID: 39392
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSegmentInfo_iPreEntryDuration_set(IntPtr jarg1, int jarg2);

	// Token: 0x060099E1 RID: 39393
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkSegmentInfo_iPreEntryDuration_get(IntPtr jarg1);

	// Token: 0x060099E2 RID: 39394
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSegmentInfo_iActiveDuration_set(IntPtr jarg1, int jarg2);

	// Token: 0x060099E3 RID: 39395
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkSegmentInfo_iActiveDuration_get(IntPtr jarg1);

	// Token: 0x060099E4 RID: 39396
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSegmentInfo_iPostExitDuration_set(IntPtr jarg1, int jarg2);

	// Token: 0x060099E5 RID: 39397
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkSegmentInfo_iPostExitDuration_get(IntPtr jarg1);

	// Token: 0x060099E6 RID: 39398
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSegmentInfo_iRemainingLookAheadTime_set(IntPtr jarg1, int jarg2);

	// Token: 0x060099E7 RID: 39399
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkSegmentInfo_iRemainingLookAheadTime_get(IntPtr jarg1);

	// Token: 0x060099E8 RID: 39400
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSegmentInfo_fBeatDuration_set(IntPtr jarg1, float jarg2);

	// Token: 0x060099E9 RID: 39401
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkSegmentInfo_fBeatDuration_get(IntPtr jarg1);

	// Token: 0x060099EA RID: 39402
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSegmentInfo_fBarDuration_set(IntPtr jarg1, float jarg2);

	// Token: 0x060099EB RID: 39403
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkSegmentInfo_fBarDuration_get(IntPtr jarg1);

	// Token: 0x060099EC RID: 39404
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSegmentInfo_fGridDuration_set(IntPtr jarg1, float jarg2);

	// Token: 0x060099ED RID: 39405
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkSegmentInfo_fGridDuration_get(IntPtr jarg1);

	// Token: 0x060099EE RID: 39406
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSegmentInfo_fGridOffset_set(IntPtr jarg1, float jarg2);

	// Token: 0x060099EF RID: 39407
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkSegmentInfo_fGridOffset_get(IntPtr jarg1);

	// Token: 0x060099F0 RID: 39408
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkSegmentInfo();

	// Token: 0x060099F1 RID: 39409
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkSegmentInfo(IntPtr jarg1);

	// Token: 0x060099F2 RID: 39410
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AK_INVALID_MIDI_CHANNEL_get();

	// Token: 0x060099F3 RID: 39411
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AK_INVALID_MIDI_NOTE_get();

	// Token: 0x060099F4 RID: 39412
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_byChan_set(IntPtr jarg1, byte jarg2);

	// Token: 0x060099F5 RID: 39413
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_byChan_get(IntPtr jarg1);

	// Token: 0x060099F6 RID: 39414
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_tGen_byParam1_set(IntPtr jarg1, byte jarg2);

	// Token: 0x060099F7 RID: 39415
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_tGen_byParam1_get(IntPtr jarg1);

	// Token: 0x060099F8 RID: 39416
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_tGen_byParam2_set(IntPtr jarg1, byte jarg2);

	// Token: 0x060099F9 RID: 39417
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_tGen_byParam2_get(IntPtr jarg1);

	// Token: 0x060099FA RID: 39418
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMIDIEvent_tGen();

	// Token: 0x060099FB RID: 39419
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMIDIEvent_tGen(IntPtr jarg1);

	// Token: 0x060099FC RID: 39420
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_tNoteOnOff_byNote_set(IntPtr jarg1, byte jarg2);

	// Token: 0x060099FD RID: 39421
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_tNoteOnOff_byNote_get(IntPtr jarg1);

	// Token: 0x060099FE RID: 39422
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_tNoteOnOff_byVelocity_set(IntPtr jarg1, byte jarg2);

	// Token: 0x060099FF RID: 39423
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_tNoteOnOff_byVelocity_get(IntPtr jarg1);

	// Token: 0x06009A00 RID: 39424
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMIDIEvent_tNoteOnOff();

	// Token: 0x06009A01 RID: 39425
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMIDIEvent_tNoteOnOff(IntPtr jarg1);

	// Token: 0x06009A02 RID: 39426
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_tCc_byCc_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A03 RID: 39427
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_tCc_byCc_get(IntPtr jarg1);

	// Token: 0x06009A04 RID: 39428
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_tCc_byValue_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A05 RID: 39429
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_tCc_byValue_get(IntPtr jarg1);

	// Token: 0x06009A06 RID: 39430
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMIDIEvent_tCc();

	// Token: 0x06009A07 RID: 39431
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMIDIEvent_tCc(IntPtr jarg1);

	// Token: 0x06009A08 RID: 39432
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_tPitchBend_byValueLsb_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A09 RID: 39433
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_tPitchBend_byValueLsb_get(IntPtr jarg1);

	// Token: 0x06009A0A RID: 39434
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_tPitchBend_byValueMsb_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A0B RID: 39435
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_tPitchBend_byValueMsb_get(IntPtr jarg1);

	// Token: 0x06009A0C RID: 39436
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMIDIEvent_tPitchBend();

	// Token: 0x06009A0D RID: 39437
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMIDIEvent_tPitchBend(IntPtr jarg1);

	// Token: 0x06009A0E RID: 39438
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_tNoteAftertouch_byNote_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A0F RID: 39439
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_tNoteAftertouch_byNote_get(IntPtr jarg1);

	// Token: 0x06009A10 RID: 39440
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_tNoteAftertouch_byValue_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A11 RID: 39441
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_tNoteAftertouch_byValue_get(IntPtr jarg1);

	// Token: 0x06009A12 RID: 39442
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMIDIEvent_tNoteAftertouch();

	// Token: 0x06009A13 RID: 39443
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMIDIEvent_tNoteAftertouch(IntPtr jarg1);

	// Token: 0x06009A14 RID: 39444
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_tChanAftertouch_byValue_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A15 RID: 39445
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_tChanAftertouch_byValue_get(IntPtr jarg1);

	// Token: 0x06009A16 RID: 39446
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMIDIEvent_tChanAftertouch();

	// Token: 0x06009A17 RID: 39447
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMIDIEvent_tChanAftertouch(IntPtr jarg1);

	// Token: 0x06009A18 RID: 39448
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_tProgramChange_byProgramNum_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A19 RID: 39449
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_tProgramChange_byProgramNum_get(IntPtr jarg1);

	// Token: 0x06009A1A RID: 39450
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMIDIEvent_tProgramChange();

	// Token: 0x06009A1B RID: 39451
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMIDIEvent_tProgramChange(IntPtr jarg1);

	// Token: 0x06009A1C RID: 39452
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_Gen_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009A1D RID: 39453
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMIDIEvent_Gen_get(IntPtr jarg1);

	// Token: 0x06009A1E RID: 39454
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_Cc_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009A1F RID: 39455
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMIDIEvent_Cc_get(IntPtr jarg1);

	// Token: 0x06009A20 RID: 39456
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_NoteOnOff_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009A21 RID: 39457
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMIDIEvent_NoteOnOff_get(IntPtr jarg1);

	// Token: 0x06009A22 RID: 39458
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_PitchBend_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009A23 RID: 39459
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMIDIEvent_PitchBend_get(IntPtr jarg1);

	// Token: 0x06009A24 RID: 39460
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_NoteAftertouch_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009A25 RID: 39461
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMIDIEvent_NoteAftertouch_get(IntPtr jarg1);

	// Token: 0x06009A26 RID: 39462
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_ChanAftertouch_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009A27 RID: 39463
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMIDIEvent_ChanAftertouch_get(IntPtr jarg1);

	// Token: 0x06009A28 RID: 39464
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_ProgramChange_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009A29 RID: 39465
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMIDIEvent_ProgramChange_get(IntPtr jarg1);

	// Token: 0x06009A2A RID: 39466
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_byType_set(IntPtr jarg1, int jarg2);

	// Token: 0x06009A2B RID: 39467
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMIDIEvent_byType_get(IntPtr jarg1);

	// Token: 0x06009A2C RID: 39468
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_byOnOffNote_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A2D RID: 39469
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_byOnOffNote_get(IntPtr jarg1);

	// Token: 0x06009A2E RID: 39470
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_byVelocity_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A2F RID: 39471
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_byVelocity_get(IntPtr jarg1);

	// Token: 0x06009A30 RID: 39472
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_byCc_set(IntPtr jarg1, int jarg2);

	// Token: 0x06009A31 RID: 39473
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMIDIEvent_byCc_get(IntPtr jarg1);

	// Token: 0x06009A32 RID: 39474
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_byCcValue_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A33 RID: 39475
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_byCcValue_get(IntPtr jarg1);

	// Token: 0x06009A34 RID: 39476
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_byValueLsb_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A35 RID: 39477
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_byValueLsb_get(IntPtr jarg1);

	// Token: 0x06009A36 RID: 39478
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_byValueMsb_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A37 RID: 39479
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_byValueMsb_get(IntPtr jarg1);

	// Token: 0x06009A38 RID: 39480
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_byAftertouchNote_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A39 RID: 39481
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_byAftertouchNote_get(IntPtr jarg1);

	// Token: 0x06009A3A RID: 39482
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_byNoteAftertouchValue_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A3B RID: 39483
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_byNoteAftertouchValue_get(IntPtr jarg1);

	// Token: 0x06009A3C RID: 39484
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_byChanAftertouchValue_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A3D RID: 39485
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_byChanAftertouchValue_get(IntPtr jarg1);

	// Token: 0x06009A3E RID: 39486
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIEvent_byProgramNum_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009A3F RID: 39487
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEvent_byProgramNum_get(IntPtr jarg1);

	// Token: 0x06009A40 RID: 39488
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMIDIEvent();

	// Token: 0x06009A41 RID: 39489
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMIDIEvent(IntPtr jarg1);

	// Token: 0x06009A42 RID: 39490
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIPost_uOffset_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009A43 RID: 39491
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkMIDIPost_uOffset_get(IntPtr jarg1);

	// Token: 0x06009A44 RID: 39492
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMIDIPost_PostOnEvent(IntPtr jarg1, uint jarg2, ulong jarg3, uint jarg4);

	// Token: 0x06009A45 RID: 39493
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMIDIPost_Clone(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009A46 RID: 39494
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMIDIPost_GetSizeOf();

	// Token: 0x06009A47 RID: 39495
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMIDIPost();

	// Token: 0x06009A48 RID: 39496
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMIDIPost(IntPtr jarg1);

	// Token: 0x06009A49 RID: 39497
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMemSettings();

	// Token: 0x06009A4A RID: 39498
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMemSettings_uMaxNumPools_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009A4B RID: 39499
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkMemSettings_uMaxNumPools_get(IntPtr jarg1);

	// Token: 0x06009A4C RID: 39500
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMemSettings_uDebugFlags_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009A4D RID: 39501
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkMemSettings_uDebugFlags_get(IntPtr jarg1);

	// Token: 0x06009A4E RID: 39502
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMemSettings(IntPtr jarg1);

	// Token: 0x06009A4F RID: 39503
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkMusicSettings_fStreamingLookAheadRatio_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009A50 RID: 39504
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkMusicSettings_fStreamingLookAheadRatio_get(IntPtr jarg1);

	// Token: 0x06009A51 RID: 39505
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMusicSettings();

	// Token: 0x06009A52 RID: 39506
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMusicSettings(IntPtr jarg1);

	// Token: 0x06009A53 RID: 39507
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetPlayingSegmentInfo__SWIG_0(uint jarg1, IntPtr jarg2, bool jarg3);

	// Token: 0x06009A54 RID: 39508
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetPlayingSegmentInfo__SWIG_1(uint jarg1, IntPtr jarg2);

	// Token: 0x06009A55 RID: 39509
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkSerializedCallbackHeader_pPackage_get(IntPtr jarg1);

	// Token: 0x06009A56 RID: 39510
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkSerializedCallbackHeader_pNext_get(IntPtr jarg1);

	// Token: 0x06009A57 RID: 39511
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkSerializedCallbackHeader_eType_get(IntPtr jarg1);

	// Token: 0x06009A58 RID: 39512
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkSerializedCallbackHeader_GetData(IntPtr jarg1);

	// Token: 0x06009A59 RID: 39513
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkSerializedCallbackHeader();

	// Token: 0x06009A5A RID: 39514
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkSerializedCallbackHeader(IntPtr jarg1);

	// Token: 0x06009A5B RID: 39515
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkCallbackInfo_pCookie_get(IntPtr jarg1);

	// Token: 0x06009A5C RID: 39516
	[DllImport("AkSoundEngine")]
	public static extern ulong CSharp_AkCallbackInfo_gameObjID_get(IntPtr jarg1);

	// Token: 0x06009A5D RID: 39517
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkCallbackInfo();

	// Token: 0x06009A5E RID: 39518
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkCallbackInfo(IntPtr jarg1);

	// Token: 0x06009A5F RID: 39519
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkEventCallbackInfo_playingID_get(IntPtr jarg1);

	// Token: 0x06009A60 RID: 39520
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkEventCallbackInfo_eventID_get(IntPtr jarg1);

	// Token: 0x06009A61 RID: 39521
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkEventCallbackInfo();

	// Token: 0x06009A62 RID: 39522
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkEventCallbackInfo(IntPtr jarg1);

	// Token: 0x06009A63 RID: 39523
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEventCallbackInfo_byChan_get(IntPtr jarg1);

	// Token: 0x06009A64 RID: 39524
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEventCallbackInfo_byParam1_get(IntPtr jarg1);

	// Token: 0x06009A65 RID: 39525
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEventCallbackInfo_byParam2_get(IntPtr jarg1);

	// Token: 0x06009A66 RID: 39526
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMIDIEventCallbackInfo_byType_get(IntPtr jarg1);

	// Token: 0x06009A67 RID: 39527
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEventCallbackInfo_byOnOffNote_get(IntPtr jarg1);

	// Token: 0x06009A68 RID: 39528
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEventCallbackInfo_byVelocity_get(IntPtr jarg1);

	// Token: 0x06009A69 RID: 39529
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMIDIEventCallbackInfo_byCc_get(IntPtr jarg1);

	// Token: 0x06009A6A RID: 39530
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEventCallbackInfo_byCcValue_get(IntPtr jarg1);

	// Token: 0x06009A6B RID: 39531
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEventCallbackInfo_byValueLsb_get(IntPtr jarg1);

	// Token: 0x06009A6C RID: 39532
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEventCallbackInfo_byValueMsb_get(IntPtr jarg1);

	// Token: 0x06009A6D RID: 39533
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEventCallbackInfo_byAftertouchNote_get(IntPtr jarg1);

	// Token: 0x06009A6E RID: 39534
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEventCallbackInfo_byNoteAftertouchValue_get(IntPtr jarg1);

	// Token: 0x06009A6F RID: 39535
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEventCallbackInfo_byChanAftertouchValue_get(IntPtr jarg1);

	// Token: 0x06009A70 RID: 39536
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkMIDIEventCallbackInfo_byProgramNum_get(IntPtr jarg1);

	// Token: 0x06009A71 RID: 39537
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMIDIEventCallbackInfo();

	// Token: 0x06009A72 RID: 39538
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMIDIEventCallbackInfo(IntPtr jarg1);

	// Token: 0x06009A73 RID: 39539
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkMarkerCallbackInfo_uIdentifier_get(IntPtr jarg1);

	// Token: 0x06009A74 RID: 39540
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkMarkerCallbackInfo_uPosition_get(IntPtr jarg1);

	// Token: 0x06009A75 RID: 39541
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMarkerCallbackInfo_strLabel_get(IntPtr jarg1);

	// Token: 0x06009A76 RID: 39542
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMarkerCallbackInfo();

	// Token: 0x06009A77 RID: 39543
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMarkerCallbackInfo(IntPtr jarg1);

	// Token: 0x06009A78 RID: 39544
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkDurationCallbackInfo_fDuration_get(IntPtr jarg1);

	// Token: 0x06009A79 RID: 39545
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkDurationCallbackInfo_fEstimatedDuration_get(IntPtr jarg1);

	// Token: 0x06009A7A RID: 39546
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkDurationCallbackInfo_audioNodeID_get(IntPtr jarg1);

	// Token: 0x06009A7B RID: 39547
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkDurationCallbackInfo_mediaID_get(IntPtr jarg1);

	// Token: 0x06009A7C RID: 39548
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkDurationCallbackInfo_bStreaming_get(IntPtr jarg1);

	// Token: 0x06009A7D RID: 39549
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkDurationCallbackInfo();

	// Token: 0x06009A7E RID: 39550
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkDurationCallbackInfo(IntPtr jarg1);

	// Token: 0x06009A7F RID: 39551
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkDynamicSequenceItemCallbackInfo_playingID_get(IntPtr jarg1);

	// Token: 0x06009A80 RID: 39552
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkDynamicSequenceItemCallbackInfo_audioNodeID_get(IntPtr jarg1);

	// Token: 0x06009A81 RID: 39553
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkDynamicSequenceItemCallbackInfo_pCustomInfo_get(IntPtr jarg1);

	// Token: 0x06009A82 RID: 39554
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkDynamicSequenceItemCallbackInfo();

	// Token: 0x06009A83 RID: 39555
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkDynamicSequenceItemCallbackInfo(IntPtr jarg1);

	// Token: 0x06009A84 RID: 39556
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkMusicSyncCallbackInfo_playingID_get(IntPtr jarg1);

	// Token: 0x06009A85 RID: 39557
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMusicSyncCallbackInfo_segmentInfo_iCurrentPosition_get(IntPtr jarg1);

	// Token: 0x06009A86 RID: 39558
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMusicSyncCallbackInfo_segmentInfo_iPreEntryDuration_get(IntPtr jarg1);

	// Token: 0x06009A87 RID: 39559
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMusicSyncCallbackInfo_segmentInfo_iActiveDuration_get(IntPtr jarg1);

	// Token: 0x06009A88 RID: 39560
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMusicSyncCallbackInfo_segmentInfo_iPostExitDuration_get(IntPtr jarg1);

	// Token: 0x06009A89 RID: 39561
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMusicSyncCallbackInfo_segmentInfo_iRemainingLookAheadTime_get(IntPtr jarg1);

	// Token: 0x06009A8A RID: 39562
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkMusicSyncCallbackInfo_segmentInfo_fBeatDuration_get(IntPtr jarg1);

	// Token: 0x06009A8B RID: 39563
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkMusicSyncCallbackInfo_segmentInfo_fBarDuration_get(IntPtr jarg1);

	// Token: 0x06009A8C RID: 39564
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkMusicSyncCallbackInfo_segmentInfo_fGridDuration_get(IntPtr jarg1);

	// Token: 0x06009A8D RID: 39565
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkMusicSyncCallbackInfo_segmentInfo_fGridOffset_get(IntPtr jarg1);

	// Token: 0x06009A8E RID: 39566
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMusicSyncCallbackInfo_musicSyncType_get(IntPtr jarg1);

	// Token: 0x06009A8F RID: 39567
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMusicSyncCallbackInfo_userCueName_get(IntPtr jarg1);

	// Token: 0x06009A90 RID: 39568
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMusicSyncCallbackInfo();

	// Token: 0x06009A91 RID: 39569
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMusicSyncCallbackInfo(IntPtr jarg1);

	// Token: 0x06009A92 RID: 39570
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkMusicPlaylistCallbackInfo_playlistID_get(IntPtr jarg1);

	// Token: 0x06009A93 RID: 39571
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkMusicPlaylistCallbackInfo_uNumPlaylistItems_get(IntPtr jarg1);

	// Token: 0x06009A94 RID: 39572
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkMusicPlaylistCallbackInfo_uPlaylistSelection_get(IntPtr jarg1);

	// Token: 0x06009A95 RID: 39573
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkMusicPlaylistCallbackInfo_uPlaylistItemDone_get(IntPtr jarg1);

	// Token: 0x06009A96 RID: 39574
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMusicPlaylistCallbackInfo();

	// Token: 0x06009A97 RID: 39575
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMusicPlaylistCallbackInfo(IntPtr jarg1);

	// Token: 0x06009A98 RID: 39576
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkBankCallbackInfo_bankID_get(IntPtr jarg1);

	// Token: 0x06009A99 RID: 39577
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkBankCallbackInfo_inMemoryBankPtr_get(IntPtr jarg1);

	// Token: 0x06009A9A RID: 39578
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkBankCallbackInfo_loadResult_get(IntPtr jarg1);

	// Token: 0x06009A9B RID: 39579
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkBankCallbackInfo_memPoolId_get(IntPtr jarg1);

	// Token: 0x06009A9C RID: 39580
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkBankCallbackInfo();

	// Token: 0x06009A9D RID: 39581
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkBankCallbackInfo(IntPtr jarg1);

	// Token: 0x06009A9E RID: 39582
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMonitoringCallbackInfo_errorCode_get(IntPtr jarg1);

	// Token: 0x06009A9F RID: 39583
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkMonitoringCallbackInfo_errorLevel_get(IntPtr jarg1);

	// Token: 0x06009AA0 RID: 39584
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkMonitoringCallbackInfo_playingID_get(IntPtr jarg1);

	// Token: 0x06009AA1 RID: 39585
	[DllImport("AkSoundEngine")]
	public static extern ulong CSharp_AkMonitoringCallbackInfo_gameObjID_get(IntPtr jarg1);

	// Token: 0x06009AA2 RID: 39586
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMonitoringCallbackInfo_message_get(IntPtr jarg1);

	// Token: 0x06009AA3 RID: 39587
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkMonitoringCallbackInfo();

	// Token: 0x06009AA4 RID: 39588
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkMonitoringCallbackInfo(IntPtr jarg1);

	// Token: 0x06009AA5 RID: 39589
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkAudioInterruptionCallbackInfo_bEnterInterruption_get(IntPtr jarg1);

	// Token: 0x06009AA6 RID: 39590
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkAudioInterruptionCallbackInfo();

	// Token: 0x06009AA7 RID: 39591
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkAudioInterruptionCallbackInfo(IntPtr jarg1);

	// Token: 0x06009AA8 RID: 39592
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkAudioSourceChangeCallbackInfo_bOtherAudioPlaying_get(IntPtr jarg1);

	// Token: 0x06009AA9 RID: 39593
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkAudioSourceChangeCallbackInfo();

	// Token: 0x06009AAA RID: 39594
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkAudioSourceChangeCallbackInfo(IntPtr jarg1);

	// Token: 0x06009AAB RID: 39595
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkCallbackSerializer_Init(IntPtr jarg1, uint jarg2);

	// Token: 0x06009AAC RID: 39596
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkCallbackSerializer_Term();

	// Token: 0x06009AAD RID: 39597
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkCallbackSerializer_Lock();

	// Token: 0x06009AAE RID: 39598
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkCallbackSerializer_SetLocalOutput(uint jarg1);

	// Token: 0x06009AAF RID: 39599
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkCallbackSerializer_Unlock();

	// Token: 0x06009AB0 RID: 39600
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkCallbackSerializer_AudioSourceChangeCallbackFunc(bool jarg1, IntPtr jarg2);

	// Token: 0x06009AB1 RID: 39601
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkCallbackSerializer();

	// Token: 0x06009AB2 RID: 39602
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkCallbackSerializer(IntPtr jarg1);

	// Token: 0x06009AB3 RID: 39603
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostCode__SWIG_0(int jarg1, int jarg2, uint jarg3, ulong jarg4, uint jarg5, bool jarg6);

	// Token: 0x06009AB4 RID: 39604
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostCode__SWIG_1(int jarg1, int jarg2, uint jarg3, ulong jarg4, uint jarg5);

	// Token: 0x06009AB5 RID: 39605
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostCode__SWIG_2(int jarg1, int jarg2, uint jarg3, ulong jarg4);

	// Token: 0x06009AB6 RID: 39606
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostCode__SWIG_3(int jarg1, int jarg2, uint jarg3);

	// Token: 0x06009AB7 RID: 39607
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostCode__SWIG_4(int jarg1, int jarg2);

	// Token: 0x06009AB8 RID: 39608
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostString__SWIG_0([MarshalAs(UnmanagedType.LPWStr)] string jarg1, int jarg2, uint jarg3, ulong jarg4, uint jarg5, bool jarg6);

	// Token: 0x06009AB9 RID: 39609
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostString__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, int jarg2, uint jarg3, ulong jarg4, uint jarg5);

	// Token: 0x06009ABA RID: 39610
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostString__SWIG_2([MarshalAs(UnmanagedType.LPWStr)] string jarg1, int jarg2, uint jarg3, ulong jarg4);

	// Token: 0x06009ABB RID: 39611
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostString__SWIG_3([MarshalAs(UnmanagedType.LPWStr)] string jarg1, int jarg2, uint jarg3);

	// Token: 0x06009ABC RID: 39612
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_PostString__SWIG_4([MarshalAs(UnmanagedType.LPWStr)] string jarg1, int jarg2);

	// Token: 0x06009ABD RID: 39613
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetTimeStamp();

	// Token: 0x06009ABE RID: 39614
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_GetNumNonZeroBits(uint jarg1);

	// Token: 0x06009ABF RID: 39615
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_ResolveDialogueEvent__SWIG_0(uint jarg1, [MarshalAs(UnmanagedType.LPArray)] [In] uint[] jarg2, uint jarg3, uint jarg4);

	// Token: 0x06009AC0 RID: 39616
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_ResolveDialogueEvent__SWIG_1(uint jarg1, [MarshalAs(UnmanagedType.LPArray)] [In] uint[] jarg2, uint jarg3);

	// Token: 0x06009AC1 RID: 39617
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetDialogueEventCustomPropertyValue__SWIG_0(uint jarg1, uint jarg2, out int jarg3);

	// Token: 0x06009AC2 RID: 39618
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetDialogueEventCustomPropertyValue__SWIG_1(uint jarg1, uint jarg2, out float jarg3);

	// Token: 0x06009AC3 RID: 39619
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_fCenterPct_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009AC4 RID: 39620
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPositioningInfo_fCenterPct_get(IntPtr jarg1);

	// Token: 0x06009AC5 RID: 39621
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_pannerType_set(IntPtr jarg1, int jarg2);

	// Token: 0x06009AC6 RID: 39622
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPositioningInfo_pannerType_get(IntPtr jarg1);

	// Token: 0x06009AC7 RID: 39623
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_posSourceType_set(IntPtr jarg1, int jarg2);

	// Token: 0x06009AC8 RID: 39624
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPositioningInfo_posSourceType_get(IntPtr jarg1);

	// Token: 0x06009AC9 RID: 39625
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_bUpdateEachFrame_set(IntPtr jarg1, bool jarg2);

	// Token: 0x06009ACA RID: 39626
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkPositioningInfo_bUpdateEachFrame_get(IntPtr jarg1);

	// Token: 0x06009ACB RID: 39627
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_e3DSpatializationMode_set(IntPtr jarg1, int jarg2);

	// Token: 0x06009ACC RID: 39628
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPositioningInfo_e3DSpatializationMode_get(IntPtr jarg1);

	// Token: 0x06009ACD RID: 39629
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_bUseAttenuation_set(IntPtr jarg1, bool jarg2);

	// Token: 0x06009ACE RID: 39630
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkPositioningInfo_bUseAttenuation_get(IntPtr jarg1);

	// Token: 0x06009ACF RID: 39631
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_bUseConeAttenuation_set(IntPtr jarg1, bool jarg2);

	// Token: 0x06009AD0 RID: 39632
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkPositioningInfo_bUseConeAttenuation_get(IntPtr jarg1);

	// Token: 0x06009AD1 RID: 39633
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_fInnerAngle_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009AD2 RID: 39634
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPositioningInfo_fInnerAngle_get(IntPtr jarg1);

	// Token: 0x06009AD3 RID: 39635
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_fOuterAngle_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009AD4 RID: 39636
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPositioningInfo_fOuterAngle_get(IntPtr jarg1);

	// Token: 0x06009AD5 RID: 39637
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_fConeMaxAttenuation_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009AD6 RID: 39638
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPositioningInfo_fConeMaxAttenuation_get(IntPtr jarg1);

	// Token: 0x06009AD7 RID: 39639
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_LPFCone_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009AD8 RID: 39640
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPositioningInfo_LPFCone_get(IntPtr jarg1);

	// Token: 0x06009AD9 RID: 39641
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_HPFCone_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009ADA RID: 39642
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPositioningInfo_HPFCone_get(IntPtr jarg1);

	// Token: 0x06009ADB RID: 39643
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_fMaxDistance_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009ADC RID: 39644
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPositioningInfo_fMaxDistance_get(IntPtr jarg1);

	// Token: 0x06009ADD RID: 39645
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_fVolDryAtMaxDist_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009ADE RID: 39646
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPositioningInfo_fVolDryAtMaxDist_get(IntPtr jarg1);

	// Token: 0x06009ADF RID: 39647
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_fVolAuxGameDefAtMaxDist_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009AE0 RID: 39648
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPositioningInfo_fVolAuxGameDefAtMaxDist_get(IntPtr jarg1);

	// Token: 0x06009AE1 RID: 39649
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_fVolAuxUserDefAtMaxDist_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009AE2 RID: 39650
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPositioningInfo_fVolAuxUserDefAtMaxDist_get(IntPtr jarg1);

	// Token: 0x06009AE3 RID: 39651
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_LPFValueAtMaxDist_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009AE4 RID: 39652
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPositioningInfo_LPFValueAtMaxDist_get(IntPtr jarg1);

	// Token: 0x06009AE5 RID: 39653
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPositioningInfo_HPFValueAtMaxDist_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009AE6 RID: 39654
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPositioningInfo_HPFValueAtMaxDist_get(IntPtr jarg1);

	// Token: 0x06009AE7 RID: 39655
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkPositioningInfo();

	// Token: 0x06009AE8 RID: 39656
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkPositioningInfo(IntPtr jarg1);

	// Token: 0x06009AE9 RID: 39657
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkObjectInfo_objID_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009AEA RID: 39658
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkObjectInfo_objID_get(IntPtr jarg1);

	// Token: 0x06009AEB RID: 39659
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkObjectInfo_parentID_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009AEC RID: 39660
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkObjectInfo_parentID_get(IntPtr jarg1);

	// Token: 0x06009AED RID: 39661
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkObjectInfo_iDepth_set(IntPtr jarg1, int jarg2);

	// Token: 0x06009AEE RID: 39662
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkObjectInfo_iDepth_get(IntPtr jarg1);

	// Token: 0x06009AEF RID: 39663
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkObjectInfo();

	// Token: 0x06009AF0 RID: 39664
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkObjectInfo(IntPtr jarg1);

	// Token: 0x06009AF1 RID: 39665
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetPosition(ulong jarg1, IntPtr jarg2);

	// Token: 0x06009AF2 RID: 39666
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetListenerPosition(ulong jarg1, IntPtr jarg2);

	// Token: 0x06009AF3 RID: 39667
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetRTPCValue__SWIG_0(uint jarg1, ulong jarg2, uint jarg3, out float jarg4, ref int jarg5);

	// Token: 0x06009AF4 RID: 39668
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetRTPCValue__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, uint jarg3, out float jarg4, ref int jarg5);

	// Token: 0x06009AF5 RID: 39669
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetSwitch__SWIG_0(uint jarg1, ulong jarg2, out uint jarg3);

	// Token: 0x06009AF6 RID: 39670
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetSwitch__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ulong jarg2, out uint jarg3);

	// Token: 0x06009AF7 RID: 39671
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetState__SWIG_0(uint jarg1, out uint jarg2);

	// Token: 0x06009AF8 RID: 39672
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetState__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, out uint jarg2);

	// Token: 0x06009AF9 RID: 39673
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetGameObjectAuxSendValues(ulong jarg1, IntPtr jarg2, ref uint jarg3);

	// Token: 0x06009AFA RID: 39674
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetGameObjectDryLevelValue(ulong jarg1, ulong jarg2, out float jarg3);

	// Token: 0x06009AFB RID: 39675
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetObjectObstructionAndOcclusion(ulong jarg1, ulong jarg2, out float jarg3, out float jarg4);

	// Token: 0x06009AFC RID: 39676
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_QueryAudioObjectIDs__SWIG_0(uint jarg1, ref uint jarg2, IntPtr jarg3);

	// Token: 0x06009AFD RID: 39677
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_QueryAudioObjectIDs__SWIG_1([MarshalAs(UnmanagedType.LPWStr)] string jarg1, ref uint jarg2, IntPtr jarg3);

	// Token: 0x06009AFE RID: 39678
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetPositioningInfo(uint jarg1, IntPtr jarg2);

	// Token: 0x06009AFF RID: 39679
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_GetIsGameObjectActive(ulong jarg1);

	// Token: 0x06009B00 RID: 39680
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_GetMaxRadius(ulong jarg1);

	// Token: 0x06009B01 RID: 39681
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_GetEventIDFromPlayingID(uint jarg1);

	// Token: 0x06009B02 RID: 39682
	[DllImport("AkSoundEngine")]
	public static extern ulong CSharp_GetGameObjectFromPlayingID(uint jarg1);

	// Token: 0x06009B03 RID: 39683
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetPlayingIDsFromGameObject(ulong jarg1, ref uint jarg2, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] jarg3);

	// Token: 0x06009B04 RID: 39684
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetCustomPropertyValue__SWIG_0(uint jarg1, uint jarg2, out int jarg3);

	// Token: 0x06009B05 RID: 39685
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetCustomPropertyValue__SWIG_1(uint jarg1, uint jarg2, out float jarg3);

	// Token: 0x06009B06 RID: 39686
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AK_SPEAKER_SETUP_FIX_LEFT_TO_CENTER(ref uint jarg1);

	// Token: 0x06009B07 RID: 39687
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AK_SPEAKER_SETUP_FIX_REAR_TO_SIDE(ref uint jarg1);

	// Token: 0x06009B08 RID: 39688
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AK_SPEAKER_SETUP_CONVERT_TO_SUPPORTED(ref uint jarg1);

	// Token: 0x06009B09 RID: 39689
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_ChannelMaskToNumChannels(uint jarg1);

	// Token: 0x06009B0A RID: 39690
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_ChannelMaskFromNumChannels(uint jarg1);

	// Token: 0x06009B0B RID: 39691
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_ChannelBitToIndex(uint jarg1, uint jarg2);

	// Token: 0x06009B0C RID: 39692
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_HasSurroundChannels(uint jarg1);

	// Token: 0x06009B0D RID: 39693
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_HasStrictlyOnePairOfSurroundChannels(uint jarg1);

	// Token: 0x06009B0E RID: 39694
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_HasSideAndRearChannels(uint jarg1);

	// Token: 0x06009B0F RID: 39695
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_HasHeightChannels(uint jarg1);

	// Token: 0x06009B10 RID: 39696
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_BackToSideChannels(uint jarg1);

	// Token: 0x06009B11 RID: 39697
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_StdChannelIndexToDisplayIndex(int jarg1, uint jarg2, uint jarg3);

	// Token: 0x06009B12 RID: 39698
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkChannelConfig_uNumChannels_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B13 RID: 39699
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkChannelConfig_uNumChannels_get(IntPtr jarg1);

	// Token: 0x06009B14 RID: 39700
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkChannelConfig_eConfigType_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B15 RID: 39701
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkChannelConfig_eConfigType_get(IntPtr jarg1);

	// Token: 0x06009B16 RID: 39702
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkChannelConfig_uChannelMask_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B17 RID: 39703
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkChannelConfig_uChannelMask_get(IntPtr jarg1);

	// Token: 0x06009B18 RID: 39704
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkChannelConfig__SWIG_0();

	// Token: 0x06009B19 RID: 39705
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkChannelConfig__SWIG_1(uint jarg1, uint jarg2);

	// Token: 0x06009B1A RID: 39706
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkChannelConfig_Clear(IntPtr jarg1);

	// Token: 0x06009B1B RID: 39707
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkChannelConfig_SetStandard(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B1C RID: 39708
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkChannelConfig_SetStandardOrAnonymous(IntPtr jarg1, uint jarg2, uint jarg3);

	// Token: 0x06009B1D RID: 39709
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkChannelConfig_SetAnonymous(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B1E RID: 39710
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkChannelConfig_SetAmbisonic(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B1F RID: 39711
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkChannelConfig_IsValid(IntPtr jarg1);

	// Token: 0x06009B20 RID: 39712
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkChannelConfig_Serialize(IntPtr jarg1);

	// Token: 0x06009B21 RID: 39713
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkChannelConfig_Deserialize(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B22 RID: 39714
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkChannelConfig_RemoveLFE(IntPtr jarg1);

	// Token: 0x06009B23 RID: 39715
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkChannelConfig_RemoveCenter(IntPtr jarg1);

	// Token: 0x06009B24 RID: 39716
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkChannelConfig_IsChannelConfigSupported(IntPtr jarg1);

	// Token: 0x06009B25 RID: 39717
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkChannelConfig(IntPtr jarg1);

	// Token: 0x06009B26 RID: 39718
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkImageSourceParams__SWIG_0();

	// Token: 0x06009B27 RID: 39719
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkImageSourceParams__SWIG_1(IntPtr jarg1, float jarg2, float jarg3);

	// Token: 0x06009B28 RID: 39720
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkImageSourceParams_sourcePosition_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009B29 RID: 39721
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkImageSourceParams_sourcePosition_get(IntPtr jarg1);

	// Token: 0x06009B2A RID: 39722
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkImageSourceParams_fDistanceScalingFactor_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B2B RID: 39723
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkImageSourceParams_fDistanceScalingFactor_get(IntPtr jarg1);

	// Token: 0x06009B2C RID: 39724
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkImageSourceParams_fLevel_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B2D RID: 39725
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkImageSourceParams_fLevel_get(IntPtr jarg1);

	// Token: 0x06009B2E RID: 39726
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkImageSourceParams(IntPtr jarg1);

	// Token: 0x06009B2F RID: 39727
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_kDefaultMaxPathLength_get();

	// Token: 0x06009B30 RID: 39728
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_g_SpatialAudioPoolId_set(int jarg1);

	// Token: 0x06009B31 RID: 39729
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_g_SpatialAudioPoolId_get();

	// Token: 0x06009B32 RID: 39730
	[DllImport("AkSoundEngine")]
	public static extern int CSharp__ArrayPoolSpatialAudio_Get();

	// Token: 0x06009B33 RID: 39731
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new__ArrayPoolSpatialAudio();

	// Token: 0x06009B34 RID: 39732
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete__ArrayPoolSpatialAudio(IntPtr jarg1);

	// Token: 0x06009B35 RID: 39733
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkSpatialAudioInitSettings();

	// Token: 0x06009B36 RID: 39734
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSpatialAudioInitSettings_uPoolID_set(IntPtr jarg1, int jarg2);

	// Token: 0x06009B37 RID: 39735
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkSpatialAudioInitSettings_uPoolID_get(IntPtr jarg1);

	// Token: 0x06009B38 RID: 39736
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSpatialAudioInitSettings_uPoolSize_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B39 RID: 39737
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkSpatialAudioInitSettings_uPoolSize_get(IntPtr jarg1);

	// Token: 0x06009B3A RID: 39738
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSpatialAudioInitSettings_uMaxSoundPropagationDepth_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B3B RID: 39739
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkSpatialAudioInitSettings_uMaxSoundPropagationDepth_get(IntPtr jarg1);

	// Token: 0x06009B3C RID: 39740
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSpatialAudioInitSettings_uDiffractionFlags_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B3D RID: 39741
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkSpatialAudioInitSettings_uDiffractionFlags_get(IntPtr jarg1);

	// Token: 0x06009B3E RID: 39742
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSpatialAudioInitSettings_fDiffractionShadowAttenFactor_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B3F RID: 39743
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkSpatialAudioInitSettings_fDiffractionShadowAttenFactor_get(IntPtr jarg1);

	// Token: 0x06009B40 RID: 39744
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSpatialAudioInitSettings_fDiffractionShadowDegrees_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B41 RID: 39745
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkSpatialAudioInitSettings_fDiffractionShadowDegrees_get(IntPtr jarg1);

	// Token: 0x06009B42 RID: 39746
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkSpatialAudioInitSettings(IntPtr jarg1);

	// Token: 0x06009B43 RID: 39747
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkImageSourceSettings__SWIG_0();

	// Token: 0x06009B44 RID: 39748
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkImageSourceSettings__SWIG_1(IntPtr jarg1, float jarg2, float jarg3);

	// Token: 0x06009B45 RID: 39749
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkImageSourceSettings(IntPtr jarg1);

	// Token: 0x06009B46 RID: 39750
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkImageSourceSettings_SetOneTexture(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B47 RID: 39751
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkImageSourceSettings_SetName(IntPtr jarg1, [MarshalAs(UnmanagedType.LPStr)] string jarg2);

	// Token: 0x06009B48 RID: 39752
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkImageSourceSettings_params__set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009B49 RID: 39753
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkImageSourceSettings_params__get(IntPtr jarg1);

	// Token: 0x06009B4A RID: 39754
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkEmitterSettings();

	// Token: 0x06009B4B RID: 39755
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkEmitterSettings_reflectAuxBusID_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B4C RID: 39756
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkEmitterSettings_reflectAuxBusID_get(IntPtr jarg1);

	// Token: 0x06009B4D RID: 39757
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkEmitterSettings_reflectionMaxPathLength_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B4E RID: 39758
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkEmitterSettings_reflectionMaxPathLength_get(IntPtr jarg1);

	// Token: 0x06009B4F RID: 39759
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkEmitterSettings_reflectionsAuxBusGain_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B50 RID: 39760
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkEmitterSettings_reflectionsAuxBusGain_get(IntPtr jarg1);

	// Token: 0x06009B51 RID: 39761
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkEmitterSettings_reflectionsOrder_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B52 RID: 39762
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkEmitterSettings_reflectionsOrder_get(IntPtr jarg1);

	// Token: 0x06009B53 RID: 39763
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkEmitterSettings_reflectorFilterMask_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B54 RID: 39764
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkEmitterSettings_reflectorFilterMask_get(IntPtr jarg1);

	// Token: 0x06009B55 RID: 39765
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkEmitterSettings_roomReverbAuxBusGain_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B56 RID: 39766
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkEmitterSettings_roomReverbAuxBusGain_get(IntPtr jarg1);

	// Token: 0x06009B57 RID: 39767
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkEmitterSettings_useImageSources_set(IntPtr jarg1, byte jarg2);

	// Token: 0x06009B58 RID: 39768
	[DllImport("AkSoundEngine")]
	public static extern byte CSharp_AkEmitterSettings_useImageSources_get(IntPtr jarg1);

	// Token: 0x06009B59 RID: 39769
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkEmitterSettings(IntPtr jarg1);

	// Token: 0x06009B5A RID: 39770
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkTriangle();

	// Token: 0x06009B5B RID: 39771
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTriangle_point0_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009B5C RID: 39772
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkTriangle_point0_get(IntPtr jarg1);

	// Token: 0x06009B5D RID: 39773
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTriangle_point1_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009B5E RID: 39774
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkTriangle_point1_get(IntPtr jarg1);

	// Token: 0x06009B5F RID: 39775
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTriangle_point2_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009B60 RID: 39776
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkTriangle_point2_get(IntPtr jarg1);

	// Token: 0x06009B61 RID: 39777
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTriangle_textureID_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B62 RID: 39778
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkTriangle_textureID_get(IntPtr jarg1);

	// Token: 0x06009B63 RID: 39779
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTriangle_reflectorChannelMask_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B64 RID: 39780
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkTriangle_reflectorChannelMask_get(IntPtr jarg1);

	// Token: 0x06009B65 RID: 39781
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTriangle_strName_set(IntPtr jarg1, [MarshalAs(UnmanagedType.LPStr)] string jarg2);

	// Token: 0x06009B66 RID: 39782
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkTriangle_strName_get(IntPtr jarg1);

	// Token: 0x06009B67 RID: 39783
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkTriangle(IntPtr jarg1);

	// Token: 0x06009B68 RID: 39784
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSoundPathInfo_imageSource_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009B69 RID: 39785
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkSoundPathInfo_imageSource_get(IntPtr jarg1);

	// Token: 0x06009B6A RID: 39786
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSoundPathInfo_numReflections_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B6B RID: 39787
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkSoundPathInfo_numReflections_get(IntPtr jarg1);

	// Token: 0x06009B6C RID: 39788
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSoundPathInfo_occlusionPoint_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009B6D RID: 39789
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkSoundPathInfo_occlusionPoint_get(IntPtr jarg1);

	// Token: 0x06009B6E RID: 39790
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSoundPathInfo_isOccluded_set(IntPtr jarg1, bool jarg2);

	// Token: 0x06009B6F RID: 39791
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkSoundPathInfo_isOccluded_get(IntPtr jarg1);

	// Token: 0x06009B70 RID: 39792
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkSoundPathInfo();

	// Token: 0x06009B71 RID: 39793
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkSoundPathInfo(IntPtr jarg1);

	// Token: 0x06009B72 RID: 39794
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPropagationPathInfo_nodePoint_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009B73 RID: 39795
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPropagationPathInfo_nodePoint_get(IntPtr jarg1);

	// Token: 0x06009B74 RID: 39796
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPropagationPathInfo_numNodes_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B75 RID: 39797
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkPropagationPathInfo_numNodes_get(IntPtr jarg1);

	// Token: 0x06009B76 RID: 39798
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPropagationPathInfo_length_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B77 RID: 39799
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPropagationPathInfo_length_get(IntPtr jarg1);

	// Token: 0x06009B78 RID: 39800
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPropagationPathInfo_gain_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B79 RID: 39801
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPropagationPathInfo_gain_get(IntPtr jarg1);

	// Token: 0x06009B7A RID: 39802
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPropagationPathInfo_dryDiffractionAngle_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B7B RID: 39803
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPropagationPathInfo_dryDiffractionAngle_get(IntPtr jarg1);

	// Token: 0x06009B7C RID: 39804
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPropagationPathInfo_wetDiffractionAngle_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B7D RID: 39805
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPropagationPathInfo_wetDiffractionAngle_get(IntPtr jarg1);

	// Token: 0x06009B7E RID: 39806
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkPropagationPathInfo();

	// Token: 0x06009B7F RID: 39807
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkPropagationPathInfo(IntPtr jarg1);

	// Token: 0x06009B80 RID: 39808
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkRoomParams();

	// Token: 0x06009B81 RID: 39809
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkRoomParams_Up_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009B82 RID: 39810
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkRoomParams_Up_get(IntPtr jarg1);

	// Token: 0x06009B83 RID: 39811
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkRoomParams_Front_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009B84 RID: 39812
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkRoomParams_Front_get(IntPtr jarg1);

	// Token: 0x06009B85 RID: 39813
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkRoomParams_ReverbAuxBus_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009B86 RID: 39814
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkRoomParams_ReverbAuxBus_get(IntPtr jarg1);

	// Token: 0x06009B87 RID: 39815
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkRoomParams_ReverbLevel_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B88 RID: 39816
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkRoomParams_ReverbLevel_get(IntPtr jarg1);

	// Token: 0x06009B89 RID: 39817
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkRoomParams_WallOcclusion_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B8A RID: 39818
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkRoomParams_WallOcclusion_get(IntPtr jarg1);

	// Token: 0x06009B8B RID: 39819
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkRoomParams_Priority_set(IntPtr jarg1, int jarg2);

	// Token: 0x06009B8C RID: 39820
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkRoomParams_Priority_get(IntPtr jarg1);

	// Token: 0x06009B8D RID: 39821
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkRoomParams_RoomGameObj_AuxSendLevelToSelf_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009B8E RID: 39822
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkRoomParams_RoomGameObj_AuxSendLevelToSelf_get(IntPtr jarg1);

	// Token: 0x06009B8F RID: 39823
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkRoomParams_RoomGameObj_KeepRegistered_set(IntPtr jarg1, bool jarg2);

	// Token: 0x06009B90 RID: 39824
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkRoomParams_RoomGameObj_KeepRegistered_get(IntPtr jarg1);

	// Token: 0x06009B91 RID: 39825
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkRoomParams(IntPtr jarg1);

	// Token: 0x06009B92 RID: 39826
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetPoolID();

	// Token: 0x06009B93 RID: 39827
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RegisterEmitter(ulong jarg1, IntPtr jarg2);

	// Token: 0x06009B94 RID: 39828
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnregisterEmitter(ulong jarg1);

	// Token: 0x06009B95 RID: 39829
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetEmitterAuxSendValues(ulong jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x06009B96 RID: 39830
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetImageSource__SWIG_0(uint jarg1, IntPtr jarg2, uint jarg3, ulong jarg4, ulong jarg5);

	// Token: 0x06009B97 RID: 39831
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetImageSource__SWIG_1(uint jarg1, IntPtr jarg2, uint jarg3, ulong jarg4);

	// Token: 0x06009B98 RID: 39832
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RemoveImageSource__SWIG_0(uint jarg1, uint jarg2, ulong jarg3);

	// Token: 0x06009B99 RID: 39833
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RemoveImageSource__SWIG_1(uint jarg1, uint jarg2);

	// Token: 0x06009B9A RID: 39834
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetGeometry(ulong jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x06009B9B RID: 39835
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RemoveGeometry(ulong jarg1);

	// Token: 0x06009B9C RID: 39836
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RemoveRoom(ulong jarg1);

	// Token: 0x06009B9D RID: 39837
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RemovePortal(ulong jarg1);

	// Token: 0x06009B9E RID: 39838
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetGameObjectInRoom(ulong jarg1, ulong jarg2);

	// Token: 0x06009B9F RID: 39839
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetEmitterObstruction(ulong jarg1, float jarg2);

	// Token: 0x06009BA0 RID: 39840
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetPortalObstruction(ulong jarg1, float jarg2);

	// Token: 0x06009BA1 RID: 39841
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlatformInitSettings_threadLEngine_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009BA2 RID: 39842
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlatformInitSettings_threadLEngine_get(IntPtr jarg1);

	// Token: 0x06009BA3 RID: 39843
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlatformInitSettings_threadBankManager_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009BA4 RID: 39844
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlatformInitSettings_threadBankManager_get(IntPtr jarg1);

	// Token: 0x06009BA5 RID: 39845
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlatformInitSettings_threadMonitor_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009BA6 RID: 39846
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlatformInitSettings_threadMonitor_get(IntPtr jarg1);

	// Token: 0x06009BA7 RID: 39847
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlatformInitSettings_uLEngineDefaultPoolSize_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BA8 RID: 39848
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkPlatformInitSettings_uLEngineDefaultPoolSize_get(IntPtr jarg1);

	// Token: 0x06009BA9 RID: 39849
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlatformInitSettings_fLEngineDefaultPoolRatioThreshold_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009BAA RID: 39850
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkPlatformInitSettings_fLEngineDefaultPoolRatioThreshold_get(IntPtr jarg1);

	// Token: 0x06009BAB RID: 39851
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlatformInitSettings_uNumRefillsInVoice_set(IntPtr jarg1, ushort jarg2);

	// Token: 0x06009BAC RID: 39852
	[DllImport("AkSoundEngine")]
	public static extern ushort CSharp_AkPlatformInitSettings_uNumRefillsInVoice_get(IntPtr jarg1);

	// Token: 0x06009BAD RID: 39853
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlatformInitSettings_uSampleRate_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BAE RID: 39854
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkPlatformInitSettings_uSampleRate_get(IntPtr jarg1);

	// Token: 0x06009BAF RID: 39855
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlatformInitSettings_eAudioAPI_set(IntPtr jarg1, int jarg2);

	// Token: 0x06009BB0 RID: 39856
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPlatformInitSettings_eAudioAPI_get(IntPtr jarg1);

	// Token: 0x06009BB1 RID: 39857
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkPlatformInitSettings_bGlobalFocus_set(IntPtr jarg1, bool jarg2);

	// Token: 0x06009BB2 RID: 39858
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkPlatformInitSettings_bGlobalFocus_get(IntPtr jarg1);

	// Token: 0x06009BB3 RID: 39859
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkPlatformInitSettings();

	// Token: 0x06009BB4 RID: 39860
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkPlatformInitSettings(IntPtr jarg1);

	// Token: 0x06009BB5 RID: 39861
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkStreamMgrSettings_uMemorySize_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BB6 RID: 39862
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkStreamMgrSettings_uMemorySize_get(IntPtr jarg1);

	// Token: 0x06009BB7 RID: 39863
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkStreamMgrSettings();

	// Token: 0x06009BB8 RID: 39864
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkStreamMgrSettings(IntPtr jarg1);

	// Token: 0x06009BB9 RID: 39865
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkDeviceSettings_pIOMemory_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009BBA RID: 39866
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkDeviceSettings_pIOMemory_get(IntPtr jarg1);

	// Token: 0x06009BBB RID: 39867
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkDeviceSettings_uIOMemorySize_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BBC RID: 39868
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkDeviceSettings_uIOMemorySize_get(IntPtr jarg1);

	// Token: 0x06009BBD RID: 39869
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkDeviceSettings_uIOMemoryAlignment_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BBE RID: 39870
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkDeviceSettings_uIOMemoryAlignment_get(IntPtr jarg1);

	// Token: 0x06009BBF RID: 39871
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkDeviceSettings_ePoolAttributes_set(IntPtr jarg1, int jarg2);

	// Token: 0x06009BC0 RID: 39872
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkDeviceSettings_ePoolAttributes_get(IntPtr jarg1);

	// Token: 0x06009BC1 RID: 39873
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkDeviceSettings_uGranularity_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BC2 RID: 39874
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkDeviceSettings_uGranularity_get(IntPtr jarg1);

	// Token: 0x06009BC3 RID: 39875
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkDeviceSettings_uSchedulerTypeFlags_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BC4 RID: 39876
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkDeviceSettings_uSchedulerTypeFlags_get(IntPtr jarg1);

	// Token: 0x06009BC5 RID: 39877
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkDeviceSettings_threadProperties_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009BC6 RID: 39878
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkDeviceSettings_threadProperties_get(IntPtr jarg1);

	// Token: 0x06009BC7 RID: 39879
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkDeviceSettings_fTargetAutoStmBufferLength_set(IntPtr jarg1, float jarg2);

	// Token: 0x06009BC8 RID: 39880
	[DllImport("AkSoundEngine")]
	public static extern float CSharp_AkDeviceSettings_fTargetAutoStmBufferLength_get(IntPtr jarg1);

	// Token: 0x06009BC9 RID: 39881
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkDeviceSettings_uMaxConcurrentIO_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BCA RID: 39882
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkDeviceSettings_uMaxConcurrentIO_get(IntPtr jarg1);

	// Token: 0x06009BCB RID: 39883
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkDeviceSettings_bUseStreamCache_set(IntPtr jarg1, bool jarg2);

	// Token: 0x06009BCC RID: 39884
	[DllImport("AkSoundEngine")]
	public static extern bool CSharp_AkDeviceSettings_bUseStreamCache_get(IntPtr jarg1);

	// Token: 0x06009BCD RID: 39885
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkDeviceSettings_uMaxCachePinnedBytes_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BCE RID: 39886
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkDeviceSettings_uMaxCachePinnedBytes_get(IntPtr jarg1);

	// Token: 0x06009BCF RID: 39887
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkDeviceSettings();

	// Token: 0x06009BD0 RID: 39888
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkDeviceSettings(IntPtr jarg1);

	// Token: 0x06009BD1 RID: 39889
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkThreadProperties_nPriority_set(IntPtr jarg1, int jarg2);

	// Token: 0x06009BD2 RID: 39890
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkThreadProperties_nPriority_get(IntPtr jarg1);

	// Token: 0x06009BD3 RID: 39891
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkThreadProperties_dwAffinityMask_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BD4 RID: 39892
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkThreadProperties_dwAffinityMask_get(IntPtr jarg1);

	// Token: 0x06009BD5 RID: 39893
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkThreadProperties_uStackSize_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BD6 RID: 39894
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkThreadProperties_uStackSize_get(IntPtr jarg1);

	// Token: 0x06009BD7 RID: 39895
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkThreadProperties();

	// Token: 0x06009BD8 RID: 39896
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkThreadProperties(IntPtr jarg1);

	// Token: 0x06009BD9 RID: 39897
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_SetErrorLogger__SWIG_0(AkLogger.ErrorLoggerInteropDelegate jarg1);

	// Token: 0x06009BDA RID: 39898
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_SetErrorLogger__SWIG_1();

	// Token: 0x06009BDB RID: 39899
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_SetAudioInputCallbacks(AkAudioInputManager.AudioSamplesInteropDelegate jarg1, AkAudioInputManager.AudioFormatInteropDelegate jarg2);

	// Token: 0x06009BDC RID: 39900
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTriangleProxy_Clear(IntPtr jarg1);

	// Token: 0x06009BDD RID: 39901
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkTriangleProxy_DeleteName(IntPtr jarg1);

	// Token: 0x06009BDE RID: 39902
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkTriangleProxy_GetSizeOf();

	// Token: 0x06009BDF RID: 39903
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkTriangleProxy();

	// Token: 0x06009BE0 RID: 39904
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkTriangleProxy(IntPtr jarg1);

	// Token: 0x06009BE1 RID: 39905
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkSoundPathInfoProxy_GetSizeOf();

	// Token: 0x06009BE2 RID: 39906
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkSoundPathInfoProxy_GetReflectionPoint(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BE3 RID: 39907
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkSoundPathInfoProxy_GetTriangle(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BE4 RID: 39908
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkSoundPathInfoProxy();

	// Token: 0x06009BE5 RID: 39909
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkSoundPathInfoProxy(IntPtr jarg1);

	// Token: 0x06009BE6 RID: 39910
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AkPropagationPathInfoProxy_GetSizeOf();

	// Token: 0x06009BE7 RID: 39911
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPropagationPathInfoProxy_GetNodePoint(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BE8 RID: 39912
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkPropagationPathInfoProxy();

	// Token: 0x06009BE9 RID: 39913
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkPropagationPathInfoProxy(IntPtr jarg1);

	// Token: 0x06009BEA RID: 39914
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSoundPropagationPathParams_listenerPos_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009BEB RID: 39915
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkSoundPropagationPathParams_listenerPos_get(IntPtr jarg1);

	// Token: 0x06009BEC RID: 39916
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSoundPropagationPathParams_emitterPos_set(IntPtr jarg1, IntPtr jarg2);

	// Token: 0x06009BED RID: 39917
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkSoundPropagationPathParams_emitterPos_get(IntPtr jarg1);

	// Token: 0x06009BEE RID: 39918
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_AkSoundPropagationPathParams_numValidPaths_set(IntPtr jarg1, uint jarg2);

	// Token: 0x06009BEF RID: 39919
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_AkSoundPropagationPathParams_numValidPaths_get(IntPtr jarg1);

	// Token: 0x06009BF0 RID: 39920
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_new_AkSoundPropagationPathParams();

	// Token: 0x06009BF1 RID: 39921
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_delete_AkSoundPropagationPathParams(IntPtr jarg1);

	// Token: 0x06009BF2 RID: 39922
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_Init(IntPtr jarg1, IntPtr jarg2, IntPtr jarg3, IntPtr jarg4, IntPtr jarg5, IntPtr jarg6, IntPtr jarg7, uint jarg8);

	// Token: 0x06009BF3 RID: 39923
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_Term();

	// Token: 0x06009BF4 RID: 39924
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RegisterGameObjInternal(ulong jarg1);

	// Token: 0x06009BF5 RID: 39925
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnregisterGameObjInternal(ulong jarg1);

	// Token: 0x06009BF6 RID: 39926
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RegisterGameObjInternal_WithName(ulong jarg1, [MarshalAs(UnmanagedType.LPWStr)] string jarg2);

	// Token: 0x06009BF7 RID: 39927
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetBasePath([MarshalAs(UnmanagedType.LPWStr)] string jarg1);

	// Token: 0x06009BF8 RID: 39928
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetCurrentLanguage([MarshalAs(UnmanagedType.LPWStr)] string jarg1);

	// Token: 0x06009BF9 RID: 39929
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_LoadFilePackage([MarshalAs(UnmanagedType.LPWStr)] string jarg1, out uint jarg2, int jarg3);

	// Token: 0x06009BFA RID: 39930
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AddBasePath([MarshalAs(UnmanagedType.LPWStr)] string jarg1);

	// Token: 0x06009BFB RID: 39931
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetGameName([MarshalAs(UnmanagedType.LPWStr)] string jarg1);

	// Token: 0x06009BFC RID: 39932
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetDecodedBankPath([MarshalAs(UnmanagedType.LPWStr)] string jarg1);

	// Token: 0x06009BFD RID: 39933
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_LoadAndDecodeBank([MarshalAs(UnmanagedType.LPWStr)] string jarg1, bool jarg2, out uint jarg3);

	// Token: 0x06009BFE RID: 39934
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_LoadAndDecodeBankFromMemory(IntPtr jarg1, uint jarg2, bool jarg3, [MarshalAs(UnmanagedType.LPWStr)] string jarg4, bool jarg5, out uint jarg6);

	// Token: 0x06009BFF RID: 39935
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_GetCurrentLanguage();

	// Token: 0x06009C00 RID: 39936
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnloadFilePackage(uint jarg1);

	// Token: 0x06009C01 RID: 39937
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnloadAllFilePackages();

	// Token: 0x06009C02 RID: 39938
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetObjectPosition(ulong jarg1, float jarg2, float jarg3, float jarg4, float jarg5, float jarg6, float jarg7, float jarg8, float jarg9, float jarg10);

	// Token: 0x06009C03 RID: 39939
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_GetSourceMultiplePlayPositions(uint jarg1, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] jarg2, [MarshalAs(UnmanagedType.LPArray)] [Out] uint[] jarg3, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] jarg4, ref uint jarg5, bool jarg6);

	// Token: 0x06009C04 RID: 39940
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetListeners(ulong jarg1, ulong[] jarg2, uint jarg3);

	// Token: 0x06009C05 RID: 39941
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetDefaultListeners(ulong[] jarg1, uint jarg2);

	// Token: 0x06009C06 RID: 39942
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_AddOutput(IntPtr jarg1, out ulong jarg2, ulong[] jarg3, uint jarg4);

	// Token: 0x06009C07 RID: 39943
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_GetDefaultStreamSettings(IntPtr jarg1);

	// Token: 0x06009C08 RID: 39944
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_GetDefaultDeviceSettings(IntPtr jarg1);

	// Token: 0x06009C09 RID: 39945
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_GetDefaultMusicSettings(IntPtr jarg1);

	// Token: 0x06009C0A RID: 39946
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_GetDefaultInitSettings(IntPtr jarg1);

	// Token: 0x06009C0B RID: 39947
	[DllImport("AkSoundEngine")]
	public static extern void CSharp_GetDefaultPlatformInitSettings(IntPtr jarg1);

	// Token: 0x06009C0C RID: 39948
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_GetMajorMinorVersion();

	// Token: 0x06009C0D RID: 39949
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_GetSubminorBuildVersion();

	// Token: 0x06009C0E RID: 39950
	[DllImport("AkSoundEngine")]
	public static extern uint CSharp_GetDeviceIDFromName([MarshalAs(UnmanagedType.LPWStr)] string jarg1);

	// Token: 0x06009C0F RID: 39951
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_GetWindowsDeviceName(int jarg1, out uint jarg2);

	// Token: 0x06009C10 RID: 39952
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_QueryIndirectPaths(ulong jarg1, IntPtr jarg2, IntPtr jarg3, uint jarg4);

	// Token: 0x06009C11 RID: 39953
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_QuerySoundPropagationPaths(ulong jarg1, IntPtr jarg2, IntPtr jarg3, uint jarg4);

	// Token: 0x06009C12 RID: 39954
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRoomPortal(ulong jarg1, IntPtr jarg2, IntPtr jarg3, bool jarg4, ulong jarg5, ulong jarg6);

	// Token: 0x06009C13 RID: 39955
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_SetRoom(ulong jarg1, IntPtr jarg2, [MarshalAs(UnmanagedType.LPStr)] string jarg3);

	// Token: 0x06009C14 RID: 39956
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_RegisterSpatialAudioListener(ulong jarg1);

	// Token: 0x06009C15 RID: 39957
	[DllImport("AkSoundEngine")]
	public static extern int CSharp_UnregisterSpatialAudioListener(ulong jarg1);

	// Token: 0x06009C16 RID: 39958
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPlaylist_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06009C17 RID: 39959
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMIDIPost_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06009C18 RID: 39960
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkEventCallbackInfo_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06009C19 RID: 39961
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMIDIEventCallbackInfo_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06009C1A RID: 39962
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMarkerCallbackInfo_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06009C1B RID: 39963
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkDurationCallbackInfo_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06009C1C RID: 39964
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkDynamicSequenceItemCallbackInfo_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06009C1D RID: 39965
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMusicSyncCallbackInfo_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06009C1E RID: 39966
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkMusicPlaylistCallbackInfo_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06009C1F RID: 39967
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkTriangleProxy_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06009C20 RID: 39968
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkSoundPathInfoProxy_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06009C21 RID: 39969
	[DllImport("AkSoundEngine")]
	public static extern IntPtr CSharp_AkPropagationPathInfoProxy_SWIGUpcast(IntPtr jarg1);
}
