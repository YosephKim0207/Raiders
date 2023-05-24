using System;
using UnityEngine;

// Token: 0x02000BB3 RID: 2995
[Serializable]
public class tk2dSpriteAnimationClip
{
	// Token: 0x06003F62 RID: 16226 RVA: 0x0014162C File Offset: 0x0013F82C
	public tk2dSpriteAnimationClip()
	{
	}

	// Token: 0x06003F63 RID: 16227 RVA: 0x00141660 File Offset: 0x0013F860
	public tk2dSpriteAnimationClip(tk2dSpriteAnimationClip source)
	{
		this.CopyFrom(source);
	}

	// Token: 0x17000996 RID: 2454
	// (get) Token: 0x06003F64 RID: 16228 RVA: 0x0014169C File Offset: 0x0013F89C
	public float BaseClipLength
	{
		get
		{
			return (float)this.frames.Length / this.fps;
		}
	}

	// Token: 0x06003F65 RID: 16229 RVA: 0x001416B0 File Offset: 0x0013F8B0
	public void CopyFrom(tk2dSpriteAnimationClip source)
	{
		this.name = source.name;
		if (source.frames == null)
		{
			this.frames = null;
		}
		else
		{
			this.frames = new tk2dSpriteAnimationFrame[source.frames.Length];
			for (int i = 0; i < this.frames.Length; i++)
			{
				if (source.frames[i] == null)
				{
					this.frames[i] = null;
				}
				else
				{
					this.frames[i] = new tk2dSpriteAnimationFrame();
					this.frames[i].CopyFrom(source.frames[i]);
				}
			}
		}
		this.fps = source.fps;
		this.loopStart = source.loopStart;
		this.wrapMode = source.wrapMode;
		this.minFidgetDuration = source.minFidgetDuration;
		this.maxFidgetDuration = source.maxFidgetDuration;
		if (this.wrapMode == tk2dSpriteAnimationClip.WrapMode.Single && this.frames.Length > 1)
		{
			this.frames = new tk2dSpriteAnimationFrame[] { this.frames[0] };
			Debug.LogError(string.Format("Clip: '{0}' Fixed up frames for WrapMode.Single", this.name));
		}
	}

	// Token: 0x06003F66 RID: 16230 RVA: 0x001417CC File Offset: 0x0013F9CC
	public void Clear()
	{
		this.name = string.Empty;
		this.frames = new tk2dSpriteAnimationFrame[0];
		this.fps = 30f;
		this.loopStart = 0;
		this.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
	}

	// Token: 0x17000997 RID: 2455
	// (get) Token: 0x06003F67 RID: 16231 RVA: 0x00141800 File Offset: 0x0013FA00
	public bool Empty
	{
		get
		{
			return this.name.Length == 0 || this.frames == null || this.frames.Length == 0;
		}
	}

	// Token: 0x06003F68 RID: 16232 RVA: 0x0014182C File Offset: 0x0013FA2C
	public tk2dSpriteAnimationFrame GetFrame(int frame)
	{
		return this.frames[frame];
	}

	// Token: 0x040031AD RID: 12717
	public string name = "Default";

	// Token: 0x040031AE RID: 12718
	public tk2dSpriteAnimationFrame[] frames;

	// Token: 0x040031AF RID: 12719
	public float fps = 30f;

	// Token: 0x040031B0 RID: 12720
	public int loopStart;

	// Token: 0x040031B1 RID: 12721
	public tk2dSpriteAnimationClip.WrapMode wrapMode;

	// Token: 0x040031B2 RID: 12722
	public float minFidgetDuration = 1f;

	// Token: 0x040031B3 RID: 12723
	public float maxFidgetDuration = 2f;

	// Token: 0x02000BB4 RID: 2996
	public enum WrapMode
	{
		// Token: 0x040031B5 RID: 12725
		Loop,
		// Token: 0x040031B6 RID: 12726
		LoopSection,
		// Token: 0x040031B7 RID: 12727
		Once,
		// Token: 0x040031B8 RID: 12728
		PingPong,
		// Token: 0x040031B9 RID: 12729
		RandomFrame,
		// Token: 0x040031BA RID: 12730
		RandomLoop,
		// Token: 0x040031BB RID: 12731
		Single,
		// Token: 0x040031BC RID: 12732
		LoopFidget
	}
}
