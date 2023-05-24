using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013B2 RID: 5042
[Serializable]
public class GameActorCheeseEffect : GameActorEffect
{
	// Token: 0x06007241 RID: 29249 RVA: 0x002D676C File Offset: 0x002D496C
	public bool ShouldVanishOnDeath(GameActor actor)
	{
		return (!actor.healthHaver || !actor.healthHaver.IsBoss) && (!(actor is AIActor) || !(actor as AIActor).IsSignatureEnemy);
	}

	// Token: 0x06007242 RID: 29250 RVA: 0x002D67BC File Offset: 0x002D49BC
	public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
	{
		if (!actor || actor.healthHaver.GetCurrentHealth() <= 0f || actor.healthHaver.IsDead)
		{
			return;
		}
		effectData.OnActorPreDeath = delegate(Vector2 dir)
		{
			if (actor.IsCheezen)
			{
				this.DestroyCrystals(effectData, !actor.IsFalling);
				AkSoundEngine.PostEvent("Play_BOSS_blobulord_burst_01", actor.gameObject);
				actor.CheeseAmount = 0f;
				DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.CheeseGoop).TimedAddGoopCircle(actor.CenterPosition, this.CheeseGoopRadius, 1f, false);
				if (this.ShouldVanishOnDeath(actor))
				{
					if (actor is AIActor)
					{
						(actor as AIActor).ForceDeath(dir, false);
					}
					UnityEngine.Object.Destroy(actor.gameObject);
				}
			}
		};
		actor.healthHaver.OnPreDeath += effectData.OnActorPreDeath;
		actor.CheeseAmount += this.CheeseAmount * partialAmount;
	}

	// Token: 0x06007243 RID: 29251 RVA: 0x002D6870 File Offset: 0x002D4A70
	public override void OnDarkSoulsAccumulate(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f, Projectile sourceProjectile = null)
	{
		if (!effectData.actor.IsCheezen)
		{
			actor.CheeseAmount += this.CheeseAmount * partialAmount;
			if (actor.healthHaver.IsBoss)
			{
				actor.CheeseAmount = Mathf.Min(actor.CheeseAmount, 70f);
			}
		}
	}

	// Token: 0x06007244 RID: 29252 RVA: 0x002D68C8 File Offset: 0x002D4AC8
	public override bool IsFinished(GameActor actor, RuntimeGameActorEffectData effectData, float elapsedTime)
	{
		return actor.CheeseAmount <= 0f;
	}

	// Token: 0x06007245 RID: 29253 RVA: 0x002D68DC File Offset: 0x002D4ADC
	public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
	{
		if (actor is AIActor && actor.IsCheezen)
		{
			actor.CheeseAmount = 0f;
			if (this.AppliesTint)
			{
				actor.DeregisterOverrideColor(this.effectIdentifier);
			}
			this.DestroyCrystals(effectData, !actor.healthHaver.IsDead);
			if (actor.behaviorSpeculator)
			{
				actor.behaviorSpeculator.enabled = true;
			}
			actor.IsCheezen = false;
		}
		if (actor.aiAnimator)
		{
			actor.aiAnimator.FpsScale = 1f;
		}
		actor.healthHaver.OnPreDeath -= effectData.OnActorPreDeath;
		tk2dSpriteAnimator spriteAnimator = actor.spriteAnimator;
		if (spriteAnimator && actor.aiAnimator && spriteAnimator.CurrentClip != null && !spriteAnimator.IsPlaying(spriteAnimator.CurrentClip))
		{
			actor.aiAnimator.PlayUntilFinished(actor.spriteAnimator.CurrentClip.name, false, null, -1f, true);
		}
	}

	// Token: 0x06007246 RID: 29254 RVA: 0x002D69EC File Offset: 0x002D4BEC
	public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
	{
		if (actor && actor.CheeseAmount > 0f)
		{
			actor.CheeseAmount = Mathf.Max(0f, actor.CheeseAmount - BraveTime.DeltaTime * actor.FreezeDispelFactor);
			if (!actor.IsCheezen)
			{
				if (actor.CheeseAmount > 100f && actor.healthHaver.IsAlive)
				{
					actor.CheeseAmount = 100f;
					if (this.CheeseCrystals.Count > 0)
					{
						if (effectData.vfxObjects == null)
						{
							effectData.vfxObjects = new List<Tuple<GameObject, float>>();
						}
						int num = this.crystalNum;
						if (effectData.actor && effectData.actor.specRigidbody && effectData.actor.specRigidbody.HitboxPixelCollider != null)
						{
							float num2 = effectData.actor.specRigidbody.HitboxPixelCollider.UnitDimensions.x * effectData.actor.specRigidbody.HitboxPixelCollider.UnitDimensions.y;
							num = Mathf.Max(this.crystalNum, (int)((float)this.crystalNum * (0.5f + num2 / 4f)));
						}
						for (int i = 0; i < num; i++)
						{
							GameObject gameObject = BraveUtility.RandomElement<GameObject>(this.CheeseCrystals);
							Vector2 vector = actor.specRigidbody.HitboxPixelCollider.UnitCenter;
							Vector2 vector2 = BraveUtility.RandomVector2(-this.crystalVariation, this.crystalVariation);
							vector += vector2;
							float num3 = BraveMathCollege.QuantizeFloat(vector2.ToAngle(), 360f / (float)this.crystalRot);
							Quaternion quaternion = Quaternion.Euler(0f, 0f, num3);
							GameObject gameObject2 = SpawnManager.SpawnVFX(gameObject, vector, quaternion, true);
							gameObject2.transform.parent = actor.transform;
							tk2dSprite component = gameObject2.GetComponent<tk2dSprite>();
							if (component)
							{
								actor.sprite.AttachRenderer(component);
								component.HeightOffGround = 0.1f;
							}
							if (effectData.actor && effectData.actor.specRigidbody && effectData.actor.specRigidbody.HitboxPixelCollider != null)
							{
								Vector2 unitCenter = effectData.actor.specRigidbody.HitboxPixelCollider.UnitCenter;
								float num4 = (float)i * (360f / (float)num);
								Vector2 normalized = BraveMathCollege.DegreesToVector(num4, 1f).normalized;
								normalized.x *= effectData.actor.specRigidbody.HitboxPixelCollider.UnitDimensions.x / 2f;
								normalized.y *= effectData.actor.specRigidbody.HitboxPixelCollider.UnitDimensions.y / 2f;
								float magnitude = normalized.magnitude;
								Vector2 vector3 = unitCenter + normalized;
								vector3 += (unitCenter - vector3).normalized * (magnitude * UnityEngine.Random.Range(0.15f, 0.85f));
								gameObject2.transform.position = vector3.ToVector3ZUp(0f);
								gameObject2.transform.rotation = Quaternion.Euler(0f, 0f, num4);
							}
							effectData.vfxObjects.Add(Tuple.Create<GameObject, float>(gameObject2, num3));
						}
					}
					if (actor.behaviorSpeculator)
					{
						if (actor.behaviorSpeculator.IsInterruptable)
						{
							actor.behaviorSpeculator.InterruptAndDisable();
						}
						else
						{
							actor.behaviorSpeculator.enabled = false;
						}
					}
					if (actor is AIActor)
					{
						AIActor aiactor = actor as AIActor;
						aiactor.ClearPath();
						aiactor.BehaviorOverridesVelocity = false;
					}
					actor.IsCheezen = true;
				}
			}
			else if (actor.IsCheezen)
			{
				if (actor.CheeseAmount <= 0f)
				{
					return;
				}
				if (actor.IsFalling)
				{
					if (effectData.vfxObjects != null && effectData.vfxObjects.Count > 0)
					{
						this.DestroyCrystals(effectData, false);
					}
					if (actor.aiAnimator)
					{
						actor.aiAnimator.FpsScale = 1f;
					}
				}
			}
		}
		if (!actor.healthHaver.IsDead)
		{
			float num5 = (float)((!actor.healthHaver.IsBoss) ? 100 : 70);
			float num6 = ((!actor.IsCheezen) ? Mathf.Clamp01((100f - actor.CheeseAmount) / 100f) : 0f);
			float num7 = ((!actor.IsCheezen) ? Mathf.Clamp01(actor.CheeseAmount / num5) : 1f);
			if (actor.aiAnimator)
			{
				actor.aiAnimator.FpsScale = ((!actor.IsFalling) ? num6 : 1f);
			}
			if (this.AppliesTint)
			{
				float num8 = actor.CheeseAmount / actor.FreezeDispelFactor;
				Color color = this.TintColor;
				if (num8 < 0.1f)
				{
					color = Color.black;
				}
				else if (num8 < 0.2f)
				{
					color = Color.white;
				}
				color.a *= num7;
				actor.RegisterOverrideColor(color, this.effectIdentifier);
			}
		}
	}

	// Token: 0x06007247 RID: 29255 RVA: 0x002D6F68 File Offset: 0x002D5168
	private void DestroyCrystals(RuntimeGameActorEffectData effectData, bool playVfxExplosion = true)
	{
		if (effectData.vfxObjects == null || effectData.vfxObjects.Count == 0)
		{
			return;
		}
		Vector2 vector = Vector2.zero;
		GameActor actor = effectData.actor;
		if (actor)
		{
			vector = ((!actor.specRigidbody) ? actor.sprite.WorldCenter : actor.specRigidbody.HitboxPixelCollider.UnitCenter);
		}
		else
		{
			int num = 0;
			for (int i = 0; i < effectData.vfxObjects.Count; i++)
			{
				if (effectData.vfxObjects[i].First)
				{
					vector += effectData.vfxObjects[i].First.transform.position.XY();
					num++;
				}
			}
			if (num == 0)
			{
				return;
			}
			vector /= (float)num;
		}
		if (playVfxExplosion && this.vfxExplosion)
		{
			GameObject gameObject = SpawnManager.SpawnVFX(this.vfxExplosion, vector, Quaternion.identity);
			tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
			if (actor && component)
			{
				actor.sprite.AttachRenderer(component);
				component.HeightOffGround = 0.1f;
				component.UpdateZDepth();
			}
		}
		for (int j = 0; j < effectData.vfxObjects.Count; j++)
		{
			GameObject first = effectData.vfxObjects[j].First;
			if (first)
			{
				first.transform.parent = SpawnManager.Instance.VFX;
				DebrisObject orAddComponent = first.GetOrAddComponent<DebrisObject>();
				if (actor)
				{
					actor.sprite.AttachRenderer(orAddComponent.sprite);
				}
				orAddComponent.sprite.IsPerpendicular = true;
				orAddComponent.DontSetLayer = true;
				orAddComponent.gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
				orAddComponent.angularVelocity = Mathf.Sign(UnityEngine.Random.value - 0.5f) * 125f;
				orAddComponent.angularVelocityVariance = 60f;
				orAddComponent.decayOnBounce = 0.5f;
				orAddComponent.bounceCount = 1;
				orAddComponent.canRotate = true;
				float num2 = effectData.vfxObjects[j].Second + UnityEngine.Random.Range(-this.debrisAngleVariance, this.debrisAngleVariance);
				if (orAddComponent.name.Contains("tilt", true))
				{
					num2 += 45f;
				}
				Vector2 vector2 = BraveMathCollege.DegreesToVector(num2, 1f) * (float)UnityEngine.Random.Range(this.debrisMinForce, this.debrisMaxForce);
				Vector3 vector3 = new Vector3(vector2.x, (vector2.y >= 0f) ? 0f : vector2.y, (vector2.y <= 0f) ? 0f : vector2.y);
				float num3 = ((!actor) ? 0.75f : (first.transform.position.y - actor.specRigidbody.HitboxPixelCollider.UnitBottom));
				if (orAddComponent.minorBreakable)
				{
					orAddComponent.minorBreakable.enabled = true;
				}
				orAddComponent.Trigger(vector3, num3, 1f);
			}
		}
		effectData.vfxObjects.Clear();
	}

	// Token: 0x0400739D RID: 29597
	public float CheeseAmount = 50f;

	// Token: 0x0400739E RID: 29598
	public GoopDefinition CheeseGoop;

	// Token: 0x0400739F RID: 29599
	public float CheeseGoopRadius = 1.5f;

	// Token: 0x040073A0 RID: 29600
	public List<GameObject> CheeseCrystals;

	// Token: 0x040073A1 RID: 29601
	[NonSerialized]
	public int crystalNum = 4;

	// Token: 0x040073A2 RID: 29602
	public int crystalRot = 8;

	// Token: 0x040073A3 RID: 29603
	public Vector2 crystalVariation = new Vector2(0.05f, 0.05f);

	// Token: 0x040073A4 RID: 29604
	public int debrisMinForce = 5;

	// Token: 0x040073A5 RID: 29605
	public int debrisMaxForce = 5;

	// Token: 0x040073A6 RID: 29606
	public float debrisAngleVariance = 15f;

	// Token: 0x040073A7 RID: 29607
	public GameObject vfxExplosion;
}
