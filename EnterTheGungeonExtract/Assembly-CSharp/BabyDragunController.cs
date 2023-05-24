using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200134F RID: 4943
public class BabyDragunController : MonoBehaviour
{
	// Token: 0x17001101 RID: 4353
	// (get) Token: 0x06007014 RID: 28692 RVA: 0x002C69A4 File Offset: 0x002C4BA4
	// (set) Token: 0x06007015 RID: 28693 RVA: 0x002C69AC File Offset: 0x002C4BAC
	public bool IsEnemy { get; set; }

	// Token: 0x17001102 RID: 4354
	// (get) Token: 0x06007016 RID: 28694 RVA: 0x002C69B8 File Offset: 0x002C4BB8
	// (set) Token: 0x06007017 RID: 28695 RVA: 0x002C69C0 File Offset: 0x002C4BC0
	public Vector2 EnemyTargetPos { get; set; }

	// Token: 0x17001103 RID: 4355
	// (get) Token: 0x06007018 RID: 28696 RVA: 0x002C69CC File Offset: 0x002C4BCC
	// (set) Token: 0x06007019 RID: 28697 RVA: 0x002C69D4 File Offset: 0x002C4BD4
	public float EnemySpeed { get; set; }

	// Token: 0x17001104 RID: 4356
	// (get) Token: 0x0600701A RID: 28698 RVA: 0x002C69E0 File Offset: 0x002C4BE0
	// (set) Token: 0x0600701B RID: 28699 RVA: 0x002C69E8 File Offset: 0x002C4BE8
	public DraGunController Parent { get; set; }

	// Token: 0x17001105 RID: 4357
	// (get) Token: 0x0600701C RID: 28700 RVA: 0x002C69F4 File Offset: 0x002C4BF4
	// (set) Token: 0x0600701D RID: 28701 RVA: 0x002C69FC File Offset: 0x002C4BFC
	public AutoAimTarget ParentHeart { get; set; }

	// Token: 0x0600701E RID: 28702 RVA: 0x002C6A08 File Offset: 0x002C4C08
	private void Start()
	{
		this.m_srb = base.GetComponent<SpeculativeRigidbody>();
		SpeculativeRigidbody srb = this.m_srb;
		srb.OnPostRigidbodyMovement = (Action<SpeculativeRigidbody, Vector2, IntVector2>)Delegate.Combine(srb.OnPostRigidbodyMovement, new Action<SpeculativeRigidbody, Vector2, IntVector2>(this.OnPostRigidbodyMovement));
		this.m_lastBasePosition = base.transform.position.XY();
		this.m_path = new PooledLinkedList<Vector2>();
		this.m_path.AddLast(this.m_lastBasePosition);
		this.m_path.AddLast(this.m_lastBasePosition);
		float num = 0f;
		for (int i = 0; i < this.Segments.Count; i++)
		{
			BabyDragunSegment babyDragunSegment = default(BabyDragunSegment);
			babyDragunSegment.lastPosition = this.Segments[i].position.XY();
			babyDragunSegment.initialStartingDistance = ((i != 0) ? (this.m_segmentData[this.m_segmentData.Count - 1].lastPosition - babyDragunSegment.lastPosition).magnitude : (this.m_lastBasePosition - babyDragunSegment.lastPosition).magnitude);
			babyDragunSegment.pathDist = num;
			num += babyDragunSegment.initialStartingDistance;
			this.m_segmentData.Add(babyDragunSegment);
			this.SegmentSprites.Add(this.Segments[i].GetComponent<tk2dSprite>());
		}
	}

