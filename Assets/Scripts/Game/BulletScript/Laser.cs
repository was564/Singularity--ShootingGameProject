using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public AudioClip charging = null;
    public AudioClip fireLaser = null;

    public GameObject player = null;

    public GameObject shooter = null;
    public bool isUsed { get; set; } = false;
    // normalizing하고 넣기

    private float laser_length = 40.0f;
    private float timer = 0.0f;
    public float activate_time = 2.0f;
    public float maintain_time = 1.0f;
    public float stopping_time = 1.0f;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.Find("Player");
        }
        this.transform.position = Vector3.up * 10;
        audio = this.GetComponent<AudioSource>();
        this.audio.volume = GameObject.Find("GameRoot").GetComponent<SceneControl>().soundSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (shooter.transform.position.y < -1) return;
        if (!isUsed)
        {
            audio.Stop();
            setShowLaser(false);
            this.transform.position = Vector3.up * (-10);
            timer = 0;
            return;
        }
        timer += Time.deltaTime;

        if (timer > activate_time)
        {
            if (!audio.isPlaying || (audio.isPlaying && !audio.Equals(fireLaser)))
            {
                audio.Stop();
                audio.clip = fireLaser;
                audio.Play();
            }
            if (timer > maintain_time + activate_time)
            { 
                setShowLaser(false); isUsed = false;
                if (audio.isPlaying) audio.Stop();
            }
            setTrigger(true);
            float scale = Mathf.Clamp01(timer - (activate_time - stopping_time));
            this.transform.localScale =
                new Vector3(scale*0.5f, laser_length, scale*0.5f);
        }
        else if (timer < activate_time - stopping_time)
        {
            if (!audio.isPlaying)
            {
                audio.clip = charging;
                audio.Play();
            }
            setTrigger(false);
            this.transform.position = this.shooter.transform.position +
                shooter.transform.forward * (laser_length / 2);
            this.transform.LookAt(player.transform);
            this.transform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));
        }
    }

    public Vector3 getPosition()
    {
        return this.transform.position;
    }

    public void setPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    public void setShowLaser(bool show)
    {
        this.GetComponent<MeshRenderer>().enabled = show;
        this.transform.localScale = new Vector3(0.1f, laser_length, 0.1f);
    }

    public void setTrigger(bool isTrigger)
    {
        this.GetComponent<BoxCollider>().enabled = isTrigger;
    }

    public void activateLaser(bool isActive)
    {
        setShowLaser(isActive);
        this.isUsed = isActive;
    }
}
