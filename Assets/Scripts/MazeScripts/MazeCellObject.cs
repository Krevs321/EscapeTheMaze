using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellObject : MonoBehaviour
{
    [SerializeField] GameObject topWall;
    [SerializeField] GameObject bottomWall;
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject floorObject;
    [SerializeField] GameObject holdsTop;
    [SerializeField] GameObject holdsBot;
    [SerializeField] GameObject holdsLeft;
    [SerializeField] GameObject holdsRight;
    [SerializeField] GameObject table;
    [SerializeField] GameObject gun;

    public void Init(bool top, bool bottom, bool left, bool right, bool floor, bool hTop, bool hBot, bool hLeft, bool hRight, bool tbl, bool gn)
    {
        topWall.SetActive(top);
        bottomWall.SetActive(bottom);
        leftWall.SetActive(left);
        rightWall.SetActive(right);
        floorObject.SetActive(floor);
        holdsTop.SetActive(hTop);
        holdsBot.SetActive(hBot);
        holdsLeft.SetActive(hLeft);
        holdsRight.SetActive(hRight);
        table.SetActive(tbl);
        gun.SetActive(gn);
    }
}
