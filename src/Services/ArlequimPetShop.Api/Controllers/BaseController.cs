using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SrShut.Common.Exceptions;

namespace ArlequimPetShop.Api.Controllers
{
    /// <summary>
    /// Controller base para todos os controllers da API. 
    /// Responsável por tratamento de exceções comuns e lógica compartilhada.
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public BaseController() { }

        /// <summary>
        /// Executado após a execução de uma ação. Trata exceções específicas como <see cref="HttpException"/>.
        /// </summary>
        /// <param name="context">Contexto da execução da ação.</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if (context.Exception != null)
            {
                if (context.Exception is HttpException exception)
                {
                    // Trata erros do tipo "403 Forbidden" e retorna resposta adequada.
                    if (exception.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        context.ExceptionHandled = true;
                        context.Result = Forbid();
                    }
                }
            }
        }

        /// <summary>
        /// Executado antes e depois da execução da ação. Pode ser sobrescrito para lógica de pré/pós-processamento.
        /// </summary>
        /// <param name="context">Contexto da execução.</param>
        /// <param name="next">Delegate que executa a próxima etapa da pipeline.</param>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await base.OnActionExecutionAsync(context, next);
        }
    }
}