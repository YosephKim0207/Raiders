using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001109 RID: 4361
public class BreakableColumn : DungeonPlaceableBehaviour
{
	// Token: 0x06006031 RID: 24625 RVA: 0x00250B7C File Offset: 0x0024ED7C
	public void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreRigidbodyCollision));
	}

	// Token: 0x06006032 RID: 24626 RVA: 0x00250BA8 File Offset: 0x0024EDA8
	public void Update()
	{
	}

	// Token: 0x06006033 RID: 24627 RVA: 0x00250BAC File Offset: 0x0024EDAC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06006034 RID: 24628 RVA: 0x00250BB4 File Offset: 0x0024EDB4
	private void OnPreRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (!otherRigidbody.projectile)
		{
			return;
		}
		if (!otherRigidbody.name.StartsWith("TankTreader_Fast_Projectile") && !otherRigidbody.name.StartsWith("TankTreader_Scatter_Projectile") && !otherRigidbody.name.StartsWith("TankTreader_Spawn_Projectile") && !otherRigidbody.name.StartsWith("TankTreader_Rocket_Projectile"))
		{
			return;
		}
		if (this.m_state == BreakableColumn.State.Default)
		{
			base.sprite.SetSprite(this.damagedSprite);
			this.m_state = BreakableColumn.State.Damaged;
			this.SpawnFlakes();
			if (!PhysicsEngine.PendingCastResult.Overlap)
			{
				return;
			}
		}
		if (this.m_state == BreakableColumn.State.Damaged)
		{
			PhysicsEngine.SkipCollision = true;
			Exploder.Explode(PhysicsEngine.PendingCastResult.Contact, this.explosionData, PhysicsEngine.PendingCastResult.Normal, null, false, CoreDamageTypes.None, false);
			base.sprite.SetSprite(this.destroyedSprite);
			base.specRigidbody.enabled = false;
			base.SetAreaPassable();
			base.sprite.IsPerpendicular = false;
			base.sprite.HeightOffGround = -1.95f;
			base.sprite.UpdateZDepth();
			base.gameObject.layer = LayerMask.NameToLayer("BG_Critical");
			BreakableChunk component = base.GetComponent<BreakableChunk>();
			if (component)
			{
				component.Trigger(false, new Vector3?(PhysicsEngine.PendingCastResult.Contact));
			}
			this.m_state = BreakableColumn.State.Destroyed;
		}
	}

	// Token: 0x06006035 RID: 24629 RVA: 0x00250D30 File Offset: 0x0024EF30
	private void SpawnFlakes()
	{
		if (this.flakeCount > 0)
		{
			for (int i = 0; i < this.flakeCount; i++)
			{
				if (this.flakeSpawnDuration == 0f)
				{
					this.SpawnRandomizeFlakes();
				}
				else
				{
					base.Invoke("SpawnRandomizeFlakes", UnityEngine.Random.Range(0f, this.flakeSpawnDuration));
				}
			}
		}
	}

	// Token: 0x06006036 RID: 24630 RVA: 0x00250D98 File Offset: 0x0024EF98
	private void SpawnRandomizeFlakes()
	{
		Vector3 vector = base.transform.position + new Vector3(UnityEngine.Random.Range(0f, this.flakeAreaWidth), UnityEngine.Random.Range(0f, this.flakeAreaHeight));
		this.puff.SpawnAtPosition(vector, 0f, null, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), null, false, null, null, false);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.flake, vector, Quaternion.identity);
		tk2dSprite component = gameObject.GetComponent<tk2dSprite>();
		component.HeightOffGround = 0.1f;
		base.sprite.AttachRenderer(component);
		component.UpdateZDepth();
	}

	// Token: 0x04005AC7 RID: 23239
	[FormerlySerializedAs("damagedAnimation")]
	public string damagedSprite;

	// Token: 0x04005AC8 RID: 23240
	[FormerlySerializedAs("destroyAnimation")]
	public string destroyedSprite;

	// Token: 0x04005AC9 RID: 23241
	[Header("Flake Data")]
	public GameObject flake;

	// Token: 0x04005ACA RID: 23242
	public VFXPool puff;

	// Token: 0x04005ACB RID: 23243
	public int flakeCount;

	// Token: 0x04005ACC RID: 23244
	public float flakeAreaWidth;

	// Token: 0x04005ACD RID: 23245
	public float flakeAreaHeight;

	// Token: 0x04005ACE RID: 23246
	public float flakeSpawnDuration;

	// Token: 0x04005ACF RID: 23247
	[Header("Explosion Data")]
	public ExplosionData explosionData;

	// Token: 0x04005AD0 RID: 23248
	private BreakableColumn.State m_state;

	// Token: 0x0200110A RID: 4362
	private enum State
	{
		// Token: 0x04005AD2 RID: 23250
		Default,
		// Token: 0x04005AD3 RID: 23251
		Damaged,
		// Token: 0x04005AD4 RID: 23252
		Destroyed
	}
}
