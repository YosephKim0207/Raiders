using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200156B RID: 5483
public class PunchoutAIActor : PunchoutGameActor
{
	// Token: 0x17001279 RID: 4729
	// (get) Token: 0x06007D89 RID: 32137 RVA: 0x0032D198 File Offset: 0x0032B398
	// (set) Token: 0x06007D8A RID: 32138 RVA: 0x0032D1A0 File Offset: 0x0032B3A0
	public int Phase { get; set; }

	// Token: 0x1700127A RID: 4730
	// (get) Token: 0x06007D8B RID: 32139 RVA: 0x0032D1AC File Offset: 0x0032B3AC
	// (set) Token: 0x06007D8C RID: 32140 RVA: 0x0032D1B4 File Offset: 0x0032B3B4
	public float TauntCooldownTimer { get; set; }

	// Token: 0x1700127B RID: 4731
	// (get) Token: 0x06007D8D RID: 32141 RVA: 0x0032D1C0 File Offset: 0x0032B3C0
	// (set) Token: 0x06007D8E RID: 32142 RVA: 0x0032D1C8 File Offset: 0x0032B3C8
	public int SuccessfulHeals { get; set; }

	// Token: 0x1700127C RID: 4732
	// (get) Token: 0x06007D8F RID: 32143 RVA: 0x0032D1D4 File Offset: 0x0032B3D4
	// (set) Token: 0x06007D90 RID: 32144 RVA: 0x0032D1DC File Offset: 0x0032B3DC
	public int NumKeysDropped { get; set; }

	// Token: 0x1700127D RID: 4733
	// (get) Token: 0x06007D91 RID: 32145 RVA: 0x0032D1E8 File Offset: 0x0032B3E8
	// (set) Token: 0x06007D92 RID: 32146 RVA: 0x0032D1F0 File Offset: 0x0032B3F0
	public int NumTimesTripleStarred { get; set; }

	// Token: 0x1700127E RID: 4734
	// (get) Token: 0x06007D93 RID: 32147 RVA: 0x0032D1FC File Offset: 0x0032B3FC
	public override bool IsDead
	{
		get
		{
			return base.state is PunchoutAIActor.DeathState;
		}
	}

	// Token: 0x06007D94 RID: 32148 RVA: 0x0032D20C File Offset: 0x0032B40C
	public override void Start()
	{
		base.Start();
		this.m_punchoutController = UnityEngine.Object.FindObjectOfType<PunchoutController>();
		base.state = new PunchoutAIActor.IntroState();
		float num = this.p1TauntChance + this.p1PunchChance + this.p1UppercutChance;
		this.p1TauntChance /= num;
		this.p1PunchChance /= num;
		this.p1UppercutChance /= num;
		num = this.p2TauntChance + this.p2SneakChance + this.p2ComboChance + this.p2TailwhipChance + this.p2ThrowAmmoChance;
		this.p2TauntChance /= num;
		this.p2SneakChance /= num;
		this.p2ComboChance /= num;
		this.p2TailwhipChance /= num;
		this.p2ThrowAmmoChance /= num;
		num = this.p3TauntChance + this.p3SneakChance + this.p3ComboChance + this.p3TailwhipChance + this.p3ThrowAmmoChance + this.p3BrassKnucklesChance + this.p3TailTornadoChance;
		this.p3TauntChance /= num;
		this.p3SneakChance /= num;
		this.p3ComboChance /= num;
		this.p3TailwhipChance /= num;
		this.p3ThrowAmmoChance /= num;
		this.p3BrassKnucklesChance /= num;
		this.p3TailTornadoChance /= num;
		this.m_startPosition = base.transform.localPosition;
		this.m_hitUntilFirstDrop = UnityEngine.Random.Range(5, 9);
	}

	// Token: 0x06007D95 RID: 32149 RVA: 0x0032D394 File Offset: 0x0032B594
	public override void ManualUpdate()
	{
		base.ManualUpdate();
		this.m_punishTimer = Mathf.Max(0f, this.m_punishTimer - BraveTime.DeltaTime);
		this.TauntCooldownTimer = Mathf.Max(0f, this.TauntCooldownTimer - BraveTime.DeltaTime);
		bool flag = false;
		if (base.state != null)
		{
			base.state.Update();
			if (base.state.IsDone)
			{
				if (base.state.PunishTime > 0f && !base.state.WasBlocked)
				{
					this.m_punishTimer = base.state.PunishTime;
				}
				base.state = null;
				flag = true;
			}
		}
		if (base.state == null)
		{
			base.state = this.GetNextState(flag);
		}
	}

	// Token: 0x06007D96 RID: 32150 RVA: 0x0032D460 File Offset: 0x0032B660
	public PunchoutGameActor.State GetNextState(bool finishedThisFrame)
	{
		if (this.m_punishTimer > 0f)
		{
			return null;
		}
		if (this.Opponent.state is PunchoutPlayerController.DeathState && base.state == null)
		{
			return new PunchoutAIActor.WinState();
		}
		if (this.Opponent.state is PunchoutGameActor.BasicAttackState)
		{
			if (this.Opponent.aiAnimator.IsPlaying("super"))
			{
				return new PunchoutAIActor.PunchState(BraveUtility.RandomBool(), true);
			}
			return new PunchoutGameActor.BlockState();
		}
		else
		{
			if (finishedThisFrame)
			{
				return null;
			}
			if (this.Phase == 0)
			{
				bool flag = UnityEngine.Random.value < BraveMathCollege.SliceProbability(this.p1AttackChance, BraveTime.DeltaTime);
				if (flag)
				{
					bool flag2 = this.TauntCooldownTimer <= 0f;
					float num = ((!flag2) ? UnityEngine.Random.RandomRange(0f, 1f - this.p1TauntChance) : UnityEngine.Random.value);
					if (num < this.p1PunchChance)
					{
						return new PunchoutAIActor.PunchState(BraveUtility.RandomBool(), true);
					}
					num -= this.p1PunchChance;
					if (num < this.p1UppercutChance)
					{
						return new PunchoutAIActor.UppercutState(BraveUtility.RandomBool());
					}
					num -= this.p1UppercutChance;
					if (num < this.p1TauntChance)
					{
						return new PunchoutAIActor.LaughTauntState();
					}
					num -= this.p1TauntChance;
				}
				return null;
			}
			if (this.Phase == 1)
			{
				bool flag3 = UnityEngine.Random.value < BraveMathCollege.SliceProbability(this.p2AttackChance, BraveTime.DeltaTime);
				if (flag3)
				{
					bool flag4 = this.TauntCooldownTimer <= 0f && this.Health < 100f && this.SuccessfulHeals < this.MaxHeals;
					float num2 = ((!flag4) ? UnityEngine.Random.RandomRange(0f, 1f - this.p2TauntChance) : UnityEngine.Random.value);
					if (num2 < this.p2SneakChance)
					{
						return new PunchoutAIActor.SuperAttackState();
					}
					num2 -= this.p2SneakChance;
					if (num2 < this.p2ComboChance)
					{
						return new PunchoutAIActor.PunchBasicComboState(BraveUtility.RandomBool());
					}
					num2 -= this.p2ComboChance;
					if (num2 < this.p2TailwhipChance)
					{
						return new PunchoutAIActor.TailWhipState();
					}
					num2 -= this.p2TailwhipChance;
					if (num2 < this.p2ThrowAmmoChance)
					{
						return new PunchoutAIActor.ThrowAmmoState(BraveUtility.RandomBool());
					}
					num2 -= this.p2ThrowAmmoChance;
					if (num2 < this.p2TauntChance)
					{
						return new PunchoutAIActor.CheeseTauntState();
					}
					num2 -= this.p2TauntChance;
				}
				return null;
			}
			if (this.Phase == 2)
			{
				bool flag5 = UnityEngine.Random.value < BraveMathCollege.SliceProbability(this.p3AttackChance, BraveTime.DeltaTime);
				if (flag5)
				{
					bool flag6 = this.TauntCooldownTimer <= 0f && this.Health < 100f && this.SuccessfulHeals < this.MaxHeals;
					float num3 = ((!flag6) ? UnityEngine.Random.RandomRange(0f, 1f - this.p3TauntChance) : UnityEngine.Random.value);
					if (num3 < this.p3SneakChance)
					{
						return new PunchoutAIActor.SuperAttackState();
					}
					num3 -= this.p3SneakChance;
					if (num3 < this.p3ComboChance)
					{
						return new PunchoutAIActor.PunchBasicComboState(BraveUtility.RandomBool());
					}
					num3 -= this.p3ComboChance;
					if (num3 < this.p3TailwhipChance)
					{
						return new PunchoutAIActor.TailWhipState();
					}
					num3 -= this.p3TailwhipChance;
					if (num3 < this.p3ThrowAmmoChance)
					{
						return new PunchoutAIActor.ThrowAmmoState(BraveUtility.RandomBool());
					}
					num3 -= this.p3ThrowAmmoChance;
					if (num3 < this.p3BrassKnucklesChance)
					{
						return new PunchoutAIActor.BrassKnucklesPunchState(BraveUtility.RandomBool());
					}
					num3 -= this.p3BrassKnucklesChance;
					if (num3 < this.p3TailTornadoChance)
					{
						return new PunchoutAIActor.SuperTailWhipState();
					}
					num3 -= this.p3TailTornadoChance;
					if (num3 < this.p3TauntChance)
					{
						return new PunchoutAIActor.CheeseTauntState();
					}
					num3 -= this.p3TauntChance;
				}
				return null;
			}
			return null;
		}
	}

