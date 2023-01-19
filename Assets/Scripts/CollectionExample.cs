using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runner
{
    public class CollectionExample : MonoBehaviour
    {
        public Book[] _array;
        public List<Book> _list;
        public Dictionary<float, Book> _dictionary;

        public Queue<Book> _queue; //FIFO first in first out
        public Stack<Book> _stack; //LIFO last in first out

        public LinkedList<Book> _linkedList;

        private Book _currentBook;

        private void Start()
        {
            #region Array

            _array = new Book[10];

            for (int i = 0; i < 10; i++)
            {
                _array[i] = new Book(Random.Range(0.99f, 19.99f), "test", Random.Range(100, 200));
            }

            for (int i = 0; i < _array.Length; i++)
            {
                Debug.Log(_array[i].Price);
            }

            foreach (Book book in _array)
            {
                Debug.Log(book.Price);
            }

            List<Book> temp = new List<Book>();
            foreach (Book book in _array)
            {
                if (book.PageCount < 100)
                {
                    temp.Add(book);
                }
            }
           
            temp.Sort();

            Debug.Log(_array[Random.Range(0, _array.Length)].Price);

            #endregion

            #region List

            _list = new List<Book>(10);
            for (int i = 0; i < 10; i++)
            {
                _list.Add(new Book(Random.Range(0.99f, 19.99f), "test", Random.Range(100, 200)));
            }

            Debug.Log(_list[Random.Range(0, _array.Length)].Price);
            _list.RemoveAt(Random.Range(0, _array.Length));
            _currentBook = _list[5];
            _list.Remove(_currentBook);
            Debug.Log(_list.Count);
            for (int i = 0; i < 10; i++)
            {
                _list.Add(new Book(Random.Range(0.99f, 19.99f), "test", Random.Range(100, 200)));
            }

            Debug.Log(_list.Count);

            int[] tempList = _list.Where(book => book.PageCount < 100 && book.Price > 5f).OrderBy(book => book.PageCount ).Select(book =>(int) book.Price ).ToArray();


            PlayerController[] players = FindObjectsOfType<PlayerController>();


            float dist = float.MaxValue;
            PlayerController nearestPlayer;
            foreach (var player in players)
            {
                var distToPlayer = Vector3.Distance(transform.position, player.transform.position); 
                if (distToPlayer < dist)
                {
                    dist = distToPlayer;
                    nearestPlayer = player;
                }
            }

            nearestPlayer = players.OrderBy(player => Vector3.Distance(transform.position, player.transform.position)).FirstOrDefault();
            
            
            
            
            

            #endregion

            #region Dictionary

            _dictionary = new Dictionary<float, Book>(10);
            float price = 0f;
            for (int i = 0; i < 10; i++)
            {
                price = Random.Range(0.99f, 19.99f);
                _dictionary.Add(price, new Book(price, "test", Random.Range(100, 200)));
            }

            Debug.Log(_dictionary[price].PageCount);

            #endregion

            #region Queue

            _queue = new Queue<Book>(10);

            for (int i = 0; i < 10; i++)
            {
                _queue.Enqueue(new Book(Random.Range(0.99f, 19.99f), "test", Random.Range(100, 200)));
            }
            Debug.Log(_queue.Count);
            Debug.Log(_queue.Peek().Price);
            Debug.Log(_queue.Count);
            Debug.Log(_queue.Dequeue().Price);
            Debug.Log(_queue.Count);
            Debug.Log(_queue.Dequeue().Price);
            Debug.Log(_queue.Count);
            #endregion

            #region Stack

            _stack = new Stack<Book>(10);
            for (int i = 0; i < 10; i++)
            {
                _stack.Push(new Book(Random.Range(0.99f, 19.99f), "test", Random.Range(100, 200)));
            }
            Debug.Log(_stack.Count);
            Debug.Log(_stack.Peek().Price);
            Debug.Log(_stack.Count);
            Debug.Log(_stack.Pop().Price);
            Debug.Log(_stack.Count);
            Debug.Log(_stack.Pop().Price);
            Debug.Log(_stack.Count);
            
            #endregion

            #region LinkedList

            _linkedList = new LinkedList<Book>();

            for (int i = 0; i < 10; i++)
            {
                _linkedList.AddLast(new Book(Random.Range(0.99f, 19.99f), "test", Random.Range(100, 200)));
            }

            Debug.Log(_linkedList.First.Value.Price);
            Debug.Log(_linkedList.Last.Value.Price);
            
            #endregion

        }
    }

    [Serializable]
    public class Book
    {
        public float Price;
        public string Title;
        public int PageCount;

        public Book(float price, string title, int pageCount)
        {
            Price = price;
            Title = title;
            PageCount = pageCount;
        }
    }
}