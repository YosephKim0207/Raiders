using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E0F RID: 3599
public class BraveBehaviour : MonoBehaviour
{
	// Token: 0x06004C24 RID: 19492 RVA: 0x0019FBCC File Offset: 0x0019DDCC
	public void RegenerateCache()
	{
		base.GetComponents<BraveBehaviour>(BraveBehaviour.s_braveBehaviours);
		this.m_cachedCache = new BraveCache();
		this.m_cachedCache.name = base.gameObject.name;
		for (int i = 0; i < BraveBehaviour.s_braveBehaviours.Count; i++)
		{
			BraveBehaviour.s_braveBehaviours[i].m_cachedCache = this.m_cachedCache;
		}
		BraveBehaviour.s_braveBehaviours.Clear();
	}

	// Token: 0x06004C25 RID: 19493 RVA: 0x0019FC40 File Offset: 0x0019DE40
	protected virtual void OnDestroy()
	{
		this.m_cachedCache = null;
	}

	// Token: 0x17000AAA RID: 2730
	// (get) Token: 0x06004C26 RID: 19494 RVA: 0x0019FC4C File Offset: 0x0019DE4C
	public new Transform transform
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasTransform)
			{
				cache.transform = base.GetComponent<Transform>();
				cache.hasTransform = true;
			}
			return cache.transform;
		}
	}

	// Token: 0x17000AAB RID: 2731
	// (get) Token: 0x06004C27 RID: 19495 RVA: 0x0019FC84 File Offset: 0x0019DE84
	// (set) Token: 0x06004C28 RID: 19496 RVA: 0x0019FCBC File Offset: 0x0019DEBC
	public Renderer renderer
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasRenderer)
			{
				cache.renderer = base.GetComponent<Renderer>();
				cache.hasRenderer = true;
			}
			return cache.renderer;
		}
		set
		{
			BraveCache cache = this.GetCache();
			cache.renderer = value;
			cache.hasRenderer = true;
		}
	}

	// Token: 0x17000AAC RID: 2732
	// (get) Token: 0x06004C29 RID: 19497 RVA: 0x0019FCE0 File Offset: 0x0019DEE0
	public Animation unityAnimation
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasUnityAnimation)
			{
				cache.unityAnimation = base.GetComponent<Animation>();
				cache.hasUnityAnimation = true;
			}
			return cache.unityAnimation;
		}
	}

	// Token: 0x17000AAD RID: 2733
	// (get) Token: 0x06004C2A RID: 19498 RVA: 0x0019FD18 File Offset: 0x0019DF18
	public ParticleSystem particleSystem
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasParticleSystem)
			{
				cache.particleSystem = base.GetComponent<ParticleSystem>();
				cache.hasParticleSystem = true;
			}
			return cache.particleSystem;
		}
	}

	// Token: 0x17000AAE RID: 2734
	// (get) Token: 0x06004C2B RID: 19499 RVA: 0x0019FD50 File Offset: 0x0019DF50
	public DungeonPlaceableBehaviour dungeonPlaceable
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasDungeonPlaceable)
			{
				cache.dungeonPlaceable = base.GetComponent<DungeonPlaceableBehaviour>();
				cache.hasDungeonPlaceable = true;
			}
			return cache.dungeonPlaceable;
		}
	}

	// Token: 0x17000AAF RID: 2735
	// (get) Token: 0x06004C2C RID: 19500 RVA: 0x0019FD88 File Offset: 0x0019DF88
	// (set) Token: 0x06004C2D RID: 19501 RVA: 0x0019FDC0 File Offset: 0x0019DFC0
	public AIActor aiActor
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasAiActor)
			{
				cache.aiActor = base.GetComponent<AIActor>();
				cache.hasAiActor = true;
			}
			return cache.aiActor;
		}
		set
		{
			BraveCache cache = this.GetCache();
			cache.aiActor = value;
			cache.hasAiActor = true;
		}
	}

	// Token: 0x17000AB0 RID: 2736
	// (get) Token: 0x06004C2E RID: 19502 RVA: 0x0019FDE4 File Offset: 0x0019DFE4
	public AIShooter aiShooter
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasAiShooter)
			{
				cache.aiShooter = base.GetComponent<AIShooter>();
				cache.hasAiShooter = true;
			}
			return cache.aiShooter;
		}
	}

	// Token: 0x17000AB1 RID: 2737
	// (get) Token: 0x06004C2F RID: 19503 RVA: 0x0019FE1C File Offset: 0x0019E01C
	public AIBulletBank bulletBank
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasBulletBank)
			{
				cache.bulletBank = base.GetComponent<AIBulletBank>();
				cache.hasBulletBank = true;
			}
			return cache.bulletBank;
		}
	}

	// Token: 0x17000AB2 RID: 2738
	// (get) Token: 0x06004C30 RID: 19504 RVA: 0x0019FE54 File Offset: 0x0019E054
	// (set) Token: 0x06004C31 RID: 19505 RVA: 0x0019FE8C File Offset: 0x0019E08C
	public HealthHaver healthHaver
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasHealthHaver)
			{
				cache.healthHaver = base.GetComponent<HealthHaver>();
				cache.hasHealthHaver = true;
			}
			return cache.healthHaver;
		}
		set
		{
			BraveCache cache = this.GetCache();
			cache.healthHaver = value;
			cache.hasHealthHaver = true;
		}
	}

	// Token: 0x17000AB3 RID: 2739
	// (get) Token: 0x06004C32 RID: 19506 RVA: 0x0019FEB0 File Offset: 0x0019E0B0
	public KnockbackDoer knockbackDoer
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasKnockbackDoer)
			{
				cache.knockbackDoer = base.GetComponent<KnockbackDoer>();
				cache.hasKnockbackDoer = true;
			}
			return cache.knockbackDoer;
		}
	}

	// Token: 0x17000AB4 RID: 2740
	// (get) Token: 0x06004C33 RID: 19507 RVA: 0x0019FEE8 File Offset: 0x0019E0E8
	public HitEffectHandler hitEffectHandler
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasHitEffectHandler)
			{
				cache.hitEffectHandler = base.GetComponent<HitEffectHandler>();
				cache.hasHitEffectHandler = true;
			}
			return cache.hitEffectHandler;
		}
	}

	// Token: 0x17000AB5 RID: 2741
	// (get) Token: 0x06004C34 RID: 19508 RVA: 0x0019FF20 File Offset: 0x0019E120
	// (set) Token: 0x06004C35 RID: 19509 RVA: 0x0019FF58 File Offset: 0x0019E158
	public AIAnimator aiAnimator
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasAiAnimator)
			{
				cache.aiAnimator = base.GetComponent<AIAnimator>();
				cache.hasAiAnimator = true;
			}
			return cache.aiAnimator;
		}
		set
		{
			BraveCache cache = this.GetCache();
			cache.aiAnimator = value;
			cache.hasAiAnimator = true;
		}
	}

	// Token: 0x17000AB6 RID: 2742
	// (get) Token: 0x06004C36 RID: 19510 RVA: 0x0019FF7C File Offset: 0x0019E17C
	public BehaviorSpeculator behaviorSpeculator
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasBehaviorSpeculator)
			{
				cache.behaviorSpeculator = base.GetComponent<BehaviorSpeculator>();
				cache.hasBehaviorSpeculator = true;
			}
			return cache.behaviorSpeculator;
		}
	}

	// Token: 0x17000AB7 RID: 2743
	// (get) Token: 0x06004C37 RID: 19511 RVA: 0x0019FFB4 File Offset: 0x0019E1B4
	// (set) Token: 0x06004C38 RID: 19512 RVA: 0x0019FFEC File Offset: 0x0019E1EC
	public GameActor gameActor
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasGameActor)
			{
				cache.gameActor = base.GetComponent<GameActor>();
				cache.hasGameActor = true;
			}
			return cache.gameActor;
		}
		set
		{
			BraveCache cache = this.GetCache();
			cache.gameActor = value;
			cache.hasGameActor = true;
		}
	}

	// Token: 0x17000AB8 RID: 2744
	// (get) Token: 0x06004C39 RID: 19513 RVA: 0x001A0010 File Offset: 0x0019E210
	public MinorBreakable minorBreakable
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasMinorBreakable)
			{
				cache.minorBreakable = base.GetComponent<MinorBreakable>();
				cache.hasMinorBreakable = true;
			}
			return cache.minorBreakable;
		}
	}

	// Token: 0x17000AB9 RID: 2745
	// (get) Token: 0x06004C3A RID: 19514 RVA: 0x001A0048 File Offset: 0x0019E248
	// (set) Token: 0x06004C3B RID: 19515 RVA: 0x001A0080 File Offset: 0x0019E280
	public MajorBreakable majorBreakable
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasMajorBreakable)
			{
				cache.majorBreakable = base.GetComponent<MajorBreakable>();
				cache.hasMajorBreakable = true;
			}
			return cache.majorBreakable;
		}
		set
		{
			BraveCache cache = this.GetCache();
			cache.majorBreakable = value;
			cache.hasMajorBreakable = true;
		}
	}

	// Token: 0x17000ABA RID: 2746
	// (get) Token: 0x06004C3C RID: 19516 RVA: 0x001A00A4 File Offset: 0x0019E2A4
	public Projectile projectile
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasProjectile)
			{
				cache.projectile = base.GetComponent<Projectile>();
				cache.hasProjectile = true;
			}
			return cache.projectile;
		}
	}

	// Token: 0x17000ABB RID: 2747
	// (get) Token: 0x06004C3D RID: 19517 RVA: 0x001A00DC File Offset: 0x0019E2DC
	public ObjectVisibilityManager visibilityManager
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasVisibilityManager)
			{
				cache.visibilityManager = base.GetComponent<ObjectVisibilityManager>();
				cache.hasVisibilityManager = true;
			}
			return cache.visibilityManager;
		}
	}

	// Token: 0x17000ABC RID: 2748
	// (get) Token: 0x06004C3E RID: 19518 RVA: 0x001A0114 File Offset: 0x0019E314
	public TalkDoerLite talkDoer
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasTalkDoer)
			{
				cache.talkDoer = base.GetComponent<TalkDoerLite>();
				cache.hasTalkDoer = true;
			}
			return cache.talkDoer;
		}
	}

	// Token: 0x17000ABD RID: 2749
	// (get) Token: 0x06004C3F RID: 19519 RVA: 0x001A014C File Offset: 0x0019E34C
	public UltraFortunesFavor ultraFortunesFavor
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasUltraFortunesFavor)
			{
				cache.ultraFortunesFavor = base.GetComponent<UltraFortunesFavor>();
				cache.hasUltraFortunesFavor = true;
			}
			return cache.ultraFortunesFavor;
		}
	}

	// Token: 0x17000ABE RID: 2750
	// (get) Token: 0x06004C40 RID: 19520 RVA: 0x001A0184 File Offset: 0x0019E384
	public DebrisObject debris
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasDebris)
			{
				cache.debris = base.GetComponent<DebrisObject>();
				cache.hasDebris = true;
			}
			return cache.debris;
		}
	}

	// Token: 0x17000ABF RID: 2751
	// (get) Token: 0x06004C41 RID: 19521 RVA: 0x001A01BC File Offset: 0x0019E3BC
	// (set) Token: 0x06004C42 RID: 19522 RVA: 0x001A01F4 File Offset: 0x0019E3F4
	public EncounterTrackable encounterTrackable
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasEncounterTrackable)
			{
				cache.encounterTrackable = base.GetComponent<EncounterTrackable>();
				cache.hasEncounterTrackable = true;
			}
			return cache.encounterTrackable;
		}
		set
		{
			BraveCache cache = this.GetCache();
			cache.encounterTrackable = value;
			cache.hasEncounterTrackable = true;
		}
	}

	// Token: 0x17000AC0 RID: 2752
	// (get) Token: 0x06004C43 RID: 19523 RVA: 0x001A0218 File Offset: 0x0019E418
	// (set) Token: 0x06004C44 RID: 19524 RVA: 0x001A0250 File Offset: 0x0019E450
	public SpeculativeRigidbody specRigidbody
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasSpecRigidbody)
			{
				cache.specRigidbody = base.GetComponent<SpeculativeRigidbody>();
				cache.hasSpecRigidbody = true;
			}
			return cache.specRigidbody;
		}
		set
		{
			BraveCache cache = this.GetCache();
			cache.specRigidbody = value;
			cache.hasSpecRigidbody = true;
		}
	}

	// Token: 0x17000AC1 RID: 2753
	// (get) Token: 0x06004C45 RID: 19525 RVA: 0x001A0274 File Offset: 0x0019E474
	// (set) Token: 0x06004C46 RID: 19526 RVA: 0x001A02AC File Offset: 0x0019E4AC
	public tk2dBaseSprite sprite
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasSprite)
			{
				cache.sprite = base.GetComponent<tk2dBaseSprite>();
				cache.hasSprite = true;
			}
			return cache.sprite;
		}
		set
		{
			BraveCache cache = this.GetCache();
			cache.sprite = value;
			cache.hasSprite = true;
		}
	}

	// Token: 0x17000AC2 RID: 2754
	// (get) Token: 0x06004C47 RID: 19527 RVA: 0x001A02D0 File Offset: 0x0019E4D0
	// (set) Token: 0x06004C48 RID: 19528 RVA: 0x001A0308 File Offset: 0x0019E508
	public tk2dSpriteAnimator spriteAnimator
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasSpriteAnimator)
			{
				cache.spriteAnimator = base.GetComponent<tk2dSpriteAnimator>();
				cache.hasSpriteAnimator = true;
			}
			return cache.spriteAnimator;
		}
		set
		{
			BraveCache cache = this.GetCache();
			cache.spriteAnimator = value;
			cache.hasSpriteAnimator = true;
		}
	}

	// Token: 0x17000AC3 RID: 2755
	// (get) Token: 0x06004C49 RID: 19529 RVA: 0x001A032C File Offset: 0x0019E52C
	public PlayMakerFSM playmakerFsm
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasPlaymakerFsm)
			{
				cache.playmakerFsm = base.GetComponent<PlayMakerFSM>();
				cache.hasPlaymakerFsm = true;
			}
			return cache.playmakerFsm;
		}
	}

	// Token: 0x17000AC4 RID: 2756
	// (get) Token: 0x06004C4A RID: 19530 RVA: 0x001A0364 File Offset: 0x0019E564
	public PlayMakerFSM[] playmakerFsms
	{
		get
		{
			BraveCache cache = this.GetCache();
			if (!cache.hasPlaymakerFsms)
			{
				cache.playmakerFsms = base.GetComponents<PlayMakerFSM>();
				cache.hasPlaymakerFsms = true;
			}
			return cache.playmakerFsms;
		}
	}

	// Token: 0x06004C4B RID: 19531 RVA: 0x001A039C File Offset: 0x0019E59C
	public void SendPlaymakerEvent(string eventName)
	{
		PlayMakerFSM[] playmakerFsms = this.playmakerFsms;
		for (int i = 0; i < playmakerFsms.Length; i++)
		{
			if (playmakerFsms[i].enabled)
			{
				playmakerFsms[i].SendEvent(eventName);
			}
		}
	}

	// Token: 0x06004C4C RID: 19532 RVA: 0x001A03DC File Offset: 0x0019E5DC
	public PlayMakerFSM GetDungeonFSM()
	{
		PlayMakerFSM[] playmakerFsms = this.playmakerFsms;
		for (int i = 0; i < playmakerFsms.Length; i++)
		{
			if (playmakerFsms[i].FsmName.Contains("Dungeon"))
			{
				return playmakerFsms[i];
			}
		}
		return this.playmakerFsm;
	}

	// Token: 0x06004C4D RID: 19533 RVA: 0x001A0428 File Offset: 0x0019E628
	public PlayMakerFSM GetFoyerFSM()
	{
		PlayMakerFSM[] playmakerFsms = this.playmakerFsms;
		for (int i = 0; i < playmakerFsms.Length; i++)
		{
			if (playmakerFsms[i].FsmName.Contains("Foyer"))
			{
				return playmakerFsms[i];
			}
		}
		return this.playmakerFsm;
	}

	// Token: 0x06004C4E RID: 19534 RVA: 0x001A0474 File Offset: 0x0019E674
	private BraveCache GetCache()
	{
		if (this.m_cachedCache == null)
		{
			if (BraveBehaviour.s_braveBehaviours == null)
			{
				BraveBehaviour.s_braveBehaviours = new List<BraveBehaviour>();
			}
			BraveBehaviour.s_braveBehaviours.Clear();
			base.GetComponents<BraveBehaviour>(BraveBehaviour.s_braveBehaviours);
			for (int i = 0; i < BraveBehaviour.s_braveBehaviours.Count; i++)
			{
				if (BraveBehaviour.s_braveBehaviours[i].m_cachedCache != null)
				{
					this.m_cachedCache = BraveBehaviour.s_braveBehaviours[i].m_cachedCache;
				}
			}
			if (this.m_cachedCache == null)
			{
				this.m_cachedCache = new BraveCache();
				this.m_cachedCache.name = base.gameObject.name;
				for (int j = 0; j < BraveBehaviour.s_braveBehaviours.Count; j++)
				{
					BraveBehaviour.s_braveBehaviours[j].m_cachedCache = this.m_cachedCache;
				}
			}
			BraveBehaviour.s_braveBehaviours.Clear();
		}
		return this.m_cachedCache;
	}

	// Token: 0x04004201 RID: 16897
	private BraveCache m_cachedCache;

	// Token: 0x04004202 RID: 16898
	private static List<BraveBehaviour> s_braveBehaviours = new List<BraveBehaviour>();
}
