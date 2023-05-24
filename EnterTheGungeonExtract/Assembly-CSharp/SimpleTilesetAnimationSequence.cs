using System;
using System.Collections.Generic;

// Token: 0x02000E7F RID: 3711
[Serializable]
public class SimpleTilesetAnimationSequence
{
	// Token: 0x040045D6 RID: 17878
	public SimpleTilesetAnimationSequence.TilesetSequencePlayStyle playstyle;

	// Token: 0x040045D7 RID: 17879
	public float loopDelayMin = 5f;

	// Token: 0x040045D8 RID: 17880
	public float loopDelayMax = 10f;

	// Token: 0x040045D9 RID: 17881
	public int loopceptionTarget = -1;

	// Token: 0x040045DA RID: 17882
	public int loopceptionMin = 1;

	// Token: 0x040045DB RID: 17883
	public int loopceptionMax = 3;

	// Token: 0x040045DC RID: 17884
	public int coreceptionMin = 1;

	// Token: 0x040045DD RID: 17885
	public int coreceptionMax = 1;

	// Token: 0x040045DE RID: 17886
	public bool randomStartFrame;

	// Token: 0x040045DF RID: 17887
	public List<SimpleTilesetAnimationSequenceEntry> entries = new List<SimpleTilesetAnimationSequenceEntry>();

	// Token: 0x02000E80 RID: 3712
	public enum TilesetSequencePlayStyle
	{
		// Token: 0x040045E1 RID: 17889
		SIMPLE_LOOP,
		// Token: 0x040045E2 RID: 17890
		DELAYED_LOOP,
		// Token: 0x040045E3 RID: 17891
		RANDOM_FRAMES,
		// Token: 0x040045E4 RID: 17892
		TRIGGERED_ONCE,
		// Token: 0x040045E5 RID: 17893
		LOOPCEPTION
	}
}
