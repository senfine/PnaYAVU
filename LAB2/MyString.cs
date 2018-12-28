using System;

namespace LAB2
{
    class MyString
    {
        private char[] str; // массив символов

        /* Конструкторы */
        #region
        public MyString(char[] array) {
            str = new char[array.Length];
            array.CopyTo(str, 0);
        }

        public MyString(string array) { str = array.ToCharArray(); }

        #endregion

        /* Перегрузка операторов */
        #region
        public static MyString operator +(MyString _strF, MyString _strT) { return _strF.Concat(_strT); }
        public static MyString operator +(MyString _strF, string _strT) { return _strF.Concat(_strT); }

        public static int operator <(MyString _strF, MyString _strT) { return Compare(_strF, _strT); }
        public static int operator <(MyString _strF, string _strT) { return Compare(_strF, _strT); }
        public static int operator >(MyString _strF, MyString _strT) { return Compare(_strF, _strT); }
        public static int operator >(MyString _strF, string _strT) { return Compare(_strF, _strT); }

        public static bool operator ==(MyString _strF, MyString _strT)
        {
            if (Compare(_strF, _strT) == 0) return true;
            return false;
        }
        public static bool operator ==(MyString _strF, string _strT)
        {
            if (Compare(_strF, _strT) == 0) return true;
            return false;
        }

        public static bool operator !=(MyString _strF, MyString _strT)
        {
            if (Compare(_strF, _strT) == 0) return false;
            return true;
        }
        public static bool operator !=(MyString _strF, string _strT)
        {
            if (Compare(_strF, _strT) == 0) return false;
            return true;
        }
        #endregion

        /* Методы для работы с массивом символов */
        #region
        public int Len => str.Length; // длина строки
        
        public MyString Concat(MyString _strT) // объединение строк
        {
            int count = str.Length + _strT.Len;
            int t_char = 0;
            char[] result = new char[count];

            for (int i = 0; i < str.Length; i++)
            {
                result[t_char] = str[i];
                t_char++;
            }
               
            for (int i = 0; i < _strT.Len; i++)
            {
                result[t_char] = _strT.str[i];
                t_char++;
            }

            return new MyString(result);
        }
        public MyString Concat(string _strT) // объединение строк
        {
            int count = str.Length + _strT.Length;
            int t_char = 0;
            char[] result = new char[count];

            for (int i = 0; i < str.Length; i++)
            {
                result[t_char] =str[i];
                t_char++;
            }

            for (int i = 0; i < _strT.Length; i++)
            {
                result[t_char] = _strT[i];
                t_char++;
            }

            return new MyString(result);
        }

        public static int Compare(MyString _strF, MyString _strT) // сравнение
        {
            int result = 0;

            int min = Math.Min(_strF.Len, _strT.Len);

            for(int i = 0; i < min; i++)
            {
                int a = (int)_strF.str[i];
                int b = (int)_strT.str[i];

                if (a > b)
                    result = -1;
                else if (a < b)
                    result = 1;
            }

            return result;
        }
        public static int Compare(MyString _strF, string _strT) // сравнение
        {
            int result = 0;

            int min = Math.Min(_strF.Len, _strT.Length);

            for (int i = 0; i < min; i++)
            {
                int a = (int)_strF.str[i];
                int b = (int)_strT[i];

                if (a > b)
                    result = -1;
                else if (a < b)
                    result = 1;
            }

            return result;
        }

        public MyString Sub(int _a) // извлекает из строки подстроку
        {
            int len = str.Length - _a;

            if (len < 0)
                len = 0;

            char[] result = new char[len];
            
            int j = 0;
            for (int i = _a; i < str.Length; i++)
            {
                result[j] = str[i];
                j++;
            }
            
            return new MyString(result);
        }
        public MyString Sub(int _a, int _b) // извлекает из строки подстроку
        {
            int len = str.Length;

            if (_b > 0)
            {
                if (_a + _b < len)
                    len = _a + _b;
            }
            else
                len = len + _b;

            char[] result = new char[len - _a + 1];
            int j = 0;
            for (int i = _a; i < len; i++)
            {
                result[j] = str[i];
                j++;
            }
            return new MyString(result);
        }

        public int IndexOf(char symb) // находит индекс первого вхождения символа c отчетом от нуля
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == symb) return i + 1;
            }

            return -1;
        }
        public int IndexOf(char symb, int _a) // находит индекс первого вхождения символа 
        {
            for (int i = _a; i < str.Length; i++)
            {
                if (str[i] == symb) return i + 1;
            }

            return -1;
        }

        #endregion

        /* Перегрузка операций преобразования типов */
        #region
        public static implicit operator string(MyString _str) { return new string(_str.str); }
        public static explicit operator int(MyString _str) { return _str.Len; }
        #endregion

    }
}
