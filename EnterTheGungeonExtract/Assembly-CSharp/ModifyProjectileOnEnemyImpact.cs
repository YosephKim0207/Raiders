using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200143E RID: 5182
public class ModifyProjectileOnEnemyImpact : PassiveItem
{
	// Token: 0x0600759B RID: 30107 RVA: 0x002ED694 File Offset: 0x002EB894
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.m_player = player;
		base.Pickup(player);
		player.PostProcessProjectile += this.PostProcessProjectile;
	}

	// Token: 0x0600759C RID: 30108 RVA: 0x002ED6C4 File Offset: 0x002EB8C4
	private void PostProcessProjectile(Projectile p, float effectChanceScalar)
	{
		p.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(p.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleProjectileHitEnemy));
	}

	// Token: 0x0600759D RID: 30109 RVA: 0x002ED6E8 File Offset: 0x002EB8E8
	private void HandleProjectileHitEnemy(Projectile obj, SpeculativeRigidbody enemy, bool killed)
	{
		if (this.ApplyRandomBounceOffEnemy)
		{
			PierceProjModifier orAddComponent = obj.gameObject.GetOrAddComponent<PierceProjModifier>();
			orAddComponent.penetratesBreakables = true;
			orAddComponent.penetration++;
			HomingModifier component = obj.gameObject.GetComponent<HomingModifier>();
			if (component)
			{
				component.AngularVelocity *= 0.75f;
			}
			Vector2 vector = UnityEngine.Random.insideUnitCircle;
			float num = this.ChanceToSeekEnemyOnBounce;
			Gun possibleSourceGun = obj.PossibleSourceGun;
			if (this.NormalizeAcrossFireRate && possibleSourceGun)
			{
				float num2 = 1f / possibleSourceGun.DefaultModule.cooldownTime;
				if (possibleSourceGun.Volley != null && possibleSourceGun.Volley.UsesShotgunStyleVelocityRandomizer)
				{
					num2 *= (float)Mathf.Max(1, possibleSourceGun.Volley.projectiles.Count);
				}
				num = Mathf.Clamp01(this.ActivationsPerSecond / num2);
				num = Mathf.Max(this.MinActivationChance, num);
			}
			if (UnityEngine.Random.value < num && enemy.aiActor)
			{
				Func<AIActor, bool> func = (AIActor a) => a && a.HasBeenEngaged && a.healthHaver && a.healthHaver.IsVulnerable;
				AIActor closestToPosition = BraveUtility.GetClosestToPosition<AIActor>(enemy.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All), enemy.UnitCenter, func, new AIActor[] { enemy.aiActor });
				if (closestToPosition)
				{
					vector = closestToPosition.CenterPosition - obj.transform.position.XY();
				}
			}
			obj.SendInDirection(vector, false, true);
		}
	}

	// Token: 0x0600759E RID: 30110 RVA: 0x002ED884 File Offset: 0x002EBA84
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		this.m_player = null;
		debrisObject.GetComponent<ModifyProjectileOnEnemyImpact>().m_pickedUpThisRun = true;
		player.PostProcessProjectile -= this.PostProcessProjectile;
		return debrisObject;
	}

	// Token: 0x0600759F RID: 30111 RVA: 0x002ED8C0 File Offset: 0x002EBAC0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_player)
		{
			this.m_player.PostProcessProjectile -= this.PostProcessProjectile;
		}
	}

	// Token: 0x04007766 RID: 30566
	public bool ApplyRandomBounceOffEnemy = true;

	// Token: 0x04007767 RID: 30567
	[ShowInInspectorIf("ApplyRandomBounceOffEnemy", false)]
	public float ChanceToSeekEnemyOnBounce = 0.5f;

	// Token: 0x04007768 RID: 30568
	public bool NormalizeAcrossFireRate;

	// Token: 0x04007769 RID: 30569
	[ShowInInspectorIf("NormalizeAcrossFireRate", false)]
	public float ActivationsPerSecond = 1f;

	// Token: 0x0400776A RID: 30570
	[ShowInInspectorIf("NormalizeAcrossFireRate", false)]
	public float MinActivationChance = 0.05f;

	// Token: 0x0400776B RID: 30571
	private PlayerController m_player;
}
