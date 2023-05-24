using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using InControl;
using UnityEngine;

// Token: 0x0200178B RID: 6027
public class GameUIRoot : TimeInvariantMonoBehaviour
{
	// Token: 0x06008CAB RID: 36011 RVA: 0x003AF584 File Offset: 0x003AD784
	public GameUIReloadBarController GetReloadBarForPlayer(PlayerController p)
	{
		if (this.m_extantReloadBars != null && this.m_extantReloadBars.Count > 1 && p)
		{
			return this.m_extantReloadBars[(!p.IsPrimaryPlayer) ? 1 : 0];
		}
		if (this.m_extantReloadBars != null)
		{
			return this.m_extantReloadBars[0];
		}
		return null;
	}

	// Token: 0x1700150B RID: 5387
	// (get) Token: 0x06008CAC RID: 36012 RVA: 0x003AF5F0 File Offset: 0x003AD7F0
	// (set) Token: 0x06008CAD RID: 36013 RVA: 0x003AF5F8 File Offset: 0x003AD7F8
	public bool ForceHideGunPanel
	{
		get
		{
			return this.m_forceHideGunPanel;
		}
		set
		{
			this.m_forceHideGunPanel = value;
			if (!this.m_forceHideGunPanel)
			{
				for (int i = 0; i < this.ammoControllers.Count; i++)
				{
					this.ammoControllers[i].TriggerUIDisabled();
				}
			}
		}
	}

	// Token: 0x1700150C RID: 5388
	// (get) Token: 0x06008CAE RID: 36014 RVA: 0x003AF644 File Offset: 0x003AD844
	// (set) Token: 0x06008CAF RID: 36015 RVA: 0x003AF64C File Offset: 0x003AD84C
	public bool ForceHideItemPanel
	{
		get
		{
			return this.m_forceHideItemPanel;
		}
		set
		{
			this.m_forceHideItemPanel = value;
			if (!this.m_forceHideItemPanel)
			{
				for (int i = 0; i < this.itemControllers.Count; i++)
				{
					this.itemControllers[i].TriggerUIDisabled();
				}
			}
		}
	}

	// Token: 0x1700150D RID: 5389
	// (get) Token: 0x06008CB0 RID: 36016 RVA: 0x003AF698 File Offset: 0x003AD898
	// (set) Token: 0x06008CB1 RID: 36017 RVA: 0x003AF6D8 File Offset: 0x003AD8D8
	public static GameUIRoot Instance
	{
		get
		{
			if (GameUIRoot.m_root == null || !GameUIRoot.m_root)
			{
				GameUIRoot.m_root = (GameUIRoot)UnityEngine.Object.FindObjectOfType(typeof(GameUIRoot));
			}
			return GameUIRoot.m_root;
		}
		set
		{
			GameUIRoot.m_root = value;
		}
	}

	// Token: 0x1700150E RID: 5390
	// (get) Token: 0x06008CB2 RID: 36018 RVA: 0x003AF6E0 File Offset: 0x003AD8E0
	public static bool HasInstance
	{
		get
		{
			return GameUIRoot.m_root;
		}
	}

	// Token: 0x1700150F RID: 5391
	// (get) Token: 0x06008CB3 RID: 36019 RVA: 0x003AF6EC File Offset: 0x003AD8EC
	public bool BossHealthBarVisible
	{
		get
		{
			return this.bossController.IsActive || this.bossController2.IsActive || this.bossControllerSide.IsActive;
		}
	}

	// Token: 0x06008CB4 RID: 36020 RVA: 0x003AF71C File Offset: 0x003AD91C
	public dfPanel AddControlToMotionGroups(dfControl control, DungeonData.Direction dir, bool nonCore = false)
	{
		dfAnchorStyle anchor = control.Anchor;
		control.Anchor = dfAnchorStyle.None;
		if (!this.motionGroups.Contains(control))
		{
			this.motionGroups.Add(control);
			this.motionDirections.Add(dir);
		}
		if (nonCore && !this.customNonCoreMotionGroups.Contains(control))
		{
			this.customNonCoreMotionGroups.Add(control);
		}
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		Vector3 relativePosition = control.RelativePosition;
		Vector2 size = control.Size;
		Vector3 initialInactivePosition = this.GetInitialInactivePosition(control, dir);
		switch (dir)
		{
		case DungeonData.Direction.NORTH:
			vector = initialInactivePosition;
			vector2 = relativePosition + size.ToVector3ZUp(0f);
			break;
		case DungeonData.Direction.EAST:
			vector = initialInactivePosition + size.ToVector3ZUp(0f);
			vector2 = relativePosition;
			break;
		case DungeonData.Direction.SOUTH:
			vector = initialInactivePosition + size.ToVector3ZUp(0f);
			vector2 = relativePosition;
			break;
		case DungeonData.Direction.WEST:
			vector = initialInactivePosition;
			vector2 = relativePosition + size.ToVector3ZUp(0f);
			break;
		}
		Vector2 vector3 = Vector2.Min(vector.XY(), vector2.XY());
		Vector2 vector4 = Vector2.Max(vector.XY(), vector2.XY());
		Vector2 vector5 = vector4 - vector3;
		dfPanel dfPanel;
		if (control.Parent == null)
		{
			dfPanel = control.GetManager().AddControl<dfPanel>();
		}
		else
		{
			dfPanel = control.Parent.AddControl<dfPanel>();
		}
		dfPanel.RelativePosition = vector3;
		dfPanel.Size = vector5;
		dfPanel.Pivot = control.Pivot;
		dfPanel.Anchor = anchor;
		dfPanel.IsInteractive = false;
		dfPanel.AddControl(control);
		switch (dir)
		{
		case DungeonData.Direction.NORTH:
			control.RelativePosition = new Vector3(0f, vector5.y - control.Size.y, 0f);
			break;
		case DungeonData.Direction.EAST:
			control.RelativePosition = new Vector3(0f, 0f, 0f);
			break;
		case DungeonData.Direction.SOUTH:
			control.RelativePosition = new Vector3(0f, 0f, 0f);
			break;
		case DungeonData.Direction.WEST:
			control.RelativePosition = new Vector3(vector5.x - control.Size.x, 0f, 0f);
			break;
		}
		control.Anchor = anchor;
		if (nonCore)
		{
			this.RecalculateTargetPositions();
		}
		return dfPanel;
	}

	// Token: 0x06008CB5 RID: 36021 RVA: 0x003AF9C0 File Offset: 0x003ADBC0
	public void UpdateControlMotionGroup(dfControl control)
	{
		if (control == null || !control)
		{
			return;
		}
		if (this.motionGroups.Contains(control))
		{
			DungeonData.Direction direction = this.motionDirections[this.motionGroups.IndexOf(control)];
			this.RemoveControlFromMotionGroups(control);
			this.AddControlToMotionGroups(control, direction, false);
			control.enabled = true;
		}
	}

	// Token: 0x06008CB6 RID: 36022 RVA: 0x003AFA28 File Offset: 0x003ADC28
	public dfPanel GetMotionGroupParent(dfControl control)
	{
		if (this.motionGroups.Contains(control))
		{
			return this.motionGroups[this.motionGroups.IndexOf(control)].Parent as dfPanel;
		}
		return null;
	}

	// Token: 0x06008CB7 RID: 36023 RVA: 0x003AFA60 File Offset: 0x003ADC60
	public void RemoveControlFromMotionGroups(dfControl control)
	{
		int num = this.motionGroups.IndexOf(control);
		if (num != -1)
		{
			this.motionGroups.Remove(control);
			this.motionDirections.RemoveAt(num);
		}
		dfControl parent = control.Parent;
		if (control.Parent && control.Parent.Parent)
		{
			control.Parent.Parent.AddControl(control);
		}
		else if (control.Parent)
		{
			control.Parent.RemoveControl(control);
		}
		UnityEngine.Object.Destroy(parent.gameObject);
	}

	// Token: 0x06008CB8 RID: 36024 RVA: 0x003AFB04 File Offset: 0x003ADD04
	private Vector3 GetActivePosition(dfControl panel, DungeonData.Direction direction)
	{
		Vector3 relativePosition = panel.RelativePosition;
		Vector3 vector = panel.Size.ToVector3ZUp(0f);
		dfPanel dfPanel = panel.Parent as dfPanel;
		if (dfPanel != null)
		{
			switch (direction)
			{
			case DungeonData.Direction.NORTH:
				return new Vector3(0f, dfPanel.Size.y - vector.y, 0f);
			case DungeonData.Direction.EAST:
				return Vector3.zero;
			case DungeonData.Direction.SOUTH:
				return Vector3.zero;
			case DungeonData.Direction.WEST:
				return new Vector3(dfPanel.Size.x - vector.x, 0f, 0f);
			}
		}
		return relativePosition;
	}

