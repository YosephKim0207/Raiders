using System;
using UnityEngine;

// Token: 0x0200172B RID: 5931
public class AmmonomiconBookmarkController : MonoBehaviour
{
	// Token: 0x17001470 RID: 5232
	// (get) Token: 0x060089B4 RID: 35252 RVA: 0x00394568 File Offset: 0x00392768
	public bool IsFocused
	{
		get
		{
			return this.m_sprite.HasFocus;
		}
	}

	// Token: 0x17001471 RID: 5233
	// (get) Token: 0x060089B5 RID: 35253 RVA: 0x00394578 File Offset: 0x00392778
	// (set) Token: 0x060089B6 RID: 35254 RVA: 0x00394580 File Offset: 0x00392780
	public bool IsCurrentPage
	{
		get
		{
			return this.m_isCurrentPage;
		}
		set
		{
			if (this.m_isCurrentPage != value)
			{
				int currentlySelectedTabIndex = this.m_ammonomiconInstance.CurrentlySelectedTabIndex;
				this.m_isCurrentPage = value;
				if (this.m_isCurrentPage)
				{
					this.m_sprite.BackgroundSprite = this.DeselectSelectedSpriteName;
					this.TriggerSelectedAnimation();
					for (int i = 0; i < this.m_ammonomiconInstance.bookmarks.Length; i++)
					{
						if (this.m_ammonomiconInstance.bookmarks[i] != this && this.m_ammonomiconInstance.bookmarks[i].IsCurrentPage)
						{
							this.m_ammonomiconInstance.bookmarks[i].IsCurrentPage = false;
						}
					}
					this.m_ammonomiconInstance.CurrentlySelectedTabIndex = Array.IndexOf<AmmonomiconBookmarkController>(this.m_ammonomiconInstance.bookmarks, this);
					if (this.m_ammonomiconInstance.CurrentlySelectedTabIndex > currentlySelectedTabIndex)
					{
						AmmonomiconController.Instance.TurnToNextPage(this.TargetNewPageLeft, this.LeftPageType, this.TargetNewPageRight, this.RightPageType);
					}
					else if (this.m_ammonomiconInstance.CurrentlySelectedTabIndex < currentlySelectedTabIndex)
					{
						AmmonomiconController.Instance.TurnToPreviousPage(this.TargetNewPageLeft, this.LeftPageType, this.TargetNewPageRight, this.RightPageType);
					}
					this.m_sprite.Focus(true);
				}
				else
				{
					this.m_animator.Stop();
					this.m_sprite.BackgroundSprite = this.AppearClip.Sprites[this.AppearClip.Sprites.Count - 1];
				}
			}
		}
	}

	// Token: 0x060089B7 RID: 35255 RVA: 0x00394700 File Offset: 0x00392900
	private void Start()
	{
		this.m_sprite = base.GetComponent<dfButton>();
		this.m_ammonomiconInstance = this.m_sprite.GetManager().GetComponent<AmmonomiconInstanceManager>();
		this.m_sprite.IsVisible = false;
		this.m_animator = base.gameObject.AddComponent<dfSpriteAnimation>();
		this.m_animator.LoopType = dfTweenLoopType.Once;
		this.m_animator.Target = new dfComponentMemberInfo();
		dfComponentMemberInfo target = this.m_animator.Target;
		target.Component = this.m_sprite;
		target.MemberName = "BackgroundSprite";
		this.m_animator.Clip = this.AppearClip;
		this.m_animator.Length = 0.35f;
		this.m_sprite.MouseEnter += this.OnMouseEnter;
		this.m_sprite.MouseLeave += this.OnMouseLeave;
		this.m_sprite.GotFocus += this.Focus;
		this.m_sprite.LostFocus += this.Defocus;
		this.m_sprite.Click += this.SelectThisTab;
		UIKeyControls component = base.GetComponent<UIKeyControls>();
		if (component != null)
		{
			UIKeyControls uikeyControls = component;
			uikeyControls.OnRightDown = (Action)Delegate.Combine(uikeyControls.OnRightDown, new Action(delegate
			{
				if (AmmonomiconController.Instance.ImpendingLeftPageRenderer != null && AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget != null)
				{
					InControlInputAdapter.SkipInputForRestOfFrame = true;
					AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget.Focus(true);
				}
				else if (AmmonomiconController.Instance.CurrentLeftPageRenderer.LastFocusTarget != null)
				{
					InControlInputAdapter.SkipInputForRestOfFrame = true;
					AmmonomiconController.Instance.CurrentLeftPageRenderer.LastFocusTarget.Focus(true);
				}
				else if (AmmonomiconController.Instance.CurrentRightPageRenderer.LastFocusTarget != null)
				{
					InControlInputAdapter.SkipInputForRestOfFrame = true;
					AmmonomiconController.Instance.CurrentRightPageRenderer.LastFocusTarget.Focus(true);
				}
			}));
		}
	}

