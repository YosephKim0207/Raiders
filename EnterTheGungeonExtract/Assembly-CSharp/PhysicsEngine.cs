using System;
using System.Collections.Generic;
using System.Diagnostics;
using BraveDynamicTree;
using Dungeonator;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x0200084F RID: 2127
public class PhysicsEngine : MonoBehaviour
{
	// Token: 0x170008B2 RID: 2226
	// (get) Token: 0x06002ECA RID: 11978 RVA: 0x000F2080 File Offset: 0x000F0280
	public List<SpeculativeRigidbody> AllRigidbodies
	{
		get
		{
			return this.m_rigidbodies;
		}
	}

	// Token: 0x170008B3 RID: 2227
	// (get) Token: 0x06002ECB RID: 11979 RVA: 0x000F2088 File Offset: 0x000F0288
	// (set) Token: 0x06002ECC RID: 11980 RVA: 0x000F2090 File Offset: 0x000F0290
	public static PhysicsEngine Instance
	{
		get
		{
			return PhysicsEngine.m_instance;
		}
		set
		{
			PhysicsEngine.m_instance = value;
		}
	}

	// Token: 0x170008B4 RID: 2228
	// (get) Token: 0x06002ECD RID: 11981 RVA: 0x000F2098 File Offset: 0x000F0298
	public static bool HasInstance
	{
		get
		{
			return PhysicsEngine.m_instance != null;
		}
	}

	// Token: 0x170008B5 RID: 2229
	// (get) Token: 0x06002ECE RID: 11982 RVA: 0x000F20A8 File Offset: 0x000F02A8
	// (set) Token: 0x06002ECF RID: 11983 RVA: 0x000F20B0 File Offset: 0x000F02B0
	public static bool SkipCollision { get; set; }

	// Token: 0x170008B6 RID: 2230
	// (get) Token: 0x06002ED0 RID: 11984 RVA: 0x000F20B8 File Offset: 0x000F02B8
	// (set) Token: 0x06002ED1 RID: 11985 RVA: 0x000F20C0 File Offset: 0x000F02C0
	public static bool? CollisionHaltsVelocity { get; set; }

	// Token: 0x170008B7 RID: 2231
	// (get) Token: 0x06002ED2 RID: 11986 RVA: 0x000F20C8 File Offset: 0x000F02C8
	// (set) Token: 0x06002ED3 RID: 11987 RVA: 0x000F20D0 File Offset: 0x000F02D0
	public static bool HaltRemainingMovement { get; set; }

	// Token: 0x170008B8 RID: 2232
	// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x000F20D8 File Offset: 0x000F02D8
	// (set) Token: 0x06002ED5 RID: 11989 RVA: 0x000F20E0 File Offset: 0x000F02E0
	public static Vector2? PostSliceVelocity { get; set; }

	// Token: 0x170008B9 RID: 2233
	// (get) Token: 0x06002ED6 RID: 11990 RVA: 0x000F20E8 File Offset: 0x000F02E8
	public float PixelUnitWidth
	{
		get
		{
			return 1f / (float)this.PixelsPerUnit;
		}
	}

	// Token: 0x170008BA RID: 2234
	// (get) Token: 0x06002ED7 RID: 11991 RVA: 0x000F20F8 File Offset: 0x000F02F8
	public float HalfPixelUnitWidth
	{
		get
		{
			return 0.5f / (float)this.PixelsPerUnit;
		}
	}

	// Token: 0x14000080 RID: 128
	// (add) Token: 0x06002ED8 RID: 11992 RVA: 0x000F2108 File Offset: 0x000F0308
	// (remove) Token: 0x06002ED9 RID: 11993 RVA: 0x000F2140 File Offset: 0x000F0340
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPreRigidbodyMovement;

	// Token: 0x14000081 RID: 129
	// (add) Token: 0x06002EDA RID: 11994 RVA: 0x000F2178 File Offset: 0x000F0378
	// (remove) Token: 0x06002EDB RID: 11995 RVA: 0x000F21B0 File Offset: 0x000F03B0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPostRigidbodyMovement;

	// Token: 0x06002EDC RID: 11996 RVA: 0x000F21E8 File Offset: 0x000F03E8
	private void Awake()
	{
		PhysicsEngine.m_instance = this;
		if (this.TileMap == null)
		{
			this.TileMap = UnityEngine.Object.FindObjectOfType<tk2dTileMap>();
		}
		this.m_cachedProjectileMask = CollisionMask.LayerToMask(CollisionLayer.Projectile);
	}

	// Token: 0x06002EDD RID: 11997 RVA: 0x000F2218 File Offset: 0x000F0418
	[Conditional("PROFILE_PHYSICS")]
	public static void ProfileBegin(CustomSampler sampler)
	{
	}

	// Token: 0x06002EDE RID: 11998 RVA: 0x000F221C File Offset: 0x000F041C
	[Conditional("PROFILE_PHYSICS")]
	public static void ProfileEnd(CustomSampler sampler)
	{
	}

	// Token: 0x06002EDF RID: 11999 RVA: 0x000F2220 File Offset: 0x000F0420
	private void Update()
	{
	}

	// Token: 0x06002EE0 RID: 12000 RVA: 0x000F2224 File Offset: 0x000F0424
	private void OnDestroy()
	{
		if (PhysicsEngine.m_instance == this)
		{
			PhysicsEngine.m_instance = null;
			PhysicsEngine.PendingCastResult = null;
			this.m_deregisterRigidBodies.Clear();
			PhysicsEngine.c_boundedRigidbodies.Clear();
			PhysicsEngine.m_cwrqRigidbody = null;
			PhysicsEngine.m_cwrqStepList = null;
			PhysicsEngine.m_cwrqCollisionData = null;
		}
	}

