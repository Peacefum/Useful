using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCreator : MonoBehaviour
{
    public Transform[] wallPos;
    public Material sturfeeShadow;
    public Material drawmat;

    public Transform wallParent;
    // Start is called before the first frame update
    void OnEnable()
    {
        GameObject gl = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
        GameObject qd = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Quad));

        for (int i=0; i< wallPos.Length-1; i++)
        {

            int j = i + 1;
            GameObject g =
                Instantiate(gl);
            g.transform.position = wallPos[i].position+ ( wallPos[j].position - wallPos[i].position) * 0.5f;
            var pos = g.transform.position;
            pos.y = 26.5f;
            g.transform.position = pos;
            g.transform.localScale = Vector3.right * Vector3.Distance(wallPos[i].position, wallPos[j].position)
                + Vector3.up * 5f + Vector3.forward * 1;

            Vector3 v = new Vector3(wallPos[i].position.x, 0, wallPos[i].position.z);
            Vector3 w = new Vector3(wallPos[j].position.x, 0, wallPos[j].position.z);
            g.transform.rotation = Quaternion.LookRotation(v
               - w);
            g.transform.Rotate(Vector3.up * 90);
            g.GetComponent<Renderer>().material = sturfeeShadow;
            g.name = "wall" + i;
            g.transform.parent = wallParent;

            GameObject q = Instantiate(qd);
            q.transform.position = g.transform.position + g.transform.forward * 0.05f;
            q.transform.rotation = g.transform.rotation;
            q.transform.localScale = g.transform.localScale;
            q.GetComponent<Renderer>().material = drawmat;
            q.transform.Rotate(180, 0, 0);
            q.transform.parent = wallParent;
            q.transform.gameObject.layer = 11;


        }
        //벽제조 프로토타입 
    }
}
