using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001789 RID: 6025
public class GameUIReloadBarController : BraveBehaviour
{
	// Token: 0x17001508 RID: 5384
	// (get) Token: 0x06008C96 RID: 35990 RVA: 0x003AE818 File Offset: 0x003ACA18
	public bool ReloadIsActive
	{
		get
		{
			return this.m_reloadIsActive;
		}
	}

	// Token: 0x06008C97 RID: 35991 RVA: 0x003AE820 File Offset: 0x003ACA20
	private void Awake()
	{
		if (this.statusBarDrain != null)
		{
			this.StatusBarPanel = this.statusBarDrain.Parent as dfPanel;
		}
	}

	// Token: 0x06008C98 RID: 35992 RVA: 0x003AE84C File Offset: 0x003ACA4C
	private void Start()
	{
		CameraController mainCameraController = GameManager.Instance.MainCameraController;
		mainCameraController.OnFinishedFrame = (Action)Delegate.Combine(mainCameraController.OnFinishedFrame, new Action(this.OnMainCameraFinishedFrame));
	}

	// Token: 0x06008C99 RID: 35993 RVA: 0x003AE87C File Offset: 0x003ACA7C
	public void SetInvisibility(bool visible, string reason)
	{
		this.m_isInvisible.SetOverride(reason, visible, null);
	}

	// Token: 0x06008C9A RID: 35994 RVA: 0x003AE8A0 File Offset: 0x003ACAA0
	public void TriggerReload(PlayerController attachParent, Vector3 offset, float duration, float activeReloadStartPercent, int pixelWidth)
	{
		this.progressSlider.transform.localScale = Vector3.one / GameUIRoot.GameUIScalar;
		this.progressSlider.IsVisible = true;
		this.m_attachPlayer = attachParent;
		this.worldCamera = GameManager.Instance.MainCameraController.GetComponent<Camera>();
		this.uiCamera = this.progressSlider.GetManager().RenderCamera;
		this.m_worldOffset = offset;
		this.m_screenOffset = new Vector3(-this.progressSlider.Width / (2f * GameUIRoot.GameUIScalar) * this.progressSlider.PixelsToUnits(), 0f, 0f);
		this.m_reloadIsActive = true;
		this.activeReloadSprite.enabled = true;
		this.progressSlider.Thumb.enabled = true;
		this.celebrationSprite.enabled = true;
		dfSpriteAnimation component = this.celebrationSprite.GetComponent<dfSpriteAnimation>();
		component.Stop();
		component.SetFrameExternal(0);
		this.celebrationSprite.enabled = false;
		this.progressSlider.Color = Color.white;
		float width = this.progressSlider.Width;
		float maxValue = this.progressSlider.MaxValue;
		float num = (float)this.startValue / maxValue * width;
		float num2 = (float)this.endValue / maxValue * width;
		float num3 = num + (num2 - num) * activeReloadStartPercent;
		float num4 = (float)pixelWidth * Pixelator.Instance.CurrentTileScale;
		this.activeReloadSprite.RelativePosition = GameUIUtility.QuantizeUIPosition(this.activeReloadSprite.RelativePosition.WithX(num3));
		this.celebrationSprite.RelativePosition = this.activeReloadSprite.RelativePosition + new Vector3(Pixelator.Instance.CurrentTileScale * -1f, Pixelator.Instance.CurrentTileScale * -2f, 0f);
		this.activeReloadSprite.Width = num4;
		this.m_activeReloadStartValue = Mathf.RoundToInt((float)(this.endValue - this.startValue) * activeReloadStartPercent) + this.startValue - this.lieFactor / 2;
		this.m_activeReloadEndValue = this.m_activeReloadStartValue + this.lieFactor;
		bool flag = attachParent && attachParent.CurrentGun && attachParent.CurrentGun.LocalActiveReload;
		if (attachParent.IsPrimaryPlayer)
		{
			this.activeReloadSprite.IsVisible = Gun.ActiveReloadActivated || flag;
		}
		else
		{
			this.activeReloadSprite.IsVisible = Gun.ActiveReloadActivatedPlayerTwo || flag;
		}
		base.StartCoroutine(this.HandlePlayerReloadBar(duration));
	}

	// Token: 0x06008C9B RID: 35995 RVA: 0x003AEB3C File Offset: 0x003ACD3C
	public bool IsActiveReloadGracePeriod()
	{
		return this.progressSlider.Value <= 0.3f * this.progressSlider.MaxValue;
	}

	// Token: 0x06008C9C RID: 35996 RVA: 0x003AEB64 File Offset: 0x003ACD64
	public bool AttemptActiveReload()
	{
		if (!this.m_reloadIsActive)
		{
			return false;
		}
		if (this.progressSlider.Value >= (float)this.m_activeReloadStartValue && this.progressSlider.Value <= (float)this.m_activeReloadEndValue)
		{
			this.progressSlider.Color = Color.green;
			AkSoundEngine.PostEvent("Play_WPN_active_reload_01", base.gameObject);
			this.celebrationSprite.enabled = true;
			this.activeReloadSprite.enabled = false;
			this.progressSlider.Thumb.enabled = false;
			this.m_reloadIsActive = false;
			this.celebrationSprite.GetComponent<dfSpriteAnimation>().Play();
			return true;
		}
		this.progressSlider.Color = Color.red;
		return false;
	}

