using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001615 RID: 5653
public class AfterImageTrailController : BraveBehaviour
{
	// Token: 0x060083C1 RID: 33729 RVA: 0x0035FA98 File Offset: 0x0035DC98
	public void Start()
	{
		if (this.OptionalImageShader != null)
		{
			this.OverrideImageShader = this.OptionalImageShader;
		}
		if (base.transform.parent != null && base.transform.parent.GetComponent<Projectile>() != null)
		{
			base.transform.parent.GetComponent<Projectile>().OnDestruction += this.projectile_OnDestruction;
		}
		this.m_lastSpawnPosition = base.transform.position;
	}

	// Token: 0x060083C2 RID: 33730 RVA: 0x0035FB2C File Offset: 0x0035DD2C
	private void projectile_OnDestruction(Projectile source)
	{
		if (this.m_activeShadows.Count > 0)
		{
			GameManager.Instance.StartCoroutine(this.HandleDeathShadowCleanup());
		}
	}

	// Token: 0x060083C3 RID: 33731 RVA: 0x0035FB50 File Offset: 0x0035DD50
	public void LateUpdate()
	{
		if (this.spawnShadows && !this.m_previousFrameSpawnShadows)
		{
			this.m_spawnTimer = this.shadowTimeDelay;
		}
		this.m_previousFrameSpawnShadows = this.spawnShadows;
		LinkedListNode<AfterImageTrailController.Shadow> next;
		for (LinkedListNode<AfterImageTrailController.Shadow> linkedListNode = this.m_activeShadows.First; linkedListNode != null; linkedListNode = next)
		{
			next = linkedListNode.Next;
			linkedListNode.Value.timer -= BraveTime.DeltaTime;
			if (linkedListNode.Value.timer <= 0f)
			{
				this.m_activeShadows.Remove(linkedListNode);
				this.m_inactiveShadows.AddLast(linkedListNode);
				if (linkedListNode.Value.sprite)
				{
					linkedListNode.Value.sprite.renderer.enabled = false;
				}
			}
			else if (linkedListNode.Value.sprite)
			{
				float num = linkedListNode.Value.timer / this.shadowLifetime;
				Material sharedMaterial = linkedListNode.Value.sprite.renderer.sharedMaterial;
				sharedMaterial.SetFloat("_EmissivePower", Mathf.Lerp(this.maxEmission, this.minEmission, num));
				sharedMaterial.SetFloat("_Opacity", num);
			}
		}
		if (this.spawnShadows)
		{
			if (this.m_spawnTimer > 0f)
			{
				this.m_spawnTimer -= BraveTime.DeltaTime;
			}
			if (this.m_spawnTimer <= 0f && Vector2.Distance(this.m_lastSpawnPosition, base.transform.position) > this.minTranslation)
			{
				this.SpawnNewShadow();
				this.m_spawnTimer += this.shadowTimeDelay;
				this.m_lastSpawnPosition = base.transform.position;
			}
		}
	}

