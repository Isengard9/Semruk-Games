using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace NC.Strategy.Managers.Game.InGameDynamics.NavMesh
{
    public class NavMeshManager : MonoBehaviour
    {
        private NavMeshSurface[] _surfaces;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            _surfaces = FindObjectsOfType<NavMeshSurface>();

            foreach (var surface in _surfaces)
            {
                surface.BuildNavMesh();
            }
        }
    }
}