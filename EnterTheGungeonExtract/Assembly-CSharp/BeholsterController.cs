using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000FCB RID: 4043
public class BeholsterController : BraveBehaviour
{
	// Token: 0x17000C98 RID: 3224
	// (get) Token: 0x06005819 RID: 22553 RVA: 0x0021A398 File Offset: 0x00218598
	public bool LaserActive
	{
		get
		{
			return this.m_laserActive;
		}
	}

	// Token: 0x17000C99 RID: 3225
	// (get) Token: 0x0600581A RID: 22554 RVA: 0x0021A3A0 File Offset: 0x002185A0
	public bool FiringLaser
	{
		get
		{
			return this.m_firingLaser;
		}
	}

	// Token: 0x17000C9A RID: 3226
	// (get) Token: 0x0600581B RID: 22555 RVA: 0x0021A3A8 File Offset: 0x002185A8
	// (set) Token: 0x0600581C RID: 22556 RVA: 0x0021A3B0 File Offset: 0x002185B0
	public float LaserAngle
	{
		get
		{
			return this.m_laserAngle;
		}
		set
		{
			this.m_laserAngle = value;
			if (this.m_firingLaser)
			{
				base.aiAnimator.FacingDirection = value;
			}
		}
	}

	// Token: 0x17000C9B RID: 3227
	// (get) Token: 0x0600581D RID: 22557 RVA: 0x0021A3D0 File Offset: 0x002185D0
	public BasicBeamController LaserBeam
	{
		get
		{
			return this.m_laserBeam;
		}
	}

	// Token: 0x17000C9C RID: 3228
	// (get) Token: 0x0600581E RID: 22558 RVA: 0x0021A3D8 File Offset: 0x002185D8
	public Vector2 LaserFiringCenter
	{
		get
		{
			return base.transform.position.XY() + this.firingEllipseCenter;
		}
	}

	// Token: 0x0600581F RID: 22559 RVA: 0x0021A3F8 File Offset: 0x002185F8
	public void Start()
	{
		if (base.aiActor.ParentRoom != null && base.aiActor.ParentRoom.area.PrototypeRoomName == "DoubleBeholsterRoom01")
		{
			GameManager.Instance.Dungeon.IsGlitchDungeon = true;
			base.healthHaver.SetHealthMaximum(base.healthHaver.GetMaxHealth() * this.GlitchWorldHealthModifier, null, false);
			base.healthHaver.ForceSetCurrentHealth(base.healthHaver.GetMaxHealth());
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/Glitch");
		}
		this.m_tentacles = base.GetComponentsInChildren<BeholsterTentacleController>();
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.2f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		if (this.eyeSprite)
		{
			base.healthHaver.RegisterBodySprite(this.eyeSprite, false, 0);
			this.eyeSprite.usesOverrideMaterial = false;
		}
		if (this.pupilSprite)
		{
			base.healthHaver.RegisterBodySprite(this.pupilSprite, false, 0);
		}
		base.aiAnimator.FacingDirection = -90f;
		base.aiAnimator.Update();
		base.healthHaver.OnDamaged += this.OnDamaged;
	}

	// Token: 0x06005820 RID: 22560 RVA: 0x0021A55C File Offset: 0x0021875C
	public void Update()
	{
		float facingDirection = base.aiAnimator.FacingDirection;
		if (base.spriteAnimator.CurrentClip != null && base.spriteAnimator.CurrentClip.name.Contains("idle"))
		{
			if (facingDirection > 155f || facingDirection < 25f)
			{
				if (facingDirection <= -60f && facingDirection >= -120f)
				{
					float num = Mathf.InverseLerp(-120f, -60f, facingDirection);
					this.pupilSprite.transform.localPosition = new Vector3(PhysicsEngine.PixelToUnit((int)(num * 11f) - 5), ((double)Mathf.Abs(num - 0.5f) <= 0.35) ? 0f : PhysicsEngine.PixelToUnit(1), this.pupilSprite.transform.localPosition.z);
				}
				else if (Mathf.Abs(facingDirection) >= 90f)
				{
					float num2 = ((facingDirection <= 0f) ? facingDirection : (facingDirection - 360f));
					if (num2 < -180f)
					{
						this.pupilSprite.transform.localPosition = new Vector3(0f, 0f, this.pupilSprite.transform.localPosition.z);
					}
					else
					{
						float num3 = Mathf.InverseLerp(-180f, -120f, num2);
						this.pupilSprite.transform.localPosition = new Vector3(PhysicsEngine.PixelToUnit((int)(num3 * 21f)), -PhysicsEngine.PixelToUnit(Mathf.Min((int)(num3 * 26f), 7)), this.pupilSprite.transform.localPosition.z);
					}
				}
				else if (facingDirection > 0f)
				{
					this.pupilSprite.transform.localPosition = new Vector3(0f, 0f, this.pupilSprite.transform.localPosition.z);
				}
				else
				{
					float num4 = Mathf.InverseLerp(0f, -60f, facingDirection);
					this.pupilSprite.transform.localPosition = new Vector3(-PhysicsEngine.PixelToUnit((int)(num4 * 21f)), -PhysicsEngine.PixelToUnit(Mathf.Min((int)(num4 * 26f), 7)), this.pupilSprite.transform.localPosition.z);
				}
			}
		}
		else
		{
			this.pupilSprite.transform.localPosition = new Vector3(0f, 0f, this.pupilSprite.transform.localPosition.z);
		}
		if (this.m_firingLaser)
		{
			base.aiAnimator.PlayUntilCancelled("eyelaser", true, null, -1f, false);
		}
	}

