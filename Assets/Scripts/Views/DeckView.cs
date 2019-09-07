using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.Views
{
    public class DeckView : MonoBehaviour
    {
        [Required] [SerializeField] private Transform topCard = null;
        [Required] [SerializeField] private Transform squisher = null;

        public Transform TopCard => topCard;

        public void SetDeckSize(float size)
        {
            squisher.localScale = Mathf.Approximately(size, 0f) ? Vector3.zero : new Vector3(1f, size, 1f);
        }
    }
}
