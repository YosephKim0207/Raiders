using System;
using System.Collections;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CC0 RID: 3264
	[ActionCategory(".NPCs")]
	[Tooltip("Plays a robot bard song.")]
	public class StartBardSong : FsmStateAction
	{
		// Token: 0x0600456C RID: 17772 RVA: 0x00168138 File Offset: 0x00166338
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			PlayerController talkingPlayer = component.TalkingPlayer;
			int num = UnityEngine.Random.Range(0, this.songsToChooseFrom.Length);
			this.ApplySongToPlayer(talkingPlayer, this.songsToChooseFrom[num]);
			this.targetDialogueVariable.Value = this.songDialogues[num].Value;
			base.Finish();
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x00168194 File Offset: 0x00166394
		protected void ApplySongToPlayer(PlayerController targetPlayer, StartBardSong.BardSong targetSong)
		{
			List<StatModifier> list = new List<StatModifier>();
			if (targetSong != StartBardSong.BardSong.DAMAGE_BOOST)
			{
				if (targetSong == StartBardSong.BardSong.SPEED_BOOST)
				{
					list.Add(new StatModifier
					{
						statToBoost = PlayerStats.StatType.MovementSpeed,
						amount = 1f,
						modifyType = StatModifier.ModifyMethod.ADDITIVE
					});
				}
			}
			else
			{
				list.Add(new StatModifier
				{
					statToBoost = PlayerStats.StatType.Damage,
					amount = 1.1f,
					modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE
				});
			}
			for (int i = 0; i < list.Count; i++)
			{
				targetPlayer.ownerlessStatModifiers.Add(list[i]);
			}
			targetPlayer.stats.RecalculateStats(targetPlayer, false, false);
			if (this.HasDuration || this.LimitedToFloor)
			{
				targetPlayer.StartCoroutine(this.HandleSongLifetime(targetPlayer, targetSong, list));
			}
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x0016826C File Offset: 0x0016646C
		private IEnumerator HandleSongLifetime(PlayerController targetPlayer, StartBardSong.BardSong targetSong, List<StatModifier> activeModifiers)
		{
			float elapsed = 0f;
			for (;;)
			{
				elapsed += BraveTime.DeltaTime;
				if (this.HasDuration && elapsed > this.Duration)
				{
					break;
				}
				if (this.LimitedToFloor && GameManager.Instance.IsLoadingLevel)
				{
					goto Block_4;
				}
				yield return null;
			}
			yield break;
			Block_4:
			yield break;
			yield break;
		}

		// Token: 0x040037AB RID: 14251
		public bool HasDuration;

		// Token: 0x040037AC RID: 14252
		public float Duration = 120f;

		// Token: 0x040037AD RID: 14253
		public bool LimitedToFloor = true;

		// Token: 0x040037AE RID: 14254
		[CompoundArray("Songs", "Song Type", "Dialogue")]
		public StartBardSong.BardSong[] songsToChooseFrom;

		// Token: 0x040037AF RID: 14255
		public FsmString[] songDialogues;

		// Token: 0x040037B0 RID: 14256
		public FsmString targetDialogueVariable;

		// Token: 0x02000CC1 RID: 3265
		[Skip]
		public enum BardSong
		{
			// Token: 0x040037B2 RID: 14258
			DAMAGE_BOOST,
			// Token: 0x040037B3 RID: 14259
			SPEED_BOOST
		}
	}
}
