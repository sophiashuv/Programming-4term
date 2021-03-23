using System;
using System.Collections;
using System.Collections.Generic;

namespace module_04.Properties
{
    class ServiceComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            Service oneService = x as Service;
            Service anotherService = y as Service;
            if (oneService != null && anotherService != null)
            {
                return oneService.Title.CompareTo(anotherService.Title);
            }
            else
            {
                throw new ArgumentException("Parameter is not a Service!");
            }
        }
    }
}