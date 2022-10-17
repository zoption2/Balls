using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TheGame
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private Transform[] fieldColumns;
        private List<CellPosition> allBusyPositions = new List<CellPosition>();

        private CellPosition[,] grid;
        private List<CellPosition> firstRow;

        [Inject]
        public void Inject()
        {

        }

        public void Initialize()
        {
            BuildGrid();
            BuildFirstRow();
        }

        private void BuildGrid()
        {
            var columns = fieldColumns.Length;
            var rows = fieldColumns[0].childCount;
            grid = new CellPosition[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var positions = fieldColumns[j].GetComponentsInChildren<CellPosition>();
                    grid[i, j] = positions[j];
                    positions[j].SetAdress(i, j);
                }
            }
        }

        private void BuildFirstRow()
        {
            var length = fieldColumns.Length;
            firstRow = new List<CellPosition>(length);
            for (int i = 0; i < length; i++)
            {
                firstRow[i] = grid[0, i];
            }
        }

        public bool IsFirstRawEmpty()
        {
            for (int i = 0, j = grid.GetLength(1); i < j; i++)
            {
                var position = grid[0, i];
                if (position.IsPointBusy)
                {
                    return false;
                }
            }
            return true;
        }

        public void PlaceEnemies(List<Enemy> enemies)
        {
            if (IsFirstRawEmpty())
            {
                var neededCount = enemies.Count;
                var randomEmptyIndexes = GetRandomIndexes(neededCount, firstRow);

                for (int i = 0, j = randomEmptyIndexes.Count; i < j; i++)
                {
                    var index = randomEmptyIndexes[i];
                    var enemy = enemies[i];
                    var position = randomEmptyIndexes[i];
                    position.KeepPosition(enemy);
                    allBusyPositions.Add(position);
                }
            }
        }

        public void MoveEnemies()
        {
            for (int i = allBusyPositions.Count - 1, j = 0; i >= j; i--)
            {
                var enemy = allBusyPositions[i].Enemy;
                var nextPos = FieldMovement.GetNextPosition(grid, allBusyPositions[i], enemy.MoveType, null);
            }
        }

        //private bool TryGoNext(CellPosition current, out CellPosition next)
        //{
        //    var currentPos = current.Address;
        //    next = positions[currentPos + 1];
        //    if (!next.IsPointBusy)
        //    {
        //        current.LeavePosition();
        //        next.KeepPosition(current.Enemy, this);
        //        return true;
        //    }
        //    return false;
        //}

        private List<CellPosition> GetRandomIndexes(int neededAmount, List<CellPosition> allPositions)
        {
            var tempList = allPositions;
            var resultList = new List<CellPosition>(neededAmount);

            for (int i = 0; i < neededAmount; i++)
            {
                var position = tempList[Random.Range(0, tempList.Count - 1)];
                resultList[i] = position;
                tempList.Remove(position);
            }

            return resultList;
        }
    }
}

