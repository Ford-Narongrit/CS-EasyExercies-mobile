using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class windowChart : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private float marginLR = 150f;
    private List<GameObject> gameObjectList = new List<GameObject>();

    private void Awake()
    {
        renderGraph();
    }
    public void reRender()
    {
        clearGraph();
        renderGraph();
    }
    public void renderGraph()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        StartCoroutine(ApiCall.getRecord(ApiManager.Instance.user.access_token, () =>
        {
            List<int> valueList = new List<int>();
            List<string> labelX = new List<string>();
            foreach (Activity activity in ApiManager.Instance.activitiesList.activity)
            {
                valueList.Add(activity.burned_cal);
                labelX.Add(activity.date);
            }
            ShowGraphBar(valueList, (int _i) => labelX[_i], (float _f) => Mathf.RoundToInt(_f) + " Kcal");
        }, () =>
        {
            Debug.Log("create chart fail");
        }));
    }
    public void clearGraph()
    {
        foreach (GameObject item in gameObjectList)
        {
            Destroy(item);
        }
        gameObjectList = new List<GameObject>();
    }
    float GetAngleFromVectorFloat(Vector2 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
    private GameObject CreateBar(Vector2 graphPosition, float barWidth)
    {
        GameObject gameObject = new GameObject("bar", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color32(226, 153, 42, 255);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
        rectTransform.sizeDelta = new Vector2(barWidth, graphPosition.y);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0.5f, 0f);
        return gameObject;
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11f);
        rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraphBar(List<int> valueList, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }
        float graphHeigt = graphContainer.sizeDelta.y;

        float yMaximum = valueList[0];
        float yMinimum = valueList[0];

        foreach (int value in valueList)
        {
            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }
        yMaximum = yMaximum + ((yMaximum - yMinimum) * 0.2f);
        yMinimum = yMinimum + ((yMaximum - yMinimum) * 0.2f);
        yMinimum = 0f; //Start the graph at zero;

        float xSize = (graphContainer.rect.width - marginLR) / 7;

        int xIndex = 0;

        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = marginLR + xIndex * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeigt;
            GameObject barGameObject = CreateBar(new Vector2(xPosition, yPosition), xSize * 0.8f);

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.localScale = new Vector3(1, 1, 1);
            labelX.anchoredPosition = new Vector2(xPosition, -25f);
            labelX.GetComponent<TMP_Text>().text = getAxisLabelX(i);

            gameObjectList.Add(barGameObject);
            gameObjectList.Add(labelX.gameObject);
            xIndex++;
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            labelY.localScale = new Vector3(1, 1, 1);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-18f, normalizedValue * graphHeigt);
            labelY.GetComponent<TMP_Text>().text = getAxisLabelY(normalizedValue * yMaximum);
            gameObjectList.Add(labelY.gameObject);
        }
    }

    private void ShowGraph(List<int> valueList, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }
        float graphHeigt = graphContainer.sizeDelta.y;
        float yMaximum = graphContainer.rect.height;
        float xSize = graphContainer.rect.width / 7;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = marginLR + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeigt;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<TMP_Text>().text = getAxisLabelX(i);
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-18f, normalizedValue * graphHeigt);
            labelY.GetComponent<TMP_Text>().text = getAxisLabelY(normalizedValue * yMaximum);
        }
    }
    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }
}
