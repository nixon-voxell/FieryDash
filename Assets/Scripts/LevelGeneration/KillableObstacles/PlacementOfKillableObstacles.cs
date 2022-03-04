using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = System.Random;
using Random1= UnityEngine.Random;
using System.Threading.Tasks;

public class PlacementOfKillableObstacles : MonoBehaviour
{
    KillableObstaclesPool objectPooler;
    
    //declaration
    public GameObject platform;
    private int _rangeOfObstacles,_amountOfObstacles, _amountOfSlotsOnPlatform, _index = 0,_maxPosInt, _minPosInt, _posX, _count = 0, _tempPosX; 
    private int _platformStartPosition, _platformEndPosition;
    private float  _platformSize, _maxPos, _minPos;
    private Vector3 _platformPositionVector, _obstaclePosition;
    private GameObject  _platform;
    private GameObject[] _platformsList;
    private List<int> _obstacleCheckingList = new List<int>();
    private bool _isValid;

    private void SpawnKillableObjects()
    {
        var random = new Random();
        Debug.Log(_index);
        //get platform
        _platformsList = GameObject.FindGameObjectsWithTag("platform");
        for (int i = 0; i< _platformsList.Length; i ++)
        {
            Debug.Log(_platformsList[i].transform.position);
        }
        
        _platform = _platformsList[_index];

        //getting platform's information
        _platformPositionVector = _platform.transform.position;
        _platformSize = _platform.transform.localScale.x;
        _amountOfSlotsOnPlatform = Mathf.RoundToInt(_platformSize);

        //get amount of obstacles in a single platform
        _rangeOfObstacles = 3;
        if (_amountOfSlotsOnPlatform > 10)
        {
            _rangeOfObstacles = 5;
        }
        _amountOfObstacles = random.Next(1, _rangeOfObstacles);

        //get the max and min position
        _platformStartPosition = Mathf.RoundToInt(_platformPositionVector.x - _platformSize / 2);
        _platformEndPosition = Mathf.RoundToInt(_platformPositionVector.x + _platformSize / 2);

        _obstaclePosition = _platformPositionVector;

        for (int i = 0; i < _amountOfObstacles; i++)
        {
            _posX = random.Next(_platformStartPosition, _platformEndPosition);
            _obstaclePosition.x = _posX;
            _obstaclePosition.y = _platform.transform.position.y + _platform.transform.localScale.y / 2 + 0.5f;
            objectPooler.SpawningFromPool("Square", _obstaclePosition, _platform);
        }

        //check position
        // _obstacleCheckingList.Add(_posX);
        // _isValid = true;

        // if (_isValid)
        // {
        //     _obstaclePosition = _platformPositionVector;
        //     _obstaclePosition.x = _posX;
        //     _obstaclePosition.y = platform.transform.position.y + platform.transform.localScale.y / 2 + 0.5f;
        //     objectPooler.SpawningFromPool("Square", _obstaclePosition, platform);
        // }
        _index +=1;
    }
    private void SpawnPlatform()
    {
        Vector3 pos = transform.position;
        Vector3 minPos = pos;
        minPos.x -= 16; 
        Vector3 maxPos = pos;
        maxPos.x += 16;
        var temp = Random1.Range(minPos.x, maxPos.x);
        Vector3 platPos = pos;
        platPos.x = temp;
        GameObject newPlat = Instantiate(platform, platPos , Quaternion.identity) as GameObject;
        newPlat.transform.localScale = new Vector3(Random1.Range(4,10),Random1.Range(1,4), 1);
        newPlat.gameObject.tag = "platform";
    }
    void Start()
    {
        objectPooler = KillableObstaclesPool.Instance;
        InvokeRepeating("SpawnPlatform",3f,3f);
        InvokeRepeating("SpawnKillableObjects", 3.5f, 3.5f);
    }
    void Update() 
    {
        
        //generate a position and spawn it 

        // if (_index == _obstacleAmountList.Count)
        // {
        //     this.enabled = false;
        // }


            //restriction

            // all the _isValid will be turn into something else
            // if (_count != 0)
            // {
            //     if (_obstacleCheckingList.Contains(_tempPosX))
            //     {
            //         if (_posX < _maxPos/2)
            //         {
            //             _posX = random.Next(_tempPosX +2, _maxPosInt);
            //             // _isValid = false;
            //         }
            //         else
            //         {
            //             _posX = random.Next(_minPosInt, _tempPosX - 1);
            //             // _isValid = false;
            //         }
            //     }
            //     if (_count >= 2)
            //     {
            //         //looping list and check position
            //         _obstacleCheckingList.ForEach(delegate(int pos)
            //         {
            //             if (_obstacleCheckingList.Contains(pos + 1))
            //             {
            //                 if (_obstacleCheckingList.Contains(pos - 1))
            //                 {
            //                     _isValid = false;
            //                 }
            //                 else if (_obstacleCheckingList.Contains(pos + 2))
            //                 {
            //                     _isValid = false;
            //                 }
            //             }
            //             if (_obstacleCheckingList.Contains(pos - 1))
            //             {
            //                 if (_obstacleCheckingList.Contains(pos - 2))
            //                 {
            //                     _isValid = false;
            //                 }
            //             }
            //         });
            //     }
            // }
            //to check previous position
            // _tempPosX = _posX;

            // //spawning 
            // if (_isValid)
            // {
            //     _platform = _platformsList[_index];
            //     _obstaclePosition = _platformPositionVectorList[_index];
            //     _obstaclePosition.x = _posX;
            //     _obstaclePosition.y = _platform.transform.position.y + _platform.transform.localScale.y/2  + 1.0f;
            //     objectPooler.SpawningFromPool("Spike", _obstaclePosition, _platform);             
            // }
            //     _count += 1;
    }
}   

