using System;
using System.Collections;
using InControl;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C8D RID: 3213
	[Tooltip("Only use this in the Foyer!")]
	[ActionCategory(".NPCs")]
	public class ChangeCoopMode : FsmStateAction
	{
		// Token: 0x060044CF RID: 17615 RVA: 0x00163AF0 File Offset: 0x00161CF0
		public override void Reset()
		{
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x00163AF4 File Offset: 0x00161CF4
		public override void OnEnter()
		{
			if (this.IsTestCoopValid)
			{
				base.Fsm.Event(this.IfCoopValidEvent);
			}
			else
			{
				base.Fsm.GameObject.GetComponent<TalkDoerLite>().StartCoroutine(this.HandleCharacterChange());
			}
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x00163B34 File Offset: 0x00161D34
		private IEnumerator HandleCharacterChange()
		{
			InputDevice lastActiveDevice = GameManager.Instance.LastUsedInputDeviceForConversation;
			if (this.TargetCoopMode)
			{
				GameManager.Instance.CurrentGameType = GameManager.GameType.COOP_2_PLAYER;
				if (GameManager.Instance.PrimaryPlayer)
				{
					GameManager.Instance.PrimaryPlayer.ReinitializeMovementRestrictors();
				}
				PlayerController newPlayer = this.GeneratePlayer();
				yield return null;
				GameUIRoot.Instance.ConvertCoreUIToCoopMode();
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(newPlayer.specRigidbody, null, false);
				GameManager.Instance.MainCameraController.ClearPlayerCache();
				Foyer.Instance.ProcessPlayerEnteredFoyer(newPlayer);
			}
			else
			{
				GameManager.Instance.SecondaryPlayer.SetInputOverride("getting deleted");
				UnityEngine.Object.Destroy(GameManager.Instance.SecondaryPlayer.gameObject);
				GameManager.Instance.SecondaryPlayer = null;
				GameManager.Instance.CurrentGameType = GameManager.GameType.SINGLE_PLAYER;
				if (GameManager.Instance.PrimaryPlayer)
				{
					GameManager.Instance.PrimaryPlayer.ReinitializeMovementRestrictors();
				}
			}
			BraveInput.ReassignAllControllers(lastActiveDevice);
			if (Foyer.Instance.OnCoopModeChanged != null)
			{
				Foyer.Instance.OnCoopModeChanged();
			}
			base.Finish();
			yield break;
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x00163B50 File Offset: 0x00161D50
		private PlayerController GeneratePlayer()
		{
			if (GameManager.Instance.SecondaryPlayer != null)
			{
				return GameManager.Instance.SecondaryPlayer;
			}
			GameManager.Instance.ClearSecondaryPlayer();
			GameManager.LastUsedCoopPlayerPrefab = (GameObject)BraveResources.Load(this.PlayerPrefabPath, ".prefab");
			PlayerController playerController = null;
			if (playerController == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameManager.LastUsedCoopPlayerPrefab, base.Fsm.GameObject.transform.position, Quaternion.identity);
				gameObject.SetActive(true);
				playerController = gameObject.GetComponent<PlayerController>();
			}
			FoyerCharacterSelectFlag component = base.Owner.GetComponent<FoyerCharacterSelectFlag>();
			if (component && component.IsAlternateCostume)
			{
				playerController.SwapToAlternateCostume(null);
			}
			GameManager.Instance.SecondaryPlayer = playerController;
			playerController.PlayerIDX = 1;
			return playerController;
		}

		// Token: 0x040036DF RID: 14047
		public string PlayerPrefabPath;

		// Token: 0x040036E0 RID: 14048
		public bool TargetCoopMode = true;

		// Token: 0x040036E1 RID: 14049
		public bool IsTestCoopValid;

		// Token: 0x040036E2 RID: 14050
		public FsmEvent IfCoopValidEvent;
	}
}
