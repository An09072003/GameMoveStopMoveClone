using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetDetector : MonoBehaviour
{
    public float attackRange = 3f;
    public LayerMask botLayer;

    private List<GameObject> botsInRange = new List<GameObject>();

    void Start()
    {
        // Tắt tất cả TargetCircle của bot khi bắt đầu
        GameObject[] allBots = GameObject.FindGameObjectsWithTag("Bot");
        foreach (GameObject bot in allBots)
        {
            Transform circle = bot.transform.Find("TargetCircle");
            if (circle != null)
            {
                circle.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        // Tìm tất cả bot trong tầm
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, botLayer);
        List<GameObject> currentBots = new List<GameObject>();

        foreach (var hit in hitColliders)
        {
            GameObject bot = hit.gameObject;
            if (bot == null) continue;

            currentBots.Add(bot);

            if (!botsInRange.Contains(bot))
            {
                Transform circle = bot.transform.Find("TargetCircle");
                if (circle != null)
                    circle.gameObject.SetActive(true);
            }
        }

        // Tắt TargetCircle cho bot không còn trong tầm
        foreach (GameObject bot in botsInRange)
        {
            if (bot == null || !currentBots.Contains(bot))
            {
                // Nếu bị destroy thì bỏ qua
                if (bot == null) continue;

                Transform circle = bot.transform.Find("TargetCircle");
                if (circle != null)
                    circle.gameObject.SetActive(false);
            }
        }

        // Cập nhật lại danh sách, bỏ bot đã bị destroy
        botsInRange = currentBots;
    }


    // Vẽ gizmo trong Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
