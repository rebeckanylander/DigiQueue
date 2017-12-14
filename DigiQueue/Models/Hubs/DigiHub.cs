using DigiQueue.Models.Repositories;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace DigiQueue.Models.Hubs
{
    public class DigiHub : Hub
    {
        IRepository repository;
        static List<ProblemVM> waitingList = new List<ProblemVM>();
        public static Dictionary<string, LoggedInVM> loggedInList = new Dictionary<string, LoggedInVM>();
        

        public DigiHub(IRepository repository)
        {
            this.repository = repository;
        }

        public Task GetLoggedInList(string jsonMessage)
        {
            var json = JsonConvert.DeserializeObject<ProtocolMessage>(jsonMessage);
            if (json.Command == "LogIn")
            {
                if ((loggedInList.Values.SingleOrDefault(p => p.Alias == json.Alias && p.ClassroomName == json.ClassroomName)) == null)
                {
                    Groups.AddAsync(Context.ConnectionId, json.ClassroomName);
                    loggedInList.Add(Context.ConnectionId, new LoggedInVM { Alias = json.Alias, ClassroomName = json.ClassroomName});
                }
                else
                {
                    return Clients.Client(Context.ConnectionId).InvokeAsync("onDisconnect", "a");
                }
            }
            var jsonList = JsonConvert.SerializeObject(loggedInList.Values.Where(c=> c.ClassroomName == json.ClassroomName));
            return Clients.Group(json.ClassroomName).InvokeAsync("onLogIn", jsonList);
        }

        public Task GetWaitingList(string classroomName)
        {
            var jsonList = JsonConvert.SerializeObject(waitingList.Where(c => c.ClassroomName == classroomName));
            return Clients.Client(Context.ConnectionId).InvokeAsync("onUpdateWaitingListItem", jsonList);
        }

        //Skicka meddelande i infobox - digimaster
        public Task InfoSend(string jsonMessage)
        {
            var json = JsonConvert.DeserializeObject<ProtocolMessage>(jsonMessage);
            if (json.Command == "Info")
            {
                return Clients.Group(json.ClassroomName).InvokeAsync("onInfoSend", UtilClass.ParseHtml(json.Description));
            }

            return Task.FromResult<object>(null); //ingenting händer
        }

        //Ta bort student från waiting list - digimaster
        public Task DeleteWaitingListItem(string jsonMessage)
        {
            ProtocolMessage json = JsonConvert.DeserializeObject<ProtocolMessage>(jsonMessage);
            if (json.Command == "Delete")
            {
                ProblemVM problem = waitingList.SingleOrDefault(p => p.Alias == json.Alias);
                if (problem != null)
                {
                    repository.EndProblem(json.Alias, json.ClassroomName);
                    waitingList.Remove(problem);
                }
                string jsonList = JsonConvert.SerializeObject(waitingList);
                return Clients.Group(json.ClassroomName).InvokeAsync("onUpdateWaitingListItem", jsonList);
            }

            return Task.FromResult<object>(null); //ingenting händer
        }

        //Skicka meddelande i chatten - digistudent
        public Task ChatSend(string jsonMessage)
        {
            var json = JsonConvert.DeserializeObject<ProtocolMessage>(jsonMessage);
            if (json.Command == "Message")
            {
                repository.SaveChatToDigiBase(json);
                string send = $"{json.Alias}: {UtilClass.ParseHtml(json.Description)}";
                return Clients.Group(json.ClassroomName).InvokeAsync("onChatSend", send);
            }

            return Task.FromResult<object>(null); //ingenting händer
        }

        //Lägga till sig på listan - digistudent
        public Task AddWaitingListItem(string jsonString)
        {
            var json = JsonConvert.DeserializeObject<ProtocolMessage>(jsonString);
            if (json.Command == "Add")
            {
                if ((waitingList.SingleOrDefault(p => p.Alias == json.Alias && p.ClassroomName == json.ClassroomName)) == null)
                {
                    waitingList.Add(
                        new ProblemVM
                        {
                            Alias = json.Alias,
                            Description = UtilClass.ParseHtml(json.Description),
                            Location = UtilClass.ParseHtml(json.Location),
                            ClassroomName = json.ClassroomName,
                            Language = json.PType.ToString(),
                            Time = DateTime.Now
                        }
                        );

                    repository.SaveProblemToDigiBase(json);
                    string jsonList = JsonConvert.SerializeObject(waitingList.Where(c => c.ClassroomName == json.ClassroomName));
                    return Clients.Group(json.ClassroomName).InvokeAsync("onUpdateWaitingListItem", jsonList);
                }
            }

            return Task.FromResult<object>(null); //ingenting händer
        }

        //Ta bort sig själv på listan - digistudent
        public Task RemoveSelfFromWaitingList(string jsonString)
        {
            var json = JsonConvert.DeserializeObject<ProtocolMessage>(jsonString);
            if (json.Command == "Remove")
            {
                ProblemVM problem = waitingList.SingleOrDefault(p => p.Alias == json.Alias && p.ClassroomName == json.ClassroomName);

                if (problem != null)
                {
                    repository.EndProblem(json.Alias, json.ClassroomName);
                    waitingList.Remove(problem);
                }
                string jsonList = JsonConvert.SerializeObject(waitingList.Where(c => c.ClassroomName == json.ClassroomName));
                return Clients.Group(json.ClassroomName).InvokeAsync("onUpdateWaitingListItem", jsonList);
            }

            return Task.FromResult<object>(null); //ingenting händer
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var classroomName = loggedInList.Single(x => x.Key == Context.ConnectionId).Value.ClassroomName;
            loggedInList.Remove(Context.ConnectionId);
            Groups.RemoveAsync(Context.ConnectionId, classroomName);
            //waitingList.SingleOrDefault()

            var jsonList = JsonConvert.SerializeObject(loggedInList.Values.Where(c => c.ClassroomName == classroomName));
            Clients.Group(classroomName).InvokeAsync("onLogIn", jsonList);

            
            return base.OnDisconnectedAsync(exception);
        }
        

    }
}
