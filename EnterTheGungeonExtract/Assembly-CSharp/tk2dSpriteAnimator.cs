using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000BB6 RID: 2998
[AddComponentMenu("2D Toolkit/Sprite/tk2dSpriteAnimator")]
public class tk2dSpriteAnimator : BraveBehaviour
{
	// Token: 0x17000999 RID: 2457
	// (get) Token: 0x06003F70 RID: 16240 RVA: 0x001419F8 File Offset: 0x0013FBF8
	// (set) Token: 0x06003F71 RID: 16241 RVA: 0x00141A08 File Offset: 0x0013FC08
	public static bool g_Paused
	{
		get
		{
			return (tk2dSpriteAnimator.globalState & tk2dSpriteAnimator.State.Paused) != tk2dSpriteAnimator.State.Init;
		}
		set
		{
			tk2dSpriteAnimator.globalState = ((!value) ? tk2dSpriteAnimator.State.Init : tk2dSpriteAnimator.State.Paused);
		}
	}

	// Token: 0x1700099A RID: 2458
	// (get) Token: 0x06003F72 RID: 16242 RVA: 0x00141A1C File Offset: 0x0013FC1C
	// (set) Token: 0x06003F73 RID: 16243 RVA: 0x00141A24 File Offset: 0x0013FC24
	public bool MuteAudio { get; set; }

	// Token: 0x1700099B RID: 2459
	// (get) Token: 0x06003F74 RID: 16244 RVA: 0x00141A30 File Offset: 0x0013FC30
	// (set) Token: 0x06003F75 RID: 16245 RVA: 0x00141A40 File Offset: 0x0013FC40
	public bool Paused
	{
		get
		{
			return (this.state & tk2dSpriteAnimator.State.Paused) != tk2dSpriteAnimator.State.Init;
		}
		set
		{
			if (value)
			{
				this.state |= tk2dSpriteAnimator.State.Paused;
			}
			else
			{
				this.state &= (tk2dSpriteAnimator.State)(-3);
			}
		}
	}

	// Token: 0x1700099C RID: 2460
	// (get) Token: 0x06003F76 RID: 16246 RVA: 0x00141A6C File Offset: 0x0013FC6C
	// (set) Token: 0x06003F77 RID: 16247 RVA: 0x00141A74 File Offset: 0x0013FC74
	public tk2dSpriteAnimation Library
	{
		get
		{
			return this.library;
		}
		set
		{
			this.library = value;
		}
	}

	// Token: 0x1700099D RID: 2461
	// (get) Token: 0x06003F78 RID: 16248 RVA: 0x00141A80 File Offset: 0x0013FC80
	// (set) Token: 0x06003F79 RID: 16249 RVA: 0x00141A88 File Offset: 0x0013FC88
	public int DefaultClipId
	{
		get
		{
			return this.defaultClipId;
		}
		set
		{
			this.defaultClipId = value;
		}
	}

	// Token: 0x1700099E RID: 2462
	// (get) Token: 0x06003F7A RID: 16250 RVA: 0x00141A94 File Offset: 0x0013FC94
	public tk2dSpriteAnimationClip DefaultClip
	{
		get
		{
			return this.GetClipById(this.defaultClipId);
		}
	}

	// Token: 0x06003F7B RID: 16251 RVA: 0x00141AA4 File Offset: 0x0013FCA4
	public void ForceClearCurrentClip()
	{
		this.currentClip = null;
	}

