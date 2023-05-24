using System;
using System.Collections;
using System.Diagnostics;
using Dungeonator;
using UnityEngine;

// Token: 0x02001684 RID: 5764
public class TeleportProjModifier : BraveBehaviour
{
	// Token: 0x140000D1 RID: 209
	// (add) Token: 0x0600866E RID: 34414 RVA: 0x0037A5A0 File Offset: 0x003787A0
	// (remove) Token: 0x0600866F RID: 34415 RVA: 0x0037A5D8 File Offset: 0x003787D8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnTeleport;

	// Token: 0x06008670 RID: 34416 RVA: 0x0037A610 File Offset: 0x00378810
	private bool ShowMMinAngleToTeleport()
	{
		return this.trigger == TeleportProjModifier.TeleportTrigger.AngleToTarget;
	}

	// Token: 0x06008671 RID: 34417 RVA: 0x0037A61C File Offset: 0x0037881C
	private bool ShowDistToTeleport()
	{
		return this.trigger == TeleportProjModifier.TeleportTrigger.DistanceFromTarget;
	}

	// Token: 0x06008672 RID: 34418 RVA: 0x0037A628 File Offset: 0x00378828
	private bool ShowBehindTargetDistance()
	{
		return this.type == TeleportProjModifier.TeleportType.BehindTarget;
	}

	// Token: 0x06008673 RID: 34419 RVA: 0x0037A634 File Offset: 0x00378834
	public void Start()
	{
		if (!base.sprite)
		{
			base.sprite = base.GetComponentInChildren<tk2dSprite>();
		}
		if (base.projectile && base.projectile.Owner is AIActor)
		{
			this.m_targetRigidbody = (base.projectile.Owner as AIActor).TargetRigidbody;
		}
		if (!this.m_targetRigidbody)
		{
			base.enabled = false;
			return;
		}
		this.m_startingPos = base.transform.position;
	}

	// Token: 0x06008674 RID: 34420 RVA: 0x0037A6C8 File Offset: 0x003788C8
	public void Update()
	{
		if (this.m_isTeleporting)
		{
			return;
		}
		if (this.m_cooldown > 0f)
		{
			this.m_cooldown -= BraveTime.DeltaTime;
			return;
		}
		if (this.numTeleports > 0 && this.ShouldTeleport())
		{
			base.StartCoroutine(this.DoTeleport());
		}
	}

