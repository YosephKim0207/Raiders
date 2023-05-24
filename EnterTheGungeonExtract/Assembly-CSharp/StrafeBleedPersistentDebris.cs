using System;
using UnityEngine;

// Token: 0x02000E34 RID: 3636
public class StrafeBleedPersistentDebris : BraveBehaviour
{
	// Token: 0x06004CF2 RID: 19698 RVA: 0x001A5350 File Offset: 0x001A3550
	public void InitializeSelf(StrafeBleedBuff source)
	{
		this.m_initialized = true;
		this.explosionData = source.explosionData;
		Projectile component = source.GetComponent<Projectile>();
		if (component.PossibleSourceGun != null)
		{
			this.m_attachedGun = component.PossibleSourceGun;
			Gun possibleSourceGun = component.PossibleSourceGun;
			possibleSourceGun.OnFinishAttack = (Action<PlayerController, Gun>)Delegate.Combine(possibleSourceGun.OnFinishAttack, new Action<PlayerController, Gun>(this.HandleCeaseAttack));
		}
		else if (component && component.Owner && component.Owner.CurrentGun)
		{
			this.m_attachedGun = component.Owner.CurrentGun;
			Gun currentGun = component.Owner.CurrentGun;
			currentGun.OnFinishAttack = (Action<PlayerController, Gun>)Delegate.Combine(currentGun.OnFinishAttack, new Action<PlayerController, Gun>(this.HandleCeaseAttack));
		}
	}

	// Token: 0x06004CF3 RID: 19699 RVA: 0x001A5430 File Offset: 0x001A3630
	private void HandleCeaseAttack(PlayerController arg1, Gun arg2)
	{
		this.DoEffect();
		this.Disconnect();
	}

	// Token: 0x06004CF4 RID: 19700 RVA: 0x001A5440 File Offset: 0x001A3640
	private void Disconnect()
	{
		this.m_initialized = false;
		if (this.m_attachedGun)
		{
			Gun attachedGun = this.m_attachedGun;
			attachedGun.OnFinishAttack = (Action<PlayerController, Gun>)Delegate.Remove(attachedGun.OnFinishAttack, new Action<PlayerController, Gun>(this.HandleCeaseAttack));
		}
	}

	// Token: 0x06004CF5 RID: 19701 RVA: 0x001A5480 File Offset: 0x001A3680
	private void Update()
	{
		if (this.m_initialized)
		{
			this.m_elapsed += BraveTime.DeltaTime;
			if (this.m_elapsed > this.CascadeTime)
			{
				this.DoEffect();
				this.Disconnect();
			}
		}
	}

	// Token: 0x06004CF6 RID: 19702 RVA: 0x001A54BC File Offset: 0x001A36BC
	private void DoEffect()
	{
		this.explosionData.force = 0f;
		if (base.sprite)
		{
			Exploder.Explode(base.sprite.WorldCenter, this.explosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
		}
		else
		{
			Exploder.Explode(base.transform.position.XY(), this.explosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004CF7 RID: 19703 RVA: 0x001A5548 File Offset: 0x001A3748
	protected override void OnDestroy()
	{
		this.Disconnect();
		base.OnDestroy();
	}

	// Token: 0x04004305 RID: 17157
	public ExplosionData explosionData;

	// Token: 0x04004306 RID: 17158
	public float CascadeTime = 3f;

	// Token: 0x04004307 RID: 17159
	private Gun m_attachedGun;

	// Token: 0x04004308 RID: 17160
	private bool m_initialized;

	// Token: 0x04004309 RID: 17161
	private float m_elapsed;
}
