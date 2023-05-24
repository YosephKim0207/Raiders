using System;
using System.Collections.Generic;
using BraveDynamicTree;
using UnityEngine;

// Token: 0x0200086C RID: 2156
public class SpeculativeRigidbody : BraveBehaviour, ICollidableObject, ILevelLoadedListener
{
	// Token: 0x170008F5 RID: 2293
	// (get) Token: 0x06002FDD RID: 12253 RVA: 0x000FCF30 File Offset: 0x000FB130
	// (set) Token: 0x06002FDE RID: 12254 RVA: 0x000FCF38 File Offset: 0x000FB138
	public Position Position
	{
		get
		{
			return this.m_position;
		}
		set
		{
			this.m_position = value;
			this.UpdateColliderPositions();
			PhysicsEngine.UpdatePosition(this);
		}
	}

	// Token: 0x170008F6 RID: 2294
	// (get) Token: 0x06002FDF RID: 12255 RVA: 0x000FCF50 File Offset: 0x000FB150
	public b2AABB b2AABB
	{
		get
		{
			int count = this.PixelColliders.Count;
			if (count == 1)
			{
				PixelCollider pixelCollider = this.PixelColliders[0];
				IntVector2 position = pixelCollider.Position;
				IntVector2 dimensions = pixelCollider.Dimensions;
				return new b2AABB((float)position.x * 0.0625f, (float)position.y * 0.0625f, (float)(position.x + dimensions.x - 1) * 0.0625f, (float)(position.y + dimensions.y - 1) * 0.0625f);
			}
			if (count > 1)
			{
				PixelCollider pixelCollider2 = this.PixelColliders[0];
				IntVector2 intVector = pixelCollider2.Position;
				IntVector2 intVector2 = pixelCollider2.Dimensions;
				float num = (float)intVector.x;
				float num2 = (float)intVector.y;
				float num3 = (float)(intVector.x + intVector2.x - 1);
				float num4 = (float)(intVector.y + intVector2.y - 1);
				int num5 = 1;
				do
				{
					pixelCollider2 = this.PixelColliders[num5];
					intVector = pixelCollider2.Position;
					intVector2 = pixelCollider2.Dimensions;
					num = Mathf.Min(num, (float)intVector.x);
					num2 = Mathf.Min(num2, (float)intVector.y);
					num3 = Mathf.Max(num3, (float)(intVector.x + intVector2.x - 1));
					num4 = Mathf.Max(num4, (float)(intVector.y + intVector2.y - 1));
					num5++;
				}
				while (num5 < count);
				return new b2AABB(num * 0.0625f, num2 * 0.0625f, num3 * 0.0625f, num4 * 0.0625f);
			}
			Debug.LogError("Trying to access a b2AABB for a SpecRigidbody with NO COLLIDERS.");
			return new b2AABB(Vector2.zero, Vector2.zero);
		}
	}

	// Token: 0x170008F7 RID: 2295
	// (get) Token: 0x06002FE0 RID: 12256 RVA: 0x000FD10C File Offset: 0x000FB30C
	public PixelCollider PrimaryPixelCollider
	{
		get
		{
			if (this.PixelColliders == null || this.PixelColliders.Count == 0)
			{
				return null;
			}
			return this.PixelColliders[0];
		}
	}

	// Token: 0x170008F8 RID: 2296
	// (get) Token: 0x06002FE1 RID: 12257 RVA: 0x000FD138 File Offset: 0x000FB338
	public PixelCollider HitboxPixelCollider
	{
		get
		{
			for (int i = 0; i < this.PixelColliders.Count; i++)
			{
				if (this.PixelColliders[i].Enabled)
				{
					if (!this.SkipEmptyColliders || this.PixelColliders[i].Height != 0 || this.PixelColliders[i].Width != 0)
					{
						if (this.PixelColliders[i].CollisionLayer == CollisionLayer.EnemyHitBox || this.PixelColliders[i].CollisionLayer == CollisionLayer.PlayerHitBox)
						{
							return this.PixelColliders[i];
						}
					}
				}
			}
			for (int j = 0; j < this.PixelColliders.Count; j++)
			{
				if (this.PixelColliders[j].Enabled)
				{
					if (!this.SkipEmptyColliders || this.PixelColliders[j].Height != 0 || this.PixelColliders[j].Width != 0)
					{
						if (this.PixelColliders[j].CollisionLayer == CollisionLayer.BulletBlocker || this.PixelColliders[j].CollisionLayer == CollisionLayer.BulletBreakable)
						{
							return this.PixelColliders[j];
						}
					}
				}
			}
			for (int k = 0; k < this.PixelColliders.Count; k++)
			{
				if (this.PixelColliders[k].Enabled)
				{
					if (!this.SkipEmptyColliders || this.PixelColliders[k].Height != 0 || this.PixelColliders[k].Width != 0)
					{
						if (this.PixelColliders[k].CollisionLayer == CollisionLayer.HighObstacle)
						{
							return this.PixelColliders[k];
						}
					}
				}
			}
			for (int l = 0; l < this.PixelColliders.Count; l++)
			{
				if (this.PixelColliders[l].Enabled)
				{
					if (!this.SkipEmptyColliders || this.PixelColliders[l].Height != 0 || this.PixelColliders[l].Width != 0)
					{
						if (this.PixelColliders[l].CollisionLayer == CollisionLayer.Projectile)
						{
							return this.PixelColliders[l];
						}
					}
				}
			}
			return this.PrimaryPixelCollider;
		}
	}

	// Token: 0x170008F9 RID: 2297
	// (get) Token: 0x06002FE2 RID: 12258 RVA: 0x000FD3D8 File Offset: 0x000FB5D8
	public PixelCollider GroundPixelCollider
	{
		get
		{
			for (int i = 0; i < this.PixelColliders.Count; i++)
			{
				if (this.PixelColliders[i].Enabled)
				{
					if (this.PixelColliders[i].CollisionLayer == CollisionLayer.EnemyCollider || this.PixelColliders[i].CollisionLayer == CollisionLayer.EnemyHitBox)
					{
						return this.PixelColliders[i];
					}
				}
			}
			for (int j = 0; j < this.PixelColliders.Count; j++)
			{
				if (this.PixelColliders[j].Enabled)
				{
					if (this.PixelColliders[j].CollisionLayer == CollisionLayer.PlayerCollider)
					{
						return this.PixelColliders[j];
					}
				}
			}
			return null;
		}
	}

	// Token: 0x170008FA RID: 2298
	public PixelCollider this[CollisionLayer layer]
	{
		get
		{
			return this.PixelColliders.Find((PixelCollider c) => c.CollisionLayer == layer);
		}
	}

