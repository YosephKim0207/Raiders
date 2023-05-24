using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001666 RID: 5734
public class RaidenBeamController : BeamController
{
	// Token: 0x1700141B RID: 5147
	// (get) Token: 0x060085C5 RID: 34245 RVA: 0x003727FC File Offset: 0x003709FC
	public override bool ShouldUseAmmo
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060085C6 RID: 34246 RVA: 0x00372800 File Offset: 0x00370A00
	public void Start()
	{
		base.transform.parent = SpawnManager.Instance.VFX;
		base.transform.rotation = Quaternion.identity;
		base.transform.position = Vector3.zero;
		this.m_sprite = base.GetComponent<tk2dTiledSprite>();
		this.m_sprite.OverrideGetTiledSpriteGeomDesc = new tk2dTiledSprite.OverrideGetTiledSpriteGeomDescDelegate(this.GetTiledSpriteGeomDesc);
		this.m_sprite.OverrideSetTiledSpriteGeom = new tk2dTiledSprite.OverrideSetTiledSpriteGeomDelegate(this.SetTiledSpriteGeom);
		tk2dSpriteDefinition currentSpriteDef = this.m_sprite.GetCurrentSpriteDef();
		this.m_spriteSubtileWidth = Mathf.RoundToInt(currentSpriteDef.untrimmedBoundsDataExtents.x / currentSpriteDef.texelSize.x) / 4;
		if (this.usesStartAnimation)
		{
			this.m_startAnimationClip = base.spriteAnimator.GetClipByName(this.startAnimation);
		}
		this.m_animationClip = base.spriteAnimator.GetClipByName(this.beamAnimation);
		PlayerController playerController = base.projectile.Owner as PlayerController;
		if (playerController)
		{
			this.m_projectileScale = playerController.BulletScaleModifier;
		}
		if (this.ImpactRenderer)
		{
			this.ImpactRenderer.transform.localScale = new Vector3(this.m_projectileScale, this.m_projectileScale, 1f);
		}
	}

