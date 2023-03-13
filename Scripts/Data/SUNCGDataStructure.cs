using System;
using Newtonsoft.Json;

namespace SUNCGData
{


    [Serializable]
    public class SUNCGDataStructure:DataStructure
    {

        public string id;
        public int[] up;
        public int[] front;
        public int scaleToMeters;
        public Level[] levels;

    }
    [Serializable]
    public class Level
    {
        public string id;
        [JsonIgnore]
        public bbox bbox;
        public Node[] nodes;
        public string title;
        public string type;
        public float[] size;
    }
    [Serializable]
    public class Node
    {
        public string id;
        public string type;
        public int valid;
        public string modelId;
        public string[] roomTypes;
        public float[] transform;
        public string[] materials;
        public int state;
        public bbox bbox;
        public int[] nodeIndices;
        public string instanceid;
        public SubNode[] subItems;
        
        public Node(string modelId,string id)
        {
            type = "Object";
            valid = 1;
            transform = new float[] { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1};
            this.modelId = modelId;
            this.id = id;
        }
    }
    
    [Serializable]
    public class SubNode
    {
        public string id;
        public string type;
        public int valid;
        public string modelId;
        public string[] roomTypes;
        public float[] transform;
        public string[] materials;
        public int state;
        public bbox bbox;
        public int[] nodeIndices;
        public string instanceid;
        public SubNode(string modelId,string id)
        {
            type = "Item";
            valid = 1;
            transform = new float[] { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1};
            this.modelId = modelId;
            this.id = id;
        }
    }
    [Serializable]
    public class bbox
    {
        public float[] min;
        public float[] max;
    }

}