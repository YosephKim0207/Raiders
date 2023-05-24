using System;
using HutongGames.PlayMaker.Actions;
using InControl;
using UnityEngine;

// Token: 0x02001773 RID: 6003
public class FoyerCharacterSelectFlag : BraveBehaviour
{
	// Token: 0x06008BC8 RID: 35784 RVA: 0x003A3644 File Offset: 0x003A1844
	public bool PrerequisitesFulfilled()
	{
		bool flag = true;
		for (int i = 0; i < this.prerequisites.Length; i++)
		{
			if (!this.prerequisites[i].CheckConditionsFulfilled())
			{
				flag = false;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06008BC9 RID: 35785 RVA: 0x003A3688 File Offset: 0x003A1888
	public bool CanBeSelected()
	{
		return (!this.IsEevee || GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY) >= 5f) && (!this.IsGunslinger || GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY) >= 7f);
	}

	// Token: 0x06008BCA RID: 35786 RVA: 0x003A36DC File Offset: 0x003A18DC
	private void EnsureStartingEquipmentEncountered()
	{
		if (!this.PrerequisitesFulfilled())
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.CharacterPrefabPath))
		{
			GameObject gameObject = (GameObject)BraveResources.Load(this.CharacterPrefabPath, ".prefab");
			if (gameObject)
			{
				PlayerController component = gameObject.GetComponent<PlayerController>();
				if (component)
				{
					if (component.startingGunIds != null)
					{
						for (int i = 0; i < component.startingGunIds.Count; i++)
						{
							Gun gun = PickupObjectDatabase.GetById(component.startingGunIds[i]) as Gun;
							if (gun && gun.encounterTrackable)
							{
								GameStatsManager.Instance.HandleEncounteredObjectRaw(gun.encounterTrackable.EncounterGuid);
							}
						}
					}
					if (component.startingActiveItemIds != null)
					{
						for (int j = 0; j < component.startingActiveItemIds.Count; j++)
						{
							PlayerItem playerItem = PickupObjectDatabase.GetById(component.startingActiveItemIds[j]) as PlayerItem;
							if (playerItem && playerItem.encounterTrackable)
							{
								GameStatsManager.Instance.HandleEncounteredObjectRaw(playerItem.encounterTrackable.EncounterGuid);
							}
						}
					}
					if (component.startingPassiveItemIds != null)
					{
						for (int k = 0; k < component.startingPassiveItemIds.Count; k++)
						{
							PlayerItem playerItem2 = PickupObjectDatabase.GetById(component.startingPassiveItemIds[k]) as PlayerItem;
							if (playerItem2 && playerItem2.encounterTrackable)
							{
								GameStatsManager.Instance.HandleEncounteredObjectRaw(playerItem2.encounterTrackable.EncounterGuid);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06008BCB RID: 35787 RVA: 0x003A3894 File Offset: 0x003A1A94
	public void Start()
	{
		this.EnsureStartingEquipmentEncountered();
	}

	// Token: 0x06008BCC RID: 35788 RVA: 0x003A389C File Offset: 0x003A1A9C
	private void ToggleSelf(bool activate)
	{
		this.m_active = activate;
		base.specRigidbody.enabled = activate;
		base.renderer.enabled = activate;
		base.talkDoer.IsInteractable = activate;
		base.talkDoer.ShowOutlines = activate;
		SetNpcVisibility.SetVisible(base.talkDoer, activate);
		SpriteOutlineManager.ToggleOutlineRenderers(base.sprite, activate);
	}

	// Token: 0x06008BCD RID: 35789 RVA: 0x003A38F8 File Offset: 0x003A1AF8
	private void Update()
	{
		if (this.IsCoopCharacter)
		{
			if (this.m_active && InputManager.Devices.Count == 0)
			{
				this.ToggleSelf(false);
			}
			else if (!this.m_active && InputManager.Devices.Count > 0)
			{
				this.ToggleSelf(true);
			}
		}
	}

	// Token: 0x06008BCE RID: 35790 RVA: 0x003A3958 File Offset: 0x003A1B58
	public void ToggleOverheadElementVisibility(bool value)
	{
		if (this.m_extantOverheadUIElement && this.m_extantOverheadUIElement.IsVisible != value)
		{
			this.m_extantOverheadUIElement.IsVisible = value;
			FoyerInfoPanelController component = this.m_extantOverheadUIElement.GetComponent<FoyerInfoPanelController>();
			if (component.arrow && component.arrow.transform.childCount > 0)
			{
				MeshRenderer component2 = component.arrow.transform.GetChild(0).GetComponent<MeshRenderer>();
				if (component2)
				{
					component2.enabled = value;
				}
			}
		}
	}

	// Token: 0x170014E6 RID: 5350
	// (get) Token: 0x06008BCF RID: 35791 RVA: 0x003A39F0 File Offset: 0x003A1BF0
	public bool IsAlternateCostume
	{
		get
		{
			return this.m_isAlternateCostume;
		}
	}

	// Token: 0x06008BD0 RID: 35792 RVA: 0x003A39F8 File Offset: 0x003A1BF8
	public void ChangeToAlternateCostume()
	{
		if (this.AltCostumeLibrary != null && !this.m_isAlternateCostume)
		{
			CharacterSelectIdleDoer component = base.GetComponent<CharacterSelectIdleDoer>();
			if (component)
			{
				component.enabled = false;
			}
			this.m_isAlternateCostume = true;
			tk2dSpriteAnimation library = base.spriteAnimator.Library;
			base.spriteAnimator.Library = this.AltCostumeLibrary;
			base.spriteAnimator.Play(this.AltCostumeClipName);
			this.AltCostumeLibrary = library;
		}
		else if (this.AltCostumeLibrary != null)
		{
			this.m_isAlternateCostume = false;
			tk2dSpriteAnimation library2 = base.spriteAnimator.Library;
			base.spriteAnimator.Library = this.AltCostumeLibrary;
			base.spriteAnimator.Play("select_idle");
			this.AltCostumeLibrary = library2;
		}
	}

	// Token: 0x06008BD1 RID: 35793 RVA: 0x003A3AC8 File Offset: 0x003A1CC8
	public FoyerInfoPanelController CreateOverheadElement()
	{
		this.m_extantOverheadUIElement = GameUIRoot.Instance.Manager.AddPrefab(this.OverheadElement);
		FoyerInfoPanelController component = this.m_extantOverheadUIElement.GetComponent<FoyerInfoPanelController>();
		if (component)
		{
			component.followTransform = base.transform;
			component.offset = new Vector3(0.75f, 1.625f, 0f);
		}
		return component;
	}

	// Token: 0x06008BD2 RID: 35794 RVA: 0x003A3B30 File Offset: 0x003A1D30
	private void OnDisable()
	{
		this.ClearOverheadElement();
	}

	// Token: 0x06008BD3 RID: 35795 RVA: 0x003A3B38 File Offset: 0x003A1D38
	public void OnCoopChangedCallback()
	{
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			base.gameObject.SetActive(false);
			base.GetComponent<SpeculativeRigidbody>().enabled = false;
		}
		else
		{
			base.gameObject.SetActive(true);
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
			base.specRigidbody.enabled = true;
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
			CharacterSelectIdleDoer component = base.GetComponent<CharacterSelectIdleDoer>();
			component.enabled = true;
		}
	}

	// Token: 0x06008BD4 RID: 35796 RVA: 0x003A3BC0 File Offset: 0x003A1DC0
	public void OnSelectedCharacterCallback(PlayerController newCharacter)
	{
		Debug.Log(string.Concat(new object[] { newCharacter.name, "|", newCharacter.characterIdentity, " <====" }));
		if (newCharacter.gameObject.name.Contains(this.CharacterPrefabPath))
		{
			base.gameObject.SetActive(false);
			base.GetComponent<SpeculativeRigidbody>().enabled = false;
			if (this.IsEevee)
			{
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.META_CURRENCY, -5f);
			}
			if (this.IsGunslinger)
			{
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.META_CURRENCY, -7f);
			}
		}
		else if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
			base.specRigidbody.enabled = true;
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
			if (!this.m_isAlternateCostume)
			{
				CharacterSelectIdleDoer component = base.GetComponent<CharacterSelectIdleDoer>();
				component.enabled = true;
			}
		}
	}

	// Token: 0x06008BD5 RID: 35797 RVA: 0x003A3CDC File Offset: 0x003A1EDC
	public void ClearOverheadElement()
	{
		if (this.m_extantOverheadUIElement != null)
		{
			UnityEngine.Object.Destroy(this.m_extantOverheadUIElement.gameObject);
			this.m_extantOverheadUIElement = null;
		}
	}

	// Token: 0x040092CE RID: 37582
	public GameObject OverheadElement;

	// Token: 0x040092CF RID: 37583
	public string CharacterPrefabPath;

	// Token: 0x040092D0 RID: 37584
	public bool IsCoopCharacter;

	// Token: 0x040092D1 RID: 37585
	public bool IsEevee;

	// Token: 0x040092D2 RID: 37586
	public bool IsGunslinger;

	// Token: 0x040092D3 RID: 37587
	public DungeonPrerequisite[] prerequisites;

	// Token: 0x040092D4 RID: 37588
	public tk2dSpriteAnimation AltCostumeLibrary;

	// Token: 0x040092D5 RID: 37589
	public string AltCostumeClipName;

	// Token: 0x040092D6 RID: 37590
	private dfControl m_extantOverheadUIElement;

	// Token: 0x040092D7 RID: 37591
	private bool m_active = true;

	// Token: 0x040092D8 RID: 37592
	private bool m_isAlternateCostume;
}
