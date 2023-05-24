using System;
using System.Collections.Generic;

// Token: 0x020014FE RID: 5374
public class PlayableCharactersComparer : IEqualityComparer<PlayableCharacters>
{
	// Token: 0x06007AA3 RID: 31395 RVA: 0x00312C64 File Offset: 0x00310E64
	public bool Equals(PlayableCharacters x, PlayableCharacters y)
	{
		return x == y;
	}

	// Token: 0x06007AA4 RID: 31396 RVA: 0x00312C6C File Offset: 0x00310E6C
	public int GetHashCode(PlayableCharacters obj)
	{
		return (int)obj;
	}
}
