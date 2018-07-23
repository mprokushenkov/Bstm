using System.Management.Automation;
using Bstm.Common;
using Bstm.PowerShellServices.Commands;
using JetBrains.Annotations;

namespace Bstm.PowerShellServices
{
    // ReSharper disable once InconsistentNaming
    internal static class PSObjectExtensions
    {
        public static void SetProperty([NotNull] this PSObject psObject, [NotNull] string name, [NotNull] object value)
        {
            Guard.CheckNull(psObject, nameof(psObject));
            Guard.CheckNull(name, nameof(name));
            Guard.CheckNull(value, nameof(value));

            psObject.Properties.Add(new PSNoteProperty(name, value));
        }

        public static T GetProperty<T>([NotNull] this PSObject psObject, [NotNull] PsObjectProperty property)
        {
            return (T) psObject.Properties[property]?.Value;
        }

        public static string GetProperty([NotNull] this PSObject psObject, [NotNull] PsObjectProperty property)
        {
            return psObject.Properties[property]?.Value.ToString();
        }
    }
}