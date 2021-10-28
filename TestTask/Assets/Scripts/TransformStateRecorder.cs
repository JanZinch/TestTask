using System.Collections.Generic;
using UnityEngine;

class TransformStateRecorder
{
    private Stack<Vector3> _positions = null;
    private Stack<Quaternion> _rotation = null;

    public TransformStateRecorder() {

        _positions = new Stack<Vector3>();
        _rotation = new Stack<Quaternion>();
    }

    public void Record(Transform transform) {

        _positions.Push(transform.position);
        _rotation.Push(transform.rotation);
    }

    public bool IsEmpty() {

        return _positions.Count == 0 || _rotation.Count == 0;
    }

    public void PlayNext(Transform transform) {

        transform.position = _positions.Pop();
        transform.rotation = _rotation.Pop();
    }
}
