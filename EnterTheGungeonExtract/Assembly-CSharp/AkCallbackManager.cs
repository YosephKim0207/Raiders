using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020018C0 RID: 6336
public static class AkCallbackManager
{
	// Token: 0x06009C72 RID: 40050 RVA: 0x003EC3E8 File Offset: 0x003EA5E8
	public static void RemoveEventCallback(uint in_playingID)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, AkCallbackManager.EventCallbackPackage> keyValuePair in AkCallbackManager.m_mapEventCallbacks)
		{
			if (keyValuePair.Value.m_playingID == in_playingID)
			{
				list.Add(keyValuePair.Key);
				break;
			}
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			AkCallbackManager.m_mapEventCallbacks.Remove(list[i]);
		}
		AkSoundEnginePINVOKE.CSharp_CancelEventCallback(in_playingID);
	}

	// Token: 0x06009C73 RID: 40051 RVA: 0x003EC49C File Offset: 0x003EA69C
	public static void RemoveEventCallbackCookie(object in_cookie)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, AkCallbackManager.EventCallbackPackage> keyValuePair in AkCallbackManager.m_mapEventCallbacks)
		{
			if (keyValuePair.Value.m_Cookie == in_cookie)
			{
				list.Add(keyValuePair.Key);
			}
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			int num = list[i];
			AkCallbackManager.m_mapEventCallbacks.Remove(num);
			AkSoundEnginePINVOKE.CSharp_CancelEventCallbackCookie((IntPtr)num);
		}
	}

	// Token: 0x06009C74 RID: 40052 RVA: 0x003EC558 File Offset: 0x003EA758
	public static void RemoveBankCallback(object in_cookie)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, AkCallbackManager.BankCallbackPackage> keyValuePair in AkCallbackManager.m_mapBankCallbacks)
		{
			if (keyValuePair.Value.m_Cookie == in_cookie)
			{
				list.Add(keyValuePair.Key);
			}
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			int num = list[i];
			AkCallbackManager.m_mapBankCallbacks.Remove(num);
			AkSoundEnginePINVOKE.CSharp_CancelBankCallbackCookie((IntPtr)num);
		}
	}

	// Token: 0x06009C75 RID: 40053 RVA: 0x003EC614 File Offset: 0x003EA814
	public static void SetLastAddedPlayingID(uint in_playingID)
	{
		if (AkCallbackManager.m_LastAddedEventPackage != null && AkCallbackManager.m_LastAddedEventPackage.m_playingID == 0U)
		{
			AkCallbackManager.m_LastAddedEventPackage.m_playingID = in_playingID;
		}
	}

	// Token: 0x06009C76 RID: 40054 RVA: 0x003EC63C File Offset: 0x003EA83C
	public static AKRESULT Init(int BufferSize)
	{
		AkCallbackManager.m_pNotifMem = ((BufferSize <= 0) ? IntPtr.Zero : Marshal.AllocHGlobal(BufferSize));
		return AkCallbackSerializer.Init(AkCallbackManager.m_pNotifMem, (uint)BufferSize);
	}

	// Token: 0x06009C77 RID: 40055 RVA: 0x003EC668 File Offset: 0x003EA868
	public static void Term()
	{
		if (AkCallbackManager.m_pNotifMem != IntPtr.Zero)
		{
			AkCallbackSerializer.Term();
			Marshal.FreeHGlobal(AkCallbackManager.m_pNotifMem);
			AkCallbackManager.m_pNotifMem = IntPtr.Zero;
		}
	}

	// Token: 0x06009C78 RID: 40056 RVA: 0x003EC698 File Offset: 0x003EA898
	public static void SetMonitoringCallback(AkMonitorErrorLevel in_Level, AkCallbackManager.MonitoringCallback in_CB)
	{
		AkCallbackSerializer.SetLocalOutput((uint)((in_CB == null) ? ((AkMonitorErrorLevel)0) : in_Level));
		AkCallbackManager.m_MonitoringCB = in_CB;
	}

	// Token: 0x06009C79 RID: 40057 RVA: 0x003EC6B4 File Offset: 0x003EA8B4
	public static void SetBGMCallback(AkCallbackManager.BGMCallback in_CB, object in_cookie)
	{
		AkCallbackManager.ms_sourceChangeCallbackPkg = new AkCallbackManager.BGMCallbackPackage
		{
			m_Callback = in_CB,
			m_Cookie = in_cookie
		};
	}

	// Token: 0x06009C7A RID: 40058 RVA: 0x003EC6DC File Offset: 0x003EA8DC
	public static int PostCallbacks()
	{
		if (AkCallbackManager.m_pNotifMem == IntPtr.Zero)
		{
			return 0;
		}
		int num2;
		try
		{
			int num = 0;
			IntPtr intPtr = AkCallbackSerializer.Lock();
			while (intPtr != IntPtr.Zero)
			{
				IntPtr intPtr2 = AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_pPackage_get(intPtr);
				AkCallbackType akCallbackType = (AkCallbackType)AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_eType_get(intPtr);
				IntPtr intPtr3 = AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_GetData(intPtr);
				if (akCallbackType != AkCallbackType.AK_Monitoring)
				{
					if (akCallbackType != AkCallbackType.AK_AudioInterruption)
					{
						if (akCallbackType != AkCallbackType.AK_AudioSourceChange)
						{
							if (akCallbackType != AkCallbackType.AK_Bank)
							{
								AkCallbackManager.EventCallbackPackage eventCallbackPackage = null;
								if (!AkCallbackManager.m_mapEventCallbacks.TryGetValue((int)intPtr2, out eventCallbackPackage))
								{
									Debug.LogError("WwiseUnity: EventCallbackPackage not found for <" + intPtr2 + ">.");
									return num;
								}
								AkCallbackInfo akCallbackInfo = null;
								switch (akCallbackType)
								{
								case AkCallbackType.AK_EndOfEvent:
									AkCallbackManager.m_mapEventCallbacks.Remove(eventCallbackPackage.GetHashCode());
									if (eventCallbackPackage.m_bNotifyEndOfEvent)
									{
										AkCallbackManager.AkEventCallbackInfo.setCPtr(intPtr3);
										akCallbackInfo = AkCallbackManager.AkEventCallbackInfo;
									}
									break;
								case AkCallbackType.AK_EndOfDynamicSequenceItem:
									AkCallbackManager.AkDynamicSequenceItemCallbackInfo.setCPtr(intPtr3);
									akCallbackInfo = AkCallbackManager.AkDynamicSequenceItemCallbackInfo;
									break;
								default:
									if (akCallbackType != AkCallbackType.AK_MusicPlaylistSelect)
									{
										if (akCallbackType != AkCallbackType.AK_MusicPlayStarted)
										{
											if (akCallbackType != AkCallbackType.AK_MusicSyncBeat && akCallbackType != AkCallbackType.AK_MusicSyncBar && akCallbackType != AkCallbackType.AK_MusicSyncEntry && akCallbackType != AkCallbackType.AK_MusicSyncExit && akCallbackType != AkCallbackType.AK_MusicSyncGrid && akCallbackType != AkCallbackType.AK_MusicSyncUserCue && akCallbackType != AkCallbackType.AK_MusicSyncPoint)
											{
												if (akCallbackType != AkCallbackType.AK_MIDIEvent)
												{
													Debug.LogError("WwiseUnity: PostCallbacks aborted due to error: Undefined callback type <" + akCallbackType + "> found. Callback object possibly corrupted.");
													return num;
												}
												AkCallbackManager.AkMIDIEventCallbackInfo.setCPtr(intPtr3);
												akCallbackInfo = AkCallbackManager.AkMIDIEventCallbackInfo;
											}
											else
											{
												AkCallbackManager.AkMusicSyncCallbackInfo.setCPtr(intPtr3);
												akCallbackInfo = AkCallbackManager.AkMusicSyncCallbackInfo;
											}
										}
										else
										{
											AkCallbackManager.AkEventCallbackInfo.setCPtr(intPtr3);
											akCallbackInfo = AkCallbackManager.AkEventCallbackInfo;
										}
									}
									else
									{
										AkCallbackManager.AkMusicPlaylistCallbackInfo.setCPtr(intPtr3);
										akCallbackInfo = AkCallbackManager.AkMusicPlaylistCallbackInfo;
									}
									break;
								case AkCallbackType.AK_Marker:
									AkCallbackManager.AkMarkerCallbackInfo.setCPtr(intPtr3);
									akCallbackInfo = AkCallbackManager.AkMarkerCallbackInfo;
									break;
								case AkCallbackType.AK_Duration:
									AkCallbackManager.AkDurationCallbackInfo.setCPtr(intPtr3);
									akCallbackInfo = AkCallbackManager.AkDurationCallbackInfo;
									break;
								}
								if (akCallbackInfo != null)
								{
									eventCallbackPackage.m_Callback(eventCallbackPackage.m_Cookie, akCallbackType, akCallbackInfo);
								}
							}
							else
							{
								AkCallbackManager.BankCallbackPackage bankCallbackPackage = null;
								if (!AkCallbackManager.m_mapBankCallbacks.TryGetValue((int)intPtr2, out bankCallbackPackage))
								{
									Debug.LogError("WwiseUnity: BankCallbackPackage not found for <" + intPtr2 + ">.");
									return num;
								}
								AkCallbackManager.m_mapBankCallbacks.Remove((int)intPtr2);
								if (bankCallbackPackage != null && bankCallbackPackage.m_Callback != null)
								{
									AkCallbackManager.AkBankCallbackInfo.setCPtr(intPtr3);
									bankCallbackPackage.m_Callback(AkCallbackManager.AkBankCallbackInfo.bankID, AkCallbackManager.AkBankCallbackInfo.inMemoryBankPtr, AkCallbackManager.AkBankCallbackInfo.loadResult, (uint)AkCallbackManager.AkBankCallbackInfo.memPoolId, bankCallbackPackage.m_Cookie);
								}
							}
						}
						else if (AkCallbackManager.ms_sourceChangeCallbackPkg != null && AkCallbackManager.ms_sourceChangeCallbackPkg.m_Callback != null)
						{
							AkCallbackManager.AkAudioSourceChangeCallbackInfo.setCPtr(intPtr3);
							AkCallbackManager.ms_sourceChangeCallbackPkg.m_Callback(AkCallbackManager.AkAudioSourceChangeCallbackInfo.bOtherAudioPlaying, AkCallbackManager.ms_sourceChangeCallbackPkg.m_Cookie);
						}
					}
				}
				else if (AkCallbackManager.m_MonitoringCB != null)
				{
					AkCallbackManager.AkMonitoringCallbackInfo.setCPtr(intPtr3);
					AkCallbackManager.m_MonitoringCB(AkCallbackManager.AkMonitoringCallbackInfo.errorCode, AkCallbackManager.AkMonitoringCallbackInfo.errorLevel, AkCallbackManager.AkMonitoringCallbackInfo.playingID, AkCallbackManager.AkMonitoringCallbackInfo.gameObjID, AkCallbackManager.AkMonitoringCallbackInfo.message);
				}
				intPtr = AkSoundEnginePINVOKE.CSharp_AkSerializedCallbackHeader_pNext_get(intPtr);
				num++;
			}
			num2 = num;
		}
		finally
		{
			AkCallbackSerializer.Unlock();
		}
		return num2;
	}

	// Token: 0x04009E4C RID: 40524
	private static readonly AkEventCallbackInfo AkEventCallbackInfo = new AkEventCallbackInfo(IntPtr.Zero, false);

	// Token: 0x04009E4D RID: 40525
	private static readonly AkDynamicSequenceItemCallbackInfo AkDynamicSequenceItemCallbackInfo = new AkDynamicSequenceItemCallbackInfo(IntPtr.Zero, false);

	// Token: 0x04009E4E RID: 40526
	private static readonly AkMIDIEventCallbackInfo AkMIDIEventCallbackInfo = new AkMIDIEventCallbackInfo(IntPtr.Zero, false);

	// Token: 0x04009E4F RID: 40527
	private static readonly AkMarkerCallbackInfo AkMarkerCallbackInfo = new AkMarkerCallbackInfo(IntPtr.Zero, false);

	// Token: 0x04009E50 RID: 40528
	private static readonly AkDurationCallbackInfo AkDurationCallbackInfo = new AkDurationCallbackInfo(IntPtr.Zero, false);

	// Token: 0x04009E51 RID: 40529
	private static readonly AkMusicSyncCallbackInfo AkMusicSyncCallbackInfo = new AkMusicSyncCallbackInfo(IntPtr.Zero, false);

	// Token: 0x04009E52 RID: 40530
	private static readonly AkMusicPlaylistCallbackInfo AkMusicPlaylistCallbackInfo = new AkMusicPlaylistCallbackInfo(IntPtr.Zero, false);

	// Token: 0x04009E53 RID: 40531
	private static readonly AkAudioSourceChangeCallbackInfo AkAudioSourceChangeCallbackInfo = new AkAudioSourceChangeCallbackInfo(IntPtr.Zero, false);

	// Token: 0x04009E54 RID: 40532
	private static readonly AkMonitoringCallbackInfo AkMonitoringCallbackInfo = new AkMonitoringCallbackInfo(IntPtr.Zero, false);

	// Token: 0x04009E55 RID: 40533
	private static readonly AkBankCallbackInfo AkBankCallbackInfo = new AkBankCallbackInfo(IntPtr.Zero, false);

	// Token: 0x04009E56 RID: 40534
	private static readonly Dictionary<int, AkCallbackManager.EventCallbackPackage> m_mapEventCallbacks = new Dictionary<int, AkCallbackManager.EventCallbackPackage>();

	// Token: 0x04009E57 RID: 40535
	private static readonly Dictionary<int, AkCallbackManager.BankCallbackPackage> m_mapBankCallbacks = new Dictionary<int, AkCallbackManager.BankCallbackPackage>();

	// Token: 0x04009E58 RID: 40536
	private static AkCallbackManager.EventCallbackPackage m_LastAddedEventPackage;

	// Token: 0x04009E59 RID: 40537
	private static IntPtr m_pNotifMem = IntPtr.Zero;

	// Token: 0x04009E5A RID: 40538
	private static AkCallbackManager.MonitoringCallback m_MonitoringCB;

	// Token: 0x04009E5B RID: 40539
	private static AkCallbackManager.BGMCallbackPackage ms_sourceChangeCallbackPkg;

	// Token: 0x020018C1 RID: 6337
	// (Invoke) Token: 0x06009C7D RID: 40061
	public delegate void EventCallback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info);

	// Token: 0x020018C2 RID: 6338
	// (Invoke) Token: 0x06009C81 RID: 40065
	public delegate void MonitoringCallback(AkMonitorErrorCode in_errorCode, AkMonitorErrorLevel in_errorLevel, uint in_playingID, ulong in_gameObjID, string in_msg);

	// Token: 0x020018C3 RID: 6339
	// (Invoke) Token: 0x06009C85 RID: 40069
	public delegate void BankCallback(uint in_bankID, IntPtr in_InMemoryBankPtr, AKRESULT in_eLoadResult, uint in_memPoolId, object in_Cookie);

	// Token: 0x020018C4 RID: 6340
	public class EventCallbackPackage
	{
		// Token: 0x06009C89 RID: 40073 RVA: 0x003ECBC0 File Offset: 0x003EADC0
		public static AkCallbackManager.EventCallbackPackage Create(AkCallbackManager.EventCallback in_cb, object in_cookie, ref uint io_Flags)
		{
			if (io_Flags == 0U || in_cb == null)
			{
				io_Flags = 0U;
				return null;
			}
			AkCallbackManager.EventCallbackPackage eventCallbackPackage = new AkCallbackManager.EventCallbackPackage();
			eventCallbackPackage.m_Callback = in_cb;
			eventCallbackPackage.m_Cookie = in_cookie;
			eventCallbackPackage.m_bNotifyEndOfEvent = (io_Flags & 1U) != 0U;
			io_Flags |= 1U;
			AkCallbackManager.m_mapEventCallbacks[eventCallbackPackage.GetHashCode()] = eventCallbackPackage;
			AkCallbackManager.m_LastAddedEventPackage = eventCallbackPackage;
			return eventCallbackPackage;
		}

		// Token: 0x06009C8A RID: 40074 RVA: 0x003ECC24 File Offset: 0x003EAE24
		~EventCallbackPackage()
		{
			if (this.m_Cookie != null)
			{
				AkCallbackManager.RemoveEventCallbackCookie(this.m_Cookie);
			}
		}

		// Token: 0x04009E5C RID: 40540
		public bool m_bNotifyEndOfEvent;

		// Token: 0x04009E5D RID: 40541
		public AkCallbackManager.EventCallback m_Callback;

		// Token: 0x04009E5E RID: 40542
		public object m_Cookie;

		// Token: 0x04009E5F RID: 40543
		public uint m_playingID;
	}

	// Token: 0x020018C5 RID: 6341
	public class BankCallbackPackage
	{
		// Token: 0x06009C8B RID: 40075 RVA: 0x003ECC64 File Offset: 0x003EAE64
		public BankCallbackPackage(AkCallbackManager.BankCallback in_cb, object in_cookie)
		{
			this.m_Callback = in_cb;
			this.m_Cookie = in_cookie;
			AkCallbackManager.m_mapBankCallbacks[this.GetHashCode()] = this;
		}

		// Token: 0x04009E60 RID: 40544
		public AkCallbackManager.BankCallback m_Callback;

		// Token: 0x04009E61 RID: 40545
		public object m_Cookie;
	}

	// Token: 0x020018C6 RID: 6342
	// (Invoke) Token: 0x06009C8D RID: 40077
	public delegate AKRESULT BGMCallback(bool in_bOtherAudioPlaying, object in_Cookie);

	// Token: 0x020018C7 RID: 6343
	public class BGMCallbackPackage
	{
		// Token: 0x04009E62 RID: 40546
		public AkCallbackManager.BGMCallback m_Callback;

		// Token: 0x04009E63 RID: 40547
		public object m_Cookie;
	}
}