	// Token: 0x06007D97 RID: 32151 RVA: 0x0032D844 File Offset: 0x0032BA44
	public override void Hit(bool isLeft, float damage, int starsUsed = 0, bool skipProcessing = false)
	{
		PunchoutGameActor.State state = base.state;
		bool flag = false;
		if (base.state != null && !skipProcessing)
		{
			if (base.state.ShouldInstantKO(starsUsed))
			{
				if (starsUsed > 0 && !this.m_hasDroppedStarDrop)
				{
					this.DropReward(isLeft, new PickupObject.ItemQuality[] { PickupObject.ItemQuality.A });
					this.m_hasDroppedStarDrop = true;
				}
				if (starsUsed >= 3)
				{
					this.NumTimesTripleStarred++;
					Debug.LogWarningFormat("Hit by 3 stars {0} times", new object[] { this.NumTimesTripleStarred });
				}
				base.aiAnimator.PlayVfx("star_hit", null, null, null);
				AkSoundEngine.PostEvent("Play_BOSS_Punchout_Punch_Hit_01", base.gameObject);
				GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Slow, Vibration.Strength.Hard);
				this.Knockdown(isLeft, true);
				return;
			}
			base.state.OnHit(ref flag, isLeft, starsUsed);
		}
		if (!flag)
		{
			if (base.IsYellow)
			{
				((PunchoutPlayerController)this.Opponent).AddStar();
			}
			if (!this.m_droppedFirstKey)
			{
				this.DropKey(isLeft);
				this.m_droppedFirstKey = true;
			}
			if (this.m_hitUntilFirstDrop > 0)
			{
				this.m_hitUntilFirstDrop--;
				if (this.m_hitUntilFirstDrop == 0)
				{
					this.DropReward(isLeft, new PickupObject.ItemQuality[]
					{
						PickupObject.ItemQuality.COMMON,
						PickupObject.ItemQuality.D,
						PickupObject.ItemQuality.C
					});
				}
			}
			if (UnityEngine.Random.value < this.m_punchoutController.NormalHitRewardChance)
			{
				this.DropReward(isLeft, new PickupObject.ItemQuality[0]);
			}
			base.aiAnimator.PlayVfx((starsUsed <= 0) ? "normal_hit" : "star_hit", null, null, null);
			AkSoundEngine.PostEvent("Play_BOSS_Punchout_Punch_Hit_01", base.gameObject);
			if (starsUsed > 0)
			{
				GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Slow, Vibration.Strength.Hard);
				if (!this.m_hasDroppedStarDrop)
				{
					this.DropReward(isLeft, new PickupObject.ItemQuality[] { PickupObject.ItemQuality.A });
					this.m_hasDroppedStarDrop = true;
				}
			}
			else
			{
				GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
			}
			if (starsUsed > 0 && !(base.state is PunchoutAIActor.InstantKnockdownState))
			{
				if (this.Health - damage <= 0f)
				{
					if (starsUsed >= 3)
					{
						this.NumTimesTripleStarred++;
						Debug.LogWarningFormat("Hit by 3 stars {0} times", new object[] { this.NumTimesTripleStarred });
					}
					this.Knockdown(isLeft, true);
					return;
				}
				base.state = new PunchoutAIActor.SuperHitState();
			}
			else if (base.state == state && !skipProcessing)
			{
				if (base.state is PunchoutAIActor.HitState)
				{
					(base.state as PunchoutAIActor.HitState).HitAgain(isLeft);
				}
				else
				{
					int num = ((!(base.state is PunchoutAIActor.DazeState)) ? 3 : 5);
					base.state = new PunchoutAIActor.HitState(isLeft, num);
				}
			}
			base.LastHitBy = this.Opponent.state;
			this.Health -= damage;
			base.FlashDamage(0.04f);
			if (skipProcessing && this.Health < 1f)
			{
				this.Health = 1f;
			}
		}
		else
		{
			GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
		}
		if (this.Health <= 0f && !skipProcessing)
		{
			if (this.Phase == 0)
			{
				this.DropReward(isLeft, new PickupObject.ItemQuality[] { PickupObject.ItemQuality.C });
			}
			else
			{
				this.DropReward(isLeft, new PickupObject.ItemQuality[] { PickupObject.ItemQuality.B });
			}
			this.GoToNextPhase(new bool?(isLeft), starsUsed > 0);
		}
	}

	// Token: 0x06007D98 RID: 32152 RVA: 0x0032DC10 File Offset: 0x0032BE10
	private void DropKey(bool isLeft)
	{
		base.StartCoroutine(this.DropKeyCR(isLeft));
	}

	// Token: 0x06007D99 RID: 32153 RVA: 0x0032DC20 File Offset: 0x0032BE20
	private IEnumerator DropKeyCR(bool isLeft)
	{
		this.NumKeysDropped++;
		while (base.state is PunchoutAIActor.ThrowAmmoState)
		{
			yield return null;
		}
		GameObject droppedItem = SpawnManager.SpawnVFX(this.DroppedItemPrefab, base.transform.position + new Vector3(-0.25f, 2.5f), Quaternion.identity);
		droppedItem.GetComponent<PunchoutDroppedItem>().Init(isLeft);
		yield break;
	}

	// Token: 0x06007D9A RID: 32154 RVA: 0x0032DC44 File Offset: 0x0032BE44
	private void DropReward(bool isLeft, params PickupObject.ItemQuality[] targetQualities)
	{
		base.StartCoroutine(this.DropRewardCR(isLeft, targetQualities));
	}

	// Token: 0x06007D9B RID: 32155 RVA: 0x0032DC58 File Offset: 0x0032BE58
	private IEnumerator DropRewardCR(bool isLeft, params PickupObject.ItemQuality[] targetQualities)
	{
		int rewardId = -1;
		if (targetQualities == null || targetQualities.Length == 0)
		{
			rewardId = BraveUtility.RandomElement<int>(this.m_punchoutController.NormalHitRewards);
			if (rewardId == GlobalItemIds.GlassGuonStone)
			{
				if (this.m_glassGuonsDropped >= this.m_punchoutController.MaxGlassGuonStones)
				{
					rewardId = GlobalItemIds.SmallHeart;
				}
				else
				{
					this.m_glassGuonsDropped++;
				}
			}
		}
		else
		{
			if (targetQualities.Length > 1)
			{
				Debug.LogFormat("Dropping a {0}-{1} item.", new object[]
				{
					targetQualities[0].ToString(),
					targetQualities[targetQualities.Length - 1].ToString()
				});
			}
			else
			{
				Debug.LogFormat("Dropping a {0} item.", new object[] { targetQualities[0].ToString() });
			}
			RewardManager rewardManager = GameManager.Instance.RewardManager;
			GenericLootTable genericLootTable = ((!BraveUtility.RandomBool()) ? rewardManager.ItemsLootTable : rewardManager.GunsLootTable);
			GameObject itemForPlayer = rewardManager.GetItemForPlayer(GameManager.Instance.BestActivePlayer, genericLootTable, BraveUtility.RandomElement<PickupObject.ItemQuality>(targetQualities), null, false, null, false, null, false, RewardManager.RewardSource.UNSPECIFIED);
			if (itemForPlayer)
			{
				rewardId = itemForPlayer.GetComponent<PickupObject>().PickupObjectId;
			}
		}
		if (rewardId >= 0)
		{
			this.DroppedRewardIds.Add(rewardId);
			while (base.state is PunchoutAIActor.ThrowAmmoState)
			{
				yield return null;
			}
			GameObject droppedItem = SpawnManager.SpawnVFX(this.DroppedItemPrefab, base.transform.position + new Vector3(-0.25f, 2.5f), Quaternion.identity);
			tk2dSprite droppedItemSprite = droppedItem.GetComponent<tk2dSprite>();
			tk2dSprite rewardSprite = PickupObjectDatabase.GetById(rewardId).GetComponent<tk2dSprite>();
			droppedItemSprite.SetSprite(rewardSprite.Collection, rewardSprite.spriteId);
			droppedItem.GetComponent<PunchoutDroppedItem>().Init(isLeft);
		}
		yield break;
	}

	// Token: 0x06007D9C RID: 32156 RVA: 0x0032DC84 File Offset: 0x0032BE84
	public void Knockdown(bool isLeft, bool triggeredBySuper)
	{
		this.Health = 0f;
		if (this.Phase < 2)
		{
			if (this.Phase == 0)
			{
				this.DropReward(isLeft, new PickupObject.ItemQuality[] { PickupObject.ItemQuality.C });
			}
			else
			{
				this.DropReward(isLeft, new PickupObject.ItemQuality[] { PickupObject.ItemQuality.B });
			}
			base.state = new PunchoutAIActor.InstantKnockdownState(isLeft);
		}
		else
		{
			base.state = new PunchoutAIActor.DeathState(isLeft, triggeredBySuper);
		}
	}

	// Token: 0x06007D9D RID: 32157 RVA: 0x0032DCF8 File Offset: 0x0032BEF8
	public void GoToNextPhase(bool? isLeft, bool triggeredBySuper)
	{
		if (isLeft == null)
		{
			isLeft = new bool?(false);
		}
		if (this.Phase < 2)
		{
			base.state = new PunchoutAIActor.PhaseTransitionState(isLeft.Value, this.Health);
		}
		else
		{
			base.state = new PunchoutAIActor.DeathState(isLeft.Value, triggeredBySuper);
		}
	}

	// Token: 0x06007D9E RID: 32158 RVA: 0x0032DD58 File Offset: 0x0032BF58
	public void UpdateUI(int phase = -1)
	{
		if (phase < 0)
		{
			phase = this.Phase;
		}
		if (phase == 0)
		{
			this.HealthBarUI.SpriteName = "punch_health_bar_green_001";
			this.RatUiSprite.SpriteName = "punch_boss_health_rat_001";
		}
		else if (phase == 1)
		{
			this.HealthBarUI.SpriteName = "punch_health_bar_yellow_001";
			this.RatUiSprite.SpriteName = "punch_boss_health_rat_002";
		}
		else
		{
			this.HealthBarUI.SpriteName = "punch_health_bar_001";
			this.RatUiSprite.SpriteName = "punch_boss_health_rat_003";
		}
	}

	// Token: 0x06007D9F RID: 32159 RVA: 0x0032DDEC File Offset: 0x0032BFEC
	public bool ShouldInstantKO(int starsUsed)
	{
		return base.state != null && base.state.ShouldInstantKO(starsUsed);
	}

	// Token: 0x06007DA0 RID: 32160 RVA: 0x0032DE08 File Offset: 0x0032C008
	public void DoFadeOut()
	{
		base.aiAnimator.PlayVfx("bomb_explosion", null, null, null);
		this.m_punchoutController.DoBombFade();
	}

	// Token: 0x06007DA1 RID: 32161 RVA: 0x0032DE4C File Offset: 0x0032C04C
	public void DoHealSuck(Vector3 deltaPos)
	{
		GameObject gameObject = SpawnManager.SpawnVFX(this.HealParticleVfx, base.transform.position + deltaPos, Quaternion.Euler(-45f, 0f, 0f));
		ParticleKiller component = gameObject.GetComponent<ParticleKiller>();
		component.ForceInit();
	}

	// Token: 0x06007DA2 RID: 32162 RVA: 0x0032DE98 File Offset: 0x0032C098
	public void DoBoxShells(Vector3 deltaPos)
	{
		GameObject gameObject = SpawnManager.SpawnVFX(this.BoxShellVfx, base.transform.position + deltaPos, Quaternion.Euler(325f, 0f, 0f));
		ParticleKiller component = gameObject.GetComponent<ParticleKiller>();
		component.ForceInit();
	}

	// Token: 0x06007DA3 RID: 32163 RVA: 0x0032DEE4 File Offset: 0x0032C0E4
	public void DoBoxShellsBack(Vector3 deltaPos, bool isLeft)
	{
		GameObject gameObject = SpawnManager.SpawnVFX(this.BoxShellBackVfx, base.transform.position + deltaPos + new Vector3(0f, 0f, 1.75f), Quaternion.Euler(340f, (float)((!isLeft) ? 225 : 135), 180f));
		ParticleKiller component = gameObject.GetComponent<ParticleKiller>();
		component.ForceInit();
	}

	// Token: 0x06007DA4 RID: 32164 RVA: 0x0032DF5C File Offset: 0x0032C15C
	public void Reset()
	{
		this.Phase = 0;
		this.Health = 100f;
		this.UpdateUI(0);
		this.NumKeysDropped = 0;
		this.DroppedRewardIds.Clear();
		this.SuccessfulHeals = 0;
		this.m_droppedFirstKey = false;
		this.m_hitUntilFirstDrop = UnityEngine.Random.Range(5, 9);
		this.m_hasDroppedStarDrop = false;
		this.m_glassGuonsDropped = 0;
		this.NumTimesTripleStarred = 0;
		if (base.state is PunchoutAIActor.DeathState)
		{
			base.state.IsDone = true;
			base.aiAnimator.EndAnimationIf("die");
		}
		base.aiAnimator.EndAnimation();
		base.state = new PunchoutAIActor.IntroState();
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.transform.localPosition = this.m_startPosition.ToVector3ZUp(base.transform.localPosition.z);
		this.Opponent.state = null;
		this.Opponent.Health = 0f;
		this.Opponent.aiAnimator.EndAnimation();
		(this.Opponent as PunchoutPlayerController).CurrentExhaust = 0f;
		(this.Opponent as PunchoutPlayerController).Stars = 0;
	}

	// Token: 0x040080B3 RID: 32947
	public dfSprite RatUiSprite;

	// Token: 0x040080B4 RID: 32948
	public tk2dSpriteAnimator BoxAnimator;

	// Token: 0x040080B5 RID: 32949
	public GameObject HealParticleVfx;

	// Token: 0x040080B6 RID: 32950
	public GameObject BoxShellVfx;

	// Token: 0x040080B7 RID: 32951
	public GameObject BoxShellBackVfx;

	// Token: 0x040080B8 RID: 32952
	public GameObject DroppedItemPrefab;

	// Token: 0x040080B9 RID: 32953
	[Header("Constants")]
	public float TauntCooldown = 5f;

	// Token: 0x040080BA RID: 32954
	public int MaxHeals = 2;

	// Token: 0x040080BB RID: 32955
	public Vector2 BoxStart;

	// Token: 0x040080BC RID: 32956
	public Vector2 BoxEnd;

	// Token: 0x040080BD RID: 32957
	public float BoxThrowTime;

	// Token: 0x040080BE RID: 32958
	public float BoxCounterStartTime;

	// Token: 0x040080BF RID: 32959
	public float BoxCounterReturnTime;

	// Token: 0x040080C0 RID: 32960
	public Color RedPulseColor;

	// Token: 0x040080C1 RID: 32961
	[Header("AI Phase 1")]
	public float p1AttackChance;

	// Token: 0x040080C2 RID: 32962
	public float p1TauntChance;

	// Token: 0x040080C3 RID: 32963
	public float p1PunchChance;

	// Token: 0x040080C4 RID: 32964
	public float p1UppercutChance;

	// Token: 0x040080C5 RID: 32965
	[Header("AI Phase 2")]
	public float p2AttackChance;

	// Token: 0x040080C6 RID: 32966
	public float p2TauntChance;

	// Token: 0x040080C7 RID: 32967
	public float p2SneakChance;

	// Token: 0x040080C8 RID: 32968
	public float p2ComboChance;

	// Token: 0x040080C9 RID: 32969
	public float p2TailwhipChance;

	// Token: 0x040080CA RID: 32970
	public float p2ThrowAmmoChance;

	// Token: 0x040080CB RID: 32971
	[Header("AI Phase 3")]
	public float p3AttackChance;

	// Token: 0x040080CC RID: 32972
	public float p3TauntChance;

	// Token: 0x040080CD RID: 32973
	public float p3SneakChance;

	// Token: 0x040080CE RID: 32974
	public float p3ComboChance;

	// Token: 0x040080CF RID: 32975
	public float p3TailwhipChance;

	// Token: 0x040080D0 RID: 32976
	public float p3ThrowAmmoChance;

	// Token: 0x040080D1 RID: 32977
	public float p3BrassKnucklesChance;

	// Token: 0x040080D2 RID: 32978
	public float p3TailTornadoChance;

	// Token: 0x040080D8 RID: 32984
	public List<int> DroppedRewardIds = new List<int>();

	// Token: 0x040080D9 RID: 32985
	private PunchoutController m_punchoutController;

	// Token: 0x040080DA RID: 32986
	private float m_punishTimer;

	// Token: 0x040080DB RID: 32987
	private Vector2 m_startPosition;

	// Token: 0x040080DC RID: 32988
	private bool m_droppedFirstKey;

	// Token: 0x040080DD RID: 32989
	private int m_hitUntilFirstDrop = 5;

	// Token: 0x040080DE RID: 32990
	private bool m_hasDroppedStarDrop;

	// Token: 0x040080DF RID: 32991
	private int m_glassGuonsDropped;

	// Token: 0x0200156C RID: 5484
	public class PunchState : PunchoutGameActor.BasicAttackState
	{
		// Token: 0x06007DA5 RID: 32165 RVA: 0x0032E0A0 File Offset: 0x0032C2A0
		public PunchState(bool isLeft, bool canWhiff)
			: base(isLeft)
		{
			this.m_canWhiff = canWhiff;
		}

		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06007DA6 RID: 32166 RVA: 0x0032E0B0 File Offset: 0x0032C2B0
		public override string AnimName
		{
			get
			{
				return "punch";
			}
		}

		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06007DA7 RID: 32167 RVA: 0x0032E0B8 File Offset: 0x0032C2B8
		public override int DamageFrame
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x17001281 RID: 4737
		// (get) Token: 0x06007DA8 RID: 32168 RVA: 0x0032E0BC File Offset: 0x0032C2BC
		public override float Damage
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x06007DA9 RID: 32169 RVA: 0x0032E0C4 File Offset: 0x0032C2C4
		public override bool CanHitOpponent(PunchoutGameActor.State state)
		{
			if (this.m_missed)
			{
				return false;
			}
			bool flag = !(state is PunchoutGameActor.DuckState);
			if (state is PunchoutGameActor.BlockState)
			{
				(state as PunchoutGameActor.BlockState).Bonk();
				flag = false;
			}
			PunchoutGameActor.DodgeState dodgeState = state as PunchoutGameActor.DodgeState;
			if (dodgeState != null && dodgeState.IsLeft != this.IsLeft)
			{
				flag = false;
			}
			if (!flag && this.m_canWhiff)
			{
				base.Actor.Play("punch_miss", this.IsLeft);
				this.m_missed = true;
				return false;
			}
			return flag;
		}

		// Token: 0x06007DAA RID: 32170 RVA: 0x0032E154 File Offset: 0x0032C354
		public override bool CanBeHit(bool isLeft)
		{
			return !base.WasBlocked && (this.m_missed || (base.Actor.CurrentFrame >= 3 && base.Actor.CurrentFrameFloat < 5.5f));
		}

		// Token: 0x06007DAB RID: 32171 RVA: 0x0032E1A4 File Offset: 0x0032C3A4
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (!this.m_missed && currentFrame == 3)
			{
				base.Actor.FlashWarn(2.5f);
			}
		}

		// Token: 0x06007DAC RID: 32172 RVA: 0x0032E1D0 File Offset: 0x0032C3D0
		public override void OnHit(ref bool preventDamage, bool isLeft, int starsUsed)
		{
			base.OnHit(ref preventDamage, isLeft, starsUsed);
			if (!this.m_missed && (base.Actor.CurrentFrame == 4 || base.Actor.CurrentFrame == 5))
			{
				base.Actor.state = new PunchoutAIActor.DazeState();
			}
		}

		// Token: 0x040080E0 RID: 32992
		private bool m_missed;

		// Token: 0x040080E1 RID: 32993
		private bool m_canWhiff;
	}

	// Token: 0x0200156D RID: 5485
	public class UppercutState : PunchoutGameActor.BasicAttackState
	{
		// Token: 0x06007DAD RID: 32173 RVA: 0x0032E224 File Offset: 0x0032C424
		public UppercutState(bool isLeft)
			: base(isLeft)
		{
		}

		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x06007DAE RID: 32174 RVA: 0x0032E230 File Offset: 0x0032C430
		public override string AnimName
		{
			get
			{
				return "uppercut";
			}
		}

		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x06007DAF RID: 32175 RVA: 0x0032E238 File Offset: 0x0032C438
		public override int DamageFrame
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x06007DB0 RID: 32176 RVA: 0x0032E23C File Offset: 0x0032C43C
		public override float Damage
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06007DB1 RID: 32177 RVA: 0x0032E244 File Offset: 0x0032C444
		public override float PunishTime
		{
			get
			{
				return 0.3f;
			}
		}

		// Token: 0x06007DB2 RID: 32178 RVA: 0x0032E24C File Offset: 0x0032C44C
		public override bool CanHitOpponent(PunchoutGameActor.State state)
		{
			if (state is PunchoutGameActor.BlockState)
			{
				(state as PunchoutGameActor.BlockState).Bonk();
				return false;
			}
			if (state is PunchoutGameActor.DuckState)
			{
				return false;
			}
			PunchoutGameActor.DodgeState dodgeState = state as PunchoutGameActor.DodgeState;
			return dodgeState == null || dodgeState.IsLeft == this.IsLeft;
		}

		// Token: 0x06007DB3 RID: 32179 RVA: 0x0032E2A0 File Offset: 0x0032C4A0
		public override bool CanBeHit(bool isLeft)
		{
			return !base.WasBlocked && base.Actor.CurrentFrame > this.DamageFrame;
		}
	}

	// Token: 0x0200156E RID: 5486
	public class SuperAttackState : PunchoutGameActor.BasicAttackState
	{
		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x06007DB5 RID: 32181 RVA: 0x0032E2CC File Offset: 0x0032C4CC
		public override string AnimName
		{
			get
			{
				return "super";
			}
		}

		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06007DB6 RID: 32182 RVA: 0x0032E2D4 File Offset: 0x0032C4D4
		public override int DamageFrame
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06007DB7 RID: 32183 RVA: 0x0032E2D8 File Offset: 0x0032C4D8
		public override float Damage
		{
			get
			{
				return 35f;
			}
		}

		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x06007DB8 RID: 32184 RVA: 0x0032E2E0 File Offset: 0x0032C4E0
		public override float PunishTime
		{
			get
			{
				return 0.3f;
			}
		}

		// Token: 0x06007DB9 RID: 32185 RVA: 0x0032E2E8 File Offset: 0x0032C4E8
		public override bool CanHitOpponent(PunchoutGameActor.State state)
		{
			return !(state is PunchoutGameActor.DuckState);
		}

		// Token: 0x06007DBA RID: 32186 RVA: 0x0032E2F8 File Offset: 0x0032C4F8
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (currentFrame == 15)
			{
				base.Actor.FlashWarn(1f);
			}
		}

		// Token: 0x06007DBB RID: 32187 RVA: 0x0032E31C File Offset: 0x0032C51C
		public override bool CanBeHit(bool isLeft)
		{
			return !base.WasBlocked && (base.Actor.CurrentFrame == 15 || base.Actor.CurrentFrame >= 17);
		}

		// Token: 0x06007DBC RID: 32188 RVA: 0x0032E354 File Offset: 0x0032C554
		public override bool IsFarAway()
		{
			return base.Actor.CurrentFrame >= 2 && base.Actor.CurrentFrame <= 15;
		}

		// Token: 0x06007DBD RID: 32189 RVA: 0x0032E37C File Offset: 0x0032C57C
		public override void OnHit(ref bool preventDamage, bool isLeft, int starsUsed)
		{
			base.OnHit(ref preventDamage, isLeft, starsUsed);
			if (base.Actor.CurrentFrame == 15)
			{
				base.Actor.state = new PunchoutAIActor.DazeState();
			}
		}
	}

	// Token: 0x0200156F RID: 5487
	public class TailWhipState : PunchoutGameActor.BasicAttackState
	{
		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x06007DBF RID: 32191 RVA: 0x0032E3B4 File Offset: 0x0032C5B4
		public override string AnimName
		{
			get
			{
				return "tail_whip";
			}
		}

		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x06007DC0 RID: 32192 RVA: 0x0032E3BC File Offset: 0x0032C5BC
		public override int DamageFrame
		{
			get
			{
				return 11;
			}
		}

		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x06007DC1 RID: 32193 RVA: 0x0032E3C0 File Offset: 0x0032C5C0
		public override float Damage
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x06007DC2 RID: 32194 RVA: 0x0032E3C8 File Offset: 0x0032C5C8
		public override float PunishTime
		{
			get
			{
				return 0.3f;
			}
		}

		// Token: 0x06007DC3 RID: 32195 RVA: 0x0032E3D0 File Offset: 0x0032C5D0
		public override void Update()
		{
			base.Update();
			base.WasBlocked = false;
		}

		// Token: 0x06007DC4 RID: 32196 RVA: 0x0032E3E0 File Offset: 0x0032C5E0
		public override bool CanHitOpponent(PunchoutGameActor.State state)
		{
			if (state is PunchoutGameActor.BlockState)
			{
				(state as PunchoutGameActor.BlockState).Bonk();
				return false;
			}
			return true;
		}

		// Token: 0x06007DC5 RID: 32197 RVA: 0x0032E3FC File Offset: 0x0032C5FC
		public override bool CanBeHit(bool isLeft)
		{
			return base.Actor.CurrentFrameFloat >= 8.5f && base.Actor.CurrentFrame <= 10;
		}

		// Token: 0x06007DC6 RID: 32198 RVA: 0x0032E428 File Offset: 0x0032C628
		public override void OnHit(ref bool preventDamage, bool isLeft, int starsUsed)
		{
			base.OnHit(ref preventDamage, isLeft, starsUsed);
			base.Actor.state = new PunchoutAIActor.TailDazeState();
		}

		// Token: 0x06007DC7 RID: 32199 RVA: 0x0032E444 File Offset: 0x0032C644
		public override bool ShouldInstantKO(int starsUsed)
		{
			return starsUsed >= 1;
		}
	}

	// Token: 0x02001570 RID: 5488
	public class ThrowAmmoState : PunchoutGameActor.State
	{
		// Token: 0x06007DC8 RID: 32200 RVA: 0x0032E450 File Offset: 0x0032C650
		public ThrowAmmoState(bool isLeft)
			: base(isLeft)
		{
		}

		// Token: 0x06007DC9 RID: 32201 RVA: 0x0032E47C File Offset: 0x0032C67C
		public override void Start()
		{
			base.Actor.Play("throw_intro", this.IsLeft);
			this.m_state = PunchoutAIActor.ThrowAmmoState.ThrowState.Intro;
			base.Actor.MoveCamera(new Vector2(0f, 1f), 0.5f);
			base.Start();
		}

		// Token: 0x06007DCA RID: 32202 RVA: 0x0032E4CC File Offset: 0x0032C6CC
		public override void Update()
		{
			base.Update();
			if (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Throw && this.m_hasThrown)
			{
				this.m_boxThrowTimer += BraveTime.DeltaTime;
				float num = this.m_boxThrowTimer / this.m_boxThrowTime;
				Vector2 vector = Vector2.Lerp(this.m_boxStartPos, this.m_boxEndPos, num);
				vector.y += Mathf.Sin(num * 3.1415927f) * 0.5f;
				base.ActorEnemy.BoxAnimator.transform.localPosition = vector.ToVector3ZisY(0f);
				base.ActorEnemy.BoxAnimator.sprite.HeightOffGround = Mathf.Lerp(16f, 5f, num);
				base.ActorEnemy.BoxAnimator.sprite.UpdateZDepth();
				if (this.m_boxThrowTimer >= this.m_boxThrowTime)
				{
					if (this.CanHitOpponent())
					{
						base.ActorEnemy.DoBoxShells(this.m_boxEndPos + new Vector2(0f, 1f));
						PunchoutPlayerController.PlayerPunchState playerPunchState = base.Actor.Opponent.state as PunchoutPlayerController.PlayerPunchState;
						if (playerPunchState != null && playerPunchState.RealFrame == 0)
						{
							this.m_state = PunchoutAIActor.ThrowAmmoState.ThrowState.Return;
							this.BoxReturn();
						}
						else
						{
							this.m_state = PunchoutAIActor.ThrowAmmoState.ThrowState.ThrowLaugh;
							base.Actor.Play("throw_laugh", this.IsLeft);
							base.Actor.Opponent.Hit(!this.IsLeft, this.Damage, 0, false);
							base.Actor.Opponent.aiAnimator.PlayVfx("box_hit", null, null, null);
							base.ActorEnemy.BoxAnimator.gameObject.SetActive(false);
						}
					}
					else
					{
						this.m_state = PunchoutAIActor.ThrowAmmoState.ThrowState.ThrowMiss;
						base.Actor.Play("throw_miss", this.IsLeft);
						this.BoxMiss();
					}
				}
			}
			else if (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.ThrowMiss)
			{
				this.m_boxThrowTimer += BraveTime.DeltaTime;
				float num2 = this.m_boxThrowTimer / this.m_boxThrowTime;
				base.ActorEnemy.BoxAnimator.transform.localPosition = Vector2.Lerp(this.m_boxStartPos, this.m_boxEndPos, num2);
				base.ActorEnemy.BoxAnimator.sprite.HeightOffGround = 5f;
				base.ActorEnemy.BoxAnimator.sprite.UpdateZDepth();
				if (this.m_boxThrowTimer >= this.m_boxThrowTime)
				{
					base.ActorEnemy.BoxAnimator.gameObject.SetActive(false);
				}
			}
			else if (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Return)
			{
				this.m_boxThrowTimer += BraveTime.DeltaTime;
				float num3 = this.m_boxThrowTimer / this.m_boxThrowTime;
				base.ActorEnemy.BoxAnimator.transform.localPosition = Vector2.Lerp(this.m_boxStartPos, this.m_boxEndPos, num3);
				base.ActorEnemy.BoxAnimator.sprite.HeightOffGround = Mathf.Lerp(6f, 16f, num3);
				base.ActorEnemy.BoxAnimator.sprite.UpdateZDepth();
				if (this.m_boxThrowTimer >= this.m_boxThrowTime)
				{
					base.Actor.Play("throw_hit", this.IsLeft);
					this.m_state = PunchoutAIActor.ThrowAmmoState.ThrowState.Hit;
					base.Actor.Hit(this.IsLeft, this.ReturnDamage, 0, true);
					base.Actor.aiAnimator.PlayVfx((!this.IsLeft) ? "box_hit_right" : "box_hit_left", null, null, null);
					base.ActorEnemy.BoxAnimator.gameObject.SetActive(false);
					base.ActorEnemy.DoBoxShellsBack(this.m_boxEndPos, this.IsLeft);
				}
			}
		}

		// Token: 0x06007DCB RID: 32203 RVA: 0x0032E8E0 File Offset: 0x0032CAE0
		public bool CanHitOpponent()
		{
			PunchoutGameActor.State state = base.Actor.Opponent.state;
			if (this.m_state != PunchoutAIActor.ThrowAmmoState.ThrowState.Throw)
			{
				return false;
			}
			if (state is PunchoutGameActor.DuckState)
			{
				return false;
			}
			PunchoutGameActor.DodgeState dodgeState = state as PunchoutGameActor.DodgeState;
			return dodgeState == null || dodgeState.IsLeft != this.IsLeft;
		}

		// Token: 0x06007DCC RID: 32204 RVA: 0x0032E93C File Offset: 0x0032CB3C
		public override void OnAnimationCompleted()
		{
			base.OnAnimationCompleted();
			if (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Intro)
			{
				if (UnityEngine.Random.value < this.SwitchChance)
				{
					base.Actor.Play("throw_switch", this.IsLeft);
					this.m_state = PunchoutAIActor.ThrowAmmoState.ThrowState.Switch;
				}
				else
				{
					base.Actor.Play("throw", this.IsLeft);
					this.m_state = PunchoutAIActor.ThrowAmmoState.ThrowState.Throw;
				}
				GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Quick, Vibration.Strength.Light);
			}
			else if (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Switch)
			{
				this.IsLeft = !this.IsLeft;
				base.Actor.Play("throw", this.IsLeft);
				this.m_state = PunchoutAIActor.ThrowAmmoState.ThrowState.Throw;
			}
			else if (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Throw && base.Actor.aiAnimator.IsPlaying("throw"))
			{
				base.Actor.Play("throw_outro", this.IsLeft);
				this.m_state = PunchoutAIActor.ThrowAmmoState.ThrowState.Outro;
				base.Actor.MoveCamera(new Vector2(0f, 0f), 0.5f);
			}
			else if (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.ThrowLaugh || this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.ThrowMiss || this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Hit)
			{
				base.Actor.Play("throw_outro", this.IsLeft);
				this.m_state = PunchoutAIActor.ThrowAmmoState.ThrowState.Outro;
				base.Actor.MoveCamera(new Vector2(0f, 0f), 0.5f);
			}
			else if (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Outro)
			{
				base.IsDone = true;
				GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Quick, Vibration.Strength.Light);
			}
		}

		// Token: 0x06007DCD RID: 32205 RVA: 0x0032EAF4 File Offset: 0x0032CCF4
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Switch && currentFrame == 6)
			{
				GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Quick, Vibration.Strength.Light);
			}
			else if (base.Actor.aiAnimator.IsPlaying("throw_miss") && currentFrame % 3 == 2)
			{
				GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Quick, Vibration.Strength.Light);
			}
			else if (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Throw && currentFrame == 15)
			{
				this.BoxThrow();
			}
		}

		// Token: 0x06007DCE RID: 32206 RVA: 0x0032EB88 File Offset: 0x0032CD88
		public override bool CanBeHit(bool isLeft)
		{
			return (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Throw && this.m_boxThrowTimer > base.ActorEnemy.BoxCounterStartTime) || (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Return && (double)this.m_boxThrowTimer < 0.33);
		}

		// Token: 0x06007DCF RID: 32207 RVA: 0x0032EBDC File Offset: 0x0032CDDC
		public override bool IsFarAway()
		{
			return true;
		}

		// Token: 0x06007DD0 RID: 32208 RVA: 0x0032EBE0 File Offset: 0x0032CDE0
		public override void OnHit(ref bool preventDamage, bool isLeft, int starsUsed)
		{
			base.OnHit(ref preventDamage, isLeft, starsUsed);
			if (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Throw)
			{
				preventDamage = true;
				this.m_state = PunchoutAIActor.ThrowAmmoState.ThrowState.Return;
				this.BoxReturn();
			}
			else if (this.m_state == PunchoutAIActor.ThrowAmmoState.ThrowState.Return)
			{
				preventDamage = true;
			}
		}

		// Token: 0x06007DD1 RID: 32209 RVA: 0x0032EC1C File Offset: 0x0032CE1C
		private void BoxThrow()
		{
			this.m_boxStartPos = base.ActorEnemy.BoxStart;
			if (this.IsLeft)
			{
				this.m_boxStartPos.x = this.m_boxStartPos.x * -1f;
			}
			this.m_boxEndPos = base.ActorEnemy.BoxEnd;
			this.m_boxThrowTime = base.ActorEnemy.BoxThrowTime;
			this.m_boxThrowTimer = 0f;
			base.ActorEnemy.BoxAnimator.gameObject.SetActive(true);
			base.ActorEnemy.BoxAnimator.Play(this.IsLeft ? "rat_box_left" : "rat_box_right");
			base.ActorEnemy.BoxAnimator.transform.localPosition = this.m_boxStartPos;
			base.ActorEnemy.BoxAnimator.sprite.HeightOffGround = 16f;
			base.ActorEnemy.BoxAnimator.sprite.UpdateZDepth();
			this.m_hasThrown = true;
		}

		// Token: 0x06007DD2 RID: 32210 RVA: 0x0032ED20 File Offset: 0x0032CF20
		private void BoxMiss()
		{
			base.ActorEnemy.BoxAnimator.Play(this.IsLeft ? "rat_box_left_fall" : "rat_box_right_fall");
			Vector2 vector = this.m_boxEndPos - this.m_boxStartPos;
			this.m_boxStartPos = this.m_boxEndPos;
			this.m_boxEndPos = this.m_boxStartPos + vector;
			this.m_boxThrowTime = base.ActorEnemy.BoxAnimator.CurrentClip.BaseClipLength;
			this.m_boxThrowTimer = 0f;
		}

		// Token: 0x06007DD3 RID: 32211 RVA: 0x0032EDB0 File Offset: 0x0032CFB0
		private void BoxReturn()
		{
			this.m_boxEndPos = this.m_boxStartPos;
			this.m_boxStartPos = base.ActorEnemy.BoxAnimator.transform.localPosition.XY();
			if (this.m_boxStartPos.y < 2f)
			{
				this.m_boxStartPos.y = 2f;
			}
			this.m_boxThrowTime = base.ActorEnemy.BoxCounterReturnTime;
			this.m_boxThrowTimer = 0f;
			base.ActorEnemy.BoxAnimator.Play(this.IsLeft ? "rat_box_left_return" : "rat_box_right_return");
			base.ActorEnemy.BoxAnimator.transform.localPosition = this.m_boxStartPos;
			base.ActorEnemy.BoxAnimator.sprite.HeightOffGround = 6f;
			base.ActorEnemy.BoxAnimator.sprite.UpdateZDepth();
			base.ActorEnemy.BoxAnimator.ignoreTimeScale = true;
			StickyFrictionManager.Instance.RegisterCustomStickyFriction(0.3125f, 0f, false, false);
			base.Actor.Opponent.aiAnimator.PlayVfx((!base.Actor.Opponent.state.IsLeft) ? "box_punch_right" : "box_punch_left", null, null, null);
		}

		// Token: 0x040080E2 RID: 32994
		public float SwitchChance = 0.33f;

		// Token: 0x040080E3 RID: 32995
		public float Damage = 20f;

		// Token: 0x040080E4 RID: 32996
		public float ReturnDamage = 20f;

		// Token: 0x040080E5 RID: 32997
		private PunchoutAIActor.ThrowAmmoState.ThrowState m_state;

		// Token: 0x040080E6 RID: 32998
		private bool m_hasThrown;

		// Token: 0x040080E7 RID: 32999
		private Vector2 m_boxStartPos;

		// Token: 0x040080E8 RID: 33000
		private Vector2 m_boxEndPos;

		// Token: 0x040080E9 RID: 33001
		private float m_boxThrowTime;

		// Token: 0x040080EA RID: 33002
		private float m_boxThrowTimer;

		// Token: 0x02001571 RID: 5489
		private enum ThrowState
		{
			// Token: 0x040080EC RID: 33004
			None,
			// Token: 0x040080ED RID: 33005
			Intro,
			// Token: 0x040080EE RID: 33006
			Switch,
			// Token: 0x040080EF RID: 33007
			Throw,
			// Token: 0x040080F0 RID: 33008
			ThrowMiss,
			// Token: 0x040080F1 RID: 33009
			ThrowLaugh,
			// Token: 0x040080F2 RID: 33010
			Outro,
			// Token: 0x040080F3 RID: 33011
			Return,
			// Token: 0x040080F4 RID: 33012
			Hit
		}
	}

	// Token: 0x02001572 RID: 5490
	public class PunchBasicComboState : PunchoutGameActor.BasicComboState
	{
		// Token: 0x06007DD4 RID: 32212 RVA: 0x0032EF24 File Offset: 0x0032D124
		public PunchBasicComboState(bool firstIsLeft)
			: base(new PunchoutGameActor.State[]
			{
				new PunchoutAIActor.PunchState(firstIsLeft, false),
				new PunchoutAIActor.PunchState(!firstIsLeft, false),
				new PunchoutAIActor.PunchState(firstIsLeft, false),
				new PunchoutAIActor.UppercutState(!firstIsLeft)
			})
		{
		}

		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x06007DD5 RID: 32213 RVA: 0x0032EF60 File Offset: 0x0032D160
		public override float PunishTime
		{
			get
			{
				return 0.3f;
			}
		}
	}

	// Token: 0x02001573 RID: 5491
	public class BrassKnucklesPunchState : PunchoutGameActor.BasicAttackState
	{
		// Token: 0x06007DD6 RID: 32214 RVA: 0x0032EF68 File Offset: 0x0032D168
		public BrassKnucklesPunchState(bool isLeft)
			: base(isLeft)
		{
		}

		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x06007DD7 RID: 32215 RVA: 0x0032EF74 File Offset: 0x0032D174
		public override string AnimName
		{
			get
			{
				return "brass_punch";
			}
		}

		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x06007DD8 RID: 32216 RVA: 0x0032EF7C File Offset: 0x0032D17C
		public override int DamageFrame
		{
			get
			{
				return 26;
			}
		}

		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x06007DD9 RID: 32217 RVA: 0x0032EF80 File Offset: 0x0032D180
		public override float Damage
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x06007DDA RID: 32218 RVA: 0x0032EF88 File Offset: 0x0032D188
		public override float PunishTime
		{
			get
			{
				return 0.3f;
			}
		}

		// Token: 0x06007DDB RID: 32219 RVA: 0x0032EF90 File Offset: 0x0032D190
		public override bool CanHitOpponent(PunchoutGameActor.State state)
		{
			return true;
		}

		// Token: 0x06007DDC RID: 32220 RVA: 0x0032EF94 File Offset: 0x0032D194
		public override bool CanBeHit(bool isLeft)
		{
			return !base.WasBlocked && (base.Actor.CurrentFrame == 24 || base.Actor.CurrentFrame == 25);
		}

		// Token: 0x06007DDD RID: 32221 RVA: 0x0032EFC8 File Offset: 0x0032D1C8
		public override bool IsFarAway()
		{
			return base.Actor.CurrentFrame < 23 || base.Actor.CurrentFrameFloat >= 25.5f;
		}

		// Token: 0x06007DDE RID: 32222 RVA: 0x0032EFF4 File Offset: 0x0032D1F4
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (currentFrame == 4 || currentFrame == 14 || currentFrame == 19 || currentFrame == 24)
			{
				GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Quick, Vibration.Strength.Light);
			}
			if (currentFrame == 23)
			{
				base.Actor.FlashWarn(2.5f);
			}
		}

		// Token: 0x06007DDF RID: 32223 RVA: 0x0032F054 File Offset: 0x0032D254
		public override void OnHit(ref bool preventDamage, bool isLeft, int starsUsed)
		{
			base.OnHit(ref preventDamage, isLeft, starsUsed);
			base.Actor.state = new PunchoutAIActor.DazeState();
		}

		// Token: 0x06007DE0 RID: 32224 RVA: 0x0032F070 File Offset: 0x0032D270
		public override bool ShouldInstantKO(int starsUsed)
		{
			return starsUsed >= 3;
		}
	}

	// Token: 0x02001574 RID: 5492
	public class SuperTailWhipState : PunchoutGameActor.BasicAttackState
	{
		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x06007DE2 RID: 32226 RVA: 0x0032F084 File Offset: 0x0032D284
		public override string AnimName
		{
			get
			{
				return "super_tail_whip";
			}
		}

		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x06007DE3 RID: 32227 RVA: 0x0032F08C File Offset: 0x0032D28C
		public int FlashFrame
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x06007DE4 RID: 32228 RVA: 0x0032F090 File Offset: 0x0032D290
		public override int DamageFrame
		{
			get
			{
				return 18;
			}
		}

		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x06007DE5 RID: 32229 RVA: 0x0032F094 File Offset: 0x0032D294
		public override float Damage
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x06007DE6 RID: 32230 RVA: 0x0032F09C File Offset: 0x0032D29C
		public override float PunishTime
		{
			get
			{
				return 0.3f;
			}
		}

		// Token: 0x06007DE7 RID: 32231 RVA: 0x0032F0A4 File Offset: 0x0032D2A4
		public override void Update()
		{
			base.Update();
			base.WasBlocked = false;
		}

		// Token: 0x06007DE8 RID: 32232 RVA: 0x0032F0B4 File Offset: 0x0032D2B4
		public override bool CanHitOpponent(PunchoutGameActor.State state)
		{
			if (state is PunchoutGameActor.BlockState)
			{
				(state as PunchoutGameActor.BlockState).Bonk();
				return false;
			}
			this.m_hitPlayer = true;
			return true;
		}

		// Token: 0x06007DE9 RID: 32233 RVA: 0x0032F0D8 File Offset: 0x0032D2D8
		public override bool CanBeHit(bool isLeft)
		{
			return this.m_spins == 0 && base.Actor.CurrentFrame >= this.FlashFrame && base.Actor.CurrentFrame < this.DamageFrame;
		}

		// Token: 0x06007DEA RID: 32234 RVA: 0x0032F114 File Offset: 0x0032D314
		public override bool IsFarAway()
		{
			return base.Actor.CurrentFrame >= 6 && base.Actor.CurrentFrame <= 14;
		}

		// Token: 0x06007DEB RID: 32235 RVA: 0x0032F13C File Offset: 0x0032D33C
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (currentFrame == 5 || currentFrame == 7 || currentFrame == 9 || currentFrame == 11)
			{
				GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Quick, Vibration.Strength.Light);
			}
			if (this.m_spins == 0 && currentFrame == this.FlashFrame)
			{
				base.Actor.FlashWarn((float)(this.DamageFrame - this.FlashFrame));
			}
			if (currentFrame == this.DamageFrame)
			{
				this.m_spins++;
			}
			if (currentFrame == 22 && this.m_spins >= 4)
			{
				if (this.m_hitPlayer)
				{
					base.Actor.aiAnimator.EndAnimation();
				}
				else
				{
					base.Actor.state = new PunchoutAIActor.DazeState();
				}
			}
		}

		// Token: 0x06007DEC RID: 32236 RVA: 0x0032F210 File Offset: 0x0032D410
		public override void OnHit(ref bool preventDamage, bool isLeft, int starsUsed)
		{
			base.OnHit(ref preventDamage, isLeft, starsUsed);
			base.Actor.state = new PunchoutAIActor.DazeState();
		}

		// Token: 0x06007DED RID: 32237 RVA: 0x0032F22C File Offset: 0x0032D42C
		public override bool ShouldInstantKO(int starsUsed)
		{
			return starsUsed >= 3;
		}

		// Token: 0x040080F5 RID: 33013
		private int m_spins;

		// Token: 0x040080F6 RID: 33014
		private bool m_hitPlayer;
	}

	// Token: 0x02001575 RID: 5493
	public class LaughTauntState : PunchoutGameActor.State
	{
		// Token: 0x06007DEE RID: 32238 RVA: 0x0032F238 File Offset: 0x0032D438
		public LaughTauntState()
			: base(false)
		{
		}

		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x06007DEF RID: 32239 RVA: 0x0032F244 File Offset: 0x0032D444
		public override string AnimName
		{
			get
			{
				return "laugh_taunt";
			}
		}

		// Token: 0x06007DF0 RID: 32240 RVA: 0x0032F24C File Offset: 0x0032D44C
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (currentFrame == 6)
			{
				base.Actor.FlashWarn(1.5f);
			}
			if (currentFrame % 3 == 1)
			{
				GameManager.Instance.PrimaryPlayer.DoVibration(Vibration.Time.Quick, Vibration.Strength.Light);
			}
		}

		// Token: 0x06007DF1 RID: 32241 RVA: 0x0032F288 File Offset: 0x0032D488
		public override bool ShouldInstantKO(int starsUsed)
		{
			return starsUsed >= 2;
		}

		// Token: 0x06007DF2 RID: 32242 RVA: 0x0032F294 File Offset: 0x0032D494
		public override void Stop()
		{
			base.Stop();
			base.ActorEnemy.TauntCooldownTimer = base.ActorEnemy.TauntCooldown;
		}
	}

	// Token: 0x02001576 RID: 5494
	public class CheeseTauntState : PunchoutGameActor.State
	{
		// Token: 0x06007DF3 RID: 32243 RVA: 0x0032F2B4 File Offset: 0x0032D4B4
		public CheeseTauntState()
			: base(false)
		{
		}

		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x06007DF4 RID: 32244 RVA: 0x0032F2C8 File Offset: 0x0032D4C8
		public override string AnimName
		{
			get
			{
				return "cheese_taunt";
			}
		}

		// Token: 0x06007DF5 RID: 32245 RVA: 0x0032F2D0 File Offset: 0x0032D4D0
		public override void Start()
		{
			base.Start();
			this.m_startingHealth = base.Actor.Health;
			this.m_targetHealth = new float?(Mathf.Min(100f, this.m_startingHealth + this.HealAmount));
		}

		// Token: 0x06007DF6 RID: 32246 RVA: 0x0032F30C File Offset: 0x0032D50C
		public override bool CanBeHit(bool isLeft)
		{
			if (this.m_isCountering)
			{
				return false;
			}
			if (base.Actor.CurrentFrame < 9)
			{
				this.m_isCountering = true;
				base.Actor.Play("cheese_taunt_counter");
				return false;
			}
			return base.Actor.CurrentFrameFloat >= 8.5f && base.Actor.CurrentFrame <= 9 && !isLeft;
		}

		// Token: 0x06007DF7 RID: 32247 RVA: 0x0032F380 File Offset: 0x0032D580
		public override void OnFrame(int currentFrame)
		{
			if (!this.m_isCountering)
			{
				if (currentFrame == 9)
				{
					GameManager.Instance.PrimaryPlayer.DoVibration(0.54545456f, Vibration.Strength.Light);
				}
				else if (currentFrame == 10)
				{
					base.ActorEnemy.DoHealSuck(new Vector3(-0.125f, 2f, -2.5f));
					base.ActorEnemy.SuccessfulHeals++;
				}
				else if (currentFrame == 11 || currentFrame == 15)
				{
					base.Actor.PulseColor(base.ActorEnemy.RedPulseColor, 3f);
				}
			}
			else if (currentFrame == 8)
			{
				base.Actor.Opponent.Hit(true, 3f, 0, false);
				(base.Actor.Opponent as PunchoutPlayerController).Exhaust(new float?(1.45f));
			}
			else if (currentFrame == 16)
			{
				GameManager.Instance.PrimaryPlayer.DoVibration(0.72727275f, Vibration.Strength.Light);
			}
			else if (currentFrame == 17)
			{
				base.ActorEnemy.DoHealSuck(new Vector3(-0.125f, 2f, -2.5f));
				base.ActorEnemy.SuccessfulHeals++;
			}
			else if (currentFrame == 18 || currentFrame == 22)
			{
				base.Actor.PulseColor(base.ActorEnemy.RedPulseColor, 3f);
			}
		}

		// Token: 0x06007DF8 RID: 32248 RVA: 0x0032F4FC File Offset: 0x0032D6FC
		public override void Update()
		{
			base.Update();
			if (!this.m_isCountering)
			{
				if (base.Actor.CurrentFrame >= 10)
				{
					float? targetHealth = this.m_targetHealth;
					if (targetHealth != null)
					{
						base.Actor.Health = Mathf.Lerp(this.m_startingHealth, this.m_targetHealth.Value, Mathf.Clamp01((base.Actor.CurrentFrameFloat - 10f) / 7f));
					}
				}
			}
			else if (base.Actor.CurrentFrame >= 17)
			{
				float? targetHealth2 = this.m_targetHealth;
				if (targetHealth2 != null)
				{
					base.Actor.Health = Mathf.Lerp(this.m_startingHealth, this.m_targetHealth.Value, Mathf.Clamp01((base.Actor.CurrentFrameFloat - 17f) / 7f));
				}
			}
		}

		// Token: 0x06007DF9 RID: 32249 RVA: 0x0032F5E4 File Offset: 0x0032D7E4
		public override void OnHit(ref bool preventDamage, bool isLeft, int starsUsed)
		{
			base.OnHit(ref preventDamage, isLeft, starsUsed);
			this.m_targetHealth = null;
			if (starsUsed == 0 && !this.m_isCountering && base.Actor.CurrentFrame == 9)
			{
				preventDamage = true;
				base.Actor.state = new PunchoutAIActor.CheeseHitState();
				((PunchoutPlayerController)base.Actor.Opponent).AddStar();
			}
		}

		// Token: 0x06007DFA RID: 32250 RVA: 0x0032F654 File Offset: 0x0032D854
		public override void Stop()
		{
			base.Stop();
			base.ActorEnemy.TauntCooldownTimer = base.ActorEnemy.TauntCooldown;
			float? targetHealth = this.m_targetHealth;
			if (targetHealth != null)
			{
				base.Actor.Health = this.m_targetHealth.Value;
			}
		}

		// Token: 0x040080F7 RID: 33015
		public float HealAmount = 30f;

		// Token: 0x040080F8 RID: 33016
		private bool m_isCountering;

		// Token: 0x040080F9 RID: 33017
		private float m_startingHealth;

		// Token: 0x040080FA RID: 33018
		private float? m_targetHealth;
	}

	// Token: 0x02001577 RID: 5495
	public class IntroState : PunchoutGameActor.State
	{
		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x06007DFC RID: 32252 RVA: 0x0032F6B0 File Offset: 0x0032D8B0
		public override string AnimName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06007DFD RID: 32253 RVA: 0x0032F6B4 File Offset: 0x0032D8B4
		public override void Start()
		{
			base.Start();
			GameManager.Instance.DungeonMusicController.SwitchToBossMusic("Play_MUS_RatPunch_Intro_01", base.Actor.gameObject);
			base.Actor.Play("intro");
			this.m_state = PunchoutAIActor.IntroState.State.MaybeIntro;
		}

		// Token: 0x06007DFE RID: 32254 RVA: 0x0032F6F4 File Offset: 0x0032D8F4
		public override void Update()
		{
			base.Update();
			if (this.m_state == PunchoutAIActor.IntroState.State.MaybeIntro)
			{
				if (PunchoutController.InTutorial)
				{
					base.Actor.Play("intro_tutorial");
					this.m_state = PunchoutAIActor.IntroState.State.Tutorial;
				}
			}
			else if (this.m_state == PunchoutAIActor.IntroState.State.Tutorial)
			{
				if (!PunchoutController.InTutorial)
				{
					base.Actor.Play("intro_transition");
					this.m_state = PunchoutAIActor.IntroState.State.Transition;
				}
			}
			else if (this.m_state == PunchoutAIActor.IntroState.State.Transition && base.Actor.aiAnimator.IsIdle())
			{
				base.Actor.Play("intro");
				this.m_state = PunchoutAIActor.IntroState.State.Intro;
			}
		}

		// Token: 0x06007DFF RID: 32255 RVA: 0x0032F7A4 File Offset: 0x0032D9A4
		public override void Stop()
		{
			base.Stop();
			GameManager.Instance.DungeonMusicController.SwitchToBossMusic("Play_MUS_RatPunch_Theme_01", base.Actor.gameObject);
		}

		// Token: 0x06007E00 RID: 32256 RVA: 0x0032F7CC File Offset: 0x0032D9CC
		public override bool IsFarAway()
		{
			return !PunchoutController.IsActive || (this.m_state != PunchoutAIActor.IntroState.State.MaybeIntro && this.m_state != PunchoutAIActor.IntroState.State.Intro);
		}

		// Token: 0x06007E01 RID: 32257 RVA: 0x0032F7F4 File Offset: 0x0032D9F4
		public override bool CanBeHit(bool isLeft)
		{
			return PunchoutController.IsActive && (this.m_state == PunchoutAIActor.IntroState.State.MaybeIntro || this.m_state == PunchoutAIActor.IntroState.State.Intro);
		}

		// Token: 0x040080FB RID: 33019
		private PunchoutAIActor.IntroState.State m_state;

		// Token: 0x02001578 RID: 5496
		private enum State
		{
			// Token: 0x040080FD RID: 33021
			MaybeIntro,
			// Token: 0x040080FE RID: 33022
			Tutorial,
			// Token: 0x040080FF RID: 33023
			Transition,
			// Token: 0x04008100 RID: 33024
			Intro
		}
	}

	// Token: 0x02001579 RID: 5497
	public class InstantKnockdownState : PunchoutGameActor.State
	{
		// Token: 0x06007E02 RID: 32258 RVA: 0x0032F81C File Offset: 0x0032DA1C
		public InstantKnockdownState(bool isLeft)
			: base(isLeft)
		{
		}

		// Token: 0x06007E03 RID: 32259 RVA: 0x0032F838 File Offset: 0x0032DA38
		public override void Start()
		{
			base.Actor.Play("knockdown", this.IsLeft);
			this.m_state = PunchoutAIActor.InstantKnockdownState.KnockdownState.Fall;
			base.ActorEnemy.UpdateUI(base.ActorEnemy.Phase + 1);
			base.ActorEnemy.DropKey(this.IsLeft);
			base.Start();
		}

		// Token: 0x06007E04 RID: 32260 RVA: 0x0032F894 File Offset: 0x0032DA94
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (this.m_state == PunchoutAIActor.InstantKnockdownState.KnockdownState.Fall && currentFrame == 10)
			{
				this.m_state = PunchoutAIActor.InstantKnockdownState.KnockdownState.Attack;
				base.Actor.Play("knockdown_cheat", !this.IsLeft);
				return;
			}
			if (this.m_state == PunchoutAIActor.InstantKnockdownState.KnockdownState.Attack && currentFrame == this.DamageFrame && this.CanHitOpponent())
			{
				base.Actor.Opponent.Hit(!this.IsLeft, this.Damage, 0, false);
				return;
			}
		}

		// Token: 0x06007E05 RID: 32261 RVA: 0x0032F924 File Offset: 0x0032DB24
		public bool CanHitOpponent()
		{
			PunchoutGameActor.State state = base.Actor.Opponent.state;
			if (state is PunchoutGameActor.BlockState)
			{
				(state as PunchoutGameActor.BlockState).Bonk();
				return false;
			}
			return !(state is PunchoutGameActor.DuckState);
		}

		// Token: 0x06007E06 RID: 32262 RVA: 0x0032F968 File Offset: 0x0032DB68
		public override void OnAnimationCompleted()
		{
			base.OnAnimationCompleted();
			if (this.m_state == PunchoutAIActor.InstantKnockdownState.KnockdownState.Attack)
			{
				base.ActorEnemy.GoToNextPhase(null, false);
			}
		}

		// Token: 0x06007E07 RID: 32263 RVA: 0x0032F99C File Offset: 0x0032DB9C
		public override bool CanBeHit(bool isLeft)
		{
			return false;
		}

		// Token: 0x06007E08 RID: 32264 RVA: 0x0032F9A0 File Offset: 0x0032DBA0
		public override bool IsFarAway()
		{
			return true;
		}

		// Token: 0x04008101 RID: 33025
		public int DamageFrame = 11;

		// Token: 0x04008102 RID: 33026
		public float Damage = 10f;

		// Token: 0x04008103 RID: 33027
		private PunchoutAIActor.InstantKnockdownState.KnockdownState m_state;

		// Token: 0x0200157A RID: 5498
		private enum KnockdownState
		{
			// Token: 0x04008105 RID: 33029
			None,
			// Token: 0x04008106 RID: 33030
			Fall,
			// Token: 0x04008107 RID: 33031
			Attack
		}
	}

	// Token: 0x0200157B RID: 5499
	public class DeathState : PunchoutGameActor.State
	{
		// Token: 0x06007E09 RID: 32265 RVA: 0x0032F9A4 File Offset: 0x0032DBA4
		public DeathState(bool isLeft, bool killedBySuper)
			: base(isLeft)
		{
			this.m_killedBySuper = killedBySuper;
		}

		// Token: 0x06007E0A RID: 32266 RVA: 0x0032F9B4 File Offset: 0x0032DBB4
		public override void Start()
		{
			base.Start();
			base.Actor.aiAnimator.FacingDirection = (float)((!this.IsLeft) ? 0 : 180);
			base.Actor.aiAnimator.PlayUntilCancelled((!this.m_killedBySuper) ? "die" : "die_super", false, null, -1f, false);
			base.ActorEnemy.DropKey(this.IsLeft);
			if (this.m_killedBySuper)
			{
				base.ActorEnemy.DropKey(this.IsLeft);
			}
			if (base.ActorEnemy.NumTimesTripleStarred >= 3)
			{
				base.ActorEnemy.DropKey(this.IsLeft);
			}
			base.ActorEnemy.DropReward(this.IsLeft, new PickupObject.ItemQuality[]
			{
				PickupObject.ItemQuality.A,
				PickupObject.ItemQuality.S
			});
		}

		// Token: 0x06007E0B RID: 32267 RVA: 0x0032FA90 File Offset: 0x0032DC90
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (this.m_killedBySuper)
			{
				if (currentFrame == 10)
				{
					SpriteOutlineManager.RemoveOutlineFromSprite(base.Actor.sprite, false);
				}
				else if (currentFrame == 18 && !(base.Actor.Opponent.state is PunchoutPlayerController.WinState))
				{
					((PunchoutPlayerController)base.Actor.Opponent).Win();
				}
				else if (currentFrame == 30)
				{
					base.Actor.transform.position += new Vector3(0f, -0.6875f);
				}
			}
			else if (currentFrame == 13 && !(base.Actor.Opponent.state is PunchoutPlayerController.WinState))
			{
				((PunchoutPlayerController)base.Actor.Opponent).Win();
			}
		}

		// Token: 0x06007E0C RID: 32268 RVA: 0x0032FB78 File Offset: 0x0032DD78
		public override bool CanBeHit(bool isLeft)
		{
			return false;
		}

		// Token: 0x06007E0D RID: 32269 RVA: 0x0032FB7C File Offset: 0x0032DD7C
		public override bool IsFarAway()
		{
			return true;
		}

		// Token: 0x04008108 RID: 33032
		private bool m_killedBySuper;
	}

	// Token: 0x0200157C RID: 5500
	public class WinState : PunchoutGameActor.State
	{
		// Token: 0x06007E0F RID: 32271 RVA: 0x0032FB88 File Offset: 0x0032DD88
		public override void Start()
		{
			base.Start();
			base.Actor.aiAnimator.PlayUntilCancelled("win", false, null, -1f, false);
		}

		// Token: 0x06007E10 RID: 32272 RVA: 0x0032FBB0 File Offset: 0x0032DDB0
		public override bool CanBeHit(bool isLeft)
		{
			return false;
		}
	}

	// Token: 0x0200157D RID: 5501
	public class EscapeState : PunchoutGameActor.State
	{
		// Token: 0x06007E12 RID: 32274 RVA: 0x0032FBBC File Offset: 0x0032DDBC
		public override void Start()
		{
			base.Start();
			base.Actor.aiAnimator.FacingDirection = -90f;
			base.Actor.aiAnimator.PlayUntilCancelled("escape", false, null, -1f, false);
		}

		// Token: 0x06007E13 RID: 32275 RVA: 0x0032FBF8 File Offset: 0x0032DDF8
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (currentFrame == 27)
			{
				base.ActorEnemy.DoFadeOut();
			}
		}

		// Token: 0x06007E14 RID: 32276 RVA: 0x0032FC14 File Offset: 0x0032DE14
		public override bool CanBeHit(bool isLeft)
		{
			return false;
		}

		// Token: 0x06007E15 RID: 32277 RVA: 0x0032FC18 File Offset: 0x0032DE18
		public override bool IsFarAway()
		{
			return true;
		}
	}

	// Token: 0x0200157E RID: 5502
	public class PhaseTransitionState : PunchoutGameActor.State
	{
		// Token: 0x06007E16 RID: 32278 RVA: 0x0032FC1C File Offset: 0x0032DE1C
		public PhaseTransitionState(bool isLeft, float startingHealth)
			: base(isLeft)
		{
			this.m_startingHealth = startingHealth;
		}

		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x06007E17 RID: 32279 RVA: 0x0032FC2C File Offset: 0x0032DE2C
		public override string AnimName
		{
			get
			{
				return "transition";
			}
		}

		// Token: 0x06007E18 RID: 32280 RVA: 0x0032FC34 File Offset: 0x0032DE34
		public override void Start()
		{
			base.Start();
			base.ActorEnemy.UpdateUI(base.ActorEnemy.Phase + 1);
			if (base.ActorEnemy.Phase == 0)
			{
				GameManager.Instance.DungeonMusicController.SwitchToBossMusic("Play_MUS_RatPunch_Transition_01", base.Actor.gameObject);
			}
			else
			{
				GameManager.Instance.DungeonMusicController.SwitchToBossMusic("Play_MUS_RatPunch_Transition_02", base.Actor.gameObject);
			}
		}

		// Token: 0x06007E19 RID: 32281 RVA: 0x0032FCB4 File Offset: 0x0032DEB4
		public override void Update()
		{
			base.Update();
			if (base.Actor.CurrentFrame >= 16)
			{
				base.Actor.Health = Mathf.Lerp(this.m_startingHealth, 100f, Mathf.Clamp01((base.Actor.CurrentFrameFloat - 16f) / 8f));
			}
		}

		// Token: 0x06007E1A RID: 32282 RVA: 0x0032FD10 File Offset: 0x0032DF10
		public override void OnFrame(int currentFrame)
		{
			base.OnFrame(currentFrame);
			if (currentFrame == 16)
			{
				base.ActorEnemy.DoHealSuck(new Vector3((float)((!this.IsLeft) ? (-1) : 1), 3.625f, -4.3125f));
			}
			else if (currentFrame == 17 || currentFrame == 21)
			{
				base.Actor.PulseColor(base.ActorEnemy.RedPulseColor, 3f);
			}
		}

		// Token: 0x06007E1B RID: 32283 RVA: 0x0032FD8C File Offset: 0x0032DF8C
		public override bool CanBeHit(bool isLeft)
		{
			return false;
		}

		// Token: 0x06007E1C RID: 32284 RVA: 0x0032FD90 File Offset: 0x0032DF90
		public override bool IsFarAway()
		{
			return true;
		}

		// Token: 0x06007E1D RID: 32285 RVA: 0x0032FD94 File Offset: 0x0032DF94
		public override void Stop()
		{
			base.Stop();
			base.Actor.Health = 100f;
			base.ActorEnemy.Phase = (base.ActorEnemy.Phase + 1) % 3;
			base.ActorEnemy.SuccessfulHeals = 0;
			if (base.ActorEnemy.Phase == 1)
			{
				GameManager.Instance.DungeonMusicController.SwitchToBossMusic("Play_MUS_RatPunch_Theme_02", base.Actor.gameObject);
			}
			else
			{
				GameManager.Instance.DungeonMusicController.SwitchToBossMusic("Play_MUS_RatPunch_Theme_03", base.Actor.gameObject);
			}
		}

		// Token: 0x04008109 RID: 33033
		private float m_startingHealth;
	}

	// Token: 0x0200157F RID: 5503
	public new class HitState : PunchoutGameActor.State
	{
		// Token: 0x06007E1E RID: 32286 RVA: 0x0032FE34 File Offset: 0x0032E034
		public HitState(bool isLeft, int remainingHits)
			: base(isLeft)
		{
			this.m_maxHits = remainingHits;
		}

		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x06007E1F RID: 32287 RVA: 0x0032FE54 File Offset: 0x0032E054
		public override string AnimName
		{
			get
			{
				return "hit";
			}
		}

		// Token: 0x06007E20 RID: 32288 RVA: 0x0032FE5C File Offset: 0x0032E05C
		public override void Update()
		{
			base.Update();
			if (base.Actor.Opponent.state is PunchoutGameActor.BasicAttackState && base.Actor.Opponent.state != base.Actor.LastHitBy)
			{
				if (this.m_isAlternating && (base.Actor.Opponent.state as PunchoutGameActor.BasicAttackState).IsLeft != this.IsLeft)
				{
					if (this.m_hits + 1 > this.m_maxHits * 2)
					{
						base.Actor.state = new PunchoutGameActor.BlockState();
					}
				}
				else if (this.m_hits + 1 > this.m_maxHits)
				{
					base.Actor.state = new PunchoutGameActor.BlockState();
				}
			}
		}

		// Token: 0x06007E21 RID: 32289 RVA: 0x0032FF28 File Offset: 0x0032E128
		public void HitAgain(bool newIsLeft)
		{
			this.m_hits++;
			if (newIsLeft == this.IsLeft)
			{
				this.m_isAlternating = false;
			}
			this.IsLeft = newIsLeft;
			base.Actor.Play("hit", this.IsLeft);
		}

		// Token: 0x0400810A RID: 33034
		private int m_maxHits;

		// Token: 0x0400810B RID: 33035
		private int m_hits = 1;

		// Token: 0x0400810C RID: 33036
		private bool m_isAlternating = true;
	}

	// Token: 0x02001580 RID: 5504
	public class SuperHitState : PunchoutGameActor.State
	{
		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x06007E23 RID: 32291 RVA: 0x0032FF70 File Offset: 0x0032E170
		public override string AnimName
		{
			get
			{
				return "hit_super";
			}
		}

		// Token: 0x06007E24 RID: 32292 RVA: 0x0032FF78 File Offset: 0x0032E178
		public override bool CanBeHit(bool isLeft)
		{
			return false;
		}
	}

	// Token: 0x02001581 RID: 5505
	public class CheeseHitState : PunchoutGameActor.State
	{
		// Token: 0x1700129E RID: 4766
		// (get) Token: 0x06007E26 RID: 32294 RVA: 0x0032FF84 File Offset: 0x0032E184
		public override string AnimName
		{
			get
			{
				return "cheese_taunt_hit";
			}
		}

		// Token: 0x06007E27 RID: 32295 RVA: 0x0032FF8C File Offset: 0x0032E18C
		public override bool CanBeHit(bool isLeft)
		{
			return false;
		}
	}

	// Token: 0x02001582 RID: 5506
	public class DazeState : PunchoutGameActor.State
	{
		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x06007E29 RID: 32297 RVA: 0x0032FF98 File Offset: 0x0032E198
		public override string AnimName
		{
			get
			{
				return "daze";
			}
		}

		// Token: 0x06007E2A RID: 32298 RVA: 0x0032FFA0 File Offset: 0x0032E1A0
		public override void Stop()
		{
			base.Stop();
			base.Actor.aiAnimator.EndAnimationIf("daze");
		}
	}

	// Token: 0x02001583 RID: 5507
	public class TailDazeState : PunchoutGameActor.State
	{
		// Token: 0x170012A0 RID: 4768
		// (get) Token: 0x06007E2C RID: 32300 RVA: 0x0032FFC8 File Offset: 0x0032E1C8
		public override string AnimName
		{
			get
			{
				return "tail_whip_hit";
			}
		}

		// Token: 0x06007E2D RID: 32301 RVA: 0x0032FFD0 File Offset: 0x0032E1D0
		public override bool CanBeHit(bool isLeft)
		{
			return false;
		}

		// Token: 0x06007E2E RID: 32302 RVA: 0x0032FFD4 File Offset: 0x0032E1D4
		public override void OnAnimationCompleted()
		{
			base.OnAnimationCompleted();
			base.Actor.state = new PunchoutAIActor.DazeState();
		}
	}
}
