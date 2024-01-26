using Grpc.Core;
using Grpc.Core.Interceptors;


namespace Grpc.Crud.Concept.Test.Middlewere
{
    public class ErrorHandelingInterceptor: Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Unexpected error occurred: {ex.Message}"));
            }
        }
    }
}
