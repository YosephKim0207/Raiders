using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001780 RID: 6016
public class GameUIBlankController : MonoBehaviour, ILevelLoadedListener
{
	// Token: 0x170014FA RID: 5370
	// (get) Token: 0x06008C3B RID: 35899 RVA: 0x003A9A78 File Offset: 0x003A7C78
	public dfPanel Panel
	{
		get
		{
			return this.m_panel;
		}
	}

	// Token: 0x06008C3C RID: 35900 RVA: 0x003A9A80 File Offset: 0x003A7C80
	private void Awake()
	{
		this.m_panel = base.GetComponent<dfPanel>();
		this.extantBlanks = new List<dfSprite>();
	}

	// Token: 0x06008C3D RID: 35901 RVA: 0x003A9A9C File Offset: 0x003A7C9C
	private void Start()
	{
		this.m_panel.IsInteractive = false;
		Collider[] components = base.GetComponents<Collider>();
		for (int i = 0; i < components.Length; i++)
		{
			UnityEngine.Object.Destroy(components[i]);
		}
	}

	// Token: 0x06008C3E RID: 35902 RVA: 0x003A9AD8 File Offset: 0x003A7CD8
	public void BraveOnLevelWasLoaded()
	{
		if (this.extantBlanks != null)
		{
			this.extantBlanks.Clear();
		}
	}

	// Token: 0x06008C3F RID: 35903 RVA: 0x003A9AF0 File Offset: 0x003A7CF0
	public void UpdateScale()
	{
		for (int i = 0; i < this.extantBlanks.Count; i++)
		{
			dfSprite dfSprite = this.extantBlanks[i];
			if (dfSprite)
			{
				Vector2 sizeInPixels = dfSprite.SpriteInfo.sizeInPixels;
				dfSprite.Size = sizeInPixels * Pixelator.Instance.CurrentTileScale;
			}
		}
	}

	// Token: 0x06008C40 RID: 35904 RVA: 0x003A9B58 File Offset: 0x003A7D58
	public dfSprite AddBlank()
	{
		Vector3 position = base.transform.position;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.blankSpritePrefab.gameObject, position, Quaternion.identity);
		gameObject.transform.parent = base.transform.parent;
		gameObject.layer = base.gameObject.layer;
		dfSprite component = gameObject.GetComponent<dfSprite>();
		if (this.IsRightAligned)
		{
			component.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Right;
		}
		Vector2 sizeInPixels = component.SpriteInfo.sizeInPixels;
		component.Size = sizeInPixels * Pixelator.Instance.CurrentTileScale;
		if (!this.IsRightAligned)
		{
			float num = (component.Width + Pixelator.Instance.CurrentTileScale) * (float)this.extantBlanks.Count;
			component.RelativePosition = this.m_panel.RelativePosition + new Vector3(num, 0f, 0f);
		}
		else
		{
			component.RelativePosition = this.m_panel.RelativePosition - new Vector3(component.Width, 0f, 0f);
			for (int i = 0; i < this.extantBlanks.Count; i++)
			{
				dfSprite dfSprite = this.extantBlanks[i];
				if (dfSprite)
				{
					dfSprite.RelativePosition += new Vector3(-1f * (component.Width + Pixelator.Instance.CurrentTileScale), 0f, 0f);
					GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite);
				}
			}
		}
		component.IsInteractive = false;
		this.extantBlanks.Add(component);
		GameUIRoot.Instance.AddControlToMotionGroups(component, (!this.IsRightAligned) ? DungeonData.Direction.WEST : DungeonData.Direction.EAST, false);
		return component;
	}

	// Token: 0x06008C41 RID: 35905 RVA: 0x003A9D28 File Offset: 0x003A7F28
	public void RemoveBlank()
	{
		if (this.extantBlanks.Count > 0)
		{
			float width = this.extantBlanks[0].Width;
			dfSprite dfSprite = this.extantBlanks[this.extantBlanks.Count - 1];
			if (dfSprite)
			{
				GameUIRoot.Instance.RemoveControlFromMotionGroups(dfSprite);
				UnityEngine.Object.Destroy(dfSprite);
			}
			this.extantBlanks.RemoveAt(this.extantBlanks.Count - 1);
			if (this.IsRightAligned)
			{
				for (int i = 0; i < this.extantBlanks.Count; i++)
				{
					dfSprite dfSprite2 = this.extantBlanks[i];
					if (dfSprite2)
					{
						dfSprite2.RelativePosition += new Vector3(width + Pixelator.Instance.CurrentTileScale, 0f, 0f);
						GameUIRoot.Instance.UpdateControlMotionGroup(dfSprite2);
					}
				}
			}
		}
	}

	// Token: 0x06008C42 RID: 35906 RVA: 0x003A9E20 File Offset: 0x003A8020
	public void UpdateBlanks(int numBlanks)
	{
		if (GameManager.Instance.IsLoadingLevel || Time.timeSinceLevelLoad < 0.01f)
		{
			return;
		}
		if (this.extantBlanks.Count < numBlanks)
		{
			for (int i = this.extantBlanks.Count; i < numBlanks; i++)
			{
				this.AddBlank();
			}
		}
		else if (this.extantBlanks.Count > numBlanks)
		{
			while (this.extantBlanks.Count > numBlanks)
			{
				this.RemoveBlank();
			}
		}
	}

	// Token: 0x04009384 RID: 37764
	public dfSprite blankSpritePrefab;

	// Token: 0x04009385 RID: 37765
	public List<dfSprite> extantBlanks;

	// Token: 0x04009386 RID: 37766
	public bool IsRightAligned;

	// Token: 0x04009387 RID: 37767
	private dfPanel m_panel;
}
