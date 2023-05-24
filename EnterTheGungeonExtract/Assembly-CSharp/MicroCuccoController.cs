using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001438 RID: 5176
public class MicroCuccoController : BraveBehaviour
{
	// Token: 0x06007580 RID: 30080 RVA: 0x002ECC48 File Offset: 0x002EAE48
	public void Initialize(PlayerController owner)
	{
		this.m_owner = owner;
		base.StartCoroutine(this.FindTarget());
	}

	// Token: 0x06007581 RID: 30081 RVA: 0x002ECC60 File Offset: 0x002EAE60
	private void Update()
	{
		if (this.m_target)
		{
			Vector2 vector = this.m_target.CenterPosition - base.sprite.WorldCenter;
			if (vector.x > 0f)
			{
				if (!base.spriteAnimator.IsPlaying("attack_right"))
				{
					base.spriteAnimator.Play("attack_right");
				}
			}
			else if (!base.spriteAnimator.IsPlaying("attack_left"))
			{
				base.spriteAnimator.Play("attack_left");
			}
			if (vector.magnitude < 0.5f)
			{
				float num = 1f;
				if (PassiveItem.IsFlagSetAtAll(typeof(BattleStandardItem)) && this.m_owner && this.m_owner.CurrentGun && this.m_owner.CurrentGun.IsLuteCompanionBuff)
				{
					num = BattleStandardItem.BattleStandardCompanionDamageMultiplier;
				}
				this.m_target.healthHaver.ApplyDamage(num * this.Damage, Vector2.zero, "Cucco", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				base.transform.position = base.transform.position + (vector.normalized * this.Speed * BraveTime.DeltaTime).ToVector3ZUp(0f);
			}
		}
	}

	// Token: 0x06007582 RID: 30082 RVA: 0x002ECDDC File Offset: 0x002EAFDC
	private IEnumerator FindTarget()
	{
		while (this.m_owner)
		{
			List<AIActor> activeEnemies = this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				float num = float.MaxValue;
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					AIActor aiactor = activeEnemies[i];
					if (aiactor && aiactor.healthHaver && !aiactor.healthHaver.IsDead)
					{
						if (!aiactor.IsGone && aiactor.specRigidbody)
						{
							float num2 = Vector2.Distance(aiactor.specRigidbody.GetUnitCenter(ColliderType.HitBox), base.sprite.WorldCenter);
							if (num2 < num)
							{
								this.m_target = aiactor;
								num = num2;
							}
						}
					}
				}
				if (this.m_target == null)
				{
					IL_158:
					UnityEngine.Object.Destroy(base.gameObject);
					yield break;
				}
			}
			yield return new WaitForSeconds(1f);
		}
		goto IL_158;
	}

	// Token: 0x0400775E RID: 30558
	public float Speed = 8f;

	// Token: 0x0400775F RID: 30559
	public float Damage = 4f;

	// Token: 0x04007760 RID: 30560
	private PlayerController m_owner;

	// Token: 0x04007761 RID: 30561
	private AIActor m_target;
}
