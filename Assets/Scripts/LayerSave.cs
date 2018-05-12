using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class LayerSave
{

    public List<List<TileSave>> tiles = new List<List<TileSave>>();
    public int sizex = 0;
    public int sizey = 0;
    public string name ="";
    
}
