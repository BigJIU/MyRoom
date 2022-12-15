using System;
using Newtonsoft.Json;

namespace FrontData
{
    [Serializable]
    public class FrontDataStructure
    {

        public string uid;
        public int[] north_vector;
        public Furniture[] furniture;
        public FrontMesh[] mesh;
        public FrontMaterial[] material;
        public FrontScene scene;
    }
    
    [Serializable]
    public class Furniture
    {
        public string uid; //Inner ID
        public string jid;
        public float[] size;
        public string sourceCategoryId;
        public float[] bbox;
        public bool valid;
    }
    
    [Serializable]
    public class FrontMesh
    {
        public string jid;
        public string uid; //Inner ID
        public float[] xyz;
        public float[] normal;
        public float[] uv;
        public int[] faces;
        public string material; 
        public string type;
    }
    
    [Serializable]
    public class FrontMaterial
    {
        public string uid;
        public string jid;
        public string texture;
        public string normaltexture;
        public int[] color;
    }
    
    [Serializable]
    public class FrontScene
    {
        public float[] pos;
        public float[] rot;
        public float[] scale;
        public FrontRoom[] room;
    }
    
    [Serializable]
    public class FrontRoom
    {
        public string type;
        public string instanceid;
        public float[] pos;
        public float[] rot;
        public float[] scale;
        public FrontModel[] children;
        public int empty;
    }
    
    [Serializable]
    public class FrontModel
    {
        public string @ref;//Inner ID for Indexing
        public string instanceid;//
        public float[] pos;
        public float[] rot;
        public float[] scale;
    }
}
