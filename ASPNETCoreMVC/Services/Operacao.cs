namespace ASPNETCoreMVC.Services
{
    public class OperacaoService
    {
        public OperacaoService(IOperacaoTransient transiente, IOperacaoScoped scoped,
                               IOperacaoSingleton singleton, IOperacaoSingletonInstance singletonInstance)
        {
            Transiente = transiente;
            Scoped = scoped;
            Singleton = singleton;
            SingletonInstance = singletonInstance;
        }

        public IOperacaoTransient Transiente { get; set; }
        public IOperacaoScoped Scoped { get; set; }
        public IOperacaoSingleton Singleton { get; set; }
        public IOperacaoSingletonInstance SingletonInstance { get; set; }
    }

    public class Operacao : IOperacaoTransient, IOperacaoScoped,
                            IOperacaoSingleton, IOperacaoSingletonInstance
    {
        public Operacao() : this(Guid.NewGuid())
        {
            
        }

        public Operacao(Guid id)
        {
            OperacaoId = id;
        }

        public Guid OperacaoId { get; private set; }
    }

    public interface IOperacao
    {
        Guid OperacaoId { get; }
    }

    public interface IOperacaoTransient : IOperacao
    {
        
    }

    public interface IOperacaoScoped : IOperacao
    {
        
    }

    public interface IOperacaoSingleton : IOperacao
    {
        
    }

    public interface IOperacaoSingletonInstance : IOperacao
    {
        
    }
}
