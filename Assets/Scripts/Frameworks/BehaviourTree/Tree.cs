using System;
using System.Collections.Generic;

namespace Frameworks.BehaviourTree
{
    public sealed class Tree
    {
        private Node _Root;
        public Dictionary<string, object> _TreeData { get; private set; } = new();

        public Tree(Node node)
        {
            _Root = node;
            Init();
        }

        private void Init()
        {
            _Root.Init(this);
        }

        public void Run()
        {
            _Root.Evaluate();
        }

        public void WriteData<T>(string key, T data)
        {
            if (_TreeData.ContainsKey(key))
                _TreeData[key] = data;
            else
                _TreeData.Add(key, data);
        }

        public T ReadData<T>(string key)
        {
            if (_TreeData.ContainsKey(key))
                return (T)Convert.ChangeType(_TreeData[key], typeof(T));
                // return (T)_TreeData[key];

            return default;
        }
    }
}
