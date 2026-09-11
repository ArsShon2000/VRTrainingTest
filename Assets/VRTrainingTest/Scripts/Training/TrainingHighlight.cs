using UnityEngine;
using UnityEngine.UI;

namespace VRTrainingTest.Training
{
    public sealed class TrainingHighlight : MonoBehaviour
    {
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private Graphic[] graphics;
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private float blinkSpeed = 2f;
        [SerializeField] private float highlightStrength = 0.35f;

        private Color[] originalRendererColors;
        private Color[] originalGraphicColors;
        private bool isHighlighted;

        private void Awake()
        {
            // Если поля не заполнены руками, берем все подходящие части внутри объекта
            if (renderers == null || renderers.Length == 0)
            {
                renderers = GetComponentsInChildren<Renderer>();
            }

            if (graphics == null || graphics.Length == 0)
            {
                graphics = GetComponentsInChildren<Graphic>();
            }

            originalRendererColors = new Color[renderers.Length];
            originalGraphicColors = new Color[graphics.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                originalRendererColors[i] = renderers[i].material.color;
            }

            for (int i = 0; i < graphics.Length; i++)
            {
                originalGraphicColors[i] = graphics[i].color;
            }

            ApplyOriginalColors();
        }

        private void Update()
        {
            if (!isHighlighted)
                return;

            // Мигаем мягко: смешиваем родной цвет с цветом подсветки, а не закрашиваем целиком
            float pulse = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            float amount = pulse * highlightStrength;

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = Color.Lerp(
                    originalRendererColors[i],
                    highlightColor,
                    amount);
            }

            for (int i = 0; i < graphics.Length; i++)
            {
                graphics[i].color = Color.Lerp(
                    originalGraphicColors[i],
                    highlightColor,
                    amount);
            }
        }

        public void SetHighlighted(bool highlighted)
        {
            isHighlighted = highlighted;

            if (!isHighlighted)
            {
                ApplyOriginalColors();
            }
        }

        private void ApplyOriginalColors()
        {
            // Возвращаем цвет назад, когда шаг перестал быть текущим
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = originalRendererColors[i];
            }

            for (int i = 0; i < graphics.Length; i++)
            {
                graphics[i].color = originalGraphicColors[i];
            }
        }
    }
}
