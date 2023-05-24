using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using UnityEngine;

// Token: 0x02000385 RID: 901
[AddComponentMenu("Daikon Forge/Tweens/Sprite Animator")]
[Serializable]
public class dfSpriteAnimation : dfTweenPlayableBase
{
	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000F41 RID: 3905 RVA: 0x00047330 File Offset: 0x00045530
	// (remove) Token: 0x06000F42 RID: 3906 RVA: 0x00047368 File Offset: 0x00045568
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification AnimationStarted;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000F43 RID: 3907 RVA: 0x000473A0 File Offset: 0x000455A0
	// (remove) Token: 0x06000F44 RID: 3908 RVA: 0x000473D8 File Offset: 0x000455D8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification AnimationStopped;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000F45 RID: 3909 RVA: 0x00047410 File Offset: 0x00045610
	// (remove) Token: 0x06000F46 RID: 3910 RVA: 0x00047448 File Offset: 0x00045648
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification AnimationPaused;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06000F47 RID: 3911 RVA: 0x00047480 File Offset: 0x00045680
	// (remove) Token: 0x06000F48 RID: 3912 RVA: 0x000474B8 File Offset: 0x000456B8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification AnimationResumed;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000F49 RID: 3913 RVA: 0x000474F0 File Offset: 0x000456F0
	// (remove) Token: 0x06000F4A RID: 3914 RVA: 0x00047528 File Offset: 0x00045728
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification AnimationReset;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000F4B RID: 3915 RVA: 0x00047560 File Offset: 0x00045760
	// (remove) Token: 0x06000F4C RID: 3916 RVA: 0x00047598 File Offset: 0x00045798
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TweenNotification AnimationCompleted;

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x06000F4D RID: 3917 RVA: 0x000475D0 File Offset: 0x000457D0
	// (set) Token: 0x06000F4E RID: 3918 RVA: 0x000475D8 File Offset: 0x000457D8
	public dfAnimationClip Clip
	{
		get
		{
			return this.clip;
		}
		set
		{
			this.clip = value;
		}
	}

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x06000F4F RID: 3919 RVA: 0x000475E4 File Offset: 0x000457E4
	// (set) Token: 0x06000F50 RID: 3920 RVA: 0x000475EC File Offset: 0x000457EC
	public dfComponentMemberInfo Target
	{
		get
		{
			return this.memberInfo;
		}
		set
		{
			this.memberInfo = value;
		}
	}

