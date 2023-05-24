using System;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x020016D2 RID: 5842
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x1700144C RID: 5196
	// (get) Token: 0x060087E8 RID: 34792 RVA: 0x00385CBC File Offset: 0x00383EBC
	private static SteamManager Instance
	{
		get
		{
			return SteamManager.s_instance ?? new GameObject("SteamManager").AddComponent<SteamManager>();
		}
	}

	// Token: 0x1700144D RID: 5197
	// (get) Token: 0x060087E9 RID: 34793 RVA: 0x00385CDC File Offset: 0x00383EDC
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x060087EA RID: 34794 RVA: 0x00385CE8 File Offset: 0x00383EE8
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x060087EB RID: 34795 RVA: 0x00385CF0 File Offset: 0x00383EF0
	private void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary((AppId_t)311690U))
			{
				SaveManager.PreventMidgameSaveDeletionOnExit = true;
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex, this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInialized = true;
	}

	// Token: 0x060087EC RID: 34796 RVA: 0x00385DE8 File Offset: 0x00383FE8
	private void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x060087ED RID: 34797 RVA: 0x00385E40 File Offset: 0x00384040
	private void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x060087EE RID: 34798 RVA: 0x00385E6C File Offset: 0x0038406C
	private void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x04008D20 RID: 36128
	private static SteamManager s_instance;

	// Token: 0x04008D21 RID: 36129
	private static bool s_EverInialized;

	// Token: 0x04008D22 RID: 36130
	private bool m_bInitialized;

	// Token: 0x04008D23 RID: 36131
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
