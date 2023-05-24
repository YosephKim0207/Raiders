using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using InControl;
using UnityEngine;

// Token: 0x020017C6 RID: 6086
public class MinimapUIController : MonoBehaviour
{
	// Token: 0x17001562 RID: 5474
	// (get) Token: 0x06008EAE RID: 36526 RVA: 0x003C1574 File Offset: 0x003BF774
	private List<Tuple<tk2dSprite, PassiveItem>> SelectedDockItems
	{
		get
		{
			return (this.m_targetDockIndex != 1) ? this.dockItems : this.secondaryDockItems;
		}
	}

	// Token: 0x06008EAF RID: 36527 RVA: 0x003C1594 File Offset: 0x003BF794
	private Vector3 GetActivePosition(dfPanel panel, DungeonData.Direction direction)
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

	// Token: 0x06008EB0 RID: 36528 RVA: 0x003C1658 File Offset: 0x003BF858
	private Vector3 GetInactivePosition(dfPanel panel, DungeonData.Direction direction)
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

	// Token: 0x06008EB1 RID: 36529 RVA: 0x003C171C File Offset: 0x003BF91C
	private void InitializeMasterPanel(dfPanel panel, float startTime, float endTime)
	{
		panel.ResolutionChangedPostLayout = (Action<dfControl, Vector3, Vector3>)Delegate.Combine(panel.ResolutionChangedPostLayout, new Action<dfControl, Vector3, Vector3>(this.OnControlResolutionChanged));
		this.panels.Add(panel);
		this.panelTimings.Add(panel, new Tuple<float, float>(startTime, endTime));
	}

