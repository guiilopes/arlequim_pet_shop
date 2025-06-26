using Microsoft.AspNetCore.Mvc.Filters;
using SrShut.Common;
using SrShut.Data;

namespace ArlequimPetShop.Api.Helpers
{
    /// <summary>
    /// Filtro de ação que injeta uma instância de <see cref="IUnitOfWork"/> no contexto HTTP
    /// antes da execução de uma ação e garante sua finalização após a execução.
    /// </summary>
    public class UnitOfWorkAttribute : ActionFilterAttribute
    {
        private readonly IUnitOfWorkFactory _uofwFactory;

        /// <summary>
        /// Inicializa uma nova instância do atributo <see cref="UnitOfWorkAttribute"/>.
        /// </summary>
        /// <param name="uofwFactory">Fábrica de Unit of Work para gerar sessões de transação.</param>
        public UnitOfWorkAttribute(IUnitOfWorkFactory uofwFactory)
        {
            Throw.ArgumentIsNull(uofwFactory, nameof(uofwFactory));
            _uofwFactory = uofwFactory;
        }

        /// <summary>
        /// Executado antes da ação. Cria e adiciona o UnitOfWork no contexto da requisição HTTP.
        /// </summary>
        /// <param name="filterContext">Contexto da execução da ação.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Items.Add("SessionKey", _uofwFactory.Get());
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Executado após a ação. Garante o descarte do UnitOfWork criado.
        /// </summary>
        /// <param name="context">Contexto da execução da ação.</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                var unitOfWork = (IUnitOfWork)context.HttpContext.Items["SessionKey"];
                unitOfWork?.Dispose();
            }
            catch
            {
                // Silencia qualquer exceção ao descartar o UnitOfWork.
            }
        }
    }
}