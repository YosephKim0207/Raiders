using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020016CA RID: 5834
public class SpriteAnimatorKiller : MonoBehaviour
{
	// Token: 0x060087B3 RID: 34739 RVA: 0x00384424 File Offset: 0x00382624
	public void Awake()
	{
		if (!this.animator)
		{
			this.animator = base.GetComponent<tk2dSpriteAnimator>();
		}
		if (!this.dfAnimation)
		{
			this.dfAnimation = base.GetComponent<dfSpriteAnimation>();
		}
		this.m_renderer = ((!this.animator) ? base.GetComponent<Renderer>() : this.animator.renderer);
	}

	// Token: 0x060087B4 RID: 34740 RVA: 0x00384498 File Offset: 0x00382698
	public void Start()
	{
		if (!this.m_initialized && base.enabled)
		{
			if (this.onlyDisable)
			{
				this.m_renderer.enabled = true;
				this.animator.enabled = true;
			}
			if (this.animator != null)
			{
				tk2dSpriteAnimator tk2dSpriteAnimator = this.animator;
				tk2dSpriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(tk2dSpriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnAnimationCompleted));
			}
			if (this.dfAnimation != null)
			{
				this.dfAnimation.AnimationCompleted += this.dfAnimationComplete;
			}
			if (this.deparentOnStart)
			{
				base.transform.parent = SpawnManager.Instance.VFX;
			}
			this.m_initialized = true;
		}
	}

	// Token: 0x060087B5 RID: 34741 RVA: 0x00384568 File Offset: 0x00382768
	public void Update()
	{
		if (this.m_killTimer > 0f)
		{
			this.m_killTimer -= BraveTime.DeltaTime;
			if (this.m_killTimer <= 0f)
			{
				this.BeginDeath();
			}
		}
		else if (this.m_fadeTimer > 0f)
		{
			this.m_fadeTimer -= BraveTime.DeltaTime;
			this.animator.sprite.color = this.animator.sprite.color.WithAlpha(Mathf.Clamp01(this.m_fadeTimer / this.fadeTime));
			if (this.m_fadeTimer <= 0f)
			{
				this.FinishDeath();
			}
		}
	}

	// Token: 0x060087B6 RID: 34742 RVA: 0x00384624 File Offset: 0x00382824
	public void OnSpawned()
	{
		if (base.enabled)
		{
			this.Start();
		}
		if (this.hasChildAnimators)
		{
			SpriteAnimatorKiller[] componentsInChildren = base.GetComponentsInChildren<SpriteAnimatorKiller>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!(componentsInChildren[i] == this))
				{
					componentsInChildren[i].OnSpawned();
				}
			}
		}
	}

	// Token: 0x060087B7 RID: 34743 RVA: 0x00384684 File Offset: 0x00382884
	public void OnDespawned()
	{
		this.Cleanup();
		if (this.hasChildAnimators)
		{
			SpriteAnimatorKiller[] componentsInChildren = base.GetComponentsInChildren<SpriteAnimatorKiller>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!(componentsInChildren[i] == this))
				{
					componentsInChildren[i].OnDespawned();
				}
			}
		}
	}

	// Token: 0x060087B8 RID: 34744 RVA: 0x003846D8 File Offset: 0x003828D8
	public void Cleanup()
	{
		if (this.animator != null)
		{
			tk2dSpriteAnimator tk2dSpriteAnimator = this.animator;
			tk2dSpriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(tk2dSpriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnAnimationCompleted));
		}
		if (this.dfAnimation != null)
		{
			this.dfAnimation.AnimationCompleted -= this.dfAnimationComplete;
		}
		if (this && base.transform && SpawnManager.Instance && base.transform.parent != SpawnManager.Instance.VFX && (base.transform.parent == null || base.transform.parent.GetComponent<SpriteAnimatorKiller>() == null))
		{
			base.transform.parent = SpawnManager.Instance.VFX;
		}
		this.m_initialized = false;
	}

	// Token: 0x060087B9 RID: 34745 RVA: 0x003847DC File Offset: 0x003829DC
	public void Restart()
	{
		if (this.animator)
		{
			this.animator.enabled = true;
			this.animator.PlayFrom(0f);
		}
		if (this.dfAnimation)
		{
			Debug.LogWarning("unsupported");
		}
		this.m_renderer.enabled = true;
		for (int i = 0; i < this.childObjectToDisable.Count; i++)
		{
			this.childObjectToDisable[i].SetActive(true);
		}
	}

	// Token: 0x060087BA RID: 34746 RVA: 0x0038486C File Offset: 0x00382A6C
	public void Disable()
	{
		if (this.animator)
		{
			this.animator.enabled = false;
		}
		if (this.dfAnimation)
		{
			this.dfAnimation.enabled = false;
		}
		this.m_renderer.enabled = false;
		for (int i = 0; i < this.childObjectToDisable.Count; i++)
		{
			this.childObjectToDisable[i].SetActive(false);
		}
	}

	// Token: 0x060087BB RID: 34747 RVA: 0x003848EC File Offset: 0x00382AEC
	public void OnAnimationCompleted(tk2dSpriteAnimator a, tk2dSpriteAnimationClip c)
	{
		if (this.delayDestructionTime > 0f)
		{
			if (this.m_renderer && this.disableRendererOnDelay)
			{
				this.m_renderer.enabled = false;
			}
			this.m_killTimer = this.delayDestructionTime;
		}
		else
		{
			this.BeginDeath();
		}
	}

	// Token: 0x060087BC RID: 34748 RVA: 0x00384948 File Offset: 0x00382B48
	public void dfAnimationComplete(dfTweenPlayableBase source)
	{
		if (this.delayDestructionTime > 0f)
		{
			this.m_killTimer = this.delayDestructionTime;
		}
		else
		{
			this.BeginDeath();
		}
	}

	// Token: 0x060087BD RID: 34749 RVA: 0x00384974 File Offset: 0x00382B74
	private void BeginDeath()
	{
		if (this.fadeTime > 0f)
		{
			this.m_fadeTimer = this.fadeTime;
		}
		else
		{
			this.FinishDeath();
		}
	}

	// Token: 0x060087BE RID: 34750 RVA: 0x003849A0 File Offset: 0x00382BA0
	private void FinishDeath()
	{
		if (!this || !base.transform)
		{
			return;
		}
		if (this.deparentAllChildren)
		{
			while (base.transform.childCount > 0)
			{
				base.transform.GetChild(0).parent = base.transform.parent;
			}
		}
		if (this.fadeTime > 0f && this.animator && this.animator.sprite)
		{
			this.animator.sprite.color = this.animator.sprite.color.WithAlpha(1f);
		}
		if (this.onlyDisable)
		{
			this.Disable();
			return;
		}
		this.Cleanup();
		SpawnManager.Despawn(base.gameObject);
	}

	// Token: 0x04008CE1 RID: 36065
	public bool onlyDisable;

	// Token: 0x04008CE2 RID: 36066
	[FormerlySerializedAs("deparentOnAwake")]
	public bool deparentOnStart;

	// Token: 0x04008CE3 RID: 36067
	public List<GameObject> childObjectToDisable;

	// Token: 0x04008CE4 RID: 36068
	public tk2dSpriteAnimator animator;

	// Token: 0x04008CE5 RID: 36069
	public dfSpriteAnimation dfAnimation;

	// Token: 0x04008CE6 RID: 36070
	public bool hasChildAnimators;

	// Token: 0x04008CE7 RID: 36071
	public bool deparentAllChildren;

	// Token: 0x04008CE8 RID: 36072
	public bool disableRendererOnDelay;

	// Token: 0x04008CE9 RID: 36073
	public float delayDestructionTime;

	// Token: 0x04008CEA RID: 36074
	public float fadeTime;

	// Token: 0x04008CEB RID: 36075
	private bool m_initialized;

	// Token: 0x04008CEC RID: 36076
	private Renderer m_renderer;

	// Token: 0x04008CED RID: 36077
	private float m_killTimer;

	// Token: 0x04008CEE RID: 36078
	private float m_fadeTimer;
}
