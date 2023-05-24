using System;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020015BA RID: 5562
public class PathMover : BraveBehaviour
{
	// Token: 0x170012F1 RID: 4849
	// (get) Token: 0x06007FB0 RID: 32688 RVA: 0x00338D70 File Offset: 0x00336F70
	public float AbsPathSpeed
	{
		get
		{
			return Mathf.Abs(this.PathSpeed);
		}
	}

	// Token: 0x170012F2 RID: 4850
	// (get) Token: 0x06007FB1 RID: 32689 RVA: 0x00338D80 File Offset: 0x00336F80
	// (set) Token: 0x06007FB2 RID: 32690 RVA: 0x00338D88 File Offset: 0x00336F88
	public float PathSpeed
	{
		get
		{
			return this.m_pathSpeed;
		}
		set
		{
			bool flag = false;
			if (Mathf.Sign(value) != Mathf.Sign(this.m_pathSpeed))
			{
				flag = true;
			}
			this.m_pathSpeed = value;
			if (flag)
			{
				this.HandleDirectionFlip();
			}
		}
	}

	// Token: 0x170012F3 RID: 4851
	// (get) Token: 0x06007FB3 RID: 32691 RVA: 0x00338DC4 File Offset: 0x00336FC4
	public int PreviousIndex
	{
		get
		{
			return (this.m_currentIndex + this.m_currentIndexDelta * -1 + this.Path.nodes.Count) % this.Path.nodes.Count;
		}
	}

	// Token: 0x170012F4 RID: 4852
	// (get) Token: 0x06007FB4 RID: 32692 RVA: 0x00338DF8 File Offset: 0x00336FF8
	public int CurrentIndex
	{
		get
		{
			return this.m_currentIndex;
		}
	}

	// Token: 0x06007FB5 RID: 32693 RVA: 0x00338E00 File Offset: 0x00337000
	public static SerializedPath GetRoomPath(RoomHandler room, int pathIndex)
	{
		return room.area.runtimePrototypeData.paths[pathIndex];
	}

	// Token: 0x06007FB6 RID: 32694 RVA: 0x00338E18 File Offset: 0x00337018
	private void Awake()
	{
		this.m_pathSpeed = this.OriginalPathSpeed;
	}

