using System;
using UnityEngine;

// Token: 0x020012A9 RID: 4777
public class PotemkinWafter : MonoBehaviour
{
	// Token: 0x06006AE3 RID: 27363 RVA: 0x0029EAC4 File Offset: 0x0029CCC4
	private void Start()
	{
		this.xSpeed = UnityEngine.Random.Range(0.7f, 1.3f) / 3f;
		this.ySpeed = UnityEngine.Random.Range(0.7f, 1.3f);
		if (PotemkinWafter.invert)
		{
			this.m_elapsed_x = 1f;
			PotemkinWafter.invert = false;
		}
		else
		{
			this.m_elapsed_x = 0f;
			PotemkinWafter.invert = true;
		}
	}

	// Token: 0x06006AE4 RID: 27364 RVA: 0x0029EB34 File Offset: 0x0029CD34
	private void Update()
	{
		this.m_elapsed_x += BraveTime.DeltaTime * this.xSpeed;
		this.m_elapsed_y += BraveTime.DeltaTime * this.ySpeed;
		this.m_currentVelocity.x = Mathf.Lerp(1f, -1f, Mathf.SmoothStep(0f, 1f, Mathf.PingPong(this.m_elapsed_x, 1f)));
		this.m_currentVelocity.y = Mathf.Lerp(0.25f, -0.25f, Mathf.SmoothStep(0f, 1f, Mathf.PingPong(this.m_elapsed_y + 0.25f, 1f)));
		this.m_currentVelocity.x = this.m_currentVelocity.x / 3f;
		base.transform.position += this.m_currentVelocity.ToVector3ZUp(0f) * BraveTime.DeltaTime;
	}

	// Token: 0x0400677C RID: 26492
	private static bool invert;

	// Token: 0x0400677D RID: 26493
	private Vector2 m_currentVelocity = Vector2.zero;

	// Token: 0x0400677E RID: 26494
	private float m_elapsed_x;

	// Token: 0x0400677F RID: 26495
	private float m_elapsed_y;

	// Token: 0x04006780 RID: 26496
	private float xSpeed = 1f;

	// Token: 0x04006781 RID: 26497
	private float ySpeed = 1f;
}
