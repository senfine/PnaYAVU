using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3pnyavy
{
    class MyQueue<T> : Node<T>
    {
        
        public MyQueue()
        {
            head = 0;
            tail = 0;
            count = 0;
            items = new T[n];
        }

        public MyQueue(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException();
            items = new T[capacity];
            head = 0;
            tail = 0;
            count = 0;
        }


        // Добавление элемента в очередь.
        public void Enqueue(T item)
        {
            if (count == items.Length)
            {
                var newArray = new T[2 * items.Length];
                Array.Copy(items, 0, newArray, 0, count);
                items = newArray; //просто создаём новый массив с двойным размером
            }
            items[tail] = item;
            tail = (tail + 1) % items.Length;
            count++;
        }

        // Извлечение элемента из очереди.
        public T Dequeue()
        {
            if (count == 0)
                throw new InvalidOperationException();
            T local = items[head];
            items[head] = default(T);
            head = (head + 1) % items.Length;
            count--;
            return local;
        }

        // Просмотр элемента на вершине очереди.  
        public T Peek()
        {
            if (count == 0)
                throw new InvalidOperationException();
            return items[head];
        }
        // Очистка очереди.
        public void Clear()
        {
            if (head < tail)
                Array.Clear(items, head, count);
            else
            {
                Array.Clear(items, head, items.Length - head);
                Array.Clear(items, 0, tail);
            }
            head = 0;
            tail = 0;
            count = 0;
        }
       
        // Проверка на нахождении элемента в очереди.       
        /*public bool Contains(T item)
        {
            int index = head;
            int num2 = count;
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            while (num2-- > 0)
            {
                if (item == null)
                {
                    if (items[index] == null)
                        return true;
                }
                else if ((items[index] != null) && comparer.Equals(items[index], item))
                    return true;
                index = (index + 1) % items.Length;
            }
            return false;
        }*/
 
        // Изменение ёмкости очереди.
        /*private void SetCapacity(int capacity)
        {
            var destinationArray = new T[capacity];
            if (count > 0)
            {
                if (head < tail)
                    Array.Copy(items, head, destinationArray, 0, count);
                else
                {
                    Array.Copy(items, head, destinationArray, 0, items.Length - head);
                    Array.Copy(items, 0, destinationArray, items.Length - head, tail);
                }
            }
            items = destinationArray;
            head = 0;
            tail = (count == capacity) ? 0 : count;
        }*/

       
        // Преобразование очереди в массив.
       
       /* public T[] ToArray()
        {
            var destinationArray = new T[count];
            if (count != 0)
            {
                if (head < tail)
                {
                    Array.Copy(items, head, destinationArray, 0, count);
                    return destinationArray;
                }
                Array.Copy(items, head, destinationArray, 0, items.Length - head);
                Array.Copy(items, 0, destinationArray, items.Length - head, tail);
            }
            return destinationArray;
        }*/
    }
}