	// Token: 0x06003F7C RID: 16252 RVA: 0x00141AB0 File Offset: 0x0013FCB0
	private void OnEnable()
	{
		if (this.Sprite == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06003F7D RID: 16253 RVA: 0x00141ACC File Offset: 0x0013FCCC
	private void Awake()
	{
		if (this.Sprite)
		{
			this._startingSpriteCollection = this.Sprite.Collection;
			this._startingSpriteId = this.Sprite.spriteId;
		}
		if (this.AlwaysIgnoreTimeScale)
		{
			this.ignoreTimeScale = true;
		}
	}

	// Token: 0x06003F7E RID: 16254 RVA: 0x00141B20 File Offset: 0x0013FD20
	private void Start()
	{
		if (!this.deferNextStartClip && this.playAutomatically && !this.IsPlaying(this.DefaultClip))
		{
			this.Play(this.DefaultClip);
		}
		this.deferNextStartClip = false;
		if (base.GetComponent<tk2dSpriteAttachPoint>())
		{
			this.m_hasAttachPoints = true;
		}
	}

	// Token: 0x06003F7F RID: 16255 RVA: 0x00141B80 File Offset: 0x0013FD80
	public void OnSpawned()
	{
		if (base.enabled)
		{
			this.OnEnable();
			this.Start();
		}
	}

	// Token: 0x06003F80 RID: 16256 RVA: 0x00141B9C File Offset: 0x0013FD9C
	public void OnDespawned()
	{
		if (this.playAutomatically)
		{
			this.StopAndResetFrame();
		}
		else
		{
			this.Stop();
		}
	}

	// Token: 0x06003F81 RID: 16257 RVA: 0x00141BBC File Offset: 0x0013FDBC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x1700099F RID: 2463
	// (get) Token: 0x06003F82 RID: 16258 RVA: 0x00141BC4 File Offset: 0x0013FDC4
	public virtual tk2dBaseSprite Sprite
	{
		get
		{
			if (this._sprite == null)
			{
				this._sprite = base.GetComponent<tk2dBaseSprite>();
				if (this._sprite == null)
				{
					Debug.LogError("Sprite not found attached to tk2dSpriteAnimator.");
				}
			}
			return this._sprite;
		}
	}

	// Token: 0x06003F83 RID: 16259 RVA: 0x00141C04 File Offset: 0x0013FE04
	public static tk2dSpriteAnimator AddComponent(GameObject go, tk2dSpriteAnimation anim, int clipId)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = anim.clips[clipId];
		tk2dSpriteAnimator tk2dSpriteAnimator = go.AddComponent<tk2dSpriteAnimator>();
		tk2dSpriteAnimator.Library = anim;
		if (tk2dSpriteAnimationClip.frames[0].requiresOffscreenUpdate)
		{
			tk2dSpriteAnimator.m_forceNextSpriteUpdate = true;
		}
		tk2dSpriteAnimator.SetSprite(tk2dSpriteAnimationClip.frames[0].spriteCollection, tk2dSpriteAnimationClip.frames[0].spriteId);
		if (tk2dSpriteAnimationClip.frames[0].requiresOffscreenUpdate)
		{
			tk2dSpriteAnimator.m_forceNextSpriteUpdate = true;
		}
		return tk2dSpriteAnimator;
	}

	// Token: 0x06003F84 RID: 16260 RVA: 0x00141C7C File Offset: 0x0013FE7C
	private tk2dSpriteAnimationClip GetClipByNameVerbose(string name)
	{
		if (this.library == null)
		{
			Debug.LogError("Library not set");
			return null;
		}
		tk2dSpriteAnimationClip clipByName = this.library.GetClipByName(name);
		if (clipByName == null)
		{
			Debug.LogError("Unable to find clip '" + name + "' in library");
			return null;
		}
		return clipByName;
	}

	// Token: 0x06003F85 RID: 16261 RVA: 0x00141CD4 File Offset: 0x0013FED4
	public void Play()
	{
		if (this.currentClip == null)
		{
			this.currentClip = this.DefaultClip;
		}
		this.Play(this.currentClip);
	}

	// Token: 0x06003F86 RID: 16262 RVA: 0x00141CFC File Offset: 0x0013FEFC
	public void Play(string name)
	{
		this.Play(this.GetClipByNameVerbose(name));
	}

	// Token: 0x06003F87 RID: 16263 RVA: 0x00141D0C File Offset: 0x0013FF0C
	public void Play(tk2dSpriteAnimationClip clip)
	{
		this.Play(clip, 0f, tk2dSpriteAnimator.DefaultFps, false);
	}

	// Token: 0x06003F88 RID: 16264 RVA: 0x00141D20 File Offset: 0x0013FF20
	public void PlayFromFrame(int frame)
	{
		if (this.currentClip == null)
		{
			this.currentClip = this.DefaultClip;
		}
		this.PlayFromFrame(this.currentClip, frame);
	}

	// Token: 0x06003F89 RID: 16265 RVA: 0x00141D48 File Offset: 0x0013FF48
	public void PlayFromFrame(string name, int frame)
	{
		this.PlayFromFrame(this.GetClipByNameVerbose(name), frame);
	}

	// Token: 0x06003F8A RID: 16266 RVA: 0x00141D58 File Offset: 0x0013FF58
	public void PlayFromFrame(tk2dSpriteAnimationClip clip, int frame)
	{
		this.PlayFrom(clip, ((float)frame + 0.001f) / clip.fps);
	}

	// Token: 0x06003F8B RID: 16267 RVA: 0x00141D70 File Offset: 0x0013FF70
	public void PlayFrom(float clipStartTime)
	{
		if (this.currentClip == null)
		{
			this.currentClip = this.DefaultClip;
		}
		this.PlayFrom(this.currentClip, clipStartTime);
	}

	// Token: 0x06003F8C RID: 16268 RVA: 0x00141D98 File Offset: 0x0013FF98
	public void PlayFrom(string name, float clipStartTime)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = ((!this.library) ? null : this.library.GetClipByName(name));
		if (tk2dSpriteAnimationClip == null)
		{
			this.ClipNameError(name);
		}
		else
		{
			this.PlayFrom(tk2dSpriteAnimationClip, clipStartTime);
		}
	}

	// Token: 0x06003F8D RID: 16269 RVA: 0x00141DE4 File Offset: 0x0013FFE4
	public void PlayFrom(tk2dSpriteAnimationClip clip, float clipStartTime)
	{
		this.Play(clip, clipStartTime, tk2dSpriteAnimator.DefaultFps, false);
	}

