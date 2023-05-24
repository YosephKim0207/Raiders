using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001078 RID: 4216
public class TankTreaderDeathController : BraveBehaviour
{
	// Token: 0x06005CC7 RID: 23751 RVA: 0x00238818 File Offset: 0x00236A18
	public void Start()
	{
		base.healthHaver.ManualDeathHandling = true;
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		base.healthHaver.OverrideKillCamTime = new float?(4.5f);
	}

	// Token: 0x06005CC8 RID: 23752 RVA: 0x00238854 File Offset: 0x00236A54
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005CC9 RID: 23753 RVA: 0x0023885C File Offset: 0x00236A5C
	private void OnBossDeath(Vector2 dir)
	{
		base.behaviorSpeculator.enabled = false;
		base.aiActor.BehaviorOverridesVelocity = true;
		base.aiActor.BehaviorVelocity = Vector2.zero;
		foreach (TankTreaderMiniTurretController tankTreaderMiniTurretController in base.GetComponentsInChildren<TankTreaderMiniTurretController>())
		{
			tankTreaderMiniTurretController.enabled = false;
		}
		base.StartCoroutine(this.OnDeathExplosionsCR());
	}

	// Token: 0x06005CCA RID: 23754 RVA: 0x002388C4 File Offset: 0x00236AC4
	private IEnumerator OnDeathExplosionsCR()
	{
		PixelCollider collider = base.specRigidbody.HitboxPixelCollider;
		for (int i = 0; i < this.explosionCount; i++)
		{
			Vector2 minPos = collider.UnitBottomLeft;
			Vector2 maxPos = collider.UnitTopRight;
			GameObject vfxPrefab = BraveUtility.RandomElement<GameObject>(this.explosionVfx);
			Vector2 pos = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.5f, 0.5f));
			GameObject vfxObj = SpawnManager.SpawnVFX(vfxPrefab, pos, Quaternion.identity);
			tk2dBaseSprite vfxSprite = vfxObj.GetComponent<tk2dBaseSprite>();
			vfxSprite.HeightOffGround = UnityEngine.Random.Range(3f, 4.5f);
			base.sprite.AttachRenderer(vfxSprite);
			base.sprite.UpdateZDepth();
			if (i < this.explosionCount - 1)
			{
				yield return new WaitForSeconds(this.explosionMidDelay);
			}
		}
		for (int j = 0; j < this.bigExplosionCount; j++)
		{
			Vector2 minPos2 = collider.UnitBottomLeft;
			Vector2 maxPos2 = collider.UnitTopRight;
			GameObject vfxPrefab2 = BraveUtility.RandomElement<GameObject>(this.bigExplosionVfx);
			Vector2 pos2 = BraveUtility.RandomVector2(minPos2, maxPos2, new Vector2(1f, 1f));
			GameObject vfxObj2 = SpawnManager.SpawnVFX(vfxPrefab2, pos2, Quaternion.identity);
			tk2dBaseSprite vfxSprite2 = vfxObj2.GetComponent<tk2dBaseSprite>();
			vfxSprite2.HeightOffGround = UnityEngine.Random.Range(3f, 4.5f);
			base.sprite.AttachRenderer(vfxSprite2);
			base.sprite.UpdateZDepth();
			if (j < this.bigExplosionCount - 1)
			{
				yield return new WaitForSeconds(this.bigExplosionMidDelay);
			}
		}
		Vector2 unitBottomLeft = collider.UnitBottomLeft;
		Vector2 unitCenter = collider.UnitCenter;
		Vector2 unitTopRight = collider.UnitTopRight;
		for (int k = 0; k < this.debrisCount; k++)
		{
			Vector2 vector = BraveUtility.RandomVector2(unitBottomLeft, unitTopRight, new Vector2(1f, 1f));
			GameObject gameObject = SpawnManager.SpawnVFX(BraveUtility.RandomElement<GameObject>(this.debrisObjects), vector, Quaternion.identity);
			if (gameObject)
			{
				gameObject.transform.parent = SpawnManager.Instance.VFX;
				DebrisObject orAddComponent = gameObject.GetOrAddComponent<DebrisObject>();
				if (base.aiActor)
				{
					base.aiActor.sprite.AttachRenderer(orAddComponent.sprite);
				}
				orAddComponent.angularVelocity = 0f;
				orAddComponent.angularVelocityVariance = 10f;
				orAddComponent.GravityOverride = 20f;
				orAddComponent.decayOnBounce = 0.5f;
				orAddComponent.bounceCount = 1;
				orAddComponent.canRotate = true;
				float num = (vector - unitCenter).ToAngle() + UnityEngine.Random.Range(-this.debrisAngleVariance, this.debrisAngleVariance);
				Vector2 vector2 = BraveMathCollege.DegreesToVector(num, 1f) * (float)UnityEngine.Random.Range(this.debrisMinForce, this.debrisMaxForce);
				Vector3 vector3 = new Vector3(vector2.x, vector2.y, (float)this.debrisUpForce);
				if (orAddComponent.minorBreakable)
				{
					orAddComponent.minorBreakable.enabled = true;
				}
				orAddComponent.Trigger(vector3, 1f, 1f);
			}
		}
		if (this.debrisLauncher)
		{
			this.debrisLauncher.Launch();
		}
		base.healthHaver.DeathAnimationComplete(null, null);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04005671 RID: 22129
	public List<GameObject> explosionVfx;

	// Token: 0x04005672 RID: 22130
	public float explosionMidDelay = 0.3f;

	// Token: 0x04005673 RID: 22131
	public int explosionCount = 10;

	// Token: 0x04005674 RID: 22132
	[Space(12f)]
	public List<GameObject> bigExplosionVfx;

	// Token: 0x04005675 RID: 22133
	public float bigExplosionMidDelay = 0.3f;

	// Token: 0x04005676 RID: 22134
	public int bigExplosionCount = 10;

	// Token: 0x04005677 RID: 22135
	[Space(12f)]
	public List<GameObject> debrisObjects;

	// Token: 0x04005678 RID: 22136
	public int debrisCount = 10;

	// Token: 0x04005679 RID: 22137
	public int debrisMinForce = 5;

	// Token: 0x0400567A RID: 22138
	public int debrisMaxForce = 5;

	// Token: 0x0400567B RID: 22139
	public int debrisUpForce = 8;

	// Token: 0x0400567C RID: 22140
	public float debrisAngleVariance = 15f;

	// Token: 0x0400567D RID: 22141
	public ExplosionDebrisLauncher debrisLauncher;
}
