using System;
using UnityEngine;

// Token: 0x020014DA RID: 5338
public class ThrownGoopItem : MonoBehaviour
{
	// Token: 0x0600795D RID: 31069 RVA: 0x00308EF4 File Offset: 0x003070F4
	private void Start()
	{
		AkSoundEngine.PostEvent("Play_OBJ_item_throw_01", base.gameObject);
		DebrisObject component = base.GetComponent<DebrisObject>();
		component.killTranslationOnBounce = false;
		if (component)
		{
			DebrisObject debrisObject = component;
			debrisObject.OnBounced = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnBounced, new Action<DebrisObject>(this.OnBounced));
			DebrisObject debrisObject2 = component;
			debrisObject2.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject2.OnGrounded, new Action<DebrisObject>(this.OnHitGround));
		}
	}

	// Token: 0x0600795E RID: 31070 RVA: 0x00308F70 File Offset: 0x00307170
	private void OnBounced(DebrisObject obj)
	{
		if (this.goop != null)
		{
			DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goop).TimedAddGoopCircle(obj.sprite.WorldCenter, this.goopRadius, 0.5f, false);
		}
		if (this.CreatesProjectiles)
		{
			float num = 360f / (float)this.NumProjectiles;
			float num2 = UnityEngine.Random.Range(0f, num);
			Projectile projectile = this.SourceProjectile;
			if (this.UsesSynergyOverrideProjectile && GameManager.Instance.PrimaryPlayer.HasActiveBonusSynergy(this.SynergyToCheck, false))
			{
				projectile = this.SynergyProjectile;
			}
			for (int i = 0; i < this.NumProjectiles; i++)
			{
				float num3 = num2 + num * (float)i;
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, obj.sprite.WorldCenter, Quaternion.Euler(0f, 0f, num3), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				component.Owner = GameManager.Instance.PrimaryPlayer;
				component.Shooter = GameManager.Instance.PrimaryPlayer.specRigidbody;
				component.collidesWithPlayer = false;
				component.collidesWithEnemies = true;
			}
		}
	}

	// Token: 0x0600795F RID: 31071 RVA: 0x0030909C File Offset: 0x0030729C
	private void OnHitGround(DebrisObject obj)
	{
		AkSoundEngine.PostEvent("Play_WPN_molotov_impact_01", base.gameObject);
		this.OnBounced(obj);
		this.burstVFX.SpawnAtPosition(base.GetComponent<tk2dSprite>().WorldCenter, 0f, null, null, null, null, false, null, null, false);
		if (!string.IsNullOrEmpty(this.burstAnim))
		{
			base.GetComponent<tk2dSpriteAnimator>().PlayAndDestroyObject(this.burstAnim, null);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04007BCF RID: 31695
	public GoopDefinition goop;

	// Token: 0x04007BD0 RID: 31696
	public float goopRadius = 3f;

	// Token: 0x04007BD1 RID: 31697
	public bool CreatesProjectiles;

	// Token: 0x04007BD2 RID: 31698
	public int NumProjectiles;

	// Token: 0x04007BD3 RID: 31699
	public Projectile SourceProjectile;

	// Token: 0x04007BD4 RID: 31700
	public bool UsesSynergyOverrideProjectile;

	// Token: 0x04007BD5 RID: 31701
	public CustomSynergyType SynergyToCheck;

	// Token: 0x04007BD6 RID: 31702
	public Projectile SynergyProjectile;

	// Token: 0x04007BD7 RID: 31703
	public string burstAnim;

	// Token: 0x04007BD8 RID: 31704
	public VFXPool burstVFX;
}
