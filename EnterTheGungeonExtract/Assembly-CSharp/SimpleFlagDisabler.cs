using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200120C RID: 4620
public class SimpleFlagDisabler : MonoBehaviour
{
	// Token: 0x06006761 RID: 26465 RVA: 0x002879B0 File Offset: 0x00285BB0
	private IEnumerator Start()
	{
		while (Foyer.DoIntroSequence || Foyer.DoMainMenu)
		{
			yield return null;
		}
		if (!string.IsNullOrEmpty(this.ChangeSpriteInstead))
		{
			this.UsesStatComparisonInstead = true;
		}
		if (this.DisableIfNotFoyer)
		{
			if (!GameManager.Instance.IsFoyer)
			{
				this.Disable();
			}
			yield break;
		}
		if (this.UsesStatComparisonInstead && base.transform.parent != null && base.transform.parent.name.Contains("Livery") && GameStatsManager.Instance.AnyPastBeaten() && this.RelevantStat == TrackedStats.NUMBER_ATTEMPTS)
		{
			yield break;
		}
		if (this.UsesStatComparisonInstead)
		{
			if (GameStatsManager.Instance.GetPlayerStatValue(this.RelevantStat) < (float)this.minStatValue)
			{
				this.Disable();
			}
		}
		else if (this.FlagToCheckFor != GungeonFlags.NONE && GameStatsManager.Instance.GetFlag(this.FlagToCheckFor) == this.DisableOnThisFlagValue)
		{
			this.Disable();
		}
		yield break;
	}

	// Token: 0x06006762 RID: 26466 RVA: 0x002879CC File Offset: 0x00285BCC
	private void Update()
	{
		if (this.EnableOnGunGameMode && !GameManager.Instance.IsSelectingCharacter && GameManager.Instance.PrimaryPlayer != null && (GameManager.Instance.PrimaryPlayer.CharacterUsesRandomGuns || ChallengeManager.CHALLENGE_MODE_ACTIVE))
		{
			SpeculativeRigidbody component = base.GetComponent<SpeculativeRigidbody>();
			if (!component.enabled)
			{
				component.enabled = true;
				component.Reinitialize();
				base.GetComponent<MeshRenderer>().enabled = true;
			}
		}
	}

	// Token: 0x06006763 RID: 26467 RVA: 0x00287A54 File Offset: 0x00285C54
	private void Disable()
	{
		SpeculativeRigidbody component = base.GetComponent<SpeculativeRigidbody>();
		if (!string.IsNullOrEmpty(this.ChangeSpriteInstead))
		{
			base.GetComponent<tk2dBaseSprite>().SetSprite(this.ChangeSpriteInstead);
			if (component)
			{
				component.Reinitialize();
			}
		}
		else
		{
			if (component)
			{
				component.enabled = false;
			}
			if (!this.EnableOnGunGameMode)
			{
				base.gameObject.SetActive(false);
			}
			else
			{
				base.GetComponent<MeshRenderer>().enabled = false;
			}
		}
	}

	// Token: 0x04006342 RID: 25410
	[LongEnum]
	public GungeonFlags FlagToCheckFor;

	// Token: 0x04006343 RID: 25411
	public bool DisableOnThisFlagValue;

	// Token: 0x04006344 RID: 25412
	public bool UsesStatComparisonInstead;

	// Token: 0x04006345 RID: 25413
	public TrackedStats RelevantStat = TrackedStats.NUMBER_ATTEMPTS;

	// Token: 0x04006346 RID: 25414
	public int minStatValue = 1;

	// Token: 0x04006347 RID: 25415
	public string ChangeSpriteInstead;

	// Token: 0x04006348 RID: 25416
	public bool EnableOnGunGameMode;

	// Token: 0x04006349 RID: 25417
	public bool DisableIfNotFoyer;
}
