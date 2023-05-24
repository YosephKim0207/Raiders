using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AK.Wwise;
using AOT;
using UnityEngine;

// Token: 0x020018B5 RID: 6325
public static class AkAudioInputManager
{
	// Token: 0x06009C2E RID: 39982 RVA: 0x003EB8B4 File Offset: 0x003E9AB4
	public static uint PostAudioInputEvent(AK.Wwise.Event akEvent, GameObject gameObject, AkAudioInputManager.AudioSamplesDelegate sampleDelegate, AkAudioInputManager.AudioFormatDelegate formatDelegate = null)
	{
		AkAudioInputManager.TryInitialize();
		uint num = akEvent.Post(gameObject, 1U, new AkCallbackManager.EventCallback(AkAudioInputManager.EventCallback), null);
		AkAudioInputManager.AddPlayingID(num, sampleDelegate, formatDelegate);
		return num;
	}

	// Token: 0x06009C2F RID: 39983 RVA: 0x003EB8F8 File Offset: 0x003E9AF8
	public static uint PostAudioInputEvent(uint akEventID, GameObject gameObject, AkAudioInputManager.AudioSamplesDelegate sampleDelegate, AkAudioInputManager.AudioFormatDelegate formatDelegate = null)
	{
		AkAudioInputManager.TryInitialize();
		uint num = AkSoundEngine.PostEvent(akEventID, gameObject, 1U, new AkCallbackManager.EventCallback(AkAudioInputManager.EventCallback), null);
		AkAudioInputManager.AddPlayingID(num, sampleDelegate, formatDelegate);
		return num;
	}

	// Token: 0x06009C30 RID: 39984 RVA: 0x003EB93C File Offset: 0x003E9B3C
	public static uint PostAudioInputEvent(string akEventName, GameObject gameObject, AkAudioInputManager.AudioSamplesDelegate sampleDelegate, AkAudioInputManager.AudioFormatDelegate formatDelegate = null)
	{
		AkAudioInputManager.TryInitialize();
		uint num = AkSoundEngine.PostEvent(akEventName, gameObject, 1U, new AkCallbackManager.EventCallback(AkAudioInputManager.EventCallback), null);
		AkAudioInputManager.AddPlayingID(num, sampleDelegate, formatDelegate);
		return num;
	}

	// Token: 0x06009C31 RID: 39985 RVA: 0x003EB980 File Offset: 0x003E9B80
	[AOT.MonoPInvokeCallback(typeof(AkAudioInputManager.AudioSamplesInteropDelegate))]
	private static bool InternalAudioSamplesDelegate(uint playingID, float[] samples, uint channelIndex, uint frames)
	{
		return AkAudioInputManager.audioSamplesDelegates.ContainsKey(playingID) && AkAudioInputManager.audioSamplesDelegates[playingID](playingID, channelIndex, samples);
	}

	// Token: 0x06009C32 RID: 39986 RVA: 0x003EB9A8 File Offset: 0x003E9BA8
	[AOT.MonoPInvokeCallback(typeof(AkAudioInputManager.AudioFormatInteropDelegate))]
	private static void InternalAudioFormatDelegate(uint playingID, IntPtr format)
	{
		if (AkAudioInputManager.audioFormatDelegates.ContainsKey(playingID))
		{
			AkAudioInputManager.audioFormat.setCPtr(format);
			AkAudioInputManager.audioFormatDelegates[playingID](playingID, AkAudioInputManager.audioFormat);
		}
	}

	// Token: 0x06009C33 RID: 39987 RVA: 0x003EB9DC File Offset: 0x003E9BDC
	private static void TryInitialize()
	{
		if (!AkAudioInputManager.initialized)
		{
			AkAudioInputManager.initialized = true;
			AkSoundEngine.SetAudioInputCallbacks(AkAudioInputManager.audioSamplesDelegate, AkAudioInputManager.audioFormatDelegate);
		}
	}

	// Token: 0x06009C34 RID: 39988 RVA: 0x003EBA00 File Offset: 0x003E9C00
	private static void AddPlayingID(uint playingID, AkAudioInputManager.AudioSamplesDelegate sampleDelegate, AkAudioInputManager.AudioFormatDelegate formatDelegate)
	{
		if (playingID == 0U || sampleDelegate == null)
		{
			return;
		}
		AkAudioInputManager.audioSamplesDelegates.Add(playingID, sampleDelegate);
		if (formatDelegate != null)
		{
			AkAudioInputManager.audioFormatDelegates.Add(playingID, formatDelegate);
		}
	}

	// Token: 0x06009C35 RID: 39989 RVA: 0x003EBA30 File Offset: 0x003E9C30
	private static void EventCallback(object cookie, AkCallbackType type, AkCallbackInfo callbackInfo)
	{
		if (type == AkCallbackType.AK_EndOfEvent)
		{
			AkEventCallbackInfo akEventCallbackInfo = callbackInfo as AkEventCallbackInfo;
			if (akEventCallbackInfo != null)
			{
				AkAudioInputManager.audioSamplesDelegates.Remove(akEventCallbackInfo.playingID);
				AkAudioInputManager.audioFormatDelegates.Remove(akEventCallbackInfo.playingID);
			}
		}
	}

	// Token: 0x04009E32 RID: 40498
	private static bool initialized;

	// Token: 0x04009E33 RID: 40499
	private static readonly Dictionary<uint, AkAudioInputManager.AudioSamplesDelegate> audioSamplesDelegates = new Dictionary<uint, AkAudioInputManager.AudioSamplesDelegate>();

	// Token: 0x04009E34 RID: 40500
	private static readonly Dictionary<uint, AkAudioInputManager.AudioFormatDelegate> audioFormatDelegates = new Dictionary<uint, AkAudioInputManager.AudioFormatDelegate>();

	// Token: 0x04009E35 RID: 40501
	private static readonly AkAudioFormat audioFormat = new AkAudioFormat();

	// Token: 0x04009E36 RID: 40502
	private static readonly AkAudioInputManager.AudioSamplesInteropDelegate audioSamplesDelegate = new AkAudioInputManager.AudioSamplesInteropDelegate(AkAudioInputManager.InternalAudioSamplesDelegate);

	// Token: 0x04009E37 RID: 40503
	private static readonly AkAudioInputManager.AudioFormatInteropDelegate audioFormatDelegate = new AkAudioInputManager.AudioFormatInteropDelegate(AkAudioInputManager.InternalAudioFormatDelegate);

	// Token: 0x020018B6 RID: 6326
	// (Invoke) Token: 0x06009C38 RID: 39992
	public delegate void AudioFormatDelegate(uint playingID, AkAudioFormat format);

	// Token: 0x020018B7 RID: 6327
	// (Invoke) Token: 0x06009C3C RID: 39996
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void AudioFormatInteropDelegate(uint playingID, IntPtr format);

	// Token: 0x020018B8 RID: 6328
	// (Invoke) Token: 0x06009C40 RID: 40000
	public delegate bool AudioSamplesDelegate(uint playingID, uint channelIndex, float[] samples);

	// Token: 0x020018B9 RID: 6329
	// (Invoke) Token: 0x06009C44 RID: 40004
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate bool AudioSamplesInteropDelegate(uint playingID, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] [In] [Out] float[] samples, uint channelIndex, uint frames);
}
