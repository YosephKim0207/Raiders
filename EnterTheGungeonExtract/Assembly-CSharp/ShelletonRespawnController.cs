using System;
using System.Collections;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x020010BB RID: 4283
public class ShelletonRespawnController : BraveBehaviour
{
	// Token: 0x06005E65 RID: 24165 RVA: 0x00243220 File Offset: 0x00241420
	public void Start()
	{
		this.m_cachedStartingHealth = base.healthHaver.GetMaxHealth();
		this.m_radiusSquared = this.radius * this.radius;
		base.healthHaver.minimumHealth = 1f;
		base.healthHaver.OnDamaged += this.OnDamaged;
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventTriggered));
		base.aiActor.CustomPitDeathHandling += this.CustomPitDeathHandling;
		this.m_cachedHeadDefaultSpriteId = this.headSprite.spriteId;
	}

	// Token: 0x06005E66 RID: 24166 RVA: 0x002432C8 File Offset: 0x002414C8
	public void Update()
	{
		if (base.aiActor.IsFalling && base.behaviorSpeculator.enabled)
		{
			base.behaviorSpeculator.InterruptAndDisable();
		}
		if (this.m_shouldShellSuck)
		{
			for (int i = 0; i < StaticReferenceManager.AllDebris.Count; i++)
			{
				this.AdjustDebrisVelocity(StaticReferenceManager.AllDebris[i]);
			}
		}
	}

	// Token: 0x06005E67 RID: 24167 RVA: 0x00243338 File Offset: 0x00241538
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005E68 RID: 24168 RVA: 0x00243340 File Offset: 0x00241540
	private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if (this.m_state == ShelletonRespawnController.State.Normal && resultValue == 1f)
		{
			base.StartCoroutine(this.RegenerationCR());
		}
	}

	// Token: 0x06005E69 RID: 24169 RVA: 0x00243368 File Offset: 0x00241568
	private void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame)
	{
		if (clip.GetFrame(frame).eventInfo == "shell_suck")
		{
			this.m_shouldShellSuck = true;
		}
	}

	// Token: 0x06005E6A RID: 24170 RVA: 0x0024338C File Offset: 0x0024158C
	private void CustomPitDeathHandling(AIActor actor, ref bool suppressDeath)
	{
		if (this.m_state == ShelletonRespawnController.State.SkullRegeneration)
		{
			base.healthHaver.minimumHealth = 0f;
			base.healthHaver.IsVulnerable = true;
			return;
		}
		suppressDeath = true;
		this.Reposition();
		TileSpriteClipper[] componentsInChildren = base.GetComponentsInChildren<TileSpriteClipper>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i]);
		}
		this.headSprite.SetSprite(this.m_cachedHeadDefaultSpriteId);
		base.aiActor.RecoverFromFall();
		base.StartCoroutine(this.RegenerateFromNothingCR());
	}

	// Token: 0x06005E6B RID: 24171 RVA: 0x00243418 File Offset: 0x00241618
	private IEnumerator RegenerationCR()
	{
		this.m_state = ShelletonRespawnController.State.SkullRegeneration;
		base.behaviorSpeculator.InterruptAndDisable();
		base.aiActor.ClearPath();
		base.aiActor.CollisionDamage = 0f;
		base.knockbackDoer.SetImmobile(true, "ShelletonRespawnController");
		base.specRigidbody.PixelColliders[1].Enabled = false;
		base.specRigidbody.PixelColliders[2].Enabled = true;
		base.healthHaver.SetHealthMaximum(this.skullHealth, null, false);
		base.healthHaver.ForceSetCurrentHealth(this.skullHealth);
		base.aiAnimator.PlayUntilCancelled(this.deathAnim, false, null, -1f, false);
		while (base.aiAnimator.IsPlaying(this.deathAnim))
		{
			yield return null;
		}
		base.healthHaver.minimumHealth = 0f;
		base.knockbackDoer.SetImmobile(false, "ShelletonRespawnController");
		base.aiActor.OverridePitfallAnim = "pitfall_head";
		yield return new WaitForSeconds(this.skullTime);
		if (base.aiActor.IsFalling || base.healthHaver.IsDead)
		{
			yield break;
		}
		base.aiAnimator.PlayUntilFinished(this.preRegenAnim, false, null, -1f, false);
		while (base.aiAnimator.IsPlaying(this.preRegenAnim))
		{
			yield return null;
			if (base.aiActor.IsFalling || base.healthHaver.IsDead)
			{
				yield break;
			}
		}
		base.healthHaver.IsVulnerable = false;
		base.healthHaver.SetHealthMaximum(this.m_cachedStartingHealth, null, false);
		base.healthHaver.ForceSetCurrentHealth(this.m_cachedStartingHealth);
		base.healthHaver.minimumHealth = 1f;
		base.knockbackDoer.SetImmobile(true, "ShelletonRespawnController");
		this.m_numRegenerations++;
		if (this.m_numRegenerations >= 2)
		{
			base.healthHaver.PreventCooldownGainFromDamage = true;
		}
		base.aiAnimator.PlayUntilFinished(this.regenAnim, false, null, -1f, false);
		while (base.aiAnimator.IsPlaying(this.regenAnim))
		{
			yield return null;
		}
		this.m_shouldShellSuck = false;
		base.aiActor.CollisionDamage = 0.5f;
		base.knockbackDoer.SetImmobile(false, "ShelletonRespawnController");
		base.specRigidbody.PixelColliders[1].Enabled = true;
		base.specRigidbody.PixelColliders[2].Enabled = false;
		base.aiActor.OverridePitfallAnim = null;
		base.healthHaver.IsVulnerable = true;
		base.behaviorSpeculator.enabled = true;
		this.m_state = ShelletonRespawnController.State.Normal;
		yield break;
	}

	// Token: 0x06005E6C RID: 24172 RVA: 0x00243434 File Offset: 0x00241634
	private IEnumerator RegenerateFromNothingCR()
	{
		this.m_state = ShelletonRespawnController.State.SkullRegeneration;
		base.behaviorSpeculator.InterruptAndDisable();
		base.aiActor.ClearPath();
		base.aiActor.CollisionDamage = 0f;
		base.knockbackDoer.SetImmobile(true, "ShelletonRespawnController");
		base.specRigidbody.PixelColliders[1].Enabled = false;
		base.specRigidbody.PixelColliders[2].Enabled = false;
		base.healthHaver.IsVulnerable = false;
		base.aiAnimator.PlayUntilCancelled(this.regenFromNothingAnim, false, null, -1f, false);
		while (base.aiAnimator.IsPlaying(this.deathAnim))
		{
			yield return null;
		}
		base.healthHaver.minimumHealth = 1f;
		base.aiAnimator.PlayUntilFinished(this.regenAnim, false, null, -1f, false);
		while (base.aiAnimator.IsPlaying(this.regenAnim))
		{
			yield return null;
		}
		this.m_shouldShellSuck = false;
		base.aiActor.CollisionDamage = 0.5f;
		base.knockbackDoer.SetImmobile(false, "ShelletonRespawnController");
		base.specRigidbody.PixelColliders[1].Enabled = true;
		base.specRigidbody.PixelColliders[2].Enabled = false;
		base.aiActor.OverridePitfallAnim = null;
		base.healthHaver.IsVulnerable = true;
		base.behaviorSpeculator.enabled = true;
		this.m_state = ShelletonRespawnController.State.Normal;
		yield break;
	}

	// Token: 0x06005E6D RID: 24173 RVA: 0x00243450 File Offset: 0x00241650
	private bool AdjustDebrisVelocity(DebrisObject debris)
	{
		if (debris.IsPickupObject)
		{
			return false;
		}
		if (debris.GetComponent<BlackHoleDoer>() != null)
		{
			return false;
		}
		if (!debris.name.Contains("shell", true))
		{
			return false;
		}
		Vector2 vector = debris.sprite.WorldCenter - base.specRigidbody.UnitCenter;
		float num = Vector2.SqrMagnitude(vector);
		if (num > this.m_radiusSquared)
		{
			return false;
		}
		float num2 = Mathf.Sqrt(num);
		if (num2 < this.destroyRadius)
		{
			UnityEngine.Object.Destroy(debris.gameObject);
			return true;
		}
		Vector2 frameAccelerationForRigidbody = this.GetFrameAccelerationForRigidbody(debris.sprite.WorldCenter, num2, this.gravityForce);
		float num3 = Mathf.Clamp(BraveTime.DeltaTime, 0f, 0.02f);
		if (debris.HasBeenTriggered)
		{
			debris.ApplyVelocity(frameAccelerationForRigidbody * num3);
		}
		else if (num2 < this.radius / 2f)
		{
			debris.Trigger(frameAccelerationForRigidbody * num3, 0.5f, 1f);
		}
		return true;
	}

	// Token: 0x06005E6E RID: 24174 RVA: 0x00243564 File Offset: 0x00241764
	private Vector2 GetFrameAccelerationForRigidbody(Vector2 unitCenter, float currentDistance, float g)
	{
		float num = Mathf.Clamp01(1f - currentDistance / this.radius);
		float num2 = g * num * num;
		return (base.specRigidbody.UnitCenter - unitCenter).normalized * num2;
	}

	// Token: 0x06005E6F RID: 24175 RVA: 0x002435AC File Offset: 0x002417AC
	private void Reposition()
	{
		Vector2 vector = BraveUtility.ViewportToWorldpoint(new Vector2(0f, 0f), ViewportType.Gameplay);
		Vector2 vector2 = BraveUtility.ViewportToWorldpoint(new Vector2(1f, 1f), ViewportType.Gameplay);
		IntVector2 bottomLeft = vector.ToIntVector2(VectorConversions.Ceil);
		IntVector2 topRight = vector2.ToIntVector2(VectorConversions.Floor) - IntVector2.One;
		PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
		Vector2 playerLowerLeft = bestActivePlayer.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
		Vector2 playerUpperRight = bestActivePlayer.specRigidbody.HitboxPixelCollider.UnitTopRight;
		bool hasOtherPlayer = false;
		Vector2 otherPlayerLowerLeft = Vector2.zero;
		Vector2 otherPlayerUpperRight = Vector2.zero;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(bestActivePlayer);
			if (otherPlayer && otherPlayer.healthHaver && otherPlayer.healthHaver.IsAlive)
			{
				hasOtherPlayer = true;
				otherPlayerLowerLeft = otherPlayer.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
				otherPlayerUpperRight = otherPlayer.specRigidbody.HitboxPixelCollider.UnitTopRight;
			}
		}
		CellValidator cellValidator = delegate(IntVector2 c)
		{
			for (int i = 0; i < this.aiActor.Clearance.x; i++)
			{
				for (int j = 0; j < this.aiActor.Clearance.y; j++)
				{
					if (GameManager.Instance.Dungeon.data.isTopWall(c.x + i, c.y + j))
					{
						return false;
					}
				}
			}
			PixelCollider hitboxPixelCollider = this.aiActor.specRigidbody.HitboxPixelCollider;
			Vector2 vector4 = new Vector2((float)c.x + 0.5f * ((float)this.aiActor.Clearance.x - hitboxPixelCollider.UnitWidth), (float)c.y);
			Vector2 vector5 = vector4 + hitboxPixelCollider.UnitDimensions;
			return BraveMathCollege.AABBDistanceSquared(vector4, vector5, playerLowerLeft, playerUpperRight) >= this.minDistFromPlayer && (!hasOtherPlayer || BraveMathCollege.AABBDistanceSquared(vector4, vector5, otherPlayerLowerLeft, otherPlayerUpperRight) >= this.minDistFromPlayer) && c.x >= bottomLeft.x && c.y >= bottomLeft.y && c.x + this.aiActor.Clearance.x - 1 <= topRight.x && c.y + this.aiActor.Clearance.y - 1 <= topRight.y;
		};
		Vector2 vector3 = base.aiActor.specRigidbody.UnitCenter - base.aiActor.transform.position.XY();
		IntVector2? randomAvailableCell = base.aiActor.ParentRoom.GetRandomAvailableCell(new IntVector2?(base.aiActor.Clearance), new CellTypes?(base.aiActor.PathableTiles), false, cellValidator);
		if (randomAvailableCell != null)
		{
			base.aiActor.transform.position = Pathfinder.GetClearanceOffset(randomAvailableCell.Value, base.aiActor.Clearance) - vector3;
			base.aiActor.specRigidbody.Reinitialize();
		}
		else
		{
			Debug.LogWarning("TELEPORT FAILED!", base.aiActor);
		}
	}

	// Token: 0x04005874 RID: 22644
	public tk2dBaseSprite headSprite;

	// Token: 0x04005875 RID: 22645
	public float skullHealth = 50f;

	// Token: 0x04005876 RID: 22646
	public float skullTime = 8f;

	// Token: 0x04005877 RID: 22647
	public float minDistFromPlayer = 4f;

	// Token: 0x04005878 RID: 22648
	public string deathAnim;

	// Token: 0x04005879 RID: 22649
	public string preRegenAnim;

	// Token: 0x0400587A RID: 22650
	public string regenAnim;

	// Token: 0x0400587B RID: 22651
	public string regenFromNothingAnim;

	// Token: 0x0400587C RID: 22652
	[Header("Shell Sucking")]
	public float radius = 15f;

	// Token: 0x0400587D RID: 22653
	public float gravityForce = 50f;

	// Token: 0x0400587E RID: 22654
	public float destroyRadius = 0.2f;

	// Token: 0x0400587F RID: 22655
	private float m_cachedStartingHealth;

	// Token: 0x04005880 RID: 22656
	private float m_radiusSquared;

	// Token: 0x04005881 RID: 22657
	private bool m_shouldShellSuck;

	// Token: 0x04005882 RID: 22658
	private int m_numRegenerations;

	// Token: 0x04005883 RID: 22659
	private int m_cachedHeadDefaultSpriteId;

	// Token: 0x04005884 RID: 22660
	private ShelletonRespawnController.State m_state;

	// Token: 0x020010BC RID: 4284
	private enum State
	{
		// Token: 0x04005886 RID: 22662
		Normal,
		// Token: 0x04005887 RID: 22663
		SkullRegeneration
	}
}
