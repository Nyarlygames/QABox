using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MapLoader : MonoBehaviour {

    GameManager GM;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void LoadMap(string mapfile, MapSave map)
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        StreamReader reader = new StreamReader(mapfile);
        string line = reader.ReadLine();
        while (!reader.EndOfStream)
        {
            if (line.Contains("<map"))
            {
                map.sizex = Convert.ToInt32(map.GetValueFromKey("width", line));
                map.sizey = Convert.ToInt32(map.GetValueFromKey("height", line));
                map.tilesizex = Convert.ToInt32(map.GetValueFromKey("tilewidth", line));
                map.tilesizey = Convert.ToInt32(map.GetValueFromKey("tileheight", line));
                GM.mapmaxsizex = map.tilesizex;
                GM.mapmaxsizey = map.tilesizey;
                GM.mapcursizex = map.tilesizex;
                GM.mapcursizey = map.tilesizey;
                map.basevalue = Convert.ToInt32(map.GetValueFromKey("infinite", line));
                map.render = map.GetValueFromKey("renderorder", line);
                map.orientation = map.GetValueFromKey("orientation", line);
                map.nextobject = Convert.ToInt32(map.GetValueFromKey("nextobjectid", line));
                LayerSave maptiles = new LayerSave();
                map.layer = maptiles; // here
            }
            if (line.Contains("<layer"))
            {
                LayerSave ground = map.layer;
                ground.name = map.GetValueFromKey("name", line);
                ground.sizex = Convert.ToInt32(map.GetValueFromKey("width", line));
                ground.sizey = Convert.ToInt32(map.GetValueFromKey("height", line));
                line = reader.ReadLine();
                if (line.Contains("<data encoding=\"csv\""))
                {

                    string newid = reader.ReadLine();
                    for (int j = 0; j < ground.sizey; j++)
                    {
                        if (ground.name == "Ground")
                            ground.tiles.Add(new List<TileSave>());
                        for (int i = 0; i < ground.sizex; i++)
                        {
                            TileSave tilesave = new TileSave();
                            if (ground.name == "Ground")
                            {
                                if (newid.IndexOf(",") > 0)
                                {
                                    tilesave.mapid = Convert.ToInt32(newid.Substring(0, newid.IndexOf(",")));
                                    // newid for other layers
                                    newid = newid.Substring(newid.IndexOf(",") + 1);
                                    // same
                                }
                                else
                                {
                                    tilesave.mapid = Convert.ToInt32(newid);
                                }
                                tilesave.posx = i;
                                tilesave.posy = j;
                                ground.tiles[j].Add(tilesave);
                            }
                            /*else if (ground.name == "Triggers")
                            {
                                tilesave = ground.tiles[j][i];
                                if (newid.IndexOf(",") > 0)
                                {
                                    tilesave.triggerid = Convert.ToInt32(newid.Substring(0, newid.IndexOf(",")));
                                    newid = newid.Substring(newid.IndexOf(",") + 1);
                                }
                                else
                                {
                                    tilesave.triggerid = Convert.ToInt32(newid);
                                }
                            }*/
                            else if (ground.name == "AddedGround")
                            {
                                // tile already created in ground.
                                tilesave = ground.tiles[j][i];
                                if (newid.IndexOf(",") > 0)
                                {
                                    tilesave.addedid = Convert.ToInt32(newid.Substring(0, newid.IndexOf(",")));
                                    newid = newid.Substring(newid.IndexOf(",") + 1);
                                }
                                else
                                {
                                    tilesave.addedid = Convert.ToInt32(newid);
                                }
                            }
                        }
                        newid = reader.ReadLine();
                    }
                }
            }
            if (line.Contains("<objectgroup"))
            {
                ObjectGroupSave objectlayer = new ObjectGroupSave();
                objectlayer.name = map.GetValueFromKey("name", line);
                if (line.Contains("offsetx"))
                    objectlayer.offsetx = Convert.ToInt32(map.GetValueFromKey("offsetx", line));
                if (line.Contains("offsety"))
                    objectlayer.offsety = Convert.ToInt32(map.GetValueFromKey("offsety", line));
                map.objectgroups.Add(objectlayer);
                line = reader.ReadLine();
                if (line.Contains("<object"))
                {
                    while (!line.Contains("</objectgroup>"))
                    {
                        ObjectSave obj = new ObjectSave();
                        obj.id = Convert.ToInt32(map.GetValueFromKey("id", line));
                        obj.gid = Convert.ToInt32(map.GetValueFromKey("gid", line));
                        obj.x = Convert.ToSingle(map.GetValueFromKey("x", line));
                        obj.y = Convert.ToSingle(map.GetValueFromKey("y", line));
                        obj.width = Convert.ToInt32(map.GetValueFromKey("width", line));
                        obj.height = Convert.ToInt32(map.GetValueFromKey("height", line));
                        obj.offsetx = objectlayer.offsetx;
                        obj.offsety = objectlayer.offsety;

                        line = reader.ReadLine();
                        if (line.Contains("<properties"))
                        {
                            while ((!line.Contains("</object>")) && (!line.Contains("</properties")))
                            {
                                if (line.Contains("<property"))
                                {
                                    obj.modifiers.Add(map.GetValueFromKey("name", line), map.GetValueFromKey("value", line));
                                }
                                line = reader.ReadLine();
                            }
                            if (line.Contains("</properties"))
                                line = reader.ReadLine();
                            line = reader.ReadLine();
                        }
                        map.objects.Add(obj);
                    }
                }
            }
            if (line.Contains("<tileset"))
            {
                TileSetsSave tilesets = new TileSetsSave();
                tilesets.first = Convert.ToInt32(map.GetValueFromKey("firstgid", line));
                tilesets.source = map.GetValueFromKey("source", line);
                StreamReader readertileset = new StreamReader("Assets/Resources/Map/" + tilesets.source);
                int internid = 0;
                string linetileset = readertileset.ReadLine();
                while (!readertileset.EndOfStream)
                {
                    if (linetileset.Contains("<tileset"))
                    {
                        tilesets.name = map.GetValueFromKey("name", linetileset);
                        tilesets.tilewidth = Convert.ToInt32(map.GetValueFromKey("tilewidth", linetileset));
                        tilesets.tileheight = Convert.ToInt32(map.GetValueFromKey("tileheight", linetileset));
                        tilesets.spritecount = Convert.ToInt32(map.GetValueFromKey("tilecount", linetileset));
                    }
                    if (linetileset.Contains("<tile "))
                    {
                        TileSetSave tileset = new TileSetSave();
                        tileset.id = Convert.ToInt32(map.GetValueFromKey("id", linetileset));
                        linetileset = readertileset.ReadLine();
                        tileset.internal_id = internid;
                        while (!linetileset.Contains("</tile>"))
                        {
                            if (linetileset.Contains("<property"))
                            {
                                tileset.modifiers.Add(map.GetValueFromKey("name", linetileset), map.GetValueFromKey("value", linetileset));
                            }
                            if (linetileset.Contains("<image") && (!linetileset.Contains("format")))
                            {
                                tileset.height = Convert.ToInt32(map.GetValueFromKey("height", linetileset));
                                tileset.spritefile = map.GetValueFromKey("source", linetileset);
                                tileset.width = Convert.ToInt32(map.GetValueFromKey("width", linetileset));
                            }
                            linetileset = readertileset.ReadLine();
                        }
                        internid++;
                        tilesets.tilesets.Add(tileset);
                    }
                    linetileset = readertileset.ReadLine();
                }
                map.tilesets.Add(tilesets);
            }
            line = reader.ReadLine();
        }
        map.layer.tiles.Reverse();
        reader.Close();
    }

}