	// Token: 0x060083C4 RID: 33732 RVA: 0x0035FD18 File Offset: 0x0035DF18
	private IEnumerator HandleDeathShadowCleanup()
	{
		while (this.m_activeShadows.Count > 0)
		{
			LinkedListNode<AfterImageTrailController.Shadow> next;
			for (LinkedListNode<AfterImageTrailController.Shadow> node = this.m_activeShadows.First; node != null; node = next)
			{
				next = node.Next;
				node.Value.timer -= BraveTime.DeltaTime;
				if (node.Value.timer <= 0f)
				{
					this.m_activeShadows.Remove(node);
					this.m_inactiveShadows.AddLast(node);
					if (node.Value.sprite)
					{
						node.Value.sprite.renderer.enabled = false;
					}
				}
				else if (node.Value.sprite)
				{
					float num = node.Value.timer / this.shadowLifetime;
					Material sharedMaterial = node.Value.sprite.renderer.sharedMaterial;
					sharedMaterial.SetFloat("_EmissivePower", Mathf.Lerp(this.maxEmission, this.minEmission, num));
					sharedMaterial.SetFloat("_Opacity", num);
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060083C5 RID: 33733 RVA: 0x0035FD34 File Offset: 0x0035DF34
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060083C6 RID: 33734 RVA: 0x0035FD3C File Offset: 0x0035DF3C
	private void SpawnNewShadow()
	{
		if (this.m_inactiveShadows.Count == 0)
		{
			this.CreateInactiveShadow();
		}
		LinkedListNode<AfterImageTrailController.Shadow> first = this.m_inactiveShadows.First;
		tk2dSprite sprite = first.Value.sprite;
		this.m_inactiveShadows.RemoveFirst();
		if (!sprite || !sprite.renderer)
		{
			return;
		}
		first.Value.timer = this.shadowLifetime;
		sprite.SetSprite(base.sprite.Collection, base.sprite.spriteId);
		sprite.transform.position = base.sprite.transform.position;
		sprite.transform.rotation = base.sprite.transform.rotation;
		sprite.scale = base.sprite.scale;
		sprite.usesOverrideMaterial = true;
		sprite.IsPerpendicular = true;
		if (sprite.renderer)
		{
			sprite.renderer.enabled = true;
			sprite.renderer.material.shader = this.OverrideImageShader ?? ShaderCache.Acquire("Brave/Internal/HighPriestAfterImage");
			sprite.renderer.sharedMaterial.SetFloat("_EmissivePower", this.minEmission);
			sprite.renderer.sharedMaterial.SetFloat("_Opacity", 1f);
			sprite.renderer.sharedMaterial.SetColor("_DashColor", this.dashColor);
		}
		sprite.HeightOffGround = this.targetHeight;
		sprite.UpdateZDepth();
		this.m_activeShadows.AddLast(first);
	}

	// Token: 0x060083C7 RID: 33735 RVA: 0x0035FED4 File Offset: 0x0035E0D4
	private void CreateInactiveShadow()
	{
		GameObject gameObject = new GameObject("after image");
		if (this.UseTargetLayer)
		{
			gameObject.layer = LayerMask.NameToLayer(this.TargetLayer);
		}
		tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
		gameObject.transform.parent = SpawnManager.Instance.VFX;
		this.m_inactiveShadows.AddLast(new AfterImageTrailController.Shadow
		{
			timer = this.shadowLifetime,
			sprite = tk2dSprite
		});
	}

	// Token: 0x0400870D RID: 34573
	public bool spawnShadows = true;

	// Token: 0x0400870E RID: 34574
	public float shadowTimeDelay = 0.1f;

	// Token: 0x0400870F RID: 34575
	public float shadowLifetime = 0.6f;

	// Token: 0x04008710 RID: 34576
	public float minTranslation = 0.2f;

	// Token: 0x04008711 RID: 34577
	public float maxEmission = 800f;

	// Token: 0x04008712 RID: 34578
	public float minEmission = 100f;

	// Token: 0x04008713 RID: 34579
	public float targetHeight = -2f;

	// Token: 0x04008714 RID: 34580
	public Color dashColor = new Color(1f, 0f, 1f, 1f);

	// Token: 0x04008715 RID: 34581
	public Shader OptionalImageShader;

	// Token: 0x04008716 RID: 34582
	public bool UseTargetLayer;

	// Token: 0x04008717 RID: 34583
	public string TargetLayer;

	// Token: 0x04008718 RID: 34584
	[NonSerialized]
	public Shader OverrideImageShader;

	// Token: 0x04008719 RID: 34585
	private readonly LinkedList<AfterImageTrailController.Shadow> m_activeShadows = new LinkedList<AfterImageTrailController.Shadow>();

	// Token: 0x0400871A RID: 34586
	private readonly LinkedList<AfterImageTrailController.Shadow> m_inactiveShadows = new LinkedList<AfterImageTrailController.Shadow>();

	// Token: 0x0400871B RID: 34587
	private float m_spawnTimer;

	// Token: 0x0400871C RID: 34588
	private Vector2 m_lastSpawnPosition;

	// Token: 0x0400871D RID: 34589
	private bool m_previousFrameSpawnShadows;

	// Token: 0x02001616 RID: 5654
	private class Shadow
	{
		// Token: 0x0400871E RID: 34590
		public float timer;

		// Token: 0x0400871F RID: 34591
		public tk2dSprite sprite;
	}
}
