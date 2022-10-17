using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TheGame
{
    public class PlayerInfoBar : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button button;

        private Account playerInfo;
        private IPlayerInfoHandler handler;

        public void Init(Account info, float step, IPlayerInfoHandler handler, bool isSelected)
        {
            playerInfo = info;
            this.handler = handler;
            text.text = playerInfo.name;

            var position = rectTransform.localPosition;
            position.y += step;
            rectTransform.localPosition = position;
            button.onClick.AddListener(DoOnButtonPressed);
            if (isSelected)
            {
                button.Select();
            }
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(DoOnButtonPressed);
        }

        private void DoOnButtonPressed()
        {
            handler.SelectInfo(playerInfo);
        }
    }
}