	// Token: 0x06002FE4 RID: 12260 RVA: 0x000FD4E8 File Offset: 0x000FB6E8
	public List<PixelCollider> GetPixelColliders()
	{
		return this.PixelColliders;
	}

	// Token: 0x06002FE5 RID: 12261 RVA: 0x000FD4F0 File Offset: 0x000FB6F0
	public void ForceRegenerate(bool? allowRotation = null, bool? allowScale = null)
	{
		if (allowRotation == null)
		{
			allowRotation = new bool?(this.UpdateCollidersOnRotation);
		}
		if (allowScale == null)
		{
			allowScale = new bool?(this.UpdateCollidersOnScale);
		}
		for (int i = 0; i < this.PixelColliders.Count; i++)
		{
			this.PixelColliders[i].Regenerate(base.transform, allowRotation.Value, allowScale.Value);
		}
		this.RegenerateColliders = false;
		PhysicsEngine.Instance.Register(this);
		PhysicsEngine.UpdatePosition(this);
	}

	// Token: 0x170008FB RID: 2299
	// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x000FD590 File Offset: 0x000FB790
	public Vector2 UnitTopLeft
	{
		get
		{
			PixelCollider primaryPixelCollider = this.PrimaryPixelCollider;
			return new Vector2((float)primaryPixelCollider.Position.x / (float)PhysicsEngine.Instance.PixelsPerUnit, (float)(primaryPixelCollider.Position.y + primaryPixelCollider.Height) / (float)PhysicsEngine.Instance.PixelsPerUnit);
		}
	}

	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x06002FE7 RID: 12263 RVA: 0x000FD5E8 File Offset: 0x000FB7E8
	public Vector2 UnitTopCenter
	{
		get
		{
			PixelCollider primaryPixelCollider = this.PrimaryPixelCollider;
			return new Vector2(((float)primaryPixelCollider.Position.x + (float)primaryPixelCollider.Width / 2f) / (float)PhysicsEngine.Instance.PixelsPerUnit, (float)(primaryPixelCollider.Position.y + primaryPixelCollider.Height) / (float)PhysicsEngine.Instance.PixelsPerUnit);
		}
	}

	// Token: 0x170008FD RID: 2301
	// (get) Token: 0x06002FE8 RID: 12264 RVA: 0x000FD64C File Offset: 0x000FB84C
	public Vector2 UnitTopRight
	{
		get
		{
			PixelCollider primaryPixelCollider = this.PrimaryPixelCollider;
			return new Vector2((float)(primaryPixelCollider.Position.x + primaryPixelCollider.Width) / (float)PhysicsEngine.Instance.PixelsPerUnit, (float)(primaryPixelCollider.Position.y + primaryPixelCollider.Height) / (float)PhysicsEngine.Instance.PixelsPerUnit);
		}
	}

	// Token: 0x170008FE RID: 2302
	// (get) Token: 0x06002FE9 RID: 12265 RVA: 0x000FD6AC File Offset: 0x000FB8AC
	public Vector2 UnitCenterLeft
	{
		get
		{
			PixelCollider primaryPixelCollider = this.PrimaryPixelCollider;
			return new Vector2((float)primaryPixelCollider.Position.x / (float)PhysicsEngine.Instance.PixelsPerUnit, ((float)primaryPixelCollider.Position.y + (float)primaryPixelCollider.Height / 2f) / (float)PhysicsEngine.Instance.PixelsPerUnit);
		}
	}

	// Token: 0x170008FF RID: 2303
	// (get) Token: 0x06002FEA RID: 12266 RVA: 0x000FD70C File Offset: 0x000FB90C
	public Vector2 UnitCenter
	{
		get
		{
			PixelCollider primaryPixelCollider = this.PrimaryPixelCollider;
			return new Vector2(((float)primaryPixelCollider.Position.x + (float)primaryPixelCollider.Width / 2f) / (float)PhysicsEngine.Instance.PixelsPerUnit, ((float)primaryPixelCollider.Position.y + (float)primaryPixelCollider.Height / 2f) / (float)PhysicsEngine.Instance.PixelsPerUnit);
		}
	}

	// Token: 0x17000900 RID: 2304
	// (get) Token: 0x06002FEB RID: 12267 RVA: 0x000FD778 File Offset: 0x000FB978
	public Vector2 UnitCenterRight
	{
		get
		{
			PixelCollider primaryPixelCollider = this.PrimaryPixelCollider;
			return new Vector2((float)(primaryPixelCollider.Position.x + primaryPixelCollider.Width) / (float)PhysicsEngine.Instance.PixelsPerUnit, ((float)primaryPixelCollider.Position.y + (float)primaryPixelCollider.Height / 2f) / (float)PhysicsEngine.Instance.PixelsPerUnit);
		}
	}

	// Token: 0x17000901 RID: 2305
	// (get) Token: 0x06002FEC RID: 12268 RVA: 0x000FD7DC File Offset: 0x000FB9DC
	public Vector2 UnitBottomLeft
	{
		get
		{
			PixelCollider primaryPixelCollider = this.PrimaryPixelCollider;
			return new Vector2((float)primaryPixelCollider.Position.x / (float)PhysicsEngine.Instance.PixelsPerUnit, (float)primaryPixelCollider.Position.y / (float)PhysicsEngine.Instance.PixelsPerUnit);
		}
	}

	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x06002FED RID: 12269 RVA: 0x000FD82C File Offset: 0x000FBA2C
	public Vector2 UnitBottomCenter
	{
		get
		{
			PixelCollider primaryPixelCollider = this.PrimaryPixelCollider;
			return new Vector2(((float)primaryPixelCollider.Position.x + (float)primaryPixelCollider.Width / 2f) / (float)PhysicsEngine.Instance.PixelsPerUnit, (float)primaryPixelCollider.Position.y / (float)PhysicsEngine.Instance.PixelsPerUnit);
		}
	}

	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x06002FEE RID: 12270 RVA: 0x000FD88C File Offset: 0x000FBA8C
	public Vector2 UnitBottomRight
	{
		get
		{
			PixelCollider primaryPixelCollider = this.PrimaryPixelCollider;
			return new Vector2((float)(primaryPixelCollider.Position.x + primaryPixelCollider.Width) / (float)PhysicsEngine.Instance.PixelsPerUnit, (float)primaryPixelCollider.Position.y / (float)PhysicsEngine.Instance.PixelsPerUnit);
		}
	}

