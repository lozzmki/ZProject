using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Global Controller
/// </summary>
public static class GameManager {

    #region Stage
    public const int MaxStageNum = 5;
    static int StagePointer = -1;
    static Stage[] Stages;
    public static Stage CurrentStage
    {
        get
        {
            if (StagePointer < 0 || StagePointer >= MaxStageNum)
                return null;
            return Stages[StagePointer];
        }
    }
    #endregion

    #region GameObjects
    public static GameObject Player;
    #endregion

    public static void Init()
    {
        //Generate Map
        Stages = new Stage[MaxStageNum];
        for(int i=0; i< MaxStageNum; i++) {
            Stages[i] = MapGenerator.NewMap(15);
        }

        //Create Player
        Player = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Player_"));
        Camera.main.GetComponent<CameraFollowing>().BindToTarget(Player.transform);
    }

    public static void NextLevel()
    {
//         if(StagePointer > 0)
//             Stages[StagePointer].
        StagePointer++;
        if (StagePointer >= MaxStageNum)
            return;

        if (StagePointer > 0)
            Stages[StagePointer - 1]._stage.SetActive(false);
        Stages[StagePointer].CreateStage(new Vector3(0, 10 * StagePointer, 0));
    }


}
