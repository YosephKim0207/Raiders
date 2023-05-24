using System;
using Galaxy.Api;
using UnityEngine;

// Token: 0x020012E2 RID: 4834
[DisallowMultipleComponent]
public class GalaxyManager : MonoBehaviour
{
	// Token: 0x17001013 RID: 4115
	// (get) Token: 0x06006C4F RID: 27727 RVA: 0x002AA15C File Offset: 0x002A835C
	private static GalaxyManager Instance
	{
		get
		{
			return GalaxyManager.s_instance ?? new GameObject("GalaxyManager").AddComponent<GalaxyManager>();
		}
	}

	// Token: 0x17001014 RID: 4116
	// (get) Token: 0x06006C50 RID: 27728 RVA: 0x002AA17C File Offset: 0x002A837C
	// (set) Token: 0x06006C51 RID: 27729 RVA: 0x002AA188 File Offset: 0x002A8388
	public static bool Initialized
	{
		get
		{
			return GalaxyManager.Instance.m_bInitialized;
		}
		private set
		{
			GalaxyManager.Instance.m_bInitialized = value;
			if (value)
			{
				GalaxyManager.s_EverInialized = true;
			}
		}
	}

	// Token: 0x06006C52 RID: 27730 RVA: 0x002AA1A4 File Offset: 0x002A83A4
	private void Awake()
	{
		if (GalaxyManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GalaxyManager.s_instance = this;
		if (GalaxyManager.s_EverInialized)
		{
			throw new Exception("Tried to Initialize the Galaxy API twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		try
		{
			GalaxyInstance.Init("48944359584830756", "3847f0113681121feddcd75acdcfcde13320be288b24f33b003821c9e776737d");
			this.m_authListener = new GalaxyManager.AuthListener();
			GalaxyInstance.User().SignIn();
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
			try
			{
				Debug.LogError("GalaxyManager failed to start; attempting shut down.");
				GalaxyInstance.Shutdown();
			}
			catch (Exception)
			{
				Debug.LogError(ex);
			}
		}
	}

	// Token: 0x06006C53 RID: 27731 RVA: 0x002AA264 File Offset: 0x002A8464
	private void OnEnable()
	{
		if (GalaxyManager.s_instance == null)
		{
			GalaxyManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
	}

	// Token: 0x06006C54 RID: 27732 RVA: 0x002AA288 File Offset: 0x002A8488
	private void OnDestroy()
	{
		if (GalaxyManager.s_instance != this)
		{
			return;
		}
		GalaxyManager.s_instance = null;
		if (!this.m_bInitialized)
		{
		}
		GalaxyInstance.Shutdown();
	}

	// Token: 0x06006C55 RID: 27733 RVA: 0x002AA2B4 File Offset: 0x002A84B4
	private void Update()
	{
		GalaxyInstance.ProcessData();
		if (!this.m_bInitialized)
		{
			return;
		}
	}

	// Token: 0x0400695B RID: 26971
	private static GalaxyManager s_instance;

	// Token: 0x0400695C RID: 26972
	private static bool s_EverInialized;

	// Token: 0x0400695D RID: 26973
	private bool m_bInitialized;

	// Token: 0x0400695E RID: 26974
	private GlobalAuthListener m_authListener;

	// Token: 0x020012E3 RID: 4835
	public class AuthListener : GlobalAuthListener
	{
		// Token: 0x06006C57 RID: 27735 RVA: 0x002AA2D0 File Offset: 0x002A84D0
		public override void OnAuthSuccess()
		{
			Debug.Log("Auth success!");
			GalaxyManager.Initialized = true;
		}

		// Token: 0x06006C58 RID: 27736 RVA: 0x002AA2E4 File Offset: 0x002A84E4
		public override void OnAuthFailure(IAuthListener.FailureReason failureReason)
		{
			Debug.LogFormat("Auth failed! {0}", new object[] { failureReason });
		}

		// Token: 0x06006C59 RID: 27737 RVA: 0x002AA300 File Offset: 0x002A8500
		public override void OnAuthLost()
		{
			Debug.LogFormat("Auth lost!", new object[0]);
		}
	}
}
