using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001368 RID: 4968
public class CardboardBoxItem : PlayerItem
{
	// Token: 0x0600708D RID: 28813 RVA: 0x002CA6EC File Offset: 0x002C88EC
	protected override void OnPreDrop(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			this.BreakStealth(user);
		}
		base.OnPreDrop(user);
	}

	// Token: 0x0600708E RID: 28814 RVA: 0x002CA708 File Offset: 0x002C8908
	protected override void DoEffect(PlayerController user)
	{
		this.m_player = user;
		if (this.m_player && this.m_player.CurrentGun)
		{
			this.m_player.CurrentGun.CeaseAttack(false, null);
		}
		base.IsCurrentlyActive = true;
		bool flag = this.CanAnyBossSee(user);
		this.m_player.OnDidUnstealthyAction += this.BreakStealth;
		this.m_player.PostProcessProjectile += this.SneakAttackProcessor;
		this.m_player.healthHaver.OnDamaged += this.OnDamaged;
		if (!flag)
		{
			user.SetIsStealthed(true, "box");
			user.SetCapableOfStealing(true, "CardboardBoxItem", null);
		}
		this.instanceBox = user.RegisterAttachedObject(this.prefabToAttachToPlayer, string.Empty, 0f);
		this.instanceBoxSprite = this.instanceBox.GetComponent<tk2dSprite>();
		this.instanceBoxSprite.PlaceAtLocalPositionByAnchor(Vector3.zero, tk2dBaseSprite.Anchor.LowerLeft);
		this.instanceBoxSprite.spriteAnimator.Play("cardboard_on");
		user.StartCoroutine(this.HandlePutOn(user, this.instanceBoxSprite));
	}

	// Token: 0x0600708F RID: 28815 RVA: 0x002CA83C File Offset: 0x002C8A3C
	private void SneakAttackProcessor(Projectile arg1, float arg2)
	{
		if (this.m_player && this.m_player.IsStealthed)
		{
			arg1.baseData.damage *= this.SneakAttackDamageMultiplier;
		}
	}

	// Token: 0x06007090 RID: 28816 RVA: 0x002CA878 File Offset: 0x002C8A78
	private bool CanAnyBossSee(PlayerController user)
	{
		Vector2 centerPosition = user.CenterPosition;
		for (int i = 0; i < StaticReferenceManager.AllNpcs.Count; i++)
		{
			if (StaticReferenceManager.AllNpcs[i].ParentRoom == user.CurrentRoom)
			{
				Vector2 unitCenter = StaticReferenceManager.AllNpcs[i].specRigidbody.UnitCenter;
				int standardEnemyVisibilityMask = CollisionMask.StandardEnemyVisibilityMask;
				RaycastResult raycastResult;
				if (!PhysicsEngine.Instance.Raycast(unitCenter, centerPosition - unitCenter, (centerPosition - unitCenter).magnitude, out raycastResult, true, true, standardEnemyVisibilityMask, null, false, null, StaticReferenceManager.AllNpcs[i].specRigidbody))
				{
					RaycastResult.Pool.Free(ref raycastResult);
				}
				else
				{
					if (!(raycastResult.SpeculativeRigidbody == null) && !(raycastResult.SpeculativeRigidbody.gameObject != user.gameObject))
					{
						RaycastResult.Pool.Free(ref raycastResult);
						return true;
					}
					RaycastResult.Pool.Free(ref raycastResult);
				}
			}
		}
		if (user.CurrentRoom != null)
		{
			List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				for (int j = 0; j < activeEnemies.Count; j++)
				{
					if (activeEnemies[j] && activeEnemies[j].specRigidbody && activeEnemies[j].healthHaver && activeEnemies[j].healthHaver.IsBoss)
					{
						Vector2 unitCenter2 = activeEnemies[j].specRigidbody.UnitCenter;
						int standardEnemyVisibilityMask2 = CollisionMask.StandardEnemyVisibilityMask;
						RaycastResult raycastResult2;
						if (!PhysicsEngine.Instance.Raycast(unitCenter2, centerPosition - unitCenter2, (centerPosition - unitCenter2).magnitude, out raycastResult2, true, true, standardEnemyVisibilityMask2, null, false, null, activeEnemies[j].specRigidbody))
						{
							RaycastResult.Pool.Free(ref raycastResult2);
						}
						else
						{
							if (!(raycastResult2.SpeculativeRigidbody == null) && !(raycastResult2.SpeculativeRigidbody.gameObject != user.gameObject))
							{
								RaycastResult.Pool.Free(ref raycastResult2);
								return true;
							}
							RaycastResult.Pool.Free(ref raycastResult2);
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06007091 RID: 28817 RVA: 0x002CAAE0 File Offset: 0x002C8CE0
	private IEnumerator HandlePutOn(PlayerController user, tk2dBaseSprite instanceBoxSprite)
	{
		yield return new WaitForSeconds(0.2f);
		if (base.IsCurrentlyActive)
		{
			user.IsVisible = false;
			instanceBoxSprite.renderer.enabled = true;
		}
		yield break;
	}

	// Token: 0x06007092 RID: 28818 RVA: 0x002CAB0C File Offset: 0x002C8D0C
	private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		this.BreakStealth(this.m_player);
	}

	// Token: 0x06007093 RID: 28819 RVA: 0x002CAB1C File Offset: 0x002C8D1C
	private void BreakStealth(PlayerController obj)
	{
		this.m_player.OnDidUnstealthyAction -= this.BreakStealth;
		this.m_player.healthHaver.OnDamaged -= this.OnDamaged;
		this.m_player.PostProcessProjectile -= this.SneakAttackProcessor;
		base.IsCurrentlyActive = false;
		obj.IsVisible = true;
		obj.SetIsStealthed(false, "box");
		obj.SetCapableOfStealing(false, "CardboardBoxItem", null);
		obj.DeregisterAttachedObject(this.instanceBox, false);
		this.instanceBoxSprite.spriteAnimator.PlayAndDestroyObject("cardboard_off", null);
		this.instanceBoxSprite = null;
	}

	// Token: 0x06007094 RID: 28820 RVA: 0x002CABCC File Offset: 0x002C8DCC
	protected override void DoActiveEffect(PlayerController user)
	{
		this.BreakStealth(user);
	}

	// Token: 0x06007095 RID: 28821 RVA: 0x002CABD8 File Offset: 0x002C8DD8
	public void LateUpdate()
	{
		if (base.IsCurrentlyActive)
		{
			if (this.instanceBoxSprite.FlipX != this.m_player.sprite.FlipX)
			{
				this.instanceBoxSprite.FlipX = this.m_player.sprite.FlipX;
			}
			this.instanceBoxSprite.PlaceAtPositionByAnchor(this.m_player.SpriteBottomCenter + (float)((!this.m_player.sprite.FlipX) ? 1 : (-1)) * new Vector3(-0.5f, 0f, 0f), tk2dBaseSprite.Anchor.LowerCenter);
			if (!this.instanceBoxSprite.spriteAnimator.IsPlaying("cardboard_on"))
			{
				if (this.m_player.specRigidbody.Velocity == Vector2.zero)
				{
					if (this.m_player.spriteAnimator.CurrentClip.name.Contains("backward") || this.m_player.spriteAnimator.CurrentClip.name.Contains("_bw"))
					{
						this.instanceBoxSprite.spriteAnimator.Play("idle_back");
					}
					else
					{
						this.instanceBoxSprite.spriteAnimator.Play("idle");
					}
				}
				else if (this.m_player.spriteAnimator.CurrentClip.name.Contains("run_up") || this.m_player.spriteAnimator.CurrentClip.name.Contains("_bw"))
				{
					this.instanceBoxSprite.spriteAnimator.Play("move_right_backwards");
				}
				else
				{
					this.instanceBoxSprite.spriteAnimator.Play("move_right_forward");
				}
			}
		}
	}

	// Token: 0x06007096 RID: 28822 RVA: 0x002CADAC File Offset: 0x002C8FAC
	public override void OnItemSwitched(PlayerController user)
	{
		if (base.IsCurrentlyActive)
		{
			this.DoActiveEffect(user);
		}
	}

	// Token: 0x06007097 RID: 28823 RVA: 0x002CADC0 File Offset: 0x002C8FC0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400700E RID: 28686
	public GameObject prefabToAttachToPlayer;

	// Token: 0x0400700F RID: 28687
	public float SneakAttackDamageMultiplier = 2f;

	// Token: 0x04007010 RID: 28688
	private GameObject instanceBox;

	// Token: 0x04007011 RID: 28689
	private tk2dSprite instanceBoxSprite;

	// Token: 0x04007012 RID: 28690
	private PlayerController m_player;
}
