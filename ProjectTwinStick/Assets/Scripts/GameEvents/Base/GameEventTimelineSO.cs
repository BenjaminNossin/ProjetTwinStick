using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameEventTimeline", menuName = "GameEvents/Timeline", order = 1)]
public class GameEventTimelineSO : ScriptableObject
{
   [SerializeField]
   private GameEventTimeCode[] _gameEventTimeCodes;

   public GameEventData[] GetGameEvents(int timeCodeIndex)
   {
      return _gameEventTimeCodes[timeCodeIndex].gameEventDatas;
   }
   
   public float GetTimeCode(int timeCodeIndex)
   {
      return _gameEventTimeCodes[timeCodeIndex].timeCode;
   }

   public int GetTimeCodeCount() => _gameEventTimeCodes.Length;


}
