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

    public void AddTime(float time)
    {
        currentTimer += time;
    }

    private void Start()
    {
        currentTimer = startingTimeSeconds;

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
    }
}