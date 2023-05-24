using System;
using UnityEngine;

// Token: 0x020010F1 RID: 4337
public class ArtfulDodgerBumperController : DungeonPlaceableBehaviour
{
	// Token: 0x06005F98 RID: 24472 RVA: 0x0024CD70 File Offset: 0x0024AF70
	private void Start()
	{
		tk2dSpriteAnimator spriteAnimator = this.mySprite.spriteAnimator;
		spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationCompleted));
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		if (this.diagonalDirection != ArtfulDodgerBumperController.DiagonalDirection.None)
		{
			SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
			specRigidbody2.ReflectProjectilesNormalGenerator = (Func<Vector2, Vector2, Vector2>)Delegate.Combine(specRigidbody2.ReflectProjectilesNormalGenerator, new Func<Vector2, Vector2, Vector2>(this.ReflectNormalGenerator));
		}
	}

	// Token: 0x06005F99 RID: 24473 RVA: 0x0024CE04 File Offset: 0x0024B004
	protected override void OnDestroy()
	{
		if (base.gameObject)
		{
			tk2dSpriteAnimator spriteAnimator = this.mySprite.spriteAnimator;
			spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationCompleted));
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
			if (this.diagonalDirection != ArtfulDodgerBumperController.DiagonalDirection.None)
			{
				SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
				specRigidbody2.ReflectProjectilesNormalGenerator = (Func<Vector2, Vector2, Vector2>)Delegate.Remove(specRigidbody2.ReflectProjectilesNormalGenerator, new Func<Vector2, Vector2, Vector2>(this.ReflectNormalGenerator));
			}
		}
		base.OnDestroy();
	}

	// Token: 0x06005F9A RID: 24474 RVA: 0x0024CEAC File Offset: 0x0024B0AC
	private void AnimationCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		if (this.DestroyBumperOnGameCollision && clip.name == this.breakAnimation && this.m_canDestroy)
		{
			this.BumperPopVFX.SpawnAtPosition(base.gameObject.transform.position, 0f, null, null, null, new float?(1f), false, null, null, false);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (clip.name == this.hitAnimation)
		{
			this.mySprite.spriteAnimator.Play(this.idleAnimation);
		}
	}

	// Token: 0x06005F9B RID: 24475 RVA: 0x0024CF60 File Offset: 0x0024B160
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (rigidbodyCollision.OtherRigidbody.projectile != null)
		{
			Projectile projectile = rigidbodyCollision.OtherRigidbody.projectile;
			this.m_canDestroy = projectile.name.StartsWith("ArtfulDodger");
			this.mySprite.spriteAnimator.Play((!this.m_canDestroy || !this.DestroyBumperOnGameCollision) ? this.hitAnimation : this.breakAnimation);
			if (this.StopsGameProjectileBounces)
			{
				projectile.DieInAir(false, true, true, false);
			}
		}
	}

	// Token: 0x06005F9C RID: 24476 RVA: 0x0024CFF4 File Offset: 0x0024B1F4
	private Vector2 ReflectNormalGenerator(Vector2 contact, Vector2 normal)
	{
		switch (this.diagonalDirection)
		{
		case ArtfulDodgerBumperController.DiagonalDirection.NorthEast:
			if (normal.x > 0.5f || normal.y > 0.5f)
			{
				Vector2 vector = new Vector2(1f, 1f);
				return vector.normalized;
			}
			break;
		case ArtfulDodgerBumperController.DiagonalDirection.SouthEast:
			if (normal.x > 0.5f || normal.y < -0.5f)
			{
				Vector2 vector2 = new Vector2(1f, -1f);
				return vector2.normalized;
			}
			break;
		case ArtfulDodgerBumperController.DiagonalDirection.SouthWest:
			if (normal.x < -0.5f || normal.y < -0.5f)
			{
				Vector2 vector3 = new Vector2(-1f, -1f);
				return vector3.normalized;
			}
			break;
		case ArtfulDodgerBumperController.DiagonalDirection.NorthWest:
			if (normal.x < -0.5f || normal.y > 0.5f)
			{
				Vector2 vector4 = new Vector2(-1f, 1f);
				return vector4.normalized;
			}
			break;
		}
		return normal;
	}

	// Token: 0x04005A16 RID: 23062
	[Header("Bumper Data")]
	public tk2dBaseSprite mySprite;

	// Token: 0x04005A17 RID: 23063
	public bool StopsGameProjectileBounces;

	// Token: 0x04005A18 RID: 23064
	public bool DestroyBumperOnGameCollision;

	// Token: 0x04005A19 RID: 23065
	public ArtfulDodgerBumperController.DiagonalDirection diagonalDirection;

	// Token: 0x04005A1A RID: 23066
	public VFXPool BumperPopVFX;

	// Token: 0x04005A1B RID: 23067
	public string hitAnimation = string.Empty;

	// Token: 0x04005A1C RID: 23068
	[ShowInInspectorIf("DestroyBumperOnGameCollision", false)]
	public string breakAnimation = string.Empty;

	// Token: 0x04005A1D RID: 23069
	public string idleAnimation = string.Empty;

	// Token: 0x04005A1E RID: 23070
	private bool m_canDestroy;

	// Token: 0x020010F2 RID: 4338
	public enum DiagonalDirection
	{
		// Token: 0x04005A20 RID: 23072
		None,
		// Token: 0x04005A21 RID: 23073
		NorthEast,
		// Token: 0x04005A22 RID: 23074
		SouthEast,
		// Token: 0x04005A23 RID: 23075
		SouthWest,
		// Token: 0x04005A24 RID: 23076
		NorthWest
	}
}
