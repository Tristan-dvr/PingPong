using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class CustomBalls
{
    public Data defaultBall = new Data();
    public List<Data> balls = new List<Data>();

    [Serializable]
    public class Data
    {
        public string id;
        public string name;
        public Sprite icon;
        public AssetReference prefab;
    }
}
