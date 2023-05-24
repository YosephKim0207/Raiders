using System;
using UnityEngine;

// Token: 0x02000BA4 RID: 2980
[AddComponentMenu("2D Toolkit/Sprite/tk2dAnimatedSprite (Obsolete)")]
public class tk2dAnimatedSprite : tk2dSprite
{
	// Token: 0x1700096C RID: 2412
	// (get) Token: 0x06003E78 RID: 15992 RVA: 0x0013BFA8 File Offset: 0x0013A1A8
	public tk2dSpriteAnimator Animator
	{
		get
		{
			this.CheckAddAnimatorInternal();
			return this._animator;
		}
	}

	// Token: 0x06003E79 RID: 15993 RVA: 0x0013BFB8 File Offset: 0x0013A1B8
	private void CheckAddAnimatorInternal()
	{
		if (this._animator == null)
		{
			this._animator = base.gameObject.GetComponent<tk2dSpriteAnimator>();
			if (this._animator == null)
			{
				this._animator = base.gameObject.AddComponent<tk2dSpriteAnimator>();
				this._animator.Library = this.anim;
				this._animator.DefaultClipId = this.clipId;
				this._animator.playAutomatically = this.playAutomatically;
			}
		}
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x0013C03C File Offset: 0x0013A23C
	protected override bool NeedBoxCollider()
	{
		return this.createCollider;
	}

	// Token: 0x1700096D RID: 2413
	// (get) Token: 0x06003E7B RID: 15995 RVA: 0x0013C044 File Offset: 0x0013A244
	// (set) Token: 0x06003E7C RID: 15996 RVA: 0x0013C054 File Offset: 0x0013A254
	public tk2dSpriteAnimation Library
	{
		get
		{
			return this.Animator.Library;
		}
		set
		{
			this.Animator.Library = value;
		}
	}

	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x06003E7D RID: 15997 RVA: 0x0013C064 File Offset: 0x0013A264
	// (set) Token: 0x06003E7E RID: 15998 RVA: 0x0013C074 File Offset: 0x0013A274
	public int DefaultClipId
	{
		get
		{
			return this.Animator.DefaultClipId;
		}
		set
		{
			this.Animator.DefaultClipId = value;
		}
	}

	// Token: 0x1700096F RID: 2415
	// (get) Token: 0x06003E7F RID: 15999 RVA: 0x0013C084 File Offset: 0x0013A284
	// (set) Token: 0x06003E80 RID: 16000 RVA: 0x0013C08C File Offset: 0x0013A28C
	public static bool g_paused
	{
		get
		{
			return tk2dSpriteAnimator.g_Paused;
		}
		set
		{
			tk2dSpriteAnimator.g_Paused = value;
		}
	}

	// Token: 0x17000970 RID: 2416
	// (get) Token: 0x06003E81 RID: 16001 RVA: 0x0013C094 File Offset: 0x0013A294
	// (set) Token: 0x06003E82 RID: 16002 RVA: 0x0013C0A4 File Offset: 0x0013A2A4
	public bool Paused
	{
		get
		{
			return this.Animator.Paused;
		}
		set
		{
			this.Animator.Paused = value;
		}
	}

	// Token: 0x06003E83 RID: 16003 RVA: 0x0013C0B4 File Offset: 0x0013A2B4
	private void ProxyCompletedHandler(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip)
	{
		if (this.animationCompleteDelegate != null)
		{
			int num = -1;
			tk2dSpriteAnimationClip[] array = ((!(anim.Library != null)) ? null : anim.Library.clips);
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == clip)
					{
						num = i;
						break;
					}
				}
			}
			this.animationCompleteDelegate(this, num);
		}
	}

	// Token: 0x06003E84 RID: 16004 RVA: 0x0013C128 File Offset: 0x0013A328
	private void ProxyEventTriggeredHandler(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.animationEventDelegate != null)
		{
			this.animationEventDelegate(this, clip, clip.frames[frame], frame);
		}
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x0013C14C File Offset: 0x0013A34C
	private void OnEnable()
	{
		this.Animator.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.ProxyCompletedHandler);
		this.Animator.AnimationEventTriggered = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.ProxyEventTriggeredHandler);
	}

	// Token: 0x06003E86 RID: 16006 RVA: 0x0013C17C File Offset: 0x0013A37C
	private void OnDisable()
	{
		this.Animator.AnimationCompleted = null;
		this.Animator.AnimationEventTriggered = null;
	}

	// Token: 0x06003E87 RID: 16007 RVA: 0x0013C198 File Offset: 0x0013A398
	private void Start()
	{
		this.CheckAddAnimatorInternal();
	}

	// Token: 0x06003E88 RID: 16008 RVA: 0x0013C1A0 File Offset: 0x0013A3A0
	public static tk2dAnimatedSprite AddComponent(GameObject go, tk2dSpriteAnimation anim, int clipId)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = anim.clips[clipId];
		tk2dAnimatedSprite tk2dAnimatedSprite = go.AddComponent<tk2dAnimatedSprite>();
		tk2dAnimatedSprite.SetSprite(tk2dSpriteAnimationClip.frames[0].spriteCollection, tk2dSpriteAnimationClip.frames[0].spriteId);
		tk2dAnimatedSprite.anim = anim;
		return tk2dAnimatedSprite;
	}

	// Token: 0x06003E89 RID: 16009 RVA: 0x0013C1E8 File Offset: 0x0013A3E8
	public void Play()
	{
		if (this.Animator.DefaultClip != null)
		{
			this.Animator.Play(this.Animator.DefaultClip);
		}
	}

	// Token: 0x06003E8A RID: 16010 RVA: 0x0013C210 File Offset: 0x0013A410
	public void Play(float clipStartTime)
	{
		if (this.Animator.DefaultClip != null)
		{
			this.Animator.PlayFrom(this.Animator.DefaultClip, clipStartTime);
		}
	}

	// Token: 0x06003E8B RID: 16011 RVA: 0x0013C23C File Offset: 0x0013A43C
	public void PlayFromFrame(int frame)
	{
		if (this.Animator.DefaultClip != null)
		{
			this.Animator.PlayFromFrame(this.Animator.DefaultClip, frame);
		}
	}

	// Token: 0x06003E8C RID: 16012 RVA: 0x0013C268 File Offset: 0x0013A468
	public void Play(string name)
	{
		this.Animator.Play(name);
	}

	// Token: 0x06003E8D RID: 16013 RVA: 0x0013C278 File Offset: 0x0013A478
	public void PlayFromFrame(string name, int frame)
	{
		this.Animator.PlayFromFrame(name, frame);
	}

	// Token: 0x06003E8E RID: 16014 RVA: 0x0013C288 File Offset: 0x0013A488
	public void Play(string name, float clipStartTime)
	{
		this.Animator.PlayFrom(name, clipStartTime);
	}

	// Token: 0x06003E8F RID: 16015 RVA: 0x0013C298 File Offset: 0x0013A498
	public void Play(tk2dSpriteAnimationClip clip, float clipStartTime)
	{
		this.Animator.PlayFrom(clip, clipStartTime);
	}

	// Token: 0x06003E90 RID: 16016 RVA: 0x0013C2A8 File Offset: 0x0013A4A8
	public void Play(tk2dSpriteAnimationClip clip, float clipStartTime, float overrideFps)
	{
		this.Animator.Play(clip, clipStartTime, overrideFps, false);
	}

	// Token: 0x17000971 RID: 2417
	// (get) Token: 0x06003E91 RID: 16017 RVA: 0x0013C2BC File Offset: 0x0013A4BC
	public tk2dSpriteAnimationClip CurrentClip
	{
		get
		{
			return this.Animator.CurrentClip;
		}
	}

	// Token: 0x17000972 RID: 2418
	// (get) Token: 0x06003E92 RID: 16018 RVA: 0x0013C2CC File Offset: 0x0013A4CC
	public float ClipTimeSeconds
	{
		get
		{
			return this.Animator.ClipTimeSeconds;
		}
	}

	// Token: 0x17000973 RID: 2419
	// (get) Token: 0x06003E93 RID: 16019 RVA: 0x0013C2DC File Offset: 0x0013A4DC
	// (set) Token: 0x06003E94 RID: 16020 RVA: 0x0013C2EC File Offset: 0x0013A4EC
	public float ClipFps
	{
		get
		{
			return this.Animator.ClipFps;
		}
		set
		{
			this.Animator.ClipFps = value;
		}
	}

	// Token: 0x06003E95 RID: 16021 RVA: 0x0013C2FC File Offset: 0x0013A4FC
	public void Stop()
	{
		this.Animator.Stop();
	}

	// Token: 0x06003E96 RID: 16022 RVA: 0x0013C30C File Offset: 0x0013A50C
	public void StopAndResetFrame()
	{
		this.Animator.StopAndResetFrame();
	}

	// Token: 0x06003E97 RID: 16023 RVA: 0x0013C31C File Offset: 0x0013A51C
	[Obsolete]
	public bool isPlaying()
	{
		return this.Animator.Playing;
	}

	// Token: 0x06003E98 RID: 16024 RVA: 0x0013C32C File Offset: 0x0013A52C
	public bool IsPlaying(string name)
	{
		return this.Animator.Playing;
	}

	// Token: 0x06003E99 RID: 16025 RVA: 0x0013C33C File Offset: 0x0013A53C
	public bool IsPlaying(tk2dSpriteAnimationClip clip)
	{
		return this.Animator.IsPlaying(clip);
	}

	// Token: 0x17000974 RID: 2420
	// (get) Token: 0x06003E9A RID: 16026 RVA: 0x0013C34C File Offset: 0x0013A54C
	public bool Playing
	{
		get
		{
			return this.Animator.Playing;
		}
	}

	// Token: 0x06003E9B RID: 16027 RVA: 0x0013C35C File Offset: 0x0013A55C
	public int GetClipIdByName(string name)
	{
		return this.Animator.GetClipIdByName(name);
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x0013C36C File Offset: 0x0013A56C
	public tk2dSpriteAnimationClip GetClipByName(string name)
	{
		return this.Animator.GetClipByName(name);
	}

	// Token: 0x17000975 RID: 2421
	// (get) Token: 0x06003E9D RID: 16029 RVA: 0x0013C37C File Offset: 0x0013A57C
	public static float DefaultFps
	{
		get
		{
			return tk2dSpriteAnimator.DefaultFps;
		}
	}

	// Token: 0x06003E9E RID: 16030 RVA: 0x0013C384 File Offset: 0x0013A584
	public void Pause()
	{
		this.Animator.Pause();
	}

	// Token: 0x06003E9F RID: 16031 RVA: 0x0013C394 File Offset: 0x0013A594
	public void Resume()
	{
		this.Animator.Resume();
	}

	// Token: 0x06003EA0 RID: 16032 RVA: 0x0013C3A4 File Offset: 0x0013A5A4
	public void SetFrame(int currFrame)
	{
		this.Animator.SetFrame(currFrame);
	}

	// Token: 0x06003EA1 RID: 16033 RVA: 0x0013C3B4 File Offset: 0x0013A5B4
	public void SetFrame(int currFrame, bool triggerEvent)
	{
		this.Animator.SetFrame(currFrame, triggerEvent);
	}

	// Token: 0x06003EA2 RID: 16034 RVA: 0x0013C3C4 File Offset: 0x0013A5C4
	public void UpdateAnimation(float deltaTime)
	{
		this.Animator.UpdateAnimation(deltaTime);
	}

	// Token: 0x06003EA3 RID: 16035 RVA: 0x0013C3D4 File Offset: 0x0013A5D4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04003118 RID: 12568
	[SerializeField]
	private tk2dSpriteAnimator _animator;

	// Token: 0x04003119 RID: 12569
	[SerializeField]
	private tk2dSpriteAnimation anim;

	// Token: 0x0400311A RID: 12570
	[SerializeField]
	private int clipId;

	// Token: 0x0400311B RID: 12571
	public bool playAutomatically;

	// Token: 0x0400311C RID: 12572
	public bool createCollider;

	// Token: 0x0400311D RID: 12573
	public tk2dAnimatedSprite.AnimationCompleteDelegate animationCompleteDelegate;

	// Token: 0x0400311E RID: 12574
	public tk2dAnimatedSprite.AnimationEventDelegate animationEventDelegate;

	// Token: 0x02000BA5 RID: 2981
	// (Invoke) Token: 0x06003EA5 RID: 16037
	public delegate void AnimationCompleteDelegate(tk2dAnimatedSprite sprite, int clipId);

	// Token: 0x02000BA6 RID: 2982
	// (Invoke) Token: 0x06003EA9 RID: 16041
	public delegate void AnimationEventDelegate(tk2dAnimatedSprite sprite, tk2dSpriteAnimationClip clip, tk2dSpriteAnimationFrame frame, int frameNum);
}
