using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Playing,
    WaveClear,
    AllClear,
    GameOver,
    Paused
}

public class GameManager : MonoBehaviour
{
    private float HP { get; set; }
    int maxEnemyBox = 0;
    public static int leftEnemyBox = 0;
    [SerializeField] private float MaxHP = 10;
    [SerializeField] GameObject BGM;
    [SerializeField] GameObject lifeGage;
    [SerializeField] GameObject charaImg;
    [SerializeField] GameObject spawner;
    [SerializeField] GameObject bulletTypeUI;
    [SerializeField] GameObject leftEnemyTextObject;
    [SerializeField] GameObject getTimeTextObject;
    [SerializeField] GameObject clearTextObject;
    [SerializeField] GameObject Button;
    TextMeshProUGUI leftEnemyBoxText;
    TextMeshProUGUI timeText;
    TextMeshProUGUI clearText;



    Image _blackfade;
    [SerializeField] AudioClip typeChangeSound;
    AudioSource audioSource;

    [SerializeField] BulletType bulletType;
    private Image characterImage;
    private Image lifeImage;
    private Image BulletTypeImage;
    float time = 0;
    float clearTime = 0;
    public static bool cleared = false;

    UnityEvent _getDamage;

    // Start is called before the first frame update
    void Start()
    {
        time += Time.deltaTime;
        maxEnemyBox = GameObject.FindGameObjectsWithTag("EnemyTurret").Length;
        leftEnemyBox = maxEnemyBox;
        HP = MaxHP;
        leftEnemyBoxText = leftEnemyTextObject.GetComponent<TextMeshProUGUI>();
        timeText = getTimeTextObject.GetComponent<TextMeshProUGUI>();
        clearText = clearTextObject.GetComponent<TextMeshProUGUI>();
        lifeImage = lifeGage.GetComponent<Image>();
        characterImage = charaImg.GetComponent<Image>();
        BulletTypeImage = bulletTypeUI.GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        GameObject blackFade = GameObject.Find("BlackFade");
        _blackfade = blackFade.GetComponent<Image>();
        StartCoroutine("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            audioSource.PlayOneShot(typeChangeSound);
            if (bulletType == BulletType.Explode)
            {
                bulletType = BulletType.Normal;
            }
            else
            {
                bulletType++;
            }
        }
        switch (bulletType)
        {
            case BulletType.Normal:
                BulletTypeImage.color = new Color(1, 1, 1, 1);
                break;
            case BulletType.Penetration:
                BulletTypeImage.color = new Color(0.2f, 0.4f, 1, 1);
                break;
            case BulletType.Explode:
                BulletTypeImage.color = new Color(1, 0.4f, 0.2f, 1);
                break;
        }
        if (!cleared)
        {
            time += Time.deltaTime;
        }
        leftEnemyBoxText.SetText(leftEnemyBox.ToString() + " / " + maxEnemyBox.ToString());
        timeText.SetText("TIME : {0}", Mathf.Round(time * 100.0f) / 100);

        if (leftEnemyBox <= 0)
        {
            cleared = true;
            GameObject.Find("BGM").GetComponent<AudioSource>().Pause();
            clearTime = time;
            clearText.SetText("Clear!\ntime:" + clearTime);
            clearTextObject.SetActive(true);
            Button.SetActive(true);
            StartCoroutine("BackToTitle");
        }
    }

    public void GetDamage_Heal(int change_HP)
    {
        HP += change_HP;
        characterImage.color = change_HP >= 0 ? new Color(0, 1, 0, 1) : new Color(1, 0, 0, 1);
        if (HP > MaxHP) { HP = MaxHP; }
        lifeImage.fillAmount = HP / MaxHP;
        characterImage.DOColor(new Color(1, 1, 1), 0.8f);
    }

    public void BulletShoot()
    {
        GameObject bullet = bulletType switch
        {

        };

        Instantiate(bullet, new Vector3(spawner.transform.position.x, spawner.transform.position.y, spawner.transform.position.z), Quaternion.identity);
    }

    IEnumerator FadeIn()
    {
        _blackfade.color = new Color32(0, 0, 0, 255);
        for (int i = 0; i < 51; i++)
        {
            _blackfade.color -= new Color32(0, 0, 0, 5);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator FadeOut()
    {
        for (int i = 0; i < 51; i++)
        {
            _blackfade.color += new Color32(0, 0, 0, 5);
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator BackToTitle()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < 51; i++)
        {
            _blackfade.color += new Color32(0, 0, 0, 5);
            yield return new WaitForSeconds(0.05f);
        }
        SceneManager.LoadScene("Title");
    }
    public enum BulletType
    {
        Normal,
        Penetration,
        Explode
    }
}
