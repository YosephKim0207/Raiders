using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001292 RID: 4754
public class DumbGunShooter : GameActor
{
	// Token: 0x06006A62 RID: 27234 RVA: 0x0029B84C File Offset: 0x00299A4C
	public override void Start()
	{
		this.inventory = new GunInventory(this);
		this.inventory.maxGuns = 1;
		this.inventory.AddGunToInventory(this.gunToUse, true);
		SpriteOutlineManager.AddOutlineToSprite(this.inventory.CurrentGun.sprite, Color.black, 0.1f, 0.05f, SpriteOutlineManager.OutlineType.NORMAL);
		base.StartCoroutine(this.HandleGunShoot());
	}

	// Token: 0x06006A63 RID: 27235 RVA: 0x0029B8B8 File Offset: 0x00299AB8
	private IEnumerator HandleGunShoot()
	{
		yield return null;
		this.CurrentGun.ammo = 100000;
		this.CurrentGun.ClearReloadData();
		this.CurrentGun.HandleAimRotation(base.transform.position + Vector3.right * 100f, false, 1f);
		this.CurrentGun.Attack(null, null);
		if (this.continueShootTime > 0f)
		{
			float elapsed = 0f;
			while (elapsed < this.continueShootTime)
			{
				elapsed += BraveTime.DeltaTime;
				this.CurrentGun.ContinueAttack(true, null);
				yield return null;
			}
			this.CurrentGun.CeaseAttack(true, null);
		}
		yield return new WaitForSeconds(this.shootPauseTime);
		base.StartCoroutine(this.HandleGunShoot());
		yield break;
	}

	// Token: 0x17000FC0 RID: 4032
	// (get) Token: 0x06006A64 RID: 27236 RVA: 0x0029B8D4 File Offset: 0x00299AD4
	public override Gun CurrentGun
	{
		get
		{
			return this.inventory.CurrentGun;
		}
	}

	// Token: 0x17000FC1 RID: 4033
	// (get) Token: 0x06006A65 RID: 27237 RVA: 0x0029B8E4 File Offset: 0x00299AE4
	public override Transform GunPivot
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x17000FC2 RID: 4034
	// (get) Token: 0x06006A66 RID: 27238 RVA: 0x0029B8EC File Offset: 0x00299AEC
	public override bool SpriteFlipped
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000FC3 RID: 4035
	// (get) Token: 0x06006A67 RID: 27239 RVA: 0x0029B8F0 File Offset: 0x00299AF0
	public override Vector3 SpriteDimensions
	{
		get
		{
			return Vector3.one;
		}
	}

	// Token: 0x06006A68 RID: 27240 RVA: 0x0029B8F8 File Offset: 0x00299AF8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040066E7 RID: 26343
	public Gun gunToUse;

	// Token: 0x040066E8 RID: 26344
	public float continueShootTime;

	// Token: 0x040066E9 RID: 26345
	public float shootPauseTime;

	// Token: 0x040066EA RID: 26346
	public bool overridesInaccuracy = true;

	// Token: 0x040066EB RID: 26347
	public float inaccuracyFraction;

	// Token: 0x040066EC RID: 26348
	private GunInventory inventory;
}
