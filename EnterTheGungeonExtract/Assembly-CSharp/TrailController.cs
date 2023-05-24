using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200168F RID: 5775
public class TrailController : BraveBehaviour
{
	// Token: 0x060086A7 RID: 34471 RVA: 0x0037BE30 File Offset: 0x0037A030
	public void Start()
	{
		base.specRigidbody = base.transform.parent.GetComponent<SpeculativeRigidbody>();
		base.specRigidbody.Initialize();
		base.transform.parent = SpawnManager.Instance.VFX;
		base.transform.rotation = Quaternion.identity;
		base.transform.position = Vector3.zero;
		if (base.specRigidbody.projectile.Owner is PlayerController)
		{
			this.m_projectileScale = (base.specRigidbody.projectile.Owner as PlayerController).BulletScaleModifier;
		}
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
		this.m_sprite = base.GetComponent<tk2dTiledSprite>();
		this.m_sprite.OverrideGetTiledSpriteGeomDesc = new tk2dTiledSprite.OverrideGetTiledSpriteGeomDescDelegate(this.GetTiledSpriteGeomDesc);
		this.m_sprite.OverrideSetTiledSpriteGeom = new tk2dTiledSprite.OverrideSetTiledSpriteGeomDelegate(this.SetTiledSpriteGeom);
		tk2dSpriteDefinition currentSpriteDef = this.m_sprite.GetCurrentSpriteDef();
		this.m_spriteSubtileWidth = Mathf.RoundToInt(currentSpriteDef.untrimmedBoundsDataExtents.x / currentSpriteDef.texelSize.x) / 4;
		float num = ((!this.rampHeight) ? 0f : this.rampStartHeight);
		this.m_bones.AddLast(new TrailController.Bone(base.specRigidbody.Position.UnitPosition + this.boneSpawnOffset, 0f, num));
		this.m_bones.AddLast(new TrailController.Bone(base.specRigidbody.Position.UnitPosition + this.boneSpawnOffset, 0f, num));
		if (this.usesStartAnimation)
		{
			this.m_startAnimationClip = base.spriteAnimator.GetClipByName(this.startAnimation);
		}
		if (this.usesAnimation)
		{
			this.m_animationClip = base.spriteAnimator.GetClipByName(this.animation);
		}
		if ((this.usesStartAnimation || this.usesAnimation) && this.usesCascadeTimer)
		{
			this.m_bones.First.Value.IsAnimating = true;
		}
		if (this.UsesDispersalParticles)
		{
			this.m_dispersalParticles = GlobalDispersalParticleManager.GetSystemForPrefab(this.DispersalParticleSystemPrefab);
		}
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.UpdateOnCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Combine(specRigidbody2.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.PostRigidbodyMovement));
	}

	// Token: 0x060086A8 RID: 34472 RVA: 0x0037C0B8 File Offset: 0x0037A2B8
	public void Update()
	{
		int num = Mathf.RoundToInt(this.m_sprite.GetCurrentSpriteDef().untrimmedBoundsDataExtents.x / this.m_sprite.GetCurrentSpriteDef().texelSize.x);
		int num2 = num / 4;
		this.m_globalTimer += BraveTime.DeltaTime;
		this.m_rampTimer += BraveTime.DeltaTime;
		if (this.usesAnimation)
		{
			LinkedListNode<TrailController.Bone> linkedListNode = this.m_bones.First;
			float num3 = 0f;
			while (linkedListNode != null)
			{
				bool flag = false;
				if (linkedListNode.Value.IsAnimating)
				{
					tk2dSpriteAnimationClip tk2dSpriteAnimationClip = ((!this.usesStartAnimation || linkedListNode != this.m_bones.First) ? this.m_animationClip : this.m_startAnimationClip);
					linkedListNode.Value.AnimationTimer += BraveTime.DeltaTime;
					num3 = linkedListNode.Value.AnimationTimer;
					int num4 = Mathf.FloorToInt((linkedListNode.Value.AnimationTimer - BraveTime.DeltaTime) * tk2dSpriteAnimationClip.fps);
					int num5 = Mathf.FloorToInt(linkedListNode.Value.AnimationTimer * tk2dSpriteAnimationClip.fps);
					if (num5 != num4)
					{
						this.m_isDirty = true;
					}
					if (linkedListNode.Value.AnimationTimer > (float)tk2dSpriteAnimationClip.frames.Length / tk2dSpriteAnimationClip.fps)
					{
						if (this.usesStartAnimation && linkedListNode == this.m_bones.First)
						{
							this.usesStartAnimation = false;
						}
						int num6 = 0;
						while (num6 < num2 && linkedListNode != null)
						{
							LinkedListNode<TrailController.Bone> linkedListNode2 = linkedListNode;
							linkedListNode = linkedListNode.Next;
							this.m_bones.Remove(linkedListNode2);
							num6++;
						}
						flag = true;
						this.m_isDirty = true;
					}
				}
				if (linkedListNode != null)
				{
					if (!linkedListNode.Value.IsAnimating && this.usesGlobalTimer && this.m_globalTimer > this.globalTimer)
					{
						linkedListNode.Value.IsAnimating = true;
						linkedListNode.Value.AnimationTimer = this.m_globalTimer - this.globalTimer;
						this.DoDispersalParticles(linkedListNode, num2);
						this.m_isDirty = true;
					}
					if (!linkedListNode.Value.IsAnimating && this.usesCascadeTimer && (linkedListNode == this.m_bones.First || num3 >= this.cascadeTimer))
					{
						linkedListNode.Value.IsAnimating = true;
						num3 = 0f;
						this.DoDispersalParticles(linkedListNode, num2);
						this.m_isDirty = true;
					}
					if (!linkedListNode.Value.IsAnimating && this.usesSoftMaxLength && this.m_maxPosX - linkedListNode.Value.posX > this.softMaxLength)
					{
						linkedListNode.Value.IsAnimating = true;
						num3 = 0f;
						this.DoDispersalParticles(linkedListNode, num2);
						this.m_isDirty = true;
					}
				}
				if (!flag && linkedListNode != null)
				{
					int num7 = 0;
					while (num7 < num2 && linkedListNode != null)
					{
						linkedListNode = linkedListNode.Next;
						num7++;
					}
				}
			}
		}
		if (this.m_bones.Count == 0)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060086A9 RID: 34473 RVA: 0x0037C3DC File Offset: 0x0037A5DC
	public void LateUpdate()
	{
		this.UpdateIfDirty();
	}

	// Token: 0x060086AA RID: 34474 RVA: 0x0037C3E4 File Offset: 0x0037A5E4
	protected override void OnDestroy()
	{
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.UpdateOnCollision));
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Remove(specRigidbody2.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.PostRigidbodyMovement));
		}
		base.OnDestroy();
	}

	// Token: 0x060086AB RID: 34475 RVA: 0x0037C458 File Offset: 0x0037A658
	public void DisconnectFromSpecRigidbody()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.UpdateOnCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Remove(specRigidbody2.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.PostRigidbodyMovement));
	}

	// Token: 0x060086AC RID: 34476 RVA: 0x0037C4B4 File Offset: 0x0037A6B4
	private void UpdateOnCollision(CollisionData obj)
	{
		Vector2 vector = base.specRigidbody.Position.UnitPosition + PhysicsEngine.PixelToUnit(obj.NewPixelsToMove);
		this.HandleExtension(vector);
		this.m_bones.Last.Value.Hide = true;
		this.m_bones.AddLast(new TrailController.Bone(this.m_bones.Last.Value.pos, this.m_bones.Last.Value.posX, this.m_bones.Last.Value.HeightOffset));
		this.m_bones.AddLast(new TrailController.Bone(this.m_bones.Last.Value.pos, this.m_bones.Last.Value.posX, this.m_bones.Last.Value.HeightOffset));
	}

	// Token: 0x060086AD RID: 34477 RVA: 0x0037C5A4 File Offset: 0x0037A7A4
	private void PostRigidbodyMovement(SpeculativeRigidbody rigidbody, Vector2 unitDelta, IntVector2 pixelDelta)
	{
		this.HandleExtension(base.specRigidbody.Position.UnitPosition);
		this.UpdateIfDirty();
	}

	// Token: 0x060086AE RID: 34478 RVA: 0x0037C5D0 File Offset: 0x0037A7D0
	private void UpdateIfDirty()
	{
		if (!this.m_isDirty)
		{
			return;
		}
		this.m_minBonePosition = new Vector2(float.MaxValue, float.MaxValue);
		this.m_maxBonePosition = new Vector2(float.MinValue, float.MinValue);
		for (LinkedListNode<TrailController.Bone> linkedListNode = this.m_bones.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			this.m_minBonePosition = Vector2.Min(this.m_minBonePosition, linkedListNode.Value.pos);
			this.m_maxBonePosition = Vector2.Max(this.m_maxBonePosition, linkedListNode.Value.pos);
		}
		base.transform.position = new Vector3(this.m_minBonePosition.x, this.m_minBonePosition.y, this.m_minBonePosition.y + -0.5f);
		this.m_sprite.ForceBuild();
		this.m_sprite.UpdateZDepth();
		this.m_isDirty = false;
	}

	// Token: 0x060086AF RID: 34479 RVA: 0x0037C6C0 File Offset: 0x0037A8C0
	private void HandleExtension(Vector2 specRigidbodyPosition)
	{
		if (!this.destroyOnEmpty && this.m_bones.Count == 0)
		{
			float num = ((!this.rampHeight) ? 0f : this.rampStartHeight);
			this.m_bones.AddLast(new TrailController.Bone(base.specRigidbody.Position.UnitPosition + this.boneSpawnOffset, this.m_maxPosX, num));
			this.m_bones.AddLast(new TrailController.Bone(base.specRigidbody.Position.UnitPosition + this.boneSpawnOffset, this.m_maxPosX, num));
		}
		Vector2 vector = specRigidbodyPosition;
		if (base.specRigidbody.projectile && base.specRigidbody.projectile.OverrideTrailPoint != null)
		{
			vector = base.specRigidbody.projectile.OverrideTrailPoint.Value;
		}
		this.ExtendBonesTo(vector + this.boneSpawnOffset);
		this.m_isDirty = true;
	}

	// Token: 0x060086B0 RID: 34480 RVA: 0x0037C7D8 File Offset: 0x0037A9D8
	private void ExtendBonesTo(Vector2 newPos)
	{
		if (this.m_bones == null || this.m_bones.Last == null || this.m_bones.Last.Value == null || this.m_bones.Last.Previous == null || this.m_bones.Last.Previous.Value == null)
		{
			return;
		}
		Vector2 vector = newPos - this.m_bones.Last.Value.pos;
		Vector2 vector2 = this.m_bones.Last.Value.pos - this.m_bones.Last.Previous.Value.pos;
		float magnitude = vector2.magnitude;
		LinkedListNode<TrailController.Bone> previous = this.m_bones.Last.Previous;
		float num = Vector3.Distance(newPos, this.m_bones.Last.Previous.Value.pos);
		if (num < 0.25f)
		{
			this.m_bones.Last.Value.pos = newPos;
			this.m_bones.Last.Value.posX = this.m_bones.Last.Previous.Value.posX + num;
		}
		else
		{
			if (Mathf.Approximately(magnitude, 0f))
			{
				this.m_bones.Last.Value.pos = this.m_bones.Last.Previous.Value.pos + vector.normalized * 0.25f;
				this.m_bones.Last.Value.posX = this.m_bones.Last.Previous.Value.posX + 0.25f;
			}
			else
			{
				float num2 = 0.25f;
				float num3 = magnitude;
				float num4 = BraveMathCollege.ClampAnglePi(Mathf.Atan2(vector.y, vector.x) - Mathf.Atan2(-vector2.y, -vector2.x));
				float num5 = Mathf.Abs(num4);
				float num6 = Mathf.Asin(num3 / num2 * Mathf.Sin(num5));
				float num7 = 3.1415927f - num6 - num5;
				Vector2 vector3 = vector2.Rotate(Mathf.Sign(num4) * -num7 * 57.29578f);
				this.m_bones.Last.Value.pos = this.m_bones.Last.Previous.Value.pos + vector3.normalized * 0.25f;
				this.m_bones.Last.Value.posX = this.m_bones.Last.Previous.Value.posX + 0.25f;
			}
			Vector2 vector4 = this.m_bones.Last.Value.pos;
			Vector2 vector5 = newPos - vector4;
			float num8 = vector5.magnitude;
			float num9 = ((!this.rampHeight) ? 0f : Mathf.Lerp(this.rampStartHeight, 0f, this.m_rampTimer / this.rampTime));
			vector5.Normalize();
			while (num8 > 0f)
			{
				if (num8 < 0.25f)
				{
					this.m_bones.AddLast(new TrailController.Bone(newPos, this.m_bones.Last.Value.posX + num8, num9));
					break;
				}
				vector4 += vector5 * 0.25f;
				this.m_bones.AddLast(new TrailController.Bone(vector4, this.m_bones.Last.Value.posX + 0.25f, num9));
				num8 -= 0.25f;
				if (this.usesGlobalTimer && this.m_globalTimer > this.globalTimer)
				{
					this.m_bones.Last.Value.AnimationTimer = this.m_globalTimer - this.globalTimer;
				}
			}
		}
		this.m_maxPosX = this.m_bones.Last.Value.posX;
		LinkedListNode<TrailController.Bone> linkedListNode = previous;
		while (linkedListNode != null && linkedListNode.Next != null)
		{
			linkedListNode.Value.normal = (Quaternion.Euler(0f, 0f, 90f) * (linkedListNode.Next.Value.pos - linkedListNode.Value.pos)).normalized;
			linkedListNode = linkedListNode.Next;
		}
		this.m_bones.Last.Value.normal = this.m_bones.Last.Previous.Value.normal;
		this.m_isDirty = true;
	}

	// Token: 0x060086B1 RID: 34481 RVA: 0x0037CCCC File Offset: 0x0037AECC
	private void DoDispersalParticles(LinkedListNode<TrailController.Bone> boneNode, int subtilesPerTile)
	{
		if (this.UsesDispersalParticles && boneNode.Value != null && boneNode.Next != null && boneNode.Next.Value != null)
		{
			Vector3 vector = boneNode.Value.pos.ToVector3ZUp(boneNode.Value.pos.y);
			LinkedListNode<TrailController.Bone> linkedListNode = boneNode;
			int num = 0;
			while (num < subtilesPerTile && linkedListNode.Next != null)
			{
				linkedListNode = linkedListNode.Next;
				num++;
			}
			Vector3 vector2 = linkedListNode.Value.pos.ToVector3ZUp(linkedListNode.Value.pos.y);
			int num2 = Mathf.Max(Mathf.CeilToInt(Vector2.Distance(vector.XY(), vector2.XY()) * this.DispersalDensity), 1);
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)i / (float)num2;
				Vector3 vector3 = Vector3.Lerp(vector, vector2, num3);
				vector3 += Vector3.back;
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

	// Token: 0x060086B2 RID: 34482 RVA: 0x0037CEB4 File Offset: 0x0037B0B4
	public void GetTiledSpriteGeomDesc(out int numVertices, out int numIndices, tk2dSpriteDefinition spriteDef, Vector2 dimensions)
	{
		int num = Mathf.Max(this.m_bones.Count - 1, 0);
		numVertices = num * 4;
		numIndices = num * 6;
	}

	// Token: 0x060086B3 RID: 34483 RVA: 0x0037CEE0 File Offset: 0x0037B0E0
	public void SetTiledSpriteGeom(Vector3[] pos, Vector2[] uv, int offset, out Vector3 boundsCenter, out Vector3 boundsExtents, tk2dSpriteDefinition spriteDef, Vector3 scale, Vector2 dimensions, tk2dBaseSprite.Anchor anchor, float colliderOffsetZ, float colliderExtentZ)
	{
		int num = Mathf.RoundToInt(spriteDef.untrimmedBoundsDataExtents.x / spriteDef.texelSize.x);
		int num2 = num / 4;
		int num3 = Mathf.Max(this.m_bones.Count - 1, 0);
		int num4 = Mathf.CeilToInt((float)num3 / (float)num2);
		boundsCenter = (this.m_minBonePosition + this.m_maxBonePosition) / 2f;
		boundsExtents = (this.m_maxBonePosition - this.m_minBonePosition) / 2f;
		LinkedListNode<TrailController.Bone> linkedListNode = this.m_bones.First;
		int num5 = 0;
		for (int i = 0; i < num4; i++)
		{
			int num6 = 0;
			int num7 = num2 - 1;
			if (i == num4 - 1 && num3 % num2 != 0)
			{
				num7 = num3 % num2 - 1;
			}
			tk2dSpriteDefinition tk2dSpriteDefinition = spriteDef;
			if (this.usesStartAnimation && i == 0)
			{
				int num8 = Mathf.Clamp(Mathf.FloorToInt(linkedListNode.Value.AnimationTimer * this.m_startAnimationClip.fps), 0, this.m_startAnimationClip.frames.Length - 1);
				tk2dSpriteDefinition = this.m_sprite.Collection.spriteDefinitions[this.m_startAnimationClip.frames[num8].spriteId];
			}
			else if (this.usesAnimation && linkedListNode.Value.IsAnimating)
			{
				int num9 = Mathf.Min((int)(linkedListNode.Value.AnimationTimer * this.m_animationClip.fps), this.m_animationClip.frames.Length - 1);
				tk2dSpriteDefinition = this.m_sprite.Collection.spriteDefinitions[this.m_animationClip.frames[num9].spriteId];
			}
			float num10 = 0f;
			for (int j = num6; j <= num7; j++)
			{
				float num11 = 1f;
				if (i == num4 - 1 && j == num7)
				{
					num11 = Vector2.Distance(linkedListNode.Next.Value.pos, linkedListNode.Value.pos);
				}
				int num12 = offset + num5;
				pos[num12++] = linkedListNode.Value.pos + linkedListNode.Value.normal * (tk2dSpriteDefinition.position0.y * this.m_projectileScale) - this.m_minBonePosition;
				pos[num12++] = linkedListNode.Next.Value.pos + linkedListNode.Next.Value.normal * (tk2dSpriteDefinition.position1.y * this.m_projectileScale) - this.m_minBonePosition;
				pos[num12++] = linkedListNode.Value.pos + linkedListNode.Value.normal * (tk2dSpriteDefinition.position2.y * this.m_projectileScale) - this.m_minBonePosition;
				pos[num12++] = linkedListNode.Next.Value.pos + linkedListNode.Next.Value.normal * (tk2dSpriteDefinition.position3.y * this.m_projectileScale) - this.m_minBonePosition;
				num12 = offset + num5;
				pos[num12++] += new Vector3(0f, 0f, -linkedListNode.Value.HeightOffset);
				pos[num12++] += new Vector3(0f, 0f, -linkedListNode.Next.Value.HeightOffset);
				pos[num12++] += new Vector3(0f, 0f, -linkedListNode.Value.HeightOffset);
				pos[num12++] += new Vector3(0f, 0f, -linkedListNode.Next.Value.HeightOffset);
				Vector2 vector = Vector2.Lerp(tk2dSpriteDefinition.uvs[0], tk2dSpriteDefinition.uvs[1], num10);
				Vector2 vector2 = Vector2.Lerp(tk2dSpriteDefinition.uvs[2], tk2dSpriteDefinition.uvs[3], num10 + num11 / (float)num2);
				num12 = offset + num5;
				if (tk2dSpriteDefinition.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
				{
					uv[num12++] = new Vector2(vector.x, vector.y);
					uv[num12++] = new Vector2(vector.x, vector2.y);
					uv[num12++] = new Vector2(vector2.x, vector.y);
					uv[num12++] = new Vector2(vector2.x, vector2.y);
				}
				else if (tk2dSpriteDefinition.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
				{
					uv[num12++] = new Vector2(vector.x, vector.y);
					uv[num12++] = new Vector2(vector2.x, vector.y);
					uv[num12++] = new Vector2(vector.x, vector2.y);
					uv[num12++] = new Vector2(vector2.x, vector2.y);
				}
				else
				{
					uv[num12++] = new Vector2(vector.x, vector.y);
					uv[num12++] = new Vector2(vector2.x, vector.y);
					uv[num12++] = new Vector2(vector.x, vector2.y);
					uv[num12++] = new Vector2(vector2.x, vector2.y);
				}
				if (linkedListNode.Value.Hide)
				{
					uv[num12 - 4] = Vector2.zero;
					uv[num12 - 3] = Vector2.zero;
					uv[num12 - 2] = Vector2.zero;
					uv[num12 - 1] = Vector2.zero;
				}
				if (this.FlipUvsY)
				{
					Vector2 vector3 = uv[num12 - 4];
					uv[num12 - 4] = uv[num12 - 2];
					uv[num12 - 2] = vector3;
					vector3 = uv[num12 - 3];
					uv[num12 - 3] = uv[num12 - 1];
					uv[num12 - 1] = vector3;
				}
				num5 += 4;
				num10 += num11 / (float)this.m_spriteSubtileWidth;
				if (linkedListNode != null)
				{
					linkedListNode = linkedListNode.Next;
				}
			}
		}
	}

	// Token: 0x04008BA9 RID: 35753
	public bool usesStartAnimation;

	// Token: 0x04008BAA RID: 35754
	public string startAnimation;

	// Token: 0x04008BAB RID: 35755
	public bool usesAnimation;

	// Token: 0x04008BAC RID: 35756
	public string animation;

	// Token: 0x04008BAD RID: 35757
	[TogglesProperty("cascadeTimer", "Cascade Timer")]
	public bool usesCascadeTimer;

	// Token: 0x04008BAE RID: 35758
	[HideInInspector]
	public float cascadeTimer;

	// Token: 0x04008BAF RID: 35759
	[TogglesProperty("softMaxLength", "Soft Max Length")]
	public bool usesSoftMaxLength;

	// Token: 0x04008BB0 RID: 35760
	[HideInInspector]
	public float softMaxLength;

	// Token: 0x04008BB1 RID: 35761
	[TogglesProperty("globalTimer", "Global Timer")]
	public bool usesGlobalTimer;

	// Token: 0x04008BB2 RID: 35762
	[HideInInspector]
	public float globalTimer;

	// Token: 0x04008BB3 RID: 35763
	public bool destroyOnEmpty = true;

	// Token: 0x04008BB4 RID: 35764
	[HideInInspector]
	public bool FlipUvsY;

	// Token: 0x04008BB5 RID: 35765
	public bool rampHeight;

	// Token: 0x04008BB6 RID: 35766
	public float rampStartHeight = 2f;

	// Token: 0x04008BB7 RID: 35767
	public float rampTime = 1f;

	// Token: 0x04008BB8 RID: 35768
	public Vector2 boneSpawnOffset;

	// Token: 0x04008BB9 RID: 35769
	public bool UsesDispersalParticles;

	// Token: 0x04008BBA RID: 35770
	[ShowInInspectorIf("UsesDispersalParticles", false)]
	public float DispersalDensity = 3f;

	// Token: 0x04008BBB RID: 35771
	[ShowInInspectorIf("UsesDispersalParticles", false)]
	public float DispersalMinCoherency = 0.2f;

	// Token: 0x04008BBC RID: 35772
	[ShowInInspectorIf("UsesDispersalParticles", false)]
	public float DispersalMaxCoherency = 1f;

	// Token: 0x04008BBD RID: 35773
	[ShowInInspectorIf("UsesDispersalParticles", false)]
	public GameObject DispersalParticleSystemPrefab;

	// Token: 0x04008BBE RID: 35774
	private tk2dTiledSprite m_sprite;

	// Token: 0x04008BBF RID: 35775
	private tk2dSpriteAnimationClip m_startAnimationClip;

	// Token: 0x04008BC0 RID: 35776
	private tk2dSpriteAnimationClip m_animationClip;

	// Token: 0x04008BC1 RID: 35777
	private float m_projectileScale = 1f;

	// Token: 0x04008BC2 RID: 35778
	private int m_spriteSubtileWidth;

	// Token: 0x04008BC3 RID: 35779
	private readonly LinkedList<TrailController.Bone> m_bones = new LinkedList<TrailController.Bone>();

	// Token: 0x04008BC4 RID: 35780
	private ParticleSystem m_dispersalParticles;

	// Token: 0x04008BC5 RID: 35781
	private Vector2 m_minBonePosition;

	// Token: 0x04008BC6 RID: 35782
	private Vector2 m_maxBonePosition;

	// Token: 0x04008BC7 RID: 35783
	private bool m_isDirty;

	// Token: 0x04008BC8 RID: 35784
	private float m_globalTimer;

	// Token: 0x04008BC9 RID: 35785
	private float m_rampTimer;

	// Token: 0x04008BCA RID: 35786
	private float m_maxPosX;

	// Token: 0x04008BCB RID: 35787
	private const int c_bonePixelLength = 4;

	// Token: 0x04008BCC RID: 35788
	private const float c_boneUnitLength = 0.25f;

	// Token: 0x04008BCD RID: 35789
	private const float c_trailHeightOffset = -0.5f;

	// Token: 0x02001690 RID: 5776
	private class Bone
	{
		// Token: 0x060086B4 RID: 34484 RVA: 0x0037D6C8 File Offset: 0x0037B8C8
		public Bone(Vector2 pos, float posX, float heightOffset)
		{
			this.pos = pos;
			this.posX = posX;
			this.HeightOffset = heightOffset;
		}

		// Token: 0x17001436 RID: 5174
		// (get) Token: 0x060086B5 RID: 34485 RVA: 0x0037D6E8 File Offset: 0x0037B8E8
		// (set) Token: 0x060086B6 RID: 34486 RVA: 0x0037D6F0 File Offset: 0x0037B8F0
		public Vector2 pos { get; set; }

		// Token: 0x04008BCF RID: 35791
		public float posX;

		// Token: 0x04008BD0 RID: 35792
		public Vector2 normal;

		// Token: 0x04008BD1 RID: 35793
		public bool IsAnimating;

		// Token: 0x04008BD2 RID: 35794
		public float AnimationTimer;

		// Token: 0x04008BD3 RID: 35795
		public readonly float HeightOffset;

		// Token: 0x04008BD4 RID: 35796
		public bool Hide;
	}
}
