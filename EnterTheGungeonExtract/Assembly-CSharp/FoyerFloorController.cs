using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200116B RID: 4459
public class FoyerFloorController : MonoBehaviour
{
	// Token: 0x0600630C RID: 25356 RVA: 0x002662CC File Offset: 0x002644CC
	private IEnumerator Start()
	{
		while (Foyer.DoIntroSequence || Foyer.DoMainMenu)
		{
			yield return null;
		}
		bool alternateGunsUnlocked = GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_ALTERNATE_GUNS_UNLOCKED);
		alternateGunsUnlocked = alternateGunsUnlocked || GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLINGER_UNLOCKED);
		if (alternateGunsUnlocked)
		{
			base.GetComponent<tk2dBaseSprite>().SetSprite(this.FinalFormSpriteName);
			this.PitSprite.SetSprite(this.FinalPitName);
		}
		else if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.NUMBER_ATTEMPTS) >= 4f)
		{
			base.GetComponent<tk2dBaseSprite>().SetSprite(this.IntermediateFormSpriteName);
			this.PitSprite.SetSprite(this.IntermediatePitName);
		}
		else
		{
			base.GetComponent<tk2dBaseSprite>().SetSprite(this.BaseSpriteName);
			this.PitSprite.SetSprite(this.BasePitName);
		}
		base.GetComponent<SpeculativeRigidbody>().Reinitialize();
		this.PitSprite.GetComponent<SpeculativeRigidbody>().Reinitialize();
		yield break;
	}

	// Token: 0x04005E2B RID: 24107
	public string FinalFormSpriteName;

	// Token: 0x04005E2C RID: 24108
	public string IntermediateFormSpriteName;

	// Token: 0x04005E2D RID: 24109
	public string BaseSpriteName;

	// Token: 0x04005E2E RID: 24110
	public tk2dSprite PitSprite;

	// Token: 0x04005E2F RID: 24111
	public string FinalPitName;

	// Token: 0x04005E30 RID: 24112
	public string IntermediatePitName;

	// Token: 0x04005E31 RID: 24113
	public string BasePitName;
}
