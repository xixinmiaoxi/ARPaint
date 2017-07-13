using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Area : MonoBehaviour {

    public GameObject Sun;
    public GameObject Penguin;//模型变量
    public GameObject SuccessImage;//声明识别成功显示的UI
    public Material Green_Matetial;//声明材质变量，存储绿色材质
    public Material Red_Material;//声明材质变量，存储红色材质
    public Material Trans_Material;//声明材质变量，存储透明材质

    private bool HasRec = false;//是否已识别图片信息

    private CanvasScaler CanScal;//控制UICanvas（屏幕自适度）缩放的变量
    private float C_Scale;//用来存储实际缩放比例
   

    private Vector2 TopLeft_UI;//记录扫描框左上角的屏幕坐标
    private Vector2 BottomLeft_UI;//记录扫描框左下角的屏幕坐标
    private Vector2 TopRight_UI;//记录扫描框右上角的屏幕坐标
    private Vector2 BottomRight_UI;//记录扫描框右下角的屏幕坐标

    private Vector3 TopLeft_Plane_W;//记录面片左上角的世界坐标
    private Vector3 BottomLeft_Plane_W;//记录面片左下角的世界坐标
    private Vector3 TopRight_Plane_W;//记录面片右上角的世界坐标
    private Vector3 BottomRight_Plane_W;//记录面片右下角的世界坐标

    private Vector2 PlaneWH;//记录面片的宽高

    //记录面片的屏幕坐标
    private Vector2 TopLeft_Plane_Sc;
    private Vector2 BottomLeft_Plane_Sc;
    private Vector2 TopRight_Plane_Sc;
    private Vector2 BottomRight_Plane_Sc;


   
	// Use this for initialization
	void Start ()
    {
        CanScal = GameObject.Find("Canvas").gameObject.GetComponent<CanvasScaler>();//找到Canvas获得Canvas上的CanvasScaler（控制屏幕自适度）组件
        C_Scale = Screen.width / CanScal.referenceResolution.x;//屏幕的宽度除以预设的宽度，得到UICanvas的缩放比例


        //计算扫描框四个点的坐标位置，并赋值
        TopLeft_UI = new Vector2(Screen.width - 400 * C_Scale, Screen.height + 300 * C_Scale) * 0.5f;
        BottomLeft_UI = new Vector2(Screen.width - 400 * C_Scale, Screen.height - 300 * C_Scale) * 0.5f;
        TopRight_UI = new Vector2(Screen.width + 400 * C_Scale, Screen.height + 300 * C_Scale) * 0.5f;
        BottomRight_UI = new Vector2(Screen.width + 400 * C_Scale, Screen.height - 300 * C_Scale) * 0.5f;
    }

    // Update is called once per frame
    void Update ()
    {
        
        //获取面片宽高的一半 ,  "gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x"获取面片X方向的宽度
        PlaneWH = new Vector2(gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x * 60f, gameObject.GetComponent<MeshFilter>().mesh.bounds.size.z * 31.8f) * 0.5f;
       
        //获取面片四个点的世界坐标 , "gameObject.transform.parent.position"父物体的世界坐标
        TopLeft_Plane_W = gameObject.transform.parent.position + new Vector3(-PlaneWH.x, 0.001f, PlaneWH.y);
        BottomLeft_Plane_W = gameObject.transform.parent.position + new Vector3(-PlaneWH.x, 0.001f, -PlaneWH.y);
        TopRight_Plane_W = gameObject.transform.parent.position + new Vector3(PlaneWH.x, 0.001f, PlaneWH.y);
        BottomRight_Plane_W = gameObject.transform.parent.position + new Vector3(PlaneWH.x, 0.001f, -PlaneWH.y);

        //将上面获取到的面片的世界坐标转换为屏幕坐标，并赋值给已声明变量
        TopLeft_Plane_Sc = Camera.main.WorldToScreenPoint(TopLeft_Plane_W);
        BottomLeft_Plane_Sc = Camera.main.WorldToScreenPoint(BottomLeft_Plane_W);
        TopRight_Plane_Sc = Camera.main.WorldToScreenPoint(TopRight_Plane_W);
        BottomRight_Plane_Sc = Camera.main.WorldToScreenPoint(BottomRight_Plane_W);


        //判断面片是否在扫描框内
        if (TopLeft_Plane_Sc.x > TopLeft_UI.x && TopLeft_Plane_Sc.y < TopLeft_UI.y && BottomLeft_Plane_Sc.x > BottomLeft_UI.x
            && BottomLeft_Plane_Sc.y > BottomLeft_UI.y && TopRight_Plane_Sc.x < TopRight_UI.x && TopRight_Plane_Sc.y < TopRight_UI.y
            && BottomRight_Plane_Sc.x < BottomRight_UI.x && BottomRight_Plane_Sc.y > BottomRight_UI.y) 
        {
            if (HasRec == false)//尚未识别，开始执行下面识别程序
            {
                //当面片完全处于扫描框内时，执行下列代码
                gameObject.GetComponent<Renderer>().material = Green_Matetial;//将材质所附着物体（此处是Plane)的材质更改为 Green_Material

                // Penguin.GetComponent<MeshRenderer>().material = Trans_Material;//不可以

                Penguin.GetComponent<MeshRenderer>().enabled = false;

                StopAllCoroutines();

                StartCoroutine("SuccessUI");//调用显示“识别成功”UI的延迟函数SuccessUI

                StartCoroutine("ScreenShot");//调用截图的延迟函数

                //StartCoroutine("ShowSun");

                HasRec = true;//识别完成，将HasRec设为true
            }
        }
        else
        {
            //当面片并非完全处于扫描框内时，执行下列代码
            gameObject.GetComponent<Renderer>().material = Red_Material;//将材质所附着物体（此处是Plane)的材质更改为 Red_Material

            HasRec = false;//未识别或者识别后截图前又移出，则又变为未识别。将HasRec设为false。

            //Sun.SetActive(false);
        }


	}

    /// <summary>
    /// 显示“识别成功”UI的延迟函数SuccessUI
    /// </summary>
    /// <returns></returns>
    IEnumerator SuccessUI()
    {
        yield return new WaitForSeconds(0.5f);//延迟0.5秒

        SuccessImage.SetActive(true);//激活识别成功的UI

        gameObject.GetComponent<Renderer>().material = Trans_Material;//将面片材质设为透明
    }


    /// <summary>
    /// 截图的延迟函数
    /// </summary>
    /// <returns></returns>
    IEnumerator ScreenShot()
    {
        yield return new WaitForSeconds(2.0f);//延迟2秒

        if(HasRec == true)//当时别状态时才可执行截图
        { 
            gameObject.GetComponent<Renderer>().material = Trans_Material;//将面片材质设为透明，为避免0.5-2秒之间误操作，再来一次
           
            Penguin.GetComponent<ScreenShot>().ScreenShot_Btn();//调用模型上的截图方法
           
        }
        Penguin.GetComponent<MeshRenderer>().enabled = true ;
    }
    /// <summary>
    /// 延迟显示太阳系
    /// </summary>
    /// <returns></returns>
    //IEnumerator ShowSun()
    //{
    //    yield return new WaitForSeconds(2.5f);//延迟5秒

    //    Sun.SetActive(true);//激活太阳系
    //}
}