	// Token: 0x06008CB9 RID: 36025 RVA: 0x003AFBC8 File Offset: 0x003ADDC8
	public void DoDamageNumber(Vector3 worldPosition, float heightOffGround, int damage)
	{
		string stringForInt = IntToStringSansGarbage.GetStringForInt(damage);
		if (this.m_inactiveDamageLabels.Count == 0)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("DamagePopupLabel", ".prefab"), base.transform);
			this.m_inactiveDamageLabels.Add(gameObject.GetComponent<dfLabel>());
		}
		dfLabel dfLabel = this.m_inactiveDamageLabels[0];
		this.m_inactiveDamageLabels.RemoveAt(0);
		dfLabel.gameObject.SetActive(true);
		dfLabel.Text = stringForInt;
		dfLabel.Color = Color.red;
		dfLabel.Opacity = 1f;
		dfLabel.transform.position = dfFollowObject.ConvertWorldSpaces(worldPosition, GameManager.Instance.MainCameraController.Camera, this.m_manager.RenderCamera).WithZ(0f);
		dfLabel.transform.position = dfLabel.transform.position.QuantizeFloor(dfLabel.PixelsToUnits() / (Pixelator.Instance.ScaleTileScale / Pixelator.Instance.CurrentTileScale));
		dfLabel.StartCoroutine(this.HandleDamageNumberCR(worldPosition, worldPosition.y - heightOffGround, dfLabel));
	}

	// Token: 0x06008CBA RID: 36026 RVA: 0x003AFCE8 File Offset: 0x003ADEE8
	private IEnumerator HandleDamageNumberCR(Vector3 startWorldPosition, float worldFloorHeight, dfLabel damageLabel)
	{
		float elapsed = 0f;
		float duration = 1.5f;
		float holdTime = 0f;
		Camera mainCam = GameManager.Instance.MainCameraController.Camera;
		Vector3 worldPosition = startWorldPosition;
		Vector3 lastVelocity = new Vector3(Mathf.Lerp(-8f, 8f, UnityEngine.Random.value), UnityEngine.Random.Range(15f, 25f), 0f);
		while (elapsed < duration)
		{
			float dt = BraveTime.DeltaTime;
			elapsed += dt;
			if (GameManager.Instance.IsPaused)
			{
				break;
			}
			if (elapsed > holdTime)
			{
				lastVelocity += new Vector3(0f, -50f, 0f) * dt;
				Vector3 vector = lastVelocity * dt + worldPosition;
				if (vector.y < worldFloorHeight)
				{
					float num = worldFloorHeight - vector.y;
					float num2 = worldFloorHeight + num;
					vector.y = num2 * 0.5f;
					lastVelocity.y *= -0.5f;
				}
				worldPosition = vector;
				damageLabel.transform.position = dfFollowObject.ConvertWorldSpaces(worldPosition, mainCam, this.m_manager.RenderCamera).WithZ(0f);
			}
			float t = elapsed / duration;
			damageLabel.Opacity = 1f - t;
			yield return null;
		}
		damageLabel.gameObject.SetActive(false);
		this.m_inactiveDamageLabels.Add(damageLabel);
		yield break;
	}

	// Token: 0x06008CBB RID: 36027 RVA: 0x003AFD18 File Offset: 0x003ADF18
	public bool TransformHasDefaultLabel(Transform attachTransform)
	{
		for (int i = 0; i < this.extantBasicLabels.Count; i++)
		{
			if (this.extantBasicLabels[i].targetObject == attachTransform)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06008CBC RID: 36028 RVA: 0x003AFD60 File Offset: 0x003ADF60
	public GameObject RegisterDefaultLabel(Transform attachTransform, Vector3 offset, string text)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("DefaultLabelPanel", ".prefab"));
		DefaultLabelController component = gameObject.GetComponent<DefaultLabelController>();
		this.m_manager.AddControl(component.panel);
		component.label.Text = text;
		component.Trigger(attachTransform, offset);
		this.extantBasicLabels.Add(component);
		return gameObject;
	}

	// Token: 0x06008CBD RID: 36029 RVA: 0x003AFDC0 File Offset: 0x003ADFC0
	public void ToggleAllDefaultLabels(bool visible, string reason)
	{
		if (visible)
		{
			this.m_defaultLabelsHidden.RemoveOverride(reason);
		}
		else
		{
			this.m_defaultLabelsHidden.SetOverride(reason, true, null);
		}
		for (int i = 0; i < this.extantBasicLabels.Count; i++)
		{
			if (this.extantBasicLabels[i] && this.extantBasicLabels[i].panel)
			{
				this.extantBasicLabels[i].panel.IsVisible = !this.m_defaultLabelsHidden.Value;
			}
		}
	}

	// Token: 0x06008CBE RID: 36030 RVA: 0x003AFE6C File Offset: 0x003AE06C
	public void ClearAllDefaultLabels()
	{
		for (int i = 0; i < this.extantBasicLabels.Count; i++)
		{
			UnityEngine.Object.Destroy(this.extantBasicLabels[i].gameObject);
			this.extantBasicLabels.RemoveAt(i);
			i--;
		}
	}

	// Token: 0x06008CBF RID: 36031 RVA: 0x003AFEBC File Offset: 0x003AE0BC
	public void ForceRemoveDefaultLabel(DefaultLabelController label)
	{
		int num = this.extantBasicLabels.IndexOf(label);
		if (num >= 0)
		{
			this.extantBasicLabels.RemoveAt(num);
		}
		UnityEngine.Object.Destroy(label.gameObject);
	}

	// Token: 0x06008CC0 RID: 36032 RVA: 0x003AFEF4 File Offset: 0x003AE0F4
	public void DeregisterDefaultLabel(Transform attachTransform)
	{
		for (int i = 0; i < this.extantBasicLabels.Count; i++)
		{
			if (this.extantBasicLabels[i].targetObject == attachTransform)
			{
				UnityEngine.Object.Destroy(this.extantBasicLabels[i].gameObject);
				this.extantBasicLabels.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06008CC1 RID: 36033 RVA: 0x003AFF60 File Offset: 0x003AE160
	public void TriggerDemoModeTutorialScreens()
	{
		if (this.demoTutorialPanels_Controller.Count == 0)
		{
			return;
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.TUTORIAL_COMPLETED))
		{
			return;
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.INTERNALDEBUG_HAS_SEEN_DEMO_TEXT))
		{
			return;
		}
		GameStatsManager.Instance.SetFlag(GungeonFlags.INTERNALDEBUG_HAS_SEEN_DEMO_TEXT, true);
		base.StartCoroutine(this.HandleDemoModeTutorialScreens());
	}

	// Token: 0x06008CC2 RID: 36034 RVA: 0x003AFFC0 File Offset: 0x003AE1C0
	private IEnumerator HandleDemoModeTutorialScreens()
	{
		this.levelNameUI.BanishLevelNameText();
		GameManager.Instance.PauseRaw(true);
		int currentPanelIndex = 0;
		List<dfPanel> panelList = ((!BraveInput.GetInstanceForPlayer(0).IsKeyboardAndMouse(false)) ? this.demoTutorialPanels_Controller : this.demoTutorialPanels_Keyboard);
		bool cachedKM = BraveInput.GetInstanceForPlayer(0).IsKeyboardAndMouse(false);
		while (currentPanelIndex < panelList.Count)
		{
			dfPanel currentPanel = panelList[currentPanelIndex];
			if (cachedKM != BraveInput.GetInstanceForPlayer(0).IsKeyboardAndMouse(false))
			{
				currentPanel.IsVisible = false;
				cachedKM = BraveInput.GetInstanceForPlayer(0).IsKeyboardAndMouse(false);
				panelList = ((!cachedKM) ? this.demoTutorialPanels_Controller : this.demoTutorialPanels_Keyboard);
				currentPanel = panelList[currentPanelIndex];
			}
			if (!currentPanel.IsVisible)
			{
				currentPanel.IsVisible = true;
			}
			else if (BraveInput.GetInstanceForPlayer(0).ActiveActions.MenuSelectAction.WasPressed || BraveInput.GetInstanceForPlayer(0).ActiveActions.ShootAction.WasPressed)
			{
				currentPanel.IsVisible = false;
				currentPanelIndex++;
				if (currentPanelIndex < panelList.Count)
				{
					panelList[currentPanelIndex].IsVisible = true;
				}
			}
			yield return null;
		}
		GameManager.Instance.ForceUnpause();
		GameManager.Instance.PreventPausing = false;
		yield break;
	}

	// Token: 0x06008CC3 RID: 36035 RVA: 0x003AFFDC File Offset: 0x003AE1DC
	private Vector3 GetInactivePosition(dfControl panel, DungeonData.Direction direction)
	{
		Vector3 relativePosition = panel.RelativePosition;
		Vector3 vector = panel.Size.ToVector3ZUp(0f);
		dfPanel dfPanel = panel.Parent as dfPanel;
		if (dfPanel != null)
		{
			switch (direction)
			{
			case DungeonData.Direction.NORTH:
				return Vector3.zero;
			case DungeonData.Direction.EAST:
				return new Vector3(dfPanel.Size.x - vector.x, 0f, 0f);
			case DungeonData.Direction.SOUTH:
				return new Vector3(0f, dfPanel.Size.y - vector.y, 0f);
			case DungeonData.Direction.WEST:
				return Vector3.zero;
			}
		}
		return relativePosition;
	}

	// Token: 0x06008CC4 RID: 36036 RVA: 0x003B00A0 File Offset: 0x003AE2A0
	private Vector3 GetInitialInactivePosition(dfControl panel, DungeonData.Direction direction)
	{
		Vector3 relativePosition = panel.RelativePosition;
		Vector3 vector = panel.Size.ToVector3ZUp(0f);
		Vector2 screenSize = panel.GetManager().GetScreenSize();
		switch (direction)
		{
		case DungeonData.Direction.NORTH:
			relativePosition = new Vector3(relativePosition.x, -vector.y - this.MotionGroupBufferWidth, relativePosition.z);
			break;
		case DungeonData.Direction.EAST:
			relativePosition = new Vector3(screenSize.x + vector.x + this.MotionGroupBufferWidth, relativePosition.y, relativePosition.z);
			break;
		case DungeonData.Direction.SOUTH:
			relativePosition = new Vector3(relativePosition.x, screenSize.y + vector.y + this.MotionGroupBufferWidth, relativePosition.z);
			break;
		case DungeonData.Direction.WEST:
			relativePosition = new Vector3(-vector.x - this.MotionGroupBufferWidth, relativePosition.y, relativePosition.z);
			break;
		}
		return relativePosition;
	}

	// Token: 0x06008CC5 RID: 36037 RVA: 0x003B01B0 File Offset: 0x003AE3B0
	public void AddPassiveItemToDock(PassiveItem item, PlayerController sourcePlayer)
	{
		MinimapUIController minimapUIController = null;
		if (Minimap.Instance)
		{
			minimapUIController = Minimap.Instance.UIMinimap;
		}
		if (!minimapUIController)
		{
			minimapUIController = UnityEngine.Object.FindObjectOfType<MinimapUIController>();
		}
		minimapUIController.AddPassiveItemToDock(item, sourcePlayer);
	}

	// Token: 0x06008CC6 RID: 36038 RVA: 0x003B01F4 File Offset: 0x003AE3F4
	public void RemovePassiveItemFromDock(PassiveItem item)
	{
		MinimapUIController minimapUIController = UnityEngine.Object.FindObjectOfType<MinimapUIController>();
		minimapUIController.RemovePassiveItemFromDock(item);
	}

	// Token: 0x06008CC7 RID: 36039 RVA: 0x003B0210 File Offset: 0x003AE410
	private IEnumerator Start()
	{
		for (int i = 0; i < this.motionGroups.Count; i++)
		{
			this.AddControlToMotionGroups(this.motionGroups[i], this.motionDirections[i], false);
		}
		this.RecalculateTargetPositions();
		if (this.AreYouSurePanel != null)
		{
			this.m_AreYouSureYesButton = this.AreYouSurePanel.transform.Find("YesButton").GetComponent<dfButton>();
			this.m_AreYouSureNoButton = this.AreYouSurePanel.transform.Find("NoButton").GetComponent<dfButton>();
			this.m_AreYouSurePrimaryLabel = this.AreYouSurePanel.transform.Find("TopLabel").GetComponent<dfLabel>();
			this.m_AreYouSureSecondaryLabel = this.AreYouSurePanel.transform.Find("SecondaryLabel").GetComponent<dfLabel>();
		}
		this.notificationController.Initialize();
		this.m_extantReloadLabels = new List<dfLabel>();
		this.m_extantReloadLabels.Add(this.p_needsReloadLabel);
		this.m_displayingReloadNeeded = new List<bool>();
		this.m_displayingReloadNeeded.Add(false);
		this.m_extantReloadBars = new List<GameUIReloadBarController>();
		this.m_extantReloadBars.Add(this.p_playerReloadBar);
		this.m_gunNameVisibilityTimers = new float[this.gunNameLabels.Count];
		this.m_itemNameVisibilityTimers = new float[this.itemNameLabels.Count];
		if (GameManager.Instance.PrimaryPlayer == null)
		{
			this.HideCoreUI(string.Empty);
			this.ToggleLowerPanels(false, false, string.Empty);
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			if (Foyer.DoMainMenu)
			{
				this.Manager.RenderCamera.enabled = false;
			}
			for (int j = 0; j < this.motionGroups.Count; j++)
			{
				if (!(this.motionGroups[j] == this.notificationController.Panel))
				{
					this.motionGroups[j].Parent.IsVisible = false;
					this.motionGroups.RemoveAt(j);
					j--;
				}
			}
		}
		CameraController mainCameraController = GameManager.Instance.MainCameraController;
		mainCameraController.OnFinishedFrame = (Action)Delegate.Combine(mainCameraController.OnFinishedFrame, new Action(this.UpdateReloadLabelsOnCameraFinishedFrame));
		yield return null;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.ConvertCoreUIToCoopMode();
		}
		yield break;
	}

	// Token: 0x06008CC8 RID: 36040 RVA: 0x003B022C File Offset: 0x003AE42C
	public void DisableCoopPlayerUI(PlayerController deadPlayer)
	{
		if (deadPlayer.IsPrimaryPlayer)
		{
			this.ammoControllers[1].ToggleRenderers(false);
			this.itemControllers[0].ToggleRenderers(false);
			this.itemControllers[0].temporarilyPreventVisible = true;
		}
		else
		{
			this.ammoControllers[0].ToggleRenderers(false);
			this.itemControllers[1].ToggleRenderers(false);
			this.itemControllers[1].temporarilyPreventVisible = true;
		}
	}

	// Token: 0x06008CC9 RID: 36041 RVA: 0x003B02B8 File Offset: 0x003AE4B8
	public void ReenableCoopPlayerUI(PlayerController deadPlayer)
	{
		if (deadPlayer.IsPrimaryPlayer)
		{
			this.ammoControllers[1].GetComponent<dfPanel>().IsVisible = true;
			this.ammoControllers[1].ToggleRenderers(true);
			this.itemControllers[0].GetComponent<dfPanel>().IsVisible = true;
			this.itemControllers[0].ToggleRenderers(true);
			this.itemControllers[0].temporarilyPreventVisible = false;
		}
		else
		{
			this.ammoControllers[0].GetComponent<dfPanel>().IsVisible = true;
			this.ammoControllers[0].ToggleRenderers(true);
			this.itemControllers[1].GetComponent<dfPanel>().IsVisible = true;
			this.itemControllers[1].ToggleRenderers(true);
			this.itemControllers[1].temporarilyPreventVisible = false;
		}
	}

	// Token: 0x06008CCA RID: 36042 RVA: 0x003B03A0 File Offset: 0x003AE5A0
	public void ConvertCoreUIToCoopMode()
	{
		float num = this.gunNameLabels[0].PixelsToUnits();
		bool flag = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER;
		bool flag2 = !flag;
		this.heartControllers[1].GetComponent<dfPanel>().IsVisible = flag2;
		this.blankControllers[1].GetComponent<dfPanel>().IsVisible = flag2;
		this.ammoControllers[1].GetComponent<dfPanel>().IsEnabled = flag2;
		this.ammoControllers[1].GetComponent<dfPanel>().IsVisible = flag2;
		this.itemControllers[0].transform.position = this.ammoControllers[1].GunBoxSprite.transform.position + new Vector3((3f + this.ammoControllers[1].GunBoxSprite.Width + (float)(2 * this.ammoControllers[1].AdditionalGunBoxSprites.Count)) * num, 0f, 0f);
		this.itemControllers[1].transform.position = this.ammoControllers[0].GunBoxSprite.transform.position + new Vector3((3f + this.ammoControllers[0].GunBoxSprite.Width + (float)(2 * this.ammoControllers[0].AdditionalGunBoxSprites.Count)) * -1f * num, 0f, 0f);
		this.itemNameLabels[0].RelativePosition += new Vector3(this.ammoControllers[0].GunBoxSprite.Width * num, 0f, 0f);
		this.itemNameLabels[1].RelativePosition += new Vector3(-this.ammoControllers[0].GunBoxSprite.Width * num, 0f, 0f);
		dfLabel component = this.Manager.AddPrefab(this.p_needsReloadLabel.gameObject).GetComponent<dfLabel>();
		component.IsVisible = false;
		this.m_extantReloadLabels.Add(component);
		this.m_displayingReloadNeeded.Add(false);
		this.m_extantReloadBars.Add(this.p_secondaryPlayerReloadBar);
	}

	// Token: 0x06008CCB RID: 36043 RVA: 0x003B0604 File Offset: 0x003AE804
	protected void RecalculateTargetPositions()
	{
		if (this.motionInteriorPositions == null)
		{
			this.motionInteriorPositions = new Dictionary<dfControl, Vector3>();
		}
		else
		{
			this.motionInteriorPositions.Clear();
		}
		if (this.motionExteriorPositions == null)
		{
			this.motionExteriorPositions = new Dictionary<dfControl, Vector3>();
		}
		else
		{
			this.motionExteriorPositions.Clear();
		}
		for (int i = 0; i < this.motionGroups.Count; i++)
		{
			this.motionInteriorPositions.Add(this.motionGroups[i], this.GetActivePosition(this.motionGroups[i], this.motionDirections[i]));
			this.motionExteriorPositions.Add(this.motionGroups[i], this.GetInactivePosition(this.motionGroups[i], this.motionDirections[i]));
		}
	}

	// Token: 0x06008CCC RID: 36044 RVA: 0x003B06E4 File Offset: 0x003AE8E4
	public bool IsCoreUIVisible()
	{
		return !this.CoreUIHidden.Value;
	}

	// Token: 0x06008CCD RID: 36045 RVA: 0x003B06F4 File Offset: 0x003AE8F4
	public void HideCoreUI(string reason = "")
	{
		if (string.IsNullOrEmpty(reason))
		{
			reason = "generic";
		}
		bool value = this.CoreUIHidden.Value;
		this.CoreUIHidden.SetOverride(reason, true, null);
		if (this.CoreUIHidden.Value == value)
		{
			return;
		}
		this.RecalculateTargetPositions();
		base.StartCoroutine(this.CoreUITransition());
		for (int i = 0; i < this.m_extantReloadBars.Count; i++)
		{
			this.m_extantReloadBars[i].SetInvisibility(true, "CoreUI");
		}
	}

	// Token: 0x06008CCE RID: 36046 RVA: 0x003B0790 File Offset: 0x003AE990
	public GameUIAmmoController GetAmmoControllerForPlayerID(int playerID)
	{
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			return (playerID != 1) ? this.ammoControllers[1] : this.ammoControllers[0];
		}
		if (this.ammoControllers.Count > 1)
		{
			return (playerID != 0) ? this.ammoControllers[1] : this.ammoControllers[0];
		}
		return this.ammoControllers[0];
	}

	// Token: 0x06008CCF RID: 36047 RVA: 0x003B0814 File Offset: 0x003AEA14
	private GameUIItemController GetItemControllerForPlayerID(int playerID)
	{
		if (playerID >= this.itemControllers.Count)
		{
			return null;
		}
		return this.itemControllers[playerID];
	}

	// Token: 0x06008CD0 RID: 36048 RVA: 0x003B0838 File Offset: 0x003AEA38
	public void ToggleLowerPanels(bool targetVisible, bool permanent = false, string source = "")
	{
		if (targetVisible && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			return;
		}
		if (string.IsNullOrEmpty(source))
		{
			source = "generic";
		}
		this.ForceLowerPanelsInvisibleOverride.SetOverride(source, !targetVisible, null);
		for (int i = 0; i < this.ammoControllers.Count; i++)
		{
			bool flag = targetVisible;
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
			{
				flag = false;
			}
			if (i >= GameManager.Instance.AllPlayers.Length)
			{
				flag = false;
			}
			if (i >= GameManager.Instance.AllPlayers.Length || !GameManager.Instance.AllPlayers[i].IsGhost)
			{
				if (this.ForceLowerPanelsInvisibleOverride.Value)
				{
					flag = false;
				}
				GameUIAmmoController ammoControllerForPlayerID = this.GetAmmoControllerForPlayerID(i);
				GameUIItemController itemControllerForPlayerID = this.GetItemControllerForPlayerID(i);
				if (!ammoControllerForPlayerID.forceInvisiblePermanent)
				{
					dfPanel component = ammoControllerForPlayerID.GetComponent<dfPanel>();
					dfPanel component2 = itemControllerForPlayerID.GetComponent<dfPanel>();
					component.IsVisible = flag;
					component2.IsVisible = flag;
					ammoControllerForPlayerID.ToggleRenderers(flag);
					itemControllerForPlayerID.ToggleRenderers(flag);
					if (permanent)
					{
						ammoControllerForPlayerID.forceInvisiblePermanent = !flag;
					}
					ammoControllerForPlayerID.temporarilyPreventVisible = !flag;
					itemControllerForPlayerID.temporarilyPreventVisible = !flag;
				}
			}
		}
	}

	// Token: 0x06008CD1 RID: 36049 RVA: 0x003B0980 File Offset: 0x003AEB80
	public void ToggleItemPanels(bool targetVisible)
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			bool flag = targetVisible;
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
			{
				flag = false;
			}
			if (i >= GameManager.Instance.AllPlayers.Length)
			{
				flag = false;
			}
			if (i < GameManager.Instance.AllPlayers.Length && GameManager.Instance.AllPlayers[i].IsGhost)
			{
				flag = false;
			}
			GameUIItemController itemControllerForPlayerID = this.GetItemControllerForPlayerID(i);
			if (itemControllerForPlayerID)
			{
				dfPanel component = itemControllerForPlayerID.GetComponent<dfPanel>();
				component.IsVisible = flag;
				itemControllerForPlayerID.ToggleRenderers(flag);
				itemControllerForPlayerID.temporarilyPreventVisible = !flag;
			}
		}
	}

	// Token: 0x06008CD2 RID: 36050 RVA: 0x003B0A34 File Offset: 0x003AEC34
	public void MoveNonCoreGroupImmediately(dfControl control, bool offScreen = false)
	{
		if (this.motionInteriorPositions.ContainsKey(control) && this.motionExteriorPositions.ContainsKey(control))
		{
			if (offScreen)
			{
				control.RelativePosition = this.motionExteriorPositions[control];
			}
			else
			{
				control.RelativePosition = this.motionInteriorPositions[control];
			}
		}
	}

	// Token: 0x06008CD3 RID: 36051 RVA: 0x003B0A94 File Offset: 0x003AEC94
	public void MoveNonCoreGroupOnscreen(dfControl control, bool reversed = false)
	{
		if (this.customNonCoreMotionGroups.Contains(control))
		{
			base.StartCoroutine(this.NonCoreControlTransition(control, reversed));
		}
	}

	// Token: 0x06008CD4 RID: 36052 RVA: 0x003B0AB8 File Offset: 0x003AECB8
	private IEnumerator NonCoreControlTransition(dfControl control, bool reversed = false)
	{
		float transitionTime = 0.25f;
		float elapsed = 0f;
		while (elapsed < transitionTime)
		{
			elapsed += this.m_deltaTime;
			float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / transitionTime));
			if (reversed)
			{
				t = 1f - t;
			}
			control.RelativePosition = Vector3.Lerp(this.motionExteriorPositions[control], this.motionInteriorPositions[control], t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008CD5 RID: 36053 RVA: 0x003B0AE4 File Offset: 0x003AECE4
	public void ShowCoreUI(string reason = "")
	{
		if (string.IsNullOrEmpty(reason))
		{
			reason = "generic";
		}
		bool value = this.CoreUIHidden.Value;
		this.CoreUIHidden.SetOverride(reason, false, null);
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			return;
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			return;
		}
		if (value == this.CoreUIHidden.Value)
		{
			return;
		}
		this.RecalculateTargetPositions();
		base.StartCoroutine(this.CoreUITransition());
		for (int i = 0; i < this.m_extantReloadBars.Count; i++)
		{
			this.m_extantReloadBars[i].SetInvisibility(false, "CoreUI");
		}
	}

	// Token: 0x06008CD6 RID: 36054 RVA: 0x003B0BA0 File Offset: 0x003AEDA0
	public void TransitionTargetMotionGroup(dfControl motionGroup, bool targetVisibility, bool targetLockState, bool instant)
	{
		this.RecalculateTargetPositions();
		base.StartCoroutine(this.TransitionTargetMotionGroup_CR(motionGroup, targetVisibility, targetLockState, instant));
	}

	// Token: 0x06008CD7 RID: 36055 RVA: 0x003B0BBC File Offset: 0x003AEDBC
	private IEnumerator TransitionTargetMotionGroup_CR(dfControl motionGroup, bool targetVisibility, bool targetLockState, bool instant)
	{
		if (!this.motionExteriorPositions.ContainsKey(motionGroup) || !this.motionInteriorPositions.ContainsKey(motionGroup))
		{
			yield break;
		}
		Vector3 interiorPosition = this.motionInteriorPositions[motionGroup];
		Vector3 exteriorPosition = this.motionExteriorPositions[motionGroup];
		float transitionTime = 0.25f;
		float elapsed = 0f;
		if (instant)
		{
			transitionTime = 0f;
		}
		Color targetColor = ((!targetVisibility) ? new Color(0.3f, 0.3f, 0.3f) : Color.white);
		dfControl[] controls = motionGroup.GetComponentsInChildren<dfControl>();
		for (int i = 0; i < controls.Length; i++)
		{
			controls[i].Color = targetColor;
		}
		if (targetLockState && !this.lockedMotionGroups.Contains(motionGroup))
		{
			this.lockedMotionGroups.Add(motionGroup);
		}
		if (targetVisibility && motionGroup.RelativePosition.XY() == interiorPosition.XY())
		{
			yield break;
		}
		if (!targetVisibility && motionGroup.RelativePosition.XY() == exteriorPosition.XY())
		{
			yield break;
		}
		if (instant)
		{
			motionGroup.RelativePosition = ((!targetVisibility) ? exteriorPosition : interiorPosition);
		}
		else
		{
			while (elapsed < transitionTime)
			{
				elapsed += this.m_deltaTime;
				float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / transitionTime));
				if (!targetVisibility)
				{
					t = 1f - t;
				}
				motionGroup.RelativePosition = Vector3.Lerp(exteriorPosition, interiorPosition, t);
				yield return null;
			}
		}
		if (!targetLockState && this.lockedMotionGroups.Contains(motionGroup))
		{
			this.lockedMotionGroups.Remove(motionGroup);
		}
		yield break;
	}

	// Token: 0x06008CD8 RID: 36056 RVA: 0x003B0BF4 File Offset: 0x003AEDF4
	private IEnumerator CoreUITransition()
	{
		bool cachedVisibility = !this.CoreUIHidden.Value;
		float transitionTime = 0.25f;
		float elapsed = 0f;
		Color targetColor = (this.CoreUIHidden.Value ? new Color(0.3f, 0.3f, 0.3f) : Color.white);
		for (int i = 0; i < this.motionGroups.Count; i++)
		{
			if (!this.customNonCoreMotionGroups.Contains(this.motionGroups[i]))
			{
				if (!this.lockedMotionGroups.Contains(this.motionGroups[i]))
				{
					dfControl[] componentsInChildren = this.motionGroups[i].GetComponentsInChildren<dfControl>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].Color = targetColor;
					}
				}
			}
		}
		while (elapsed < transitionTime)
		{
			if (cachedVisibility != !this.CoreUIHidden.Value)
			{
				yield break;
			}
			elapsed += this.m_deltaTime;
			float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / transitionTime));
			if (this.CoreUIHidden.Value)
			{
				t = 1f - t;
			}
			for (int k = 0; k < this.motionGroups.Count; k++)
			{
				if (!this.customNonCoreMotionGroups.Contains(this.motionGroups[k]))
				{
					if (!this.lockedMotionGroups.Contains(this.motionGroups[k]))
					{
						if (this.motionExteriorPositions.ContainsKey(this.motionGroups[k]) && this.motionInteriorPositions.ContainsKey(this.motionGroups[k]))
						{
							this.motionGroups[k].RelativePosition = Vector3.Lerp(this.motionExteriorPositions[this.motionGroups[k]], this.motionInteriorPositions[this.motionGroups[k]], t);
						}
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008CD9 RID: 36057 RVA: 0x003B0C10 File Offset: 0x003AEE10
	public tk2dClippedSprite GetSpriteForUnfoldedItem(int playerID, int itemIndex)
	{
		GameUIItemController itemControllerForPlayerID = this.GetItemControllerForPlayerID(playerID);
		Transform parent = itemControllerForPlayerID.ItemBoxSprite.transform.parent;
		Transform transform = parent.Find("AdditionalItemBox" + IntToStringSansGarbage.GetStringForInt(itemIndex));
		if (transform != null)
		{
			dfSprite component = transform.GetComponent<dfSprite>();
			return component.transform.Find("AdditionalItemSprite").GetComponent<tk2dClippedSprite>();
		}
		return null;
	}

	// Token: 0x06008CDA RID: 36058 RVA: 0x003B0C7C File Offset: 0x003AEE7C
	public tk2dClippedSprite GetSpriteForUnfoldedGun(int playerID, int gunIndex)
	{
		GameUIAmmoController ammoControllerForPlayerID = this.GetAmmoControllerForPlayerID(playerID);
		Transform parent = ammoControllerForPlayerID.GunBoxSprite.transform.parent;
		Transform transform = parent.Find("AdditionalWeaponBox" + IntToStringSansGarbage.GetStringForInt(gunIndex));
		if (transform != null)
		{
			dfSprite component = transform.GetComponent<dfSprite>();
			dfSprite component2 = component.transform.GetChild(0).GetComponent<dfSprite>();
			return component.transform.Find("AdditionalGunSprite").GetComponent<tk2dClippedSprite>();
		}
		return null;
	}

	// Token: 0x06008CDB RID: 36059 RVA: 0x003B0CFC File Offset: 0x003AEEFC
	public void ToggleHighlightUnfoldedGun(int gunIndex, bool highlighted)
	{
		if (gunIndex == 0)
		{
			for (int i = 0; i < this.ammoControllers[0].gunSprites.Length; i++)
			{
				tk2dClippedSprite tk2dClippedSprite = this.ammoControllers[0].gunSprites[i];
				if (highlighted)
				{
					tk2dClippedSprite.renderer.enabled = false;
				}
				else
				{
					tk2dClippedSprite.renderer.enabled = true;
				}
			}
		}
		else
		{
			tk2dClippedSprite component = this.additionalGunBoxes[gunIndex - 1].transform.Find("AdditionalGunSprite").GetComponent<tk2dClippedSprite>();
			if (highlighted)
			{
				component.renderer.enabled = false;
			}
			else
			{
				component.renderer.enabled = true;
			}
		}
	}

	// Token: 0x06008CDC RID: 36060 RVA: 0x003B0DB8 File Offset: 0x003AEFB8
	public void UnfoldGunventory(bool doItems = true)
	{
		if (GameManager.Instance.PrimaryPlayer.inventory.AllGuns.Count > 8)
		{
			return;
		}
		if (!this.GunventoryFolded)
		{
			return;
		}
		this.GunventoryFolded = false;
		base.StartCoroutine(this.HandlePauseInventoryFolding(GameManager.Instance.PrimaryPlayer, true, doItems, -1f, 0, false));
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			base.StartCoroutine(this.HandlePauseInventoryFolding(GameManager.Instance.SecondaryPlayer, true, true, -1f, 0, false));
		}
	}

	// Token: 0x06008CDD RID: 36061 RVA: 0x003B0E48 File Offset: 0x003AF048
	public void RefoldGunventory()
	{
		if (this.GunventoryFolded)
		{
			return;
		}
		this.GunventoryFolded = true;
		base.StartCoroutine(this.HandlePauseInventoryFolding(GameManager.Instance.PrimaryPlayer, true, true, -1f, 0, false));
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			base.StartCoroutine(this.HandlePauseInventoryFolding(GameManager.Instance.SecondaryPlayer, true, true, -1f, 0, false));
		}
	}

	// Token: 0x06008CDE RID: 36062 RVA: 0x003B0EB8 File Offset: 0x003AF0B8
	private void DestroyAdditionalFrames(bool GunventoryFolded, GameUIAmmoController ammoController, GameUIItemController itemController, List<dfSprite> additionalGunFrames, List<dfSprite> additionalItemFrames, bool forceDestroy = false)
	{
		if (!GunventoryFolded)
		{
			if (ammoController != null)
			{
				for (int i = 0; i < ammoController.AdditionalGunBoxSprites.Count; i++)
				{
					dfControl dfControl = ammoController.AdditionalGunBoxSprites[i];
					if (dfControl)
					{
						dfControl.transform.parent = null;
						UnityEngine.Object.Destroy(dfControl.gameObject);
					}
				}
				ammoController.AdditionalGunBoxSprites.Clear();
			}
			if (itemController != null)
			{
				for (int j = 0; j < itemController.AdditionalItemBoxSprites.Count; j++)
				{
					dfControl dfControl2 = itemController.AdditionalItemBoxSprites[j];
					if (dfControl2)
					{
						dfControl2.transform.parent = null;
						UnityEngine.Object.Destroy(dfControl2.gameObject);
					}
				}
				itemController.AdditionalItemBoxSprites.Clear();
			}
		}
		if (!GunventoryFolded || forceDestroy)
		{
			if (additionalGunFrames != null)
			{
				for (int k = 0; k < additionalGunFrames.Count; k++)
				{
					dfSprite dfSprite = additionalGunFrames[k];
					if (dfSprite)
					{
						UnityEngine.Object.Destroy(dfSprite.gameObject);
					}
				}
			}
			if (additionalItemFrames != null)
			{
				for (int l = 0; l < additionalItemFrames.Count; l++)
				{
					dfSprite dfSprite2 = additionalItemFrames[l];
					if (dfSprite2)
					{
						UnityEngine.Object.Destroy(dfSprite2.gameObject);
					}
				}
			}
		}
		if (additionalGunFrames != null)
		{
			additionalGunFrames.Clear();
		}
		if (additionalItemFrames != null)
		{
			additionalItemFrames.Clear();
		}
	}

	// Token: 0x06008CDF RID: 36063 RVA: 0x003B1044 File Offset: 0x003AF244
	private void HandleStackedFrameFoldMotion(float t, dfSprite baseBoxSprite, List<dfSprite> additionalGunFrames, List<tk2dClippedSprite> gunSpritesByBox, Dictionary<tk2dClippedSprite, tk2dClippedSprite[]> gunToOutlineMap)
	{
		float num = this.gunNameLabels[0].PixelsToUnits();
		for (int i = 0; i < additionalGunFrames.Count; i++)
		{
			float num2 = 1f / (float)additionalGunFrames.Count;
			Vector3 vector = baseBoxSprite.RelativePosition - baseBoxSprite.Size.WithX(0f).ToVector3ZUp(0f) * (float)i;
			Vector3 vector2 = vector - baseBoxSprite.Size.WithX(0f).ToVector3ZUp(0f);
			float num3 = num2 * (float)i;
			float num4 = Mathf.Clamp01((t - num3) / num2);
			float num5 = Mathf.SmoothStep(0f, 1f, num4);
			additionalGunFrames[i].FillAmount = num5;
			additionalGunFrames[i].IsVisible = additionalGunFrames[i].FillAmount > 0f;
			tk2dClippedSprite tk2dClippedSprite = gunSpritesByBox[i];
			if (tk2dClippedSprite != null)
			{
				float num6 = tk2dClippedSprite.GetUntrimmedBounds().size.y / (additionalGunFrames[i].Size.y * num);
				float num7 = (1f - num6) / 2f;
				float num8 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((num4 - num7) / num6));
				tk2dClippedSprite.clipBottomLeft = new Vector2(0f, 1f - num8);
				for (int j = 0; j < gunToOutlineMap[tk2dClippedSprite].Length; j++)
				{
					gunToOutlineMap[tk2dClippedSprite][j].clipBottomLeft = new Vector2(0f, 1f - num8);
				}
			}
			additionalGunFrames[i].RelativePosition = Vector3.Lerp(vector, vector2, num5);
		}
	}

	// Token: 0x06008CE0 RID: 36064 RVA: 0x003B1214 File Offset: 0x003AF414
	private void UpdateFramedGunSprite(Gun sourceGun, dfSprite targetFrame, GameUIAmmoController ammoController)
	{
		tk2dBaseSprite sprite = sourceGun.GetSprite();
		tk2dClippedSprite componentInChildren = targetFrame.GetComponentInChildren<tk2dClippedSprite>();
		componentInChildren.SetSprite(sprite.Collection, sprite.spriteId);
		componentInChildren.scale = GameUIUtility.GetCurrentTK2D_DFScale(this.m_manager) * Vector3.one;
		Vector3 center = targetFrame.GetCenter();
		componentInChildren.transform.position = center + ammoController.GetOffsetVectorForGun(sourceGun, false);
	}

	// Token: 0x17001510 RID: 5392
	// (get) Token: 0x06008CE1 RID: 36065 RVA: 0x003B127C File Offset: 0x003AF47C
	public bool MetalGearActive
	{
		get
		{
			return this.m_metalGearGunSelectActive;
		}
	}

	// Token: 0x06008CE2 RID: 36066 RVA: 0x003B1284 File Offset: 0x003AF484
	public void TriggerMetalGearGunSelect(PlayerController sourcePlayer)
	{
		if (sourcePlayer.IsGhost)
		{
			return;
		}
		if (sourcePlayer.inventory.AllGuns.Count < 2)
		{
			return;
		}
		int num = -1;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			num = ((!sourcePlayer.IsPrimaryPlayer) ? (-1) : 1);
		}
		if (sourcePlayer.inventory.AllGuns.Count == 2)
		{
			num = 0;
		}
		base.StartCoroutine(this.HandleMetalGearGunSelect(sourcePlayer, num));
	}

	// Token: 0x06008CE3 RID: 36067 RVA: 0x003B1300 File Offset: 0x003AF500
	private void AssignClippedSpriteFadeFractions(tk2dClippedSprite gunSpr, float fadeScreenSpaceY, float fadeScreenSpaceXStart, float fadeScreenSpaceXEnd, bool leftAligned)
	{
		Dictionary<Texture, Material> dictionary = ((!leftAligned) ? this.MetalGearAtlasToFadeMaterialMapR : this.MetalGearAtlasToFadeMaterialMapL);
		Dictionary<Material, Material> dictionary2 = ((!leftAligned) ? this.MetalGearFadeToOutlineMaterialMapR : this.MetalGearFadeToOutlineMaterialMapL);
		Material sharedMaterial = gunSpr.renderer.sharedMaterial;
		Material material;
		if (dictionary.ContainsKey(sharedMaterial.mainTexture))
		{
			material = dictionary[sharedMaterial.mainTexture];
		}
		else
		{
			material = gunSpr.renderer.material;
			dictionary.Add(sharedMaterial.mainTexture, material);
		}
		if (sharedMaterial != material)
		{
			gunSpr.renderer.sharedMaterial = material;
		}
		gunSpr.usesOverrideMaterial = true;
		gunSpr.renderer.sharedMaterial.shader = ShaderCache.Acquire("tk2d/BlendVertexColorFadeRange");
		gunSpr.renderer.sharedMaterial.SetFloat("_YFadeStart", Mathf.Min(0.75f, fadeScreenSpaceY));
		gunSpr.renderer.sharedMaterial.SetFloat("_YFadeEnd", 0.03f);
		gunSpr.renderer.sharedMaterial.SetFloat("_XFadeStart", fadeScreenSpaceXStart);
		gunSpr.renderer.sharedMaterial.SetFloat("_XFadeEnd", fadeScreenSpaceXEnd);
		tk2dClippedSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites<tk2dClippedSprite>(gunSpr);
		if (outlineSprites != null && outlineSprites.Length > 0)
		{
			Material material2;
			if (dictionary2.ContainsKey(material))
			{
				material2 = dictionary2[material];
			}
			else
			{
				material2 = UnityEngine.Object.Instantiate<Material>(gunSpr.renderer.sharedMaterial);
				dictionary2.Add(material, material2);
			}
			material2.SetFloat("_YFadeStart", Mathf.Min(0.75f, fadeScreenSpaceY));
			material2.SetFloat("_YFadeEnd", 0.03f);
			material2.SetFloat("_XFadeStart", fadeScreenSpaceXStart);
			material2.SetFloat("_XFadeEnd", fadeScreenSpaceXEnd);
			material2.SetColor("_OverrideColor", new Color(1f, 1f, 1f, 1f));
			material2.SetFloat("_DivPower", 4f);
			for (int i = 0; i < outlineSprites.Length; i++)
			{
				if (outlineSprites[i])
				{
					outlineSprites[i].usesOverrideMaterial = true;
					outlineSprites[i].renderer.sharedMaterial = material2;
				}
			}
		}
	}

	// Token: 0x06008CE4 RID: 36068 RVA: 0x003B153C File Offset: 0x003AF73C
	private Material GetDFAtlasMaterialForMetalGear(Material source, bool leftAligned)
	{
		Dictionary<Material, Material> dictionary = ((!leftAligned) ? this.MetalGearDFAtlasMapR : this.MetalGearDFAtlasMapL);
		Material material;
		if (dictionary.ContainsKey(source))
		{
			material = dictionary[source];
		}
		else
		{
			material = UnityEngine.Object.Instantiate<Material>(source);
			material.shader = ShaderCache.Acquire("Daikon Forge/Default UI Shader FadeRange");
			dictionary.Add(source, material);
		}
		return material;
	}

	// Token: 0x06008CE5 RID: 36069 RVA: 0x003B159C File Offset: 0x003AF79C
	private void SetFadeMaterials(dfSprite targetSprite, bool leftAligned)
	{
		Material dfatlasMaterialForMetalGear = this.GetDFAtlasMaterialForMetalGear(targetSprite.Atlas.Material, leftAligned);
		targetSprite.OverrideMaterial = dfatlasMaterialForMetalGear;
	}

	// Token: 0x06008CE6 RID: 36070 RVA: 0x003B15C4 File Offset: 0x003AF7C4
	private void SetFadeFractions(dfSprite targetSprite, float fadeScreenSpaceXStart, float fadeScreenSpaceXEnd, float fadeScreenSpaceY, bool isLeftAligned)
	{
		Material dfatlasMaterialForMetalGear = this.GetDFAtlasMaterialForMetalGear(targetSprite.Atlas.Material, isLeftAligned);
		dfatlasMaterialForMetalGear.SetFloat("_YFadeStart", Mathf.Min(0.75f, fadeScreenSpaceY));
		dfatlasMaterialForMetalGear.SetFloat("_YFadeEnd", 0.03f);
		dfatlasMaterialForMetalGear.SetFloat("_XFadeStart", fadeScreenSpaceXStart);
		dfatlasMaterialForMetalGear.SetFloat("_XFadeEnd", fadeScreenSpaceXEnd);
		targetSprite.OverrideMaterial = dfatlasMaterialForMetalGear;
		dfMaterialCache.ForceUpdate(targetSprite.OverrideMaterial);
		targetSprite.Invalidate();
	}

	// Token: 0x06008CE7 RID: 36071 RVA: 0x003B163C File Offset: 0x003AF83C
	private IEnumerator HandleMetalGearGunSelect(PlayerController targetPlayer, int numToL)
	{
		GameUIAmmoController ammoController = this.ammoControllers[0];
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			ammoController = this.ammoControllers[(!targetPlayer.IsPrimaryPlayer) ? 0 : 1];
		}
		BraveInput inputInstance = BraveInput.GetInstanceForPlayer(targetPlayer.PlayerIDX);
		while (ammoController.IsFlipping)
		{
			if (!inputInstance.ActiveActions.GunQuickEquipAction.IsPressed && !targetPlayer.ForceMetalGearMenu)
			{
				targetPlayer.DoQuickEquip();
				yield break;
			}
			yield return null;
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.ToggleItemPanels(false);
		}
		this.ClearGunName(targetPlayer.IsPrimaryPlayer);
		targetPlayer.SetInputOverride("metal gear");
		BraveTime.RegisterTimeScaleMultiplier(0.05f, base.gameObject);
		this.m_metalGearGunSelectActive = true;
		Tribool gunSelectPhase = Tribool.Unready;
		List<dfSprite> additionalGunFrames = ((!targetPlayer.IsPrimaryPlayer) ? this.additionalGunBoxesSecondary : this.additionalGunBoxes);
		GunInventory playerInventory = targetPlayer.inventory;
		List<Gun> playerGuns = playerInventory.AllGuns;
		dfSprite baseBoxSprite = ammoController.GunBoxSprite;
		if (playerGuns.Count <= 1)
		{
			this.m_metalGearGunSelectActive = false;
			yield break;
		}
		Vector3 originalBaseBoxSpriteRelativePosition = baseBoxSprite.RelativePosition;
		dfSprite boxToMoveOffTop = null;
		dfSprite boxToMoveOffBottom = null;
		int totalGunShift = 0;
		float totalTimeMetalGeared = 0f;
		bool isTransitioning = false;
		int queuedTransition = 0;
		float transitionSpeed = 12.5f;
		float boxWidth = baseBoxSprite.Size.x + 3f;
		List<tk2dSprite> noAmmoIcons = new List<tk2dSprite>();
		Dictionary<dfSprite, Gun> frameToGunMap = new Dictionary<dfSprite, Gun>();
		Pixelator.Instance.FadeColor = Color.black;
		bool triedQueueLeft = false;
		bool triedQueueRight = false;
		bool prevStickLeft = true;
		bool prevStickRight = true;
		float ignoreStickTimer = 0f;
		bool isLeftAligned = targetPlayer.IsPrimaryPlayer && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER;
		while (this.m_metalGearGunSelectActive)
		{
			Pixelator.Instance.fade = 1f - Mathf.Clamp01(totalTimeMetalGeared * 8f) * 0.5f;
			if ((!inputInstance.ActiveActions.GunQuickEquipAction.IsPressed && !GameManager.Instance.PrimaryPlayer.ForceMetalGearMenu) || GameManager.IsBossIntro || GameManager.Instance.IsPaused || GameManager.Instance.IsLoadingLevel)
			{
				this.m_metalGearGunSelectActive = false;
				break;
			}
			if (gunSelectPhase == Tribool.Unready)
			{
				this.GunventoryFolded = false;
				base.StartCoroutine(this.HandlePauseInventoryFolding(targetPlayer, true, false, 0.1f, numToL, false));
				yield return null;
				for (int i = 0; i < additionalGunFrames.Count; i++)
				{
					dfSprite dfSprite = additionalGunFrames[i];
					dfSprite component = dfSprite.transform.GetChild(0).GetComponent<dfSprite>();
					this.SetFadeMaterials(dfSprite, isLeftAligned);
					this.SetFadeMaterials(component, isLeftAligned);
					float y = dfSprite.GUIManager.RenderCamera.WorldToViewportPoint(baseBoxSprite.transform.position + new Vector3(0f, baseBoxSprite.Size.y * (float)(additionalGunFrames.Count - (Mathf.Abs(numToL) + ((numToL != 0) ? (-1) : 0))) * baseBoxSprite.PixelsToUnits(), 0f)).y;
					float num = 0f;
					float num2 = 1f;
					if (numToL < 0)
					{
						num = dfSprite.GUIManager.RenderCamera.WorldToViewportPoint(additionalGunFrames[0].transform.position + new Vector3(boxWidth * -2f * baseBoxSprite.PixelsToUnits(), 0f, 0f)).x;
					}
					else if (numToL > 0)
					{
						num2 = dfSprite.GUIManager.RenderCamera.WorldToViewportPoint(additionalGunFrames[0].transform.position + new Vector3(boxWidth * 2f * baseBoxSprite.PixelsToUnits(), 0f, 0f)).x;
					}
					tk2dClippedSprite componentInChildren = dfSprite.GetComponentInChildren<tk2dClippedSprite>();
					this.AssignClippedSpriteFadeFractions(componentInChildren, y, num, num2, isLeftAligned);
					frameToGunMap.Add(dfSprite, playerGuns[(i + playerGuns.IndexOf(playerInventory.CurrentGun)) % playerGuns.Count]);
					if (frameToGunMap[dfSprite].CurrentAmmo == 0)
					{
						componentInChildren.renderer.material.SetFloat("_Saturation", 0f);
						tk2dSprite component2 = componentInChildren.transform.Find("NoAmmoIcon").GetComponent<tk2dSprite>();
						component2.transform.parent = dfSprite.transform;
						component2.HeightOffGround = 2f;
						component2.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE;
						component2.renderer.material.shader = ShaderCache.Acquire("tk2d/BlendVertexColorFadeRange");
						component2.renderer.material.SetFloat("_YFadeStart", Mathf.Min(0.75f, y));
						component2.renderer.material.SetFloat("_YFadeEnd", 0.03f);
						component2.renderer.material.SetFloat("_XFadeStart", num);
						component2.renderer.material.SetFloat("_XFadeEnd", num2);
						component2.scale = componentInChildren.scale;
						component2.transform.position = dfSprite.GetCenter().Quantize(0.0625f * component2.scale.x);
						noAmmoIcons.Add(component2);
					}
					this.SetFadeFractions(dfSprite, num, num2, y, isLeftAligned);
					this.SetFadeFractions(component, num, num2, y, isLeftAligned);
					dfSprite.Invalidate();
				}
				gunSelectPhase = Tribool.Ready;
			}
			else if (gunSelectPhase == Tribool.Ready)
			{
				if (!isTransitioning)
				{
					if (triedQueueLeft || queuedTransition > 0)
					{
						isTransitioning = true;
						queuedTransition = Mathf.Max(queuedTransition - 1, 0);
						totalGunShift--;
						if (boxToMoveOffTop != null)
						{
							UnityEngine.Object.Destroy(boxToMoveOffTop.gameObject);
							boxToMoveOffTop = null;
						}
						dfSprite dfSprite2 = additionalGunFrames[additionalGunFrames.Count - 1];
						if (numToL != 0 && additionalGunFrames.Count > 2)
						{
							dfSprite2 = additionalGunFrames[additionalGunFrames.Count - 2];
						}
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(dfSprite2.gameObject, dfSprite2.transform.position, Quaternion.identity);
						boxToMoveOffTop = gameObject.GetComponent<dfSprite>();
						dfSprite2.Parent.AddControl(boxToMoveOffTop);
						boxToMoveOffTop.RelativePosition = dfSprite2.RelativePosition;
						dfSprite component3 = boxToMoveOffTop.transform.GetChild(0).GetComponent<dfSprite>();
						if (numToL != 0 && additionalGunFrames.Count > 2)
						{
							dfSprite2.RelativePosition = originalBaseBoxSpriteRelativePosition.WithX(originalBaseBoxSpriteRelativePosition.x + boxWidth * 2f * Mathf.Sign((float)numToL));
						}
						else
						{
							dfSprite2.RelativePosition = originalBaseBoxSpriteRelativePosition.WithY(originalBaseBoxSpriteRelativePosition.y + baseBoxSprite.Size.y);
						}
						this.SetFadeMaterials(boxToMoveOffTop, isLeftAligned);
						this.SetFadeMaterials(component3, isLeftAligned);
						boxToMoveOffTop.Invalidate();
						additionalGunFrames.Insert(0, additionalGunFrames[additionalGunFrames.Count - 1]);
						additionalGunFrames.RemoveAt(additionalGunFrames.Count - 1);
					}
					else if (triedQueueRight || queuedTransition < 0)
					{
						isTransitioning = true;
						queuedTransition = Mathf.Min(queuedTransition + 1, 0);
						totalGunShift++;
						if (boxToMoveOffBottom != null)
						{
							UnityEngine.Object.Destroy(boxToMoveOffBottom.gameObject);
							boxToMoveOffBottom = null;
						}
						dfSprite dfSprite3 = additionalGunFrames[0];
						if (numToL != 0 && additionalGunFrames.Count > 2)
						{
							dfSprite3 = additionalGunFrames[additionalGunFrames.Count - 1];
						}
						GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(dfSprite3.gameObject, dfSprite3.transform.position, Quaternion.identity);
						boxToMoveOffBottom = gameObject2.GetComponent<dfSprite>();
						dfSprite3.Parent.AddControl(boxToMoveOffBottom);
						boxToMoveOffBottom.RelativePosition = dfSprite3.RelativePosition;
						dfSprite component4 = boxToMoveOffBottom.transform.GetChild(0).GetComponent<dfSprite>();
						if (numToL != 0 && additionalGunFrames.Count > 2)
						{
							dfSprite3.RelativePosition = originalBaseBoxSpriteRelativePosition.WithY(originalBaseBoxSpriteRelativePosition.y - baseBoxSprite.Size.y * (float)(additionalGunFrames.Count - 1));
						}
						else
						{
							dfSprite3.RelativePosition = originalBaseBoxSpriteRelativePosition.WithY(originalBaseBoxSpriteRelativePosition.y - baseBoxSprite.Size.y * (float)additionalGunFrames.Count);
						}
						this.SetFadeMaterials(boxToMoveOffBottom, isLeftAligned);
						this.SetFadeMaterials(component4, isLeftAligned);
						boxToMoveOffBottom.Invalidate();
						additionalGunFrames.Add(additionalGunFrames[0]);
						additionalGunFrames.RemoveAt(0);
					}
				}
				else if (isTransitioning)
				{
					if (triedQueueLeft)
					{
						queuedTransition++;
						triedQueueLeft = false;
					}
					else if (triedQueueRight)
					{
						queuedTransition--;
						triedQueueRight = false;
					}
				}
				bool flag = true;
				for (int j = 0; j < additionalGunFrames.Count; j++)
				{
					dfSprite dfSprite4 = additionalGunFrames[j];
					float num3 = 1f / (float)(additionalGunFrames.Count + 1);
					Vector3 vector = originalBaseBoxSpriteRelativePosition - baseBoxSprite.Size.WithX(0f).ToVector3ZUp(0f) * (float)(j - 1);
					Vector3 vector2 = vector - baseBoxSprite.Size.WithX(0f).ToVector3ZUp(0f);
					if (numToL != 0 && additionalGunFrames.Count > 2 && j == additionalGunFrames.Count - 1)
					{
						vector = originalBaseBoxSpriteRelativePosition;
						vector2 = vector + new Vector3(boxWidth, 0f, 0f) * Mathf.Sign((float)numToL);
					}
					float num4 = num3 * (float)j;
					float num5 = Mathf.Clamp01((1f - num4) / num3);
					float num6 = Mathf.SmoothStep(0f, 1f, num5);
					Vector3 vector3 = Vector3.Lerp(vector, vector2, num6);
					if (dfSprite4.RelativePosition.IntXY(VectorConversions.Round) != vector3.IntXY(VectorConversions.Round))
					{
						flag = false;
					}
					float num7 = this.m_deltaTime * baseBoxSprite.Size.y * transitionSpeed;
					float num8 = num7 * (baseBoxSprite.Size.x / baseBoxSprite.Size.y);
					dfSprite4.RelativePosition = BraveMathCollege.LShapedMoveTowards(dfSprite4.RelativePosition, vector3, num8, num7);
				}
				if (flag)
				{
					isTransitioning = false;
				}
				if (boxToMoveOffTop != null)
				{
					Vector3 vector4 = originalBaseBoxSpriteRelativePosition - baseBoxSprite.Size.WithX(0f).ToVector3ZUp(0f) * (float)(additionalGunFrames.Count - 1 - Mathf.Abs(numToL));
					Vector3 vector5 = vector4 - baseBoxSprite.Size.WithX(0f).ToVector3ZUp(0f);
					float num9 = this.m_deltaTime * baseBoxSprite.Size.y * transitionSpeed;
					float num10 = num9 * (baseBoxSprite.Size.x / baseBoxSprite.Size.y);
					boxToMoveOffTop.RelativePosition = BraveMathCollege.LShapedMoveTowards(boxToMoveOffTop.RelativePosition, vector5, num10, num9);
					if (boxToMoveOffTop.RelativePosition.IntXY(VectorConversions.Round) == vector5.IntXY(VectorConversions.Round))
					{
						UnityEngine.Object.Destroy(boxToMoveOffTop.gameObject);
						boxToMoveOffTop = null;
					}
				}
				if (boxToMoveOffBottom != null)
				{
					Vector3 vector6 = originalBaseBoxSpriteRelativePosition;
					Vector3 vector7 = vector6 + baseBoxSprite.Size.WithX(0f).ToVector3ZUp(0f);
					if (numToL != 0 && additionalGunFrames.Count > 2)
					{
						vector6 = originalBaseBoxSpriteRelativePosition + new Vector3(boxWidth * Mathf.Sign((float)numToL), 0f, 0f);
						vector7 = vector6 + new Vector3(boxWidth * Mathf.Sign((float)numToL), 0f, 0f);
					}
					float num11 = this.m_deltaTime * baseBoxSprite.Size.y * transitionSpeed;
					float num12 = num11 * (baseBoxSprite.Size.x / baseBoxSprite.Size.y);
					boxToMoveOffBottom.RelativePosition = BraveMathCollege.LShapedMoveTowards(boxToMoveOffBottom.RelativePosition, vector7, num12, num11);
					if (boxToMoveOffBottom.RelativePosition.IntXY(VectorConversions.Round) == vector7.IntXY(VectorConversions.Round))
					{
						UnityEngine.Object.Destroy(boxToMoveOffBottom.gameObject);
						boxToMoveOffBottom = null;
					}
				}
			}
			GungeonActions currentActions = inputInstance.ActiveActions;
			InputDevice currentDevice = inputInstance.ActiveActions.Device;
			bool gunUp = inputInstance.IsKeyboardAndMouse(true) && currentActions.GunUpAction.WasPressed;
			bool gunDown = inputInstance.IsKeyboardAndMouse(true) && currentActions.GunDownAction.WasPressed;
			if (targetPlayer.ForceMetalGearMenu)
			{
				gunUp = true;
			}
			if (!gunUp && !gunDown && currentDevice != null && (!inputInstance.IsKeyboardAndMouse(true) || GameManager.Options.AllowMoveKeysToChangeGuns))
			{
				bool flag2 = currentDevice.DPadRight.WasPressedRepeating || currentDevice.DPadUp.WasPressedRepeating;
				bool flag3 = currentDevice.DPadLeft.WasPressedRepeating || currentDevice.DPadDown.WasPressedRepeating;
				if (flag2 || flag3)
				{
					ignoreStickTimer = 0.25f;
				}
				bool flag4 = false;
				bool flag5 = false;
				if (ignoreStickTimer <= 0f)
				{
					flag4 |= currentDevice.LeftStickDown.RawValue > 0.4f || currentActions.Down.RawValue > 0.4f;
					flag5 |= currentDevice.LeftStickUp.RawValue > 0.4f || currentActions.Up.RawValue > 0.4f;
					flag4 |= currentDevice.LeftStickLeft.RawValue > 0.4f || currentActions.Left.RawValue > 0.4f;
					flag5 |= currentDevice.LeftStickRight.RawValue > 0.4f || currentActions.Right.RawValue > 0.4f;
				}
				triedQueueLeft = flag3 || (flag4 && !prevStickLeft);
				triedQueueRight = flag2 || (flag5 && !prevStickRight);
				prevStickLeft = flag4;
				prevStickRight = flag5;
			}
			else
			{
				triedQueueLeft = gunUp;
				triedQueueRight = gunDown;
			}
			yield return null;
			ignoreStickTimer = Mathf.Max(0f, ignoreStickTimer - GameManager.INVARIANT_DELTA_TIME);
			totalTimeMetalGeared += GameManager.INVARIANT_DELTA_TIME;
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.ToggleItemPanels(true);
		}
		Pixelator.Instance.fade = 1f;
		if (boxToMoveOffTop != null)
		{
			UnityEngine.Object.Destroy(boxToMoveOffTop.gameObject);
			boxToMoveOffTop = null;
		}
		if (boxToMoveOffBottom != null)
		{
			UnityEngine.Object.Destroy(boxToMoveOffBottom.gameObject);
			boxToMoveOffBottom = null;
		}
		totalGunShift -= queuedTransition;
		if (totalGunShift % targetPlayer.inventory.AllGuns.Count != 0)
		{
			targetPlayer.CacheQuickEquipGun();
			targetPlayer.ChangeGun(totalGunShift, true, false);
			ammoController.SuppressNextGunFlip = true;
		}
		else
		{
			this.TemporarilyShowGunName(targetPlayer.IsPrimaryPlayer);
		}
		BraveTime.ClearMultiplier(base.gameObject);
		targetPlayer.ClearInputOverride("metal gear");
		this.m_metalGearGunSelectActive = false;
		if (totalGunShift == 0 && totalTimeMetalGeared < 0.005f)
		{
			targetPlayer.DoQuickEquip();
		}
		for (int k = 0; k < noAmmoIcons.Count; k++)
		{
			UnityEngine.Object.Destroy(noAmmoIcons[k].gameObject);
		}
		this.GunventoryFolded = true;
		yield return base.StartCoroutine(this.HandlePauseInventoryFolding(targetPlayer, true, false, 0.25f, numToL, true));
		ammoController.GunAmmoCountLabel.IsVisible = true;
		yield break;
	}

	// Token: 0x06008CE8 RID: 36072 RVA: 0x003B1668 File Offset: 0x003AF868
	private IEnumerator HandlePauseInventoryFolding(PlayerController targetPlayer, bool doGuns = true, bool doItems = true, float overrideTransitionTime = -1f, int numToL = 0, bool forceUseExistingList = false)
	{
		if (targetPlayer.IsGhost)
		{
			yield break;
		}
		GunInventory playerInventory = targetPlayer.inventory;
		List<Gun> playerGuns = playerInventory.AllGuns;
		List<PlayerItem> playerItems = targetPlayer.activeItems;
		GameUIAmmoController ammoController = this.ammoControllers[0];
		GameUIItemController itemController = this.itemControllers[0];
		List<dfSprite> additionalGunFrames = ((!targetPlayer.IsPrimaryPlayer) ? this.additionalGunBoxesSecondary : this.additionalGunBoxes);
		List<dfSprite> additionalItemFrames = ((!targetPlayer.IsPrimaryPlayer) ? this.additionalItemBoxesSecondary : this.additionalItemBoxes);
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			ammoController = this.ammoControllers[(!targetPlayer.IsPrimaryPlayer) ? 0 : 1];
			itemController = this.itemControllers[(!targetPlayer.IsPrimaryPlayer) ? 1 : 0];
		}
		ammoController.GunAmmoCountLabel.IsVisible = this.GunventoryFolded && ammoController.GunBoxSprite.IsVisible;
		bool cachedFoldedness = this.GunventoryFolded;
		float transitionTime = ((!this.GunventoryFolded) ? 0.4f : 0.2f);
		if (overrideTransitionTime > 0f)
		{
			transitionTime = overrideTransitionTime;
		}
		float elapsed = 0f;
		if (!forceUseExistingList)
		{
			this.DestroyAdditionalFrames(this.GunventoryFolded, (!doGuns) ? null : ammoController, (!doItems) ? null : itemController, (!doGuns) ? null : additionalGunFrames, (!doItems) ? null : additionalItemFrames, false);
		}
		List<tk2dClippedSprite> gunSpritesByBox = new List<tk2dClippedSprite>();
		Dictionary<tk2dClippedSprite, tk2dClippedSprite[]> gunToOutlineMap = new Dictionary<tk2dClippedSprite, tk2dClippedSprite[]>();
		List<tk2dClippedSprite> itemSpritesByBox = new List<tk2dClippedSprite>();
		Dictionary<tk2dClippedSprite, tk2dClippedSprite[]> itemToOutlineMap = new Dictionary<tk2dClippedSprite, tk2dClippedSprite[]>();
		dfSprite baseBoxSprite = ammoController.GunBoxSprite;
		dfSprite baseItemBoxSprite = itemController.ItemBoxSprite;
		bool isLeftAligned = targetPlayer.IsPrimaryPlayer && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER;
		if (doGuns)
		{
			baseBoxSprite.IsVisible = this.GunventoryFolded;
			if (this.GunventoryFolded)
			{
				ammoController.UndimGunSprite();
			}
			else
			{
				ammoController.DimGunSprite();
			}
		}
		if (doItems)
		{
			baseItemBoxSprite.IsVisible = this.GunventoryFolded;
			if (this.GunventoryFolded)
			{
				itemController.UndimItemSprite();
			}
			else
			{
				itemController.DimItemSprite();
			}
		}
		if (this.GunventoryFolded)
		{
			Transform parent = ammoController.GunBoxSprite.transform.parent;
			Transform parent2 = itemController.ItemBoxSprite.transform.parent;
			if (doGuns)
			{
				if (!forceUseExistingList)
				{
					for (int i = 0; i < playerGuns.Count; i++)
					{
						Transform transform = parent.Find("AdditionalWeaponBox" + IntToStringSansGarbage.GetStringForInt(i));
						if (transform != null)
						{
							dfSprite component = transform.GetComponent<dfSprite>();
							additionalGunFrames.Add(component);
							dfSprite component2 = component.transform.GetChild(0).GetComponent<dfSprite>();
							component2.IsVisible = targetPlayer.IsQuickEquipGun(playerGuns[i]);
							tk2dClippedSprite component3 = component.transform.Find("AdditionalGunSprite").GetComponent<tk2dClippedSprite>();
							gunSpritesByBox.Add(component3);
							gunToOutlineMap.Add(component3, SpriteOutlineManager.GetOutlineSprites<tk2dClippedSprite>(component3));
						}
					}
				}
				else
				{
					for (int j = 0; j < additionalGunFrames.Count; j++)
					{
						dfSprite dfSprite = additionalGunFrames[j];
						dfSprite component4 = dfSprite.transform.GetChild(0).GetComponent<dfSprite>();
						component4.IsVisible = false;
						tk2dClippedSprite component5 = dfSprite.transform.Find("AdditionalGunSprite").GetComponent<tk2dClippedSprite>();
						gunSpritesByBox.Add(component5);
						gunToOutlineMap.Add(component5, SpriteOutlineManager.GetOutlineSprites<tk2dClippedSprite>(component5));
					}
				}
			}
			if (doItems)
			{
				Transform transform2 = parent2.Find("AdditionalItemBox" + IntToStringSansGarbage.GetStringForInt(0));
				int num = 0;
				while (transform2 && num < 50)
				{
					dfSprite component6 = transform2.GetComponent<dfSprite>();
					additionalItemFrames.Add(component6);
					tk2dClippedSprite component7 = component6.transform.Find("AdditionalItemSprite").GetComponent<tk2dClippedSprite>();
					itemSpritesByBox.Add(component7);
					itemToOutlineMap.Add(component7, SpriteOutlineManager.GetOutlineSprites<tk2dClippedSprite>(component7));
					num++;
					transform2 = parent2.Find("AdditionalItemBox" + IntToStringSansGarbage.GetStringForInt(num));
				}
			}
		}
		else
		{
			int num2 = 0;
			if (doGuns)
			{
				int num3 = playerGuns.IndexOf(targetPlayer.CurrentGun);
				int num4 = num3 + playerGuns.Count;
				for (int k = num3; k < num4; k++)
				{
					int num5 = k % playerGuns.Count;
					if (num5 >= 0 && num5 < playerGuns.Count)
					{
						dfSprite component8 = UnityEngine.Object.Instantiate<GameObject>(baseBoxSprite.gameObject).GetComponent<dfSprite>();
						component8.IsVisible = true;
						component8.enabled = true;
						baseBoxSprite.Parent.AddControl(component8);
						component8.RelativePosition = baseBoxSprite.RelativePosition;
						component8.gameObject.name = "AdditionalWeaponBox" + IntToStringSansGarbage.GetStringForInt(num2);
						component8.FillDirection = dfFillDirection.Vertical;
						component8.FillAmount = 0f;
						component8.OverrideMaterial = this.GetDFAtlasMaterialForMetalGear(component8.Atlas.Material, isLeftAligned);
						component8.OverrideMaterial.SetFloat("_YFadeStart", 0.75f);
						tk2dBaseSprite sprite = playerGuns[num5].GetSprite();
						GameObject gameObject = new GameObject("AdditionalGunSprite");
						tk2dClippedSprite tk2dClippedSprite = tk2dBaseSprite.AddComponent<tk2dClippedSprite>(gameObject, sprite.Collection, sprite.spriteId);
						tk2dClippedSprite.scale = GameUIUtility.GetCurrentTK2D_DFScale(this.m_manager) * Vector3.one;
						Vector3 center = component8.GetCenter();
						tk2dClippedSprite.transform.position = center + ammoController.GetOffsetVectorForGun(playerGuns[num5], false);
						tk2dClippedSprite.transform.position = tk2dClippedSprite.transform.position.Quantize(component8.PixelsToUnits() * 3f);
						gameObject.transform.parent = component8.transform;
						gameObject.SetLayerRecursively(LayerMask.NameToLayer("GUI"));
						tk2dClippedSprite.ignoresTiltworldDepth = true;
						Material material = null;
						Texture mainTexture = tk2dClippedSprite.renderer.sharedMaterial.mainTexture;
						Dictionary<Texture, Material> dictionary = ((!isLeftAligned) ? this.MetalGearAtlasToFadeMaterialMapR : this.MetalGearAtlasToFadeMaterialMapL);
						Dictionary<Material, Material> dictionary2 = ((!isLeftAligned) ? this.MetalGearFadeToOutlineMaterialMapR : this.MetalGearFadeToOutlineMaterialMapL);
						if (dictionary.ContainsKey(mainTexture))
						{
							Material material2 = dictionary[mainTexture];
							if (dictionary2.ContainsKey(material2))
							{
								material = dictionary2[material2];
							}
						}
						SpriteOutlineManager.AddOutlineToSprite<tk2dClippedSprite>(tk2dClippedSprite, Color.white, material);
						tk2dClippedSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites<tk2dClippedSprite>(tk2dClippedSprite);
						for (int l = 0; l < outlineSprites.Length; l++)
						{
							outlineSprites[l].scale = tk2dClippedSprite.scale;
						}
						dfSprite component9 = component8.transform.GetChild(0).GetComponent<dfSprite>();
						component9.IsVisible = targetPlayer.IsQuickEquipGun(playerGuns[num5]);
						additionalGunFrames.Add(component8);
						gunSpritesByBox.Add(tk2dClippedSprite);
						gunToOutlineMap.Add(tk2dClippedSprite, outlineSprites);
						num2++;
						this.AssignClippedSpriteFadeFractions(tk2dClippedSprite, 0.75f, 0f, 1f, isLeftAligned);
						if (doGuns && !doItems && playerGuns[num5].CurrentAmmo == 0)
						{
							tk2dClippedSprite.renderer.material.SetFloat("_Saturation", 0f);
							tk2dSprite component10 = ((GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/NoAmmoIcon", ".prefab"))).GetComponent<tk2dSprite>();
							component10.name = "NoAmmoIcon";
							component10.transform.parent = tk2dClippedSprite.transform;
							component10.HeightOffGround = 2f;
							component10.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE;
							component10.renderer.material.shader = ShaderCache.Acquire("tk2d/BlendVertexColorFadeRange");
							component10.scale = tk2dClippedSprite.scale;
							component10.transform.position = component8.GetCenter().Quantize(0.0625f * component10.scale.x);
							SpriteOutlineManager.RemoveOutlineFromSprite(tk2dClippedSprite, true);
						}
					}
				}
			}
			num2 = 0;
			if (doItems)
			{
				int num6 = playerItems.IndexOf(targetPlayer.CurrentItem);
				int num7 = num6 + playerItems.Count;
				for (int m = num6; m < num7; m++)
				{
					int num8 = m % playerItems.Count;
					if (num8 >= 0 && num8 < playerItems.Count)
					{
						dfSprite component11 = UnityEngine.Object.Instantiate<GameObject>(baseItemBoxSprite.gameObject).GetComponent<dfSprite>();
						baseItemBoxSprite.Parent.AddControl(component11);
						component11.RelativePosition = baseItemBoxSprite.RelativePosition;
						component11.gameObject.name = "AdditionalItemBox" + IntToStringSansGarbage.GetStringForInt(num2);
						component11.FillDirection = dfFillDirection.Vertical;
						component11.FillAmount = 0f;
						tk2dBaseSprite sprite2 = playerItems[num8].sprite;
						GameObject gameObject2 = new GameObject("AdditionalItemSprite");
						tk2dClippedSprite tk2dClippedSprite2 = tk2dBaseSprite.AddComponent<tk2dClippedSprite>(gameObject2, sprite2.Collection, sprite2.spriteId);
						tk2dClippedSprite2.scale = GameUIUtility.GetCurrentTK2D_DFScale(this.m_manager) * Vector3.one;
						Vector3 center2 = component11.GetCenter();
						tk2dClippedSprite2.transform.position = center2 + itemController.GetOffsetVectorForItem(playerItems[num8], false);
						tk2dClippedSprite2.transform.position = tk2dClippedSprite2.transform.position.Quantize(component11.PixelsToUnits() * 3f);
						gameObject2.transform.parent = component11.transform;
						gameObject2.SetLayerRecursively(LayerMask.NameToLayer("GUI"));
						tk2dClippedSprite2.ignoresTiltworldDepth = true;
						SpriteOutlineManager.AddOutlineToSprite<tk2dClippedSprite>(tk2dClippedSprite2, Color.white);
						tk2dClippedSprite[] outlineSprites2 = SpriteOutlineManager.GetOutlineSprites<tk2dClippedSprite>(tk2dClippedSprite2);
						for (int n = 0; n < outlineSprites2.Length; n++)
						{
							outlineSprites2[n].scale = tk2dClippedSprite2.scale;
						}
						additionalItemFrames.Add(component11);
						itemSpritesByBox.Add(tk2dClippedSprite2);
						itemToOutlineMap.Add(tk2dClippedSprite2, outlineSprites2);
						num2++;
					}
				}
			}
		}
		while (elapsed < transitionTime)
		{
			if (cachedFoldedness != this.GunventoryFolded)
			{
				yield break;
			}
			elapsed += this.m_deltaTime;
			float p2u = this.gunNameLabels[0].PixelsToUnits();
			float t = Mathf.Clamp01(elapsed / transitionTime);
			if (this.GunventoryFolded)
			{
				t = 1f - t;
			}
			if (doGuns)
			{
				for (int num9 = 0; num9 < additionalGunFrames.Count; num9++)
				{
					float num10 = 1f / (float)additionalGunFrames.Count;
					Vector3 vector = baseBoxSprite.RelativePosition - baseBoxSprite.Size.WithX(0f).ToVector3ZUp(0f) * (float)(num9 - 1);
					Vector3 vector2 = vector - baseBoxSprite.Size.WithX(0f).ToVector3ZUp(0f);
					if (numToL != 0 && additionalGunFrames.Count > 2 && num9 == additionalGunFrames.Count - 1)
					{
						vector = baseBoxSprite.RelativePosition;
						vector2 = vector + new Vector3(baseBoxSprite.Size.x + 3f, 0f, 0f) * Mathf.Sign((float)numToL);
					}
					float num11 = num10 * (float)num9;
					float num12 = Mathf.Clamp01((t - num11) / num10);
					float num13 = Mathf.SmoothStep(0f, 1f, num12);
					if (num9 == 0)
					{
						num12 = (float)((!this.GunventoryFolded) ? 1 : 0);
					}
					if (num9 == 0)
					{
						num13 = (float)((!this.GunventoryFolded) ? 1 : 0);
					}
					if (numToL != 0 && additionalGunFrames.Count > 2 && num9 == additionalGunFrames.Count - 1)
					{
						additionalGunFrames[num9].FillDirection = dfFillDirection.Horizontal;
						additionalGunFrames[num9].FillAmount = num13;
					}
					else
					{
						additionalGunFrames[num9].FillDirection = dfFillDirection.Vertical;
						additionalGunFrames[num9].FillAmount = num13;
					}
					additionalGunFrames[num9].IsVisible = additionalGunFrames[num9].FillAmount > 0f;
					tk2dClippedSprite tk2dClippedSprite3 = gunSpritesByBox[num9];
					if (tk2dClippedSprite3 != null)
					{
						if (numToL != 0 && additionalGunFrames.Count > 2 && num9 == additionalGunFrames.Count - 1)
						{
							float num14 = tk2dClippedSprite3.GetUntrimmedBounds().size.x / (additionalGunFrames[num9].Size.x * p2u);
							float num15 = (1f - num14) / 2f;
							float num16 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((num12 - num15) / num14));
							tk2dClippedSprite3.clipTopRight = new Vector2(num16, 1f);
							if (gunToOutlineMap[tk2dClippedSprite3] != null)
							{
								for (int num17 = 0; num17 < gunToOutlineMap[tk2dClippedSprite3].Length; num17++)
								{
									if (gunToOutlineMap[tk2dClippedSprite3][num17])
									{
										gunToOutlineMap[tk2dClippedSprite3][num17].clipTopRight = new Vector2(num16, 1f);
									}
								}
							}
						}
						else
						{
							float num18 = tk2dClippedSprite3.GetUntrimmedBounds().size.y / (additionalGunFrames[num9].Size.y * p2u);
							float num19 = (1f - num18) / 2f;
							float num20 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((num12 - num19) / num18));
							tk2dClippedSprite3.clipBottomLeft = new Vector2(0f, 1f - num20);
							if (gunToOutlineMap[tk2dClippedSprite3] != null)
							{
								for (int num21 = 0; num21 < gunToOutlineMap[tk2dClippedSprite3].Length; num21++)
								{
									if (gunToOutlineMap[tk2dClippedSprite3][num21])
									{
										gunToOutlineMap[tk2dClippedSprite3][num21].clipBottomLeft = new Vector2(0f, 1f - num20);
									}
								}
							}
						}
					}
					additionalGunFrames[num9].RelativePosition = Vector3.Lerp(vector, vector2, num13);
				}
			}
			if (doItems)
			{
				for (int num22 = 0; num22 < additionalItemFrames.Count; num22++)
				{
					float num23 = 1f / (float)additionalItemFrames.Count;
					Vector3 vector3 = baseItemBoxSprite.RelativePosition - baseItemBoxSprite.Size.WithX(0f).ToVector3ZUp(0f) * (float)(num22 - 1);
					Vector3 vector4 = vector3 - baseItemBoxSprite.Size.WithX(0f).ToVector3ZUp(0f);
					float num24 = num23 * (float)num22;
					float num25 = Mathf.Clamp01((t - num24) / num23);
					float num26 = Mathf.SmoothStep(0f, 1f, num25);
					if (num22 == 0)
					{
						num25 = (float)((!this.GunventoryFolded) ? 1 : 0);
					}
					if (num22 == 0)
					{
						num26 = (float)((!this.GunventoryFolded) ? 1 : 0);
					}
					additionalItemFrames[num22].FillAmount = num26;
					additionalItemFrames[num22].IsVisible = additionalItemFrames[num22].FillAmount > 0f;
					tk2dClippedSprite tk2dClippedSprite4 = itemSpritesByBox[num22];
					if (tk2dClippedSprite4 != null)
					{
						float num27 = tk2dClippedSprite4.GetUntrimmedBounds().size.y / (additionalItemFrames[num22].Size.y * p2u);
						float num28 = (1f - num27) / 2f;
						float num29 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((num25 - num28) / num27));
						tk2dClippedSprite4.clipBottomLeft = new Vector2(0f, 1f - num29);
						for (int num30 = 0; num30 < itemToOutlineMap[tk2dClippedSprite4].Length; num30++)
						{
							if (itemToOutlineMap[tk2dClippedSprite4][num30])
							{
								itemToOutlineMap[tk2dClippedSprite4][num30].clipBottomLeft = new Vector2(0f, 1f - num29);
							}
						}
					}
					additionalItemFrames[num22].RelativePosition = Vector3.Lerp(vector3, vector4, num26);
				}
			}
			yield return null;
		}
		if (this.GunventoryFolded)
		{
			this.DestroyAdditionalFrames(this.GunventoryFolded, (!doGuns) ? null : ammoController, (!doItems) ? null : itemController, (!doGuns) ? null : additionalGunFrames, (!doItems) ? null : additionalItemFrames, true);
		}
		ammoController.GunAmmoCountLabel.IsVisible = this.GunventoryFolded && ammoController.GunBoxSprite.IsVisible;
		yield break;
	}

	// Token: 0x06008CE9 RID: 36073 RVA: 0x003B16B0 File Offset: 0x003AF8B0
	public void ToggleUICamera(bool enable)
	{
		this.gunNameLabels[0].GetManager().RenderCamera.enabled = enable;
	}

	// Token: 0x17001511 RID: 5393
	// (get) Token: 0x06008CEA RID: 36074 RVA: 0x003B16D0 File Offset: 0x003AF8D0
	public static float GameUIScalar
	{
		get
		{
			if (GameManager.Instance.IsPaused)
			{
				return 1f;
			}
			if (TimeTubeCreditsController.IsTimeTubing)
			{
				return 1f;
			}
			return (!GameManager.Options.SmallUIEnabled) ? 1f : 0.5f;
		}
	}

	// Token: 0x06008CEB RID: 36075 RVA: 0x003B1720 File Offset: 0x003AF920
	public void UpdateScale()
	{
		for (int i = 0; i < this.heartControllers.Count; i++)
		{
			this.heartControllers[i].UpdateScale();
		}
		for (int j = 0; j < this.blankControllers.Count; j++)
		{
			this.blankControllers[j].UpdateScale();
		}
		for (int k = 0; k < this.ammoControllers.Count; k++)
		{
			this.ammoControllers[k].UpdateScale();
		}
		for (int l = 0; l < this.itemControllers.Count; l++)
		{
			this.itemControllers[l].UpdateScale();
		}
		for (int m = 0; m < this.gunNameLabels.Count; m++)
		{
			this.gunNameLabels[m].TextScale = Pixelator.Instance.CurrentTileScale;
		}
		for (int n = 0; n < this.itemNameLabels.Count; n++)
		{
			this.itemNameLabels[n].TextScale = Pixelator.Instance.CurrentTileScale;
		}
		if (this.m_manager != null)
		{
			this.m_manager.UIScale = Pixelator.Instance.ScaleTileScale / 3f * GameUIRoot.GameUIScalar;
		}
		if (this.OnScaleUpdate != null)
		{
			this.OnScaleUpdate();
		}
	}

	// Token: 0x06008CEC RID: 36076 RVA: 0x003B18A4 File Offset: 0x003AFAA4
	public void DisplayUndiePanel()
	{
		dfPanel component = this.undiePanel.GetComponent<dfPanel>();
		this.undiePanel.SetActive(true);
		component.ZOrder = 1500;
		dfGUIManager.PushModal(component);
	}

	// Token: 0x06008CED RID: 36077 RVA: 0x003B18DC File Offset: 0x003AFADC
	public float PixelsToUnits()
	{
		return this.Manager.PixelsToUnits();
	}

	// Token: 0x17001512 RID: 5394
	// (get) Token: 0x06008CEE RID: 36078 RVA: 0x003B18EC File Offset: 0x003AFAEC
	public dfGUIManager Manager
	{
		get
		{
			if (this.m_manager == null)
			{
				this.m_manager = base.GetComponent<dfGUIManager>();
			}
			return this.m_manager;
		}
	}

	// Token: 0x06008CEF RID: 36079 RVA: 0x003B1914 File Offset: 0x003AFB14
	public void DoNotification(EncounterTrackable trackable)
	{
		this.notificationController.DoNotification(trackable, false);
	}

	// Token: 0x06008CF0 RID: 36080 RVA: 0x003B1924 File Offset: 0x003AFB24
	public void UpdatePlayerBlankUI(PlayerController player)
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			return;
		}
		if (player.IsPrimaryPlayer)
		{
			this.blankControllers[0].UpdateBlanks(player.Blanks);
		}
		else
		{
			this.blankControllers[1].UpdateBlanks(player.Blanks);
		}
	}

	// Token: 0x06008CF1 RID: 36081 RVA: 0x003B1980 File Offset: 0x003AFB80
	private IEnumerator HandleGenericPositionLerp(dfControl targetControl, Vector3 delta, float duration)
	{
		float ela = 0f;
		Vector3 startPos = targetControl.RelativePosition;
		while (ela < duration)
		{
			ela += BraveTime.DeltaTime;
			targetControl.RelativePosition = Vector3.Lerp(startPos, startPos + delta, ela / duration);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008CF2 RID: 36082 RVA: 0x003B19AC File Offset: 0x003AFBAC
	public void TransitionToGhostUI(PlayerController player)
	{
	}

	// Token: 0x06008CF3 RID: 36083 RVA: 0x003B19BC File Offset: 0x003AFBBC
	public void UpdateGhostUI(PlayerController player)
	{
		if (player.IsGhost)
		{
		}
	}

	// Token: 0x06008CF4 RID: 36084 RVA: 0x003B19CC File Offset: 0x003AFBCC
	public void UpdatePlayerHealthUI(PlayerController player, HealthHaver hh)
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			return;
		}
		if (player.IsPrimaryPlayer)
		{
			this.heartControllers[0].UpdateHealth(hh);
		}
		else
		{
			this.heartControllers[1].UpdateHealth(hh);
		}
	}

	// Token: 0x06008CF5 RID: 36085 RVA: 0x003B1A20 File Offset: 0x003AFC20
	public void SetAmmoCountColor(Color targetcolor, PlayerController sourcePlayer)
	{
		int num = 0;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			num = ((!sourcePlayer.IsPrimaryPlayer) ? 0 : 1);
		}
		this.ammoControllers[num].SetAmmoCountLabelColor(targetcolor);
	}

	// Token: 0x06008CF6 RID: 36086 RVA: 0x003B1A64 File Offset: 0x003AFC64
	public void UpdateGunData(GunInventory inventory, int inventoryShift, PlayerController sourcePlayer)
	{
		if (sourcePlayer.healthHaver.IsDead)
		{
			return;
		}
		int num = 0;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			num = ((!sourcePlayer.IsPrimaryPlayer) ? 0 : 1);
		}
		this.UpdateGunDataInternal(sourcePlayer, inventory, inventoryShift, this.ammoControllers[num], num);
	}

	// Token: 0x06008CF7 RID: 36087 RVA: 0x003B1AC0 File Offset: 0x003AFCC0
	private void UpdateGunDataInternal(PlayerController targetPlayer, GunInventory inventory, int inventoryShift, GameUIAmmoController targetAmmoController, int labelTarget)
	{
		Gun currentGun = inventory.CurrentGun;
		float num = this.gunNameLabels[labelTarget].PixelsToUnits();
		if (currentGun != null)
		{
			EncounterTrackable component = currentGun.GetComponent<EncounterTrackable>();
			this.gunNameLabels[labelTarget].Text = ((!(component != null)) ? currentGun.gunName : component.GetModifiedDisplayName());
		}
		else
		{
			this.gunNameLabels[labelTarget].Text = string.Empty;
		}
		targetAmmoController.UpdateUIGun(inventory, inventoryShift);
		if (inventoryShift != 0)
		{
			this.TemporarilyShowGunName(targetPlayer.IsPrimaryPlayer);
		}
		if (currentGun != null && currentGun.ClipShotsRemaining == 0 && (currentGun.ClipCapacity > 1 || currentGun.ammo == 0) && !currentGun.IsReloading && !targetPlayer.IsInputOverridden && !currentGun.IsHeroSword)
		{
			targetPlayer.gunReloadDisplayTimer += BraveTime.DeltaTime;
			if (targetPlayer.gunReloadDisplayTimer > 0.25f)
			{
				this.InformNeedsReload(targetPlayer, new Vector3(targetPlayer.specRigidbody.UnitCenter.x - targetPlayer.transform.position.x, 1.25f, 0f), -1f, string.Empty);
			}
		}
		else if (!this.m_isDisplayingCustomReload)
		{
			if (this.m_displayingReloadNeeded.Count < 2)
			{
				this.m_displayingReloadNeeded.Add(false);
			}
			targetPlayer.gunReloadDisplayTimer = 0f;
			this.m_displayingReloadNeeded[(!targetPlayer.IsPrimaryPlayer) ? 1 : 0] = false;
		}
		else
		{
			targetPlayer.gunReloadDisplayTimer = 0f;
		}
		this.m_gunNameVisibilityTimers[labelTarget] -= this.m_deltaTime;
		if (this.m_gunNameVisibilityTimers[labelTarget] > 1f)
		{
			this.gunNameLabels[labelTarget].IsVisible = true;
			this.gunNameLabels[labelTarget].Opacity = 1f;
		}
		else if (this.m_gunNameVisibilityTimers[labelTarget] > 0f)
		{
			this.gunNameLabels[labelTarget].IsVisible = true;
			this.gunNameLabels[labelTarget].Opacity = this.m_gunNameVisibilityTimers[labelTarget];
		}
		else
		{
			this.gunNameLabels[labelTarget].IsVisible = false;
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.itemControllers[0].transform.position = this.ammoControllers[1].GunBoxSprite.transform.position + new Vector3((3f + this.ammoControllers[1].GunBoxSprite.Width + (float)(2 * this.ammoControllers[1].AdditionalGunBoxSprites.Count)) * num, 0f, 0f);
			this.itemControllers[1].transform.position = this.ammoControllers[0].GunBoxSprite.transform.position + new Vector3((3f + this.ammoControllers[0].GunBoxSprite.Width + (float)(2 * this.ammoControllers[0].AdditionalGunBoxSprites.Count)) * -1f * num, 0f, 0f);
			if (this.itemControllers[(labelTarget != 0) ? 0 : 1].ItemBoxSprite.IsVisible)
			{
				this.gunNameLabels[labelTarget].transform.position = this.itemNameLabels[(labelTarget != 0) ? 0 : 1].transform.position + new Vector3(0f, -1f * (this.itemNameLabels[labelTarget].Height * num), 0f);
			}
			else if (targetAmmoController.IsLeftAligned)
			{
				this.gunNameLabels[labelTarget].transform.position = this.gunNameLabels[labelTarget].transform.position.WithX(targetAmmoController.GunBoxSprite.transform.position.x + (targetAmmoController.GunBoxSprite.Width + 4f) * num).WithY(targetAmmoController.GunBoxSprite.transform.position.y);
			}
			else
			{
				this.gunNameLabels[labelTarget].transform.position = this.gunNameLabels[labelTarget].transform.position.WithX(targetAmmoController.GunBoxSprite.transform.position.x - (targetAmmoController.GunBoxSprite.Width + 4f) * num).WithY(targetAmmoController.GunBoxSprite.transform.position.y);
			}
		}
		else if (targetAmmoController.IsLeftAligned)
		{
			this.gunNameLabels[labelTarget].transform.position = this.gunNameLabels[labelTarget].transform.position.WithX(targetAmmoController.GunBoxSprite.transform.position.x + (targetAmmoController.GunBoxSprite.Width + 4f) * num).WithY(targetAmmoController.GunBoxSprite.transform.position.y);
		}
		else
		{
			this.gunNameLabels[labelTarget].transform.position = this.gunNameLabels[labelTarget].transform.position.WithX(targetAmmoController.GunBoxSprite.transform.position.x - (targetAmmoController.GunBoxSprite.Width + 4f) * num).WithY(targetAmmoController.GunBoxSprite.transform.position.y);
		}
	}

	// Token: 0x06008CF8 RID: 36088 RVA: 0x003B2104 File Offset: 0x003B0304
	public void TemporarilyShowGunName(bool primaryPlayer)
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			return;
		}
		int num = 0;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			num = ((!primaryPlayer) ? 0 : 1);
		}
		this.m_gunNameVisibilityTimers[num] = 3f;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.m_itemNameVisibilityTimers[(num != 0) ? 0 : 1] = 0f;
		}
	}

	// Token: 0x06008CF9 RID: 36089 RVA: 0x003B2178 File Offset: 0x003B0378
	public void TemporarilyShowItemName(bool primaryPlayer)
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			return;
		}
		int num = ((!primaryPlayer) ? 1 : 0);
		this.m_itemNameVisibilityTimers[num] = 3f;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.m_gunNameVisibilityTimers[(num != 0) ? 0 : 1] = 0f;
		}
	}

	// Token: 0x06008CFA RID: 36090 RVA: 0x003B21DC File Offset: 0x003B03DC
	public void ClearGunName(bool primaryPlayer)
	{
		int num = 0;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			num = ((!primaryPlayer) ? 0 : 1);
		}
		this.m_gunNameVisibilityTimers[num] = 0f;
		this.gunNameLabels[num].IsVisible = false;
	}

	// Token: 0x06008CFB RID: 36091 RVA: 0x003B2228 File Offset: 0x003B0428
	public void ClearItemName(bool primaryPlayer)
	{
		int num = ((!primaryPlayer) ? 1 : 0);
		this.m_itemNameVisibilityTimers[num] = 0f;
		this.itemNameLabels[num].IsVisible = false;
	}

	// Token: 0x06008CFC RID: 36092 RVA: 0x003B2264 File Offset: 0x003B0464
	public void UpdateItemData(PlayerController targetPlayer, PlayerItem item, List<PlayerItem> items)
	{
		int num = 0;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			num = ((!targetPlayer.IsPrimaryPlayer) ? 1 : 0);
		}
		string text = string.Empty;
		if (item != null)
		{
			EncounterTrackable component = item.GetComponent<EncounterTrackable>();
			text = ((!(component != null)) ? item.DisplayName : component.journalData.GetPrimaryDisplayName(false));
			if (!item.consumable || item.numberOfUses > 1)
			{
			}
		}
		this.m_itemNameVisibilityTimers[num] -= this.m_deltaTime;
		if (this.m_itemNameVisibilityTimers[num] > 1f)
		{
			this.itemNameLabels[num].IsVisible = true;
			this.itemNameLabels[num].Opacity = 1f;
		}
		else if (this.m_itemNameVisibilityTimers[num] > 0f)
		{
			this.itemNameLabels[num].IsVisible = true;
			this.itemNameLabels[num].Opacity = this.m_itemNameVisibilityTimers[num];
		}
		else
		{
			this.itemNameLabels[num].IsVisible = false;
		}
		this.itemNameLabels[num].Text = text;
		GameUIItemController gameUIItemController = this.itemControllers[num];
		float num2 = gameUIItemController.ItemBoxSprite.PixelsToUnits();
		if (gameUIItemController.IsRightAligned)
		{
			this.itemNameLabels[num].transform.position = this.itemNameLabels[num].transform.position.WithX(gameUIItemController.ItemBoxSprite.transform.position.x + -4f * num2).WithY(gameUIItemController.ItemBoxSprite.transform.position.y + this.itemNameLabels[num].Height * num2);
		}
		else
		{
			this.itemNameLabels[num].transform.position = this.itemNameLabels[num].transform.position.WithX(gameUIItemController.ItemBoxSprite.transform.position.x + (gameUIItemController.ItemBoxSprite.Size.x + 4f) * num2).WithY(gameUIItemController.ItemBoxSprite.transform.position.y + this.itemNameLabels[num].Height * num2);
		}
		gameUIItemController.UpdateItem(item, items);
	}

	// Token: 0x06008CFD RID: 36093 RVA: 0x003B24F8 File Offset: 0x003B06F8
	public void UpdatePlayerConsumables(PlayerConsumables playerConsumables)
	{
		this.p_playerCoinLabel.Text = IntToStringSansGarbage.GetStringForInt(playerConsumables.Currency);
		this.p_playerKeyLabel.Text = IntToStringSansGarbage.GetStringForInt(playerConsumables.KeyBullets);
		this.UpdateSpecialKeys(playerConsumables);
		if (GameManager.Instance.PrimaryPlayer != null && GameManager.Instance.PrimaryPlayer.Blanks == 0)
		{
			this.p_playerCoinLabel.Parent.Parent.RelativePosition = this.p_playerCoinLabel.Parent.Parent.RelativePosition.WithY(this.blankControllers[0].Panel.RelativePosition.y);
			this.p_playerKeyLabel.Parent.Parent.RelativePosition = this.p_playerKeyLabel.Parent.Parent.RelativePosition.WithY(this.blankControllers[0].Panel.RelativePosition.y);
		}
		else
		{
			this.p_playerCoinLabel.Parent.Parent.RelativePosition = this.p_playerCoinLabel.Parent.Parent.RelativePosition.WithY(this.blankControllers[0].Panel.RelativePosition.y + this.blankControllers[0].Panel.Height - 9f);
			this.p_playerKeyLabel.Parent.Parent.RelativePosition = this.p_playerKeyLabel.Parent.Parent.RelativePosition.WithY(this.blankControllers[0].Panel.RelativePosition.y + this.blankControllers[0].Panel.Height - 9f);
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
		{
			int num = Mathf.RoundToInt(GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY));
			if (num > 0)
			{
				this.p_playerCoinLabel.Text = IntToStringSansGarbage.GetStringForInt(num);
				if (this.p_playerCoinSprite == null)
				{
					this.p_playerCoinSprite = this.p_playerCoinLabel.Parent.GetComponentInChildren<dfSprite>();
				}
				this.p_playerCoinSprite.SpriteName = "hbux_text_icon";
				this.p_playerCoinSprite.Size = this.p_playerCoinSprite.SpriteInfo.sizeInPixels * 3f;
			}
			else
			{
				if (this.p_playerCoinSprite == null)
				{
					this.p_playerCoinSprite = this.p_playerCoinLabel.Parent.GetComponentInChildren<dfSprite>();
				}
				this.p_playerCoinLabel.IsVisible = false;
				this.p_playerCoinSprite.IsVisible = false;
			}
		}
	}

	// Token: 0x06008CFE RID: 36094 RVA: 0x003B27B4 File Offset: 0x003B09B4
	private void UpdateSpecialKeys(PlayerConsumables playerConsumables)
	{
		bool flag = false;
		bool flag2 = false;
		int resourcefulRatKeys = playerConsumables.ResourcefulRatKeys;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			for (int j = 0; j < playerController.additionalItems.Count; j++)
			{
				if (playerController.additionalItems[j] is NPCCellKeyItem)
				{
					flag = true;
				}
			}
			for (int k = 0; k < playerController.passiveItems.Count; k++)
			{
				if (playerController.passiveItems[k] is SpecialKeyItem)
				{
					SpecialKeyItem specialKeyItem = playerController.passiveItems[k] as SpecialKeyItem;
					if (specialKeyItem.keyType == SpecialKeyItem.SpecialKeyType.RESOURCEFUL_RAT_LAIR)
					{
						flag2 = true;
					}
				}
			}
		}
		int count = this.m_extantSpecialKeySprites.Count;
		int num = resourcefulRatKeys + ((!flag2) ? 0 : 1) + ((!flag) ? 0 : 1);
		if (num != count)
		{
			for (int l = 0; l < this.m_extantSpecialKeySprites.Count; l++)
			{
				UnityEngine.Object.Destroy(this.m_extantSpecialKeySprites[l].gameObject);
			}
			this.m_extantSpecialKeySprites.Clear();
			for (int m = 0; m < num; m++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.p_specialKeySprite.gameObject);
				dfSprite component = gameObject.GetComponent<dfSprite>();
				component.IsVisible = true;
				this.p_specialKeySprite.Parent.AddControl(component);
				component.RelativePosition = this.p_specialKeySprite.RelativePosition + new Vector3((float)(33 * m), 0f, 0f);
				this.m_extantSpecialKeySprites.Add(component);
				bool flag3 = flag && m == 0;
				bool flag4 = ((!flag || !flag2) ? (flag2 && m == 0) : (m == 1));
				bool flag5 = !flag3 && !flag4;
				if (!flag3)
				{
					if (flag4)
					{
						component.SpriteName = "resourcefulrat_key_001";
						component.RelativePosition += new Vector3(9f, 15f, 0f);
					}
					else if (flag5)
					{
						component.SpriteName = "room_rat_reward_key_001";
						component.RelativePosition += new Vector3(6f, 12f, 0f);
					}
				}
				component.Size = component.SpriteInfo.sizeInPixels * 3f;
			}
			this.p_playerCoinLabel.Parent.Parent.RelativePosition = this.p_playerCoinLabel.Parent.Parent.RelativePosition.WithX(this.p_playerCoinLabel.Parent.Parent.RelativePosition.x + (float)((num - count) * 33));
		}
	}

	// Token: 0x06008CFF RID: 36095 RVA: 0x003B2ACC File Offset: 0x003B0CCC
	public bool AttemptActiveReload(PlayerController targetPlayer)
	{
		int num = ((!targetPlayer.IsPrimaryPlayer) ? 1 : 0);
		bool flag = this.m_extantReloadBars[num].AttemptActiveReload();
		if (flag)
		{
		}
		return flag;
	}

	// Token: 0x06008D00 RID: 36096 RVA: 0x003B2B08 File Offset: 0x003B0D08
	public void DoHealthBarForEnemy(AIActor sourceEnemy)
	{
		if (this.m_enemyToHealthbarMap.ContainsKey(sourceEnemy))
		{
			this.m_enemyToHealthbarMap[sourceEnemy].Value = sourceEnemy.healthHaver.GetCurrentHealthPercentage();
		}
		else if (this.m_unusedHealthbars.Count <= 0)
		{
			dfControl dfControl = this.m_manager.AddPrefab((GameObject)BraveResources.Load("Global Prefabs/EnemyHealthBar", ".prefab"));
			dfFollowObject component = dfControl.GetComponent<dfFollowObject>();
			component.mainCamera = GameManager.Instance.MainCameraController.GetComponent<Camera>();
			component.attach = sourceEnemy.gameObject;
			component.offset = new Vector3(0.5f, 2f, 0f);
			component.enabled = true;
			dfSlider component2 = component.GetComponent<dfSlider>();
			component2.Value = sourceEnemy.healthHaver.GetCurrentHealthPercentage();
			this.m_enemyToHealthbarMap.Add(sourceEnemy, component2);
		}
	}

	// Token: 0x06008D01 RID: 36097 RVA: 0x003B2BEC File Offset: 0x003B0DEC
	public void ForceClearReload(int targetPlayerIndex = -1)
	{
		for (int i = 0; i < this.m_extantReloadBars.Count; i++)
		{
			if (targetPlayerIndex == -1 || targetPlayerIndex == i)
			{
				this.m_extantReloadBars[i].CancelReload();
				this.m_extantReloadBars[i].UpdateStatusBars(null);
			}
		}
		for (int j = 0; j < this.m_displayingReloadNeeded.Count; j++)
		{
			if (targetPlayerIndex == -1 || targetPlayerIndex == j)
			{
				this.m_displayingReloadNeeded[j] = false;
			}
		}
	}

	// Token: 0x06008D02 RID: 36098 RVA: 0x003B2C88 File Offset: 0x003B0E88
	public void InformNeedsReload(PlayerController attachPlayer, Vector3 offset, float customDuration = -1f, string customKey = "")
	{
		if (!attachPlayer)
		{
			return;
		}
		int num = ((!attachPlayer.IsPrimaryPlayer) ? 1 : 0);
		if (this.m_displayingReloadNeeded == null || num >= this.m_displayingReloadNeeded.Count)
		{
			return;
		}
		if (this.m_extantReloadLabels == null || num >= this.m_extantReloadLabels.Count)
		{
			return;
		}
		if (this.m_displayingReloadNeeded[num])
		{
			return;
		}
		dfLabel dfLabel = this.m_extantReloadLabels[num];
		if (dfLabel == null || dfLabel.IsVisible)
		{
			return;
		}
		dfFollowObject component = dfLabel.GetComponent<dfFollowObject>();
		dfLabel.IsVisible = true;
		if (component)
		{
			component.enabled = false;
		}
		base.StartCoroutine(this.FlashReloadLabel(dfLabel, attachPlayer, offset, customDuration, customKey));
	}

	// Token: 0x06008D03 RID: 36099 RVA: 0x003B2D58 File Offset: 0x003B0F58
	protected override void InvariantUpdate(float realDeltaTime)
	{
		if (GameManager.Instance.IsLoadingLevel)
		{
			this.levelNameUI.BanishLevelNameText();
		}
		else
		{
			if (this.ForceLowerPanelsInvisibleOverride.HasOverride("conversation") && !GameManager.Instance.IsSelectingCharacter && GameManager.Instance.AllPlayers != null)
			{
				bool flag = true;
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					if (GameManager.Instance.AllPlayers[i] && GameManager.Instance.AllPlayers[i].CurrentInputState != PlayerInputState.AllInput)
					{
						flag = false;
					}
				}
				if (flag)
				{
					this.ToggleLowerPanels(true, false, "conversation");
				}
			}
			if (!this.m_displayingPlayerConversationOptions && this.ForceLowerPanelsInvisibleOverride.HasOverride("conversationBar"))
			{
				this.ToggleLowerPanels(true, false, "conversationBar");
			}
		}
	}

	// Token: 0x06008D04 RID: 36100 RVA: 0x003B2E44 File Offset: 0x003B1044
	private void UpdateReloadLabelsOnCameraFinishedFrame()
	{
		for (int i = 0; i < this.m_displayingReloadNeeded.Count; i++)
		{
			if (this.m_displayingReloadNeeded[i])
			{
				PlayerController playerController = GameManager.Instance.PrimaryPlayer;
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && i != 0)
				{
					playerController = GameManager.Instance.SecondaryPlayer;
				}
				dfControl dfControl = this.m_extantReloadLabels[i];
				float num = 0.125f;
				if (this.m_extantReloadLabels[i].GetLocalizationKey() == "#RELOAD_FULL")
				{
					num = 0.1875f;
				}
				float num2 = 0f;
				if (playerController && playerController.CurrentGun && playerController.CurrentGun.Handedness == GunHandedness.NoHanded)
				{
					num2 += 0.5f;
				}
				Vector3 vector = new Vector3(playerController.specRigidbody.UnitCenter.x - playerController.transform.position.x + num, playerController.SpriteDimensions.y + num2, 0f);
				Vector2 vector2 = dfFollowObject.ConvertWorldSpaces(playerController.transform.position + vector, GameManager.Instance.MainCameraController.Camera, this.Manager.RenderCamera).WithZ(0f);
				dfControl.transform.position = vector2;
				dfControl.transform.position = dfControl.transform.position.QuantizeFloor(dfControl.PixelsToUnits() / (Pixelator.Instance.ScaleTileScale / Pixelator.Instance.CurrentTileScale));
			}
		}
	}

	// Token: 0x06008D05 RID: 36101 RVA: 0x003B2FF4 File Offset: 0x003B11F4
	private IEnumerator FlashReloadLabel(dfControl target, PlayerController attachPlayer, Vector3 offset, float customDuration = -1f, string customStringKey = "")
	{
		int targetIndex = ((!attachPlayer.IsPrimaryPlayer) ? 1 : 0);
		this.m_displayingReloadNeeded[targetIndex] = true;
		target.transform.localScale = Vector3.one / GameUIRoot.GameUIScalar;
		dfLabel targetLabel = target as dfLabel;
		string customString = string.Empty;
		if (!string.IsNullOrEmpty(customStringKey))
		{
			customString = target.getLocalizedValue(customStringKey);
		}
		string reloadString = target.getLocalizedValue("#RELOAD");
		string emptyString = target.getLocalizedValue("#RELOAD_EMPTY");
		if (customDuration > 0f)
		{
			this.m_isDisplayingCustomReload = true;
			float outerElapsed = 0f;
			while (outerElapsed < customDuration && !GameManager.Instance.IsPaused)
			{
				target.IsVisible = true;
				targetLabel.Text = customString;
				targetLabel.Color = Color.white;
				outerElapsed += BraveTime.DeltaTime;
				yield return null;
			}
			this.m_isDisplayingCustomReload = false;
		}
		else
		{
			while (this.m_displayingReloadNeeded[targetIndex] && !GameManager.Instance.IsPaused)
			{
				target.IsVisible = true;
				if (!string.IsNullOrEmpty(customString))
				{
					targetLabel.Text = customString;
					targetLabel.Color = Color.white;
				}
				else if (attachPlayer.CurrentGun.CurrentAmmo != 0)
				{
					if (attachPlayer.CurrentGun.name.Contains("Beholster_Gun") && GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
					{
						targetLabel.Text = target.getLocalizedValue("#RELOAD_BEHOLD");
					}
					else
					{
						targetLabel.Text = reloadString;
					}
					targetLabel.Color = Color.white;
				}
				else
				{
					targetLabel.Text = emptyString;
					targetLabel.Color = Color.red;
				}
				bool shouldShowEver = customDuration > 0f || attachPlayer.CurrentGun.CurrentAmmo != 0 || attachPlayer.IsInCombat;
				float elapsed = 0f;
				while (elapsed < 0.6f)
				{
					elapsed += this.m_deltaTime;
					if (!this.m_displayingReloadNeeded[targetIndex])
					{
						target.IsVisible = false;
						yield break;
					}
					if (!shouldShowEver)
					{
						target.IsVisible = false;
					}
					if (GameManager.Instance.IsPaused)
					{
						target.IsVisible = false;
					}
					yield return null;
				}
				target.IsVisible = false;
				elapsed = 0f;
				while (elapsed < 0.6f)
				{
					elapsed += this.m_deltaTime;
					if (!this.m_displayingReloadNeeded[targetIndex])
					{
						target.IsVisible = false;
						yield break;
					}
					yield return null;
				}
			}
		}
		this.m_displayingReloadNeeded[targetIndex] = false;
		target.IsVisible = false;
		yield break;
	}

	// Token: 0x06008D06 RID: 36102 RVA: 0x003B3030 File Offset: 0x003B1230
	public void StartPlayerReloadBar(PlayerController attachObject, Vector3 offset, float duration)
	{
		int num = ((!attachObject.IsPrimaryPlayer) ? 1 : 0);
		if (num >= 0 && num < this.m_displayingReloadNeeded.Count)
		{
			this.m_displayingReloadNeeded[num] = false;
		}
		this.m_extantReloadBars[num].TriggerReload(attachObject, offset, duration, 0.65f, 1);
	}

	// Token: 0x06008D07 RID: 36103 RVA: 0x003B3090 File Offset: 0x003B1290
	public void TriggerBossKillCam(Projectile killerProjectile, SpeculativeRigidbody bossSRB)
	{
		if (this.m_bossKillCamActive)
		{
			return;
		}
		if (GameManager.Instance.InTutorial)
		{
			StaticReferenceManager.DestroyAllEnemyProjectiles();
			return;
		}
		StaticReferenceManager.DestroyAllEnemyProjectiles();
		this.m_bossKillCamActive = true;
		BossKillCam bossKillCam = base.gameObject.AddComponent<BossKillCam>();
		bossKillCam.TriggerSequence(killerProjectile, bossSRB);
	}

	// Token: 0x06008D08 RID: 36104 RVA: 0x003B30E0 File Offset: 0x003B12E0
	public void EndBossKillCam()
	{
		this.m_bossKillCamActive = false;
	}

	// Token: 0x06008D09 RID: 36105 RVA: 0x003B30EC File Offset: 0x003B12EC
	public void ShowPauseMenu()
	{
		AkSoundEngine.PostEvent("Play_UI_menu_pause_01", base.gameObject);
		GameUIRoot.Instance.ToggleLowerPanels(false, false, "gm_pause");
		GameUIRoot.Instance.HideCoreUI("gm_pause");
		this.levelNameUI.BanishLevelNameText();
		this.notificationController.ForceHide();
		GameUIRoot.Instance.ForceClearReload(-1);
		PauseMenuController component = this.PauseMenuPanel.GetComponent<PauseMenuController>();
		this.PauseMenuPanel.IsVisible = true;
		this.PauseMenuPanel.IsInteractive = true;
		this.PauseMenuPanel.IsEnabled = true;
		component.SetDefaultFocus();
		component.ShwoopOpen();
		component.SetDefaultFocus();
		dfGUIManager.PushModal(this.PauseMenuPanel);
	}

	// Token: 0x06008D0A RID: 36106 RVA: 0x003B3198 File Offset: 0x003B1398
	public bool HasOpenPauseSubmenu()
	{
		if (this.PauseMenuPanel == null)
		{
			return false;
		}
		if (this.m_pmc == null)
		{
			this.m_pmc = this.PauseMenuPanel.GetComponent<PauseMenuController>();
		}
		return !(this.m_pmc == null) && ((this.m_pmc.OptionsMenu != null && (this.m_pmc.OptionsMenu.IsVisible || this.m_pmc.OptionsMenu.PreOptionsMenu.IsVisible || this.m_pmc.OptionsMenu.ModalKeyBindingDialog.IsVisible)) || this.m_pmc.AdditionalMenuElementsToClear.Count > 0);
	}

	// Token: 0x06008D0B RID: 36107 RVA: 0x003B3264 File Offset: 0x003B1464
	public void ReturnToBasePause()
	{
		PauseMenuController component = this.PauseMenuPanel.GetComponent<PauseMenuController>();
		component.RevertToBaseState();
	}

	// Token: 0x06008D0C RID: 36108 RVA: 0x003B3284 File Offset: 0x003B1484
	public void HidePauseMenu()
	{
		PauseMenuController component = this.PauseMenuPanel.GetComponent<PauseMenuController>();
		if (this.PauseMenuPanel.IsVisible)
		{
			component.ShwoopClosed();
		}
		this.PauseMenuPanel.IsInteractive = false;
		if (component.OptionsMenu != null)
		{
			component.OptionsMenu.IsVisible = false;
			component.OptionsMenu.PreOptionsMenu.IsVisible = false;
		}
		if (this.PauseMenuPanel.IsVisible)
		{
			dfGUIManager.PopModalToControl(this.PauseMenuPanel, true);
		}
		if (AmmonomiconController.Instance != null && AmmonomiconController.Instance.IsOpen)
		{
			AmmonomiconController.Instance.CloseAmmonomicon(false);
		}
		AkSoundEngine.PostEvent("Play_UI_menu_cancel_01", base.gameObject);
		AkSoundEngine.PostEvent("Play_UI_menu_unpause_01", base.gameObject);
	}

	// Token: 0x17001513 RID: 5395
	// (get) Token: 0x06008D0D RID: 36109 RVA: 0x003B3358 File Offset: 0x003B1558
	public bool DisplayingConversationBar
	{
		get
		{
			return this.m_displayingPlayerConversationOptions;
		}
	}

	// Token: 0x06008D0E RID: 36110 RVA: 0x003B3360 File Offset: 0x003B1560
	public void InitializeConversationPortrait(PlayerController player)
	{
		PlayableCharacters characterIdentity = player.characterIdentity;
		dfSprite component = this.ConversationBar.transform.Find("FacecardFrame/Facecard").GetComponent<dfSprite>();
		switch (characterIdentity)
		{
		case PlayableCharacters.Pilot:
			component.SpriteName = "talking_bar_character_window_rogue_001";
			break;
		case PlayableCharacters.Convict:
			component.SpriteName = "talking_bar_character_window_convict_001";
			break;
		case PlayableCharacters.Soldier:
			component.SpriteName = "talking_bar_character_window_marine_001";
			break;
		case PlayableCharacters.Guide:
			component.SpriteName = "talking_bar_character_window_guide_001";
			break;
		case PlayableCharacters.Bullet:
			component.SpriteName = "talking_bar_character_window_bullet_001";
			break;
		case PlayableCharacters.Gunslinger:
			component.SpriteName = "talking_bar_character_window_slinger_003";
			break;
		}
	}

	// Token: 0x06008D0F RID: 36111 RVA: 0x003B342C File Offset: 0x003B162C
	public bool DisplayPlayerConversationOptions(PlayerController interactingPlayer, TalkModule sourceModule, string overrideResponse1 = "", string overrideResponse2 = "")
	{
		int num = ((sourceModule == null) ? 0 : sourceModule.responses.Count);
		if (!string.IsNullOrEmpty(overrideResponse1))
		{
			num = Mathf.Max(1, num);
		}
		if (!string.IsNullOrEmpty(overrideResponse2))
		{
			num = Mathf.Max(2, num);
		}
		string[] array = new string[num];
		for (int i = 0; i < num; i++)
		{
			if (sourceModule != null && sourceModule.responses.Count > i)
			{
				array[i] = StringTableManager.GetString(sourceModule.responses[i].response);
			}
		}
		if (!string.IsNullOrEmpty(overrideResponse1))
		{
			array[0] = overrideResponse1;
		}
		if (!string.IsNullOrEmpty(overrideResponse2))
		{
			array[1] = overrideResponse2;
		}
		return this.DisplayPlayerConversationOptions(interactingPlayer, array);
	}

	// Token: 0x06008D10 RID: 36112 RVA: 0x003B34EC File Offset: 0x003B16EC
	public bool DisplayPlayerConversationOptions(PlayerController interactingPlayer, string[] responses)
	{
		if (this.m_displayingPlayerConversationOptions)
		{
			return false;
		}
		this.m_displayingPlayerConversationOptions = true;
		this.hasSelectedOption = false;
		this.selectedResponse = 0;
		for (int i = 0; i < this.itemControllers.Count; i++)
		{
			this.itemControllers[i].DimItemSprite();
		}
		for (int j = 0; j < this.ammoControllers.Count; j++)
		{
			this.ammoControllers[j].DimGunSprite();
		}
		this.ToggleLowerPanels(false, false, "conversationBar");
		base.StartCoroutine(this.HandlePlayerConversationResponse(interactingPlayer, responses));
		return true;
	}

	// Token: 0x06008D11 RID: 36113 RVA: 0x003B3594 File Offset: 0x003B1794
	public void SetConversationResponse(int selected)
	{
		if (this.selectedResponse != selected)
		{
			this.selectedResponse = selected;
			AkSoundEngine.PostEvent("Play_UI_menu_select_01", base.gameObject);
		}
	}

	// Token: 0x06008D12 RID: 36114 RVA: 0x003B35BC File Offset: 0x003B17BC
	public void SelectConversationResponse()
	{
		this.hasSelectedOption = true;
		AkSoundEngine.PostEvent("Play_UI_menu_confirm_01", base.gameObject);
	}

	// Token: 0x06008D13 RID: 36115 RVA: 0x003B35D8 File Offset: 0x003B17D8
	private IEnumerator HandlePlayerConversationResponse(PlayerController interactingPlayer, string[] responses)
	{
		this.ConversationBar.ShowBar(interactingPlayer, responses);
		float timer = 0f;
		int numResponses = ((responses == null) ? 2 : responses.Length);
		while (!this.hasSelectedOption)
		{
			if (GameManager.Instance.IsPaused)
			{
				timer += BraveTime.DeltaTime;
				yield return null;
			}
			else
			{
				if (BraveInput.GetInstanceForPlayer(interactingPlayer.PlayerIDX).ActiveActions.SelectUp.WasPressedAsDpadRepeating)
				{
					this.selectedResponse = Mathf.Clamp(this.selectedResponse - 1, 0, numResponses - 1);
					AkSoundEngine.PostEvent("Play_UI_menu_select_01", base.gameObject);
				}
				else if (BraveInput.GetInstanceForPlayer(interactingPlayer.PlayerIDX).ActiveActions.SelectDown.WasPressedAsDpadRepeating)
				{
					this.selectedResponse = Mathf.Clamp(this.selectedResponse + 1, 0, numResponses - 1);
					AkSoundEngine.PostEvent("Play_UI_menu_select_01", base.gameObject);
				}
				else if (BraveInput.GetInstanceForPlayer(interactingPlayer.PlayerIDX).MenuInteractPressed && timer > 0.4f)
				{
					this.hasSelectedOption = true;
					AkSoundEngine.PostEvent("Play_UI_menu_confirm_01", base.gameObject);
					this.ToggleLowerPanels(true, false, "conversationBar");
				}
				this.ConversationBar.SetSelectedResponse(this.selectedResponse);
				yield return null;
				timer += BraveTime.DeltaTime;
			}
		}
		this.ConversationBar.HideBar();
		for (int i = 0; i < this.itemControllers.Count; i++)
		{
			this.itemControllers[i].UndimItemSprite();
		}
		for (int j = 0; j < this.ammoControllers.Count; j++)
		{
			this.ammoControllers[j].UndimGunSprite();
		}
		this.m_displayingPlayerConversationOptions = false;
		yield break;
	}

	// Token: 0x06008D14 RID: 36116 RVA: 0x003B3604 File Offset: 0x003B1804
	public bool GetPlayerConversationResponse(out int responseIndex)
	{
		responseIndex = this.selectedResponse;
		return this.hasSelectedOption;
	}

	// Token: 0x06008D15 RID: 36117 RVA: 0x003B3614 File Offset: 0x003B1814
	public static void ToggleBG(dfControl rawTarget)
	{
		if (rawTarget is dfButton)
		{
			dfButton dfButton = rawTarget as dfButton;
			if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
			{
				dfButton.BackgroundSprite = string.Empty;
				dfButton.Padding = new RectOffset(0, 0, 0, 0);
			}
			else
			{
				dfButton.BackgroundSprite = "chamber_flash_small_001";
				dfButton.Padding = new RectOffset(6, 6, 0, 0);
				dfButton.NormalBackgroundColor = Color.black;
				dfButton.FocusBackgroundColor = Color.black;
				dfButton.HoverBackgroundColor = Color.black;
				dfButton.DisabledColor = Color.black;
				dfButton.PressedBackgroundColor = Color.black;
			}
		}
		else if (rawTarget is dfLabel)
		{
			dfLabel dfLabel = rawTarget as dfLabel;
			if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
			{
				dfLabel.BackgroundSprite = string.Empty;
				dfLabel.Padding = new RectOffset(0, 0, 0, 0);
			}
			else
			{
				dfLabel.BackgroundSprite = "chamber_flash_small_001";
				dfLabel.Padding = new RectOffset(6, 6, 0, 0);
				dfLabel.BackgroundColor = Color.black;
			}
		}
	}

	// Token: 0x06008D16 RID: 36118 RVA: 0x003B3730 File Offset: 0x003B1930
	public void CheckKeepModifiersQuickRestart(int requiredCredits)
	{
		this.m_hasSelectedAreYouSureOption = false;
		this.KeepMetasIsVisible = true;
		dfPanel QuestionPanel = (dfPanel)this.m_manager.AddPrefab((GameObject)BraveResources.Load("QuickRestartDetailsPanel", ".prefab"));
		QuestionPanel.BringToFront();
		dfGUIManager.PushModal(QuestionPanel);
		dfControl component = QuestionPanel.transform.Find("AreYouSurePanelBGSlicedSprite").GetComponent<dfControl>();
		QuestionPanel.PerformLayout();
		component.PerformLayout();
		dfButton component2 = QuestionPanel.transform.Find("YesButton").GetComponent<dfButton>();
		dfButton component3 = QuestionPanel.transform.Find("NoButton").GetComponent<dfButton>();
		component2.ModifyLocalizedText(component2.Text + " (" + requiredCredits.ToString() + "[sprite \"hbux_text_icon\"])");
		float metas = GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY);
		if (metas >= (float)requiredCredits)
		{
			component2.Focus(true);
		}
		else
		{
			component2.Disable();
			component3.GetComponent<UIKeyControls>().up = null;
			component3.Focus(true);
		}
		dfLabel component4 = QuestionPanel.transform.Find("TopLabel").GetComponent<dfLabel>();
		component4.IsLocalized = true;
		component4.Text = component4.getLocalizedValue("#QUICKRESTARTDETAIL");
		Action<bool> HandleChoice = delegate(bool choice)
		{
			if (!this.m_hasSelectedAreYouSureOption)
			{
				if (choice)
				{
					GameStatsManager.Instance.ClearStatValueGlobal(TrackedStats.META_CURRENCY);
					GameStatsManager.Instance.SetStat(TrackedStats.META_CURRENCY, metas - (float)requiredCredits);
				}
				this.m_hasSelectedAreYouSureOption = true;
				this.m_AreYouSureSelection = choice;
				dfGUIManager.PopModal();
				QuestionPanel.IsVisible = false;
				this.KeepMetasIsVisible = false;
			}
		};
		component2.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			mouseEvent.Use();
			HandleChoice(true);
		};
		component3.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			mouseEvent.Use();
			HandleChoice(false);
		};
		base.StartCoroutine(this.DelayedCenterControl(component));
	}

	// Token: 0x06008D17 RID: 36119 RVA: 0x003B38F4 File Offset: 0x003B1AF4
	private IEnumerator DelayedCenterControl(dfControl panel)
	{
		yield return null;
		panel.Anchor = dfAnchorStyle.CenterHorizontal | dfAnchorStyle.CenterVertical;
		panel.PerformLayout();
		yield break;
	}

	// Token: 0x06008D18 RID: 36120 RVA: 0x003B3910 File Offset: 0x003B1B10
	public void DoAreYouSure(string questionKey, bool focusYes = false, string secondaryKey = null)
	{
		this.m_hasSelectedAreYouSureOption = false;
		this.AreYouSurePanel.IsVisible = true;
		dfGUIManager.PushModal(this.AreYouSurePanel);
		if (focusYes)
		{
			this.m_AreYouSureYesButton.Focus(true);
		}
		else
		{
			this.m_AreYouSureNoButton.Focus(true);
		}
		GameUIRoot.ToggleBG(this.m_AreYouSureYesButton);
		GameUIRoot.ToggleBG(this.m_AreYouSureNoButton);
		GameUIRoot.ToggleBG(this.m_AreYouSurePrimaryLabel);
		GameUIRoot.ToggleBG(this.m_AreYouSureSecondaryLabel);
		this.m_AreYouSurePrimaryLabel.IsLocalized = true;
		this.m_AreYouSurePrimaryLabel.Text = this.m_AreYouSurePrimaryLabel.getLocalizedValue(questionKey);
		if (!string.IsNullOrEmpty(secondaryKey))
		{
			this.m_AreYouSureSecondaryLabel.IsLocalized = true;
			this.m_AreYouSureSecondaryLabel.Text = this.m_AreYouSureSecondaryLabel.getLocalizedValue(secondaryKey);
			if (this.m_AreYouSureSecondaryLabel.Text.Contains("%CURRENTSLOT"))
			{
				string text;
				switch (SaveManager.CurrentSaveSlot)
				{
				case SaveManager.SaveSlot.A:
					text = "#OPTIONS_SAVESLOTA";
					break;
				case SaveManager.SaveSlot.B:
					text = "#OPTIONS_SAVESLOTB";
					break;
				case SaveManager.SaveSlot.C:
					text = "#OPTIONS_SAVESLOTC";
					break;
				case SaveManager.SaveSlot.D:
					text = "#OPTIONS_SAVESLOTD";
					break;
				default:
					text = "#OPTIONS_SAVESLOTA";
					break;
				}
				string text2 = this.m_AreYouSureSecondaryLabel.Text;
				text2 = text2.Replace("%CURRENTSLOT", this.m_AreYouSureSecondaryLabel.getLocalizedValue(text));
				this.m_AreYouSureSecondaryLabel.ModifyLocalizedText(StringTableManager.PostprocessString(text2));
			}
			else
			{
				this.m_AreYouSureSecondaryLabel.ModifyLocalizedText(StringTableManager.PostprocessString(this.m_AreYouSureSecondaryLabel.Text));
			}
			this.m_AreYouSureSecondaryLabel.IsVisible = true;
		}
		else
		{
			this.m_AreYouSureSecondaryLabel.IsVisible = false;
		}
		this.m_AreYouSureYesButton.Click += this.SelectedAreYouSureYes;
		this.m_AreYouSureNoButton.Click += this.SelectedAreYouSureNo;
	}

	// Token: 0x06008D19 RID: 36121 RVA: 0x003B3AF0 File Offset: 0x003B1CF0
	private void SelectedAreYouSureNo(dfControl control, dfMouseEventArgs mouseEvent)
	{
		mouseEvent.Use();
		this.SelectAreYouSureOption(false);
	}

	// Token: 0x06008D1A RID: 36122 RVA: 0x003B3B00 File Offset: 0x003B1D00
	private void SelectedAreYouSureYes(dfControl control, dfMouseEventArgs mouseEvent)
	{
		mouseEvent.Use();
		this.SelectAreYouSureOption(true);
	}

	// Token: 0x06008D1B RID: 36123 RVA: 0x003B3B10 File Offset: 0x003B1D10
	public void SelectAreYouSureOption(bool isSure)
	{
		this.m_AreYouSureNoButton.Click -= this.SelectedAreYouSureNo;
		this.m_AreYouSureYesButton.Click -= this.SelectedAreYouSureYes;
		this.m_hasSelectedAreYouSureOption = true;
		this.m_AreYouSureSelection = isSure;
		dfGUIManager.PopModal();
		this.AreYouSurePanel.IsVisible = false;
	}

	// Token: 0x06008D1C RID: 36124 RVA: 0x003B3B6C File Offset: 0x003B1D6C
	public bool HasSelectedAreYouSureOption()
	{
		return this.m_hasSelectedAreYouSureOption;
	}

	// Token: 0x06008D1D RID: 36125 RVA: 0x003B3B74 File Offset: 0x003B1D74
	public bool GetAreYouSureOption()
	{
		return this.m_AreYouSureSelection;
	}

	// Token: 0x06008D1E RID: 36126 RVA: 0x003B3B7C File Offset: 0x003B1D7C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		GameUIRoot.Instance = null;
	}

	// Token: 0x04009432 RID: 37938
	public static GameUIRoot m_root;

	// Token: 0x04009433 RID: 37939
	public dfLabel p_playerAmmoLabel;

	// Token: 0x04009434 RID: 37940
	public dfLabel FoyerAmmonomiconLabel;

	// Token: 0x04009435 RID: 37941
	public List<GameUIHeartController> heartControllers;

	// Token: 0x04009436 RID: 37942
	public List<GameUIAmmoController> ammoControllers;

	// Token: 0x04009437 RID: 37943
	public List<GameUIItemController> itemControllers;

	// Token: 0x04009438 RID: 37944
	public List<GameUIBlankController> blankControllers;

	// Token: 0x04009439 RID: 37945
	public GameUIBossHealthController bossController;

	// Token: 0x0400943A RID: 37946
	public GameUIBossHealthController bossController2;

	// Token: 0x0400943B RID: 37947
	public GameUIBossHealthController bossControllerSide;

	// Token: 0x0400943C RID: 37948
	public UINotificationController notificationController;

	// Token: 0x0400943D RID: 37949
	public dfPanel AreYouSurePanel;

	// Token: 0x0400943E RID: 37950
	public bool KeepMetasIsVisible;

	// Token: 0x0400943F RID: 37951
	public dfLabel p_playerCoinLabel;

	// Token: 0x04009440 RID: 37952
	public dfLabel p_playerKeyLabel;

	// Token: 0x04009441 RID: 37953
	public dfSprite p_specialKeySprite;

	// Token: 0x04009442 RID: 37954
	[NonSerialized]
	private List<dfSprite> m_extantSpecialKeySprites = new List<dfSprite>();

	// Token: 0x04009443 RID: 37955
	public dfLabel p_needsReloadLabel;

	// Token: 0x04009444 RID: 37956
	[NonSerialized]
	private List<dfLabel> m_extantReloadLabels;

	// Token: 0x04009445 RID: 37957
	public List<dfLabel> gunNameLabels;

	// Token: 0x04009446 RID: 37958
	public List<dfLabel> itemNameLabels;

	// Token: 0x04009447 RID: 37959
	public LevelNameUIManager levelNameUI;

	// Token: 0x04009448 RID: 37960
	public GameUIReloadBarController p_playerReloadBar;

	// Token: 0x04009449 RID: 37961
	public GameUIReloadBarController p_secondaryPlayerReloadBar;

	// Token: 0x0400944A RID: 37962
	[NonSerialized]
	private List<GameUIReloadBarController> m_extantReloadBars;

	// Token: 0x0400944B RID: 37963
	public GameObject undiePanel;

	// Token: 0x0400944C RID: 37964
	[Header("Dynamism")]
	[NonSerialized]
	private List<dfControl> customNonCoreMotionGroups = new List<dfControl>();

	// Token: 0x0400944D RID: 37965
	public List<dfControl> motionGroups;

	// Token: 0x0400944E RID: 37966
	public List<DungeonData.Direction> motionDirections;

	// Token: 0x0400944F RID: 37967
	[NonSerialized]
	private List<dfControl> lockedMotionGroups = new List<dfControl>();

	// Token: 0x04009450 RID: 37968
	[NonSerialized]
	protected Dictionary<dfControl, Vector3> motionInteriorPositions;

	// Token: 0x04009451 RID: 37969
	[NonSerialized]
	protected Dictionary<dfControl, Vector3> motionExteriorPositions;

	// Token: 0x04009452 RID: 37970
	[NonSerialized]
	public List<DefaultLabelController> extantBasicLabels = new List<DefaultLabelController>();

	// Token: 0x04009453 RID: 37971
	[Header("Demo Tutorial Panels")]
	public List<dfPanel> demoTutorialPanels_Keyboard;

	// Token: 0x04009454 RID: 37972
	public List<dfPanel> demoTutorialPanels_Controller;

	// Token: 0x04009455 RID: 37973
	private bool m_forceHideGunPanel;

	// Token: 0x04009456 RID: 37974
	private bool m_forceHideItemPanel;

	// Token: 0x04009457 RID: 37975
	private List<bool> m_displayingReloadNeeded;

	// Token: 0x04009458 RID: 37976
	private bool m_bossKillCamActive;

	// Token: 0x04009459 RID: 37977
	[NonSerialized]
	private float[] m_gunNameVisibilityTimers;

	// Token: 0x0400945A RID: 37978
	[NonSerialized]
	private float[] m_itemNameVisibilityTimers;

	// Token: 0x0400945B RID: 37979
	private List<dfLabel> m_inactiveDamageLabels = new List<dfLabel>();

	// Token: 0x0400945C RID: 37980
	private OverridableBool m_defaultLabelsHidden = new OverridableBool(false);

	// Token: 0x0400945D RID: 37981
	private float MotionGroupBufferWidth = 21f;

	// Token: 0x0400945E RID: 37982
	private List<dfSprite> additionalGunBoxes = new List<dfSprite>();

	// Token: 0x0400945F RID: 37983
	private List<dfSprite> additionalItemBoxes = new List<dfSprite>();

	// Token: 0x04009460 RID: 37984
	private List<dfSprite> additionalGunBoxesSecondary = new List<dfSprite>();

	// Token: 0x04009461 RID: 37985
	private List<dfSprite> additionalItemBoxesSecondary = new List<dfSprite>();

	// Token: 0x04009462 RID: 37986
	protected OverridableBool CoreUIHidden = new OverridableBool(false);

	// Token: 0x04009463 RID: 37987
	public bool GunventoryFolded = true;

	// Token: 0x04009464 RID: 37988
	private OverridableBool ForceLowerPanelsInvisibleOverride = new OverridableBool(false);

	// Token: 0x04009465 RID: 37989
	private bool m_metalGearGunSelectActive;

	// Token: 0x04009466 RID: 37990
	private Dictionary<Texture, Material> MetalGearAtlasToFadeMaterialMapR = new Dictionary<Texture, Material>();

	// Token: 0x04009467 RID: 37991
	private Dictionary<Material, Material> MetalGearFadeToOutlineMaterialMapR = new Dictionary<Material, Material>();

	// Token: 0x04009468 RID: 37992
	private Dictionary<Material, Material> MetalGearDFAtlasMapR = new Dictionary<Material, Material>();

	// Token: 0x04009469 RID: 37993
	private Dictionary<Texture, Material> MetalGearAtlasToFadeMaterialMapL = new Dictionary<Texture, Material>();

	// Token: 0x0400946A RID: 37994
	private Dictionary<Material, Material> MetalGearFadeToOutlineMaterialMapL = new Dictionary<Material, Material>();

	// Token: 0x0400946B RID: 37995
	private Dictionary<Material, Material> MetalGearDFAtlasMapL = new Dictionary<Material, Material>();

	// Token: 0x0400946C RID: 37996
	public Action OnScaleUpdate;

	// Token: 0x0400946D RID: 37997
	private dfGUIManager m_manager;

	// Token: 0x0400946E RID: 37998
	private dfSprite p_playerCoinSprite;

	// Token: 0x0400946F RID: 37999
	private Dictionary<AIActor, dfSlider> m_enemyToHealthbarMap = new Dictionary<AIActor, dfSlider>();

	// Token: 0x04009470 RID: 38000
	private List<dfSlider> m_unusedHealthbars = new List<dfSlider>();

	// Token: 0x04009471 RID: 38001
	private bool m_isDisplayingCustomReload;

	// Token: 0x04009472 RID: 38002
	public dfPanel PauseMenuPanel;

	// Token: 0x04009473 RID: 38003
	private PauseMenuController m_pmc;

	// Token: 0x04009474 RID: 38004
	public ConversationBarController ConversationBar;

	// Token: 0x04009475 RID: 38005
	protected bool m_displayingPlayerConversationOptions;

	// Token: 0x04009476 RID: 38006
	protected bool hasSelectedOption;

	// Token: 0x04009477 RID: 38007
	protected int selectedResponse = -1;

	// Token: 0x04009478 RID: 38008
	private bool m_hasSelectedAreYouSureOption;

	// Token: 0x04009479 RID: 38009
	private bool m_AreYouSureSelection;

	// Token: 0x0400947A RID: 38010
	private dfButton m_AreYouSureYesButton;

	// Token: 0x0400947B RID: 38011
	private dfButton m_AreYouSureNoButton;

	// Token: 0x0400947C RID: 38012
	private dfLabel m_AreYouSurePrimaryLabel;

	// Token: 0x0400947D RID: 38013
	private dfLabel m_AreYouSureSecondaryLabel;
}
