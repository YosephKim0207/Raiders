using System;
using UnityEngine;

// Token: 0x020013BB RID: 5051
public class GunParticleSystemController : MonoBehaviour
{
	// Token: 0x06007280 RID: 29312 RVA: 0x002D871C File Offset: 0x002D691C
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		if (this.TargetSystem)
		{
			this.m_localPositionOffset = this.TargetSystem.transform.localPosition;
		}
	}

	// Token: 0x06007281 RID: 29313 RVA: 0x002D8750 File Offset: 0x002D6950
	private void Start()
	{
		this.m_gun = base.GetComponent<Gun>();
		if (this.DoesParticlesOnFire)
		{
			Gun gun = this.m_gun;
			gun.OnPostFired = (Action<PlayerController, Gun>)Delegate.Combine(gun.OnPostFired, new Action<PlayerController, Gun>(this.HandlePostFired));
		}
		if (this.DoesParticlesOnReload)
		{
			Gun gun2 = this.m_gun;
			gun2.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun2.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReload));
		}
	}

	// Token: 0x06007282 RID: 29314 RVA: 0x002D87D0 File Offset: 0x002D69D0
	private void LateUpdate()
	{
		if (this.TargetSystem)
		{
			if (this.m_gun.GetSprite().FlipY)
			{
				this.TargetSystem.transform.localPosition = this.m_localPositionOffset.WithY(this.m_localPositionOffset.y * -1f);
			}
			else
			{
				this.TargetSystem.transform.localPosition = this.m_localPositionOffset;
			}
		}
	}

	// Token: 0x06007283 RID: 29315 RVA: 0x002D884C File Offset: 0x002D6A4C
	private void HandleReload(PlayerController arg1, Gun arg2, bool arg3)
	{
		if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.MEDIUM)
		{
			this.TargetSystem.Emit(UnityEngine.Random.Range(this.MinParticlesOnReload, this.MaxParticlesOnReload + 1));
		}
	}

	// Token: 0x06007284 RID: 29316 RVA: 0x002D888C File Offset: 0x002D6A8C
	private void HandlePostFired(PlayerController arg1, Gun arg2)
	{
		if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.MEDIUM)
		{
			this.TargetSystem.Emit(UnityEngine.Random.Range(this.MinParticlesOnFire, this.MaxParticlesOnFire + 1));
		}
	}

	// Token: 0x06007285 RID: 29317 RVA: 0x002D88CC File Offset: 0x002D6ACC
	private void OnEnable()
	{
		if (this.DoesParticlesOnFire)
		{
			Gun gun = this.m_gun;
			gun.OnPostFired = (Action<PlayerController, Gun>)Delegate.Combine(gun.OnPostFired, new Action<PlayerController, Gun>(this.HandlePostFired));
		}
		if (this.DoesParticlesOnReload)
		{
			Gun gun2 = this.m_gun;
			gun2.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun2.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReload));
		}
	}

	// Token: 0x06007286 RID: 29318 RVA: 0x002D8940 File Offset: 0x002D6B40
	private void OnDisable()
	{
		Gun gun = this.m_gun;
		gun.OnPostFired = (Action<PlayerController, Gun>)Delegate.Remove(gun.OnPostFired, new Action<PlayerController, Gun>(this.HandlePostFired));
		Gun gun2 = this.m_gun;
		gun2.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Remove(gun2.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReload));
	}

	// Token: 0x06007287 RID: 29319 RVA: 0x002D899C File Offset: 0x002D6B9C
	private void OnDestroy()
	{
		Gun gun = this.m_gun;
		gun.OnPostFired = (Action<PlayerController, Gun>)Delegate.Remove(gun.OnPostFired, new Action<PlayerController, Gun>(this.HandlePostFired));
		Gun gun2 = this.m_gun;
		gun2.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Remove(gun2.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReload));
	}

	// Token: 0x040073D7 RID: 29655
	public ParticleSystem TargetSystem;

	// Token: 0x040073D8 RID: 29656
	public bool DoesParticlesOnFire;

	// Token: 0x040073D9 RID: 29657
	public int MinParticlesOnFire = 10;

	// Token: 0x040073DA RID: 29658
	public int MaxParticlesOnFire = 10;

	// Token: 0x040073DB RID: 29659
	public bool DoesParticlesOnReload;

	// Token: 0x040073DC RID: 29660
	public int MinParticlesOnReload = 20;

	// Token: 0x040073DD RID: 29661
	public int MaxParticlesOnReload = 20;

	// Token: 0x040073DE RID: 29662
	private Gun m_gun;

	// Token: 0x040073DF RID: 29663
	private Vector3 m_localPositionOffset;
}
