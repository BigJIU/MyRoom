
using System;
using System.Linq;
using Dummiesman;
using Unity.VisualScripting;
using UnityEngine;

namespace FrontData
{
    public class FrontSceneBuilder
    {

        /// <summary>
        /// Main method of Scene Builder, which will call Wall/FurBuild if needed.
        /// </summary>
        /// <param name="data">The SUNCGDataStructure content</param>
        /// <param name="node"></param>
        /// <param name="wall"> If the wall is needed</param>
        /// <param name="raw"> If the obj is raw model</param>
        /// <param name="parent"> The parent GameObject it need to belong to</param>
        /// <returns> Return the center position of the Room </returns>
        public static void NewRoomBuild(FrontDataStructure data, FrontRoom room, bool existWall,bool existFloor, bool raw, GameObject floor = null)
        {
            //Basic GameObject Parent
            GameObject roomGameObject = new GameObject($"{room.instanceid}");
            roomGameObject.tag = "Room";
            Transform parent = roomGameObject.transform;
            Transform tarTrans = parent;
            Vector3 scaleVector3 = new Vector3(1, 1, 1);
            
            if (!floor.IsUnityNull())
            {
                Debug.Log("Generate by input floor");
                /*GameObject tmpFloor = NewFloorBuild(data.id, node, parent);
                Mesh t_mesh = floor.GetComponentInChildren<MeshFilter>().mesh;
                Mesh s_mesh = tmpFloor.GetComponentInChildren<MeshFilter>().mesh;
                
                MeshRenderer t_meshr = floor.GetComponentInChildren<MeshRenderer>();
                MeshRenderer s_meshr = tmpFloor.GetComponentInChildren<MeshRenderer>();
                
                //scale = new Vector3(t_bound.size.x / s_bound.size.x,1,t_bound.size.z / s_bound.size.z);
                //Rotation -> Scale -> Position
                //Rotation
                //FIXME: Make it 3D
                //t_mesh.
                tarTrans = t_meshr.transform;
                
                //Scale
                scaleVector3 = new Vector3(t_mesh.bounds.center.x/s_meshr.bounds.center.x,0,t_mesh.bounds.center.z/s_meshr.bounds.center.z);


                //posi = t_bound.center - s_bound.center;
                //parent = roomGameObject.transform;
                //GameObject.Destroy(tmpFloor);
                if (existWall)
                {
                    NewWallBuild(data.id, node, parent);
                }*/
            }
            
            //Generate Temp Objects
            
            //Import Objects
            foreach (FrontModel model in room.children)
            {
                //Debug.Log("Onbuilding "+ nodeid);

                foreach (Furniture f in data.furniture)
                {
                    if (f.uid == model.@ref && f.valid)
                    {
                        GameObject tmp = NewFurBuild(f, parent, raw);
                        tmp.transform.position = new Vector3(model.pos[0],model.pos[1],model.pos[2]);
                        tmp.transform.rotation = new Quaternion(model.rot[0],model.rot[1],model.rot[2],model.rot[3]);
                        tmp.transform.localScale = new Vector3(model.scale[0],model.scale[1],model.scale[2]);
                    }
                }

                foreach (FrontMesh m in data.mesh)
                {
                    if (m.uid == model.@ref)
                    {
                        GameObject tmp = NewFurBuild(m, parent, raw);
                        tmp.transform.position = new Vector3(model.pos[0],model.pos[1],model.pos[2]);
                        tmp.transform.rotation = new Quaternion(model.rot[0],model.rot[1],model.rot[2],model.rot[3]);
                        tmp.transform.localScale = new Vector3(model.scale[0],model.scale[1],model.scale[2]);
                    }
                }
            }

            roomGameObject.transform.position = tarTrans.position;
            roomGameObject.transform.rotation = tarTrans.rotation;
            //FIXME: BUGBUGBUGBUG MAY EXIST FOR DIFFERNT PIVOT
            roomGameObject.transform.localScale = new Vector3(tarTrans.lossyScale.x * scaleVector3.x,tarTrans.lossyScale.y,tarTrans.lossyScale.z * scaleVector3.z);

            //roomGameObject.transform.position = posi;
        }

        public static GameObject NewFurBuild(FrontMesh node, Transform parent, bool raw)
        {
            
            
            
            
            
            
            
            return null;
        }
        
        public static GameObject NewFurBuild(Furniture f, Transform parent, bool raw)
        {
            GameObject furnitureTemp = null;

                string mname = raw ? "raw" : "normalized";
                var objPath = $"{Config.ModelPath}{f.jid}\\{mname}_model.obj";
                var mtlPath = $"{Config.ModelPath}{f.jid}\\model.mtl";
/*
                float[,] innerTrans = new float[4, 4];

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        innerTrans[i, j] = f.transform[i + j * 4];
                        // innerTrans[i, j] = node.transform[i][j];
                    }
                }
*/
                
                furnitureTemp = new OBJLoader().Load(objPath, mtlPath, null);
            
                furnitureTemp.name = f.uid;
                furnitureTemp.transform.SetParent(parent);
                return furnitureTemp;

        }

        static Vector3 CalculateCenter(Vector3[] vecs)
        {
            Vector3 vec = Vector3.zero;
            vecs.ToList().ForEach((v) => { vec += v; });
            return vec / vecs.Length;
        }


    }
}