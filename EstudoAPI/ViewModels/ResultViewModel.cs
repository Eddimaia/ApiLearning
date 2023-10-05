namespace EstudoAPI.ViewModels
{
    public class ResultViewModel<T> where T : class
    {
        public T Data { get; private set; }
        public List<string> Erros { get; private set; } = new();

        public ResultViewModel(T data, List<string> erros)
        {
            Data = data;
            Erros = erros;
        }

        public ResultViewModel(List<string> erros)
        {
            Erros = erros;
        }

        public ResultViewModel(T data)
        {
            Data = data;
        }
        public ResultViewModel(string erro)
        {
            Erros.Add(erro);
        }
    }
}
