
using System.Linq;
using Dummiesman;
using Unity.VisualScripting;
using UnityEngine;

namespace SUNCGData
{
    public class SUNCGSceneBuilder
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
        public static void NewRoomBuild(SUNCGDataStructure data, Node node, bool existWall,bool existFloor, bool raw, GameObject floor = null)
        {
            //Basic GameObject Parent

            
            
            GameObject roomGameObject = new GameObject($"{node.modelId}");
            roomGameObject.tag = "Room";
            Transform parent = roomGameObject.transform;
            Transform tarTrans = parent;
            Vector3 scaleVector3 = new Vector3(1, 1, 1);
            if (!floor.IsUnityNull())
            {
                Debug.Log("Generate by input floor");
                GameObject tmpFloor = NewFloorBuild(data.id, node, parent);
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
                }
            }
            else
            {
                Debug.Log("Generate by default floor");
                if (existFloor)
                {
                    NewFloorBuild(data.id, node, parent);
                }
                if (existWall)
                {
                    NewWallBuild(data.id, node, parent);
                }
            }
            
            foreach (int nodeid in node.nodeIndices)
            {
                //Debug.Log("Onbuilding "+ nodeid);
                for (int d = 0; d < data.levels[0].nodes.Length; d++)
                {
                    if (data.levels[0].nodes[d].id.Split('_')[1] == $"{nodeid}")
                    {
                        NewFurBuild(data.levels[0].nodes[d], parent, raw);
                    }
                }

            }

            roomGameObject.transform.position = tarTrans.position;
            roomGameObject.transform.rotation = tarTrans.rotation;
            //FIXME: BUGBUGBUGBUG MAY EXIST FOR DIFFERNT PIVOT
            roomGameObject.transform.localScale = new Vector3(tarTrans.lossyScale.x * scaleVector3.x,tarTrans.lossyScale.y,tarTrans.lossyScale.z * scaleVector3.z);

            //roomGameObject.transform.position = posi;
        }
        
        public static GameObject NewFloorBuild(string houseId, Node node, Transform parent)
        {
            Debug.Log("Building floor");
            string floorPath = $"{Config.HomePath}{houseId}\\{node.modelId}f.obj";
            string floorlMtl = $"{Application.dataPath}\\floor.mtl";

            Utils.EditFloorFormat(floorPath);
            var floor = new OBJLoader().Load(floorPath, floorlMtl, null);
            floor.transform.SetParent(parent);

            return floor;
        }
        
        public static void NewWallBuild(string houseId, Node node, Transform parent)
        {
            Debug.Log("Building wall");
            string wallPath = $"{Config.HomePath}{houseId}\\{node.modelId}w.obj";
            string wallMtl = $"{Application.dataPath}\\wall.mtl";
            
            var wall = new OBJLoader().Load(wallPath, wallMtl, null);
            wall.transform.SetParent(parent);
        }




        public static void NewFurBuild(Node node, Transform parent, bool raw)
        {
            GameObject furnitureTemp = null;
            if (node.type != "Room")
            {
                string mname = raw ? "raw" : "normalized";
                var objPath = $"{Config.ModelPath}{node.modelId}\\{mname}_model.obj";
                var mtlPath = $"{Config.ModelPath}{node.modelId}\\model.mtl";

                float[,] innerTrans = new float[4, 4];

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        innerTrans[i, j] = node.transform[i + j * 4];
                        // innerTrans[i, j] = node.transform[i][j];
                    }
                }

                furnitureTemp = new OBJLoader().Load(objPath, mtlPath, innerTrans);
                furnitureTemp.name = node.id;
                furnitureTemp.transform.SetParent(parent);
            }

            if (node.subItems != null)
            {
                foreach (var nodeSub in node.subItems)
                {
                    NewSubItemBuild(nodeSub, furnitureTemp.transform);
                }
            }

        }


        public static void NewSubItemBuild(SubNode node, Transform parent)
        {
            GameObject furnitureTemp = null;
            if (node.type == "Item")
            {
                var objPath = $"{Config.ItemPath}\\{node.modelId}\\{node.modelId}.obj";
                var mtlPath = $"{Config.ItemPath}\\{node.modelId}\\{node.modelId}.mtl";

                float[,] innerTrans = new float[4, 4];

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        innerTrans[i, j] = node.transform[i + j * 4];
                        // innerTrans[i, j] = node.transform[i][j];
                    }
                }

                furnitureTemp = new OBJLoader().Load(objPath, mtlPath, innerTrans);
                furnitureTemp.name = node.id;
                furnitureTemp.transform.SetParent(parent);
            }
        }

        static Vector3 CalculateCenter(Vector3[] vecs)
        {
            Vector3 vec = Vector3.zero;
            vecs.ToList().ForEach((v) => { vec += v; });
            return vec / vecs.Length;
        }


    }
}