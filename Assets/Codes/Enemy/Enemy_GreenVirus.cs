using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using UnityEngine;

public class Enemy_GreenVirus : MonoBehaviour
{
    private GameObject me;
    private ChangeGravity cg;

    private Rigidbody rb;

    public GameObject player;

    private float speed = 0.01f;

    private float enTurn = -25f;
    //’¼‘O‚ÌˆÊ’u
    float oldPos = 0f;

    bool LR = false;

    bool isDeath = false;
    int deathTimer = 20;

    public AudioSource damage;
    public AudioSource down;

    private float knockBack = 1f;

    private StartSpinhit ssh;

    void Start()
    {
        TryGetComponent(out rb);
        me = this.gameObject;
        cg = me.GetComponent<ChangeGravity>();
        ssh = me.GetComponent<StartSpinhit>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!LR)
        {
            enTurn = -25f;
        }
        else
        {
            enTurn = 25f;
        }

        //ZŽ²ƒYƒŒ‰ñ”ð
        me.transform.position = new Vector3(me.transform.position.x, me.transform.position.y, 0f);

        if (!isDeath)
        {
            CgMove(cg.GetNum());

            if (cg.GetNum() < 2)
            {
                if (!LR)
                {
                    if (oldPos == me.transform.position.x)
                    {
                        LR = true;
                        speed = -speed;
                    }
                }
                else
                {
                    if (LR)
                    {
                        if (oldPos == me.transform.position.x)
                        {
                            LR = false;
                            speed = -speed;
                        }
                    }
                }
            }
            else
            {
                if (!LR)
                {
                    if (oldPos == me.transform.position.y)
                    {
                        LR = true;
                        speed = -speed;
                    }
                }
                else
                {
                    if (LR)
                    {
                        if (oldPos == me.transform.position.y)
                        {
                            LR = false;
                            speed = -speed;
                        }
                    }
                }
            }

            if (cg.GetNum() < 2)
            {
                oldPos = me.transform.position.x;
            }
            else
            {
                oldPos = me.transform.position.y;
            }
        }

        if (isDeath)
        {
            if (deathTimer > 0)
            {
                deathTimer--;
            }
            else
            {
                down.Play();
                me.gameObject.SetActive(false);
            }
        }
    }

    private void CgMove(int gravityNum)
    {
        if (gravityNum == 0)
        {
            me.transform.position += new Vector3(speed, 0f, 0f);
            me.transform.rotation = Quaternion.Euler(0f, enTurn, 0f);
        }
        else if (gravityNum == 1)
        {
            me.transform.position += new Vector3(-speed, 0f, 0f);
            me.transform.rotation = Quaternion.Euler(0f, -enTurn, 180f);
        }
        else if (gravityNum == 2)
        {
            me.transform.position += new Vector3(0f, -speed, 0f);
            me.transform.rotation = Quaternion.Euler(enTurn, 0f, -90f);
        }
        else if (gravityNum == 3)
        {
            me.transform.position += new Vector3(0f, -speed, 0f);
            me.transform.rotation = Quaternion.Euler(enTurn, 0f, 90f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            if (cg.GetNum() == 0)
            {
                if (!LR)
                {
                    if (collision.gameObject.transform.position.x > me.transform.position.x)
                    {
                        LR = true;
                        speed = -speed;
                    }
                }

                else
                {
                    if (collision.gameObject.transform.position.x < me.transform.position.x)
                    {
                        LR = false;
                        speed = -speed;
                    }
                }

                if(collision.gameObject.tag == "Player")
                {
                    rb.velocity = Vector3.zero;

                    Vector3 distination = (me.transform.position - collision.transform.position).normalized;

                    rb.AddForce(distination * knockBack, ForceMode.VelocityChange);
                }
            }
            else if (cg.GetNum() == 1)
            {
                if (!LR)
                {
                    if (collision.gameObject.transform.position.x < me.transform.position.x)
                    {
                        LR = true;
                        speed = -speed;
                    }
                }

                else
                {
                    if (collision.gameObject.transform.position.x > me.transform.position.x)
                    {
                        LR = false;
                        speed = -speed;
                    }
                }

                if (collision.gameObject.tag == "Player")
                {
                    rb.velocity = Vector3.zero;

                    Vector3 distination = (me.transform.position - collision.transform.position).normalized;

                    rb.AddForce(distination * knockBack, ForceMode.VelocityChange);
                }
            }
            else
            {
                if (!LR)
                {
                    if (collision.gameObject.transform.position.y < me.transform.position.y)
                    {
                        LR = true;
                        speed = -speed;
                    }
                }

                else
                {
                    if (collision.gameObject.transform.position.y > me.transform.position.y)
                    {
                        LR = false;
                        speed = -speed;
                    }
                }

                if (collision.gameObject.tag == "Player")
                {
                    rb.velocity = Vector3.zero;

                    Vector3 distination = (me.transform.position - collision.transform.position).normalized;

                    rb.AddForce(distination * knockBack, ForceMode.VelocityChange);
                }
            }
        }

        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().IsAttack())
            {
                isDeath = true;
                damage.Play();
                ssh.ParticleStart();
                if (collision.gameObject.tag == "Player")
                {
                    rb.velocity = Vector3.zero;

                    Vector3 distination = (me.transform.position - collision.transform.position).normalized;

                    rb.AddForce(distination * knockBack * 2, ForceMode.VelocityChange);
                }
            }
        }
        if (collision.gameObject.tag == "PAttack")
        {
            damage.Play();
            ssh.ParticleStart();
            isDeath = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "PAttack")
        {
            ssh.ParticleStart();
            isDeath =true;
            rb.velocity = Vector3.zero;

            Vector3 distination = (me.transform.position - other.transform.position).normalized;

            rb.AddForce(distination * knockBack * 2, ForceMode.VelocityChange);
            damage.Play();
        }
    }

    public bool IsDeath()
    {
        return isDeath;
    }
}
