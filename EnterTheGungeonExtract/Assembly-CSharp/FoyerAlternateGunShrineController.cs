using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001164 RID: 4452
public class FoyerAlternateGunShrineController : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x17000E94 RID: 3732
	// (get) Token: 0x060062DE RID: 25310 RVA: 0x002659F0 File Offset: 0x00263BF0
	private bool IsCurrentlyActive
	{
		get
		{
			return GameManager.HasInstance && GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.UsingAlternateStartingGuns;
		}
	}

	// Token: 0x060062DF RID: 25311 RVA: 0x00265A24 File Offset: 0x00263C24
	private IEnumerator Start()
	{
		while (Foyer.DoIntroSequence || Foyer.DoMainMenu)
		{
			yield return null;
		}
		base.GetComponent<tk2dBaseSprite>().HeightOffGround = -1f;
		base.GetComponent<tk2dBaseSprite>().UpdateZDepth();
		if (!GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_ALTERNATE_GUNS_UNLOCKED))
		{
			base.transform.position.GetAbsoluteRoom().DeregisterInteractable(this);
			base.specRigidbody.enabled = false;
			base.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x060062E0 RID: 25312 RVA: 0x00265A40 File Offset: 0x00263C40
	public void DoEffect(PlayerController interactor)
	{
		if (interactor.characterIdentity == PlayableCharacters.Eevee || interactor.characterIdentity == PlayableCharacters.Gunslinger)
		{
			return;
		}
		interactor.UsingAlternateStartingGuns = !interactor.UsingAlternateStartingGuns;
		interactor.ReinitializeGuns();
		interactor.PlayEffectOnActor((GameObject)ResourceCache.Acquire("Global VFX/VFX_AltGunShrine"), Vector3.zero, true, false, false);
	}

	// Token: 0x060062E1 RID: 25313 RVA: 0x00265A9C File Offset: 0x00263C9C
	public void Interact(PlayerController interactor)
	{
		if (TextBoxManager.HasTextBox(this.talkPoint))
		{
			return;
		}
		if (interactor.characterIdentity == PlayableCharacters.Eevee || interactor.characterIdentity == PlayableCharacters.Gunslinger)
		{
			return;
		}
		base.StartCoroutine(this.HandleShrineConversation(interactor));
	}

	// Token: 0x060062E2 RID: 25314 RVA: 0x00265AD8 File Offset: 0x00263CD8
	private IEnumerator HandleShrineConversation(PlayerController interactor)
	{
		string targetDisplayKey = this.displayTextKey;
		TextBoxManager.ShowStoneTablet(this.talkPoint.position, this.talkPoint, -1f, StringTableManager.GetLongString(targetDisplayKey), true, false);
		int selectedResponse = -1;
		interactor.SetInputOverride("shrineConversation");
		bool canUse = GameStatsManager.Instance.GetCharacterSpecificFlag(interactor.characterIdentity, CharacterSpecificGungeonFlags.KILLED_PAST_ALTERNATE_COSTUME);
		yield return null;
		if (canUse)
		{
			string @string = StringTableManager.GetString(this.acceptOptionKey);
			GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, @string, StringTableManager.GetString(this.declineOptionKey));
		}
		else
		{
			GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, StringTableManager.GetString(this.declineOptionKey), string.Empty);
		}
		while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
		{
			yield return null;
		}
		interactor.ClearInputOverride("shrineConversation");
		TextBoxManager.ClearTextBox(this.talkPoint);
		if (canUse && selectedResponse == 0)
		{
			this.DoEffect(interactor);
		}
		yield break;
	}

	// Token: 0x060062E3 RID: 25315 RVA: 0x00265AFC File Offset: 0x00263CFC
	private void Update()
	{
		if (Foyer.DoIntroSequence || Foyer.DoMainMenu || !GameManager.HasInstance || Dungeon.IsGenerating || GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		if (base.gameObject.activeSelf)
		{
			this.Flame.sprite.HeightOffGround = -0.5f;
			this.Flame.renderer.enabled = this.IsCurrentlyActive;
		}
	}

	// Token: 0x060062E4 RID: 25316 RVA: 0x00265B7C File Offset: 0x00263D7C
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060062E5 RID: 25317 RVA: 0x00265B88 File Offset: 0x00263D88
	public float GetDistanceToPoint(Vector2 point)
	{
		if (base.sprite == null)
		{
			return 100f;
		}
		Vector3 vector = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
		return Vector2.Distance(point, vector) / 1.5f;
	}

	// Token: 0x060062E6 RID: 25318 RVA: 0x00265BE0 File Offset: 0x00263DE0
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x060062E7 RID: 25319 RVA: 0x00265BE8 File Offset: 0x00263DE8
	public void OnEnteredRange(PlayerController interactor)
	{
		if (interactor.characterIdentity == PlayableCharacters.Eevee || interactor.characterIdentity == PlayableCharacters.Gunslinger)
		{
			return;
		}
		if (this.AlternativeOutlineTarget != null)
		{
			SpriteOutlineManager.AddOutlineToSprite(this.AlternativeOutlineTarget, Color.white);
		}
		else
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
		}
	}

	// Token: 0x060062E8 RID: 25320 RVA: 0x00265C48 File Offset: 0x00263E48
	public void OnExitRange(PlayerController interactor)
	{
		if (interactor.characterIdentity == PlayableCharacters.Eevee || interactor.characterIdentity == PlayableCharacters.Gunslinger)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite((!(this.AlternativeOutlineTarget != null)) ? base.sprite : this.AlternativeOutlineTarget, false);
	}

	// Token: 0x04005E0C RID: 24076
	public Transform talkPoint;

	// Token: 0x04005E0D RID: 24077
	public string displayTextKey;

	// Token: 0x04005E0E RID: 24078
	public string acceptOptionKey;

	// Token: 0x04005E0F RID: 24079
	public string declineOptionKey;

	// Token: 0x04005E10 RID: 24080
	public tk2dSpriteAnimator Flame;

	// Token: 0x04005E11 RID: 24081
	public tk2dBaseSprite AlternativeOutlineTarget;
}
