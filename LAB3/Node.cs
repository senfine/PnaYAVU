using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3pnyavy
{
    abstract class Node<T>
    {      
        public int head;
        public int tail;
        public T[] items; // элементы стека
        public int count;  // количество элементов
        public const int n = 10;
        public int Count //параметр для вывода размера
        {
            get
            {
                return count;
            }
        }


        public bool IsEmpty()
        {
            return head == 0;
        }


    }
}