	// Token: 0x17000353 RID: 851
	// (get) Token: 0x06000F51 RID: 3921 RVA: 0x000475F8 File Offset: 0x000457F8
	// (set) Token: 0x06000F52 RID: 3922 RVA: 0x00047600 File Offset: 0x00045800
	public bool AutoRun
	{
		get
		{
			return this.autoStart;
		}
		set
		{
			this.autoStart = value;
		}
	}

	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06000F53 RID: 3923 RVA: 0x0004760C File Offset: 0x0004580C
	// (set) Token: 0x06000F54 RID: 3924 RVA: 0x00047614 File Offset: 0x00045814
	public float Length
	{
		get
		{
			return this.length;
		}
		set
		{
			this.length = Mathf.Max(value, 0.03f);
		}
	}

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x06000F55 RID: 3925 RVA: 0x00047628 File Offset: 0x00045828
	// (set) Token: 0x06000F56 RID: 3926 RVA: 0x00047630 File Offset: 0x00045830
	public dfTweenLoopType LoopType
	{
		get
		{
			return this.loopType;
		}
		set
		{
			this.loopType = value;
		}
	}

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x06000F57 RID: 3927 RVA: 0x0004763C File Offset: 0x0004583C
	// (set) Token: 0x06000F58 RID: 3928 RVA: 0x00047644 File Offset: 0x00045844
	public dfPlayDirection Direction
	{
		get
		{
			return this.playDirection;
		}
		set
		{
			this.playDirection = value;
			if (this.IsPlaying)
			{
				this.Play();
			}
		}
	}

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x06000F59 RID: 3929 RVA: 0x00047660 File Offset: 0x00045860
	// (set) Token: 0x06000F5A RID: 3930 RVA: 0x00047678 File Offset: 0x00045878
	public bool IsPaused
	{
		get
		{
			return this.isRunning && this.isPaused;
		}
		set
		{
			if (value != this.IsPaused)
			{
				if (value)
				{
					this.Pause();
				}
				else
				{
					this.Resume();
				}
			}
		}
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x000476A0 File Offset: 0x000458A0
	public void Awake()
	{
	}

	// Token: 0x06000F5C RID: 3932 RVA: 0x000476A4 File Offset: 0x000458A4
	public void Start()
	{
		this.m_lastRealtime = Time.realtimeSinceStartup;
		this.m_selfControl = base.GetComponent<dfControl>();
		this.m_cachedBaseClip = this.clip;
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x000476CC File Offset: 0x000458CC
	public void LateUpdate()
	{
		if (this.AutoRun && !this.IsPlaying && !this.autoRunStarted)
		{
			this.autoRunStarted = true;
			this.Play();
		}
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x000476FC File Offset: 0x000458FC
	public void PlayForward()
	{
		this.playDirection = dfPlayDirection.Forward;
		this.Play();
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x0004770C File Offset: 0x0004590C
	public void PlayReverse()
	{
		this.playDirection = dfPlayDirection.Reverse;
		this.Play();
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x0004771C File Offset: 0x0004591C
	public void Pause()
	{
		if (this.isRunning)
		{
			this.isPaused = true;
			this.onPaused();
		}
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x00047738 File Offset: 0x00045938
	public void Resume()
	{
		if (this.isRunning && this.isPaused)
		{
			this.isPaused = false;
			this.onResumed();
		}
	}

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x06000F62 RID: 3938 RVA: 0x00047760 File Offset: 0x00045960
	public override bool IsPlaying
	{
		get
		{
			return this.isRunning;
		}
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x00047768 File Offset: 0x00045968
	public override void Play()
	{
		if (this.IsPlaying)
		{
			this.Stop();
		}
		if (!base.enabled || !base.gameObject.activeSelf || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.memberInfo == null)
		{
			throw new NullReferenceException("Animation target is NULL");
		}
		base.StartCoroutine(this.Execute());
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x000477D8 File Offset: 0x000459D8
	public override void Reset()
	{
		List<string> list = ((!(this.clip != null)) ? null : this.clip.Sprites);
		if (this.memberInfo.IsValid && list != null && list.Count > 0)
		{
			dfSpriteAnimation.SetProperty(this.memberInfo.Component, this.memberInfo.MemberName, list[0]);
		}
		if (!this.isRunning)
		{
			return;
		}
		base.StopAllCoroutines();
		this.isRunning = false;
		this.isPaused = false;
		this.onReset();
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x00047874 File Offset: 0x00045A74
	public override void Stop()
	{
		if (!this.isRunning)
		{
			return;
		}
		List<string> list = ((!(this.clip != null)) ? null : this.clip.Sprites);
		if (this.skipToEndOnStop && list != null)
		{
			this.setFrame(Mathf.Max(list.Count - 1, 0));
		}
		base.StopAllCoroutines();
		this.isRunning = false;
		this.isPaused = false;
		this.onStopped();
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x06000F66 RID: 3942 RVA: 0x000478F0 File Offset: 0x00045AF0
	// (set) Token: 0x06000F67 RID: 3943 RVA: 0x000478F8 File Offset: 0x00045AF8
	public override string TweenName
	{
		get
		{
			return this.animationName;
		}
		set
		{
			this.animationName = value;
		}
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x00047904 File Offset: 0x00045B04
	protected void onPaused()
	{
		base.SendMessage("AnimationPaused", this, SendMessageOptions.DontRequireReceiver);
		if (this.AnimationPaused != null)
		{
			this.AnimationPaused(this);
		}
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x0004792C File Offset: 0x00045B2C
	protected void onResumed()
	{
		base.SendMessage("AnimationResumed", this, SendMessageOptions.DontRequireReceiver);
		if (this.AnimationResumed != null)
		{
			this.AnimationResumed(this);
		}
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x00047954 File Offset: 0x00045B54
	protected void onStarted()
	{
		base.SendMessage("AnimationStarted", this, SendMessageOptions.DontRequireReceiver);
		if (this.AnimationStarted != null)
		{
			this.AnimationStarted(this);
		}
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x0004797C File Offset: 0x00045B7C
	protected void onStopped()
	{
		base.SendMessage("AnimationStopped", this, SendMessageOptions.DontRequireReceiver);
		if (this.AnimationStopped != null)
		{
			this.AnimationStopped(this);
		}
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x000479A4 File Offset: 0x00045BA4
	protected void onReset()
	{
		base.SendMessage("AnimationReset", this, SendMessageOptions.DontRequireReceiver);
		if (this.AnimationReset != null)
		{
			this.AnimationReset(this);
		}
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x000479CC File Offset: 0x00045BCC
	protected void onCompleted()
	{
		base.SendMessage("AnimationCompleted", this, SendMessageOptions.DontRequireReceiver);
		if (this.AnimationCompleted != null)
		{
			this.AnimationCompleted(this);
		}
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x000479F4 File Offset: 0x00045BF4
	internal static void SetProperty(object target, string property, object value)
	{
		if (target == null)
		{
			throw new NullReferenceException("Target is null");
		}
		MemberInfo[] member = target.GetType().GetMember(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (member == null || member.Length == 0)
		{
			throw new IndexOutOfRangeException("Property not found: " + property);
		}
		MemberInfo memberInfo = member[0];
		if (memberInfo is FieldInfo)
		{
			((FieldInfo)memberInfo).SetValue(target, value);
			return;
		}
		if (memberInfo is PropertyInfo)
		{
			((PropertyInfo)memberInfo).SetValue(target, value, null);
			return;
		}
		throw new InvalidOperationException("Member type not supported: " + memberInfo.GetMemberType());
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x00047A94 File Offset: 0x00045C94
	private IEnumerator Execute()
	{
		dfSprite localTargetSprite = this.memberInfo.Component as dfSprite;
		if (this.myRandom == null)
		{
			this.myRandom = new System.Random();
		}
		if (this.clip == null || this.clip.Sprites == null || this.clip.Sprites.Count == 0)
		{
			yield break;
		}
		this.isRunning = true;
		this.isPaused = false;
		this.onStarted();
		this.m_elapsedSinceLoop = 0f;
		this.m_lastRealtime = Time.realtimeSinceStartup;
		int direction = ((this.playDirection != dfPlayDirection.Forward) ? (-1) : 1);
		int lastFrameIndex = ((direction != 1) ? (this.clip.Sprites.Count - 1) : 0);
		this.setFrame(lastFrameIndex);
		for (;;)
		{
			yield return null;
			if (!localTargetSprite || localTargetSprite.IsVisible)
			{
				float localDeltaTime = Time.realtimeSinceStartup - this.m_lastRealtime;
				if (this.maxOneFrameDelta)
				{
					localDeltaTime = Mathf.Min(localDeltaTime, 1f / ((float)this.clip.Sprites.Count / this.length));
				}
				if (!this.IsPaused)
				{
					List<string> sprites = this.clip.Sprites;
					int maxFrameIndex = sprites.Count - 1;
					int testFrameIndex = Mathf.FloorToInt(Mathf.Clamp01(this.m_elapsedSinceLoop / this.length) * (float)sprites.Count);
					if (this.loopType == dfTweenLoopType.LoopSection && this.LoopSectionFirstLength > 0f && testFrameIndex < this.LoopSectionFrameTarget)
					{
						float num = this.length / this.LoopSectionFirstLength;
						this.m_elapsedSinceLoop += localDeltaTime * num;
					}
					else
					{
						this.m_elapsedSinceLoop += localDeltaTime;
					}
					this.m_lastRealtime = Time.realtimeSinceStartup;
					int frameIndex = Mathf.FloorToInt(Mathf.Clamp01(this.m_elapsedSinceLoop / this.length) * (float)sprites.Count);
					if (this.m_elapsedSinceLoop >= this.length)
					{
						switch (this.loopType)
						{
						case dfTweenLoopType.Once:
							this.isRunning = false;
							this.onCompleted();
							break;
						case dfTweenLoopType.Loop:
							this.m_elapsedSinceLoop = 0f;
							frameIndex = 0;
							if (this.alternativeLoopClip != null && this.clip != this.alternativeLoopClip)
							{
								if ((float)this.myRandom.NextDouble() < this.percentChanceToPlayAlternative)
								{
									this.clip = this.alternativeLoopClip;
								}
							}
							else if (this.clip != this.m_cachedBaseClip)
							{
								this.clip = this.m_cachedBaseClip;
							}
							this.m_elapsedSinceLoop = 0f;
							if (this.LoopDelayMax > 0f)
							{
								float delay = UnityEngine.Random.Range(this.LoopDelayMin, this.LoopDelayMax);
								float ela = 0f;
								while (ela < delay)
								{
									ela += GameManager.INVARIANT_DELTA_TIME;
									yield return null;
								}
								this.m_elapsedSinceLoop = 0f;
								this.m_lastRealtime = Time.realtimeSinceStartup;
							}
							break;
						case dfTweenLoopType.PingPong:
							this.m_elapsedSinceLoop = 0f;
							direction *= -1;
							frameIndex = 0;
							break;
						case dfTweenLoopType.LoopSection:
							frameIndex = this.LoopSectionFrameTarget;
							this.m_elapsedSinceLoop = (float)this.LoopSectionFrameTarget / (float)sprites.Count * this.length;
							if (this.LoopDelayMax > 0f)
							{
								float delay2 = UnityEngine.Random.Range(this.LoopDelayMin, this.LoopDelayMax);
								float ela2 = 0f;
								while (ela2 < delay2)
								{
									ela2 += GameManager.INVARIANT_DELTA_TIME;
									yield return null;
								}
								this.m_elapsedSinceLoop = (float)this.LoopSectionFrameTarget / (float)sprites.Count * this.length;
								this.m_lastRealtime = Time.realtimeSinceStartup;
							}
							break;
						}
					}
					if (direction == -1)
					{
						frameIndex = maxFrameIndex - frameIndex;
					}
					if (lastFrameIndex != frameIndex)
					{
						lastFrameIndex = frameIndex;
						this.setFrame(frameIndex);
					}
					if (!this.isRunning)
					{
						break;
					}
				}
			}
		}
		yield break;
		yield break;
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x00047AB0 File Offset: 0x00045CB0
	private string getPath(Transform obj)
	{
		StringBuilder stringBuilder = new StringBuilder();
		while (obj != null)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Insert(0, "\\");
				stringBuilder.Insert(0, obj.name);
			}
			else
			{
				stringBuilder.Append(obj.name);
			}
			obj = obj.parent;
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x00047B1C File Offset: 0x00045D1C
	public void SetFrameExternal(int index)
	{
		this.setFrame(index);
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x00047B28 File Offset: 0x00045D28
	private void setFrame(int frameIndex)
	{
		List<string> sprites = this.clip.Sprites;
		if (sprites.Count == 0)
		{
			return;
		}
		frameIndex = Mathf.Max(0, Mathf.Min(frameIndex, sprites.Count - 1));
		if (this.memberInfo != null)
		{
			dfSprite dfSprite = this.memberInfo.Component as dfSprite;
			if (dfSprite)
			{
				dfSprite.SpriteName = sprites[frameIndex];
			}
			if (this.m_selfControl != null)
			{
				this.m_selfControl.Invalidate();
			}
		}
	}

	// Token: 0x04000EA0 RID: 3744
	[SerializeField]
	private string animationName = "ANIMATION";

	// Token: 0x04000EA1 RID: 3745
	[SerializeField]
	private dfAnimationClip clip;

	// Token: 0x04000EA2 RID: 3746
	[SerializeField]
	public dfAnimationClip alternativeLoopClip;

	// Token: 0x04000EA3 RID: 3747
	[SerializeField]
	public float percentChanceToPlayAlternative = 0.05f;

	// Token: 0x04000EA4 RID: 3748
	private dfAnimationClip m_cachedBaseClip;

	// Token: 0x04000EA5 RID: 3749
	[SerializeField]
	private dfComponentMemberInfo memberInfo = new dfComponentMemberInfo();

	// Token: 0x04000EA6 RID: 3750
	[SerializeField]
	private dfTweenLoopType loopType = dfTweenLoopType.Loop;

	// Token: 0x04000EA7 RID: 3751
	[SerializeField]
	public int LoopSectionFrameTarget = 7;

	// Token: 0x04000EA8 RID: 3752
	[SerializeField]
	public float LoopSectionFirstLength = -1f;

	// Token: 0x04000EA9 RID: 3753
	[SerializeField]
	public float LoopDelayMin;

	// Token: 0x04000EAA RID: 3754
	[SerializeField]
	public float LoopDelayMax;

	// Token: 0x04000EAB RID: 3755
	[SerializeField]
	public bool maxOneFrameDelta;

	// Token: 0x04000EAC RID: 3756
	[SerializeField]
	private float length = 1f;

	// Token: 0x04000EAD RID: 3757
	[SerializeField]
	private bool autoStart;

	// Token: 0x04000EAE RID: 3758
	[SerializeField]
	private bool skipToEndOnStop;

	// Token: 0x04000EAF RID: 3759
	[SerializeField]
	private dfPlayDirection playDirection;

	// Token: 0x04000EB0 RID: 3760
	private bool autoRunStarted;

	// Token: 0x04000EB1 RID: 3761
	private bool isRunning;

	// Token: 0x04000EB2 RID: 3762
	private bool isPaused;

	// Token: 0x04000EB3 RID: 3763
	private dfControl m_selfControl;

	// Token: 0x04000EB4 RID: 3764
	public bool UseDefaultSpriteNameProperty = true;

	// Token: 0x04000EB5 RID: 3765
	private System.Random myRandom;

	// Token: 0x04000EB6 RID: 3766
	private float m_elapsedSinceLoop;

	// Token: 0x04000EB7 RID: 3767
	private float m_lastRealtime;
}
