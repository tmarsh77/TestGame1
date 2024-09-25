using UnityEngine;
using UnityEngine.UI;

namespace Lk.TestGameMP.Code.View
{
    public class UISkillProgress : MonoBehaviour
    {
        [SerializeField] private Image progress;

        public void SetValue(float value, float maxValue)
        {
            progress.fillAmount = value / maxValue;
        }
    }
}