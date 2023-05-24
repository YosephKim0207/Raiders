using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001429 RID: 5161
public class KatanaItem : PlayerItem
{
	// Token: 0x06007523 RID: 29987 RVA: 0x002EA1F0 File Offset: 0x002E83F0
	protected override void DoEffect(PlayerController user)
	{
		AkSoundEngine.PostEvent("Play_CHR_ninja_dash_01", base.gameObject);
		if (this.m_isDashing)
		{
			return;
		}
		this.m_useCount++;
		base.StartCoroutine(this.HandleDash(user));
	}

	// Token: 0x06007524 RID: 29988 RVA: 0x002EA22C File Offset: 0x002E842C
	protected override void AfterCooldownApplied(PlayerController user)
	{
		if (this.m_useCount >= this.sequentialValidUses)
		{
			this.m_useCount = 0;
		}
		else
		{
			base.ClearCooldowns();
		}
	}

	// Token: 0x06007525 RID: 29989 RVA: 0x002EA254 File Offset: 0x002E8454
	private float CalculateAdjustedDashDistance(PlayerController user, Vector2 dashDirection)
	{
		return this.dashDistance;
	}

	// Token: 0x06007526 RID: 29990 RVA: 0x002EA26C File Offset: 0x002E846C
	private IEnumerator HandleDash(PlayerController user)
	{
		this.m_isDashing = true;
		if (this.poofVFX != null)
		{
			user.PlayEffectOnActor(this.poofVFX, Vector3.zero, false, false, false);
		}
		Vector2 startPosition = user.sprite.WorldCenter;
		this.actorsPassed.Clear();
		this.breakablesPassed.Clear();
		user.IsVisible = false;
		user.SetInputOverride("katana");
		user.healthHaver.IsVulnerable = false;
		GameObject trailInstance = UnityEngine.Object.Instantiate<GameObject>(this.trailVFXPrefab, user.sprite.WorldCenter.ToVector3ZUp(0f), Quaternion.identity);
		trailInstance.transform.parent = user.transform;
		TrailController trail = trailInstance.GetComponent<TrailController>();
		trail.boneSpawnOffset = user.sprite.WorldCenter - user.specRigidbody.Position.UnitPosition;
		user.FallingProhibited = true;
		PixelCollider playerHitbox = user.specRigidbody.HitboxPixelCollider;
		playerHitbox.CollisionLayerCollidableOverride |= CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
		SpeculativeRigidbody specRigidbody = user.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.KatanaPreCollision));
		Vector2 dashDirection = BraveInput.GetInstanceForPlayer(user.PlayerIDX).ActiveActions.Move.Vector;
		float adjDashDistance = this.CalculateAdjustedDashDistance(user, dashDirection);
		float duration = Mathf.Max(0.0001f, adjDashDistance / this.dashSpeed);
		float elapsed = -BraveTime.DeltaTime;
		while (elapsed < duration)
		{
			user.healthHaver.IsVulnerable = false;
			elapsed += BraveTime.DeltaTime;
			float adjSpeed = Mathf.Min(this.dashSpeed, adjDashDistance / BraveTime.DeltaTime);
			user.specRigidbody.Velocity = dashDirection.normalized * adjSpeed;
			yield return null;
		}
		user.IsVisible = true;
		user.ToggleGunRenderers(false, "katana");
		base.renderer.enabled = true;
		base.transform.localPosition = new Vector3(-0.125f, 0.125f, 0f);
		base.transform.localRotation = Quaternion.Euler(0f, 0f, 280f);
		if (this.poofVFX != null)
		{
			user.PlayEffectOnActor(this.poofVFX, Vector3.zero, false, false, false);
		}
		base.StartCoroutine(this.EndAndDamage(new List<AIActor>(this.actorsPassed), new List<MajorBreakable>(this.breakablesPassed), user, dashDirection, startPosition, user.sprite.WorldCenter));
		if (this.momentaryPause > 0f)
		{
			yield return new WaitForSeconds(this.finalDelay);
		}
		base.renderer.enabled = false;
		user.ToggleGunRenderers(true, "katana");
		playerHitbox.CollisionLayerCollidableOverride &= ~CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
		SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
		specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.KatanaPreCollision));
		user.FallingProhibited = false;
		user.ClearInputOverride("katana");
		this.m_isDashing = false;
		trail.DisconnectFromSpecRigidbody();
		yield break;
	}

	// Token: 0x06007527 RID: 29991 RVA: 0x002EA290 File Offset: 0x002E8490
	private IEnumerator EndAndDamage(List<AIActor> actors, List<MajorBreakable> breakables, PlayerController user, Vector2 dashDirection, Vector2 startPosition, Vector2 endPosition)
	{
		yield return new WaitForSeconds(this.finalDelay);
		Exploder.DoLinearPush(user.sprite.WorldCenter, startPosition, 13f, 5f);
		user.healthHaver.IsVulnerable = true;
		for (int i = 0; i < actors.Count; i++)
		{
			if (actors[i])
			{
				actors[i].healthHaver.ApplyDamage(this.collisionDamage, dashDirection, "Katana", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			}
		}
		for (int j = 0; j < breakables.Count; j++)
		{
			if (breakables[j])
			{
				breakables[j].ApplyDamage(100f, dashDirection, false, false, false);
			}
		}
		yield break;
	}

	// Token: 0x06007528 RID: 29992 RVA: 0x002EA2D0 File Offset: 0x002E84D0
	private void KatanaPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (otherRigidbody.projectile != null)
		{
			PhysicsEngine.SkipCollision = true;
		}
		if (otherRigidbody.aiActor != null)
		{
			PhysicsEngine.SkipCollision = true;
			if (!this.actorsPassed.Contains(otherRigidbody.aiActor))
			{
				otherRigidbody.aiActor.DelayActions(1f);
				this.actorsPassed.Add(otherRigidbody.aiActor);
			}
		}
		if (otherRigidbody.majorBreakable != null)
		{
			PhysicsEngine.SkipCollision = true;
			if (!this.breakablesPassed.Contains(otherRigidbody.majorBreakable))
			{
				this.breakablesPassed.Add(otherRigidbody.majorBreakable);
			}
		}
	}

	// Token: 0x06007529 RID: 29993 RVA: 0x002EA380 File Offset: 0x002E8580
	public override void OnItemSwitched(PlayerController user)
	{
		base.OnItemSwitched(user);
		if (this.m_useCount > 0)
		{
			this.m_useCount = 0;
			base.ApplyCooldown(user);
		}
	}

	// Token: 0x0600752A RID: 29994 RVA: 0x002EA3A4 File Offset: 0x002E85A4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040076F0 RID: 30448
	public float dashDistance = 10f;

	// Token: 0x040076F1 RID: 30449
	public float dashSpeed = 30f;

	// Token: 0x040076F2 RID: 30450
	public float collisionDamage = 50f;

	// Token: 0x040076F3 RID: 30451
	public float stunDuration = 1f;

	// Token: 0x040076F4 RID: 30452
	public float momentaryPause = 0.25f;

	// Token: 0x040076F5 RID: 30453
	public float finalDelay = 0.5f;

	// Token: 0x040076F6 RID: 30454
	public int sequentialValidUses = 3;

	// Token: 0x040076F7 RID: 30455
	public GameObject trailVFXPrefab;

	// Token: 0x040076F8 RID: 30456
	public GameObject poofVFX;

	// Token: 0x040076F9 RID: 30457
	private bool m_isDashing;

	// Token: 0x040076FA RID: 30458
	private int m_useCount;

	// Token: 0x040076FB RID: 30459
	private List<AIActor> actorsPassed = new List<AIActor>();

	// Token: 0x040076FC RID: 30460
	private List<MajorBreakable> breakablesPassed = new List<MajorBreakable>();
}
