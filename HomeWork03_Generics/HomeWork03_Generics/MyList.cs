using System;
using System.Collections;
using System.Collections.Generic;

namespace HomeWork03_Generics
{
    public class MyList<T>: IEnumerable<T>, ICloneable where T: IEquatable<T>, IComparable<T>
    {
        internal class Node {  
            internal T data;  
            internal Node next;  
            public Node(T d) {  
                data = d;  
                next = null;  
            }  
        }
        internal Node head;
        internal int Count 
        {
            get
            {
                Node temp = head;
                if (temp is null) return 0;
                var count = 0;
                while (temp != null)
                {
                    temp = temp.next;
                    count++;
                }

                return count;
            }
        }
        internal void InsertFront(T new_data) {    
            Node new_node = new Node(new_data);    
            new_node.next = head;    
            head = new_node;    
        }
        internal void InsertLast(T new_data) 
        {    
            Node new_node = new Node(new_data);    
            if (head == null) {    
                head = new_node;    
                return;    
            }    
            Node lastNode = GetLastNode();    
            lastNode.next = new_node;    
        }
        internal T this[int i]
        {
            get
            {
                Node temp = head;
                for (int j = 0; j < i; j++)
                    temp = temp.next;
                return temp.data;
            }
            set
            {
                Node temp = head;
                for (int j = 0; j < i; j++)
                    temp = temp.next;
                temp.data = value;
            }
        }
        internal void Sort()
        {
            T buff;
            for (int i = 0; i < Count; i++)
            {
                for (int j = Count - 1; j > i; j--)
                {
                    if (this[j].CompareTo(this[j - 1]) == -1)
                    {
                        buff = this[j];
                        this[j] = this[j - 1];
                        this[j - 1] = buff;
                    }
                }
            }
        }
        internal Node GetLastNode() {  
            Node temp = head;  
            while (temp.next != null) {  
                temp = temp.next;  
            }  
            return temp;  
        }
        internal void DeleteNodeByKey(T key)  
        {
            Node temp = head;  
            Node prev = null;
            if (temp != null && temp.data.Equals(key))
            {
                DeleteFront();
                return;
            }

            while (temp != null && !temp.data.Equals(key)) {  
                prev = temp;  
                temp = temp.next;  
            }  
            if (temp == null) 
                return;
            prev.next = temp.next;
        }
        internal void DeleteFront()  
        {
            if (head != null) head = head.next;
        }
        internal void DeleteLast()  
        {
            Node temp = head;  
            Node prev = null;
            while (temp.next != null) {  
                prev = temp;  
                temp = temp.next;  
            }
            prev.next = temp.next;  
        }
        public void ReverseLinkedList()  
        {  
            Node prev = null;  
            Node current = head;  
            Node temp = null;  
            while (current != null) {  
                temp = current.next;  
                current.next = prev;  
                prev = current;  
                current = temp;  
            }  
            head = prev;  
        }
        internal IEnumerator<T> Enumerator()
        {
            var current = head;
            while (current != null)
            {
                yield return current.data;
                current = current.next;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return Enumerator();
        }
        public override string ToString()
        {
            var res = "";
            Node current = head;
            while (current != null)
            {
                res += current.data + " ";
                current = current.next;
            }
            return res;
        }
        public object Clone()
        {
            var res = new MyList<T>();
            foreach (var el in this) res.InsertLast(el);
            return res;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        internal void uniqueElements() 
        {
            Dictionary<T, int> hash = new Dictionary<T, int>();
            for (Node temp = head; temp != null; temp = temp.next) 
            {
                if(hash.ContainsKey(temp.data))
                    hash[temp.data] = hash[temp.data] + 1; 
                
                else
                    hash.Add(temp.data, 1);
            } 
            
            for (Node temp = head; temp != null; temp = temp.next)
            {
                if (hash[temp.data] != 1)
                {
                    DeleteNodeByKey(temp.data);
                    hash[temp.data] -= 1;
                }
            }
        }
        internal void Clear()
        {
            while (head != null) DeleteFront();
        }
    }
}