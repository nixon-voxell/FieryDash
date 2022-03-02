using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = System.Random;
using System;

public class PlacementOfKillableObstacles : MonoBehaviour
{
    KillableObstaclesPool objectPooler;
    
    //declaration
    private int _rangeOfObstacles,_amountOfObstacles, _amountOfSlotsOnPlatform, _index = 0,_maxPosInt, _minPosInt, _posX, _count = 0, _tempPosX;
    private float  _platformSize, _platformStartPosition, _platformEndPosition, _maxPos, _minPos;
    private Vector3 _platformPositionVector, _obstaclePosition;
    private GameObject  _platform;
    private List<GameObject> _platformsList = new List<GameObject>();
    private List<Vector3> _platformPositionVectorList = new List<Vector3>();
    private List<float> _platformStartPositionList = new List<float>(), _platformEndPositionList = new List<float>();
    private List<int> _obstacleAmountList = new List<int>(), _obstacleCheckingList = new List<int>();
    private bool _isValid;
    void Start()
    {
        objectPooler = KillableObstaclesPool.Instance;

        //getting all platforms 
        _platformsList.AddRange(GameObject.FindGameObjectsWithTag("platform"));


        foreach (GameObject platform in _platformsList)
        {
            //getting platform's information
            _platformPositionVector = platform.transform.position;
            _platformPositionVectorList.Add(_platformPositionVector);
            _platformSize = platform.transform.localScale.x;
            _amountOfSlotsOnPlatform = Mathf.RoundToInt(_platformSize);

            _rangeOfObstacles = 3;
            if (_amountOfSlotsOnPlatform > 10)
            {
                _rangeOfObstacles = 5;
            }

            //get amount of obstacles in a single platform
            var rnd = new Random();
            _amountOfObstacles = rnd.Next(1,_rangeOfObstacles);
            _obstacleAmountList.Add(_amountOfObstacles);

            //get the max and min position
            _platformStartPosition = _platformPositionVector.x - _platformSize / 2  + 1;
            _platformEndPosition = _platformPositionVector.x + _platformSize / 2 - 1; 
            _platformStartPositionList.Add(_platformStartPosition);
            _platformEndPositionList.Add(_platformEndPosition);
        }
    }
    void Update() 
    {
        var random = new Random();

        //generate a position and spawn it 
        if (_count < _obstacleAmountList[_index])
        {
            //position
            _maxPos = _platformEndPositionList[_index];
            _minPos = _platformStartPositionList[_index];
            _maxPosInt = Mathf.RoundToInt(_maxPos);
            _minPosInt = Mathf.RoundToInt(_minPos);
            _posX =  random.Next(_minPosInt, _maxPosInt);
            _obstacleCheckingList.Add(_posX);
            _isValid = true;

            //restriction

            // all the _isValid will be turn into something else
            if (_count != 0)
            {
                if (_obstacleCheckingList.Contains(_tempPosX))
                {
                    if (_posX < _maxPos/2)
                    {
                        _posX = random.Next(_tempPosX +1, _maxPosInt);
                        _isValid = false;
                    }
                    else
                    {
                        _posX = random.Next(_minPosInt, _tempPosX - 1);
                        _isValid = false;
                    }
                }
                if (_count >= 2)
                {
                    //looping list and check position
                    _obstacleCheckingList.ForEach(delegate(int pos)
                    {
                        if (_obstacleCheckingList.Contains(pos + 1))
                        {
                            if (_obstacleCheckingList.Contains(pos - 1))
                            {
                                _isValid = false;
                            }
                            else if (_obstacleCheckingList.Contains(pos + 2))
                            {
                                _isValid = false;
                            }
                        }
                        if (_obstacleCheckingList.Contains(pos - 1))
                        {
                            if (_obstacleCheckingList.Contains(pos - 2))
                            {
                                _isValid = false;
                            }
                        }
                    });
                }
            }
            //to check previous position
            _tempPosX = _posX;

            //spawning 
            if (_isValid)
            {
                _platform = _platformsList[_index];
                _platformPositionVector = _platformPositionVectorList[_index];
                _obstaclePosition = _platformPositionVector;
                _obstaclePosition.x = _posX;
                _obstaclePosition.y += 1.0f;
                objectPooler.SpawningFromPool("Spike", _obstaclePosition, _platform);             
            }
                _count += 1;
        }
        if (_count == _obstacleAmountList[_index])
        {
            _obstacleCheckingList.Clear();
            _index +=1; 
            _count = 0;
        }
    }
}   

