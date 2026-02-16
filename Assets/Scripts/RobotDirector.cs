using UnityEngine;

public class RobotDirector : MonoBehaviour
{
    public RobotAI robot;

    public float startSearchingAt = 0.25f;
    public float startChasingAt = 0.60f;

    private void Start()
    {
        if (robot == null) robot = FindFirstObjectByType<RobotAI>();

        GameManager.I.OnAlertChanged += Handle;
        Handle(GameManager.I.alert);
    }

    void Handle(float a)
    {
        if (robot == null) return;

        robot.detectionRange = Mathf.Lerp(8f, 20f, a);

        if (a >= startChasingAt) robot.ForceChase();
        else if (a >= startSearchingAt) robot.ForceSearch();
    }
}
