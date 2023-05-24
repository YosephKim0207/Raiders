using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012D1 RID: 4817
public class FakeGameActorEffectHandler : BraveBehaviour
{
	// Token: 0x17000FFE RID: 4094
	// (get) Token: 0x06006BD8 RID: 27608 RVA: 0x002A6BB4 File Offset: 0x002A4DB4
	// (set) Token: 0x06006BD9 RID: 27609 RVA: 0x002A6BBC File Offset: 0x002A4DBC
	public bool IsGone { get; set; }

	// Token: 0x06006BDA RID: 27610 RVA: 0x002A6BC8 File Offset: 0x002A4DC8
	public virtual void Awake()
	{
		this.m_overrideColorID = Shader.PropertyToID("_OverrideColor");
		this.RegisterOverrideColor(new Color(1f, 1f, 1f, 0f), "base");
	}

	// Token: 0x06006BDB RID: 27611 RVA: 0x002A6C00 File Offset: 0x002A4E00
	public virtual void Update()
	{
		for (int i = 0; i < this.m_activeEffects.Count; i++)
		{
			GameActorEffect gameActorEffect = this.m_activeEffects[i];
			if (gameActorEffect != null)
			{
				if (this.m_activeEffectData != null && i < this.m_activeEffectData.Count)
				{
					FakeGameActorEffectHandler.RuntimeGameActorEffectData runtimeGameActorEffectData = this.m_activeEffectData[i];
					if (runtimeGameActorEffectData != null)
					{
						if (runtimeGameActorEffectData.instanceOverheadVFX != null)
						{
							if (!this.IsGone)
							{
								Vector2 vector = base.transform.position.XY();
								if (gameActorEffect.PlaysVFXOnActor)
								{
									if (base.specRigidbody && base.specRigidbody.HitboxPixelCollider != null)
									{
										vector = base.specRigidbody.HitboxPixelCollider.UnitBottomCenter.Quantize(0.0625f);
									}
									runtimeGameActorEffectData.instanceOverheadVFX.transform.position = vector;
								}
								else
								{
									if (base.specRigidbody && base.specRigidbody.HitboxPixelCollider != null)
									{
										vector = base.specRigidbody.HitboxPixelCollider.UnitTopCenter.Quantize(0.0625f);
									}
									runtimeGameActorEffectData.instanceOverheadVFX.transform.position = vector;
								}
								runtimeGameActorEffectData.instanceOverheadVFX.renderer.enabled = true;
							}
							else if (runtimeGameActorEffectData.instanceOverheadVFX)
							{
								UnityEngine.Object.Destroy(runtimeGameActorEffectData.instanceOverheadVFX.gameObject);
							}
						}
						runtimeGameActorEffectData.elapsed += BraveTime.DeltaTime;
						if (runtimeGameActorEffectData.elapsed >= gameActorEffect.duration)
						{
							this.RemoveEffect(gameActorEffect);
						}
					}
				}
			}
		}
	}

	// Token: 0x06006BDC RID: 27612 RVA: 0x002A6DBC File Offset: 0x002A4FBC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06006BDD RID: 27613 RVA: 0x002A6DC4 File Offset: 0x002A4FC4
	public void ApplyEffect(GameActorEffect effect)
	{
		FakeGameActorEffectHandler.RuntimeGameActorEffectData runtimeGameActorEffectData = new FakeGameActorEffectHandler.RuntimeGameActorEffectData();
		if (effect.AppliesTint)
		{
			this.RegisterOverrideColor(effect.TintColor, effect.effectIdentifier);
		}
		if (effect.OverheadVFX != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(effect.OverheadVFX);
			tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
			gameObject.transform.parent = base.transform;
			if (base.healthHaver && base.healthHaver.IsBoss)
			{
				gameObject.transform.position = base.specRigidbody.HitboxPixelCollider.UnitTopCenter;
			}
			else
			{
				Bounds bounds = base.sprite.GetBounds();
				Vector3 vector = base.transform.position + new Vector3((bounds.max.x + bounds.min.x) / 2f, bounds.max.y, 0f).Quantize(0.0625f);
				if (effect.PlaysVFXOnActor)
				{
					vector.y = base.transform.position.y + bounds.min.y;
				}
				gameObject.transform.position = base.sprite.WorldCenter.ToVector3ZUp(0f).WithY(vector.y);
			}
			component.HeightOffGround = 0.5f;
			base.sprite.AttachRenderer(component);
			runtimeGameActorEffectData.instanceOverheadVFX = gameObject.GetComponent<tk2dBaseSprite>();
		}
		this.m_activeEffects.Add(effect);
		this.m_activeEffectData.Add(runtimeGameActorEffectData);
	}

	// Token: 0x06006BDE RID: 27614 RVA: 0x002A6F78 File Offset: 0x002A5178
	public void RemoveEffect(GameActorEffect effect)
	{
		for (int i = 0; i < this.m_activeEffects.Count; i++)
		{
			if (this.m_activeEffects[i].effectIdentifier == effect.effectIdentifier)
			{
				this.RemoveEffect(i, false);
				return;
			}
		}
	}

