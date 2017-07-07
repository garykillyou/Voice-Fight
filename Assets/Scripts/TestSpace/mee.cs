using UnityEngine;
using System.Collections;

public class mee : MonoBehaviour
{
    public GameObject circleModel;
    //旋转改变的角度
    public int changeAngle = 0;
    //旋转一周需要的预制物体个数
    private int count;

    private float angle = 0;
    public float r = 5;

    // Use this for initialization
    void Start()
    {
        count = (int)360 / changeAngle;
        for (int i = 0; i < count; i++)
        {
            Vector3 center = circleModel.transform.position;
            GameObject cube = (GameObject)Instantiate(circleModel);
            float hudu = (angle / 180) * Mathf.PI;
            float xx = center.x + r * Mathf.Cos(hudu);
            float zz = center.z + r * Mathf.Sin(hudu);
            cube.transform.position = new Vector3(xx, 0, zz);
            cube.transform.LookAt(center);
            angle += changeAngle;
        }
    }
}