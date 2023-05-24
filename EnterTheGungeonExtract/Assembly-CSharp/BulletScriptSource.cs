using System;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x0200033D RID: 829
public class BulletScriptSource : BraveBehaviour
{
	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06000CDE RID: 3294 RVA: 0x0003DB70 File Offset: 0x0003BD70
	// (set) Token: 0x06000CDF RID: 3295 RVA: 0x0003DB78 File Offset: 0x0003BD78
	public Bullet RootBullet { get; set; }

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x0003DB84 File Offset: 0x0003BD84
	// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x0003DB8C File Offset: 0x0003BD8C
	public AIBulletBank BulletManager { get; set; }

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x0003DB98 File Offset: 0x0003BD98
	// (set) Token: 0x06000CE3 RID: 3299 RVA: 0x0003DBA0 File Offset: 0x0003BDA0
	public bool FreezeTopPosition { get; set; }

	// Token: 0x06000CE4 RID: 3300 RVA: 0x0003DBAC File Offset: 0x0003BDAC
	public void Awake()
	{
		StaticReferenceManager.AllBulletScriptSources.Add(this);
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x0003DBBC File Offset: 0x0003BDBC
	public void Start()
	{
		if (!this.BulletManager)
		{
			this.BulletManager = base.bulletBank;
		}
		if (this.RootBullet == null)
		{
			this.Initialize();
		}
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x0003DBEC File Offset: 0x0003BDEC
	public void Update()
	{
		if (this.RootBullet == null || this.RootBullet.IsEnded)
		{
			return;
		}
		if (this.m_lastUpdatedFrame == Time.frameCount)
		{
			return;
		}
		if (!this.FreezeTopPosition)
		{
			this.RootBullet.Position = base.transform.position.XY();
			this.RootBullet.Direction = base.transform.rotation.eulerAngles.z;
		}
		this.RootBullet.TimeScale = this.BulletManager.TimeScale;
		this.RootBullet.FrameUpdate();
		this.m_lastUpdatedFrame = Time.frameCount;
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x0003DCA0 File Offset: 0x0003BEA0
	protected override void OnDestroy()
	{
		StaticReferenceManager.AllBulletScriptSources.Remove(this);
		base.OnDestroy();
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0003DCB4 File Offset: 0x0003BEB4
	public void ForceStop()
	{
		if (this.RootBullet != null)
		{
			this.RootBullet.ForceEnd();
			this.RootBullet.Destroyed = true;
			this.RootBullet = null;
		}
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x0003DCE0 File Offset: 0x0003BEE0
	public void Initialize()
	{
		this.RootBullet = this.BulletScript.CreateInstance();
		if (this.RootBullet != null)
		{
			this.RootBullet.BulletManager = this.BulletManager;
			this.RootBullet.RootTransform = base.transform;
			this.RootBullet.Position = base.transform.position.XY();
			this.RootBullet.Direction = base.transform.rotation.eulerAngles.z;
			this.RootBullet.Initialize();
		}
		this.Update();
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06000CEA RID: 3306 RVA: 0x0003DD80 File Offset: 0x0003BF80
	public bool IsEnded
	{
		get
		{
			return this.RootBullet == null || this.RootBullet.IsEnded;
		}
	}

	// Token: 0x04000D87 RID: 3463
	public BulletScriptSelector BulletScript;

	// Token: 0x04000D8B RID: 3467
	private int m_lastUpdatedFrame = -1;
}
