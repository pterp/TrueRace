using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Storage<T>
    {
        private List<T> _list = new List<T>();

        public void AddList(T element)
        {
            _list.Add(element);
        }
    }
}
