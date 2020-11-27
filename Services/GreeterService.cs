using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcServerStreaming
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task CallStream(StreamRequest request, IServerStreamWriter<StreamContent> responseStream, ServerCallContext context)
        {
            List<StreamContent> list = new List<StreamContent>();
            list.Add(
                new StreamContent{Id = 1, Name = "John Doe", Contact = "0214443332"}
            );
            list.Add(
                new StreamContent{Id = 2, Name = "Alex Ross", Contact = "0214453332"}
            );
            list.Add(
                new StreamContent{Id = 3, Name = "Tim Drake", Contact = "0215443332"}
            );
            foreach(StreamContent content in list)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(content);
            }
        }
    }
}
