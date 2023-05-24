using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200162E RID: 5678
public class ChainBulletsModifier : MonoBehaviour
{
	// Token: 0x0600848E RID: 33934 RVA: 0x0036965C File Offset: 0x0036785C
	private void Start()
	{
		Projectile component = base.GetComponent<Projectile>();
		if (component)
		{
			Projectile projectile = component;
			projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
		}
	}

	// Token: 0x0600848F RID: 33935 RVA: 0x003696A0 File Offset: 0x003678A0
	private void HandleHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
	{
		this.DoChain(arg1, arg2);
	}

	// Token: 0x06008490 RID: 33936 RVA: 0x003696AC File Offset: 0x003678AC
	private void DoChain(Projectile proj, SpeculativeRigidbody enemy)
	{
		if (this.m_numBounces < this.GuaranteedNumberOfChains || UnityEngine.Random.value < this.ChanceToContinueChaining)
		{
			this.m_numBounces++;
			if (this.BounceRandomlyOnNoTarget)
			{
				PierceProjModifier orAddComponent = proj.gameObject.GetOrAddComponent<PierceProjModifier>();
				orAddComponent.penetratesBreakables = true;
				orAddComponent.penetration++;
			}
			Vector2 vector = UnityEngine.Random.insideUnitCircle;
			if (enemy.aiActor)
			{
				AIActor closestToPosition = BraveUtility.GetClosestToPosition<AIActor>(enemy.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All), enemy.UnitCenter, null, this.EnemyDetectRadius, new AIActor[] { enemy.aiActor });
				if (closestToPosition)
				{
					if (!this.BounceRandomlyOnNoTarget)
					{
						PierceProjModifier orAddComponent2 = proj.gameObject.GetOrAddComponent<PierceProjModifier>();
						orAddComponent2.penetratesBreakables = true;
						orAddComponent2.penetration++;
					}
					vector = closestToPosition.CenterPosition - proj.transform.position.XY();
					if (!this.BounceRandomlyOnNoTarget)
					{
						proj.SendInDirection(vector, false, true);
					}
				}
			}
			if (this.BounceRandomlyOnNoTarget)
			{
				proj.SendInDirection(vector, false, true);
			}
		}
	}

	// Token: 0x04008834 RID: 34868
	public int GuaranteedNumberOfChains = 1;

	// Token: 0x04008835 RID: 34869
	public float ChanceToContinueChaining;

	// Token: 0x04008836 RID: 34870
	public bool BounceRandomlyOnNoTarget = true;

	// Token: 0x04008837 RID: 34871
	public float EnemyDetectRadius = -1f;

	// Token: 0x04008838 RID: 34872
	private int m_numBounces;
}
