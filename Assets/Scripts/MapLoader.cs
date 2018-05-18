using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MapLoader : MonoBehaviour {

    GameManager GM;
    public string mapfile = "";

	// Use this for initialization
	void Start ()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadMapGO(MapSave mapfile)
    {
        LoadGroundGO();
        LoadObjGO();
    }

    public void LoadGroundGO()
    {
        GameObject emptyMap = new GameObject("Map");
        Vector3 placement = Vector3.zero;
        for (int y = 0; y < GM.map.sizey; y++)
        {
            for (int x = 0; x < GM.map.sizex; x++)
            {
                TileSave tileCur = GM.map.layer.tiles[y][x];
                TileSetSave tileset = new TileSetSave();
                GameObject tilego = null;
                if (tileCur.mapid > GM.map.basevalue)
                {
                    foreach (TileSetsSave tss in GM.map.tilesets)
                    {
                        foreach (TileSetSave ts in tss.tilesets)
                        {
                            if ((tileCur.mapid == tss.first + ts.id))
                            {
                                tileset = ts;
                                string id = tileset.spritefile.Substring(0, tileset.spritefile.IndexOf("."));
                                tilego = new GameObject(id + "_[" + x + "/" + y + "]");
                                SpriteRenderer tilesprite = tilego.AddComponent<SpriteRenderer>();
                                tilesprite.sprite = GM.SpriteList[id];
                                placement = new Vector3((x * GM.map.tilesizex + GM.map.tilesizex / 2.0f) / 100.0f, (y * GM.map.tilesizey + GM.map.tilesizey / 2.0f) / 100.0f, GM.ZGround);
                                tilego.GetComponent<Transform>().position = placement;
                                tilego.transform.SetParent(emptyMap.GetComponent<Transform>());
                                tilego.AddComponent<BoxCollider2D>();
                                Rigidbody2D tilebody = tilego.AddComponent<Rigidbody2D>();
                                tilebody.isKinematic = true;
                                tilego.layer = LayerMask.NameToLayer("ground");
                                GM.TilesGO.Add(tilego);
                                break;
                            }
                        }
                        if (tileset.id > 0)
                            break;
                    }
                }
            }
        }
    }


    public void LoadObjGO()
    {
        GameObject emptyGO = new GameObject("Objects");
        Vector3 placement = Vector3.zero;
        foreach (ObjectSave obj in GM.map.objects)
        {
            GameObject curObj = null;
            TileSetSave tileset = new TileSetSave();
            if (obj.gid > GM.map.basevalue)
            {
                foreach (TileSetsSave tss in GM.map.tilesets)
                {
                    foreach (TileSetSave ts in tss.tilesets)
                    {
                        if ((obj.gid == tss.first + ts.id))
                        {
                            tileset = ts;
                            string id = tileset.spritefile.Substring(0, ts.spritefile.IndexOf("."));
                            foreach (string key in tileset.modifiers.Keys)
                            {
                                if (obj.modifiers.ContainsKey(key) == false)
                                    obj.modifiers.Add(key, tileset.modifiers[key]);
                            }

                            curObj = new GameObject(id + "_[" + obj.x + "/" + obj.y + "]");
                            SpriteRenderer objsprite = curObj.AddComponent<SpriteRenderer>();
                            objsprite.sprite = GM.SpriteList[id];
                            if (obj.width != tileset.width)
                            {
                                curObj.GetComponent<Transform>().localScale = new Vector3((float)obj.width / tileset.width, (float)obj.height / tileset.height, 0.0f);
                            }
                            break;
                        }
                    }
                    if (tileset.id > 0)
                        break;
                }
            }
            if ((curObj != null))
            {
                placement = Vector3.zero;
                placement = new Vector3((obj.x + obj.offsetx + obj.width / 2) / 100.0f, ((GM.map.sizey * GM.map.tilesizey) - ((obj.y + obj.offsety - obj.height / 2.0f))) / 100.0f, GM.ZObject); 
                //placement.z += 2;
                curObj.GetComponent<Transform>().position = placement;
                curObj.transform.SetParent(emptyGO.GetComponent<Transform>());
                checkObjModifiers(obj, curObj);
            }
        }
    }

    public void checkObjModifiers(ObjectSave obj, GameObject curObj)
    {
        if (obj.modifiers.ContainsKey("spawner") && (obj.modifiers["spawner"] == "true"))
        {
            GM.Player = new GameObject("Player");
            GM.Player.tag = "player";
            GM.Player.GetComponent<Transform>().position = new Vector3(curObj.GetComponent<Transform>().position.x, curObj.GetComponent<Transform>().position.y, GM.ZPlayer);
            GM.Player.AddComponent<PlayerController>();
            GM.Camera = GameObject.Find("Camera");
            GM.Camera.GetComponent<CameraController>().ReplaceCam(curObj);
            Destroy(curObj.GetComponent<SpriteRenderer>());
        }
        if (obj.modifiers.ContainsKey("visible") && (obj.modifiers["visible"] == "false"))
        {
            Destroy(curObj.GetComponent<SpriteRenderer>());
        }
        if (obj.modifiers.ContainsKey("collider") && (obj.modifiers["collider"] == "true"))
        {
            curObj.layer = LayerMask.NameToLayer("ground");
            BoxCollider2D mybox = curObj.AddComponent<BoxCollider2D>();
            mybox.isTrigger = false;
        }

    }

    public void LoadMap(string mapfile, MapSave map)
    {
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
                map.basevalue = Convert.ToInt32(map.GetValueFromKey("infinite", line));
                map.render = map.GetValueFromKey("renderorder", line);
                map.orientation = map.GetValueFromKey("orientation", line);
                map.nextobject = Convert.ToInt32(map.GetValueFromKey("nextobjectid", line));
                LayerSave maptiles = new LayerSave();
                map.layer = maptiles;
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
                                    newid = newid.Substring(newid.IndexOf(",") + 1);
                                }
                                else
                                {
                                    tilesave.mapid = Convert.ToInt32(newid);
                                }
                                tilesave.posx = i;
                                tilesave.posy = j;
                                ground.tiles[j].Add(tilesave);
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
