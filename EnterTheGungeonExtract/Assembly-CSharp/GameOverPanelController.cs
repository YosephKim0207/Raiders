using System;
using UnityEngine;

// Token: 0x020017DD RID: 6109
public class GameOverPanelController : TimeInvariantMonoBehaviour
{
	// Token: 0x06008FA9 RID: 36777 RVA: 0x003CBFB4 File Offset: 0x003CA1B4
	private void Start()
	{
		this.m_panel = base.GetComponent<dfPanel>();
		this.QuickRestartButton.Click += this.DoQuickRestart;
		this.MainMenuButton.Click += this.DoMainMenu;
	}

	// Token: 0x06008FAA RID: 36778 RVA: 0x003CBFF0 File Offset: 0x003CA1F0
	private void DoMainMenu(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (!this.m_panel.IsVisible)
		{
			return;
		}
		this.m_panel.IsVisible = false;
		dfGUIManager.PopModal();
		Pixelator.Instance.DoFinalNonFadedLayer = false;
		GameUIRoot.Instance.ToggleUICamera(false);
		Pixelator.Instance.FadeToBlack(0.15f, false, 0f);
		GameManager.Instance.DelayedLoadMainMenu(0.15f);
		AkSoundEngine.PostEvent("Play_UI_menu_cancel_01", base.gameObject);
	}

	// Token: 0x06008FAB RID: 36779 RVA: 0x003CC06C File Offset: 0x003CA26C
	private void DoQuickRestart(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (!this.m_panel.IsVisible)
		{
			return;
		}
		this.m_panel.IsVisible = false;
		dfGUIManager.PopModal();
		Pixelator.Instance.DoFinalNonFadedLayer = false;
		GameUIRoot.Instance.ToggleUICamera(false);
		Pixelator.Instance.FadeToBlack(0.15f, false, 0f);
		GameManager.Instance.DelayedQuickRestart(0.15f, default(QuickRestartOptions));
		AkSoundEngine.PostEvent("Play_UI_menu_characterselect_01", base.gameObject);
	}

	// Token: 0x06008FAC RID: 36780 RVA: 0x003CC0F0 File Offset: 0x003CA2F0
	public void Activate()
	{
		this.QuickRestartButton.Focus(true);
		this.deathGuyLeft.ignoresTiltworldDepth = true;
		this.deathGuyRight.ignoresTiltworldDepth = true;
		this.UpdateDeathGuys();
	}

	// Token: 0x06008FAD RID: 36781 RVA: 0x003CC11C File Offset: 0x003CA31C
	protected void UpdateDeathGuys()
	{
		this.deathGuyLeft.scale = GameUIUtility.GetCurrentTK2D_DFScale(this.m_panel.GetManager()) * Vector3.one;
		this.deathGuyRight.scale = this.deathGuyLeft.scale.WithX(this.deathGuyLeft.scale.x * -1f);
		Vector3 vector = (this.m_panel.Size.ToVector3ZUp(0f) + new Vector3(36f, 0f, 0f)) * this.m_panel.PixelsToUnits() * 0.5f;
		this.deathGuyLeft.transform.position = this.m_panel.transform.position - vector + new Vector3(-this.deathGuyLeft.GetBounds().size.x, 0f, 0f);
		this.deathGuyRight.transform.position = this.m_panel.transform.position + new Vector3(vector.x, -vector.y, vector.z) + new Vector3(this.deathGuyRight.GetBounds().size.x, 0f, 0f);
		this.deathGuyLeft.renderer.enabled = true;
		this.deathGuyRight.renderer.enabled = true;
	}

	// Token: 0x06008FAE RID: 36782 RVA: 0x003CC2B4 File Offset: 0x003CA4B4
	protected override void InvariantUpdate(float realDeltaTime)
	{
		if (this.m_panel.IsVisible)
		{
			this.UpdateDeathGuys();
			if (this.deathGuyLeft.renderer.enabled)
			{
				this.deathGuyLeft.spriteAnimator.UpdateAnimation(realDeltaTime);
			}
			if (this.deathGuyRight.renderer.enabled)
			{
				this.deathGuyRight.spriteAnimator.UpdateAnimation(realDeltaTime);
			}
			GameUIRoot.Instance.ForceClearReload(-1);
		}
	}

	// Token: 0x06008FAF RID: 36783 RVA: 0x003CC330 File Offset: 0x003CA530
	public void Deactivate()
	{
		this.deathGuyRight.renderer.enabled = false;
		this.deathGuyLeft.renderer.enabled = false;
	}

	// Token: 0x06008FB0 RID: 36784 RVA: 0x003CC354 File Offset: 0x003CA554
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040097DE RID: 38878
	public dfButton QuickRestartButton;

	// Token: 0x040097DF RID: 38879
	public dfButton MainMenuButton;

	// Token: 0x040097E0 RID: 38880
	public tk2dSprite deathGuyLeft;

	// Token: 0x040097E1 RID: 38881
	public tk2dSprite deathGuyRight;

	// Token: 0x040097E2 RID: 38882
	private dfPanel m_panel;
}
