using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

// Token: 0x020018C9 RID: 6345
public class AkLogger
{
	// Token: 0x06009C98 RID: 40088 RVA: 0x003ECF9C File Offset: 0x003EB19C
	private AkLogger()
	{
		if (AkLogger.ms_Instance == null)
		{
			AkLogger.ms_Instance = this;
			AkSoundEngine.SetErrorLogger(this.errorLoggerDelegate);
		}
	}

	// Token: 0x170016F5 RID: 5877
	// (get) Token: 0x06009C99 RID: 40089 RVA: 0x003ECFF0 File Offset: 0x003EB1F0
	public static AkLogger Instance
	{
		get
		{
			return AkLogger.ms_Instance;
		}
	}

	// Token: 0x06009C9A RID: 40090 RVA: 0x003ECFF8 File Offset: 0x003EB1F8
	~AkLogger()
	{
		if (AkLogger.ms_Instance == this)
		{
			AkLogger.ms_Instance = null;
			this.errorLoggerDelegate = null;
			AkSoundEngine.SetErrorLogger();
		}
	}

	// Token: 0x06009C9B RID: 40091 RVA: 0x003ED040 File Offset: 0x003EB240
	public void Init()
	{
	}

	// Token: 0x06009C9C RID: 40092 RVA: 0x003ED044 File Offset: 0x003EB244
	[AOT.MonoPInvokeCallback(typeof(AkLogger.ErrorLoggerInteropDelegate))]
	public static void WwiseInternalLogError(string message)
	{
		Debug.LogError("Wwise: " + message);
	}

	// Token: 0x06009C9D RID: 40093 RVA: 0x003ED058 File Offset: 0x003EB258
	public static void Message(string message)
	{
		Debug.Log("WwiseUnity: " + message);
	}

	// Token: 0x06009C9E RID: 40094 RVA: 0x003ED06C File Offset: 0x003EB26C
	public static void Warning(string message)
	{
		Debug.LogWarning("WwiseUnity: " + message);
	}

	// Token: 0x06009C9F RID: 40095 RVA: 0x003ED080 File Offset: 0x003EB280
	public static void Error(string message)
	{
		Debug.LogError("WwiseUnity: " + message);
	}

	// Token: 0x04009E68 RID: 40552
	private static AkLogger ms_Instance = new AkLogger();

	// Token: 0x04009E69 RID: 40553
	private AkLogger.ErrorLoggerInteropDelegate errorLoggerDelegate = new AkLogger.ErrorLoggerInteropDelegate(AkLogger.WwiseInternalLogError);

	// Token: 0x020018CA RID: 6346
	// (Invoke) Token: 0x06009CA2 RID: 40098
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void ErrorLoggerInteropDelegate([MarshalAs(UnmanagedType.LPStr)] string message);
}
