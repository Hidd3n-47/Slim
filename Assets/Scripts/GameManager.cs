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

    private bool gameStarted;
    private float currentTimer;

    private void Start()
    {
        currentTimer = startingTimeSeconds;

        StartCoroutine(StartGameEvent());
    }

    private void Update()
    {
        if (!gameStarted) return;

        currentTimer -= Time.deltaTime;
        timerText.text = $"{(int)currentTimer:0}.{Mathf.Floor(10.0f * (currentTimer - (int)currentTimer))}";
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
    }
}
