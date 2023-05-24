using System;
using UnityEngine;

// Token: 0x02000465 RID: 1125
public class dfFollowObjectSorter : MonoBehaviour
{
	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06001A08 RID: 6664 RVA: 0x000798A8 File Offset: 0x00077AA8
	public static dfFollowObjectSorter Instance
	{
		get
		{
			object typeFromHandle = typeof(dfFollowObjectSorter);
			dfFollowObjectSorter instance;
			lock (typeFromHandle)
			{
				if (dfFollowObjectSorter._instance == null && Application.isPlaying)
				{
					GameObject gameObject = new GameObject("Follow Object Sorter");
					dfFollowObjectSorter._instance = gameObject.AddComponent<dfFollowObjectSorter>();
					dfFollowObjectSorter.list.Clear();
				}
				instance = dfFollowObjectSorter._instance;
			}
			return instance;
		}
	}

	// Token: 0x06001A09 RID: 6665 RVA: 0x00079924 File Offset: 0x00077B24
	public static void Register(dfFollowObject follow)
	{
		if (Application.isPlaying)
		{
			dfFollowObjectSorter.Instance.register(follow);
		}
	}

	// Token: 0x06001A0A RID: 6666 RVA: 0x0007993C File Offset: 0x00077B3C
	public static void Unregister(dfFollowObject follow)
	{
		for (int i = 0; i < dfFollowObjectSorter.list.Count; i++)
		{
			if (dfFollowObjectSorter.list[i].follow == follow)
			{
				dfFollowObjectSorter.list.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06001A0B RID: 6667 RVA: 0x0007998C File Offset: 0x00077B8C
	public void Update()
	{
		int num = int.MaxValue;
		for (int i = 0; i < dfFollowObjectSorter.list.Count; i++)
		{
			dfFollowObjectSorter.FollowSortRecord followSortRecord = dfFollowObjectSorter.list[i];
			if (followSortRecord.follow.attach)
			{
				followSortRecord.distance = this.getDistance(followSortRecord.follow);
				if (followSortRecord.control.ZOrder < num)
				{
					num = followSortRecord.control.ZOrder;
				}
			}
		}
		dfFollowObjectSorter.list.Sort();
		for (int j = 0; j < dfFollowObjectSorter.list.Count; j++)
		{
			dfControl control = dfFollowObjectSorter.list[j].control;
			control.ZOrder = num++;
		}
	}

	// Token: 0x06001A0C RID: 6668 RVA: 0x00079A54 File Offset: 0x00077C54
	private void register(dfFollowObject follow)
	{
		for (int i = 0; i < dfFollowObjectSorter.list.Count; i++)
		{
			if (dfFollowObjectSorter.list[i].follow == follow)
			{
				return;
			}
		}
		dfFollowObjectSorter.list.Add(new dfFollowObjectSorter.FollowSortRecord(follow));
	}

	// Token: 0x06001A0D RID: 6669 RVA: 0x00079AA8 File Offset: 0x00077CA8
	private float getDistance(dfFollowObject follow)
	{
		return (follow.mainCamera.transform.position - follow.attach.transform.position).sqrMagnitude;
	}

	// Token: 0x0400146C RID: 5228
	private static dfFollowObjectSorter _instance;

	// Token: 0x0400146D RID: 5229
	private static dfList<dfFollowObjectSorter.FollowSortRecord> list = new dfList<dfFollowObjectSorter.FollowSortRecord>();

	// Token: 0x02000466 RID: 1126
	private class FollowSortRecord : IComparable<dfFollowObjectSorter.FollowSortRecord>
	{
		// Token: 0x06001A0F RID: 6671 RVA: 0x00079AF0 File Offset: 0x00077CF0
		public FollowSortRecord(dfFollowObject follow)
		{
			this.follow = follow;
			this.control = follow.GetComponent<dfControl>();
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x00079B0C File Offset: 0x00077D0C
		public int CompareTo(dfFollowObjectSorter.FollowSortRecord other)
		{
			return other.distance.CompareTo(this.distance);
		}

		// Token: 0x0400146E RID: 5230
		public float distance;

		// Token: 0x0400146F RID: 5231
		public dfFollowObject follow;

		// Token: 0x04001470 RID: 5232
		public dfControl control;
	}
}
