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
    GameObject[] exp;
    ParticleSystem[] par;
    GameObject fire;
    // Start is called before the first frame update
    void Start()
    {
        if (is3D)
        {
            exp = new GameObject[6];
            par = new ParticleSystem[6];
        }
        else
        {
            exp = new GameObject[4];
            par = new ParticleSystem[4];
        }
        fuseBurn = 0.2f / maxLifetime;
        lifetime = maxLifetime;
        fire = Instantiate(fireEffect, this.transform.position+new Vector3(0,0.5f,0), Quaternion.identity);
    }
    void CreateParticleEffect(int id, float angle)
    {
        if(id<4)
            exp[id] = Instantiate(explosionEffect, this.transform.position, Quaternion.Euler(0, angle, 0));
        else
            exp[id] = Instantiate(explosionEffect, this.transform.position, Quaternion.Euler(angle, 0, 0));
        par[id] = exp[id].gameObject.GetComponent<ParticleSystem>();
        var main = par[id].main;
        main.startLifetime = 0.02f * range;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        lifetime--;
        fuse.gameObject.transform.position -= new Vector3(0,fuseBurn,0);
        fire.gameObject.transform.position = fuse.gameObject.transform.position+new Vector3(0,0.3f,0);
        if (lifetime < -30
            && xExplosion == null
            && yExplosion == null
            && zExplosion == null)
        {
            for (int i = 0; i < exp.Length; i++)
                Destroy(exp[i].gameObject);
            Destroy(fire.gameObject);
            Destroy(this.gameObject);
        }
        if(lifetime==0)
        {
            this.gameObject.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            CreateParticleEffect(0, 0);
            CreateParticleEffect(1, 90);
            CreateParticleEffect(2, 180);
            CreateParticleEffect(3, 270);
            if (is3D)
            {
                CreateParticleEffect(4, 90);
                CreateParticleEffect(5, 270);
            }
            foreach(ParticleSystem ps in par)
            {
                ps.Play();
            }
        }
    }
}
