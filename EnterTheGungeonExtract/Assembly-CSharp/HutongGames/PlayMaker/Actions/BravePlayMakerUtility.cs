using System;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C7F RID: 3199
	public static class BravePlayMakerUtility
	{
		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x0600449E RID: 17566 RVA: 0x00162AB0 File Offset: 0x00160CB0
		// (set) Token: 0x0600449F RID: 17567 RVA: 0x00162AB8 File Offset: 0x00160CB8
		public static FsmTransition[] CachedGlobalTransitions { get; set; }

		// Token: 0x060044A0 RID: 17568 RVA: 0x00162AC0 File Offset: 0x00160CC0
		public static string CheckCurrentModeVariable(Fsm fsm)
		{
			if (fsm.Variables.FindFsmString("currentMode") == null)
			{
				List<FsmString> list = new List<FsmString>(fsm.Variables.StringVariables);
				list.Add(new FsmString("currentMode")
				{
					Value = "modeBegin"
				});
				fsm.Variables.StringVariables = list.ToArray();
			}
			return string.Empty;
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x00162B28 File Offset: 0x00160D28
		public static string CheckEventExists(Fsm fsm, string eventName)
		{
			if (fsm != null && !Array.Exists<FsmEvent>(fsm.Events, (FsmEvent e) => e.Name == eventName))
			{
				return string.Format("No event with name \"{0}\" exists.\n", eventName);
			}
			return string.Empty;
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x00162B7C File Offset: 0x00160D7C
		public static string CheckGlobalTransitionExists(Fsm fsm, string eventName)
		{
			if (fsm != null && !Array.Exists<FsmTransition>(fsm.GlobalTransitions, (FsmTransition t) => t.EventName == eventName))
			{
				return string.Format("No global transition exists for the event \"{0}\".\n", eventName);
			}
			return string.Empty;
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x00162BD0 File Offset: 0x00160DD0
		public static bool ModeIsSetSomewhere(Fsm fsm, string eventName)
		{
			foreach (FsmState fsmState in fsm.States)
			{
				foreach (FsmStateAction fsmStateAction in fsmState.Actions)
				{
					if (fsmStateAction is SetMode && (fsmStateAction as SetMode).mode.Value == eventName)
					{
						return true;
					}
					if (fsmStateAction is TestSaveFlag && (fsmStateAction as TestSaveFlag).successType == TestSaveFlag.SuccessType.SetMode && (fsmStateAction as TestSaveFlag).mode.Value == eventName)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060044A4 RID: 17572 RVA: 0x00162C84 File Offset: 0x00160E84
		public static bool AllOthersAreFinished(FsmStateAction action)
		{
			for (int i = 0; i < action.State.Actions.Length; i++)
			{
				FsmStateAction fsmStateAction = action.State.Actions[i];
				if (fsmStateAction != action)
				{
					if (!(fsmStateAction is INonFinishingState))
					{
						if (!fsmStateAction.Finished)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x00162CE8 File Offset: 0x00160EE8
		public static void DisconnectState(FsmState fsmState)
		{
			Fsm fsm = fsmState.Fsm;
			for (int i = 0; i < fsm.GlobalTransitions.Length; i++)
			{
				FsmTransition fsmTransition = fsm.GlobalTransitions[i];
				if (fsmTransition.ToState == fsmState.Name)
				{
					fsmTransition.FsmEvent = null;
					fsmTransition.ToState = string.Empty;
				}
			}
			for (int j = 0; j < fsm.States.Length; j++)
			{
				FsmState fsmState2 = fsm.States[j];
				for (int k = 0; k < fsmState2.Transitions.Length; k++)
				{
					FsmTransition fsmTransition2 = fsmState2.Transitions[k];
					if (fsmTransition2.ToState == fsmState.Name)
					{
						fsmTransition2.FsmEvent = null;
						fsmTransition2.ToState = string.Empty;
					}
				}
			}
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x00162DC0 File Offset: 0x00160FC0
		public static float GetConsumableValue(PlayerController player, BravePlayMakerUtility.ConsumableType consumableType)
		{
			switch (consumableType)
			{
			case BravePlayMakerUtility.ConsumableType.Currency:
				return (float)player.carriedConsumables.Currency;
			case BravePlayMakerUtility.ConsumableType.Keys:
				return (float)player.carriedConsumables.KeyBullets;
			case BravePlayMakerUtility.ConsumableType.Hearts:
				return player.healthHaver.GetCurrentHealth();
			case BravePlayMakerUtility.ConsumableType.HeartContainers:
				return player.healthHaver.GetMaxHealth();
			case BravePlayMakerUtility.ConsumableType.MetaCurrency:
				return GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY);
			case BravePlayMakerUtility.ConsumableType.Blanks:
				return (float)player.Blanks;
			case BravePlayMakerUtility.ConsumableType.Armor:
				return player.healthHaver.Armor;
			default:
				Debug.LogError("Unknown consumable type: " + consumableType);
				return 0f;
			}
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x00162E64 File Offset: 0x00161064
		public static void SetConsumableValue(PlayerController player, BravePlayMakerUtility.ConsumableType consumableType, float value)
		{
			switch (consumableType)
			{
			case BravePlayMakerUtility.ConsumableType.Currency:
				player.carriedConsumables.Currency = Mathf.RoundToInt(value);
				break;
			case BravePlayMakerUtility.ConsumableType.Keys:
				player.carriedConsumables.KeyBullets = Mathf.RoundToInt(value);
				break;
			case BravePlayMakerUtility.ConsumableType.Hearts:
				player.healthHaver.ForceSetCurrentHealth(BraveMathCollege.QuantizeFloat(value, 0.5f));
				break;
			case BravePlayMakerUtility.ConsumableType.HeartContainers:
				player.healthHaver.SetHealthMaximum(BraveMathCollege.QuantizeFloat(value, 0.5f), null, false);
				break;
			case BravePlayMakerUtility.ConsumableType.MetaCurrency:
				GameStatsManager.Instance.ClearStatValueGlobal(TrackedStats.META_CURRENCY);
				GameStatsManager.Instance.SetStat(TrackedStats.META_CURRENCY, value);
				break;
			case BravePlayMakerUtility.ConsumableType.Blanks:
				player.Blanks = Mathf.FloorToInt(value);
				break;
			case BravePlayMakerUtility.ConsumableType.Armor:
				if (player.ForceZeroHealthState && value == 0f)
				{
					value = 1f;
				}
				player.healthHaver.Armor = (float)Mathf.RoundToInt(value);
				break;
			default:
				Debug.LogError("Unknown consumable type: " + consumableType);
				break;
			}
		}

		// Token: 0x02000C80 RID: 3200
		public enum ConsumableType
		{
			// Token: 0x040036B6 RID: 14006
			Currency,
			// Token: 0x040036B7 RID: 14007
			Keys,
			// Token: 0x040036B8 RID: 14008
			Hearts,
			// Token: 0x040036B9 RID: 14009
			HeartContainers,
			// Token: 0x040036BA RID: 14010
			MetaCurrency,
			// Token: 0x040036BB RID: 14011
			Blanks,
			// Token: 0x040036BC RID: 14012
			Armor
		}
	}
}
