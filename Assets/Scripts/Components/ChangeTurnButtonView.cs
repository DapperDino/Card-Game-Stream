using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CardGame.Components
{
    public class ChangeTurnButtonView : MonoBehaviour
    {
        [Required] [SerializeField] private Transform rotationHandle = null;
        [Required] [SerializeField] private TextMeshProUGUI allyText = null;
        [Required] [SerializeField] private TextMeshProUGUI enemyText = null;

        public Transform RotationHandle => rotationHandle;
    }
}