	// Token: 0x0600701F RID: 28703 RVA: 0x002C6B70 File Offset: 0x002C4D70
	public void Update()
	{
		if (this.IsEnemy && BraveTime.DeltaTime > 0f)
		{
			Vector2 enemyTargetPos = this.EnemyTargetPos;
			if (this.Parent.head && this.Parent.head.transform.position.x + 5f > enemyTargetPos.x)
			{
				enemyTargetPos.x = this.Parent.head.transform.position.x + 5f;
			}
			Vector2 vector = enemyTargetPos + new Vector2(1f, 0f).Rotate(Time.realtimeSinceStartup / 3f * 360f).Scale(3f, 1.5f);
			if (this.ParentHeart.enabled)
			{
				if (this.m_concernTimer <= 0f)
				{
					this.EnemySpeed = this.EnemyFastSpeed;
					this.EnemyTargetPos += new Vector2(-5.5f, -3.4f);
					this.m_behaviorSpeculator.InterruptAndDisable();
				}
				this.m_concernTimer += BraveTime.DeltaTime;
				vector = this.EnemyTargetPos + new Vector2(1.5f, 0f).Rotate(Time.realtimeSinceStartup / 2f * -360f);
			}
			Vector2 vector2 = vector - base.transform.position.XY();
			if (vector2.magnitude < this.EnemySpeed * BraveTime.DeltaTime)
			{
				this.m_srb.Velocity = vector2 / BraveTime.DeltaTime;
			}
			else
			{
				this.m_srb.Velocity = vector2.normalized * this.EnemySpeed;
			}
		}
	}

	// Token: 0x06007020 RID: 28704 RVA: 0x002C6D4C File Offset: 0x002C4F4C
	private void UpdatePath(Vector2 newPosition)
	{
		float num = Vector2.Distance(newPosition, this.m_path.First.Value);
		for (int i = 0; i < this.m_segmentData.Count; i++)
		{
			BabyDragunSegment babyDragunSegment = this.m_segmentData[i];
			babyDragunSegment.pathDist += num;
			this.m_segmentData[i] = babyDragunSegment;
		}
		if (this.m_pathSegmentLength > 0.05f)
		{
			this.m_path.AddFirst(newPosition);
		}
		else
		{
			this.m_path.First.Value = newPosition;
		}
		this.m_pathSegmentLength = Vector2.Distance(this.m_path.First.Value, this.m_path.First.Next.Value);
	}

	// Token: 0x06007021 RID: 28705 RVA: 0x002C6E18 File Offset: 0x002C5018
	private float GetPerp(float totalPathDist, int segmentIndex)
	{
		float num = Mathf.Sin(totalPathDist * this.SinWave1Multiplier + Time.time * this.SinTimeMultiplier) * this.SinAmplitude;
		float num2 = 1f - (float)segmentIndex / (1f * (float)this.Segments.Count);
		return Mathf.Lerp(0f, num, num2);
	}

	// Token: 0x06007022 RID: 28706 RVA: 0x002C6E74 File Offset: 0x002C5074
	private void UpdatePathSegment(LinkedListNode<Vector2> pathNode, float totalPathDist, float thisNodeDist, int segmentIndex)
	{
		BabyDragunSegment babyDragunSegment = this.m_segmentData[segmentIndex];
		float num = ((segmentIndex > 0) ? this.m_segmentData[segmentIndex - 1].pathDist : 0f);
		Transform transform = this.Segments[segmentIndex];
		float num2 = babyDragunSegment.pathDist - num;
		float num3 = babyDragunSegment.initialStartingDistance;
		bool flag = false;
		if (num2 < num3)
		{
			num3 = num2;
		}
		float num4 = -thisNodeDist;
		while (pathNode.Next != null)
		{
			float num5 = Vector2.Distance(pathNode.Next.Value, pathNode.Value);
			if (num4 + num5 >= num3)
			{
				float num6 = num3 - num4;
				if (!flag)
				{
					Vector2 vector = Vector2.Lerp(pathNode.Value, pathNode.Next.Value, num6 / num5);
					transform.position = vector;
					this.SegmentSprites[segmentIndex].UpdateZDepth();
				}
				babyDragunSegment.pathDist = totalPathDist + num3;
				if (segmentIndex + 1 < this.m_segmentData.Count)
				{
					this.UpdatePathSegment(pathNode, totalPathDist + num3, num6, segmentIndex + 1);
				}
				else
				{
					while (this.m_path.Last != pathNode.Next)
					{
						this.m_path.RemoveLast();
					}
				}
				this.m_segmentData[segmentIndex] = babyDragunSegment;
				return;
			}
			num4 += num5;
			pathNode = pathNode.Next;
		}
		this.m_segmentData[segmentIndex] = babyDragunSegment;
	}

