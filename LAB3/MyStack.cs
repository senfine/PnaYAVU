using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3pnyavy
{
    class MyStack<T> : Node<T>
    {
        public MyStack() //конструктор
        {
            head = 0;
            count = 0;
            items = new T[n];
        }

        public MyStack(int length)
        {
            items = new T[length];
        }

        public T Pop() //метод взятия с вершины
        {
            if (IsEmpty())
            {
                //вброс ошибки при взятии пустого стека(Overflow)
                throw new InvalidOperationException("Стек пуст");
            }
            T item = items[--head];
            count--;
            items[head] = default(T); // сбрасываем ссылку
            return item;            
        }


        public void Push(T item)
        {
            if (head == items.Length) //если переполнение
            {
                var newArray = new T[2 * items.Length];
                Array.Copy(items, 0, newArray, 0, head);
                items = newArray; //просто создаём новый массив с двойным размером
            }
            items[head++] = item;
            count++;
        }

        // возвращаем элемент из верхушки стека
        public T Peek()
         {
             if (head == 0)
             {
                 throw new InvalidProgramException();
             }

             return items[head - 1];
         }

        public void StClear()
        {
            if (head < Count)
                Array.Clear(items, head, count);
            /*else
            {
                Array.Clear(items, head, items.Length - head);
                Array.Clear(items, 0, tail);
            }*/
            head = 0;
            tail = 0;
            count = 0;
        }
        ~MyStack() { }
    }
}
