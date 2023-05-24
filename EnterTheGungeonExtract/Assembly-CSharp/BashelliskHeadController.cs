using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02000FC3 RID: 4035
public class BashelliskHeadController : BashelliskSegment
{
	// Token: 0x17000C90 RID: 3216
	// (get) Token: 0x060057E5 RID: 22501 RVA: 0x0021898C File Offset: 0x00216B8C
	// (set) Token: 0x060057E6 RID: 22502 RVA: 0x00218994 File Offset: 0x00216B94
	public bool CanPickup { get; set; }

	// Token: 0x17000C91 RID: 3217
	// (get) Token: 0x060057E7 RID: 22503 RVA: 0x002189A0 File Offset: 0x00216BA0
	// (set) Token: 0x060057E8 RID: 22504 RVA: 0x002189A8 File Offset: 0x00216BA8
	public bool ReinitMovementDirection { get; set; }

	// Token: 0x17000C92 RID: 3218
	// (get) Token: 0x060057E9 RID: 22505 RVA: 0x002189B4 File Offset: 0x00216BB4
	// (set) Token: 0x060057EA RID: 22506 RVA: 0x002189BC File Offset: 0x00216BBC
	public bool IsMidPickup { get; set; }

	// Token: 0x060057EB RID: 22507 RVA: 0x002189C8 File Offset: 0x00216BC8
	public void Start()
	{
		this.Body.AddFirst(this);
		base.healthHaver.bodyRigidbodies = new List<SpeculativeRigidbody>();
		base.healthHaver.bodyRigidbodies.Add(base.specRigidbody);
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreRigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody2.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
		PhysicsEngine.Instance.OnPostRigidbodyMovement += this.OnPostRigidbodyMovement;
		for (int i = 0; i < this.startingSegments; i++)
		{
			this.Grow();
		}
		this.m_path.AddLast(this.center.position);
		this.m_path.AddLast(this.center.position);
		this.m_pathSegmentLength = 0f;
		this.m_spawnTimer = this.initialSpawnDelay;
		List<BashelliskPickupSpawnPoint> componentsInRoom = base.aiActor.ParentRoom.GetComponentsInRoom<BashelliskPickupSpawnPoint>();
		for (int j = 0; j < componentsInRoom.Count; j++)
		{
			this.m_pickupLocations.Add(componentsInRoom[j].transform.position + new Vector3(-0.3125f, 0f));
		}
	}