	// Token: 0x06008C9D RID: 35997 RVA: 0x003AEC2C File Offset: 0x003ACE2C
	public void CancelReload()
	{
		this.m_reloadIsActive = false;
		this.m_isReloading = false;
		this.progressSlider.IsVisible = false;
	}

	// Token: 0x06008C9E RID: 35998 RVA: 0x003AEC48 File Offset: 0x003ACE48
	private Vector3 ConvertWorldSpaces(Vector3 inPoint, Camera inCamera, Camera outCamera)
	{
		Vector3 vector = inCamera.WorldToViewportPoint(inPoint);
		return outCamera.ViewportToWorldPoint(vector);
	}

	// Token: 0x06008C9F RID: 35999 RVA: 0x003AEC64 File Offset: 0x003ACE64
	private IEnumerator HandlePlayerReloadBar(float duration)
	{
		this.m_isReloading = true;
		float elapsed = 0f;
		this.activeReloadSprite.RelativePosition = GameUIUtility.QuantizeUIPosition(this.activeReloadSprite.RelativePosition);
		while (this.m_reloadIsActive)
		{
			elapsed += BraveTime.DeltaTime;
			float modifiedElapsed = Mathf.Max(0f, elapsed - this.initialDelay);
			float completedFraction = Mathf.Clamp01(modifiedElapsed / (duration - this.initialDelay));
			int intValue = this.startValue + Mathf.RoundToInt((float)(this.endValue - this.startValue) * completedFraction);
			this.progressSlider.Value = (float)intValue;
			this.progressSlider.IsVisible = !this.m_isInvisible.Value;
			if (elapsed > duration)
			{
				this.m_reloadIsActive = false;
			}
			yield return null;
		}
		elapsed = 0f;
		if (this.m_isReloading)
		{
			while (elapsed < this.finalDelay)
			{
				elapsed += BraveTime.DeltaTime;
				yield return null;
			}
		}
		if (!this.m_reloadIsActive)
		{
			this.progressSlider.Value = (float)this.startValue;
			this.progressSlider.IsVisible = false;
		}
		this.m_isReloading = false;
		yield break;
	}

	// Token: 0x06008CA0 RID: 36000 RVA: 0x003AEC88 File Offset: 0x003ACE88
	public bool AnyStatusBarVisible()
	{
		return this.statusBarDrain.IsVisible || this.statusBarCurse.IsVisible || this.statusBarFire.IsVisible || this.statusBarPoison.IsVisible;
	}