	// Token: 0x17000904 RID: 2308
	// (get) Token: 0x06002FEF RID: 12271 RVA: 0x000FD8E4 File Offset: 0x000FBAE4
	public Vector2 UnitDimensions
	{
		get
		{
			return this.PrimaryPixelCollider.Dimensions.ToVector2() / (float)PhysicsEngine.Instance.PixelsPerUnit;
		}
	}

	// Token: 0x17000905 RID: 2309
	// (get) Token: 0x06002FF0 RID: 12272 RVA: 0x000FD914 File Offset: 0x000FBB14
	public float UnitLeft
	{
		get
		{
			return (float)this.PrimaryPixelCollider.MinX / (float)PhysicsEngine.Instance.PixelsPerUnit;
		}
	}

	// Token: 0x17000906 RID: 2310
	// (get) Token: 0x06002FF1 RID: 12273 RVA: 0x000FD930 File Offset: 0x000FBB30
	public float UnitRight
	{
		get
		{
			return (float)(this.PrimaryPixelCollider.MaxX + 1) / (float)PhysicsEngine.Instance.PixelsPerUnit;
		}
	}

	// Token: 0x17000907 RID: 2311
	// (get) Token: 0x06002FF2 RID: 12274 RVA: 0x000FD94C File Offset: 0x000FBB4C
	public float UnitBottom
	{
		get
		{
			return (float)this.PrimaryPixelCollider.MinY / (float)PhysicsEngine.Instance.PixelsPerUnit;
		}
	}

	// Token: 0x17000908 RID: 2312
	// (get) Token: 0x06002FF3 RID: 12275 RVA: 0x000FD968 File Offset: 0x000FBB68
	public float UnitTop
	{
		get
		{
			return (float)(this.PrimaryPixelCollider.MaxY + 1) / (float)PhysicsEngine.Instance.PixelsPerUnit;
		}
	}

	// Token: 0x17000909 RID: 2313
	// (get) Token: 0x06002FF4 RID: 12276 RVA: 0x000FD984 File Offset: 0x000FBB84
	public float UnitWidth
	{
		get
		{
			return (float)this.PrimaryPixelCollider.Dimensions.x / (float)PhysicsEngine.Instance.PixelsPerUnit;
		}
	}

	// Token: 0x1700090A RID: 2314
	// (get) Token: 0x06002FF5 RID: 12277 RVA: 0x000FD9B4 File Offset: 0x000FBBB4
	public float UnitHeight
	{
		get
		{
			return (float)this.PrimaryPixelCollider.Dimensions.y / (float)PhysicsEngine.Instance.PixelsPerUnit;
		}
	}

	// Token: 0x06002FF6 RID: 12278 RVA: 0x000FD9E4 File Offset: 0x000FBBE4
	public PixelCollider GetPixelCollider(ColliderType preferredCollider)
	{
		PixelCollider pixelCollider = null;
		if (preferredCollider == ColliderType.Ground)
		{
			pixelCollider = this.GroundPixelCollider;
		}
		else if (preferredCollider == ColliderType.HitBox)
		{
			pixelCollider = this.HitboxPixelCollider;
		}
		if (pixelCollider == null)
		{
			pixelCollider = this.PrimaryPixelCollider;
		}
		return pixelCollider;
	}

	// Token: 0x06002FF7 RID: 12279 RVA: 0x000FDA24 File Offset: 0x000FBC24
	public Vector2 GetUnitCenter(ColliderType preferredCollider)
	{
		return this.GetPixelCollider(preferredCollider).UnitCenter;
	}

	// Token: 0x1700090B RID: 2315
	// (get) Token: 0x06002FF8 RID: 12280 RVA: 0x000FDA34 File Offset: 0x000FBC34
	// (set) Token: 0x06002FF9 RID: 12281 RVA: 0x000FDA3C File Offset: 0x000FBC3C
	public bool ReflectProjectiles { get; set; }

	// Token: 0x1700090C RID: 2316
	// (get) Token: 0x06002FFA RID: 12282 RVA: 0x000FDA48 File Offset: 0x000FBC48
	// (set) Token: 0x06002FFB RID: 12283 RVA: 0x000FDA50 File Offset: 0x000FBC50
	public bool ReflectBeams { get; set; }

	// Token: 0x1700090D RID: 2317
	// (get) Token: 0x06002FFC RID: 12284 RVA: 0x000FDA5C File Offset: 0x000FBC5C
	// (set) Token: 0x06002FFD RID: 12285 RVA: 0x000FDA64 File Offset: 0x000FBC64
	public bool BlockBeams { get; set; }

	// Token: 0x1700090E RID: 2318
	// (get) Token: 0x06002FFE RID: 12286 RVA: 0x000FDA70 File Offset: 0x000FBC70
	public bool IsSimpleProjectile
	{
		get
		{
			bool? cachedIsSimpleProjectile = this.m_cachedIsSimpleProjectile;
			if (cachedIsSimpleProjectile == null)
			{
				this.m_cachedIsSimpleProjectile = new bool?(this.PixelColliders.Count == 1 && this.PixelColliders[0].CollisionLayer == CollisionLayer.Projectile);
				if (base.projectile)
				{
					this.m_cachedIsSimpleProjectile &= !base.projectile.collidesWithProjectiles;
				}
			}
			return this.m_cachedIsSimpleProjectile.Value;
		}
	}

	// Token: 0x1700090F RID: 2319
	// (get) Token: 0x06002FFF RID: 12287 RVA: 0x000FDB08 File Offset: 0x000FBD08
	// (set) Token: 0x06003000 RID: 12288 RVA: 0x000FDB10 File Offset: 0x000FBD10
	public bool HasTriggerCollisions { get; set; }

	// Token: 0x17000910 RID: 2320
	// (get) Token: 0x06003001 RID: 12289 RVA: 0x000FDB1C File Offset: 0x000FBD1C
	// (set) Token: 0x06003002 RID: 12290 RVA: 0x000FDB24 File Offset: 0x000FBD24
	public bool HasFrameSpecificCollisionExceptions { get; set; }

