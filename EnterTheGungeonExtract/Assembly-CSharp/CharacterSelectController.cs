using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020017A9 RID: 6057
public class CharacterSelectController : MonoBehaviour
{
	// Token: 0x06008DBF RID: 36287 RVA: 0x003BA1F8 File Offset: 0x003B83F8
	public static string GetCharacterPathFromIdentity(PlayableCharacters id)
	{
		switch (id)
		{
		case PlayableCharacters.Pilot:
			return "PlayerRogue";
		case PlayableCharacters.Convict:
			return "PlayerConvict";
		case PlayableCharacters.Robot:
			return "PlayerRobot";
		case PlayableCharacters.Soldier:
			return "PlayerMarine";
		case PlayableCharacters.Guide:
			return "PlayerGuide";
		case PlayableCharacters.CoopCultist:
			return "PlayerCoopCultist";
		case PlayableCharacters.Bullet:
			return "PlayerBullet";
		case PlayableCharacters.Eevee:
			return "PlayerEevee";
		case PlayableCharacters.Gunslinger:
			return "PlayerGunslinger";
		}
		return "PlayerRogue";
	}

	// Token: 0x06008DC0 RID: 36288 RVA: 0x003BA278 File Offset: 0x003B8478
	public static string GetCharacterPathFromQuickStart()
	{
		GameOptions.QuickstartCharacter quickstartCharacter = GameManager.Options.PreferredQuickstartCharacter;
		if (quickstartCharacter == GameOptions.QuickstartCharacter.LAST_USED)
		{
			switch (GameManager.Options.LastPlayedCharacter)
			{
			case PlayableCharacters.Pilot:
				quickstartCharacter = GameOptions.QuickstartCharacter.PILOT;
				goto IL_7C;
			case PlayableCharacters.Convict:
				quickstartCharacter = GameOptions.QuickstartCharacter.CONVICT;
				goto IL_7C;
			case PlayableCharacters.Robot:
				quickstartCharacter = GameOptions.QuickstartCharacter.ROBOT;
				goto IL_7C;
			case PlayableCharacters.Soldier:
				quickstartCharacter = GameOptions.QuickstartCharacter.SOLDIER;
				goto IL_7C;
			case PlayableCharacters.Guide:
				quickstartCharacter = GameOptions.QuickstartCharacter.GUIDE;
				goto IL_7C;
			case PlayableCharacters.Bullet:
				quickstartCharacter = GameOptions.QuickstartCharacter.BULLET;
				goto IL_7C;
			}
			quickstartCharacter = GameOptions.QuickstartCharacter.PILOT;
		}
		IL_7C:
		if (quickstartCharacter == GameOptions.QuickstartCharacter.BULLET && !GameStatsManager.Instance.GetFlag(GungeonFlags.SECRET_BULLETMAN_SEEN_05))
		{
			quickstartCharacter = GameOptions.QuickstartCharacter.PILOT;
		}
		if (quickstartCharacter == GameOptions.QuickstartCharacter.ROBOT && !GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_RECEIVED_BUSTED_TELEVISION))
		{
			quickstartCharacter = GameOptions.QuickstartCharacter.PILOT;
		}
		switch (quickstartCharacter)
		{
		case GameOptions.QuickstartCharacter.PILOT:
			return "PlayerRogue";
		case GameOptions.QuickstartCharacter.CONVICT:
			return "PlayerConvict";
		case GameOptions.QuickstartCharacter.SOLDIER:
			return "PlayerMarine";
		case GameOptions.QuickstartCharacter.GUIDE:
			return "PlayerGuide";
		case GameOptions.QuickstartCharacter.BULLET:
			return "PlayerBullet";
		case GameOptions.QuickstartCharacter.ROBOT:
			return "PlayerRobot";
		default:
			return "PlayerRogue";
		}
	}

	// Token: 0x06008DC1 RID: 36289 RVA: 0x003BA38C File Offset: 0x003B858C
	private void Start()
	{
		this.FadeImage.color = new Color(0f, 0f, 0f, 1f);
		base.StartCoroutine(this.LerpFadeAlpha(1f, 0f, 0.3f));
		CharacterSelectController.HasSelected = false;
		this.currentSelected = this.startCharacter;
		this.m_lastMouseSelected = this.currentSelected;
		for (int i = 0; i < this.playerArrows.Length; i++)
		{
			this.arrowToTextPanelMap.Add(this.playerArrows[i], this.playerArrows[i].transform.parent.parent.Find("TextPanel").GetComponent<dfPanel>());
			if (this.currentSelected != i)
			{
				this.playerArrows[i].SetActive(false);
			}
			else
			{
				this.playerArrows[i].GetComponent<tk2dSpriteAnimator>().Play();
				dfPanel dfPanel = this.arrowToTextPanelMap[this.playerArrows[i]];
				dfPanel.Width = 500f;
				dfPanel dfPanel2 = dfPanel;
				dfPanel2.ResolutionChangedPostLayout = (Action<dfControl, Vector3, Vector3>)Delegate.Combine(dfPanel2.ResolutionChangedPostLayout, new Action<dfControl, Vector3, Vector3>(this.ResolutionChangedPanel));
				this.ResolutionChangedPanel(dfPanel, Vector3.zero, Vector3.zero);
			}
		}
		base.StartCoroutine(this.HandleGroundWinds());
		base.StartCoroutine(this.HandleSkyWinds());
		base.StartCoroutine(this.HandlePterodactyl());
	}

	// Token: 0x06008DC2 RID: 36290 RVA: 0x003BA4F4 File Offset: 0x003B86F4
	public void OnDestroy()
	{
		if (this.activeActions != null)
		{
			this.activeActions.Destroy();
			this.activeActions = null;
		}
	}

	// Token: 0x06008DC3 RID: 36291 RVA: 0x003BA514 File Offset: 0x003B8714
	private IEnumerator HandleSkyWinds()
	{
		for (;;)
		{
			int randomWindex = UnityEngine.Random.Range(0, this.skyWinds.Length);
			dfSprite windSprite = this.skyWinds[randomWindex];
			dfSpriteAnimation windAnimation = windSprite.GetComponent<dfSpriteAnimation>();
			windSprite.IsVisible = true;
			windAnimation.Play();
			yield return new WaitForSeconds(windAnimation.Length);
			windSprite.IsVisible = false;
			windAnimation.Reset();
			yield return new WaitForSeconds((float)UnityEngine.Random.Range(2, 6));
		}
		yield break;
	}

	// Token: 0x06008DC4 RID: 36292 RVA: 0x003BA530 File Offset: 0x003B8730
	private IEnumerator HandleGroundWinds()
	{
		for (;;)
		{
			int randomWindex = UnityEngine.Random.Range(0, this.groundWinds.Length);
			dfSprite windSprite = this.groundWinds[randomWindex];
			dfSpriteAnimation windAnimation = windSprite.GetComponent<dfSpriteAnimation>();
			windSprite.IsVisible = true;
			windAnimation.Play();
			yield return new WaitForSeconds(windAnimation.Length);
			windSprite.IsVisible = false;
			windAnimation.Reset();
			yield return new WaitForSeconds((float)UnityEngine.Random.Range(3, 8));
		}
		yield break;
	}

	// Token: 0x06008DC5 RID: 36293 RVA: 0x003BA54C File Offset: 0x003B874C
	private IEnumerator HandlePterodactyl()
	{
		dfSpriteAnimation animator = this.pterodactylVFX.GetComponent<dfSpriteAnimation>();
		dfSprite sprite = animator.GetComponent<dfSprite>();
		Vector2 startRelativePositionBase = sprite.RelativePosition;
		dfGUIManager manager = sprite.GetManager();
		yield return null;
		for (;;)
		{
			sprite.IsVisible = true;
			float scaleFactor = (float)Screen.height * manager.RenderCamera.rect.height / (float)manager.FixedHeight;
			float xOffset = 800f * scaleFactor;
			float xOffset2 = 1200f * scaleFactor;
			float yOffset = 20f * scaleFactor;
			animator.Play();
			Vector2 startRelativePosition = startRelativePositionBase + new Vector2(0f, UnityEngine.Random.Range(-yOffset, yOffset));
			Vector2 targetRelativePosition = new Vector2(startRelativePosition.x + UnityEngine.Random.Range(xOffset, xOffset2), startRelativePosition.y + UnityEngine.Random.Range(-yOffset, yOffset));
			float elapsed = 0f;
			float duration = animator.Length;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				sprite.RelativePosition = Vector2.Lerp(startRelativePosition, targetRelativePosition, Mathf.SmoothStep(0f, 1f, elapsed / duration));
				yield return null;
			}
			sprite.IsVisible = false;
			animator.Reset();
			sprite.RelativePosition = startRelativePositionBase;
			yield return new WaitForSeconds((float)UnityEngine.Random.Range(6, 20));
		}
		yield break;
	}

	// Token: 0x06008DC6 RID: 36294 RVA: 0x003BA568 File Offset: 0x003B8768
	private void Initialize()
	{
		this.m_isInitialized = true;
		uint num = 1U;
		DebugTime.RecordStartTime();
		AkSoundEngine.LoadBank("SFX.bnk", -1, out num);
		DebugTime.Log("CharacterSelectController.Initialize.LoadBank()", new object[0]);
		AkSoundEngine.PostEvent("Play_AMB_night_loop_01", base.gameObject);
	}

	// Token: 0x06008DC7 RID: 36295 RVA: 0x003BA5B4 File Offset: 0x003B87B4
	private void Do()
	{
		CharacterSelectController.HasSelected = true;
		GameObject gameObject = this.playerArrows[this.currentSelected];
		CharacterSelectIdleDoer componentInParent = gameObject.GetComponentInParent<CharacterSelectIdleDoer>();
		componentInParent.enabled = false;
		float num = 0.25f;
		if (componentInParent != null && !string.IsNullOrEmpty(componentInParent.onSelectedAnimation))
		{
			tk2dSpriteAnimationClip clipByName = componentInParent.spriteAnimator.GetClipByName(componentInParent.onSelectedAnimation);
			num = (float)clipByName.frames.Length / clipByName.fps;
			componentInParent.spriteAnimator.Play(clipByName);
		}
		base.StartCoroutine(this.OnSelectedCharacter(num, this.playerPrefabPaths[this.currentSelected]));
	}

	// Token: 0x06008DC8 RID: 36296 RVA: 0x003BA650 File Offset: 0x003B8850
	private IEnumerator OnSelectedCharacter(float delayTime, string playerPrefabPath)
	{
		yield return new WaitForSeconds(delayTime);
		base.StartCoroutine(this.LerpFadeAlpha(0f, 1f, 0.15f));
		yield return new WaitForSeconds(0.15f);
		GameManager.PlayerPrefabForNewGame = (GameObject)BraveResources.Load(this.playerPrefabPaths[this.currentSelected], ".prefab");
		PlayerController playerController = GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>();
		GameStatsManager.Instance.BeginNewSession(playerController);
		GameManager.Instance.DelayedLoadNextLevel(0.25f);
		yield break;
	}

	// Token: 0x06008DC9 RID: 36297 RVA: 0x003BA674 File Offset: 0x003B8874
	private IEnumerator HandleTransition(GameObject arrowToSlide, GameObject targetArrow)
	{
		this.m_isTransitioning = true;
		dfPanel currentTextPanel = this.arrowToTextPanelMap[arrowToSlide];
		dfPanel newTextPanel = this.arrowToTextPanelMap[targetArrow];
		Vector3 initialPosition = arrowToSlide.transform.position;
		Vector3 targetPosition = targetArrow.transform.position;
		float elapsed = 0f;
		float duration = 0.15f;
		currentTextPanel.IsVisible = false;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			Vector3 currentPosition = Vector3.Lerp(initialPosition, targetPosition, t);
			arrowToSlide.transform.position = currentPosition;
			currentTextPanel.IsVisible = false;
			yield return null;
		}
		int targetReticleFrame = arrowToSlide.GetComponent<tk2dSpriteAnimator>().CurrentFrame + 1;
		arrowToSlide.SetActive(false);
		arrowToSlide.transform.position = initialPosition;
		currentTextPanel.IsVisible = false;
		targetArrow.SetActive(true);
		targetArrow.GetComponent<tk2dSpriteAnimator>().Play();
		targetArrow.GetComponent<tk2dSpriteAnimator>().SetFrame(targetReticleFrame);
		this.m_isTransitioning = false;
		elapsed = 0f;
		duration = 0.5f;
		newTextPanel.Width = 1f;
		newTextPanel.IsVisible = true;
		newTextPanel.ResolutionChangedPostLayout = null;
		dfPanel dfPanel = newTextPanel;
		dfPanel.ResolutionChangedPostLayout = (Action<dfControl, Vector3, Vector3>)Delegate.Combine(dfPanel.ResolutionChangedPostLayout, new Action<dfControl, Vector3, Vector3>(this.ResolutionChangedPanel));
		yield return new WaitForSeconds(0.45f);
		this.ResolutionChangedPanel(newTextPanel, Vector3.zero, Vector3.zero);
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t2 = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			newTextPanel.Width = (float)((int)Mathf.Lerp(1f, 450f, t2));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008DCA RID: 36298 RVA: 0x003BA6A0 File Offset: 0x003B88A0
	private void ResolutionChangedPanel(dfControl newTextPanel, Vector3 previousRelativePosition, Vector3 newRelativePosition)
	{
		dfLabel component = newTextPanel.transform.Find("NameLabel").GetComponent<dfLabel>();
		dfLabel component2 = newTextPanel.transform.Find("DescLabel").GetComponent<dfLabel>();
		dfLabel component3 = newTextPanel.transform.Find("GunLabel").GetComponent<dfLabel>();
		float num = (float)Screen.height * component.GetManager().RenderCamera.rect.height / 1080f * 4f;
		int num2 = Mathf.FloorToInt(num);
		tk2dBaseSprite sprite = newTextPanel.Parent.GetComponentsInChildren<CharacterSelectFacecardIdleDoer>(true)[0].sprite;
		newTextPanel.transform.position = sprite.transform.position + new Vector3(18f * num * component.PixelsToUnits(), 41f * num * component.PixelsToUnits(), 0f);
		component.TextScale = num;
		component2.TextScale = num;
		component3.TextScale = num;
		component.Padding = new RectOffset(2 * num2, 2 * num2, -2 * num2, num2);
		component2.Padding = new RectOffset(2 * num2, 2 * num2, -2 * num2, num2);
		component3.Padding = new RectOffset(2 * num2, 2 * num2, -2 * num2, num2);
		component.RelativePosition = new Vector3(num * 2f, num, 0f);
		component2.RelativePosition = new Vector3(0f, num + component.Size.y, 0f) + component.RelativePosition;
		component3.RelativePosition = new Vector3(0f, num + component2.Size.y, 0f) + component2.RelativePosition;
	}

	// Token: 0x06008DCB RID: 36299 RVA: 0x003BA858 File Offset: 0x003B8A58
	private void HandleShiftLeft()
	{
		if (this.m_isTransitioning)
		{
			this.m_queuedChange = -1;
		}
		else
		{
			this.currentSelected = (this.currentSelected - 1 + this.playerArrows.Length) % this.playerArrows.Length;
			AkSoundEngine.PostEvent("Play_UI_menu_select_01", base.gameObject);
		}
	}

	// Token: 0x06008DCC RID: 36300 RVA: 0x003BA8B0 File Offset: 0x003B8AB0
	private void HandleShiftRight()
	{
		if (this.m_isTransitioning)
		{
			this.m_queuedChange = 1;
		}
		else
		{
			this.currentSelected = (this.currentSelected + 1 + this.playerArrows.Length) % this.playerArrows.Length;
			AkSoundEngine.PostEvent("Play_UI_menu_select_01", base.gameObject);
		}
	}

	// Token: 0x06008DCD RID: 36301 RVA: 0x003BA908 File Offset: 0x003B8B08
	private void ForceSelect(int index)
	{
		if (!this.m_isTransitioning)
		{
			this.currentSelected = index;
			AkSoundEngine.PostEvent("Play_UI_menu_confirm_01", base.gameObject);
		}
	}

	// Token: 0x06008DCE RID: 36302 RVA: 0x003BA930 File Offset: 0x003B8B30
	private IEnumerator LerpFadeAlpha(float startAlpha, float targetAlpha, float duration)
	{
		yield return null;
		float elapsed = 0f;
		Color startColor = new Color(0f, 0f, 0f, startAlpha);
		Color endColor = new Color(0f, 0f, 0f, targetAlpha);
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			this.FadeImage.color = Color.Lerp(startColor, endColor, t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008DCF RID: 36303 RVA: 0x003BA960 File Offset: 0x003B8B60
	public void HandleSelect()
	{
		if (GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		this.Do();
		AkSoundEngine.PostEvent("Play_UI_menu_characterselect_01", base.gameObject);
		AkSoundEngine.PostEvent("Stop_AMB_night_loop_01", base.gameObject);
	}

	// Token: 0x06008DD0 RID: 36304 RVA: 0x003BA99C File Offset: 0x003B8B9C
	private void Update()
	{
		if (!this.m_isInitialized)
		{
			this.Initialize();
		}
		if (CharacterSelectController.HasSelected)
		{
			return;
		}
		GameObject gameObject = this.playerArrows[this.currentSelected];
		this.ResolutionChangedPanel(this.arrowToTextPanelMap[this.playerArrows[this.currentSelected]], Vector3.zero, Vector3.zero);
		if ((Input.mousePosition.XY() - this.m_lastMousePosition).magnitude > 2f)
		{
			int num = -1;
			float num2 = float.MaxValue;
			Vector2 vector = this.uiCamera.ScreenToWorldPoint(Input.mousePosition).XY();
			for (int i = 0; i < this.playerArrows.Length; i++)
			{
				tk2dBaseSprite component = this.playerArrows[i].transform.parent.GetComponent<tk2dBaseSprite>();
				Vector2 vector2 = component.transform.position.XY() + Vector2.Scale(component.transform.localScale.XY(), Vector2.Scale(component.scale.XY(), component.GetUntrimmedBounds().extents.XY()));
				float num3 = Vector2.Distance(vector, vector2);
				if (num3 < num2 && num3 < 0.1f)
				{
					num2 = num3;
					num = i;
				}
			}
			if (!this.m_isTransitioning)
			{
				if (num != -1 && num != this.currentSelected)
				{
					this.ForceSelect(num);
					this.currentSelected = num;
				}
				this.m_lastMouseSelected = num;
			}
		}
		if (this.activeActions.SelectLeft.WasPressedAsDpadRepeating)
		{
			this.HandleShiftLeft();
		}
		if (this.activeActions.SelectRight.WasPressedAsDpadRepeating)
		{
			this.HandleShiftRight();
		}
		if (this.activeActions.MenuSelectAction.WasPressed || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
		{
			this.HandleSelect();
		}
		if (Input.GetMouseButtonDown(0) && this.m_lastMouseSelected != -1)
		{
			this.currentSelected = this.m_lastMouseSelected;
			this.HandleSelect();
		}
		if (this.m_queuedChange != 0 && !this.m_isTransitioning)
		{
			if (gameObject == this.playerArrows[this.currentSelected])
			{
				this.currentSelected = (this.currentSelected + this.m_queuedChange + this.playerArrows.Length) % this.playerArrows.Length;
				AkSoundEngine.PostEvent("Play_UI_menu_select_01", base.gameObject);
			}
			this.m_queuedChange = 0;
		}
		GameObject gameObject2 = this.playerArrows[this.currentSelected];
		if (gameObject != gameObject2)
		{
			base.StartCoroutine(this.HandleTransition(gameObject, gameObject2));
		}
		this.m_lastMousePosition = Input.mousePosition;
	}

	// Token: 0x0400957D RID: 38269
	public static bool HasSelected;

	// Token: 0x0400957E RID: 38270
	public int startCharacter = 1;

	// Token: 0x0400957F RID: 38271
	public GameObject[] playerArrows;

	// Token: 0x04009580 RID: 38272
	public string[] playerPrefabPaths;

	// Token: 0x04009581 RID: 38273
	public Camera uiCamera;

	// Token: 0x04009582 RID: 38274
	public int currentSelected;

	// Token: 0x04009583 RID: 38275
	public Transform pterodactylVFX;

	// Token: 0x04009584 RID: 38276
	public dfSprite[] groundWinds;

	// Token: 0x04009585 RID: 38277
	public dfSprite[] skyWinds;

	// Token: 0x04009586 RID: 38278
	public Image FadeImage;

	// Token: 0x04009587 RID: 38279
	protected int m_queuedChange;

	// Token: 0x04009588 RID: 38280
	protected bool m_isTransitioning;

	// Token: 0x04009589 RID: 38281
	protected bool m_isInitialized;

	// Token: 0x0400958A RID: 38282
	protected Dictionary<GameObject, dfPanel> arrowToTextPanelMap = new Dictionary<GameObject, dfPanel>();

	// Token: 0x0400958B RID: 38283
	protected GungeonActions activeActions;

	// Token: 0x0400958C RID: 38284
	private Vector2 m_lastMousePosition = Vector2.zero;

	// Token: 0x0400958D RID: 38285
	private int m_lastMouseSelected = -1;
}
