using System;

namespace Stack
{
    public class Stack<T> where T: ICloneable
    {
        public Stack(uint size = 100)
        {
            items = new T[size];
        }

        public void Push(T item)
        {
            if (isFull())
                throw new StackOverflowException();
            items[stackPointer] = (T) item.Clone();
            stackPointer++;
        }

        public T Pop()
        {
            if (isEmpty())
                throw new InvalidOperationException("Can not pop an empty stack");
            stackPointer--;
            var t = items[stackPointer];
            items[stackPointer] = default;
            return t;
        }

        public bool isEmpty() => stackPointer == 0;
        
        public bool isFull() => stackPointer == Length;

        public uint Length
        {
            get => (uint) items.Length;
        }

        private T[] items;
        private uint stackPointer = 0;
        
    }
}