using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020014E1 RID: 5345
public class WingsItem : PassiveItem
{
	// Token: 0x06007992 RID: 31122 RVA: 0x0030A230 File Offset: 0x00308430
	private Vector2 GetLocalOffsetForCharacter(PlayableCharacters character)
	{
		switch (character)
		{
		case PlayableCharacters.Pilot:
			return new Vector2(-0.5625f, -0.5f);
		case PlayableCharacters.Convict:
			return new Vector2(-0.625f, -0.5f);
		case PlayableCharacters.Robot:
			return new Vector2(-0.5625f, -0.5f);
		case PlayableCharacters.Soldier:
			return new Vector2(-0.5f, -0.5f);
		case PlayableCharacters.Guide:
			return new Vector2(-0.5625f, -0.5f);
		case PlayableCharacters.Bullet:
			return new Vector2(-0.5625f, -0.46875f);
		}
		return new Vector2(-0.5625f, -0.5f);
	}

	// Token: 0x06007993 RID: 31123 RVA: 0x0030A2DC File Offset: 0x003084DC
	protected override void Update()
	{
		base.Update();
		if (this.m_owner != null && this.m_pickedUp)
		{
			this.m_radialBurstCooldown -= BraveTime.DeltaTime;
			if (this.IsCatThrone && this.wasRolling && !this.m_owner.IsDodgeRolling)
			{
				this.m_owner.IsVisible = true;
				this.wasRolling = false;
			}
			if (this.m_isCurrentlyActive)
			{
				if (this.m_owner.IsFalling)
				{
					this.m_hiddenForFall = true;
					this.instanceWingsSprite.renderer.enabled = false;
				}
				else
				{
					if (this.m_hiddenForFall)
					{
						this.m_hiddenForFall = false;
						this.instanceWingsSprite.renderer.enabled = true;
					}
					string text = this.animPrefix + this.m_owner.GetBaseAnimationSuffix(this.usesCardinalAnimations);
					if (!this.instanceWingsSprite.spriteAnimator.IsPlaying(text) && (!this.IsCatThrone || !this.m_owner.IsDodgeRolling))
					{
						this.instanceWingsSprite.spriteAnimator.Play(text);
					}
					if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
					{
						this.DisengageEffect(this.m_owner);
					}
				}
			}
			else if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.END_TIMES)
			{
				this.EngageEffect(this.m_owner);
			}
			if (this.IsCatThrone && this.m_owner && this.m_owner.HasActiveBonusSynergy(CustomSynergyType.TRUE_CAT_KING, false) && this.m_owner.CurrentRoom != null)
			{
				List<AIActor> activeEnemies = this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				this.ProcessEnemies(activeEnemies);
			}
		}
	}

	// Token: 0x06007994 RID: 31124 RVA: 0x0030A4A8 File Offset: 0x003086A8
	private void DoGoop()
	{
		DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.RollGoop).AddGoopCircle(this.m_owner.specRigidbody.UnitCenter, this.RollGoopRadius, -1, false, -1);
	}

	// Token: 0x06007995 RID: 31125 RVA: 0x0030A4D4 File Offset: 0x003086D4
	private void OnRollFrame(PlayerController obj)
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			return;
		}
		if (this.GoopsOnRoll)
		{
			this.DoGoop();
		}
		if (this.IsCatThrone)
		{
			this.wasRolling = true;
			obj.IsVisible = false;
			this.instanceWingsSprite.renderer.enabled = true;
			if (!this.instanceWingsSprite.spriteAnimator.IsPlaying("cat_throne_spin"))
			{
				this.instanceWingsSprite.spriteAnimator.Play("cat_throne_spin");
			}
		}
	}

	// Token: 0x06007996 RID: 31126 RVA: 0x0030A55C File Offset: 0x0030875C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
		{
			PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
		}
		if (!PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player].Add(base.GetType(), 1);
		}
		else
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = PassiveItem.ActiveFlagItems[player][base.GetType()] + 1;
		}
		if (this.GoopsOnRoll || this.IsCatThrone)
		{
			player.OnIsRolling += this.OnRollFrame;
		}
		if (this.DoesRadialBurstOnDodgeRoll)
		{
			player.OnRollStarted += this.HandleRollStarted;
		}
		this.EngageEffect(player);
		base.Pickup(player);
	}

	// Token: 0x06007997 RID: 31127 RVA: 0x0030A650 File Offset: 0x00308850
	private void HandleRollStarted(PlayerController p, Vector2 rollDirection)
	{
		if (this.DoesRadialBurstOnDodgeRoll && this.m_radialBurstCooldown <= 0f)
		{
			this.m_radialBurstCooldown = this.RadialBurstCooldown;
			this.RadialBurstOnDodgeRoll.DoBurst(p, null, new Vector2?(Vector2.up * 0.625f));
		}
	}

	// Token: 0x06007998 RID: 31128 RVA: 0x0030A6B0 File Offset: 0x003088B0
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		if (PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
			}
		}
		player.OnIsRolling -= this.OnRollFrame;
		player.OnRollStarted -= this.HandleRollStarted;
		this.DisengageEffect(player);
		debrisObject.GetComponent<WingsItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007999 RID: 31129 RVA: 0x0030A780 File Offset: 0x00308980
	protected override void OnDestroy()
	{
		if (this.m_pickedUp)
		{
			if (PassiveItem.ActiveFlagItems.ContainsKey(this.m_owner) && PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
			{
				PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] - 1);
				if (PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] == 0)
				{
					PassiveItem.ActiveFlagItems[this.m_owner].Remove(base.GetType());
				}
			}
			this.m_owner.OnIsRolling -= this.OnRollFrame;
			this.m_owner.OnRollStarted -= this.HandleRollStarted;
			this.DisengageEffect(this.m_owner);
		}
		base.OnDestroy();
	}

	// Token: 0x0600799A RID: 31130 RVA: 0x0030A888 File Offset: 0x00308A88
	protected void EngageEffect(PlayerController user)
	{
		if (Dungeon.IsGenerating)
		{
			return;
		}
		if (!user || !user.sprite)
		{
			return;
		}
		if (!user.sprite.GetComponent<tk2dSpriteAttachPoint>())
		{
			return;
		}
		this.m_isCurrentlyActive = true;
		user.SetIsFlying(true, "wings", true, false);
		this.instanceWings = user.RegisterAttachedObject(this.prefabToAttachToPlayer, "jetpack", 0.1f);
		this.instanceWingsSprite = this.instanceWings.GetComponent<tk2dSprite>();
		if (!this.instanceWingsSprite)
		{
			this.instanceWingsSprite = this.instanceWings.GetComponentInChildren<tk2dSprite>();
		}
		if (this.usesCardinalAnimations)
		{
			this.instanceWingsSprite.transform.localPosition = this.GetLocalOffsetForCharacter(user.characterIdentity).ToVector3ZUp(0f);
		}
	}

	// Token: 0x0600799B RID: 31131 RVA: 0x0030A968 File Offset: 0x00308B68
	private void ProcessEnemies(List<AIActor> enemies)
	{
		if (enemies == null)
		{
			return;
		}
		for (int i = 0; i < enemies.Count; i++)
		{
			if (enemies[i] && enemies[i].GetEffect(this.CatCharmEffect.effectIdentifier) == null && this.CatThroneCharmGuids.Contains(enemies[i].EnemyGuid))
			{
				enemies[i].ApplyEffect(this.CatCharmEffect, 1f, null);
			}
		}
	}

	// Token: 0x0600799C RID: 31132 RVA: 0x0030A9F4 File Offset: 0x00308BF4
	protected void DisengageEffect(PlayerController user)
	{
		this.m_isCurrentlyActive = false;
		user.SetIsFlying(false, "wings", true, false);
		user.DeregisterAttachedObject(this.instanceWings, true);
		this.instanceWingsSprite = null;
		if (this.IsCatThrone && this.wasRolling)
		{
			user.IsVisible = true;
			this.wasRolling = false;
		}
	}

	// Token: 0x04007C0D RID: 31757
	public GameObject prefabToAttachToPlayer;

	// Token: 0x04007C0E RID: 31758
	public string animPrefix = "white_wing";

	// Token: 0x04007C0F RID: 31759
	public bool usesCardinalAnimations;

	// Token: 0x04007C10 RID: 31760
	public bool GoopsOnRoll;

	// Token: 0x04007C11 RID: 31761
	public GoopDefinition RollGoop;

	// Token: 0x04007C12 RID: 31762
	public float RollGoopRadius = 1f;

	// Token: 0x04007C13 RID: 31763
	public bool DoesRadialBurstOnDodgeRoll;

	// Token: 0x04007C14 RID: 31764
	public RadialBurstInterface RadialBurstOnDodgeRoll;

	// Token: 0x04007C15 RID: 31765
	public bool IsCatThrone;

	// Token: 0x04007C16 RID: 31766
	[EnemyIdentifier]
	public List<string> CatThroneCharmGuids;

	// Token: 0x04007C17 RID: 31767
	public float RadialBurstCooldown = 2f;

	// Token: 0x04007C18 RID: 31768
	private float m_radialBurstCooldown;

	// Token: 0x04007C19 RID: 31769
	public GameActorCharmEffect CatCharmEffect;

	// Token: 0x04007C1A RID: 31770
	private GameObject instanceWings;

	// Token: 0x04007C1B RID: 31771
	private tk2dSprite instanceWingsSprite;

	// Token: 0x04007C1C RID: 31772
	private bool m_isCurrentlyActive;

	// Token: 0x04007C1D RID: 31773
	private bool m_hiddenForFall;

	// Token: 0x04007C1E RID: 31774
	private bool wasRolling;
}
