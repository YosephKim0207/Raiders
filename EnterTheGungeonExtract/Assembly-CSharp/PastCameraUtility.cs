using System;
using UnityEngine;

// Token: 0x02001130 RID: 4400
public static class PastCameraUtility
{
	// Token: 0x0600612D RID: 24877 RVA: 0x00256968 File Offset: 0x00254B68
	public static void UnlockConversation()
	{
		GameManager.Instance.PrimaryPlayer.ClearInputOverride("past");
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.SecondaryPlayer.ClearInputOverride("past");
		}
		Pixelator.Instance.LerpToLetterbox(0.5f, 0.25f);
		Pixelator.Instance.DoFinalNonFadedLayer = false;
		GameUIRoot.Instance.ToggleLowerPanels(true, false, string.Empty);
		GameUIRoot.Instance.ShowCoreUI(string.Empty);
		GameManager.Instance.MainCameraController.SetManualControl(false, true);
	}

	// Token: 0x0600612E RID: 24878 RVA: 0x00256A00 File Offset: 0x00254C00
	public static void LockConversation(Vector2 lockPos)
	{
		GameManager.Instance.PrimaryPlayer.SetInputOverride("past");
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			GameManager.Instance.SecondaryPlayer.SetInputOverride("past");
		}
		Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
		Pixelator.Instance.DoFinalNonFadedLayer = true;
		GameUIRoot.Instance.ToggleLowerPanels(false, false, string.Empty);
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		CameraController mainCameraController = GameManager.Instance.MainCameraController;
		mainCameraController.SetManualControl(true, true);
		mainCameraController.OverridePosition = lockPos.ToVector3ZUp(0f);
	}
}
