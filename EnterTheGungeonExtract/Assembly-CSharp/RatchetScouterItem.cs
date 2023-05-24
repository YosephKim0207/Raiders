using System;
using UnityEngine;

// Token: 0x020013E3 RID: 5091
public class RatchetScouterItem : PassiveItem
{
	// Token: 0x06007389 RID: 29577 RVA: 0x002DF768 File Offset: 0x002DD968
	public override void Pickup(PlayerController player)
	{
		player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.AnyDamageDealt));
		base.Pickup(player);
	}

	// Token: 0x0600738A RID: 29578 RVA: 0x002DF794 File Offset: 0x002DD994
	private void AnyDamageDealt(float damageAmount, bool fatal, HealthHaver target)
	{
		int num = Mathf.RoundToInt(damageAmount);
		Vector3 vector = target.transform.position;
		float num2 = 1f;
		SpeculativeRigidbody component = target.GetComponent<SpeculativeRigidbody>();
		if (component)
		{
			vector = component.UnitCenter.ToVector3ZisY(0f);
			num2 = vector.y - component.UnitBottomCenter.y;
			if (component.healthHaver && !component.healthHaver.HasHealthBar && !component.healthHaver.HasRatchetHealthBar && !component.healthHaver.IsBoss)
			{
				component.healthHaver.HasRatchetHealthBar = true;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.VFXHealthBar);
				SimpleHealthBarController component2 = gameObject.GetComponent<SimpleHealthBarController>();
				component2.Initialize(component, component.healthHaver);
			}
		}
		else
		{
			AIActor component3 = target.GetComponent<AIActor>();
			if (component3)
			{
				vector = component3.CenterPosition.ToVector3ZisY(0f);
				if (component3.sprite)
				{
					num2 = vector.y - component3.sprite.WorldBottomCenter.y;
				}
			}
		}
		num = Mathf.Max(num, 1);
		GameUIRoot.Instance.DoDamageNumber(vector, num2, num);
	}

	// Token: 0x0600738B RID: 29579 RVA: 0x002DF8D4 File Offset: 0x002DDAD4
	public override DebrisObject Drop(PlayerController player)
	{
		if (player)
		{
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.AnyDamageDealt));
		}
		return base.Drop(player);
	}

	// Token: 0x0600738C RID: 29580 RVA: 0x002DF90C File Offset: 0x002DDB0C
	protected override void OnDestroy()
	{
		if (base.Owner)
		{
			PlayerController owner = base.Owner;
			owner.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(owner.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.AnyDamageDealt));
		}
		base.OnDestroy();
	}

	// Token: 0x04007529 RID: 29993
	public GameObject VFXHealthBar;
}
