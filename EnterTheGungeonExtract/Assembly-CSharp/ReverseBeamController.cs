using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200166C RID: 5740
public class ReverseBeamController : BeamController
{
	// Token: 0x1700141D RID: 5149
	// (get) Token: 0x060085E0 RID: 34272 RVA: 0x00374150 File Offset: 0x00372350
	public override bool ShouldUseAmmo
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060085E1 RID: 34273 RVA: 0x00374154 File Offset: 0x00372354
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
		if (this.UsesDispersalParticles && this.m_dispersalParticles == null)
		{
			this.m_dispersalParticles = GlobalDispersalParticleManager.GetSystemForPrefab(this.DispersalParticleSystemPrefab);
		}
	}

	// Token: 0x060085E2 RID: 34274 RVA: 0x003742C4 File Offset: 0x003724C4
	public void Update()
	{
		this.m_globalTimer += BraveTime.DeltaTime;
		if (!this.m_target || !this.m_target.healthHaver || this.m_target.healthHaver.IsDead)
		{
			this.m_target = null;
		}
		this.m_hitRigidbody = null;
		this.HandleBeamFrame(base.Origin, base.Direction, this.m_isCurrentlyFiring);
		if (this.m_target == null)
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
		if (this.m_target != null && this.m_elapsed >= 1f && this.m_target.healthHaver)
		{
			if (!string.IsNullOrEmpty(this.BossImpactAnimation) && this.m_target.healthHaver.IsBoss)
			{
				text = this.BossImpactAnimation;
			}
			else
			{
				text = this.EnemyImpactAnimation;
			}
			if (this.m_target.healthHaver.IsBoss && base.projectile)
			{
				num *= base.projectile.BossDamageMultiplier;
			}
			if (base.projectile && base.projectile.BlackPhantomDamageMultiplier != 1f && this.m_target.IsBlackPhantom)
			{
				num *= base.projectile.BlackPhantomDamageMultiplier;
			}
			this.m_target.healthHaver.ApplyDamage(num * BraveTime.DeltaTime, Vector2.zero, base.Owner.ActorName, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
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

	// Token: 0x060085E3 RID: 34275 RVA: 0x003745E0 File Offset: 0x003727E0
	public void LateUpdate()
	{
		if (this.m_isDirty)
		{
			this.m_minBonePosition = new Vector2(float.MaxValue, float.MaxValue);
			this.m_maxBonePosition = new Vector2(float.MinValue, float.MinValue);
			for (LinkedListNode<ReverseBeamController.Bone> linkedListNode = this.m_bones.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				this.m_minBonePosition = Vector2.Min(this.m_minBonePosition, linkedListNode.Value.pos);
				this.m_maxBonePosition = Vector2.Max(this.m_maxBonePosition, linkedListNode.Value.pos);
			}
			Vector2 vector = new Vector2(this.m_minBonePosition.x, this.m_minBonePosition.y) - base.transform.position.XY();
			base.transform.position = new Vector3(this.m_minBonePosition.x, this.m_minBonePosition.y);
			this.m_sprite.HeightOffGround = 0.5f;
			if (this.ImpactRenderer)
			{
				this.ImpactRenderer.transform.position -= vector.ToVector3ZUp(0f);
			}
			this.m_sprite.ForceBuild();
			this.m_sprite.UpdateZDepth();
		}
	}

	// Token: 0x060085E4 RID: 34276 RVA: 0x0037472C File Offset: 0x0037292C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060085E5 RID: 34277 RVA: 0x00374734 File Offset: 0x00372934
	public void HandleBeamFrame(Vector2 barrelPosition, Vector2 direction, bool isCurrentlyFiring)
	{
		if (base.Owner is PlayerController)
		{
			base.HandleChanceTick();
		}
		this.m_elapsed += BraveTime.DeltaTime;
		AIActor target = this.m_target;
		if (this.targetType == ReverseBeamController.TargetType.Screen)
		{
			if (this.m_target == null)
			{
				this.m_elapsed = 0f;
				List<AIActor> allEnemies = StaticReferenceManager.AllEnemies;
				for (int i = 0; i < allEnemies.Count; i++)
				{
					AIActor aiactor = allEnemies[i];
					if (aiactor.IsNormalEnemy && aiactor.renderer.isVisible && aiactor.healthHaver.IsAlive && !aiactor.IsGone)
					{
						this.m_target = aiactor;
						break;
					}
				}
			}
		}
		else if (this.m_target == null)
		{
			this.m_elapsed = 0f;
			RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(barrelPosition.ToIntVector2(VectorConversions.Floor));
			absoluteRoomFromPosition.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref this.s_enemiesInRoom);
			this.s_enemiesInRoom.Sort((AIActor a, AIActor b) => Vector2.Distance(barrelPosition, a.CenterPosition).CompareTo(Vector2.Distance(barrelPosition, b.CenterPosition)));
			for (int j = 0; j < this.s_enemiesInRoom.Count; j++)
			{
				AIActor aiactor2 = this.s_enemiesInRoom[j];
				if (aiactor2.IsNormalEnemy && aiactor2.renderer.isVisible && aiactor2.healthHaver.IsAlive && !aiactor2.IsGone)
				{
					this.m_target = aiactor2;
					break;
				}
			}
		}
		if (this.m_target != target && this.UsesDispersalParticles && this.OnlyParticlesOnDestruction)
		{
			for (LinkedListNode<ReverseBeamController.Bone> linkedListNode = this.m_bones.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				this.DoDispersalParticles(linkedListNode, 1, true);
			}
		}
		this.m_bones.Clear();
		Vector3? vector = null;
		float num = 3f;
		float num2 = 100f;
		float num3 = num2 / 2f;
		if (this.m_target)
		{
			Vector3 vector2 = direction.normalized * 5f;
			vector2 = Quaternion.Euler(0f, 0f, Mathf.PingPong(Time.realtimeSinceStartup * 147.14f, num2) - num3) * direction.normalized * num;
			Vector3 vector3 = barrelPosition;
			Vector3 vector4 = vector3 + vector2;
			vector2 = Quaternion.Euler(0f, 0f, Mathf.PingPong(Time.realtimeSinceStartup * 172.63f, num2) - num3) * -direction.normalized * num;
			Vector3 vector5 = this.m_target.specRigidbody.HitboxPixelCollider.UnitCenter;
			Vector3 vector6 = vector5 + vector2;
			float num4 = Mathf.Clamp01(this.m_elapsed);
			this.DrawBezierCurve(vector3, vector4, vector6, vector5, num4);
			if (this.ImpactRenderer)
			{
				this.ImpactRenderer.renderer.enabled = false;
			}
		}
		else
		{
			Vector3 vector7 = Quaternion.Euler(0f, 0f, Mathf.PingPong(Time.realtimeSinceStartup * 147.14f, num2) - num3) * direction.normalized * num;
			Vector3 vector8 = barrelPosition;
			Vector3 vector9 = vector8 + vector7;
			vector7 = Quaternion.Euler(0f, 0f, Mathf.PingPong(Time.realtimeSinceStartup * 172.63f, num2) - num3) * -direction.normalized * num;
			int num5 = CollisionLayerMatrix.GetMask(CollisionLayer.Projectile);
			num5 |= CollisionMask.LayerToMask(CollisionLayer.BeamBlocker);
			num5 &= ~CollisionMask.LayerToMask(CollisionLayer.PlayerCollider, CollisionLayer.PlayerHitBox);
			PhysicsEngine instance = PhysicsEngine.Instance;
			Vector2 vector10 = vector8;
			Vector2 vector11 = direction;
			float num6 = 30f;
			RaycastResult raycastResult;
			ref RaycastResult ptr = ref raycastResult;
			bool flag = true;
			bool flag2 = true;
			int num7 = num5;
			CollisionLayer? collisionLayer = null;
			bool flag3 = false;
			SpeculativeRigidbody[] ignoreRigidbodies = base.GetIgnoreRigidbodies();
			bool flag4 = instance.RaycastWithIgnores(vector10, vector11, num6, out ptr, flag, flag2, num7, collisionLayer, flag3, null, ignoreRigidbodies);
			Vector3 vector12 = vector8 + (direction.normalized * 30f).ToVector3ZUp(0f);
			if (flag4)
			{
				vector12 = raycastResult.Contact;
				this.m_hitRigidbody = raycastResult.SpeculativeRigidbody;
			}
			RaycastResult.Pool.Free(ref raycastResult);
			vector = new Vector3?(vector12);
			Vector3 vector13 = vector12 + vector7;
			this.DrawBezierCurve(vector8, vector9, vector13, vector12, 1f);
			if (this.ImpactRenderer)
			{
				this.ImpactRenderer.renderer.enabled = false;
			}
		}
		LinkedListNode<ReverseBeamController.Bone> linkedListNode2 = this.m_bones.First;
		while (linkedListNode2 != null && linkedListNode2 != this.m_bones.Last)
		{
			linkedListNode2.Value.normal = (Quaternion.Euler(0f, 0f, 90f) * (linkedListNode2.Next.Value.pos - linkedListNode2.Value.pos)).normalized;
			linkedListNode2 = linkedListNode2.Next;
		}
		if (this.m_bones.Count > 1)
		{
			this.m_bones.Last.Value.normal = this.m_bones.Last.Previous.Value.normal;
		}
		this.m_isDirty = true;
		if (this.ImpactRenderer)
		{
			if (!this.m_target)
			{
				this.ImpactRenderer.renderer.enabled = true;
				this.ImpactRenderer.transform.position = ((vector == null) ? (base.Gun.CurrentOwner as PlayerController).unadjustedAimPoint.XY() : vector.Value.XY());
				this.ImpactRenderer.IsPerpendicular = false;
			}
			else
			{
				this.ImpactRenderer.renderer.enabled = true;
				this.ImpactRenderer.transform.position = this.m_target.CenterPosition;
				this.ImpactRenderer.IsPerpendicular = true;
			}
			this.ImpactRenderer.HeightOffGround = 6f;
			this.ImpactRenderer.UpdateZDepth();
		}
		if (this.UsesDispersalParticles)
		{
			int particleSkipCount = this.ParticleSkipCount;
			LinkedListNode<ReverseBeamController.Bone> linkedListNode3 = this.m_bones.First;
			int num8 = UnityEngine.Random.Range(0, particleSkipCount);
			while (linkedListNode3 != null)
			{
				num8++;
				if (num8 != particleSkipCount)
				{
					linkedListNode3 = linkedListNode3.Next;
				}
				else
				{
					num8 = 0;
					this.DoDispersalParticles(linkedListNode3, 1, true);
					linkedListNode3 = linkedListNode3.Next;
				}
			}
		}
	}

	// Token: 0x060085E6 RID: 34278 RVA: 0x00374E98 File Offset: 0x00373098
	private Vector2 GetBonePosition(ReverseBeamController.Bone bone)
	{
		return bone.pos;
	}

	// Token: 0x060085E7 RID: 34279 RVA: 0x00374EA0 File Offset: 0x003730A0
	private void DoDispersalParticles(LinkedListNode<ReverseBeamController.Bone> boneNode, int subtilesPerTile, bool didImpact)
	{
		if (this.UsesDispersalParticles && boneNode.Value != null && boneNode.Next != null && boneNode.Next.Value != null)
		{
			bool flag = boneNode == this.m_bones.First;
			Vector2 bonePosition = this.GetBonePosition(boneNode.Value);
			Vector3 vector = bonePosition.ToVector3ZUp(bonePosition.y);
			LinkedListNode<ReverseBeamController.Bone> next = boneNode.Next;
			Vector2 bonePosition2 = this.GetBonePosition(next.Value);
			Vector3 vector2 = bonePosition2.ToVector3ZUp(bonePosition2.y);
			bool flag2 = next == this.m_bones.Last && didImpact;
			float num = (float)((!flag && !flag2) ? 1 : 3);
			int num2 = 1;
			if (flag2)
			{
				num2 = Mathf.CeilToInt((float)num2 * this.DispersalExtraImpactFactor);
			}
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)i / (float)num2;
				if (flag)
				{
					num3 = Mathf.Lerp(0f, 0.5f, num3);
				}
				if (flag2)
				{
					num3 = Mathf.Lerp(0.5f, 1f, num3);
				}
				Vector3 vector3 = Vector3.Lerp(vector, vector2, num3);
				float num4 = Mathf.PerlinNoise(vector3.x / 3f, vector3.y / 3f);
				Vector3 vector4 = Quaternion.Euler(0f, 0f, num4 * 360f) * Vector3.right;
				Vector3 vector5 = Vector3.Lerp(vector4, UnityEngine.Random.insideUnitSphere, UnityEngine.Random.Range(this.DispersalMinCoherency, this.DispersalMaxCoherency));
				ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
				{
					position = vector3,
					velocity = vector5 * this.m_dispersalParticles.startSpeed,
					startSize = this.m_dispersalParticles.startSize,
					startLifetime = this.m_dispersalParticles.startLifetime,
					startColor = this.m_dispersalParticles.startColor
				};
				this.m_dispersalParticles.Emit(emitParams, 1);
			}
		}
	}

	// Token: 0x060085E8 RID: 34280 RVA: 0x003750B0 File Offset: 0x003732B0
	public override void LateUpdatePosition(Vector3 origin)
	{
	}

	// Token: 0x060085E9 RID: 34281 RVA: 0x003750B4 File Offset: 0x003732B4
	public override void CeaseAttack()
	{
		this.DestroyBeam();
	}

	// Token: 0x060085EA RID: 34282 RVA: 0x003750BC File Offset: 0x003732BC
	public override void DestroyBeam()
	{
		if (this.UsesDispersalParticles && this.OnlyParticlesOnDestruction)
		{
			for (LinkedListNode<ReverseBeamController.Bone> linkedListNode = this.m_bones.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				this.DoDispersalParticles(linkedListNode, 1, true);
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060085EB RID: 34283 RVA: 0x00375114 File Offset: 0x00373314
	public override void AdjustPlayerBeamTint(Color targetTintColor, int priority, float lerpTime = 0f)
	{
	}

	// Token: 0x060085EC RID: 34284 RVA: 0x00375118 File Offset: 0x00373318
	private void DrawBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float percentComplete)
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
			this.m_bones.AddLast(new ReverseBeamController.Bone(BraveMathCollege.CalculateBezierPoint(1f - percentComplete, p0, p1, p2, p3)));
		}
		int num4 = 1;
		while ((float)num4 <= num3)
		{
			float num5 = (float)num4 / num3;
			Vector3 vector3 = BraveMathCollege.CalculateBezierPoint(num5, p0, p1, p2, p3);
			if (num5 > 1f - percentComplete)
			{
				this.m_bones.AddLast(new ReverseBeamController.Bone(vector3));
			}
			num4++;
		}
	}

	// Token: 0x060085ED RID: 34285 RVA: 0x00375230 File Offset: 0x00373430
	public void GetTiledSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, Vector2 dimensions)
	{
		int num = Mathf.Max(this.m_bones.Count - 1, 0);
		numVertices = num * 4;
		numIndices = num * 6;
	}

	// Token: 0x1700141E RID: 5150
	// (get) Token: 0x060085EE RID: 34286 RVA: 0x0037525C File Offset: 0x0037345C
	// (set) Token: 0x060085EF RID: 34287 RVA: 0x00375264 File Offset: 0x00373464
	public float RampHeightOffset { get; set; }

	// Token: 0x060085F0 RID: 34288 RVA: 0x00375270 File Offset: 0x00373470
	public void SetTiledSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 dimensions, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ)
	{
		int num = Mathf.RoundToInt(spriteDef.untrimmedBoundsDataExtents.x / spriteDef.texelSize.x);
		int num2 = num / 4;
		int num3 = Mathf.Max(this.m_bones.Count - 1, 0);
		int num4 = Mathf.CeilToInt((float)num3 / (float)num2);
		boundsCenter = (this.m_minBonePosition + this.m_maxBonePosition) / 2f;
		boundsExtents = (this.m_maxBonePosition - this.m_minBonePosition) / 2f;
		int num5 = 0;
		LinkedListNode<ReverseBeamController.Bone> linkedListNode = this.m_bones.First;
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

	// Token: 0x04008A5C RID: 35420
	[FormerlySerializedAs("animation")]
	public string beamAnimation;

	// Token: 0x04008A5D RID: 35421
	public bool usesStartAnimation;

	// Token: 0x04008A5E RID: 35422
	public string startAnimation;

	// Token: 0x04008A5F RID: 35423
	public tk2dBaseSprite ImpactRenderer;

	// Token: 0x04008A60 RID: 35424
	[CheckAnimation(null)]
	public string EnemyImpactAnimation;

	// Token: 0x04008A61 RID: 35425
	[CheckAnimation(null)]
	public string BossImpactAnimation;

	// Token: 0x04008A62 RID: 35426
	[CheckAnimation(null)]
	public string OtherImpactAnimation;

	// Token: 0x04008A63 RID: 35427
	public ReverseBeamController.TargetType targetType = ReverseBeamController.TargetType.Screen;

	// Token: 0x04008A64 RID: 35428
	public float endRampHeight;

	// Token: 0x04008A65 RID: 35429
	public int endRampSteps;

	// Token: 0x04008A66 RID: 35430
	[HideInInspector]
	public bool FlipUvsY;

	// Token: 0x04008A67 RID: 35431
	[Header("Particles")]
	public bool UsesDispersalParticles;

	// Token: 0x04008A68 RID: 35432
	[ShowInInspectorIf("UsesDispersalParticles", true)]
	public bool OnlyParticlesOnDestruction;

	// Token: 0x04008A69 RID: 35433
	[ShowInInspectorIf("UsesDispersalParticles", true)]
	public float DispersalDensity = 3f;

	// Token: 0x04008A6A RID: 35434
	[ShowInInspectorIf("UsesDispersalParticles", true)]
	public float DispersalMinCoherency = 0.2f;

	// Token: 0x04008A6B RID: 35435
	[ShowInInspectorIf("UsesDispersalParticles", true)]
	public float DispersalMaxCoherency = 1f;

	// Token: 0x04008A6C RID: 35436
	[ShowInInspectorIf("UsesDispersalParticles", true)]
	public GameObject DispersalParticleSystemPrefab;

	// Token: 0x04008A6D RID: 35437
	[ShowInInspectorIf("UsesDispersalParticles", true)]
	public float DispersalExtraImpactFactor = 1f;

	// Token: 0x04008A6E RID: 35438
	[ShowInInspectorIf("UsesDispersalParticles", true)]
	public int ParticleSkipCount = 20;

	// Token: 0x04008A6F RID: 35439
	private ParticleSystem m_dispersalParticles;

	// Token: 0x04008A70 RID: 35440
	private List<AIActor> s_enemiesInRoom = new List<AIActor>();

	// Token: 0x04008A71 RID: 35441
	private float m_elapsed;

	// Token: 0x04008A73 RID: 35443
	private tk2dTiledSprite m_sprite;

	// Token: 0x04008A74 RID: 35444
	private tk2dSpriteAnimationClip m_startAnimationClip;

	// Token: 0x04008A75 RID: 35445
	private tk2dSpriteAnimationClip m_animationClip;

	// Token: 0x04008A76 RID: 35446
	private bool m_isCurrentlyFiring = true;

	// Token: 0x04008A77 RID: 35447
	private bool m_audioPlaying;

	// Token: 0x04008A78 RID: 35448
	private AIActor m_target;

	// Token: 0x04008A79 RID: 35449
	private SpeculativeRigidbody m_hitRigidbody;

	// Token: 0x04008A7A RID: 35450
	private int m_spriteSubtileWidth;

	// Token: 0x04008A7B RID: 35451
	private LinkedList<ReverseBeamController.Bone> m_bones = new LinkedList<ReverseBeamController.Bone>();

	// Token: 0x04008A7C RID: 35452
	private Vector2 m_minBonePosition;

	// Token: 0x04008A7D RID: 35453
	private Vector2 m_maxBonePosition;

	// Token: 0x04008A7E RID: 35454
	private bool m_isDirty;

	// Token: 0x04008A7F RID: 35455
	private float m_globalTimer;

	// Token: 0x04008A80 RID: 35456
	private const int c_segmentCount = 20;

	// Token: 0x04008A81 RID: 35457
	private const int c_bonePixelLength = 4;

	// Token: 0x04008A82 RID: 35458
	private const float c_boneUnitLength = 0.25f;

	// Token: 0x04008A83 RID: 35459
	private const float c_trailHeightOffset = 0.5f;

	// Token: 0x04008A84 RID: 35460
	private float m_projectileScale = 1f;

	// Token: 0x0200166D RID: 5741
	public enum TargetType
	{
		// Token: 0x04008A86 RID: 35462
		Screen = 10,
		// Token: 0x04008A87 RID: 35463
		Room = 20
	}

	// Token: 0x0200166E RID: 5742
	private class Bone
	{
		// Token: 0x060085F1 RID: 34289 RVA: 0x00375908 File Offset: 0x00373B08
		public Bone(Vector2 pos)
		{
			this.pos = pos;
		}

		// Token: 0x04008A88 RID: 35464
		public Vector2 pos;

		// Token: 0x04008A89 RID: 35465
		public Vector2 normal;
	}
}