	// Token: 0x06007FB7 RID: 32695 RVA: 0x00338E28 File Offset: 0x00337028
	public void Start()
	{
		if (this.Path == null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (this.Path != null && this.Path.overrideSpeed != -1f)
		{
			this.PathSpeed = this.Path.overrideSpeed;
		}
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPathTargetReached = (Action)Delegate.Combine(specRigidbody.OnPathTargetReached, new Action(this.PathTargetReached));
		this.m_currentIndex = this.PathStartNode;
		if (base.talkDoer)
		{
			this.Paused = true;
			this.PathToNextNode();
		}
		else
		{
			base.transform.position = this.RoomHandler.area.basePosition.ToVector2() + this.Path.nodes[this.PathStartNode].RoomPosition + this.nodeOffset;
			base.specRigidbody.Reinitialize();
			this.PathTargetReached();
		}
	}

	// Token: 0x06007FB8 RID: 32696 RVA: 0x00338F34 File Offset: 0x00337134
	public void Update()
	{
		base.specRigidbody.PathSpeed = ((!this.Paused) ? Mathf.Abs(this.PathSpeed) : 0f);
	}

	// Token: 0x06007FB9 RID: 32697 RVA: 0x00338F64 File Offset: 0x00337164
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06007FBA RID: 32698 RVA: 0x00338F6C File Offset: 0x0033716C
	protected float GetTotalPathLength()
	{
		float num = 0f;
		for (int i = 0; i < this.Path.nodes.Count - 1; i++)
		{
			num += (this.Path.nodes[i + 1].RoomPosition - this.Path.nodes[i].RoomPosition).magnitude;
		}
		if (this.Path.wrapMode == SerializedPath.SerializedPathWrapMode.Loop)
		{
			num += (this.Path.nodes[0].RoomPosition - this.Path.nodes[this.Path.nodes.Count - 1].RoomPosition).magnitude;
		}
		return num;
	}

	// Token: 0x06007FBB RID: 32699 RVA: 0x00339050 File Offset: 0x00337250
	public Vector2 GetPositionOfNode(int nodeIndex)
	{
		return this.Path.nodes[nodeIndex].RoomPosition + this.RoomHandler.area.basePosition.ToVector2() + this.nodeOffset;
	}

	// Token: 0x06007FBC RID: 32700 RVA: 0x0033909C File Offset: 0x0033729C
	public float GetParametrizedPathPosition()
	{
		float totalPathLength = this.GetTotalPathLength();
		float num = 0f;
		if (this.PathSpeed >= 0f)
		{
			int num2 = ((this.m_currentIndex != 0) ? this.m_currentIndex : this.Path.nodes.Count);
			for (int i = 0; i < num2 - 1; i++)
			{
				num += (this.Path.nodes[i + 1].RoomPosition - this.Path.nodes[i].RoomPosition).magnitude;
			}
			int num3 = (this.m_currentIndex + this.Path.nodes.Count - 1) % this.Path.nodes.Count;
			num += Vector2.Distance(this.Path.nodes[num3].RoomPosition + this.RoomHandler.area.basePosition.ToVector2() + this.nodeOffset, base.specRigidbody.Position.UnitPosition);
		}
		else
		{
			for (int j = 0; j < this.m_currentIndex; j++)
			{
				num += (this.Path.nodes[j + 1].RoomPosition - this.Path.nodes[j].RoomPosition).magnitude;
			}
			num += Vector2.Distance(this.Path.nodes[this.m_currentIndex].RoomPosition + this.RoomHandler.area.basePosition.ToVector2() + this.nodeOffset, base.specRigidbody.Position.UnitPosition);
		}
		return num / totalPathLength;
	}

	// Token: 0x06007FBD RID: 32701 RVA: 0x0033929C File Offset: 0x0033749C
	private void PathTargetReached()
	{
		if (this.ForceCornerDelayHack && Vector2.Distance(this.Path.nodes[this.m_currentIndex].RoomPosition + this.RoomHandler.area.basePosition.ToVector2() + this.nodeOffset, base.specRigidbody.Position.UnitPosition) > 0.1f)
		{
			base.specRigidbody.Velocity = Vector2.zero;
			return;
		}
		SerializedPathNode serializedPathNode = this.Path.nodes[this.m_currentIndex];
		Vector2 vector = this.Path.nodes[(this.m_currentIndex - 1 + this.Path.nodes.Count) % this.Path.nodes.Count].position.ToVector2();
		Vector2 vector2 = this.Path.nodes[this.m_currentIndex].position.ToVector2();
		bool flag = this.UpdatePathIndex();
		if (this.OnNodeReached != null)
		{
			bool flag2 = flag;
			Vector2 vector3 = Vector2.zero;
			if (flag2)
			{
				vector3 = this.Path.nodes[this.m_currentIndex].position.ToVector2();
			}
			if (this.prevMotionVec == Vector2.zero)
			{
				this.prevMotionVec = vector2 - vector;
			}
			Vector2 vector4 = vector3 - vector2;
			this.OnNodeReached(this.prevMotionVec, vector4, flag2);
			if (vector4 != Vector2.zero)
			{
				this.prevMotionVec = vector3 - vector2;
			}
		}
		if (!flag)
		{
			base.specRigidbody.PathMode = false;
			base.specRigidbody.Velocity = Vector2.zero;
		}
		else if (serializedPathNode.delayTime == 0f && this.AdditionalNodeDelay == 0f)
		{
			this.PathToNextNode();
		}
		else
		{
			base.specRigidbody.PathMode = false;
			base.specRigidbody.Velocity = Vector2.zero;
			base.Invoke("PathToNextNode", serializedPathNode.delayTime + this.AdditionalNodeDelay);
		}
	}

	// Token: 0x06007FBE RID: 32702 RVA: 0x003394E8 File Offset: 0x003376E8
	public void WarpToStart()
	{
		this.m_currentIndex = this.PathStartNode;
		base.transform.position = this.RoomHandler.area.basePosition.ToVector2() + this.Path.nodes[this.PathStartNode].RoomPosition + this.nodeOffset;
		base.specRigidbody.Reinitialize();
		this.PathTargetReached();
	}

	// Token: 0x06007FBF RID: 32703 RVA: 0x00339568 File Offset: 0x00337768
	public void WarpToNearestPoint(Vector2 targetPoint)
	{
		Vector2 vector = Vector2.zero;
		float num = float.MaxValue;
		int num2 = -1;
		for (int i = 0; i < this.Path.nodes.Count - 1; i++)
		{
			Vector2 vector2 = this.Path.nodes[i].RoomPosition + this.nodeOffset + this.RoomHandler.area.basePosition.ToVector2();
			Vector2 vector3 = this.Path.nodes[i + 1].RoomPosition + this.nodeOffset + this.RoomHandler.area.basePosition.ToVector2();
			Vector2 vector4 = BraveMathCollege.ClosestPointOnLineSegment(targetPoint, vector2, vector3);
			float num3 = Vector2.Distance(vector4, targetPoint);
			if (num3 < 1f)
			{
				Vector2 vector5 = ((Vector2.Distance(vector2, vector4) >= Vector2.Distance(vector3, vector4)) ? vector2 : vector3);
				Vector2 vector6 = vector5 - vector4;
				if (vector6.magnitude > 1f)
				{
					vector4 += vector6.normalized;
				}
			}
			if (num3 < num)
			{
				num = num3;
				num2 = i + 1;
				vector = vector4;
			}
		}
		if (this.Path.wrapMode == SerializedPath.SerializedPathWrapMode.Loop)
		{
			Vector2 vector7 = this.Path.nodes[this.Path.nodes.Count - 1].RoomPosition + this.nodeOffset + this.RoomHandler.area.basePosition.ToVector2();
			Vector2 vector8 = this.Path.nodes[0].RoomPosition + this.nodeOffset + this.RoomHandler.area.basePosition.ToVector2();
			Vector2 vector9 = BraveMathCollege.ClosestPointOnLineSegment(targetPoint, vector7, vector8);
			float num4 = Vector2.Distance(vector9, targetPoint);
			if (num4 < 1f)
			{
				Vector2 vector10 = ((Vector2.Distance(vector7, vector9) >= Vector2.Distance(vector8, vector9)) ? vector7 : vector8);
				Vector2 vector11 = vector10 - vector9;
				if (vector11.magnitude > 1f)
				{
					vector9 += vector11.normalized;
				}
			}
			if (num4 < num)
			{
				num2 = 0;
				vector = vector9;
			}
		}
		this.m_currentIndex = num2;
		base.transform.position = vector.ToVector3ZUp(0f);
		base.specRigidbody.Reinitialize();
		this.PathToNextNode();
	}

	// Token: 0x06007FC0 RID: 32704 RVA: 0x00339800 File Offset: 0x00337A00
	public void ForcePathToNextNode()
	{
		this.PathToNextNode();
	}

	// Token: 0x06007FC1 RID: 32705 RVA: 0x00339808 File Offset: 0x00337A08
	protected void HandleDirectionFlip()
	{
		this.m_currentIndexDelta *= -1;
		switch (this.Path.wrapMode)
		{
		case SerializedPath.SerializedPathWrapMode.PingPong:
			if (this.m_currentIndex + this.m_currentIndexDelta < 0 || this.m_currentIndex + this.m_currentIndexDelta >= this.Path.nodes.Count)
			{
				this.m_currentIndexDelta *= -1;
			}
			this.m_currentIndex += this.m_currentIndexDelta;
			break;
		case SerializedPath.SerializedPathWrapMode.Loop:
			this.m_currentIndex = (this.m_currentIndex + this.Path.nodes.Count + this.m_currentIndexDelta) % this.Path.nodes.Count;
			break;
		case SerializedPath.SerializedPathWrapMode.Once:
			this.m_currentIndex = (this.m_currentIndex + this.Path.nodes.Count + this.m_currentIndexDelta) % this.Path.nodes.Count;
			break;
		default:
			this.m_currentIndex = (this.m_currentIndex + this.Path.nodes.Count + this.m_currentIndexDelta) % this.Path.nodes.Count;
			break;
		}
		this.PathToNextNode();
	}

	// Token: 0x06007FC2 RID: 32706 RVA: 0x00339958 File Offset: 0x00337B58
	private void PathToNextNode()
	{
		SerializedPathNode serializedPathNode = this.Path.nodes[this.m_currentIndex];
		base.specRigidbody.PathMode = true;
		base.specRigidbody.PathTarget = PhysicsEngine.UnitToPixel(this.RoomHandler.area.basePosition.ToVector2() + this.nodeOffset + serializedPathNode.RoomPosition);
		base.specRigidbody.PathSpeed = ((!this.Paused) ? Mathf.Abs(this.PathSpeed) : 0f);
	}

	// Token: 0x06007FC3 RID: 32707 RVA: 0x003399F0 File Offset: 0x00337BF0
	public Vector2 GetCurrentTargetPosition()
	{
		return this.GetPositionOfNode(this.m_currentIndex);
	}

	// Token: 0x06007FC4 RID: 32708 RVA: 0x00339A00 File Offset: 0x00337C00
	public Vector2 GetPreviousSourcePosition()
	{
		return this.GetPositionOfNode((this.m_currentIndex + this.m_currentIndexDelta * -2 + this.Path.nodes.Count) % this.Path.nodes.Count);
	}

	// Token: 0x06007FC5 RID: 32709 RVA: 0x00339A3C File Offset: 0x00337C3C
	public Vector2 GetPreviousTargetPosition()
	{
		return this.GetPositionOfNode((this.m_currentIndex + this.m_currentIndexDelta * -1 + this.Path.nodes.Count) % this.Path.nodes.Count);
	}

	// Token: 0x06007FC6 RID: 32710 RVA: 0x00339A78 File Offset: 0x00337C78
	public Vector2 GetNextTargetPosition()
	{
		return this.GetNextTargetRoomPosition() + this.nodeOffset + this.RoomHandler.area.basePosition.ToVector2();
	}

	// Token: 0x06007FC7 RID: 32711 RVA: 0x00339AA8 File Offset: 0x00337CA8
	private Vector2 GetNextTargetRoomPosition()
	{
		SerializedPath serializedPath = this.Path;
		int i = this.m_currentIndex;
		if (this.IsUsingAlternateTargets && this.Path.nodes[this.m_currentIndex].UsesAlternateTarget)
		{
			SerializedPathNode serializedPathNode = this.Path.nodes[this.m_currentIndex];
			serializedPath = PathMover.GetRoomPath(this.RoomHandler, serializedPathNode.AlternateTargetPathIndex);
			i = serializedPathNode.AlternateTargetNodeIndex;
			return serializedPath.nodes[i].RoomPosition;
		}
		if (this.Path.wrapMode == SerializedPath.SerializedPathWrapMode.Once)
		{
			i += this.m_currentIndexDelta;
			if (i >= this.Path.nodes.Count)
			{
				if (this.m_currentIndex >= this.Path.nodes.Count)
				{
					this.m_currentIndex = 0;
				}
				return this.Path.nodes[this.m_currentIndex].RoomPosition;
			}
		}
		else if (this.Path.wrapMode == SerializedPath.SerializedPathWrapMode.Loop)
		{
			i = (i + this.Path.nodes.Count + this.m_currentIndexDelta) % this.Path.nodes.Count;
		}
		else
		{
			if (this.Path.wrapMode != SerializedPath.SerializedPathWrapMode.PingPong)
			{
				Debug.LogError("Unsupported path type " + this.Path.wrapMode);
				return this.Path.nodes[this.m_currentIndex].RoomPosition;
			}
			i += this.m_currentIndexDelta;
			if (i < 0 || i >= this.Path.nodes.Count)
			{
				this.m_currentIndexDelta *= -1;
				i += this.m_currentIndexDelta * 2;
			}
		}
		while (i < 0)
		{
			i += this.Path.nodes.Count;
		}
		i %= this.Path.nodes.Count;
		return this.Path.nodes[i].RoomPosition;
	}

	// Token: 0x06007FC8 RID: 32712 RVA: 0x00339CD0 File Offset: 0x00337ED0
	private bool UpdatePathIndex()
	{
		if (this.IsUsingAlternateTargets && this.Path.nodes[this.m_currentIndex].UsesAlternateTarget)
		{
			SerializedPathNode serializedPathNode = this.Path.nodes[this.m_currentIndex];
			this.Path = PathMover.GetRoomPath(this.RoomHandler, serializedPathNode.AlternateTargetPathIndex);
			this.m_currentIndex = serializedPathNode.AlternateTargetNodeIndex;
			return true;
		}
		if (this.Path.wrapMode == SerializedPath.SerializedPathWrapMode.Once)
		{
			this.m_currentIndex += this.m_currentIndexDelta;
			if (this.m_currentIndex >= this.Path.nodes.Count || this.m_currentIndex < 0)
			{
				return false;
			}
		}
		else if (this.Path.wrapMode == SerializedPath.SerializedPathWrapMode.Loop)
		{
			this.m_currentIndex = (this.m_currentIndex + this.m_currentIndexDelta + this.Path.nodes.Count) % this.Path.nodes.Count;
		}
		else
		{
			if (this.Path.wrapMode != SerializedPath.SerializedPathWrapMode.PingPong)
			{
				Debug.LogError("Unsupported path type " + this.Path.wrapMode);
				return false;
			}
			if (this.m_currentIndex == 0)
			{
				this.m_currentIndex = 1;
				this.m_currentIndexDelta = 1;
			}
			else if (this.m_currentIndex == this.Path.nodes.Count - 1)
			{
				this.m_currentIndex = this.Path.nodes.Count - 2;
				this.m_currentIndexDelta = -1;
			}
			else
			{
				this.m_currentIndex += this.m_currentIndexDelta;
				if (this.m_currentIndex < 0 || this.m_currentIndex >= this.Path.nodes.Count)
				{
					this.m_currentIndexDelta *= -1;
					this.m_currentIndex += this.m_currentIndexDelta * 2;
				}
			}
		}
		return true;
	}

	// Token: 0x04008244 RID: 33348
	public bool Paused;

	// Token: 0x04008245 RID: 33349
	[NonSerialized]
	protected float m_pathSpeed = 1f;

	// Token: 0x04008246 RID: 33350
	[FormerlySerializedAs("PathSpeed")]
	public float OriginalPathSpeed = 1f;

	// Token: 0x04008247 RID: 33351
	public float AdditionalNodeDelay;

	// Token: 0x04008248 RID: 33352
	[NonSerialized]
	public Vector2 nodeOffset;

	// Token: 0x04008249 RID: 33353
	[NonSerialized]
	public SerializedPath Path;

	// Token: 0x0400824A RID: 33354
	[NonSerialized]
	public RoomHandler RoomHandler;

	// Token: 0x0400824B RID: 33355
	[NonSerialized]
	public int PathStartNode;

	// Token: 0x0400824C RID: 33356
	[NonSerialized]
	public bool IsUsingAlternateTargets;

	// Token: 0x0400824D RID: 33357
	[NonSerialized]
	public bool ForceCornerDelayHack;

	// Token: 0x0400824E RID: 33358
	public Action<Vector2, Vector2, bool> OnNodeReached;

	// Token: 0x0400824F RID: 33359
	private Vector2 prevMotionVec = Vector2.zero;

	// Token: 0x04008250 RID: 33360
	private int m_currentIndex;

	// Token: 0x04008251 RID: 33361
	private int m_currentIndexDelta = 1;
}
