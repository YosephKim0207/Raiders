using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020014D3 RID: 5331
public class TargetedAttackPlayerItem : PlayerItem
{
	// Token: 0x0600792D RID: 31021 RVA: 0x00307A30 File Offset: 0x00305C30
	public override void Update()
	{
		base.Update();
		if (base.IsCurrentlyActive)
		{
			if (this.m_extantReticleQuad)
			{
				this.UpdateReticlePosition();
			}
			else
			{
				base.IsCurrentlyActive = false;
				base.ClearCooldowns();
			}
		}
	}

	// Token: 0x0600792E RID: 31022 RVA: 0x00307A6C File Offset: 0x00305C6C
	private void UpdateReticlePosition()
	{
		if (BraveInput.GetInstanceForPlayer(this.m_currentUser.PlayerIDX).IsKeyboardAndMouse(false))
		{
			Vector2 vector = this.m_currentUser.unadjustedAimPoint.XY();
			Vector2 vector2 = vector - this.m_extantReticleQuad.GetBounds().extents.XY();
			this.m_extantReticleQuad.transform.position = vector2;
		}
		else
		{
			BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(this.m_currentUser.PlayerIDX);
			Vector2 vector3 = this.m_currentUser.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
			vector3 += instanceForPlayer.ActiveActions.Aim.Vector * 8f * BraveTime.DeltaTime;
			this.m_currentAngle = BraveMathCollege.Atan2Degrees(vector3 - this.m_currentUser.CenterPosition);
			this.m_currentDistance = Vector2.Distance(vector3, this.m_currentUser.CenterPosition);
			this.m_currentDistance = Mathf.Min(this.m_currentDistance, this.maxDistance);
			vector3 = this.m_currentUser.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
			Vector2 vector4 = vector3 - this.m_extantReticleQuad.GetBounds().extents.XY();
			this.m_extantReticleQuad.transform.position = vector4;
		}
	}

	// Token: 0x0600792F RID: 31023 RVA: 0x00307C30 File Offset: 0x00305E30
	protected override void OnPreDrop(PlayerController user)
	{
		base.OnPreDrop(user);
		if (this.m_extantReticleQuad)
		{
			UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
		}
	}

	// Token: 0x06007930 RID: 31024 RVA: 0x00307C5C File Offset: 0x00305E5C
	protected override void DoEffect(PlayerController user)
	{
		base.IsCurrentlyActive = true;
		this.m_currentUser = user;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.reticleQuad);
		this.m_extantReticleQuad = gameObject.GetComponent<tk2dBaseSprite>();
		this.m_currentAngle = BraveMathCollege.Atan2Degrees(this.m_currentUser.unadjustedAimPoint.XY() - this.m_currentUser.CenterPosition);
		this.m_currentDistance = 5f;
		this.UpdateReticlePosition();
	}

	// Token: 0x06007931 RID: 31025 RVA: 0x00307CCC File Offset: 0x00305ECC
	protected override void DoActiveEffect(PlayerController user)
	{
		Vector2 worldCenter = this.m_extantReticleQuad.WorldCenter;
		if (this.m_extantReticleQuad)
		{
			UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
		}
		base.IsCurrentlyActive = true;
		if (this.doesStrike)
		{
			this.DoStrike(worldCenter);
		}
		if (this.doesGoop)
		{
			this.HandleEngoopening(worldCenter, this.goopRadius);
		}
		if (this.itemName == "Nuke" && user && user.HasActiveBonusSynergy(CustomSynergyType.MELTDOWN, false))
		{
			user.CurrentGun.GainAmmo(100);
			AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", base.gameObject);
		}
		if (this.TransmogrifySurvivors && user && user.CurrentRoom != null)
		{
			List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				int count = activeEnemies.Count;
				for (int i = 0; i < count; i++)
				{
					if (activeEnemies[i] && activeEnemies[i].HasBeenEngaged && activeEnemies[i].healthHaver && activeEnemies[i].IsNormalEnemy && !activeEnemies[i].healthHaver.IsDead && !activeEnemies[i].healthHaver.IsBoss && !activeEnemies[i].IsTransmogrified && UnityEngine.Random.value < this.TransmogrifyChance && Vector2.Distance(activeEnemies[i].CenterPosition, worldCenter) < this.TransmogrifyRadius)
					{
						activeEnemies[i].Transmogrify(EnemyDatabase.GetOrLoadByGuid(this.TransmogrifyTargetGuid), null);
					}
				}
			}
		}
		if (this.DoScreenFlash)
		{
			Pixelator.Instance.FadeToColor(this.FlashFadetime, Color.white, true, this.FlashHoldtime);
			StickyFrictionManager.Instance.RegisterCustomStickyFriction(0.15f, 1f, false, false);
		}
		base.IsCurrentlyActive = false;
	}

	// Token: 0x06007932 RID: 31026 RVA: 0x00307EE4 File Offset: 0x003060E4
	protected void HandleEngoopening(Vector2 startPoint, float radius)
	{
		float num = 1f;
		DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopDefinition);
		goopManagerForGoopType.TimedAddGoopCircle(startPoint, radius, num, false);
	}

	// Token: 0x06007933 RID: 31027 RVA: 0x00307F10 File Offset: 0x00306110
	private void DoStrike(Vector2 currentTarget)
	{
		Exploder.Explode(currentTarget, this.strikeExplosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
	}

	// Token: 0x06007934 RID: 31028 RVA: 0x00307F2C File Offset: 0x0030612C
	protected override void OnDestroy()
	{
		if (this.m_extantReticleQuad)
		{
			UnityEngine.Object.Destroy(this.m_extantReticleQuad.gameObject);
		}
		base.OnDestroy();
	}

	// Token: 0x04007B9C RID: 31644
	public float minDistance = 1f;

	// Token: 0x04007B9D RID: 31645
	public float maxDistance = 15f;

	// Token: 0x04007B9E RID: 31646
	public GameObject reticleQuad;

	// Token: 0x04007B9F RID: 31647
	public bool doesGoop;

	// Token: 0x04007BA0 RID: 31648
	public GoopDefinition goopDefinition;

	// Token: 0x04007BA1 RID: 31649
	public float goopRadius = 3f;

	// Token: 0x04007BA2 RID: 31650
	public bool doesStrike = true;

	// Token: 0x04007BA3 RID: 31651
	public GameObject strikeVFX;

	// Token: 0x04007BA4 RID: 31652
	public ExplosionData strikeExplosionData;

	// Token: 0x04007BA5 RID: 31653
	public bool DoScreenFlash = true;

	// Token: 0x04007BA6 RID: 31654
	public float FlashHoldtime = 0.1f;

	// Token: 0x04007BA7 RID: 31655
	public float FlashFadetime = 0.5f;

	// Token: 0x04007BA8 RID: 31656
	public bool TransmogrifySurvivors;

	// Token: 0x04007BA9 RID: 31657
	public float TransmogrifyRadius = 15f;

	// Token: 0x04007BAA RID: 31658
	public float TransmogrifyChance = 0.5f;

	// Token: 0x04007BAB RID: 31659
	[EnemyIdentifier]
	public string TransmogrifyTargetGuid;

	// Token: 0x04007BAC RID: 31660
	private PlayerController m_currentUser;

	// Token: 0x04007BAD RID: 31661
	private tk2dBaseSprite m_extantReticleQuad;

	// Token: 0x04007BAE RID: 31662
	private float m_currentAngle;

	// Token: 0x04007BAF RID: 31663
	private float m_currentDistance = 5f;
}
