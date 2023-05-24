using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001635 RID: 5685
[Serializable]
public class DebrisDirectionalAnimationInfo
{
	// Token: 0x060084B3 RID: 33971 RVA: 0x0036A5BC File Offset: 0x003687BC
	public string GetAnimationForVector(Vector2 dir)
	{
		switch (DungeonData.GetCardinalFromVector2(dir))
		{
		case DungeonData.Direction.NORTH:
			return this.fallUp;
		case DungeonData.Direction.EAST:
			return this.fallRight;
		case DungeonData.Direction.SOUTH:
			return this.fallDown;
		case DungeonData.Direction.WEST:
			return this.fallLeft;
		}
		return this.fallDown;
	}

	// Token: 0x04008861 RID: 34913
	public string fallUp;

	// Token: 0x04008862 RID: 34914
	public string fallRight;

	// Token: 0x04008863 RID: 34915
	public string fallDown;

	// Token: 0x04008864 RID: 34916
	public string fallLeft;
}
