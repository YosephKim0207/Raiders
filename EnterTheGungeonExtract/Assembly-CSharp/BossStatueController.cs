using System;
using System.Collections;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;

// Token: 0x02000FF0 RID: 4080
public class BossStatueController : BaseBehavior<FullSerializerSerializer>
{
	// Token: 0x17000CCA RID: 3274
	// (get) Token: 0x0600590A RID: 22794 RVA: 0x0021FBFC File Offset: 0x0021DDFC
	public BossStatueController.LevelData CurrentLevel
	{
		get
		{
			return this.levelData[this.m_level];
		}
	}

	// Token: 0x17000CCB RID: 3275
	// (get) Token: 0x0600590B RID: 22795 RVA: 0x0021FC10 File Offset: 0x0021DE10
	// (set) Token: 0x0600590C RID: 22796 RVA: 0x0021FC18 File Offset: 0x0021DE18
	public Vector2? Target
	{
		get
		{
			return this.m_target;
		}
		set
		{
			this.m_target = value;
		}
	}

	// Token: 0x17000CCC RID: 3276
	// (get) Token: 0x0600590D RID: 22797 RVA: 0x0021FC24 File Offset: 0x0021DE24
	public float DistancetoTarget
	{
		get
		{
			Vector2? target = this.m_target;
			if (target == null)
			{
				return 0f;
			}
			Vector2 vector = base.specRigidbody.UnitCenter - new Vector2(0f, this.m_height);
			return Vector2.Distance(this.m_target.Value, vector);
		}
	}

	// Token: 0x17000CCD RID: 3277
	// (get) Token: 0x0600590E RID: 22798 RVA: 0x0021FC80 File Offset: 0x0021DE80
	public Vector2 Position
	{
		get
		{
			return base.specRigidbody.UnitCenter;
		}
	}

	// Token: 0x17000CCE RID: 3278
	// (get) Token: 0x0600590F RID: 22799 RVA: 0x0021FC90 File Offset: 0x0021DE90
	public Vector2 GroundPosition
	{
		get
		{
			return base.specRigidbody.UnitCenter - new Vector2(0f, this.m_height);
		}
	}

	// Token: 0x17000CCF RID: 3279
	// (get) Token: 0x06005910 RID: 22800 RVA: 0x0021FCB4 File Offset: 0x0021DEB4
	public bool IsKali
	{
		get
		{
			return this.m_level >= this.levelData.Count - 1;
		}
	}

	// Token: 0x17000CD0 RID: 3280
	// (get) Token: 0x06005911 RID: 22801 RVA: 0x0021FCD0 File Offset: 0x0021DED0
	// (set) Token: 0x06005912 RID: 22802 RVA: 0x0021FCD8 File Offset: 0x0021DED8
	public bool IsGrounded { get; set; }

	// Token: 0x17000CD1 RID: 3281
	// (get) Token: 0x06005913 RID: 22803 RVA: 0x0021FCE4 File Offset: 0x0021DEE4
	// (set) Token: 0x06005914 RID: 22804 RVA: 0x0021FCEC File Offset: 0x0021DEEC
	public bool IsStomping { get; set; }

	// Token: 0x17000CD2 RID: 3282
	// (get) Token: 0x06005915 RID: 22805 RVA: 0x0021FCF8 File Offset: 0x0021DEF8
	// (set) Token: 0x06005916 RID: 22806 RVA: 0x0021FD00 File Offset: 0x0021DF00
	public bool IsTransforming { get; set; }

	// Token: 0x17000CD3 RID: 3283
	// (get) Token: 0x06005917 RID: 22807 RVA: 0x0021FD0C File Offset: 0x0021DF0C
	public bool ReadyToJump
	{
		get
		{
			return this.m_landTimer <= 0f;
		}
	}

	// Token: 0x17000CD4 RID: 3284
	// (get) Token: 0x06005918 RID: 22808 RVA: 0x0021FD20 File Offset: 0x0021DF20
	// (set) Token: 0x06005919 RID: 22809 RVA: 0x0021FD28 File Offset: 0x0021DF28
	public float HangTime { get; set; }

