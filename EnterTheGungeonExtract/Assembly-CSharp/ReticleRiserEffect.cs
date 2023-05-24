using System;
using UnityEngine;

// Token: 0x02001489 RID: 5257
public class ReticleRiserEffect : MonoBehaviour
{
	// Token: 0x06007785 RID: 30597 RVA: 0x002FA2CC File Offset: 0x002F84CC
	private void Start()
	{
		this.m_sprite = base.GetComponent<tk2dSlicedSprite>();
		this.m_sprite.usesOverrideMaterial = true;
		this.m_shader = ShaderCache.Acquire("tk2d/BlendVertexColorUnlitTilted");
		this.m_sprite.renderer.material.shader = this.m_shader;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		UnityEngine.Object.Destroy(gameObject.GetComponent<ReticleRiserEffect>());
		this.m_risers = new tk2dSlicedSprite[this.NumRisers];
		this.m_risers[0] = gameObject.GetComponent<tk2dSlicedSprite>();
		for (int i = 0; i < this.NumRisers - 1; i++)
		{
			this.m_risers[i + 1] = UnityEngine.Object.Instantiate<GameObject>(gameObject).GetComponent<tk2dSlicedSprite>();
		}
		this.OnSpawned();
	}

	// Token: 0x06007786 RID: 30598 RVA: 0x002FA388 File Offset: 0x002F8588
	private void OnSpawned()
	{
		this.m_localElapsed = 0f;
		if (this.m_risers != null)
		{
			for (int i = 0; i < this.m_risers.Length; i++)
			{
				this.m_risers[i].transform.parent = base.transform;
				this.m_risers[i].transform.localPosition = Vector3.zero;
				this.m_risers[i].transform.localRotation = Quaternion.identity;
				this.m_risers[i].dimensions = this.m_sprite.dimensions;
				this.m_risers[i].usesOverrideMaterial = true;
				this.m_risers[i].renderer.material.shader = this.m_shader;
				this.m_risers[i].gameObject.SetLayerRecursively(LayerMask.NameToLayer("FG_Critical"));
			}
		}
	}

	// Token: 0x06007787 RID: 30599 RVA: 0x002FA46C File Offset: 0x002F866C
	private void Update()
	{
		if (!this.m_sprite)
		{
			return;
		}
		this.m_localElapsed += BraveTime.DeltaTime;
		this.m_sprite.ForceRotationRebuild();
		this.m_sprite.UpdateZDepth();
		this.m_sprite.renderer.material.shader = this.m_shader;
		if (this.m_risers != null)
		{
			for (int i = 0; i < this.m_risers.Length; i++)
			{
				float num = Mathf.Max(0f, this.m_localElapsed - this.RiseTime / (float)this.NumRisers * (float)i);
				float num2 = num % this.RiseTime / this.RiseTime;
				this.m_risers[i].color = Color.Lerp(new Color(1f, 1f, 1f, 0.75f), new Color(1f, 1f, 1f, 0f), num2);
				float num3 = Mathf.Lerp(0f, this.RiserHeight, num2);
				this.m_risers[i].transform.localPosition = Vector3.zero;
				this.m_risers[i].transform.position += Vector3.zero.WithY(num3);
				this.m_risers[i].ForceRotationRebuild();
				this.m_risers[i].UpdateZDepth();
				this.m_risers[i].renderer.material.shader = this.m_shader;
			}
		}
	}

	// Token: 0x04007979 RID: 31097
	public int NumRisers = 4;

	// Token: 0x0400797A RID: 31098
	public float RiserHeight = 1f;

	// Token: 0x0400797B RID: 31099
	public float RiseTime = 1.5f;

	// Token: 0x0400797C RID: 31100
	private tk2dSlicedSprite m_sprite;

	// Token: 0x0400797D RID: 31101
	private tk2dSlicedSprite[] m_risers;

	// Token: 0x0400797E RID: 31102
	private Shader m_shader;

	// Token: 0x0400797F RID: 31103
	private float m_localElapsed;
}
