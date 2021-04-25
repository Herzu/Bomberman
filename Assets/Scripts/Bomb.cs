using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int maxLifetime;
    int lifetime;
    float fuseBurn;
    public int range;
    public GameObject xExplosion;
    public GameObject yExplosion;
    public GameObject zExplosion;
    public GameObject fuse;
    public GameObject explosionEffect;
    public GameObject fireEffect;
    public GameObject playerCollider;
    public bool is3D;
    GameObject exp1, exp2, exp3, exp4, exp5, exp6,fire;
    // Start is called before the first frame update
    void Start()
    {
        if(!is3D)
            playerCollider.gameObject.GetComponent<SphereCollider>().radius = 0.0f;
        fuseBurn = 0.2f / maxLifetime;
        lifetime = maxLifetime;
        fire = Instantiate(fireEffect, this.transform.position+new Vector3(0,0.5f,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        lifetime--;
        fuse.gameObject.transform.position -= new Vector3(0,fuseBurn,0);
        fire.gameObject.transform.position = fuse.gameObject.transform.position+new Vector3(0,0.3f,0);
        if (lifetime == maxLifetime - 100)
            playerCollider.gameObject.GetComponent<SphereCollider>().radius = 0.5f;
        else if (lifetime < -50
            && xExplosion == null
            && yExplosion == null
            && zExplosion == null)
        {
            Destroy(exp1.gameObject);
            Destroy(exp2.gameObject);
            Destroy(exp3.gameObject);
            Destroy(exp4.gameObject);
            if(is3D)
            {
                Destroy(exp5.gameObject);
                Destroy(exp6.gameObject);
            }
            Destroy(fire.gameObject);
            Destroy(this.gameObject);
        }
        if(lifetime==0)
        {
            this.gameObject.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            exp1 = Instantiate(explosionEffect, this.transform.position, Quaternion.Euler(0, 0, 0));
            exp2 = Instantiate(explosionEffect, this.transform.position, Quaternion.Euler(0,90,0));
            exp3 = Instantiate(explosionEffect, this.transform.position, Quaternion.Euler(0, 180, 0));
            exp4 = Instantiate(explosionEffect, this.transform.position, Quaternion.Euler(0, 270, 0));
            ParticleSystem ps1 = exp1.gameObject.GetComponent<ParticleSystem>();
            ParticleSystem ps2 = exp2.gameObject.GetComponent<ParticleSystem>();
            ParticleSystem ps3 = exp3.gameObject.GetComponent<ParticleSystem>();
            ParticleSystem ps4 = exp4.gameObject.GetComponent<ParticleSystem>();
            var main = ps1.main;
            main.startLifetime = 0.02f * range;
            main = ps2.main;
            main.startLifetime = 0.02f * range;
            main = ps3.main;
            main.startLifetime = 0.02f * range;
            main = ps4.main;
            main.startLifetime = 0.02f * range;
            if (is3D)
            {
                exp5 = Instantiate(explosionEffect, this.transform.position, Quaternion.Euler(90, 0, 0));
                exp6 = Instantiate(explosionEffect, this.transform.position, Quaternion.Euler(270, 0, 0));
                ParticleSystem ps5 = exp5.gameObject.GetComponent<ParticleSystem>();
                ParticleSystem ps6 = exp6.gameObject.GetComponent<ParticleSystem>();
                main = ps5.main;
                main.startLifetime = 0.02f * range;
                main = ps6.main;
                main.startLifetime = 0.02f * range;
                ps5.Play();
                ps6.Play();
            }
            ps1.Play();
            ps2.Play();
            ps3.Play();
            ps4.Play();
        }
    }
}
