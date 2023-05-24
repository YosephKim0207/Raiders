using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020018B0 RID: 6320
public class AkSoundEngine
{
	// Token: 0x170016E1 RID: 5857
	// (get) Token: 0x060096D2 RID: 38610 RVA: 0x003E913C File Offset: 0x003E733C
	public static uint AK_SOUNDBANK_VERSION
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AK_SOUNDBANK_VERSION_get();
		}
	}

	// Token: 0x170016E2 RID: 5858
	// (get) Token: 0x060096D3 RID: 38611 RVA: 0x003E9144 File Offset: 0x003E7344
	public static ushort AK_INT
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AK_INT_get();
		}
	}

	// Token: 0x170016E3 RID: 5859
	// (get) Token: 0x060096D4 RID: 38612 RVA: 0x003E914C File Offset: 0x003E734C
	public static ushort AK_FLOAT
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AK_FLOAT_get();
		}
	}

	// Token: 0x170016E4 RID: 5860
	// (get) Token: 0x060096D5 RID: 38613 RVA: 0x003E9154 File Offset: 0x003E7354
	public static byte AK_INTERLEAVED
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AK_INTERLEAVED_get();
		}
	}

	// Token: 0x170016E5 RID: 5861
	// (get) Token: 0x060096D6 RID: 38614 RVA: 0x003E915C File Offset: 0x003E735C
	public static byte AK_NONINTERLEAVED
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AK_NONINTERLEAVED_get();
		}
	}

	// Token: 0x170016E6 RID: 5862
	// (get) Token: 0x060096D7 RID: 38615 RVA: 0x003E9164 File Offset: 0x003E7364
	public static uint AK_LE_NATIVE_BITSPERSAMPLE
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AK_LE_NATIVE_BITSPERSAMPLE_get();
		}
	}

	// Token: 0x170016E7 RID: 5863
	// (get) Token: 0x060096D8 RID: 38616 RVA: 0x003E916C File Offset: 0x003E736C
	public static uint AK_LE_NATIVE_SAMPLETYPE
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AK_LE_NATIVE_SAMPLETYPE_get();
		}
	}

	// Token: 0x170016E8 RID: 5864
	// (get) Token: 0x060096D9 RID: 38617 RVA: 0x003E9174 File Offset: 0x003E7374
	public static uint AK_LE_NATIVE_INTERLEAVE
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AK_LE_NATIVE_INTERLEAVE_get();
		}
	}

	// Token: 0x060096DA RID: 38618 RVA: 0x003E917C File Offset: 0x003E737C
	public static uint DynamicSequenceOpen(GameObject in_gameObjectID, uint in_uFlags, AkCallbackManager.EventCallback in_pfnCallback, object in_pCookie, AkDynamicSequenceType in_eDynamicSequenceType)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		in_pCookie = AkCallbackManager.EventCallbackPackage.Create(in_pfnCallback, in_pCookie, ref in_uFlags);
		uint num = AkSoundEnginePINVOKE.CSharp_DynamicSequenceOpen__SWIG_0(akGameObjectID, in_uFlags, (in_uFlags == 0U) ? IntPtr.Zero : ((IntPtr)1), (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), (int)in_eDynamicSequenceType);
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x060096DB RID: 38619 RVA: 0x003E91E4 File Offset: 0x003E73E4
	public static uint DynamicSequenceOpen(GameObject in_gameObjectID, uint in_uFlags, AkCallbackManager.EventCallback in_pfnCallback, object in_pCookie)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		in_pCookie = AkCallbackManager.EventCallbackPackage.Create(in_pfnCallback, in_pCookie, ref in_uFlags);
		uint num = AkSoundEnginePINVOKE.CSharp_DynamicSequenceOpen__SWIG_1(akGameObjectID, in_uFlags, (in_uFlags == 0U) ? IntPtr.Zero : ((IntPtr)1), (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x060096DC RID: 38620 RVA: 0x003E924C File Offset: 0x003E744C
	public static uint DynamicSequenceOpen(GameObject in_gameObjectID, uint in_uFlags)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		uint num = AkSoundEnginePINVOKE.CSharp_DynamicSequenceOpen__SWIG_2(akGameObjectID, in_uFlags);
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x060096DD RID: 38621 RVA: 0x003E9278 File Offset: 0x003E7478
	public static uint DynamicSequenceOpen(GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		uint num = AkSoundEnginePINVOKE.CSharp_DynamicSequenceOpen__SWIG_3(akGameObjectID);
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x060096DE RID: 38622 RVA: 0x003E92A4 File Offset: 0x003E74A4
	public static AKRESULT DynamicSequenceClose(uint in_playingID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequenceClose(in_playingID);
	}

	// Token: 0x060096DF RID: 38623 RVA: 0x003E92AC File Offset: 0x003E74AC
	public static AKRESULT DynamicSequencePlay(uint in_playingID, int in_uTransitionDuration, AkCurveInterpolation in_eFadeCurve)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequencePlay__SWIG_0(in_playingID, in_uTransitionDuration, (int)in_eFadeCurve);
	}

	// Token: 0x060096E0 RID: 38624 RVA: 0x003E92B8 File Offset: 0x003E74B8
	public static AKRESULT DynamicSequencePlay(uint in_playingID, int in_uTransitionDuration)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequencePlay__SWIG_1(in_playingID, in_uTransitionDuration);
	}

	// Token: 0x060096E1 RID: 38625 RVA: 0x003E92C4 File Offset: 0x003E74C4
	public static AKRESULT DynamicSequencePlay(uint in_playingID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequencePlay__SWIG_2(in_playingID);
	}

	// Token: 0x060096E2 RID: 38626 RVA: 0x003E92CC File Offset: 0x003E74CC
	public static AKRESULT DynamicSequencePause(uint in_playingID, int in_uTransitionDuration, AkCurveInterpolation in_eFadeCurve)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequencePause__SWIG_0(in_playingID, in_uTransitionDuration, (int)in_eFadeCurve);
	}

	// Token: 0x060096E3 RID: 38627 RVA: 0x003E92D8 File Offset: 0x003E74D8
	public static AKRESULT DynamicSequencePause(uint in_playingID, int in_uTransitionDuration)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequencePause__SWIG_1(in_playingID, in_uTransitionDuration);
	}

	// Token: 0x060096E4 RID: 38628 RVA: 0x003E92E4 File Offset: 0x003E74E4
	public static AKRESULT DynamicSequencePause(uint in_playingID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequencePause__SWIG_2(in_playingID);
	}

	// Token: 0x060096E5 RID: 38629 RVA: 0x003E92EC File Offset: 0x003E74EC
	public static AKRESULT DynamicSequenceResume(uint in_playingID, int in_uTransitionDuration, AkCurveInterpolation in_eFadeCurve)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequenceResume__SWIG_0(in_playingID, in_uTransitionDuration, (int)in_eFadeCurve);
	}

	// Token: 0x060096E6 RID: 38630 RVA: 0x003E92F8 File Offset: 0x003E74F8
	public static AKRESULT DynamicSequenceResume(uint in_playingID, int in_uTransitionDuration)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequenceResume__SWIG_1(in_playingID, in_uTransitionDuration);
	}

	// Token: 0x060096E7 RID: 38631 RVA: 0x003E9304 File Offset: 0x003E7504
	public static AKRESULT DynamicSequenceResume(uint in_playingID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequenceResume__SWIG_2(in_playingID);
	}

	// Token: 0x060096E8 RID: 38632 RVA: 0x003E930C File Offset: 0x003E750C
	public static AKRESULT DynamicSequenceStop(uint in_playingID, int in_uTransitionDuration, AkCurveInterpolation in_eFadeCurve)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequenceStop__SWIG_0(in_playingID, in_uTransitionDuration, (int)in_eFadeCurve);
	}

	// Token: 0x060096E9 RID: 38633 RVA: 0x003E9318 File Offset: 0x003E7518
	public static AKRESULT DynamicSequenceStop(uint in_playingID, int in_uTransitionDuration)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequenceStop__SWIG_1(in_playingID, in_uTransitionDuration);
	}

	// Token: 0x060096EA RID: 38634 RVA: 0x003E9324 File Offset: 0x003E7524
	public static AKRESULT DynamicSequenceStop(uint in_playingID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequenceStop__SWIG_2(in_playingID);
	}

	// Token: 0x060096EB RID: 38635 RVA: 0x003E932C File Offset: 0x003E752C
	public static AKRESULT DynamicSequenceBreak(uint in_playingID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequenceBreak(in_playingID);
	}

	// Token: 0x060096EC RID: 38636 RVA: 0x003E9334 File Offset: 0x003E7534
	public static AKRESULT DynamicSequenceGetPauseTimes(uint in_playingID, out uint out_uTime, out uint out_uDuration)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequenceGetPauseTimes(in_playingID, out out_uTime, out out_uDuration);
	}

	// Token: 0x060096ED RID: 38637 RVA: 0x003E9340 File Offset: 0x003E7540
	public static AkPlaylist DynamicSequenceLockPlaylist(uint in_playingID)
	{
		IntPtr intPtr = AkSoundEnginePINVOKE.CSharp_DynamicSequenceLockPlaylist(in_playingID);
		return (!(intPtr == IntPtr.Zero)) ? new AkPlaylist(intPtr, false) : null;
	}

	// Token: 0x060096EE RID: 38638 RVA: 0x003E9374 File Offset: 0x003E7574
	public static AKRESULT DynamicSequenceUnlockPlaylist(uint in_playingID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_DynamicSequenceUnlockPlaylist(in_playingID);
	}

	// Token: 0x060096EF RID: 38639 RVA: 0x003E937C File Offset: 0x003E757C
	public static bool IsInitialized()
	{
		return AkSoundEnginePINVOKE.CSharp_IsInitialized();
	}

	// Token: 0x060096F0 RID: 38640 RVA: 0x003E9384 File Offset: 0x003E7584
	public static AKRESULT GetAudioSettings(AkAudioSettings out_audioSettings)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetAudioSettings(AkAudioSettings.getCPtr(out_audioSettings));
	}

	// Token: 0x060096F1 RID: 38641 RVA: 0x003E9394 File Offset: 0x003E7594
	public static AkChannelConfig GetSpeakerConfiguration(ulong in_idOutput)
	{
		return new AkChannelConfig(AkSoundEnginePINVOKE.CSharp_GetSpeakerConfiguration__SWIG_0(in_idOutput), true);
	}

	// Token: 0x060096F2 RID: 38642 RVA: 0x003E93B0 File Offset: 0x003E75B0
	public static AkChannelConfig GetSpeakerConfiguration()
	{
		return new AkChannelConfig(AkSoundEnginePINVOKE.CSharp_GetSpeakerConfiguration__SWIG_1(), true);
	}

	// Token: 0x060096F3 RID: 38643 RVA: 0x003E93CC File Offset: 0x003E75CC
	public static AKRESULT GetPanningRule(out int out_ePanningRule, ulong in_idOutput)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetPanningRule__SWIG_0(out out_ePanningRule, in_idOutput);
	}

	// Token: 0x060096F4 RID: 38644 RVA: 0x003E93D8 File Offset: 0x003E75D8
	public static AKRESULT GetPanningRule(out int out_ePanningRule)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetPanningRule__SWIG_1(out out_ePanningRule);
	}

	// Token: 0x060096F5 RID: 38645 RVA: 0x003E93E0 File Offset: 0x003E75E0
	public static AKRESULT SetPanningRule(AkPanningRule in_ePanningRule, ulong in_idOutput)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetPanningRule__SWIG_0((int)in_ePanningRule, in_idOutput);
	}

	// Token: 0x060096F6 RID: 38646 RVA: 0x003E93EC File Offset: 0x003E75EC
	public static AKRESULT SetPanningRule(AkPanningRule in_ePanningRule)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetPanningRule__SWIG_1((int)in_ePanningRule);
	}

	// Token: 0x060096F7 RID: 38647 RVA: 0x003E93F4 File Offset: 0x003E75F4
	public static AKRESULT GetSpeakerAngles(float[] io_pfSpeakerAngles, ref uint io_uNumAngles, out float out_fHeightAngle, ulong in_idOutput)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetSpeakerAngles__SWIG_0(io_pfSpeakerAngles, ref io_uNumAngles, out out_fHeightAngle, in_idOutput);
	}

	// Token: 0x060096F8 RID: 38648 RVA: 0x003E9400 File Offset: 0x003E7600
	public static AKRESULT GetSpeakerAngles(float[] io_pfSpeakerAngles, ref uint io_uNumAngles, out float out_fHeightAngle)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetSpeakerAngles__SWIG_1(io_pfSpeakerAngles, ref io_uNumAngles, out out_fHeightAngle);
	}

	// Token: 0x060096F9 RID: 38649 RVA: 0x003E940C File Offset: 0x003E760C
	public static AKRESULT SetSpeakerAngles(float[] in_pfSpeakerAngles, uint in_uNumAngles, float in_fHeightAngle, ulong in_idOutput)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetSpeakerAngles__SWIG_0(in_pfSpeakerAngles, in_uNumAngles, in_fHeightAngle, in_idOutput);
	}

	// Token: 0x060096FA RID: 38650 RVA: 0x003E9418 File Offset: 0x003E7618
	public static AKRESULT SetSpeakerAngles(float[] in_pfSpeakerAngles, uint in_uNumAngles, float in_fHeightAngle)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetSpeakerAngles__SWIG_1(in_pfSpeakerAngles, in_uNumAngles, in_fHeightAngle);
	}

	// Token: 0x060096FB RID: 38651 RVA: 0x003E9424 File Offset: 0x003E7624
	public static AKRESULT SetVolumeThreshold(float in_fVolumeThresholdDB)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetVolumeThreshold(in_fVolumeThresholdDB);
	}

	// Token: 0x060096FC RID: 38652 RVA: 0x003E942C File Offset: 0x003E762C
	public static AKRESULT SetMaxNumVoicesLimit(ushort in_maxNumberVoices)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetMaxNumVoicesLimit(in_maxNumberVoices);
	}

	// Token: 0x060096FD RID: 38653 RVA: 0x003E9434 File Offset: 0x003E7634
	public static AKRESULT RenderAudio(bool in_bAllowSyncRender)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RenderAudio__SWIG_0(in_bAllowSyncRender);
	}

	// Token: 0x060096FE RID: 38654 RVA: 0x003E943C File Offset: 0x003E763C
	public static AKRESULT RenderAudio()
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RenderAudio__SWIG_1();
	}

	// Token: 0x060096FF RID: 38655 RVA: 0x003E9444 File Offset: 0x003E7644
	public static AKRESULT RegisterPluginDLL(string in_DllName)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RegisterPluginDLL(in_DllName);
	}

	// Token: 0x06009700 RID: 38656 RVA: 0x003E944C File Offset: 0x003E764C
	public static uint GetIDFromString(string in_pszString)
	{
		return AkSoundEnginePINVOKE.CSharp_GetIDFromString__SWIG_0(in_pszString);
	}

	// Token: 0x06009701 RID: 38657 RVA: 0x003E9454 File Offset: 0x003E7654
	public static uint PostEvent(uint in_eventID, GameObject in_gameObjectID, uint in_uFlags, AkCallbackManager.EventCallback in_pfnCallback, object in_pCookie, uint in_cExternals, AkExternalSourceInfo in_pExternalSources, uint in_PlayingID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		in_pCookie = AkCallbackManager.EventCallbackPackage.Create(in_pfnCallback, in_pCookie, ref in_uFlags);
		uint num = AkSoundEnginePINVOKE.CSharp_PostEvent__SWIG_0(in_eventID, akGameObjectID, in_uFlags, (in_uFlags == 0U) ? IntPtr.Zero : ((IntPtr)1), (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), in_cExternals, AkExternalSourceInfo.getCPtr(in_pExternalSources), in_PlayingID);
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x06009702 RID: 38658 RVA: 0x003E94CC File Offset: 0x003E76CC
	public static uint PostEvent(uint in_eventID, GameObject in_gameObjectID, uint in_uFlags, AkCallbackManager.EventCallback in_pfnCallback, object in_pCookie, uint in_cExternals, AkExternalSourceInfo in_pExternalSources)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		in_pCookie = AkCallbackManager.EventCallbackPackage.Create(in_pfnCallback, in_pCookie, ref in_uFlags);
		uint num = AkSoundEnginePINVOKE.CSharp_PostEvent__SWIG_1(in_eventID, akGameObjectID, in_uFlags, (in_uFlags == 0U) ? IntPtr.Zero : ((IntPtr)1), (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), in_cExternals, AkExternalSourceInfo.getCPtr(in_pExternalSources));
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x06009703 RID: 38659 RVA: 0x003E9540 File Offset: 0x003E7740
	public static uint PostEvent(uint in_eventID, GameObject in_gameObjectID, uint in_uFlags, AkCallbackManager.EventCallback in_pfnCallback, object in_pCookie, uint in_cExternals)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		in_pCookie = AkCallbackManager.EventCallbackPackage.Create(in_pfnCallback, in_pCookie, ref in_uFlags);
		uint num = AkSoundEnginePINVOKE.CSharp_PostEvent__SWIG_2(in_eventID, akGameObjectID, in_uFlags, (in_uFlags == 0U) ? IntPtr.Zero : ((IntPtr)1), (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), in_cExternals);
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x06009704 RID: 38660 RVA: 0x003E95AC File Offset: 0x003E77AC
	public static uint PostEvent(uint in_eventID, GameObject in_gameObjectID, uint in_uFlags, AkCallbackManager.EventCallback in_pfnCallback, object in_pCookie)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		in_pCookie = AkCallbackManager.EventCallbackPackage.Create(in_pfnCallback, in_pCookie, ref in_uFlags);
		uint num = AkSoundEnginePINVOKE.CSharp_PostEvent__SWIG_3(in_eventID, akGameObjectID, in_uFlags, (in_uFlags == 0U) ? IntPtr.Zero : ((IntPtr)1), (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x06009705 RID: 38661 RVA: 0x003E9618 File Offset: 0x003E7818
	public static uint PostEvent(uint in_eventID, GameObject in_gameObjectID, uint in_uFlags)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		uint num = AkSoundEnginePINVOKE.CSharp_PostEvent__SWIG_4(in_eventID, akGameObjectID, in_uFlags);
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x06009706 RID: 38662 RVA: 0x003E9644 File Offset: 0x003E7844
	public static uint PostEvent(uint in_eventID, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		uint num = AkSoundEnginePINVOKE.CSharp_PostEvent__SWIG_5(in_eventID, akGameObjectID);
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x06009707 RID: 38663 RVA: 0x003E9670 File Offset: 0x003E7870
	public static uint PostEvent(string in_pszEventName, GameObject in_gameObjectID, uint in_uFlags, AkCallbackManager.EventCallback in_pfnCallback, object in_pCookie, uint in_cExternals, AkExternalSourceInfo in_pExternalSources, uint in_PlayingID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		in_pCookie = AkCallbackManager.EventCallbackPackage.Create(in_pfnCallback, in_pCookie, ref in_uFlags);
		uint num = AkSoundEnginePINVOKE.CSharp_PostEvent__SWIG_6(in_pszEventName, akGameObjectID, in_uFlags, (in_uFlags == 0U) ? IntPtr.Zero : ((IntPtr)1), (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), in_cExternals, AkExternalSourceInfo.getCPtr(in_pExternalSources), in_PlayingID);
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x06009708 RID: 38664 RVA: 0x003E96E8 File Offset: 0x003E78E8
	public static uint PostEvent(string in_pszEventName, GameObject in_gameObjectID, uint in_uFlags, AkCallbackManager.EventCallback in_pfnCallback, object in_pCookie, uint in_cExternals, AkExternalSourceInfo in_pExternalSources)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		in_pCookie = AkCallbackManager.EventCallbackPackage.Create(in_pfnCallback, in_pCookie, ref in_uFlags);
		uint num = AkSoundEnginePINVOKE.CSharp_PostEvent__SWIG_7(in_pszEventName, akGameObjectID, in_uFlags, (in_uFlags == 0U) ? IntPtr.Zero : ((IntPtr)1), (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), in_cExternals, AkExternalSourceInfo.getCPtr(in_pExternalSources));
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x06009709 RID: 38665 RVA: 0x003E975C File Offset: 0x003E795C
	public static uint PostEvent(string in_pszEventName, GameObject in_gameObjectID, uint in_uFlags, AkCallbackManager.EventCallback in_pfnCallback, object in_pCookie, uint in_cExternals)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		in_pCookie = AkCallbackManager.EventCallbackPackage.Create(in_pfnCallback, in_pCookie, ref in_uFlags);
		uint num = AkSoundEnginePINVOKE.CSharp_PostEvent__SWIG_8(in_pszEventName, akGameObjectID, in_uFlags, (in_uFlags == 0U) ? IntPtr.Zero : ((IntPtr)1), (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), in_cExternals);
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x0600970A RID: 38666 RVA: 0x003E97C8 File Offset: 0x003E79C8
	public static uint PostEvent(string in_pszEventName, GameObject in_gameObjectID, uint in_uFlags, AkCallbackManager.EventCallback in_pfnCallback, object in_pCookie)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		in_pCookie = AkCallbackManager.EventCallbackPackage.Create(in_pfnCallback, in_pCookie, ref in_uFlags);
		uint num = AkSoundEnginePINVOKE.CSharp_PostEvent__SWIG_9(in_pszEventName, akGameObjectID, in_uFlags, (in_uFlags == 0U) ? IntPtr.Zero : ((IntPtr)1), (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x0600970B RID: 38667 RVA: 0x003E9834 File Offset: 0x003E7A34
	public static uint PostEvent(string in_pszEventName, GameObject in_gameObjectID, uint in_uFlags)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		uint num = AkSoundEnginePINVOKE.CSharp_PostEvent__SWIG_10(in_pszEventName, akGameObjectID, in_uFlags);
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x0600970C RID: 38668 RVA: 0x003E9860 File Offset: 0x003E7A60
	public static uint PostEvent(string in_pszEventName, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		uint num = AkSoundEnginePINVOKE.CSharp_PostEvent__SWIG_11(in_pszEventName, akGameObjectID);
		AkCallbackManager.SetLastAddedPlayingID(num);
		return num;
	}

	// Token: 0x0600970D RID: 38669 RVA: 0x003E988C File Offset: 0x003E7A8C
	public static AKRESULT ExecuteActionOnEvent(uint in_eventID, AkActionOnEventType in_ActionType, GameObject in_gameObjectID, int in_uTransitionDuration, AkCurveInterpolation in_eFadeCurve, uint in_PlayingID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ExecuteActionOnEvent__SWIG_0(in_eventID, (int)in_ActionType, akGameObjectID, in_uTransitionDuration, (int)in_eFadeCurve, in_PlayingID);
	}

	// Token: 0x0600970E RID: 38670 RVA: 0x003E98B4 File Offset: 0x003E7AB4
	public static AKRESULT ExecuteActionOnEvent(uint in_eventID, AkActionOnEventType in_ActionType, GameObject in_gameObjectID, int in_uTransitionDuration, AkCurveInterpolation in_eFadeCurve)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ExecuteActionOnEvent__SWIG_1(in_eventID, (int)in_ActionType, akGameObjectID, in_uTransitionDuration, (int)in_eFadeCurve);
	}

	// Token: 0x0600970F RID: 38671 RVA: 0x003E98DC File Offset: 0x003E7ADC
	public static AKRESULT ExecuteActionOnEvent(uint in_eventID, AkActionOnEventType in_ActionType, GameObject in_gameObjectID, int in_uTransitionDuration)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ExecuteActionOnEvent__SWIG_2(in_eventID, (int)in_ActionType, akGameObjectID, in_uTransitionDuration);
	}

	// Token: 0x06009710 RID: 38672 RVA: 0x003E9900 File Offset: 0x003E7B00
	public static AKRESULT ExecuteActionOnEvent(uint in_eventID, AkActionOnEventType in_ActionType, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ExecuteActionOnEvent__SWIG_3(in_eventID, (int)in_ActionType, akGameObjectID);
	}

	// Token: 0x06009711 RID: 38673 RVA: 0x003E9924 File Offset: 0x003E7B24
	public static AKRESULT ExecuteActionOnEvent(uint in_eventID, AkActionOnEventType in_ActionType)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ExecuteActionOnEvent__SWIG_4(in_eventID, (int)in_ActionType);
	}

	// Token: 0x06009712 RID: 38674 RVA: 0x003E9930 File Offset: 0x003E7B30
	public static AKRESULT ExecuteActionOnEvent(string in_pszEventName, AkActionOnEventType in_ActionType, GameObject in_gameObjectID, int in_uTransitionDuration, AkCurveInterpolation in_eFadeCurve, uint in_PlayingID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ExecuteActionOnEvent__SWIG_5(in_pszEventName, (int)in_ActionType, akGameObjectID, in_uTransitionDuration, (int)in_eFadeCurve, in_PlayingID);
	}

	// Token: 0x06009713 RID: 38675 RVA: 0x003E9958 File Offset: 0x003E7B58
	public static AKRESULT ExecuteActionOnEvent(string in_pszEventName, AkActionOnEventType in_ActionType, GameObject in_gameObjectID, int in_uTransitionDuration, AkCurveInterpolation in_eFadeCurve)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ExecuteActionOnEvent__SWIG_6(in_pszEventName, (int)in_ActionType, akGameObjectID, in_uTransitionDuration, (int)in_eFadeCurve);
	}

	// Token: 0x06009714 RID: 38676 RVA: 0x003E9980 File Offset: 0x003E7B80
	public static AKRESULT ExecuteActionOnEvent(string in_pszEventName, AkActionOnEventType in_ActionType, GameObject in_gameObjectID, int in_uTransitionDuration)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ExecuteActionOnEvent__SWIG_7(in_pszEventName, (int)in_ActionType, akGameObjectID, in_uTransitionDuration);
	}

	// Token: 0x06009715 RID: 38677 RVA: 0x003E99A4 File Offset: 0x003E7BA4
	public static AKRESULT ExecuteActionOnEvent(string in_pszEventName, AkActionOnEventType in_ActionType, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ExecuteActionOnEvent__SWIG_8(in_pszEventName, (int)in_ActionType, akGameObjectID);
	}

	// Token: 0x06009716 RID: 38678 RVA: 0x003E99C8 File Offset: 0x003E7BC8
	public static AKRESULT ExecuteActionOnEvent(string in_pszEventName, AkActionOnEventType in_ActionType)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ExecuteActionOnEvent__SWIG_9(in_pszEventName, (int)in_ActionType);
	}

	// Token: 0x06009717 RID: 38679 RVA: 0x003E99D4 File Offset: 0x003E7BD4
	public static AKRESULT PostMIDIOnEvent(uint in_eventID, GameObject in_gameObjectID, AkMIDIPostArray in_pPosts, ushort in_uNumPosts)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostMIDIOnEvent(in_eventID, akGameObjectID, in_pPosts.GetBuffer(), in_uNumPosts);
	}

	// Token: 0x06009718 RID: 38680 RVA: 0x003E9A00 File Offset: 0x003E7C00
	public static AKRESULT StopMIDIOnEvent(uint in_eventID, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_StopMIDIOnEvent__SWIG_0(in_eventID, akGameObjectID);
	}

	// Token: 0x06009719 RID: 38681 RVA: 0x003E9A24 File Offset: 0x003E7C24
	public static AKRESULT StopMIDIOnEvent(uint in_eventID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_StopMIDIOnEvent__SWIG_1(in_eventID);
	}

	// Token: 0x0600971A RID: 38682 RVA: 0x003E9A2C File Offset: 0x003E7C2C
	public static AKRESULT StopMIDIOnEvent()
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_StopMIDIOnEvent__SWIG_2();
	}

	// Token: 0x0600971B RID: 38683 RVA: 0x003E9A34 File Offset: 0x003E7C34
	public static AKRESULT PinEventInStreamCache(uint in_eventID, char in_uActivePriority, char in_uInactivePriority)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PinEventInStreamCache__SWIG_0(in_eventID, in_uActivePriority, in_uInactivePriority);
	}

	// Token: 0x0600971C RID: 38684 RVA: 0x003E9A40 File Offset: 0x003E7C40
	public static AKRESULT PinEventInStreamCache(string in_pszEventName, char in_uActivePriority, char in_uInactivePriority)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PinEventInStreamCache__SWIG_1(in_pszEventName, in_uActivePriority, in_uInactivePriority);
	}

	// Token: 0x0600971D RID: 38685 RVA: 0x003E9A4C File Offset: 0x003E7C4C
	public static AKRESULT UnpinEventInStreamCache(uint in_eventID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnpinEventInStreamCache__SWIG_0(in_eventID);
	}

	// Token: 0x0600971E RID: 38686 RVA: 0x003E9A54 File Offset: 0x003E7C54
	public static AKRESULT UnpinEventInStreamCache(string in_pszEventName)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnpinEventInStreamCache__SWIG_1(in_pszEventName);
	}

	// Token: 0x0600971F RID: 38687 RVA: 0x003E9A5C File Offset: 0x003E7C5C
	public static AKRESULT GetBufferStatusForPinnedEvent(uint in_eventID, out float out_fPercentBuffered, out int out_bCachePinnedMemoryFull)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetBufferStatusForPinnedEvent__SWIG_0(in_eventID, out out_fPercentBuffered, out out_bCachePinnedMemoryFull);
	}

	// Token: 0x06009720 RID: 38688 RVA: 0x003E9A68 File Offset: 0x003E7C68
	public static AKRESULT GetBufferStatusForPinnedEvent(string in_pszEventName, out float out_fPercentBuffered, out int out_bCachePinnedMemoryFull)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetBufferStatusForPinnedEvent__SWIG_1(in_pszEventName, out out_fPercentBuffered, out out_bCachePinnedMemoryFull);
	}

	// Token: 0x06009721 RID: 38689 RVA: 0x003E9A74 File Offset: 0x003E7C74
	public static AKRESULT SeekOnEvent(uint in_eventID, GameObject in_gameObjectID, int in_iPosition, bool in_bSeekToNearestMarker, uint in_PlayingID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SeekOnEvent__SWIG_0(in_eventID, akGameObjectID, in_iPosition, in_bSeekToNearestMarker, in_PlayingID);
	}

	// Token: 0x06009722 RID: 38690 RVA: 0x003E9A9C File Offset: 0x003E7C9C
	public static AKRESULT SeekOnEvent(uint in_eventID, GameObject in_gameObjectID, int in_iPosition, bool in_bSeekToNearestMarker)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SeekOnEvent__SWIG_1(in_eventID, akGameObjectID, in_iPosition, in_bSeekToNearestMarker);
	}

	// Token: 0x06009723 RID: 38691 RVA: 0x003E9AC0 File Offset: 0x003E7CC0
	public static AKRESULT SeekOnEvent(uint in_eventID, GameObject in_gameObjectID, int in_iPosition)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SeekOnEvent__SWIG_2(in_eventID, akGameObjectID, in_iPosition);
	}

	// Token: 0x06009724 RID: 38692 RVA: 0x003E9AE4 File Offset: 0x003E7CE4
	public static AKRESULT SeekOnEvent(string in_pszEventName, GameObject in_gameObjectID, int in_iPosition, bool in_bSeekToNearestMarker, uint in_PlayingID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SeekOnEvent__SWIG_3(in_pszEventName, akGameObjectID, in_iPosition, in_bSeekToNearestMarker, in_PlayingID);
	}

	// Token: 0x06009725 RID: 38693 RVA: 0x003E9B0C File Offset: 0x003E7D0C
	public static AKRESULT SeekOnEvent(string in_pszEventName, GameObject in_gameObjectID, int in_iPosition, bool in_bSeekToNearestMarker)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SeekOnEvent__SWIG_4(in_pszEventName, akGameObjectID, in_iPosition, in_bSeekToNearestMarker);
	}

	// Token: 0x06009726 RID: 38694 RVA: 0x003E9B30 File Offset: 0x003E7D30
	public static AKRESULT SeekOnEvent(string in_pszEventName, GameObject in_gameObjectID, int in_iPosition)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SeekOnEvent__SWIG_5(in_pszEventName, akGameObjectID, in_iPosition);
	}

	// Token: 0x06009727 RID: 38695 RVA: 0x003E9B54 File Offset: 0x003E7D54
	public static AKRESULT SeekOnEvent(uint in_eventID, GameObject in_gameObjectID, float in_fPercent, bool in_bSeekToNearestMarker, uint in_PlayingID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SeekOnEvent__SWIG_9(in_eventID, akGameObjectID, in_fPercent, in_bSeekToNearestMarker, in_PlayingID);
	}

	// Token: 0x06009728 RID: 38696 RVA: 0x003E9B7C File Offset: 0x003E7D7C
	public static AKRESULT SeekOnEvent(uint in_eventID, GameObject in_gameObjectID, float in_fPercent, bool in_bSeekToNearestMarker)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SeekOnEvent__SWIG_10(in_eventID, akGameObjectID, in_fPercent, in_bSeekToNearestMarker);
	}

	// Token: 0x06009729 RID: 38697 RVA: 0x003E9BA0 File Offset: 0x003E7DA0
	public static AKRESULT SeekOnEvent(uint in_eventID, GameObject in_gameObjectID, float in_fPercent)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SeekOnEvent__SWIG_11(in_eventID, akGameObjectID, in_fPercent);
	}

	// Token: 0x0600972A RID: 38698 RVA: 0x003E9BC4 File Offset: 0x003E7DC4
	public static AKRESULT SeekOnEvent(string in_pszEventName, GameObject in_gameObjectID, float in_fPercent, bool in_bSeekToNearestMarker, uint in_PlayingID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SeekOnEvent__SWIG_12(in_pszEventName, akGameObjectID, in_fPercent, in_bSeekToNearestMarker, in_PlayingID);
	}

	// Token: 0x0600972B RID: 38699 RVA: 0x003E9BEC File Offset: 0x003E7DEC
	public static AKRESULT SeekOnEvent(string in_pszEventName, GameObject in_gameObjectID, float in_fPercent, bool in_bSeekToNearestMarker)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SeekOnEvent__SWIG_13(in_pszEventName, akGameObjectID, in_fPercent, in_bSeekToNearestMarker);
	}

	// Token: 0x0600972C RID: 38700 RVA: 0x003E9C10 File Offset: 0x003E7E10
	public static AKRESULT SeekOnEvent(string in_pszEventName, GameObject in_gameObjectID, float in_fPercent)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SeekOnEvent__SWIG_14(in_pszEventName, akGameObjectID, in_fPercent);
	}

	// Token: 0x0600972D RID: 38701 RVA: 0x003E9C34 File Offset: 0x003E7E34
	public static void CancelEventCallbackCookie(object in_pCookie)
	{
		AkCallbackManager.RemoveEventCallbackCookie(in_pCookie);
	}

	// Token: 0x0600972E RID: 38702 RVA: 0x003E9C3C File Offset: 0x003E7E3C
	public static void CancelEventCallbackGameObject(GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		AkSoundEnginePINVOKE.CSharp_CancelEventCallbackGameObject(akGameObjectID);
	}

	// Token: 0x0600972F RID: 38703 RVA: 0x003E9C60 File Offset: 0x003E7E60
	public static void CancelEventCallback(uint in_playingID)
	{
		AkCallbackManager.RemoveEventCallback(in_playingID);
	}

	// Token: 0x06009730 RID: 38704 RVA: 0x003E9C68 File Offset: 0x003E7E68
	public static AKRESULT GetSourcePlayPosition(uint in_PlayingID, out int out_puPosition, bool in_bExtrapolate)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetSourcePlayPosition__SWIG_0(in_PlayingID, out out_puPosition, in_bExtrapolate);
	}

	// Token: 0x06009731 RID: 38705 RVA: 0x003E9C74 File Offset: 0x003E7E74
	public static AKRESULT GetSourcePlayPosition(uint in_PlayingID, out int out_puPosition)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetSourcePlayPosition__SWIG_1(in_PlayingID, out out_puPosition);
	}

	// Token: 0x06009732 RID: 38706 RVA: 0x003E9C80 File Offset: 0x003E7E80
	public static AKRESULT GetSourceStreamBuffering(uint in_PlayingID, out int out_buffering, out int out_bIsBuffering)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetSourceStreamBuffering(in_PlayingID, out out_buffering, out out_bIsBuffering);
	}

	// Token: 0x06009733 RID: 38707 RVA: 0x003E9C8C File Offset: 0x003E7E8C
	public static void StopAll(GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		AkSoundEnginePINVOKE.CSharp_StopAll__SWIG_0(akGameObjectID);
	}

	// Token: 0x06009734 RID: 38708 RVA: 0x003E9CB0 File Offset: 0x003E7EB0
	public static void StopAll()
	{
		AkSoundEnginePINVOKE.CSharp_StopAll__SWIG_1();
	}

	// Token: 0x06009735 RID: 38709 RVA: 0x003E9CB8 File Offset: 0x003E7EB8
	public static void StopPlayingID(uint in_playingID, int in_uTransitionDuration, AkCurveInterpolation in_eFadeCurve)
	{
		AkSoundEnginePINVOKE.CSharp_StopPlayingID__SWIG_0(in_playingID, in_uTransitionDuration, (int)in_eFadeCurve);
	}

	// Token: 0x06009736 RID: 38710 RVA: 0x003E9CC4 File Offset: 0x003E7EC4
	public static void StopPlayingID(uint in_playingID, int in_uTransitionDuration)
	{
		AkSoundEnginePINVOKE.CSharp_StopPlayingID__SWIG_1(in_playingID, in_uTransitionDuration);
	}

	// Token: 0x06009737 RID: 38711 RVA: 0x003E9CD0 File Offset: 0x003E7ED0
	public static void StopPlayingID(uint in_playingID)
	{
		AkSoundEnginePINVOKE.CSharp_StopPlayingID__SWIG_2(in_playingID);
	}

	// Token: 0x06009738 RID: 38712 RVA: 0x003E9CD8 File Offset: 0x003E7ED8
	public static void SetRandomSeed(uint in_uSeed)
	{
		AkSoundEnginePINVOKE.CSharp_SetRandomSeed(in_uSeed);
	}

	// Token: 0x06009739 RID: 38713 RVA: 0x003E9CE0 File Offset: 0x003E7EE0
	public static void MuteBackgroundMusic(bool in_bMute)
	{
		AkSoundEnginePINVOKE.CSharp_MuteBackgroundMusic(in_bMute);
	}

	// Token: 0x0600973A RID: 38714 RVA: 0x003E9CE8 File Offset: 0x003E7EE8
	public static bool GetBackgroundMusicMute()
	{
		return AkSoundEnginePINVOKE.CSharp_GetBackgroundMusicMute();
	}

	// Token: 0x0600973B RID: 38715 RVA: 0x003E9CF0 File Offset: 0x003E7EF0
	public static AKRESULT SendPluginCustomGameData(uint in_busID, GameObject in_busObjectID, AkPluginType in_eType, uint in_uCompanyID, uint in_uPluginID, IntPtr in_pData, uint in_uSizeInBytes)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_busObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_busObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SendPluginCustomGameData(in_busID, akGameObjectID, (int)in_eType, in_uCompanyID, in_uPluginID, in_pData, in_uSizeInBytes);
	}

	// Token: 0x0600973C RID: 38716 RVA: 0x003E9D1C File Offset: 0x003E7F1C
	public static AKRESULT UnregisterAllGameObj()
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnregisterAllGameObj();
	}

	// Token: 0x0600973D RID: 38717 RVA: 0x003E9D24 File Offset: 0x003E7F24
	public static AKRESULT SetMultiplePositions(GameObject in_GameObjectID, AkPositionArray in_pPositions, ushort in_NumPositions, AkMultiPositionType in_eMultiPositionType)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetMultiplePositions__SWIG_0(AkSoundEngine.GetAkGameObjectID(in_GameObjectID), in_pPositions.m_Buffer, in_NumPositions, (int)in_eMultiPositionType);
	}

	// Token: 0x0600973E RID: 38718 RVA: 0x003E9D3C File Offset: 0x003E7F3C
	public static AKRESULT SetMultiplePositions(GameObject in_GameObjectID, AkPositionArray in_pPositions, ushort in_NumPositions)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetMultiplePositions__SWIG_1(AkSoundEngine.GetAkGameObjectID(in_GameObjectID), in_pPositions.m_Buffer, in_NumPositions);
	}

	// Token: 0x0600973F RID: 38719 RVA: 0x003E9D50 File Offset: 0x003E7F50
	public static AKRESULT SetMultiplePositions(GameObject in_GameObjectID, AkChannelEmitterArray in_pPositions, ushort in_NumPositions, AkMultiPositionType in_eMultiPositionType)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetMultiplePositions__SWIG_2(AkSoundEngine.GetAkGameObjectID(in_GameObjectID), in_pPositions.m_Buffer, in_NumPositions, (int)in_eMultiPositionType);
	}

	// Token: 0x06009740 RID: 38720 RVA: 0x003E9D68 File Offset: 0x003E7F68
	public static AKRESULT SetMultiplePositions(GameObject in_GameObjectID, AkChannelEmitterArray in_pPositions, ushort in_NumPositions)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetMultiplePositions__SWIG_3(AkSoundEngine.GetAkGameObjectID(in_GameObjectID), in_pPositions.m_Buffer, in_NumPositions);
	}

	// Token: 0x06009741 RID: 38721 RVA: 0x003E9D7C File Offset: 0x003E7F7C
	public static AKRESULT SetScalingFactor(GameObject in_GameObjectID, float in_fAttenuationScalingFactor)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetScalingFactor(AkSoundEngine.GetAkGameObjectID(in_GameObjectID), in_fAttenuationScalingFactor);
	}

	// Token: 0x06009742 RID: 38722 RVA: 0x003E9D8C File Offset: 0x003E7F8C
	public static AKRESULT ClearBanks()
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ClearBanks();
	}

	// Token: 0x06009743 RID: 38723 RVA: 0x003E9D94 File Offset: 0x003E7F94
	public static AKRESULT SetBankLoadIOSettings(float in_fThroughput, char in_priority)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetBankLoadIOSettings(in_fThroughput, in_priority);
	}

	// Token: 0x06009744 RID: 38724 RVA: 0x003E9DA0 File Offset: 0x003E7FA0
	public static AKRESULT LoadBank(string in_pszString, int in_memPoolId, out uint out_bankID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_LoadBank__SWIG_0(in_pszString, in_memPoolId, out out_bankID);
	}

	// Token: 0x06009745 RID: 38725 RVA: 0x003E9DAC File Offset: 0x003E7FAC
	public static AKRESULT LoadBank(uint in_bankID, int in_memPoolId)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_LoadBank__SWIG_1(in_bankID, in_memPoolId);
	}

	// Token: 0x06009746 RID: 38726 RVA: 0x003E9DB8 File Offset: 0x003E7FB8
	public static AKRESULT LoadBank(IntPtr in_pInMemoryBankPtr, uint in_uInMemoryBankSize, out uint out_bankID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_LoadBank__SWIG_2(in_pInMemoryBankPtr, in_uInMemoryBankSize, out out_bankID);
	}

	// Token: 0x06009747 RID: 38727 RVA: 0x003E9DC4 File Offset: 0x003E7FC4
	public static AKRESULT LoadBank(IntPtr in_pInMemoryBankPtr, uint in_uInMemoryBankSize, int in_uPoolForBankMedia, out uint out_bankID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_LoadBank__SWIG_3(in_pInMemoryBankPtr, in_uInMemoryBankSize, in_uPoolForBankMedia, out out_bankID);
	}

	// Token: 0x06009748 RID: 38728 RVA: 0x003E9DD0 File Offset: 0x003E7FD0
	public static AKRESULT LoadBank(string in_pszString, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie, int in_memPoolId, out uint out_bankID)
	{
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_LoadBank__SWIG_4(in_pszString, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), in_memPoolId, out out_bankID);
	}

	// Token: 0x06009749 RID: 38729 RVA: 0x003E9E04 File Offset: 0x003E8004
	public static AKRESULT LoadBank(uint in_bankID, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie, int in_memPoolId)
	{
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_LoadBank__SWIG_5(in_bankID, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), in_memPoolId);
	}

	// Token: 0x0600974A RID: 38730 RVA: 0x003E9E38 File Offset: 0x003E8038
	public static AKRESULT LoadBank(IntPtr in_pInMemoryBankPtr, uint in_uInMemoryBankSize, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie, out uint out_bankID)
	{
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_LoadBank__SWIG_6(in_pInMemoryBankPtr, in_uInMemoryBankSize, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), out out_bankID);
	}

	// Token: 0x0600974B RID: 38731 RVA: 0x003E9E6C File Offset: 0x003E806C
	public static AKRESULT LoadBank(IntPtr in_pInMemoryBankPtr, uint in_uInMemoryBankSize, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie, int in_uPoolForBankMedia, out uint out_bankID)
	{
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_LoadBank__SWIG_7(in_pInMemoryBankPtr, in_uInMemoryBankSize, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), in_uPoolForBankMedia, out out_bankID);
	}

	// Token: 0x0600974C RID: 38732 RVA: 0x003E9EA4 File Offset: 0x003E80A4
	public static AKRESULT UnloadBank(string in_pszString, IntPtr in_pInMemoryBankPtr, out int out_pMemPoolId)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnloadBank__SWIG_0(in_pszString, in_pInMemoryBankPtr, out out_pMemPoolId);
	}

	// Token: 0x0600974D RID: 38733 RVA: 0x003E9EB0 File Offset: 0x003E80B0
	public static AKRESULT UnloadBank(string in_pszString, IntPtr in_pInMemoryBankPtr)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnloadBank__SWIG_1(in_pszString, in_pInMemoryBankPtr);
	}

	// Token: 0x0600974E RID: 38734 RVA: 0x003E9EBC File Offset: 0x003E80BC
	public static AKRESULT UnloadBank(uint in_bankID, IntPtr in_pInMemoryBankPtr, out int out_pMemPoolId)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnloadBank__SWIG_4(in_bankID, in_pInMemoryBankPtr, out out_pMemPoolId);
	}

	// Token: 0x0600974F RID: 38735 RVA: 0x003E9EC8 File Offset: 0x003E80C8
	public static AKRESULT UnloadBank(uint in_bankID, IntPtr in_pInMemoryBankPtr)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnloadBank__SWIG_5(in_bankID, in_pInMemoryBankPtr);
	}

	// Token: 0x06009750 RID: 38736 RVA: 0x003E9ED4 File Offset: 0x003E80D4
	public static AKRESULT UnloadBank(string in_pszString, IntPtr in_pInMemoryBankPtr, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie)
	{
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnloadBank__SWIG_6(in_pszString, in_pInMemoryBankPtr, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
	}

	// Token: 0x06009751 RID: 38737 RVA: 0x003E9F08 File Offset: 0x003E8108
	public static AKRESULT UnloadBank(uint in_bankID, IntPtr in_pInMemoryBankPtr, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie)
	{
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnloadBank__SWIG_8(in_bankID, in_pInMemoryBankPtr, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
	}

	// Token: 0x06009752 RID: 38738 RVA: 0x003E9F3C File Offset: 0x003E813C
	public static void CancelBankCallbackCookie(object in_pCookie)
	{
		AkCallbackManager.RemoveBankCallback(in_pCookie);
	}

	// Token: 0x06009753 RID: 38739 RVA: 0x003E9F44 File Offset: 0x003E8144
	public static AKRESULT PrepareBank(AkPreparationType in_PreparationType, string in_pszString, AkBankContent in_uFlags)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareBank__SWIG_0((int)in_PreparationType, in_pszString, (int)in_uFlags);
	}

	// Token: 0x06009754 RID: 38740 RVA: 0x003E9F50 File Offset: 0x003E8150
	public static AKRESULT PrepareBank(AkPreparationType in_PreparationType, string in_pszString)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareBank__SWIG_1((int)in_PreparationType, in_pszString);
	}

	// Token: 0x06009755 RID: 38741 RVA: 0x003E9F5C File Offset: 0x003E815C
	public static AKRESULT PrepareBank(AkPreparationType in_PreparationType, uint in_bankID, AkBankContent in_uFlags)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareBank__SWIG_4((int)in_PreparationType, in_bankID, (int)in_uFlags);
	}

	// Token: 0x06009756 RID: 38742 RVA: 0x003E9F68 File Offset: 0x003E8168
	public static AKRESULT PrepareBank(AkPreparationType in_PreparationType, uint in_bankID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareBank__SWIG_5((int)in_PreparationType, in_bankID);
	}

	// Token: 0x06009757 RID: 38743 RVA: 0x003E9F74 File Offset: 0x003E8174
	public static AKRESULT PrepareBank(AkPreparationType in_PreparationType, string in_pszString, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie, AkBankContent in_uFlags)
	{
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareBank__SWIG_6((int)in_PreparationType, in_pszString, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), (int)in_uFlags);
	}

	// Token: 0x06009758 RID: 38744 RVA: 0x003E9FA8 File Offset: 0x003E81A8
	public static AKRESULT PrepareBank(AkPreparationType in_PreparationType, string in_pszString, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie)
	{
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareBank__SWIG_7((int)in_PreparationType, in_pszString, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
	}

	// Token: 0x06009759 RID: 38745 RVA: 0x003E9FDC File Offset: 0x003E81DC
	public static AKRESULT PrepareBank(AkPreparationType in_PreparationType, uint in_bankID, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie, AkBankContent in_uFlags)
	{
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareBank__SWIG_10((int)in_PreparationType, in_bankID, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()), (int)in_uFlags);
	}

	// Token: 0x0600975A RID: 38746 RVA: 0x003EA010 File Offset: 0x003E8210
	public static AKRESULT PrepareBank(AkPreparationType in_PreparationType, uint in_bankID, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie)
	{
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareBank__SWIG_11((int)in_PreparationType, in_bankID, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
	}

	// Token: 0x0600975B RID: 38747 RVA: 0x003EA044 File Offset: 0x003E8244
	public static AKRESULT ClearPreparedEvents()
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ClearPreparedEvents();
	}

	// Token: 0x0600975C RID: 38748 RVA: 0x003EA04C File Offset: 0x003E824C
	public static AKRESULT PrepareEvent(AkPreparationType in_PreparationType, string[] in_ppszString, uint in_uNumEvent)
	{
		int num = 0;
		foreach (string text in in_ppszString)
		{
			num += text.Length + 1;
		}
		int num2 = 2;
		IntPtr intPtr = Marshal.AllocHGlobal(num * num2);
		Marshal.WriteInt16(intPtr, (short)in_ppszString.Length);
		IntPtr intPtr2 = (IntPtr)(intPtr.ToInt64() + (long)num2);
		foreach (string text2 in in_ppszString)
		{
			Marshal.Copy(text2.ToCharArray(), 0, intPtr2, text2.Length);
			intPtr2 = (IntPtr)(intPtr2.ToInt64() + (long)(num2 * text2.Length));
			Marshal.WriteInt16(intPtr2, 0);
			intPtr2 = (IntPtr)(intPtr2.ToInt64() + (long)num2);
		}
		AKRESULT akresult;
		try
		{
			akresult = (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareEvent__SWIG_0((int)in_PreparationType, intPtr, in_uNumEvent);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
		return akresult;
	}

	// Token: 0x0600975D RID: 38749 RVA: 0x003EA144 File Offset: 0x003E8344
	public static AKRESULT PrepareEvent(AkPreparationType in_PreparationType, uint[] in_pEventID, uint in_uNumEvent)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareEvent__SWIG_1((int)in_PreparationType, in_pEventID, in_uNumEvent);
	}

	// Token: 0x0600975E RID: 38750 RVA: 0x003EA150 File Offset: 0x003E8350
	public static AKRESULT PrepareEvent(AkPreparationType in_PreparationType, string[] in_ppszString, uint in_uNumEvent, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie)
	{
		int num = 0;
		foreach (string text in in_ppszString)
		{
			num += text.Length + 1;
		}
		int num2 = 2;
		IntPtr intPtr = Marshal.AllocHGlobal(num * num2);
		Marshal.WriteInt16(intPtr, (short)in_ppszString.Length);
		IntPtr intPtr2 = (IntPtr)(intPtr.ToInt64() + (long)num2);
		foreach (string text2 in in_ppszString)
		{
			Marshal.Copy(text2.ToCharArray(), 0, intPtr2, text2.Length);
			intPtr2 = (IntPtr)(intPtr2.ToInt64() + (long)(num2 * text2.Length));
			Marshal.WriteInt16(intPtr2, 0);
			intPtr2 = (IntPtr)(intPtr2.ToInt64() + (long)num2);
		}
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		AKRESULT akresult;
		try
		{
			akresult = (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareEvent__SWIG_2((int)in_PreparationType, intPtr, in_uNumEvent, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
		return akresult;
	}

	// Token: 0x0600975F RID: 38751 RVA: 0x003EA274 File Offset: 0x003E8474
	public static AKRESULT PrepareEvent(AkPreparationType in_PreparationType, uint[] in_pEventID, uint in_uNumEvent, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie)
	{
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareEvent__SWIG_3((int)in_PreparationType, in_pEventID, in_uNumEvent, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
	}

	// Token: 0x06009760 RID: 38752 RVA: 0x003EA2AC File Offset: 0x003E84AC
	public static AKRESULT SetMedia(AkSourceSettings in_pSourceSettings, uint in_uNumSourceSettings)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetMedia(AkSourceSettings.getCPtr(in_pSourceSettings), in_uNumSourceSettings);
	}

	// Token: 0x06009761 RID: 38753 RVA: 0x003EA2BC File Offset: 0x003E84BC
	public static AKRESULT UnsetMedia(AkSourceSettings in_pSourceSettings, uint in_uNumSourceSettings)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnsetMedia(AkSourceSettings.getCPtr(in_pSourceSettings), in_uNumSourceSettings);
	}

	// Token: 0x06009762 RID: 38754 RVA: 0x003EA2CC File Offset: 0x003E84CC
	public static AKRESULT PrepareGameSyncs(AkPreparationType in_PreparationType, AkGroupType in_eGameSyncType, string in_pszGroupName, string[] in_ppszGameSyncName, uint in_uNumGameSyncs)
	{
		int num = 0;
		foreach (string text in in_ppszGameSyncName)
		{
			num += text.Length + 1;
		}
		int num2 = 2;
		IntPtr intPtr = Marshal.AllocHGlobal(num * num2);
		Marshal.WriteInt16(intPtr, (short)in_ppszGameSyncName.Length);
		IntPtr intPtr2 = (IntPtr)(intPtr.ToInt64() + (long)num2);
		foreach (string text2 in in_ppszGameSyncName)
		{
			Marshal.Copy(text2.ToCharArray(), 0, intPtr2, text2.Length);
			intPtr2 = (IntPtr)(intPtr2.ToInt64() + (long)(num2 * text2.Length));
			Marshal.WriteInt16(intPtr2, 0);
			intPtr2 = (IntPtr)(intPtr2.ToInt64() + (long)num2);
		}
		AKRESULT akresult;
		try
		{
			akresult = (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareGameSyncs__SWIG_0((int)in_PreparationType, (int)in_eGameSyncType, in_pszGroupName, intPtr, in_uNumGameSyncs);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
		return akresult;
	}

	// Token: 0x06009763 RID: 38755 RVA: 0x003EA3C8 File Offset: 0x003E85C8
	public static AKRESULT PrepareGameSyncs(AkPreparationType in_PreparationType, AkGroupType in_eGameSyncType, uint in_GroupID, uint[] in_paGameSyncID, uint in_uNumGameSyncs)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareGameSyncs__SWIG_1((int)in_PreparationType, (int)in_eGameSyncType, in_GroupID, in_paGameSyncID, in_uNumGameSyncs);
	}

	// Token: 0x06009764 RID: 38756 RVA: 0x003EA3D8 File Offset: 0x003E85D8
	public static AKRESULT PrepareGameSyncs(AkPreparationType in_PreparationType, AkGroupType in_eGameSyncType, string in_pszGroupName, string[] in_ppszGameSyncName, uint in_uNumGameSyncs, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie)
	{
		int num = 0;
		foreach (string text in in_ppszGameSyncName)
		{
			num += text.Length + 1;
		}
		int num2 = 2;
		IntPtr intPtr = Marshal.AllocHGlobal(num * num2);
		Marshal.WriteInt16(intPtr, (short)in_ppszGameSyncName.Length);
		IntPtr intPtr2 = (IntPtr)(intPtr.ToInt64() + (long)num2);
		foreach (string text2 in in_ppszGameSyncName)
		{
			Marshal.Copy(text2.ToCharArray(), 0, intPtr2, text2.Length);
			intPtr2 = (IntPtr)(intPtr2.ToInt64() + (long)(num2 * text2.Length));
			Marshal.WriteInt16(intPtr2, 0);
			intPtr2 = (IntPtr)(intPtr2.ToInt64() + (long)num2);
		}
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		AKRESULT akresult;
		try
		{
			akresult = (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareGameSyncs__SWIG_2((int)in_PreparationType, (int)in_eGameSyncType, in_pszGroupName, intPtr, in_uNumGameSyncs, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
		return akresult;
	}

	// Token: 0x06009765 RID: 38757 RVA: 0x003EA500 File Offset: 0x003E8700
	public static AKRESULT PrepareGameSyncs(AkPreparationType in_PreparationType, AkGroupType in_eGameSyncType, uint in_GroupID, uint[] in_paGameSyncID, uint in_uNumGameSyncs, AkCallbackManager.BankCallback in_pfnBankCallback, object in_pCookie)
	{
		in_pCookie = new AkCallbackManager.BankCallbackPackage(in_pfnBankCallback, in_pCookie);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PrepareGameSyncs__SWIG_3((int)in_PreparationType, (int)in_eGameSyncType, in_GroupID, in_paGameSyncID, in_uNumGameSyncs, IntPtr.Zero, (in_pCookie == null) ? IntPtr.Zero : ((IntPtr)in_pCookie.GetHashCode()));
	}

	// Token: 0x06009766 RID: 38758 RVA: 0x003EA53C File Offset: 0x003E873C
	public static AKRESULT AddListener(GameObject in_emitterGameObj, GameObject in_listenerGameObj)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_emitterGameObj);
		AkSoundEngine.PreGameObjectAPICall(in_emitterGameObj, akGameObjectID);
		ulong akGameObjectID2 = AkSoundEngine.GetAkGameObjectID(in_listenerGameObj);
		AkSoundEngine.PreGameObjectAPICall(in_listenerGameObj, akGameObjectID2);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AddListener(akGameObjectID, akGameObjectID2);
	}

	// Token: 0x06009767 RID: 38759 RVA: 0x003EA56C File Offset: 0x003E876C
	public static AKRESULT RemoveListener(GameObject in_emitterGameObj, GameObject in_listenerGameObj)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_emitterGameObj);
		AkSoundEngine.PreGameObjectAPICall(in_emitterGameObj, akGameObjectID);
		ulong akGameObjectID2 = AkSoundEngine.GetAkGameObjectID(in_listenerGameObj);
		AkSoundEngine.PreGameObjectAPICall(in_listenerGameObj, akGameObjectID2);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RemoveListener(akGameObjectID, akGameObjectID2);
	}

	// Token: 0x06009768 RID: 38760 RVA: 0x003EA59C File Offset: 0x003E879C
	public static AKRESULT AddDefaultListener(GameObject in_listenerGameObj)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_listenerGameObj);
		AkSoundEngine.PreGameObjectAPICall(in_listenerGameObj, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AddDefaultListener(akGameObjectID);
	}

	// Token: 0x06009769 RID: 38761 RVA: 0x003EA5C0 File Offset: 0x003E87C0
	public static AKRESULT RemoveDefaultListener(GameObject in_listenerGameObj)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_listenerGameObj);
		AkSoundEngine.PreGameObjectAPICall(in_listenerGameObj, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RemoveDefaultListener(akGameObjectID);
	}

	// Token: 0x0600976A RID: 38762 RVA: 0x003EA5E4 File Offset: 0x003E87E4
	public static AKRESULT ResetListenersToDefault(GameObject in_emitterGameObj)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_emitterGameObj);
		AkSoundEngine.PreGameObjectAPICall(in_emitterGameObj, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ResetListenersToDefault(akGameObjectID);
	}

	// Token: 0x0600976B RID: 38763 RVA: 0x003EA608 File Offset: 0x003E8808
	public static AKRESULT SetListenerSpatialization(GameObject in_uListenerID, bool in_bSpatialized, AkChannelConfig in_channelConfig, float[] in_pVolumeOffsets)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_uListenerID);
		AkSoundEngine.PreGameObjectAPICall(in_uListenerID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetListenerSpatialization__SWIG_0(akGameObjectID, in_bSpatialized, AkChannelConfig.getCPtr(in_channelConfig), in_pVolumeOffsets);
	}

	// Token: 0x0600976C RID: 38764 RVA: 0x003EA634 File Offset: 0x003E8834
	public static AKRESULT SetListenerSpatialization(GameObject in_uListenerID, bool in_bSpatialized, AkChannelConfig in_channelConfig)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_uListenerID);
		AkSoundEngine.PreGameObjectAPICall(in_uListenerID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetListenerSpatialization__SWIG_1(akGameObjectID, in_bSpatialized, AkChannelConfig.getCPtr(in_channelConfig));
	}

	// Token: 0x0600976D RID: 38765 RVA: 0x003EA65C File Offset: 0x003E885C
	public static AKRESULT SetRTPCValue(uint in_rtpcID, float in_value, GameObject in_gameObjectID, int in_uValueChangeDuration, AkCurveInterpolation in_eFadeCurve, bool in_bBypassInternalValueInterpolation)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValue__SWIG_0(in_rtpcID, in_value, akGameObjectID, in_uValueChangeDuration, (int)in_eFadeCurve, in_bBypassInternalValueInterpolation);
	}

	// Token: 0x0600976E RID: 38766 RVA: 0x003EA684 File Offset: 0x003E8884
	public static AKRESULT SetRTPCValue(uint in_rtpcID, float in_value, GameObject in_gameObjectID, int in_uValueChangeDuration, AkCurveInterpolation in_eFadeCurve)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValue__SWIG_1(in_rtpcID, in_value, akGameObjectID, in_uValueChangeDuration, (int)in_eFadeCurve);
	}

	// Token: 0x0600976F RID: 38767 RVA: 0x003EA6AC File Offset: 0x003E88AC
	public static AKRESULT SetRTPCValue(uint in_rtpcID, float in_value, GameObject in_gameObjectID, int in_uValueChangeDuration)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValue__SWIG_2(in_rtpcID, in_value, akGameObjectID, in_uValueChangeDuration);
	}

	// Token: 0x06009770 RID: 38768 RVA: 0x003EA6D0 File Offset: 0x003E88D0
	public static AKRESULT SetRTPCValue(uint in_rtpcID, float in_value, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValue__SWIG_3(in_rtpcID, in_value, akGameObjectID);
	}

	// Token: 0x06009771 RID: 38769 RVA: 0x003EA6F4 File Offset: 0x003E88F4
	public static AKRESULT SetRTPCValue(uint in_rtpcID, float in_value)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValue__SWIG_4(in_rtpcID, in_value);
	}

	// Token: 0x06009772 RID: 38770 RVA: 0x003EA700 File Offset: 0x003E8900
	public static AKRESULT SetRTPCValue(string in_pszRtpcName, float in_value, GameObject in_gameObjectID, int in_uValueChangeDuration, AkCurveInterpolation in_eFadeCurve, bool in_bBypassInternalValueInterpolation)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValue__SWIG_5(in_pszRtpcName, in_value, akGameObjectID, in_uValueChangeDuration, (int)in_eFadeCurve, in_bBypassInternalValueInterpolation);
	}

	// Token: 0x06009773 RID: 38771 RVA: 0x003EA728 File Offset: 0x003E8928
	public static AKRESULT SetRTPCValue(string in_pszRtpcName, float in_value, GameObject in_gameObjectID, int in_uValueChangeDuration, AkCurveInterpolation in_eFadeCurve)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValue__SWIG_6(in_pszRtpcName, in_value, akGameObjectID, in_uValueChangeDuration, (int)in_eFadeCurve);
	}

	// Token: 0x06009774 RID: 38772 RVA: 0x003EA750 File Offset: 0x003E8950
	public static AKRESULT SetRTPCValue(string in_pszRtpcName, float in_value, GameObject in_gameObjectID, int in_uValueChangeDuration)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValue__SWIG_7(in_pszRtpcName, in_value, akGameObjectID, in_uValueChangeDuration);
	}

	// Token: 0x06009775 RID: 38773 RVA: 0x003EA774 File Offset: 0x003E8974
	public static AKRESULT SetRTPCValue(string in_pszRtpcName, float in_value, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValue__SWIG_8(in_pszRtpcName, in_value, akGameObjectID);
	}

	// Token: 0x06009776 RID: 38774 RVA: 0x003EA798 File Offset: 0x003E8998
	public static AKRESULT SetRTPCValue(string in_pszRtpcName, float in_value)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValue__SWIG_9(in_pszRtpcName, in_value);
	}

	// Token: 0x06009777 RID: 38775 RVA: 0x003EA7A4 File Offset: 0x003E89A4
	public static AKRESULT SetRTPCValueByPlayingID(uint in_rtpcID, float in_value, uint in_playingID, int in_uValueChangeDuration, AkCurveInterpolation in_eFadeCurve, bool in_bBypassInternalValueInterpolation)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValueByPlayingID__SWIG_0(in_rtpcID, in_value, in_playingID, in_uValueChangeDuration, (int)in_eFadeCurve, in_bBypassInternalValueInterpolation);
	}

	// Token: 0x06009778 RID: 38776 RVA: 0x003EA7B4 File Offset: 0x003E89B4
	public static AKRESULT SetRTPCValueByPlayingID(uint in_rtpcID, float in_value, uint in_playingID, int in_uValueChangeDuration, AkCurveInterpolation in_eFadeCurve)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValueByPlayingID__SWIG_1(in_rtpcID, in_value, in_playingID, in_uValueChangeDuration, (int)in_eFadeCurve);
	}

	// Token: 0x06009779 RID: 38777 RVA: 0x003EA7C4 File Offset: 0x003E89C4
	public static AKRESULT SetRTPCValueByPlayingID(uint in_rtpcID, float in_value, uint in_playingID, int in_uValueChangeDuration)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValueByPlayingID__SWIG_2(in_rtpcID, in_value, in_playingID, in_uValueChangeDuration);
	}

	// Token: 0x0600977A RID: 38778 RVA: 0x003EA7D0 File Offset: 0x003E89D0
	public static AKRESULT SetRTPCValueByPlayingID(uint in_rtpcID, float in_value, uint in_playingID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValueByPlayingID__SWIG_3(in_rtpcID, in_value, in_playingID);
	}

	// Token: 0x0600977B RID: 38779 RVA: 0x003EA7DC File Offset: 0x003E89DC
	public static AKRESULT SetRTPCValueByPlayingID(string in_pszRtpcName, float in_value, uint in_playingID, int in_uValueChangeDuration, AkCurveInterpolation in_eFadeCurve, bool in_bBypassInternalValueInterpolation)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValueByPlayingID__SWIG_4(in_pszRtpcName, in_value, in_playingID, in_uValueChangeDuration, (int)in_eFadeCurve, in_bBypassInternalValueInterpolation);
	}

	// Token: 0x0600977C RID: 38780 RVA: 0x003EA7EC File Offset: 0x003E89EC
	public static AKRESULT SetRTPCValueByPlayingID(string in_pszRtpcName, float in_value, uint in_playingID, int in_uValueChangeDuration, AkCurveInterpolation in_eFadeCurve)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValueByPlayingID__SWIG_5(in_pszRtpcName, in_value, in_playingID, in_uValueChangeDuration, (int)in_eFadeCurve);
	}

	// Token: 0x0600977D RID: 38781 RVA: 0x003EA7FC File Offset: 0x003E89FC
	public static AKRESULT SetRTPCValueByPlayingID(string in_pszRtpcName, float in_value, uint in_playingID, int in_uValueChangeDuration)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValueByPlayingID__SWIG_6(in_pszRtpcName, in_value, in_playingID, in_uValueChangeDuration);
	}

	// Token: 0x0600977E RID: 38782 RVA: 0x003EA808 File Offset: 0x003E8A08
	public static AKRESULT SetRTPCValueByPlayingID(string in_pszRtpcName, float in_value, uint in_playingID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRTPCValueByPlayingID__SWIG_7(in_pszRtpcName, in_value, in_playingID);
	}

	// Token: 0x0600977F RID: 38783 RVA: 0x003EA814 File Offset: 0x003E8A14
	public static AKRESULT ResetRTPCValue(uint in_rtpcID, GameObject in_gameObjectID, int in_uValueChangeDuration, AkCurveInterpolation in_eFadeCurve, bool in_bBypassInternalValueInterpolation)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ResetRTPCValue__SWIG_0(in_rtpcID, akGameObjectID, in_uValueChangeDuration, (int)in_eFadeCurve, in_bBypassInternalValueInterpolation);
	}

	// Token: 0x06009780 RID: 38784 RVA: 0x003EA83C File Offset: 0x003E8A3C
	public static AKRESULT ResetRTPCValue(uint in_rtpcID, GameObject in_gameObjectID, int in_uValueChangeDuration, AkCurveInterpolation in_eFadeCurve)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ResetRTPCValue__SWIG_1(in_rtpcID, akGameObjectID, in_uValueChangeDuration, (int)in_eFadeCurve);
	}

	// Token: 0x06009781 RID: 38785 RVA: 0x003EA860 File Offset: 0x003E8A60
	public static AKRESULT ResetRTPCValue(uint in_rtpcID, GameObject in_gameObjectID, int in_uValueChangeDuration)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ResetRTPCValue__SWIG_2(in_rtpcID, akGameObjectID, in_uValueChangeDuration);
	}

	// Token: 0x06009782 RID: 38786 RVA: 0x003EA884 File Offset: 0x003E8A84
	public static AKRESULT ResetRTPCValue(uint in_rtpcID, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ResetRTPCValue__SWIG_3(in_rtpcID, akGameObjectID);
	}

	// Token: 0x06009783 RID: 38787 RVA: 0x003EA8A8 File Offset: 0x003E8AA8
	public static AKRESULT ResetRTPCValue(uint in_rtpcID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ResetRTPCValue__SWIG_4(in_rtpcID);
	}

	// Token: 0x06009784 RID: 38788 RVA: 0x003EA8B0 File Offset: 0x003E8AB0
	public static AKRESULT ResetRTPCValue(string in_pszRtpcName, GameObject in_gameObjectID, int in_uValueChangeDuration, AkCurveInterpolation in_eFadeCurve, bool in_bBypassInternalValueInterpolation)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ResetRTPCValue__SWIG_5(in_pszRtpcName, akGameObjectID, in_uValueChangeDuration, (int)in_eFadeCurve, in_bBypassInternalValueInterpolation);
	}

	// Token: 0x06009785 RID: 38789 RVA: 0x003EA8D8 File Offset: 0x003E8AD8
	public static AKRESULT ResetRTPCValue(string in_pszRtpcName, GameObject in_gameObjectID, int in_uValueChangeDuration, AkCurveInterpolation in_eFadeCurve)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ResetRTPCValue__SWIG_6(in_pszRtpcName, akGameObjectID, in_uValueChangeDuration, (int)in_eFadeCurve);
	}

	// Token: 0x06009786 RID: 38790 RVA: 0x003EA8FC File Offset: 0x003E8AFC
	public static AKRESULT ResetRTPCValue(string in_pszRtpcName, GameObject in_gameObjectID, int in_uValueChangeDuration)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ResetRTPCValue__SWIG_7(in_pszRtpcName, akGameObjectID, in_uValueChangeDuration);
	}

	// Token: 0x06009787 RID: 38791 RVA: 0x003EA920 File Offset: 0x003E8B20
	public static AKRESULT ResetRTPCValue(string in_pszRtpcName, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ResetRTPCValue__SWIG_8(in_pszRtpcName, akGameObjectID);
	}

	// Token: 0x06009788 RID: 38792 RVA: 0x003EA944 File Offset: 0x003E8B44
	public static AKRESULT ResetRTPCValue(string in_pszRtpcName)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_ResetRTPCValue__SWIG_9(in_pszRtpcName);
	}

	// Token: 0x06009789 RID: 38793 RVA: 0x003EA94C File Offset: 0x003E8B4C
	public static AKRESULT SetSwitch(uint in_switchGroup, uint in_switchState, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetSwitch__SWIG_0(in_switchGroup, in_switchState, akGameObjectID);
	}

	// Token: 0x0600978A RID: 38794 RVA: 0x003EA970 File Offset: 0x003E8B70
	public static AKRESULT SetSwitch(string in_pszSwitchGroup, string in_pszSwitchState, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetSwitch__SWIG_1(in_pszSwitchGroup, in_pszSwitchState, akGameObjectID);
	}

	// Token: 0x0600978B RID: 38795 RVA: 0x003EA994 File Offset: 0x003E8B94
	public static AKRESULT PostTrigger(uint in_triggerID, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostTrigger__SWIG_0(in_triggerID, akGameObjectID);
	}

	// Token: 0x0600978C RID: 38796 RVA: 0x003EA9B8 File Offset: 0x003E8BB8
	public static AKRESULT PostTrigger(string in_pszTrigger, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostTrigger__SWIG_1(in_pszTrigger, akGameObjectID);
	}

	// Token: 0x0600978D RID: 38797 RVA: 0x003EA9DC File Offset: 0x003E8BDC
	public static AKRESULT SetState(uint in_stateGroup, uint in_state)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetState__SWIG_0(in_stateGroup, in_state);
	}

	// Token: 0x0600978E RID: 38798 RVA: 0x003EA9E8 File Offset: 0x003E8BE8
	public static AKRESULT SetState(string in_pszStateGroup, string in_pszState)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetState__SWIG_1(in_pszStateGroup, in_pszState);
	}

	// Token: 0x0600978F RID: 38799 RVA: 0x003EA9F4 File Offset: 0x003E8BF4
	public static AKRESULT SetGameObjectAuxSendValues(GameObject in_gameObjectID, AkAuxSendArray in_aAuxSendValues, uint in_uNumSendValues)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetGameObjectAuxSendValues(akGameObjectID, in_aAuxSendValues.GetBuffer(), in_uNumSendValues);
	}

	// Token: 0x06009790 RID: 38800 RVA: 0x003EAA1C File Offset: 0x003E8C1C
	public static AKRESULT SetGameObjectOutputBusVolume(GameObject in_emitterObjID, GameObject in_listenerObjID, float in_fControlValue)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_emitterObjID);
		AkSoundEngine.PreGameObjectAPICall(in_emitterObjID, akGameObjectID);
		ulong akGameObjectID2 = AkSoundEngine.GetAkGameObjectID(in_listenerObjID);
		AkSoundEngine.PreGameObjectAPICall(in_listenerObjID, akGameObjectID2);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetGameObjectOutputBusVolume(akGameObjectID, akGameObjectID2, in_fControlValue);
	}

	// Token: 0x06009791 RID: 38801 RVA: 0x003EAA50 File Offset: 0x003E8C50
	public static AKRESULT SetActorMixerEffect(uint in_audioNodeID, uint in_uFXIndex, uint in_shareSetID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetActorMixerEffect(in_audioNodeID, in_uFXIndex, in_shareSetID);
	}

	// Token: 0x06009792 RID: 38802 RVA: 0x003EAA5C File Offset: 0x003E8C5C
	public static AKRESULT SetBusEffect(uint in_audioNodeID, uint in_uFXIndex, uint in_shareSetID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetBusEffect__SWIG_0(in_audioNodeID, in_uFXIndex, in_shareSetID);
	}

	// Token: 0x06009793 RID: 38803 RVA: 0x003EAA68 File Offset: 0x003E8C68
	public static AKRESULT SetBusEffect(string in_pszBusName, uint in_uFXIndex, uint in_shareSetID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetBusEffect__SWIG_1(in_pszBusName, in_uFXIndex, in_shareSetID);
	}

	// Token: 0x06009794 RID: 38804 RVA: 0x003EAA74 File Offset: 0x003E8C74
	public static AKRESULT SetMixer(uint in_audioNodeID, uint in_shareSetID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetMixer__SWIG_0(in_audioNodeID, in_shareSetID);
	}

	// Token: 0x06009795 RID: 38805 RVA: 0x003EAA80 File Offset: 0x003E8C80
	public static AKRESULT SetMixer(string in_pszBusName, uint in_shareSetID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetMixer__SWIG_1(in_pszBusName, in_shareSetID);
	}

	// Token: 0x06009796 RID: 38806 RVA: 0x003EAA8C File Offset: 0x003E8C8C
	public static AKRESULT SetBusConfig(uint in_audioNodeID, AkChannelConfig in_channelConfig)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetBusConfig__SWIG_0(in_audioNodeID, AkChannelConfig.getCPtr(in_channelConfig));
	}

	// Token: 0x06009797 RID: 38807 RVA: 0x003EAA9C File Offset: 0x003E8C9C
	public static AKRESULT SetBusConfig(string in_pszBusName, AkChannelConfig in_channelConfig)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetBusConfig__SWIG_1(in_pszBusName, AkChannelConfig.getCPtr(in_channelConfig));
	}

	// Token: 0x06009798 RID: 38808 RVA: 0x003EAAAC File Offset: 0x003E8CAC
	public static AKRESULT SetObjectObstructionAndOcclusion(GameObject in_EmitterID, GameObject in_ListenerID, float in_fObstructionLevel, float in_fOcclusionLevel)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_EmitterID);
		AkSoundEngine.PreGameObjectAPICall(in_EmitterID, akGameObjectID);
		ulong akGameObjectID2 = AkSoundEngine.GetAkGameObjectID(in_ListenerID);
		AkSoundEngine.PreGameObjectAPICall(in_ListenerID, akGameObjectID2);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetObjectObstructionAndOcclusion(akGameObjectID, akGameObjectID2, in_fObstructionLevel, in_fOcclusionLevel);
	}

	// Token: 0x06009799 RID: 38809 RVA: 0x003EAAE0 File Offset: 0x003E8CE0
	public static AKRESULT SetMultipleObstructionAndOcclusion(GameObject in_EmitterID, GameObject in_uListenerID, AkObstructionOcclusionValues in_fObstructionOcclusionValues, uint in_uNumOcclusionObstruction)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_EmitterID);
		AkSoundEngine.PreGameObjectAPICall(in_EmitterID, akGameObjectID);
		ulong akGameObjectID2 = AkSoundEngine.GetAkGameObjectID(in_uListenerID);
		AkSoundEngine.PreGameObjectAPICall(in_uListenerID, akGameObjectID2);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetMultipleObstructionAndOcclusion(akGameObjectID, akGameObjectID2, AkObstructionOcclusionValues.getCPtr(in_fObstructionOcclusionValues), in_uNumOcclusionObstruction);
	}

	// Token: 0x0600979A RID: 38810 RVA: 0x003EAB18 File Offset: 0x003E8D18
	public static AKRESULT StartOutputCapture(string in_CaptureFileName)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_StartOutputCapture(in_CaptureFileName);
	}

	// Token: 0x0600979B RID: 38811 RVA: 0x003EAB20 File Offset: 0x003E8D20
	public static AKRESULT StopOutputCapture()
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_StopOutputCapture();
	}

	// Token: 0x0600979C RID: 38812 RVA: 0x003EAB28 File Offset: 0x003E8D28
	public static AKRESULT AddOutputCaptureMarker(string in_MarkerText)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AddOutputCaptureMarker(in_MarkerText);
	}

	// Token: 0x0600979D RID: 38813 RVA: 0x003EAB30 File Offset: 0x003E8D30
	public static AKRESULT StartProfilerCapture(string in_CaptureFileName)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_StartProfilerCapture(in_CaptureFileName);
	}

	// Token: 0x0600979E RID: 38814 RVA: 0x003EAB38 File Offset: 0x003E8D38
	public static AKRESULT StopProfilerCapture()
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_StopProfilerCapture();
	}

	// Token: 0x0600979F RID: 38815 RVA: 0x003EAB40 File Offset: 0x003E8D40
	public static AKRESULT RemoveOutput(ulong in_idOutput)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RemoveOutput(in_idOutput);
	}

	// Token: 0x060097A0 RID: 38816 RVA: 0x003EAB48 File Offset: 0x003E8D48
	public static ulong GetOutputID(uint in_idShareset, uint in_idDevice)
	{
		return AkSoundEnginePINVOKE.CSharp_GetOutputID__SWIG_0(in_idShareset, in_idDevice);
	}

	// Token: 0x060097A1 RID: 38817 RVA: 0x003EAB54 File Offset: 0x003E8D54
	public static ulong GetOutputID(string in_szShareSet, uint in_idDevice)
	{
		return AkSoundEnginePINVOKE.CSharp_GetOutputID__SWIG_1(in_szShareSet, in_idDevice);
	}

	// Token: 0x060097A2 RID: 38818 RVA: 0x003EAB60 File Offset: 0x003E8D60
	public static AKRESULT SetBusDevice(uint in_idBus, uint in_idNewDevice)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetBusDevice__SWIG_0(in_idBus, in_idNewDevice);
	}

	// Token: 0x060097A3 RID: 38819 RVA: 0x003EAB6C File Offset: 0x003E8D6C
	public static AKRESULT SetBusDevice(string in_BusName, string in_DeviceName)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetBusDevice__SWIG_1(in_BusName, in_DeviceName);
	}

	// Token: 0x060097A4 RID: 38820 RVA: 0x003EAB78 File Offset: 0x003E8D78
	public static AKRESULT SetOutputVolume(ulong in_idOutput, float in_fVolume)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetOutputVolume(in_idOutput, in_fVolume);
	}

	// Token: 0x060097A5 RID: 38821 RVA: 0x003EAB84 File Offset: 0x003E8D84
	public static AKRESULT Suspend(bool in_bRenderAnyway)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_Suspend__SWIG_0(in_bRenderAnyway);
	}

	// Token: 0x060097A6 RID: 38822 RVA: 0x003EAB8C File Offset: 0x003E8D8C
	public static AKRESULT Suspend()
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_Suspend__SWIG_1();
	}

	// Token: 0x060097A7 RID: 38823 RVA: 0x003EAB94 File Offset: 0x003E8D94
	public static AKRESULT WakeupFromSuspend()
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_WakeupFromSuspend();
	}

	// Token: 0x060097A8 RID: 38824 RVA: 0x003EAB9C File Offset: 0x003E8D9C
	public static uint GetBufferTick()
	{
		return AkSoundEnginePINVOKE.CSharp_GetBufferTick();
	}

	// Token: 0x170016E9 RID: 5865
	// (get) Token: 0x060097A9 RID: 38825 RVA: 0x003EABA4 File Offset: 0x003E8DA4
	public static byte AK_INVALID_MIDI_CHANNEL
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AK_INVALID_MIDI_CHANNEL_get();
		}
	}

	// Token: 0x170016EA RID: 5866
	// (get) Token: 0x060097AA RID: 38826 RVA: 0x003EABAC File Offset: 0x003E8DAC
	public static byte AK_INVALID_MIDI_NOTE
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_AK_INVALID_MIDI_NOTE_get();
		}
	}

	// Token: 0x060097AB RID: 38827 RVA: 0x003EABB4 File Offset: 0x003E8DB4
	public static AKRESULT GetPlayingSegmentInfo(uint in_PlayingID, AkSegmentInfo out_segmentInfo, bool in_bExtrapolate)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetPlayingSegmentInfo__SWIG_0(in_PlayingID, AkSegmentInfo.getCPtr(out_segmentInfo), in_bExtrapolate);
	}

	// Token: 0x060097AC RID: 38828 RVA: 0x003EABC4 File Offset: 0x003E8DC4
	public static AKRESULT GetPlayingSegmentInfo(uint in_PlayingID, AkSegmentInfo out_segmentInfo)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetPlayingSegmentInfo__SWIG_1(in_PlayingID, AkSegmentInfo.getCPtr(out_segmentInfo));
	}

	// Token: 0x060097AD RID: 38829 RVA: 0x003EABD4 File Offset: 0x003E8DD4
	public static AKRESULT PostCode(AkMonitorErrorCode in_eError, AkMonitorErrorLevel in_eErrorLevel, uint in_playingID, GameObject in_gameObjID, uint in_audioNodeID, bool in_bIsBus)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostCode__SWIG_0((int)in_eError, (int)in_eErrorLevel, in_playingID, akGameObjectID, in_audioNodeID, in_bIsBus);
	}

	// Token: 0x060097AE RID: 38830 RVA: 0x003EABFC File Offset: 0x003E8DFC
	public static AKRESULT PostCode(AkMonitorErrorCode in_eError, AkMonitorErrorLevel in_eErrorLevel, uint in_playingID, GameObject in_gameObjID, uint in_audioNodeID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostCode__SWIG_1((int)in_eError, (int)in_eErrorLevel, in_playingID, akGameObjectID, in_audioNodeID);
	}

	// Token: 0x060097AF RID: 38831 RVA: 0x003EAC24 File Offset: 0x003E8E24
	public static AKRESULT PostCode(AkMonitorErrorCode in_eError, AkMonitorErrorLevel in_eErrorLevel, uint in_playingID, GameObject in_gameObjID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostCode__SWIG_2((int)in_eError, (int)in_eErrorLevel, in_playingID, akGameObjectID);
	}

	// Token: 0x060097B0 RID: 38832 RVA: 0x003EAC48 File Offset: 0x003E8E48
	public static AKRESULT PostCode(AkMonitorErrorCode in_eError, AkMonitorErrorLevel in_eErrorLevel, uint in_playingID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostCode__SWIG_3((int)in_eError, (int)in_eErrorLevel, in_playingID);
	}

	// Token: 0x060097B1 RID: 38833 RVA: 0x003EAC54 File Offset: 0x003E8E54
	public static AKRESULT PostCode(AkMonitorErrorCode in_eError, AkMonitorErrorLevel in_eErrorLevel)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostCode__SWIG_4((int)in_eError, (int)in_eErrorLevel);
	}

	// Token: 0x060097B2 RID: 38834 RVA: 0x003EAC60 File Offset: 0x003E8E60
	public static AKRESULT PostString(string in_pszError, AkMonitorErrorLevel in_eErrorLevel, uint in_playingID, GameObject in_gameObjID, uint in_audioNodeID, bool in_bIsBus)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostString__SWIG_0(in_pszError, (int)in_eErrorLevel, in_playingID, akGameObjectID, in_audioNodeID, in_bIsBus);
	}

	// Token: 0x060097B3 RID: 38835 RVA: 0x003EAC88 File Offset: 0x003E8E88
	public static AKRESULT PostString(string in_pszError, AkMonitorErrorLevel in_eErrorLevel, uint in_playingID, GameObject in_gameObjID, uint in_audioNodeID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostString__SWIG_1(in_pszError, (int)in_eErrorLevel, in_playingID, akGameObjectID, in_audioNodeID);
	}

	// Token: 0x060097B4 RID: 38836 RVA: 0x003EACB0 File Offset: 0x003E8EB0
	public static AKRESULT PostString(string in_pszError, AkMonitorErrorLevel in_eErrorLevel, uint in_playingID, GameObject in_gameObjID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostString__SWIG_2(in_pszError, (int)in_eErrorLevel, in_playingID, akGameObjectID);
	}

	// Token: 0x060097B5 RID: 38837 RVA: 0x003EACD4 File Offset: 0x003E8ED4
	public static AKRESULT PostString(string in_pszError, AkMonitorErrorLevel in_eErrorLevel, uint in_playingID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostString__SWIG_3(in_pszError, (int)in_eErrorLevel, in_playingID);
	}

	// Token: 0x060097B6 RID: 38838 RVA: 0x003EACE0 File Offset: 0x003E8EE0
	public static AKRESULT PostString(string in_pszError, AkMonitorErrorLevel in_eErrorLevel)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_PostString__SWIG_4(in_pszError, (int)in_eErrorLevel);
	}

	// Token: 0x060097B7 RID: 38839 RVA: 0x003EACEC File Offset: 0x003E8EEC
	public static int GetTimeStamp()
	{
		return AkSoundEnginePINVOKE.CSharp_GetTimeStamp();
	}

	// Token: 0x060097B8 RID: 38840 RVA: 0x003EACF4 File Offset: 0x003E8EF4
	public static uint GetNumNonZeroBits(uint in_uWord)
	{
		return AkSoundEnginePINVOKE.CSharp_GetNumNonZeroBits(in_uWord);
	}

	// Token: 0x060097B9 RID: 38841 RVA: 0x003EACFC File Offset: 0x003E8EFC
	public static uint ResolveDialogueEvent(uint in_eventID, uint[] in_aArgumentValues, uint in_uNumArguments, uint in_idSequence)
	{
		return AkSoundEnginePINVOKE.CSharp_ResolveDialogueEvent__SWIG_0(in_eventID, in_aArgumentValues, in_uNumArguments, in_idSequence);
	}

	// Token: 0x060097BA RID: 38842 RVA: 0x003EAD08 File Offset: 0x003E8F08
	public static uint ResolveDialogueEvent(uint in_eventID, uint[] in_aArgumentValues, uint in_uNumArguments)
	{
		return AkSoundEnginePINVOKE.CSharp_ResolveDialogueEvent__SWIG_1(in_eventID, in_aArgumentValues, in_uNumArguments);
	}

	// Token: 0x060097BB RID: 38843 RVA: 0x003EAD14 File Offset: 0x003E8F14
	public static AKRESULT GetDialogueEventCustomPropertyValue(uint in_eventID, uint in_uPropID, out int out_iValue)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetDialogueEventCustomPropertyValue__SWIG_0(in_eventID, in_uPropID, out out_iValue);
	}

	// Token: 0x060097BC RID: 38844 RVA: 0x003EAD20 File Offset: 0x003E8F20
	public static AKRESULT GetDialogueEventCustomPropertyValue(uint in_eventID, uint in_uPropID, out float out_fValue)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetDialogueEventCustomPropertyValue__SWIG_1(in_eventID, in_uPropID, out out_fValue);
	}

	// Token: 0x060097BD RID: 38845 RVA: 0x003EAD2C File Offset: 0x003E8F2C
	public static AKRESULT GetPosition(GameObject in_GameObjectID, AkTransform out_rPosition)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetPosition(AkSoundEngine.GetAkGameObjectID(in_GameObjectID), AkTransform.getCPtr(out_rPosition));
	}

	// Token: 0x060097BE RID: 38846 RVA: 0x003EAD40 File Offset: 0x003E8F40
	public static AKRESULT GetListenerPosition(GameObject in_uIndex, AkTransform out_rPosition)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_uIndex);
		AkSoundEngine.PreGameObjectAPICall(in_uIndex, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetListenerPosition(akGameObjectID, AkTransform.getCPtr(out_rPosition));
	}

	// Token: 0x060097BF RID: 38847 RVA: 0x003EAD68 File Offset: 0x003E8F68
	public static AKRESULT GetRTPCValue(uint in_rtpcID, GameObject in_gameObjectID, uint in_playingID, out float out_rValue, ref int io_rValueType)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetRTPCValue__SWIG_0(in_rtpcID, akGameObjectID, in_playingID, out out_rValue, ref io_rValueType);
	}

	// Token: 0x060097C0 RID: 38848 RVA: 0x003EAD90 File Offset: 0x003E8F90
	public static AKRESULT GetRTPCValue(string in_pszRtpcName, GameObject in_gameObjectID, uint in_playingID, out float out_rValue, ref int io_rValueType)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetRTPCValue__SWIG_1(in_pszRtpcName, akGameObjectID, in_playingID, out out_rValue, ref io_rValueType);
	}

	// Token: 0x060097C1 RID: 38849 RVA: 0x003EADB8 File Offset: 0x003E8FB8
	public static AKRESULT GetSwitch(uint in_switchGroup, GameObject in_gameObjectID, out uint out_rSwitchState)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetSwitch__SWIG_0(in_switchGroup, akGameObjectID, out out_rSwitchState);
	}

	// Token: 0x060097C2 RID: 38850 RVA: 0x003EADDC File Offset: 0x003E8FDC
	public static AKRESULT GetSwitch(string in_pstrSwitchGroupName, GameObject in_GameObj, out uint out_rSwitchState)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetSwitch__SWIG_1(in_pstrSwitchGroupName, AkSoundEngine.GetAkGameObjectID(in_GameObj), out out_rSwitchState);
	}

	// Token: 0x060097C3 RID: 38851 RVA: 0x003EADEC File Offset: 0x003E8FEC
	public static AKRESULT GetState(uint in_stateGroup, out uint out_rState)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetState__SWIG_0(in_stateGroup, out out_rState);
	}

	// Token: 0x060097C4 RID: 38852 RVA: 0x003EADF8 File Offset: 0x003E8FF8
	public static AKRESULT GetState(string in_pstrStateGroupName, out uint out_rState)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetState__SWIG_1(in_pstrStateGroupName, out out_rState);
	}

	// Token: 0x060097C5 RID: 38853 RVA: 0x003EAE04 File Offset: 0x003E9004
	public static AKRESULT GetGameObjectAuxSendValues(GameObject in_gameObjectID, AkAuxSendArray out_paAuxSendValues, ref uint io_ruNumSendValues)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetGameObjectAuxSendValues(akGameObjectID, out_paAuxSendValues.GetBuffer(), ref io_ruNumSendValues);
	}

	// Token: 0x060097C6 RID: 38854 RVA: 0x003EAE2C File Offset: 0x003E902C
	public static AKRESULT GetGameObjectDryLevelValue(GameObject in_EmitterID, GameObject in_ListenerID, out float out_rfControlValue)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_EmitterID);
		AkSoundEngine.PreGameObjectAPICall(in_EmitterID, akGameObjectID);
		ulong akGameObjectID2 = AkSoundEngine.GetAkGameObjectID(in_ListenerID);
		AkSoundEngine.PreGameObjectAPICall(in_ListenerID, akGameObjectID2);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetGameObjectDryLevelValue(akGameObjectID, akGameObjectID2, out out_rfControlValue);
	}

	// Token: 0x060097C7 RID: 38855 RVA: 0x003EAE60 File Offset: 0x003E9060
	public static AKRESULT GetObjectObstructionAndOcclusion(GameObject in_EmitterID, GameObject in_ListenerID, out float out_rfObstructionLevel, out float out_rfOcclusionLevel)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_EmitterID);
		AkSoundEngine.PreGameObjectAPICall(in_EmitterID, akGameObjectID);
		ulong akGameObjectID2 = AkSoundEngine.GetAkGameObjectID(in_ListenerID);
		AkSoundEngine.PreGameObjectAPICall(in_ListenerID, akGameObjectID2);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetObjectObstructionAndOcclusion(akGameObjectID, akGameObjectID2, out out_rfObstructionLevel, out out_rfOcclusionLevel);
	}

	// Token: 0x060097C8 RID: 38856 RVA: 0x003EAE94 File Offset: 0x003E9094
	public static AKRESULT QueryAudioObjectIDs(uint in_eventID, ref uint io_ruNumItems, AkObjectInfo out_aObjectInfos)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_QueryAudioObjectIDs__SWIG_0(in_eventID, ref io_ruNumItems, AkObjectInfo.getCPtr(out_aObjectInfos));
	}

	// Token: 0x060097C9 RID: 38857 RVA: 0x003EAEA4 File Offset: 0x003E90A4
	public static AKRESULT QueryAudioObjectIDs(string in_pszEventName, ref uint io_ruNumItems, AkObjectInfo out_aObjectInfos)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_QueryAudioObjectIDs__SWIG_1(in_pszEventName, ref io_ruNumItems, AkObjectInfo.getCPtr(out_aObjectInfos));
	}

	// Token: 0x060097CA RID: 38858 RVA: 0x003EAEB4 File Offset: 0x003E90B4
	public static AKRESULT GetPositioningInfo(uint in_ObjectID, AkPositioningInfo out_rPositioningInfo)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetPositioningInfo(in_ObjectID, AkPositioningInfo.getCPtr(out_rPositioningInfo));
	}

	// Token: 0x060097CB RID: 38859 RVA: 0x003EAEC4 File Offset: 0x003E90C4
	public static bool GetIsGameObjectActive(GameObject in_GameObjId)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_GameObjId);
		AkSoundEngine.PreGameObjectAPICall(in_GameObjId, akGameObjectID);
		return AkSoundEnginePINVOKE.CSharp_GetIsGameObjectActive(akGameObjectID);
	}

	// Token: 0x060097CC RID: 38860 RVA: 0x003EAEE8 File Offset: 0x003E90E8
	public static float GetMaxRadius(GameObject in_GameObjId)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_GameObjId);
		AkSoundEngine.PreGameObjectAPICall(in_GameObjId, akGameObjectID);
		return AkSoundEnginePINVOKE.CSharp_GetMaxRadius(akGameObjectID);
	}

	// Token: 0x060097CD RID: 38861 RVA: 0x003EAF0C File Offset: 0x003E910C
	public static uint GetEventIDFromPlayingID(uint in_playingID)
	{
		return AkSoundEnginePINVOKE.CSharp_GetEventIDFromPlayingID(in_playingID);
	}

	// Token: 0x060097CE RID: 38862 RVA: 0x003EAF14 File Offset: 0x003E9114
	public static ulong GetGameObjectFromPlayingID(uint in_playingID)
	{
		return AkSoundEnginePINVOKE.CSharp_GetGameObjectFromPlayingID(in_playingID);
	}

	// Token: 0x060097CF RID: 38863 RVA: 0x003EAF1C File Offset: 0x003E911C
	public static AKRESULT GetPlayingIDsFromGameObject(GameObject in_GameObjId, ref uint io_ruNumIDs, uint[] out_aPlayingIDs)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_GameObjId);
		AkSoundEngine.PreGameObjectAPICall(in_GameObjId, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetPlayingIDsFromGameObject(akGameObjectID, ref io_ruNumIDs, out_aPlayingIDs);
	}

	// Token: 0x060097D0 RID: 38864 RVA: 0x003EAF40 File Offset: 0x003E9140
	public static AKRESULT GetCustomPropertyValue(uint in_ObjectID, uint in_uPropID, out int out_iValue)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetCustomPropertyValue__SWIG_0(in_ObjectID, in_uPropID, out out_iValue);
	}

	// Token: 0x060097D1 RID: 38865 RVA: 0x003EAF4C File Offset: 0x003E914C
	public static AKRESULT GetCustomPropertyValue(uint in_ObjectID, uint in_uPropID, out float out_fValue)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetCustomPropertyValue__SWIG_1(in_ObjectID, in_uPropID, out out_fValue);
	}

	// Token: 0x060097D2 RID: 38866 RVA: 0x003EAF58 File Offset: 0x003E9158
	public static void AK_SPEAKER_SETUP_FIX_LEFT_TO_CENTER(ref uint io_uChannelMask)
	{
		AkSoundEnginePINVOKE.CSharp_AK_SPEAKER_SETUP_FIX_LEFT_TO_CENTER(ref io_uChannelMask);
	}

	// Token: 0x060097D3 RID: 38867 RVA: 0x003EAF60 File Offset: 0x003E9160
	public static void AK_SPEAKER_SETUP_FIX_REAR_TO_SIDE(ref uint io_uChannelMask)
	{
		AkSoundEnginePINVOKE.CSharp_AK_SPEAKER_SETUP_FIX_REAR_TO_SIDE(ref io_uChannelMask);
	}

	// Token: 0x060097D4 RID: 38868 RVA: 0x003EAF68 File Offset: 0x003E9168
	public static void AK_SPEAKER_SETUP_CONVERT_TO_SUPPORTED(ref uint io_uChannelMask)
	{
		AkSoundEnginePINVOKE.CSharp_AK_SPEAKER_SETUP_CONVERT_TO_SUPPORTED(ref io_uChannelMask);
	}

	// Token: 0x060097D5 RID: 38869 RVA: 0x003EAF70 File Offset: 0x003E9170
	public static byte ChannelMaskToNumChannels(uint in_uChannelMask)
	{
		return AkSoundEnginePINVOKE.CSharp_ChannelMaskToNumChannels(in_uChannelMask);
	}

	// Token: 0x060097D6 RID: 38870 RVA: 0x003EAF78 File Offset: 0x003E9178
	public static uint ChannelMaskFromNumChannels(uint in_uNumChannels)
	{
		return AkSoundEnginePINVOKE.CSharp_ChannelMaskFromNumChannels(in_uNumChannels);
	}

	// Token: 0x060097D7 RID: 38871 RVA: 0x003EAF80 File Offset: 0x003E9180
	public static byte ChannelBitToIndex(uint in_uChannelBit, uint in_uChannelMask)
	{
		return AkSoundEnginePINVOKE.CSharp_ChannelBitToIndex(in_uChannelBit, in_uChannelMask);
	}

	// Token: 0x060097D8 RID: 38872 RVA: 0x003EAF8C File Offset: 0x003E918C
	public static bool HasSurroundChannels(uint in_uChannelMask)
	{
		return AkSoundEnginePINVOKE.CSharp_HasSurroundChannels(in_uChannelMask);
	}

	// Token: 0x060097D9 RID: 38873 RVA: 0x003EAF94 File Offset: 0x003E9194
	public static bool HasStrictlyOnePairOfSurroundChannels(uint in_uChannelMask)
	{
		return AkSoundEnginePINVOKE.CSharp_HasStrictlyOnePairOfSurroundChannels(in_uChannelMask);
	}

	// Token: 0x060097DA RID: 38874 RVA: 0x003EAF9C File Offset: 0x003E919C
	public static bool HasSideAndRearChannels(uint in_uChannelMask)
	{
		return AkSoundEnginePINVOKE.CSharp_HasSideAndRearChannels(in_uChannelMask);
	}

	// Token: 0x060097DB RID: 38875 RVA: 0x003EAFA4 File Offset: 0x003E91A4
	public static bool HasHeightChannels(uint in_uChannelMask)
	{
		return AkSoundEnginePINVOKE.CSharp_HasHeightChannels(in_uChannelMask);
	}

	// Token: 0x060097DC RID: 38876 RVA: 0x003EAFAC File Offset: 0x003E91AC
	public static uint BackToSideChannels(uint in_uChannelMask)
	{
		return AkSoundEnginePINVOKE.CSharp_BackToSideChannels(in_uChannelMask);
	}

	// Token: 0x060097DD RID: 38877 RVA: 0x003EAFB4 File Offset: 0x003E91B4
	public static uint StdChannelIndexToDisplayIndex(AkChannelOrdering in_eOrdering, uint in_uChannelMask, uint in_uChannelIdx)
	{
		return AkSoundEnginePINVOKE.CSharp_StdChannelIndexToDisplayIndex((int)in_eOrdering, in_uChannelMask, in_uChannelIdx);
	}

	// Token: 0x170016EB RID: 5867
	// (get) Token: 0x060097DE RID: 38878 RVA: 0x003EAFC0 File Offset: 0x003E91C0
	public static float kDefaultMaxPathLength
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_kDefaultMaxPathLength_get();
		}
	}

	// Token: 0x170016EC RID: 5868
	// (get) Token: 0x060097E0 RID: 38880 RVA: 0x003EAFD0 File Offset: 0x003E91D0
	// (set) Token: 0x060097DF RID: 38879 RVA: 0x003EAFC8 File Offset: 0x003E91C8
	public static int g_SpatialAudioPoolId
	{
		get
		{
			return AkSoundEnginePINVOKE.CSharp_g_SpatialAudioPoolId_get();
		}
		set
		{
			AkSoundEnginePINVOKE.CSharp_g_SpatialAudioPoolId_set(value);
		}
	}

	// Token: 0x060097E1 RID: 38881 RVA: 0x003EAFD8 File Offset: 0x003E91D8
	public static int GetPoolID()
	{
		return AkSoundEnginePINVOKE.CSharp_GetPoolID();
	}

	// Token: 0x060097E2 RID: 38882 RVA: 0x003EAFE0 File Offset: 0x003E91E0
	public static AKRESULT RegisterEmitter(GameObject in_gameObjectID, AkEmitterSettings in_settings)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RegisterEmitter(akGameObjectID, AkEmitterSettings.getCPtr(in_settings));
	}

	// Token: 0x060097E3 RID: 38883 RVA: 0x003EB008 File Offset: 0x003E9208
	public static AKRESULT UnregisterEmitter(GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnregisterEmitter(akGameObjectID);
	}

	// Token: 0x060097E4 RID: 38884 RVA: 0x003EB02C File Offset: 0x003E922C
	public static AKRESULT SetEmitterAuxSendValues(GameObject in_gameObjectID, AkAuxSendArray in_pAuxSends, uint in_uNumAux)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetEmitterAuxSendValues(akGameObjectID, in_pAuxSends.GetBuffer(), in_uNumAux);
	}

	// Token: 0x060097E5 RID: 38885 RVA: 0x003EB054 File Offset: 0x003E9254
	public static AKRESULT SetImageSource(uint in_srcID, AkImageSourceSettings in_info, uint in_AuxBusID, ulong in_roomID, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetImageSource__SWIG_0(in_srcID, AkImageSourceSettings.getCPtr(in_info), in_AuxBusID, in_roomID, akGameObjectID);
	}

	// Token: 0x060097E6 RID: 38886 RVA: 0x003EB080 File Offset: 0x003E9280
	public static AKRESULT SetImageSource(uint in_srcID, AkImageSourceSettings in_info, uint in_AuxBusID, ulong in_roomID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetImageSource__SWIG_1(in_srcID, AkImageSourceSettings.getCPtr(in_info), in_AuxBusID, in_roomID);
	}

	// Token: 0x060097E7 RID: 38887 RVA: 0x003EB090 File Offset: 0x003E9290
	public static AKRESULT RemoveImageSource(uint in_srcID, uint in_AuxBusID, GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RemoveImageSource__SWIG_0(in_srcID, in_AuxBusID, akGameObjectID);
	}

	// Token: 0x060097E8 RID: 38888 RVA: 0x003EB0B4 File Offset: 0x003E92B4
	public static AKRESULT RemoveImageSource(uint in_srcID, uint in_AuxBusID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RemoveImageSource__SWIG_1(in_srcID, in_AuxBusID);
	}

	// Token: 0x060097E9 RID: 38889 RVA: 0x003EB0C0 File Offset: 0x003E92C0
	public static AKRESULT SetGeometry(ulong in_GeomSetID, AkTriangleArray in_pTriangles, uint in_uNumTriangles)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetGeometry(in_GeomSetID, in_pTriangles.GetBuffer(), in_uNumTriangles);
	}

	// Token: 0x060097EA RID: 38890 RVA: 0x003EB0D0 File Offset: 0x003E92D0
	public static AKRESULT RemoveGeometry(ulong in_SetID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RemoveGeometry(in_SetID);
	}

	// Token: 0x060097EB RID: 38891 RVA: 0x003EB0D8 File Offset: 0x003E92D8
	public static AKRESULT RemoveRoom(ulong in_RoomID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RemoveRoom(in_RoomID);
	}

	// Token: 0x060097EC RID: 38892 RVA: 0x003EB0E0 File Offset: 0x003E92E0
	public static AKRESULT RemovePortal(ulong in_PortalID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RemovePortal(in_PortalID);
	}

	// Token: 0x060097ED RID: 38893 RVA: 0x003EB0E8 File Offset: 0x003E92E8
	public static AKRESULT SetGameObjectInRoom(GameObject in_gameObjectID, ulong in_CurrentRoomID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetGameObjectInRoom(akGameObjectID, in_CurrentRoomID);
	}

	// Token: 0x060097EE RID: 38894 RVA: 0x003EB10C File Offset: 0x003E930C
	public static AKRESULT SetEmitterObstruction(GameObject in_gameObjectID, float in_fObstruction)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetEmitterObstruction(akGameObjectID, in_fObstruction);
	}

	// Token: 0x060097EF RID: 38895 RVA: 0x003EB130 File Offset: 0x003E9330
	public static AKRESULT SetPortalObstruction(ulong in_PortalID, float in_fObstruction)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetPortalObstruction(in_PortalID, in_fObstruction);
	}

	// Token: 0x060097F0 RID: 38896 RVA: 0x003EB13C File Offset: 0x003E933C
	public static void SetErrorLogger(AkLogger.ErrorLoggerInteropDelegate logger)
	{
		AkSoundEnginePINVOKE.CSharp_SetErrorLogger__SWIG_0(logger);
	}

	// Token: 0x060097F1 RID: 38897 RVA: 0x003EB144 File Offset: 0x003E9344
	public static void SetErrorLogger()
	{
		AkSoundEnginePINVOKE.CSharp_SetErrorLogger__SWIG_1();
	}

	// Token: 0x060097F2 RID: 38898 RVA: 0x003EB14C File Offset: 0x003E934C
	public static void SetAudioInputCallbacks(AkAudioInputManager.AudioSamplesInteropDelegate getAudioSamples, AkAudioInputManager.AudioFormatInteropDelegate getAudioFormat)
	{
		AkSoundEnginePINVOKE.CSharp_SetAudioInputCallbacks(getAudioSamples, getAudioFormat);
	}

	// Token: 0x060097F3 RID: 38899 RVA: 0x003EB158 File Offset: 0x003E9358
	public static AKRESULT Init(AkMemSettings in_pMemSettings, AkStreamMgrSettings in_pStmSettings, AkDeviceSettings in_pDefaultDeviceSettings, AkInitSettings in_pSettings, AkPlatformInitSettings in_pPlatformSettings, AkMusicSettings in_pMusicSettings, AkSpatialAudioInitSettings in_pSpatialAudioSettings, uint in_preparePoolSizeByte)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_Init(AkMemSettings.getCPtr(in_pMemSettings), AkStreamMgrSettings.getCPtr(in_pStmSettings), AkDeviceSettings.getCPtr(in_pDefaultDeviceSettings), AkInitSettings.getCPtr(in_pSettings), AkPlatformInitSettings.getCPtr(in_pPlatformSettings), AkMusicSettings.getCPtr(in_pMusicSettings), AkSpatialAudioInitSettings.getCPtr(in_pSpatialAudioSettings), in_preparePoolSizeByte);
	}

	// Token: 0x060097F4 RID: 38900 RVA: 0x003EB190 File Offset: 0x003E9390
	public static void Term()
	{
		AkSoundEnginePINVOKE.CSharp_Term();
	}

	// Token: 0x060097F5 RID: 38901 RVA: 0x003EB198 File Offset: 0x003E9398
	public static AKRESULT RegisterGameObjInternal(GameObject in_GameObj)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RegisterGameObjInternal(AkSoundEngine.GetAkGameObjectID(in_GameObj));
	}

	// Token: 0x060097F6 RID: 38902 RVA: 0x003EB1A8 File Offset: 0x003E93A8
	public static AKRESULT UnregisterGameObjInternal(GameObject in_GameObj)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnregisterGameObjInternal(AkSoundEngine.GetAkGameObjectID(in_GameObj));
	}

	// Token: 0x060097F7 RID: 38903 RVA: 0x003EB1B8 File Offset: 0x003E93B8
	public static AKRESULT RegisterGameObjInternal_WithName(GameObject in_GameObj, string in_pszObjName)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RegisterGameObjInternal_WithName(AkSoundEngine.GetAkGameObjectID(in_GameObj), in_pszObjName);
	}

	// Token: 0x060097F8 RID: 38904 RVA: 0x003EB1C8 File Offset: 0x003E93C8
	public static AKRESULT SetBasePath(string in_pszBasePath)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetBasePath(in_pszBasePath);
	}

	// Token: 0x060097F9 RID: 38905 RVA: 0x003EB1D0 File Offset: 0x003E93D0
	public static AKRESULT SetCurrentLanguage(string in_pszAudioSrcPath)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetCurrentLanguage(in_pszAudioSrcPath);
	}

	// Token: 0x060097FA RID: 38906 RVA: 0x003EB1D8 File Offset: 0x003E93D8
	public static AKRESULT LoadFilePackage(string in_pszFilePackageName, out uint out_uPackageID, int in_memPoolID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_LoadFilePackage(in_pszFilePackageName, out out_uPackageID, in_memPoolID);
	}

	// Token: 0x060097FB RID: 38907 RVA: 0x003EB1E4 File Offset: 0x003E93E4
	public static AKRESULT AddBasePath(string in_pszBasePath)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AddBasePath(in_pszBasePath);
	}

	// Token: 0x060097FC RID: 38908 RVA: 0x003EB1EC File Offset: 0x003E93EC
	public static AKRESULT SetGameName(string in_GameName)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetGameName(in_GameName);
	}

	// Token: 0x060097FD RID: 38909 RVA: 0x003EB1F4 File Offset: 0x003E93F4
	public static AKRESULT SetDecodedBankPath(string in_DecodedPath)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetDecodedBankPath(in_DecodedPath);
	}

	// Token: 0x060097FE RID: 38910 RVA: 0x003EB1FC File Offset: 0x003E93FC
	public static AKRESULT LoadAndDecodeBank(string in_pszString, bool in_bSaveDecodedBank, out uint out_bankID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_LoadAndDecodeBank(in_pszString, in_bSaveDecodedBank, out out_bankID);
	}

	// Token: 0x060097FF RID: 38911 RVA: 0x003EB208 File Offset: 0x003E9408
	public static AKRESULT LoadAndDecodeBankFromMemory(IntPtr in_BankData, uint in_BankDataSize, bool in_bSaveDecodedBank, string in_DecodedBankName, bool in_bIsLanguageSpecific, out uint out_bankID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_LoadAndDecodeBankFromMemory(in_BankData, in_BankDataSize, in_bSaveDecodedBank, in_DecodedBankName, in_bIsLanguageSpecific, out out_bankID);
	}

	// Token: 0x06009800 RID: 38912 RVA: 0x003EB218 File Offset: 0x003E9418
	public static string GetCurrentLanguage()
	{
		return AkSoundEngine.StringFromIntPtrOSString(AkSoundEnginePINVOKE.CSharp_GetCurrentLanguage());
	}

	// Token: 0x06009801 RID: 38913 RVA: 0x003EB224 File Offset: 0x003E9424
	public static AKRESULT UnloadFilePackage(uint in_uPackageID)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnloadFilePackage(in_uPackageID);
	}

	// Token: 0x06009802 RID: 38914 RVA: 0x003EB22C File Offset: 0x003E942C
	public static AKRESULT UnloadAllFilePackages()
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnloadAllFilePackages();
	}

	// Token: 0x06009803 RID: 38915 RVA: 0x003EB234 File Offset: 0x003E9434
	public static AKRESULT SetObjectPosition(GameObject in_GameObjectID, float PosX, float PosY, float PosZ, float FrontX, float FrontY, float FrontZ, float TopX, float TopY, float TopZ)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetObjectPosition(AkSoundEngine.GetAkGameObjectID(in_GameObjectID), PosX, PosY, PosZ, FrontX, FrontY, FrontZ, TopX, TopY, TopZ);
	}

	// Token: 0x06009804 RID: 38916 RVA: 0x003EB25C File Offset: 0x003E945C
	public static AKRESULT GetSourceMultiplePlayPositions(uint in_PlayingID, uint[] out_audioNodeID, uint[] out_mediaID, int[] out_msTime, ref uint io_pcPositions, bool in_bExtrapolate)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_GetSourceMultiplePlayPositions(in_PlayingID, out_audioNodeID, out_mediaID, out_msTime, ref io_pcPositions, in_bExtrapolate);
	}

	// Token: 0x06009805 RID: 38917 RVA: 0x003EB26C File Offset: 0x003E946C
	public static AKRESULT SetListeners(GameObject in_emitterGameObj, ulong[] in_pListenerGameObjs, uint in_uNumListeners)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_emitterGameObj);
		AkSoundEngine.PreGameObjectAPICall(in_emitterGameObj, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetListeners(akGameObjectID, in_pListenerGameObjs, in_uNumListeners);
	}

	// Token: 0x06009806 RID: 38918 RVA: 0x003EB290 File Offset: 0x003E9490
	public static AKRESULT SetDefaultListeners(ulong[] in_pListenerObjs, uint in_uNumListeners)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetDefaultListeners(in_pListenerObjs, in_uNumListeners);
	}

	// Token: 0x06009807 RID: 38919 RVA: 0x003EB29C File Offset: 0x003E949C
	public static AKRESULT AddOutput(AkOutputSettings in_Settings, out ulong out_pDeviceID, ulong[] in_pListenerIDs, uint in_uNumListeners)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_AddOutput(AkOutputSettings.getCPtr(in_Settings), out out_pDeviceID, in_pListenerIDs, in_uNumListeners);
	}

	// Token: 0x06009808 RID: 38920 RVA: 0x003EB2AC File Offset: 0x003E94AC
	public static void GetDefaultStreamSettings(AkStreamMgrSettings out_settings)
	{
		AkSoundEnginePINVOKE.CSharp_GetDefaultStreamSettings(AkStreamMgrSettings.getCPtr(out_settings));
	}

	// Token: 0x06009809 RID: 38921 RVA: 0x003EB2BC File Offset: 0x003E94BC
	public static void GetDefaultDeviceSettings(AkDeviceSettings out_settings)
	{
		AkSoundEnginePINVOKE.CSharp_GetDefaultDeviceSettings(AkDeviceSettings.getCPtr(out_settings));
	}

	// Token: 0x0600980A RID: 38922 RVA: 0x003EB2CC File Offset: 0x003E94CC
	public static void GetDefaultMusicSettings(AkMusicSettings out_settings)
	{
		AkSoundEnginePINVOKE.CSharp_GetDefaultMusicSettings(AkMusicSettings.getCPtr(out_settings));
	}

	// Token: 0x0600980B RID: 38923 RVA: 0x003EB2DC File Offset: 0x003E94DC
	public static void GetDefaultInitSettings(AkInitSettings out_settings)
	{
		AkSoundEnginePINVOKE.CSharp_GetDefaultInitSettings(AkInitSettings.getCPtr(out_settings));
	}

	// Token: 0x0600980C RID: 38924 RVA: 0x003EB2EC File Offset: 0x003E94EC
	public static void GetDefaultPlatformInitSettings(AkPlatformInitSettings out_settings)
	{
		AkSoundEnginePINVOKE.CSharp_GetDefaultPlatformInitSettings(AkPlatformInitSettings.getCPtr(out_settings));
	}

	// Token: 0x0600980D RID: 38925 RVA: 0x003EB2FC File Offset: 0x003E94FC
	public static uint GetMajorMinorVersion()
	{
		return AkSoundEnginePINVOKE.CSharp_GetMajorMinorVersion();
	}

	// Token: 0x0600980E RID: 38926 RVA: 0x003EB304 File Offset: 0x003E9504
	public static uint GetSubminorBuildVersion()
	{
		return AkSoundEnginePINVOKE.CSharp_GetSubminorBuildVersion();
	}

	// Token: 0x0600980F RID: 38927 RVA: 0x003EB30C File Offset: 0x003E950C
	public static uint GetDeviceIDFromName(string in_szToken)
	{
		return AkSoundEnginePINVOKE.CSharp_GetDeviceIDFromName(in_szToken);
	}

	// Token: 0x06009810 RID: 38928 RVA: 0x003EB314 File Offset: 0x003E9514
	public static string GetWindowsDeviceName(int index, out uint out_uDeviceID)
	{
		return AkSoundEngine.StringFromIntPtrWString(AkSoundEnginePINVOKE.CSharp_GetWindowsDeviceName(index, out out_uDeviceID));
	}

	// Token: 0x06009811 RID: 38929 RVA: 0x003EB324 File Offset: 0x003E9524
	public static AKRESULT QueryIndirectPaths(GameObject in_gameObjectID, AkSoundPropagationPathParams arg1, AkSoundPathInfoArray paths, uint numPaths)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_QueryIndirectPaths(akGameObjectID, AkSoundPropagationPathParams.getCPtr(arg1), paths.GetBuffer(), numPaths);
	}

	// Token: 0x06009812 RID: 38930 RVA: 0x003EB354 File Offset: 0x003E9554
	public static AKRESULT QuerySoundPropagationPaths(GameObject in_gameObjectID, AkSoundPropagationPathParams arg1, AkPropagationPathInfoArray paths, uint numPaths)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_QuerySoundPropagationPaths(akGameObjectID, AkSoundPropagationPathParams.getCPtr(arg1), paths.GetBuffer(), numPaths);
	}

	// Token: 0x06009813 RID: 38931 RVA: 0x003EB384 File Offset: 0x003E9584
	public static AKRESULT SetRoomPortal(ulong in_PortalID, AkTransform Transform, AkVector Extent, bool bEnabled, ulong FrontRoom, ulong BackRoom)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRoomPortal(in_PortalID, AkTransform.getCPtr(Transform), AkVector.getCPtr(Extent), bEnabled, FrontRoom, BackRoom);
	}

	// Token: 0x06009814 RID: 38932 RVA: 0x003EB3A0 File Offset: 0x003E95A0
	public static AKRESULT SetRoom(ulong in_RoomID, AkRoomParams in_roomParams, string in_pName)
	{
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetRoom(in_RoomID, AkRoomParams.getCPtr(in_roomParams), in_pName);
	}

	// Token: 0x06009815 RID: 38933 RVA: 0x003EB3B0 File Offset: 0x003E95B0
	public static AKRESULT RegisterSpatialAudioListener(GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_RegisterSpatialAudioListener(akGameObjectID);
	}

	// Token: 0x06009816 RID: 38934 RVA: 0x003EB3D4 File Offset: 0x003E95D4
	public static AKRESULT UnregisterSpatialAudioListener(GameObject in_gameObjectID)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(in_gameObjectID);
		AkSoundEngine.PreGameObjectAPICall(in_gameObjectID, akGameObjectID);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnregisterSpatialAudioListener(akGameObjectID);
	}

	// Token: 0x06009817 RID: 38935 RVA: 0x003EB3F8 File Offset: 0x003E95F8
	public static string StringFromIntPtrString(IntPtr ptr)
	{
		return Marshal.PtrToStringAnsi(ptr);
	}

	// Token: 0x06009818 RID: 38936 RVA: 0x003EB400 File Offset: 0x003E9600
	public static string StringFromIntPtrWString(IntPtr ptr)
	{
		return Marshal.PtrToStringUni(ptr);
	}

	// Token: 0x06009819 RID: 38937 RVA: 0x003EB408 File Offset: 0x003E9608
	public static string StringFromIntPtrOSString(IntPtr ptr)
	{
		return AkSoundEngine.StringFromIntPtrWString(ptr);
	}

	// Token: 0x0600981A RID: 38938 RVA: 0x003EB410 File Offset: 0x003E9610
	private static ulong InternalGameObjectHash(GameObject gameObject)
	{
		return (ulong)((!(gameObject == null)) ? ((long)gameObject.GetInstanceID()) : (-1L));
	}

	// Token: 0x170016ED RID: 5869
	// (set) Token: 0x0600981B RID: 38939 RVA: 0x003EB42C File Offset: 0x003E962C
	public static AkSoundEngine.GameObjectHashFunction GameObjectHash
	{
		set
		{
			AkSoundEngine.GameObjectHashFunction gameObjectHashFunction;
			if (value == null)
			{
				gameObjectHashFunction = new AkSoundEngine.GameObjectHashFunction(AkSoundEngine.InternalGameObjectHash);
			}
			else
			{
				gameObjectHashFunction = value;
			}
			AkSoundEngine.gameObjectHash = gameObjectHashFunction;
		}
	}

	// Token: 0x0600981C RID: 38940 RVA: 0x003EB45C File Offset: 0x003E965C
	public static ulong GetAkGameObjectID(GameObject gameObject)
	{
		return AkSoundEngine.gameObjectHash(gameObject);
	}

	// Token: 0x0600981D RID: 38941 RVA: 0x003EB46C File Offset: 0x003E966C
	public static AKRESULT RegisterGameObj(GameObject gameObject)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObject);
		AKRESULT akresult = (AKRESULT)AkSoundEnginePINVOKE.CSharp_RegisterGameObjInternal(akGameObjectID);
		AkSoundEngine.PostRegisterGameObjUserHook(akresult, gameObject, akGameObjectID);
		return akresult;
	}

	// Token: 0x0600981E RID: 38942 RVA: 0x003EB490 File Offset: 0x003E9690
	public static AKRESULT RegisterGameObj(GameObject gameObject, string name)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObject);
		AKRESULT akresult = (AKRESULT)AkSoundEnginePINVOKE.CSharp_RegisterGameObjInternal_WithName(akGameObjectID, name);
		AkSoundEngine.PostRegisterGameObjUserHook(akresult, gameObject, akGameObjectID);
		return akresult;
	}

	// Token: 0x0600981F RID: 38943 RVA: 0x003EB4B8 File Offset: 0x003E96B8
	public static AKRESULT UnregisterGameObj(GameObject gameObject)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObject);
		AKRESULT akresult = (AKRESULT)AkSoundEnginePINVOKE.CSharp_UnregisterGameObjInternal(akGameObjectID);
		AkSoundEngine.PostUnregisterGameObjUserHook(akresult, gameObject, akGameObjectID);
		return akresult;
	}

	// Token: 0x06009820 RID: 38944 RVA: 0x003EB4DC File Offset: 0x003E96DC
	public static AKRESULT SetObjectPosition(GameObject gameObject, Transform transform)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObject);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetObjectPosition(akGameObjectID, transform.position.x, transform.position.y, transform.position.z, transform.forward.x, transform.forward.y, transform.forward.z, transform.up.x, transform.up.y, transform.up.z);
	}

	// Token: 0x06009821 RID: 38945 RVA: 0x003EB57C File Offset: 0x003E977C
	public static AKRESULT SetObjectPosition(GameObject gameObject, Vector3 position, Vector3 forward, Vector3 up)
	{
		ulong akGameObjectID = AkSoundEngine.GetAkGameObjectID(gameObject);
		return (AKRESULT)AkSoundEnginePINVOKE.CSharp_SetObjectPosition(akGameObjectID, position.x, position.y, position.z, forward.x, forward.y, forward.z, up.x, up.y, up.z);
	}

	// Token: 0x06009822 RID: 38946 RVA: 0x003EB5D8 File Offset: 0x003E97D8
	public static void PreGameObjectAPICall(GameObject gameObject, ulong id)
	{
		AkSoundEngine.PreGameObjectAPICallUserHook(gameObject, id);
	}

	// Token: 0x06009823 RID: 38947 RVA: 0x003EB5E4 File Offset: 0x003E97E4
	private static void AutoRegister(GameObject gameObject, ulong id)
	{
		if (gameObject == null || !gameObject.activeInHierarchy)
		{
			new AkSoundEngine.AutoObject(gameObject);
		}
		else if (gameObject.GetComponent<AkGameObj>() == null)
		{
			gameObject.AddComponent<AkGameObj>();
		}
	}

	// Token: 0x06009824 RID: 38948 RVA: 0x003EB624 File Offset: 0x003E9824
	private static void PreGameObjectAPICallUserHook(GameObject gameObject, ulong id)
	{
		if (!AkSoundEngine.IsInRegisteredList(id) && AkSoundEngine.IsInitialized())
		{
			AkSoundEngine.AutoRegister(gameObject, id);
		}
	}

	// Token: 0x06009825 RID: 38949 RVA: 0x003EB644 File Offset: 0x003E9844
	private static void PostRegisterGameObjUserHook(AKRESULT result, GameObject gameObject, ulong id)
	{
		if (result == AKRESULT.AK_Success)
		{
			AkSoundEngine.RegisteredGameObjects.Add(id);
		}
	}

	// Token: 0x06009826 RID: 38950 RVA: 0x003EB65C File Offset: 0x003E985C
	private static void PostUnregisterGameObjUserHook(AKRESULT result, GameObject gameObject, ulong id)
	{
		if (result == AKRESULT.AK_Success)
		{
			AkSoundEngine.RegisteredGameObjects.Remove(id);
		}
	}

	// Token: 0x06009827 RID: 38951 RVA: 0x003EB674 File Offset: 0x003E9874
	private static bool IsInRegisteredList(ulong id)
	{
		return AkSoundEngine.RegisteredGameObjects.Contains(id);
	}

	// Token: 0x06009828 RID: 38952 RVA: 0x003EB684 File Offset: 0x003E9884
	public static bool IsGameObjectRegistered(GameObject in_gameObject)
	{
		return AkSoundEngine.IsInRegisteredList(AkSoundEngine.GetAkGameObjectID(in_gameObject));
	}

	// Token: 0x04009CE9 RID: 40169
	public const int _WIN32_WINNT = 1538;

	// Token: 0x04009CEA RID: 40170
	public const int AK_SIMD_ALIGNMENT = 16;

	// Token: 0x04009CEB RID: 40171
	public const int AK_BUFFER_ALIGNMENT = 16;

	// Token: 0x04009CEC RID: 40172
	public const int AK_XAUDIO2_FLAGS = 0;

	// Token: 0x04009CED RID: 40173
	public const int AK_MAX_PATH = 260;

	// Token: 0x04009CEE RID: 40174
	public const int AK_BANK_PLATFORM_DATA_ALIGNMENT = 16;

	// Token: 0x04009CEF RID: 40175
	public const uint AK_INVALID_PLUGINID = 4294967295U;

	// Token: 0x04009CF0 RID: 40176
	public const ulong AK_INVALID_GAME_OBJECT = 18446744073709551615UL;

	// Token: 0x04009CF1 RID: 40177
	public const uint AK_INVALID_UNIQUE_ID = 0U;

	// Token: 0x04009CF2 RID: 40178
	public const uint AK_INVALID_RTPC_ID = 0U;

	// Token: 0x04009CF3 RID: 40179
	public const uint AK_INVALID_LISTENER_INDEX = 4294967295U;

	// Token: 0x04009CF4 RID: 40180
	public const uint AK_INVALID_PLAYING_ID = 0U;

	// Token: 0x04009CF5 RID: 40181
	public const uint AK_DEFAULT_SWITCH_STATE = 0U;

	// Token: 0x04009CF6 RID: 40182
	public const int AK_INVALID_POOL_ID = -1;

	// Token: 0x04009CF7 RID: 40183
	public const int AK_DEFAULT_POOL_ID = -1;

	// Token: 0x04009CF8 RID: 40184
	public const uint AK_INVALID_AUX_ID = 0U;

	// Token: 0x04009CF9 RID: 40185
	public const uint AK_INVALID_FILE_ID = 4294967295U;

	// Token: 0x04009CFA RID: 40186
	public const uint AK_INVALID_DEVICE_ID = 4294967295U;

	// Token: 0x04009CFB RID: 40187
	public const uint AK_INVALID_BANK_ID = 0U;

	// Token: 0x04009CFC RID: 40188
	public const uint AK_FALLBACK_ARGUMENTVALUE_ID = 0U;

	// Token: 0x04009CFD RID: 40189
	public const uint AK_INVALID_CHANNELMASK = 0U;

	// Token: 0x04009CFE RID: 40190
	public const uint AK_INVALID_OUTPUT_DEVICE_ID = 0U;

	// Token: 0x04009CFF RID: 40191
	public const uint AK_MIXER_FX_SLOT = 4294967295U;

	// Token: 0x04009D00 RID: 40192
	public const ulong AK_DEFAULT_LISTENER_OBJ = 0UL;

	// Token: 0x04009D01 RID: 40193
	public const uint AK_DEFAULT_PRIORITY = 50U;

	// Token: 0x04009D02 RID: 40194
	public const uint AK_MIN_PRIORITY = 0U;

	// Token: 0x04009D03 RID: 40195
	public const uint AK_MAX_PRIORITY = 100U;

	// Token: 0x04009D04 RID: 40196
	public const uint AK_DEFAULT_BANK_IO_PRIORITY = 50U;

	// Token: 0x04009D05 RID: 40197
	public const double AK_DEFAULT_BANK_THROUGHPUT = 1048.576;

	// Token: 0x04009D06 RID: 40198
	public const uint AKCOMPANYID_AUDIOKINETIC = 0U;

	// Token: 0x04009D07 RID: 40199
	public const uint AK_LISTENERS_MASK_ALL = 4294967295U;

	// Token: 0x04009D08 RID: 40200
	public const int NULL = 0;

	// Token: 0x04009D09 RID: 40201
	public const int AKCURVEINTERPOLATION_NUM_STORAGE_BIT = 5;

	// Token: 0x04009D0A RID: 40202
	public const int AK_MAX_LANGUAGE_NAME_SIZE = 32;

	// Token: 0x04009D0B RID: 40203
	public const int AKCOMPANYID_AUDIOKINETIC_EXTERNAL = 1;

	// Token: 0x04009D0C RID: 40204
	public const int AKCOMPANYID_MCDSP = 256;

	// Token: 0x04009D0D RID: 40205
	public const int AKCOMPANYID_WAVEARTS = 257;

	// Token: 0x04009D0E RID: 40206
	public const int AKCOMPANYID_PHONETICARTS = 258;

	// Token: 0x04009D0F RID: 40207
	public const int AKCOMPANYID_IZOTOPE = 259;

	// Token: 0x04009D10 RID: 40208
	public const int AKCOMPANYID_GENAUDIO = 260;

	// Token: 0x04009D11 RID: 40209
	public const int AKCOMPANYID_CRANKCASEAUDIO = 261;

	// Token: 0x04009D12 RID: 40210
	public const int AKCOMPANYID_IOSONO = 262;

	// Token: 0x04009D13 RID: 40211
	public const int AKCOMPANYID_AUROTECHNOLOGIES = 263;

	// Token: 0x04009D14 RID: 40212
	public const int AKCOMPANYID_DOLBY = 264;

	// Token: 0x04009D15 RID: 40213
	public const int AKCOMPANYID_TWOBIGEARS = 265;

	// Token: 0x04009D16 RID: 40214
	public const int AKCOMPANYID_OCULUS = 266;

	// Token: 0x04009D17 RID: 40215
	public const int AKCOMPANYID_BLUERIPPLESOUND = 267;

	// Token: 0x04009D18 RID: 40216
	public const int AKCOMPANYID_ENZIEN = 268;

	// Token: 0x04009D19 RID: 40217
	public const int AKCOMPANYID_KROTOS = 269;

	// Token: 0x04009D1A RID: 40218
	public const int AKCOMPANYID_NURULIZE = 270;

	// Token: 0x04009D1B RID: 40219
	public const int AKCOMPANYID_SUPERPOWERED = 271;

	// Token: 0x04009D1C RID: 40220
	public const int AKCOMPANYID_GOOGLE = 272;

	// Token: 0x04009D1D RID: 40221
	public const int AKCOMPANYID_VISISONICS = 277;

	// Token: 0x04009D1E RID: 40222
	public const int AKCODECID_BANK = 0;

	// Token: 0x04009D1F RID: 40223
	public const int AKCODECID_PCM = 1;

	// Token: 0x04009D20 RID: 40224
	public const int AKCODECID_ADPCM = 2;

	// Token: 0x04009D21 RID: 40225
	public const int AKCODECID_XMA = 3;

	// Token: 0x04009D22 RID: 40226
	public const int AKCODECID_VORBIS = 4;

	// Token: 0x04009D23 RID: 40227
	public const int AKCODECID_WIIADPCM = 5;

	// Token: 0x04009D24 RID: 40228
	public const int AKCODECID_PCMEX = 7;

	// Token: 0x04009D25 RID: 40229
	public const int AKCODECID_EXTERNAL_SOURCE = 8;

	// Token: 0x04009D26 RID: 40230
	public const int AKCODECID_XWMA = 9;

	// Token: 0x04009D27 RID: 40231
	public const int AKCODECID_AAC = 10;

	// Token: 0x04009D28 RID: 40232
	public const int AKCODECID_FILE_PACKAGE = 11;

	// Token: 0x04009D29 RID: 40233
	public const int AKCODECID_ATRAC9 = 12;

	// Token: 0x04009D2A RID: 40234
	public const int AKCODECID_VAG = 13;

	// Token: 0x04009D2B RID: 40235
	public const int AKCODECID_PROFILERCAPTURE = 14;

	// Token: 0x04009D2C RID: 40236
	public const int AKCODECID_ANALYSISFILE = 15;

	// Token: 0x04009D2D RID: 40237
	public const int AKCODECID_MIDI = 16;

	// Token: 0x04009D2E RID: 40238
	public const int AKCODECID_OPUS = 17;

	// Token: 0x04009D2F RID: 40239
	public const int AKCODECID_CAF = 18;

	// Token: 0x04009D30 RID: 40240
	public const int AK_WAVE_FORMAT_VAG = 65531;

	// Token: 0x04009D31 RID: 40241
	public const int AK_WAVE_FORMAT_AT9 = 65532;

	// Token: 0x04009D32 RID: 40242
	public const int AK_WAVE_FORMAT_VORBIS = 65535;

	// Token: 0x04009D33 RID: 40243
	public const int AK_WAVE_FORMAT_AAC = 43712;

	// Token: 0x04009D34 RID: 40244
	public const int AK_WAVE_FORMAT_OPUS = 12345;

	// Token: 0x04009D35 RID: 40245
	public const int WAVE_FORMAT_XMA2 = 358;

	// Token: 0x04009D36 RID: 40246
	public const int PANNER_NUM_STORAGE_BITS = 2;

	// Token: 0x04009D37 RID: 40247
	public const int POSSOURCE_NUM_STORAGE_BITS = 2;

	// Token: 0x04009D38 RID: 40248
	public const int AK_MAX_BITS_METERING_FLAGS = 5;

	// Token: 0x04009D39 RID: 40249
	public const int AK_OS_STRUCT_ALIGN = 4;

	// Token: 0x04009D3A RID: 40250
	public const int AK_64B_OS_STRUCT_ALIGN = 8;

	// Token: 0x04009D3B RID: 40251
	public const bool AK_ASYNC_OPEN_DEFAULT = false;

	// Token: 0x04009D3C RID: 40252
	public const int AK_COMM_DEFAULT_DISCOVERY_PORT = 24024;

	// Token: 0x04009D3D RID: 40253
	public const int AK_MIDI_EVENT_TYPE_INVALID = 0;

	// Token: 0x04009D3E RID: 40254
	public const int AK_MIDI_EVENT_TYPE_NOTE_OFF = 128;

	// Token: 0x04009D3F RID: 40255
	public const int AK_MIDI_EVENT_TYPE_NOTE_ON = 144;

	// Token: 0x04009D40 RID: 40256
	public const int AK_MIDI_EVENT_TYPE_NOTE_AFTERTOUCH = 160;

	// Token: 0x04009D41 RID: 40257
	public const int AK_MIDI_EVENT_TYPE_CONTROLLER = 176;

	// Token: 0x04009D42 RID: 40258
	public const int AK_MIDI_EVENT_TYPE_PROGRAM_CHANGE = 192;

	// Token: 0x04009D43 RID: 40259
	public const int AK_MIDI_EVENT_TYPE_CHANNEL_AFTERTOUCH = 208;

	// Token: 0x04009D44 RID: 40260
	public const int AK_MIDI_EVENT_TYPE_PITCH_BEND = 224;

	// Token: 0x04009D45 RID: 40261
	public const int AK_MIDI_EVENT_TYPE_SYSEX = 240;

	// Token: 0x04009D46 RID: 40262
	public const int AK_MIDI_EVENT_TYPE_ESCAPE = 247;

	// Token: 0x04009D47 RID: 40263
	public const int AK_MIDI_EVENT_TYPE_META = 255;

	// Token: 0x04009D48 RID: 40264
	public const int AK_MIDI_CC_BANK_SELECT_COARSE = 0;

	// Token: 0x04009D49 RID: 40265
	public const int AK_MIDI_CC_MOD_WHEEL_COARSE = 1;

	// Token: 0x04009D4A RID: 40266
	public const int AK_MIDI_CC_BREATH_CTRL_COARSE = 2;

	// Token: 0x04009D4B RID: 40267
	public const int AK_MIDI_CC_CTRL_3_COARSE = 3;

	// Token: 0x04009D4C RID: 40268
	public const int AK_MIDI_CC_FOOT_PEDAL_COARSE = 4;

	// Token: 0x04009D4D RID: 40269
	public const int AK_MIDI_CC_PORTAMENTO_COARSE = 5;

	// Token: 0x04009D4E RID: 40270
	public const int AK_MIDI_CC_DATA_ENTRY_COARSE = 6;

	// Token: 0x04009D4F RID: 40271
	public const int AK_MIDI_CC_VOLUME_COARSE = 7;

	// Token: 0x04009D50 RID: 40272
	public const int AK_MIDI_CC_BALANCE_COARSE = 8;

	// Token: 0x04009D51 RID: 40273
	public const int AK_MIDI_CC_CTRL_9_COARSE = 9;

	// Token: 0x04009D52 RID: 40274
	public const int AK_MIDI_CC_PAN_POSITION_COARSE = 10;

	// Token: 0x04009D53 RID: 40275
	public const int AK_MIDI_CC_EXPRESSION_COARSE = 11;

	// Token: 0x04009D54 RID: 40276
	public const int AK_MIDI_CC_EFFECT_CTRL_1_COARSE = 12;

	// Token: 0x04009D55 RID: 40277
	public const int AK_MIDI_CC_EFFECT_CTRL_2_COARSE = 13;

	// Token: 0x04009D56 RID: 40278
	public const int AK_MIDI_CC_CTRL_14_COARSE = 14;

	// Token: 0x04009D57 RID: 40279
	public const int AK_MIDI_CC_CTRL_15_COARSE = 15;

	// Token: 0x04009D58 RID: 40280
	public const int AK_MIDI_CC_GEN_SLIDER_1 = 16;

	// Token: 0x04009D59 RID: 40281
	public const int AK_MIDI_CC_GEN_SLIDER_2 = 17;

	// Token: 0x04009D5A RID: 40282
	public const int AK_MIDI_CC_GEN_SLIDER_3 = 18;

	// Token: 0x04009D5B RID: 40283
	public const int AK_MIDI_CC_GEN_SLIDER_4 = 19;

	// Token: 0x04009D5C RID: 40284
	public const int AK_MIDI_CC_CTRL_20_COARSE = 20;

	// Token: 0x04009D5D RID: 40285
	public const int AK_MIDI_CC_CTRL_21_COARSE = 21;

	// Token: 0x04009D5E RID: 40286
	public const int AK_MIDI_CC_CTRL_22_COARSE = 22;

	// Token: 0x04009D5F RID: 40287
	public const int AK_MIDI_CC_CTRL_23_COARSE = 23;

	// Token: 0x04009D60 RID: 40288
	public const int AK_MIDI_CC_CTRL_24_COARSE = 24;

	// Token: 0x04009D61 RID: 40289
	public const int AK_MIDI_CC_CTRL_25_COARSE = 25;

	// Token: 0x04009D62 RID: 40290
	public const int AK_MIDI_CC_CTRL_26_COARSE = 26;

	// Token: 0x04009D63 RID: 40291
	public const int AK_MIDI_CC_CTRL_27_COARSE = 27;

	// Token: 0x04009D64 RID: 40292
	public const int AK_MIDI_CC_CTRL_28_COARSE = 28;

	// Token: 0x04009D65 RID: 40293
	public const int AK_MIDI_CC_CTRL_29_COARSE = 29;

	// Token: 0x04009D66 RID: 40294
	public const int AK_MIDI_CC_CTRL_30_COARSE = 30;

	// Token: 0x04009D67 RID: 40295
	public const int AK_MIDI_CC_CTRL_31_COARSE = 31;

	// Token: 0x04009D68 RID: 40296
	public const int AK_MIDI_CC_BANK_SELECT_FINE = 32;

	// Token: 0x04009D69 RID: 40297
	public const int AK_MIDI_CC_MOD_WHEEL_FINE = 33;

	// Token: 0x04009D6A RID: 40298
	public const int AK_MIDI_CC_BREATH_CTRL_FINE = 34;

	// Token: 0x04009D6B RID: 40299
	public const int AK_MIDI_CC_CTRL_3_FINE = 35;

	// Token: 0x04009D6C RID: 40300
	public const int AK_MIDI_CC_FOOT_PEDAL_FINE = 36;

	// Token: 0x04009D6D RID: 40301
	public const int AK_MIDI_CC_PORTAMENTO_FINE = 37;

	// Token: 0x04009D6E RID: 40302
	public const int AK_MIDI_CC_DATA_ENTRY_FINE = 38;

	// Token: 0x04009D6F RID: 40303
	public const int AK_MIDI_CC_VOLUME_FINE = 39;

	// Token: 0x04009D70 RID: 40304
	public const int AK_MIDI_CC_BALANCE_FINE = 40;

	// Token: 0x04009D71 RID: 40305
	public const int AK_MIDI_CC_CTRL_9_FINE = 41;

	// Token: 0x04009D72 RID: 40306
	public const int AK_MIDI_CC_PAN_POSITION_FINE = 42;

	// Token: 0x04009D73 RID: 40307
	public const int AK_MIDI_CC_EXPRESSION_FINE = 43;

	// Token: 0x04009D74 RID: 40308
	public const int AK_MIDI_CC_EFFECT_CTRL_1_FINE = 44;

	// Token: 0x04009D75 RID: 40309
	public const int AK_MIDI_CC_EFFECT_CTRL_2_FINE = 45;

	// Token: 0x04009D76 RID: 40310
	public const int AK_MIDI_CC_CTRL_14_FINE = 46;

	// Token: 0x04009D77 RID: 40311
	public const int AK_MIDI_CC_CTRL_15_FINE = 47;

	// Token: 0x04009D78 RID: 40312
	public const int AK_MIDI_CC_CTRL_20_FINE = 52;

	// Token: 0x04009D79 RID: 40313
	public const int AK_MIDI_CC_CTRL_21_FINE = 53;

	// Token: 0x04009D7A RID: 40314
	public const int AK_MIDI_CC_CTRL_22_FINE = 54;

	// Token: 0x04009D7B RID: 40315
	public const int AK_MIDI_CC_CTRL_23_FINE = 55;

	// Token: 0x04009D7C RID: 40316
	public const int AK_MIDI_CC_CTRL_24_FINE = 56;

	// Token: 0x04009D7D RID: 40317
	public const int AK_MIDI_CC_CTRL_25_FINE = 57;

	// Token: 0x04009D7E RID: 40318
	public const int AK_MIDI_CC_CTRL_26_FINE = 58;

	// Token: 0x04009D7F RID: 40319
	public const int AK_MIDI_CC_CTRL_27_FINE = 59;

	// Token: 0x04009D80 RID: 40320
	public const int AK_MIDI_CC_CTRL_28_FINE = 60;

	// Token: 0x04009D81 RID: 40321
	public const int AK_MIDI_CC_CTRL_29_FINE = 61;

	// Token: 0x04009D82 RID: 40322
	public const int AK_MIDI_CC_CTRL_30_FINE = 62;

	// Token: 0x04009D83 RID: 40323
	public const int AK_MIDI_CC_CTRL_31_FINE = 63;

	// Token: 0x04009D84 RID: 40324
	public const int AK_MIDI_CC_HOLD_PEDAL = 64;

	// Token: 0x04009D85 RID: 40325
	public const int AK_MIDI_CC_PORTAMENTO_ON_OFF = 65;

	// Token: 0x04009D86 RID: 40326
	public const int AK_MIDI_CC_SUSTENUTO_PEDAL = 66;

	// Token: 0x04009D87 RID: 40327
	public const int AK_MIDI_CC_SOFT_PEDAL = 67;

	// Token: 0x04009D88 RID: 40328
	public const int AK_MIDI_CC_LEGATO_PEDAL = 68;

	// Token: 0x04009D89 RID: 40329
	public const int AK_MIDI_CC_HOLD_PEDAL_2 = 69;

	// Token: 0x04009D8A RID: 40330
	public const int AK_MIDI_CC_SOUND_VARIATION = 70;

	// Token: 0x04009D8B RID: 40331
	public const int AK_MIDI_CC_SOUND_TIMBRE = 71;

	// Token: 0x04009D8C RID: 40332
	public const int AK_MIDI_CC_SOUND_RELEASE_TIME = 72;

	// Token: 0x04009D8D RID: 40333
	public const int AK_MIDI_CC_SOUND_ATTACK_TIME = 73;

	// Token: 0x04009D8E RID: 40334
	public const int AK_MIDI_CC_SOUND_BRIGHTNESS = 74;

	// Token: 0x04009D8F RID: 40335
	public const int AK_MIDI_CC_SOUND_CTRL_6 = 75;

	// Token: 0x04009D90 RID: 40336
	public const int AK_MIDI_CC_SOUND_CTRL_7 = 76;

	// Token: 0x04009D91 RID: 40337
	public const int AK_MIDI_CC_SOUND_CTRL_8 = 77;

	// Token: 0x04009D92 RID: 40338
	public const int AK_MIDI_CC_SOUND_CTRL_9 = 78;

	// Token: 0x04009D93 RID: 40339
	public const int AK_MIDI_CC_SOUND_CTRL_10 = 79;

	// Token: 0x04009D94 RID: 40340
	public const int AK_MIDI_CC_GENERAL_BUTTON_1 = 80;

	// Token: 0x04009D95 RID: 40341
	public const int AK_MIDI_CC_GENERAL_BUTTON_2 = 81;

	// Token: 0x04009D96 RID: 40342
	public const int AK_MIDI_CC_GENERAL_BUTTON_3 = 82;

	// Token: 0x04009D97 RID: 40343
	public const int AK_MIDI_CC_GENERAL_BUTTON_4 = 83;

	// Token: 0x04009D98 RID: 40344
	public const int AK_MIDI_CC_REVERB_LEVEL = 91;

	// Token: 0x04009D99 RID: 40345
	public const int AK_MIDI_CC_TREMOLO_LEVEL = 92;

	// Token: 0x04009D9A RID: 40346
	public const int AK_MIDI_CC_CHORUS_LEVEL = 93;

	// Token: 0x04009D9B RID: 40347
	public const int AK_MIDI_CC_CELESTE_LEVEL = 94;

	// Token: 0x04009D9C RID: 40348
	public const int AK_MIDI_CC_PHASER_LEVEL = 95;

	// Token: 0x04009D9D RID: 40349
	public const int AK_MIDI_CC_DATA_BUTTON_P1 = 96;

	// Token: 0x04009D9E RID: 40350
	public const int AK_MIDI_CC_DATA_BUTTON_M1 = 97;

	// Token: 0x04009D9F RID: 40351
	public const int AK_MIDI_CC_NON_REGISTER_COARSE = 98;

	// Token: 0x04009DA0 RID: 40352
	public const int AK_MIDI_CC_NON_REGISTER_FINE = 99;

	// Token: 0x04009DA1 RID: 40353
	public const int AK_MIDI_CC_ALL_SOUND_OFF = 120;

	// Token: 0x04009DA2 RID: 40354
	public const int AK_MIDI_CC_ALL_CONTROLLERS_OFF = 121;

	// Token: 0x04009DA3 RID: 40355
	public const int AK_MIDI_CC_LOCAL_KEYBOARD = 122;

	// Token: 0x04009DA4 RID: 40356
	public const int AK_MIDI_CC_ALL_NOTES_OFF = 123;

	// Token: 0x04009DA5 RID: 40357
	public const int AK_MIDI_CC_OMNI_MODE_OFF = 124;

	// Token: 0x04009DA6 RID: 40358
	public const int AK_MIDI_CC_OMNI_MODE_ON = 125;

	// Token: 0x04009DA7 RID: 40359
	public const int AK_MIDI_CC_OMNI_MONOPHONIC_ON = 126;

	// Token: 0x04009DA8 RID: 40360
	public const int AK_MIDI_CC_OMNI_POLYPHONIC_ON = 127;

	// Token: 0x04009DA9 RID: 40361
	public const int AK_SPEAKER_FRONT_LEFT = 1;

	// Token: 0x04009DAA RID: 40362
	public const int AK_SPEAKER_FRONT_RIGHT = 2;

	// Token: 0x04009DAB RID: 40363
	public const int AK_SPEAKER_FRONT_CENTER = 4;

	// Token: 0x04009DAC RID: 40364
	public const int AK_SPEAKER_LOW_FREQUENCY = 8;

	// Token: 0x04009DAD RID: 40365
	public const int AK_SPEAKER_BACK_LEFT = 16;

	// Token: 0x04009DAE RID: 40366
	public const int AK_SPEAKER_BACK_RIGHT = 32;

	// Token: 0x04009DAF RID: 40367
	public const int AK_SPEAKER_BACK_CENTER = 256;

	// Token: 0x04009DB0 RID: 40368
	public const int AK_SPEAKER_SIDE_LEFT = 512;

	// Token: 0x04009DB1 RID: 40369
	public const int AK_SPEAKER_SIDE_RIGHT = 1024;

	// Token: 0x04009DB2 RID: 40370
	public const int AK_SPEAKER_TOP = 2048;

	// Token: 0x04009DB3 RID: 40371
	public const int AK_SPEAKER_HEIGHT_FRONT_LEFT = 4096;

	// Token: 0x04009DB4 RID: 40372
	public const int AK_SPEAKER_HEIGHT_FRONT_CENTER = 8192;

	// Token: 0x04009DB5 RID: 40373
	public const int AK_SPEAKER_HEIGHT_FRONT_RIGHT = 16384;

	// Token: 0x04009DB6 RID: 40374
	public const int AK_SPEAKER_HEIGHT_BACK_LEFT = 32768;

	// Token: 0x04009DB7 RID: 40375
	public const int AK_SPEAKER_HEIGHT_BACK_CENTER = 65536;

	// Token: 0x04009DB8 RID: 40376
	public const int AK_SPEAKER_HEIGHT_BACK_RIGHT = 131072;

	// Token: 0x04009DB9 RID: 40377
	public const int AK_SPEAKER_SETUP_MONO = 4;

	// Token: 0x04009DBA RID: 40378
	public const int AK_SPEAKER_SETUP_0POINT1 = 8;

	// Token: 0x04009DBB RID: 40379
	public const int AK_SPEAKER_SETUP_1POINT1 = 12;

	// Token: 0x04009DBC RID: 40380
	public const int AK_SPEAKER_SETUP_STEREO = 3;

	// Token: 0x04009DBD RID: 40381
	public const int AK_SPEAKER_SETUP_2POINT1 = 11;

	// Token: 0x04009DBE RID: 40382
	public const int AK_SPEAKER_SETUP_3STEREO = 7;

	// Token: 0x04009DBF RID: 40383
	public const int AK_SPEAKER_SETUP_3POINT1 = 15;

	// Token: 0x04009DC0 RID: 40384
	public const int AK_SPEAKER_SETUP_4 = 1539;

	// Token: 0x04009DC1 RID: 40385
	public const int AK_SPEAKER_SETUP_4POINT1 = 1547;

	// Token: 0x04009DC2 RID: 40386
	public const int AK_SPEAKER_SETUP_5 = 1543;

	// Token: 0x04009DC3 RID: 40387
	public const int AK_SPEAKER_SETUP_5POINT1 = 1551;

	// Token: 0x04009DC4 RID: 40388
	public const int AK_SPEAKER_SETUP_6 = 1587;

	// Token: 0x04009DC5 RID: 40389
	public const int AK_SPEAKER_SETUP_6POINT1 = 1595;

	// Token: 0x04009DC6 RID: 40390
	public const int AK_SPEAKER_SETUP_7 = 1591;

	// Token: 0x04009DC7 RID: 40391
	public const int AK_SPEAKER_SETUP_7POINT1 = 1599;

	// Token: 0x04009DC8 RID: 40392
	public const int AK_SPEAKER_SETUP_SURROUND = 259;

	// Token: 0x04009DC9 RID: 40393
	public const int AK_SPEAKER_SETUP_DPL2 = 1539;

	// Token: 0x04009DCA RID: 40394
	public const int AK_SPEAKER_SETUP_HEIGHT_4 = 184320;

	// Token: 0x04009DCB RID: 40395
	public const int AK_SPEAKER_SETUP_HEIGHT_5 = 192512;

	// Token: 0x04009DCC RID: 40396
	public const int AK_SPEAKER_SETUP_HEIGHT_ALL = 258048;

	// Token: 0x04009DCD RID: 40397
	public const int AK_SPEAKER_SETUP_AURO_222 = 22019;

	// Token: 0x04009DCE RID: 40398
	public const int AK_SPEAKER_SETUP_AURO_8 = 185859;

	// Token: 0x04009DCF RID: 40399
	public const int AK_SPEAKER_SETUP_AURO_9 = 185863;

	// Token: 0x04009DD0 RID: 40400
	public const int AK_SPEAKER_SETUP_AURO_9POINT1 = 185871;

	// Token: 0x04009DD1 RID: 40401
	public const int AK_SPEAKER_SETUP_AURO_10 = 187911;

	// Token: 0x04009DD2 RID: 40402
	public const int AK_SPEAKER_SETUP_AURO_10POINT1 = 187919;

	// Token: 0x04009DD3 RID: 40403
	public const int AK_SPEAKER_SETUP_AURO_11 = 196103;

	// Token: 0x04009DD4 RID: 40404
	public const int AK_SPEAKER_SETUP_AURO_11POINT1 = 196111;

	// Token: 0x04009DD5 RID: 40405
	public const int AK_SPEAKER_SETUP_AURO_11_740 = 185911;

	// Token: 0x04009DD6 RID: 40406
	public const int AK_SPEAKER_SETUP_AURO_11POINT1_740 = 185919;

	// Token: 0x04009DD7 RID: 40407
	public const int AK_SPEAKER_SETUP_AURO_13_751 = 196151;

	// Token: 0x04009DD8 RID: 40408
	public const int AK_SPEAKER_SETUP_AURO_13POINT1_751 = 196159;

	// Token: 0x04009DD9 RID: 40409
	public const int AK_SPEAKER_SETUP_DOLBY_5_0_2 = 22023;

	// Token: 0x04009DDA RID: 40410
	public const int AK_SPEAKER_SETUP_DOLBY_5_1_2 = 22031;

	// Token: 0x04009DDB RID: 40411
	public const int AK_SPEAKER_SETUP_DOLBY_6_0_2 = 22067;

	// Token: 0x04009DDC RID: 40412
	public const int AK_SPEAKER_SETUP_DOLBY_6_1_2 = 22075;

	// Token: 0x04009DDD RID: 40413
	public const int AK_SPEAKER_SETUP_DOLBY_6_0_4 = 185907;

	// Token: 0x04009DDE RID: 40414
	public const int AK_SPEAKER_SETUP_DOLBY_6_1_4 = 185915;

	// Token: 0x04009DDF RID: 40415
	public const int AK_SPEAKER_SETUP_DOLBY_7_0_2 = 22071;

	// Token: 0x04009DE0 RID: 40416
	public const int AK_SPEAKER_SETUP_DOLBY_7_1_2 = 22079;

	// Token: 0x04009DE1 RID: 40417
	public const int AK_SPEAKER_SETUP_DOLBY_7_0_4 = 185911;

	// Token: 0x04009DE2 RID: 40418
	public const int AK_SPEAKER_SETUP_DOLBY_7_1_4 = 185919;

	// Token: 0x04009DE3 RID: 40419
	public const int AK_SPEAKER_SETUP_ALL_SPEAKERS = 261951;

	// Token: 0x04009DE4 RID: 40420
	public const int AK_IDX_SETUP_FRONT_LEFT = 0;

	// Token: 0x04009DE5 RID: 40421
	public const int AK_IDX_SETUP_FRONT_RIGHT = 1;

	// Token: 0x04009DE6 RID: 40422
	public const int AK_IDX_SETUP_CENTER = 2;

	// Token: 0x04009DE7 RID: 40423
	public const int AK_IDX_SETUP_NOCENTER_BACK_LEFT = 2;

	// Token: 0x04009DE8 RID: 40424
	public const int AK_IDX_SETUP_NOCENTER_BACK_RIGHT = 3;

	// Token: 0x04009DE9 RID: 40425
	public const int AK_IDX_SETUP_NOCENTER_SIDE_LEFT = 4;

	// Token: 0x04009DEA RID: 40426
	public const int AK_IDX_SETUP_NOCENTER_SIDE_RIGHT = 5;

	// Token: 0x04009DEB RID: 40427
	public const int AK_IDX_SETUP_WITHCENTER_BACK_LEFT = 3;

	// Token: 0x04009DEC RID: 40428
	public const int AK_IDX_SETUP_WITHCENTER_BACK_RIGHT = 4;

	// Token: 0x04009DED RID: 40429
	public const int AK_IDX_SETUP_WITHCENTER_SIDE_LEFT = 5;

	// Token: 0x04009DEE RID: 40430
	public const int AK_IDX_SETUP_WITHCENTER_SIDE_RIGHT = 6;

	// Token: 0x04009DEF RID: 40431
	public const int AK_IDX_SETUP_0_LFE = 0;

	// Token: 0x04009DF0 RID: 40432
	public const int AK_IDX_SETUP_1_CENTER = 0;

	// Token: 0x04009DF1 RID: 40433
	public const int AK_IDX_SETUP_1_LFE = 1;

	// Token: 0x04009DF2 RID: 40434
	public const int AK_IDX_SETUP_2_LEFT = 0;

	// Token: 0x04009DF3 RID: 40435
	public const int AK_IDX_SETUP_2_RIGHT = 1;

	// Token: 0x04009DF4 RID: 40436
	public const int AK_IDX_SETUP_2_LFE = 2;

	// Token: 0x04009DF5 RID: 40437
	public const int AK_IDX_SETUP_3_LEFT = 0;

	// Token: 0x04009DF6 RID: 40438
	public const int AK_IDX_SETUP_3_RIGHT = 1;

	// Token: 0x04009DF7 RID: 40439
	public const int AK_IDX_SETUP_3_CENTER = 2;

	// Token: 0x04009DF8 RID: 40440
	public const int AK_IDX_SETUP_3_LFE = 3;

	// Token: 0x04009DF9 RID: 40441
	public const int AK_IDX_SETUP_4_FRONTLEFT = 0;

	// Token: 0x04009DFA RID: 40442
	public const int AK_IDX_SETUP_4_FRONTRIGHT = 1;

	// Token: 0x04009DFB RID: 40443
	public const int AK_IDX_SETUP_4_REARLEFT = 2;

	// Token: 0x04009DFC RID: 40444
	public const int AK_IDX_SETUP_4_REARRIGHT = 3;

	// Token: 0x04009DFD RID: 40445
	public const int AK_IDX_SETUP_4_LFE = 4;

	// Token: 0x04009DFE RID: 40446
	public const int AK_IDX_SETUP_5_FRONTLEFT = 0;

	// Token: 0x04009DFF RID: 40447
	public const int AK_IDX_SETUP_5_FRONTRIGHT = 1;

	// Token: 0x04009E00 RID: 40448
	public const int AK_IDX_SETUP_5_CENTER = 2;

	// Token: 0x04009E01 RID: 40449
	public const int AK_IDX_SETUP_5_REARLEFT = 3;

	// Token: 0x04009E02 RID: 40450
	public const int AK_IDX_SETUP_5_REARRIGHT = 4;

	// Token: 0x04009E03 RID: 40451
	public const int AK_IDX_SETUP_5_LFE = 5;

	// Token: 0x04009E04 RID: 40452
	public const int AK_IDX_SETUP_6_FRONTLEFT = 0;

	// Token: 0x04009E05 RID: 40453
	public const int AK_IDX_SETUP_6_FRONTRIGHT = 1;

	// Token: 0x04009E06 RID: 40454
	public const int AK_IDX_SETUP_6_REARLEFT = 2;

	// Token: 0x04009E07 RID: 40455
	public const int AK_IDX_SETUP_6_REARRIGHT = 3;

	// Token: 0x04009E08 RID: 40456
	public const int AK_IDX_SETUP_6_SIDELEFT = 4;

	// Token: 0x04009E09 RID: 40457
	public const int AK_IDX_SETUP_6_SIDERIGHT = 5;

	// Token: 0x04009E0A RID: 40458
	public const int AK_IDX_SETUP_6_LFE = 6;

	// Token: 0x04009E0B RID: 40459
	public const int AK_IDX_SETUP_7_FRONTLEFT = 0;

	// Token: 0x04009E0C RID: 40460
	public const int AK_IDX_SETUP_7_FRONTRIGHT = 1;

	// Token: 0x04009E0D RID: 40461
	public const int AK_IDX_SETUP_7_CENTER = 2;

	// Token: 0x04009E0E RID: 40462
	public const int AK_IDX_SETUP_7_REARLEFT = 3;

	// Token: 0x04009E0F RID: 40463
	public const int AK_IDX_SETUP_7_REARRIGHT = 4;

	// Token: 0x04009E10 RID: 40464
	public const int AK_IDX_SETUP_7_SIDELEFT = 5;

	// Token: 0x04009E11 RID: 40465
	public const int AK_IDX_SETUP_7_SIDERIGHT = 6;

	// Token: 0x04009E12 RID: 40466
	public const int AK_IDX_SETUP_7_LFE = 7;

	// Token: 0x04009E13 RID: 40467
	public const int AK_SPEAKER_SETUP_0_1 = 8;

	// Token: 0x04009E14 RID: 40468
	public const int AK_SPEAKER_SETUP_1_0_CENTER = 4;

	// Token: 0x04009E15 RID: 40469
	public const int AK_SPEAKER_SETUP_1_1_CENTER = 12;

	// Token: 0x04009E16 RID: 40470
	public const int AK_SPEAKER_SETUP_2_0 = 3;

	// Token: 0x04009E17 RID: 40471
	public const int AK_SPEAKER_SETUP_2_1 = 11;

	// Token: 0x04009E18 RID: 40472
	public const int AK_SPEAKER_SETUP_3_0 = 7;

	// Token: 0x04009E19 RID: 40473
	public const int AK_SPEAKER_SETUP_3_1 = 15;

	// Token: 0x04009E1A RID: 40474
	public const int AK_SPEAKER_SETUP_FRONT = 7;

	// Token: 0x04009E1B RID: 40475
	public const int AK_SPEAKER_SETUP_4_0 = 1539;

	// Token: 0x04009E1C RID: 40476
	public const int AK_SPEAKER_SETUP_4_1 = 1547;

	// Token: 0x04009E1D RID: 40477
	public const int AK_SPEAKER_SETUP_5_0 = 1543;

	// Token: 0x04009E1E RID: 40478
	public const int AK_SPEAKER_SETUP_5_1 = 1551;

	// Token: 0x04009E1F RID: 40479
	public const int AK_SPEAKER_SETUP_6_0 = 1587;

	// Token: 0x04009E20 RID: 40480
	public const int AK_SPEAKER_SETUP_6_1 = 1595;

	// Token: 0x04009E21 RID: 40481
	public const int AK_SPEAKER_SETUP_7_0 = 1591;

	// Token: 0x04009E22 RID: 40482
	public const int AK_SPEAKER_SETUP_7_1 = 1599;

	// Token: 0x04009E23 RID: 40483
	public const int AK_SPEAKER_SETUP_DEFAULT_PLANE = 1599;

	// Token: 0x04009E24 RID: 40484
	public const int AK_SUPPORTED_STANDARD_CHANNEL_MASK = 261951;

	// Token: 0x04009E25 RID: 40485
	public const int AK_STANDARD_MAX_NUM_CHANNELS = 8;

	// Token: 0x04009E26 RID: 40486
	public const int AK_MAX_NUM_TEXTURE = 4;

	// Token: 0x04009E27 RID: 40487
	public const int AK_MAX_REFLECT_ORDER = 4;

	// Token: 0x04009E28 RID: 40488
	public const int AK_MAX_SOUND_PROPAGATION_DEPTH = 8;

	// Token: 0x04009E29 RID: 40489
	public const double AK_DEFAULT_DIFFR_SHADOW_DEGREES = 30.0;

	// Token: 0x04009E2A RID: 40490
	public const double AK_DEFAULT_DIFFR_SHADOW_ATTEN = 2.0;

	// Token: 0x04009E2B RID: 40491
	private static AkSoundEngine.GameObjectHashFunction gameObjectHash = new AkSoundEngine.GameObjectHashFunction(AkSoundEngine.InternalGameObjectHash);

	// Token: 0x04009E2C RID: 40492
	private static readonly HashSet<ulong> RegisteredGameObjects = new HashSet<ulong>();

	// Token: 0x020018B1 RID: 6321
	// (Invoke) Token: 0x0600982B RID: 38955
	public delegate ulong GameObjectHashFunction(GameObject gameObject);

	// Token: 0x020018B2 RID: 6322
	private class AutoObject
	{
		// Token: 0x0600982E RID: 38958 RVA: 0x003EB6C4 File Offset: 0x003E98C4
		public AutoObject(GameObject go)
		{
			this.gameObject = go;
			AkSoundEngine.RegisterGameObj(this.gameObject, (!(this.gameObject != null)) ? "AkAutoObject" : ("AkAutoObject for " + this.gameObject.name));
		}

		// Token: 0x0600982F RID: 38959 RVA: 0x003EB71C File Offset: 0x003E991C
		~AutoObject()
		{
			AkSoundEngine.UnregisterGameObj(this.gameObject);
		}

		// Token: 0x04009E2F RID: 40495
		private readonly GameObject gameObject;
	}
}
