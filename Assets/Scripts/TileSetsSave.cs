using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class TileSetsSave
{
    public List<TileSetSave> tilesets = new List<TileSetSave>();
    public int first = 0;
    public int spritecount = 0;
    public int tilewidth = 0; // unused ?
    public int tileheight = 0; // unused ?
    public string name = ""; // unusued ?
    public string source = "";

}
