using System.Collections.Generic;
using UnityEngine;

class TransformStateRecorder
{
    private Stack<Vector3> _positions = null;
    private Stack<Vector3> _eulerAngles = null;

    public TransformStateRecorder() {

        _positions = new Stack<Vector3>();
        _eulerAngles = new Stack<Vector3>();
    }

    public void Record(Transform transform) {

        _positions.Push(transform.position);
        _eulerAngles.Push(transform.eulerAngles);
    }

    public void Record(Vector3 position, Vector3 eulerAngles)
    {
        _positions.Push(position);
        _eulerAngles.Push(eulerAngles);
    }

    public bool IsEmpty() {

        return _positions.Count == 0 || _eulerAngles.Count == 0;
    }

    public void PlayNext(Transform transform) {

        transform.position = _positions.Pop();
        transform.eulerAngles = _eulerAngles.Pop();
    }
}
