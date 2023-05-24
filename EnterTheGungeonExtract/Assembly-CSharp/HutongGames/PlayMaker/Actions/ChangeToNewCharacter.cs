using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C8F RID: 3215
	[Tooltip("Only use this in the Foyer!")]
	[ActionCategory(".NPCs")]
	public class ChangeToNewCharacter : FsmStateAction
	{
		// Token: 0x060044DA RID: 17626 RVA: 0x00163DEC File Offset: 0x00161FEC
		public override void Reset()
		{
		}

		// Token: 0x060044DB RID: 17627 RVA: 0x00163DF0 File Offset: 0x00161FF0
		public override void OnEnter()
		{
			GameManager.Instance.StartCoroutine(this.HandleCharacterChange());
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x00163E04 File Offset: 0x00162004
		private IEnumerator HandleCharacterChange()
		{
			Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
			bool wasInGunGame = false;
			if (GameManager.Instance.PrimaryPlayer)
			{
				wasInGunGame = GameManager.Instance.PrimaryPlayer.CharacterUsesRandomGuns;
			}
			GameManager.Instance.PrimaryPlayer.SetInputOverride("getting deleted");
			yield return new WaitForSeconds(0.5f);
			PlayerController newPlayer = this.GeneratePlayer();
			yield return null;
			GameManager.Instance.MainCameraController.ClearPlayerCache();
			GameManager.Instance.MainCameraController.SetManualControl(false, true);
			Foyer.Instance.ProcessPlayerEnteredFoyer(newPlayer);
			Foyer.Instance.PlayerCharacterChanged(newPlayer);
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(newPlayer.specRigidbody, null, false);
			Pixelator.Instance.FadeToBlack(0.5f, true, 0f);
			yield return new WaitForSeconds(0.1f);
			if (wasInGunGame)
			{
				PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
				primaryPlayer.CharacterUsesRandomGuns = true;
				for (int i = 1; i < primaryPlayer.inventory.AllGuns.Count; i++)
				{
					Gun gun = primaryPlayer.inventory.AllGuns[i];
					primaryPlayer.inventory.RemoveGunFromInventory(gun);
					UnityEngine.Object.Destroy(gun.gameObject);
					i--;
				}
			}
			if (GameManager.Instance.SecondaryPlayer)
			{
				GameManager.Instance.SecondaryPlayer.UpdateRandomStartingEquipmentCoop(newPlayer.characterIdentity == PlayableCharacters.Eevee);
			}
			base.Finish();
			yield break;
		}

		// Token: 0x060044DD RID: 17629 RVA: 0x00163E20 File Offset: 0x00162020
		private PlayerController GeneratePlayer()
		{
			PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
			Vector3 position = primaryPlayer.transform.position;
			UnityEngine.Object.Destroy(primaryPlayer.gameObject);
			GameManager.Instance.ClearPrimaryPlayer();
			GameManager.PlayerPrefabForNewGame = (GameObject)BraveResources.Load(this.PlayerPrefabPath, ".prefab");
			PlayerController component = GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>();
			GameStatsManager.Instance.BeginNewSession(component);
			PlayerController playerController = null;
			if (playerController == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameManager.PlayerPrefabForNewGame, position, Quaternion.identity);
				GameManager.PlayerPrefabForNewGame = null;
				gameObject.SetActive(true);
				playerController = gameObject.GetComponent<PlayerController>();
			}
			FoyerCharacterSelectFlag component2 = base.Owner.GetComponent<FoyerCharacterSelectFlag>();
			if (component2 && component2.IsAlternateCostume)
			{
				playerController.SwapToAlternateCostume(null);
			}
			GameManager.Instance.PrimaryPlayer = playerController;
			playerController.PlayerIDX = 0;
			return playerController;
		}

		// Token: 0x040036E9 RID: 14057
		public string PlayerPrefabPath;
	}
}
