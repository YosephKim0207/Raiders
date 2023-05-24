using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001427 RID: 5159
public class KageBunshinController : BraveBehaviour
{
	// Token: 0x06007514 RID: 29972 RVA: 0x002E9A70 File Offset: 0x002E7C70
	public void InitializeOwner(PlayerController p)
	{
		this.Owner = p;
		base.sprite = base.GetComponentInChildren<tk2dSprite>();
		base.GetComponentInChildren<Renderer>().material.SetColor("_FlatColor", new Color(0.25f, 0.25f, 0.25f, 1f));
		base.sprite.usesOverrideMaterial = true;
		this.Owner.PostProcessProjectile += this.HandleProjectile;
		this.Owner.PostProcessBeam += this.HandleBeam;
		if (this.Duration > 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject, this.Duration);
		}
	}

	// Token: 0x06007515 RID: 29973 RVA: 0x002E9B1C File Offset: 0x002E7D1C
	private void HandleBeam(BeamController obj)
	{
		if (!obj || !obj.projectile)
		{
			return;
		}
		GameObject gameObject = SpawnManager.SpawnProjectile(obj.projectile.gameObject, base.sprite.WorldCenter, Quaternion.identity, true);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Owner = this.Owner;
		BeamController component2 = gameObject.GetComponent<BeamController>();
		BasicBeamController basicBeamController = component2 as BasicBeamController;
		if (basicBeamController)
		{
			basicBeamController.SkipPostProcessing = true;
		}
		component2.Owner = this.Owner;
		component2.HitsPlayers = false;
		component2.HitsEnemies = true;
		Vector3 vector = BraveMathCollege.DegreesToVector(this.Owner.CurrentGun.CurrentAngle, 1f);
		component2.Direction = vector;
		component2.Origin = base.sprite.WorldCenter;
		GameManager.Instance.Dungeon.StartCoroutine(this.HandleFiringBeam(component2));
	}

	// Token: 0x06007516 RID: 29974 RVA: 0x002E9C10 File Offset: 0x002E7E10
	private IEnumerator HandleFiringBeam(BeamController beam)
	{
		float elapsed = 0f;
		yield return null;
		while (this.Owner && this.Owner.IsFiring && this && base.sprite)
		{
			elapsed += BraveTime.DeltaTime;
			beam.Origin = base.sprite.WorldCenter;
			beam.LateUpdatePosition(base.sprite.WorldCenter);
			if (this.Owner)
			{
				Vector2 vector = this.Owner.unadjustedAimPoint.XY();
				if (!BraveInput.GetInstanceForPlayer(this.Owner.PlayerIDX).IsKeyboardAndMouse(false) && this.Owner.CurrentGun)
				{
					vector = this.Owner.CenterPosition + BraveMathCollege.DegreesToVector(this.Owner.CurrentGun.CurrentAngle, 10f);
				}
				float num = (vector - base.specRigidbody.UnitCenter).ToAngle();
				beam.Direction = BraveMathCollege.DegreesToVector(num, 1f);
			}
			yield return null;
		}
		beam.CeaseAttack();
		beam.DestroyBeam();
		yield break;
	}

	// Token: 0x06007517 RID: 29975 RVA: 0x002E9C34 File Offset: 0x002E7E34
	private void Disconnect()
	{
		if (this.Owner)
		{
			this.Owner.PostProcessProjectile -= this.HandleProjectile;
			this.Owner.PostProcessBeam -= this.HandleBeam;
		}
	}

	// Token: 0x06007518 RID: 29976 RVA: 0x002E9C74 File Offset: 0x002E7E74
	private void HandleProjectile(Projectile sourceProjectile, float arg2)
	{
		Quaternion quaternion = sourceProjectile.transform.rotation;
		if (this.Owner && this.Owner.CurrentGun)
		{
			Vector2 vector = this.Owner.unadjustedAimPoint.XY();
			float num = (vector - this.Owner.CenterPosition).ToAngle();
			float num2 = Mathf.DeltaAngle(quaternion.eulerAngles.z, num);
			if (!BraveInput.GetInstanceForPlayer(this.Owner.PlayerIDX).IsKeyboardAndMouse(false))
			{
				vector = this.Owner.CenterPosition + BraveMathCollege.DegreesToVector(this.Owner.CurrentGun.CurrentAngle, 10f);
			}
			float num3 = (vector - base.specRigidbody.UnitCenter).ToAngle() + num2;
			quaternion = Quaternion.Euler(0f, 0f, num3);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(sourceProjectile.gameObject, base.sprite.WorldCenter, quaternion);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.specRigidbody.RegisterSpecificCollisionException(base.specRigidbody);
		component.SetOwnerSafe(sourceProjectile.Owner, sourceProjectile.Owner.ActorName);
		component.SetNewShooter(sourceProjectile.Shooter);
	}

	// Token: 0x06007519 RID: 29977 RVA: 0x002E9DC4 File Offset: 0x002E7FC4
	private void LateUpdate()
	{
		if (this.Owner)
		{
			if (this.Owner.IsGhost)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				base.sprite.SetSprite(this.Owner.sprite.Collection, this.Owner.sprite.spriteId);
				base.sprite.FlipX = this.Owner.sprite.FlipX;
				base.sprite.transform.localPosition = this.Owner.sprite.transform.localPosition;
				if (this.UsesRotationInsteadOfInversion)
				{
					base.specRigidbody.Velocity = (Quaternion.Euler(0f, 0f, this.RotationAngle) * this.Owner.specRigidbody.Velocity).XY();
				}
				else
				{
					base.specRigidbody.Velocity = this.Owner.specRigidbody.Velocity * -1f;
				}
			}
		}
	}

	// Token: 0x0600751A RID: 29978 RVA: 0x002E9EE0 File Offset: 0x002E80E0
	private void AttractEnemies(RoomHandler room)
	{
		List<AIActor> activeEnemies = room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies != null)
		{
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				if (activeEnemies[i].OverrideTarget == null)
				{
					activeEnemies[i].OverrideTarget = base.specRigidbody;
				}
			}
		}
	}

	// Token: 0x0600751B RID: 29979 RVA: 0x002E9F3C File Offset: 0x002E813C
	protected override void OnDestroy()
	{
		this.Disconnect();
		base.OnDestroy();
	}

	// Token: 0x040076E6 RID: 30438
	public float Duration = -1f;

	// Token: 0x040076E7 RID: 30439
	[NonSerialized]
	public PlayerController Owner;

	// Token: 0x040076E8 RID: 30440
	[NonSerialized]
	public bool UsesRotationInsteadOfInversion;

	// Token: 0x040076E9 RID: 30441
	[NonSerialized]
	public float RotationAngle = 90f;
}
