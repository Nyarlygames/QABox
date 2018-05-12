using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class TileSetSave
{
    public string spritefile;
    public int id;
    public int internal_id;
    public Dictionary<string, string> modifiers = new Dictionary<string, string>();
    public int height = 0;
    public int width = 0;

}
