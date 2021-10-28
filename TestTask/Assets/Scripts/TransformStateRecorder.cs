using System.Collections.Generic;
using UnityEngine;

class TransformStateRecorder
{
    private Stack<Vector3> _positions = null;


    public TransformStateRecorder() {

        _positions = new Stack<Vector3>();
    }

    public void Record(Vector3 position) {

        _positions.Push(position);
    }

    public bool IsEmpty() {

        return _positions.Count == 0;
    }

    public Vector3 Play() {

        return _positions.Pop();    
    }
}
