using System.ComponentModel;

namespace DSA;

public class _Queue<T>
{
    T[] _arr;
    int rear;
    const int _front = 0;

    public _Queue(int size = 10)
    {
        _arr = new T[size];
        rear = 0;
    }

    public void Push(T item)
    {
        if (rear == _arr.Length)
            throw new Exception("Queue is full");
        _arr[rear++] = item;
    }

    public T? Pop()
    {

        if (rear == 0)
            return default(T);
        
        T re = _arr[_front];

        for (int i = 0; i < rear - 1; i++)
        {
            _arr[i] = _arr[i + 1];
        }

        rear--;
        return re;
    }
}
