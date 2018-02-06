namespace ArpmarCore.Domain
{
    public class ArpEntryDiff
    {
        private readonly ArpEntry _old;
        private readonly ArpEntry _new;
        private readonly ArpEntryDiffState _state;
        
        public ArpEntryDiff(ArpEntry old, ArpEntry @new)
        {
            _old = old;
            _new = @new;
            _state = DeduceState();
        }

        private ArpEntryDiffState DeduceState()
        {
            if (_old == null && _new != null)
                return ArpEntryDiffState.Added;

            if (_old != null && _new == null)
                return ArpEntryDiffState.Deleted;

            if (_old != null && _new != null)
                return ArpEntryDiffState.Edited;

            return ArpEntryDiffState.Unknown;
        }

        public override string ToString()
        {
            switch (_state)
            {
                case ArpEntryDiffState.Added:
                    return $"[+] Interface: {_new.Interface} IP: {_new.IpAddress} MAC: {_new.MacAddress}";

                case ArpEntryDiffState.Edited:
                    return $"[E] Interface: {_old.Interface} IP: {_old.IpAddress} -> {_new.IpAddress} MAC: {_old.MacAddress} -> {_new.MacAddress}";

                case ArpEntryDiffState.Deleted:
                    return $"[-] Interface: {_old.Interface} IP: {_old.IpAddress} MAC: {_old.MacAddress}";

                default:
                    return "[?] Dunno what this is.";                 
            }
        }
    }
}