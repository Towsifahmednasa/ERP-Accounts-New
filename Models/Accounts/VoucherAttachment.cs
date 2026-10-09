using System;

namespace New_Accounts.Models.Accounts
{
    // Voucher attachment metadata (tblVoucherAttachment); FileData travels separately on download only.
    public class VoucherAttachment
    {
        public int AttachmentId { get; set; }
        public int VoucherMasterId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public byte[] FileData { get; set; }
        public string EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
    }

    // One closed month of a company (tblFiPeriodClose); only closed months are returned.
    public class FiPeriodClose
    {
        public int PeriodMonth { get; set; }
        public bool IsClosed { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