	// Token: 0x06002EE1 RID: 12001 RVA: 0x000F2274 File Offset: 0x000F0474
	private void LateUpdate()
	{
		if (Time.timeScale == 0f || BraveTime.DeltaTime == 0f)
		{
			return;
		}
		DungeonData data = GameManager.Instance.Dungeon.data;
		this.m_cachedDungeonWidth = data.Width;
		this.m_cachedDungeonHeight = data.Height;
		this.m_frameCount++;
		if (this.m_frameCount > 5)
		{
			this.SortRigidbodies();
			if (this.OnPreRigidbodyMovement != null)
			{
				this.OnPreRigidbodyMovement();
			}
			Dungeon dungeon = GameManager.Instance.Dungeon;
			for (int i = 0; i < this.m_rigidbodies.Count; i++)
			{
				SpeculativeRigidbody speculativeRigidbody = this.m_rigidbodies[i];
				if (speculativeRigidbody.isActiveAndEnabled)
				{
					List<PixelCollider> pixelColliders = speculativeRigidbody.PixelColliders;
					int count = pixelColliders.Count;
					Transform transform = speculativeRigidbody.transform;
					if (speculativeRigidbody.PhysicsRegistration == SpeculativeRigidbody.RegistrationState.Unknown)
					{
						this.InferRegistration(speculativeRigidbody);
					}
					if (speculativeRigidbody.RegenerateColliders)
					{
						speculativeRigidbody.ForceRegenerate(null, null);
					}
					if ((speculativeRigidbody.UpdateCollidersOnScale && transform.hasChanged) || speculativeRigidbody.UpdateCollidersOnRotation)
					{
						float num = 0f;
						if (speculativeRigidbody.UpdateCollidersOnRotation)
						{
							num = transform.eulerAngles.z;
						}
						Vector2 one;
						if (speculativeRigidbody.UpdateCollidersOnScale)
						{
							Vector3 localScale = transform.localScale;
							one = new Vector2(speculativeRigidbody.AxialScale.x * localScale.x, speculativeRigidbody.AxialScale.y * localScale.y);
						}
						else
						{
							one = Vector2.one;
						}
						tk2dBaseSprite sprite = speculativeRigidbody.sprite;
						if (sprite)
						{
							Vector2 vector = sprite.scale;
							one = new Vector2(one.x * Mathf.Abs(vector.x), one.y * Mathf.Abs(vector.y));
						}
						if ((speculativeRigidbody.UpdateCollidersOnRotation && num != speculativeRigidbody.LastRotation) || (speculativeRigidbody.UpdateCollidersOnScale && one != speculativeRigidbody.LastScale))
						{
							speculativeRigidbody.LastRotation = num;
							speculativeRigidbody.LastScale = one;
							for (int j = 0; j < count; j++)
							{
								pixelColliders[j].SetRotationAndScale(num, one);
							}
							speculativeRigidbody.UpdateColliderPositions();
						}
						transform.hasChanged = false;
					}
					List<SpeculativeRigidbody.TemporaryException> temporaryCollisionExceptions = speculativeRigidbody.m_temporaryCollisionExceptions;
					if (temporaryCollisionExceptions != null)
					{
						for (int k = temporaryCollisionExceptions.Count - 1; k >= 0; k--)
						{
							SpeculativeRigidbody.TemporaryException ex = temporaryCollisionExceptions[k];
							if (ex.HasEnded(speculativeRigidbody))
							{
								speculativeRigidbody.DeregisterTemporaryCollisionException(temporaryCollisionExceptions[k].SpecRigidbody);
							}
							else
							{
								temporaryCollisionExceptions[k] = ex;
							}
						}
					}
					bool flag = false;
					for (int l = 0; l < count; l++)
					{
						PixelCollider pixelCollider = pixelColliders[l];
						if (pixelCollider.ColliderGenerationMode == PixelCollider.PixelColliderGeneration.BagelCollider && !pixelCollider.BagleUseFirstFrameOnly && pixelCollider.m_lastSpriteDef != pixelCollider.Sprite.GetTrueCurrentSpriteDef())
						{
							pixelCollider.RegenerateFromBagelCollider(pixelCollider.Sprite, transform, pixelCollider.m_rotation, null, false);
							flag = true;
						}
					}
					if (flag)
					{
						PhysicsEngine.UpdatePosition(speculativeRigidbody);
					}
					if (speculativeRigidbody.HasTriggerCollisions)
					{
						speculativeRigidbody.ResetTriggerCollisionData();
					}
					if (speculativeRigidbody.HasFrameSpecificCollisionExceptions)
					{
						speculativeRigidbody.ClearFrameSpecificCollisionExceptions();
					}
					if (speculativeRigidbody.OnPreMovement != null)
					{
						speculativeRigidbody.OnPreMovement(speculativeRigidbody);
					}
				}
			}
			if (this.m_nbt.tileMap == null)
			{
				this.m_nbt.tileMap = this.TileMap;
				this.m_nbt.layerName = "Collision Layer";
				this.m_nbt.layer = BraveUtility.GetTileMapLayerByName("Collision Layer", this.TileMap);
			}
			float deltaTime = BraveTime.DeltaTime;
			for (int m = 0; m < this.m_rigidbodies.Count; m++)
			{
				SpeculativeRigidbody speculativeRigidbody2 = this.m_rigidbodies[m];
				if (speculativeRigidbody2 && speculativeRigidbody2.isActiveAndEnabled)
				{
					Position position = speculativeRigidbody2.m_position;
					Vector2 vector2 = new Vector2((float)position.m_position.x * 0.0625f + position.m_remainder.x, (float)position.m_position.y * 0.0625f + position.m_remainder.y);
					IntVector2 position2 = position.m_position;
					if (speculativeRigidbody2.CapVelocity)
					{
						Vector2 maxVelocity = speculativeRigidbody2.MaxVelocity;
						if (Mathf.Abs(speculativeRigidbody2.Velocity.x) > maxVelocity.x)
						{
							speculativeRigidbody2.Velocity.x = Mathf.Sign(speculativeRigidbody2.Velocity.x) * maxVelocity.x;
						}
						if (Mathf.Abs(speculativeRigidbody2.Velocity.y) > maxVelocity.y)
						{
							speculativeRigidbody2.Velocity.y = Mathf.Sign(speculativeRigidbody2.Velocity.y) * maxVelocity.y;
						}
					}
					if (float.IsNaN(speculativeRigidbody2.Velocity.x))
					{
						speculativeRigidbody2.Velocity.x = 0f;
					}
					else
					{
						speculativeRigidbody2.Velocity.x = Mathf.Clamp(speculativeRigidbody2.Velocity.x, -1000f, 1000f);
					}
					if (float.IsNaN(speculativeRigidbody2.Velocity.y))
					{
						speculativeRigidbody2.Velocity.y = 0f;
					}
					else
					{
						speculativeRigidbody2.Velocity.y = Mathf.Clamp(speculativeRigidbody2.Velocity.y, -1000f, 1000f);
					}
					speculativeRigidbody2.TimeRemaining = deltaTime;
					if (speculativeRigidbody2.Velocity == Vector2.zero && speculativeRigidbody2.ImpartedPixelsToMove == IntVector2.Zero && !speculativeRigidbody2.ForceAlwaysUpdate && (!speculativeRigidbody2.PathMode || speculativeRigidbody2.PathSpeed == 0f))
					{
						speculativeRigidbody2.TimeRemaining = 0f;
					}
					Vector2? vector3 = null;
					List<SpeculativeRigidbody.PushedRigidbodyData> pushedRigidbodies = speculativeRigidbody2.m_pushedRigidbodies;
					int count2 = pushedRigidbodies.Count;
					for (int n = 0; n < count2; n++)
					{
						SpeculativeRigidbody.PushedRigidbodyData pushedRigidbodyData = pushedRigidbodies[n];
						pushedRigidbodyData.PushedThisFrame = false;
						pushedRigidbodies[n] = pushedRigidbodyData;
					}
					if (speculativeRigidbody2.PathMode)
					{
						speculativeRigidbody2.Velocity = (PhysicsEngine.PixelToUnit(speculativeRigidbody2.PathTarget) - speculativeRigidbody2.Position.UnitPosition).normalized * speculativeRigidbody2.PathSpeed;
					}
					int num2 = 0;
					while (speculativeRigidbody2.TimeRemaining > 0f && num2 < 50)
					{
						if (vector3 != null)
						{
							speculativeRigidbody2.Velocity = vector3.Value;
							vector3 = null;
						}
						float timeRemaining = speculativeRigidbody2.TimeRemaining;
						float num3 = timeRemaining;
						bool flag2 = false;
						IntVector2 impartedPixelsToMove = speculativeRigidbody2.ImpartedPixelsToMove;
						if (impartedPixelsToMove.x != 0 || impartedPixelsToMove.y != 0)
						{
							flag2 = true;
							num3 = 0f;
							speculativeRigidbody2.PixelsToMove = impartedPixelsToMove;
						}
						else
						{
							Vector2 velocity = speculativeRigidbody2.Velocity;
							Vector2 vector4 = new Vector2(velocity.x * timeRemaining, velocity.y * timeRemaining);
							IntVector2 position3 = speculativeRigidbody2.m_position.m_position;
							Vector2 remainder = speculativeRigidbody2.m_position.m_remainder;
							speculativeRigidbody2.PixelsToMove = new IntVector2(Mathf.RoundToInt(((float)position3.x * 0.0625f + remainder.x + vector4.x) * 16f) - position3.x, Mathf.RoundToInt(((float)position3.y * 0.0625f + remainder.y + vector4.y) * 16f) - position3.y);
						}
						speculativeRigidbody2.CollidedX = false;
						speculativeRigidbody2.CollidedY = false;
						CollisionData collisionData = null;
						bool flag3 = true;
						bool flag4 = true;
						List<PixelCollider.StepData> stepList = PixelCollider.m_stepList;
						if (flag2)
						{
							PhysicsEngine.PixelMovementGenerator(speculativeRigidbody2.PixelsToMove, stepList);
						}
						else
						{
							PhysicsEngine.PixelMovementGenerator(speculativeRigidbody2.m_position.m_remainder, speculativeRigidbody2.Velocity, speculativeRigidbody2.PixelsToMove, stepList);
						}
						if (speculativeRigidbody2.PathMode)
						{
							float num4 = Vector2.Distance(PhysicsEngine.PixelToUnit(speculativeRigidbody2.PathTarget), speculativeRigidbody2.m_position.UnitPosition) / speculativeRigidbody2.PathSpeed;
							if (num4 <= timeRemaining && (collisionData == null || num4 < collisionData.TimeUsed))
							{
								CollisionData.Pool.Free(ref collisionData);
								if (collisionData == null)
								{
									collisionData = CollisionData.Pool.Allocate();
								}
								collisionData.collisionType = CollisionData.CollisionType.PathEnd;
								collisionData.NewPixelsToMove = speculativeRigidbody2.PathTarget - speculativeRigidbody2.m_position.PixelPosition;
								collisionData.CollidedX = speculativeRigidbody2.Velocity.x != 0f;
								collisionData.CollidedY = speculativeRigidbody2.Velocity.y != 0f;
								collisionData.MyRigidbody = speculativeRigidbody2;
								collisionData.MyPixelCollider = speculativeRigidbody2.PrimaryPixelCollider;
								collisionData.TimeUsed = num4;
							}
						}
						if (speculativeRigidbody2.MovementRestrictor != null)
						{
							IntVector2 intVector = IntVector2.Zero;
							IntVector2 intVector2 = IntVector2.Zero;
							for (int num5 = 0; num5 < stepList.Count; num5++)
							{
								bool flag5 = true;
								intVector2 += stepList[num5].deltaPos;
								speculativeRigidbody2.MovementRestrictor(speculativeRigidbody2, intVector, intVector2, ref flag5);
								if (!flag5)
								{
									float num6 = 0f;
									for (int num7 = 0; num7 <= num5; num7++)
									{
										num6 += stepList[num7].deltaTime;
									}
									if (collisionData == null || num6 < collisionData.TimeUsed)
									{
										CollisionData.Pool.Free(ref collisionData);
										if (collisionData == null)
										{
											collisionData = CollisionData.Pool.Allocate();
										}
										collisionData.collisionType = CollisionData.CollisionType.MovementRestriction;
										collisionData.NewPixelsToMove = intVector2 - stepList[num5].deltaPos;
										collisionData.CollidedX = stepList[num5].deltaPos.x != 0;
										collisionData.CollidedY = stepList[num5].deltaPos.y != 0;
										collisionData.MyRigidbody = speculativeRigidbody2;
										collisionData.MyPixelCollider = speculativeRigidbody2.PrimaryPixelCollider;
										collisionData.TimeUsed = num6;
									}
									break;
								}
								intVector = intVector2;
							}
						}
						if (speculativeRigidbody2.CanPush && speculativeRigidbody2.PushedRigidbodies.Count > 0)
						{
							for (int num8 = 0; num8 < speculativeRigidbody2.PushedRigidbodies.Count; num8++)
							{
								SpeculativeRigidbody.PushedRigidbodyData pushedRigidbodyData2 = speculativeRigidbody2.PushedRigidbodies[num8];
								if (pushedRigidbodyData2.PushedThisFrame)
								{
									IntVector2 pushedPixelsToMove = pushedRigidbodyData2.GetPushedPixelsToMove(speculativeRigidbody2.PixelsToMove);
									CollisionData collisionData2;
									if (this.RigidbodyCast(pushedRigidbodyData2.SpecRigidbody, pushedPixelsToMove, out collisionData2, true, true, null, false))
									{
										collisionData2.collisionType = CollisionData.CollisionType.Pushable;
										collisionData2.CollidedX = pushedRigidbodyData2.CollidedX;
										collisionData2.CollidedY = pushedRigidbodyData2.CollidedY;
										collisionData2.TimeUsed = 0f;
										collisionData2.NewPixelsToMove = IntVector2.Zero;
										for (int num9 = 0; num9 < stepList.Count; num9++)
										{
											if (pushedRigidbodyData2.GetPushedPixelsToMove(collisionData2.NewPixelsToMove) == collisionData2.NewPixelsToMove)
											{
												break;
											}
											collisionData2.NewPixelsToMove += stepList[num9].deltaPos;
											collisionData2.TimeUsed += stepList[num9].deltaTime;
										}
										collisionData2.IsPushCollision = true;
										if (collisionData == null || collisionData2.TimeUsed < collisionData.TimeUsed)
										{
											collisionData = collisionData2;
										}
									}
								}
							}
							if (flag2)
							{
								PhysicsEngine.PixelMovementGenerator(speculativeRigidbody2.PixelsToMove, stepList);
							}
							else
							{
								PhysicsEngine.PixelMovementGenerator(speculativeRigidbody2, stepList);
							}
						}
						if (speculativeRigidbody2.CollideWithOthers)
						{
							this.CollideWithRigidbodies(speculativeRigidbody2, stepList, ref collisionData);
						}
						if (speculativeRigidbody2.CollideWithTileMap && dungeon != null && this.TileMap != null)
						{
							List<PixelCollider> pixelColliders2 = speculativeRigidbody2.PixelColliders;
							for (int num10 = 0; num10 < pixelColliders2.Count; num10++)
							{
								PixelCollider pixelCollider2 = pixelColliders2[num10];
								if (pixelCollider2.Enabled && (pixelCollider2.CollisionLayer == CollisionLayer.TileBlocker || (CollisionLayerMatrix.GetMask(pixelCollider2.CollisionLayer) & 64) == 64))
								{
									this.CollideWithTilemap(speculativeRigidbody2, pixelCollider2, stepList, ref num3, data, ref collisionData);
								}
							}
						}
						if (speculativeRigidbody2.CanPush && speculativeRigidbody2.PushedRigidbodies.Count > 0)
						{
							IntVector2 intVector3 = ((collisionData == null) ? speculativeRigidbody2.PixelsToMove : collisionData.NewPixelsToMove);
							for (int num11 = 0; num11 < speculativeRigidbody2.PushedRigidbodies.Count; num11++)
							{
								SpeculativeRigidbody.PushedRigidbodyData pushedRigidbodyData3 = speculativeRigidbody2.PushedRigidbodies[num11];
								if (pushedRigidbodyData3.PushedThisFrame)
								{
									SpeculativeRigidbody specRigidbody = pushedRigidbodyData3.SpecRigidbody;
									IntVector2 pushedPixelsToMove2 = pushedRigidbodyData3.GetPushedPixelsToMove(intVector3);
									Position position4 = specRigidbody.Position;
									position4.PixelPosition += pushedPixelsToMove2;
									specRigidbody.Position = position4;
									specRigidbody.transform.position = specRigidbody.Position.GetPixelVector2().ToVector3ZUp(specRigidbody.transform.position.z);
									if (specRigidbody.OnPostRigidbodyMovement != null)
									{
										specRigidbody.OnPostRigidbodyMovement(specRigidbody, PhysicsEngine.PixelToUnit(pushedPixelsToMove2), pushedPixelsToMove2);
									}
								}
							}
							if (collisionData != null && collisionData.IsPushCollision && collisionData.OtherRigidbody)
							{
								MinorBreakable minorBreakable = collisionData.OtherRigidbody.minorBreakable;
								if (minorBreakable && !minorBreakable.isInvulnerableToGameActors)
								{
									minorBreakable.Break(-collisionData.Normal);
								}
							}
						}
						if (collisionData != null)
						{
							if (!collisionData.Overlap && speculativeRigidbody2.CanPush && collisionData.OtherRigidbody != null && collisionData.OtherRigidbody.CanBePushed)
							{
								int num12 = -1;
								for (int num13 = 0; num13 < speculativeRigidbody2.PushedRigidbodies.Count; num13++)
								{
									if (speculativeRigidbody2.PushedRigidbodies[num13].SpecRigidbody == collisionData.OtherRigidbody)
									{
										num12 = num13;
										break;
									}
								}
								if (num12 < 0)
								{
									num12 = speculativeRigidbody2.PushedRigidbodies.Count;
									speculativeRigidbody2.PushedRigidbodies.Add(new SpeculativeRigidbody.PushedRigidbodyData(collisionData.OtherRigidbody));
								}
								else
								{
									collisionData.TimeUsed = 0f;
								}
								SpeculativeRigidbody.PushedRigidbodyData pushedRigidbodyData4 = speculativeRigidbody2.PushedRigidbodies[num12];
								pushedRigidbodyData4.Direction = ((!collisionData.CollidedX) ? IntVector2.Up : IntVector2.Right);
								pushedRigidbodyData4.PushedThisFrame = true;
								speculativeRigidbody2.PushedRigidbodies[num12] = pushedRigidbodyData4;
								collisionData.MyPixelCollider.RegisterFrameSpecificCollisionException(collisionData.MyRigidbody, collisionData.OtherPixelCollider);
								flag3 = false;
								flag4 = false;
								vector3 = new Vector2?(speculativeRigidbody2.Velocity);
								if (collisionData.CollidedX)
								{
									vector3 = new Vector2?(vector3.Value.WithX(vector3.Value.x * speculativeRigidbody2.PushSpeedModifier));
								}
								if (collisionData.CollidedY)
								{
									vector3 = new Vector2?(vector3.Value.WithY(vector3.Value.y * speculativeRigidbody2.PushSpeedModifier));
								}
							}
							PhysicsEngine.CollisionHaltsVelocity = null;
							PhysicsEngine.HaltRemainingMovement = false;
							PhysicsEngine.PostSliceVelocity = null;
							CollisionData collisionData3 = null;
							if (!collisionData.IsTriggerCollision)
							{
								if (speculativeRigidbody2.OnCollision != null)
								{
									speculativeRigidbody2.OnCollision(collisionData);
								}
								if (collisionData.OtherRigidbody != null && collisionData.OtherRigidbody.OnCollision != null)
								{
									if (collisionData3 == null)
									{
										collisionData3 = collisionData.GetInverse();
									}
									collisionData.OtherRigidbody.OnCollision(collisionData.GetInverse());
								}
							}
							if (collisionData.OtherRigidbody != null)
							{
								if (!collisionData.IsTriggerCollision)
								{
									if (speculativeRigidbody2.OnRigidbodyCollision != null)
									{
										speculativeRigidbody2.OnRigidbodyCollision(collisionData);
									}
									if (collisionData.OtherRigidbody.OnRigidbodyCollision != null)
									{
										if (collisionData3 == null)
										{
											collisionData3 = collisionData.GetInverse();
										}
										collisionData.OtherRigidbody.OnRigidbodyCollision(collisionData.GetInverse());
									}
								}
							}
							else if (collisionData.TileLayerName != null && speculativeRigidbody2.OnTileCollision != null)
							{
								speculativeRigidbody2.OnTileCollision(collisionData);
							}
							if (PhysicsEngine.CollisionHaltsVelocity != null)
							{
								flag3 = PhysicsEngine.CollisionHaltsVelocity.Value;
							}
							if (collisionData.OtherRigidbody != null && collisionData.IsTriggerCollision)
							{
								SpeculativeRigidbody otherRigidbody = collisionData.OtherRigidbody;
								speculativeRigidbody2.PixelsToMove = collisionData.NewPixelsToMove;
								speculativeRigidbody2.CollidedX = collisionData.CollidedX;
								speculativeRigidbody2.CollidedY = collisionData.CollidedY;
								num3 = collisionData.TimeUsed;
								flag3 = false;
								flag4 = false;
								collisionData.MyPixelCollider.RegisterFrameSpecificCollisionException(speculativeRigidbody2, collisionData.OtherPixelCollider);
								TriggerCollisionData triggerCollisionData = collisionData.MyPixelCollider.RegisterTriggerCollision(collisionData.MyRigidbody, collisionData.OtherRigidbody, collisionData.OtherPixelCollider);
								TriggerCollisionData triggerCollisionData2 = collisionData.OtherPixelCollider.RegisterTriggerCollision(collisionData.MyRigidbody, collisionData.MyRigidbody, collisionData.MyPixelCollider);
								if (triggerCollisionData.FirstFrame)
								{
									if (speculativeRigidbody2.OnEnterTrigger != null)
									{
										speculativeRigidbody2.OnEnterTrigger(otherRigidbody, speculativeRigidbody2, collisionData);
									}
									if (otherRigidbody.OnEnterTrigger != null)
									{
										if (collisionData3 == null)
										{
											collisionData3 = collisionData.GetInverse();
										}
										otherRigidbody.OnEnterTrigger(speculativeRigidbody2, otherRigidbody, collisionData.GetInverse());
									}
								}
								if (triggerCollisionData.FirstFrame || triggerCollisionData.ContinuedCollision)
								{
									if (speculativeRigidbody2.OnTriggerCollision != null)
									{
										speculativeRigidbody2.OnTriggerCollision(otherRigidbody, speculativeRigidbody2, collisionData);
									}
									if (otherRigidbody.OnTriggerCollision != null)
									{
										if (collisionData3 == null)
										{
											collisionData3 = collisionData.GetInverse();
										}
										otherRigidbody.OnTriggerCollision(speculativeRigidbody2, otherRigidbody, collisionData.GetInverse());
									}
								}
								triggerCollisionData.Notified = true;
								triggerCollisionData2.Notified = true;
							}
							else if (collisionData.OtherRigidbody && (speculativeRigidbody2.IsGhostCollisionException(collisionData.OtherRigidbody) || collisionData.OtherRigidbody.IsGhostCollisionException(speculativeRigidbody2)))
							{
								if (!collisionData.Overlap)
								{
									speculativeRigidbody2.PixelsToMove = collisionData.NewPixelsToMove;
									num3 = collisionData.TimeUsed;
								}
								else
								{
									speculativeRigidbody2.PixelsToMove = IntVector2.Zero;
									num3 = 0f;
								}
								collisionData.MyPixelCollider.RegisterFrameSpecificCollisionException(speculativeRigidbody2, collisionData.OtherPixelCollider);
							}
							else
							{
								speculativeRigidbody2.CollidedX = collisionData.CollidedX;
								speculativeRigidbody2.CollidedY = collisionData.CollidedY;
								speculativeRigidbody2.PixelsToMove = collisionData.NewPixelsToMove;
								num3 = collisionData.TimeUsed;
							}
							if (collisionData3 != null)
							{
								CollisionData.Pool.Free(ref collisionData3);
							}
							if (!flag2 && collisionData.collisionType != CollisionData.CollisionType.PathEnd)
							{
								float num14 = PhysicsEngine.PixelToUnit(1) / 2f;
								if (speculativeRigidbody2.CollidedX && !speculativeRigidbody2.CollidedY)
								{
									num3 = Mathf.Max(0f, num3 - Mathf.Abs(num14 / speculativeRigidbody2.Velocity.x));
								}
								else if (speculativeRigidbody2.CollidedY && !speculativeRigidbody2.CollidedX)
								{
									num3 = Mathf.Max(0f, num3 - Mathf.Abs(num14 / speculativeRigidbody2.Velocity.y));
								}
							}
						}
						if (flag2)
						{
							num3 = 0f;
							speculativeRigidbody2.Position = new Position(speculativeRigidbody2.Position.PixelPosition + speculativeRigidbody2.PixelsToMove, speculativeRigidbody2.Position.Remainder);
							speculativeRigidbody2.ImpartedPixelsToMove -= speculativeRigidbody2.PixelsToMove;
							if (collisionData == null || !collisionData.IsTriggerCollision)
							{
								if (speculativeRigidbody2.CollidedX)
								{
									speculativeRigidbody2.ImpartedPixelsToMove = speculativeRigidbody2.ImpartedPixelsToMove.WithX(0);
								}
								if (speculativeRigidbody2.CollidedY)
								{
									speculativeRigidbody2.ImpartedPixelsToMove = speculativeRigidbody2.ImpartedPixelsToMove.WithY(0);
								}
							}
						}
						else
						{
							Position position5 = speculativeRigidbody2.Position;
							if (speculativeRigidbody2.CollidedX && flag4)
							{
								position5.X += speculativeRigidbody2.PixelsToMove.x;
							}
							else
							{
								position5.UnitX += speculativeRigidbody2.Velocity.x * num3;
							}
							if (speculativeRigidbody2.CollidedY && flag4)
							{
								position5.Y += speculativeRigidbody2.PixelsToMove.y;
							}
							else
							{
								position5.UnitY += speculativeRigidbody2.Velocity.y * num3;
							}
							if (flag3)
							{
								if (speculativeRigidbody2.CollidedX)
								{
									speculativeRigidbody2.Velocity.x = 0f;
								}
								if (speculativeRigidbody2.CollidedY)
								{
									speculativeRigidbody2.Velocity.y = 0f;
								}
							}
							if (PhysicsEngine.PostSliceVelocity != null)
							{
								speculativeRigidbody2.Velocity = PhysicsEngine.PostSliceVelocity.Value;
								PhysicsEngine.PostSliceVelocity = null;
							}
							speculativeRigidbody2.Position = position5;
						}
						if (speculativeRigidbody2.CarriedRigidbodies != null)
						{
							for (int num15 = 0; num15 < speculativeRigidbody2.CarriedRigidbodies.Count; num15++)
							{
								SpeculativeRigidbody speculativeRigidbody3 = speculativeRigidbody2.CarriedRigidbodies[num15];
								if (speculativeRigidbody3.CanBeCarried || speculativeRigidbody2.ForceCarriesRigidbodies)
								{
									speculativeRigidbody3.ImpartedPixelsToMove += speculativeRigidbody2.PixelsToMove;
								}
							}
						}
						if (speculativeRigidbody2.IgnorePixelGrid)
						{
							IntVector2 position6 = speculativeRigidbody2.m_position.m_position;
							Vector2 remainder2 = speculativeRigidbody2.m_position.m_remainder;
							Transform transform2 = speculativeRigidbody2.transform;
							transform2.position = new Vector3((float)position6.x * 0.0625f + remainder2.x, (float)position6.y * 0.0625f + remainder2.y, transform2.position.z);
						}
						else
						{
							IntVector2 position7 = speculativeRigidbody2.Position.m_position;
							Transform transform3 = speculativeRigidbody2.transform;
							transform3.position = new Vector3((float)position7.x * 0.0625f, (float)position7.y * 0.0625f, transform3.position.z);
						}
						speculativeRigidbody2.TimeRemaining -= num3;
						if (speculativeRigidbody2.PathMode && collisionData != null && collisionData.collisionType == CollisionData.CollisionType.PathEnd)
						{
							if (speculativeRigidbody2.OnPathTargetReached != null)
							{
								speculativeRigidbody2.OnPathTargetReached();
								if (speculativeRigidbody2.PathMode)
								{
									speculativeRigidbody2.Velocity = (PhysicsEngine.PixelToUnit(speculativeRigidbody2.PathTarget) - speculativeRigidbody2.Position.UnitPosition).normalized * speculativeRigidbody2.PathSpeed;
								}
							}
							else
							{
								speculativeRigidbody2.PathMode = false;
							}
						}
						if (PhysicsEngine.HaltRemainingMovement || (speculativeRigidbody2.Velocity == Vector2.zero && speculativeRigidbody2.ImpartedPixelsToMove == IntVector2.Zero && !speculativeRigidbody2.PathMode && !speculativeRigidbody2.HasUnresolvedTriggerCollisions))
						{
							speculativeRigidbody2.TimeRemaining = 0f;
						}
						num2++;
						if (collisionData != null)
						{
							CollisionData.Pool.Free(ref collisionData);
						}
					}
					List<SpeculativeRigidbody.PushedRigidbodyData> pushedRigidbodies2 = speculativeRigidbody2.m_pushedRigidbodies;
					for (int num16 = pushedRigidbodies2.Count - 1; num16 >= 0; num16--)
					{
						if (!pushedRigidbodies2[num16].PushedThisFrame)
						{
							pushedRigidbodies2.RemoveAt(num16);
						}
					}
					IntVector2 intVector4 = speculativeRigidbody2.Position.m_position - position2;
					if (speculativeRigidbody2.OnPostRigidbodyMovement != null)
					{
						speculativeRigidbody2.OnPostRigidbodyMovement(speculativeRigidbody2, speculativeRigidbody2.m_position.UnitPosition - vector2, intVector4);
					}
					if (speculativeRigidbody2.TK2DSprite != null && (speculativeRigidbody2.TK2DSprite.IsZDepthDirty || intVector4.x != 0 || intVector4.y != 0))
					{
						speculativeRigidbody2.TK2DSprite.UpdateZDepth();
					}
					speculativeRigidbody2.RecheckTriggers = false;
				}
			}
			if (this.OnPostRigidbodyMovement != null)
			{
				this.OnPostRigidbodyMovement();
			}
			for (int num17 = 0; num17 < this.m_rigidbodies.Count; num17++)
			{
				SpeculativeRigidbody speculativeRigidbody4 = this.m_rigidbodies[num17];
				if (speculativeRigidbody4 && speculativeRigidbody4.HasTriggerCollisions)
				{
					for (int num18 = 0; num18 < speculativeRigidbody4.PixelColliders.Count; num18++)
					{
						PixelCollider pixelCollider3 = speculativeRigidbody4.PixelColliders[num18];
						for (int num19 = pixelCollider3.TriggerCollisions.Count - 1; num19 >= 0; num19--)
						{
							TriggerCollisionData triggerCollisionData3 = pixelCollider3.TriggerCollisions[num19];
							PixelCollider pixelCollider4 = triggerCollisionData3.PixelCollider;
							SpeculativeRigidbody specRigidbody2 = triggerCollisionData3.SpecRigidbody;
							if (!triggerCollisionData3.Notified)
							{
								if (!triggerCollisionData3.FirstFrame && !triggerCollisionData3.ContinuedCollision)
								{
									if (speculativeRigidbody4.OnExitTrigger != null)
									{
										speculativeRigidbody4.OnExitTrigger(specRigidbody2, speculativeRigidbody4);
									}
									if (specRigidbody2.OnExitTrigger != null)
									{
										specRigidbody2.OnExitTrigger(speculativeRigidbody4, specRigidbody2);
									}
								}
								triggerCollisionData3.Notified = true;
								for (int num20 = 0; num20 < pixelCollider4.TriggerCollisions.Count; num20++)
								{
									if (pixelCollider4.TriggerCollisions[num20].PixelCollider == pixelCollider3)
									{
										pixelCollider4.TriggerCollisions[num20].Notified = true;
										break;
									}
								}
							}
							if (!triggerCollisionData3.FirstFrame && !triggerCollisionData3.ContinuedCollision)
							{
								pixelCollider3.TriggerCollisions.RemoveAt(num19);
								for (int num21 = 0; num21 < pixelCollider4.TriggerCollisions.Count; num21++)
								{
									if (pixelCollider4.TriggerCollisions[num21].PixelCollider == pixelCollider3)
									{
										pixelCollider4.TriggerCollisions.RemoveAt(num21);
										num21--;
									}
								}
							}
						}
					}
				}
			}
			for (int num22 = 0; num22 < this.m_rigidbodies.Count; num22++)
			{
				SpeculativeRigidbody speculativeRigidbody5 = this.m_rigidbodies[num22];
				if (speculativeRigidbody5.isActiveAndEnabled)
				{
					List<SpeculativeRigidbody> ghostCollisionExceptions = speculativeRigidbody5.GhostCollisionExceptions;
					if (ghostCollisionExceptions != null)
					{
						for (int num23 = 0; num23 < ghostCollisionExceptions.Count; num23++)
						{
							SpeculativeRigidbody speculativeRigidbody6 = ghostCollisionExceptions[num23];
							bool flag6 = false;
							if (speculativeRigidbody6)
							{
								int num24 = 0;
								while (num24 < speculativeRigidbody5.PixelColliders.Count && !flag6)
								{
									PixelCollider pixelCollider5 = speculativeRigidbody5.PixelColliders[num24];
									int num25 = 0;
									while (num25 < speculativeRigidbody6.PixelColliders.Count && !flag6)
									{
										PixelCollider pixelCollider6 = speculativeRigidbody6.PixelColliders[num25];
										if (pixelCollider5.CanCollideWith(pixelCollider6, true))
										{
											flag6 |= pixelCollider5.Overlaps(pixelCollider6);
										}
										num25++;
									}
									num24++;
								}
							}
							if (!flag6)
							{
								speculativeRigidbody5.DeregisterGhostCollisionException(num23);
								num23--;
							}
						}
					}
				}
			}
			if (this.DebugDraw != PhysicsEngine.DebugDrawType.None)
			{
				this.m_debugTilesDrawnThisFrame.Clear();
			}
		}
		for (int num26 = 0; num26 < this.m_deregisterRigidBodies.Count; num26++)
		{
			this.Deregister(this.m_deregisterRigidBodies[num26]);
		}
		this.m_deregisterRigidBodies.Clear();
	}

