using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour {

    public GameObject Plane;//声明面片名称为Plane

    private int ScreenWidth;
    //记录屏幕宽度

    private int ScreenHeight;
    //记录屏幕高度

    private Texture2D TextureShot;
    //存储屏幕截图

    private Vector2 PlaneWH;//记录面片的宽高

    private Vector3 TopLeft_Plane_W;//记录面片左上角的世界坐标
    private Vector3 BottomLeft_Plane_W;//记录面片左下角的世界坐标
    private Vector3 TopRight_Plane_W;//记录面片右上角的世界坐标
    private Vector3 BottomRight_Plane_W;//记录面片右下角的世界坐标

    // Use this for initialization
    void Start ()
    {
        ScreenWidth = Screen.width;//获取屏幕宽度
        ScreenHeight = Screen.height;//获取屏幕高度

        TextureShot = new Texture2D(ScreenWidth, ScreenHeight, TextureFormat.RGB24, false);
        //Texture2D(int width, int height, TextureFormat format, bool mipmap)
        //TextureFormat format 颜色模式  RGB24没有透明通道  RGBA32有透明通道
        //bool mipmap 一种分级纹理 在屏幕中远近大小不同时给予不同级别的纹理 此处不用
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public void ScreenShot_Btn()
    {
        PlaneWH = new Vector2(Plane.GetComponent<MeshFilter>().mesh.bounds.size.x * 60f, Plane.GetComponent<MeshFilter>().mesh.bounds.size.z * 31.8f) * 0.5f;
        // 获取面片宽高的一半 

        //获取面片四个点的世界坐标
        TopLeft_Plane_W = Plane.transform.parent.position + new Vector3(-PlaneWH.x, 0.001f, PlaneWH.y);
        BottomLeft_Plane_W = Plane.transform.parent.position + new Vector3(-PlaneWH.x, 0.001f, -PlaneWH.y);
        TopRight_Plane_W = Plane.transform.parent.position + new Vector3(PlaneWH.x, 0.001f, PlaneWH.y);
        BottomRight_Plane_W = Plane.transform.parent.position + new Vector3(PlaneWH.x, 0.001f, -PlaneWH.y);

        //将截图时识别图四个角的世界坐标信息传递给Shader
        gameObject.GetComponent<Renderer>().material.SetVector("_Uvpoint1", new Vector4(TopLeft_Plane_W.x, TopLeft_Plane_W.y, TopLeft_Plane_W.z, 1f));
        //将左上角的世界坐标传递给Shader ，其中1f是否了凑齐四位浮点数 ，用来进行后续的矩阵变换操作
        gameObject.GetComponent<Renderer>().material.SetVector("_Uvpoint2", new Vector4(BottomLeft_Plane_W.x, BottomLeft_Plane_W.y, BottomLeft_Plane_W.z, 1f));
        gameObject.GetComponent<Renderer>().material.SetVector("_Uvpoint3", new Vector4(TopRight_Plane_W.x, TopRight_Plane_W.y, TopRight_Plane_W.z, 1f));
        gameObject.GetComponent<Renderer>().material.SetVector("_Uvpoint4", new Vector4(BottomRight_Plane_W.x, BottomRight_Plane_W.y, BottomRight_Plane_W.z, 1f));

        Matrix4x4 ProjectionMatrix = GL.GetGPUProjectionMatrix(Camera.main.projectionMatrix,false);
        //获取截图时GPU的投影矩阵
        Matrix4x4 CameraMatrix = Camera.main.worldToCameraMatrix;
        //获取截图时世界坐标到相机的矩阵
        Matrix4x4 CP = ProjectionMatrix * CameraMatrix;
        //存储两个矩阵的乘积
        gameObject.GetComponent<Renderer>().material.SetMatrix("_CP",CP);

        TextureShot.ReadPixels(new Rect(0, 0, ScreenWidth, ScreenHeight), 0, 0);
        //获取屏幕的像素信息
        // public void ReadPixels(Rect source, int destX, int destY);
        //“int destX, int destY”即第二个"0,0"是填充Texture2D时的起始填充坐标
        //Rect(0, 0, ScreenWidth, ScreenHeight)里面"0,0"是获取屏幕像素的起始点
        //"ScreenWidth, ScreenHeight"获取屏幕像素的范围

        TextureShot.Apply();
        //确认之前对Texture的修改，如此才可把截取到的Texture存储下来

        gameObject.GetComponent<Renderer>().material.mainTexture = TextureShot;
        //获取模型渲染组件中材质的主纹理，并将截图赋值给主纹理
    }
}
