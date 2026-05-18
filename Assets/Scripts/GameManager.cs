using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnGameStartEvent;
    [SerializeField] private float gameStartEventDelay = 2.0f;

    [SerializeField] private float startingTimeSeconds = 30.0f;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private Transform endUi;

    private bool gameStarted;
    private float currentTimer;

    public void AddTime(float time)
    {
        currentTimer += time;
    }

    public void EndPlaytest()
    {
        gameStarted = false;
        timerText.transform.parent = endUi;
        timerText.transform.localPosition = Vector3.zero;
        timerText.transform.localScale = Vector3.one * 1.5f;
        timerText.rectTransform.localPosition = Vector3.zero;
    }

    private void Start()
    {
        currentTimer = startingTimeSeconds;
        timerText.gameObject.SetActive(false);
        StartCoroutine(StartGameEvent());
    }

    private void Update()
    {
        if (!gameStarted) return;

        currentTimer -= Time.deltaTime;
        timerText.text = $"{(int)(currentTimer / 60.0f):0}:{(int)currentTimer % 60:00}";
    }

    private IEnumerator StartGameEvent()
    {
        float timer = gameStartEventDelay;

        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        OnGameStartEvent?.Invoke();
        gameStarted = true;
        timerText.gameObject.SetActive(true);
    }
}