using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001673 RID: 5747
public class ShaderProjModifier : BraveBehaviour
{
	// Token: 0x060085FE RID: 34302 RVA: 0x003764C8 File Offset: 0x003746C8
	private float GetStartValue()
	{
		return this.StartValue;
	}

	// Token: 0x060085FF RID: 34303 RVA: 0x003764D0 File Offset: 0x003746D0
	private float GetEndValue(HealthHaver hitEnemy)
	{
		float num = this.EndValue;
		if (this.DoesScaleAmounts && hitEnemy && hitEnemy.specRigidbody && hitEnemy.specRigidbody.HitboxPixelCollider != null)
		{
			num = Mathf.Lerp(this.EndValue, Mathf.Max(this.StartValue, this.EndValue / 10f), hitEnemy.specRigidbody.HitboxPixelCollider.UnitWidth * hitEnemy.specRigidbody.HitboxPixelCollider.UnitHeight / 5f);
		}
		return num;
	}

	// Token: 0x06008600 RID: 34304 RVA: 0x00376568 File Offset: 0x00374768
	private void Start()
	{
		if (this.ShaderProperty == "_EmissivePower")
		{
			this.DoesScaleAmounts = true;
		}
		Projectile projectile = base.projectile;
		projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.projectile_OnHitEnemy));
	}

	// Token: 0x06008601 RID: 34305 RVA: 0x003765B8 File Offset: 0x003747B8
	private void projectile_OnHitEnemy(Projectile proj, SpeculativeRigidbody enemyRigidbody, bool killed)
	{
		if (this.ColorAttribute && enemyRigidbody.gameActor && enemyRigidbody.gameActor.HasSourcedOverrideColor(this.ShaderProperty) && !this.GlobalSparksRepeat)
		{
			return;
		}
		HealthHaver healthHaver = enemyRigidbody.healthHaver;
		if (!healthHaver)
		{
			return;
		}
		if (killed && this.AppliesLocalSlowdown)
		{
			AIActor component = enemyRigidbody.GetComponent<AIActor>();
			if (component && (!component.healthHaver || !component.healthHaver.IsBoss))
			{
				component.LocalTimeScale *= this.LocalTimescaleMultiplier;
				if (component.ParentRoom != null)
				{
					component.ParentRoom.DeregisterEnemy(component, false);
				}
				if (component.aiAnimator)
				{
					component.aiAnimator.FpsScale *= this.LocalTimescaleMultiplier;
				}
				if (component.specRigidbody)
				{
					for (int i = 0; i < component.specRigidbody.PixelColliders.Count; i++)
					{
						component.specRigidbody.PixelColliders[i].Enabled = false;
					}
				}
				if (component.knockbackDoer)
				{
					component.knockbackDoer.timeScalar = 0f;
				}
				if (component.GetComponent<SpawnEnemyOnDeath>())
				{
					component.GetComponent<SpawnEnemyOnDeath>().chanceToSpawn = 0f;
				}
				if (this.AppliesParticleSystem)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ParticleSystemToSpawn, component.CenterPosition.ToVector3ZisY(0f), Quaternion.identity);
					ParticleSystem component2 = gameObject.GetComponent<ParticleSystem>();
					gameObject.transform.parent = component.transform;
					if (component.sprite)
					{
						gameObject.transform.position = component.sprite.WorldCenter;
						Bounds bounds = component.sprite.GetBounds();
						component2.shape.scale = new Vector3(bounds.extents.x * 2f, bounds.extents.y * 2f, 0.125f);
					}
				}
			}
		}
		if (!this.OnDeath || killed)
		{
			if (enemyRigidbody.aiActor && (this.IsGlitter || this.ShouldAffectBosses || !enemyRigidbody.healthHaver.IsBoss))
			{
				if (this.PreventCorpse)
				{
					if (enemyRigidbody.aiActor)
					{
						enemyRigidbody.aiActor.CorpseObject = null;
					}
					FreezeOnDeath component3 = enemyRigidbody.GetComponent<FreezeOnDeath>();
					if (component3)
					{
						component3.HandleDisintegration();
					}
				}
				if (this.DisablesOutlines && enemyRigidbody.sprite)
				{
					SpriteOutlineManager.RemoveOutlineFromSprite(enemyRigidbody.sprite, false);
				}
				if (this.ProcessProperty)
				{
					if (this.LerpTime <= 0f)
					{
						for (int j = 0; j < healthHaver.bodySprites.Count; j++)
						{
							tk2dBaseSprite tk2dBaseSprite = healthHaver.bodySprites[j];
							if (!tk2dBaseSprite)
							{
								return;
							}
							tk2dBaseSprite.usesOverrideMaterial = true;
							if (this.EnableEmission)
							{
								tk2dBaseSprite.renderer.material.EnableKeyword("EMISSIVE_ON");
								tk2dBaseSprite.renderer.material.DisableKeyword("EMISSIVE_OFF");
							}
							if (this.GlobalSparks)
							{
								int num = 100;
								if (this.GlobalSparksRepeat)
								{
									num = 20;
								}
								GlobalSparksDoer.EmitFromRegion(GlobalSparksDoer.EmitRegionStyle.RADIAL, (float)num, this.LerpTime + 0.1f, tk2dBaseSprite.WorldBottomLeft.ToVector3ZisY(0f), tk2dBaseSprite.WorldTopRight.ToVector3ZisY(0f), new Vector3(this.GlobalSparksForce, 0f, 0f), 15f, 0.5f, null, (this.GlobalSparksOverrideLifespan <= 0f) ? null : new float?(this.GlobalSparksOverrideLifespan), new Color?(this.GlobalSparksColor), this.GlobalSparkType);
							}
							tk2dBaseSprite.renderer.material.SetFloat(this.ShaderProperty, this.GetEndValue(healthHaver));
							if (this.AddsEncircler)
							{
								tk2dBaseSprite.gameObject.GetOrAddComponent<Encircler>();
							}
						}
					}
					else
					{
						GameManager.Instance.StartCoroutine(this.ApplyEffect(healthHaver, killed));
					}
				}
				if (this.AddMaterialPass)
				{
					for (int k = 0; k < healthHaver.bodySprites.Count; k++)
					{
						MeshRenderer component4 = healthHaver.bodySprites[k].GetComponent<MeshRenderer>();
						Material[] sharedMaterials = component4.sharedMaterials;
						Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
						Material material = UnityEngine.Object.Instantiate<Material>(this.AddPass);
						material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
						sharedMaterials[sharedMaterials.Length - 1] = material;
						component4.sharedMaterials = sharedMaterials;
					}
				}
			}
			if (this.IsGlitter && enemyRigidbody.aiActor)
			{
				enemyRigidbody.aiActor.HasBeenGlittered = true;
			}
		}
	}

	// Token: 0x06008602 RID: 34306 RVA: 0x00376AFC File Offset: 0x00374CFC
	private IEnumerator ApplyEffect(HealthHaver hh, bool killed)
	{
		float elapsed = 0f;
		bool processedOnce = false;
		while (elapsed < this.LerpTime)
		{
			if (!hh)
			{
				yield break;
			}
			float modifiedDeltaTime = BraveTime.DeltaTime;
			if (this.AppliesLocalSlowdown)
			{
				modifiedDeltaTime *= this.LocalTimescaleMultiplier;
			}
			elapsed += modifiedDeltaTime;
			float t = elapsed / this.LerpTime;
			for (int i = 0; i < hh.bodySprites.Count; i++)
			{
				hh.bodySprites[i].usesOverrideMaterial = true;
				if (this.EnableEmission && !processedOnce)
				{
					hh.bodySprites[i].renderer.material.EnableKeyword("EMISSIVE_ON");
					hh.bodySprites[i].renderer.material.DisableKeyword("EMISSIVE_OFF");
				}
				if (this.GlobalSparks && (this.GlobalSparksRepeat || !processedOnce))
				{
					int num = 100;
					if (this.GlobalSparksRepeat)
					{
						num = 20;
					}
					GlobalSparksDoer.EmitFromRegion(GlobalSparksDoer.EmitRegionStyle.RADIAL, (float)num, this.LerpTime + 0.1f, hh.bodySprites[i].WorldBottomLeft.ToVector3ZisY(0f), hh.bodySprites[i].WorldTopRight.ToVector3ZisY(0f), new Vector3(this.GlobalSparksForce, 0f, 0f), 15f, 0.5f, null, (this.GlobalSparksOverrideLifespan <= 0f) ? null : new float?(this.GlobalSparksOverrideLifespan), new Color?(this.GlobalSparksColor), this.GlobalSparkType);
				}
				if (this.ColorAttribute)
				{
					if (hh.gameActor)
					{
						hh.gameActor.RegisterOverrideColor(Color.Lerp(this.StartColor, this.EndColor, t), this.ShaderProperty);
					}
				}
				else
				{
					hh.bodySprites[i].renderer.material.SetFloat(this.ShaderProperty, Mathf.Lerp(this.GetStartValue(), this.GetEndValue(hh), t));
				}
			}
			processedOnce = true;
			yield return null;
		}
		if (this.AppliesLocalSlowdown && hh && hh.aiActor)
		{
			hh.aiActor.LocalTimeScale /= this.LocalTimescaleMultiplier;
			if (hh.aiAnimator)
			{
				hh.aiAnimator.FpsScale /= this.LocalTimescaleMultiplier;
			}
		}
		yield break;
	}

	// Token: 0x06008603 RID: 34307 RVA: 0x00376B20 File Offset: 0x00374D20
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008AAC RID: 35500
	public bool ProcessProperty = true;

	// Token: 0x04008AAD RID: 35501
	public string ShaderProperty;

	// Token: 0x04008AAE RID: 35502
	[HideInInspectorIf("ColorAttribute", false)]
	public float StartValue;

	// Token: 0x04008AAF RID: 35503
	[HideInInspectorIf("ColorAttribute", false)]
	public float EndValue = 1f;

	// Token: 0x04008AB0 RID: 35504
	public float LerpTime;

	// Token: 0x04008AB1 RID: 35505
	public bool ColorAttribute;

	// Token: 0x04008AB2 RID: 35506
	[ShowInInspectorIf("ColorAttribute", false)]
	public Color StartColor;

	// Token: 0x04008AB3 RID: 35507
	[ShowInInspectorIf("ColorAttribute", false)]
	public Color EndColor;

	// Token: 0x04008AB4 RID: 35508
	public bool OnDeath;

	// Token: 0x04008AB5 RID: 35509
	public bool PreventCorpse;

	// Token: 0x04008AB6 RID: 35510
	public bool DisablesOutlines;

	// Token: 0x04008AB7 RID: 35511
	public bool EnableEmission;

	// Token: 0x04008AB8 RID: 35512
	public bool GlobalSparks;

	// Token: 0x04008AB9 RID: 35513
	public Color GlobalSparksColor;

	// Token: 0x04008ABA RID: 35514
	public float GlobalSparksForce = 3f;

	// Token: 0x04008ABB RID: 35515
	public float GlobalSparksOverrideLifespan = -1f;

	// Token: 0x04008ABC RID: 35516
	public bool AddMaterialPass;

	// Token: 0x04008ABD RID: 35517
	public Material AddPass;

	// Token: 0x04008ABE RID: 35518
	public bool IsGlitter;

	// Token: 0x04008ABF RID: 35519
	public bool ShouldAffectBosses;

	// Token: 0x04008AC0 RID: 35520
	public bool AddsEncircler;

	// Token: 0x04008AC1 RID: 35521
	[Header("Combine Rifle")]
	public bool AppliesLocalSlowdown;

	// Token: 0x04008AC2 RID: 35522
	public float LocalTimescaleMultiplier = 0.5f;

	// Token: 0x04008AC3 RID: 35523
	public bool AppliesParticleSystem;

	// Token: 0x04008AC4 RID: 35524
	public GameObject ParticleSystemToSpawn;

	// Token: 0x04008AC5 RID: 35525
	private bool DoesScaleAmounts;

	// Token: 0x04008AC6 RID: 35526
	public GlobalSparksDoer.SparksType GlobalSparkType;

	// Token: 0x04008AC7 RID: 35527
	public bool GlobalSparksRepeat;
}
