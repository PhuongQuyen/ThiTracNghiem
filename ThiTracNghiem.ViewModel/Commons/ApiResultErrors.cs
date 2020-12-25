namespace ThiTracNghiem_ViewModel.Commons
{
    public class ApiResultErrors<T>:ApiResult<T>
    {
        public ApiResultErrors(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
        public ApiResultErrors()
        {
            IsSuccessed = false;
        }
    }
}
