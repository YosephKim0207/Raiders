using System;
using UnityEngine;

// Token: 0x0200116F RID: 4463
public class FuseController : MonoBehaviour
{
	// Token: 0x06006322 RID: 25378 RVA: 0x00266908 File Offset: 0x00264B08
	private void Start()
	{
		for (int i = 0; i < this.fuseSegments.Length; i++)
		{
			this.TotalPixelLength += this.fuseSegments[i].dimensions.x;
		}
	}

	// Token: 0x06006323 RID: 25379 RVA: 0x00266950 File Offset: 0x00264B50
	public void Trigger()
	{
		this.m_triggered = true;
	}

	// Token: 0x06006324 RID: 25380 RVA: 0x0026695C File Offset: 0x00264B5C
	private void Update()
	{
		if (this.m_triggered)
		{
			float usedPixelLength = this.UsedPixelLength;
			this.UsedPixelLength = usedPixelLength + this.TotalPixelLength / this.duration * BraveTime.DeltaTime;
			float num = this.UsedPixelLength - usedPixelLength;
			for (int i = 0; i < this.fuseSegments.Length; i++)
			{
				if (this.fuseSegments[i].dimensions.x > 0f)
				{
					if (this.fuseSegments[i].dimensions.x >= num)
					{
						this.fuseSegments[i].dimensions = this.fuseSegments[i].dimensions - new Vector2(num, 0f);
						break;
					}
					num -= this.fuseSegments[i].dimensions.x;
					this.fuseSegments[i].dimensions = new Vector2(0f, this.fuseSegments[i].dimensions.y);
				}
			}
		}
	}

	// Token: 0x04005E3D RID: 24125
	public tk2dTiledSprite[] fuseSegments;

	// Token: 0x04005E3E RID: 24126
	public GameObject sparkVFXPrefab;

	// Token: 0x04005E3F RID: 24127
	public float duration = 5f;

	// Token: 0x04005E40 RID: 24128
	private float TotalPixelLength;

	// Token: 0x04005E41 RID: 24129
	private Transform m_sparkInstance;

	// Token: 0x04005E42 RID: 24130
	private float UsedPixelLength;

	// Token: 0x04005E43 RID: 24131
	private bool m_triggered;
}
