using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x0200167A RID: 5754
public class ShellCasing : MonoBehaviour
{
	// Token: 0x06008638 RID: 34360 RVA: 0x0037843C File Offset: 0x0037663C
	private void Start()
	{
		this.m_transform = base.transform;
	}

	// Token: 0x06008639 RID: 34361 RVA: 0x0037844C File Offset: 0x0037664C
	public void Trigger()
	{
		this.m_transform = base.transform;
		this.m_renderer = base.GetComponent<Renderer>();
		float y;
		if (this.usesCustomTargetHeight)
		{
			y = this.customTargetHeight;
		}
		else
		{
			y = GameManager.Instance.PrimaryPlayer.transform.position.y;
		}
		this.startPosition = this.m_transform.position;
		int num = ((this.m_transform.right.x <= 0f) ? (-1) : 1);
		if (this.usesCustomVelocity)
		{
			this.velocity = this.customVelocity.ToVector3ZUp(0f);
		}
		else
		{
			this.velocity = Vector3.up * (UnityEngine.Random.value * 1.5f + 1f) + -1.5f * Vector3.right * (float)num * (UnityEngine.Random.value + 1.5f);
		}
		this.amountToFall = this.startPosition.y - y + UnityEngine.Random.value * this.heightVariance;
		if (this.amountToFall > 0f)
		{
			this.hasBeenAboveTargetHeight = true;
		}
		base.GetComponent<tk2dSprite>().automaticallyManagesDepth = false;
		DepthLookupManager.ProcessRenderer(this.m_renderer, DepthLookupManager.GungeonSortingLayer.PLAYFIELD);
		DepthLookupManager.UpdateRendererWithWorldYPosition(this.m_renderer, y);
		this.isDone = false;
		this.m_hasBeenTriggered = true;
	}

	// Token: 0x0600863A RID: 34362 RVA: 0x003785B8 File Offset: 0x003767B8
	private void Update()
	{
		if (BraveUtility.isLoadingLevel)
		{
			return;
		}
		if (!this.m_hasBeenTriggered)
		{
			return;
		}
		if (this.isDone)
		{
			return;
		}
		IntVector2 intVector = new IntVector2(Mathf.FloorToInt(this.m_transform.position.x), Mathf.FloorToInt(this.m_transform.position.y));
		if (!GameManager.Instance.Dungeon.data.CheckInBounds(intVector))
		{
			this.isDone = true;
			return;
		}
		CellData cellData = GameManager.Instance.Dungeon.data.cellData[intVector.x][intVector.y];
		if (cellData.type == CellType.WALL)
		{
			this.velocity.x = -this.velocity.x;
		}
		float num = BraveTime.DeltaTime * this.overallMultiplier;
		if (this.velocity.y < 0f)
		{
			this.hasBeenAboveTargetHeight = true;
		}
		if (this.m_transform.position.y > this.startPosition.y - this.amountToFall || (cellData != null && !cellData.IsPassable) || !this.hasBeenAboveTargetHeight)
		{
			this.m_transform.Rotate(0f, 0f, this.angularVelocity * num);
			this.m_transform.position += this.velocity * num;
			this.velocity += Vector3.down * 10f * num;
		}
		else
		{
			this.isDone = true;
			if (!string.IsNullOrEmpty(this.audioEventName))
			{
				AkSoundEngine.PostEvent(this.audioEventName, base.gameObject);
			}
			DepthLookupManager.UpdateRendererWithWorldYPosition(this.m_renderer, this.m_transform.position.y);
			MinorBreakable component = base.GetComponent<MinorBreakable>();
			if (component != null)
			{
				component.Break(UnityEngine.Random.insideUnitCircle);
			}
		}
	}

	// Token: 0x0600863B RID: 34363 RVA: 0x003787D0 File Offset: 0x003769D0
	public void ApplyVelocity(Vector2 vel)
	{
		base.StartCoroutine(this.HandlePush(vel * this.pushStrengthMultiplier));
	}

	// Token: 0x0600863C RID: 34364 RVA: 0x003787EC File Offset: 0x003769EC
	private IEnumerator HandlePush(Vector2 vel)
	{
		Vector3 pushVel = new Vector3(vel.x, vel.y, 0f);
		while (pushVel != Vector3.zero)
		{
			pushVel *= 0.97f;
			if (pushVel.magnitude < 0.025f)
			{
				pushVel = Vector3.zero;
			}
			IntVector2 nextGridCellX = new IntVector2(Mathf.FloorToInt(this.m_transform.position.x + pushVel.x * BraveTime.DeltaTime), Mathf.FloorToInt(this.m_transform.position.y));
			IntVector2 nextGridCellY = new IntVector2(Mathf.FloorToInt(this.m_transform.position.x), Mathf.FloorToInt(this.m_transform.position.y + pushVel.y * BraveTime.DeltaTime));
			CellData tileX = GameManager.Instance.Dungeon.data.cellData[nextGridCellX.x][nextGridCellX.y];
			CellData tileY = GameManager.Instance.Dungeon.data.cellData[nextGridCellY.x][nextGridCellY.y];
			if (!tileX.IsPassable)
			{
				pushVel = new Vector3(0f, pushVel.y, 0f);
			}
			if (!tileY.IsPassable)
			{
				pushVel = new Vector3(pushVel.x, 0f, 0f);
			}
			this.m_transform.position += pushVel * BraveTime.DeltaTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04008AFA RID: 35578
	private Vector3 startPosition;

	// Token: 0x04008AFB RID: 35579
	public float heightVariance = 0.5f;

	// Token: 0x04008AFC RID: 35580
	private float amountToFall;

	// Token: 0x04008AFD RID: 35581
	public float angularVelocity = 1080f;

	// Token: 0x04008AFE RID: 35582
	public float pushStrengthMultiplier = 1f;

	// Token: 0x04008AFF RID: 35583
	public bool usesCustomTargetHeight;

	// Token: 0x04008B00 RID: 35584
	public float customTargetHeight;

	// Token: 0x04008B01 RID: 35585
	public float overallMultiplier = 1f;

	// Token: 0x04008B02 RID: 35586
	public bool usesCustomVelocity;

	// Token: 0x04008B03 RID: 35587
	public Vector2 customVelocity = Vector2.zero;

	// Token: 0x04008B04 RID: 35588
	public string audioEventName;

	// Token: 0x04008B05 RID: 35589
	private Vector3 velocity;

	// Token: 0x04008B06 RID: 35590
	private bool isDone;

	// Token: 0x04008B07 RID: 35591
	private bool hasBeenAboveTargetHeight;

	// Token: 0x04008B08 RID: 35592
	private Transform m_transform;

	// Token: 0x04008B09 RID: 35593
	private Renderer m_renderer;

	// Token: 0x04008B0A RID: 35594
	private bool m_hasBeenTriggered;
}