	// Token: 0x06003F8E RID: 16270 RVA: 0x00141DF4 File Offset: 0x0013FFF4
	public void QueueAnimation(string animationName)
	{
		this.m_queuedAnimationName = animationName;
		this.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(this.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.StartQueuedAnimationSimple));
	}

	// Token: 0x06003F8F RID: 16271 RVA: 0x00141E20 File Offset: 0x00140020
	private void StartQueuedAnimationSimple(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
	{
		this.Play(this.m_queuedAnimationName);
		this.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(this.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.StartQueuedAnimationSimple));
	}

	// Token: 0x06003F90 RID: 16272 RVA: 0x00141E50 File Offset: 0x00140050
	private void StopAndDisableGameObject(tk2dSpriteAnimator source, tk2dSpriteAnimationClip clip)
	{
		this.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(this.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.StopAndDisableGameObject));
		this.Stop();
		if (this.m_overrideTargetDisableObject != null)
		{
			this.m_overrideTargetDisableObject.SetActive(false);
			this.m_overrideTargetDisableObject = null;
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003F91 RID: 16273 RVA: 0x00141EBC File Offset: 0x001400BC
	private void StopAndDestroyGameObject(tk2dSpriteAnimator source, tk2dSpriteAnimationClip clip)
	{
		if (this.m_onDestroyAction != null)
		{
			this.m_onDestroyAction();
		}
		this.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(this.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.StopAndDestroyGameObject));
		this.Stop();
		SpawnManager.Despawn(base.gameObject);
	}

	// Token: 0x06003F92 RID: 16274 RVA: 0x00141F14 File Offset: 0x00140114
	public void PlayAndDestroyObject(string clipName = "", Action onDestroy = null)
	{
		this.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(this.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.StopAndDestroyGameObject));
		if (onDestroy != null)
		{
			this.m_onDestroyAction = onDestroy;
		}
		if (string.IsNullOrEmpty(clipName))
		{
			this.Play();
		}
		else
		{
			this.Play(clipName);
		}
	}

	// Token: 0x06003F93 RID: 16275 RVA: 0x00141F70 File Offset: 0x00140170
	public void PlayAndDisableObject(string clipName = "", GameObject overrideTargetObject = null)
	{
		this.m_overrideTargetDisableObject = overrideTargetObject;
		this.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(this.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.StopAndDisableGameObject));
		if (string.IsNullOrEmpty(clipName))
		{
			this.Play();
		}
		else
		{
			this.Play(clipName);
		}
	}

	// Token: 0x06003F94 RID: 16276 RVA: 0x00141FC4 File Offset: 0x001401C4
	public void PlayAndDisableRenderer(string clipName = "")
	{
		this.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(this.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.StopAndDisableRenderer));
		if (string.IsNullOrEmpty(clipName))
		{
			this.Play();
		}
		else
		{
			this.Play(clipName);
		}
	}

	// Token: 0x06003F95 RID: 16277 RVA: 0x00142010 File Offset: 0x00140210
	private void StopAndDisableRenderer(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
	{
		this.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(this.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.StopAndDisableRenderer));
		this.Stop();
		base.GetComponent<Renderer>().enabled = false;
	}

	// Token: 0x06003F96 RID: 16278 RVA: 0x00142048 File Offset: 0x00140248
	public void PlayForDurationForceLoop(tk2dSpriteAnimationClip clip, float duration)
	{
		if (clip == null)
		{
			this.ClipNameError(base.name);
		}
		else
		{
			this.Play(clip);
			if (duration < 0f)
			{
				duration = clip.BaseClipLength;
			}
			base.StartCoroutine(this.RevertToClipForceLoop(clip, duration));
		}
	}

	// Token: 0x06003F97 RID: 16279 RVA: 0x00142098 File Offset: 0x00140298
	private IEnumerator RevertToClipForceLoop(tk2dSpriteAnimationClip playingClip, float duration)
	{
		float timer = duration;
		yield return null;
		while (timer > 0f)
		{
			timer -= this.GetDeltaTime();
			if (timer <= 0f)
			{
				yield break;
			}
			if (!this.IsPlaying(playingClip))
			{
				this.Play(playingClip);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003F98 RID: 16280 RVA: 0x001420C4 File Offset: 0x001402C4
	public void PlayForDuration(string name, float duration, string revertAnimName, bool returnToLoopSection = false)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = ((!this.library) ? null : this.library.GetClipByName(revertAnimName));
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip2 = ((!this.library) ? null : this.library.GetClipByName(name));
		if (tk2dSpriteAnimationClip2 != null)
		{
			this.Play(tk2dSpriteAnimationClip2);
			if (duration < 0f)
			{
				duration = tk2dSpriteAnimationClip2.BaseClipLength;
			}
			base.StartCoroutine(this.RevertToClip(tk2dSpriteAnimationClip2, tk2dSpriteAnimationClip, duration, returnToLoopSection));
		}
	}

	// Token: 0x06003F99 RID: 16281 RVA: 0x0014214C File Offset: 0x0014034C
	public void PlayForDuration(string name, float duration)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = this.currentClip;
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip2 = ((!this.library) ? null : this.library.GetClipByName(name));
		if (tk2dSpriteAnimationClip2 == null)
		{
			this.ClipNameError(name);
		}
		else
		{
			this.Play(tk2dSpriteAnimationClip2);
			if (duration < 0f)
			{
				duration = tk2dSpriteAnimationClip2.BaseClipLength;
			}
			base.StartCoroutine(this.RevertToClip(tk2dSpriteAnimationClip2, tk2dSpriteAnimationClip, duration, false));
		}
	}

	// Token: 0x06003F9A RID: 16282 RVA: 0x001421C0 File Offset: 0x001403C0
	private IEnumerator RevertToClip(tk2dSpriteAnimationClip playingClip, tk2dSpriteAnimationClip revertToClip, float duration, bool returnToLoopSection = false)
	{
		float timer = duration;
		yield return null;
		while (this.currentClip == playingClip)
		{
			timer -= this.GetDeltaTime();
			if (timer <= 0f)
			{
				if (revertToClip != null)
				{
					if (revertToClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopSection && returnToLoopSection)
					{
						this.Play(revertToClip, (float)revertToClip.loopStart / revertToClip.fps, tk2dSpriteAnimator.DefaultFps, false);
					}
					else
					{
						this.Play(revertToClip);
					}
				}
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003F9B RID: 16283 RVA: 0x001421F8 File Offset: 0x001403F8
	public void PlayAndForceTime(string clipName, float forceTime)
	{
		this.PlayAndForceTime(this.GetClipByName(clipName), forceTime);
	}

	// Token: 0x06003F9C RID: 16284 RVA: 0x00142208 File Offset: 0x00140408
	public void PlayAndForceTime(tk2dSpriteAnimationClip clip, float forceTime)
	{
		this.Play(clip, 0f, clip.fps * (clip.BaseClipLength / forceTime), false);
	}

	// Token: 0x06003F9D RID: 16285 RVA: 0x00142228 File Offset: 0x00140428
	public void Play(tk2dSpriteAnimationClip clip, float clipStartTime, float overrideFps, bool skipEvents = false)
	{
		if (this.OnPlayAnimationCalled != null)
		{
			this.OnPlayAnimationCalled(this, clip);
		}
		if (clip != null)
		{
			float num = ((overrideFps <= 0f) ? clip.fps : overrideFps);
			bool flag = clipStartTime == 0f && this.IsPlaying(clip);
			if (flag)
			{
				this.clipFps = num;
			}
			else
			{
				this.state |= tk2dSpriteAnimator.State.Playing;
				this.currentClip = clip;
				this.clipFps = num;
				if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Single || this.currentClip.frames == null)
				{
					this.WarpClipToLocalTime(this.currentClip, 0f, skipEvents);
					this.state &= (tk2dSpriteAnimator.State)(-2);
				}
				else if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomFrame || this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomLoop)
				{
					int num2 = UnityEngine.Random.Range(0, this.currentClip.frames.Length);
					this.WarpClipToLocalTime(this.currentClip, (float)num2, skipEvents);
					if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomFrame)
					{
						this.previousFrame = -1;
						this.state &= (tk2dSpriteAnimator.State)(-2);
					}
				}
				else
				{
					float num3 = clipStartTime * this.clipFps;
					if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Once && num3 >= this.clipFps * (float)this.currentClip.frames.Length)
					{
						this.WarpClipToLocalTime(this.currentClip, (float)(this.currentClip.frames.Length - 1), skipEvents);
						this.state &= (tk2dSpriteAnimator.State)(-2);
					}
					else
					{
						this.WarpClipToLocalTime(this.currentClip, num3, skipEvents);
						this.clipTime = num3;
					}
				}
			}
		}
		else
		{
			Debug.LogWarning("Calling clip.Play() with a null clip");
			this.OnAnimationCompleted();
			this.state &= (tk2dSpriteAnimator.State)(-2);
		}
	}

	// Token: 0x06003F9E RID: 16286 RVA: 0x0014240C File Offset: 0x0014060C
	public bool QueryPreviousInvulnerabilityFrame(int framesBack)
	{
		return this.CurrentClip != null && this.CurrentFrame >= framesBack && this.CurrentFrame < this.CurrentClip.frames.Length && this.CurrentClip.frames[this.CurrentFrame - framesBack].invulnerableFrame;
	}

	// Token: 0x06003F9F RID: 16287 RVA: 0x00142464 File Offset: 0x00140664
	public bool QueryInvulnerabilityFrame()
	{
		return this.CurrentClip != null && this.CurrentFrame >= 0 && this.CurrentFrame < this.CurrentClip.frames.Length && this.CurrentClip.frames[this.CurrentFrame].invulnerableFrame;
	}

	// Token: 0x06003FA0 RID: 16288 RVA: 0x001424BC File Offset: 0x001406BC
	public bool QueryGroundedFrame()
	{
		return this.CurrentClip == null || this.CurrentFrame < 0 || this.CurrentFrame >= this.CurrentClip.frames.Length || this.CurrentClip.frames[this.CurrentFrame].groundedFrame;
	}

	// Token: 0x06003FA1 RID: 16289 RVA: 0x00142514 File Offset: 0x00140714
	public void Stop()
	{
		this.state &= (tk2dSpriteAnimator.State)(-2);
	}

	// Token: 0x06003FA2 RID: 16290 RVA: 0x00142528 File Offset: 0x00140728
	public void StopAndResetFrame()
	{
		if (this.currentClip != null)
		{
			if (this.currentClip.frames[0].requiresOffscreenUpdate)
			{
				this.m_forceNextSpriteUpdate = true;
			}
			this.SetSprite(this.currentClip.frames[0].spriteCollection, this.currentClip.frames[0].spriteId);
			if (this.currentClip.frames[0].requiresOffscreenUpdate)
			{
				this.m_forceNextSpriteUpdate = true;
			}
		}
		this.Stop();
	}

	// Token: 0x06003FA3 RID: 16291 RVA: 0x001425AC File Offset: 0x001407AC
	public void StopAndResetFrameToDefault()
	{
		if (this.currentClip != null)
		{
			if (this.currentClip.frames[0].requiresOffscreenUpdate)
			{
				this.m_forceNextSpriteUpdate = true;
			}
			this.SetSprite(this._startingSpriteCollection, this._startingSpriteId);
			if (this.currentClip.frames[0].requiresOffscreenUpdate)
			{
				this.m_forceNextSpriteUpdate = true;
			}
		}
		this.Stop();
	}

	// Token: 0x06003FA4 RID: 16292 RVA: 0x00142618 File Offset: 0x00140818
	public bool IsPlaying(string name)
	{
		return this.Playing && this.CurrentClip != null && this.CurrentClip.name == name;
	}

	// Token: 0x06003FA5 RID: 16293 RVA: 0x00142644 File Offset: 0x00140844
	public bool IsPlaying(tk2dSpriteAnimationClip clip)
	{
		return this.Playing && this.CurrentClip != null && this.CurrentClip == clip;
	}

	// Token: 0x170009A0 RID: 2464
	// (get) Token: 0x06003FA6 RID: 16294 RVA: 0x00142668 File Offset: 0x00140868
	public bool Playing
	{
		get
		{
			return (this.state & tk2dSpriteAnimator.State.Playing) != tk2dSpriteAnimator.State.Init;
		}
	}

	// Token: 0x170009A1 RID: 2465
	// (get) Token: 0x06003FA7 RID: 16295 RVA: 0x00142678 File Offset: 0x00140878
	public tk2dSpriteAnimationClip CurrentClip
	{
		get
		{
			return this.currentClip;
		}
	}

	// Token: 0x170009A2 RID: 2466
	// (get) Token: 0x06003FA8 RID: 16296 RVA: 0x00142680 File Offset: 0x00140880
	public float ClipTimeSeconds
	{
		get
		{
			return (this.clipFps <= 0f) ? (this.clipTime / this.currentClip.fps) : (this.clipTime / this.clipFps);
		}
	}

	// Token: 0x170009A3 RID: 2467
	// (get) Token: 0x06003FA9 RID: 16297 RVA: 0x001426B8 File Offset: 0x001408B8
	// (set) Token: 0x06003FAA RID: 16298 RVA: 0x001426C0 File Offset: 0x001408C0
	public float ClipFps
	{
		get
		{
			return this.clipFps;
		}
		set
		{
			if (this.currentClip != null)
			{
				this.clipFps = ((value <= 0f) ? this.currentClip.fps : value);
			}
		}
	}

	// Token: 0x06003FAB RID: 16299 RVA: 0x001426F0 File Offset: 0x001408F0
	public tk2dSpriteAnimationClip GetClipById(int id)
	{
		if (this.library == null)
		{
			return null;
		}
		return this.library.GetClipById(id);
	}

	// Token: 0x170009A4 RID: 2468
	// (get) Token: 0x06003FAC RID: 16300 RVA: 0x00142714 File Offset: 0x00140914
	public static float DefaultFps
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06003FAD RID: 16301 RVA: 0x0014271C File Offset: 0x0014091C
	public int GetClipIdByName(string name)
	{
		return (!this.library) ? (-1) : this.library.GetClipIdByName(name);
	}

	// Token: 0x06003FAE RID: 16302 RVA: 0x00142740 File Offset: 0x00140940
	public tk2dSpriteAnimationClip GetClipByName(string name)
	{
		return (!this.library) ? null : this.library.GetClipByName(name);
	}

	// Token: 0x06003FAF RID: 16303 RVA: 0x00142764 File Offset: 0x00140964
	public void Pause()
	{
		this.state |= tk2dSpriteAnimator.State.Paused;
	}

	// Token: 0x06003FB0 RID: 16304 RVA: 0x00142774 File Offset: 0x00140974
	public void Resume()
	{
		this.state &= (tk2dSpriteAnimator.State)(-3);
	}

	// Token: 0x06003FB1 RID: 16305 RVA: 0x00142788 File Offset: 0x00140988
	public void SetFrame(int currFrame)
	{
		this.SetFrame(currFrame, true);
	}

	// Token: 0x06003FB2 RID: 16306 RVA: 0x00142794 File Offset: 0x00140994
	public void SetFrame(int currFrame, bool triggerEvent)
	{
		if (this.currentClip == null)
		{
			this.currentClip = this.DefaultClip;
		}
		if (this.currentClip != null)
		{
			int num = currFrame % this.currentClip.frames.Length;
			this.SetFrameInternal(num);
			if (triggerEvent && this.currentClip.frames.Length > 0 && currFrame >= 0)
			{
				this.ProcessEvents(num - 1, num, 1);
			}
		}
	}

	// Token: 0x170009A5 RID: 2469
	// (get) Token: 0x06003FB3 RID: 16307 RVA: 0x00142808 File Offset: 0x00140A08
	public int CurrentFrame
	{
		get
		{
			switch (this.currentClip.wrapMode)
			{
			case tk2dSpriteAnimationClip.WrapMode.Loop:
			case tk2dSpriteAnimationClip.WrapMode.RandomLoop:
			case tk2dSpriteAnimationClip.WrapMode.LoopFidget:
				break;
			case tk2dSpriteAnimationClip.WrapMode.LoopSection:
			{
				int num = (int)this.clipTime;
				int num2 = this.currentClip.loopStart + (num - this.currentClip.loopStart) % (this.currentClip.frames.Length - this.currentClip.loopStart);
				if (num >= this.currentClip.loopStart)
				{
					return num2;
				}
				return num;
			}
			case tk2dSpriteAnimationClip.WrapMode.Once:
				return Mathf.Min((int)this.clipTime, this.currentClip.frames.Length);
			case tk2dSpriteAnimationClip.WrapMode.PingPong:
			{
				int num3 = ((this.currentClip.frames.Length <= 1) ? 0 : ((int)this.clipTime % (this.currentClip.frames.Length + this.currentClip.frames.Length - 2)));
				if (num3 >= this.currentClip.frames.Length)
				{
					num3 = 2 * this.currentClip.frames.Length - 2 - num3;
				}
				return num3;
			}
			case tk2dSpriteAnimationClip.WrapMode.RandomFrame:
				goto IL_122;
			case tk2dSpriteAnimationClip.WrapMode.Single:
				return 0;
			default:
				goto IL_122;
			}
			IL_53:
			return (int)this.clipTime % this.currentClip.frames.Length;
			IL_122:
			Debug.LogError("Unhandled clip wrap mode");
			goto IL_53;
		}
	}

	// Token: 0x06003FB4 RID: 16308 RVA: 0x00142948 File Offset: 0x00140B48
	public void UpdateAnimation(float deltaTime)
	{
		tk2dSpriteAnimator.State state = this.state | tk2dSpriteAnimator.globalState;
		if (state != tk2dSpriteAnimator.State.Playing)
		{
			return;
		}
		if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopFidget && this.m_fidgetDuration > 0f)
		{
			this.m_fidgetElapsed += deltaTime;
			if (this.m_fidgetElapsed >= this.m_fidgetDuration)
			{
				this.m_fidgetElapsed = 0f;
				this.m_fidgetDuration = 0f;
				this.clipTime += deltaTime * this.clipFps;
			}
		}
		else
		{
			this.clipTime += deltaTime * this.clipFps;
		}
		int num = this.previousFrame;
		switch (this.currentClip.wrapMode)
		{
		case tk2dSpriteAnimationClip.WrapMode.Loop:
		case tk2dSpriteAnimationClip.WrapMode.RandomLoop:
		{
			int num2 = (int)this.clipTime % this.currentClip.frames.Length;
			this.SetFrameInternal(num2);
			if (num2 < num)
			{
				this.ProcessEvents(num, this.currentClip.frames.Length - 1, 1);
				this.ProcessEvents(-1, num2, 1);
			}
			else
			{
				this.ProcessEvents(num, num2, 1);
			}
			break;
		}
		case tk2dSpriteAnimationClip.WrapMode.LoopSection:
		{
			int num3 = (int)this.clipTime;
			int num4 = this.currentClip.loopStart + (num3 - this.currentClip.loopStart) % (this.currentClip.frames.Length - this.currentClip.loopStart);
			if (num3 >= this.currentClip.loopStart)
			{
				this.SetFrameInternal(num4);
				num3 = num4;
				if (num < this.currentClip.loopStart)
				{
					this.ProcessEvents(num, this.currentClip.loopStart - 1, 1);
					this.ProcessEvents(this.currentClip.loopStart - 1, num3, 1);
				}
				else if (num3 < num)
				{
					this.ProcessEvents(num, this.currentClip.frames.Length - 1, 1);
					this.ProcessEvents(this.currentClip.loopStart - 1, num3, 1);
				}
				else
				{
					this.ProcessEvents(num, num3, 1);
				}
			}
			else
			{
				this.SetFrameInternal(num3);
				this.ProcessEvents(num, num3, 1);
			}
			break;
		}
		case tk2dSpriteAnimationClip.WrapMode.Once:
		{
			int num5 = (int)this.clipTime;
			if (num5 >= this.currentClip.frames.Length)
			{
				this.SetFrameInternal(this.currentClip.frames.Length - 1);
				this.state &= (tk2dSpriteAnimator.State)(-2);
				this.ProcessEvents(num, this.currentClip.frames.Length - 1, 1);
				this.OnAnimationCompleted();
			}
			else
			{
				this.SetFrameInternal(num5);
				this.ProcessEvents(num, num5, 1);
			}
			break;
		}
		case tk2dSpriteAnimationClip.WrapMode.PingPong:
		{
			int num6 = ((this.currentClip.frames.Length <= 1) ? 0 : (this.currentClip.frames.Length + this.currentClip.frames.Length - 2));
			int num7 = 1;
			if (num6 >= this.currentClip.frames.Length)
			{
				num6 = 2 * this.currentClip.frames.Length - 2 - num6;
				num7 = -1;
			}
			if (num6 < num)
			{
				num7 = -1;
			}
			this.SetFrameInternal(num6);
			this.ProcessEvents(num, num6, num7);
			break;
		}
		case tk2dSpriteAnimationClip.WrapMode.LoopFidget:
		{
			int num8 = (int)this.clipTime % this.currentClip.frames.Length;
			this.SetFrameInternal(num8);
			if (num8 < num)
			{
				this.ProcessEvents(num, this.currentClip.frames.Length - 1, 1);
				this.ProcessEvents(-1, num8, 1);
				this.m_fidgetElapsed = 0f;
				this.m_fidgetDuration = Mathf.Lerp(this.currentClip.minFidgetDuration, this.currentClip.maxFidgetDuration, UnityEngine.Random.value);
			}
			else
			{
				this.ProcessEvents(num, num8, 1);
			}
			break;
		}
		}
	}

	// Token: 0x06003FB5 RID: 16309 RVA: 0x00142D14 File Offset: 0x00140F14
	private void ClipNameError(string name)
	{
		Debug.LogError("Unable to find clip named '" + name + "' in library");
	}

	// Token: 0x06003FB6 RID: 16310 RVA: 0x00142D2C File Offset: 0x00140F2C
	private void ClipIdError(int id)
	{
		Debug.LogError("Play - Invalid clip id '" + id.ToString() + "' in library");
	}

	// Token: 0x06003FB7 RID: 16311 RVA: 0x00142D50 File Offset: 0x00140F50
	private void WarpClipToLocalTime(tk2dSpriteAnimationClip clip, float time, bool skipEvents)
	{
		this.clipTime = time;
		int num = (int)this.clipTime % clip.frames.Length;
		tk2dSpriteAnimationFrame tk2dSpriteAnimationFrame = clip.frames[num];
		if (tk2dSpriteAnimationFrame.requiresOffscreenUpdate)
		{
			this.m_forceNextSpriteUpdate = true;
		}
		this.SetSprite(tk2dSpriteAnimationFrame.spriteCollection, tk2dSpriteAnimationFrame.spriteId);
		if (tk2dSpriteAnimationFrame.requiresOffscreenUpdate)
		{
			this.m_forceNextSpriteUpdate = true;
		}
		if (tk2dSpriteAnimationFrame.triggerEvent && !skipEvents)
		{
			if (this.AnimationEventTriggered != null)
			{
				this.AnimationEventTriggered(this, clip, num);
			}
			if (!base.aiActor && tk2dSpriteAnimationFrame.eventOutline != tk2dSpriteAnimationFrame.OutlineModifier.Unspecified)
			{
				if (tk2dSpriteAnimationFrame.eventOutline == tk2dSpriteAnimationFrame.OutlineModifier.TurnOn && !SpriteOutlineManager.HasOutline(base.sprite))
				{
					SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
				}
				if (tk2dSpriteAnimationFrame.eventOutline == tk2dSpriteAnimationFrame.OutlineModifier.TurnOff && SpriteOutlineManager.HasOutline(base.sprite))
				{
					SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
				}
			}
			if (!string.IsNullOrEmpty(tk2dSpriteAnimationFrame.eventAudio) && !this.MuteAudio)
			{
				AkSoundEngine.PostEvent(tk2dSpriteAnimationFrame.eventAudio, this.AudioBaseObject);
			}
			if (!string.IsNullOrEmpty(tk2dSpriteAnimationFrame.eventVfx) && base.aiAnimator)
			{
				base.aiAnimator.PlayVfx(tk2dSpriteAnimationFrame.eventVfx, null, null, null);
			}
			if (!string.IsNullOrEmpty(tk2dSpriteAnimationFrame.eventStopVfx) && base.aiAnimator)
			{
				base.aiAnimator.StopVfx(tk2dSpriteAnimationFrame.eventStopVfx);
			}
			if (tk2dSpriteAnimationFrame.eventLerpEmissive)
			{
				base.StartCoroutine(this.HandleEmissivePowerLerp(tk2dSpriteAnimationFrame.eventLerpEmissiveTime, tk2dSpriteAnimationFrame.eventLerpEmissivePower));
			}
			if (tk2dSpriteAnimationFrame.forceMaterialUpdate && (!base.aiActor || !base.aiActor.IsBlackPhantom))
			{
				this.Sprite.ForceUpdateMaterial();
			}
		}
		this.previousFrame = num;
	}

	// Token: 0x06003FB8 RID: 16312 RVA: 0x00142F68 File Offset: 0x00141168
	private void SetFrameInternal(int currFrame)
	{
		if (this.previousFrame != currFrame)
		{
			if (this.currentClip.frames[currFrame].requiresOffscreenUpdate)
			{
				this.m_forceNextSpriteUpdate = true;
			}
			this.SetSprite(this.currentClip.frames[currFrame].spriteCollection, this.currentClip.frames[currFrame].spriteId);
			if (this.currentClip.frames[currFrame].requiresOffscreenUpdate)
			{
				this.m_forceNextSpriteUpdate = true;
			}
			this.previousFrame = currFrame;
		}
		if (this.IsFrameBlendedAnimation)
		{
			float num = this.clipTime % 1f;
			base.sprite.renderer.material.SetFloat("_BlendFraction", num);
		}
	}

	// Token: 0x06003FB9 RID: 16313 RVA: 0x00143024 File Offset: 0x00141224
	private void ProcessEvents(int start, int last, int direction)
	{
		if (start == last || Mathf.Sign((float)(last - start)) != Mathf.Sign((float)direction))
		{
			return;
		}
		int num = last + direction;
		tk2dSpriteAnimationFrame[] frames = this.currentClip.frames;
		for (int num2 = start + direction; num2 != num; num2 += direction)
		{
			if (this.ForceSetEveryFrame)
			{
				this.SetFrameInternal(num2);
			}
			if (frames[num2].triggerEvent && this.AnimationEventTriggered != null)
			{
				this.AnimationEventTriggered(this, this.currentClip, num2);
			}
			if (frames[num2].triggerEvent && !string.IsNullOrEmpty(frames[num2].eventAudio) && !this.MuteAudio)
			{
				AkSoundEngine.PostEvent(frames[num2].eventAudio, this.AudioBaseObject);
			}
			if (!string.IsNullOrEmpty(frames[num2].eventVfx) && base.aiAnimator)
			{
				base.aiAnimator.PlayVfx(frames[num2].eventVfx, null, null, null);
			}
			if (!string.IsNullOrEmpty(frames[num2].eventStopVfx) && base.aiAnimator)
			{
				base.aiAnimator.StopVfx(frames[num2].eventStopVfx);
			}
			if (frames[num2].eventLerpEmissive)
			{
				base.StartCoroutine(this.HandleEmissivePowerLerp(frames[num2].eventLerpEmissiveTime, frames[num2].eventLerpEmissivePower));
			}
		}
	}

	// Token: 0x06003FBA RID: 16314 RVA: 0x0014319C File Offset: 0x0014139C
	private IEnumerator HandleEmissivePowerLerp(float duration, float targetPower)
	{
		if (Application.isPlaying)
		{
			Material targetMaterial = base.sprite.renderer.material;
			if (targetMaterial.HasProperty("_EmissivePower"))
			{
				base.sprite.usesOverrideMaterial = true;
				if (duration <= 0f)
				{
					targetMaterial.SetFloat("_EmissivePower", targetPower);
				}
				else
				{
					float elapsed = 0f;
					float startPower = targetMaterial.GetFloat("_EmissivePower");
					while (elapsed < duration)
					{
						elapsed += ((!this.AnimateDuringBossIntros || !GameManager.IsBossIntro) ? BraveTime.DeltaTime : GameManager.INVARIANT_DELTA_TIME);
						float t = elapsed / duration;
						targetMaterial.SetFloat("_EmissivePower", Mathf.Lerp(startPower, targetPower, t));
						yield return null;
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x170009A6 RID: 2470
	// (get) Token: 0x06003FBB RID: 16315 RVA: 0x001431C8 File Offset: 0x001413C8
	// (set) Token: 0x06003FBC RID: 16316 RVA: 0x00143244 File Offset: 0x00141444
	public GameObject AudioBaseObject
	{
		get
		{
			if (this.m_cachedAudioBaseObject == null)
			{
				if (base.transform.parent && base.transform.parent.GetComponent<PlayerController>())
				{
					this.m_cachedAudioBaseObject = base.transform.parent.gameObject;
				}
				else
				{
					this.m_cachedAudioBaseObject = base.gameObject;
				}
			}
			return this.m_cachedAudioBaseObject;
		}
		set
		{
			this.m_cachedAudioBaseObject = value;
		}
	}

	// Token: 0x06003FBD RID: 16317 RVA: 0x00143250 File Offset: 0x00141450
	private void OnAnimationCompleted()
	{
		this.previousFrame = -1;
		if (this.AnimationCompleted != null)
		{
			this.AnimationCompleted(this, this.currentClip);
		}
	}

	// Token: 0x06003FBE RID: 16318 RVA: 0x00143278 File Offset: 0x00141478
	private void HandleVisibilityCheck()
	{
		if (this.alwaysUpdateOffscreen)
		{
			this.m_isCurrentlyVisible = true;
			return;
		}
		if (!tk2dSpriteAnimator.InDungeonScene)
		{
			this.m_isCurrentlyVisible = true;
			return;
		}
		Vector2 vector = base.transform.position.XY() - tk2dSpriteAnimator.CameraPositionThisFrame;
		vector.y *= 1.7f;
		this.m_isCurrentlyVisible = vector.sqrMagnitude < 420f + this.AdditionalCameraVisibilityRadius * this.AdditionalCameraVisibilityRadius;
	}

	// Token: 0x06003FBF RID: 16319 RVA: 0x001432FC File Offset: 0x001414FC
	private float GetDeltaTime()
	{
		float num = ((!this.AnimateDuringBossIntros || !GameManager.IsBossIntro) ? BraveTime.DeltaTime : GameManager.INVARIANT_DELTA_TIME);
		if (base.aiActor)
		{
			num = base.aiActor.LocalDeltaTime;
		}
		if (this.OverrideTimeScale > 0f)
		{
			num *= this.OverrideTimeScale;
		}
		if (this.ignoreTimeScale)
		{
			num = GameManager.INVARIANT_DELTA_TIME;
		}
		if (this.maximumDeltaOneFrame && this.CurrentClip != null)
		{
			num = Mathf.Min(num, 1f / this.CurrentClip.fps);
		}
		return num;
	}

	// Token: 0x06003FC0 RID: 16320 RVA: 0x001433A4 File Offset: 0x001415A4
	public virtual void LateUpdate()
	{
		if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH)
		{
			this.HandleVisibilityCheck();
		}
		this.deferNextStartClip = false;
		this.UpdateAnimation(this.GetDeltaTime());
	}

	// Token: 0x06003FC1 RID: 16321 RVA: 0x001433D0 File Offset: 0x001415D0
	public virtual void SetSprite(tk2dSpriteCollectionData spriteCollection, int spriteId)
	{
		bool flag = this.alwaysUpdateOffscreen;
		if (!this.alwaysUpdateOffscreen)
		{
			flag = base.renderer.isVisible;
			if (Application.isPlaying && GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH)
			{
				flag |= this.m_isCurrentlyVisible;
			}
		}
		if (this.alwaysUpdateOffscreen || !(this.Sprite is tk2dSprite) || flag || !Application.isPlaying || this.m_forceNextSpriteUpdate || this.m_hasAttachPoints)
		{
			this.Sprite.SetSprite(spriteCollection, spriteId);
			this.m_forceNextSpriteUpdate = false;
		}
		else
		{
			this.Sprite.hasOffScreenCachedUpdate = true;
			this.Sprite.offScreenCachedCollection = spriteCollection;
			this.Sprite.offScreenCachedID = spriteId;
		}
	}

	// Token: 0x06003FC2 RID: 16322 RVA: 0x0014349C File Offset: 0x0014169C
	public void OnBecameVisible()
	{
		if (this && this.Sprite && this.Sprite.hasOffScreenCachedUpdate)
		{
			this.Sprite.hasOffScreenCachedUpdate = false;
			this.Sprite.SetSprite(this.Sprite.offScreenCachedCollection, this.Sprite.offScreenCachedID);
			this.Sprite.UpdateZDepth();
		}
	}

	// Token: 0x06003FC3 RID: 16323 RVA: 0x0014350C File Offset: 0x0014170C
	public void ForceInvisibleSpriteUpdate()
	{
		if (this && this.Sprite && this.Sprite.hasOffScreenCachedUpdate)
		{
			this.Sprite.hasOffScreenCachedUpdate = false;
			this.Sprite.SetSprite(this.Sprite.offScreenCachedCollection, this.Sprite.offScreenCachedID);
			this.Sprite.UpdateZDepth();
		}
	}

	// Token: 0x06003FC4 RID: 16324 RVA: 0x0014357C File Offset: 0x0014177C
	public Vector2[] GetNextFrameUVs()
	{
		if (this.state == tk2dSpriteAnimator.State.Playing)
		{
			int num = (this.CurrentFrame + 1) % this.currentClip.frames.Length;
			if (this.CurrentFrame + 1 >= this.currentClip.frames.Length && this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopSection)
			{
				num = this.currentClip.loopStart;
			}
			return this.currentClip.frames[num].spriteCollection.spriteDefinitions[this.currentClip.frames[num].spriteId].uvs;
		}
		return this.Sprite.GetCurrentSpriteDef().uvs;
	}

	// Token: 0x040031BE RID: 12734
	[SerializeField]
	private tk2dSpriteAnimation library;

	// Token: 0x040031BF RID: 12735
	[SerializeField]
	private int defaultClipId;

	// Token: 0x040031C0 RID: 12736
	public float AdditionalCameraVisibilityRadius;

	// Token: 0x040031C1 RID: 12737
	private float m_fidgetDuration;

	// Token: 0x040031C2 RID: 12738
	private float m_fidgetElapsed;

	// Token: 0x040031C3 RID: 12739
	public bool AnimateDuringBossIntros;

	// Token: 0x040031C4 RID: 12740
	public bool AlwaysIgnoreTimeScale;

	// Token: 0x040031C5 RID: 12741
	public bool ForceSetEveryFrame;

	// Token: 0x040031C6 RID: 12742
	public bool playAutomatically;

	// Token: 0x040031C7 RID: 12743
	[NonSerialized]
	public bool alwaysUpdateOffscreen;

	// Token: 0x040031C8 RID: 12744
	[NonSerialized]
	public bool maximumDeltaOneFrame;

	// Token: 0x040031C9 RID: 12745
	[SerializeField]
	public bool IsFrameBlendedAnimation;

	// Token: 0x040031CA RID: 12746
	private static tk2dSpriteAnimator.State globalState;

	// Token: 0x040031CC RID: 12748
	private tk2dSpriteAnimationClip currentClip;

	// Token: 0x040031CD RID: 12749
	public float clipTime;

	// Token: 0x040031CE RID: 12750
	private float clipFps = -1f;

	// Token: 0x040031CF RID: 12751
	private int previousFrame = -1;

	// Token: 0x040031D0 RID: 12752
	public Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> OnPlayAnimationCalled;

	// Token: 0x040031D1 RID: 12753
	public Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> AnimationCompleted;

	// Token: 0x040031D2 RID: 12754
	private Action m_onDestroyAction;

	// Token: 0x040031D3 RID: 12755
	public Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int> AnimationEventTriggered;

	// Token: 0x040031D4 RID: 12756
	private tk2dSpriteAnimator.State state;

	// Token: 0x040031D5 RID: 12757
	public bool deferNextStartClip;

	// Token: 0x040031D6 RID: 12758
	private bool m_hasAttachPoints;

	// Token: 0x040031D7 RID: 12759
	protected tk2dBaseSprite _sprite;

	// Token: 0x040031D8 RID: 12760
	protected tk2dSpriteCollectionData _startingSpriteCollection;

	// Token: 0x040031D9 RID: 12761
	protected int _startingSpriteId;

	// Token: 0x040031DA RID: 12762
	private string m_queuedAnimationName;

	// Token: 0x040031DB RID: 12763
	private GameObject m_overrideTargetDisableObject;

	// Token: 0x040031DC RID: 12764
	private GameObject m_cachedAudioBaseObject;

	// Token: 0x040031DD RID: 12765
	private bool m_isCurrentlyVisible;

	// Token: 0x040031DE RID: 12766
	public static Vector2 CameraPositionThisFrame;

	// Token: 0x040031DF RID: 12767
	public static bool InDungeonScene;

	// Token: 0x040031E0 RID: 12768
	[NonSerialized]
	public bool ignoreTimeScale;

	// Token: 0x040031E1 RID: 12769
	[NonSerialized]
	public float OverrideTimeScale = -1f;

	// Token: 0x040031E2 RID: 12770
	private bool m_forceNextSpriteUpdate;

	// Token: 0x02000BB7 RID: 2999
	private enum State
	{
		// Token: 0x040031E4 RID: 12772
		Init,
		// Token: 0x040031E5 RID: 12773
		Playing,
		// Token: 0x040031E6 RID: 12774
		Paused
	}
}
