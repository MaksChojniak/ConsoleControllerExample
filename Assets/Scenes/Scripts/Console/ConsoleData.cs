
using System;
using System.ComponentModel.Design;

namespace Console
{

    public class CommandBaseData
    {
        public string id;
        public string format;

        public CommandBaseData(string id, string format)
        {
            this.id = id;
            this.format = format;
        }

    }
    
    
    public class CommandData : CommandBaseData
    {
        public Action action;

        public CommandData(string id, string format, Action action) : base(id, format)
        {
            this.action = action;
        }

        public void Invoke()
        {
            action.Invoke();
        }
    }
    
    public class CommandData<T> : CommandBaseData
    {
        public Action<T> action;

        public CommandData(string id, string format, Action<T> action) : base(id, format)
        {
            this.action = action;
        }

        public void Invoke(T value)
        {
            action.Invoke(value);
        }
        
    }
}
