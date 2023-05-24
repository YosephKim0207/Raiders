using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001131 RID: 4401
public class CoopPastController : MonoBehaviour
{
	// Token: 0x06006130 RID: 24880 RVA: 0x00256AB4 File Offset: 0x00254CB4
	private void Start()
	{
		base.StartCoroutine(this.HandleIntro());
	}

	// Token: 0x06006131 RID: 24881 RVA: 0x00256AC4 File Offset: 0x00254CC4
	private void Update()
	{
		GameManager.Instance.IsFoyer = false;
		if (GameManager.PVP_ENABLED)
		{
			if (GameManager.Instance.PrimaryPlayer.healthHaver.IsDead)
			{
				this.HandlePlayerTwoVictory();
				GameManager.PVP_ENABLED = false;
			}
			else if (GameManager.Instance.SecondaryPlayer.healthHaver.IsDead)
			{
				this.HandlePlayerOneVictory();
				GameManager.PVP_ENABLED = false;
			}
		}
	}

	// Token: 0x06006132 RID: 24882 RVA: 0x00256B38 File Offset: 0x00254D38
	private void HandlePlayerOneVictory()
	{
		base.StartCoroutine(this.HandleOutro(false));
	}

	// Token: 0x06006133 RID: 24883 RVA: 0x00256B48 File Offset: 0x00254D48
	private void HandlePlayerTwoVictory()
	{
		base.StartCoroutine(this.HandleOutro(true));
	}