	// Token: 0x06002EE2 RID: 12002 RVA: 0x000F3FC8 File Offset: 0x000F21C8
	public void Query(Vector2 worldMin, Vector2 worldMax, Func<SpeculativeRigidbody, bool> callback)
	{
		this.m_rigidbodyTree.Query(PhysicsEngine.GetSafeB2AABB(PhysicsEngine.UnitToPixel(worldMin), PhysicsEngine.UnitToPixel(worldMax)), callback);
	}

	// Token: 0x06002EE3 RID: 12003 RVA: 0x000F3FE8 File Offset: 0x000F21E8
	public bool Raycast(Vector2 unitOrigin, Vector2 direction, float dist, out RaycastResult result, bool collideWithTiles = true, bool collideWithRigidbodies = true, int rayMask = 2147483647, CollisionLayer? sourceLayer = null, bool collideWithTriggers = false, Func<SpeculativeRigidbody, bool> rigidbodyExcluder = null, SpeculativeRigidbody ignoreRigidbody = null)
	{
		bool flag;
		if (ignoreRigidbody == null)
		{
			flag = this.RaycastWithIgnores(new Position(unitOrigin), direction, dist, out result, collideWithTiles, collideWithRigidbodies, rayMask, sourceLayer, collideWithTriggers, rigidbodyExcluder, this.m_emptyIgnoreList);
		}
		else
		{
			this.m_singleIgnoreList[0] = ignoreRigidbody;
			flag = this.RaycastWithIgnores(new Position(unitOrigin), direction, dist, out result, collideWithTiles, collideWithRigidbodies, rayMask, sourceLayer, collideWithTriggers, rigidbodyExcluder, this.m_singleIgnoreList);
			this.m_singleIgnoreList[0] = null;
		}
		return flag;
	}

	// Token: 0x06002EE4 RID: 12004 RVA: 0x000F4064 File Offset: 0x000F2264
	public bool RaycastWithIgnores(Vector2 unitOrigin, Vector2 direction, float dist, out RaycastResult result, bool collideWithTiles = true, bool collideWithRigidbodies = true, int rayMask = 2147483647, CollisionLayer? sourceLayer = null, bool collideWithTriggers = false, Func<SpeculativeRigidbody, bool> rigidbodyExcluder = null, ICollection<SpeculativeRigidbody> ignoreList = null)
	{
		return this.RaycastWithIgnores(new Position(unitOrigin), direction, dist, out result, collideWithTiles, collideWithRigidbodies, rayMask, sourceLayer, collideWithTriggers, rigidbodyExcluder, ignoreList);
	}

	// Token: 0x06002EE5 RID: 12005 RVA: 0x000F4090 File Offset: 0x000F2290
	public bool RaycastWithIgnores(Position origin, Vector2 direction, float dist, out RaycastResult result, bool collideWithTiles = true, bool collideWithRigidbodies = true, int rayMask = 2147483647, CollisionLayer? sourceLayer = null, bool collideWithTriggers = false, Func<SpeculativeRigidbody, bool> rigidbodyExcluder = null, ICollection<SpeculativeRigidbody> ignoreList = null)
	{
		PhysicsEngine.m_raycaster.SetAll(this, GameManager.Instance.Dungeon.data, origin, direction, dist, collideWithTiles, collideWithRigidbodies, rayMask, sourceLayer, collideWithTriggers, rigidbodyExcluder, ignoreList);
		bool flag = PhysicsEngine.m_raycaster.DoRaycast(out result);
		PhysicsEngine.m_raycaster.Clear();
		return flag;
	}

	// Token: 0x06002EE6 RID: 12006 RVA: 0x000F40E0 File Offset: 0x000F22E0
	public bool Pointcast(Vector2 point, out SpeculativeRigidbody result, bool collideWithTiles = true, bool collideWithRigidbodies = true, int rayMask = 2147483647, CollisionLayer? sourceLayer = null, bool collideWithTriggers = false, params SpeculativeRigidbody[] ignoreList)
	{
		return this.Pointcast(PhysicsEngine.UnitToPixel(point), out result, collideWithTiles, collideWithRigidbodies, rayMask, sourceLayer, collideWithTriggers, ignoreList);
	}

	// Token: 0x06002EE7 RID: 12007 RVA: 0x000F4108 File Offset: 0x000F2308
	public bool Pointcast(IntVector2 point, out SpeculativeRigidbody result, bool collideWithTiles = true, bool collideWithRigidbodies = true, int rayMask = 2147483647, CollisionLayer? sourceLayer = null, bool collideWithTriggers = false, params SpeculativeRigidbody[] ignoreList)
	{
		ICollidableObject tempResult = null;
		Func<ICollidableObject, IntVector2, ICollidableObject> collideWithCollidable = delegate(ICollidableObject collidable, IntVector2 p)
		{
			SpeculativeRigidbody speculativeRigidbody = collidable as SpeculativeRigidbody;
			if (speculativeRigidbody && !speculativeRigidbody.enabled)
			{
				return null;
			}
			for (int i = 0; i < collidable.GetPixelColliders().Count; i++)
			{
				PixelCollider pixelCollider = collidable.GetPixelColliders()[i];
				if (collideWithTriggers || !pixelCollider.IsTrigger)
				{
					if (pixelCollider.CanCollideWith(rayMask, sourceLayer) && pixelCollider.ContainsPixel(p))
					{
						return collidable;
					}
				}
			}
			return null;
		};
		if (collideWithTiles && this.TileMap)
		{
			int num;
			int num2;
			this.TileMap.GetTileAtPosition(PhysicsEngine.PixelToUnit(point), out num, out num2);
			int tileMapLayerByName = BraveUtility.GetTileMapLayerByName("Collision Layer", this.TileMap);
			PhysicsEngine.Tile tile = this.GetTile(num, num2, this.TileMap, tileMapLayerByName, "Collision Layer", GameManager.Instance.Dungeon.data);
			if (tile != null)
			{
				tempResult = collideWithCollidable(tile, point);
				if (tempResult != null)
				{
					result = tempResult as SpeculativeRigidbody;
					return true;
				}
			}
		}
		if (collideWithRigidbodies)
		{
			Func<SpeculativeRigidbody, bool> func = delegate(SpeculativeRigidbody rigidbody)
			{
				tempResult = collideWithCollidable(rigidbody, point);
				return tempResult == null;
			};
			this.m_rigidbodyTree.Query(PhysicsEngine.GetSafeB2AABB(point, point), func);
			if (this.CollidesWithProjectiles(rayMask, sourceLayer))
			{
				this.m_projectileTree.Query(PhysicsEngine.GetSafeB2AABB(point, point), func);
			}
		}
		result = tempResult as SpeculativeRigidbody;
		return result != null;
	}

	// Token: 0x06002EE8 RID: 12008 RVA: 0x000F4278 File Offset: 0x000F2478
	private static b2AABB GetSafeB2AABB(IntVector2 lowerBounds, IntVector2 upperBounds)
	{
		return new b2AABB(PhysicsEngine.PixelToUnit(lowerBounds - IntVector2.One), PhysicsEngine.PixelToUnit(upperBounds + 2 * IntVector2.One));
	}

