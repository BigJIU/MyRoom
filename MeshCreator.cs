using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//如果添加的对象上没有MeshFilter，则给对象添加一个MeshFilter
[RequireComponent(typeof(MeshFilter))]
//如果添加的对象上没有MeshRenderer，则给对象添加一个MeshRenderer
[RequireComponent(typeof(MeshRenderer))]

public class MeshCreator: MonoBehaviour
{
	//实际操作的MeshFilter 
	private MeshFilter filter;
	//实际的Mesh
	private Mesh mesh;
	//记录顶点信息
	private List<Vector3> vts = new List<Vector3>();
	private List<Vector3> nos = new List<Vector3>();
	//自身的uv坐标
	private List<Vector2> uvs = new List<Vector2>();
	
	//三角形数组
	private int[] tris;

	public string modelName;
	
	//初始化
	private void Start(){
		
		filter = GetComponent<MeshFilter>();
		//得到每个顶点的坐标
		vts = GetVts(modelName);
		nos = GetNors(modelName);
		uvs = GetUV(modelName);
		//设置Mesh
		SetMesh();
	}
	

	
	//Get the list of vts
	private List<Vector3> GetVts(string name){
		List<Vector3> vals = new List<Vector3>();
		//should be locate by name...
		//TODO: Can be fix for old front
		float[] xyz = null;//SceneImporter.data.mesh[SceneImporter.getMeshByUid(name)].xyz;
		for (int i = 0; i < xyz.GetLength(0); i += 3)
		{
			vals.Add(new Vector3(xyz[i],xyz[i+1],xyz[i+2]));
		}
		//我们以挂载对象的位置作为起始点
		// Vector3 pos = transform.position;
		//
		// for(int i = 0 ; i < xCount ; i++){
		// 	for(int j = 0 ; j < yCount ; j++){
		// 		Vector3 vector =pos + new Vector3(i*xSeg,j*ySeg,0);
		// 		vals.Add(vector);
		// 	}
		// }
		return vals;
	}

	private List<Vector3> GetNors(string name)
	{
		List<Vector3> vals = new List<Vector3>();
		//TODO: Can be fix for old front
		float[] nor = null;//SceneImporter.data.mesh[SceneImporter.getMeshByUid(name)].normal;
		for (int i = 0; i < nor.GetLength(0); i += 3)
		{
			vals.Add(new Vector3(nor[i],nor[i+1],nor[i+2]));
		}
		return vals;
	}
	
	private List<Vector2> GetUV(string name)
	{
		List<Vector2> vals = new List<Vector2>();
		//TODO: Can be fix for old front
		float[] uv = null;//SceneImporter.data.mesh[SceneImporter.getMeshByUid(name)].uv;
		for (int i = 0; i < uv.GetLength(0); i += 2)
		{
			vals.Add(new Vector2(uv[i],uv[i+1]));
		}
		return vals;
	}
	
	

	//Set Mesh content
	private void SetMesh(){
		mesh = new Mesh();
		//将顶点传给mesh
		mesh.vertices = vts.ToArray();

		//将uv设置给mesh
		mesh.uv = uvs.ToArray();
		mesh.normals = nos.ToArray();
		//这三个方法是相当于对mesh用新数据进行重置
		
		mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        
        //将mesh交给filter
        filter.mesh = mesh;
	}
}
