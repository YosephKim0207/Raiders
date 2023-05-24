using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200158F RID: 5519
public class PunchoutDroppedItem : MonoBehaviour
{
	// Token: 0x06007E89 RID: 32393 RVA: 0x00332504 File Offset: 0x00330704
	public void Init(bool isLeft)
	{
		if (PunchoutDroppedItem.s_randomIndices == null || PunchoutDroppedItem.s_randomIndices.Length != this.targetOffsets.Count)
		{
			PunchoutDroppedItem.s_randomIndices = new int[this.targetOffsets.Count];
			for (int i = 0; i < PunchoutDroppedItem.s_randomIndices.Length; i++)
			{
				PunchoutDroppedItem.s_randomIndices[i] = i;
			}
			BraveUtility.RandomizeArray<int>(PunchoutDroppedItem.s_randomIndices, 0, -1);
			PunchoutDroppedItem.s_indicesIndex = 0;
		}
		this.m_offset = base.transform.position.XY() - base.GetComponent<tk2dBaseSprite>().WorldBottomCenter;
		this.m_startPosition = base.transform.position;
		this.m_targetPosition = this.m_startPosition + this.targetOffsets[PunchoutDroppedItem.s_randomIndices[PunchoutDroppedItem.s_indicesIndex]].Scale((float)((!isLeft) ? 1 : (-1)), 1f);
		PunchoutDroppedItem.s_indicesIndex = (PunchoutDroppedItem.s_indicesIndex + 1) % PunchoutDroppedItem.s_randomIndices.Length;
		base.StartCoroutine(this.MoveCR());
		AkSoundEngine.PostEvent("Play_OBJ_coin_spill_01", base.gameObject);
	}

	// Token: 0x06007E8A RID: 32394 RVA: 0x00332628 File Offset: 0x00330828
	private IEnumerator MoveCR()
	{
		tk2dSprite sprite = base.GetComponent<tk2dSprite>();
		sprite.HeightOffGround = 8f;
		sprite.UpdateZDepth();
		float timer = 0f;
		while (timer < this.airTime)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			float t = Mathf.Clamp01(timer / this.airTime);
			Vector2 pos = Vector2.Lerp(this.m_startPosition, this.m_targetPosition, t);
			pos.y += Mathf.Sin(t * 3.1415927f) * this.airHeight;
			base.transform.position = pos + this.m_offset;
			sprite.UpdateZDepth();
		}
		Vector2 deltaPos = this.m_targetPosition - this.m_startPosition;
		this.m_startPosition = this.m_targetPosition;
		this.m_targetPosition = this.m_startPosition + deltaPos.normalized * 2f;
		timer = 0f;
		while (timer < this.airTime2)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			float t2 = Mathf.Clamp01(timer / this.airTime2);
			Vector2 pos2 = Vector2.Lerp(this.m_startPosition, this.m_targetPosition, t2);
			pos2.y += Mathf.Sin(t2 * 3.1415927f) * this.airHeight2;
			base.transform.position = pos2 + this.m_offset;
			sprite.UpdateZDepth();
		}
		sprite.renderer.sortingLayerName = "Background";
		sprite.SortingOrder = 1;
		sprite.HeightOffGround = -2f;
		sprite.UpdateZDepth();
		Vector2 midPos = Vector2.Lerp(this.m_startPosition, this.m_targetPosition, this.motionStartPercent);
		midPos.y += Mathf.Sin(this.motionStartPercent * 3.1415927f) * this.airHeight2;
		Vector2 endPos = this.m_targetPosition;
		Vector2 velocity = (endPos - midPos).normalized * this.motionMultiplier;
		timer = 0f;
		while (timer < 1.25f)
		{
			yield return null;
			timer += BraveTime.DeltaTime;
			Vector2 pos3 = endPos + timer * velocity;
			pos3.y -= Mathf.Sin(Mathf.Clamp(timer * 0.5f, 0f, 0.5f) * 3.1415927f) * this.gravityMultiplier;
			base.transform.position = pos3 + this.m_offset;
			sprite.UpdateZDepth();
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04008176 RID: 33142
	private static int[] s_randomIndices;

	// Token: 0x04008177 RID: 33143
	private static int s_indicesIndex;

	// Token: 0x04008178 RID: 33144
	public List<Vector2> targetOffsets;

	// Token: 0x04008179 RID: 33145
	public float airTime;

	// Token: 0x0400817A RID: 33146
	public float airHeight;

	// Token: 0x0400817B RID: 33147
	public float airTime2;

	// Token: 0x0400817C RID: 33148
	public float airHeight2;

	// Token: 0x0400817D RID: 33149
	public float motionStartPercent = 0.75f;

	// Token: 0x0400817E RID: 33150
	public float motionMultiplier = 4f;

	// Token: 0x0400817F RID: 33151
	public float gravityMultiplier = 6f;

	// Token: 0x04008180 RID: 33152
	private Vector2 m_offset;

	// Token: 0x04008181 RID: 33153
	private Vector2 m_startPosition;

	// Token: 0x04008182 RID: 33154
	private Vector2 m_targetPosition;
}
