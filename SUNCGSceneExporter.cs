#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace SUNCGData
{
    public class SUNCGSceneExporter
    {
        static void ExportJsonFromData(SUNCGDataStructure SUNCGdata)
        {
            //string content = JsonUtility.ToJson(SUNCGdata);
            string content = JsonConvert.SerializeObject(SUNCGdata,Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }).Replace('\n',' ');
            
            
            string lastPath = EditorPrefs.GetString("a4_OBJExport_lastPath", "");
            string lastFileName = EditorPrefs.GetString("a4_OBJExport_lastFile", "exportjson.json");
            string expFile = EditorUtility.SaveFilePanel("Export OBJ", lastPath, lastFileName, "json");
            if (expFile.Length > 0)
            {
                var fi = new System.IO.FileInfo(expFile);
                EditorPrefs.SetString("a4_OBJExport_lastFile", fi.Name);
                EditorPrefs.SetString("a4_OBJExport_lastPath", fi.Directory.FullName);
                
                
                byte[] databyte = Encoding.UTF8.GetBytes(content);
                Debug.Log(content);

                FileStream jsonFileStream = File.Create(expFile);
                jsonFileStream.Write(databyte, 0, databyte.Count());

            }
        }
        
        //Get Transform matrix base on the position of the object, index by Object id 
        static float[] getTransform(string id)
        {
            //Debug.Log(id);
            Transform objTrans = GameObject.Find(id).transform;
            Matrix4x4 m = Matrix4x4.TRS(objTrans.position,objTrans.rotation,objTrans.localScale);
            
            
            /*float[,] output = new float[4, 4]
            {
                {m.m00,m.m10,m.m20,m.m30},
                {m.m01,m.m11,m.m21,m.m31},
                {m.m02,m.m12,m.m22,m.m32},
                {m.m03,m.m13,m.m23,m.m33}
            };*/
            
            return new float[]{ m.m00,m.m10,m.m20,m.m30,m.m01,m.m11,m.m21,m.m31,m.m02,m.m12,m.m22,m.m32,m.m03,m.m13,m.m23,m.m33}
            ;
        }
        public static void ExportJSON(SUNCGDataStructure SUNCGdata, Node avaRoom)
        {
            foreach (int nodeid in avaRoom.nodeIndices)
            {
                //Debug.Log("Onbuilding "+ nodeid);
                for (int d = 0; d < SUNCGdata.levels[0].nodes.Length; d++)
                {
                    if (SUNCGdata.levels[0].nodes[d].id.Split('_')[1] == $"{nodeid}")
                    {
                        Node node = SUNCGdata.levels[0].nodes[d];
                        node.transform = matrixMul(node.transform,getTransform(node.id),4);
                        if (node.subItems != null)
                        {
                            foreach (SubNode subNode in node.subItems)
                            {
                                subNode.transform = matrixMul(subNode.transform,getTransform(subNode.id),4);
                                Debug.Log(subNode.transform);
                            }
                        }
                    }
                }
                
            }
            
            ExportJsonFromData(SUNCGdata);
            
        }

        public static float[] matrixMul(float[] a, float[] b,int len)
        {
            float[] o = new float[len*len];
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    o[i * 4 + j] = b[j] * a[i*4] + b[4+j] * a[i*4+1] + b[8+j] * a[i*4+2] + b[12+j] * a[i*4+3];

                }
            }
            return o;
        }
    }
}

/*Debug.Log(objTrans.localToWorldMatrix);
Debug.Log(objTrans.worldToLocalMatrix);
Debug.Log(objTrans.position);
Debug.Log(objTrans.localPosition);*/
#endif
