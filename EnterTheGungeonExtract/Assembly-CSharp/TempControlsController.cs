using System;
using InControl;
using UnityEngine;

// Token: 0x020017F6 RID: 6134
public class TempControlsController : MonoBehaviour
{
	// Token: 0x1700159B RID: 5531
	// (get) Token: 0x06009080 RID: 36992 RVA: 0x003D1D34 File Offset: 0x003CFF34
	public bool CanClose
	{
		get
		{
			return this.m_elapsed > 0f;
		}
	}

	// Token: 0x06009081 RID: 36993 RVA: 0x003D1D44 File Offset: 0x003CFF44
	private void Awake()
	{
		this.m_elapsed = 0f;
		this.m_lastTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06009082 RID: 36994 RVA: 0x003D1D5C File Offset: 0x003CFF5C
	private void Update()
	{
		this.m_elapsed += Time.realtimeSinceStartup - this.m_lastTime;
		this.m_lastTime = Time.realtimeSinceStartup;
		Debug.Log(this.m_elapsed);
		if (this.m_elapsed > 0f && (BraveInput.PrimaryPlayerInstance.ActiveActions.Device.AnyButton.WasPressed || BraveInput.PrimaryPlayerInstance.ActiveActions.Device.GetControl(InputControlType.Start).WasPressed || BraveInput.PrimaryPlayerInstance.ActiveActions.Device.GetControl(InputControlType.Select).WasPressed))
		{
			GameManager.Instance.Unpause();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04009882 RID: 39042
	private float m_elapsed;

	// Token: 0x04009883 RID: 39043
	private float m_lastTime;

	// Token: 0x04009884 RID: 39044
	private const float CLOSE_THRESHOLD = 0f;
}