	// Token: 0x060057EC RID: 22508 RVA: 0x00218B34 File Offset: 0x00216D34
	public void Update()
	{
		for (LinkedListNode<BashelliskSegment> linkedListNode = this.Body.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			if (linkedListNode.Value != null && linkedListNode.Value.specRigidbody)
			{
				linkedListNode.Value.specRigidbody.CollideWithOthers = base.specRigidbody.CollideWithOthers;
			}
		}
		if (base.aiActor.enabled)
		{
			bool flag = false;
			if (this.m_spawnTimer <= 0f)
			{
				flag = true;
			}
			if (base.healthHaver.GetCurrentHealthPercentage() < this.m_nextSpawnHealthThreshold)
			{
				flag = true;
			}
			if (flag)
			{
				this.SpawnBodyPickup();
				this.m_spawnTimer = UnityEngine.Random.Range(this.minSpawnDelay, this.maxSpawnDelay);
				this.m_nextSpawnHealthThreshold -= 0.2f;
			}
			if (this.AvailablePickups.Count == 0)
			{
				this.m_spawnTimer -= BraveTime.DeltaTime;
			}
		}
		for (int i = this.m_collidedProjectiles.Count - 1; i >= 0; i--)
		{
			Projectile projectile = this.m_collidedProjectiles[i];
			if (!projectile || !projectile.gameObject || !projectile.gameObject.activeSelf || projectile.specRigidbody.PrimaryPixelCollider == null)
			{
				this.m_collidedProjectiles.RemoveAt(i);
			}
			else
			{
				bool flag2 = false;
				LinkedListNode<BashelliskSegment> linkedListNode = this.Body.First;
				PixelCollider primaryPixelCollider = projectile.specRigidbody.PrimaryPixelCollider;
				while (linkedListNode != null && !flag2)
				{
					if (linkedListNode.Value != null && linkedListNode.Value.specRigidbody != null)
					{
						SpeculativeRigidbody specRigidbody = linkedListNode.Value.specRigidbody;
						for (int j = 0; j < specRigidbody.PixelColliders.Count; j++)
						{
							PixelCollider pixelCollider = specRigidbody.PixelColliders[j];
							if (pixelCollider.Enabled && pixelCollider.Overlaps(primaryPixelCollider))
							{
								flag2 = true;
								break;
							}
						}
					}
					linkedListNode = linkedListNode.Next;
				}
				if (!flag2)
				{
					this.m_collidedProjectiles.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x060057ED RID: 22509 RVA: 0x00218D7C File Offset: 0x00216F7C
	protected override void OnDestroy()
	{
		if (GameManager.HasInstance && this)
		{
			while (this.Body.Count > 0)
			{
				if (this.Body.First.Value)
				{
					UnityEngine.Object.Destroy(this.Body.First.Value.gameObject);
				}
				this.Body.RemoveFirst();
			}
			while (this.AvailablePickups.Count > 0)
			{
				if (this.AvailablePickups.First.Value)
				{
					UnityEngine.Object.Destroy(this.AvailablePickups.First.Value.gameObject);
				}
				this.AvailablePickups.RemoveFirst();
			}
		}
		this.m_path.ClearPool();
		if (PhysicsEngine.HasInstance)
		{
			PhysicsEngine.Instance.OnPostRigidbodyMovement -= this.OnPostRigidbodyMovement;
		}
		base.OnDestroy();
	}

	// Token: 0x060057EE RID: 22510 RVA: 0x00218E7C File Offset: 0x0021707C
	public void OnPostRigidbodyMovement()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.IsMidPickup)
		{
			return;
		}
		this.UpdatePath(this.center.position);
		if (this.next)
		{
			this.next.UpdatePosition(this.m_path, this.m_path.First, 0f, 0f);
		}
		base.aiAnimator.FacingDirection = (this.Body.First.Value.center.position - this.Body.First.Next.Value.center.position).XY().ToAngle();
		if (this.CanPickup)
		{
			PixelCollider hitboxPixelCollider = base.specRigidbody.HitboxPixelCollider;
			LinkedListNode<BashelliskBodyPickupController> linkedListNode = this.AvailablePickups.First;
			while (linkedListNode != null)
			{
				if (!linkedListNode.Value)
				{
					LinkedListNode<BashelliskBodyPickupController> linkedListNode2 = linkedListNode;
					linkedListNode = linkedListNode.Next;
					this.AvailablePickups.Remove(linkedListNode2, true);
				}
				else
				{
					PixelCollider hitboxPixelCollider2 = linkedListNode.Value.specRigidbody.HitboxPixelCollider;
					if (hitboxPixelCollider2 != null && hitboxPixelCollider.Overlaps(hitboxPixelCollider2))
					{
						base.StartCoroutine(this.PickupCR(linkedListNode.Value));
						this.AvailablePickups.Remove(linkedListNode, true);
						break;
					}
					linkedListNode = linkedListNode.Next;
				}
			}
		}
		else if (this.AvailablePickups.Count > 0)
		{
			LinkedListNode<BashelliskBodyPickupController> linkedListNode3 = this.AvailablePickups.First;
			while (linkedListNode3 != null)
			{
				if (!linkedListNode3.Value)
				{
					LinkedListNode<BashelliskBodyPickupController> linkedListNode4 = linkedListNode3;
					linkedListNode3 = linkedListNode3.Next;
					this.AvailablePickups.Remove(linkedListNode4, true);
				}
				else
				{
					linkedListNode3 = linkedListNode3.Next;
				}
			}
		}
	}

	// Token: 0x060057EF RID: 22511 RVA: 0x00219054 File Offset: 0x00217254
	public void Grow()
	{
		Vector3 position = this.center.position;
		int num = this.Body.Count - 1;
		int num2 = this.segmentCounts.Count - 1;
		int num3 = this.segmentCounts.Count - 1;
		while (num3 >= 0 && num >= 0)
		{
			num -= this.segmentCounts[num3];
			num2 = num3;
			num3--;
		}
		num2 = Mathf.Max(0, num2);
		BashelliskBodyController bashelliskBodyController = UnityEngine.Object.Instantiate<BashelliskBodyController>(this.segmentPrefabs[num2], this.center.position, Quaternion.identity);
		bashelliskBodyController.transform.position = position - bashelliskBodyController.center.localPosition;
		bashelliskBodyController.Init(this);
		if (this.Body.Count > 1)
		{
			bashelliskBodyController.next = this.Body.First.Next.Value;
			this.Body.First.Next.Value.previous = bashelliskBodyController;
		}
		this.next = bashelliskBodyController;
		bashelliskBodyController.previous = this;
		this.Body.AddAfter(this.Body.First, bashelliskBodyController);
		base.healthHaver.bodyRigidbodies.Add(bashelliskBodyController.specRigidbody);
		SpeculativeRigidbody specRigidbody = bashelliskBodyController.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreRigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = bashelliskBodyController.specRigidbody;
		specRigidbody2.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody2.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
	}

	// Token: 0x060057F0 RID: 22512 RVA: 0x002191EC File Offset: 0x002173EC
	private IEnumerator PickupCR(BashelliskBodyPickupController pickup)
	{
		Vector2 pickupCenter = pickup.center.position;
		this.IsMidPickup = true;
		base.aiActor.BehaviorVelocity = Vector2.zero;
		base.aiAnimator.PlayUntilCancelled("bite_open", true, null, -1f, false);
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.FacingDirection = (pickup.center.position - this.center.position).XY().ToAngle();
		pickup.aiAnimator.PlayUntilCancelled("fear", true, null, -1f, false);
		pickup.behaviorSpeculator.InterruptAndDisable();
		while (base.aiAnimator.IsPlaying("bite_open"))
		{
			yield return null;
		}
		if (!pickup)
		{
			base.aiAnimator.PlayUntilFinished("bite_close", false, null, -1f, false);
			this.IsMidPickup = false;
			yield break;
		}
		pickup.healthHaver.minimumHealth = 1f;
		base.healthHaver.ApplyHealing(this.pickupHealthScaler * pickup.healthHaver.GetCurrentHealth());
		this.Grow();
		if (this.next)
		{
			this.next.aiAnimator.PlayUntilFinished("intro", false, null, -1f, false);
		}
		pickup.specRigidbody.enabled = false;
		base.sprite.AttachRenderer(pickup.sprite);
		pickup.sprite.HeightOffGround = -0.1f;
		pickup.sprite.UpdateZDepth();
		Vector2 baseOffset = pickupCenter - this.center.position.XY();
		Vector2 offset = baseOffset + baseOffset.normalized * 0.25f;
		base.specRigidbody.PathMode = true;
		base.specRigidbody.PathTarget = PhysicsEngine.UnitToPixel(base.specRigidbody.Position.UnitPosition + offset);
		base.specRigidbody.PathSpeed = this.pickupLurchSpeed;
		base.aiAnimator.PlayUntilCancelled("bite_close", true, null, -1f, false);
		float timer = 0f;
		float maxSafeTime = offset.magnitude / this.pickupLurchSpeed + 0.4f;
		while (base.specRigidbody.PathMode && timer < maxSafeTime)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			if ((double)timer > ((double)maxSafeTime - 0.4) * 0.6000000238418579 && pickup)
			{
				pickup.transform.localScale = new Vector3(0.7f, 0.7f);
			}
		}
		base.specRigidbody.PathMode = false;
		base.aiActor.ApplyEffect(pickup.buffEffect, 1f, null);
		UnityEngine.Object.Destroy(pickup.gameObject);
		offset = baseOffset.normalized * -0.5f;
		base.specRigidbody.PathMode = true;
		base.specRigidbody.PathTarget = PhysicsEngine.UnitToPixel(base.specRigidbody.Position.UnitPosition + offset);
		timer = 0f;
		maxSafeTime = offset.magnitude / this.pickupLurchSpeed + 0.4f;
		while (base.specRigidbody.PathMode && timer < maxSafeTime)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
		}
		base.specRigidbody.PathMode = false;
		this.UpdatePath(this.center.position);
		if (this.next)
		{
			this.next.UpdatePosition(this.m_path, this.m_path.First, 0f, 0f);
		}
		while (base.aiAnimator.IsPlaying("bite_close"))
		{
			yield return null;
		}
		base.aiAnimator.EndAnimationIf("bite_close");
		this.IsMidPickup = false;
		yield break;
	}