	// Token: 0x060089B8 RID: 35256 RVA: 0x00394868 File Offset: 0x00392A68
	private void OnMouseLeave(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (!this.m_animator.IsPlaying && !this.m_sprite.HasFocus)
		{
			this.Defocus(control, null);
		}
	}

	// Token: 0x060089B9 RID: 35257 RVA: 0x00394894 File Offset: 0x00392A94
	private void OnMouseEnter(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (!this.m_animator.IsPlaying)
		{
			if (this.IsCurrentPage)
			{
				this.m_sprite.BackgroundSprite = this.SelectClip.Sprites[this.SelectClip.Sprites.Count - 1];
			}
			else
			{
				this.m_sprite.BackgroundSprite = this.SelectSpriteName;
			}
			this.m_sprite.Focus(true);
		}
	}

	// Token: 0x060089BA RID: 35258 RVA: 0x0039490C File Offset: 0x00392B0C
	public void Enable()
	{
		this.m_sprite.Enable();
		this.m_sprite.IsVisible = true;
		this.m_sprite.IsInteractive = true;
		this.m_sprite.Click += this.SelectThisTab;
	}

	// Token: 0x060089BB RID: 35259 RVA: 0x00394948 File Offset: 0x00392B48
	public void Disable()
	{
		this.m_animator.Stop();
		this.m_sprite.Disable();
		this.m_sprite.IsVisible = false;
		this.m_sprite.IsInteractive = false;
		this.m_sprite.Click -= this.SelectThisTab;
	}

	// Token: 0x060089BC RID: 35260 RVA: 0x0039499C File Offset: 0x00392B9C
	public void ForceFocus()
	{
		if (this.m_sprite != null)
		{
			this.m_sprite.Focus(true);
		}
	}

	// Token: 0x060089BD RID: 35261 RVA: 0x003949BC File Offset: 0x00392BBC
	private void SelectThisTab(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.IsCurrentPage = true;
	}

	// Token: 0x060089BE RID: 35262 RVA: 0x003949C8 File Offset: 0x00392BC8
	private void Defocus(dfControl control, dfFocusEventArgs args)
	{
		this.m_animator.Stop();
		if (this.IsCurrentPage)
		{
			this.m_sprite.BackgroundSprite = this.DeselectSelectedSpriteName;
		}
		else
		{
			this.m_sprite.BackgroundSprite = this.AppearClip.Sprites[this.AppearClip.Sprites.Count - 1];
		}
	}

	// Token: 0x060089BF RID: 35263 RVA: 0x00394A30 File Offset: 0x00392C30
	public void Focus(dfControl control, dfFocusEventArgs args)
	{
		if (this.IsCurrentPage)
		{
			this.m_sprite.BackgroundSprite = this.SelectClip.Sprites[this.SelectClip.Sprites.Count - 1];
		}
		else
		{
			this.m_sprite.BackgroundSprite = this.SelectSpriteName;
		}
	}

	// Token: 0x060089C0 RID: 35264 RVA: 0x00394A8C File Offset: 0x00392C8C
	public void TriggerAppearAnimation()
	{
		this.Enable();
		if (!this.IsCurrentPage)
		{
			this.m_animator.Clip = this.AppearClip;
			this.m_animator.Length = 0.35f;
			this.m_animator.Play();
		}
		else
		{
			this.TriggerSelectedAnimation();
		}
	}

	// Token: 0x060089C1 RID: 35265 RVA: 0x00394AE4 File Offset: 0x00392CE4
	public void TriggerSelectedAnimation()
	{
		this.m_sprite.IsVisible = true;
		this.m_animator.Clip = this.SelectClip;
		this.m_animator.Length = 0.275f;
		this.m_animator.Play();
	}

	// Token: 0x04009015 RID: 36885
	public dfAnimationClip AppearClip;

	// Token: 0x04009016 RID: 36886
	public string SelectSpriteName;

	// Token: 0x04009017 RID: 36887
	public dfAnimationClip SelectClip;

	// Token: 0x04009018 RID: 36888
	public string DeselectSelectedSpriteName;

	// Token: 0x04009019 RID: 36889
	public string TargetNewPageLeft;

	// Token: 0x0400901A RID: 36890
	public AmmonomiconPageRenderer.PageType LeftPageType;

	// Token: 0x0400901B RID: 36891
	public string TargetNewPageRight;

	// Token: 0x0400901C RID: 36892
	public AmmonomiconPageRenderer.PageType RightPageType;

	// Token: 0x0400901D RID: 36893
	private bool m_isCurrentPage;

	// Token: 0x0400901E RID: 36894
	private dfButton m_sprite;

	// Token: 0x0400901F RID: 36895
	private dfSpriteAnimation m_animator;

	// Token: 0x04009020 RID: 36896
	private AmmonomiconInstanceManager m_ammonomiconInstance;
}
