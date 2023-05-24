using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace BraveDynamicTree
{
	// Token: 0x02000359 RID: 857
	public class b2DynamicTree : RigidbodyContainer
	{
		// Token: 0x06000D7D RID: 3453 RVA: 0x0003F3FC File Offset: 0x0003D5FC
		public b2DynamicTree()
		{
			this.m_root = -1;
			this.m_nodeCapacity = 16;
			this.m_nodeCount = 0;
			this.m_nodes = new b2TreeNode[this.m_nodeCapacity];
			for (int i = 0; i < this.m_nodeCapacity - 1; i++)
			{
				this.m_nodes[i] = new b2TreeNode();
				this.m_nodes[i].next = i + 1;
				this.m_nodes[i].height = -1;
			}
			this.m_nodes[this.m_nodeCapacity - 1] = new b2TreeNode();
			this.m_nodes[this.m_nodeCapacity - 1].next = -1;
			this.m_nodes[this.m_nodeCapacity - 1].height = -1;
			this.m_freeList = 0;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0003F4D0 File Offset: 0x0003D6D0
		[Conditional("BRAVE_INTERNAL")]
		public static void b2Assert(bool mustBeTrue)
		{
			if (!mustBeTrue)
			{
				throw new Exception("Failed assert in b2DynamicTree!");
			}
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0003F4E4 File Offset: 0x0003D6E4
		public int CreateProxy(b2AABB aabb, SpeculativeRigidbody rigidbody)
		{
			int num = this.AllocateNode();
			Vector2 vector = new Vector2(0.1f, 0.1f);
			this.m_nodes[num].fatAabb.lowerBound = aabb.lowerBound - vector;
			this.m_nodes[num].fatAabb.upperBound = aabb.upperBound + vector;
			this.m_nodes[num].tightAabb = this.m_nodes[num].fatAabb;
			this.m_nodes[num].rigidbody = rigidbody;
			this.m_nodes[num].height = 0;
			this.InsertLeaf(num);
			return num;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0003F588 File Offset: 0x0003D788
		public void DestroyProxy(int proxyId)
		{
			this.RemoveLeaf(proxyId);
			this.FreeNode(proxyId);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0003F598 File Offset: 0x0003D798
		public bool MoveProxy(int proxyId, b2AABB aabb, Vector2 displacement)
		{
			float num = aabb.lowerBound.x - 0.1f;
			float num2 = aabb.lowerBound.y - 0.1f;
			float num3 = aabb.upperBound.x + 0.1f;
			float num4 = aabb.upperBound.y + 0.1f;
			this.m_nodes[proxyId].tightAabb = new b2AABB(num, num2, num3, num4);
			if (this.m_nodes[proxyId].fatAabb.Contains(aabb))
			{
				return false;
			}
			this.RemoveLeaf(proxyId);
			Vector2 vector = 2f * displacement;
			if (vector.x < 0f)
			{
				num += vector.x;
			}
			else
			{
				num3 += vector.x;
			}
			if (vector.y < 0f)
			{
				num2 += vector.y;
			}
			else
			{
				num4 += vector.y;
			}
			this.m_nodes[proxyId].fatAabb = new b2AABB(num, num2, num3, num4);
			this.InsertLeaf(proxyId);
			return true;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0003F6A8 File Offset: 0x0003D8A8
		public SpeculativeRigidbody GetSpeculativeRigidbody(int proxyId)
		{
			return this.m_nodes[proxyId].rigidbody;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0003F6B8 File Offset: 0x0003D8B8
		public b2AABB GetFatAABB(int proxyId)
		{
			return this.m_nodes[proxyId].fatAabb;
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0003F6C8 File Offset: 0x0003D8C8
		public void Query(b2AABB aabb, Func<SpeculativeRigidbody, bool> callback)
		{
			this.m_stack.Clear();
			this.m_stack.Push(this.m_root);
			while (this.m_stack.Count > 0)
			{
				int num = this.m_stack.Pop();
				if (num != -1)
				{
					b2TreeNode b2TreeNode = this.m_nodes[num];
					b2AABB fatAabb = b2TreeNode.fatAabb;
					if (aabb.lowerBound.x <= fatAabb.upperBound.x && fatAabb.lowerBound.x <= aabb.upperBound.x && aabb.lowerBound.y <= fatAabb.upperBound.y && fatAabb.lowerBound.y <= aabb.upperBound.y)
					{
						if (b2TreeNode.child1 == -1)
						{
							b2AABB tightAabb = b2TreeNode.tightAabb;
							if (aabb.lowerBound.x <= tightAabb.upperBound.x && tightAabb.lowerBound.x <= aabb.upperBound.x && aabb.lowerBound.y <= tightAabb.upperBound.y && tightAabb.lowerBound.y <= aabb.upperBound.y && !callback(b2TreeNode.rigidbody))
							{
								return;
							}
						}
						else
						{
							this.m_stack.Push(b2TreeNode.child1);
							this.m_stack.Push(b2TreeNode.child2);
						}
					}
				}
			}
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0003F864 File Offset: 0x0003DA64
		public void RayCast(b2RayCastInput input, Func<b2RayCastInput, SpeculativeRigidbody, float> callback)
		{
			Vector2 p = input.p1;
			Vector2 p2 = input.p2;
			Vector2 vector = p2 - p;
			if ((double)vector.sqrMagnitude <= 0.0)
			{
				return;
			}
			vector.Normalize();
			Vector2 vector2 = Vector2Extensions.Cross(1f, vector);
			Vector2 vector3 = vector2.Abs();
			float num = input.maxFraction;
			Vector2 vector4 = p + num * (p2 - p);
			b2AABB b2AABB;
			b2AABB.lowerBound = Vector2.Min(p, vector4);
			b2AABB.upperBound = Vector2.Max(p, vector4);
			this.m_stack.Clear();
			this.m_stack.Push(this.m_root);
			while (this.m_stack.Count > 0)
			{
				int num2 = this.m_stack.Pop();
				if (num2 != -1)
				{
					b2TreeNode b2TreeNode = this.m_nodes[num2];
					if (b2AABB.b2TestOverlap(ref b2TreeNode.fatAabb, ref b2AABB))
					{
						Vector2 center = b2TreeNode.fatAabb.GetCenter();
						Vector2 extents = b2TreeNode.fatAabb.GetExtents();
						float num3 = Mathf.Abs(Vector2.Dot(vector2, p - center)) - Vector2.Dot(vector3, extents);
						if (num3 <= 0f)
						{
							if (b2TreeNode.IsLeaf())
							{
								b2RayCastInput b2RayCastInput;
								b2RayCastInput.p1 = input.p1;
								b2RayCastInput.p2 = input.p2;
								b2RayCastInput.maxFraction = num;
								float num4 = callback(b2RayCastInput, b2TreeNode.rigidbody);
								if (num4 == 0f)
								{
									return;
								}
								if (num4 > 0f)
								{
									num = num4;
									Vector2 vector5 = p + num * (p2 - p);
									b2AABB.lowerBound = Vector2.Min(p, vector5);
									b2AABB.upperBound = Vector2.Max(p, vector5);
								}
							}
							else
							{
								this.m_stack.Push(b2TreeNode.child1);
								this.m_stack.Push(b2TreeNode.child2);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0003FA70 File Offset: 0x0003DC70
		public void Validate()
		{
			this.ValidateStructure(this.m_root);
			this.ValidateMetrics(this.m_root);
			int num = 0;
			int num2 = this.m_freeList;
			while (num2 != -1)
			{
				num2 = this.m_nodes[num2].next;
				num++;
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0003FABC File Offset: 0x0003DCBC
		public int GetHeight()
		{
			if (this.m_root == -1)
			{
				return 0;
			}
			return this.m_nodes[this.m_root].height;
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0003FAE0 File Offset: 0x0003DCE0
		public int GetMaxBalance()
		{
			int num = 0;
			for (int i = 0; i < this.m_nodeCapacity; i++)
			{
				b2TreeNode b2TreeNode = this.m_nodes[i];
				if (b2TreeNode.height > 1)
				{
					int child = b2TreeNode.child1;
					int child2 = b2TreeNode.child2;
					int num2 = Mathf.Abs(this.m_nodes[child2].height - this.m_nodes[child].height);
					num = Mathf.Max(num, num2);
				}
			}
			return num;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0003FB5C File Offset: 0x0003DD5C
		public float GetAreaRatio()
		{
			if (this.m_root == -1)
			{
				return 0f;
			}
			b2TreeNode b2TreeNode = this.m_nodes[this.m_root];
			float perimeter = b2TreeNode.fatAabb.GetPerimeter();
			float num = 0f;
			for (int i = 0; i < this.m_nodeCapacity; i++)
			{
				b2TreeNode b2TreeNode2 = this.m_nodes[i];
				if (b2TreeNode2.height >= 0)
				{
					num += b2TreeNode2.fatAabb.GetPerimeter();
				}
			}
			return num / perimeter;
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0003FBE0 File Offset: 0x0003DDE0
		public void RebuildBottomUp()
		{
			int[] array = new int[this.m_nodeCount];
			int i = 0;
			for (int j = 0; j < this.m_nodeCapacity; j++)
			{
				if (this.m_nodes[j].height >= 0)
				{
					if (this.m_nodes[j].IsLeaf())
					{
						this.m_nodes[j].parent = -1;
						array[i] = j;
						i++;
					}
					else
					{
						this.FreeNode(j);
					}
				}
			}
			while (i > 1)
			{
				float num = float.MaxValue;
				int num2 = -1;
				int num3 = -1;
				for (int k = 0; k < i; k++)
				{
					b2AABB fatAabb = this.m_nodes[array[k]].fatAabb;
					for (int l = k + 1; l < i; l++)
					{
						b2AABB fatAabb2 = this.m_nodes[array[l]].fatAabb;
						b2AABB b2AABB = default(b2AABB);
						b2AABB.Combine(fatAabb, fatAabb2);
						float perimeter = b2AABB.GetPerimeter();
						if (perimeter < num)
						{
							num2 = k;
							num3 = l;
							num = perimeter;
						}
					}
				}
				int num4 = array[num2];
				int num5 = array[num3];
				b2TreeNode b2TreeNode = this.m_nodes[num4];
				b2TreeNode b2TreeNode2 = this.m_nodes[num5];
				int num6 = this.AllocateNode();
				b2TreeNode b2TreeNode3 = this.m_nodes[num6];
				b2TreeNode3.child1 = num4;
				b2TreeNode3.child2 = num5;
				b2TreeNode3.height = 1 + Mathf.Max(b2TreeNode.height, b2TreeNode2.height);
				b2TreeNode3.fatAabb.Combine(b2TreeNode.fatAabb, b2TreeNode2.fatAabb);
				b2TreeNode3.parent = -1;
				b2TreeNode.parent = num6;
				b2TreeNode2.parent = num6;
				array[num3] = array[i - 1];
				array[num2] = num6;
				i--;
			}
			this.m_root = array[0];
			this.Validate();
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0003FDB4 File Offset: 0x0003DFB4
		public void ShiftOrigin(Vector2 newOrigin)
		{
			for (int i = 0; i < this.m_nodeCapacity; i++)
			{
				b2TreeNode b2TreeNode = this.m_nodes[i];
				b2TreeNode.fatAabb.lowerBound = b2TreeNode.fatAabb.lowerBound - newOrigin;
				b2TreeNode b2TreeNode2 = this.m_nodes[i];
				b2TreeNode2.fatAabb.upperBound = b2TreeNode2.fatAabb.upperBound - newOrigin;
			}
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0003FE14 File Offset: 0x0003E014
		private int AllocateNode()
		{
			if (this.m_freeList == -1)
			{
				this.m_nodeCapacity *= 2;
				Array.Resize<b2TreeNode>(ref this.m_nodes, this.m_nodeCapacity);
				for (int i = this.m_nodeCount; i < this.m_nodeCapacity - 1; i++)
				{
					this.m_nodes[i] = new b2TreeNode();
					this.m_nodes[i].next = i + 1;
					this.m_nodes[i].height = -1;
				}
				this.m_nodes[this.m_nodeCapacity - 1] = new b2TreeNode();
				this.m_nodes[this.m_nodeCapacity - 1].next = -1;
				this.m_nodes[this.m_nodeCapacity - 1].height = -1;
				this.m_freeList = this.m_nodeCount;
			}
			int freeList = this.m_freeList;
			this.m_freeList = this.m_nodes[freeList].next;
			this.m_nodes[freeList].parent = -1;
			this.m_nodes[freeList].child1 = -1;
			this.m_nodes[freeList].child2 = -1;
			this.m_nodes[freeList].height = 0;
			this.m_nodes[freeList].rigidbody = null;
			this.m_nodeCount++;
			return freeList;
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0003FF50 File Offset: 0x0003E150
		private void FreeNode(int nodeId)
		{
			this.m_nodes[nodeId].next = this.m_freeList;
			this.m_nodes[nodeId].height = -1;
			this.m_freeList = nodeId;
			this.m_nodeCount--;
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0003FF88 File Offset: 0x0003E188
		private void InsertLeaf(int leaf)
		{
			if (this.m_root == -1)
			{
				this.m_root = leaf;
				this.m_nodes[this.m_root].parent = -1;
				return;
			}
			b2AABB fatAabb = this.m_nodes[leaf].fatAabb;
			int num = this.m_root;
			while (!this.m_nodes[num].IsLeaf())
			{
				int child = this.m_nodes[num].child1;
				int child2 = this.m_nodes[num].child2;
				float perimeter = this.m_nodes[num].fatAabb.GetPerimeter();
				b2AABB b2AABB = default(b2AABB);
				b2AABB.Combine(this.m_nodes[num].fatAabb, fatAabb);
				float perimeter2 = b2AABB.GetPerimeter();
				float num2 = 2f * perimeter2;
				float num3 = 2f * (perimeter2 - perimeter);
				float num4;
				if (this.m_nodes[child].IsLeaf())
				{
					b2AABB b2AABB2 = default(b2AABB);
					b2AABB2.Combine(fatAabb, this.m_nodes[child].fatAabb);
					num4 = b2AABB2.GetPerimeter() + num3;
				}
				else
				{
					b2AABB b2AABB3 = default(b2AABB);
					b2AABB3.Combine(fatAabb, this.m_nodes[child].fatAabb);
					float perimeter3 = this.m_nodes[child].fatAabb.GetPerimeter();
					float perimeter4 = b2AABB3.GetPerimeter();
					num4 = perimeter4 - perimeter3 + num3;
				}
				float num5;
				if (this.m_nodes[child2].IsLeaf())
				{
					b2AABB b2AABB4 = default(b2AABB);
					b2AABB4.Combine(fatAabb, this.m_nodes[child2].fatAabb);
					num5 = b2AABB4.GetPerimeter() + num3;
				}
				else
				{
					b2AABB b2AABB5 = default(b2AABB);
					b2AABB5.Combine(fatAabb, this.m_nodes[child2].fatAabb);
					float perimeter5 = this.m_nodes[child2].fatAabb.GetPerimeter();
					float perimeter6 = b2AABB5.GetPerimeter();
					num5 = perimeter6 - perimeter5 + num3;
				}
				if (num2 < num4 && num2 < num5)
				{
					break;
				}
				if (num4 < num5)
				{
					num = child;
				}
				else
				{
					num = child2;
				}
			}
			int num6 = num;
			int parent = this.m_nodes[num6].parent;
			int num7 = this.AllocateNode();
			this.m_nodes[num7].parent = parent;
			this.m_nodes[num7].rigidbody = null;
			this.m_nodes[num7].fatAabb.Combine(fatAabb, this.m_nodes[num6].fatAabb);
			this.m_nodes[num7].height = this.m_nodes[num6].height + 1;
			if (parent != -1)
			{
				if (this.m_nodes[parent].child1 == num6)
				{
					this.m_nodes[parent].child1 = num7;
				}
				else
				{
					this.m_nodes[parent].child2 = num7;
				}
				this.m_nodes[num7].child1 = num6;
				this.m_nodes[num7].child2 = leaf;
				this.m_nodes[num6].parent = num7;
				this.m_nodes[leaf].parent = num7;
			}
			else
			{
				this.m_nodes[num7].child1 = num6;
				this.m_nodes[num7].child2 = leaf;
				this.m_nodes[num6].parent = num7;
				this.m_nodes[leaf].parent = num7;
				this.m_root = num7;
			}
			for (num = this.m_nodes[leaf].parent; num != -1; num = this.m_nodes[num].parent)
			{
				num = this.Balance(num);
				int child3 = this.m_nodes[num].child1;
				int child4 = this.m_nodes[num].child2;
				this.m_nodes[num].height = 1 + Mathf.Max(this.m_nodes[child3].height, this.m_nodes[child4].height);
				this.m_nodes[num].fatAabb.Combine(this.m_nodes[child3].fatAabb, this.m_nodes[child4].fatAabb);
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00040378 File Offset: 0x0003E578
		private void RemoveLeaf(int leaf)
		{
			if (leaf == this.m_root)
			{
				this.m_root = -1;
				return;
			}
			int parent = this.m_nodes[leaf].parent;
			int parent2 = this.m_nodes[parent].parent;
			int num;
			if (this.m_nodes[parent].child1 == leaf)
			{
				num = this.m_nodes[parent].child2;
			}
			else
			{
				num = this.m_nodes[parent].child1;
			}
			if (parent2 != -1)
			{
				if (this.m_nodes[parent2].child1 == parent)
				{
					this.m_nodes[parent2].child1 = num;
				}
				else
				{
					this.m_nodes[parent2].child2 = num;
				}
				this.m_nodes[num].parent = parent2;
				this.FreeNode(parent);
				for (int num2 = parent2; num2 != -1; num2 = this.m_nodes[num2].parent)
				{
					num2 = this.Balance(num2);
					int child = this.m_nodes[num2].child1;
					int child2 = this.m_nodes[num2].child2;
					this.m_nodes[num2].fatAabb.Combine(this.m_nodes[child].fatAabb, this.m_nodes[child2].fatAabb);
					this.m_nodes[num2].height = 1 + Mathf.Max(this.m_nodes[child].height, this.m_nodes[child2].height);
				}
			}
			else
			{
				this.m_root = num;
				this.m_nodes[num].parent = -1;
				this.FreeNode(parent);
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x000404FC File Offset: 0x0003E6FC
		private int Balance(int iA)
		{
			b2TreeNode b2TreeNode = this.m_nodes[iA];
			if (b2TreeNode.IsLeaf() || b2TreeNode.height < 2)
			{
				return iA;
			}
			int child = b2TreeNode.child1;
			int child2 = b2TreeNode.child2;
			b2TreeNode b2TreeNode2 = this.m_nodes[child];
			b2TreeNode b2TreeNode3 = this.m_nodes[child2];
			int num = b2TreeNode3.height - b2TreeNode2.height;
			if (num > 1)
			{
				int child3 = b2TreeNode3.child1;
				int child4 = b2TreeNode3.child2;
				b2TreeNode b2TreeNode4 = this.m_nodes[child3];
				b2TreeNode b2TreeNode5 = this.m_nodes[child4];
				b2TreeNode3.child1 = iA;
				b2TreeNode3.parent = b2TreeNode.parent;
				b2TreeNode.parent = child2;
				if (b2TreeNode3.parent != -1)
				{
					if (this.m_nodes[b2TreeNode3.parent].child1 == iA)
					{
						this.m_nodes[b2TreeNode3.parent].child1 = child2;
					}
					else
					{
						this.m_nodes[b2TreeNode3.parent].child2 = child2;
					}
				}
				else
				{
					this.m_root = child2;
				}
				if (b2TreeNode4.height > b2TreeNode5.height)
				{
					b2TreeNode3.child2 = child3;
					b2TreeNode.child2 = child4;
					b2TreeNode5.parent = iA;
					b2TreeNode.fatAabb.Combine(b2TreeNode2.fatAabb, b2TreeNode5.fatAabb);
					b2TreeNode3.fatAabb.Combine(b2TreeNode.fatAabb, b2TreeNode4.fatAabb);
					b2TreeNode.height = 1 + Mathf.Max(b2TreeNode2.height, b2TreeNode5.height);
					b2TreeNode3.height = 1 + Mathf.Max(b2TreeNode.height, b2TreeNode4.height);
				}
				else
				{
					b2TreeNode3.child2 = child4;
					b2TreeNode.child2 = child3;
					b2TreeNode4.parent = iA;
					b2TreeNode.fatAabb.Combine(b2TreeNode2.fatAabb, b2TreeNode4.fatAabb);
					b2TreeNode3.fatAabb.Combine(b2TreeNode.fatAabb, b2TreeNode5.fatAabb);
					b2TreeNode.height = 1 + Mathf.Max(b2TreeNode2.height, b2TreeNode4.height);
					b2TreeNode3.height = 1 + Mathf.Max(b2TreeNode.height, b2TreeNode5.height);
				}
				return child2;
			}
			if (num < -1)
			{
				int child5 = b2TreeNode2.child1;
				int child6 = b2TreeNode2.child2;
				b2TreeNode b2TreeNode6 = this.m_nodes[child5];
				b2TreeNode b2TreeNode7 = this.m_nodes[child6];
				b2TreeNode2.child1 = iA;
				b2TreeNode2.parent = b2TreeNode.parent;
				b2TreeNode.parent = child;
				if (b2TreeNode2.parent != -1)
				{
					if (this.m_nodes[b2TreeNode2.parent].child1 == iA)
					{
						this.m_nodes[b2TreeNode2.parent].child1 = child;
					}
					else
					{
						this.m_nodes[b2TreeNode2.parent].child2 = child;
					}
				}
				else
				{
					this.m_root = child;
				}
				if (b2TreeNode6.height > b2TreeNode7.height)
				{
					b2TreeNode2.child2 = child5;
					b2TreeNode.child1 = child6;
					b2TreeNode7.parent = iA;
					b2TreeNode.fatAabb.Combine(b2TreeNode3.fatAabb, b2TreeNode7.fatAabb);
					b2TreeNode2.fatAabb.Combine(b2TreeNode.fatAabb, b2TreeNode6.fatAabb);
					b2TreeNode.height = 1 + Mathf.Max(b2TreeNode3.height, b2TreeNode7.height);
					b2TreeNode2.height = 1 + Mathf.Max(b2TreeNode.height, b2TreeNode6.height);
				}
				else
				{
					b2TreeNode2.child2 = child6;
					b2TreeNode.child1 = child5;
					b2TreeNode6.parent = iA;
					b2TreeNode.fatAabb.Combine(b2TreeNode3.fatAabb, b2TreeNode6.fatAabb);
					b2TreeNode2.fatAabb.Combine(b2TreeNode.fatAabb, b2TreeNode7.fatAabb);
					b2TreeNode.height = 1 + Mathf.Max(b2TreeNode3.height, b2TreeNode6.height);
					b2TreeNode2.height = 1 + Mathf.Max(b2TreeNode.height, b2TreeNode7.height);
				}
				return child;
			}
			return iA;
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x000408DC File Offset: 0x0003EADC
		private int ComputeHeight(int nodeId)
		{
			b2TreeNode b2TreeNode = this.m_nodes[nodeId];
			if (b2TreeNode.IsLeaf())
			{
				return 0;
			}
			int num = this.ComputeHeight(b2TreeNode.child1);
			int num2 = this.ComputeHeight(b2TreeNode.child2);
			return 1 + Mathf.Max(num, num2);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00040924 File Offset: 0x0003EB24
		private int ComputeHeight()
		{
			return this.ComputeHeight(this.m_root);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00040940 File Offset: 0x0003EB40
		private void ValidateStructure(int index)
		{
			if (index == -1)
			{
				return;
			}
			b2TreeNode b2TreeNode = this.m_nodes[index];
			int child = b2TreeNode.child1;
			int child2 = b2TreeNode.child2;
			if (b2TreeNode.IsLeaf())
			{
				return;
			}
			this.ValidateStructure(child);
			this.ValidateStructure(child2);
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00040988 File Offset: 0x0003EB88
		private void ValidateMetrics(int index)
		{
			if (index == -1)
			{
				return;
			}
			b2TreeNode b2TreeNode = this.m_nodes[index];
			int child = b2TreeNode.child1;
			int child2 = b2TreeNode.child2;
			if (b2TreeNode.IsLeaf())
			{
				return;
			}
			int height = this.m_nodes[child].height;
			int height2 = this.m_nodes[child2].height;
			int num = 1 + Mathf.Max(height, height2);
			default(b2AABB).Combine(this.m_nodes[child].fatAabb, this.m_nodes[child2].fatAabb);
			this.ValidateMetrics(child);
			this.ValidateMetrics(child2);
		}

		// Token: 0x04000DE5 RID: 3557
		private int m_root;

		// Token: 0x04000DE6 RID: 3558
		private b2TreeNode[] m_nodes;

		// Token: 0x04000DE7 RID: 3559
		private int m_nodeCount;

		// Token: 0x04000DE8 RID: 3560
		private int m_nodeCapacity;

		// Token: 0x04000DE9 RID: 3561
		private int m_freeList;

		// Token: 0x04000DEA RID: 3562
		private Stack<int> m_stack = new Stack<int>(256);

		// Token: 0x04000DEB RID: 3563
		public const int b2_nullNode = -1;

		// Token: 0x04000DEC RID: 3564
		private const float b2_aabbExtension = 0.1f;

		// Token: 0x04000DED RID: 3565
		private const float b2_aabbMultiplier = 2f;
	}
}
