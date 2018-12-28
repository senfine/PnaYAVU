using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3pnyavy
{
    class MyArray<T> : Node <T>
    {
        public MyArray() //конструктор
        {
            count = 0;
            items = new T[n];
        }

        public MyArray(int length)
        {
            items = new T[length];
        }

        public void Add(T item)
        {
            if (count == items.Length) //если переполнение
            {
                var newArray = new T[2 * items.Length];
                Array.Copy(items, 0, newArray, 0, head);
                items = newArray; //просто создаём новый массив с двойным размером
            }
            items[count++] = item;    
        }

        public void Remove (int i)
        {
            for(int j=0; j<count; j++)
            {
                items[j] = items[j + 1];
                items[i] = default(T);
            }
            count--;
        }
        
    }
}
