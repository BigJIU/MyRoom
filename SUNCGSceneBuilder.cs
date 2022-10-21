using System.Linq;
using Dummiesman;
using UnityEditor;
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
        public static Vector3 NewRoomBuild(SUNCGDataStructure data, Node node,bool wall,bool raw, Transform parent = null)
        {

            ViewerWindow.ClearRoom();
            GameObject roomGameObject = new GameObject($"{node.modelId}");
            roomGameObject.tag = "Room";

            parent = roomGameObject.transform;
            Vector3 center=Vector3.zero;
            
            //FIXME
            if (wall)
            {
                center = NewWallBuild(data.id, node, parent);
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
            return center;
        }

        public static Vector3 NewWallBuild(string houseId, Node node, Transform parent)
        {
            Debug.Log("Building wall and floor");
            string wallPath = $"{Config.HomePath}{houseId}\\{node.modelId}w.obj";
            string floorPath = $"{Config.HomePath}{houseId}\\{node.modelId}f.obj";
            string wallMtl = $"{Application.dataPath}\\wall.mtl";
            string floorlMtl = $"{Application.dataPath}\\floor.mtl";
            
            Utils.EditFloorFormat(floorPath);
            var wall = new OBJLoader().Load(wallPath,wallMtl,null);
            wall.transform.SetParent(parent);
            var floor = new OBJLoader().Load(floorPath,floorlMtl,null);

            floor.transform.SetParent(parent);
            var center = CalculateCenter(floor.GetComponentInChildren<MeshFilter>().sharedMesh.vertices);
            center.y += 0.3f;
            return center;
        }

        


        public static void NewFurBuild(Node node, Transform parent, bool raw)
        {
            GameObject furnitureTemp = null;
            if (node.type != "Room")
            {
                string mname = raw ? "raw" : "normalized";
                var objPath = $"{Config.ModelPath}{node.modelId}\\{mname}_model.obj";
                var mtlPath = $"{Config.ModelPath}{node.modelId}\\model.mtl";

            float[,] innerTrans = new float[4,4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    innerTrans[i, j] = node.transform[i+j*4];
                    // innerTrans[i, j] = node.transform[i][j];
                }
            }

                furnitureTemp = new OBJLoader().Load(objPath, mtlPath ,innerTrans);
                furnitureTemp.name = node.id;
                furnitureTemp.transform.SetParent(parent);
            }

            if (node.subItems != null)
            {
                foreach (var nodeSub in node.subItems)
                {
                    NewSubItemBuild(nodeSub,furnitureTemp.transform);
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

                float[,] innerTrans = new float[4,4];

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        innerTrans[i, j] = node.transform[i+j*4];
                        // innerTrans[i, j] = node.transform[i][j];
                    }
                }

                furnitureTemp = new OBJLoader().Load(objPath, mtlPath ,innerTrans);
                furnitureTemp.name = node.id;
                furnitureTemp.transform.SetParent(parent);
            }
        }
        
        static Vector3 CalculateCenter(Vector3[] vecs)
        {
            Vector3 vec = Vector3.zero;
            vecs.ToList().ForEach((v) =>
            {
                vec += v;
            });
            return vec / vecs.Length;
        }
        
        
    }
}