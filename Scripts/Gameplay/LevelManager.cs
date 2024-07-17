using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        public int LevelId { get; private set; }

        [SerializeField] private List<GameObject> _levelPrefabs;

        [SerializeField] private Transform _levelParent;

        private GameObject _level;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }
        public void SpawnLevel()
        {
            LevelId = PlayerPrefs.GetInt("CurrentLevel", 0);
            SortingUI.Instance.UpdateLevelNumber(LevelId);
            if (_level != null)
                Destroy(_level);
            StartCoroutine(Spawn());
        }
        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(0.2f);
            if (LevelId < 8)
                _level = Instantiate(_levelPrefabs[LevelId], _levelParent);
            else
                _level = Instantiate(_levelPrefabs[8], _levelParent);
        }
    }
}
