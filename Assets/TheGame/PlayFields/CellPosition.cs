using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheGame
{
    public class CellPosition : MonoBehaviour
    {
        private Enemy enemy;
        private bool isPointBusy;
        public bool IsPointBusy => isPointBusy;
        public Enemy Enemy => enemy;
        public Address Address { get; private set; }


        public void SetAdress(int rows, int columns)
        {
            Address = new Address(rows, columns);
        }

        public void KeepPosition(Enemy enemy)
        {
            if (!isPointBusy)
            {
                this.enemy = enemy;
                isPointBusy = true;
            }
        }

        public void LeavePosition()
        {
            isPointBusy = false;
            enemy = null;
        }
    }

    public class FieldMovement
    {
        public static CellPosition GetNextPosition(CellPosition[ , ] grid, CellPosition current, EnemyMoveType moveType, System.Action onFinish)
        {
            var address = current.Address;
            var totalRows = grid.GetLength(0);
            CellPosition newPosition = current;

            int next = address.rows + 1;
            if (next >= totalRows)
            {
                onFinish?.Invoke();
                return current;
            }

            switch (moveType)
            {
                case EnemyMoveType.nextCell:
 
                    newPosition = grid[next, address.columns];
                    break;
                case EnemyMoveType.overNextCell:
                    break;
                case EnemyMoveType.randomeEmpty:
                    break;
                case EnemyMoveType.randomeColumnNext:
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }

            return newPosition;
        }
    }

    public struct Address
    {
        public int rows;
        public int columns;

        public Address(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
        }
    }
}

