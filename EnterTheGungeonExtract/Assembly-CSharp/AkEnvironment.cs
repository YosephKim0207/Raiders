using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018E5 RID: 6373
[RequireComponent(typeof(Collider))]
[AddComponentMenu("Wwise/AkEnvironment")]
[ExecuteInEditMode]
public class AkEnvironment : MonoBehaviour
{
	// Token: 0x06009D22 RID: 40226 RVA: 0x003EE5B0 File Offset: 0x003EC7B0
	public uint GetAuxBusID()
	{
		return (uint)this.m_auxBusID;
	}

	// Token: 0x06009D23 RID: 40227 RVA: 0x003EE5B8 File Offset: 0x003EC7B8
	public void SetAuxBusID(int in_auxBusID)
	{
		this.m_auxBusID = in_auxBusID;
	}

	// Token: 0x06009D24 RID: 40228 RVA: 0x003EE5C4 File Offset: 0x003EC7C4
	public float GetAuxSendValueForPosition(Vector3 in_position)
	{
		return 1f;
	}

	// Token: 0x06009D25 RID: 40229 RVA: 0x003EE5CC File Offset: 0x003EC7CC
	public Collider GetCollider()
	{
		return this.m_Collider;
	}

	// Token: 0x06009D26 RID: 40230 RVA: 0x003EE5D4 File Offset: 0x003EC7D4
	public void Awake()
	{
		this.m_Collider = base.GetComponent<Collider>();
	}

	// Token: 0x04009E9C RID: 40604
	public const int MAX_NB_ENVIRONMENTS = 4;

	// Token: 0x04009E9D RID: 40605
	public static AkEnvironment.AkEnvironment_CompareByPriority s_compareByPriority = new AkEnvironment.AkEnvironment_CompareByPriority();

	// Token: 0x04009E9E RID: 40606
	public static AkEnvironment.AkEnvironment_CompareBySelectionAlgorithm s_compareBySelectionAlgorithm = new AkEnvironment.AkEnvironment_CompareBySelectionAlgorithm();

	// Token: 0x04009E9F RID: 40607
	public bool excludeOthers;

	// Token: 0x04009EA0 RID: 40608
	public bool isDefault;

	// Token: 0x04009EA1 RID: 40609
	public int m_auxBusID;

	// Token: 0x04009EA2 RID: 40610
	private Collider m_Collider;

	// Token: 0x04009EA3 RID: 40611
	public int priority;

	// Token: 0x020018E6 RID: 6374
	public class AkEnvironment_CompareByPriority : IComparer<AkEnvironment>
	{
		// Token: 0x06009D29 RID: 40233 RVA: 0x003EE604 File Offset: 0x003EC804
		public virtual int Compare(AkEnvironment a, AkEnvironment b)
		{
			int num = a.priority.CompareTo(b.priority);
			return (num != 0 || !(a != b)) ? num : 1;
		}
	}

	// Token: 0x020018E7 RID: 6375
	public class AkEnvironment_CompareBySelectionAlgorithm : AkEnvironment.AkEnvironment_CompareByPriority
	{
		// Token: 0x06009D2B RID: 40235 RVA: 0x003EE644 File Offset: 0x003EC844
		public override int Compare(AkEnvironment a, AkEnvironment b)
		{
			if (a.isDefault)
			{
				return (!b.isDefault) ? 1 : base.Compare(a, b);
			}
			if (b.isDefault)
			{
				return -1;
			}
			if (a.excludeOthers)
			{
				return (!b.excludeOthers) ? (-1) : base.Compare(a, b);
			}
			return (!b.excludeOthers) ? base.Compare(a, b) : 1;
		}
	}
}
