using System;
using System.Threading.Tasks;
using Nocturne.Auth.Core.Shared.Results;
using Mvc = Microsoft.AspNetCore.Mvc;

namespace Nocturne.Auth.Admin.Areas.Applications.Controllers
{
    public class ResultToActionResultBuilder
    {
        public delegate Task<Mvc.IActionResult> AsyncResultAction();
        public delegate Mvc.IActionResult ResultAction();

        private readonly Result result;
        private readonly Mvc.Controller controller;

        private AsyncResultAction successAction;
        private AsyncResultAction problemsAction;

        public ResultToActionResultBuilder(
            Mvc.Controller controller,
            Result result)
        {
            this.controller = controller;
            this.result = result;
        }

        public ResultToActionResultBuilder Success(AsyncResultAction successAction)
        {
            this.successAction = successAction;

            return this;
        }

        public ResultToActionResultBuilder Success(ResultAction successAction)
        {
            this.successAction = AsyncWrapper;

            return this;

            async Task<Mvc.IActionResult> AsyncWrapper()
            {
                await Task.CompletedTask;

                return successAction();
            }
        }

        public ResultToActionResultBuilder Problems(AsyncResultAction problemsAction)
        {
            this.problemsAction = problemsAction;

            return this;
        }

        public ResultToActionResultBuilder Problems(ResultAction problemsAction)
        {
            this.problemsAction = AsyncWrapper;

            return this;

            async Task<Mvc.IActionResult> AsyncWrapper()
            {
                await Task.CompletedTask;

                return problemsAction();
            }
        }

        public Task<Mvc.IActionResult> BuildAsync()
        {
            return result switch
            {
                SuccessResult => Success(),
                NotFoundResult => NotFound(),
                ProblemsResult problemsResult => Problems(problemsResult),

                _ => throw new NotSupportedException(
                    $"Result of type {result.GetType().FullName} is not supported"),
            };
        }

        private async Task<Mvc.IActionResult> NotFound()
        {
            await Task.CompletedTask;

            return controller.NotFound();
        }

        private Task<Mvc.IActionResult> Problems(ProblemsResult result)
        {
            foreach (var problem in result.Problems)
            {
                controller.ModelState.AddModelError(problem.Name ?? string.Empty, problem.Description);
            }

            return problemsAction?.Invoke()
                ?? throw new InvalidOperationException(
                    $"An action was not configured for {nameof(ProblemsResult)}");
        }

        private Task<Mvc.IActionResult> Success()
        {
            return successAction?.Invoke()
                ?? throw new InvalidOperationException(
                    $"An action was not configured for {nameof(SuccessResult)}");
        }
    }
}
