using System;
namespace CapstoneProject.Areas.Orders.Models.Schemas
{
	public class PaymentInfo
	{
		public PaymentInfo()
		{
		}
		public int Amount { get; set; }
		public string Vnp_TxnRef { get; set; }
        public string Vnp_BankCode { get; set; }
		public string Vnp_PayDate { get; set; }
        public string Vnp_ResponseCode { get; set; }
		public string Vnp_TransactionStatus { get; set; }
		public string Vnp_TransactionNo { get; set; }
    }
}