	// Token: 0x06008675 RID: 34421 RVA: 0x0037A728 File Offset: 0x00378928
	protected override void OnDestroy()
	{
		base.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x06008676 RID: 34422 RVA: 0x0037A738 File Offset: 0x00378938
	private bool ShouldTeleport()
	{
		Vector2 unitCenter = this.m_targetRigidbody.GetUnitCenter(ColliderType.HitBox);
		if (this.trigger == TeleportProjModifier.TeleportTrigger.AngleToTarget)
		{
			float num = (unitCenter - base.specRigidbody.UnitCenter).ToAngle();
			float num2 = base.specRigidbody.Velocity.ToAngle();
			return BraveMathCollege.AbsAngleBetween(num, num2) > this.minAngleToTeleport;
		}
		return this.trigger == TeleportProjModifier.TeleportTrigger.DistanceFromTarget && Vector2.Distance(unitCenter, base.specRigidbody.UnitCenter) < this.distToTeleport;
	}

	// Token: 0x06008677 RID: 34423 RVA: 0x0037A7C0 File Offset: 0x003789C0
	private Vector2 GetTeleportPosition()
	{
		if (this.type == TeleportProjModifier.TeleportType.BackToSpawn)
		{
			return this.m_startingPos;
		}
		if (this.type == TeleportProjModifier.TeleportType.BehindTarget && this.m_targetRigidbody && this.m_targetRigidbody.gameActor)
		{
			Vector2 unitCenter = this.m_targetRigidbody.GetUnitCenter(ColliderType.HitBox);
			float facingDirection = this.m_targetRigidbody.gameActor.FacingDirection;
			Dungeon dungeon = GameManager.Instance.Dungeon;
			for (int i = 0; i < 18; i++)
			{
				Vector2 vector = unitCenter + BraveMathCollege.DegreesToVector(facingDirection + 180f + (float)(i * 20), this.behindTargetDistance);
				if (!dungeon.CellExists(vector) || !dungeon.data.isWall((int)vector.x, (int)vector.y))
				{
					return vector;
				}
				vector = unitCenter + BraveMathCollege.DegreesToVector(facingDirection + 180f + (float)(i * -20), this.behindTargetDistance);
				if (!dungeon.CellExists(vector) || !dungeon.data.isWall((int)vector.x, (int)vector.y))
				{
					return vector;
				}
			}
		}
		return this.m_startingPos;
	}

	// Token: 0x06008678 RID: 34424 RVA: 0x0037A900 File Offset: 0x00378B00
	private IEnumerator DoTeleport()
	{
		VFXPool vfxpool = this.teleportVfx;
		Vector3 vector = base.specRigidbody.UnitCenter;
		Transform transform = base.transform;
		vfxpool.SpawnAtPosition(vector, 0f, transform, null, null, null, false, null, null, false);
		if (this.teleportPauseTime > 0f)
		{
			this.m_isTeleporting = true;
			base.sprite.renderer.enabled = false;
			base.projectile.enabled = false;
			base.specRigidbody.enabled = false;
			if (base.projectile.braveBulletScript)
			{
				base.projectile.braveBulletScript.enabled = false;
			}
			yield return new WaitForSeconds(this.teleportPauseTime);
			if (!this || !this.m_targetRigidbody)
			{
				yield break;
			}
			this.m_isTeleporting = false;
			base.sprite.renderer.enabled = true;
			base.projectile.enabled = true;
			base.specRigidbody.enabled = true;
			if (base.projectile.braveBulletScript)
			{
				base.projectile.braveBulletScript.enabled = true;
			}
		}
		Vector2 newPosition = this.GetTeleportPosition();
		base.transform.position = newPosition;
		base.specRigidbody.Reinitialize();
		VFXPool vfxpool2 = this.teleportVfx;
		vector = base.specRigidbody.UnitCenter;
		transform = base.transform;
		vfxpool2.SpawnAtPosition(vector, 0f, transform, null, null, null, false, null, null, false);
		Vector2 firingCenter = base.specRigidbody.UnitCenter;
		Vector2 targetCenter = this.m_targetRigidbody.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		PlayerController targetPlayer = this.m_targetRigidbody.gameActor as PlayerController;
		if (this.leadAmount > 0f && targetPlayer)
		{
			Vector2 vector2 = ((!targetPlayer) ? this.m_targetRigidbody.Velocity : targetPlayer.AverageVelocity);
			Vector2 predictedPosition = BraveMathCollege.GetPredictedPosition(targetCenter, vector2, firingCenter, base.projectile.Speed);
			targetCenter = Vector2.Lerp(targetCenter, predictedPosition, this.leadAmount);
		}
		base.projectile.SendInDirection(targetCenter - firingCenter, true, true);
		if (base.projectile.braveBulletScript && base.projectile.braveBulletScript.bullet != null)
		{
			base.projectile.braveBulletScript.bullet.Position = newPosition;
			base.projectile.braveBulletScript.bullet.Direction = (targetCenter - newPosition).ToAngle();
		}
		this.numTeleports--;
		this.m_cooldown = this.teleportCooldown;
		if (this.OnTeleport != null)
		{
			this.OnTeleport();
		}
		yield break;
	}

	// Token: 0x04008B60 RID: 35680
	public TeleportProjModifier.TeleportTrigger trigger = TeleportProjModifier.TeleportTrigger.AngleToTarget;

	// Token: 0x04008B61 RID: 35681
	[ShowInInspectorIf("ShowMMinAngleToTeleport", true)]
	public float minAngleToTeleport = 70f;

	// Token: 0x04008B62 RID: 35682
	[ShowInInspectorIf("ShowDistToTeleport", true)]
	public float distToTeleport = 3f;

	// Token: 0x04008B63 RID: 35683
	public TeleportProjModifier.TeleportType type = TeleportProjModifier.TeleportType.BackToSpawn;

	// Token: 0x04008B64 RID: 35684
	[ShowInInspectorIf("ShowBehindTargetDistance", true)]
	public float behindTargetDistance = 5f;

	// Token: 0x04008B65 RID: 35685
	public int numTeleports;

	// Token: 0x04008B66 RID: 35686
	public float teleportPauseTime;

	// Token: 0x04008B67 RID: 35687
	public float leadAmount;

	// Token: 0x04008B68 RID: 35688
	public float teleportCooldown;

	// Token: 0x04008B69 RID: 35689
	public VFXPool teleportVfx;

	// Token: 0x04008B6B RID: 35691
	private SpeculativeRigidbody m_targetRigidbody;

	// Token: 0x04008B6C RID: 35692
	private Vector3 m_startingPos;

	// Token: 0x04008B6D RID: 35693
	private bool m_isTeleporting;

	// Token: 0x04008B6E RID: 35694
	private float m_cooldown;

	// Token: 0x02001685 RID: 5765
	public enum TeleportTrigger
	{
		// Token: 0x04008B70 RID: 35696
		AngleToTarget = 10,
		// Token: 0x04008B71 RID: 35697
		DistanceFromTarget = 20
	}

	// Token: 0x02001686 RID: 5766
	public enum TeleportType
	{
		// Token: 0x04008B73 RID: 35699
		BackToSpawn = 10,
		// Token: 0x04008B74 RID: 35700
		BehindTarget = 20
	}
}
