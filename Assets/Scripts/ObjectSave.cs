using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ObjectSave
{
    public int id = 0;
    public int gid = 0;
    public float x = 0;
    public float y = 0;
    public int width = 0;
    public int height = 0;
    public int offsetx = 0;
    public int offsety = 0;
    public int visitState = 0;
    public Dictionary<string, string> modifiers = new Dictionary<string, string>();
}
