using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015FC RID: 5628
public class PlayerSpaceshipController : PlayerController
{
	// Token: 0x17001382 RID: 4994
	// (get) Token: 0x060082BA RID: 33466 RVA: 0x00356704 File Offset: 0x00354904
	public override bool IsFlying
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17001383 RID: 4995
	// (get) Token: 0x060082BB RID: 33467 RVA: 0x00356708 File Offset: 0x00354908
	protected override bool CanDodgeRollWhileFlying
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060082BC RID: 33468 RVA: 0x0035670C File Offset: 0x0035490C
	public override void Start()
	{
		base.Start();
		if (this.PaletteTex != null)
		{
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.SetTexture("_PaletteTex", this.PaletteTex);
		}
		base.ToggleHandRenderers(false, "ships don't have hands");
		base.sprite.IsPerpendicular = false;
		base.sprite.HeightOffGround = 3f;
		base.sprite.UpdateZDepth();
	}

	// Token: 0x060082BD RID: 33469 RVA: 0x00356790 File Offset: 0x00354990
	public override void Update()
	{
		base.Update();
		if (!base.AcceptingNonMotionInput)
		{
			this.m_isFiring = false;
			this.m_shouldContinueFiring = false;
		}
		if (!base.IsDodgeRolling)
		{
			int num = BraveMathCollege.AngleToOctant(this.m_aimAngle);
			float num2 = (float)num * -45f;
			float aimAngle = this.m_aimAngle;
			float num3 = aimAngle - num2;
			num3 = num3.Quantize(10f);
			base.sprite.transform.parent.rotation = Quaternion.Euler(0f, 0f, -90f + num3);
		}
		if (this.m_isFiring && this.m_fireCooldown <= 0f)
		{
			this.FireProjectiles();
			this.m_fireCooldown = this.LaserACooldown;
		}
		this.m_missileCooldown -= BraveTime.DeltaTime;
		this.m_fireCooldown -= BraveTime.DeltaTime;
	}

