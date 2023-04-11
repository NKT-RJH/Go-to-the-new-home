using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatKey : MonoBehaviour
{
    public static int stage = 1;
    private void Update()
    {
        if (!TimeManager.isStart) return;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ShootPlayerBullet.level = 4;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            try
            {
                PlayerSkill playerSkill = FindObjectOfType<PlayerSkill>();

                playerSkill.countBombCoolTime = 0;
                playerSkill.countRepairCoolTime = 0;
                playerSkill.countPowerUPCoolTime = 0;
                playerSkill.countBeWaterCoolTime = 0;
                playerSkill.countSunPowerCoolTime = 0;
            }
            catch (System.NullReferenceException) { }
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            PlayerStatus.hp = PlayerStatus.maxHP;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            PlayerStatus.gage = PlayerStatus.maxGage;
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SceneManager.LoadScene(string.Format("Stage{0}", stage == 3 ? stage = 1 : ++stage));
        }
    }
}