	// Token: 0x060085C7 RID: 34247 RVA: 0x00372944 File Offset: 0x00370B44
	public void Update()
	{
		this.m_globalTimer += BraveTime.DeltaTime;
		for (int i = this.m_targets.Count - 1; i >= 0; i--)
		{
			AIActor aiactor = this.m_targets[i];
			if (!aiactor || !aiactor.healthHaver || aiactor.healthHaver.IsDead)
			{
				this.m_targets.RemoveAt(i);
			}
		}
		this.m_hitRigidbody = null;
		this.HandleBeamFrame(base.Origin, base.Direction, this.m_isCurrentlyFiring);
		if (this.m_targets == null || this.m_targets.Count == 0)
		{
			if (GameManager.AUDIO_ENABLED && this.m_audioPlaying)
			{
				this.m_audioPlaying = false;
				AkSoundEngine.PostEvent("Stop_WPN_loop_01", base.gameObject);
			}
		}
		else if (GameManager.AUDIO_ENABLED && !this.m_audioPlaying)
		{
			this.m_audioPlaying = true;
			AkSoundEngine.PostEvent("Play_WPN_shot_01", base.gameObject);
		}
		float num = base.projectile.baseData.damage + base.DamageModifier;
		PlayerController playerController = base.projectile.Owner as PlayerController;
		if (playerController)
		{
			num *= playerController.stats.GetStatValue(PlayerStats.StatType.RateOfFire);
			num *= playerController.stats.GetStatValue(PlayerStats.StatType.Damage);
		}
		if (base.ChanceBasedShadowBullet)
		{
			num *= 2f;
		}
		string text = this.OtherImpactAnimation;
		if (this.m_targets != null && this.m_targets.Count > 0)
		{
			foreach (AIActor aiactor2 in this.m_targets)
			{
				if (aiactor2 && aiactor2.healthHaver)
				{
					if (!string.IsNullOrEmpty(this.BossImpactAnimation) && aiactor2.healthHaver.IsBoss)
					{
						text = this.BossImpactAnimation;
					}
					else
					{
						text = this.EnemyImpactAnimation;
					}
					if (aiactor2.healthHaver.IsBoss && base.projectile)
					{
						num *= base.projectile.BossDamageMultiplier;
					}
					if (base.projectile && base.projectile.BlackPhantomDamageMultiplier != 1f && aiactor2.IsBlackPhantom)
					{
						num *= base.projectile.BlackPhantomDamageMultiplier;
					}
					aiactor2.healthHaver.ApplyDamage(num * BraveTime.DeltaTime, Vector2.zero, base.Owner.ActorName, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
				}
			}
		}
		if (this.m_hitRigidbody)
		{
			if (this.m_hitRigidbody.minorBreakable)
			{
				this.m_hitRigidbody.minorBreakable.Break(base.Direction);
			}
			if (this.m_hitRigidbody.majorBreakable)
			{
				this.m_hitRigidbody.majorBreakable.ApplyDamage(num * BraveTime.DeltaTime, base.Direction, false, false, false);
			}
		}
		if (this.ImpactRenderer && this.ImpactRenderer.spriteAnimator && !string.IsNullOrEmpty(text))
		{
			this.ImpactRenderer.spriteAnimator.Play(text);
		}
	}

	// Token: 0x060085C8 RID: 34248 RVA: 0x00372CDC File Offset: 0x00370EDC
	public void LateUpdate()
	{
		if (this.m_isDirty)
		{
			this.m_minBonePosition = new Vector2(float.MaxValue, float.MaxValue);
			this.m_maxBonePosition = new Vector2(float.MinValue, float.MinValue);
			for (LinkedListNode<RaidenBeamController.Bone> linkedListNode = this.m_bones.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				this.m_minBonePosition = Vector2.Min(this.m_minBonePosition, linkedListNode.Value.pos);
				this.m_maxBonePosition = Vector2.Max(this.m_maxBonePosition, linkedListNode.Value.pos);
			}
			Vector2 vector = new Vector2(this.m_minBonePosition.x, this.m_minBonePosition.y) - base.transform.position.XY();
			base.transform.position = new Vector3(this.m_minBonePosition.x, this.m_minBonePosition.y);
			this.m_sprite.HeightOffGround = 0.5f;
			this.ImpactRenderer.transform.position -= vector.ToVector3ZUp(0f);
			this.m_sprite.ForceBuild();
			this.m_sprite.UpdateZDepth();
		}
	}

	// Token: 0x060085C9 RID: 34249 RVA: 0x00372E18 File Offset: 0x00371018
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060085CA RID: 34250 RVA: 0x00372E20 File Offset: 0x00371020
	public void HandleBeamFrame(Vector2 barrelPosition, Vector2 direction, bool isCurrentlyFiring)
	{
		if (base.Owner is PlayerController)
		{
			base.HandleChanceTick();
		}
		if (this.targetType == RaidenBeamController.TargetType.Screen)
		{
			this.m_targets.Clear();
			List<AIActor> allEnemies = StaticReferenceManager.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				AIActor aiactor = allEnemies[i];
				if (aiactor.IsNormalEnemy && aiactor.renderer.isVisible && aiactor.healthHaver.IsAlive && !aiactor.IsGone)
				{
					this.m_targets.Add(aiactor);
				}
				if (this.maxTargets > 0 && this.m_targets.Count >= this.maxTargets)
				{
					break;
				}
			}
		}
		else if (this.maxTargets <= 0 || this.m_targets.Count < this.maxTargets)
		{
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(barrelPosition.ToIntVector2(VectorConversions.Floor));
			absoluteRoomFromPosition.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref this.s_enemiesInRoom);
			if (this.SelectRandomTarget)
			{
				this.s_enemiesInRoom = this.s_enemiesInRoom.Shuffle<AIActor>();
			}
			else
			{
				this.s_enemiesInRoom.Sort((AIActor a, AIActor b) => Vector2.Distance(barrelPosition, a.CenterPosition).CompareTo(Vector2.Distance(barrelPosition, b.CenterPosition)));
			}
			for (int j = 0; j < this.s_enemiesInRoom.Count; j++)
			{
				AIActor aiactor2 = this.s_enemiesInRoom[j];
				if (aiactor2.IsNormalEnemy && aiactor2.renderer.isVisible && aiactor2.healthHaver.IsAlive && !aiactor2.IsGone)
				{
					this.m_targets.Add(aiactor2);
				}
				if (this.maxTargets > 0 && this.m_targets.Count >= this.maxTargets)
				{
					break;
				}
			}
		}
		this.m_bones.Clear();
		Vector3? vector = null;
		if (this.m_targets.Count > 0)
		{
			Vector3 vector2 = direction.normalized * 5f;
			Vector3 vector3 = barrelPosition;
			Vector3 vector4 = vector3 + vector2;
			vector2 = Quaternion.Euler(0f, 0f, 180f) * vector2;
			Vector3 vector5 = this.m_targets[0].specRigidbody.HitboxPixelCollider.UnitCenter;
			Vector3 vector6 = vector5 + vector2;
			vector2 = Quaternion.Euler(0f, 0f, 180f) * vector2;
			this.DrawBezierCurve(vector3, vector4, vector6, vector5);
			for (int k = 0; k < this.m_targets.Count - 1; k++)
			{
				vector3 = this.m_targets[k].specRigidbody.HitboxPixelCollider.UnitCenter;
				vector4 = vector3 + vector2;
				vector2 = Quaternion.Euler(0f, 0f, 90f) * vector2;
				vector5 = this.m_targets[k + 1].specRigidbody.HitboxPixelCollider.UnitCenter;
				vector6 = vector5 + vector2;
				vector2 = Quaternion.Euler(0f, 0f, 180f) * vector2;
				this.DrawBezierCurve(vector3, vector4, vector6, vector5);
			}
			if (this.ImpactRenderer)
			{
				this.ImpactRenderer.renderer.enabled = false;
			}
		}
		else
		{
			Vector3 vector7 = Quaternion.Euler(0f, 0f, Mathf.PingPong(Time.realtimeSinceStartup * 15f, 60f) - 30f) * direction.normalized * 5f;
			Vector3 vector8 = barrelPosition;
			Vector3 vector9 = vector8 + vector7;
			vector7 = Quaternion.Euler(0f, 0f, 180f) * vector7;
			int num = CollisionLayerMatrix.GetMask(CollisionLayer.Projectile);
			num |= CollisionMask.LayerToMask(CollisionLayer.BeamBlocker);
			num &= ~CollisionMask.LayerToMask(CollisionLayer.PlayerCollider, CollisionLayer.PlayerHitBox);
			PhysicsEngine instance = PhysicsEngine.Instance;
			Vector2 vector10 = vector8;
			Vector2 vector11 = direction;
			float num2 = 30f;
			RaycastResult raycastResult;
			ref RaycastResult ptr = ref raycastResult;
			bool flag = true;
			bool flag2 = true;
			int num3 = num;
			CollisionLayer? collisionLayer = null;
			bool flag3 = false;
			SpeculativeRigidbody[] ignoreRigidbodies = base.GetIgnoreRigidbodies();
			bool flag4 = instance.RaycastWithIgnores(vector10, vector11, num2, out ptr, flag, flag2, num3, collisionLayer, flag3, null, ignoreRigidbodies);
			Vector3 vector12 = vector8 + (direction.normalized * 30f).ToVector3ZUp(0f);
			if (flag4)
			{
				vector12 = raycastResult.Contact;
				this.m_hitRigidbody = raycastResult.SpeculativeRigidbody;
			}
			RaycastResult.Pool.Free(ref raycastResult);
			vector = new Vector3?(vector12);
			Vector3 vector13 = vector12 + vector7;
			vector7 = Quaternion.Euler(0f, 0f, 180f) * vector7;
			this.DrawBezierCurve(vector8, vector9, vector13, vector12);
			if (this.ImpactRenderer)
			{
				this.ImpactRenderer.renderer.enabled = false;
			}
		}
		LinkedListNode<RaidenBeamController.Bone> linkedListNode = this.m_bones.First;
		while (linkedListNode != null && linkedListNode != this.m_bones.Last)
		{
			linkedListNode.Value.normal = (Quaternion.Euler(0f, 0f, 90f) * (linkedListNode.Next.Value.pos - linkedListNode.Value.pos)).normalized;
			linkedListNode = linkedListNode.Next;
		}
		if (this.m_bones.Count > 0)
		{
			this.m_bones.Last.Value.normal = this.m_bones.Last.Previous.Value.normal;
		}
		this.m_isDirty = true;
		if (this.ImpactRenderer)
		{
			if (this.m_targets.Count == 0)
			{
				this.ImpactRenderer.renderer.enabled = true;
				this.ImpactRenderer.transform.position = ((vector == null) ? (base.Gun.CurrentOwner as PlayerController).unadjustedAimPoint.XY() : vector.Value.XY());
				this.ImpactRenderer.IsPerpendicular = false;
			}
			else
			{
				this.ImpactRenderer.renderer.enabled = true;
				this.ImpactRenderer.transform.position = this.m_targets[this.m_targets.Count - 1].CenterPosition;
				this.ImpactRenderer.IsPerpendicular = true;
			}
			this.ImpactRenderer.HeightOffGround = 6f;
			this.ImpactRenderer.UpdateZDepth();
		}
	}

	// Token: 0x060085CB RID: 34251 RVA: 0x00373590 File Offset: 0x00371790
	public override void LateUpdatePosition(Vector3 origin)
	{
	}

	// Token: 0x060085CC RID: 34252 RVA: 0x00373594 File Offset: 0x00371794
	public override void CeaseAttack()
	{
		this.DestroyBeam();
	}

	// Token: 0x060085CD RID: 34253 RVA: 0x0037359C File Offset: 0x0037179C
	public override void DestroyBeam()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060085CE RID: 34254 RVA: 0x003735AC File Offset: 0x003717AC
	public override void AdjustPlayerBeamTint(Color targetTintColor, int priority, float lerpTime = 0f)
	{
	}

	// Token: 0x060085CF RID: 34255 RVA: 0x003735B0 File Offset: 0x003717B0
	private void DrawBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		Vector3 vector = BraveMathCollege.CalculateBezierPoint(0f, p0, p1, p2, p3);
		float num = 0f;
		for (int i = 1; i <= 20; i++)
		{
			float num2 = (float)i / 20f;
			Vector2 vector2 = BraveMathCollege.CalculateBezierPoint(num2, p0, p1, p2, p3);
			num += Vector2.Distance(vector, vector2);
			vector = vector2;
		}
		float num3 = num / 0.25f;
		vector = BraveMathCollege.CalculateBezierPoint(0f, p0, p1, p2, p3);
		if (this.m_bones.Count == 0)
		{
			this.m_bones.AddLast(new RaidenBeamController.Bone(vector));
		}
		int num4 = 1;
		while ((float)num4 <= num3)
		{
			float num5 = (float)num4 / num3;
			Vector3 vector3 = BraveMathCollege.CalculateBezierPoint(num5, p0, p1, p2, p3);
			this.m_bones.AddLast(new RaidenBeamController.Bone(vector3));
			num4++;
		}
	}

	// Token: 0x060085D0 RID: 34256 RVA: 0x003736AC File Offset: 0x003718AC
	public void GetTiledSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, Vector2 dimensions)
	{
		int num = Mathf.Max(this.m_bones.Count - 1, 0);
		numVertices = num * 4;
		numIndices = num * 6;
	}

	// Token: 0x1700141C RID: 5148
	// (get) Token: 0x060085D1 RID: 34257 RVA: 0x003736D8 File Offset: 0x003718D8
	// (set) Token: 0x060085D2 RID: 34258 RVA: 0x003736E0 File Offset: 0x003718E0
	public float RampHeightOffset { get; set; }

	// Token: 0x060085D3 RID: 34259 RVA: 0x003736EC File Offset: 0x003718EC
	public void SetTiledSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 dimensions, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ)
	{
		int num = Mathf.RoundToInt(spriteDef.untrimmedBoundsDataExtents.x / spriteDef.texelSize.x);
		int num2 = num / 4;
		int num3 = Mathf.Max(this.m_bones.Count - 1, 0);
		int num4 = Mathf.CeilToInt((float)num3 / (float)num2);
		boundsCenter = (this.m_minBonePosition + this.m_maxBonePosition) / 2f;
		boundsExtents = (this.m_maxBonePosition - this.m_minBonePosition) / 2f;
		int num5 = 0;
		LinkedListNode<RaidenBeamController.Bone> linkedListNode = this.m_bones.First;
		int num6 = 0;
		for (int i = 0; i < num4; i++)
		{
			int num7 = 0;
			int num8 = num2 - 1;
			if (i == num4 - 1 && num3 % num2 != 0)
			{
				num8 = num3 % num2 - 1;
			}
			tk2dSpriteDefinition tk2dSpriteDefinition;
			if (this.usesStartAnimation && i == 0)
			{
				int num9 = Mathf.FloorToInt(Mathf.Repeat(this.m_globalTimer * this.m_startAnimationClip.fps, (float)this.m_startAnimationClip.frames.Length));
				tk2dSpriteDefinition = this.m_sprite.Collection.spriteDefinitions[this.m_startAnimationClip.frames[num9].spriteId];
			}
			else
			{
				int num10 = Mathf.FloorToInt(Mathf.Repeat(this.m_globalTimer * this.m_animationClip.fps, (float)this.m_animationClip.frames.Length));
				tk2dSpriteDefinition = this.m_sprite.Collection.spriteDefinitions[this.m_animationClip.frames[num10].spriteId];
			}
			float num11 = 0f;
			for (int j = num7; j <= num8; j++)
			{
				float num12 = 1f;
				if (i == num4 - 1 && j == num8)
				{
					num12 = Vector2.Distance(linkedListNode.Next.Value.pos, linkedListNode.Value.pos);
				}
				float num13 = 0f;
				if (this.endRampHeight != 0f)
				{
				}
				int num14 = offset + num6;
				pos[num14++] = (linkedListNode.Value.pos + linkedListNode.Value.normal * (tk2dSpriteDefinition.position0.y * this.m_projectileScale) - this.m_minBonePosition).ToVector3ZUp(-num13);
				pos[num14++] = (linkedListNode.Next.Value.pos + linkedListNode.Next.Value.normal * (tk2dSpriteDefinition.position1.y * this.m_projectileScale) - this.m_minBonePosition).ToVector3ZUp(-num13);
				pos[num14++] = (linkedListNode.Value.pos + linkedListNode.Value.normal * (tk2dSpriteDefinition.position2.y * this.m_projectileScale) - this.m_minBonePosition).ToVector3ZUp(-num13);
				pos[num14++] = (linkedListNode.Next.Value.pos + linkedListNode.Next.Value.normal * (tk2dSpriteDefinition.position3.y * this.m_projectileScale) - this.m_minBonePosition).ToVector3ZUp(-num13);
				Vector2 vector = Vector2.Lerp(tk2dSpriteDefinition.uvs[0], tk2dSpriteDefinition.uvs[1], num11);
				Vector2 vector2 = Vector2.Lerp(tk2dSpriteDefinition.uvs[2], tk2dSpriteDefinition.uvs[3], num11 + num12 / (float)num2);
				num14 = offset + num6;
				if (tk2dSpriteDefinition.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
				{
					uv[num14++] = new Vector2(vector.x, vector.y);
					uv[num14++] = new Vector2(vector.x, vector2.y);
					uv[num14++] = new Vector2(vector2.x, vector.y);
					uv[num14++] = new Vector2(vector2.x, vector2.y);
				}
				else if (tk2dSpriteDefinition.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
				{
					uv[num14++] = new Vector2(vector.x, vector.y);
					uv[num14++] = new Vector2(vector2.x, vector.y);
					uv[num14++] = new Vector2(vector.x, vector2.y);
					uv[num14++] = new Vector2(vector2.x, vector2.y);
				}
				else
				{
					uv[num14++] = new Vector2(vector.x, vector.y);
					uv[num14++] = new Vector2(vector2.x, vector.y);
					uv[num14++] = new Vector2(vector.x, vector2.y);
					uv[num14++] = new Vector2(vector2.x, vector2.y);
				}
				if (this.FlipUvsY)
				{
					Vector2 vector3 = uv[num14 - 4];
					uv[num14 - 4] = uv[num14 - 2];
					uv[num14 - 2] = vector3;
					vector3 = uv[num14 - 3];
					uv[num14 - 3] = uv[num14 - 1];
					uv[num14 - 1] = vector3;
				}
				num6 += 4;
				num11 += num12 / (float)this.m_spriteSubtileWidth;
				if (linkedListNode != null)
				{
					linkedListNode = linkedListNode.Next;
				}
				num5++;
			}
		}
	}

	// Token: 0x04008A29 RID: 35369
	[FormerlySerializedAs("animation")]
	public string beamAnimation;

	// Token: 0x04008A2A RID: 35370
	public bool usesStartAnimation;

	// Token: 0x04008A2B RID: 35371
	public string startAnimation;

	// Token: 0x04008A2C RID: 35372
	public tk2dBaseSprite ImpactRenderer;

	// Token: 0x04008A2D RID: 35373
	[CheckAnimation(null)]
	public string EnemyImpactAnimation;

	// Token: 0x04008A2E RID: 35374
	[CheckAnimation(null)]
	public string BossImpactAnimation;

	// Token: 0x04008A2F RID: 35375
	[CheckAnimation(null)]
	public string OtherImpactAnimation;

	// Token: 0x04008A30 RID: 35376
	public RaidenBeamController.TargetType targetType = RaidenBeamController.TargetType.Screen;

	// Token: 0x04008A31 RID: 35377
	public int maxTargets = -1;

	// Token: 0x04008A32 RID: 35378
	public float endRampHeight;

	// Token: 0x04008A33 RID: 35379
	public int endRampSteps;

	// Token: 0x04008A34 RID: 35380
	[HideInInspector]
	public bool FlipUvsY;

	// Token: 0x04008A35 RID: 35381
	[HideInInspector]
	public bool SelectRandomTarget;

	// Token: 0x04008A36 RID: 35382
	private List<AIActor> s_enemiesInRoom = new List<AIActor>();

	// Token: 0x04008A38 RID: 35384
	private tk2dTiledSprite m_sprite;

	// Token: 0x04008A39 RID: 35385
	private tk2dSpriteAnimationClip m_startAnimationClip;

	// Token: 0x04008A3A RID: 35386
	private tk2dSpriteAnimationClip m_animationClip;

	// Token: 0x04008A3B RID: 35387
	private bool m_isCurrentlyFiring = true;

	// Token: 0x04008A3C RID: 35388
	private bool m_audioPlaying;

	// Token: 0x04008A3D RID: 35389
	private List<AIActor> m_targets = new List<AIActor>();

	// Token: 0x04008A3E RID: 35390
	private SpeculativeRigidbody m_hitRigidbody;

	// Token: 0x04008A3F RID: 35391
	private int m_spriteSubtileWidth;

	// Token: 0x04008A40 RID: 35392
	private LinkedList<RaidenBeamController.Bone> m_bones = new LinkedList<RaidenBeamController.Bone>();

	// Token: 0x04008A41 RID: 35393
	private Vector2 m_minBonePosition;

	// Token: 0x04008A42 RID: 35394
	private Vector2 m_maxBonePosition;

	// Token: 0x04008A43 RID: 35395
	private bool m_isDirty;

	// Token: 0x04008A44 RID: 35396
	private float m_globalTimer;

	// Token: 0x04008A45 RID: 35397
	private const int c_segmentCount = 20;

	// Token: 0x04008A46 RID: 35398
	private const int c_bonePixelLength = 4;

	// Token: 0x04008A47 RID: 35399
	private const float c_boneUnitLength = 0.25f;

	// Token: 0x04008A48 RID: 35400
	private const float c_trailHeightOffset = 0.5f;

	// Token: 0x04008A49 RID: 35401
	private float m_projectileScale = 1f;

	// Token: 0x02001667 RID: 5735
	public enum TargetType
	{
		// Token: 0x04008A4B RID: 35403
		Screen = 10,
		// Token: 0x04008A4C RID: 35404
		Room = 20
	}

	// Token: 0x02001668 RID: 5736
	private class Bone
	{
		// Token: 0x060085D4 RID: 34260 RVA: 0x00373D84 File Offset: 0x00371F84
		public Bone(Vector2 pos)
		{
			this.pos = pos;
		}

		// Token: 0x04008A4D RID: 35405
		public Vector2 pos;

		// Token: 0x04008A4E RID: 35406
		public Vector2 normal;
	}
}