	// Token: 0x06008CA1 RID: 36001 RVA: 0x003AECC8 File Offset: 0x003ACEC8
	public void UpdateStatusBars(PlayerController player)
	{
		if (this.statusBarPoison == null || this.statusBarDrain == null || this.statusBarPoison == null)
		{
			return;
		}
		this.StatusBarPanel.transform.localScale = Vector3.one / GameUIRoot.GameUIScalar;
		if (!player || player.healthHaver.IsDead || GameManager.Instance.IsPaused)
		{
			this.statusBarPoison.IsVisible = false;
			this.statusBarDrain.IsVisible = false;
			this.statusBarFire.IsVisible = false;
			this.statusBarCurse.IsVisible = false;
			return;
		}
		this.m_attachPlayer = player;
		this.worldCamera = GameManager.Instance.MainCameraController.GetComponent<Camera>();
		this.uiCamera = this.progressSlider.GetManager().RenderCamera;
		this.m_worldOffset = new Vector3(0.1f, player.SpriteDimensions.y / 2f + 0.25f, 0f);
		this.m_screenOffset = new Vector3(-this.progressSlider.Width / (2f * GameUIRoot.GameUIScalar) * this.progressSlider.PixelsToUnits(), 0f, 0f);
		if (player.CurrentPoisonMeterValue > 0f)
		{
			this.statusBarPoison.IsVisible = true;
			this.statusBarPoison.Value = player.CurrentPoisonMeterValue;
		}
		else
		{
			this.statusBarPoison.IsVisible = false;
		}
		if (player.CurrentCurseMeterValue > 0f)
		{
			this.statusBarCurse.IsVisible = true;
			this.statusBarCurse.Value = player.CurrentCurseMeterValue;
		}
		else
		{
			this.statusBarCurse.IsVisible = false;
		}
		if (player.IsOnFire)
		{
			this.statusBarFire.IsVisible = true;
			this.statusBarFire.Value = player.CurrentFireMeterValue;
		}
		else
		{
			this.statusBarFire.IsVisible = false;
		}
		if (player.CurrentDrainMeterValue > 0f)
		{
			this.statusBarDrain.IsVisible = true;
			this.statusBarDrain.Value = player.CurrentDrainMeterValue;
		}
		else
		{
			this.statusBarDrain.IsVisible = false;
		}
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			dfProgressBar dfProgressBar = null;
			switch (i)
			{
			case 0:
				dfProgressBar = this.statusBarPoison;
				break;
			case 1:
				dfProgressBar = this.statusBarDrain;
				break;
			case 2:
				dfProgressBar = this.statusBarFire;
				break;
			case 3:
				dfProgressBar = this.statusBarCurse;
				break;
			}
			if (dfProgressBar.IsVisible)
			{
				num++;
			}
		}
		float num2 = 0f;
		int num3 = (num - 1) * 18;
		for (int j = 0; j < 4; j++)
		{
			dfProgressBar dfProgressBar2 = null;
			switch (j)
			{
			case 0:
				dfProgressBar2 = this.statusBarPoison;
				break;
			case 1:
				dfProgressBar2 = this.statusBarDrain;
				break;
			case 2:
				dfProgressBar2 = this.statusBarFire;
				break;
			case 3:
				dfProgressBar2 = this.statusBarCurse;
				break;
			}
			if (dfProgressBar2.IsVisible)
			{
				float num4 = (float)num3;
				if (num3 != 0)
				{
					num4 = Mathf.Lerp((float)(-(float)num3), (float)num3, num2 / ((float)num - 1f));
				}
				dfProgressBar2.RelativePosition = new Vector3(36f, -12f / GameUIRoot.GameUIScalar, 0f) + new Vector3(num4, 0f, 0f);
				num2 += 1f;
			}
		}
	}

	// Token: 0x06008CA2 RID: 36002 RVA: 0x003AF06C File Offset: 0x003AD26C
	private void OnMainCameraFinishedFrame()
	{
		if (this.m_attachPlayer && (this.progressSlider.IsVisible || this.AnyStatusBarVisible()))
		{
			Vector2 vector = this.m_attachPlayer.LockedApproximateSpriteCenter + this.m_worldOffset;
			Vector2 vector2 = this.ConvertWorldSpaces(vector, this.worldCamera, this.uiCamera).WithZ(0f) + this.m_screenOffset;
			this.progressSlider.transform.position = vector2;
			this.progressSlider.transform.position = this.progressSlider.transform.position.QuantizeFloor(this.progressSlider.PixelsToUnits() / (Pixelator.Instance.ScaleTileScale / Pixelator.Instance.CurrentTileScale));
			if (this.StatusBarPanel != null)
			{
				this.StatusBarPanel.transform.position = this.progressSlider.transform.position - new Vector3(0f, -48f * this.progressSlider.PixelsToUnits(), 0f);
			}
		}
	}

	// Token: 0x06008CA3 RID: 36003 RVA: 0x003AF1A8 File Offset: 0x003AD3A8
	protected override void OnDestroy()
	{
		if (GameManager.HasInstance && GameManager.Instance.MainCameraController)
		{
			CameraController mainCameraController = GameManager.Instance.MainCameraController;
			mainCameraController.OnFinishedFrame = (Action)Delegate.Remove(mainCameraController.OnFinishedFrame, new Action(this.OnMainCameraFinishedFrame));
		}
		base.OnDestroy();
	}

	// Token: 0x04009411 RID: 37905
	public dfSprite activeReloadSprite;

	// Token: 0x04009412 RID: 37906
	public dfSlider progressSlider;

	// Token: 0x04009413 RID: 37907
	public dfSprite celebrationSprite;

	// Token: 0x04009414 RID: 37908
	public int startValue;

	// Token: 0x04009415 RID: 37909
	public int endValue;

	// Token: 0x04009416 RID: 37910
	public int lieFactor = 3;

	// Token: 0x04009417 RID: 37911
	public dfFollowObject follower;

	// Token: 0x04009418 RID: 37912
	public float initialDelay = 0.1f;

	// Token: 0x04009419 RID: 37913
	public float finalDelay = 0.25f;

	// Token: 0x0400941A RID: 37914
	[Header("Status Bars")]
	public dfProgressBar statusBarDrain;

	// Token: 0x0400941B RID: 37915
	public dfProgressBar statusBarPoison;

	// Token: 0x0400941C RID: 37916
	public dfProgressBar statusBarFire;

	// Token: 0x0400941D RID: 37917
	public dfProgressBar statusBarCurse;

	// Token: 0x0400941E RID: 37918
	private int m_activeReloadStartValue;

	// Token: 0x0400941F RID: 37919
	private int m_activeReloadEndValue;

	// Token: 0x04009420 RID: 37920
	private bool m_reloadIsActive;

	// Token: 0x04009421 RID: 37921
	private Vector3 m_worldOffset;

	// Token: 0x04009422 RID: 37922
	private Vector3 m_screenOffset;

	// Token: 0x04009423 RID: 37923
	private PlayerController m_attachPlayer;

	// Token: 0x04009424 RID: 37924
	private Camera worldCamera;

	// Token: 0x04009425 RID: 37925
	private Camera uiCamera;

	// Token: 0x04009426 RID: 37926
	private dfPanel StatusBarPanel;

	// Token: 0x04009427 RID: 37927
	private OverridableBool m_isInvisible = new OverridableBool(false);

	// Token: 0x04009428 RID: 37928
	private bool m_isReloading;
}