	// Token: 0x06006134 RID: 24884 RVA: 0x00256B58 File Offset: 0x00254D58
	private IEnumerator HandleOutro(bool coopPlayerWon)
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GameManager.Instance.AllPlayers[i].SetInputOverride("past");
		}
		GameManager.Instance.MainCameraController.OverridePosition = GameManager.Instance.MainCameraController.transform.position.XY();
		GameManager.Instance.MainCameraController.SetManualControl(true, true);
		GameManager.Instance.DungeonMusicController.EndBossMusic();
		if (!GameManager.Instance.MainCameraController.PointIsVisible(GameManager.Instance.PrimaryPlayer.CenterPosition, -0.2f) || !GameManager.Instance.MainCameraController.PointIsVisible(GameManager.Instance.SecondaryPlayer.CenterPosition, -0.2f))
		{
			while (GameManager.Instance.PrimaryPlayer.IsFalling)
			{
				yield return null;
			}
			while (GameManager.Instance.SecondaryPlayer.IsFalling)
			{
				yield return null;
			}
			yield return new WaitForSeconds(3f);
			Pixelator.Instance.FadeToBlack(1f, false, 0f);
			yield return new WaitForSeconds(1f);
			GameManager.Instance.PrimaryPlayer.WarpToPoint(this.startingP1Position + new Vector2(0f, 12f), false, false);
			GameManager.Instance.SecondaryPlayer.WarpToPoint(this.startingP2Position + new Vector2(0f, 12f), false, false);
			yield return null;
			GameManager.Instance.MainCameraController.SetManualControl(false, false);
			PastCameraUtility.LockConversation(new Vector2((this.startingP1Position.x + this.startingP2Position.x) / 2f, this.startingP1Position.y + 12f));
			GameManager.Instance.PrimaryPlayer.ForceIdleFacePoint(Vector2.right, true);
			GameManager.Instance.SecondaryPlayer.ForceIdleFacePoint(Vector2.left, true);
			Pixelator.Instance.FadeToBlack(1f, true, 0f);
			yield return new WaitForSeconds(1f);
		}
		else
		{
			PastCameraUtility.LockConversation(GameManager.Instance.MainCameraController.transform.position.XY());
		}
		if (coopPlayerWon)
		{
			yield return new WaitForSeconds(1f);
			yield return base.StartCoroutine(this.DoAmbientTalk(GameManager.Instance.SecondaryPlayer.transform, new Vector3(0.5f, 1.5f, 0f), "#COOPPAST_COOP_WIN_01", -1f, -1, 1));
			yield return base.StartCoroutine(this.DoAmbientTalk(GameManager.Instance.SecondaryPlayer.transform, new Vector3(0.5f, 1.5f, 0f), "#COOPPAST_COOP_WIN_02", -1f, 0, 1));
			yield return new WaitForSeconds(0.5f);
			yield return base.StartCoroutine(this.DoAmbientTalk(GameManager.Instance.SecondaryPlayer.transform, new Vector3(0.5f, 1.5f, 0f), "#COOPPAST_COOP_WIN_02", -1f, 1, 1));
			yield return new WaitForSeconds(0.5f);
		}
		else
		{
			yield return new WaitForSeconds(1f);
			yield return base.StartCoroutine(this.DoAmbientTalk(GameManager.Instance.SecondaryPlayer.transform, new Vector3(0.5f, 1.5f, 0f), "#COOPPAST_PLAYER_WIN_02", -1f, -1, 1));
			yield return new WaitForSeconds(1f);
			yield return base.StartCoroutine(this.DoAmbientTalk(GameManager.Instance.PrimaryPlayer.transform, new Vector3(0.5f, 1.5f, 0f), "#COOPPAST_PLAYER_WIN_01", -1f, -1, 0));
			yield return new WaitForSeconds(0.5f);
		}
		GameStatsManager.Instance.SetCharacterSpecificFlag(PlayableCharacters.CoopCultist, CharacterSpecificGungeonFlags.KILLED_PAST, true);
		BraveTime.RegisterTimeScaleMultiplier(0f, base.gameObject);
		Pixelator.Instance.FreezeFrame();
		float ela = 0f;
		while (ela < ConvictPastController.FREEZE_FRAME_DURATION)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		BraveTime.ClearMultiplier(base.gameObject);
		TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
		ttcc.ClearDebris();
		GameObject borderObject = GameObject.Find("Foyer_Floor_Borders");
		if (borderObject)
		{
			tk2dBaseSprite component = borderObject.GetComponent<tk2dBaseSprite>();
			component.HeightOffGround -= 5f;
			component.UpdateZDepth();
		}
		yield return base.StartCoroutine(ttcc.HandleTimeTubeCredits((!coopPlayerWon) ? GameManager.Instance.PrimaryPlayer.sprite.WorldCenter : GameManager.Instance.SecondaryPlayer.sprite.WorldCenter, false, null, (!coopPlayerWon) ? GameManager.Instance.PrimaryPlayer.PlayerIDX : GameManager.Instance.SecondaryPlayer.PlayerIDX, false));
		AmmonomiconController.Instance.OpenAmmonomicon(true, true);
		yield break;
	}

	// Token: 0x06006135 RID: 24885 RVA: 0x00256B7C File Offset: 0x00254D7C
	private IEnumerator HandleIntro()
	{
		yield return null;
		Pixelator.Instance.TriggerPastFadeIn();
		GameStatsManager.Instance.SetFlag(GungeonFlags.COOP_PAST_REACHED, true);
		GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_SUNLIGHT_SPEAR_UNLOCK, true);
		yield return null;
		this.startingP1Position = GameManager.Instance.PrimaryPlayer.transform.position.XY();
		this.startingP2Position = GameManager.Instance.SecondaryPlayer.transform.position.XY();
		GameManager.Instance.PrimaryPlayer.ForceIdleFacePoint(Vector2.right, true);
		GameManager.Instance.SecondaryPlayer.ForceIdleFacePoint(Vector2.left, true);
		Vector2 convoCenter = (GameManager.Instance.SecondaryPlayer.CenterPosition + GameManager.Instance.PrimaryPlayer.CenterPosition) / 2f;
		PastCameraUtility.LockConversation(convoCenter);
		float ela = 0f;
		while (ela < 1f)
		{
			ela += BraveTime.DeltaTime;
			convoCenter = (GameManager.Instance.SecondaryPlayer.CenterPosition + GameManager.Instance.PrimaryPlayer.CenterPosition) / 2f;
			GameManager.Instance.MainCameraController.OverridePosition = convoCenter;
			yield return null;
		}
		yield return base.StartCoroutine(this.DoAmbientTalk(GameManager.Instance.SecondaryPlayer.transform, new Vector3(0.5f, 1.5f, 0f), "#COOPPAST_COOP_01", -1f, -1, 1));
		yield return base.StartCoroutine(this.DoAmbientTalk(GameManager.Instance.SecondaryPlayer.transform, new Vector3(0.5f, 1.5f, 0f), "#COOPPAST_COOP_02", -1f, -1, 1));
		yield return base.StartCoroutine(this.DoAmbientTalk(GameManager.Instance.PrimaryPlayer.transform, new Vector3(0.5f, 1.5f, 0f), "#COOPPAST_PLAYER_01", -1f, -1, 0));
		yield return base.StartCoroutine(this.DoAmbientTalk(GameManager.Instance.SecondaryPlayer.transform, new Vector3(0.5f, 1.5f, 0f), "#COOPPAST_COOP_03", -1f, -1, 1));
		yield return base.StartCoroutine(this.DoAmbientTalk(GameManager.Instance.PrimaryPlayer.transform, new Vector3(0.5f, 1.5f, 0f), "#COOPPAST_PLAYER_02", -1f, -1, 0));
		yield return base.StartCoroutine(this.DoAmbientTalk(GameManager.Instance.SecondaryPlayer.transform, new Vector3(0.5f, 1.5f, 0f), "#COOPPAST_COOP_04", -1f, -1, 1));
		PastCameraUtility.UnlockConversation();
		GameManager.PVP_ENABLED = true;
		GameManager.Instance.DungeonMusicController.SwitchToBossMusic("Play_MUS_Boss_Theme_Beholster", GameManager.Instance.Dungeon.gameObject);
		yield break;
	}

	// Token: 0x06006136 RID: 24886 RVA: 0x00256B98 File Offset: 0x00254D98
	public IEnumerator DoAmbientTalk(Transform baseTransform, Vector3 offset, string stringKey, float duration, int specificStringIndex = -1, int OnlyThisPlayerInput = -1)
	{
		TextBoxManager.ShowTextBox(baseTransform.position + offset, baseTransform, duration, (specificStringIndex == -1) ? StringTableManager.GetString(stringKey) : StringTableManager.GetExactString(stringKey, specificStringIndex), string.Empty, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
		bool advancedPressed = false;
		while (!advancedPressed)
		{
			if (OnlyThisPlayerInput == 0)
			{
				advancedPressed = BraveInput.GetInstanceForPlayer(0).MenuInteractPressed;
			}
			else if (OnlyThisPlayerInput == 1)
			{
				advancedPressed = BraveInput.GetInstanceForPlayer(1).MenuInteractPressed;
			}
			else
			{
				advancedPressed = BraveInput.GetInstanceForPlayer(0).MenuInteractPressed || BraveInput.GetInstanceForPlayer(1).MenuInteractPressed;
			}
			yield return null;
		}
		TextBoxManager.ClearTextBox(baseTransform);
		yield break;
	}

	// Token: 0x04005BEB RID: 23531
	private Vector2 startingP1Position;

	// Token: 0x04005BEC RID: 23532
	private Vector2 startingP2Position;
}
