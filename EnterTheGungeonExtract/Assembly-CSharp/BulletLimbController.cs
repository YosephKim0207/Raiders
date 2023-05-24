using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001081 RID: 4225
public class BulletLimbController : BraveBehaviour
{
	// Token: 0x17000DAC RID: 3500
	// (get) Token: 0x06005CFE RID: 23806 RVA: 0x00239B88 File Offset: 0x00237D88
	// (set) Token: 0x06005CFF RID: 23807 RVA: 0x00239B90 File Offset: 0x00237D90
	public bool HideBullets { get; set; }

	// Token: 0x06005D00 RID: 23808 RVA: 0x00239B9C File Offset: 0x00237D9C
	public void Start()
	{
		this.m_body = base.transform.parent.GetComponent<AIActor>();
		if (this.m_body == null)
		{
			this.m_body = base.aiAnimator.SpecifyAiActor;
		}
		Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(componentsInChildren[i] == base.transform))
			{
				if (string.IsNullOrEmpty(this.LimitPrefix) || componentsInChildren[i].name.StartsWith(this.LimitPrefix))
				{
					this.m_transforms.Add(componentsInChildren[i]);
				}
			}
		}
		for (int j = 0; j < this.m_transforms.Count; j++)
		{
			this.m_projectiles.Add(this.CreateProjectile(this.m_transforms[j]));
		}
		this.m_body.bulletBank.transforms.AddRange(this.m_transforms);
		this.m_body.specRigidbody.CanCarry = true;
		SpeculativeRigidbody specRigidbody = this.m_body.specRigidbody;
		specRigidbody.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Combine(specRigidbody.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.PostBodyMovement));
	}

	// Token: 0x06005D01 RID: 23809 RVA: 0x00239CE4 File Offset: 0x00237EE4
	public void Update()
	{
		base.renderer.enabled = false;
	}

	// Token: 0x06005D02 RID: 23810 RVA: 0x00239CF4 File Offset: 0x00237EF4
	public void LateUpdate()
	{
		if (BraveTime.DeltaTime == 0f)
		{
			this.PostBodyMovement(base.specRigidbody, Vector2.zero, IntVector2.Zero);
		}
	}

	// Token: 0x06005D03 RID: 23811 RVA: 0x00239D1C File Offset: 0x00237F1C
	public void PostBodyMovement(SpeculativeRigidbody specRigidbody, Vector2 unitDelta, IntVector2 pixelDelta)
	{
		if (!this.m_body)
		{
			return;
		}
		for (int i = 0; i < this.m_transforms.Count; i++)
		{
			GameObject gameObject = this.m_transforms[i].gameObject;
			Projectile projectile = this.m_projectiles[i];
			if (projectile && this.m_body && this.m_body.IsBlackPhantom != projectile.IsBlackBullet)
			{
				if (this.m_body.IsBlackPhantom)
				{
					projectile.ForceBlackBullet = true;
					projectile.BecomeBlackBullet();
				}
				else
				{
					projectile.ForceBlackBullet = false;
					projectile.ReturnFromBlackBullet();
				}
			}
			if (gameObject.activeSelf && !this.HideBullets)
			{
				if (!projectile)
				{
					this.m_projectiles[i] = this.CreateProjectile(gameObject.transform);
				}
				else
				{
					if (!projectile.gameObject.activeSelf)
					{
						projectile.gameObject.SetActive(true);
						projectile.specRigidbody.enabled = true;
						projectile.transform.position = gameObject.transform.position;
						projectile.specRigidbody.Reinitialize();
					}
					if (BraveTime.DeltaTime > 0f)
					{
						if (this.WarpBullets)
						{
							projectile.specRigidbody.Velocity = Vector2.zero;
							projectile.specRigidbody.transform.position = gameObject.transform.position;
							projectile.specRigidbody.Reinitialize();
							projectile.sprite.UpdateZDepth();
						}
						else
						{
							projectile.specRigidbody.Velocity = (gameObject.transform.position.XY() - projectile.specRigidbody.Position.UnitPosition) / BraveTime.DeltaTime;
						}
					}
					else
					{
						projectile.specRigidbody.Velocity = Vector2.zero;
						projectile.transform.position = gameObject.transform.position;
						projectile.transform.position = projectile.transform.position.WithZ(projectile.transform.position.y);
						projectile.specRigidbody.sprite.UpdateZDepth();
					}
					if (this.RotateToMatchTransforms)
					{
						projectile.transform.rotation = gameObject.transform.rotation;
					}
				}
			}
			else if (projectile && projectile.gameObject.activeSelf)
			{
				projectile.gameObject.SetActive(false);
				projectile.specRigidbody.enabled = false;
				projectile.specRigidbody.Velocity = Vector2.zero;
			}
		}
	}

	// Token: 0x06005D04 RID: 23812 RVA: 0x00239FCC File Offset: 0x002381CC
	protected override void OnDestroy()
	{
		this.DestroyProjectiles();
		base.OnDestroy();
	}

	// Token: 0x06005D05 RID: 23813 RVA: 0x00239FDC File Offset: 0x002381DC
	public void DestroyProjectiles()
	{
		for (int i = 0; i < this.m_projectiles.Count; i++)
		{
			Projectile projectile = this.m_projectiles[i];
			if (projectile)
			{
				if (projectile.gameObject.activeSelf)
				{
					projectile.gameObject.SetActive(false);
					projectile.specRigidbody.enabled = false;
					projectile.specRigidbody.Velocity = Vector2.zero;
				}
				projectile.DieInAir(!projectile.gameObject.activeSelf, true, true, false);
			}
		}
	}

	// Token: 0x17000DAD RID: 3501
	// (set) Token: 0x06005D06 RID: 23814 RVA: 0x0023A074 File Offset: 0x00238274
	public bool DoingTell
	{
		set
		{
			this.m_doingTell = value;
			for (int i = 0; i < this.m_projectiles.Count; i++)
			{
				Projectile projectile = this.m_projectiles[i];
				if (projectile)
				{
					if (this.m_doingTell)
					{
						projectile.spriteAnimator.Play();
					}
					else
					{
						projectile.spriteAnimator.StopAndResetFrameToDefault();
					}
				}
			}
		}
	}

	// Token: 0x06005D07 RID: 23815 RVA: 0x0023A0E8 File Offset: 0x002382E8
	private Projectile CreateProjectile(Transform transform)
	{
		if (BossKillCam.BossDeathCamRunning)
		{
			return null;
		}
		string text = "limb";
		if (!string.IsNullOrEmpty(this.OverrideFinalLimbName) && this.IsFinalLimb(transform))
		{
			text = this.OverrideFinalLimbName;
		}
		else if (!string.IsNullOrEmpty(this.OverrideLimbName))
		{
			text = this.OverrideLimbName;
		}
		GameObject gameObject = this.m_body.bulletBank.CreateProjectileFromBank(transform.position, 0f, text, null, false, true, false);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.ManualControl = true;
		component.SkipDistanceElapsedCheck = true;
		component.BulletScriptSettings.surviveRigidbodyCollisions = true;
		component.BulletScriptSettings.surviveTileCollisions = true;
		component.gameObject.SetActive(false);
		component.specRigidbody.enabled = false;
		component.specRigidbody.Velocity = Vector2.zero;
		component.specRigidbody.CanBeCarried = true;
		component.specRigidbody.CollideWithOthers = this.CollideWithOthers;
		if (this.DisableTileMapCollisions)
		{
			component.specRigidbody.CollideWithTileMap = false;
		}
		if (this.m_body && this.m_body.IsBlackPhantom)
		{
			component.ForceBlackBullet = true;
		}
		if (this.OverrideHeightOffGround)
		{
			component.sprite.HeightOffGround = this.HeightOffGround;
		}
		return component;
	}

	// Token: 0x06005D08 RID: 23816 RVA: 0x0023A23C File Offset: 0x0023843C
	private bool IsFinalLimb(Transform transform)
	{
		return transform == this.m_transforms[this.m_transforms.Count - 1];
	}

	// Token: 0x040056BE RID: 22206
	public string LimitPrefix;

	// Token: 0x040056BF RID: 22207
	public bool OverrideHeightOffGround;

	// Token: 0x040056C0 RID: 22208
	[ShowInInspectorIf("OverrideHeightOffGround", true)]
	public float HeightOffGround;

	// Token: 0x040056C1 RID: 22209
	public string OverrideLimbName;

	// Token: 0x040056C2 RID: 22210
	public string OverrideFinalLimbName;

	// Token: 0x040056C3 RID: 22211
	public bool CollideWithOthers = true;

	// Token: 0x040056C4 RID: 22212
	public bool DisableTileMapCollisions;

	// Token: 0x040056C5 RID: 22213
	public bool RotateToMatchTransforms;

	// Token: 0x040056C6 RID: 22214
	public bool WarpBullets;

	// Token: 0x040056C7 RID: 22215
	private bool m_doingTell;

	// Token: 0x040056C8 RID: 22216
	private AIActor m_body;

	// Token: 0x040056C9 RID: 22217
	private List<Transform> m_transforms = new List<Transform>();

	// Token: 0x040056CA RID: 22218
	private List<Projectile> m_projectiles = new List<Projectile>();
}