	// Token: 0x17000911 RID: 2321
	// (get) Token: 0x06003003 RID: 12291 RVA: 0x000FDB30 File Offset: 0x000FBD30
	public bool HasUnresolvedTriggerCollisions
	{
		get
		{
			if (this.RecheckTriggers)
			{
				return true;
			}
			for (int i = 0; i < this.PixelColliders.Count; i++)
			{
				for (int j = 0; j < this.PixelColliders[i].TriggerCollisions.Count; j++)
				{
					if (!this.PixelColliders[i].TriggerCollisions[j].Notified)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x17000912 RID: 2322
	// (get) Token: 0x06003004 RID: 12292 RVA: 0x000FDBB4 File Offset: 0x000FBDB4
	// (set) Token: 0x06003005 RID: 12293 RVA: 0x000FDBBC File Offset: 0x000FBDBC
	public float TimeRemaining { get; set; }

	// Token: 0x17000913 RID: 2323
	// (get) Token: 0x06003006 RID: 12294 RVA: 0x000FDBC8 File Offset: 0x000FBDC8
	// (set) Token: 0x06003007 RID: 12295 RVA: 0x000FDBD0 File Offset: 0x000FBDD0
	public IntVector2 PixelsToMove { get; set; }

	// Token: 0x17000914 RID: 2324
	// (get) Token: 0x06003008 RID: 12296 RVA: 0x000FDBDC File Offset: 0x000FBDDC
	// (set) Token: 0x06003009 RID: 12297 RVA: 0x000FDBE4 File Offset: 0x000FBDE4
	public IntVector2 ImpartedPixelsToMove { get; set; }

	// Token: 0x17000915 RID: 2325
	// (get) Token: 0x0600300A RID: 12298 RVA: 0x000FDBF0 File Offset: 0x000FBDF0
	// (set) Token: 0x0600300B RID: 12299 RVA: 0x000FDBF8 File Offset: 0x000FBDF8
	public bool CollidedX { get; set; }

	// Token: 0x17000916 RID: 2326
	// (get) Token: 0x0600300C RID: 12300 RVA: 0x000FDC04 File Offset: 0x000FBE04
	// (set) Token: 0x0600300D RID: 12301 RVA: 0x000FDC0C File Offset: 0x000FBE0C
	public bool CollidedY { get; set; }

	// Token: 0x17000917 RID: 2327
	// (get) Token: 0x0600300E RID: 12302 RVA: 0x000FDC18 File Offset: 0x000FBE18
	public List<SpeculativeRigidbody.PushedRigidbodyData> PushedRigidbodies
	{
		get
		{
			return this.m_pushedRigidbodies;
		}
	}

	// Token: 0x0600300F RID: 12303 RVA: 0x000FDC20 File Offset: 0x000FBE20
	private void Start()
	{
		this.Initialize();
	}

	// Token: 0x06003010 RID: 12304 RVA: 0x000FDC28 File Offset: 0x000FBE28
	public void Initialize()
	{
		if (this.m_initialized)
		{
			return;
		}
		if (this.TK2DSprite == null)
		{
			this.TK2DSprite = base.sprite;
		}
		this.m_position.UnitPosition = base.transform.position;
		if (this.UpdateCollidersOnRotation && base.transform != null)
		{
			this.LastRotation = base.transform.eulerAngles.z;
		}
		this.ForceRegenerate(null, null);
		if (this.TK2DSprite != null)
		{
			this.TK2DSprite.UpdateZDepth();
		}
		if (PhysicsEngine.Instance != null)
		{
			PhysicsEngine.Instance.Register(this);
		}
		this.m_initialized = true;
	}

	// Token: 0x06003011 RID: 12305 RVA: 0x000FDD04 File Offset: 0x000FBF04
	public void Reinitialize()
	{
		if (!this.m_initialized)
		{
			this.Initialize();
			return;
		}
		this.m_position.UnitPosition = base.transform.position;
		for (int i = 0; i < this.PixelColliders.Count; i++)
		{
			PixelCollider pixelCollider = this.PixelColliders[i];
			pixelCollider.Position = this.Position.PixelPosition + pixelCollider.Offset;
		}
		PhysicsEngine.Instance.Register(this);
		PhysicsEngine.UpdatePosition(this);
	}

	// Token: 0x06003012 RID: 12306 RVA: 0x000FDD98 File Offset: 0x000FBF98
	protected override void OnDestroy()
	{
		if (PhysicsEngine.HasInstance)
		{
			PhysicsEngine.Instance.Deregister(this);
		}
		base.OnDestroy();
	}

	// Token: 0x06003013 RID: 12307 RVA: 0x000FDDB8 File Offset: 0x000FBFB8
	private void OnDrawGizmos()
	{
		if (this.DebugParams.ShowPosition)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(this.Position.UnitPosition, 0.05f);
			if (this.DebugParams.PositionHistory > 0)
			{
				int num = 0;
				foreach (Vector3 vector in this.PreviousPositions)
				{
					Gizmos.color = Color.Lerp(Color.green, Color.red, (float)num / (float)this.DebugParams.PositionHistory);
					Gizmos.DrawSphere(vector, 0.05f);
					num++;
				}
			}
		}
		if (this.DebugParams.ShowVelocity && this.PrimaryPixelCollider != null)
		{
			if (this.Velocity.magnitude > 0f)
			{
				this.LastVelocity = this.Velocity;
			}
			if (this.LastVelocity.magnitude > 0f)
			{
				Gizmos.color = Color.white;
				Vector3 vector2 = this.Position.UnitPosition;
				vector2 += new Vector3((float)this.PrimaryPixelCollider.Width, (float)this.PrimaryPixelCollider.Height, 0f) / (float)(PhysicsEngine.Instance.PixelsPerUnit * 2);
				Gizmos.DrawLine(vector2, vector2 + this.LastVelocity.normalized);
			}
		}
	}

	// Token: 0x06003014 RID: 12308 RVA: 0x000FDF58 File Offset: 0x000FC158
	public bool ContainsPoint(Vector2 point, int mask = 2147483647, bool collideWithTriggers = false)
	{
		return this.ContainsPixel(PhysicsEngine.UnitToPixel(point), mask, collideWithTriggers);
	}

	// Token: 0x06003015 RID: 12309 RVA: 0x000FDF68 File Offset: 0x000FC168
	public bool ContainsPixel(IntVector2 pixel, int mask = 2147483647, bool collideWithTriggers = false)
	{
		for (int i = 0; i < this.GetPixelColliders().Count; i++)
		{
			PixelCollider pixelCollider = this.GetPixelColliders()[i];
			if (collideWithTriggers || !pixelCollider.IsTrigger)
			{
				if (pixelCollider.CanCollideWith(mask, null) && pixelCollider.ContainsPixel(pixel))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003016 RID: 12310 RVA: 0x000FDFD8 File Offset: 0x000FC1D8
	public void RegisterSpecificCollisionException(SpeculativeRigidbody specRigidbody)
	{
		if (!specRigidbody)
		{
			return;
		}
		if (this.m_specificCollisionExceptions == null)
		{
			this.m_specificCollisionExceptions = new List<SpeculativeRigidbody>();
		}
		if (this.m_specificCollisionExceptions.Contains(specRigidbody))
		{
			return;
		}
		this.m_specificCollisionExceptions.Add(specRigidbody);
	}

	// Token: 0x06003017 RID: 12311 RVA: 0x000FE028 File Offset: 0x000FC228
	public bool IsSpecificCollisionException(SpeculativeRigidbody specRigidbody)
	{
		return this.m_specificCollisionExceptions != null && this.m_specificCollisionExceptions.Count != 0 && this.m_specificCollisionExceptions.Contains(specRigidbody);
	}

	// Token: 0x06003018 RID: 12312 RVA: 0x000FE058 File Offset: 0x000FC258
	public void DeregisterSpecificCollisionException(SpeculativeRigidbody specRigidbody)
	{
		if (specRigidbody == null)
		{
			return;
		}
		if (this.m_specificCollisionExceptions == null)
		{
			return;
		}
		this.m_specificCollisionExceptions.Remove(specRigidbody);
	}

	// Token: 0x06003019 RID: 12313 RVA: 0x000FE080 File Offset: 0x000FC280
	public void ClearSpecificCollisionExceptions()
	{
		if (this.m_specificCollisionExceptions == null)
		{
			return;
		}
		this.m_specificCollisionExceptions.Clear();
	}

	// Token: 0x0600301A RID: 12314 RVA: 0x000FE09C File Offset: 0x000FC29C
	public void ClearFrameSpecificCollisionExceptions()
	{
		for (int i = 0; i < this.PixelColliders.Count; i++)
		{
			this.PixelColliders[i].ClearFrameSpecificCollisionExceptions();
		}
		this.HasFrameSpecificCollisionExceptions = false;
	}

	// Token: 0x17000918 RID: 2328
	// (get) Token: 0x0600301B RID: 12315 RVA: 0x000FE0E0 File Offset: 0x000FC2E0
	public List<SpeculativeRigidbody.TemporaryException> TemporaryCollisionExceptions
	{
		get
		{
			return this.m_temporaryCollisionExceptions;
		}
	}

	// Token: 0x0600301C RID: 12316 RVA: 0x000FE0E8 File Offset: 0x000FC2E8
	public void RegisterTemporaryCollisionException(SpeculativeRigidbody specRigidbody, float minTime = 0.01f, float? maxTime = null)
	{
		if (!specRigidbody)
		{
			return;
		}
		if (this.m_temporaryCollisionExceptions == null)
		{
			this.m_temporaryCollisionExceptions = new List<SpeculativeRigidbody.TemporaryException>();
		}
		for (int i = 0; i < this.m_temporaryCollisionExceptions.Count; i++)
		{
			if (this.m_temporaryCollisionExceptions[i].SpecRigidbody == specRigidbody)
			{
				SpeculativeRigidbody.TemporaryException ex = this.m_temporaryCollisionExceptions[i];
				ex.MinTimeRemaining = Mathf.Max(ex.MinTimeRemaining, minTime);
				if (maxTime != null)
				{
					float? maxTimeRemaining = ex.MaxTimeRemaining;
					if (maxTimeRemaining == null)
					{
						ex.MaxTimeRemaining = maxTime;
					}
					else
					{
						ex.MaxTimeRemaining = new float?(Math.Min(ex.MaxTimeRemaining.Value, maxTime.Value));
					}
				}
				this.m_temporaryCollisionExceptions[i] = ex;
				return;
			}
		}
		this.m_temporaryCollisionExceptions.Add(new SpeculativeRigidbody.TemporaryException(specRigidbody, minTime, maxTime));
	}

	// Token: 0x0600301D RID: 12317 RVA: 0x000FE1E8 File Offset: 0x000FC3E8
	public bool IsTemporaryCollisionException(SpeculativeRigidbody specRigidbody)
	{
		if (this.m_temporaryCollisionExceptions == null)
		{
			return false;
		}
		if (this.m_temporaryCollisionExceptions.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < this.m_temporaryCollisionExceptions.Count; i++)
		{
			if (this.m_temporaryCollisionExceptions[i].SpecRigidbody == specRigidbody)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x000FE254 File Offset: 0x000FC454
	public void DeregisterTemporaryCollisionException(SpeculativeRigidbody specRigidbody)
	{
		if (this.m_temporaryCollisionExceptions == null)
		{
			return;
		}
		for (int i = 0; i < this.m_temporaryCollisionExceptions.Count; i++)
		{
			if (this.m_temporaryCollisionExceptions[i].SpecRigidbody == specRigidbody)
			{
				this.m_temporaryCollisionExceptions.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x17000919 RID: 2329
	// (get) Token: 0x0600301F RID: 12319 RVA: 0x000FE2B8 File Offset: 0x000FC4B8
	public List<SpeculativeRigidbody> GhostCollisionExceptions
	{
		get
		{
			return this.m_ghostCollisionExceptions;
		}
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x000FE2C0 File Offset: 0x000FC4C0
	public void RegisterGhostCollisionException(SpeculativeRigidbody specRigidbody)
	{
		if (!specRigidbody)
		{
			return;
		}
		if (this.m_ghostCollisionExceptions == null)
		{
			this.m_ghostCollisionExceptions = new List<SpeculativeRigidbody>();
		}
		if (this.m_ghostCollisionExceptions.Contains(specRigidbody))
		{
			return;
		}
		this.m_ghostCollisionExceptions.Add(specRigidbody);
	}

	// Token: 0x06003021 RID: 12321 RVA: 0x000FE310 File Offset: 0x000FC510
	public bool IsGhostCollisionException(SpeculativeRigidbody specRigidbody)
	{
		return specRigidbody != null && this.m_ghostCollisionExceptions != null && this.m_ghostCollisionExceptions.Contains(specRigidbody);
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x000FE338 File Offset: 0x000FC538
	public void DeregisterGhostCollisionException(SpeculativeRigidbody specRigidbody)
	{
		if (this.m_ghostCollisionExceptions == null)
		{
			return;
		}
		this.m_ghostCollisionExceptions.Remove(specRigidbody);
	}

	// Token: 0x06003023 RID: 12323 RVA: 0x000FE354 File Offset: 0x000FC554
	public void DeregisterGhostCollisionException(int index)
	{
		if (this.m_ghostCollisionExceptions == null)
		{
			return;
		}
		this.m_ghostCollisionExceptions.RemoveAt(index);
	}

	// Token: 0x1700091A RID: 2330
	// (get) Token: 0x06003024 RID: 12324 RVA: 0x000FE370 File Offset: 0x000FC570
	public List<SpeculativeRigidbody> CarriedRigidbodies
	{
		get
		{
			return this.m_carriedRigidbodies;
		}
	}

	// Token: 0x06003025 RID: 12325 RVA: 0x000FE378 File Offset: 0x000FC578
	public void RegisterCarriedRigidbody(SpeculativeRigidbody specRigidbody)
	{
		if (!specRigidbody)
		{
			return;
		}
		if (this.m_carriedRigidbodies == null)
		{
			this.m_carriedRigidbodies = new List<SpeculativeRigidbody>();
		}
		if (this.m_carriedRigidbodies.Contains(specRigidbody))
		{
			return;
		}
		this.m_carriedRigidbodies.Add(specRigidbody);
	}

	// Token: 0x06003026 RID: 12326 RVA: 0x000FE3C8 File Offset: 0x000FC5C8
	public void DeregisterCarriedRigidbody(SpeculativeRigidbody specRigidbody)
	{
		if (this.m_carriedRigidbodies == null)
		{
			return;
		}
		this.m_carriedRigidbodies.Remove(specRigidbody);
	}

	// Token: 0x06003027 RID: 12327 RVA: 0x000FE3E4 File Offset: 0x000FC5E4
	public void ResetTriggerCollisionData()
	{
		this.HasTriggerCollisions = false;
		for (int i = 0; i < this.PixelColliders.Count; i++)
		{
			this.PixelColliders[i].ResetTriggerCollisionData();
			if (this.PixelColliders[i].TriggerCollisions.Count > 0)
			{
				this.HasTriggerCollisions = true;
			}
		}
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x000FE448 File Offset: 0x000FC648
	public void FlagCellsOccupied()
	{
		IntVector2 intVector = this.UnitBottomLeft.ToIntVector2(VectorConversions.Floor);
		PixelCollider primaryPixelCollider = this.PrimaryPixelCollider;
		Vector2 vector = new Vector2((float)(primaryPixelCollider.Position.x + (primaryPixelCollider.Width - 1)) / (float)PhysicsEngine.Instance.PixelsPerUnit, (float)(primaryPixelCollider.Position.y + (primaryPixelCollider.Height - 1)) / (float)PhysicsEngine.Instance.PixelsPerUnit);
		IntVector2 intVector2 = vector.ToIntVector2(VectorConversions.Floor);
		for (int i = intVector.x; i <= intVector2.x; i++)
		{
			for (int j = intVector.y; j <= intVector2.y; j++)
			{
				GameManager.Instance.Dungeon.data[new IntVector2(i, j)].isOccupied = true;
			}
		}
	}

	// Token: 0x06003029 RID: 12329 RVA: 0x000FE52C File Offset: 0x000FC72C
	public bool CanCollideWith(SpeculativeRigidbody otherRigidbody)
	{
		return this && !(this == otherRigidbody) && base.enabled && this.CollideWithOthers && otherRigidbody && otherRigidbody.enabled && otherRigidbody.CollideWithOthers && !this.IsSpecificCollisionException(otherRigidbody) && !otherRigidbody.IsSpecificCollisionException(this) && !this.IsTemporaryCollisionException(otherRigidbody) && !otherRigidbody.IsTemporaryCollisionException(this);
	}

	// Token: 0x0600302A RID: 12330 RVA: 0x000FE5C4 File Offset: 0x000FC7C4
	public void AddCollisionLayerOverride(int mask)
	{
		for (int i = 0; i < this.PixelColliders.Count; i++)
		{
			this.PixelColliders[i].CollisionLayerCollidableOverride |= mask;
		}
	}

	// Token: 0x0600302B RID: 12331 RVA: 0x000FE608 File Offset: 0x000FC808
	public void RemoveCollisionLayerOverride(int mask)
	{
		for (int i = 0; i < this.PixelColliders.Count; i++)
		{
			this.PixelColliders[i].CollisionLayerCollidableOverride &= ~mask;
		}
	}

	// Token: 0x0600302C RID: 12332 RVA: 0x000FE64C File Offset: 0x000FC84C
	public void AddCollisionLayerIgnoreOverride(int mask)
	{
		for (int i = 0; i < this.PixelColliders.Count; i++)
		{
			this.PixelColliders[i].CollisionLayerIgnoreOverride |= mask;
		}
	}

	// Token: 0x0600302D RID: 12333 RVA: 0x000FE690 File Offset: 0x000FC890
	public void RemoveCollisionLayerIgnoreOverride(int mask)
	{
		for (int i = 0; i < this.PixelColliders.Count; i++)
		{
			this.PixelColliders[i].CollisionLayerIgnoreOverride &= ~mask;
		}
	}

	// Token: 0x0600302E RID: 12334 RVA: 0x000FE6D4 File Offset: 0x000FC8D4
	public void UpdateColliderPositions()
	{
		for (int i = 0; i < this.PixelColliders.Count; i++)
		{
			this.PixelColliders[i].Position = this.Position.PixelPosition + this.PixelColliders[i].Offset;
		}
	}

	// Token: 0x0600302F RID: 12335 RVA: 0x000FE734 File Offset: 0x000FC934
	public void BraveOnLevelWasLoaded()
	{
		this.PhysicsRegistration = SpeculativeRigidbody.RegistrationState.Unknown;
	}

	// Token: 0x06003030 RID: 12336 RVA: 0x000FE740 File Offset: 0x000FC940
	public void Cleanup()
	{
		if (this.m_specificCollisionExceptions != null)
		{
			this.m_specificCollisionExceptions.Clear();
		}
		if (this.m_temporaryCollisionExceptions != null)
		{
			this.m_temporaryCollisionExceptions.Clear();
		}
		if (this.m_ghostCollisionExceptions != null)
		{
			this.m_ghostCollisionExceptions.Clear();
		}
		this.m_pushedRigidbodies.Clear();
		if (this.m_carriedRigidbodies != null)
		{
			this.m_carriedRigidbodies.Clear();
		}
	}

	// Token: 0x06003031 RID: 12337 RVA: 0x000FE7B0 File Offset: 0x000FC9B0
	public void AlignWithRigidbodyBottomLeft(SpeculativeRigidbody otherRigidbody)
	{
		Vector2 vector = this.UnitBottomLeft - base.transform.position.XY();
		Vector2 vector2 = otherRigidbody.UnitBottomLeft - otherRigidbody.transform.position.XY();
		base.transform.position = otherRigidbody.transform.position.XY() - vector + vector2;
		base.specRigidbody.Reinitialize();
	}

	// Token: 0x06003032 RID: 12338 RVA: 0x000FE82C File Offset: 0x000FCA2C
	public void AlignWithRigidbodyBottomCenter(SpeculativeRigidbody otherRigidbody, IntVector2? pixelOffset = null)
	{
		Vector2 vector = this.UnitBottomCenter - base.transform.position.XY();
		Vector2 vector2 = otherRigidbody.UnitBottomCenter - otherRigidbody.transform.position.XY();
		Vector2 vector3 = Vector2.zero;
		if (pixelOffset != null)
		{
			vector3 = PhysicsEngine.PixelToUnit(pixelOffset.Value);
		}
		base.transform.position = otherRigidbody.transform.position.XY() - vector + vector2 + vector3;
		base.specRigidbody.Reinitialize();
	}

	// Token: 0x040020A9 RID: 8361
	public bool CollideWithTileMap = true;

	// Token: 0x040020AA RID: 8362
	public bool CollideWithOthers = true;

	// Token: 0x040020AB RID: 8363
	public Vector2 Velocity = new Vector2(0f, 0f);

	// Token: 0x040020AC RID: 8364
	public bool CapVelocity;

	// Token: 0x040020AD RID: 8365
	[ShowInInspectorIf("CapVelocity", false)]
	public Vector2 MaxVelocity;

	// Token: 0x040020AE RID: 8366
	public bool ForceAlwaysUpdate;

	// Token: 0x040020AF RID: 8367
	public bool CanPush;

	// Token: 0x040020B0 RID: 8368
	public bool CanBePushed;

	// Token: 0x040020B1 RID: 8369
	[ShowInInspectorIf("CanPush", false)]
	public float PushSpeedModifier = 1f;

	// Token: 0x040020B2 RID: 8370
	public bool CanCarry;

	// Token: 0x040020B3 RID: 8371
	public bool CanBeCarried = true;

	// Token: 0x040020B4 RID: 8372
	[NonSerialized]
	public bool ForceCarriesRigidbodies;

	// Token: 0x040020B5 RID: 8373
	public bool PreventPiercing;

	// Token: 0x040020B6 RID: 8374
	public bool SkipEmptyColliders;

	// Token: 0x040020B7 RID: 8375
	[HideInInspector]
	public tk2dBaseSprite TK2DSprite;

	// Token: 0x040020B8 RID: 8376
	public Action<SpeculativeRigidbody> OnPreMovement;

	// Token: 0x040020B9 RID: 8377
	public SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate OnPreRigidbodyCollision;

	// Token: 0x040020BA RID: 8378
	public SpeculativeRigidbody.OnPreTileCollisionDelegate OnPreTileCollision;

	// Token: 0x040020BB RID: 8379
	public Action<CollisionData> OnCollision;

	// Token: 0x040020BC RID: 8380
	public SpeculativeRigidbody.OnRigidbodyCollisionDelegate OnRigidbodyCollision;

	// Token: 0x040020BD RID: 8381
	public SpeculativeRigidbody.OnBeamCollisionDelegate OnBeamCollision;

	// Token: 0x040020BE RID: 8382
	public SpeculativeRigidbody.OnTileCollisionDelegate OnTileCollision;

	// Token: 0x040020BF RID: 8383
	public SpeculativeRigidbody.OnTriggerDelegate OnEnterTrigger;

	// Token: 0x040020C0 RID: 8384
	public SpeculativeRigidbody.OnTriggerDelegate OnTriggerCollision;

	// Token: 0x040020C1 RID: 8385
	public SpeculativeRigidbody.OnTriggerExitDelegate OnExitTrigger;

	// Token: 0x040020C2 RID: 8386
	public Action OnPathTargetReached;

	// Token: 0x040020C3 RID: 8387
	public Action<SpeculativeRigidbody, Vector2, IntVector2> OnPostRigidbodyMovement;

	// Token: 0x040020C4 RID: 8388
	public SpeculativeRigidbody.MovementRestrictorDelegate MovementRestrictor;

	// Token: 0x040020C5 RID: 8389
	public Action<BasicBeamController> OnHitByBeam;

	// Token: 0x040020C6 RID: 8390
	[NonSerialized]
	public bool RegenerateColliders;

	// Token: 0x040020C7 RID: 8391
	public bool RecheckTriggers;

	// Token: 0x040020C8 RID: 8392
	public bool UpdateCollidersOnRotation;

	// Token: 0x040020C9 RID: 8393
	public bool UpdateCollidersOnScale;

	// Token: 0x040020CA RID: 8394
	[HideInInspector]
	public Vector2 AxialScale = Vector2.one;

	// Token: 0x040020CB RID: 8395
	public SpeculativeRigidbody.DebugSettings DebugParams = new SpeculativeRigidbody.DebugSettings();

	// Token: 0x040020CC RID: 8396
	[HideInInspector]
	public bool IgnorePixelGrid;

	// Token: 0x040020CD RID: 8397
	[HideInInspector]
	public List<PixelCollider> PixelColliders;

	// Token: 0x040020CE RID: 8398
	[NonSerialized]
	public int SortHash = -1;

	// Token: 0x040020CF RID: 8399
	[NonSerialized]
	public int proxyId = -1;

	// Token: 0x040020D0 RID: 8400
	[NonSerialized]
	public SpeculativeRigidbody.RegistrationState PhysicsRegistration = SpeculativeRigidbody.RegistrationState.Deregistered;

	// Token: 0x040020D2 RID: 8402
	public Func<Vector2, Vector2, Vector2> ReflectProjectilesNormalGenerator;

	// Token: 0x040020D4 RID: 8404
	public Func<Vector2, Vector2, Vector2> ReflectBeamsNormalGenerator;

	// Token: 0x040020D6 RID: 8406
	private bool? m_cachedIsSimpleProjectile;

	// Token: 0x040020DE RID: 8414
	[NonSerialized]
	public bool PathMode;

	// Token: 0x040020DF RID: 8415
	[NonSerialized]
	public IntVector2 PathTarget;

	// Token: 0x040020E0 RID: 8416
	[NonSerialized]
	public float PathSpeed;

	// Token: 0x040020E1 RID: 8417
	[NonSerialized]
	public LinkedList<Vector3> PreviousPositions = new LinkedList<Vector3>();

	// Token: 0x040020E2 RID: 8418
	[NonSerialized]
	public Vector3 LastVelocity;

	// Token: 0x040020E3 RID: 8419
	[NonSerialized]
	public float LastRotation;

	// Token: 0x040020E4 RID: 8420
	[NonSerialized]
	public Vector2 LastScale;

	// Token: 0x040020E5 RID: 8421
	public Position m_position = new Position(0, 0);

	// Token: 0x040020E6 RID: 8422
	[NonSerialized]
	private List<SpeculativeRigidbody> m_specificCollisionExceptions;

	// Token: 0x040020E7 RID: 8423
	[NonSerialized]
	public List<SpeculativeRigidbody.TemporaryException> m_temporaryCollisionExceptions;

	// Token: 0x040020E8 RID: 8424
	[NonSerialized]
	private List<SpeculativeRigidbody> m_ghostCollisionExceptions;

	// Token: 0x040020E9 RID: 8425
	[NonSerialized]
	public List<SpeculativeRigidbody.PushedRigidbodyData> m_pushedRigidbodies = new List<SpeculativeRigidbody.PushedRigidbodyData>();

	// Token: 0x040020EA RID: 8426
	[NonSerialized]
	private List<SpeculativeRigidbody> m_carriedRigidbodies;

	// Token: 0x040020EB RID: 8427
	private bool m_initialized;

	// Token: 0x0200086D RID: 2157
	// (Invoke) Token: 0x06003034 RID: 12340
	public delegate void OnPreRigidbodyCollisionDelegate(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider);

	// Token: 0x0200086E RID: 2158
	// (Invoke) Token: 0x06003038 RID: 12344
	public delegate void OnPreTileCollisionDelegate(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, PhysicsEngine.Tile tile, PixelCollider tilePixelCollider);

	// Token: 0x0200086F RID: 2159
	// (Invoke) Token: 0x0600303C RID: 12348
	public delegate void OnRigidbodyCollisionDelegate(CollisionData rigidbodyCollision);

	// Token: 0x02000870 RID: 2160
	// (Invoke) Token: 0x06003040 RID: 12352
	public delegate void OnBeamCollisionDelegate(BeamController beam);

	// Token: 0x02000871 RID: 2161
	// (Invoke) Token: 0x06003044 RID: 12356
	public delegate void OnTileCollisionDelegate(CollisionData tileCollision);

	// Token: 0x02000872 RID: 2162
	// (Invoke) Token: 0x06003048 RID: 12360
	public delegate void OnTriggerDelegate(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData);

	// Token: 0x02000873 RID: 2163
	// (Invoke) Token: 0x0600304C RID: 12364
	public delegate void OnTriggerExitDelegate(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody);

	// Token: 0x02000874 RID: 2164
	// (Invoke) Token: 0x06003050 RID: 12368
	public delegate void MovementRestrictorDelegate(SpeculativeRigidbody specRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation);

	// Token: 0x02000875 RID: 2165
	public enum RegistrationState
	{
		// Token: 0x040020ED RID: 8429
		Registered,
		// Token: 0x040020EE RID: 8430
		DeregisterScheduled,
		// Token: 0x040020EF RID: 8431
		Deregistered,
		// Token: 0x040020F0 RID: 8432
		Unknown
	}

	// Token: 0x02000876 RID: 2166
	[Serializable]
	public class DebugSettings
	{
		// Token: 0x040020F1 RID: 8433
		public bool ShowPosition;

		// Token: 0x040020F2 RID: 8434
		public int PositionHistory;

		// Token: 0x040020F3 RID: 8435
		public bool ShowVelocity;

		// Token: 0x040020F4 RID: 8436
		public bool ShowSlope;
	}

	// Token: 0x02000877 RID: 2167
	public struct PushedRigidbodyData
	{
		// Token: 0x06003054 RID: 12372 RVA: 0x000FE8D8 File Offset: 0x000FCAD8
		public PushedRigidbodyData(SpeculativeRigidbody specRigidbody)
		{
			this.SpecRigidbody = specRigidbody;
			this.PushedThisFrame = false;
			this.Direction = IntVector2.Zero;
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06003055 RID: 12373 RVA: 0x000FE8F4 File Offset: 0x000FCAF4
		public bool CollidedX
		{
			get
			{
				return this.Direction.x != 0;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06003056 RID: 12374 RVA: 0x000FE908 File Offset: 0x000FCB08
		public bool CollidedY
		{
			get
			{
				return this.Direction.y != 0;
			}
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x000FE91C File Offset: 0x000FCB1C
		internal IntVector2 GetPushedPixelsToMove(IntVector2 pixelsToMove)
		{
			return IntVector2.Scale(this.Direction, pixelsToMove);
		}

		// Token: 0x040020F5 RID: 8437
		public SpeculativeRigidbody SpecRigidbody;

		// Token: 0x040020F6 RID: 8438
		public bool PushedThisFrame;

		// Token: 0x040020F7 RID: 8439
		public IntVector2 Direction;
	}

	// Token: 0x02000878 RID: 2168
	public struct TemporaryException
	{
		// Token: 0x06003058 RID: 12376 RVA: 0x000FE92C File Offset: 0x000FCB2C
		public TemporaryException(SpeculativeRigidbody specRigidbody, float minTime, float? maxTime)
		{
			this.SpecRigidbody = specRigidbody;
			this.MinTimeRemaining = minTime;
			this.MaxTimeRemaining = maxTime;
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x000FE944 File Offset: 0x000FCB44
		public bool HasEnded(SpeculativeRigidbody myRigidbody)
		{
			if (!this.SpecRigidbody)
			{
				return true;
			}
			float? maxTimeRemaining = this.MaxTimeRemaining;
			if (maxTimeRemaining != null)
			{
				float? maxTimeRemaining2 = this.MaxTimeRemaining;
				this.MaxTimeRemaining = ((maxTimeRemaining2 == null) ? null : new float?(maxTimeRemaining2.GetValueOrDefault() - BraveTime.DeltaTime));
				if (this.MaxTimeRemaining.Value <= 0f)
				{
					return true;
				}
			}
			if (this.MinTimeRemaining > 0f)
			{
				this.MinTimeRemaining -= BraveTime.DeltaTime;
				return false;
			}
			for (int i = 0; i < myRigidbody.PixelColliders.Count; i++)
			{
				PixelCollider pixelCollider = myRigidbody.PixelColliders[i];
				for (int j = 0; j < this.SpecRigidbody.PixelColliders.Count; j++)
				{
					PixelCollider pixelCollider2 = this.SpecRigidbody.PixelColliders[j];
					if (pixelCollider.CanCollideWith(pixelCollider2, true) && pixelCollider.Overlaps(pixelCollider2))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x040020F8 RID: 8440
		public SpeculativeRigidbody SpecRigidbody;

		// Token: 0x040020F9 RID: 8441
		public float MinTimeRemaining;

		// Token: 0x040020FA RID: 8442
		public float? MaxTimeRemaining;
	}
}
