using UnityEngine;
using System.Collections;

namespace TheLiquidFire.Animation
{
	public class TransformLocalEulerTweener : Vector3Tweener 
	{
		protected override void OnUpdate ()
		{
			base.OnUpdate ();
			transform.localEulerAngles = currentTweenValue;
		}
	}
}