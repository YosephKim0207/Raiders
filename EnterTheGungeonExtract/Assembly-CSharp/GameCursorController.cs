using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02001778 RID: 6008
public class GameCursorController : MonoBehaviour
{
	// Token: 0x170014ED RID: 5357
	// (get) Token: 0x06008BF5 RID: 35829 RVA: 0x003A4CE4 File Offset: 0x003A2EE4
	private static bool showMouseCursor
	{
		get
		{
			if (GameCursorController.CursorOverride.Value)
			{
				return false;
			}
			if (GameManager.Instance.IsLoadingLevel)
			{
				return false;
			}
			if (GameManager.IsReturningToBreach)
			{
				return false;
			}
			if (GameManager.Instance.IsSelectingCharacter && BraveInput.PlayerlessInstance.IsKeyboardAndMouse(true))
			{
				return true;
			}
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				if (!BraveInput.GetInstanceForPlayer(0).HasMouse() && !BraveInput.GetInstanceForPlayer(1).HasMouse())
				{
					return false;
				}
			}
			else if (!BraveInput.GetInstanceForPlayer(0).HasMouse())
			{
				return false;
			}
			return true;
		}
	}

	// Token: 0x170014EE RID: 5358
	// (get) Token: 0x06008BF6 RID: 35830 RVA: 0x003A4D8C File Offset: 0x003A2F8C
	private static bool showPlayerOneControllerCursor
	{
		get
		{
			return !GameCursorController.CursorOverride.Value && !GameManager.Instance.IsLoadingLevel && !GameManager.IsReturningToBreach && !BraveInput.GetInstanceForPlayer(0).IsKeyboardAndMouse(false) && GameManager.Options.PlayerOneControllerCursor;
		}
	}

	// Token: 0x170014EF RID: 5359
	// (get) Token: 0x06008BF7 RID: 35831 RVA: 0x003A4DEC File Offset: 0x003A2FEC
	private static bool showPlayerTwoControllerCursor
	{
		get
		{
			return GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !GameCursorController.CursorOverride.Value && !GameManager.Instance.IsLoadingLevel && !GameManager.IsReturningToBreach && !BraveInput.GetInstanceForPlayer(1).IsKeyboardAndMouse(false) && GameManager.Options.PlayerTwoControllerCursor;
		}
	}

	// Token: 0x06008BF8 RID: 35832 RVA: 0x003A4E60 File Offset: 0x003A3060
	private void Start()
	{
		GameCursorController.GetClipCursor(out this.m_originalClippingRect);
		Cursor.visible = false;
		Cursor.lockState = ((GameManager.Options.CurrentPreferredFullscreenMode != GameOptions.PreferredFullscreenMode.FULLSCREEN) ? CursorLockMode.None : CursorLockMode.Confined);
	}

	// Token: 0x06008BF9 RID: 35833
	[DllImport("user32.dll")]
	private static extern bool ClipCursor(ref GameCursorController.RECT lpRect);

	// Token: 0x06008BFA RID: 35834
	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool GetClipCursor(out GameCursorController.RECT rcClip);

	// Token: 0x06008BFB RID: 35835 RVA: 0x003A4E90 File Offset: 0x003A3090
	public void ToggleClip(bool clipToWindow)
	{
		if (clipToWindow)
		{
			GameCursorController.RECT rect;
			rect.Left = 0;
			rect.Top = 0;
			rect.Right = Screen.width - 1;
			rect.Bottom = Screen.height - 1;
			GameCursorController.ClipCursor(ref rect);
		}
		else
		{
			GameCursorController.ClipCursor(ref this.m_originalClippingRect);
		}
	}

	// Token: 0x06008BFC RID: 35836 RVA: 0x003A4EE8 File Offset: 0x003A30E8
	private void OnDestroy()
	{
		GameCursorController.ClipCursor(ref this.m_originalClippingRect);
	}

	// Token: 0x06008BFD RID: 35837 RVA: 0x003A4EF8 File Offset: 0x003A30F8
	public void DrawCursor()
	{
		if (!BraveUtility.isLoadingLevel && GameManager.HasInstance && GameManager.Instance.Dungeon != null)
		{
			Cursor.visible = false;
		}
		if (!GameManager.HasInstance)
		{
			return;
		}
		Texture2D texture2D = this.normalCursor;
		int currentCursorIndex = GameManager.Options.CurrentCursorIndex;
		if (currentCursorIndex >= 0 && currentCursorIndex < this.cursors.Length)
		{
			texture2D = this.cursors[currentCursorIndex];
		}
		if (GameCursorController.showMouseCursor)
		{
			Vector2 mousePosition = BraveInput.GetInstanceForPlayer(0).MousePosition;
			mousePosition.y = (float)Screen.height - mousePosition.y;
			Vector2 vector = new Vector2((float)texture2D.width, (float)texture2D.height) * (float)((!(Pixelator.Instance != null)) ? 3 : ((int)Pixelator.Instance.ScaleTileScale));
			Rect rect = new Rect(mousePosition.x + 0.5f - vector.x / 2f, mousePosition.y + 0.5f - vector.y / 2f, vector.x, vector.y);
			bool flag = false;
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && BraveInput.GetInstanceForPlayer(1).IsKeyboardAndMouse(false))
			{
				flag = true;
				Graphics.DrawTexture(rect, texture2D, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, new Color(0.402f, 0.111f, 0.32f));
			}
			if (!flag)
			{
				Graphics.DrawTexture(rect, texture2D);
			}
		}
		if (GameCursorController.showPlayerOneControllerCursor && !GameManager.Instance.IsPaused && !GameManager.IsBossIntro)
		{
			PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
			BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(0);
			if (primaryPlayer && instanceForPlayer.ActiveActions.Aim.Vector != Vector2.zero && (primaryPlayer.CurrentInputState == PlayerInputState.AllInput || primaryPlayer.IsInMinecart))
			{
				Vector2 vector2 = Camera.main.WorldToViewportPoint(primaryPlayer.CenterPosition + instanceForPlayer.ActiveActions.Aim.Vector.normalized * 5f);
				Vector2 vector3 = BraveCameraUtility.ConvertGameViewportToScreenViewport(vector2);
				Vector2 vector4 = new Vector2(vector3.x * (float)Screen.width, (1f - vector3.y) * (float)Screen.height);
				Vector2 vector5 = new Vector2((float)texture2D.width, (float)texture2D.height) * (float)((!(Pixelator.Instance != null)) ? 3 : ((int)Pixelator.Instance.ScaleTileScale));
				Rect rect2 = new Rect(vector4.x + 0.5f - vector5.x / 2f, vector4.y + 0.5f - vector5.y / 2f, vector5.x, vector5.y);
				Graphics.DrawTexture(rect2, texture2D);
			}
		}
		if (GameCursorController.showPlayerTwoControllerCursor && !GameManager.Instance.IsPaused && !GameManager.IsBossIntro)
		{
			PlayerController secondaryPlayer = GameManager.Instance.SecondaryPlayer;
			BraveInput instanceForPlayer2 = BraveInput.GetInstanceForPlayer(1);
			if (secondaryPlayer && instanceForPlayer2.ActiveActions.Aim.Vector != Vector2.zero && (secondaryPlayer.CurrentInputState == PlayerInputState.AllInput || secondaryPlayer.IsInMinecart))
			{
				Vector2 vector6 = Camera.main.WorldToViewportPoint(secondaryPlayer.CenterPosition + instanceForPlayer2.ActiveActions.Aim.Vector.normalized * 5f);
				Vector2 vector7 = BraveCameraUtility.ConvertGameViewportToScreenViewport(vector6);
				Vector2 vector8 = new Vector2(vector7.x * (float)Screen.width, (1f - vector7.y) * (float)Screen.height);
				Vector2 vector9 = new Vector2((float)texture2D.width, (float)texture2D.height) * (float)((!(Pixelator.Instance != null)) ? 3 : ((int)Pixelator.Instance.ScaleTileScale));
				Rect rect3 = new Rect(vector8.x + 0.5f - vector9.x / 2f, vector8.y + 0.5f - vector9.y / 2f, vector9.x, vector9.y);
				Graphics.DrawTexture(rect3, texture2D, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, new Color(0.402f, 0.111f, 0.32f));
			}
		}
		if (GameManager.Options.CurrentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.FULLSCREEN)
		{
			Cursor.lockState = ((!GameManager.Instance.IsPaused || (!GameUIRoot.Instance.PauseMenuPanel.IsVisible && !GameUIRoot.Instance.HasOpenPauseSubmenu())) ? CursorLockMode.Confined : CursorLockMode.None);
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	// Token: 0x06008BFE RID: 35838 RVA: 0x003A541C File Offset: 0x003A361C
	private void OnGUI()
	{
		this.DrawCursor();
	}

	// Token: 0x040092FC RID: 37628
	public static OverridableBool CursorOverride = new OverridableBool(false);

	// Token: 0x040092FD RID: 37629
	public Texture2D normalCursor;

	// Token: 0x040092FE RID: 37630
	public Texture2D[] cursors;

	// Token: 0x040092FF RID: 37631
	public float sizeMultiplier = 4f;

	// Token: 0x04009300 RID: 37632
	private Vector3 lastPosition;

	// Token: 0x04009301 RID: 37633
	private GameCursorController.RECT m_originalClippingRect;

	// Token: 0x02001779 RID: 6009
	public struct RECT
	{
		// Token: 0x04009302 RID: 37634
		public int Left;

		// Token: 0x04009303 RID: 37635
		public int Top;

		// Token: 0x04009304 RID: 37636
		public int Right;

		// Token: 0x04009305 RID: 37637
		public int Bottom;
	}
}