	// Token: 0x06002EE9 RID: 12009 RVA: 0x000F42A8 File Offset: 0x000F24A8
	public bool Pointcast(List<IntVector2> points, List<IntVector2> lastFramePoints, int pointsWidth, out List<PointcastResult> pointResults, bool collideWithTiles = true, bool collideWithRigidbodies = true, int rayMask = 2147483647, CollisionLayer? sourceLayer = null, bool collideWithTriggers = false, Func<SpeculativeRigidbody, bool> rigidbodyExcluder = null, int ignoreTileBoneCount = 0, params SpeculativeRigidbody[] ignoreList)
	{
		int tileMapLayerByName = BraveUtility.GetTileMapLayerByName("Collision Layer", this.TileMap);
		pointResults = new List<PointcastResult>();
		PhysicsEngine.c_boundedRigidbodies.Clear();
		if (collideWithRigidbodies)
		{
			IntVector2 pointMin = IntVector2.MaxValue;
			IntVector2 pointMax = IntVector2.MinValue;
			for (int i = 0; i < points.Count; i++)
			{
				pointMin = IntVector2.Min(pointMin, points[i]);
				pointMax = IntVector2.Max(pointMax, points[i]);
			}
			Func<SpeculativeRigidbody, bool> func = delegate(SpeculativeRigidbody rigidbody)
			{
				if (!rigidbody || !rigidbody.enabled || !rigidbody.CollideWithOthers)
				{
					return true;
				}
				if (rigidbodyExcluder != null && rigidbodyExcluder(rigidbody))
				{
					return true;
				}
				if (Array.IndexOf<SpeculativeRigidbody>(ignoreList, rigidbody) >= 0)
				{
					return true;
				}
				for (int n = 0; n < rigidbody.PixelColliders.Count; n++)
				{
					PixelCollider pixelCollider = rigidbody.PixelColliders[n];
					if (collideWithTriggers || !pixelCollider.IsTrigger)
					{
						if (pixelCollider.CanCollideWith(rayMask, sourceLayer) && pixelCollider.AABBOverlaps(pointMin, pointMax - pointMin + IntVector2.One))
						{
							PhysicsEngine.c_boundedRigidbodies.Add(rigidbody);
							break;
						}
					}
				}
				return true;
			};
			this.m_rigidbodyTree.Query(PhysicsEngine.GetSafeB2AABB(pointMin, pointMax), func);
			if (this.CollidesWithProjectiles(rayMask, sourceLayer))
			{
				this.m_projectileTree.Query(PhysicsEngine.GetSafeB2AABB(pointMin, pointMax), func);
			}
		}
		DungeonData data = GameManager.Instance.Dungeon.data;
		HitDirection[] array = new HitDirection[pointsWidth];
		for (int j = 0; j < pointsWidth; j++)
		{
			array[j] = HitDirection.Forward;
		}
		for (int k = 0; k < points.Count - pointsWidth; k++)
		{
			Vector2 vector = PhysicsEngine.PixelToUnit(points[k]) + new Vector2(this.HalfPixelUnitWidth, this.HalfPixelUnitWidth);
			Vector2 vector2 = PhysicsEngine.PixelToUnit(points[k + pointsWidth]) + new Vector2(this.HalfPixelUnitWidth, this.HalfPixelUnitWidth);
			int num = k % pointsWidth;
			int num2 = 0;
			while ((k < points.Count - 2 * pointsWidth) ? (num2 < 2) : (num2 <= 2))
			{
				bool flag = false;
				IntVector2 intVector = PhysicsEngine.UnitToPixel(Vector2.Lerp(vector, vector2, (float)num2 / 2f));
				if (collideWithTiles && this.TileMap && k >= ignoreTileBoneCount)
				{
					int num3;
					int num4;
					this.TileMap.GetTileAtPosition(PhysicsEngine.PixelToUnit(intVector), out num3, out num4);
					PhysicsEngine.Tile tile = this.GetTile(num3, num4, this.TileMap, tileMapLayerByName, "Collision Layer", data);
					if (tile != null && this.Pointcast_CoarsePass(tile, intVector, collideWithTriggers, rayMask, sourceLayer))
					{
						flag = true;
					}
				}
				if (collideWithRigidbodies && !flag)
				{
					for (int l = 0; l < PhysicsEngine.c_boundedRigidbodies.Count; l++)
					{
						if (this.Pointcast_CoarsePass(PhysicsEngine.c_boundedRigidbodies[l], intVector, collideWithTriggers, rayMask, sourceLayer))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag && array[num] == HitDirection.Backward)
				{
					Vector2 vector3 = PhysicsEngine.PixelToUnit(lastFramePoints[k]) + new Vector2(this.HalfPixelUnitWidth, this.HalfPixelUnitWidth);
					Vector2 vector4 = PhysicsEngine.PixelToUnit(lastFramePoints[k + pointsWidth]) + new Vector2(this.HalfPixelUnitWidth, this.HalfPixelUnitWidth);
					IntVector2 intVector2 = PhysicsEngine.UnitToPixel(Vector2.Lerp(vector3, vector4, (float)num2 / 2f));
					Vector2 vector5 = PhysicsEngine.PixelToUnit(intVector2) + new Vector2(this.HalfPixelUnitWidth, this.HalfPixelUnitWidth);
					Vector2 vector6 = PhysicsEngine.PixelToUnit(intVector) + new Vector2(this.HalfPixelUnitWidth, this.HalfPixelUnitWidth);
					Vector2 normalized = (vector6 - vector5).normalized;
					float num5 = (vector6 - vector5).magnitude + 1.4142135f * this.PixelUnitWidth;
					RaycastResult raycastResult;
					flag = this.RaycastWithIgnores(vector5, normalized, num5, out raycastResult, collideWithTiles, collideWithRigidbodies, rayMask, sourceLayer, collideWithTriggers, null, ignoreList);
					RaycastResult.Pool.Free(ref raycastResult);
				}
				if (flag && array[num] == HitDirection.Forward)
				{
					int num6;
					int num7;
					Vector2 vector7;
					Vector2 vector8;
					if (k < pointsWidth && num2 == 0)
					{
						num6 = 0;
						num7 = 0;
						vector7 = PhysicsEngine.PixelToUnit(lastFramePoints[0]) + new Vector2(this.HalfPixelUnitWidth, this.HalfPixelUnitWidth);
						vector8 = PhysicsEngine.PixelToUnit(points[0]) + new Vector2(this.HalfPixelUnitWidth, this.HalfPixelUnitWidth);
					}
					else
					{
						num6 = ((num2 != 0) ? k : (k - pointsWidth));
						num7 = num6 + pointsWidth;
						vector7 = PhysicsEngine.PixelToUnit(points[num6]) + new Vector2(this.HalfPixelUnitWidth, this.HalfPixelUnitWidth);
						vector8 = PhysicsEngine.PixelToUnit(points[num7]) + new Vector2(this.HalfPixelUnitWidth, this.HalfPixelUnitWidth);
					}
					Vector2 normalized2 = (vector8 - vector7).normalized;
					float num8 = (vector8 - vector7).magnitude + 1.4142135f * this.PixelUnitWidth;
					RaycastResult raycastResult2;
					flag = this.RaycastWithIgnores(vector7, normalized2, num8, out raycastResult2, collideWithTiles && num7 >= ignoreTileBoneCount, collideWithRigidbodies, rayMask, sourceLayer, collideWithTriggers, null, ignoreList);
					if (flag)
					{
						PointcastResult pointcastResult = PointcastResult.Pool.Allocate();
						pointcastResult.SetAll(HitDirection.Forward, num6, num6 / pointsWidth, raycastResult2);
						pointResults.Add(pointcastResult);
						array[num] = HitDirection.Backward;
					}
				}
				else if (!flag && array[num] == HitDirection.Backward)
				{
					int num9 = ((num2 != 0) ? k : (k - pointsWidth));
					int num10 = num9 + pointsWidth;
					Vector2 vector9 = PhysicsEngine.PixelToUnit(points[num9]) + new Vector2(this.HalfPixelUnitWidth, this.HalfPixelUnitWidth);
					Vector2 vector10 = PhysicsEngine.PixelToUnit(points[num10]) + new Vector2(this.HalfPixelUnitWidth, this.HalfPixelUnitWidth);
					Vector2 normalized3 = (vector9 - vector10).normalized;
					float num11 = (vector9 - vector10).magnitude + 1.4142135f * this.PixelUnitWidth;
					num11 *= 3f;
					RaycastResult raycastResult3;
					flag = this.RaycastWithIgnores(vector10, normalized3, num11 * 3f, out raycastResult3, collideWithTiles && num9 >= ignoreTileBoneCount, collideWithRigidbodies, rayMask, sourceLayer, collideWithTriggers, null, ignoreList);
					if (flag)
					{
						PointcastResult pointcastResult2 = PointcastResult.Pool.Allocate();
						pointcastResult2.SetAll(HitDirection.Backward, num10, num10 / pointsWidth, raycastResult3);
						pointResults.Add(pointcastResult2);
						array[num] = HitDirection.Forward;
					}
				}
				num2++;
			}
		}
		if (pointsWidth > 1)
		{
			pointResults.Sort();
			List<PointcastResult> list = new List<PointcastResult>();
			int m = 0;
			int num12 = 0;
			while (m < pointResults.Count)
			{
				int num13 = num12;
				int num14 = m;
				while (num14 < pointResults.Count && pointResults[m].boneIndex == pointResults[num14].boneIndex)
				{
					if (pointResults[num14].hitDirection == HitDirection.Forward)
					{
						num12++;
					}
					else if (pointResults[num14].hitDirection == HitDirection.Backward)
					{
						num12--;
					}
					num14++;
				}
				if (m == 0 && num12 > 0)
				{
					list.Add(pointResults[m]);
				}
				else if (num13 == 0 && num12 > 0)
				{
					list.Add(pointResults[m]);
				}
				else if (num13 >= 0 && num12 == 0)
				{
					list.Add(pointResults[m]);
				}
				m = num14;
			}
			pointResults = list;
		}
		return pointResults.Count > 0;
	}

	// Token: 0x06002EEA RID: 12010 RVA: 0x000F4AB0 File Offset: 0x000F2CB0
	private bool Pointcast_CoarsePass(ICollidableObject collidable, IntVector2 point, bool collideWithTriggers, int rayMask, CollisionLayer? sourceLayer)
	{
		for (int i = 0; i < collidable.GetPixelColliders().Count; i++)
		{
			PixelCollider pixelCollider = collidable.GetPixelColliders()[i];
			if (collideWithTriggers || !pixelCollider.IsTrigger)
			{
				if (pixelCollider.CanCollideWith(rayMask, sourceLayer) && pixelCollider.ContainsPixel(point))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002EEB RID: 12011 RVA: 0x000F4B1C File Offset: 0x000F2D1C
	public bool RigidbodyCast(SpeculativeRigidbody rigidbody, IntVector2 pixelsToMove, out CollisionData result, bool collideWithTiles = true, bool collideWithRigidbodies = true, int? overrideCollisionMask = null, bool collideWithTriggers = false)
	{
		return this.RigidbodyCastWithIgnores(rigidbody, pixelsToMove, out result, collideWithTiles, collideWithRigidbodies, overrideCollisionMask, collideWithTriggers, this.emptyIgnoreList);
	}

	// Token: 0x06002EEC RID: 12012 RVA: 0x000F4B40 File Offset: 0x000F2D40
	public bool RigidbodyCastWithIgnores(SpeculativeRigidbody rigidbody, IntVector2 pixelsToMove, out CollisionData result, bool collideWithTiles = true, bool collideWithRigidbodies = true, int? overrideCollisionMask = null, bool collideWithTriggers = false, params SpeculativeRigidbody[] ignoreList)
	{
		PhysicsEngine.m_rigidbodyCaster.SetAll(this, GameManager.Instance.Dungeon.data, rigidbody, pixelsToMove, collideWithTiles, collideWithRigidbodies, overrideCollisionMask, collideWithTriggers, ignoreList);
		bool flag = PhysicsEngine.m_rigidbodyCaster.DoRigidbodyCast(out result);
		PhysicsEngine.m_rigidbodyCaster.Clear();
		return flag;
	}

	// Token: 0x06002EED RID: 12013 RVA: 0x000F4B8C File Offset: 0x000F2D8C
	public bool OverlapCast(SpeculativeRigidbody rigidbody, List<CollisionData> overlappingCollisions = null, bool collideWithTiles = true, bool collideWithRigidbodies = true, int? overrideCollisionMask = null, int? ignoreCollisionMask = null, bool collideWithTriggers = false, Vector2? overridePosition = null, Func<SpeculativeRigidbody, bool> rigidbodyExcluder = null, params SpeculativeRigidbody[] ignoreList)
	{
		List<CollisionData> tempOverlappingCollisions = new List<CollisionData>();
		if (!rigidbody || rigidbody.PixelColliders.Count == 0)
		{
			if (overlappingCollisions != null)
			{
				overlappingCollisions.Clear();
			}
			return false;
		}
		IntVector2 intVector = IntVector2.Zero;
		if (overridePosition != null)
		{
			Position position = new Position(overridePosition.Value);
			intVector = position.PixelPosition - rigidbody.Position.PixelPosition;
			for (int i = 0; i < rigidbody.PixelColliders.Count; i++)
			{
				rigidbody.PixelColliders[i].Position += intVector;
			}
		}
		IntVector2 intVector2 = IntVector2.MaxValue;
		IntVector2 intVector3 = IntVector2.MinValue;
		for (int j = 0; j < rigidbody.PixelColliders.Count; j++)
		{
			PixelCollider pixelCollider = rigidbody.PixelColliders[j];
			intVector2 = IntVector2.Min(intVector2, pixelCollider.Min);
			intVector3 = IntVector2.Max(intVector3, pixelCollider.Max);
		}
		if (collideWithTiles && this.TileMap)
		{
			IntVector2 intVector4 = intVector2 - IntVector2.One;
			IntVector2 intVector5 = intVector3 + IntVector2.One;
			this.InitNearbyTileCheck(PhysicsEngine.PixelToUnit(intVector4), PhysicsEngine.PixelToUnit(intVector5), this.TileMap);
			DungeonData data = GameManager.Instance.Dungeon.data;
			for (PhysicsEngine.Tile tile = this.GetNextNearbyTile(data); tile != null; tile = this.GetNextNearbyTile(data))
			{
				for (int k = 0; k < rigidbody.PixelColliders.Count; k++)
				{
					PixelCollider pixelCollider2 = rigidbody.PixelColliders[k];
					for (int l = 0; l < tile.PixelColliders.Count; l++)
					{
						PixelCollider pixelCollider3 = tile.PixelColliders[l];
						if (pixelCollider2.CanCollideWith(pixelCollider3, false) && pixelCollider2.AABBOverlaps(pixelCollider3) && pixelCollider2.Overlaps(pixelCollider3))
						{
							CollisionData collisionData = PhysicsEngine.SingleCollision(rigidbody, pixelCollider2, tile, pixelCollider3, null, false);
							if (collisionData != null)
							{
								tempOverlappingCollisions.Add(collisionData);
							}
						}
					}
				}
			}
		}
		if (collideWithRigidbodies)
		{
			Func<SpeculativeRigidbody, bool> func = delegate(SpeculativeRigidbody otherRigidbody)
			{
				if (otherRigidbody && otherRigidbody != rigidbody && otherRigidbody.enabled && otherRigidbody.CollideWithOthers && Array.IndexOf<SpeculativeRigidbody>(ignoreList, otherRigidbody) < 0)
				{
					if (rigidbodyExcluder != null && rigidbodyExcluder(otherRigidbody))
					{
						return true;
					}
					for (int num = 0; num < rigidbody.PixelColliders.Count; num++)
					{
						PixelCollider pixelCollider4 = rigidbody.PixelColliders[num];
						for (int num2 = 0; num2 < otherRigidbody.PixelColliders.Count; num2++)
						{
							PixelCollider pixelCollider5 = otherRigidbody.PixelColliders[num2];
							if (collideWithTriggers || !pixelCollider5.IsTrigger)
							{
								bool flag2;
								if (overrideCollisionMask != null || ignoreCollisionMask != null)
								{
									int num3 = ((overrideCollisionMask == null) ? CollisionLayerMatrix.GetMask(pixelCollider4.CollisionLayer) : overrideCollisionMask.Value);
									if (ignoreCollisionMask != null)
									{
										num3 &= ~ignoreCollisionMask.Value;
									}
									flag2 = pixelCollider5.CanCollideWith(num3, null);
								}
								else
								{
									flag2 = pixelCollider4.CanCollideWith(pixelCollider5, false);
								}
								if (flag2 && pixelCollider4.AABBOverlaps(pixelCollider5) && pixelCollider4.Overlaps(pixelCollider5))
								{
									CollisionData collisionData3 = PhysicsEngine.SingleCollision(rigidbody, pixelCollider4, otherRigidbody, pixelCollider5, null, false);
									if (collisionData3 != null)
									{
										tempOverlappingCollisions.Add(collisionData3);
									}
								}
							}
						}
					}
				}
				return true;
			};
			this.m_rigidbodyTree.Query(PhysicsEngine.GetSafeB2AABB(intVector2, intVector3), func);
			if (this.CollidesWithProjectiles(rigidbody))
			{
				this.m_projectileTree.Query(PhysicsEngine.GetSafeB2AABB(intVector2, intVector3), func);
			}
		}
		if (overridePosition != null)
		{
			for (int m = 0; m < rigidbody.PixelColliders.Count; m++)
			{
				rigidbody.PixelColliders[m].Position -= intVector;
			}
		}
		bool flag = tempOverlappingCollisions.Count > 0;
		if (overlappingCollisions == null)
		{
			for (int n = 0; n < tempOverlappingCollisions.Count; n++)
			{
				CollisionData collisionData2 = tempOverlappingCollisions[n];
				CollisionData.Pool.Free(ref collisionData2);
			}
		}
		else
		{
			overlappingCollisions.Clear();
			overlappingCollisions.AddRange(tempOverlappingCollisions);
		}
		return flag;
	}

	// Token: 0x06002EEE RID: 12014 RVA: 0x000F4F48 File Offset: 0x000F3148
	public void RegisterOverlappingGhostCollisionExceptions(SpeculativeRigidbody specRigidbody, int? overrideLayerMask = null, bool includeTriggers = false)
	{
		if (!this.m_rigidbodies.Contains(specRigidbody))
		{
			specRigidbody.Reinitialize();
		}
		List<SpeculativeRigidbody> overlappingRigidbodies = this.GetOverlappingRigidbodies(specRigidbody, overrideLayerMask, includeTriggers);
		for (int i = 0; i < overlappingRigidbodies.Count; i++)
		{
			specRigidbody.RegisterGhostCollisionException(overlappingRigidbodies[i]);
			overlappingRigidbodies[i].RegisterGhostCollisionException(specRigidbody);
		}
	}

	// Token: 0x06002EEF RID: 12015 RVA: 0x000F4FA8 File Offset: 0x000F31A8
	public List<SpeculativeRigidbody> GetOverlappingRigidbodies(SpeculativeRigidbody specRigidbody, int? overrideLayerMask = null, bool includeTriggers = false)
	{
		List<SpeculativeRigidbody> list = new List<SpeculativeRigidbody>();
		for (int i = 0; i < specRigidbody.PixelColliders.Count; i++)
		{
			list.AddRange(this.GetOverlappingRigidbodies(specRigidbody.PixelColliders[i], overrideLayerMask, includeTriggers));
		}
		for (int j = 0; j < list.Count - 1; j++)
		{
			for (int k = list.Count - 1; k > j; k--)
			{
				if (list[j] == list[k])
				{
					list.RemoveAt(k);
				}
			}
		}
		return list;
	}

	// Token: 0x06002EF0 RID: 12016 RVA: 0x000F5044 File Offset: 0x000F3244
	public List<SpeculativeRigidbody> GetOverlappingRigidbodies(PixelCollider pixelCollider, int? overrideLayerMask = null, bool includeTriggers = false)
	{
		List<SpeculativeRigidbody> overlappingRigidbodies = new List<SpeculativeRigidbody>();
		Func<SpeculativeRigidbody, bool> func = delegate(SpeculativeRigidbody rigidbody)
		{
			if (rigidbody.PixelColliders.Contains(pixelCollider))
			{
				return true;
			}
			if (!includeTriggers && pixelCollider.IsTrigger)
			{
				return true;
			}
			for (int i = 0; i < rigidbody.PixelColliders.Count; i++)
			{
				PixelCollider pixelCollider2 = rigidbody.PixelColliders[i];
				if (includeTriggers || !pixelCollider2.IsTrigger)
				{
					if (overrideLayerMask != null)
					{
						int num = CollisionMask.LayerToMask(pixelCollider2.CollisionLayer);
						if ((overrideLayerMask.Value & num) != num)
						{
							goto IL_D1;
						}
					}
					else if (!pixelCollider.CanCollideWith(pixelCollider2, false))
					{
						goto IL_D1;
					}
					if (pixelCollider.AABBOverlaps(pixelCollider2))
					{
						overlappingRigidbodies.Add(rigidbody);
					}
				}
				IL_D1:;
			}
			return true;
		};
		this.m_rigidbodyTree.Query(PhysicsEngine.GetSafeB2AABB(pixelCollider.Min, pixelCollider.Max), func);
		if (this.CollidesWithProjectiles(pixelCollider))
		{
			this.m_projectileTree.Query(PhysicsEngine.GetSafeB2AABB(pixelCollider.Min, pixelCollider.Max), func);
		}
		return overlappingRigidbodies;
	}

	// Token: 0x06002EF1 RID: 12017 RVA: 0x000F50EC File Offset: 0x000F32EC
	public List<ICollidableObject> GetOverlappingCollidableObjects(Vector2 min, Vector2 max, bool collideWithTiles = true, bool collideWithRigidbodies = true, int? layerMask = null, bool includeTriggers = false)
	{
		List<ICollidableObject> overlappingRigidbodies = new List<ICollidableObject>();
		PixelCollider aabbCollider = new PixelCollider();
		aabbCollider.RegenerateFromManual(min, IntVector2.Zero, new IntVector2(Mathf.CeilToInt(16f * (max.x - min.x)), Mathf.CeilToInt(16f * (max.y - min.y))), 0f, null);
		if (collideWithTiles && this.TileMap)
		{
			IntVector2 intVector = aabbCollider.Min - IntVector2.One;
			IntVector2 intVector2 = aabbCollider.Max + IntVector2.One;
			this.InitNearbyTileCheck(PhysicsEngine.PixelToUnit(intVector), PhysicsEngine.PixelToUnit(intVector2), this.TileMap);
			DungeonData data = GameManager.Instance.Dungeon.data;
			for (PhysicsEngine.Tile tile = this.GetNextNearbyTile(data); tile != null; tile = this.GetNextNearbyTile(data))
			{
				for (int i = 0; i < tile.PixelColliders.Count; i++)
				{
					PixelCollider pixelCollider = tile.PixelColliders[i];
					if ((layerMask == null || pixelCollider.CanCollideWith(layerMask.Value, null)) && aabbCollider.AABBOverlaps(pixelCollider) && aabbCollider.Overlaps(pixelCollider))
					{
						overlappingRigidbodies.Add(tile);
					}
				}
			}
		}
		if (collideWithRigidbodies)
		{
			Func<SpeculativeRigidbody, bool> func = delegate(SpeculativeRigidbody rigidbody)
			{
				for (int j = 0; j < rigidbody.PixelColliders.Count; j++)
				{
					PixelCollider pixelCollider2 = rigidbody.PixelColliders[j];
					if (includeTriggers || !pixelCollider2.IsTrigger)
					{
						if (layerMask != null)
						{
							int num2 = CollisionMask.LayerToMask(pixelCollider2.CollisionLayer);
							if ((layerMask.Value & num2) != num2)
							{
								goto IL_91;
							}
						}
						if (aabbCollider.AABBOverlaps(pixelCollider2) && aabbCollider.Overlaps(pixelCollider2))
						{
							overlappingRigidbodies.Add(rigidbody);
						}
					}
					IL_91:;
				}
				return true;
			};
			this.m_rigidbodyTree.Query(PhysicsEngine.GetSafeB2AABB(aabbCollider.Min, aabbCollider.Max), func);
			int num = CollisionMask.LayerToMask(CollisionLayer.Projectile);
			if (layerMask == null || (layerMask.Value & num) == num)
			{
				this.m_projectileTree.Query(PhysicsEngine.GetSafeB2AABB(aabbCollider.Min, aabbCollider.Max), func);
			}
		}
		return overlappingRigidbodies;
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x000F533C File Offset: 0x000F353C
	public void Register(SpeculativeRigidbody rigidbody)
	{
		if (rigidbody == null)
		{
			return;
		}
		if (rigidbody.PhysicsRegistration == SpeculativeRigidbody.RegistrationState.Unknown)
		{
			this.InferRegistration(rigidbody);
		}
		SpeculativeRigidbody.RegistrationState physicsRegistration = rigidbody.PhysicsRegistration;
		if (physicsRegistration == SpeculativeRigidbody.RegistrationState.Registered)
		{
			return;
		}
		if (physicsRegistration == SpeculativeRigidbody.RegistrationState.DeregisterScheduled)
		{
			this.m_deregisterRigidBodies.Remove(rigidbody);
			rigidbody.PhysicsRegistration = SpeculativeRigidbody.RegistrationState.Registered;
			return;
		}
		if (physicsRegistration != SpeculativeRigidbody.RegistrationState.Deregistered)
		{
			return;
		}
		this.m_rigidbodies.Add(rigidbody);
		if (rigidbody.IsSimpleProjectile)
		{
			rigidbody.proxyId = this.m_projectileTree.CreateProxy(rigidbody.b2AABB, rigidbody);
		}
		else
		{
			rigidbody.proxyId = this.m_rigidbodyTree.CreateProxy(rigidbody.b2AABB, rigidbody);
		}
		rigidbody.PhysicsRegistration = SpeculativeRigidbody.RegistrationState.Registered;
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x000F53F4 File Offset: 0x000F35F4
	public void Deregister(SpeculativeRigidbody rigidbody)
	{
		if (rigidbody == null)
		{
			return;
		}
		if (rigidbody.PhysicsRegistration == SpeculativeRigidbody.RegistrationState.Unknown)
		{
			this.InferRegistration(rigidbody);
		}
		if (rigidbody.PhysicsRegistration == SpeculativeRigidbody.RegistrationState.Deregistered)
		{
			return;
		}
		if (rigidbody.PhysicsRegistration == SpeculativeRigidbody.RegistrationState.DeregisterScheduled)
		{
			this.m_deregisterRigidBodies.Remove(rigidbody);
		}
		this.m_rigidbodies.Remove(rigidbody);
		if (rigidbody.proxyId >= 0)
		{
			if (rigidbody.IsSimpleProjectile)
			{
				this.m_projectileTree.DestroyProxy(rigidbody.proxyId);
			}
			else
			{
				this.m_rigidbodyTree.DestroyProxy(rigidbody.proxyId);
			}
			rigidbody.proxyId = -1;
		}
		rigidbody.PhysicsRegistration = SpeculativeRigidbody.RegistrationState.Deregistered;
	}

	// Token: 0x06002EF4 RID: 12020 RVA: 0x000F54A0 File Offset: 0x000F36A0
	public void DeregisterWhenAvailable(SpeculativeRigidbody rigidbody)
	{
		if (rigidbody == null)
		{
			return;
		}
		if (rigidbody.PhysicsRegistration == SpeculativeRigidbody.RegistrationState.Unknown)
		{
			this.InferRegistration(rigidbody);
		}
		if (rigidbody.PhysicsRegistration != SpeculativeRigidbody.RegistrationState.Registered)
		{
			return;
		}
		this.m_deregisterRigidBodies.Add(rigidbody);
		rigidbody.PhysicsRegistration = SpeculativeRigidbody.RegistrationState.DeregisterScheduled;
	}

	// Token: 0x06002EF5 RID: 12021 RVA: 0x000F54EC File Offset: 0x000F36EC
	private void InferRegistration(SpeculativeRigidbody rigidbody)
	{
		if (this.m_deregisterRigidBodies.Contains(rigidbody))
		{
			rigidbody.PhysicsRegistration = SpeculativeRigidbody.RegistrationState.DeregisterScheduled;
		}
		else if (this.m_rigidbodies.Contains(rigidbody))
		{
			rigidbody.PhysicsRegistration = SpeculativeRigidbody.RegistrationState.Registered;
		}
		else
		{
			rigidbody.PhysicsRegistration = SpeculativeRigidbody.RegistrationState.Deregistered;
		}
	}

	// Token: 0x06002EF6 RID: 12022 RVA: 0x000F553C File Offset: 0x000F373C
	private void CollideWithRigidbodies(SpeculativeRigidbody rigidbody, List<PixelCollider.StepData> stepList, ref CollisionData nearestCollision)
	{
		if (!rigidbody || !rigidbody.enabled)
		{
			return;
		}
		b2AABB b2AABB = rigidbody.b2AABB;
		IntVector2 pixelsToMove = rigidbody.PixelsToMove;
		if (pixelsToMove.x < 0)
		{
			b2AABB.lowerBound.x = b2AABB.lowerBound.x + ((float)pixelsToMove.x * 0.0625f - 0.0625f);
			b2AABB.upperBound.x = b2AABB.upperBound.x + 0.0625f;
		}
		else
		{
			b2AABB.lowerBound.x = b2AABB.lowerBound.x - 0.0625f;
			b2AABB.upperBound.x = b2AABB.upperBound.x + ((float)pixelsToMove.x * 0.0625f + 0.0625f);
		}
		if (pixelsToMove.y < 0)
		{
			b2AABB.lowerBound.y = b2AABB.lowerBound.y + ((float)pixelsToMove.Y * 0.0625f - 0.0625f);
			b2AABB.upperBound.y = b2AABB.upperBound.y + 0.0625f;
		}
		else
		{
			b2AABB.lowerBound.y = b2AABB.lowerBound.y - 0.0625f;
			b2AABB.upperBound.y = b2AABB.upperBound.y + ((float)pixelsToMove.y * 0.0625f + 0.0625f);
		}
		PhysicsEngine.m_cwrqRigidbody = rigidbody;
		PhysicsEngine.m_cwrqStepList = stepList;
		PhysicsEngine.m_cwrqCollisionData = nearestCollision;
		this.m_rigidbodyTree.Query(b2AABB, new Func<SpeculativeRigidbody, bool>(PhysicsEngine.CollideWithRigidbodiesQuery));
		if (this.CollidesWithProjectiles(rigidbody))
		{
			this.m_projectileTree.Query(b2AABB, new Func<SpeculativeRigidbody, bool>(PhysicsEngine.CollideWithRigidbodiesQuery));
		}
		nearestCollision = PhysicsEngine.m_cwrqCollisionData;
		PhysicsEngine.m_cwrqRigidbody = null;
		PhysicsEngine.m_cwrqStepList = null;
		PhysicsEngine.m_cwrqCollisionData = null;
	}

	// Token: 0x06002EF7 RID: 12023 RVA: 0x000F5718 File Offset: 0x000F3918
	private static bool CollideWithRigidbodiesQuery(SpeculativeRigidbody otherRigidbody)
	{
		for (int i = 0; i < PhysicsEngine.m_cwrqRigidbody.PixelColliders.Count; i++)
		{
			PixelCollider pixelCollider = PhysicsEngine.m_cwrqRigidbody.PixelColliders[i];
			for (int j = 0; j < otherRigidbody.PixelColliders.Count; j++)
			{
				PixelCollider pixelCollider2 = otherRigidbody.PixelColliders[j];
				CollisionData collisionData = PhysicsEngine.SingleCollision(PhysicsEngine.m_cwrqRigidbody, pixelCollider, otherRigidbody, pixelCollider2, PhysicsEngine.m_cwrqStepList, true);
				if (collisionData != null)
				{
					if (PhysicsEngine.m_cwrqCollisionData == null || collisionData.TimeUsed < PhysicsEngine.m_cwrqCollisionData.TimeUsed)
					{
						CollisionData.Pool.Free(ref PhysicsEngine.m_cwrqCollisionData);
						PhysicsEngine.m_cwrqCollisionData = collisionData;
					}
					else
					{
						CollisionData.Pool.Free(ref collisionData);
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06002EF8 RID: 12024 RVA: 0x000F57E4 File Offset: 0x000F39E4
	private void CollideWithTilemap(SpeculativeRigidbody rigidbody, PixelCollider pixelCollider, List<PixelCollider.StepData> stepList, ref float timeUsed, DungeonData dungeonData, ref CollisionData nearestCollision)
	{
		Position position = rigidbody.m_position;
		IntVector2 intVector = pixelCollider.m_offset + pixelCollider.m_transformOffset;
		float num = (float)position.m_position.x * 0.0625f + position.m_remainder.x + (float)intVector.x * 0.0625f;
		float num2 = (float)position.m_position.y * 0.0625f + position.m_remainder.y + (float)intVector.y * 0.0625f;
		IntVector2 pixelsToMove = rigidbody.PixelsToMove;
		float num3 = num + (float)pixelsToMove.x * 0.0625f;
		float num4 = num2 + (float)pixelsToMove.y * 0.0625f;
		IntVector2 dimensions = pixelCollider.m_dimensions;
		float num5;
		float num6;
		if (num < num3)
		{
			num5 = num - 0.25f;
			num6 = num3 + 0.25f + (float)dimensions.x * 0.0625f;
		}
		else
		{
			num5 = num3 - 0.25f;
			num6 = num + 0.25f + (float)dimensions.x * 0.0625f;
		}
		float num7;
		float num8;
		if (num2 < num4)
		{
			num7 = num2 - 0.25f;
			num8 = num4 + 0.25f + (float)dimensions.y * 0.0625f;
		}
		else
		{
			num7 = num4 - 0.25f;
			num8 = num2 + 0.25f + (float)dimensions.y * 0.0625f;
		}
		this.InitNearbyTileCheck(num5, num7, num6, num8, this.TileMap, dimensions, num, num2, pixelsToMove, dungeonData);
		for (PhysicsEngine.Tile tile = this.GetNextNearbyTile(dungeonData); tile != null; tile = this.GetNextNearbyTile(dungeonData))
		{
			for (int i = 0; i < tile.PixelColliders.Count; i++)
			{
				CollisionData collisionData = PhysicsEngine.SingleCollision(rigidbody, pixelCollider, tile, tile.PixelColliders[i], stepList, true);
				if (collisionData != null)
				{
					if (nearestCollision == null || collisionData.TimeUsed < nearestCollision.TimeUsed)
					{
						CollisionData.Pool.Free(ref nearestCollision);
						nearestCollision = collisionData;
					}
					else
					{
						CollisionData.Pool.Free(ref collisionData);
					}
					this.m_nbt.Finish(dungeonData, false);
				}
			}
		}
		this.CleanupNearbyTileCheck();
	}

	// Token: 0x06002EF9 RID: 12025 RVA: 0x000F5A14 File Offset: 0x000F3C14
	private static CollisionData SingleCollision(SpeculativeRigidbody rigidbody, PixelCollider collider, ICollidableObject otherCollidable, PixelCollider otherCollider, List<PixelCollider.StepData> stepList, bool doPreCollision)
	{
		if (collider == null || otherCollider == null)
		{
			return null;
		}
		if (!collider.AABBOverlaps(otherCollider, rigidbody.PixelsToMove))
		{
			return null;
		}
		if (!otherCollidable.CanCollideWith(rigidbody))
		{
			return null;
		}
		if (!otherCollider.CanCollideWith(collider, false))
		{
			return null;
		}
		LinearCastResult linearCastResult = null;
		CollisionData collisionData = null;
		if (otherCollider.DirectionIgnorer != null || !collider.Overlaps(otherCollider))
		{
			if (!collider.LinearCast(otherCollider, rigidbody.PixelsToMove, stepList, out linearCastResult, false, 0f))
			{
				linearCastResult = null;
			}
		}
		else if (collider.IsTrigger || otherCollider.IsTrigger)
		{
			linearCastResult = LinearCastResult.Pool.Allocate();
			linearCastResult.Contact = rigidbody.UnitCenter;
			linearCastResult.Normal = Vector2.up;
			linearCastResult.MyPixelCollider = collider;
			linearCastResult.OtherPixelCollider = otherCollider;
			linearCastResult.TimeUsed = 0f;
			linearCastResult.CollidedX = true;
			linearCastResult.CollidedY = true;
			linearCastResult.NewPixelsToMove = IntVector2.Zero;
			linearCastResult.Overlap = true;
		}
		else
		{
			IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
			int num = 0;
			int num2 = 1;
			int i;
			for (;;)
			{
				for (i = 0; i < cardinalsAndOrdinals.Length; i++)
				{
					if (!collider.Overlaps(otherCollider, cardinalsAndOrdinals[i] * num2))
					{
						goto Block_8;
					}
				}
				num2++;
				num++;
				if (num > 100)
				{
					UnityEngine.Debug.LogError(string.Format("FREEZE AVERTED!  TELL RUBEL!  (you're welcome) [{0}] & [{1}]", rigidbody.name, (!(otherCollidable is SpeculativeRigidbody)) ? "tile" : ((SpeculativeRigidbody)otherCollidable).name));
				}
			}
			Block_8:
			linearCastResult = LinearCastResult.Pool.Allocate();
			linearCastResult.Contact = rigidbody.UnitCenter;
			linearCastResult.Normal = cardinalsAndOrdinals[i].ToVector2().normalized;
			linearCastResult.MyPixelCollider = collider;
			linearCastResult.OtherPixelCollider = otherCollider;
			linearCastResult.TimeUsed = 0f;
			linearCastResult.CollidedX = true;
			linearCastResult.CollidedY = true;
			linearCastResult.NewPixelsToMove = IntVector2.Zero;
			linearCastResult.Overlap = true;
		}
		if (linearCastResult != null)
		{
			if (doPreCollision)
			{
				if (otherCollidable is SpeculativeRigidbody)
				{
					SpeculativeRigidbody speculativeRigidbody = otherCollidable as SpeculativeRigidbody;
					PhysicsEngine.SkipCollision = false;
					PhysicsEngine.PendingCastResult = linearCastResult;
					if (rigidbody.OnPreRigidbodyCollision != null)
					{
						rigidbody.OnPreRigidbodyCollision(rigidbody, collider, speculativeRigidbody, otherCollider);
					}
					if (speculativeRigidbody.OnPreRigidbodyCollision != null)
					{
						speculativeRigidbody.OnPreRigidbodyCollision(speculativeRigidbody, otherCollider, rigidbody, collider);
					}
					if (PhysicsEngine.SkipCollision)
					{
						LinearCastResult.Pool.Free(ref linearCastResult);
						return null;
					}
				}
				else if (otherCollidable is PhysicsEngine.Tile)
				{
					PhysicsEngine.Tile tile = otherCollidable as PhysicsEngine.Tile;
					PhysicsEngine.SkipCollision = false;
					PhysicsEngine.PendingCastResult = linearCastResult;
					if (rigidbody.OnPreTileCollision != null)
					{
						rigidbody.OnPreTileCollision(rigidbody, collider, tile, otherCollider);
					}
					if (PhysicsEngine.SkipCollision)
					{
						LinearCastResult.Pool.Free(ref linearCastResult);
						return null;
					}
				}
			}
			collisionData = CollisionData.Pool.Allocate();
			collisionData.SetAll(linearCastResult);
			collisionData.MyRigidbody = rigidbody;
			if (otherCollidable is SpeculativeRigidbody)
			{
				collisionData.collisionType = CollisionData.CollisionType.Rigidbody;
				collisionData.OtherRigidbody = (SpeculativeRigidbody)otherCollidable;
			}
			else if (otherCollidable is PhysicsEngine.Tile)
			{
				collisionData.collisionType = CollisionData.CollisionType.TileMap;
				collisionData.TileLayerName = ((PhysicsEngine.Tile)otherCollidable).LayerName;
				collisionData.TilePosition = ((PhysicsEngine.Tile)otherCollidable).Position;
			}
			LinearCastResult.Pool.Free(ref linearCastResult);
		}
		return collisionData;
	}

	// Token: 0x06002EFA RID: 12026 RVA: 0x000F5D64 File Offset: 0x000F3F64
	private bool CollidesWithProjectiles(int mask, CollisionLayer? sourceLayer)
	{
		return (mask & this.m_cachedProjectileMask) == this.m_cachedProjectileMask && (sourceLayer == null || (CollisionLayerMatrix.GetMask(sourceLayer.Value) & this.m_cachedProjectileMask) == this.m_cachedProjectileMask);
	}

	// Token: 0x06002EFB RID: 12027 RVA: 0x000F5DA4 File Offset: 0x000F3FA4
	private bool CollidesWithProjectiles(SpeculativeRigidbody specRigidbody)
	{
		List<PixelCollider> pixelColliders = specRigidbody.PixelColliders;
		for (int i = 0; i < pixelColliders.Count; i++)
		{
			PixelCollider pixelCollider = pixelColliders[i];
			if ((CollisionLayerMatrix.GetMask(pixelCollider.CollisionLayer) & this.m_cachedProjectileMask) == this.m_cachedProjectileMask)
			{
				return true;
			}
			if ((pixelCollider.CollisionLayerCollidableOverride & this.m_cachedProjectileMask) == this.m_cachedProjectileMask)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002EFC RID: 12028 RVA: 0x000F5E14 File Offset: 0x000F4014
	private bool CollidesWithProjectiles(PixelCollider pixelCollider)
	{
		return (CollisionLayerMatrix.GetMask(pixelCollider.CollisionLayer) & this.m_cachedProjectileMask) == this.m_cachedProjectileMask;
	}

	// Token: 0x06002EFD RID: 12029 RVA: 0x000F5E30 File Offset: 0x000F4030
	public void ClearAllCachedTiles()
	{
		Dungeon dungeon = GameManager.Instance.Dungeon;
		DungeonData data = dungeon.data;
		for (int i = 0; i < dungeon.Width; i++)
		{
			for (int j = 0; j < dungeon.Height; j++)
			{
				CellData cellData = data[i, j];
				if (cellData != null)
				{
					cellData.HasCachedPhysicsTile = false;
					cellData.CachedPhysicsTile = null;
				}
			}
		}
	}

	// Token: 0x06002EFE RID: 12030 RVA: 0x000F5EA0 File Offset: 0x000F40A0
	private PhysicsEngine.Tile GetTile(int x, int y, tk2dTileMap tileMap, int layer, string layerName, DungeonData dungeonData)
	{
		CellData cellData;
		if (x < 0 || x >= this.m_cachedDungeonWidth || y < 0 || y >= this.m_cachedDungeonHeight || (cellData = dungeonData.cellData[x][y]) == null)
		{
			return null;
		}
		if (cellData.HasCachedPhysicsTile)
		{
			return cellData.CachedPhysicsTile;
		}
		if (cellData.type == CellType.WALL && GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(cellData.position + IntVector2.Up))
		{
			CellData cellData2 = GameManager.Instance.Dungeon.data[cellData.position + IntVector2.Up];
			if (cellData2 != null && cellData2.isOccludedByTopWall && (cellData2.diagonalWallType == DiagonalWallType.SOUTHEAST || cellData2.diagonalWallType == DiagonalWallType.SOUTHWEST))
			{
				PhysicsEngine.Tile tile = this.GetTile(x, y + 1, tileMap, layer, layerName, dungeonData);
				cellData2.HasCachedPhysicsTile = true;
				cellData2.CachedPhysicsTile = tile;
				return tile;
			}
		}
		int tile2 = this.GetTile(layer, cellData.positionInTilemap.x, cellData.positionInTilemap.y);
		List<PixelCollider> list = new List<PixelCollider>();
		Vector2 vector = Vector2.Scale(new Vector2((float)x, (float)y), tileMap.data.tileSize.XY());
		Position position = new Position(tileMap.transform.position + vector);
		IntVector2 pixelPosition = position.PixelPosition;
		if (tile2 >= 0)
		{
			tk2dSpriteDefinition tk2dSpriteDefinition = tileMap.SpriteCollectionInst.spriteDefinitions[tile2];
			if (tk2dSpriteDefinition.IsTileSquare)
			{
				PixelCollider pixelCollider = new PixelCollider
				{
					IsTileCollider = true,
					CollisionLayer = tk2dSpriteDefinition.collisionLayer
				};
				pixelCollider.RegenerateFromManual(tileMap.transform, new IntVector2(0, 0), new IntVector2(16, 16), 0f, null);
				pixelCollider.Position = pixelPosition;
				list.Add(pixelCollider);
			}
			else
			{
				PixelCollider pixelCollider2 = new PixelCollider
				{
					IsTileCollider = true,
					CollisionLayer = tk2dSpriteDefinition.collisionLayer
				};
				pixelCollider2.RegenerateFrom3dCollider(tk2dSpriteDefinition.colliderVertices, tileMap.transform, 0f, null, false, false);
				pixelCollider2.Position = pixelPosition;
				list.Add(pixelCollider2);
			}
		}
		else if (cellData.cellVisualData.precludeAllTileDrawing && cellData.type == CellType.WALL)
		{
			PixelCollider pixelCollider3 = new PixelCollider
			{
				IsTileCollider = true,
				CollisionLayer = CollisionLayer.HighObstacle
			};
			pixelCollider3.RegenerateFromManual(tileMap.transform, new IntVector2(0, 0), new IntVector2(16, 16), 0f, null);
			pixelCollider3.Position = pixelPosition;
			list.Add(pixelCollider3);
		}
		if (cellData.isOccludedByTopWall && !GameManager.Instance.IsFoyer)
		{
			if (cellData.diagonalWallType == DiagonalWallType.SOUTHEAST || cellData.diagonalWallType == DiagonalWallType.SOUTHWEST)
			{
				PixelCollider pixelCollider4 = new PixelCollider
				{
					IsTileCollider = true,
					CollisionLayer = CollisionLayer.EnemyBlocker
				};
				pixelCollider4.RegenerateFromManual(tileMap.transform, new IntVector2(0, 0), new IntVector2(16, 28), 0f, null);
				pixelCollider4.Position = pixelPosition + new IntVector2(0, -16);
				list.Add(pixelCollider4);
				if (cellData.diagonalWallType == DiagonalWallType.SOUTHEAST)
				{
					PixelCollider pixelCollider5 = new PixelCollider
					{
						IsTileCollider = true,
						CollisionLayer = CollisionLayer.EnemyBulletBlocker
					};
					int num = 14;
					pixelCollider5.RegenerateFromLine(tileMap.transform, new IntVector2(1, num - 16), new IntVector2(16, num - 1));
					pixelCollider5.Position = pixelPosition + new IntVector2(0, num - 16);
					list.Add(pixelCollider5);
					pixelCollider5 = new PixelCollider
					{
						IsTileCollider = true,
						CollisionLayer = CollisionLayer.EnemyBulletBlocker
					};
					pixelCollider5.RegenerateFromManual(tileMap.transform, new IntVector2(1, num - 16), new IntVector2(16, num), 0f, null);
					pixelCollider5.Position = pixelPosition + new IntVector2(0, -16);
					list.Add(pixelCollider5);
					PixelCollider pixelCollider6 = new PixelCollider
					{
						IsTileCollider = true,
						CollisionLayer = CollisionLayer.HighObstacle
					};
					num = 8;
					pixelCollider6.RegenerateFromLine(tileMap.transform, new IntVector2(1, num - 16), new IntVector2(16, num - 1));
					pixelCollider6.Position = pixelPosition + new IntVector2(0, num - 16);
					list.Add(pixelCollider6);
					pixelCollider6 = new PixelCollider
					{
						IsTileCollider = true,
						CollisionLayer = CollisionLayer.HighObstacle
					};
					pixelCollider6.RegenerateFromManual(tileMap.transform, new IntVector2(1, num - 16), new IntVector2(16, num), 0f, null);
					pixelCollider6.Position = pixelPosition + new IntVector2(0, -16);
					list.Add(pixelCollider6);
				}
				else if (cellData.diagonalWallType == DiagonalWallType.SOUTHWEST)
				{
					PixelCollider pixelCollider7 = new PixelCollider
					{
						IsTileCollider = true,
						CollisionLayer = CollisionLayer.EnemyBulletBlocker
					};
					int num2 = 14;
					pixelCollider7.RegenerateFromLine(tileMap.transform, new IntVector2(0, num2 - 1), new IntVector2(15, num2 - 16));
					pixelCollider7.Position = pixelPosition + new IntVector2(0, num2 - 16);
					list.Add(pixelCollider7);
					pixelCollider7 = new PixelCollider
					{
						IsTileCollider = true,
						CollisionLayer = CollisionLayer.EnemyBulletBlocker
					};
					pixelCollider7.RegenerateFromManual(tileMap.transform, new IntVector2(1, num2 - 16), new IntVector2(16, num2), 0f, null);
					pixelCollider7.Position = pixelPosition + new IntVector2(0, -16);
					list.Add(pixelCollider7);
					PixelCollider pixelCollider8 = new PixelCollider
					{
						IsTileCollider = true,
						CollisionLayer = CollisionLayer.HighObstacle
					};
					num2 = 8;
					pixelCollider8.RegenerateFromLine(tileMap.transform, new IntVector2(0, num2 - 1), new IntVector2(15, num2 - 16));
					pixelCollider8.Position = pixelPosition + new IntVector2(0, num2 - 16);
					list.Add(pixelCollider8);
					pixelCollider8 = new PixelCollider
					{
						IsTileCollider = true,
						CollisionLayer = CollisionLayer.HighObstacle
					};
					pixelCollider8.RegenerateFromManual(tileMap.transform, new IntVector2(1, num2 - 16), new IntVector2(16, num2), 0f, null);
					pixelCollider8.Position = pixelPosition + new IntVector2(0, -16);
					list.Add(pixelCollider8);
				}
			}
			else
			{
				PixelCollider pixelCollider9 = new PixelCollider
				{
					IsTileCollider = true,
					CollisionLayer = CollisionLayer.EnemyBlocker
				};
				pixelCollider9.RegenerateFromManual(tileMap.transform, new IntVector2(0, 0), new IntVector2(16, 12), 0f, null);
				pixelCollider9.Position = pixelPosition;
				list.Add(pixelCollider9);
				PixelCollider pixelCollider10 = new PixelCollider
				{
					IsTileCollider = true,
					CollisionLayer = CollisionLayer.EnemyBulletBlocker
				};
				pixelCollider10.RegenerateFromManual(tileMap.transform, new IntVector2(0, 0), new IntVector2(16, 14), 0f, null);
				pixelCollider10.Position = pixelPosition;
				list.Add(pixelCollider10);
				PixelCollider pixelCollider11 = new PixelCollider
				{
					IsTileCollider = true,
					CollisionLayer = CollisionLayer.PlayerBlocker
				};
				pixelCollider11.RegenerateFromManual(tileMap.transform, new IntVector2(0, 0), new IntVector2(16, 8), 0f, null);
				pixelCollider11.Position = pixelPosition;
				list.Add(pixelCollider11);
			}
		}
		if (cellData.IsLowerFaceWall() && !GameManager.Instance.IsFoyer && cellData.diagonalWallType != DiagonalWallType.SOUTHEAST && cellData.diagonalWallType == DiagonalWallType.SOUTHWEST)
		{
			if (!GameManager.Instance.Dungeon.data.isWall(cellData.position.x - 1, cellData.position.y))
			{
				PixelCollider pixelCollider12 = new PixelCollider
				{
					IsTileCollider = true,
					CollisionLayer = CollisionLayer.BulletBlocker
				};
				pixelCollider12.RegenerateFromManual(tileMap.transform, new IntVector2(0, 0), new IntVector2(3, 10), 0f, null);
				pixelCollider12.Position = pixelPosition + new IntVector2(0, 6);
				pixelCollider12.DirectionIgnorer = (IntVector2 dir) => dir.x >= 0 || dir.y <= 0;
				pixelCollider12.NormalModifier = (Vector2 normal) => (normal.x <= 0f) ? normal : Vector2.down;
				list.Add(pixelCollider12);
			}
			if (!GameManager.Instance.Dungeon.data.isWall(cellData.position.x + 1, cellData.position.y))
			{
				PixelCollider pixelCollider13 = new PixelCollider
				{
					IsTileCollider = true,
					CollisionLayer = CollisionLayer.BulletBlocker
				};
				pixelCollider13.RegenerateFromManual(tileMap.transform, new IntVector2(0, 0), new IntVector2(3, 10), 0f, null);
				pixelCollider13.Position = pixelPosition + new IntVector2(13, 6);
				pixelCollider13.DirectionIgnorer = (IntVector2 dir) => dir.x <= 0 || dir.y <= 0;
				pixelCollider13.NormalModifier = (Vector2 normal) => (normal.x >= 0f) ? normal : Vector2.down;
				list.Add(pixelCollider13);
			}
		}
		if (list.Count == 0)
		{
			cellData.HasCachedPhysicsTile = true;
			cellData.CachedPhysicsTile = null;
			return null;
		}
		PhysicsEngine.Tile tile3 = new PhysicsEngine.Tile(list, x, y, layerName);
		cellData.HasCachedPhysicsTile = true;
		cellData.CachedPhysicsTile = tile3;
		return tile3;
	}

	// Token: 0x06002EFF RID: 12031 RVA: 0x000F688C File Offset: 0x000F4A8C
	private void InitNearbyTileCheck(Vector2 worldMin, Vector2 worldMax, tk2dTileMap tileMap)
	{
		if (this.m_nbt.tileMap == null)
		{
			this.m_nbt.tileMap = tileMap;
			this.m_nbt.layerName = "Collision Layer";
			this.m_nbt.layer = BraveUtility.GetTileMapLayerByName("Collision Layer", this.TileMap);
		}
		this.m_nbt.Init(worldMin.x, worldMin.y, worldMax.x, worldMax.y);
	}

	// Token: 0x06002F00 RID: 12032 RVA: 0x000F6910 File Offset: 0x000F4B10
	private void InitNearbyTileCheck(float worldMinX, float worldMinY, float worldMaxX, float worldMaxY, tk2dTileMap tileMap, IntVector2 pixelColliderDimensions, float positionX, float positionY, IntVector2 pixelsToMove, DungeonData dungeonData)
	{
		if (this.m_nbt.tileMap == null)
		{
			this.m_nbt.tileMap = tileMap;
			this.m_nbt.layerName = "Collision Layer";
			this.m_nbt.layer = BraveUtility.GetTileMapLayerByName("Collision Layer", this.TileMap);
		}
		this.m_nbt.Init(worldMinX, worldMinY, worldMaxX, worldMaxY, pixelColliderDimensions, positionX, positionY, pixelsToMove, dungeonData);
	}

	// Token: 0x06002F01 RID: 12033 RVA: 0x000F6988 File Offset: 0x000F4B88
	private PhysicsEngine.Tile GetNextNearbyTile(DungeonData dungeonData)
	{
		return this.m_nbt.GetNextNearbyTile(dungeonData);
	}

	// Token: 0x06002F02 RID: 12034 RVA: 0x000F6998 File Offset: 0x000F4B98
	private void CleanupNearbyTileCheck()
	{
		this.m_nbt.Cleanup();
	}

	// Token: 0x06002F03 RID: 12035 RVA: 0x000F69A8 File Offset: 0x000F4BA8
	private int GetTile(int layer, int x, int y)
	{
		if (x >= 0 && x < this.TileMap.width && y >= 0 && y < this.TileMap.height)
		{
			return this.TileMap.Layers[layer].GetTile(x, y);
		}
		return -1;
	}

	// Token: 0x06002F04 RID: 12036 RVA: 0x000F69FC File Offset: 0x000F4BFC
	public static void PixelMovementGenerator(IntVector2 pixelsToMove, List<PixelCollider.StepData> steps)
	{
		steps.Clear();
		IntVector2 intVector = IntVector2.Zero;
		float num = 1f / (float)(Mathf.Abs(pixelsToMove.x) + Mathf.Abs(pixelsToMove.y));
		while (intVector.x != pixelsToMove.x || intVector.y != pixelsToMove.y)
		{
			IntVector2 intVector2;
			if (intVector.x == pixelsToMove.x)
			{
				intVector2 = new IntVector2(0, Math.Sign(pixelsToMove.y));
			}
			else if (intVector.y == pixelsToMove.y)
			{
				intVector2 = new IntVector2(Math.Sign(pixelsToMove.x), 0);
			}
			else
			{
				float num2 = Mathf.Abs((float)intVector.x / (float)pixelsToMove.x);
				float num3 = Mathf.Abs((float)intVector.y / (float)pixelsToMove.y);
				if (num2 < num3)
				{
					intVector2 = new IntVector2(Math.Sign(pixelsToMove.x), 0);
				}
				else
				{
					intVector2 = new IntVector2(0, Math.Sign(pixelsToMove.y));
				}
			}
			intVector += intVector2;
			steps.Add(new PixelCollider.StepData
			{
				deltaPos = intVector2,
				deltaTime = num
			});
		}
	}

	// Token: 0x06002F05 RID: 12037 RVA: 0x000F6B48 File Offset: 0x000F4D48
	public static void PixelMovementGenerator(SpeculativeRigidbody rigidbody, List<PixelCollider.StepData> stepList)
	{
		PhysicsEngine.PixelMovementGenerator(rigidbody.m_position.m_remainder, rigidbody.Velocity, rigidbody.PixelsToMove, stepList);
	}

	// Token: 0x06002F06 RID: 12038 RVA: 0x000F6B68 File Offset: 0x000F4D68
	private static void PixelMovementGenerator(Vector2 remainder, Vector2 velocity, IntVector2 pixelsToMove, List<PixelCollider.StepData> stepList)
	{
		stepList.Clear();
		float num = 0.03125f;
		IntVector2 intVector;
		intVector.x = 0;
		intVector.y = 0;
		int num2 = Math.Sign(pixelsToMove.x);
		int num3 = Math.Sign(pixelsToMove.y);
		if (pixelsToMove.y == 0)
		{
			while (intVector.x != pixelsToMove.x)
			{
				float num4 = Mathf.Max(0f, ((float)num2 * num - remainder.x) / velocity.x);
				intVector.x += num2;
				remainder.x = (float)num2 * -num;
				remainder.y += num4 * velocity.y;
				stepList.Add(new PixelCollider.StepData(new IntVector2(num2, 0), num4));
			}
			return;
		}
		if (pixelsToMove.x == 0)
		{
			while (intVector.y != pixelsToMove.y)
			{
				float num5 = Mathf.Max(0f, ((float)num3 * num - remainder.y) / velocity.y);
				intVector.y += num3;
				remainder.x += num5 * velocity.x;
				remainder.y = (float)num3 * -num;
				stepList.Add(new PixelCollider.StepData(new IntVector2(0, num3), num5));
			}
			return;
		}
		while (intVector.x != pixelsToMove.x || intVector.y != pixelsToMove.y)
		{
			float num6 = Mathf.Max(0f, ((float)num2 * num - remainder.x) / velocity.x);
			float num7 = Mathf.Max(0f, ((float)num3 * num - remainder.y) / velocity.y);
			if (intVector.x != pixelsToMove.x && (intVector.y == pixelsToMove.y || num6 < num7))
			{
				intVector.x += num2;
				remainder.x = (float)num2 * -num;
				remainder.y += num6 * velocity.y;
				stepList.Add(new PixelCollider.StepData(new IntVector2(num2, 0), num6));
			}
			else
			{
				intVector.y += num3;
				remainder.x += num7 * velocity.x;
				remainder.y = (float)num3 * -num;
				stepList.Add(new PixelCollider.StepData(new IntVector2(0, num3), num7));
			}
		}
	}

	// Token: 0x06002F07 RID: 12039 RVA: 0x000F6DF4 File Offset: 0x000F4FF4
	private void SortRigidbodies()
	{
		bool flag = false;
		for (int i = 0; i < this.m_rigidbodies.Count; i++)
		{
			SpeculativeRigidbody speculativeRigidbody = this.m_rigidbodies[i];
			int num = ((!speculativeRigidbody.CanBePushed) ? 0 : 1) << ((!speculativeRigidbody.CanPush) ? 0 : 1) << 1 + ((!speculativeRigidbody.CanCarry) ? 0 : 1) << 2;
			if (num != speculativeRigidbody.SortHash)
			{
				flag = true;
			}
			speculativeRigidbody.SortHash = num;
		}
		if (flag)
		{
			this.m_rigidbodies.Sort(delegate(SpeculativeRigidbody lhs, SpeculativeRigidbody rhs)
			{
				if (lhs.CanCarry && !rhs.CanCarry)
				{
					return -1;
				}
				if (!lhs.CanCarry && rhs.CanCarry)
				{
					return 1;
				}
				if (lhs.CanPush && !rhs.CanPush)
				{
					return -1;
				}
				if (!lhs.CanPush && rhs.CanPush)
				{
					return 1;
				}
				if (lhs.CanPush && rhs.CanPush)
				{
					if (!lhs.CanBePushed && rhs.CanBePushed)
					{
						return -1;
					}
					if (lhs.CanBePushed && !rhs.CanBePushed)
					{
						return 1;
					}
				}
				return 0;
			});
		}
	}

	// Token: 0x06002F08 RID: 12040 RVA: 0x000F6EB4 File Offset: 0x000F50B4
	public static void UpdatePosition(SpeculativeRigidbody specRigidbody)
	{
		Vector2 vector = specRigidbody.Velocity * UnityEngine.Random.Range(0.8f, 1.2f);
		if (specRigidbody.IsSimpleProjectile)
		{
			PhysicsEngine.Instance.m_projectileTree.MoveProxy(specRigidbody.proxyId, specRigidbody.b2AABB, vector);
		}
		else
		{
			PhysicsEngine.Instance.m_rigidbodyTree.MoveProxy(specRigidbody.proxyId, specRigidbody.b2AABB, vector);
		}
	}

	// Token: 0x06002F09 RID: 12041 RVA: 0x000F6F28 File Offset: 0x000F5128
	public static float PixelToUnit(int pixel)
	{
		return (float)pixel / 16f;
	}

	// Token: 0x06002F0A RID: 12042 RVA: 0x000F6F34 File Offset: 0x000F5134
	public static Vector2 PixelToUnit(IntVector2 pixel)
	{
		return (Vector2)pixel / 16f;
	}

	// Token: 0x06002F0B RID: 12043 RVA: 0x000F6F48 File Offset: 0x000F5148
	public static float PixelToUnitMidpoint(int pixel)
	{
		return (float)pixel / 16f + PhysicsEngine.Instance.HalfPixelUnitWidth;
	}

	// Token: 0x06002F0C RID: 12044 RVA: 0x000F6F60 File Offset: 0x000F5160
	public static Vector2 PixelToUnitMidpoint(IntVector2 pixel)
	{
		return (Vector2)pixel / 16f + new Vector2(PhysicsEngine.Instance.HalfPixelUnitWidth, PhysicsEngine.Instance.HalfPixelUnitWidth);
	}

	// Token: 0x06002F0D RID: 12045 RVA: 0x000F6F90 File Offset: 0x000F5190
	public static int UnitToPixel(float unit)
	{
		return (int)(unit * 16f);
	}

	// Token: 0x06002F0E RID: 12046 RVA: 0x000F6F9C File Offset: 0x000F519C
	public static IntVector2 UnitToPixel(Vector2 unit)
	{
		return new IntVector2(PhysicsEngine.UnitToPixel(unit.x), PhysicsEngine.UnitToPixel(unit.y));
	}

	// Token: 0x06002F0F RID: 12047 RVA: 0x000F6FBC File Offset: 0x000F51BC
	public static int UnitRoundToPixel(float unit)
	{
		return Mathf.RoundToInt(unit * 16f);
	}

	// Token: 0x06002F10 RID: 12048 RVA: 0x000F6FCC File Offset: 0x000F51CC
	public static IntVector2 UnitRoundToPixel(Vector2 unit)
	{
		return new IntVector2(PhysicsEngine.UnitRoundToPixel(unit.x), PhysicsEngine.UnitRoundToPixel(unit.y));
	}

	// Token: 0x06002F11 RID: 12049 RVA: 0x000F6FEC File Offset: 0x000F51EC
	private static Vector2 GetSlopeScalar(float slope)
	{
		Vector2 vector = new Vector2(1f, slope);
		return vector.normalized;
	}

	// Token: 0x06002F12 RID: 12050 RVA: 0x000F7010 File Offset: 0x000F5210
	private static Vector2 RotateXTowardSlope(Vector2 v, float slope)
	{
		return PhysicsEngine.GetSlopeScalar(slope) * v.x + new Vector2(0f, v.y);
	}

	// Token: 0x04001F8A RID: 8074
	public static CustomSampler csSortRigidbodies;

	// Token: 0x04001F8B RID: 8075
	public static CustomSampler csPreRigidbodyMovement;

	// Token: 0x04001F8C RID: 8076
	public static CustomSampler csPreprocessing;

	// Token: 0x04001F8D RID: 8077
	public static CustomSampler csInitialRigidbodyUpdates;

	// Token: 0x04001F8E RID: 8078
	public static CustomSampler csRigidbodyCollisions;

	// Token: 0x04001F8F RID: 8079
	public static CustomSampler csInitCollisions;

	// Token: 0x04001F90 RID: 8080
	public static CustomSampler csBuildStepList;

	// Token: 0x04001F91 RID: 8081
	public static CustomSampler csMovementRestrictions;

	// Token: 0x04001F92 RID: 8082
	public static CustomSampler csCollideWithOthers;

	// Token: 0x04001F93 RID: 8083
	public static CustomSampler csCollideWithTilemap;

	// Token: 0x04001F94 RID: 8084
	public static CustomSampler csCollideWithTilemapInner;

	// Token: 0x04001F95 RID: 8085
	public static CustomSampler csCanCollideWith;

	// Token: 0x04001F96 RID: 8086
	public static CustomSampler csRigidbodyPushing;

	// Token: 0x04001F97 RID: 8087
	public static CustomSampler csResolveCollision;

	// Token: 0x04001F98 RID: 8088
	public static CustomSampler csUpdatePositionVelocity;

	// Token: 0x04001F99 RID: 8089
	public static CustomSampler csHandleCarriedObjects;

	// Token: 0x04001F9A RID: 8090
	public static CustomSampler csEndChecks;

	// Token: 0x04001F9B RID: 8091
	public static CustomSampler csEndCleanup;

	// Token: 0x04001F9C RID: 8092
	public static CustomSampler csUpdateZDepth;

	// Token: 0x04001F9D RID: 8093
	public static CustomSampler csPostRigidbodyMovement;

	// Token: 0x04001F9E RID: 8094
	public static CustomSampler csHandleTriggerCollisions;

	// Token: 0x04001F9F RID: 8095
	public static CustomSampler csClearGhostCollisions;

	// Token: 0x04001FA0 RID: 8096
	public static CustomSampler csRaycastTiles;

	// Token: 0x04001FA1 RID: 8097
	public static CustomSampler csRaycastRigidbodies;

	// Token: 0x04001FA2 RID: 8098
	public static CustomSampler csTreeCollisions;

	// Token: 0x04001FA3 RID: 8099
	public static CustomSampler csRigidbodyTreeSearch;

	// Token: 0x04001FA4 RID: 8100
	public static CustomSampler csProjectileTreeSearch;

	// Token: 0x04001FA5 RID: 8101
	public static CustomSampler csUpdatePosition;

	// Token: 0x04001FA6 RID: 8102
	public static CustomSampler csGetNextNearbyTile;

	// Token: 0x04001FA7 RID: 8103
	public static CustomSampler csCollideWithTilemapSetup;

	// Token: 0x04001FA8 RID: 8104
	public static CustomSampler csGetTiles;

	// Token: 0x04001FA9 RID: 8105
	public static CustomSampler csCollideWithTilemapSingle;

	// Token: 0x04001FAA RID: 8106
	public tk2dTileMap TileMap;

	// Token: 0x04001FAB RID: 8107
	public int PixelsPerUnit = 16;

	// Token: 0x04001FAC RID: 8108
	private const int c_warnIterations = 5;

	// Token: 0x04001FAD RID: 8109
	private const int c_maxIterations = 50;

	// Token: 0x04001FAE RID: 8110
	public PhysicsEngine.DebugDrawType DebugDraw;

	// Token: 0x04001FAF RID: 8111
	[HideInInspector]
	public Color[] DebugColors = new Color[]
	{
		Color.green,
		Color.magenta,
		Color.cyan
	};

	// Token: 0x04001FB0 RID: 8112
	private List<SpeculativeRigidbody> m_rigidbodies = new List<SpeculativeRigidbody>();

	// Token: 0x04001FB1 RID: 8113
	private b2DynamicTree m_rigidbodyTree = new b2DynamicTree();

	// Token: 0x04001FB2 RID: 8114
	private b2DynamicTree m_projectileTree = new b2DynamicTree();

	// Token: 0x04001FB3 RID: 8115
	private HashSet<IntVector2> m_debugTilesDrawnThisFrame = new HashSet<IntVector2>();

	// Token: 0x04001FB4 RID: 8116
	private static List<SpeculativeRigidbody> c_boundedRigidbodies = new List<SpeculativeRigidbody>();

	// Token: 0x04001FB5 RID: 8117
	private List<SpeculativeRigidbody> m_deregisterRigidBodies = new List<SpeculativeRigidbody>();

	// Token: 0x04001FB6 RID: 8118
	private int m_frameCount;

	// Token: 0x04001FB7 RID: 8119
	private int m_cachedProjectileMask;

	// Token: 0x04001FB8 RID: 8120
	private static PhysicsEngine m_instance;

	// Token: 0x04001FB9 RID: 8121
	public static LinearCastResult PendingCastResult;

	// Token: 0x04001FC0 RID: 8128
	private SpeculativeRigidbody[] m_emptyIgnoreList = new SpeculativeRigidbody[0];

	// Token: 0x04001FC1 RID: 8129
	private SpeculativeRigidbody[] m_singleIgnoreList = new SpeculativeRigidbody[1];

	// Token: 0x04001FC2 RID: 8130
	private static PhysicsEngine.Raycaster m_raycaster = new PhysicsEngine.Raycaster();

	// Token: 0x04001FC3 RID: 8131
	private SpeculativeRigidbody[] emptyIgnoreList = new SpeculativeRigidbody[0];

	// Token: 0x04001FC4 RID: 8132
	private static PhysicsEngine.RigidbodyCaster m_rigidbodyCaster = new PhysicsEngine.RigidbodyCaster();

	// Token: 0x04001FC5 RID: 8133
	private static SpeculativeRigidbody m_cwrqRigidbody;

	// Token: 0x04001FC6 RID: 8134
	private static List<PixelCollider.StepData> m_cwrqStepList;

	// Token: 0x04001FC7 RID: 8135
	private static CollisionData m_cwrqCollisionData;

	// Token: 0x04001FC8 RID: 8136
	private int m_cachedDungeonWidth;

	// Token: 0x04001FC9 RID: 8137
	private int m_cachedDungeonHeight;

	// Token: 0x04001FCA RID: 8138
	private PhysicsEngine.NearbyTileData m_nbt;

	// Token: 0x02000850 RID: 2128
	public enum DebugDrawType
	{
		// Token: 0x04001FD3 RID: 8147
		None,
		// Token: 0x04001FD4 RID: 8148
		Boundaries,
		// Token: 0x04001FD5 RID: 8149
		FullPixels
	}

	// Token: 0x02000851 RID: 2129
	private class Raycaster
	{
		// Token: 0x06002F19 RID: 12057 RVA: 0x000F7190 File Offset: 0x000F5390
		public Raycaster()
		{
			this.queryPointer = new Func<b2RayCastInput, SpeculativeRigidbody, float>(this.RaycastAtRigidbodiesQuery);
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x000F71AC File Offset: 0x000F53AC
		public void SetAll(PhysicsEngine physicsEngine, DungeonData dungeonData, Position origin, Vector2 direction, float dist, bool collideWithTiles, bool collideWithRigidbodies, int rayMask, CollisionLayer? sourceLayer, bool collideWithTriggers, Func<SpeculativeRigidbody, bool> rigidbodyExcluder, ICollection<SpeculativeRigidbody> ignoreList)
		{
			this.physicsEngine = physicsEngine;
			this.dungeonData = dungeonData;
			this.origin = origin;
			this.direction = direction;
			this.dist = dist;
			this.collideWithTiles = collideWithTiles;
			this.collideWithRigidbodies = collideWithRigidbodies;
			this.rayMask = rayMask;
			this.sourceLayer = sourceLayer;
			this.collideWithTriggers = collideWithTriggers;
			this.rigidbodyExcluder = rigidbodyExcluder;
			this.ignoreList = ignoreList;
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x000F7218 File Offset: 0x000F5418
		public void Clear()
		{
			this.physicsEngine = null;
			this.rigidbodyExcluder = null;
			this.ignoreList = null;
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x000F7230 File Offset: 0x000F5430
		public bool DoRaycast(out RaycastResult result)
		{
			result = null;
			this.direction.Normalize();
			if (this.collideWithTiles && this.physicsEngine.TileMap)
			{
				string text = "Collision Layer";
				int tileMapLayerByName = BraveUtility.GetTileMapLayerByName(text, this.physicsEngine.TileMap);
				IntVector2 intVector = PhysicsEngine.UnitToPixel(this.origin.UnitPosition);
				IntVector2 intVector2 = PhysicsEngine.UnitToPixel(this.direction.normalized * this.dist);
				IntVector2 intVector3 = IntVector2.Zero;
				IntVector2 intVector4 = intVector / this.physicsEngine.PixelsPerUnit;
				RaycastResult raycastResult = this.RaycastAtTile(intVector4, tileMapLayerByName, text, this.rayMask, this.sourceLayer, this.origin, this.direction, this.dist, this.dungeonData);
				if (raycastResult != null && (result == null || raycastResult.Distance < result.Distance))
				{
					RaycastResult.Pool.Free(ref result);
					result = raycastResult;
				}
				else
				{
					RaycastResult.Pool.Free(ref raycastResult);
				}
				IntVector2 intVector5 = intVector4;
				while (intVector3.x != intVector2.x || intVector3.y != intVector2.y)
				{
					IntVector2 intVector6;
					if (intVector3.x == intVector2.x)
					{
						intVector6 = new IntVector2(0, Math.Sign(intVector2.y));
					}
					else if (intVector3.y == intVector2.y)
					{
						intVector6 = new IntVector2(Math.Sign(intVector2.x), 0);
					}
					else
					{
						float num = Mathf.Abs((float)intVector3.x / (float)intVector2.x);
						float num2 = Mathf.Abs((float)intVector3.y / (float)intVector2.y);
						if (num < num2)
						{
							intVector6 = new IntVector2(Math.Sign(intVector2.x), 0);
						}
						else
						{
							intVector6 = new IntVector2(0, Math.Sign(intVector2.y));
						}
					}
					intVector3 += intVector6;
					IntVector2 intVector7 = (intVector + intVector3) / this.physicsEngine.PixelsPerUnit;
					if (intVector7 != intVector5)
					{
						RaycastResult raycastResult2 = this.RaycastAtTile(intVector7, tileMapLayerByName, text, this.rayMask, this.sourceLayer, this.origin, this.direction, this.dist, this.dungeonData);
						if (raycastResult2 != null && (result == null || raycastResult2.Distance < result.Distance))
						{
							if (raycastResult2.OtherPixelCollider.NormalModifier == null)
							{
								raycastResult2.Normal = -intVector6.ToVector2().normalized;
							}
							RaycastResult.Pool.Free(ref result);
							result = raycastResult2;
						}
						else
						{
							RaycastResult.Pool.Free(ref raycastResult2);
						}
						intVector5 = intVector7;
					}
				}
			}
			if (this.collideWithRigidbodies)
			{
				this.nearestRigidbodyHit = null;
				this.p1 = this.origin.UnitPosition;
				Vector2 vector = this.p1 + this.direction * this.dist;
				this.p1p2Dist = Vector2.Distance(this.p1, vector);
				this.physicsEngine.m_rigidbodyTree.RayCast(new b2RayCastInput(this.p1, vector), this.queryPointer);
				if (this.physicsEngine.CollidesWithProjectiles(this.rayMask, this.sourceLayer))
				{
					this.physicsEngine.m_projectileTree.RayCast(new b2RayCastInput(this.p1, vector), this.queryPointer);
				}
				if (this.nearestRigidbodyHit != null)
				{
					if (result == null || this.nearestRigidbodyHit.Distance < result.Distance)
					{
						RaycastResult.Pool.Free(ref result);
						result = this.nearestRigidbodyHit;
					}
					else
					{
						RaycastResult.Pool.Free(ref this.nearestRigidbodyHit);
					}
				}
			}
			return result != null;
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x000F760C File Offset: 0x000F580C
		private RaycastResult RaycastAtTile(IntVector2 pos, int layer, string layerName, int rayMask, CollisionLayer? sourceLayer, Position origin, Vector2 direction, float dist, DungeonData dungeonData)
		{
			PhysicsEngine.Tile tile = this.physicsEngine.GetTile(pos.x, pos.y, this.physicsEngine.TileMap, layer, layerName, dungeonData);
			RaycastResult raycastResult = null;
			if (tile == null || tile.PixelColliders == null || tile.PixelColliders.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < tile.PixelColliders.Count; i++)
			{
				PixelCollider pixelCollider = tile.PixelColliders[i];
				if (pixelCollider.CanCollideWith(rayMask, sourceLayer))
				{
					RaycastResult raycastResult2;
					if (pixelCollider.Raycast(origin.UnitPosition, direction, dist, out raycastResult2))
					{
						if (raycastResult == null || raycastResult2.Distance < raycastResult.Distance)
						{
							RaycastResult.Pool.Free(ref raycastResult);
							raycastResult = raycastResult2;
						}
						else
						{
							RaycastResult.Pool.Free(ref raycastResult2);
						}
					}
					else
					{
						RaycastResult.Pool.Free(ref raycastResult2);
					}
				}
			}
			return raycastResult;
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x000F7704 File Offset: 0x000F5904
		private float RaycastAtRigidbodiesQuery(b2RayCastInput rayCastInput, SpeculativeRigidbody rigidbody)
		{
			float num = rayCastInput.maxFraction;
			if (rigidbody && rigidbody.enabled && rigidbody.CollideWithOthers && (this.ignoreList == null || !this.ignoreList.Contains(rigidbody)))
			{
				if (this.rigidbodyExcluder != null && this.rigidbodyExcluder(rigidbody))
				{
					return num;
				}
				for (int i = 0; i < rigidbody.PixelColliders.Count; i++)
				{
					PixelCollider pixelCollider = rigidbody.PixelColliders[i];
					if (this.collideWithTriggers || !pixelCollider.IsTrigger)
					{
						if (pixelCollider.CanCollideWith(this.rayMask, this.sourceLayer))
						{
							RaycastResult raycastResult;
							if (pixelCollider.Raycast(this.origin.UnitPosition, this.direction, this.dist, out raycastResult))
							{
								if (this.nearestRigidbodyHit == null || raycastResult.Distance < this.nearestRigidbodyHit.Distance)
								{
									RaycastResult.Pool.Free(ref this.nearestRigidbodyHit);
									this.nearestRigidbodyHit = raycastResult;
									this.nearestRigidbodyHit.SpeculativeRigidbody = rigidbody;
									num = Vector2.Distance(this.p1, this.nearestRigidbodyHit.Contact) / this.p1p2Dist;
								}
								else
								{
									RaycastResult.Pool.Free(ref raycastResult);
								}
							}
							else
							{
								RaycastResult.Pool.Free(ref raycastResult);
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x04001FD6 RID: 8150
		private PhysicsEngine physicsEngine;

		// Token: 0x04001FD7 RID: 8151
		private DungeonData dungeonData;

		// Token: 0x04001FD8 RID: 8152
		private Position origin;

		// Token: 0x04001FD9 RID: 8153
		private Vector2 direction;

		// Token: 0x04001FDA RID: 8154
		private float dist;

		// Token: 0x04001FDB RID: 8155
		private bool collideWithTiles;

		// Token: 0x04001FDC RID: 8156
		private bool collideWithRigidbodies;

		// Token: 0x04001FDD RID: 8157
		private int rayMask;

		// Token: 0x04001FDE RID: 8158
		private CollisionLayer? sourceLayer;

		// Token: 0x04001FDF RID: 8159
		private bool collideWithTriggers;

		// Token: 0x04001FE0 RID: 8160
		private Func<SpeculativeRigidbody, bool> rigidbodyExcluder;

		// Token: 0x04001FE1 RID: 8161
		private ICollection<SpeculativeRigidbody> ignoreList;

		// Token: 0x04001FE2 RID: 8162
		private RaycastResult nearestRigidbodyHit;

		// Token: 0x04001FE3 RID: 8163
		private Vector2 p1;

		// Token: 0x04001FE4 RID: 8164
		private float p1p2Dist;

		// Token: 0x04001FE5 RID: 8165
		private Func<b2RayCastInput, SpeculativeRigidbody, float> queryPointer;
	}

	// Token: 0x02000852 RID: 2130
	public enum PointCollisionState
	{
		// Token: 0x04001FE7 RID: 8167
		Clean,
		// Token: 0x04001FE8 RID: 8168
		HitBeforeNext,
		// Token: 0x04001FE9 RID: 8169
		Hit
	}

	// Token: 0x02000853 RID: 2131
	private class RigidbodyCaster
	{
		// Token: 0x06002F1F RID: 12063 RVA: 0x000F7878 File Offset: 0x000F5A78
		public RigidbodyCaster()
		{
			this.callbackPointer = new Func<SpeculativeRigidbody, bool>(this.RigidbodyCollisionCallback);
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x000F7894 File Offset: 0x000F5A94
		public void SetAll(PhysicsEngine physicsEngine, DungeonData dungeonData, SpeculativeRigidbody rigidbody, IntVector2 pixelsToMove, bool collideWithTiles, bool collideWithRigidbodies, int? overrideCollisionMask, bool collideWithTriggers, SpeculativeRigidbody[] ignoreList)
		{
			this.physicsEngine = physicsEngine;
			this.dungeonData = dungeonData;
			this.rigidbody = rigidbody;
			this.pixelsToMove = pixelsToMove;
			this.collideWithTiles = collideWithTiles;
			this.collideWithRigidbodies = collideWithRigidbodies;
			this.overrideCollisionMask = overrideCollisionMask;
			this.collideWithTriggers = collideWithTriggers;
			this.ignoreList = ignoreList;
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x000F78E8 File Offset: 0x000F5AE8
		public void Clear()
		{
			this.physicsEngine = null;
			this.rigidbody = null;
			this.ignoreList = null;
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x000F7900 File Offset: 0x000F5B00
		public bool DoRigidbodyCast(out CollisionData result)
		{
			this.tempResult = null;
			if (!this.rigidbody || this.rigidbody.PixelColliders.Count == 0)
			{
				result = null;
				return false;
			}
			this.stepList = PixelCollider.m_stepList;
			PhysicsEngine.PixelMovementGenerator(this.pixelsToMove, this.stepList);
			IntVector2 intVector = IntVector2.MaxValue;
			IntVector2 intVector2 = IntVector2.MinValue;
			for (int i = 0; i < this.rigidbody.PixelColliders.Count; i++)
			{
				PixelCollider pixelCollider = this.rigidbody.PixelColliders[i];
				intVector = IntVector2.Min(intVector, pixelCollider.Min);
				intVector2 = IntVector2.Max(intVector2, pixelCollider.Max);
			}
			IntVector2 intVector3 = IntVector2.Min(intVector, intVector + this.pixelsToMove);
			IntVector2 intVector4 = IntVector2.Max(intVector2, intVector2 + this.pixelsToMove);
			if (this.collideWithTiles && this.physicsEngine.TileMap)
			{
				this.physicsEngine.InitNearbyTileCheck(PhysicsEngine.PixelToUnit(intVector3 - IntVector2.One), PhysicsEngine.PixelToUnit(intVector4 + IntVector2.One), this.physicsEngine.TileMap);
				for (PhysicsEngine.Tile tile = this.physicsEngine.GetNextNearbyTile(this.dungeonData); tile != null; tile = this.physicsEngine.GetNextNearbyTile(this.dungeonData))
				{
					for (int j = 0; j < this.rigidbody.PixelColliders.Count; j++)
					{
						PixelCollider pixelCollider2 = this.rigidbody.PixelColliders[j];
						if (this.collideWithTriggers || !pixelCollider2.IsTrigger)
						{
							for (int k = 0; k < tile.PixelColliders.Count; k++)
							{
								PixelCollider pixelCollider3 = tile.PixelColliders[k];
								LinearCastResult linearCastResult;
								if (pixelCollider2.CanCollideWith(pixelCollider3, false) && pixelCollider2.AABBOverlaps(pixelCollider3, this.pixelsToMove) && pixelCollider2.LinearCast(pixelCollider3, this.rigidbody.PixelsToMove, this.stepList, out linearCastResult, false, 0f))
								{
									if (this.tempResult == null || linearCastResult.TimeUsed < this.tempResult.TimeUsed)
									{
										if (this.tempResult == null)
										{
											this.tempResult = CollisionData.Pool.Allocate();
										}
										this.tempResult.SetAll(linearCastResult);
										this.tempResult.collisionType = CollisionData.CollisionType.TileMap;
										this.tempResult.MyRigidbody = this.rigidbody;
										this.tempResult.TileLayerName = "Collision Layer";
										this.tempResult.TilePosition = tile.Position;
									}
									LinearCastResult.Pool.Free(ref linearCastResult);
								}
							}
						}
					}
				}
			}
			if (this.collideWithRigidbodies)
			{
				this.physicsEngine.m_rigidbodyTree.Query(PhysicsEngine.GetSafeB2AABB(intVector3, intVector4), this.callbackPointer);
				if (this.overrideCollisionMask != null)
				{
					if ((this.overrideCollisionMask.Value & this.physicsEngine.m_cachedProjectileMask) == this.physicsEngine.m_cachedProjectileMask)
					{
						this.physicsEngine.m_projectileTree.Query(PhysicsEngine.GetSafeB2AABB(intVector3, intVector4), this.callbackPointer);
					}
				}
				else if (this.physicsEngine.CollidesWithProjectiles(this.rigidbody))
				{
					this.physicsEngine.m_projectileTree.Query(PhysicsEngine.GetSafeB2AABB(intVector3, intVector4), this.callbackPointer);
				}
			}
			result = this.tempResult;
			return result != null;
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x000F7C94 File Offset: 0x000F5E94
		private bool RigidbodyCollisionCallback(SpeculativeRigidbody otherRigidbody)
		{
			if (otherRigidbody && otherRigidbody != this.rigidbody && otherRigidbody.enabled && otherRigidbody.CollideWithOthers && Array.IndexOf<SpeculativeRigidbody>(this.ignoreList, otherRigidbody) < 0)
			{
				for (int i = 0; i < this.rigidbody.PixelColliders.Count; i++)
				{
					PixelCollider pixelCollider = this.rigidbody.PixelColliders[i];
					if (this.collideWithTriggers || !pixelCollider.IsTrigger)
					{
						for (int j = 0; j < otherRigidbody.PixelColliders.Count; j++)
						{
							PixelCollider pixelCollider2 = otherRigidbody.PixelColliders[j];
							if (this.collideWithTriggers || !pixelCollider2.IsTrigger)
							{
								bool flag;
								if (this.overrideCollisionMask != null)
								{
									flag = pixelCollider2.CanCollideWith(this.overrideCollisionMask.Value, null);
								}
								else
								{
									flag = pixelCollider.CanCollideWith(pixelCollider2, false);
								}
								LinearCastResult linearCastResult;
								if (flag && pixelCollider.AABBOverlaps(pixelCollider2, this.pixelsToMove) && pixelCollider.LinearCast(pixelCollider2, this.rigidbody.PixelsToMove, this.stepList, out linearCastResult, false, 0f))
								{
									if (this.tempResult == null || linearCastResult.TimeUsed < this.tempResult.TimeUsed)
									{
										if (this.tempResult == null)
										{
											this.tempResult = CollisionData.Pool.Allocate();
										}
										this.tempResult.SetAll(linearCastResult);
										this.tempResult.collisionType = CollisionData.CollisionType.Rigidbody;
										this.tempResult.MyRigidbody = this.rigidbody;
										this.tempResult.OtherRigidbody = otherRigidbody;
									}
									LinearCastResult.Pool.Free(ref linearCastResult);
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x04001FEA RID: 8170
		private PhysicsEngine physicsEngine;

		// Token: 0x04001FEB RID: 8171
		private DungeonData dungeonData;

		// Token: 0x04001FEC RID: 8172
		private SpeculativeRigidbody rigidbody;

		// Token: 0x04001FED RID: 8173
		private IntVector2 pixelsToMove;

		// Token: 0x04001FEE RID: 8174
		private bool collideWithTiles;

		// Token: 0x04001FEF RID: 8175
		private bool collideWithRigidbodies;

		// Token: 0x04001FF0 RID: 8176
		private int? overrideCollisionMask;

		// Token: 0x04001FF1 RID: 8177
		private bool collideWithTriggers;

		// Token: 0x04001FF2 RID: 8178
		private SpeculativeRigidbody[] ignoreList;

		// Token: 0x04001FF3 RID: 8179
		private CollisionData tempResult;

		// Token: 0x04001FF4 RID: 8180
		private List<PixelCollider.StepData> stepList;

		// Token: 0x04001FF5 RID: 8181
		private Func<SpeculativeRigidbody, bool> callbackPointer;
	}

	// Token: 0x02000854 RID: 2132
	private struct NearbyTileData
	{
		// Token: 0x06002F24 RID: 12068 RVA: 0x000F7E74 File Offset: 0x000F6074
		public void Init(float worldMinX, float worldMinY, float worldMaxX, float worldMaxY)
		{
			this.type = PhysicsEngine.NearbyTileData.Type.FullRect;
			this.baseX = (int)worldMinX;
			this.baseY = (int)worldMinY;
			this.width = (int)worldMaxX - this.baseX + 1;
			int num = (int)worldMaxY - this.baseY + 1;
			this.i = 0;
			this.imax = this.width * num;
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x000F7ECC File Offset: 0x000F60CC
		public void Init(float worldMinX, float worldMinY, float worldMaxX, float worldMaxY, IntVector2 pixelColliderDimensions, float positionX, float positionY, IntVector2 pixelsToMove, DungeonData dungeonData)
		{
			this.finished = false;
			this.minPlotX = (int)worldMinX;
			this.minPlotY = (int)worldMinY;
			this.maxPlotX = (int)worldMaxX;
			this.maxPlotY = (int)worldMaxY;
			int num = ((int)worldMaxX - (int)worldMinX + 1) * ((int)worldMaxY - (int)worldMinY + 1);
			if (num <= 6)
			{
				this.type = PhysicsEngine.NearbyTileData.Type.FullRectPrecalc;
				this.GetAllNearbyTiles(worldMinX, worldMinY, worldMaxX, worldMaxY, dungeonData);
			}
			else
			{
				float num2 = (float)pixelColliderDimensions.x * 0.0625f * 0.5f;
				float num3 = (float)pixelColliderDimensions.y * 0.0625f * 0.5f;
				float num4 = positionX + num2;
				float num5 = positionY + num3;
				this.x = (int)num4;
				this.y = (int)num5;
				this.endX = (int)(num4 + (float)pixelsToMove.x * 0.0625f);
				this.endY = (int)(num5 + (float)pixelsToMove.y * 0.0625f);
				this.deltaX = this.endX - this.x;
				this.deltaY = this.endY - this.y;
				this.extentsX = Mathf.CeilToInt(num2 + 0.25f);
				this.extentsY = Mathf.CeilToInt(num3 + 0.25f);
				if (this.deltaX == 0)
				{
					this.type = PhysicsEngine.NearbyTileData.Type.BresenhamVertical;
					this.yStep = (int)Mathf.Sign((float)this.deltaY);
					for (int i = -this.extentsY; i < 0; i++)
					{
						for (int j = -this.extentsX; j <= this.extentsX; j++)
						{
							this.Plot(this.x + j, this.y + this.yStep * i, Color.blue, dungeonData, false);
						}
					}
				}
				else if (this.deltaY == 0)
				{
					this.type = PhysicsEngine.NearbyTileData.Type.BresenhamHorizontal;
					this.xStep = (int)Mathf.Sign((float)this.deltaX);
					for (int k = -this.extentsX; k < 0; k++)
					{
						for (int l = -this.extentsY; l <= this.extentsY; l++)
						{
							this.Plot(this.x + this.xStep * k, this.y + l, Color.blue, dungeonData, false);
						}
					}
				}
				else if (Mathf.Abs(this.deltaX) >= Mathf.Abs(this.deltaY))
				{
					this.type = PhysicsEngine.NearbyTileData.Type.BresenhamShallow;
					this.xStep = (int)Mathf.Sign((float)this.deltaX);
					this.yStep = (int)Mathf.Sign((float)this.deltaY);
					for (int m = -this.extentsX; m < 0; m++)
					{
						for (int n = -this.extentsY; n <= this.extentsY; n++)
						{
							this.Plot(this.x + this.xStep * m, this.y + n, Color.blue, dungeonData, false);
						}
					}
					this.deltaError = Mathf.Abs((float)this.deltaY / (float)this.deltaX);
					this.error = 0f;
				}
				else
				{
					this.type = PhysicsEngine.NearbyTileData.Type.BresenhamSteep;
					this.xStep = (int)Mathf.Sign((float)this.deltaX);
					this.yStep = (int)Mathf.Sign((float)this.deltaY);
					for (int num6 = -this.extentsY; num6 < 0; num6++)
					{
						for (int num7 = -this.extentsX; num7 <= this.extentsX; num7++)
						{
							this.Plot(this.x + num7, this.y + this.yStep * num6, Color.blue, dungeonData, false);
						}
					}
					this.deltaError = Mathf.Abs((float)this.deltaX / (float)this.deltaY);
					this.error = 0f;
				}
			}
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x000F829C File Offset: 0x000F649C
		private void Plot(int x, int y, Color color, DungeonData dungeonData, bool core = false)
		{
			CellData cellData;
			if (x < 0 || x < this.minPlotX || x >= PhysicsEngine.m_instance.m_cachedDungeonWidth || y < 0 || y >= PhysicsEngine.m_instance.m_cachedDungeonHeight || x < this.minPlotX || x > this.maxPlotX || y < this.minPlotY || y > this.maxPlotY || (cellData = dungeonData.cellData[x][y]) == null || (cellData.HasCachedPhysicsTile && cellData.CachedPhysicsTile == null))
			{
				return;
			}
			if (cellData.HasCachedPhysicsTile)
			{
				PhysicsEngine.NearbyTileData.m_tiles.Add(cellData.CachedPhysicsTile);
			}
			else
			{
				PhysicsEngine.Tile tile = PhysicsEngine.Instance.GetTile(x, y, this.tileMap, this.layer, this.layerName, dungeonData);
				if (tile != null)
				{
					PhysicsEngine.NearbyTileData.m_tiles.Add(tile);
				}
			}
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x000F8390 File Offset: 0x000F6590
		public void Finish(DungeonData dungeonData, bool preMove = false)
		{
			if (this.finished)
			{
				return;
			}
			int num = this.x;
			int num2 = this.y;
			switch (this.type)
			{
			case PhysicsEngine.NearbyTileData.Type.BresenhamVertical:
			{
				if (!preMove)
				{
					num2 -= this.yStep;
				}
				for (int i = 1; i <= this.extentsY; i++)
				{
					for (int j = -this.extentsX; j <= this.extentsX; j++)
					{
						this.Plot(num + j, num2 + this.yStep * i, Color.blue, dungeonData, false);
					}
				}
				break;
			}
			case PhysicsEngine.NearbyTileData.Type.BresenhamHorizontal:
			{
				if (!preMove)
				{
					num -= this.xStep;
				}
				for (int k = 1; k <= this.extentsX; k++)
				{
					for (int l = -this.extentsY; l <= this.extentsY; l++)
					{
						this.Plot(num + this.xStep * k, num2 + l, Color.blue, dungeonData, false);
					}
				}
				break;
			}
			case PhysicsEngine.NearbyTileData.Type.BresenhamShallow:
			{
				if (!preMove)
				{
					num -= this.xStep;
				}
				for (int m = 1; m <= this.extentsX; m++)
				{
					for (int n = -this.extentsY; n <= this.extentsY; n++)
					{
						this.Plot(num + this.xStep * m, num2 + n, Color.blue, dungeonData, false);
					}
				}
				break;
			}
			case PhysicsEngine.NearbyTileData.Type.BresenhamSteep:
			{
				if (!preMove)
				{
					num2 -= this.yStep;
				}
				for (int num3 = 1; num3 <= this.extentsY; num3++)
				{
					for (int num4 = -this.extentsX; num4 <= this.extentsX; num4++)
					{
						this.Plot(num + num4, num2 + this.yStep * num3, Color.blue, dungeonData, false);
					}
				}
				break;
			}
			}
			this.finished = true;
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000F8598 File Offset: 0x000F6798
		public PhysicsEngine.Tile GetNextNearbyTile(DungeonData dungeonData)
		{
			switch (this.type)
			{
			case PhysicsEngine.NearbyTileData.Type.FullRect:
				while (this.i < this.imax)
				{
					PhysicsEngine.Tile tile = PhysicsEngine.Instance.GetTile(this.baseX + this.i % this.width, this.baseY + this.i / this.width, this.tileMap, this.layer, this.layerName, dungeonData);
					this.i++;
					if (tile != null)
					{
						return tile;
					}
				}
				return null;
			case PhysicsEngine.NearbyTileData.Type.FullRectPrecalc:
				if (PhysicsEngine.NearbyTileData.m_tiles.Count > 0)
				{
					int num = PhysicsEngine.NearbyTileData.m_tiles.Count - 1;
					PhysicsEngine.Tile tile2 = PhysicsEngine.NearbyTileData.m_tiles[num];
					PhysicsEngine.NearbyTileData.m_tiles.RemoveAt(num);
					return tile2;
				}
				return null;
			case PhysicsEngine.NearbyTileData.Type.BresenhamVertical:
				while (!this.finished && PhysicsEngine.NearbyTileData.m_tiles.Count == 0)
				{
					for (int i = -this.extentsX; i <= this.extentsX; i++)
					{
						this.Plot(this.x + i, this.y, Color.yellow, dungeonData, i == 0);
					}
					if (this.y == this.endY)
					{
						this.Finish(dungeonData, true);
					}
					this.y += this.yStep;
				}
				if (PhysicsEngine.NearbyTileData.m_tiles.Count > 0)
				{
					int num2 = PhysicsEngine.NearbyTileData.m_tiles.Count - 1;
					PhysicsEngine.Tile tile3 = PhysicsEngine.NearbyTileData.m_tiles[num2];
					PhysicsEngine.NearbyTileData.m_tiles.RemoveAt(num2);
					return tile3;
				}
				return null;
			case PhysicsEngine.NearbyTileData.Type.BresenhamHorizontal:
				while (!this.finished && PhysicsEngine.NearbyTileData.m_tiles.Count == 0)
				{
					for (int j = -this.extentsY; j <= this.extentsY; j++)
					{
						this.Plot(this.x, this.y + j, Color.yellow, dungeonData, j == 0);
					}
					if (this.x == this.endX)
					{
						this.Finish(dungeonData, true);
					}
					this.x += this.xStep;
				}
				if (PhysicsEngine.NearbyTileData.m_tiles.Count > 0)
				{
					int num3 = PhysicsEngine.NearbyTileData.m_tiles.Count - 1;
					PhysicsEngine.Tile tile4 = PhysicsEngine.NearbyTileData.m_tiles[num3];
					PhysicsEngine.NearbyTileData.m_tiles.RemoveAt(num3);
					return tile4;
				}
				return null;
			case PhysicsEngine.NearbyTileData.Type.BresenhamShallow:
				while (!this.finished && PhysicsEngine.NearbyTileData.m_tiles.Count == 0)
				{
					if (this.error >= 0.5f)
					{
						for (int k = 1; k <= this.extentsX; k++)
						{
							this.Plot(this.x - this.xStep + this.xStep * k, this.y - this.yStep * this.extentsY, Color.blue, dungeonData, false);
						}
						this.y += this.yStep;
						this.error -= 1f;
						for (int l = -1; l >= -this.extentsX; l--)
						{
							this.Plot(this.x + this.xStep * l, this.y + this.yStep * this.extentsY, Color.blue, dungeonData, false);
						}
					}
					for (int m = -this.extentsY; m <= this.extentsY; m++)
					{
						this.Plot(this.x, this.y + m, Color.green, dungeonData, m == 0);
					}
					this.error += this.deltaError;
					if (this.x == this.endX)
					{
						this.Finish(dungeonData, true);
					}
					this.x += this.xStep;
				}
				if (PhysicsEngine.NearbyTileData.m_tiles.Count > 0)
				{
					int num4 = PhysicsEngine.NearbyTileData.m_tiles.Count - 1;
					PhysicsEngine.Tile tile5 = PhysicsEngine.NearbyTileData.m_tiles[num4];
					PhysicsEngine.NearbyTileData.m_tiles.RemoveAt(num4);
					return tile5;
				}
				return null;
			case PhysicsEngine.NearbyTileData.Type.BresenhamSteep:
				while (!this.finished && PhysicsEngine.NearbyTileData.m_tiles.Count == 0)
				{
					if (this.error >= 0.5f)
					{
						for (int n = 1; n <= this.extentsY; n++)
						{
							this.Plot(this.x - this.xStep * this.extentsX, this.y - this.yStep + this.yStep * n, Color.blue, dungeonData, false);
						}
						this.x += this.xStep;
						this.error -= 1f;
						for (int num5 = -1; num5 >= -this.extentsY; num5--)
						{
							this.Plot(this.x + this.xStep * this.extentsX, this.y + this.yStep * num5, Color.blue, dungeonData, false);
						}
					}
					for (int num6 = -this.extentsX; num6 <= this.extentsX; num6++)
					{
						this.Plot(this.x + num6, this.y, Color.green, dungeonData, num6 == 0);
					}
					this.error += this.deltaError;
					if (this.y == this.endY)
					{
						this.Finish(dungeonData, true);
					}
					this.y += this.yStep;
				}
				if (PhysicsEngine.NearbyTileData.m_tiles.Count > 0)
				{
					int num7 = PhysicsEngine.NearbyTileData.m_tiles.Count - 1;
					PhysicsEngine.Tile tile6 = PhysicsEngine.NearbyTileData.m_tiles[num7];
					PhysicsEngine.NearbyTileData.m_tiles.RemoveAt(num7);
					return tile6;
				}
				return null;
			default:
				return null;
			}
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000F8B70 File Offset: 0x000F6D70
		private void GetAllNearbyTiles(float worldMinX, float worldMinY, float worldMaxX, float worldMaxY, DungeonData dungeonData)
		{
			this.baseX = (int)worldMinX;
			this.baseY = (int)worldMinY;
			this.width = (int)worldMaxX - this.baseX + 1;
			this.imax = this.width * ((int)worldMaxY - this.baseY + 1);
			this.i = 0;
			while (this.i < this.imax)
			{
				int num = this.baseX + this.i % this.width;
				int num2 = this.baseY + this.i / this.width;
				CellData cellData;
				if (num >= 0 && num < PhysicsEngine.m_instance.m_cachedDungeonWidth && num2 >= 0 && num2 < PhysicsEngine.m_instance.m_cachedDungeonHeight && (cellData = dungeonData.cellData[num][num2]) != null && (!cellData.HasCachedPhysicsTile || cellData.CachedPhysicsTile != null))
				{
					if (cellData.HasCachedPhysicsTile)
					{
						PhysicsEngine.NearbyTileData.m_tiles.Add(cellData.CachedPhysicsTile);
					}
					else
					{
						PhysicsEngine.Tile tile = PhysicsEngine.Instance.GetTile(num, num2, this.tileMap, this.layer, this.layerName, dungeonData);
						if (tile != null)
						{
							PhysicsEngine.NearbyTileData.m_tiles.Add(tile);
						}
					}
				}
				this.i++;
			}
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000F8CBC File Offset: 0x000F6EBC
		public void Cleanup()
		{
			PhysicsEngine.NearbyTileData.m_tiles.Clear();
		}

		// Token: 0x04001FF6 RID: 8182
		public tk2dTileMap tileMap;

		// Token: 0x04001FF7 RID: 8183
		public int layer;

		// Token: 0x04001FF8 RID: 8184
		public string layerName;

		// Token: 0x04001FF9 RID: 8185
		private PhysicsEngine.NearbyTileData.Type type;

		// Token: 0x04001FFA RID: 8186
		private int minPlotX;

		// Token: 0x04001FFB RID: 8187
		private int minPlotY;

		// Token: 0x04001FFC RID: 8188
		private int maxPlotX;

		// Token: 0x04001FFD RID: 8189
		private int maxPlotY;

		// Token: 0x04001FFE RID: 8190
		private int baseX;

		// Token: 0x04001FFF RID: 8191
		private int baseY;

		// Token: 0x04002000 RID: 8192
		private int width;

		// Token: 0x04002001 RID: 8193
		private int i;

		// Token: 0x04002002 RID: 8194
		private int imax;

		// Token: 0x04002003 RID: 8195
		private bool finished;

		// Token: 0x04002004 RID: 8196
		private int x;

		// Token: 0x04002005 RID: 8197
		private int y;

		// Token: 0x04002006 RID: 8198
		private int extentsX;

		// Token: 0x04002007 RID: 8199
		private int extentsY;

		// Token: 0x04002008 RID: 8200
		private int endX;

		// Token: 0x04002009 RID: 8201
		private int endY;

		// Token: 0x0400200A RID: 8202
		private int deltaX;

		// Token: 0x0400200B RID: 8203
		private int deltaY;

		// Token: 0x0400200C RID: 8204
		private int xStep;

		// Token: 0x0400200D RID: 8205
		private int yStep;

		// Token: 0x0400200E RID: 8206
		private float deltaError;

		// Token: 0x0400200F RID: 8207
		private float error;

		// Token: 0x04002010 RID: 8208
		private static List<PhysicsEngine.Tile> m_tiles = new List<PhysicsEngine.Tile>();

		// Token: 0x02000855 RID: 2133
		private enum Type
		{
			// Token: 0x04002012 RID: 8210
			FullRect,
			// Token: 0x04002013 RID: 8211
			FullRectPrecalc,
			// Token: 0x04002014 RID: 8212
			BresenhamVertical,
			// Token: 0x04002015 RID: 8213
			BresenhamHorizontal,
			// Token: 0x04002016 RID: 8214
			BresenhamShallow,
			// Token: 0x04002017 RID: 8215
			BresenhamSteep
		}
	}

	// Token: 0x02000856 RID: 2134
	public class Tile : ICollidableObject
	{
		// Token: 0x06002F2C RID: 12076 RVA: 0x000F8CD4 File Offset: 0x000F6ED4
		public Tile()
		{
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x000F8CE8 File Offset: 0x000F6EE8
		public Tile(List<PixelCollider> pixelColliders, int x, int y, string layerName)
		{
			this.PixelColliders = pixelColliders;
			this.X = x;
			this.Y = y;
			this.LayerName = layerName;
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002F2E RID: 12078 RVA: 0x000F8D18 File Offset: 0x000F6F18
		public IntVector2 Position
		{
			get
			{
				return new IntVector2(this.X, this.Y);
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002F2F RID: 12079 RVA: 0x000F8D2C File Offset: 0x000F6F2C
		public PixelCollider PrimaryPixelCollider
		{
			get
			{
				if (this.PixelColliders == null || this.PixelColliders.Count == 0)
				{
					return null;
				}
				if (this.PixelColliders[0].CollisionLayer == CollisionLayer.EnemyBlocker)
				{
					return null;
				}
				return this.PixelColliders[0];
			}
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x000F8D7C File Offset: 0x000F6F7C
		public bool CanCollideWith(SpeculativeRigidbody rigidbody)
		{
			return true;
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x000F8D80 File Offset: 0x000F6F80
		public List<PixelCollider> GetPixelColliders()
		{
			return this.PixelColliders;
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x000F8D88 File Offset: 0x000F6F88
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x000F8D94 File Offset: 0x000F6F94
		public override int GetHashCode()
		{
			return this.LayerName.GetHashCode() & this.X.GetHashCode() & this.Y.GetHashCode();
		}

		// Token: 0x04002018 RID: 8216
		public int X;

		// Token: 0x04002019 RID: 8217
		public int Y;

		// Token: 0x0400201A RID: 8218
		public string LayerName;

		// Token: 0x0400201B RID: 8219
		public List<PixelCollider> PixelColliders = new List<PixelCollider>();
	}
}
