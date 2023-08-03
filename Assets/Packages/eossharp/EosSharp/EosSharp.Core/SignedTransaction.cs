using System.Collections.Generic;

namespace EosSharp.Core
{
    public class SignedTransaction
    {
        //! Signatures this Transaction has been signed with
        public IEnumerable<string> Signatures { get; set; }
        //! Packed Transaction
        public byte[] PackedTransaction { get; set; }
    }
}