	// Token: 0x06007023 RID: 28707 RVA: 0x002C6FF4 File Offset: 0x002C51F4
	public void OnPostRigidbodyMovement(SpeculativeRigidbody s, Vector2 v, IntVector2 iv)
	{
		if (!base.enabled)
		{
			return;
		}
		Vector2 vector = base.transform.position.XY();
		this.UpdatePath(vector);
		this.UpdatePathSegment(this.m_path.First, 0f, 0f, 0);
		this.m_lastBasePosition = vector;
		this.m_lastVelocityAvg = BraveMathCollege.MovingAverage(this.m_lastVelocityAvg, this.m_srb.Velocity, 8);
		if (float.IsNaN(this.m_lastVelocityAvg.x) || float.IsNaN(this.m_lastVelocityAvg.y))
		{
			this.m_lastVelocityAvg = Vector2.zero;
		}
		this.HeadAnimator.LockFacingDirection = true;
		if (Time.frameCount - this.m_lastChangedFacingFrame >= 3)
		{
			this.HeadAnimator.FacingDirection = this.m_lastVelocityAvg.ToAngle();
			this.m_lastChangedFacingFrame = Time.frameCount;
		}
	}

	// Token: 0x06007024 RID: 28708 RVA: 0x002C70DC File Offset: 0x002C52DC
	public void BecomeEnemy(DraGunController draGunController)
	{
		if (this.IsEnemy)
		{
			return;
		}
		PlayerOrbital component = base.GetComponent<PlayerOrbital>();
		component.DecoupleBabyDragun();
		this.m_srb.CollideWithOthers = false;
		this.IsEnemy = true;
		this.EnemyTargetPos = draGunController.transform.position + new Vector3(10f, 6f);
		this.EnemySpeed = this.EnemyBaseSpeed;
		this.Parent = UnityEngine.Object.FindObjectOfType<DraGunController>();
		this.ParentHeart = this.Parent.GetComponentsInChildren<AutoAimTarget>(true)[0];
		tk2dBaseSprite[] componentsInChildren = base.GetComponentsInChildren<tk2dBaseSprite>();
		foreach (tk2dBaseSprite tk2dBaseSprite in componentsInChildren)
		{
			if (!tk2dBaseSprite.IsOutlineSprite)
			{
				tk2dBaseSprite.HeightOffGround += 1f;
			}
		}
		this.m_bulletBank = base.GetComponent<AIBulletBank>();
		this.m_bulletBank.ActorName = this.Parent.aiActor.GetActorName();
		this.m_bulletBank.enabled = true;
		this.m_behaviorSpeculator = base.GetComponent<BehaviorSpeculator>();
		this.m_behaviorSpeculator.enabled = true;
		this.m_behaviorSpeculator.AttackCooldown = 5f;
	}

	// Token: 0x04006F62 RID: 28514
	public List<Transform> Segments;

	// Token: 0x04006F63 RID: 28515
	private List<tk2dSprite> SegmentSprites = new List<tk2dSprite>();

	// Token: 0x04006F64 RID: 28516
	public AIAnimator HeadAnimator;

	// Token: 0x04006F65 RID: 28517
	private SpeculativeRigidbody m_srb;

	// Token: 0x04006F66 RID: 28518
	private Vector2 m_lastBasePosition;

	// Token: 0x04006F67 RID: 28519
	private Vector2 m_lastVelocityAvg;

	// Token: 0x04006F68 RID: 28520
	private List<BabyDragunSegment> m_segmentData = new List<BabyDragunSegment>();

	// Token: 0x04006F69 RID: 28521
	public float SinWave1Multiplier = 1f;

	// Token: 0x04006F6A RID: 28522
	public float SinTimeMultiplier = 1f;

	// Token: 0x04006F6B RID: 28523
	public float SinAmplitude = 0.5f;

	// Token: 0x04006F6C RID: 28524
	[Header("Enemy Stats")]
	public float EnemyBaseSpeed = 4.5f;

	// Token: 0x04006F6D RID: 28525
	public float EnemyFastSpeed = 6f;

	// Token: 0x04006F73 RID: 28531
	private float m_concernTimer;

	// Token: 0x04006F74 RID: 28532
	private PooledLinkedList<Vector2> m_path;

	// Token: 0x04006F75 RID: 28533
	private int m_lastChangedFacingFrame = -100;

	// Token: 0x04006F76 RID: 28534
	private float m_pathSegmentLength;

	// Token: 0x04006F77 RID: 28535
	private AIBulletBank m_bulletBank;

	// Token: 0x04006F78 RID: 28536
	private BehaviorSpeculator m_behaviorSpeculator;
}
