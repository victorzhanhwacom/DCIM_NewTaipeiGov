namespace VzDev.InterfaceUtils
{
    public interface IReceiveData<in T>
    {
        /// 接收資料T
        void ReceiveData(T data);
    }
}