using System;
using UnityEngine;

// Token: 0x02000BB5 RID: 2997
[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteAnimation")]
public class tk2dSpriteAnimation : MonoBehaviour
{
	// Token: 0x06003F6A RID: 16234 RVA: 0x00141840 File Offset: 0x0013FA40
	public tk2dSpriteAnimationClip GetClipByName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		for (int i = 0; i < this.clips.Length; i++)
		{
			if (this.clips[i].name == name)
			{
				return this.clips[i];
			}
		}
		return null;
	}

	// Token: 0x06003F6B RID: 16235 RVA: 0x00141898 File Offset: 0x0013FA98
	public tk2dSpriteAnimationClip GetClipById(int id)
	{
		if (id < 0 || id >= this.clips.Length || this.clips[id].Empty)
		{
			return null;
		}
		return this.clips[id];
	}

	// Token: 0x06003F6C RID: 16236 RVA: 0x001418CC File Offset: 0x0013FACC
	public int GetClipIdByName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return -1;
		}
		for (int i = 0; i < this.clips.Length; i++)
		{
			if (this.clips[i].name == name)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06003F6D RID: 16237 RVA: 0x0014191C File Offset: 0x0013FB1C
	public int GetClipIdByName(tk2dSpriteAnimationClip clip)
	{
		for (int i = 0; i < this.clips.Length; i++)
		{
			if (this.clips[i] == clip)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x17000998 RID: 2456
	// (get) Token: 0x06003F6E RID: 16238 RVA: 0x00141954 File Offset: 0x0013FB54
	public tk2dSpriteAnimationClip FirstValidClip
	{
		get
		{
			for (int i = 0; i < this.clips.Length; i++)
			{
				if (!this.clips[i].Empty && this.clips[i].frames[0].spriteCollection != null && this.clips[i].frames[0].spriteId != -1)
				{
					return this.clips[i];
				}
			}
			return null;
		}
	}

	// Token: 0x040031BD RID: 12733
	public tk2dSpriteAnimationClip[] clips;
}
