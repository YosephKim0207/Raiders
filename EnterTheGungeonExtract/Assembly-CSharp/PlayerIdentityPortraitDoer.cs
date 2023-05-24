using System;
using System.Collections;
using UnityEngine;

// Token: 0x020017EB RID: 6123
public class PlayerIdentityPortraitDoer : MonoBehaviour
{
	// Token: 0x0600902F RID: 36911 RVA: 0x003CF1E8 File Offset: 0x003CD3E8
	public static string GetPortraitSpriteName(PlayableCharacters character)
	{
		switch (character)
		{
		case PlayableCharacters.Pilot:
			return "Player_Rogue_001";
		case PlayableCharacters.Convict:
			return "Player_Convict_001";
		case PlayableCharacters.Robot:
			return "Player_Robot_001";
		case PlayableCharacters.Ninja:
			return "Player_Ninja_001";
		case PlayableCharacters.Cosmonaut:
			return "Player_Cosmonaut_001";
		case PlayableCharacters.Soldier:
			return "Player_Marine_001";
		case PlayableCharacters.Guide:
			return "Player_Guide_001";
		case PlayableCharacters.CoopCultist:
			return "Player_Coop_Pink_001";
		case PlayableCharacters.Bullet:
			return "Player_Bullet_001";
		case PlayableCharacters.Eevee:
			return "Player_Eevee_minimap_001";
		case PlayableCharacters.Gunslinger:
			return "Player_Slinger_001";
		default:
			return "Player_Rogue_001";
		}
	}

	// Token: 0x06009030 RID: 36912 RVA: 0x003CF274 File Offset: 0x003CD474
	private IEnumerator Start()
	{
		while (this.IsPlayerTwo && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer == null)
		{
			yield return null;
		}
		while ((!this.IsPlayerTwo || GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER) && GameManager.Instance.PrimaryPlayer == null)
		{
			yield return null;
		}
		dfSprite sprite = base.GetComponent<dfSprite>();
		if (this.IsPlayerTwo && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			sprite.SpriteName = PlayerIdentityPortraitDoer.GetPortraitSpriteName(GameManager.Instance.SecondaryPlayer.characterIdentity);
		}
		else
		{
			sprite.SpriteName = PlayerIdentityPortraitDoer.GetPortraitSpriteName(GameManager.Instance.PrimaryPlayer.characterIdentity);
		}
		yield break;
	}

	// Token: 0x04009852 RID: 38994
	public bool IsPlayerTwo;
}
