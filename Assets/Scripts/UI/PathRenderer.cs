using UnityEngine;
using UnityEngine.UI;

namespace BasketLegend.UI
{
    public class PathRenderer : MonoBehaviour
    {
        [Header("Path Components")]
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Image pathImage;
        
        [Header("Path Settings")]
        [SerializeField] private float pathWidth = 10f;
        [SerializeField] private Color unlockedPathColor = Color.white;
        [SerializeField] private Color lockedPathColor = Color.gray;
        [SerializeField] private Material pathMaterial;

        [Header("Animation")]
        [SerializeField] private bool animatePath = true;
        [SerializeField] private float animationSpeed = 2f;

        private Vector2 startPosition;
        private Vector2 endPosition;
        private bool isPathUnlocked;

        private void Awake()
        {
            SetupLineRenderer();
        }

        private void SetupLineRenderer()
        {
            if (lineRenderer == null)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }

            lineRenderer.material = pathMaterial;
            lineRenderer.startWidth = pathWidth;
            lineRenderer.endWidth = pathWidth;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = false;
            lineRenderer.sortingOrder = -1;
        }

        public void SetPath(Vector2 start, Vector2 end)
        {
            startPosition = start;
            endPosition = end;
            UpdatePathVisuals();
        }

        public void SetPathState(bool isUnlocked)
        {
            isPathUnlocked = isUnlocked;
            UpdatePathColor();
            
            if (animatePath && isUnlocked)
            {
                StartPathAnimation();
            }
        }

        private void UpdatePathVisuals()
        {
            if (lineRenderer != null)
            {
                // Create curved path between points
                Vector3[] pathPoints = CreateCurvedPath(startPosition, endPosition);
                lineRenderer.positionCount = pathPoints.Length;
                lineRenderer.SetPositions(pathPoints);
            }

            // Alternative: Use UI Image for simpler paths
            if (pathImage != null)
            {
                UpdateImagePath();
            }
        }

        private Vector3[] CreateCurvedPath(Vector2 start, Vector2 end)
        {
            int segments = 10;
            Vector3[] points = new Vector3[segments + 1];
            
            // Calculate control point for curve
            Vector2 midPoint = (start + end) * 0.5f;
            Vector2 direction = (end - start).normalized;
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);
            Vector2 controlPoint = midPoint + perpendicular * 50f; // Curve intensity

            for (int i = 0; i <= segments; i++)
            {
                float t = (float)i / segments;
                points[i] = CalculateBezierPoint(t, start, controlPoint, end);
            }

            return points;
        }

        private Vector3 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector2 point = uu * p0 + 2 * u * t * p1 + tt * p2;
            return new Vector3(point.x, point.y, 0);
        }

        private void UpdateImagePath()
        {
            // Calculate rotation and scale for straight line path
            Vector2 direction = endPosition - startPosition;
            float distance = direction.magnitude;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            RectTransform rectTransform = pathImage.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = (startPosition + endPosition) * 0.5f;
                rectTransform.sizeDelta = new Vector2(distance, pathWidth);
                rectTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        private void UpdatePathColor()
        {
            Color targetColor = isPathUnlocked ? unlockedPathColor : lockedPathColor;

            if (lineRenderer != null)
            {
                if (lineRenderer.material != null)
                {
                    lineRenderer.material.color = targetColor;
                }
            }

            if (pathImage != null)
            {
                pathImage.color = targetColor;
            }
        }

        private void StartPathAnimation()
        {
            if (!animatePath) return;
            
            StartCoroutine(AnimatePathReveal());
        }

        private System.Collections.IEnumerator AnimatePathReveal()
        {
            if (lineRenderer == null) yield break;

            float duration = 1f / animationSpeed;
            float elapsedTime = 0f;
            int totalPoints = lineRenderer.positionCount;

            // Start with no visible points
            lineRenderer.positionCount = 1;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                int visiblePoints = Mathf.RoundToInt(t * totalPoints);
                visiblePoints = Mathf.Max(1, visiblePoints);
                
                lineRenderer.positionCount = visiblePoints;
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure all points are visible
            lineRenderer.positionCount = totalPoints;
        }

        private void OnValidate()
        {
            if (lineRenderer != null)
            {
                lineRenderer.startWidth = pathWidth;
                lineRenderer.endWidth = pathWidth;
            }
        }
    }
}