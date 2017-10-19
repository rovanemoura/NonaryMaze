﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMazeFactory : IMazeFactory
{


    public Maze GenerateMaze()
    {
        System.Type roomType = typeof(Room);
        System.Type mazeType = typeof(Maze);
        System.Type doorRuleType = typeof(DummyDoorRule);

        GameObject doorPrefab = (GameObject)Resources.Load("Prefabs/Door");
        GameObject playerPrefab = (GameObject)Resources.Load("Prefabs/Player");

        Maze maze = (Maze)ScriptableObject.CreateInstance(mazeType);

        #region Maze Initializers
        List<Room> rooms = new List<Room>();
        List<Door> doors = new List<Door>();
        List<Player> players = new List<Player>();
        #endregion


        #region Initializes Rooms
        Room start = (Room)ScriptableObject.CreateInstance(roomType);
        Room goal = (Room)ScriptableObject.CreateInstance(roomType);

        maze.Start = start;
        maze.Goal = goal;

        rooms.Add(start);

        for (int i = 1; i < 4; i++)
        {
            rooms.Add((Room)ScriptableObject.CreateInstance(roomType)) ;
        }

        rooms.Add(goal);
        #endregion

        #region Initializes Doors
        IDoorOpeningRule doorRule = (IDoorOpeningRule)ScriptableObject.CreateInstance(doorRuleType);

        //                     1  2  3  4  5  6  7  8  9
        int[] destinations = { 4, 4, 3, 2, 2, 4, 3, 3, 5 };

        Debug.Log(rooms.Count);

        for (int i = 1; i < 10; i++)
        {
            Door door = GameObject.Instantiate(doorPrefab).GetComponent<Door>();
            door.name = "Door" + i;
            door.Code = i;
            door.Rule = doorRule;
            door.Destination = rooms[destinations[i-1]-1];
            doors.Add(door);
        }

        int[][] roomDoors = { new int[]{4, 5},      //0
                              new int[]{3, 7, 8},   //1
                              new int[]{2, 6, 1},   //2
                              new int[]{9},         //3
        };
        #endregion

        #region Links Rooms and Doors
        for(int i = 0; i < 4; i++)
        {
            List<Door> newDoors = new List<Door>();

            foreach (int j in roomDoors[i]) { newDoors.Add(doors[j-1]); }

            rooms[i].SetDoors(newDoors);

        }
        #endregion


        #region Initializes Players
        for (int i = 1; i < 10; i++)
        {
            Player player = GameObject.Instantiate(playerPrefab).GetComponent<Player>();
            player.Code = i;
            player.name = "Player" + i;
            players.Add(player);

        }

        start.AddPlayers(players);
        #endregion

        maze.InitMaze(players, doors, rooms);

        return maze;
    }
}
