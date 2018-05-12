using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class MapSave
{
    public int sizex = 0;
    public int sizey = 0;
    public int tilesizex = 0;
    public int tilesizey = 0;
    public int basevalue = 0;
    public int nextobject = 0;
    public string render = "";
    public string orientation = "orthogonal";
    public string id = "";
    
    public List<TileSetsSave> tilesets = new List<TileSetsSave>();
    public LayerSave layer = new LayerSave();
    public List<ObjectGroupSave> objectgroups = new List<ObjectGroupSave>();
    public List<ObjectSave> objects = new List<ObjectSave>();

    public string GetValueFromKey(string key, string line)
    {
        int start = line.IndexOf(key + "=\"") + (key + "=\"").Length;
        string cut = line.Substring(start);
        int end = start + cut.IndexOf("\"");
        string value = line.Substring(start, end - start);
        return (value);
    }
}