	// Token: 0x060057F1 RID: 22513 RVA: 0x00219210 File Offset: 0x00217410
	private void OnPreRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		Projectile projectile = otherRigidbody.projectile;
		if (projectile && this.m_collidedProjectiles.Contains(projectile))
		{
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x060057F2 RID: 22514 RVA: 0x00219248 File Offset: 0x00217448
	private void OnRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		Projectile projectile = rigidbodyCollision.OtherRigidbody.projectile;
		if (projectile)
		{
			this.m_collidedProjectiles.Add(projectile);
		}
	}

	// Token: 0x060057F3 RID: 22515 RVA: 0x00219278 File Offset: 0x00217478
	private void SpawnBodyPickup()
	{
		List<Vector2> list = new List<Vector2>();
		list.AddRange(this.m_pickupLocations.FindAll(delegate(Vector2 pos)
		{
			for (LinkedListNode<BashelliskBodyPickupController> linkedListNode = this.AvailablePickups.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				if (linkedListNode.Value && Vector2.Distance(pos, linkedListNode.Value.center.transform.position.XY()) < 3f)
				{
					return false;
				}
			}
			return true;
		}));
		Vector2 vector;
		if (list.Count > 0)
		{
			vector = BraveUtility.RandomElement<Vector2>(list);
		}
		else
		{
			Vector2 vector2 = BraveUtility.ViewportToWorldpoint(new Vector2(0f, 0f), ViewportType.Gameplay);
			Vector2 vector3 = BraveUtility.ViewportToWorldpoint(new Vector2(1f, 1f), ViewportType.Gameplay);
			IntVector2 bottomLeft = vector2.ToIntVector2(VectorConversions.Ceil);
			IntVector2 topRight = vector3.ToIntVector2(VectorConversions.Floor) - IntVector2.One;
			CellValidator cellValidator = delegate(IntVector2 c)
			{
				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < 2; j++)
					{
						if (GameManager.Instance.Dungeon.data.isTopWall(c.x + i, c.y + j))
						{
							return false;
						}
					}
				}
				return c.x >= bottomLeft.x && c.y >= bottomLeft.y && c.x + 1 <= topRight.x && c.y + 1 <= topRight.y;
			};
			IntVector2? randomAvailableCell = base.aiActor.ParentRoom.GetRandomAvailableCell(new IntVector2?(new IntVector2(2, 2)), new CellTypes?(CellTypes.FLOOR), false, cellValidator);
			if (randomAvailableCell != null)
			{
				vector = randomAvailableCell.Value.ToVector2();
			}
			else
			{
				vector = base.transform.position;
			}
		}
		AIActor aiactor = AIActor.Spawn(this.pickupPrefab.aiActor, vector, base.aiActor.ParentRoom, false, AIActor.AwakenAnimationType.Default, true);
		aiactor.transform.position += new Vector3(1.25f, 0f);
		aiactor.specRigidbody.Reinitialize();
		this.AvailablePickups.AddLast(aiactor.GetComponent<BashelliskBodyPickupController>());
		base.specRigidbody.RegisterSpecificCollisionException(aiactor.specRigidbody);
	}

	// Token: 0x060057F4 RID: 22516 RVA: 0x00219404 File Offset: 0x00217604
	private void SpawnBodyPickupAtMouse()
	{
		IntVector2 bestRewardLocation = base.aiActor.ParentRoom.GetBestRewardLocation(new IntVector2(2, 2), BraveUtility.GetMousePosition(), false);
		BashelliskBodyPickupController bashelliskBodyPickupController = UnityEngine.Object.Instantiate<BashelliskBodyPickupController>(this.pickupPrefab, bestRewardLocation.ToVector2(), Quaternion.identity);
		this.AvailablePickups.AddLast(bashelliskBodyPickupController);
	}

	// Token: 0x060057F5 RID: 22517 RVA: 0x00219460 File Offset: 0x00217660
	private void UpdatePath(Vector2 newPosition)
	{
		float num = Vector2.Distance(newPosition, this.m_path.First.Value);
		for (LinkedListNode<BashelliskSegment> linkedListNode = this.Body.First.Next; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value.PathDist += num;
		}
		if (this.m_pathSegmentLength > 0.5f)
		{
			this.m_path.AddFirst(newPosition);
		}
		else
		{
			this.m_path.First.Value = newPosition;
		}
		this.m_pathSegmentLength = Vector2.Distance(this.m_path.First.Value, this.m_path.First.Next.Value);
	}

	// Token: 0x040050DD RID: 20701
	[Header("Head-Specific Data")]
	public BashelliskBodyPickupController pickupPrefab;

	// Token: 0x040050DE RID: 20702
	public List<BashelliskBodyController> segmentPrefabs;

	// Token: 0x040050DF RID: 20703
	public List<int> segmentCounts;

	// Token: 0x040050E0 RID: 20704
	public int startingSegments;

	// Token: 0x040050E1 RID: 20705
	[Header("Spawn Pickup Data")]
	public float initialSpawnDelay = 20f;

	// Token: 0x040050E2 RID: 20706
	public float minSpawnDelay = 20f;

	// Token: 0x040050E3 RID: 20707
	public float maxSpawnDelay = 40f;

	// Token: 0x040050E4 RID: 20708
	public float pickupHealthScaler = 1f;

	// Token: 0x040050E5 RID: 20709
	public float pickupLurchSpeed = 13f;

	// Token: 0x040050E6 RID: 20710
	[NonSerialized]
	public LinkedList<BashelliskSegment> Body = new LinkedList<BashelliskSegment>();

	// Token: 0x040050E7 RID: 20711
	[NonSerialized]
	public readonly PooledLinkedList<BashelliskBodyPickupController> AvailablePickups = new PooledLinkedList<BashelliskBodyPickupController>();

	// Token: 0x040050EB RID: 20715
	private readonly PooledLinkedList<Vector2> m_path = new PooledLinkedList<Vector2>();

	// Token: 0x040050EC RID: 20716
	private float m_pathSegmentLength;

	// Token: 0x040050ED RID: 20717
	private float m_spawnTimer;

	// Token: 0x040050EE RID: 20718
	private float m_nextSpawnHealthThreshold = 0.8f;

	// Token: 0x040050EF RID: 20719
	private List<Vector2> m_pickupLocations = new List<Vector2>();

	// Token: 0x040050F0 RID: 20720
	private List<Projectile> m_collidedProjectiles = new List<Projectile>();
}