	// Token: 0x06005821 RID: 22561 RVA: 0x0021A834 File Offset: 0x00218A34
	public void LateUpdate()
	{
		string text = this.GetEyeSprite(base.sprite.CurrentSprite.name);
		int spriteIdByName = base.sprite.GetSpriteIdByName(text);
		if (spriteIdByName > 0)
		{
			this.eyeSprite.usesOverrideMaterial = false;
			this.eyeSprite.renderer.enabled = true;
			this.eyeSprite.SetSprite(spriteIdByName);
		}
		else
		{
			this.eyeSprite.renderer.enabled = false;
		}
	}

	// Token: 0x06005822 RID: 22562 RVA: 0x0021A8AC File Offset: 0x00218AAC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005823 RID: 22563 RVA: 0x0021A8B4 File Offset: 0x00218AB4
	public void StartFiringTentacles(BeholsterTentacleController[] tentacles = null)
	{
		if (tentacles == null)
		{
			tentacles = this.m_tentacles;
		}
		List<BeholsterTentacleController> list = new List<BeholsterTentacleController>();
		for (int i = 0; i < tentacles.Length; i++)
		{
			if (tentacles[i].IsReady)
			{
				list.Add(tentacles[i]);
			}
		}
		if (list.Count > 0)
		{
			list[UnityEngine.Random.Range(0, list.Count)].StartFiring();
		}
	}

	// Token: 0x06005824 RID: 22564 RVA: 0x0021A924 File Offset: 0x00218B24
	public void SingleFireTentacle(BeholsterTentacleController[] tentacles = null, float? angleOffset = null)
	{
		if (tentacles == null)
		{
			tentacles = this.m_tentacles;
		}
		List<BeholsterTentacleController> list = new List<BeholsterTentacleController>();
		for (int i = 0; i < tentacles.Length; i++)
		{
			if (tentacles[i].IsReady)
			{
				list.Add(tentacles[i]);
			}
		}
		if (list.Count > 0)
		{
			list[UnityEngine.Random.Range(0, list.Count)].SingleFire(angleOffset);
		}
	}

	// Token: 0x06005825 RID: 22565 RVA: 0x0021A994 File Offset: 0x00218B94
	public void StopFiringTentacles(BeholsterTentacleController[] tentacles = null)
	{
		if (tentacles == null)
		{
			tentacles = this.m_tentacles;
		}
		for (int i = 0; i < tentacles.Length; i++)
		{
			tentacles[i].CeaseAttack();
		}
	}