	// Token: 0x06006BDF RID: 27615 RVA: 0x002A6FCC File Offset: 0x002A51CC
	private void RemoveEffect(int index, bool ignoreDeathCheck = false)
	{
		if (!ignoreDeathCheck && base.healthHaver && base.healthHaver.IsDead)
		{
			return;
		}
		GameActorEffect gameActorEffect = this.m_activeEffects[index];
		if (gameActorEffect.AppliesTint)
		{
			this.DeregisterOverrideColor(gameActorEffect.effectIdentifier);
		}
		this.m_activeEffects.RemoveAt(index);
		if (this.m_activeEffectData[index].instanceOverheadVFX)
		{
			UnityEngine.Object.Destroy(this.m_activeEffectData[index].instanceOverheadVFX.gameObject);
		}
		this.m_activeEffectData.RemoveAt(index);
	}

	// Token: 0x06006BE0 RID: 27616 RVA: 0x002A7074 File Offset: 0x002A5274
	public void RemoveAllEffects(bool ignoreDeathCheck = false)
	{
		for (int i = this.m_activeEffects.Count - 1; i >= 0; i--)
		{
			this.RemoveEffect(i, ignoreDeathCheck);
		}
	}

	// Token: 0x17000FFF RID: 4095
	// (get) Token: 0x06006BE1 RID: 27617 RVA: 0x002A70A8 File Offset: 0x002A52A8
	public Color CurrentOverrideColor
	{
		get
		{
			if (this.m_overrideColorStack.Count == 0)
			{
				this.RegisterOverrideColor(new Color(1f, 1f, 1f, 0f), "base");
			}
			return this.m_overrideColorStack[this.m_overrideColorStack.Count - 1];
		}
	}

	// Token: 0x06006BE2 RID: 27618 RVA: 0x002A7104 File Offset: 0x002A5304
	public bool HasSourcedOverrideColor(string source)
	{
		return this.m_overrideColorSources.Contains(source);
	}

	// Token: 0x06006BE3 RID: 27619 RVA: 0x002A7114 File Offset: 0x002A5314
	public void RegisterOverrideColor(Color overrideColor, string source)
	{
		int num = this.m_overrideColorSources.IndexOf(source);
		if (num >= 0)
		{
			this.m_overrideColorStack[num] = overrideColor;
		}
		else
		{
			this.m_overrideColorSources.Add(source);
			this.m_overrideColorStack.Add(overrideColor);
		}
		this.OnOverrideColorsChanged();
	}

	// Token: 0x06006BE4 RID: 27620 RVA: 0x002A7168 File Offset: 0x002A5368
	public void DeregisterOverrideColor(string source)
	{
		int num = this.m_overrideColorSources.IndexOf(source);
		if (num >= 0)
		{
			this.m_overrideColorStack.RemoveAt(num);
			this.m_overrideColorSources.RemoveAt(num);
		}
		this.OnOverrideColorsChanged();
	}

	// Token: 0x06006BE5 RID: 27621 RVA: 0x002A71A8 File Offset: 0x002A53A8
	public void OnOverrideColorsChanged()
	{
		if (this.OverrideColorOverridden)
		{
			return;
		}
		if (base.healthHaver)
		{
			for (int i = 0; i < base.healthHaver.bodySprites.Count; i++)
			{
				if (base.healthHaver.bodySprites[i])
				{
					base.healthHaver.bodySprites[i].usesOverrideMaterial = true;
					base.healthHaver.bodySprites[i].renderer.material.SetColor(this.m_overrideColorID, this.CurrentOverrideColor);
				}
			}
		}
		else if (base.sprite)
		{
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.SetColor(this.m_overrideColorID, this.CurrentOverrideColor);
		}
	}

	// Token: 0x040068CC RID: 26828
	public bool OverrideColorOverridden;

	// Token: 0x040068CD RID: 26829
	private int m_overrideColorID;

	// Token: 0x040068CE RID: 26830
	private List<string> m_overrideColorSources = new List<string>();

	// Token: 0x040068CF RID: 26831
	private List<Color> m_overrideColorStack = new List<Color>();

	// Token: 0x040068D0 RID: 26832
	private List<GameActorEffect> m_activeEffects = new List<GameActorEffect>();

	// Token: 0x040068D1 RID: 26833
	private List<FakeGameActorEffectHandler.RuntimeGameActorEffectData> m_activeEffectData = new List<FakeGameActorEffectHandler.RuntimeGameActorEffectData>();

	// Token: 0x020012D2 RID: 4818
	private class RuntimeGameActorEffectData
	{
		// Token: 0x040068D2 RID: 26834
		public float elapsed;

		// Token: 0x040068D3 RID: 26835
		public tk2dBaseSprite instanceOverheadVFX;
	}
}
