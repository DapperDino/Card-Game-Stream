using UnityEngine;
using System.Collections;

namespace TheLiquidFire.Animation
{
	public abstract class Vector3Tweener : Tweener
	{
		public Vector3 startTweenValue;
		public Vector3 endTweenValue;
		public Vector3 currentTweenValue { get; private set; }

		protected override void OnUpdate ()
		{
			currentTweenValue = (endTweenValue - startTweenValue) * currentValue + startTweenValue;
			base.OnUpdate ();
		}
	}
}