using System;
using UnityEngine;

// Token: 0x0200139F RID: 5023
public class BecomeOrbitProjectileModifier : BraveBehaviour
{
	// Token: 0x060071D0 RID: 29136 RVA: 0x002D3B30 File Offset: 0x002D1D30
	public void Start()
	{
		Projectile projectile = base.projectile;
		if (this.TriggerOnBounce)
		{
			BounceProjModifier orAddComponent = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
			orAddComponent.numberOfBounces = Mathf.Max(orAddComponent.numberOfBounces, 1);
			orAddComponent.onlyBounceOffTiles = true;
			BounceProjModifier bounceProjModifier = orAddComponent;
			bounceProjModifier.OnBounceContext = (Action<BounceProjModifier, SpeculativeRigidbody>)Delegate.Combine(bounceProjModifier.OnBounceContext, new Action<BounceProjModifier, SpeculativeRigidbody>(this.HandleStartOrbit));
		}
		else
		{
			this.StartOrbit();
		}
	}

	// Token: 0x060071D1 RID: 29137 RVA: 0x002D3BA4 File Offset: 0x002D1DA4
	private void StartOrbit()
	{
		OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
		orbitProjectileMotionModule.MinRadius = this.MinRadius;
		orbitProjectileMotionModule.MaxRadius = this.MaxRadius;
		orbitProjectileMotionModule.OrbitGroup = this.OrbitGroup;
		base.projectile.OverrideMotionModule = orbitProjectileMotionModule;
	}

	// Token: 0x060071D2 RID: 29138 RVA: 0x002D3BE8 File Offset: 0x002D1DE8
	private void HandleStartOrbit(BounceProjModifier bouncer, SpeculativeRigidbody srb)
	{
		bouncer.projectile.specRigidbody.CollideWithTileMap = false;
		OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
		orbitProjectileMotionModule.MinRadius = this.MinRadius;
		orbitProjectileMotionModule.MaxRadius = this.MaxRadius;
		orbitProjectileMotionModule.OrbitGroup = this.OrbitGroup;
		orbitProjectileMotionModule.HasSpawnVFX = true;
		orbitProjectileMotionModule.SpawnVFX = this.RespawnVFX.effects[0].effects[0].effect;
		orbitProjectileMotionModule.CustomSpawnVFXElapsed = this.SpawnVFXElapsedTimer;
		bouncer.projectile.OverrideMotionModule = orbitProjectileMotionModule;
	}

	// Token: 0x04007340 RID: 29504
	public float MinRadius = 2f;

	// Token: 0x04007341 RID: 29505
	public float MaxRadius = 5f;

	// Token: 0x04007342 RID: 29506
	public int OrbitGroup = -1;

	// Token: 0x04007343 RID: 29507
	public float SpawnVFXElapsedTimer = -1f;

	// Token: 0x04007344 RID: 29508
	public VFXPool RespawnVFX;

	// Token: 0x04007345 RID: 29509
	public bool TriggerOnBounce = true;
}