	// Token: 0x06005826 RID: 22566 RVA: 0x0021A9CC File Offset: 0x00218BCC
	public void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if (resultValue <= 0f)
		{
			if (this.m_firingLaser)
			{
				this.chargeUpVfx.DestroyAll();
				this.chargeDownVfx.DestroyAll();
				this.StopFiringLaser();
				if (this.m_laserBeam != null)
				{
					this.m_laserBeam.DestroyBeam();
					this.m_laserBeam = null;
				}
			}
			foreach (BeholsterTentacleController beholsterTentacleController in this.m_tentacles)
			{
				foreach (Renderer renderer in beholsterTentacleController.GetComponentsInChildren<Renderer>())
				{
					renderer.enabled = false;
				}
			}
		}
	}

	// Token: 0x06005827 RID: 22567 RVA: 0x0021AA7C File Offset: 0x00218C7C
	public void PrechargeFiringLaser()
	{
		AkSoundEngine.PostEvent("Play_ENM_beholster_charging_01", base.gameObject);
		this.m_laserActive = true;
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.FacingDirection = (float)((base.aiAnimator.FacingDirection <= 0f || base.aiAnimator.FacingDirection >= 180f) ? (-90) : 90);
		base.aiAnimator.PlayUntilCancelled("charge", true, null, -1f, false);
	}

	// Token: 0x06005828 RID: 22568 RVA: 0x0021AB08 File Offset: 0x00218D08
	public void ChargeFiringLaser(float time)
	{
		AkSoundEngine.PostEvent("Play_ENM_deathray_charge_01", base.gameObject);
		this.m_laserActive = true;
		bool flag = base.aiAnimator.FacingDirection > 0f && base.aiAnimator.FacingDirection < 180f;
		if (flag)
		{
			this.chargeUpVfx.SpawnAtLocalPosition(Vector3.zero, 0f, this.beamTransform, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), true, null, false);
		}
		else
		{
			this.chargeDownVfx.SpawnAtLocalPosition(Vector3.zero, 0f, this.beamTransform, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), true, null, false);
		}
		SpriteAnimatorChanger[] componentsInChildren = this.beamTransform.GetComponentsInChildren<SpriteAnimatorChanger>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].time = time / 2f;
		}
		tk2dSprite[] componentsInChildren2 = this.beamTransform.GetComponentsInChildren<tk2dSprite>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].HeightOffGround += (float)((!flag) ? 1 : (-1));
			componentsInChildren2[j].UpdateZDepth();
		}
	}

	// Token: 0x06005829 RID: 22569 RVA: 0x0021AC44 File Offset: 0x00218E44
	public void StartFiringLaser(float laserAngle)
	{
		AkSoundEngine.PostEvent("Play_ENM_deathray_shot_01", base.gameObject);
		this.m_laserActive = true;
		this.m_firingLaser = true;
		this.LaserAngle = laserAngle;
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.PlayUntilCancelled("eyelaser", true, null, -1f, false);
		base.StartCoroutine(this.FireBeam(this.beamModule));
	}

	// Token: 0x0600582A RID: 22570 RVA: 0x0021ACB0 File Offset: 0x00218EB0
	public void StopFiringLaser()
	{
		if (!this.m_firingLaser)
		{
			return;
		}
		AkSoundEngine.PostEvent("Stop_ENM_deathray_loop_01", base.gameObject);
		this.m_laserActive = false;
		this.m_firingLaser = false;
		base.aiAnimator.LockFacingDirection = false;
		base.aiAnimator.EndAnimationIf("eyelaser");
	}

	// Token: 0x0600582B RID: 22571 RVA: 0x0021AD08 File Offset: 0x00218F08
	protected IEnumerator FireBeam(ProjectileModule mod)
	{
		GameObject beamObject = UnityEngine.Object.Instantiate<GameObject>(mod.GetCurrentProjectile().gameObject);
		this.m_laserBeam = beamObject.GetComponent<BasicBeamController>();
		List<AIActor> activeEnemies = base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			AIActor aiactor = activeEnemies[i];
			if (aiactor && aiactor != base.aiActor && aiactor.healthHaver && aiactor.healthHaver.IsBoss)
			{
				this.m_laserBeam.IgnoreRigidbodes.Add(aiactor.specRigidbody);
			}
		}
		this.m_laserBeam.Owner = base.aiActor;
		this.m_laserBeam.HitsPlayers = true;
		this.m_laserBeam.HitsEnemies = true;
		bool facingNorth = BraveMathCollege.ClampAngle180(base.aiAnimator.FacingDirection) > 0f;
		this.m_laserBeam.HeightOffset = 1.9f;
		this.m_laserBeam.RampHeightOffset = (float)((!facingNorth) ? 5 : 0);
		this.m_laserBeam.ContinueBeamArtToWall = true;
		float enemyTickCooldown = 0f;
		this.m_laserBeam.OverrideHitChecks = delegate(SpeculativeRigidbody hitRigidbody, Vector2 dirVec)
		{
			HealthHaver healthHaver = ((!hitRigidbody) ? null : hitRigidbody.healthHaver);
			if (hitRigidbody && hitRigidbody.projectile && hitRigidbody.GetComponent<BeholsterBounceRocket>())
			{
				BounceProjModifier component = hitRigidbody.GetComponent<BounceProjModifier>();
				if (component)
				{
					component.numberOfBounces = 0;
				}
				hitRigidbody.projectile.DieInAir(false, true, true, false);
			}
			if (healthHaver != null)
			{
				if (healthHaver.aiActor)
				{
					if (enemyTickCooldown <= 0f)
					{
						Projectile currentProjectile = mod.GetCurrentProjectile();
						healthHaver.ApplyDamage(ProjectileData.FixedFallbackDamageToEnemies, dirVec, this.aiActor.GetActorName(), currentProjectile.damageTypes, DamageCategory.Normal, false, null, false);
						enemyTickCooldown = mod.cooldownTime;
					}
				}
				else
				{
					Projectile currentProjectile2 = mod.GetCurrentProjectile();
					healthHaver.ApplyDamage(currentProjectile2.baseData.damage, dirVec, this.aiActor.GetActorName(), currentProjectile2.damageTypes, DamageCategory.Normal, false, null, false);
				}
			}
			if (hitRigidbody.majorBreakable)
			{
				hitRigidbody.majorBreakable.ApplyDamage(26f * BraveTime.DeltaTime, dirVec, false, false, false);
			}
		};
		bool firstFrame = true;
		while (this.m_laserBeam != null && this.m_firingLaser)
		{
			enemyTickCooldown = Mathf.Max(enemyTickCooldown - BraveTime.DeltaTime, 0f);
			float clampedAngle = BraveMathCollege.ClampAngle360(this.LaserAngle);
			Vector3 dirVec2 = new Vector3(Mathf.Cos(clampedAngle * 0.017453292f), Mathf.Sin(clampedAngle * 0.017453292f), 0f) * 10f;
			Vector2 startingPoint = this.LaserFiringCenter;
			float tanAngle = Mathf.Tan(clampedAngle * 0.017453292f);
			float sign = (float)((clampedAngle <= 90f || clampedAngle >= 270f) ? 1 : (-1));
			float denominator = Mathf.Sqrt(this.firingEllipseB * this.firingEllipseB + this.firingEllipseA * this.firingEllipseA * (tanAngle * tanAngle));
			startingPoint.x += sign * this.firingEllipseA * this.firingEllipseB / denominator;
			startingPoint.y += sign * this.firingEllipseA * this.firingEllipseB * tanAngle / denominator;
			this.m_laserBeam.Origin = startingPoint;
			this.m_laserBeam.Direction = dirVec2;
			if (firstFrame)
			{
				yield return null;
				firstFrame = false;
			}
			else
			{
				facingNorth = BraveMathCollege.ClampAngle180(base.aiAnimator.FacingDirection) > 0f;
				this.m_laserBeam.RampHeightOffset = (float)((!facingNorth) ? 5 : 0);
				this.m_laserBeam.LateUpdatePosition(startingPoint);
				yield return null;
				if (this.m_firingLaser && !this.m_laserBeam)
				{
					this.StopFiringLaser();
					break;
				}
				while (Time.timeScale == 0f)
				{
					yield return null;
				}
			}
		}
		if (!this.m_firingLaser && this.m_laserBeam != null)
		{
			this.m_laserBeam.DestroyBeam();
			this.m_laserBeam = null;
		}
		yield break;
	}

	// Token: 0x0600582C RID: 22572 RVA: 0x0021AD2C File Offset: 0x00218F2C
	private string GetEyeSprite(string sprite)
	{
		int num = 2;
		if (sprite.Contains("appear") || sprite.Contains("die"))
		{
			num = 1;
		}
		else if (sprite.Contains("eyelaser") || sprite.Contains("idle"))
		{
			num = 2;
		}
		else if (sprite.Contains("charge"))
		{
			num = 3;
		}
		return sprite.Insert(BraveUtility.GetNthIndexOf(sprite, '_', num), "_eye");
	}

	// Token: 0x0400511D RID: 20765
	[Header("Eye Sprites")]
	public tk2dSprite eyeSprite;

	// Token: 0x0400511E RID: 20766
	public Transform pupilTransform;

	// Token: 0x0400511F RID: 20767
	public tk2dSprite pupilSprite;

	// Token: 0x04005120 RID: 20768
	[Header("Beam Data")]
	public Transform beamTransform;

	// Token: 0x04005121 RID: 20769
	public VFXPool chargeUpVfx;

	// Token: 0x04005122 RID: 20770
	public VFXPool chargeDownVfx;

	// Token: 0x04005123 RID: 20771
	public ProjectileModule beamModule;

	// Token: 0x04005124 RID: 20772
	[Header("Beam Firing Point")]
	public Vector2 firingEllipseCenter;

	// Token: 0x04005125 RID: 20773
	public float firingEllipseA;

	// Token: 0x04005126 RID: 20774
	public float firingEllipseB;

	// Token: 0x04005127 RID: 20775
	public float GlitchWorldHealthModifier = 1f;

	// Token: 0x04005128 RID: 20776
	private BeholsterTentacleController[] m_tentacles;

	// Token: 0x04005129 RID: 20777
	private bool m_laserActive;

	// Token: 0x0400512A RID: 20778
	private bool m_firingLaser;

	// Token: 0x0400512B RID: 20779
	private float m_laserAngle;

	// Token: 0x0400512C RID: 20780
	private BasicBeamController m_laserBeam;
}
