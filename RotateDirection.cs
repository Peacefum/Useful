using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDirection : MonoBehaviour
{

    public GameObject m_player;
    public Transform m_target;

    public float m_rotspeed = 0.5f;
    public float m_movespeed = 0.5f;

    public bool isLock = false;

    public TargetMODE currentmode = TargetMODE.KeyMOD;
    public enum TargetMODE
    {
        KeyMOD,TargetMod,MouseMod
    }
    void Update()
    {
        ModeChange(currentmode);
    }
    public void ModeChange(TargetMODE mode)
    {
        switch(mode)
        {
            case TargetMODE.KeyMOD:
                if(Input.anyKey) // �밢�� �̵��� Ʋ�����°� ������
                PlayerMove();
                break;
            case TargetMODE.TargetMod:
                PlayerMove(m_target);
                break;
            case TargetMODE.MouseMod:
                PlayerMove(Input.mousePosition);
                break;
        }
    }

    public void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 movedir = new Vector3(h,0, v).normalized + m_player.transform.position;

        Debug.DrawRay(m_player.transform.position,movedir-m_player.transform.position,Color.blue,.5f);
        Vector3 targetDirection = movedir - m_player.transform.position;

        m_player.transform.Translate(targetDirection * Time.deltaTime * m_movespeed,Space.World); // self�� �������, world�� ������ü�� ���̰�


        float rotspeed = m_rotspeed * Time.deltaTime;
        Vector3 rotdir = Vector3.RotateTowards(m_player.transform.forward, targetDirection, rotspeed, 0.0f);

        Debug.DrawRay(m_player.transform.position, rotdir, Color.red,.5f); //  ���ư����� Ȯ��
        //���ʹϾ� Ȯ��
        m_player.transform.rotation = Quaternion.LookRotation(rotdir);
    }
    public void PlayerMove(Transform target)
    {
        Debug.DrawRay(m_player.transform.position, target.position - m_player.transform.position, Color.blue, .5f);
        Vector3 targetDirection = target.position - m_player.transform.position;

        m_player.transform.Translate(targetDirection * Time.deltaTime * m_movespeed, Space.Self); // self�� �������, world�� ������ü�� ���̰�

        float rotspeed = m_rotspeed * Time.deltaTime;
        Vector3 rotdir = Vector3.RotateTowards(m_player.transform.forward, targetDirection, rotspeed, 0.0f);

        Debug.DrawRay(m_player.transform.position, rotdir, Color.red, .5f); //  ���ư����� Ȯ��
        //���ʹϾ� Ȯ��
        m_player.transform.rotation = Quaternion.LookRotation(rotdir);

        Quaternion temprotation = m_player.transform.rotation;
    }
    public void PlayerMove(Vector3 mousepos)
    {
        //����Ʈ ���� 0,0 ���ϴ�, 1,1 ����
        Vector3 target = Camera.main.ScreenToWorldPoint(mousepos); // 

        target = new Vector3(target.x, m_player.transform.position.y, target.z) ;

        Debug.DrawRay(m_player.transform.position, target - m_player.transform.position, Color.blue, .5f);
        Vector3 targetDirection = target - m_player.transform.position;

  

        float rotspeed = m_rotspeed * Time.deltaTime;
        Vector3 rotdir = Vector3.RotateTowards(m_player.transform.forward, targetDirection, rotspeed, 0.0f);


        //���� ��
       float tt = Vector3.Angle(m_player.transform.forward , rotdir);
    //   float t =  Quaternion.Angle(m_player.transform.rotation, Quaternion.LookRotation(rotdir));

        Debug.Log("Ȯ�ο� " + tt);// ��� ���� ���Ҵ��� Ȯ��  ���� ������ �𸣰����� �׷����ϴ�

        Debug.DrawRay(m_player.transform.position, rotdir, Color.red, .5f); //  ���ư����� Ȯ��
        //���ʹϾ� Ȯ��
        m_player.transform.rotation = Quaternion.LookRotation(rotdir);

        if (tt == 0)
        {
            m_player.transform.Translate(targetDirection.normalized * Time.deltaTime * m_movespeed * 5, Space.World); // self�� �������, world�� ������ü�� ���̰�
        }
        else;
       // m_player.transform.Translate(targetDirection.normalized * Time.deltaTime * m_movespeed, Space.World); // self�� �������, world�� ������ü�� ���̰�

    }

}
