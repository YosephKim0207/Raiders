using System;
using UnityEngine;

// Token: 0x0200115A RID: 4442
public class Flake : BraveBehaviour
{
	// Token: 0x060062A0 RID: 25248 RVA: 0x00263AD8 File Offset: 0x00261CD8
	public void Start()
	{
		this.m_velocity = this.velocity;
		this.m_velocity.x = this.m_velocity.x + UnityEngine.Random.Range(-this.velocityVariance.x, this.velocityVariance.x);
		this.m_velocity.y = this.m_velocity.y + UnityEngine.Random.Range(-this.velocityVariance.y, this.velocityVariance.y);
	}

	// Token: 0x060062A1 RID: 25249 RVA: 0x00263B50 File Offset: 0x00261D50
	public void Update()
	{
		this.m_timer += BraveTime.DeltaTime;
		base.transform.position += this.m_velocity * BraveTime.DeltaTime;
		Color color = base.sprite.color;
		color.a = Mathf.Min(1f, Mathf.Lerp(2f, 0f, this.m_timer / this.lifetime));
		base.sprite.color = color;
		if (this.m_timer > this.lifetime)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060062A2 RID: 25250 RVA: 0x00263BFC File Offset: 0x00261DFC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005DA1 RID: 23969
	public float lifetime;

	// Token: 0x04005DA2 RID: 23970
	public Vector2 velocity;

	// Token: 0x04005DA3 RID: 23971
	public Vector2 velocityVariance;

	// Token: 0x04005DA4 RID: 23972
	private float m_timer;

	// Token: 0x04005DA5 RID: 23973
	private Vector2 m_velocity;
}
