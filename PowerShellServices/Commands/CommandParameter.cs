using System;
using System.ComponentModel;
using Bstm.Common;

namespace Bstm.PowerShellServices.Commands
{
    public class CommandParameter : Enumeration
    {
        private readonly TypeConverter converter = TypeDescriptor.GetConverter(typeof(string));

        protected CommandParameter(string name) : base(name)
        {
        }

        protected CommandParameter(string name, Type systemType) : this(name)
        {
            converter = TypeDescriptor.GetConverter(systemType);
        }

        public static CommandParameter ComputerName { get; } = new CommandParameter("ComputerName");

        public static CommandParameter Command { get; } = new CommandParameter("Command");

        public object ConvertFromString(string value)
        {
            return converter.ConvertFromString(value);
        }
    }
}