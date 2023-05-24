using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001649 RID: 5705
public class HammerOfDawnController : BraveBehaviour
{
	// Token: 0x17001402 RID: 5122
	// (get) Token: 0x06008535 RID: 34101 RVA: 0x0036EE4C File Offset: 0x0036D04C
	private float ModifiedDamageRadius
	{
		get
		{
			return this.DamageRadius * this.InputScale;
		}
	}

	// Token: 0x06008536 RID: 34102 RVA: 0x0036EE5C File Offset: 0x0036D05C
	public static bool HasExtantHammer(Projectile p)
	{
		if (p && HammerOfDawnController.m_projectileHammerMap.ContainsKey(p) && !HammerOfDawnController.m_projectileHammerMap[p])
		{
			HammerOfDawnController.m_projectileHammerMap.Remove(p);
		}
		return p && HammerOfDawnController.m_projectileHammerMap.ContainsKey(p) && HammerOfDawnController.m_projectileHammerMap[p];
	}

	// Token: 0x06008537 RID: 34103 RVA: 0x0036EED4 File Offset: 0x0036D0D4
	public static void ClearPerLevelData()
	{
		HammerOfDawnController.m_projectileHammerMap.Clear();
	}

	// Token: 0x06008538 RID: 34104 RVA: 0x0036EEE0 File Offset: 0x0036D0E0
	public void AssignOwner(PlayerController p, Projectile beam)
	{
		this.m_owner = p;
		this.m_projectile = beam;
		if (beam != null && HammerOfDawnController.m_projectileHammerMap.ContainsKey(beam))
		{
			HammerOfDawnController hammerOfDawnController = HammerOfDawnController.m_projectileHammerMap[beam];
			AkSoundEngine.PostEvent("Play_WPN_dawnhammer_charge_01", base.gameObject);
			if (hammerOfDawnController)
			{
				hammerOfDawnController.Dispose();
			}
			if (HammerOfDawnController.m_projectileHammerMap.ContainsKey(beam))
			{
				HammerOfDawnController.m_projectileHammerMap.Remove(beam);
			}
		}
		Color? color = null;
		if (beam)
		{
			HammerOfDawnController.m_projectileHammerMap.Add(beam, this);
			if (beam.sprite)
			{
				color = new Color?(beam.sprite.renderer.sharedMaterial.GetColor("_OverrideColor"));
				if (color.Value.a > 0.1f)
				{
					color = new Color?(color.Value.WithAlpha(1f));
				}
			}
		}
		if (p)
		{
			this.InputScale = p.BulletScaleModifier;
		}
		if (this.InputScale > 1f || color != null)
		{
			tk2dBaseSprite[] componentsInChildren = base.GetComponentsInChildren<tk2dBaseSprite>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].scale = new Vector3(this.InputScale, 1f, 1f);
				if (color != null)
				{
					componentsInChildren[i].usesOverrideMaterial = true;
					componentsInChildren[i].renderer.material.SetColor("_OverrideColor", color.Value);
				}
			}
		}
		HammerOfDawnController.m_extantHammers.Add(this);
	}

	// Token: 0x06008539 RID: 34105 RVA: 0x0036F098 File Offset: 0x0036D298
	private void Start()
	{
		for (int i = 0; i < this.BeamSections.Count; i++)
		{
			tk2dSpriteAnimator spriteAnimator = this.BeamSections[i].spriteAnimator;
			if (spriteAnimator)
			{
				spriteAnimator.alwaysUpdateOffscreen = true;
				spriteAnimator.PlayForDuration(this.SectionStartAnimation, -1f, this.SectionAnimation, false);
				AkSoundEngine.PostEvent("Play_WPN_dawnhammer_loop_01", base.gameObject);
				AkSoundEngine.PostEvent("Play_State_Volume_Lower_01", base.gameObject);
			}
		}
		base.spriteAnimator.alwaysUpdateOffscreen = true;
		this.BurstSprite.UpdateZDepth();
		base.sprite.renderer.enabled = false;
		this.m_currentAimPoint = base.transform.position.XY();
		Exploder.DoRadialDamage(this.InitialDamage, base.transform.position, this.ModifiedDamageRadius, false, true, false, null);
		Exploder.DoRadialMajorBreakableDamage(this.InitialDamage, base.transform.position, this.ModifiedDamageRadius);
	}

	// Token: 0x0600853A RID: 34106 RVA: 0x0036F19C File Offset: 0x0036D39C
	private void Update()
	{
		if (this.m_hasDisposed)
		{
			return;
		}
		if (this.m_owner && this.m_projectile)
		{
			this.m_lifeElapsed += BraveTime.DeltaTime;
			BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(this.m_owner.PlayerIDX);
			if (instanceForPlayer.IsKeyboardAndMouse(false))
			{
				this.m_currentAimPoint = this.m_owner.unadjustedAimPoint.XY();
			}
			else
			{
				this.m_currentAimPoint += instanceForPlayer.ActiveActions.Aim.Vector.normalized * this.m_currentTrackingSpeed * BraveTime.DeltaTime;
			}
			Vector2 vector = this.m_currentAimPoint;
			Vector2 vector2 = base.transform.position.XY();
			if (HammerOfDawnController.m_extantHammers.Count > 1)
			{
				int count = HammerOfDawnController.m_extantHammers.Count;
				int num = Mathf.Clamp(HammerOfDawnController.m_extantHammers.IndexOf(this), 0, HammerOfDawnController.m_extantHammers.Count);
				float num2 = 360f / (float)count;
				float num3 = Time.time * this.RotationSpeed % 360f;
				vector += (Quaternion.Euler(0f, 0f, num3 + num2 * (float)num) * Vector2.up * this.OverlapRadius).XY();
			}
			this.m_currentTrackingSpeed = Mathf.Lerp(0f, this.TrackingSpeed, Mathf.Clamp01(this.m_lifeElapsed / 3f));
			base.transform.position = (vector2 + (vector - vector2).normalized * this.m_currentTrackingSpeed * BraveTime.DeltaTime).ToVector3ZisY(0f);
			base.transform.position = BraveMathCollege.ClampToBounds(base.transform.position.XY(), GameManager.Instance.MainCameraController.MinVisiblePoint + new Vector2(-15f, -15f), GameManager.Instance.MainCameraController.MaxVisiblePoint + new Vector2(15f, 15f)).ToVector3ZisY(0f);
			Exploder.DoRadialDamage(this.DamagePerSecond * BraveTime.DeltaTime, vector2.ToVector3ZisY(0f), this.ModifiedDamageRadius, false, true, false, null);
			Exploder.DoRadialMajorBreakableDamage(this.DamagePerSecond * BraveTime.DeltaTime, vector2.ToVector3ZisY(0f), this.ModifiedDamageRadius);
			if (this.m_owner)
			{
				this.ApplyBeamTickToEnemiesInRadius();
			}
			if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.MEDIUM || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH)
			{
				int num4 = ((GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.HIGH) ? 50 : 125);
				this.m_particleCounter += BraveTime.DeltaTime * (float)num4;
				if (this.m_particleCounter > 1f)
				{
					GlobalSparksDoer.DoRadialParticleBurst(Mathf.FloorToInt(this.m_particleCounter), base.sprite.WorldBottomLeft, base.sprite.WorldTopRight, 30f, 2f, 1f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
					this.m_particleCounter %= 1f;
				}
			}
			if (this.m_manager == null)
			{
				this.m_manager = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.FireGoop);
			}
			this.m_manager.AddGoopCircle(vector2, this.FireGoopRadius * this.InputScale, -1, true, -1);
		}
		else
		{
			this.Dispose();
		}
		base.sprite.UpdateZDepth();
		for (int i = 0; i < this.BeamSections.Count; i++)
		{
			this.BeamSections[i].UpdateZDepth();
		}
		this.BurstSprite.UpdateZDepth();
	}

	// Token: 0x0600853B RID: 34107 RVA: 0x0036F5A4 File Offset: 0x0036D7A4
	private void ApplyBeamTickToEnemiesInRadius()
	{
		Vector2 vector = base.transform.position.XY();
		float num = this.ModifiedDamageRadius * this.ModifiedDamageRadius;
		RoomHandler absoluteRoom = vector.GetAbsoluteRoom();
		if (absoluteRoom == null)
		{
			return;
		}
		List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies == null)
		{
			return;
		}
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			AIActor aiactor = activeEnemies[i];
			if (aiactor && aiactor.specRigidbody && (aiactor.CenterPosition - vector).sqrMagnitude < num && aiactor.healthHaver)
			{
				this.m_owner.DoPostProcessBeamTick(null, aiactor.specRigidbody, 1f);
			}
		}
	}

	// Token: 0x0600853C RID: 34108 RVA: 0x0036F674 File Offset: 0x0036D874
	private void LateUpdate()
	{
		if (this.m_hasDisposed)
		{
			return;
		}
		if (!this.BurstSprite.renderer.enabled)
		{
			base.sprite.renderer.enabled = true;
			base.spriteAnimator.Play(this.CapAnimation);
		}
	}

	// Token: 0x0600853D RID: 34109 RVA: 0x0036F6C4 File Offset: 0x0036D8C4
	private void Dispose()
	{
		if (this.m_hasDisposed)
		{
			return;
		}
		base.sprite.renderer.enabled = true;
		this.m_hasDisposed = true;
		HammerOfDawnController.m_extantHammers.Remove(this);
		if (HammerOfDawnController.m_projectileHammerMap.ContainsKey(this.m_projectile))
		{
			HammerOfDawnController.m_projectileHammerMap.Remove(this.m_projectile);
		}
		this.m_owner = null;
		this.m_projectile = null;
		ParticleSystem componentInChildren = base.GetComponentInChildren<ParticleSystem>();
		if (componentInChildren)
		{
			BraveUtility.EnableEmission(componentInChildren, false);
		}
		for (int i = 0; i < this.BeamSections.Count; i++)
		{
			this.BeamSections[i].spriteAnimator.Play(this.SectionEndAnimation);
		}
		base.spriteAnimator.PlayAndDestroyObject(this.CapEndAnimation, null);
		UnityEngine.Object.Destroy(base.gameObject, 1f);
		AkSoundEngine.PostEvent("Stop_WPN_gun_loop_01", base.gameObject);
		AkSoundEngine.PostEvent("Stop_State_Volume_Lower_01", base.gameObject);
	}

	// Token: 0x0400894B RID: 35147
	public List<tk2dSprite> BeamSections;

	// Token: 0x0400894C RID: 35148
	public tk2dSprite BurstSprite;

	// Token: 0x0400894D RID: 35149
	[CheckAnimation(null)]
	public string SectionStartAnimation;

	// Token: 0x0400894E RID: 35150
	[CheckAnimation(null)]
	public string SectionAnimation;

	// Token: 0x0400894F RID: 35151
	[CheckAnimation(null)]
	public string SectionEndAnimation;

	// Token: 0x04008950 RID: 35152
	[CheckAnimation(null)]
	public string CapAnimation;

	// Token: 0x04008951 RID: 35153
	[CheckAnimation(null)]
	public string CapEndAnimation;

	// Token: 0x04008952 RID: 35154
	public GameObject InitialImpactVFX;

	// Token: 0x04008953 RID: 35155
	public float TrackingSpeed = 5f;

	// Token: 0x04008954 RID: 35156
	public float InitialDamage = 50f;

	// Token: 0x04008955 RID: 35157
	public float DamagePerSecond = 20f;

	// Token: 0x04008956 RID: 35158
	public float OverlapRadius = 2.5f;

	// Token: 0x04008957 RID: 35159
	public float DamageRadius = 2.5f;

	// Token: 0x04008958 RID: 35160
	public float RotationSpeed = 60f;

	// Token: 0x04008959 RID: 35161
	public GoopDefinition FireGoop;

	// Token: 0x0400895A RID: 35162
	public float FireGoopRadius = 1.5f;

	// Token: 0x0400895B RID: 35163
	private PlayerController m_owner;

	// Token: 0x0400895C RID: 35164
	private Projectile m_projectile;

	// Token: 0x0400895D RID: 35165
	private float m_currentTrackingSpeed;

	// Token: 0x0400895E RID: 35166
	private DeadlyDeadlyGoopManager m_manager;

	// Token: 0x0400895F RID: 35167
	private float InputScale = 1f;

	// Token: 0x04008960 RID: 35168
	private static Dictionary<Projectile, HammerOfDawnController> m_projectileHammerMap = new Dictionary<Projectile, HammerOfDawnController>();

	// Token: 0x04008961 RID: 35169
	public static List<HammerOfDawnController> m_extantHammers = new List<HammerOfDawnController>();

	// Token: 0x04008962 RID: 35170
	private float m_lifeElapsed;

	// Token: 0x04008963 RID: 35171
	private Vector2 m_currentAimPoint;

	// Token: 0x04008964 RID: 35172
	private float m_particleCounter;

	// Token: 0x04008965 RID: 35173
	private bool m_hasDisposed;
}