	// Token: 0x06008EB2 RID: 36530 RVA: 0x003C176C File Offset: 0x003BF96C
	private void Start()
	{
		this.m_manager = this.GrabbyPanel.GetManager();
		this.m_manager.UIScaleLegacyMode = false;
		this.DropItemButton.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			this.DropSelectedItem();
		};
		this.DropItemButtonForeign.Click += delegate(dfControl control, dfMouseEventArgs mouseEvent)
		{
			this.DropSelectedItem();
		};
		this.panels = new List<dfControl>();
		this.panelTimings = new Dictionary<dfControl, Tuple<float, float>>();
		this.InitializeMasterPanel(this.GrabbyPanel, 0f, 0.6f);
		this.InitializeMasterPanel(this.ItemPanel_PC, 0.2f, 0.8f);
		this.InitializeMasterPanel(this.ItemPanel_PC_Foreign, 0.2f, 0.8f);
		this.InitializeMasterPanel(this.SonyControlsPanel01, 0f, 0.6f);
		this.InitializeMasterPanel(this.SonyControlsPanel02, 0.2f, 0.8f);
		this.InitializeMasterPanel(this.SonyControlsPanel01Foreign, 0f, 0.6f);
		this.InitializeMasterPanel(this.SonyControlsPanel02Foreign, 0.2f, 0.8f);
		this.InitializeMasterPanel(this.DockPanel, 0f, 0.8f);
		this.InitializeMasterPanel(this.CoopDockPanelLeft, 0f, 0.8f);
		this.InitializeMasterPanel(this.CoopDockPanelRight, 0f, 0.8f);
		this.RecalculatePositions();
	}

	// Token: 0x06008EB3 RID: 36531 RVA: 0x003C18BC File Offset: 0x003BFABC
	private void PostprocessPassiveDockSprite(PassiveItem item, tk2dSprite itemSprite)
	{
		if (item is YellowChamberItem)
		{
			tk2dSpriteAnimator orAddComponent = itemSprite.gameObject.GetOrAddComponent<tk2dSpriteAnimator>();
			orAddComponent.Library = item.GetComponent<tk2dSpriteAnimator>().Library;
		}
	}

	// Token: 0x06008EB4 RID: 36532 RVA: 0x003C18F4 File Offset: 0x003BFAF4
	public void AddPassiveItemToDock(PassiveItem item, PlayerController itemOwner)
	{
		if (item && item.encounterTrackable && item.encounterTrackable.SuppressInInventory)
		{
			return;
		}
		for (int i = 0; i < this.dockItems.Count; i++)
		{
			if (this.dockItems[i].Second == item)
			{
				return;
			}
		}
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			for (int j = 0; j < this.secondaryDockItems.Count; j++)
			{
				if (this.secondaryDockItems[j].Second == item)
				{
					return;
				}
			}
			if (itemOwner.IsPrimaryPlayer)
			{
				tk2dSprite tk2dSprite = this.AddTK2DSpriteToPanel(item.GetComponent<tk2dBaseSprite>(), this.CoopDockPanelLeft.transform);
				this.PostprocessPassiveDockSprite(item, tk2dSprite);
				Tuple<tk2dSprite, PassiveItem> tuple = new Tuple<tk2dSprite, PassiveItem>(tk2dSprite, item);
				this.dockItems.Add(tuple);
			}
			else
			{
				tk2dSprite tk2dSprite2 = this.AddTK2DSpriteToPanel(item.GetComponent<tk2dBaseSprite>(), this.CoopDockPanelRight.transform);
				this.PostprocessPassiveDockSprite(item, tk2dSprite2);
				Tuple<tk2dSprite, PassiveItem> tuple2 = new Tuple<tk2dSprite, PassiveItem>(tk2dSprite2, item);
				this.secondaryDockItems.Add(tuple2);
			}
		}
		else
		{
			tk2dSprite tk2dSprite3 = this.AddTK2DSpriteToPanel(item.GetComponent<tk2dBaseSprite>(), this.DockPanel.transform);
			this.PostprocessPassiveDockSprite(item, tk2dSprite3);
			Tuple<tk2dSprite, PassiveItem> tuple3 = new Tuple<tk2dSprite, PassiveItem>(tk2dSprite3, item);
			this.dockItems.Add(tuple3);
		}
	}

	// Token: 0x06008EB5 RID: 36533 RVA: 0x003C1A6C File Offset: 0x003BFC6C
	public void InfoSelectedItem()
	{
		if (this.m_selectedDockItemIndex == -1)
		{
			return;
		}
		if (this.m_selectedDockItemIndex < this.SelectedDockItems.Count)
		{
			if (this.SelectedDockItems[this.m_selectedDockItemIndex].Second.encounterTrackable)
			{
				EncounterTrackable encounterTrackable = this.SelectedDockItems[this.m_selectedDockItemIndex].Second.encounterTrackable;
				if (!encounterTrackable.journalData.SuppressInAmmonomicon)
				{
					Minimap.Instance.ToggleMinimap(false, false);
					GameManager.Instance.Pause();
					GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().DoShowBestiaryToTarget(encounterTrackable);
				}
			}
		}
	}

	// Token: 0x06008EB6 RID: 36534 RVA: 0x003C1B20 File Offset: 0x003BFD20
	public void DropSelectedItem()
	{
		this.ClearSynergyHighlights();
		if (this.m_selectedDockItemIndex == -1)
		{
			return;
		}
		PlayerController playerController = GameManager.Instance.PrimaryPlayer;
		if (this.m_targetDockIndex == 1)
		{
			playerController = GameManager.Instance.SecondaryPlayer;
		}
		if (this.m_selectedDockItemIndex >= this.SelectedDockItems.Count)
		{
			int num = this.m_selectedDockItemIndex - this.SelectedDockItems.Count;
			if (playerController.inventory.AllGuns[num].CanActuallyBeDropped(playerController))
			{
				playerController.ForceDropGun(playerController.inventory.AllGuns[num]);
				this.m_selectedDockItemIndex = -1;
			}
		}
		else
		{
			PassiveItem second = this.SelectedDockItems[this.m_selectedDockItemIndex].Second;
			if (second.CanActuallyBeDropped(second.Owner))
			{
				playerController.DropPassiveItem(second);
				this.m_selectedDockItemIndex = -1;
			}
		}
	}

	// Token: 0x06008EB7 RID: 36535 RVA: 0x003C1C04 File Offset: 0x003BFE04
	public void RemovePassiveItemFromDock(PassiveItem item)
	{
		for (int i = 0; i < this.dockItems.Count; i++)
		{
			Tuple<tk2dSprite, PassiveItem> tuple = this.dockItems[i];
			if (tuple.Second == item)
			{
				UnityEngine.Object.Destroy(tuple.First.gameObject);
				this.dockItems.RemoveAt(i);
				break;
			}
		}
		for (int j = 0; j < this.secondaryDockItems.Count; j++)
		{
			Tuple<tk2dSprite, PassiveItem> tuple2 = this.secondaryDockItems[j];
			if (tuple2.Second == item)
			{
				UnityEngine.Object.Destroy(tuple2.First.gameObject);
				this.secondaryDockItems.RemoveAt(j);
				break;
			}
		}
	}

	// Token: 0x06008EB8 RID: 36536 RVA: 0x003C1CC8 File Offset: 0x003BFEC8
	protected tk2dSprite AddTK2DSpriteToPanel(tk2dBaseSprite sourceSprite, Transform parent)
	{
		GameObject gameObject = new GameObject("tk2d item sprite");
		gameObject.transform.parent = parent;
		gameObject.layer = LayerMask.NameToLayer("SecondaryGUI");
		tk2dSprite tk2dSprite = tk2dBaseSprite.AddComponent<tk2dSprite>(gameObject, sourceSprite.Collection, sourceSprite.spriteId);
		Bounds untrimmedBounds = tk2dSprite.GetUntrimmedBounds();
		Vector2 vector = GameUIUtility.TK2DtoDF(untrimmedBounds.size.XY());
		tk2dSprite.scale = new Vector3(vector.x / untrimmedBounds.size.x, vector.y / untrimmedBounds.size.y, untrimmedBounds.size.z);
		tk2dSprite.ignoresTiltworldDepth = true;
		gameObject.transform.localPosition = Vector3.zero;
		return tk2dSprite;
	}

	// Token: 0x06008EB9 RID: 36537 RVA: 0x003C1D8C File Offset: 0x003BFF8C
	protected void OnControlResolutionChanged(dfControl source, Vector3 oldRelativePosition, Vector3 newRelativePosition)
	{
	}

	// Token: 0x06008EBA RID: 36538 RVA: 0x003C1D90 File Offset: 0x003BFF90
	protected void RecalculatePositions()
	{
		if (this.activePositions == null)
		{
			this.activePositions = new Dictionary<dfControl, Vector3>();
		}
		if (this.inactivePositions == null)
		{
			this.inactivePositions = new Dictionary<dfControl, Vector3>();
		}
		this.activePositions.Clear();
		this.inactivePositions.Clear();
		this.activePositions.Add(this.GrabbyPanel, this.GetActivePosition(this.GrabbyPanel, DungeonData.Direction.WEST));
		this.inactivePositions.Add(this.GrabbyPanel, this.GetInactivePosition(this.GrabbyPanel, DungeonData.Direction.WEST));
		this.activePositions.Add(this.ItemPanel_PC, this.GetActivePosition(this.ItemPanel_PC, DungeonData.Direction.WEST));
		this.inactivePositions.Add(this.ItemPanel_PC, this.GetInactivePosition(this.ItemPanel_PC, DungeonData.Direction.WEST));
		this.activePositions.Add(this.ItemPanel_PC_Foreign, this.GetActivePosition(this.ItemPanel_PC_Foreign, DungeonData.Direction.WEST));
		this.inactivePositions.Add(this.ItemPanel_PC_Foreign, this.GetInactivePosition(this.ItemPanel_PC_Foreign, DungeonData.Direction.WEST));
		this.activePositions.Add(this.SonyControlsPanel01, this.GetActivePosition(this.SonyControlsPanel01, DungeonData.Direction.WEST));
		this.inactivePositions.Add(this.SonyControlsPanel01, this.GetInactivePosition(this.SonyControlsPanel01, DungeonData.Direction.WEST));
		this.activePositions.Add(this.SonyControlsPanel02, this.GetActivePosition(this.SonyControlsPanel02, DungeonData.Direction.WEST));
		this.inactivePositions.Add(this.SonyControlsPanel02, this.GetInactivePosition(this.SonyControlsPanel02, DungeonData.Direction.WEST));
		this.activePositions.Add(this.SonyControlsPanel01Foreign, this.GetActivePosition(this.SonyControlsPanel01Foreign, DungeonData.Direction.WEST));
		this.inactivePositions.Add(this.SonyControlsPanel01Foreign, this.GetInactivePosition(this.SonyControlsPanel01Foreign, DungeonData.Direction.WEST));
		this.activePositions.Add(this.SonyControlsPanel02Foreign, this.GetActivePosition(this.SonyControlsPanel02Foreign, DungeonData.Direction.WEST));
		this.inactivePositions.Add(this.SonyControlsPanel02Foreign, this.GetInactivePosition(this.SonyControlsPanel02Foreign, DungeonData.Direction.WEST));
		this.activePositions.Add(this.DockPanel, this.GetActivePosition(this.DockPanel, DungeonData.Direction.SOUTH));
		this.inactivePositions.Add(this.DockPanel, this.GetInactivePosition(this.DockPanel, DungeonData.Direction.SOUTH));
		this.activePositions.Add(this.CoopDockPanelLeft, this.GetActivePosition(this.CoopDockPanelLeft, DungeonData.Direction.SOUTH));
		this.inactivePositions.Add(this.CoopDockPanelLeft, this.GetInactivePosition(this.CoopDockPanelLeft, DungeonData.Direction.SOUTH));
		this.activePositions.Add(this.CoopDockPanelRight, this.GetActivePosition(this.CoopDockPanelRight, DungeonData.Direction.SOUTH));
		this.inactivePositions.Add(this.CoopDockPanelRight, this.GetInactivePosition(this.CoopDockPanelRight, DungeonData.Direction.SOUTH));
	}

	// Token: 0x06008EBB RID: 36539 RVA: 0x003C2038 File Offset: 0x003C0238
	private void PostStateChanged(bool newState)
	{
		this.TurboModeIndicator.IsVisible = GameManager.IsTurboMode;
		this.DispenserLabel.Text = HeartDispenser.CurrentHalfHeartsStored.ToString();
		this.DispenserLabel.IsVisible = HeartDispenser.CurrentHalfHeartsStored > 0;
		this.DispenserIcon.IsVisible = HeartDispenser.CurrentHalfHeartsStored > 0;
		for (int i = 0; i < this.dockItems.Count; i++)
		{
			if (this.dockItems[i].Second is YellowChamberItem)
			{
				if (newState)
				{
					if (UnityEngine.Random.value < 0.1f)
					{
						if (UnityEngine.Random.value < 0.25f)
						{
							base.StartCoroutine(this.HandleDelayedAnimation(this.dockItems[i].First.spriteAnimator, "yellow_chamber_eye", UnityEngine.Random.Range(2.5f, 10f)));
						}
						else
						{
							base.StartCoroutine(this.HandleDelayedAnimation(this.dockItems[i].First.spriteAnimator, "yellow_chamber_blink", UnityEngine.Random.Range(2.5f, 10f)));
						}
					}
					else
					{
						this.dockItems[i].First.spriteAnimator.StopAndResetFrameToDefault();
					}
				}
				else
				{
					this.dockItems[i].First.spriteAnimator.Stop();
				}
			}
		}
		for (int j = 0; j < this.secondaryDockItems.Count; j++)
		{
			if (this.secondaryDockItems[j].Second is YellowChamberItem)
			{
				if (newState)
				{
					if (UnityEngine.Random.value < 0.1f)
					{
						if (UnityEngine.Random.value < 0.25f)
						{
							base.StartCoroutine(this.HandleDelayedAnimation(this.secondaryDockItems[j].First.spriteAnimator, "yellow_chamber_eye", UnityEngine.Random.Range(2.5f, 10f)));
						}
						else
						{
							base.StartCoroutine(this.HandleDelayedAnimation(this.secondaryDockItems[j].First.spriteAnimator, "yellow_chamber_blink", UnityEngine.Random.Range(2.5f, 10f)));
						}
					}
				}
				else
				{
					this.secondaryDockItems[j].First.spriteAnimator.Stop();
				}
			}
		}
	}

	// Token: 0x06008EBC RID: 36540 RVA: 0x003C2298 File Offset: 0x003C0498
	private IEnumerator HandleDelayedAnimation(tk2dSpriteAnimator targetAnimator, string animationName, float delayTime)
	{
		float elapsed = 0f;
		while (elapsed < delayTime)
		{
			if (!this.m_active)
			{
				yield break;
			}
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		if (this.m_active)
		{
			targetAnimator.Play(animationName);
		}
		yield break;
	}

	// Token: 0x06008EBD RID: 36541 RVA: 0x003C22C8 File Offset: 0x003C04C8
	public void ToggleState(bool active)
	{
		if (active == this.m_active)
		{
			return;
		}
		if (active)
		{
			this.Activate();
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				if (playerController)
				{
					playerController.CurrentInputState = PlayerInputState.OnlyMovement;
				}
			}
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				dfSprite componentInChildren = this.CoopDockPanelLeft.GetComponentInChildren<dfSprite>();
				dfSprite componentInChildren2 = this.CoopDockPanelRight.GetComponentInChildren<dfSprite>();
				if (this.m_cachedCoopDockPanelLeftRelativePosition == null)
				{
					this.m_cachedCoopDockPanelLeftRelativePosition = new Vector3?(componentInChildren.RelativePosition);
				}
				if (this.m_cachedCoopDockPanelRightRelativePosition == null)
				{
					this.m_cachedCoopDockPanelRightRelativePosition = new Vector3?(componentInChildren2.RelativePosition);
				}
				this.ArrangeDockItems(this.dockItems, componentInChildren, 1);
				this.ArrangeDockItems(this.secondaryDockItems, componentInChildren2, 2);
			}
			else
			{
				this.ArrangeDockItems(this.dockItems, this.DockSprite, 0);
			}
		}
		else
		{
			this.Deactivate();
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				if (GameManager.Instance.AllPlayers[j])
				{
					GameManager.Instance.AllPlayers[j].CurrentInputState = PlayerInputState.AllInput;
				}
			}
		}
		this.PostStateChanged(active);
	}

	// Token: 0x06008EBE RID: 36542 RVA: 0x003C2428 File Offset: 0x003C0628
	protected void ArrangeDockItems(List<Tuple<tk2dSprite, PassiveItem>> targetDockItems, dfSprite targetDockSprite, int targetIndex = 0)
	{
		float num = this.DockPanel.PixelsToUnits() * Pixelator.Instance.CurrentTileScale;
		int count = targetDockItems.Count;
		float num2 = (float)((GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER) ? 270 : 132);
		float num3 = num2 / (float)count;
		float num4 = Pixelator.Instance.CurrentTileScale;
		for (int i = 0; i < count; i++)
		{
			num4 += targetDockItems[i].First.GetBounds().size.x / num;
			num4 += Pixelator.Instance.CurrentTileScale;
		}
		float num5 = 20f * Pixelator.Instance.CurrentTileScale / 3f;
		targetDockSprite.Width = Mathf.Min(num4, num2).Quantize(3f);
		targetDockSprite.Height = num5;
		if (targetIndex == 1 && this.m_cachedCoopDockPanelLeftRelativePosition != null)
		{
			targetDockSprite.RelativePosition = targetDockSprite.RelativePosition.WithX(targetDockSprite.Parent.Width - 132f - (132f - targetDockSprite.Width) * 2f);
		}
		else if (targetIndex == 2 && this.m_cachedCoopDockPanelRightRelativePosition != null)
		{
			targetDockSprite.RelativePosition = targetDockSprite.RelativePosition.WithX((132f - targetDockSprite.Width) * 3f);
		}
		targetDockSprite.PerformLayout();
		Vector3 vector = targetDockSprite.GetCorners()[2];
		float num6 = 0f;
		if (targetIndex != 2)
		{
			num6 = 20f * Pixelator.Instance.CurrentTileScale / 6f * num;
		}
		for (int j = 0; j < count; j++)
		{
			tk2dSprite first = targetDockItems[j].First;
			first.PlaceAtPositionByAnchor(vector, tk2dBaseSprite.Anchor.LowerCenter);
			float num7 = Pixelator.Instance.CurrentTileScale * 2f * num;
			float num8;
			if (num4 < num2)
			{
				if (j != 0 || targetIndex == 2)
				{
					num6 += targetDockItems[j].First.GetBounds().size.x / 2f + num;
				}
				num8 = num6;
				num6 += targetDockItems[j].First.GetBounds().size.x / 2f + num;
			}
			else
			{
				num8 = num6 + num3 * num * (float)j;
			}
			first.transform.localPosition += new Vector3(num8, num7, 0f);
		}
	}

	// Token: 0x06008EBF RID: 36543 RVA: 0x003C26DC File Offset: 0x003C08DC
	protected void OldArrangeDockItems(List<Tuple<tk2dSprite, PassiveItem>> targetDockItems, dfSprite targetDockSprite)
	{
		float num = this.DockPanel.PixelsToUnits();
		int num2 = ((GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER) ? 12 : 5);
		int num3 = Mathf.CeilToInt((float)targetDockItems.Count / (1f * (float)num2));
		float num4 = (float)num3 * 20f * Pixelator.Instance.CurrentTileScale / 3f;
		float num5;
		if (num2 >= targetDockItems.Count)
		{
			num5 = (float)(targetDockItems.Count + 1) * (20f * Pixelator.Instance.CurrentTileScale) / 3f;
		}
		else
		{
			num5 = (float)(num2 + 1) * (20f * Pixelator.Instance.CurrentTileScale) / 3f;
		}
		targetDockSprite.Width = num5;
		targetDockSprite.Height = num4;
		targetDockSprite.PerformLayout();
		Vector3 vector = targetDockSprite.GetCorners()[2];
		int num6 = targetDockItems.Count;
		for (int i = 0; i < num3; i++)
		{
			int num7 = Mathf.Min(num2, num6);
			for (int j = 0; j < num7; j++)
			{
				if (num6 <= 0)
				{
					break;
				}
				int num8 = targetDockItems.Count - num6;
				Tuple<tk2dSprite, PassiveItem> tuple = targetDockItems[num8];
				tk2dSprite first = tuple.First;
				first.PlaceAtPositionByAnchor(vector, tk2dBaseSprite.Anchor.LowerCenter);
				float num9 = 20f * Pixelator.Instance.CurrentTileScale * num * (float)(j + 1);
				float num10 = 20f * Pixelator.Instance.CurrentTileScale * num * (float)i;
				num10 += Pixelator.Instance.CurrentTileScale * 5f * num;
				first.transform.localPosition += new Vector3(num9, num10, 0f);
				num6--;
			}
			if (num6 <= 0)
			{
				break;
			}
		}
	}

	// Token: 0x06008EC0 RID: 36544 RVA: 0x003C28B4 File Offset: 0x003C0AB4
	private void HandlePanelVisibility()
	{
		bool flag = false;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(GameManager.Instance.AllPlayers[i].PlayerIDX);
			if (instanceForPlayer.ActiveActions.MapAction.IsPressed)
			{
				flag = instanceForPlayer.IsKeyboardAndMouse(false);
			}
		}
		bool flag2 = this.dockItems.Count > 0;
		bool flag3 = this.secondaryDockItems != null && this.secondaryDockItems.Count > 0;
		bool flag4 = flag2 && this.m_selectedDockItemIndex != -1;
		if (flag)
		{
			this.GrabbyPanel.IsVisible = true;
			if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
			{
				this.ItemPanel_PC.IsVisible = flag4;
				this.ItemPanel_PC_Foreign.IsVisible = false;
				this.ItemPanel_PC.Parent.IsInteractive = true;
				this.ItemPanel_PC_Foreign.Parent.IsInteractive = false;
			}
			else
			{
				this.ItemPanel_PC_Foreign.IsVisible = flag4;
				this.ItemPanel_PC.IsVisible = false;
				this.ItemPanel_PC.Parent.IsInteractive = false;
				this.ItemPanel_PC_Foreign.Parent.IsInteractive = true;
			}
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				this.CoopDockPanelLeft.IsVisible = flag2;
				this.CoopDockPanelRight.IsVisible = flag3;
				this.DockPanel.IsVisible = false;
			}
			else
			{
				this.CoopDockPanelLeft.IsVisible = false;
				this.CoopDockPanelRight.IsVisible = false;
				this.DockPanel.IsVisible = flag2;
			}
			this.SonyControlsPanel01.IsVisible = false;
			this.SonyControlsPanel02.IsVisible = false;
			this.SonyControlsPanel01Foreign.IsVisible = false;
			this.SonyControlsPanel02Foreign.IsVisible = false;
		}
		else
		{
			this.GrabbyPanel.IsVisible = false;
			this.ItemPanel_PC.IsVisible = false;
			this.ItemPanel_PC_Foreign.IsVisible = false;
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				this.CoopDockPanelLeft.IsVisible = flag2;
				this.CoopDockPanelRight.IsVisible = flag3;
				this.DockPanel.IsVisible = false;
			}
			else
			{
				this.CoopDockPanelLeft.IsVisible = false;
				this.CoopDockPanelRight.IsVisible = false;
				this.DockPanel.IsVisible = flag2;
			}
			if (GameManager.Instance.PrimaryPlayer.inventory != null && GameManager.Instance.PrimaryPlayer.inventory.AllGuns.Count >= 5)
			{
				this.SonyControlsPanel01.IsVisible = false;
				this.SonyControlsPanel02.IsVisible = false;
				this.SonyControlsPanel01Foreign.IsVisible = false;
				this.SonyControlsPanel02Foreign.IsVisible = false;
			}
			else if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
			{
				this.SonyControlsPanel01.IsVisible = true;
				this.SonyControlsPanel02.IsVisible = flag2;
			}
			else
			{
				this.SonyControlsPanel01Foreign.IsVisible = true;
				this.SonyControlsPanel02Foreign.IsVisible = flag2;
			}
		}
	}

	// Token: 0x06008EC1 RID: 36545 RVA: 0x003C2BBC File Offset: 0x003C0DBC
	private Vector2 ModifyMousePosition(Vector2 inputPosition)
	{
		return inputPosition;
	}

	// Token: 0x06008EC2 RID: 36546 RVA: 0x003C2BCC File Offset: 0x003C0DCC
	private void Update()
	{
		this.m_manager.UIScale = Pixelator.Instance.ScaleTileScale / 3f;
		Vector2 screenSize = this.CoopDockPanelLeft.GUIManager.GetScreenSize();
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			float num = screenSize.x / 2f;
			this.CoopDockPanelLeft.Parent.Width = num;
			this.CoopDockPanelRight.Parent.Width = num;
			this.CoopDockPanelLeft.Parent.RelativePosition = this.CoopDockPanelLeft.Parent.RelativePosition.WithX(0f);
			this.CoopDockPanelRight.Parent.RelativePosition = this.CoopDockPanelRight.Parent.RelativePosition.WithX(num);
			this.CoopDockPanelLeft.Parent.RelativePosition = this.CoopDockPanelLeft.Parent.RelativePosition.WithY(screenSize.y - this.CoopDockPanelLeft.Size.y);
			this.CoopDockPanelRight.Parent.RelativePosition = this.CoopDockPanelRight.Parent.RelativePosition.WithY(screenSize.y - this.CoopDockPanelRight.Size.y);
		}
		else
		{
			this.DockPanel.Parent.RelativePosition = this.DockPanel.Parent.RelativePosition.WithY(screenSize.y - this.DockPanel.Size.y);
		}
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			GungeonActions activeActions = BraveInput.GetInstanceForPlayer(GameManager.Instance.AllPlayers[i].PlayerIDX).ActiveActions;
			if (activeActions != null)
			{
				if (activeActions.MinimapZoomOutAction.WasPressed)
				{
					Minimap.Instance.AttemptZoomMinimap(0.2f);
				}
				if (activeActions.MinimapZoomInAction.WasPressed)
				{
					Minimap.Instance.AttemptZoomMinimap(-0.2f);
				}
			}
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.RESOURCEFUL_RAT)
		{
			this.RatTaunty.IsVisible = true;
		}
		if (this.m_active)
		{
			if (Minimap.Instance.HeldOpen)
			{
				GungeonActions gungeonActions = BraveInput.PrimaryPlayerInstance.ActiveActions;
				if (gungeonActions.MapAction.WasPressed || gungeonActions.PauseAction.WasPressed)
				{
					gungeonActions.MapAction.Suppress();
					gungeonActions.PauseAction.Suppress();
					Minimap.Instance.ToggleMinimap(false, false);
					return;
				}
				if (BraveInput.SecondaryPlayerInstance != null)
				{
					gungeonActions = BraveInput.SecondaryPlayerInstance.ActiveActions;
					if (gungeonActions.MapAction.WasPressed || gungeonActions.PauseAction.WasPressed)
					{
						gungeonActions.MapAction.Suppress();
						gungeonActions.PauseAction.Suppress();
						Minimap.Instance.ToggleMinimap(false, false);
						return;
					}
				}
			}
			else
			{
				if (BraveInput.PrimaryPlayerInstance.ActiveActions.MapAction.WasReleased && (BraveInput.SecondaryPlayerInstance == null || !BraveInput.SecondaryPlayerInstance.ActiveActions.MapAction.IsPressed))
				{
					Minimap.Instance.ToggleMinimap(false, false);
					return;
				}
				if (BraveInput.SecondaryPlayerInstance != null && BraveInput.SecondaryPlayerInstance.ActiveActions.MapAction.WasReleased && !BraveInput.PrimaryPlayerInstance.ActiveActions.MapAction.IsPressed)
				{
					Minimap.Instance.ToggleMinimap(false, false);
					return;
				}
				if (!BraveInput.PrimaryPlayerInstance.ActiveActions.MapAction.IsPressed && (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER || !BraveInput.GetInstanceForPlayer(GameManager.Instance.SecondaryPlayer.PlayerIDX).ActiveActions.MapAction.IsPressed))
				{
					Minimap.Instance.ToggleMinimap(false, false);
					return;
				}
			}
			this.UpdateDockItemSpriteScales();
			this.HandlePanelVisibility();
			bool flag = false;
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				List<Tuple<tk2dSprite, PassiveItem>> list = ((j != 0) ? this.secondaryDockItems : this.dockItems);
				BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(GameManager.Instance.AllPlayers[j].PlayerIDX);
				if (instanceForPlayer.ActiveActions.MapAction.IsPressed || Minimap.Instance.HeldOpen)
				{
					if (instanceForPlayer.IsKeyboardAndMouse(false))
					{
						Vector2 vector = Input.mousePosition;
						Vector2 vector2 = this.ModifyMousePosition(vector);
						this.ControllerCrosshair.IsVisible = false;
						this.SelectNearbyTeleportIcon(vector2);
						if (Input.GetMouseButtonDown(0))
						{
							float num2 = this.DockPanel.PixelsToUnits();
							Vector2 vector3 = this.minimapCamera.ScreenToWorldPoint(vector).XY();
							bool flag2 = false;
							for (int k = 0; k < list.Count; k++)
							{
								Vector2 worldCenter = list[k].First.WorldCenter;
								float num3 = Vector2.Distance(worldCenter, vector3);
								if (num3 < 20f * Pixelator.Instance.CurrentTileScale / 2f * num2)
								{
									flag2 = true;
									this.SelectDockItem(k, j);
									break;
								}
							}
							if (!flag2)
							{
								this.m_isPanning = true;
								this.m_lastMousePosition = vector2;
								this.m_panPixelDistTravelled = 0f;
							}
						}
						else if (Input.GetMouseButton(0) && this.m_isPanning)
						{
							Minimap.Instance.AttemptPanCamera((this.minimapCamera.ScreenToWorldPoint(vector2) - this.minimapCamera.ScreenToWorldPoint(this.m_lastMousePosition)) * -1f);
							this.m_panPixelDistTravelled += Vector2.Distance(this.m_lastMousePosition, vector2);
							this.m_lastMousePosition = vector2;
						}
						else if (Input.GetMouseButtonUp(0))
						{
							Vector2 vector4 = BraveUtility.GetMinimapViewportPosition(vector2);
							bool flag3 = vector4.x > 0.17f && vector4.y < 0.88f && vector4.x < 0.825f && vector4.y > 0.12f;
							bool flag4 = this.m_panPixelDistTravelled / (float)Screen.width < 0.005f;
							if (flag3 && flag4)
							{
								this.AttemptTeleport();
							}
							this.m_isPanning = false;
						}
						if (Input.GetAxis("Mouse ScrollWheel") != 0f)
						{
							Minimap.Instance.AttemptZoomCamera(Input.GetAxis("Mouse ScrollWheel") * -1f);
						}
					}
					else
					{
						Vector2 vector5 = Vector2.zero;
						bool flag5 = false;
						bool flag6 = false;
						bool flag7 = false;
						bool flag8 = false;
						bool flag9 = false;
						bool flag10 = false;
						bool flag11 = false;
						if (instanceForPlayer.ActiveActions != null)
						{
							vector5 = instanceForPlayer.ActiveActions.Aim.Vector;
							flag6 = instanceForPlayer.ActiveActions.InteractAction;
							InputDevice device = instanceForPlayer.ActiveActions.Device;
							if (device != null)
							{
								flag5 = device.RightStickButton.WasPressed;
								flag7 = device.Action4.WasPressed;
								flag8 = device.DPadLeft.WasPressed;
								flag9 = device.DPadRight.WasPressed;
								flag10 = device.DPadUp.WasPressed;
								flag11 = device.DPadDown.WasPressed;
							}
						}
						this.ControllerCrosshair.IsVisible = true;
						if (vector5.magnitude > 0f && !flag)
						{
							flag = true;
							Minimap.Instance.AttemptPanCamera(vector5.ToVector3ZUp(0f) * GameManager.INVARIANT_DELTA_TIME * 0.8f);
						}
						this.SelectNearbyTeleportIcon(new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f));
						if (flag5)
						{
							Minimap.Instance.TogglePresetZoomValue();
						}
						if (flag6)
						{
							this.AttemptTeleport();
						}
						if (flag7)
						{
							this.DropSelectedItem();
						}
						int count = list.Count;
						if (flag9 && list.Count > 0)
						{
							int num4 = Mathf.Max(0, (this.m_selectedDockItemIndex + 1) % count);
							if (j != this.m_targetDockIndex)
							{
								num4 = 0;
							}
							this.SelectDockItem(num4, j);
						}
						if (flag8 && list.Count > 0)
						{
							int num5 = Mathf.Max(0, (this.m_selectedDockItemIndex + count - 1) % count);
							if (j != this.m_targetDockIndex)
							{
								num5 = 0;
							}
							this.SelectDockItem(num5, j);
						}
						if (flag10 || flag11)
						{
							Minimap instance = Minimap.Instance;
							Vector2 vector6 = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
							if (instanceForPlayer.IsKeyboardAndMouse(false))
							{
								vector6 = this.ModifyMousePosition(Input.mousePosition);
							}
							float num6;
							RoomHandler nearestVisibleRoom = instance.GetNearestVisibleRoom(vector6, out num6);
							if (nearestVisibleRoom != null && !instance.IsPanning)
							{
								this.m_currentTeleportTargetIndex = instance.roomsContainingTeleporters.IndexOf(nearestVisibleRoom);
							}
							RoomHandler roomHandler;
							if (num6 < 0.5f || instance.IsPanning)
							{
								int num7 = ((!flag10) ? (-1) : 1);
								roomHandler = instance.NextSelectedTeleporter(ref this.m_currentTeleportTargetIndex, num7);
							}
							else
							{
								roomHandler = nearestVisibleRoom;
							}
							if (roomHandler != null && roomHandler.TeleportersActive)
							{
								instance.PanToPosition(instance.RoomToTeleportMap[roomHandler].GetComponent<tk2dBaseSprite>().WorldCenter);
							}
						}
					}
				}
			}
		}
		else
		{
			this.m_isPanning = false;
		}
	}

	// Token: 0x06008EC3 RID: 36547 RVA: 0x003C35A0 File Offset: 0x003C17A0
	private void SelectNearbyTeleportIcon(Vector2 positionToCheck)
	{
		GameObject gameObject = null;
		RoomHandler roomHandler = Minimap.Instance.CheckIconsNearCursor(positionToCheck, out gameObject);
		if (roomHandler != null && !roomHandler.TeleportersActive)
		{
			if (this.m_currentTeleportIconSprite != null)
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(this.m_currentTeleportIconSprite, false);
				this.m_currentTeleportIconSprite = null;
				this.m_currentTeleportTarget = null;
			}
			return;
		}
		tk2dBaseSprite tk2dBaseSprite = ((!(gameObject != null)) ? null : gameObject.GetComponent<tk2dBaseSprite>());
		if (tk2dBaseSprite == null)
		{
			if (this.m_currentTeleportIconSprite != null)
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(this.m_currentTeleportIconSprite, false);
				this.m_currentTeleportIconSprite = null;
				this.m_currentTeleportTarget = null;
			}
		}
		else
		{
			tk2dBaseSprite.ignoresTiltworldDepth = true;
			if (this.m_currentTeleportIconSprite != null && this.m_currentTeleportIconSprite != tk2dBaseSprite)
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(this.m_currentTeleportIconSprite, false);
				this.m_currentTeleportIconSprite = null;
				this.m_currentTeleportTarget = null;
				SpriteOutlineManager.AddOutlineToSprite(tk2dBaseSprite, Color.white);
				this.m_currentTeleportTarget = roomHandler;
				this.m_currentTeleportIconSprite = tk2dBaseSprite;
			}
			else if (!(this.m_currentTeleportIconSprite == tk2dBaseSprite))
			{
				SpriteOutlineManager.AddOutlineToSprite(tk2dBaseSprite, Color.white);
				this.m_currentTeleportTarget = roomHandler;
				this.m_currentTeleportIconSprite = tk2dBaseSprite;
			}
		}
		if (this.m_currentTeleportTarget != null)
		{
			this.ControllerCrosshair.SpriteName = "minimap_select_square_001";
			this.ControllerCrosshair.Size = this.ControllerCrosshair.SpriteInfo.sizeInPixels * 3f;
			this.ControllerCrosshair.GetComponentInChildren<dfPanel>().IsVisible = true;
		}
		else
		{
			this.ControllerCrosshair.SpriteName = "minimap_select_crosshair_001";
			this.ControllerCrosshair.Size = this.ControllerCrosshair.SpriteInfo.sizeInPixels * 3f;
			this.ControllerCrosshair.GetComponentInChildren<dfPanel>().IsVisible = false;
		}
	}

	// Token: 0x06008EC4 RID: 36548 RVA: 0x003C3780 File Offset: 0x003C1980
	private bool AttemptTeleport()
	{
		if (Minimap.Instance && Minimap.Instance.PreventAllTeleports)
		{
			return false;
		}
		if (GameUIRoot.Instance.DisplayingConversationBar)
		{
			return false;
		}
		foreach (PlayerController playerController in GameManager.Instance.AllPlayers)
		{
			if (playerController.CurrentRoom != null && playerController.CurrentRoom.CompletelyPreventLeaving)
			{
				return false;
			}
		}
		if (this.m_currentTeleportTarget != null)
		{
			RoomHandler currentTeleportTarget = this.m_currentTeleportTarget;
			PlayerController[] allPlayers;
			for (int j = 0; j < allPlayers.Length; j++)
			{
				allPlayers[j].AttemptTeleportToRoom(currentTeleportTarget, false, false);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06008EC5 RID: 36549 RVA: 0x003C3838 File Offset: 0x003C1A38
	private void DeselectDockItem()
	{
		this.ClearSynergyHighlights();
		if (this.m_selectedDockItemIndex != -1)
		{
			if (this.m_selectedDockItemIndex < this.SelectedDockItems.Count)
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(this.SelectedDockItems[this.m_selectedDockItemIndex].First, false);
				this.m_selectedDockItemIndex = -1;
			}
			else
			{
				this.m_selectedDockItemIndex = -1;
			}
		}
	}

	// Token: 0x06008EC6 RID: 36550 RVA: 0x003C389C File Offset: 0x003C1A9C
	private void UpdateDockItemSpriteScales()
	{
		for (int i = 0; i < this.dockItems.Count; i++)
		{
			tk2dSprite first = this.dockItems[i].First;
			first.scale = Vector3.one * GameUIUtility.GetCurrentTK2D_DFScale(this.m_manager);
			if (SpriteOutlineManager.HasOutline(first))
			{
				tk2dSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites(first);
				for (int j = 0; j < outlineSprites.Length; j++)
				{
					outlineSprites[j].scale = first.scale;
				}
			}
		}
		for (int k = 0; k < this.secondaryDockItems.Count; k++)
		{
			tk2dSprite first2 = this.secondaryDockItems[k].First;
			first2.scale = Vector3.one * GameUIUtility.GetCurrentTK2D_DFScale(this.m_manager);
			if (SpriteOutlineManager.HasOutline(first2))
			{
				tk2dSprite[] outlineSprites2 = SpriteOutlineManager.GetOutlineSprites(first2);
				for (int l = 0; l < outlineSprites2.Length; l++)
				{
					outlineSprites2[l].scale = first2.scale;
				}
			}
		}
	}

	// Token: 0x06008EC7 RID: 36551 RVA: 0x003C39B8 File Offset: 0x003C1BB8
	private void SelectDockItem(int i, int targetPlayerID)
	{
		if (this.m_selectedDockItemIndex == i && this.m_targetDockIndex == targetPlayerID)
		{
			return;
		}
		this.DeselectDockItem();
		List<Tuple<tk2dSprite, PassiveItem>> list = this.dockItems;
		if (targetPlayerID == 1)
		{
			list = this.secondaryDockItems;
		}
		if (i < list.Count)
		{
			SpriteOutlineManager.AddOutlineToSprite(list[i].First, Color.white);
			tk2dSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites(list[i].First);
			for (int j = 0; j < outlineSprites.Length; j++)
			{
				outlineSprites[j].scale = list[i].First.scale;
			}
		}
		this.m_targetDockIndex = targetPlayerID;
		this.m_selectedDockItemIndex = i;
		PassiveItem second = list[i].Second;
		this.DropItemButton.IsEnabled = second.CanActuallyBeDropped(second.Owner);
		this.DropItemSprite.Color = ((!this.DropItemButton.IsEnabled) ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white);
		this.DropItemLabel.Color = ((!this.DropItemButton.IsEnabled) ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white);
		this.DropItemSpriteForeign.Color = ((!this.DropItemButton.IsEnabled) ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white);
		this.DropItemLabelForeign.Color = ((!this.DropItemButton.IsEnabled) ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white);
		if (second)
		{
			this.UpdateSynergyHighlights(second.PickupObjectId);
		}
	}

	// Token: 0x06008EC8 RID: 36552 RVA: 0x003C3BB4 File Offset: 0x003C1DB4
	private void ClearSynergyHighlights()
	{
		for (int i = 0; i < this.extantSynergyArrows.Count; i++)
		{
			UnityEngine.Object.Destroy(this.extantSynergyArrows[i].gameObject);
		}
		this.extantSynergyArrows.Clear();
		for (int j = 0; j < this.dockItems.Count; j++)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(this.dockItems[j].First, false);
		}
		for (int k = 0; k < this.secondaryDockItems.Count; k++)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(this.secondaryDockItems[k].First, false);
		}
	}

	// Token: 0x06008EC9 RID: 36553 RVA: 0x003C3C64 File Offset: 0x003C1E64
	private void CreateArrow(tk2dBaseSprite targetSprite, dfControl targetParent)
	{
		dfSprite dfSprite = targetParent.AddControl<dfSprite>();
		dfSprite.Atlas = Minimap.Instance.UIMinimap.DispenserIcon.Atlas;
		dfSprite.SpriteName = "synergy_ammonomicon_arrow_001";
		dfSprite.Size = dfSprite.SpriteInfo.sizeInPixels * 4f;
		Bounds bounds = targetSprite.GetBounds();
		Bounds untrimmedBounds = targetSprite.GetUntrimmedBounds();
		Vector3 size = bounds.size;
		dfSprite.transform.position = (targetSprite.WorldCenter.ToVector3ZisY(0f) + new Vector3(-8f * targetParent.PixelsToUnits(), size.y / 2f + 32f * targetParent.PixelsToUnits(), 0f)).WithZ(0f);
		dfSprite.BringToFront();
		dfSprite.Invalidate();
		this.extantSynergyArrows.Add(dfSprite);
	}

	// Token: 0x06008ECA RID: 36554 RVA: 0x003C3D40 File Offset: 0x003C1F40
	private void UpdateSynergyHighlights(int selectedID)
	{
		AdvancedSynergyDatabase synergyManager = GameManager.Instance.SynergyManager;
		dfControl rootContainer = this.DockSprite.GetRootContainer();
		rootContainer.BringToFront();
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			for (int j = 0; j < synergyManager.synergies.Length; j++)
			{
				if (playerController.ActiveExtraSynergies.Contains(j))
				{
					AdvancedSynergyEntry advancedSynergyEntry = synergyManager.synergies[j];
					if (advancedSynergyEntry.ContainsPickup(selectedID))
					{
						for (int k = 0; k < this.dockItems.Count; k++)
						{
							int pickupObjectId = this.dockItems[k].Second.PickupObjectId;
							if (pickupObjectId != selectedID && advancedSynergyEntry.ContainsPickup(pickupObjectId))
							{
								SpriteOutlineManager.AddOutlineToSprite(this.dockItems[k].First, SynergyDatabase.SynergyBlue);
								this.CreateArrow(this.dockItems[k].First, rootContainer);
							}
						}
						for (int l = 0; l < this.secondaryDockItems.Count; l++)
						{
							int pickupObjectId2 = this.secondaryDockItems[l].Second.PickupObjectId;
							if (pickupObjectId2 != selectedID && advancedSynergyEntry.ContainsPickup(pickupObjectId2))
							{
								SpriteOutlineManager.AddOutlineToSprite(this.secondaryDockItems[l].First, SynergyDatabase.SynergyBlue);
								this.CreateArrow(this.secondaryDockItems[l].First, rootContainer);
							}
						}
						for (int m = 0; m < playerController.inventory.AllGuns.Count; m++)
						{
							int pickupObjectId3 = playerController.inventory.AllGuns[m].PickupObjectId;
							if (pickupObjectId3 != selectedID && advancedSynergyEntry.ContainsPickup(pickupObjectId3))
							{
								int num = playerController.inventory.AllGuns.IndexOf(playerController.CurrentGun);
								int num2 = playerController.inventory.AllGuns.Count - (num - m + playerController.inventory.AllGuns.Count - 1) % playerController.inventory.AllGuns.Count - 1;
								tk2dClippedSprite spriteForUnfoldedGun = GameUIRoot.Instance.GetSpriteForUnfoldedGun(playerController.PlayerIDX, num2);
								if (spriteForUnfoldedGun)
								{
									SpriteOutlineManager.RemoveOutlineFromSprite(spriteForUnfoldedGun, false);
									SpriteOutlineManager.AddOutlineToSprite<tk2dClippedSprite>(spriteForUnfoldedGun, SynergyDatabase.SynergyBlue);
									this.CreateArrow(spriteForUnfoldedGun, spriteForUnfoldedGun.transform.parent.parent.GetComponent<dfControl>());
								}
							}
						}
						for (int n = 0; n < playerController.activeItems.Count; n++)
						{
							int pickupObjectId4 = playerController.activeItems[n].PickupObjectId;
							if (pickupObjectId4 != selectedID && advancedSynergyEntry.ContainsPickup(pickupObjectId4))
							{
								int num3 = playerController.activeItems.IndexOf(playerController.CurrentItem);
								int num4 = playerController.activeItems.Count - (num3 - n + playerController.activeItems.Count - 1) % playerController.activeItems.Count - 1;
								tk2dClippedSprite spriteForUnfoldedItem = GameUIRoot.Instance.GetSpriteForUnfoldedItem(playerController.PlayerIDX, num4);
								if (spriteForUnfoldedItem)
								{
									SpriteOutlineManager.RemoveOutlineFromSprite(spriteForUnfoldedItem, false);
									SpriteOutlineManager.AddOutlineToSprite<tk2dClippedSprite>(spriteForUnfoldedItem, SynergyDatabase.SynergyBlue);
									this.CreateArrow(spriteForUnfoldedItem, spriteForUnfoldedItem.transform.parent.parent.GetComponent<dfControl>());
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06008ECB RID: 36555 RVA: 0x003C40C4 File Offset: 0x003C22C4
	private IEnumerator MoveAllThings()
	{
		float elapsed = 0f;
		float transitionTime = 0.25f;
		bool cachedActive = this.m_active;
		bool hasRepositioned = false;
		while (elapsed < transitionTime)
		{
			if (cachedActive != this.m_active)
			{
				yield break;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = Mathf.Clamp01(elapsed / transitionTime);
			if (!this.m_active)
			{
				t = 1f - t;
			}
			for (int i = 0; i < this.panels.Count; i++)
			{
				float first = this.panelTimings[this.panels[i]].First;
				float second = this.panelTimings[this.panels[i]].Second;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((t - first) / (second - first)));
				this.panels[i].RelativePosition = Vector3.Lerp(this.inactivePositions[this.panels[i]], this.activePositions[this.panels[i]], num);
			}
			yield return null;
			if (!hasRepositioned)
			{
				hasRepositioned = true;
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					dfSprite componentInChildren = this.CoopDockPanelLeft.GetComponentInChildren<dfSprite>();
					dfSprite componentInChildren2 = this.CoopDockPanelRight.GetComponentInChildren<dfSprite>();
					this.ArrangeDockItems(this.dockItems, componentInChildren, 1);
					this.ArrangeDockItems(this.secondaryDockItems, componentInChildren2, 2);
				}
				else
				{
					this.ArrangeDockItems(this.dockItems, this.DockSprite, 0);
				}
			}
		}
		if (!this.m_active)
		{
			this.minimapCamera.enabled = false;
			for (int j = 0; j < this.AdditionalControlsToToggle.Count; j++)
			{
				this.AdditionalControlsToToggle[j].IsVisible = false;
			}
			this.SonyControlsPanel01.IsVisible = false;
			this.SonyControlsPanel02.IsVisible = false;
			this.SonyControlsPanel01Foreign.IsVisible = false;
			this.SonyControlsPanel02Foreign.IsVisible = false;
		}
		yield break;
	}

	// Token: 0x06008ECC RID: 36556 RVA: 0x003C40E0 File Offset: 0x003C22E0
	private void UpdateLevelNameLabel()
	{
		Dungeon dungeon = GameManager.Instance.Dungeon;
		string text = this.LevelNameLabel.ForceGetLocalizedValue(dungeon.DungeonFloorName);
		GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
		int num = -1;
		if (lastLoadedLevelDefinition != null)
		{
			num = GameManager.Instance.dungeonFloors.IndexOf(lastLoadedLevelDefinition);
		}
		string text2 = this.LevelNameLabel.ForceGetLocalizedValue("#LEVEL") + ((num >= 0) ? num.ToString() : "?") + ": " + text;
		this.LevelNameLabel.Text = text2;
		this.LevelNameLabel.Invalidate();
		this.LevelNameLabel.PerformLayout();
	}

	// Token: 0x06008ECD RID: 36557 RVA: 0x003C418C File Offset: 0x003C238C
	private void UpdateQuestText()
	{
	}

	// Token: 0x06008ECE RID: 36558 RVA: 0x003C4190 File Offset: 0x003C2390
	private void Activate()
	{
		this.UpdateLevelNameLabel();
		this.UpdateQuestText();
		this.DeselectDockItem();
		this.m_active = true;
		this.minimapCamera.enabled = true;
		for (int i = 0; i < this.AdditionalControlsToToggle.Count; i++)
		{
			this.AdditionalControlsToToggle[i].IsVisible = true;
			dfSpriteAnimation componentInChildren = this.AdditionalControlsToToggle[i].GetComponentInChildren<dfSpriteAnimation>();
			if (componentInChildren)
			{
				componentInChildren.Play();
			}
		}
		this.RecalculatePositions();
		base.StartCoroutine(this.MoveAllThings());
	}

	// Token: 0x06008ECF RID: 36559 RVA: 0x003C4228 File Offset: 0x003C2428
	private void Deactivate()
	{
		this.DeselectDockItem();
		this.m_active = false;
		this.RecalculatePositions();
		this.ControllerCrosshair.IsVisible = false;
		base.StartCoroutine(this.MoveAllThings());
		if (this.m_currentTeleportIconSprite != null)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(this.m_currentTeleportIconSprite, false);
			this.m_currentTeleportIconSprite = null;
			this.m_currentTeleportTarget = null;
		}
		for (int i = 0; i < this.AdditionalControlsToToggle.Count; i++)
		{
			this.AdditionalControlsToToggle[i].IsVisible = false;
		}
	}

	// Token: 0x040096A8 RID: 38568
	public dfSprite DockSprite;

	// Token: 0x040096A9 RID: 38569
	private dfGUIManager m_manager;

	// Token: 0x040096AA RID: 38570
	public dfPanel QuestPanel;

	// Token: 0x040096AB RID: 38571
	public dfPanel GrabbyPanel;

	// Token: 0x040096AC RID: 38572
	public dfPanel ItemPanel_PC;

	// Token: 0x040096AD RID: 38573
	public dfPanel ItemPanel_PC_Foreign;

	// Token: 0x040096AE RID: 38574
	public dfPanel SonyControlsPanel01;

	// Token: 0x040096AF RID: 38575
	public dfPanel SonyControlsPanel02;

	// Token: 0x040096B0 RID: 38576
	public dfPanel SonyControlsPanel01Foreign;

	// Token: 0x040096B1 RID: 38577
	public dfPanel SonyControlsPanel02Foreign;

	// Token: 0x040096B2 RID: 38578
	public dfPanel DockPanel;

	// Token: 0x040096B3 RID: 38579
	public dfPanel CoopDockPanelLeft;

	// Token: 0x040096B4 RID: 38580
	public dfPanel CoopDockPanelRight;

	// Token: 0x040096B5 RID: 38581
	public dfSprite ControllerCrosshair;

	// Token: 0x040096B6 RID: 38582
	public dfLabel LevelNameLabel;

	// Token: 0x040096B7 RID: 38583
	public dfButton DropItemButton;

	// Token: 0x040096B8 RID: 38584
	public dfSprite DropItemSprite;

	// Token: 0x040096B9 RID: 38585
	public dfLabel DropItemLabel;

	// Token: 0x040096BA RID: 38586
	public dfButton DropItemButtonForeign;

	// Token: 0x040096BB RID: 38587
	public dfSprite DropItemSpriteForeign;

	// Token: 0x040096BC RID: 38588
	public dfLabel DropItemLabelForeign;

	// Token: 0x040096BD RID: 38589
	public List<dfControl> AdditionalControlsToToggle;

	// Token: 0x040096BE RID: 38590
	public Camera minimapCamera;

	// Token: 0x040096BF RID: 38591
	public dfSprite TurboModeIndicator;

	// Token: 0x040096C0 RID: 38592
	public dfSprite DispenserIcon;

	// Token: 0x040096C1 RID: 38593
	public dfLabel DispenserLabel;

	// Token: 0x040096C2 RID: 38594
	public dfSprite RatTaunty;

	// Token: 0x040096C3 RID: 38595
	private int m_targetDockIndex;

	// Token: 0x040096C4 RID: 38596
	private int m_selectedDockItemIndex = -1;

	// Token: 0x040096C5 RID: 38597
	private List<Tuple<tk2dSprite, PassiveItem>> dockItems = new List<Tuple<tk2dSprite, PassiveItem>>();

	// Token: 0x040096C6 RID: 38598
	private List<Tuple<tk2dSprite, PassiveItem>> secondaryDockItems = new List<Tuple<tk2dSprite, PassiveItem>>();

	// Token: 0x040096C7 RID: 38599
	private List<dfControl> panels;

	// Token: 0x040096C8 RID: 38600
	private Dictionary<dfControl, Vector3> activePositions;

	// Token: 0x040096C9 RID: 38601
	private Dictionary<dfControl, Vector3> inactivePositions;

	// Token: 0x040096CA RID: 38602
	private Dictionary<dfControl, Tuple<float, float>> panelTimings;

	// Token: 0x040096CB RID: 38603
	private bool m_active;

	// Token: 0x040096CC RID: 38604
	private bool m_isPanning;

	// Token: 0x040096CD RID: 38605
	private Vector3 m_lastMousePosition;

	// Token: 0x040096CE RID: 38606
	private float m_panPixelDistTravelled;

	// Token: 0x040096CF RID: 38607
	private tk2dBaseSprite m_currentTeleportIconSprite;

	// Token: 0x040096D0 RID: 38608
	private RoomHandler m_currentTeleportTarget;

	// Token: 0x040096D1 RID: 38609
	private int m_currentTeleportTargetIndex;

	// Token: 0x040096D2 RID: 38610
	private const float ITEM_SPACING_MPX = 20f;

	// Token: 0x040096D3 RID: 38611
	private const int NUM_ITEMS_PER_LINE = 12;

	// Token: 0x040096D4 RID: 38612
	private const int NUM_ITEMS_PER_LINE_COOP = 5;

	// Token: 0x040096D5 RID: 38613
	private Vector3? m_cachedCoopDockPanelLeftRelativePosition;

	// Token: 0x040096D6 RID: 38614
	private Vector3? m_cachedCoopDockPanelRightRelativePosition;

	// Token: 0x040096D7 RID: 38615
	private List<dfSprite> extantSynergyArrows = new List<dfSprite>();
}
