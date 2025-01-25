namespace OrderProcessing.Api.Endpoints
{
	public interface IEndpointsModule
	{
		void RegisterEndpoints(IEndpointRouteBuilder app);
	}

	public static class EndpointRouteBuilderExtensions
	{
		public static IEndpointRouteBuilder MapEndpointModule<TModule>(this IEndpointRouteBuilder app)
			where TModule : IEndpointsModule, new()
		{
			var module = new TModule();
			module.RegisterEndpoints(app);
			return app;
		}
	}
}

