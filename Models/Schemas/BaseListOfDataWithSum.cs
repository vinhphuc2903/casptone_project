namespace FMStyle.RPT.Models.Schemas
{
    public abstract class BaseListOfDataWithSum<T, E> : BaseListOfData<T>
    {
        public BaseListOfDataWithSum() : base()
        {
        }

        /// <summary>
        /// Tổng
        /// </summary>
        /// <value></value>
        public E SummaryData { get; set; }
    }
}