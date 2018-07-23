using Bstm.Common;

namespace Bstm.PowerShellServices.Commands
{
    public class PsObjectProperty : Enumeration
    {
        private PsObjectProperty(string name) : base(name)
        {
        }

        public static PsObjectProperty Alias { get; } = new PsObjectProperty("Alias");

        public static PsObjectProperty Database { get; } = new PsObjectProperty("Database");

        public static PsObjectProperty UseDatabaseQuotaDefaults { get; } =
            new PsObjectProperty("UseDatabaseQuotaDefaults");

        public static PsObjectProperty IssueWarningQuota { get; } = new PsObjectProperty("IssueWarningQuota");

        public static PsObjectProperty ProhibitSendQuota { get; } = new PsObjectProperty("ProhibitSendQuota");

        public static PsObjectProperty ProhibitSendReceiveQuota { get; } =
            new PsObjectProperty("ProhibitSendReceiveQuota");
    }
}