	// Token: 0x17000CD5 RID: 3285
	// (get) Token: 0x0600591A RID: 22810 RVA: 0x0021FD34 File Offset: 0x0021DF34
	// (set) Token: 0x0600591B RID: 22811 RVA: 0x0021FD3C File Offset: 0x0021DF3C
	public List<BulletScriptSelector> QueuedBulletScript { get; set; }

	// Token: 0x17000CD6 RID: 3286
	// (get) Token: 0x0600591C RID: 22812 RVA: 0x0021FD48 File Offset: 0x0021DF48
	// (set) Token: 0x0600591D RID: 22813 RVA: 0x0021FD50 File Offset: 0x0021DF50
	public bool SuppressShootVfx { get; set; }

	// Token: 0x17000CD7 RID: 3287
	// (get) Token: 0x0600591E RID: 22814 RVA: 0x0021FD5C File Offset: 0x0021DF5C
	// (set) Token: 0x0600591F RID: 22815 RVA: 0x0021FD64 File Offset: 0x0021DF64
	public BossStatueController.StatueState State
	{
		get
		{
			return this.m_state;
		}
		set
		{
			this.EndState(this.m_state);
			this.m_state = value;
			this.BeginState(this.m_state);
		}
	}

	// Token: 0x06005920 RID: 22816 RVA: 0x0021FD88 File Offset: 0x0021DF88
	protected override void Awake()
	{
		base.Awake();
		base.aiActor.BehaviorOverridesVelocity = true;
		this.IsGrounded = true;
		this.QueuedBulletScript = new List<BulletScriptSelector>();
		base.specRigidbody.HitboxPixelCollider.CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.PlayerCollider, CollisionLayer.PlayerHitBox, CollisionLayer.PlayerBlocker);
	}

	// Token: 0x06005921 RID: 22817 RVA: 0x0021FDDC File Offset: 0x0021DFDC
	public void Start()
	{
		this.m_statuesController = base.transform.parent.GetComponent<BossStatuesController>();
		this.m_maxJumpHeight = -0.5f * (this.m_statuesController.AttackHopSpeed * this.m_statuesController.AttackHopSpeed) / this.m_statuesController.AttackGravity;
		base.encounterTrackable = this.m_statuesController.encounterTrackable;
		this.m_shadowLocalPos = this.shadowSprite.transform.localPosition;
		this.m_landVfxOffset = base.specRigidbody.UnitCenter - this.landVfx.transform.position.XY();
		this.m_attackVfxOffset = base.specRigidbody.UnitCenter - this.attackVfx.transform.position.XY();
		this.m_landVfxKiller = this.landVfx.GetComponent<SpriteAnimatorKiller>();
		this.m_attackVfxKiller = this.attackVfx.GetComponent<SpriteAnimatorKiller>();
		this.landVfx.transform.parent = SpawnManager.Instance.VFX;
		this.attackVfx.transform.parent = SpawnManager.Instance.VFX;
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.WallMovementResctrictor));
		base.bulletBank.CollidesWithEnemies = false;
		base.gameActor.PreventAutoAimVelocity = true;
	}

	// Token: 0x06005922 RID: 22818 RVA: 0x0021FF48 File Offset: 0x0021E148
	public void Update()
	{
		Vector2? target = this.m_target;
		if (target == null)
		{
			return;
		}
		if (base.bulletBank)
		{
			PlayerController activePlayerClosestToPoint = GameManager.Instance.GetActivePlayerClosestToPoint(this.m_target.Value, false);
			if (activePlayerClosestToPoint)
			{
				base.bulletBank.FixedPlayerPosition = new Vector2?(activePlayerClosestToPoint.specRigidbody.GetUnitCenter(ColliderType.HitBox));
			}
		}
		base.specRigidbody.PixelColliders[0].Enabled = this.m_height < 1.5f;
		if (this.IsGrounded)
		{
			if (this.m_landTimer > 0f)
			{
				this.m_landTimer = Mathf.Max(0f, this.m_landTimer - BraveTime.DeltaTime);
				base.aiActor.BehaviorVelocity = Vector2.zero;
				return;
			}
			if (this.m_state == BossStatueController.StatueState.StandStill)
			{
				return;
			}
			if (this.m_state == BossStatueController.StatueState.WaitForAttack)
			{
				if (this.QueuedBulletScript.Count == 0)
				{
					return;
				}
				if (this.m_state == BossStatueController.StatueState.WaitForAttack)
				{
					this.m_state = BossStatueController.StatueState.HopToTarget;
				}
			}
			if (this.QueuedBulletScript.Count > 0)
			{
				this.m_initialVelocity = this.m_statuesController.AttackHopSpeed;
				this.m_gravity = this.m_statuesController.AttackGravity;
				this.m_totalAirTime = this.m_statuesController.attackHopTime;
				this.m_isAttacking = true;
			}
			else
			{
				this.m_initialVelocity = this.m_statuesController.MoveHopSpeed;
				this.m_gravity = this.m_statuesController.MoveGravity;
				this.m_totalAirTime = this.m_statuesController.moveHopTime;
			}
			this.IsGrounded = false;
			this.m_airTimer = 0f;
			this.m_launchGroundPosition = this.GroundPosition;
			AkSoundEngine.PostEvent("Play_ENM_statue_jump_01", base.gameObject);
		}
		this.m_airTimer += BraveTime.DeltaTime;
		float num = this.m_airTimer;
		Vector2 vector = Vector2.MoveTowards(this.GroundPosition, this.m_target.Value, this.m_statuesController.CurrentMoveSpeed * BraveTime.DeltaTime);
		if (this.IsStomping)
		{
			float num2 = this.m_airTimer / (this.m_totalAirTime / 2f);
			vector = ((num2 > 1f) ? this.GroundPosition : Vector2.Lerp(this.m_launchGroundPosition, this.Target.Value, num2));
			if (this.m_airTimer < this.m_totalAirTime / 2f)
			{
				num = this.m_airTimer;
			}
			else if (this.m_airTimer < this.m_totalAirTime / 2f + this.HangTime)
			{
				num = this.m_totalAirTime / 2f;
			}
			else
			{
				num = this.m_airTimer - this.HangTime;
			}
		}
		this.m_height = this.m_initialVelocity * num + 0.5f * this.m_gravity * num * num;
		if (this.m_height <= 0f && !this.IsGrounded)
		{
			this.m_height = 0f;
			this.landVfx.gameObject.SetActive(true);
			this.m_landVfxKiller.Restart();
			this.landVfx.transform.position = vector - this.m_landVfxOffset;
			this.landVfx.sprite.UpdateZDepth();
			this.m_landTimer = this.m_statuesController.groundedTime;
			this.IsGrounded = true;
			if (this.m_isAttacking)
			{
				if (!this.SuppressShootVfx && this.QueuedBulletScript[0] != null && !this.QueuedBulletScript[0].IsNull)
				{
					this.attackVfx.gameObject.SetActive(true);
					this.m_attackVfxKiller.Restart();
					this.attackVfx.transform.position = vector - this.m_attackVfxOffset;
					this.attackVfx.sprite.UpdateZDepth();
				}
				if (this.QueuedBulletScript[0] != null && !this.QueuedBulletScript[0].IsNull)
				{
					this.ShootBulletScript(this.QueuedBulletScript[0]);
					base.spriteAnimator.Play(this.CurrentLevel.fireAnim);
				}
				this.QueuedBulletScript.RemoveAt(0);
				this.m_isAttacking = false;
			}
		}
		int num3 = Mathf.RoundToInt((float)(this.shadowSprite.spriteAnimator.DefaultClip.frames.Length - 1) * Mathf.Clamp01(this.m_height / this.m_maxJumpHeight));
		this.shadowSprite.spriteAnimator.SetFrame(num3);
		this.shadowSprite.transform.localPosition = this.m_shadowLocalPos - new Vector2(0f, this.m_height);
		Vector2 vector2 = new Vector2(vector.x, vector.y + this.m_height);
		base.aiActor.BehaviorVelocity = (vector2 - base.specRigidbody.UnitCenter) / BraveTime.DeltaTime;
		base.sprite.HeightOffGround = this.m_height;
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06005923 RID: 22819 RVA: 0x00220478 File Offset: 0x0021E678
	protected override void OnDestroy()
	{
		if (this.landVfx)
		{
			UnityEngine.Object.Destroy(this.landVfx.gameObject);
		}
		if (this.attackVfx)
		{
			UnityEngine.Object.Destroy(this.attackVfx.gameObject);
		}
		base.OnDestroy();
	}

	// Token: 0x06005924 RID: 22820 RVA: 0x002204CC File Offset: 0x0021E6CC
	public void LevelUp()
	{
		base.StartCoroutine(this.LevelUpCR());
	}

	// Token: 0x06005925 RID: 22821 RVA: 0x002204DC File Offset: 0x0021E6DC
	private IEnumerator LevelUpCR()
	{
		this.IsTransforming = true;
		this.State = BossStatueController.StatueState.StandStill;
		this.m_level++;
		while (!this.IsGrounded)
		{
			this.State = BossStatueController.StatueState.StandStill;
			yield return null;
		}
		if (this.m_level >= 3)
		{
			base.spriteAnimator.Play(this.kaliTransformAnim);
			if (base.healthHaver.GetCurrentHealthPercentage() < 0.75f)
			{
				base.healthHaver.ForceSetCurrentHealth(0.75f * base.healthHaver.GetMaxHealth());
			}
			while (base.spriteAnimator.IsPlaying(this.kaliTransformAnim))
			{
				yield return null;
			}
			GameObject vfxObj = SpawnManager.SpawnVFX(this.kaliExplosionVfx, this.kaliExplosionTransform.position, Quaternion.identity);
			vfxObj.transform.parent = this.kaliExplosionTransform;
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 0.2f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			vfxObj = SpawnManager.SpawnVFX(this.kaliFireworkdsVfx, this.kaliFireworksTransform.position, Quaternion.identity);
			vfxObj.transform.parent = this.kaliFireworksTransform;
			vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = 0.2f;
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			if (!string.IsNullOrEmpty(this.CurrentLevel.idleSprite))
			{
				base.sprite.SetSprite(this.CurrentLevel.idleSprite);
			}
			else
			{
				base.spriteAnimator.Play(this.CurrentLevel.idleAnim);
			}
			base.sprite.ForceUpdateMaterial();
			yield return new WaitForSeconds(this.kaliPostTransformDelay);
		}
		else
		{
			yield return new WaitForSeconds(this.transformDelay);
			for (int i = 0; i < this.transformPoints.Count; i++)
			{
				if (this.m_level == 1 && i >= 2)
				{
					break;
				}
				GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.transformVfx);
				GameObject vfxObj2 = SpawnManager.SpawnVFX(vfxPrefab, this.transformPoints[i].position, Quaternion.identity);
				vfxObj2.transform.parent = this.transformPoints[i];
				tk2dBaseSprite vfxSprite2 = vfxObj2.GetComponent<tk2dBaseSprite>();
				vfxSprite2.HeightOffGround = 0.2f;
				base.sprite.AttachRenderer(vfxSprite2);
				base.sprite.UpdateZDepth();
				yield return new WaitForSeconds(this.transformMidDelay);
			}
			if (!string.IsNullOrEmpty(this.CurrentLevel.idleSprite))
			{
				base.sprite.SetSprite(this.CurrentLevel.idleSprite);
			}
			else
			{
				base.spriteAnimator.Play(this.CurrentLevel.idleAnim);
			}
		}
		if (this.m_currentEyeVfx != null)
		{
			UnityEngine.Object.Destroy(this.m_currentEyeVfx);
		}
		if (this.CurrentLevel.EyeTrailVFX != null)
		{
			this.m_currentEyeVfx = UnityEngine.Object.Instantiate<GameObject>(this.CurrentLevel.EyeTrailVFX);
			this.m_currentEyeVfx.transform.parent = base.transform;
			this.m_currentEyeVfx.transform.localPosition = new Vector3(0f, 0f, -20f);
			TrailRenderer[] componentsInChildren = this.m_currentEyeVfx.GetComponentsInChildren<TrailRenderer>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].sortingLayerName = "Foreground";
			}
		}
		if (this.IsKali)
		{
			base.spriteAnimator.Play(this.CurrentLevel.idleAnim);
			base.spriteAnimator.SetFrame(0);
			base.specRigidbody.ForceRegenerate(null, null);
		}
		this.IsTransforming = false;
		yield break;
	}

	// Token: 0x06005926 RID: 22822 RVA: 0x002204F8 File Offset: 0x0021E6F8
	public void ClearQueuedAttacks()
	{
		int num = ((!this.m_isAttacking) ? 0 : 1);
		while (this.QueuedBulletScript.Count > num)
		{
			this.QueuedBulletScript.RemoveAt(this.QueuedBulletScript.Count - 1);
		}
	}

	// Token: 0x06005927 RID: 22823 RVA: 0x00220548 File Offset: 0x0021E748
	public void FakeFireVFX()
	{
		AIBulletBank.Entry bullet = base.bulletBank.GetBullet("default");
		for (int i = 0; i < base.bulletBank.transforms.Count; i++)
		{
			Transform transform = base.bulletBank.transforms[i];
			bullet.MuzzleFlashEffects.SpawnAtLocalPosition(Vector3.zero, transform.localEulerAngles.z, transform, null, null, false, null, false);
		}
		if (bullet.PlayAudio)
		{
			if (!string.IsNullOrEmpty(bullet.AudioSwitch))
			{
				AkSoundEngine.SetSwitch("WPN_Guns", bullet.AudioSwitch, base.bulletBank.SoundChild);
				AkSoundEngine.PostEvent(bullet.AudioEvent, base.bulletBank.SoundChild);
			}
			else
			{
				AkSoundEngine.PostEvent(bullet.AudioEvent, base.gameObject);
			}
		}
	}

	// Token: 0x06005928 RID: 22824 RVA: 0x00220634 File Offset: 0x0021E834
	public void ForceStopBulletScript()
	{
		if (this.m_bulletScriptSource)
		{
			UnityEngine.Object.Destroy(this.m_bulletScriptSource);
			this.m_bulletScriptSource = null;
		}
	}

	// Token: 0x06005929 RID: 22825 RVA: 0x00220658 File Offset: 0x0021E858
	private void WallMovementResctrictor(SpeculativeRigidbody specRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		Func<IntVector2, bool> func = delegate(IntVector2 pixel)
		{
			Vector2 vector = PhysicsEngine.PixelToUnitMidpoint(pixel);
			int num = (int)vector.x;
			int num2 = (int)vector.y;
			return !GameManager.Instance.Dungeon.data.CheckInBounds(num, num2) || GameManager.Instance.Dungeon.data.isWall(num, num2) || GameManager.Instance.Dungeon.data[num, num2].isExitCell;
		};
		PixelCollider primaryPixelCollider = specRigidbody.PrimaryPixelCollider;
		if (primaryPixelCollider != null)
		{
			if (func(primaryPixelCollider.LowerLeft + pixelOffset))
			{
				validLocation = false;
				return;
			}
			if (func(primaryPixelCollider.UpperRight + pixelOffset))
			{
				validLocation = false;
				return;
			}
		}
	}

	// Token: 0x0600592A RID: 22826 RVA: 0x002206D4 File Offset: 0x0021E8D4
	private void BeginState(BossStatueController.StatueState state)
	{
	}

	// Token: 0x0600592B RID: 22827 RVA: 0x002206D8 File Offset: 0x0021E8D8
	private void EndState(BossStatueController.StatueState state)
	{
	}

	// Token: 0x0600592C RID: 22828 RVA: 0x002206DC File Offset: 0x0021E8DC
	private void ShootBulletScript(BulletScriptSelector bulletScript)
	{
		if (!this.m_bulletScriptSource)
		{
			this.m_bulletScriptSource = this.shootPoint.gameObject.GetOrAddComponent<BulletScriptSource>();
		}
		this.m_bulletScriptSource.BulletManager = base.bulletBank;
		this.m_bulletScriptSource.BulletScript = bulletScript;
		this.m_bulletScriptSource.Initialize();
	}

	// Token: 0x0400523A RID: 21050
	private const float c_maxHeightToBeGrounded = 1.5f;

	// Token: 0x0400523B RID: 21051
	public tk2dBaseSprite shadowSprite;

	// Token: 0x0400523C RID: 21052
	public tk2dSpriteAnimator landVfx;

	// Token: 0x0400523D RID: 21053
	public tk2dSpriteAnimator attackVfx;

	// Token: 0x0400523E RID: 21054
	public Transform shootPoint;

	// Token: 0x0400523F RID: 21055
	public List<BossStatueController.LevelData> levelData;

	// Token: 0x04005240 RID: 21056
	public List<GameObject> transformVfx;

	// Token: 0x04005241 RID: 21057
	public List<Transform> transformPoints;

	// Token: 0x04005242 RID: 21058
	public float transformDelay = 0.5f;

	// Token: 0x04005243 RID: 21059
	public float transformMidDelay = 1f;

	// Token: 0x04005244 RID: 21060
	public string kaliTransformAnim;

	// Token: 0x04005245 RID: 21061
	public Transform kaliExplosionTransform;

	// Token: 0x04005246 RID: 21062
	public GameObject kaliExplosionVfx;

	// Token: 0x04005247 RID: 21063
	public Transform kaliFireworksTransform;

	// Token: 0x04005248 RID: 21064
	public GameObject kaliFireworkdsVfx;

	// Token: 0x04005249 RID: 21065
	public float kaliPostTransformDelay = 1f;

	// Token: 0x04005250 RID: 21072
	private BossStatuesController m_statuesController;

	// Token: 0x04005251 RID: 21073
	private BulletScriptSource m_bulletScriptSource;

	// Token: 0x04005252 RID: 21074
	private BossStatueController.StatueState m_state;

	// Token: 0x04005253 RID: 21075
	private int m_level;

	// Token: 0x04005254 RID: 21076
	private Vector2? m_target;

	// Token: 0x04005255 RID: 21077
	private float m_landTimer;

	// Token: 0x04005256 RID: 21078
	private bool m_isAttacking;

	// Token: 0x04005257 RID: 21079
	private float m_height;

	// Token: 0x04005258 RID: 21080
	private float m_initialVelocity;

	// Token: 0x04005259 RID: 21081
	private float m_gravity;

	// Token: 0x0400525A RID: 21082
	private float m_totalAirTime;

	// Token: 0x0400525B RID: 21083
	private Vector2 m_launchGroundPosition;

	// Token: 0x0400525C RID: 21084
	private float m_airTimer;

	// Token: 0x0400525D RID: 21085
	private float m_maxJumpHeight;

	// Token: 0x0400525E RID: 21086
	private Vector2 m_shadowLocalPos;

	// Token: 0x0400525F RID: 21087
	private Vector2 m_landVfxOffset;

	// Token: 0x04005260 RID: 21088
	private Vector2 m_attackVfxOffset;

	// Token: 0x04005261 RID: 21089
	private SpriteAnimatorKiller m_landVfxKiller;

	// Token: 0x04005262 RID: 21090
	private SpriteAnimatorKiller m_attackVfxKiller;

	// Token: 0x04005263 RID: 21091
	private GameObject m_currentEyeVfx;

	// Token: 0x02000FF1 RID: 4081
	public enum StatueState
	{
		// Token: 0x04005266 RID: 21094
		HopToTarget,
		// Token: 0x04005267 RID: 21095
		WaitForAttack,
		// Token: 0x04005268 RID: 21096
		StandStill
	}

	// Token: 0x02000FF2 RID: 4082
	[Serializable]
	public class LevelData
	{
		// Token: 0x04005269 RID: 21097
		public string idleSprite;

		// Token: 0x0400526A RID: 21098
		public string idleAnim;

		// Token: 0x0400526B RID: 21099
		public string fireAnim;

		// Token: 0x0400526C RID: 21100
		public string deathAnim;

		// Token: 0x0400526D RID: 21101
		public GameObject EyeTrailVFX;
	}
}
