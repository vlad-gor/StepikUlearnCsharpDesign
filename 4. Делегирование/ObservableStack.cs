// Вставьте сюда финальное содержимое файла ObservableStack.cs
using System;
using System.Collections.Generic;
using System.Text;

namespace Delegates.Observers
{
	public class StackOperationsLogger
	{
		private readonly Observer observer = new Observer();	

		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.NotifyEvent += observer.HandleEvent;
		}

		public string GetLog()
		{
			return observer.Log.ToString();
		}
	}

	public class Observer
	{
		public StringBuilder Log = new StringBuilder();

		public void HandleEvent(object eventData)
		{
			Log.Append(eventData);
		}   
	}

	public delegate void MyDelegate(object obj);

	public class ObservableStack<T>
	{
		List<Observer> observers = new List<Observer>();
		public event MyDelegate NotifyEvent;

		List<T> data = new List<T>();

		public void Push(T obj)
		{
			data.Add(obj);
			NotifyEvent?.Invoke(new StackEventData<T> { IsPushed = true, Value = obj });			
		}

		public T Pop()
		{
		 	if (data.Count == 0)
		 		throw new InvalidOperationException();
		 	var result = data[data.Count - 1];
			NotifyEvent?.Invoke(new StackEventData<T> { IsPushed = false, Value = result });
		 	return result;			
		}
	}
}