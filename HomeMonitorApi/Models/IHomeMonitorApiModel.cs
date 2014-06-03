namespace HomeMonitorApi.Models
{
    interface IHomeMonitorApiModel<out T> where T : class 
    {
        bool IsValid { get; }
        T ToData();
    }
}
