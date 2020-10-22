using MediatR;
using ClientApi.DTOs;

namespace ClientApi.Events
{
    public class ClientCreated : INotification
    {
        public readonly FakeClient fakeClient;
        public readonly string action;
        public ClientCreated(FakeClient payload, string action){
            fakeClient = payload;
            this.action = action;
        }
    }
}