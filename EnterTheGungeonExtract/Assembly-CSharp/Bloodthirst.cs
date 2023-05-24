using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200125B RID: 4699
public class Bloodthirst : MonoBehaviour
{
	// Token: 0x06006958 RID: 26968 RVA: 0x002939A4 File Offset: 0x00291BA4
	private void Awake()
	{
		this.m_player = base.GetComponent<PlayerController>();
		SpeculativeRigidbody specRigidbody = this.m_player.specRigidbody;
		specRigidbody.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Combine(specRigidbody.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.HandlePostRigidbodyMovement));
		this.m_currentNumKillsRequired = GameManager.Instance.BloodthirstOptions.NumKillsForHealRequiredBase;
		this.m_currentNumKills = 0;
	}

	// Token: 0x06006959 RID: 26969 RVA: 0x00293A08 File Offset: 0x00291C08
	private void HandlePostRigidbodyMovement(SpeculativeRigidbody inSrb, Vector2 inVec2, IntVector2 inPixels)
	{
		if (!this.m_player || this.m_player.IsGhost || this.m_player.IsStealthed || Dungeon.IsGenerating || BraveTime.DeltaTime == 0f)
		{
			return;
		}
		RedMatterParticleController redMatterController = GlobalSparksDoer.GetRedMatterController();
		BloodthirstSettings bloodthirstOptions = GameManager.Instance.BloodthirstOptions;
		float radius = bloodthirstOptions.Radius;
		float damagePerSecond = bloodthirstOptions.DamagePerSecond;
		float percentAffected = bloodthirstOptions.PercentAffected;
		int gainPerHeal = bloodthirstOptions.NumKillsAddedPerHealthGained;
		int maxRequired = bloodthirstOptions.NumKillsRequiredCap;
		if (this.AuraAction == null)
		{
			this.AuraAction = delegate(AIActor actor, float dist)
			{
				if (!actor || !actor.healthHaver)
				{
					return;
				}
				if (!actor.HasBeenBloodthirstProcessed)
				{
					actor.HasBeenBloodthirstProcessed = true;
					actor.CanBeBloodthirsted = UnityEngine.Random.value < percentAffected;
					if (actor.CanBeBloodthirsted && actor.sprite)
					{
						Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(actor.sprite);
						if (outlineMaterial != null)
						{
							outlineMaterial.SetColor("_OverrideColor", new Color(1f, 0f, 0f));
						}
					}
				}
				if (dist < radius && actor.CanBeBloodthirsted && !actor.IsGone)
				{
					float num = damagePerSecond * BraveTime.DeltaTime;
					bool isDead = actor.healthHaver.IsDead;
					actor.healthHaver.ApplyDamage(num, Vector2.zero, "Bloodthirst", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
					if (!isDead && actor.healthHaver.IsDead)
					{
						this.m_currentNumKills++;
						if (this.m_currentNumKills >= this.m_currentNumKillsRequired)
						{
							this.m_currentNumKills = 0;
							if (this.m_player.healthHaver.GetCurrentHealthPercentage() < 1f)
							{
								this.m_player.healthHaver.ApplyHealing(0.5f);
								this.m_currentNumKillsRequired = Mathf.Min(maxRequired, this.m_currentNumKillsRequired + gainPerHeal);
								GameObject gameObject = BraveResources.Load<GameObject>("Global VFX/VFX_Healing_Sparkles_001", ".prefab");
								if (gameObject != null)
								{
									this.m_player.PlayEffectOnActor(gameObject, Vector3.zero, true, false, false);
								}
								AkSoundEngine.PostEvent("Play_OBJ_med_kit_01", this.gameObject);
							}
						}
					}
					GlobalSparksDoer.DoRadialParticleBurst(3, actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft, actor.specRigidbody.HitboxPixelCollider.UnitTopRight, 90f, 4f, 0f, null, null, null, GlobalSparksDoer.SparksType.RED_MATTER);
				}
			};
		}
		if (this.m_player != null && this.m_player.CurrentRoom != null)
		{
			this.m_player.CurrentRoom.ApplyActionToNearbyEnemies(this.m_player.CenterPosition, 100f, this.AuraAction);
		}
		if (redMatterController)
		{
			redMatterController.target.position = this.m_player.CenterPosition;
			redMatterController.ProcessParticles();
		}
	}

	// Token: 0x040065B5 RID: 26037
	private int m_currentNumKillsRequired;

	// Token: 0x040065B6 RID: 26038
	private int m_currentNumKills;

	// Token: 0x040065B7 RID: 26039
	private PlayerController m_player;

	// Token: 0x040065B8 RID: 26040
	private Action<AIActor, float> AuraAction;
}
