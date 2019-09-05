using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLiquidFire.Animation
{
	public class QuaternionTweener : Tweener 
	{
		public Quaternion startTweenValue;
		public Quaternion endTweenValue;
		public Quaternion currentTweenValue { get; private set; }

		protected override void OnUpdate ()
		{
			currentTweenValue = Quaternion.SlerpUnclamped (startTweenValue, endTweenValue, currentValue);
			base.OnUpdate ();
		}
	}
}