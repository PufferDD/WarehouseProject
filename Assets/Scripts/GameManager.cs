using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    [Header("Progress (Warehouse Tasks -> Exit)")]
    public int tasksDone = 0;
    public int requiredTasks = 3;
    public bool exitUnlocked = false;

    [Header("Control Room (Logs -> Terminal)")]
    public int logsFound = 0;
    public int requiredLogs = 3;
    public bool terminalUnlocked = false;

    [Header("Threat")]
    [Range(0f, 1f)]
    public float alert = 0f; // 0=rauhallinen, 1=tappomoodi

    [Tooltip("Robot starts chasing only when alert >= this.")]
    [Range(0f, 1f)]
    public float aggressionThreshold = 0.4f;

    [Header("Game Over")]
    public bool gameOver = false;

    [Header("UI")]
    public string currentObjective = "Complete tasks (0/3)";

    // Events
    public event Action<float> OnAlertChanged;
    public event Action<int> OnTasksChanged;
    public event Action<int> OnLogsChanged;
    public event Action<string> OnObjectiveChanged;
    public event Action<bool> OnExitUnlockedChanged;
    public event Action<bool> OnTerminalUnlockedChanged;

    public bool IsAggressive => alert >= aggressionThreshold;

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
        RefreshObjective();
        OnAlertChanged?.Invoke(alert);
        OnTasksChanged?.Invoke(tasksDone);
        OnLogsChanged?.Invoke(logsFound);
    }

    // =========================
    // WAREHOUSE TASKS -> EXIT
    // =========================
    public void CompleteTask(int alertIncreasePercent = 25)
    {
        if (gameOver) return;
        if (exitUnlocked) return; // ei enää kasvateta yli

        tasksDone++;
        OnTasksChanged?.Invoke(tasksDone);

        AddAlert(alertIncreasePercent / 100f);

        if (tasksDone >= requiredTasks)
        {
            exitUnlocked = true;
            OnExitUnlockedChanged?.Invoke(true);
        }

        RefreshObjective();
    }

    // =========================
    // LOGS -> TERMINAL (optional)
    // =========================
    public void AddLog()
    {
        if (gameOver) return;
        if (terminalUnlocked) return;

        logsFound++;
        OnLogsChanged?.Invoke(logsFound);

        if (logsFound >= requiredLogs)
        {
            terminalUnlocked = true;
            OnTerminalUnlockedChanged?.Invoke(true);
        }

        RefreshObjective();
    }

    // =========================
    // THREAT / ALERT
    // =========================
    public void AddAlert(float amount01)
    {
        float before = alert;
        alert = Mathf.Clamp01(alert + amount01);

        if (!Mathf.Approximately(before, alert))
            OnAlertChanged?.Invoke(alert);

        Debug.Log($"ALERT={alert:0.00} Aggro={IsAggressive} TASKS={tasksDone}/{requiredTasks} LOGS={logsFound}/{requiredLogs}");
    }

    // =========================
    // GAME OVER
    // =========================
    public void SetGameOver()
    {
        gameOver = true;
        PushObjective("Caught! Respawning...");
    }

    // =========================
    // OBJECTIVE
    // =========================
    private void RefreshObjective()
    {
        if (!exitUnlocked)
        {
            PushObjective($"Complete tasks ({tasksDone}/{requiredTasks})");
            return;
        }

        // Exit on auki -> seuraava vaihe
        if (!terminalUnlocked)
        {
            // jos käytät logeja control roomissa
            PushObjective("Exit unlocked! Go to Control Room.");
            return;
        }

        PushObjective("Terminal unlocked. Proceed.");
    }

    private void PushObjective(string text)
    {
        currentObjective = text;
        OnObjectiveChanged?.Invoke(text);
    }
}


