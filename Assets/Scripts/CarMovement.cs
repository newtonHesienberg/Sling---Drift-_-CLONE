using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour
{
    [Header("Game Physics")]

    public float drivingSpeed;
    public float drivingAngle;

    public float minDistance;
    public float rope_radius;

    public GameObject[] nodes;
    public int index = -1;  

    public float angular_velocity;

    public float target_angle;
    public float rope_angle;

    public float zAisValue;
    public float changeDrivingAngle;
    bool driftInfo = false;

    [Header("Refrences")]

    public GameObject removableBridge;

    public LineRenderer lineRenderer;

    [Header("Graphics & UI")]

    public AudioClip blastSFX;
    public GameObject blastVFX , blastVFX2;
    float SFXSoundVolume = 0.7f;
    public GameObject deathUI;

    public AudioSource engineSFX;

    public Text scoreText;
    public Text highScoreText;
    int score;


    private void Start()
    {
        engineSFX.Play();
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    private void Update()
    {
        scoreText.text = score.ToString();

        if(score > PlayerPrefs.GetInt("HighScore",0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreText.text = score.ToString();
        }

        if (rope_radius <= minDistance && Input.GetButton("Fire1"))
        {

            DriftState();
            driftInfo = true;
        } 
        else
        {
            driftInfo = false;
            lineRenderer.enabled = false;
            transform.position += new Vector3(Mathf.Sin(drivingAngle * (Mathf.PI / 180)) * drivingSpeed * Time.deltaTime, Mathf.Cos(drivingAngle * (Mathf.PI / 180)) * drivingSpeed * Time.deltaTime, 0);
           
            rope_radius = Vector3.Distance(nodes[index].transform.position, transform.position);
        }


    }

    public void DriftState()
    {
        lineRenderer.enabled = true;

          transform.RotateAround(nodes[index].transform.position, Vector3.forward, angular_velocity * Time.deltaTime);
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 0));
        lineRenderer.SetPosition(1, new Vector3(nodes[index].transform.position.x, nodes[index].transform.position.y, 0f));


        if (transform.rotation.eulerAngles.z >= zAisValue - 7)
        {
            if (zAisValue == 90)
                drivingAngle = zAisValue + 90;
            else if (zAisValue == -90)
                drivingAngle = -zAisValue;
            else if (zAisValue == -180)
                drivingAngle = -zAisValue;
            else if (zAisValue == 0)
                drivingAngle = -zAisValue;
            else if (zAisValue == 180)
                drivingAngle = zAisValue + 90;

            StartCoroutine(FixOreintation());
        }

        
    }

    IEnumerator FixOreintation()
    {
        yield return new WaitForSeconds(1);
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.x, -90);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("T1"))
        {
            angular_velocity = 140;
            zAisValue = 90;
            index++;
            score++;
        }
        if (collision.CompareTag("T2"))
        {
            zAisValue = -90;
            index++;
            score++;
        }
        if (collision.CompareTag("T3"))
        {
            removableBridge.SetActive(true);
            angular_velocity = -140;
            zAisValue -= 90;
            index++;
            score++;
        }
        if (collision.CompareTag("T4"))
        {
            angular_velocity = 140;
            zAisValue = 0;
            index++;
            score++;
        }
        if (collision.CompareTag("T5"))
        {
            angular_velocity = -140;
            zAisValue -= 90;
            index++;
            score++;
        }
        if (collision.CompareTag("T6"))
        {
            angular_velocity = 140;
            zAisValue = 0;
            index++;
            score++;
        }
        if (collision.CompareTag("T7"))
        {
            angular_velocity = -140;
            zAisValue = 180;
            minDistance += .5f;
            index ++;
            score++;
        }
        if (collision.CompareTag("T8"))
        {
            angular_velocity = -140;
            minDistance -= .5f;
            index = -1;
        }
        if(collision.CompareTag("RemoveBridge"))
        {
            removableBridge.SetActive(false);
        }

        if (collision.CompareTag("Track"))
        {
            Blast();
        }

    }

    public void Blast()
    {
        Instantiate(blastVFX, transform.position, Quaternion.identity);
        Instantiate(blastVFX2, transform.position, Quaternion.identity);
        deathUI.SetActive(true);
        AudioSource.PlayClipAtPoint(blastSFX, Camera.main.transform.position, SFXSoundVolume);
        Destroy(gameObject);
        Time.timeScale = 0f;
    }

    public bool GetDriftInfo()
    {
        return driftInfo;
    }
}
