using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TheGame
{
    public class FieldFactory : AddressablesFactory<Field, ScenarioType>, IFieldFactory
    {
        public async Task<Field> Create(ScenarioType scenario, int teamID, Vector2 position)
        {
            var fieldWaiter = GetPrefab(scenario);
            await fieldWaiter;
            var fieldPrefab = fieldWaiter.Result;

            var goField = GameObject.Instantiate<Field>(fieldPrefab, position, Quaternion.identity);
            var field = goField.GetComponent<Field>();
            field.Initialize(teamID);
            return field;
        }
    }

    public interface IFieldFactory : IAddressableFactory<Field, ScenarioType>
    {
        Task<Field> Create(ScenarioType scenario, int teamID, Vector2 position);
    }
}

