using System.Collections.Generic;
using UnityEngine;

public class PlacementOfKillableObstacles : MonoBehaviour
{
  KillableObstaclesPool objectPooler;

  //declaration
  public GameObject platform;
  private int _rangeOfObstacles,_amountOfObstacles, _amountOfSlotsOnPlatform;
  private int  _index = 0, _posX;
  private int _platformStartPosition, _platformEndPosition;
  private float  _platformSize, _maxPos, _minPos;
  private Vector3 _platformPositionVector, _obstaclePosition;
  private GameObject  _platform;
  private GameObject[] _platformsList;
  private List<int> _obstaclePositionList = new List<int>();
  private bool _initialPlatform = true;

  private void SpawnKillableObjects()
  {
    //getting platform's information
    _platformPositionVector = _platform.transform.position;
    _platformSize = _platform.transform.localScale.x;
    _amountOfSlotsOnPlatform = Mathf.RoundToInt(_platformSize);

    //get amount of obstacles in a single platform
    _rangeOfObstacles = 3;
    if (_amountOfSlotsOnPlatform > 15)
    {
      _rangeOfObstacles = 5;
    }
    _amountOfObstacles = Random.Range(1, _rangeOfObstacles);

    //get the max and min position
    _platformStartPosition = (int)(_platformPositionVector.x - _platformSize / 2 + 1);
    _platformEndPosition = (int)(_platformPositionVector.x + _platformSize / 2 - 1);

    //generate a list of position based on number of obstacles
    for (int i = 0; i < _amountOfObstacles; i++)
    {
      _posX = Random.Range(_platformStartPosition, _platformEndPosition);
      //restrict 3 spike spawning together
      if (i == 2)
      {
        int _firstIndex = _obstaclePositionList[0];
        int _secondIndex = _obstaclePositionList[1];
        if ( _firstIndex + 1 == _secondIndex || _firstIndex + 2 == _secondIndex || _firstIndex - 1 == _secondIndex || _firstIndex - 2 == _secondIndex )
        {
          if (_secondIndex > _platformPositionVector.x  && _firstIndex > _platformPositionVector.x)
          {
            _posX = Random.Range(_platformStartPosition, Mathf.RoundToInt(_platformPositionVector.x));
          }
          else 
          {
            _posX = Random.Range(Mathf.RoundToInt(_platformPositionVector.x), _platformEndPosition);
          }
        }
      }

      //check if same position
      _posX = CheckSamePosition(_posX);
      _obstaclePositionList.Add(_posX);
    }

    //spawn for each obstacle
    _obstaclePosition = _platformPositionVector;
    foreach (int posX in _obstaclePositionList)
    {
      _obstaclePosition.x = posX;
      _obstaclePosition.y = _platform.transform.position.y + _platform.transform.localScale.y / 2 + 0.5f;
      objectPooler.SpawningFromPool("Spike", _obstaclePosition, _platform);
    }
    _obstaclePositionList.Clear();
    _index +=1;

  }   

  private int CheckSamePosition(int position)
  {
    if (_obstaclePositionList.Count == 1)
    {
      return position;
    }
    
    if (_obstaclePositionList.Contains(position))
    {
      if (position == _platformStartPosition)
      {
        position = Random.Range(_platformStartPosition + 1, _platformEndPosition);
      }
      else if (position == _platformEndPosition)
      {
        position = Random.Range(_platformStartPosition, _platformEndPosition - 1);
      }
      else if (position > _platformPositionVector.x)
      {
        position = Random.Range(_platformStartPosition, Mathf.RoundToInt(_platformPositionVector.x));
      }else if (position < _platformPositionVector.x)
      {
        position = Random.Range(Mathf.RoundToInt(_platformPositionVector.x), _platformEndPosition);
      }

      return position;
    }else 
    {
      return position;
    }
  }

  //temporary method
  private void SpawnPlatform()
  {
    Vector3 pos = transform.position;
    Vector3 minPos = pos;
    minPos.x -= 16; 
    Vector3 maxPos = pos;
    maxPos.x += 16;
    var temp = Random.Range(minPos.x, maxPos.x);
    Vector3 platPos = pos;
    platPos.x = temp;
    GameObject newPlat = Instantiate(platform, platPos , Quaternion.identity) as GameObject;
    newPlat.transform.localScale = new Vector3(Random.Range(6, 10), Random.Range(1, 4), 1);
    newPlat.gameObject.tag = "platform";
  }
  private void Start()
  {
    objectPooler = KillableObstaclesPool.Instance;
    // InvokeRepeating("SpawnPlatform", 0.0f, 2.0f);
    InvokeRepeating("SpawnKillableObjects",0.1f, 2.1f);
  }
  private void Update()
  {

  } 
}


