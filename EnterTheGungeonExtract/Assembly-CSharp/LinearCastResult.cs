using System;

// Token: 0x02000868 RID: 2152
public class LinearCastResult : CastResult
{
	// Token: 0x06002FBD RID: 12221 RVA: 0x000FCA08 File Offset: 0x000FAC08
	private LinearCastResult()
	{
	}

	// Token: 0x06002FBE RID: 12222 RVA: 0x000FCA10 File Offset: 0x000FAC10
	public static void Cleanup(LinearCastResult linearCastResults)
	{
		linearCastResults.Contact.x = 0f;
		linearCastResults.Contact.y = 0f;
		linearCastResults.Normal.x = 0f;
		linearCastResults.Normal.y = 0f;
		linearCastResults.MyPixelCollider = null;
		linearCastResults.OtherPixelCollider = null;
		linearCastResults.TimeUsed = 0f;
		linearCastResults.CollidedX = false;
		linearCastResults.CollidedY = false;
		linearCastResults.NewPixelsToMove.x = 0;
		linearCastResults.NewPixelsToMove.y = 0;
		linearCastResults.Overlap = false;
	}

	// Token: 0x04002097 RID: 8343
	public float TimeUsed;

	// Token: 0x04002098 RID: 8344
	public bool CollidedX;

	// Token: 0x04002099 RID: 8345
	public bool CollidedY;

	// Token: 0x0400209A RID: 8346
	public IntVector2 NewPixelsToMove;

	// Token: 0x0400209B RID: 8347
	public bool Overlap;

	// Token: 0x0400209C RID: 8348
	public static ObjectPool<LinearCastResult> Pool = new ObjectPool<LinearCastResult>(() => new LinearCastResult(), 10, new ObjectPool<LinearCastResult>.Cleanup(LinearCastResult.Cleanup));
}
