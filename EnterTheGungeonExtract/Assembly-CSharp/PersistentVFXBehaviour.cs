using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015BB RID: 5563
public class PersistentVFXBehaviour : BraveBehaviour
{
	// Token: 0x06007FCA RID: 32714 RVA: 0x00339EE4 File Offset: 0x003380E4
	public void BecomeDebris(Vector3 startingForce, float startingHeight, params Type[] keepComponents)
	{
		List<Type> list = new List<Type>(keepComponents);
		foreach (Component component in base.GetComponents<Component>())
		{
			if (!(component is tk2dBaseSprite) && !(component is tk2dSpriteAnimator) && !(component is Renderer) && !(component is MeshFilter) && !(component is DebrisObject) && !(component is SpriteAnimatorStopper) && !(component is Transform) && !list.Contains(component.GetType()))
			{
				UnityEngine.Object.Destroy(component);
			}
		}
		DebrisObject orAddComponent = base.gameObject.GetOrAddComponent<DebrisObject>();
		orAddComponent.angularVelocity = 45f;
		orAddComponent.angularVelocityVariance = 20f;
		orAddComponent.decayOnBounce = 0.5f;
		orAddComponent.bounceCount = 1;
		orAddComponent.canRotate = true;
		orAddComponent.Trigger(startingForce, startingHeight, 1f);
	}

	// Token: 0x06007FCB RID: 32715 RVA: 0x00339FD4 File Offset: 0x003381D4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
