using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000E35 RID: 3637
public class TetrisBuff : AppliedEffectBase
{
	// Token: 0x06004CF9 RID: 19705 RVA: 0x001A556C File Offset: 0x001A376C
	private void InitializeSelf(float length, float maxLength)
	{
		this.hh = base.GetComponent<HealthHaver>();
		this.lifetime = length;
		this.maxLifetime = maxLength;
		if (this.hh != null)
		{
			base.StartCoroutine(this.ApplyModification());
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004CFA RID: 19706 RVA: 0x001A55BC File Offset: 0x001A37BC
	public override void Initialize(AppliedEffectBase source)
	{
		if (source is TetrisBuff)
		{
			TetrisBuff tetrisBuff = source as TetrisBuff;
			this.InitializeSelf(tetrisBuff.lifetime, tetrisBuff.maxLifetime);
			this.type = tetrisBuff.type;
			if (tetrisBuff.vfx != null)
			{
				bool flag = true;
				if (flag)
				{
					this.instantiatedVFX = SpawnManager.SpawnVFX(tetrisBuff.vfx, base.transform.position, Quaternion.identity);
					tk2dSprite component = this.instantiatedVFX.GetComponent<tk2dSprite>();
					tk2dSprite component2 = base.GetComponent<tk2dSprite>();
					if (component != null && component2 != null)
					{
						component2.AttachRenderer(component);
						component.HeightOffGround = 0.1f;
						component.IsPerpendicular = true;
						component.usesOverrideMaterial = true;
					}
					BuffVFXAnimator component3 = this.instantiatedVFX.GetComponent<BuffVFXAnimator>();
					if (component3 != null)
					{
						component3.ClearData();
						component3.Initialize(base.GetComponent<GameActor>());
					}
				}
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06004CFB RID: 19707 RVA: 0x001A56B8 File Offset: 0x001A38B8
	public void ExtendLength(float time)
	{
		this.lifetime = Mathf.Min(this.lifetime + time, this.elapsed + this.maxLifetime);
	}

	// Token: 0x06004CFC RID: 19708 RVA: 0x001A56DC File Offset: 0x001A38DC
	public override void AddSelfToTarget(GameObject target)
	{
		if (target.GetComponent<HealthHaver>() == null)
		{
			return;
		}
		bool flag = this.type == TetrisBuff.TetrisType.LINE;
		TetrisBuff[] components = target.GetComponents<TetrisBuff>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].shouldBurst = components[i].shouldBurst || flag;
			if (components[i].type == this.type)
			{
				if (!true)
				{
					components[i].ExtendLength(this.lifetime);
					return;
				}
			}
		}
		TetrisBuff tetrisBuff = target.AddComponent<TetrisBuff>();
		tetrisBuff.shouldBurst = flag;
		tetrisBuff.tetrisExplosion = this.tetrisExplosion;
		tetrisBuff.Initialize(this);
	}

	// Token: 0x06004CFD RID: 19709 RVA: 0x001A5790 File Offset: 0x001A3990
	private IEnumerator ApplyModification()
	{
		this.elapsed = 0f;
		while (this.elapsed < this.lifetime && !this.shouldBurst && this.hh && !this.hh.IsDead)
		{
			this.elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		if (this.elapsed == 0f)
		{
			yield return null;
		}
		if (this.shouldBurst)
		{
			if (this.type == TetrisBuff.TetrisType.LINE && this.hh)
			{
				AIActor component = this.hh.GetComponent<AIActor>();
				if (component)
				{
					Exploder.Explode(component.CenterPosition.ToVector3ZisY(0f), this.tetrisExplosion, Vector2.zero, null, false, CoreDamageTypes.None, false);
				}
			}
			if (this.hh && !this.hh.IsDead)
			{
				this.hh.ApplyDamage(this.ExplosionDamagePerTetromino, Vector2.zero, base.name, CoreDamageTypes.None, DamageCategory.DamageOverTime, false, null, false);
			}
			this.shouldBurst = false;
		}
		if (this.instantiatedVFX)
		{
			BuffVFXAnimator component2 = this.instantiatedVFX.GetComponent<BuffVFXAnimator>();
			if (component2 != null && component2.persistsOnDeath)
			{
				tk2dSpriteAnimator component3 = component2.GetComponent<tk2dSpriteAnimator>();
				if (component3 != null)
				{
					component3.Stop();
				}
				this.instantiatedVFX.GetComponent<PersistentVFXBehaviour>().BecomeDebris(Vector3.zero, 0.5f, new Type[0]);
			}
			else if (component2)
			{
				component2.ForceDrop();
			}
			else
			{
				UnityEngine.Object.Destroy(this.instantiatedVFX);
			}
		}
		UnityEngine.Object.Destroy(this);
		yield break;
	}

	// Token: 0x0400430A RID: 17162
	public TetrisBuff.TetrisType type;

	// Token: 0x0400430B RID: 17163
	public ExplosionData tetrisExplosion;

	// Token: 0x0400430C RID: 17164
	public float ExplosionDamagePerTetromino = 6f;

	// Token: 0x0400430D RID: 17165
	[Tooltip("How long each application lasts.")]
	public float lifetime;

	// Token: 0x0400430E RID: 17166
	[Tooltip("The maximum length of time this debuff can be extended to by repeat applications.")]
	public float maxLifetime;

	// Token: 0x0400430F RID: 17167
	public GameObject vfx;

	// Token: 0x04004310 RID: 17168
	[NonSerialized]
	public bool shouldBurst;

	// Token: 0x04004311 RID: 17169
	private float elapsed;

	// Token: 0x04004312 RID: 17170
	private GameObject instantiatedVFX;

	// Token: 0x04004313 RID: 17171
	private HealthHaver hh;

	// Token: 0x04004314 RID: 17172
	private bool wasDuplicate;

	// Token: 0x02000E36 RID: 3638
	public enum TetrisType
	{
		// Token: 0x04004316 RID: 17174
		BLOCK,
		// Token: 0x04004317 RID: 17175
		L,
		// Token: 0x04004318 RID: 17176
		L_REVERSED,
		// Token: 0x04004319 RID: 17177
		S,
		// Token: 0x0400431A RID: 17178
		S_REVERSED,
		// Token: 0x0400431B RID: 17179
		T,
		// Token: 0x0400431C RID: 17180
		LINE
	}
}
