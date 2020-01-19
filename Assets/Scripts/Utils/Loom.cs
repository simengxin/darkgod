using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Platform.Wrapper
{
    public class Loom : MonoBehaviour
    {
        private static int _mainThreadId;
        
        private const int MAX_THREAD_COUNT = 10;
        static int numThreads;

        private static Loom _current;

        public static Loom Current
        {
            get
            {
                Initialize();
                return _current;
            }
        }

        static bool initialized;

        /**
         * 初始化方法，需要在初始化场景中调用一次即可；
         */
        public static void Initialize()
        {
            if (!initialized)
            {
                if (!Application.isPlaying)
                    return;
                initialized = true;
                GameObject g = new GameObject("Loom");

                DontDestroyOnLoad(g);
                _current = g.AddComponent<Loom>();
            }
        }

        public static void InitMainThread() {
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public static bool IsMainThread() {
            return Thread.CurrentThread.ManagedThreadId == _mainThreadId;
        }

        private List<Action> _actions = new List<Action>();

        public struct DelayedQueueItem
        {
            public float time;
            public Action action;
        }

        private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

        List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

        /**
         * 运行在主线程
         */
        public static void QueueOnMainThread(Action action)
        {
            if (IsMainThread()) {
                action();
                return;
            }
            QueueOnMainThread(action, 0f);
        }

        /**
         * 运行在主线程
         *
         *  延迟delayTime（s）后执行；
         */
        public static void QueueOnMainThread(Action action, float delayTime)
        {
            if (delayTime != 0)
            {
                if (Current != null)
                {
                    lock (Current._delayed)
                    {
                        Current._delayed.Add(new DelayedQueueItem {time = Time.time + delayTime, action = action});
                    }
                }
            }
            else
            {
                if (Current != null)
                {
                    lock (Current._actions)
                    {
                        Current._actions.Add(action);
                    }
                }
            }
        }

        /**
         * 启动一个异步任务，如果超过当前处理能力则等待；
         * 注：之后再优化等待任务；
         */
        public static Thread RunAsync(Action a)
        {
            Initialize();
            while (numThreads >= MAX_THREAD_COUNT)
            {
                Thread.Sleep(1);
            }

            Interlocked.Increment(ref numThreads);
            ThreadPool.QueueUserWorkItem(RunAction, a);
            return null;
        }

        private static void RunAction(object action)
        {
            try
            {
                ((Action) action)();
            }
            catch(Exception exception)
            {
                Debug.LogError(exception);
            }
            finally
            {
                Interlocked.Decrement(ref numThreads);
            }
        }


        void OnDisable()
        {
            if (_current == this)
            {
                _current = null;
            }
        }

        private readonly List<Action> _currentActions = new List<Action>();

        // Update is called once per frame  
        void Update()
        {
            lock (_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }

            foreach (var a in _currentActions)
            {
                a();
            }

            lock (_delayed)
            {
                _currentDelayed.Clear();
                _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
                foreach (var item in _currentDelayed)
                    _delayed.Remove(item);
            }

            foreach (var delayed in _currentDelayed)
            {
                delayed.action();
            }
        }
    }
}