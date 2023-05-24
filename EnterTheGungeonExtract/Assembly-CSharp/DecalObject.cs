using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200163D RID: 5693
public class DecalObject : EphemeralObject
{
	// Token: 0x060084FE RID: 34046 RVA: 0x0036D7E4 File Offset: 0x0036B9E4
	public static void ClearPerLevelData()
	{
		DecalObject.m_roomMap.Clear();
	}

	// Token: 0x060084FF RID: 34047 RVA: 0x0036D7F0 File Offset: 0x0036B9F0
	public override void Start()
	{
		base.Start();
		if (this.IsRoomLimited)
		{
			this.m_parent = base.transform.position.GetAbsoluteRoom();
			if (!DecalObject.m_roomMap.ContainsKey(this.m_parent))
			{
				DecalObject.m_roomMap.Add(this.m_parent, new List<DecalObject>());
			}
			DecalObject.m_roomMap[this.m_parent].Add(this);
			if (DecalObject.m_roomMap[this.m_parent].Count > this.MaxNumberInRoom)
			{
				DecalObject decalObject = DecalObject.m_roomMap[this.m_parent][0];
				DecalObject.m_roomMap[this.m_parent].RemoveAt(0);
				decalObject.StartCoroutine(decalObject.FadeAndDestroy(decalObject));
			}
		}
	}

	// Token: 0x06008500 RID: 34048 RVA: 0x0036D8C0 File Offset: 0x0036BAC0
	public IEnumerator FadeAndDestroy(DecalObject decal)
	{
		float elapsed = 0f;
		float duration = 0.5f;
		tk2dBaseSprite spr = decal.sprite;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			if (spr)
			{
				spr.color = spr.color.WithAlpha(Mathf.Lerp(1f, 0f, elapsed / duration));
			}
			yield return null;
		}
		UnityEngine.Object.Destroy(decal.gameObject);
		yield break;
	}

	// Token: 0x06008501 RID: 34049 RVA: 0x0036D8DC File Offset: 0x0036BADC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.IsRoomLimited && DecalObject.m_roomMap.ContainsKey(this.m_parent))
		{
			DecalObject.m_roomMap[this.m_parent].Remove(this);
		}
	}

	// Token: 0x040088DD RID: 35037
	private static Dictionary<RoomHandler, List<DecalObject>> m_roomMap = new Dictionary<RoomHandler, List<DecalObject>>();

	// Token: 0x040088DE RID: 35038
	public bool IsRoomLimited;

	// Token: 0x040088DF RID: 35039
	[ShowInInspectorIf("IsRoomLimited", false)]
	public int MaxNumberInRoom = 5;

	// Token: 0x040088E0 RID: 35040
	private RoomHandler m_parent;
}
