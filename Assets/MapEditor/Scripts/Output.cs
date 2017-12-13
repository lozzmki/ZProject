using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/// <summary>
/// 导出地图类
/// </summary>
namespace mapEditor {
    public static class Output {

        public static void OutputBinary(string fileName) {
            var cellMap = GameObject.Find("CellContainer").GetComponent<CellManager>();
            var objList = GameObject.Find("BuildingContainer").GetComponentsInChildren<SceneObj>();

            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryWriter bw = new BinaryWriter(fs);

            //写入地图宽长
            bw.Write(cellMap.Width);
            bw.Write(cellMap.Height);
            //写入所有tile的纹理id
            for (int i = 0; i < cellMap.Width; i++) {
                for (int j = 0; j < cellMap.Height; j++) {
                    bw.Write(cellMap.getCell(new Vector2(i, j)).GetComponent<Cell>().GetMatId);
                }
            }

            //写入所有物体信息
            bw.Write(objList.Length);
            foreach (var obj in objList) {
                bw.Write((int)(obj.m_FatherCell.mapPos.x));
                bw.Write((int)(obj.m_FatherCell.mapPos.y));
                bw.Write(obj.GetObjRotate);
                bw.Write(@obj.GetName);
            }
            fs.Close();
            bw.Close();
        }

        public static void ReadBinrayMap(string fileName) {
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            var map = GameObject.Find("CellContainer").GetComponent<CellManager>();


            int width = br.ReadInt32();
            int height = br.ReadInt32();

            map.RegenerateMap_Editor(width, height);

            Debug.Log(width + " , " + height);
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    var mat_id = br.ReadInt32();
                    Debug.Log(mat_id);
                    map.getCell(new Vector2(i, j)).GetComponent<Cell>().ChangeFoorMat(mat_id);
                }
            }

            var objLength = br.ReadInt32();
            for (int i = 0; i < objLength; i++) {
                var mapx = br.ReadInt32();
                var mapy = br.ReadInt32();
                var rotateId = br.ReadInt32();
                var name = br.ReadString();
                Debug.Log(mapx + " , " + mapy + "ROTATE :" + rotateId + " NAME:" + name);

                map.getCell(new Vector2(mapx, mapy)).GetComponent<Cell>().BuildBox(
                    mapEditor.Output.NameToSceneObj(name));
                map.getCell(new Vector2(mapx, mapy)).GetComponent<Cell>().GetBuilding.GetObjRotate = rotateId;
            }
            fs.Close();
            br.Close();
        }

        public static DRoomPreset GetPresetByMap(string filename) {
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            //var map = GameObject.Find("CellContainer").GetComponent<CellManager>();


            int width = br.ReadInt32();
            int height = br.ReadInt32();

            //map.RegenerateMap_Editor(width, height);
            DRoomPreset rlsPreset = new DRoomPreset(width, height);
            SCENEOBJ_TYPE[,] objmap = new SCENEOBJ_TYPE[width, height];
            int[,] texMap = new int[width, height];

            Debug.Log(width + " , " + height);
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    var mat_id = br.ReadInt32();
                    texMap[i, j] = mat_id;
                    Debug.Log(mat_id);
                    //map.getCell(new Vector2(i, j)).GetComponent<Cell>().ChangeFoorMat(mat_id);
                }
            }

            var objLength = br.ReadInt32();
            for (int i = 0; i < objLength; i++) {
                var mapx = br.ReadInt32();
                var mapy = br.ReadInt32();
                var rotateId = br.ReadInt32();
                var name = br.ReadString();
                Debug.Log(mapx + " , " + mapy + "ROTATE :" + rotateId + " NAME:" + name);
                objmap[mapx, mapy] = NameToSceneObj(name);
                //map.getCell(new Vector2(mapx, mapy)).GetComponent<Cell>().BuildBox(
                //   GameObject.Find("GameStone").GetComponent<ResrouceManager>().NameToSceneObj(name));
                //map.getCell(new Vector2(mapx, mapy)).GetComponent<Cell>().GetBuilding.GetObjRotate = rotateId;
            }
            for (int w = 0; w < width; w++) {
                for (int h = 0; h < height; h++) {
                    DCell cell = new DCell(texMap[w,h]);
                    if(objmap[w,h] == SCENEOBJ_TYPE.DOOR0) {
                        cell.d_bIsDoor = true;
                    }
                    if(objmap[w,h] == SCENEOBJ_TYPE.WALL_1 || objmap[w, h] == SCENEOBJ_TYPE.WALL_CORNER_1) {
                        cell.d_bIsWall = true;
                    }
                    rlsPreset.d_Data[h * width + w] = cell;
                }
            }
            fs.Close();
            br.Close();
            return rlsPreset;
        }

        public static SCENEOBJ_TYPE NameToSceneObj(string name) {
            switch (name) {
                case "ROCK": return SCENEOBJ_TYPE.ROCK;
                case "TREE": return SCENEOBJ_TYPE.TREE;
                case "FLAG": return SCENEOBJ_TYPE.FLAG;
                case "BOX1": return SCENEOBJ_TYPE.BOX;
                case "DOOR1": return SCENEOBJ_TYPE.DOOR;
                case "DICI": return SCENEOBJ_TYPE.DICI;
                case "WALL_1": return SCENEOBJ_TYPE.WALL_1;
                case "WALL_CORNER_1": return SCENEOBJ_TYPE.WALL_CORNER_1;
                case "DOOR0": return SCENEOBJ_TYPE.DOOR0;
                default:
                    Debug.LogError("can't find the resources by name:" + name);
                    return SCENEOBJ_TYPE.NONE;
            }
        }
    }

}

