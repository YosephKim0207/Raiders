using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x0200115F RID: 4447
public class ForgeFlamePipeController : BraveBehaviour, IPlaceConfigurable
{
	// Token: 0x060062B4 RID: 25268 RVA: 0x002644D0 File Offset: 0x002626D0
	public void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnBeamCollision = (SpeculativeRigidbody.OnBeamCollisionDelegate)Delegate.Combine(specRigidbody2.OnBeamCollision, new SpeculativeRigidbody.OnBeamCollisionDelegate(this.HandleBeamCollision));
	}

	// Token: 0x060062B5 RID: 25269 RVA: 0x0026452C File Offset: 0x0026272C
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (this.m_hasBurst)
		{
			return;
		}
		if (rigidbodyCollision.OtherRigidbody.projectile)
		{
			this.m_hasBurst = true;
			base.StartCoroutine(this.HandleBurst());
		}
	}

	// Token: 0x060062B6 RID: 25270 RVA: 0x00264564 File Offset: 0x00262764
	private void HandleBeamCollision(BeamController beamController)
	{
		if (this.m_hasBurst)
		{
			return;
		}
		this.m_hasBurst = true;
		base.StartCoroutine(this.HandleBurst());
	}

	// Token: 0x060062B7 RID: 25271 RVA: 0x00264588 File Offset: 0x00262788
	private IEnumerator HandleBurst()
	{
		float elapsed = 0f;
		float bulletTimer = 0f;
		if (!string.IsNullOrEmpty(this.EndSpriteName))
		{
			base.sprite.SetSprite(this.EndSpriteName);
		}
		AkSoundEngine.PostEvent("Play_TRP_flame_torch_01", base.gameObject);
		while (elapsed < this.TimeToSpew)
		{
			elapsed += BraveTime.DeltaTime;
			bulletTimer -= BraveTime.DeltaTime;
			if (bulletTimer <= 0f)
			{
				for (int i = 0; i < this.vfxAnimators.Length; i++)
				{
					if (!this.vfxAnimators[i].renderer.enabled)
					{
						this.vfxAnimators[i].renderer.enabled = true;
						this.vfxAnimators[i].Play(this.LoopAnimationName);
					}
				}
				GlobalSparksDoer.DoLinearParticleBurst(Mathf.Max(4, (int)(BraveTime.DeltaTime * 80f)), this.ShootPoint.position, this.ShootPoint.position + DungeonData.GetIntVector2FromDirection(this.DirectionToSpew).ToVector3() * 2f, 120f, 1.5f, 0.75f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
				for (int j = 0; j < base.specRigidbody.PixelColliders[1].TriggerCollisions.Count; j++)
				{
					SpeculativeRigidbody specRigidbody = base.specRigidbody.PixelColliders[1].TriggerCollisions[j].SpecRigidbody;
					if (specRigidbody && specRigidbody.gameActor && specRigidbody.healthHaver)
					{
						if (specRigidbody.gameActor is AIActor)
						{
							specRigidbody.healthHaver.ApplyDamage(this.DamageToEnemies, Vector2.zero, StringTableManager.GetEnemiesString("#TRAP", -1), CoreDamageTypes.Fire, DamageCategory.Environment, false, null, false);
						}
						else
						{
							specRigidbody.healthHaver.ApplyDamage(0.5f, Vector2.zero, StringTableManager.GetEnemiesString("#TRAP", -1), CoreDamageTypes.Fire, DamageCategory.Environment, false, null, false);
						}
					}
				}
				bulletTimer += this.TimeBetweenBullets;
			}
			yield return null;
		}
		for (int k = 0; k < this.vfxAnimators.Length; k++)
		{
			if (this.vfxAnimators[k].renderer.enabled)
			{
				this.vfxAnimators[k].PlayAndDisableRenderer(this.OutAnimationName);
			}
		}
		yield break;
	}

	// Token: 0x060062B8 RID: 25272 RVA: 0x002645A4 File Offset: 0x002627A4
	private void FireBullet()
	{
		base.bulletBank.CreateProjectileFromBank(this.ShootPoint.position, BraveMathCollege.Atan2Degrees(DungeonData.GetIntVector2FromDirection(this.DirectionToSpew).ToVector2()) + UnityEngine.Random.Range(-this.ConeAngle, this.ConeAngle), "default", null, false, true, false);
	}

	// Token: 0x060062B9 RID: 25273 RVA: 0x00264604 File Offset: 0x00262804
	protected override void OnDestroy()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnBeamCollision = (SpeculativeRigidbody.OnBeamCollisionDelegate)Delegate.Remove(specRigidbody2.OnBeamCollision, new SpeculativeRigidbody.OnBeamCollisionDelegate(this.HandleBeamCollision));
		base.OnDestroy();
	}

	// Token: 0x060062BA RID: 25274 RVA: 0x00264668 File Offset: 0x00262868
	public void ConfigureOnPlacement(RoomHandler room)
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		for (int i = 0; i < 2; i++)
		{
			GameManager.Instance.Dungeon.data[intVector + new IntVector2(0, i)].cellVisualData.containsObjectSpaceStamp = true;
		}
	}

	// Token: 0x04005DC8 RID: 24008
	public float DamageToEnemies = 6f;

	// Token: 0x04005DC9 RID: 24009
	public float TimeToSpew = 10f;

	// Token: 0x04005DCA RID: 24010
	public float ConeAngle = 10f;

	// Token: 0x04005DCB RID: 24011
	public float TimeBetweenBullets = 0.05f;

	// Token: 0x04005DCC RID: 24012
	public DungeonData.Direction DirectionToSpew = DungeonData.Direction.EAST;

	// Token: 0x04005DCD RID: 24013
	public Transform ShootPoint;

	// Token: 0x04005DCE RID: 24014
	public string EndSpriteName;

	// Token: 0x04005DCF RID: 24015
	public string LoopAnimationName;

	// Token: 0x04005DD0 RID: 24016
	public string OutAnimationName;

	// Token: 0x04005DD1 RID: 24017
	public tk2dSpriteAnimator[] vfxAnimators;

	// Token: 0x04005DD2 RID: 24018
	private bool m_hasBurst;
}