	// Token: 0x060082BE RID: 33470 RVA: 0x00356874 File Offset: 0x00354A74
	protected void FireMissileVolley()
	{
		if (this.m_missileCooldown <= 0f)
		{
			for (int i = 0; i < this.LaserShootPoints.Count; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					this.FireBullet(this.LaserShootPoints[i], Quaternion.Euler(0f, 0f, -90f + this.m_aimAngle + Mathf.Lerp(-20f, 20f, (float)j / 4f)) * Vector2.up, "missile");
				}
			}
			this.m_missileCooldown = this.MissileCooldown;
			if (base.CurrentItem)
			{
				float num = -1f;
				base.CurrentItem.timeCooldown = this.MissileCooldown;
				base.CurrentItem.Use(this, out num);
			}
		}
	}

	// Token: 0x060082BF RID: 33471 RVA: 0x00356960 File Offset: 0x00354B60
	protected override void CheckSpawnEmergencyCrate()
	{
	}

	// Token: 0x060082C0 RID: 33472 RVA: 0x00356964 File Offset: 0x00354B64
	protected void FireProjectiles()
	{
		for (int i = 0; i < this.LaserShootPoints.Count; i++)
		{
			this.FireBullet(this.LaserShootPoints[i], Quaternion.Euler(0f, 0f, -90f + this.m_aimAngle) * Vector2.up, "default");
		}
	}

	// Token: 0x060082C1 RID: 33473 RVA: 0x003569D4 File Offset: 0x00354BD4
	private void FireBullet(Transform shootPoint, Vector2 dirVec, string bulletType)
	{
		GameObject gameObject = base.bulletBank.CreateProjectileFromBank(shootPoint.position, BraveMathCollege.Atan2Degrees(dirVec.normalized), bulletType, null, false, true, false);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Owner = this;
		component.Shooter = base.specRigidbody;
		component.collidesWithPlayer = false;
		component.specRigidbody.RegisterSpecificCollisionException(base.specRigidbody);
	}

	// Token: 0x060082C2 RID: 33474 RVA: 0x00356A3C File Offset: 0x00354C3C
	protected override void Die(Vector2 finalDamageDirection)
	{
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER || GameManager.Instance.NumberOfLivingPlayers == 0)
		{
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST)
			{
				GameManager.Instance.platformInterface.AchievementUnlock(Achievement.DIE_IN_PAST, 0);
			}
			base.CurrentInputState = PlayerInputState.NoInput;
			if (this.CurrentGun)
			{
				this.CurrentGun.CeaseAttack(true, null);
			}
			Transform transform = GameManager.Instance.MainCameraController.transform;
			Vector3 position = transform.position;
			GameManager.Instance.MainCameraController.OverridePosition = position;
			GameManager.Instance.StartCoroutine(this.HandleDelayedEndGame());
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x060082C3 RID: 33475 RVA: 0x00356AF4 File Offset: 0x00354CF4
	private IEnumerator HandleDelayedEndGame()
	{
		base.CurrentInputState = PlayerInputState.NoInput;
		yield return new WaitForSeconds(0.25f);
		base.HandleDeathPhotography();
		float ela = 0f;
		float dura = 4f;
		while (ela < dura)
		{
			ela += BraveTime.DeltaTime;
			yield return null;
		}
		Pixelator.Instance.CustomFade(0.6f, 0f, Color.white, Color.black, 0.1f, 0.5f);
		Pixelator.Instance.LerpToLetterbox(0.35f, 0.8f);
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.NUMBER_DEATHS, 1f);
		AmmonomiconDeathPageController.LastKilledPlayerPrimary = base.IsPrimaryPlayer;
		GameManager.Instance.DoGameOver(base.healthHaver.lastIncurredDamageSource);
		yield break;
	}

	// Token: 0x060082C4 RID: 33476 RVA: 0x00356B10 File Offset: 0x00354D10
	public override void ResurrectFromBossKill()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		Chest.ToggleCoopChests(false);
		base.healthHaver.FullHeal();
	}

	// Token: 0x060082C5 RID: 33477 RVA: 0x00356B40 File Offset: 0x00354D40
	protected override string GetBaseAnimationName(Vector2 v, float unusedGunAngle, bool invertThresholds = false, bool forceTwoHands = false)
	{
		string text = string.Empty;
		switch (BraveMathCollege.AngleToOctant(this.m_aimAngle))
		{
		case 0:
			text = ((!this.m_isFiring) ? "idle_n" : "fire_n");
			break;
		case 1:
			text = ((!this.m_isFiring) ? "idle_ne" : "fire_ne");
			break;
		case 2:
			text = ((!this.m_isFiring) ? "idle_e" : "fire_e");
			break;
		case 3:
			text = ((!this.m_isFiring) ? "idle_se" : "fire_se");
			break;
		case 4:
			text = ((!this.m_isFiring) ? "idle_s" : "fire_s");
			break;
		case 5:
			text = ((!this.m_isFiring) ? "idle_sw" : "fire_sw");
			break;
		case 6:
			text = ((!this.m_isFiring) ? "idle_w" : "fire_w");
			break;
		case 7:
			text = ((!this.m_isFiring) ? "idle_nw" : "fire_nw");
			break;
		}
		return text;
	}

	// Token: 0x060082C6 RID: 33478 RVA: 0x00356C8C File Offset: 0x00354E8C
	protected override void PlayDodgeRollAnimation(Vector2 direction)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = null;
		direction.Normalize();
		if (this.m_dodgeRollState != PlayerController.DodgeRollState.PreRollDelay)
		{
			float num = direction.ToAngle();
			int num2 = BraveMathCollege.AngleToOctant(this.m_aimAngle);
			float num3 = BraveMathCollege.ClampAngle180(num - this.m_aimAngle);
			string text = ((num3 < 0f) ? "dodgeroll_right_" : "dodgeroll_left_");
			switch (num2)
			{
			case 0:
				text += "n";
				break;
			case 1:
				text += "ne";
				break;
			case 2:
				text += "e";
				break;
			case 3:
				text += "se";
				break;
			case 4:
				text += "s";
				break;
			case 5:
				text += "sw";
				break;
			case 6:
				text += "w";
				break;
			case 7:
				text += "nw";
				break;
			}
			tk2dSpriteAnimationClip = base.spriteAnimator.GetClipByName(text);
		}
		if (tk2dSpriteAnimationClip != null)
		{
			float num4 = (float)tk2dSpriteAnimationClip.frames.Length / this.rollStats.GetModifiedTime(this);
			base.spriteAnimator.Play(tk2dSpriteAnimationClip, 0f, num4, false);
			this.m_handlingQueuedAnimation = true;
		}
	}

	// Token: 0x060082C7 RID: 33479 RVA: 0x00356DFC File Offset: 0x00354FFC
	protected override void HandleFlipping(float gunAngle)
	{
	}

	// Token: 0x060082C8 RID: 33480 RVA: 0x00356E00 File Offset: 0x00355000
	protected override Vector2 HandlePlayerInput()
	{
		Vector2 vector = Vector2.zero;
		if (base.CurrentInputState != PlayerInputState.NoMovement)
		{
			vector = base.AdjustInputVector(this.m_activeActions.Move.Vector, BraveInput.MagnetAngles.movementCardinal, BraveInput.MagnetAngles.movementOrdinal);
		}
		if (vector.magnitude > 1f)
		{
			vector.Normalize();
		}
		base.HandleStartDodgeRoll(vector);
		CollisionData collisionData = null;
		if (vector.x > 0.01f && PhysicsEngine.Instance.RigidbodyCast(base.specRigidbody, IntVector2.Right, out collisionData, true, false, null, false))
		{
			vector.x = 0f;
		}
		CollisionData.Pool.Free(ref collisionData);
		if (vector.x < -0.01f && PhysicsEngine.Instance.RigidbodyCast(base.specRigidbody, IntVector2.Left, out collisionData, true, false, null, false))
		{
			vector.x = 0f;
		}
		CollisionData.Pool.Free(ref collisionData);
		if (vector.y > 0.01f && PhysicsEngine.Instance.RigidbodyCast(base.specRigidbody, IntVector2.Up, out collisionData, true, false, null, false))
		{
			vector.y = 0f;
		}
		CollisionData.Pool.Free(ref collisionData);
		if (vector.y < -0.01f && PhysicsEngine.Instance.RigidbodyCast(base.specRigidbody, IntVector2.Down, out collisionData, true, false, null, false))
		{
			vector.y = 0f;
		}
		CollisionData.Pool.Free(ref collisionData);
		if (base.AcceptingNonMotionInput)
		{
			GameOptions.ControllerBlankControl controllerBlankControl = ((!base.IsPrimaryPlayer) ? GameManager.Options.additionalBlankControlTwo : GameManager.Options.additionalBlankControl);
			bool flag = controllerBlankControl == GameOptions.ControllerBlankControl.BOTH_STICKS_DOWN && this.m_activeActions.CheckBothSticksButton();
			if (Time.timeScale > 0f && (this.m_activeActions.BlankAction.WasPressed || flag))
			{
				base.DoConsumableBlank();
			}
			if (BraveInput.GetInstanceForPlayer(this.PlayerIDX).GetButtonDown(GungeonActions.GungeonActionType.UseItem))
			{
				this.FireMissileVolley();
				BraveInput.GetInstanceForPlayer(this.PlayerIDX).ConsumeButtonDown(GungeonActions.GungeonActionType.UseItem);
			}
		}
		if (base.AcceptingNonMotionInput || base.CurrentInputState == PlayerInputState.FoyerInputOnly)
		{
			Vector2 vector2 = base.DetermineAimPointInWorld();
			this.m_aimAngle = BraveMathCollege.Atan2Degrees(vector2 - base.CenterPosition);
		}
		if (this.m_isFiring && !BraveInput.GetInstanceForPlayer(this.PlayerIDX).GetButton(GungeonActions.GungeonActionType.Shoot))
		{
			this.m_isFiring = false;
			this.m_shouldContinueFiring = false;
		}
		if (base.SuppressThisClick)
		{
			while (BraveInput.GetInstanceForPlayer(this.PlayerIDX).GetButtonDown(GungeonActions.GungeonActionType.Shoot))
			{
				BraveInput.GetInstanceForPlayer(this.PlayerIDX).ConsumeButtonDown(GungeonActions.GungeonActionType.Shoot);
				if (BraveInput.GetInstanceForPlayer(this.PlayerIDX).GetButtonUp(GungeonActions.GungeonActionType.Shoot))
				{
					BraveInput.GetInstanceForPlayer(this.PlayerIDX).ConsumeButtonUp(GungeonActions.GungeonActionType.Shoot);
				}
			}
			if (!BraveInput.GetInstanceForPlayer(this.PlayerIDX).GetButton(GungeonActions.GungeonActionType.Shoot))
			{
				base.SuppressThisClick = false;
			}
		}
		else if (base.m_CanAttack && BraveInput.GetInstanceForPlayer(this.PlayerIDX).GetButtonDown(GungeonActions.GungeonActionType.Shoot))
		{
			bool flag2 = false;
			this.m_isFiring = true;
			flag2 |= true;
			this.m_shouldContinueFiring = true;
			if (flag2)
			{
				BraveInput.GetInstanceForPlayer(this.PlayerIDX).ConsumeButtonDown(GungeonActions.GungeonActionType.Shoot);
			}
		}
		else if (BraveInput.GetInstanceForPlayer(this.PlayerIDX).GetButtonUp(GungeonActions.GungeonActionType.Shoot))
		{
			this.m_isFiring = false;
			this.m_shouldContinueFiring = false;
			BraveInput.GetInstanceForPlayer(this.PlayerIDX).ConsumeButtonUp(GungeonActions.GungeonActionType.Shoot);
		}
		return vector;
	}

	// Token: 0x040085C2 RID: 34242
	public Texture2D PaletteTex;

	// Token: 0x040085C3 RID: 34243
	public List<Transform> LaserShootPoints;

	// Token: 0x040085C4 RID: 34244
	public tk2dSpriteAnimation TimefallCorpseLibrary;

	// Token: 0x040085C5 RID: 34245
	[Header("Spaceship Controls")]
	public float LaserACooldown = 0.15f;

	// Token: 0x040085C6 RID: 34246
	public float MissileCooldown = 1f;

	// Token: 0x040085C7 RID: 34247
	private float m_aimAngle;

	// Token: 0x040085C8 RID: 34248
	private float m_fireCooldown;

	// Token: 0x040085C9 RID: 34249
	private float m_missileCooldown;

	// Token: 0x040085CA RID: 34250
	private bool m_isFiring;
}
