using UnityEngine;

namespace CardGame.Common.Extensions
{
    public static class TransformExtensions
    {
        public static void ResetParent(this Transform obj, Transform target)
        {
            obj.SetParent(target, false);
            obj.localPosition = Vector3.zero;
            obj.localEulerAngles = Vector3.zero;
            obj.localScale = Vector3.one;
        }
    }
}
