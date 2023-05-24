using System;
using UnityEngine;

// Token: 0x02000BBC RID: 3004
[Serializable]
public class tk2dSpriteColliderIsland
{
	// Token: 0x06003FE3 RID: 16355 RVA: 0x001440F0 File Offset: 0x001422F0
	public bool IsValid()
	{
		if (this.connected)
		{
			return this.points.Length >= 3;
		}
		return this.points.Length >= 2;
	}

	// Token: 0x06003FE4 RID: 16356 RVA: 0x0014411C File Offset: 0x0014231C
	public void CopyFrom(tk2dSpriteColliderIsland src)
	{
		this.connected = src.connected;
		this.points = new Vector2[src.points.Length];
		for (int i = 0; i < this.points.Length; i++)
		{
			this.points[i] = src.points[i];
		}
	}

	// Token: 0x06003FE5 RID: 16357 RVA: 0x00144184 File Offset: 0x00142384
	public bool CompareTo(tk2dSpriteColliderIsland src)
	{
		if (this.connected != src.connected)
		{
			return false;
		}
		if (this.points.Length != src.points.Length)
		{
			return false;
		}
		for (int i = 0; i < this.points.Length; i++)
		{
			if (this.points[i] != src.points[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0400320C RID: 12812
	public bool connected = true;

	// Token: 0x0400320D RID: 12813
	public Vector2[] points;
}
